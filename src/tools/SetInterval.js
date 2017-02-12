'use strict';

var iothub = require('azure-iothub');
var config = require ('./config.json');
var registry = iothub.Registry.fromConnectionString(config.connectionString);

var interval = process.argv[2] || 30000;

registry.getTwin(config.deviceId, (err, twin) => {
  if (err) {
    console.error(err.constructor.name + ': ' + err.message);
  } else {
    var patch = {
        properties: {
            desired: {
                config: {
                    interval: interval
                }
            }
        }
    };

    twin.update(patch, (err) => {
      if (err) {
        console.error('Could not update twin: ' + err.constructor.name + ': ' + err.message);
      } else {
        console.log(twin.deviceId + ' twin updated successfully to interval: ' + interval);
      }
    });
  }
});