/*
* IoT Hub Raspberry Pi NodeJS - Microsoft Sample Code - Copyright (c) 2016 - Licensed MIT
*/
'use strict';

var sleep = require('sleep');
var BME280 = require('bme280-sensor');

// read the sensor data every 10 seconds
const INTERVAL = 10000;

const bme280 = new BME280();

const readSensorData = function() {
  bme280.readSensorData()
    .then((data) => {
      // transform the data to a smaller packet
      var payload = {
        t: data.temperature_C,
        h: data.humidity,
        p: data.pressure_hPa
      };

      console.log(`payload = ${JSON.stringify(payload, null, 2)}`);
      setTimeout(readSensorData, 10000);
    })
    .catch((err) => {
      console.log(`BME280 read error: ${err}`);
      setTimeout(readSensorData, 10000);
    })
};

bme280.init()
  .then(() => {
    console.log('BME280 initialization succeeded');
    readSensorData();
  })
  .catch((err) => {
    console.log(`BME280 initialization failed: ${err}`);
  });