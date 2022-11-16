import { SpatialAudioExtension } from "./index.esm.js";

AgoraRTC.setLogLevel(1);
let extension = new SpatialAudioExtension({ assetPath: './AgoraWebSDK/libs/spatial' });
AgoraRTC.registerExtensions([extension]);

class spatialAudioManager {

  constructor() {

    this.spatialAudioSettings = {
      azimuth: 0,
      elevation: 0,
      distance: 1,
      orientation: 0,
      attenuation: 0,
      blur: false,
      airAbsorb: false
    };

    this.remoteUsersSound = [
      './AgoraWebSDK/libs/resources/3.mp3'
    ];

    this.localPlayerSound = [
      './AgoraWebSDK/libs/resources/2.mp3'
    ];

    this.localPlayTracks = {},

      this.localPlayProcessors = {},

      this.enabled = true;

    this.processor = null;
  }

  async getLocalUserSpatialAudioProcessor(client, soundSrc, enabled) {
    setTimeout(async () => {
      try {
        console.log(client);
        var isOn = enabled == 1 ? true : false;
        var processor = await extension.createProcessor();
        client.spatialAudioProcessor = processor;
        this.localPlayProcessors.push(processor);
        await client.audioTrack.pipe(processor).pipe(client.audioTrack.processorDestination);
        client.audioTrack.uid = client.uid;
        this.enabled = isOn;
        return processor;
      } catch (error) {
        console.error(`${processor} with microphone track play fail: ${error}`);
      }
    }, 1000);

  }

  async getLocalMediaSpatialAudioProcessor(uid, soundSrc, enabled) {
    setTimeout(async () => {
      try {
        var isOn = enabled == 1 ? true : false;
        var processor = await extension.createProcessor();
        const track = await AgoraRTC.createBufferSourceAudioTrack({ source: soundSrc });
        await track.pipe(processor).pipe(track.processorDestination);
        track.startProcessAudioBuffer({ loop: true });
        track.play();
        this.localPlayProcessors[uid] = processor;
        this.localPlayTracks[uid] = track;
        this.enabled = isOn;
      } catch (error) {
        console.error(`${processor} with buffer track play fail: ${error}`);
      }
    }, 1000);

  }

  async getRemoteMediaSpatialAudioProcessor(client, soundSrc, enabled) {
    setTimeout(async () => {
      try {
        var isOn = enabled == 1 ? true : false;
        var processor = await extension.createProcessor();
        const track = await AgoraRTC.createBufferSourceAudioTrack({ source: soundSrc });
        await track.pipe(processor).pipe(track.processorDestination);
        track.startProcessAudioBuffer({ loop: true });
        client.spatialAudioProcessor = processor;
        this.localPlayProcessors[client.uid] = processor;
        this.localPlayTracks[client.uid] = track;
        this.enabled = isOn;
        this.processor = processor;
        track.play();
        return processor;
      } catch (error) {
        console.error(`${processor} with microphone track play fail: ${error}`);
      }
    }, 1000);

  }



  async getRemoteMediaSpatialAudioProcessor(enabled, media) {
    setTimeout(async () => {
      try {

        var processor = undefined;


        if (enabled === true) {
          processor = await extension.createProcessor();
          const track = await AgoraRTC.createBufferSourceAudioTrack(media)
          await client.audioTrack.pipe(processor).pipe(client.audioTrack.processorDestination);
          this.localPlayProcessors[client.uid] = processor;
          this.localPlayTracks[client.uid] = track;
        } else {
          //disable spatial audio code would go here.
        }

        this.enabled = enabled;

      } catch (error) {
        console.error(`${processor} with microphone track play fail: ${error}`);
      }
    }, 1000);
  }

