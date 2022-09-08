import { SpatialAudioExtension} from "./index.esm.js";



AgoraRTC.setLogLevel(1);
let extension = new SpatialAudioExtension();
AgoraRTC.registerExtensions([extension]);


class spatialAudioManager {

constructor(){

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

this.localPlayTracks = [],

this.localPlayProcessors = [],

this.enabled = undefined;

this.processor = null;


}





async getLocalSpatialAudioProcessor(client, soundSrc, enabled) {
      setTimeout(async () => {
        try {
          this.localPlayTracks.forEach(t => {
            if(t.uid === client.uid){
              return;
            }
          });
          var isOn = enabled == 1 ? true : false;
              let track = await AgoraRTC.createBufferSourceAudioTrack({ source: soundSrc });
              client.spatialAudioTrack = track;
              track.startProcessAudioBuffer({ loop: true });
              this.processor = await extension.createProcessor();
              client.spatialAudioProcessor = this.processor;
              this.localPlayProcessors.push(this.processor);
              await track.pipe(this.processor).pipe(track.processorDestination);
              track.uid = client.uid;
              if (isOn === true) {
                track.play();
              } else {
                track.stop();
              }
              await this.localPlayTracks.push(track);
              this.enabled = isOn;
              console.log("enabling local player", this.enabled);
              return this.processor;
        } catch (error) {
          console.error(`${this.processor} with buffersource track ${soundSrc} play fail: ${error}`);
        }
      }, 1000);

}

async getRemoteSpatialAudioProcessor(client, soundSrc, enabled) {
  setTimeout(async () => {
    try {
      
      this.localPlayTracks.forEach(t => {
        console.log(t.uid, client.uid);
        if(t.uid === client.uid){
          return;
        }
      });
      var isOn = undefined;
      if(enabled !== true && enabled !== false){
        isOn = enabled == 1 ? true : false;
      } else { 
        isOn = enabled;
      }

      var track = await AgoraRTC.createBufferSourceAudioTrack({ source: soundSrc });
      client.spatialAudioTrack = track;
      track.startProcessAudioBuffer({ loop: true });
      var processor = await extension.createProcessor();
      client.spatialAudioProcessor = processor;
      await track.pipe(processor).pipe(track.processorDestination);
      await this.localPlayProcessors.push(processor);
      track.uid = client.uid;
      console.log("playing remote track", isOn, this.enabled);
      if (isOn === true && this.enabled === true) {
        track.play();
      } else {
        track.stop();
      }
      await this.localPlayTracks.push(track);
      console.log(this.localPlayTracks);
      return processor;
    } catch (error) {
      console.error(`${processor} with buffersource track ${soundSrc} play fail: ${error}`);
    }
  }, 1000);


}

async localPlayerStop(user) {
  for (let i = 0; i < this.localPlayTracks.length; i++) {
    if(this.localPlayTracks[i].uid === user.uid){
      await this.localPlayTracks[i].stop();
      delete this.localPlayTracks[i];
      break;
    }
  }
}

async localPlayerStopAll() {
  for (let i = 0; i < this.localPlayTracks.length; i++) {
    console.log(this.localPlayTracks[i]);
    await this.localPlayTracks[i].stop();
  }
  this.localPlayTracks = [];
  this.localPlayProcessors = [];
}

updateSpatialAzimuth(value) {
  this.spatialAudioSettings.azimuth = value;
  this.localPlayProcessors.forEach(e => {
    if(e != undefined){
      e.updateSpatialAzimuth(value);
    }
  });
}

updateSpatialElevation(value) {
  this.spatialAudioSettings.elevation = value;
  this.localPlayProcessors.forEach(e => {
    if(e != undefined){
      e.updateSpatialElevation(value);
    }
  });
}

updateSpatialDistance(value) {
  this.spatialAudioSettings.distance = value;
  this.localPlayProcessors.forEach(e => {
    if(e != undefined){
      e.updateSpatialDistance(value);
    }
  });
}

updateSpatialOrientation(value) {
  this.spatialAudioSettings.orientation = value;
  this.localPlayProcessors.forEach(e => {
    if(e != undefined){
      e.updateSpatialOrientation(value);
    }
  });
}

updateSpatialAttenuation(value) {
  this.spatialAudioSettings.attenuation = value;
  this.localPlayProcessors.forEach(e => {
    if(e != undefined){
      e.updateSpatialAttenuation(value);
    }
  });
}

updateSpatialBlur(checked) {
  this.spatialAudioSettings.blur = checked;
  this.localPlayProcessors.forEach(e => {
    if(e != undefined){
      console.log(checked);
      e.updateSpatialBlur(checked);
    }
  });
}

updateSpatialAirAbsorb(checked) {
  this.spatialAudioSettings.airAbsorb = checked;
  this.localPlayProcessors.forEach(e => {
    if(e != undefined){
      e.updateSpatialAirAbsorb(checked);
    }
  });
}

}

function createSpatialAudioManager(){
  return new spatialAudioManager();
}

window.createSpatialAudioManager = createSpatialAudioManager;
