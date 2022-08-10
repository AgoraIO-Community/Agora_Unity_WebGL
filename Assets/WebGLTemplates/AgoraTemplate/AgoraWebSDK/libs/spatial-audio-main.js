import { SpatialAudioExtension} from "./index.esm.js";

let processor = null;
let extension = null;
let track = null;

AgoraRTC.setLogLevel(1);
extension = new SpatialAudioExtension();
AgoraRTC.registerExtensions([extension]);

var options = {
  appid: null,
  channel: null,
  uid: null,
  token: null
};
var spatialAudioSettings = {
  azimuth: 0,
  elevation: 0,
  distance: 1,
  orientation: 0,
  attenuation: 0,
  blur: false,
  airAbsorb: false
};
var client = AgoraRTC.createClient({ mode: "rtc", codec: "vp8" });
var localUserTrack = {
  videoTrack: null,
  audioTrack: null
};

var remoteUsers = [];
var remoteUsersSound = [
  // './AgoraWebSDK/libs/resources/1.mp3',
  // './AgoraWebSDK/libs/resources/2.mp3',
  './AgoraWebSDK/libs/resources/3.mp3',
  // './AgoraWebSDK/libs/resources/4.mp3',
];

var localPlayerSound = [
  // './AgoraWebSDK/libs/resources/1.mp3',
  './AgoraWebSDK/libs/resources/2.mp3',
  // './AgoraWebSDK/libs/resources/3.mp3',
  // './AgoraWebSDK/libs/resources/4.mp3',
];
var localPlayTracks = [];
var localPlayProcessors = [];

window.joinSpatialAudioChannel = joinSpatialAudioChannel;
window.getSpatialAudioProcessorInstance = getSpatialAudioProcessorInstance;
window.localSpatialAudioPlayerStop = localPlayerStop;
window.updateSpatialAzimuth = updateSpatialAzimuth;
window.updateSpatialElevation = updateSpatialElevation;
window.updateSpatialDistance = updateSpatialDistance;
window.updateSpatialOrientation = updateSpatialOrientation;
window.updateSpatialAttenuation = updateSpatialAttenuation;
window.updateSpatialBlur = updateSpatialBlur;
window.updateSpatialAirAbsorb = updateSpatialAirAbsorb;




