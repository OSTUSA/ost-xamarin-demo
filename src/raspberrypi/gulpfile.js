'use strict';

var gulp = require('gulp');

require('gulp-common')(gulp, 'raspberrypi-node', {
  appName: 'ost-iot-demo',
  configTemplate: {
    "device_host_name_or_ip_address": "[device hostname or IP address]",
    "device_user_name": "pi",
    "device_password": "raspberry",
    "iot_hub_connection_string": "[IoT hub connection string]",
    "iot_device_connection_string": "[IoT device connection string]"
  },
  configPostfix: 'raspberrypi',
  project_folder: 'src/raspberrypi'
});

gulp.task('run', 'Runs deployed code on the board', ['run-internal']);