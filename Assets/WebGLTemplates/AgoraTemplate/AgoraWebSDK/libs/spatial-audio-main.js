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

    (this.localPlayTracks = {}),
      (this.localPlayProcessors = {}),
      (this.enabled = true);

    this.processor = null;
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
        console.log("processing track: ", this.localPlayProcessors);
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
    return extension.setDistanceUnit(unit);
  }

  clearRemotePositions() {
    extension.clearRemotePositions();
  }

  updateSelfPosition(position, forward, right, up) {
    return extension.updateSelfPosition(position, forward, right, up);
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
      return this.localPlayProcessors[uid].updatePlayerPositionInfo(
        localPlayerPosition
      );
    } else {
      return -1;
    }
  }

  updateRemotePosition(uid, position, forward) {
    const localPlayerPosition = {
      //position: [position[0], position[1], 1],
      position: position,
      forward: forward,
    };

    if (this.localPlayProcessors[uid]) {
      return this.localPlayProcessors[uid].updateRemotePosition(localPlayerPosition);
    } else {
      return -1;
    }
  }

  removeRemotePosition(uid) {
    if (this.localPlayProcessors[uid]) {
      let rc = this.localPlayProcessors[uid].removeRemotePosition();
      delete this.localPlayProcessors[uid];
      return rc;
    } else {
      return -1;
    }
  }

  updateSpatialAzimuth(uid, value) {
    this.spatialAudioSettings.azimuth = value;
    if (this.localPlayProcessors[uid]) {
      return this.localPlayProcessors[uid].updateSpatialAzimuth(value);
    } else {
      return -1;
    }
  }

  updateSpatialElevation(uid, value) {
    this.spatialAudioSettings.elevation = value;
    if (this.localPlayProcessors[uid]) {
      return this.localPlayProcessors[uid].updateSpatialElevation(value);
    } else {
      return -1;
    }
  }

  updateSpatialDistance(uid, value) {
    this.spatialAudioSettings.distance = value;
    if (this.localPlayProcessors[uid]) {
      return this.localPlayProcessors[uid].updateSpatialDistance(value);
    } else {
      return -1;
    }
  }

  updateSpatialOrientation(uid, value) {
    this.spatialAudioSettings.orientation = value;
    if (this.localPlayProcessors[uid]) {
      return this.localPlayProcessors[uid].updateSpatialOrientation(value);
    } else {
      return -1;
    }
  }

  updateSpatialAttenuation(uid, value) {
    this.spatialAudioSettings.attenuation = value;
    console.log("local play processor", this.localPlayProcessors);
    if (this.localPlayProcessors[uid]) {
      return this.localPlayProcessors[uid].updateSpatialAttenuation(value);
    } else {
      return -1;
    }
  }

  updateSpatialBlur(uid, checked) {
    this.spatialAudioSettings.blur = checked;
    console.log("local play processor", this.localPlayProcessors);
    if (this.localPlayProcessors[uid]) {
      return this.localPlayProcessors[uid].updateSpatialBlur(checked);
    } else {
      return -1;
    }
  }

  updateSpatialAirAbsorb(uid, checked) {
    this.spatialAudioSettings.airAbsorb = checked;
    if (this.localPlayProcessors[uid]) {
      return this.localPlayProcessors[uid].updateSpatialAirAbsorb(checked);
    } else {
      return -1;
    }
  }
}

function createSpatialAudioManager() {
  return new spatialAudioManager();
}

window.createSpatialAudioManager = createSpatialAudioManager;