function getSpatialAudioProcessorInstance(enabled) {
  if (enabled == true) {
    for (let i = 0; i < localPlayerSound.length; i++) {
      setTimeout(async () => {
        try {
          track = await AgoraRTC.createBufferSourceAudioTrack({ source: localPlayerSound[i] });
          client.spatialAudioTrack = track;
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
  }

  return processor;
}


function localPlayerStop() {
  for (let i = 0; i < localPlayTracks.length; i++) {
    localPlayTracks[i].stop();
  }
  localPlayTracks = [];
}

async function join() {
  client.on("user-joined", handleUserJoined);
  client.on("user-left", handleUserLeft);

  options.uid = await Promise.all([
    client.join(options.appid, options.channel, options.token || null),
  ]);

  for(var i = 0; i < remoteUsers.length; i++){
    if (remoteUsers[i].spatialAudioTrack !== undefined) {
      remoteUsers[i].spatialAudioTrack.play();
    }
  }

  console.log("remoteUsers", remoteUsers);

}

async function handleUserJoined(user){
    processor = extension.createProcessor();
    user.processor = processor;
    const track = await AgoraRTC.createBufferSourceAudioTrack({ source: remoteUsersSound[0] });
    track.startProcessAudioBuffer({ loop: true });
    user.spatialAudioTrack = track;
    remoteUsers.push(user);
    if (track.processorDestination != user.processor) {
      track.pipe(user.processor).pipe(track.processorDestination);
    }
    
    track.play();
  console.log("remoteUsers", remoteUsers);
}

async function leave() {
  for (const trackName in localUserTrack) {
    var track = localUserTrack[trackName];
    if (track) {
      track.stop();
      track.close();
      localUserTrack[trackName] = undefined;
    }
  }
  for (var i = 0; i < remoteUsers.length; i++) {
    if (remoteUsers[i].spatialAudioTrack !== undefined) {
      remoteUsers[i].spatialAudioTrack.stop();
    }

  }
  remoteUsers = [];
  await client.spatialAudioTrack.stop();
  await client.leave();

  
  
  console.log("client leaves channel success");
}

function handleUserLeft(user) {
  const id = user.uid;
  for(var i = 0; i < remoteUsers.length; i++){
    console.log("remoteUsers", remoteUsers);
    client.unsubscribe(remoteUsers[i]);
    remoteUsers[i] = null;
  }
}

function  updateSpatialAzimuth(value) {
  spatialAudioSettings.azimuth = value;
  remoteUsers.forEach(e => {
    console.log(e.processor);
    if(e.processor != undefined){
      e.processor.updateSpatialAzimuth(value);
    }
  });
  localPlayProcessors.forEach(e => {
    if(e != undefined){
      e.updateSpatialAzimuth(value);
    }
  });
}

function updateSpatialElevation(value) {
  spatialAudioSettings.elevation = value;
  remoteUsers.forEach(e => {
    if (e.processor != undefined) {
      e.processor.updateSpatialElevation(value);
    }
  });
  localPlayProcessors.forEach(e => {
    if (e != undefined) {
      e.updateSpatialElevation(value);
    }
  });
}

function updateSpatialDistance(value) {
  spatialAudioSettings.distance = value;
  remoteUsers.forEach(e => {
    if (e.processor != undefined) {
      e.processor.updateSpatialDistance(value);
    }
  });
  localPlayProcessors.forEach(e => {
    if (e != undefined) {
      e.updateSpatialDistance(value);
    }
  });
}

function updateSpatialOrientation(value) {
  spatialAudioSettings.orientation = value;
  remoteUsers.forEach(e => {
    if (e.processor != undefined) {
      e.processor.updateSpatialOrientation(value);
    }
  });
  localPlayProcessors.forEach(e => {
    if (e != undefined) {
      e.updateSpatialOrientation(value);
    }
  });
}

function updateSpatialAttenuation(value) {
  spatialAudioSettings.attenuation = value;
  remoteUsers.forEach(e => {
    if (e.processor != undefined) {
      e.processor.updateSpatialAttenuation(value);
    }
  });
  localPlayProcessors.forEach(e => {
    if (e != undefined) {
      e.updateSpatialAttenuation(value);
    }
  });
}

function updateSpatialBlur(checked) {
  spatialAudioSettings.blur = checked;
  if (checked === true) {
    remoteUsers.forEach(e => {
      if (e.processor != undefined) {
        e.processor.updateSpatialBlur(true);
      }
    });
    localPlayProcessors.forEach(e => {
      if (e != undefined) {
        e.updateSpatialBlur(true);
      }
    });
  } else {
    remoteUsers.forEach(e => {
      if (e.processor != undefined) {
        e.processor.updateSpatialBlur(false);
      }
    });
    localPlayProcessors.forEach(e => {
      if (e != undefined) {
        e.updateSpatialBlur(false);
      }
    });
  }
}

function updateSpatialAirAbsorb(checked) {
  spatialAudioSettings.airAbsorb = checked;
  if (checked === true) {
    remoteUsers.forEach(e => {
      if (e.processor != undefined) {
        e.processor.updateSpatialAirAbsorb(true);
      }
    });
    localPlayProcessors.forEach(e => {
      if (e != undefined) {
        e.updateSpatialAirAbsorb(true);
      }
    });
  } else {
    remoteUsers.forEach(e => {
      if (e.processor != undefined) {
        e.processor.updateSpatialAirAbsorb(false);
      }
    });
    localPlayProcessors.forEach(e => {
      if (e != undefined) {
        e.updateSpatialAirAbsorb(false);
      }
    });
  }
}

async function joinSpatialAudioChannel(enabled, appid, apptoken, appchannel){
  const isEnabled = enabled == 0 ? false : true;
  console.log("join spatial audio channel", enabled, isEnabled, appid, apptoken, appchannel);
  if(isEnabled == true){
    options.appid = appid;
    options.token = apptoken;
    options.channel = appchannel;
    //await mockRemoteUserJoin();
    
    await join();

    
    processor = await getSpatialAudioProcessorInstance(enabled);
    

    return processor;
  } else {
    await leave();
  }
}
