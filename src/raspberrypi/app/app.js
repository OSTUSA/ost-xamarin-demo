/*
* IoT Hub Raspberry Pi NodeJS - Microsoft Sample Code - Copyright (c) 2016 - Licensed MIT
*/
'use strict';

const DEFAULT_INTERVAL = 30000;

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
  sendTelemetryData()
    .then((data) => {
      response.send(200, data, (err) => {
        if (err) {
          console.log(`error sending response: ${err}`);
        } else {
          console.log(`Successfully responded to method ${request.methodName}`);
        }
      });
    })
    .catch((err) => {
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
      .catch((err) => reject(`BME280 read error: ${err}`));
  });
};

const init = function () {
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

const createReceiver = function (client) {
  return new Promise((resolve, reject) => {
    receiver = Receiver.create(client);
    receiver.subscribe('refresh', onRefresh);
    resolve();
  });
};

const createTwin = function (client) {
  return new Promise((resolve, reject) => {
    twin = Twin.create(client);
    twin.init()
      .then(() => {
        twin.onDesiredChanged(updateTwinConfigInterval);
        updateTwinConfigInterval()
          .then(() => {
            resolve();
          })
          .catch((err) => reject(`unable to update twin config interval: ${err}`));
      })
      .catch((err) => console.log(`unable to initialize twin: ${err}`));
  });
};

const updateTwinConfigInterval = function (desired) {
  return new Promise((resolve, reject) => {
    var properties = twin.getProperties();
    var desired = desired || properties.desired;
    var reported = properties.reported;

    if (!reported.config || (desired.config && reported.config.interval != desired.config.interval)) {
      var interval = ((desired || {}).config || {}).interval || DEFAULT_INTERVAL;
      console.log(`setting interval: ${interval}`);
      var patch = {
        config: {
          interval: interval
        }
      };

      twin.update(patch)
        .then(() => resolve())
        .catch((err) => reject(err));
    }
    resolve();
  });
};

const sendTelemetryData = function () {
  return new Promise((resolve, reject) => {
    readSensorData()
      .then((data) => {
        twin.update(data)
          .then(() => {
            resolve(data);
          })
          .catch((err) => console.log(`error updating twin: ${err}`));
      })
      .catch((err) => console.log(`error reading sensor data: ${err}`));
  });
};

const sendAndScheduleTelemetry = function () {
  Promise.all([sendTelemetryData(), scheduleTelemetry()])
    .catch((err) => console.log(`error sending telemetry data: ${err}`));
};

const scheduleTelemetry = function () {
  return new Promise((resolve, reject) => {
    var properties = twin.getProperties();

    var interval = twin.getProperties().reported.config.interval;
    console.log(`sending next telemetry packet in ${interval}ms`);
    setTimeout(sendAndScheduleTelemetry, interval);
    resolve();
  });
}

init()
  .then((client) => {
    Promise.all([createReceiver(client), createTwin(client)])
      .then(() => {
        sendAndScheduleTelemetry();
      })
      .catch((err) => console.log(`unable to initialize IoT services: ${err}`));
  })
  .catch((err) => console.log(`unable to initialize client: ${err}`));