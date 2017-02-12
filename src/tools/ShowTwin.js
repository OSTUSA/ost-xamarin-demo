'use strict';

var iothub = require('azure-iothub');
var config = require ('./config.json');
var registry = iothub.Registry.fromConnectionString(config.connectionString);

registry.getTwin(config.deviceId, (err, twin) => {
  if (err) {
    console.error(err.constructor.name + ': ' + err.message);
  } else {
      console.log(JSON.stringify(twin.properties, null, 2));
  }
});

var queryTwins = function() {
  var query = registry.createQuery('SELECT * FROM devices WHERE tags.location.plant = \'Redmond43\'', 100);
       query.nextAsTwin(function(err, results) {
         if (err) {
             console.error('Failed to fetch the results: ' + err.message);
         } else {
             console.log("Devices in Redmond43: " + results.map(function(twin) {return twin.deviceId}).join(','));
         }
     });

     query = registry.createQuery("SELECT * FROM devices WHERE tags.location.plant = 'Redmond43' AND properties.reported.connectivity.type = 'cellular'", 100);
     query.nextAsTwin(function(err, results) {
         if (err) {
             console.error('Failed to fetch the results: ' + err.message);
         } else {
             console.log("Devices in Redmond43 using cellular network: " + results.map(function(twin) {return twin.deviceId}).join(','));
         }
     });
};