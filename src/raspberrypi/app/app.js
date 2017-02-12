/*
* IoT Hub Raspberry Pi NodeJS - Microsoft Sample Code - Copyright (c) 2016 - Licensed MIT
*/
'use strict';

var BME280 = require('bme280-sensor');

// read the sensor data every 10 seconds
const INTERVAL = 10000;

const bme280 = new BME280();
const Receiver = require('./receiver.js');
const Twin = require('./twin.js');

var receiver = {};
var twin = {
  update: function (data) {
    return new Promise((resolve, reject) => {
      reject('twin not initialized');
    });
  }
};

const onRefresh = function (request, response) {
  readSensorData()
    .then((data) => {
      twin.update(data)
        .then(() => {
          response.send(200, data, (err) => {
            if (err) {
              console.log(`error sending response: ${err}`);
            } else {
              console.log(`Successfully responded to method ${request.methodName}`);
            }
          });
        })
        .catch((err) => {
          console.log(`error updating twin: ${err}`);
          response.send(500, `error updating twin: ${err}`, (err) => {
            if (err) {
              console.log(`error sending response: ${err}`);
            } else {
              console.log(`Successfully responded to method ${request.methodName}`);
            }
          });
        });
    })
    .catch((err) => {
      console.log(`error reading sensor data: ${err}`);
      response.send(500, `error reading sensor data: ${err}`, (err) => {
        if (err) {
          console.log(`error sending response: ${err}`);
        } else {
          console.log(`Successfully responded to method ${request.methodName}`);
        }
      });
    });
};

const readSensorData = function () {
  return new Promise((resolve, reject) => {
    bme280.readSensorData()
      .then((data) => {
        // transform the data to a smaller packet
        var payload = {
          temperature: data.temperature_C,
          humidity: data.humidity,
          pressure: data.pressure_hPa
        };
        resolve(payload);
      })
      .catch((err) => {
        console.log(`BME280 read error: ${err}`);
        reject(`BME280 read error: ${err}`);
      });
  });
};

var init = function () {
  return new Promise((resolve, reject) => {
    var fs = require('fs');
    var path = require('path');
    var Client = require('azure-iot-device').Client;
    var Protocol = require('azure-iot-device-mqtt').Mqtt;
    var ConnectionString = require('azure-iot-device').ConnectionString;
    var Message = require('azure-iot-common').Message;

    var connectionStringParam = process.argv[2];
    var connectionString = ConnectionString.parse(connectionStringParam);
    var deviceId = connectionString.DeviceId;

    var client = Client.fromConnectionString(connectionStringParam, Protocol);

    // Configure the client to use X509 authentication if required by the connection string.
    if (connectionString.x509) {
      // Read X.509 certificate and private key.
      // These files should be in the current folder and use the following naming convention:
      // [device name]-cert.pem and [device name]-key.pem, example: myraspberrypi-cert.pem
      var options = {
        cert: fs.readFileSync(path.join(__dirname, deviceId + '-cert.pem')).toString(),
        key: fs.readFileSync(path.join(__dirname, deviceId + '-key.pem')).toString()
      };

      client.setOptions(options);

      console.log('[Device] Using X.509 client certificate authentication');
    }

    client.open((err) => {
      if (err) {
        reject(`unable to open client: ${err}`);
      } else {
        console.log('Successfully opened client');

        bme280.init()
          .then(() => {
            resolve(client);
          })
          .catch((err) => {
            reject(`unable to initialize BME280: ${err}`);
          });
      }
    });
  });
}

init()
  .then((client) => {
    receiver = Receiver.create(client);
    receiver.subscribe('refresh', onRefresh);
    console.log('initialized message receiver');

    var tempTwin = Twin.create(client);
    tempTwin.init()
      .then(() => {
        console.log('initialized twin');
        twin = tempTwin;
      })
      .catch((err) => {
        console.log(`unable to initialize twin: ${err}`);
      });
  })
  .catch((err) => {
    console.log(`unable to initialize client: ${err}`);
  });