  async getRemoteUserSpatialAudioProcessor(client, enabled) {
    setTimeout(async () => {
      try {

        var processor = undefined;


        if (enabled === true) {
          processor = await extension.createProcessor();
          await client.audioTrack.pipe(processor).pipe(client.audioTrack.processorDestination);
          this.localPlayProcessors[client.uid] = processor;
          this.localPlayTracks[client.uid] = client.audioTrack;
        } else {
          //disable spatial audio code would go here.
        }

        this.enabled = enabled;

      } catch (error) {
        console.error(`${processor} with microphone track play fail: ${error}`);
      }
    }, 1000);


  }

  async localPlayerStop(user) {
    if (this.localPlayTracks[user.uid] !== undefined) {
      await this.localPlayTracks[user.uid].stop();
      await this.localPlayTracks[user.uid].close();
      delete this.localPlayTracks[user.uid];
    }
  }

  async localPlayerStopAll() {
    if (Object.keys(this.localPlayTracks).length > 0) {
      Object.values(this.localPlayTracks).forEach(e => {
        e.stop();
        e.close();
      });

      this.localPlayTracks = {};
      this.localPlayProcessors = {};
    }
  }

  updateSelfPosition(position, forward) {
    const localPlayerPosition = {
      position: [position[0], position[1], 1],
      forward: forward,
      right: [1, 0, 0],
      up: [0, 1, 0]
    };


    console.log("updating extension position");
    extension.updateSelfPosition(localPlayerPosition);


  }

  updatePlayerPositionInfo(uid, position, forward) {
    const localPlayerPosition = {
      position: [position[0], position[1], position[2]],
      forward: forward
    };



    if (this.localPlayProcessors[uid] !== undefined) {
      console.log(this.localPlayProcessors[uid].updatePlayerPositionInfo({ position, forward }));
      this.localPlayProcessors[uid].updatePlayerPositionInfo(localPlayerPosition);
    }

  }

  updateRemotePosition(uid, position, forward) {
    const localPlayerPosition = {
      position: [position[0], position[1], 1],
      forward: forward,
    };



    if (this.localPlayProcessors[uid] !== undefined) {

      this.localPlayProcessors[uid].updateRemotePosition(localPlayerPosition);
    }

  }

  removeRemotePosition(uid) {
    delete this.localPlayProcessors[uid];
    delete this.localPlayTracks[uid];
  }

  updateSpatialAzimuth(uid, value) {
    this.spatialAudioSettings.azimuth = value;
    if (this.localPlayProcessors[uid] !== undefined) {
      this.localPlayProcessors[uid].updateSpatialAzimuth(value);
    }
  }

  updateSpatialElevation(uid, value) {
    this.spatialAudioSettings.elevation = value;
    if (this.localPlayProcessors[uid] !== undefined) {
      this.localPlayProcessors[uid].updateSpatialElevation(value);
    }
  }

  updateSpatialDistance(uid, value) {
    this.spatialAudioSettings.distance = value;
    if (this.localPlayProcessors[uid] !== undefined) {
      this.localPlayProcessors[uid].updateSpatialDistance(value);
    }
  }

  updateSpatialOrientation(uid, value) {
    this.spatialAudioSettings.orientation = value;
    if (this.localPlayProcessors[uid] !== undefined) {
      this.localPlayProcessors[uid].updateSpatialOrientation(value);
    }
  }

  updateSpatialAttenuation(uid, value) {
    this.spatialAudioSettings.attenuation = value;
    if (this.localPlayProcessors[uid] !== undefined) {
      this.localPlayProcessors[uid].updateSpatialAttenuation(value);
    }
  }

  updateSpatialBlur(uid, checked) {
    this.spatialAudioSettings.blur = checked;
    if (this.localPlayProcessors[uid] !== undefined) {
      this.localPlayProcessors[uid].updateSpatialBlur(checked);
    }
  }

  updateSpatialAirAbsorb(uid, checked) {
    this.spatialAudioSettings.airAbsorb = checked;
    if (this.localPlayProcessors[uid] !== undefined) {
      this.localPlayProcessors[uid].updateSpatialAirAbsorb(checked);
    }
  }

}

function createSpatialAudioManager() {
  return new spatialAudioManager();
}

window.createSpatialAudioManager = createSpatialAudioManager;
