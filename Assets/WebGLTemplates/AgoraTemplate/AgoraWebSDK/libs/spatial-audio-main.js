import { SpatialAudioExtension} from "./index.esm.js";

let processor = null;
let extension = null;
let track = null;

AgoraRTC.setLogLevel(1);
extension = new SpatialAudioExtension();
AgoraRTC.registerExtensions([extension]);

var remoteUsers = [];
var remoteUsersSound = [
  // './resources/1.mp3',
  // './resources/2.mp3',
  './resources/3.mp3',
  // './resources/4.mp3',
];

var localPlayerSound = [
  // './resources/1.mp3',
  './resources/2.mp3',
  // './resources/3.mp3',
  // './resources/4.mp3',
];
var localPlayTracks = [];
var localPlayProcessors = [];

console.log("is this working?");

function getSpatialAudioProcessorInstance(enabled) {
  if (enabled == true) {
    for (let i = 0; i < localPlayerSound.length; i++) {
      setTimeout(async () => {
        try {
          track = await AgoraRTC.createBufferSourceAudioTrack({ source: localPlayerSound[i] });
          localPlayTracks.push(track);
          track.startProcessAudioBuffer({ loop: true });
          processor = extension.createProcessor();
          localPlayProcessors.push(processor);
          track.pipe(processor).pipe(track.processorDestination);
          track.play();
        } catch (error) {
          console.error(`localPlayerSound[${i}] with buffersource track ${localPlayerSound[i]} play fail: ${error}`);
        }
      }, 500 * i);
    }
  } else {
    if (track !== null)
      track.stop();
  }

  return processor;
}


function localPlayerStop() {
  for (let i = 0; i < localPlayerSound.length; i++) {
    localPlayTracks[i].stop();
  }
  localPlayTracks = [];
}



function  updateSpatialAzimuth(value) {
  remoteUsers.forEach(e => {
    e.processor.updateSpatialAzimuth(value);
  });
  localPlayProcessors.forEach(e => {
    e.updateSpatialAzimuth(value);
  });
}

function updateSpatialElevation(value) {
  remoteUsers.forEach(e => {
    e.processor.updateSpatialElevation(value);
  });
  localPlayProcessors.forEach(e => {
    e.updateSpatialElevation(value);
  });
}

function updateSpatialDistance() {
  remoteUsers.forEach(e => {
    e.processor.updateSpatialDistance(value);
  });
  localPlayProcessors.forEach(e => {
    e.updateSpatialDistance(value);
  });
}

function updateSpatialOrientation(value) {
  remoteUsers.forEach(e => {
    e.processor.updateSpatialOrientation(value);
  });
  localPlayProcessors.forEach(e => {
    e.updateSpatialOrientation(value);
  });
}

function updateSpatialAttenuation(value) {
  remoteUsers.forEach(e => {
    e.processor.updateSpatialAttenuation(value);
  });
  localPlayProcessors.forEach(e => {
    e.updateSpatialAttenuation(value);
  });
}

function updateSpatialBlur(checked) {
  if (checked === true) {
    remoteUsers.forEach(e => {
      e.processor.updateSpatialBlur(true);
    });
    localPlayProcessors.forEach(e => {
      e.updateSpatialBlur(true);
    });
  } else {
    remoteUsers.forEach(e => {
      e.processor.updateSpatialBlur(false);
    });
    localPlayProcessors.forEach(e => {
      e.updateSpatialBlur(false);
    });
  }
}

function updateSpatialAirAbsorb(checked) {
  if (checked === true) {
    remoteUsers.forEach(e => {
      e.processor.updateSpatialAirAbsorb(true);
    });
    localPlayProcessors.forEach(e => {
      e.updateSpatialAirAbsorb(true);
    });
  } else {
    remoteUsers.forEach(e => {
      e.processor.updateSpatialAirAbsorb(false);
    });
    localPlayProcessors.forEach(e => {
      e.updateSpatialAirAbsorb(false);
    });
  }
}
