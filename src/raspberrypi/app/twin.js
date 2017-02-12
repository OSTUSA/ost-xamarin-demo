'use strict';

class Twin {
  constructor(client) {
    this._client = client;
    this._twin = {};
  }

  init() {
    return new Promise((resolve, reject) => {
      this._client.getTwin((err, twin) => {
        if (err) {
          return reject(`failed to get twin: ${err}`);
        }

        // console.log(`Successfully retrieved twin: ${JSON.stringify(twin, null, 2)}`);
        this._twin = twin;
        return resolve();
      });
    });
  }

  update(patch) {
    return new Promise((resolve, reject) => {
      console.log(`updating twin with patch: ${JSON.stringify(patch, null, 2)}`);
      this._twin.properties.reported.update(patch, (err) => {
        return err ? reject(err) : resolve(err);
      })
    });
  }

  static create(client) {
    return new Twin(client);
  }
}

module.exports = Twin;