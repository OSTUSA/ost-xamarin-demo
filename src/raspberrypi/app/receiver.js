'use strict';

class Receiver {
  constructor(client) {
    this._client = client;
    console.log('created receiver');
  }

  subscribe(methodName, method) {
    this._client.onDeviceMethod(methodName, method);
    console.log(`subscribed to ${methodName}`)
  }

  static create(client) {
    return new Receiver(client);
  }
}

module.exports = Receiver;