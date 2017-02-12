'use strict';

var Client = require('azure-iothub').Client;
var methodName = 'refresh';
var config = require ('./config.json');
var deviceId = config.deviceId;

var client = Client.fromConnectionString(config.connectionString);

var params = {
    methodName: methodName,
    payload: '',
    timeoutInSeconds: 30
};

client.invokeDeviceMethod(deviceId, params, (err, result) => {
  if (err) {
    console.log(`error invoking ${params.methodName}: ${err}`);
  } else {
    console.log(`${methodName} on ${deviceId}: ${JSON.stringify(result, null, 2)}`);
  }
});