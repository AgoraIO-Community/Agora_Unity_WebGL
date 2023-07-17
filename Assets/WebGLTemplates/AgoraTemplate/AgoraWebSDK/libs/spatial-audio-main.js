import { SpatialAudioExtension } from "./index.esm.js";

AgoraRTC.setLogLevel(1);
let extension = new SpatialAudioExtension({
  assetsPath: "./AgoraWebSDK/libs/spatial",
});
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
      airAbsorb: false,
    };

    this.remoteUsersSound = ["./AgoraWebSDK/libs/resources/3.mp3"];

    this.localPlayerSound = ["./AgoraWebSDK/libs/resources/2.mp3"];

    (this.localPlayTracks = {}),
      (this.localPlayProcessors = {}),
      (this.enabled = true);

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
        await client.audioTrack
          .pipe(processor)
          .pipe(client.audioTrack.processorDestination);
        client.audioTrack.uid = client.uid;
        this.enabled = isOn;
        return processor;
      } catch (error) {
        console.error(`${processor} with microphone track play fail: ${error}`);
      }
    }, 1000);
  }

  // each media track will be assigned a UID
  async startLocalMedia(uid, soundSrc) {
    setTimeout(async () => {
      try {
        var processor = await extension.createProcessor();
        const track = await AgoraRTC.createBufferSourceAudioTrack({
          source: soundSrc,
        });
        await track.pipe(processor).pipe(track.processorDestination);
        track.startProcessAudioBuffer({
          loop: true,
        });
        track.play();
        this.localPlayProcessors[uid] = processor;
        this.localPlayTracks[uid] = track;
      } catch (error) {
        console.error(`${processor} with buffer track play fail: ${error}`);
      }
    }, 1000);
  }

  async pipeRemoteUserSpatialAudioProcessor(user) {
    setTimeout(async () => {
      try {
        var processor = await extension.createProcessor();

        this.localPlayProcessors[user.uid] = processor;
        this.localPlayTracks[user.uid] = user.audioTrack;
        // Inject the SpatialAudioProcessor into the audio track
        const track = user.audioTrack;
        track.pipe(processor).pipe(track.processorDestination);
      } catch (error) {
        console.error(`${processor} with microphone track play fail: ${error}`);
      }
    }, 1000);
  }

  async localPlayerStop(user) {
    if (this.localPlayTracks[user.uid]) {
      await this.localPlayTracks[user.uid].stop();
      await this.localPlayTracks[user.uid].close();
      delete this.localPlayTracks[user.uid];
    }
  }

  async localPlayerStopAll() {
    if (Object.keys(this.localPlayTracks).length > 0) {
      Object.values(this.localPlayTracks).forEach((e) => {
        e.stop();
        e.close();
      });

      this.localPlayTracks = {};
      this.localPlayProcessors = {};
    }
  }

  setDistanceUnit(unit) {
    extension.setDistanceUnit(unit);
  }

  clearRemotePositions() {
    extension.clearRemotePositions();
  }

  updateSelfPosition(position, forward, right, up) {
    console.log("[EXT] updating self position");
    extension.updateSelfPosition(position, forward, right, up);
  }

  updatePlayerPositionInfo(uid, position, forward) {
    const localPlayerPosition = {
      //position: [position[0], position[1], position[2]],
      position: position,
      forward: forward,
    };

    if (this.localPlayProcessors[uid]) {
      console.log(
        this.localPlayProcessors[uid].updatePlayerPositionInfo({
          position,
          forward,
        })
      );
      this.localPlayProcessors[uid].updatePlayerPositionInfo(
        localPlayerPosition
      );
    }
  }

  updateRemotePosition(uid, position, forward) {
    const localPlayerPosition = {
      //position: [position[0], position[1], 1],
      position: position,
      forward: forward,
    };

    if (this.localPlayProcessors[uid]) {
      this.localPlayProcessors[uid].updateRemotePosition(localPlayerPosition);
    }
  }

  removeRemotePosition(uid) {
    if (this.localPlayProcessors[uid]) {
      this.localPlayProcessors[uid].removeRemotePosition();
      delete this.localPlayProcessors[uid];
      delete this.localPlayTracks[uid];
    }
  }

  updateSpatialAzimuth(uid, value) {
    this.spatialAudioSettings.azimuth = value;
    if (this.localPlayProcessors[uid]) {
      this.localPlayProcessors[uid].updateSpatialAzimuth(value);
    }
  }

  updateSpatialElevation(uid, value) {
    this.spatialAudioSettings.elevation = value;
    if (this.localPlayProcessors[uid]) {
      this.localPlayProcessors[uid].updateSpatialElevation(value);
    }
  }

  updateSpatialDistance(uid, value) {
    this.spatialAudioSettings.distance = value;
    if (this.localPlayProcessors[uid]) {
      this.localPlayProcessors[uid].updateSpatialDistance(value);
    }
  }

  updateSpatialOrientation(uid, value) {
    this.spatialAudioSettings.orientation = value;
    if (this.localPlayProcessors[uid]) {
      this.localPlayProcessors[uid].updateSpatialOrientation(value);
    }
  }

  updateSpatialAttenuation(uid, value) {
    this.spatialAudioSettings.attenuation = value;
    if (this.localPlayProcessors[uid]) {
      this.localPlayProcessors[uid].updateSpatialAttenuation(value);
    }
  }

  updateSpatialBlur(uid, checked) {
    this.spatialAudioSettings.blur = checked;
    if (this.localPlayProcessors[uid]) {
      this.localPlayProcessors[uid].updateSpatialBlur(checked);
    }
  }

  updateSpatialAirAbsorb(uid, checked) {
    this.spatialAudioSettings.airAbsorb = checked;
    if (this.localPlayProcessors[uid]) {
      this.localPlayProcessors[uid].updateSpatialAirAbsorb(checked);
    }
  }
}

function createSpatialAudioManager() {
  return new spatialAudioManager();
}

window.createSpatialAudioManager = createSpatialAudioManager;
