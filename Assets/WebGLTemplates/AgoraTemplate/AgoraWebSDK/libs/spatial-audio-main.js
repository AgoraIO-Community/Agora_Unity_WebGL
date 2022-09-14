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

async getRemoteUserSpatialAudioProcessor(client, enabled) {
  setTimeout(async () => {
    try {
      
      var processor = undefined;
      
      
      if(enabled === true){
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
    if(this.localPlayTracks[user.uid] !== undefined){
      await this.localPlayTracks[user.uid].stop();
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

updateSpatialAzimuth(uid, value) {
  console.log(this.localPlayProcessors);
  console.log(uid);
  this.spatialAudioSettings.azimuth = value;
  this.localPlayProcessors[uid].updateSpatialAzimuth(value);
}

updateSpatialElevation(uid, value) {
  this.spatialAudioSettings.elevation = value;
  this.localPlayProcessors[uid].updateSpatialElevation(value);
}

updateSpatialDistance(uid, value) {
  this.spatialAudioSettings.distance = value;
  this.localPlayProcessors[uid].updateSpatialDistance(value);
}

updateSpatialOrientation(uid, value) {
  this.spatialAudioSettings.orientation = value;
  this.localPlayProcessors[uid].updateSpatialOrientation(value);
}

updateSpatialAttenuation(uid, value) {
  this.spatialAudioSettings.attenuation = value;
  this.localPlayProcessors[uid].updateSpatialAttenuation(value);
}

updateSpatialBlur(uid, checked) {
  this.spatialAudioSettings.blur = checked;
  this.localPlayProcessors[uid].updateSpatialBlur(checked);
}

updateSpatialAirAbsorb(uid, checked) {
  this.spatialAudioSettings.airAbsorb = checked;
  this.localPlayProcessors[uid].updateSpatialAirAbsorb(checked);
}

}

function createSpatialAudioManager(){
  return new spatialAudioManager();
}

window.createSpatialAudioManager = createSpatialAudioManager;
