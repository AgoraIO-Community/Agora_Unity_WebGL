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
  }

  return processor;
}


function localPlayerStop() {
  for (let i = 0; i < localPlayerSound.length; i++) {
    localPlayTracks[i].stop();
  }
  localPlayTracks = [];
}

async function mockRemoteUserJoin() {
  for (let i = 0; i < remoteUsersSound.length; i++) {
    setTimeout(async () => {
      try {
        const track = await AgoraRTC.createBufferSourceAudioTrack({ source: remoteUsersSound[i] });
        track.startProcessAudioBuffer({ loop: true });
        const client = AgoraRTC.createClient({ mode: "rtc", codec: "vp8" });
        remoteUsers[i] = client;
        await client.join(options.appid, options.channel, options.token || null);
        await client.publish(track);
      } catch (error) {
        console.error(`remoteUsersSound[${i}] with buffersource track ${remoteUsersSound[i]} join and publish fail: ${error}`);
      }
    }, 500 * i);
  }
}
async function mockRemoteUserLeave() {
  for (let i = 0; i < remoteUsersSound.length; i++) {
    try {
      await remoteUsers[i].leave();
      console.log(`speaker[${i}] with buffersource track ${remoteUsersSound[i]} leave success`);
    } catch (error) {
      console.error(`speaker[${i}] with buffersource track ${remoteUsersSound[i]} leave fail: ${error}`);
    }
  }
}

async function join() {
  client.on("user-published", handleUserPublished);
  client.on("user-unpublished", handleUserUnpublished);

  [options.uid,] = await Promise.all([
    client.join(options.appid, options.channel, options.token || null),
  ]);
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
  remoteUsers = [];
  await client.leave();
  await mockRemoteUserLeave();
  localPlayerStop();

  console.log("client leaves channel success");
}

async function subscribe(user, mediaType) {
  const uid = user.uid;
  await client.subscribe(user, mediaType);
  console.log("subscribe success");
  
  if (mediaType === 'audio') {
    processor = extension.createProcessor();
    user.processor = processor;
    remoteUsers.push(user);
    console.log(remoteUsers);
    const track = await AgoraRTC.createBufferSourceAudioTrack({ source: remoteUsersSound[0] });

    if(track.processorDestination != processor)
      track.pipe(processor).pipe(track.processorDestination);
    
    track.play();
  }
}

function handleUserPublished(user, mediaType) {
  const id = user.uid;
  subscribe(user, mediaType);
}

function handleUserUnpublished(user) {
  const id = user.uid;
  for(var i = 0; i < remoteUsers.length; i++){
    remoteUsers[i] = null;
  }
}

function  updateSpatialAzimuth(value) {
  console.log("play processors...", localPlayProcessors, remoteUsers);
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

async function joinSpatialAudioChannel(enabled, appid, apptoken, appchannel){
  options.appid = appid;
    options.token = apptoken;
    options.channel = appchannel;
    await mockRemoteUserJoin();
    await join();
    processor = await getSpatialAudioProcessorInstance(enabled);
    return processor;
}
