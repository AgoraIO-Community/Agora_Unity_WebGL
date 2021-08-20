var idxTest = 0;
async function testCallBackFrame() {
  if (localTracks.audioTrack) {
    localTracks.audioTrack.setAudioFrameCallback((buffer) => {
      for (let channel = 0; channel < buffer.numberOfChannels; channel += 1) {
        const currentChannelData = buffer.getChannelData(channel);

        var datachunk = "";
        currentChannelData.forEach((arr) => {
          datachunk += "," + arr;
        });

        event_manager.raiseFrameHandler(datachunk);

        idxTest++;
        if (idxTest > 10) {
          stopCallBackFrame();
        }
      }
    }, 2048);
  }
}

async function stopCallBackFrame() {
  if (localTracks.audioTrack) {
    localTracks.audioTrack.setAudioFrameCallback(null);
  }
}

async function testPublish() {
  await client.publish(
    Object.values(localTracks).filter((track) => track !== null)
  );
}

function getDevices() {}

function testFindVid(uid) {
  var lVid = undefined; // set null initially

  if (remoteUsers[uid] != undefined) {
    if (remoteUsers[uid].videoTrack._player != undefined) {
      lVid = remoteUsers[uid].videoTrack._player.videoElement;
    } else {
      //console.log("no player found");
    }
  }

  if (lVid == undefined) {
    return;
  }
}

// inject stream test
function testInjectStream() {
  const injectStreamConfig = {
    width: 200,
    height: 150,
    videoGop: 30,
    videoFramerate: 15,
    videoBitrate: 400,
    audioSampleRate: 44100,
    audioChannels: 1,
  };

  var client = client_manager.getClient();
  if (client) {
    client
      .addInjectStreamUrl(
        "rtmps://live-api-s.facebook.com:443/rtmp/241978017594177?s_bl=1&s_psm=1&s_sc=241978057594173&s_sw=0&s_vt=api-s&a=AbxedTrRq7MgVERw",
        injectStreamConfig
      )
      .then(() => {
        //console.log("add inject stream url success");
      })
      .catch((e) => {
        //console.log("add inject stream failed", e);
      });
  }
}

function testStopInjectStream() {
  var client = client_manager.getClient();
  if (client) {
    client
      .removeInjectStreamUrl()
      .then(() => {
        //console.log("remove inject stream url success");
      })
      .catch((e) => {
        //console.log("remove inject stream failed", e);
      });
  }
}

function testMediaRelay(srcuid, destuid) {
  var client = client_manager.getClient();
  if (client) {
    const configuration = AgoraRTC.createChannelMediaRelayConfiguration();
    configuration.setSrcChannelInfo({
      channelName: "unity3d",
      token: "",
      uid: srcuid,
    });
    configuration.addDestChannelInfo({
      channelName: "unity2d",
      token: "",
      uid: destuid,
    });

    client
      .startChannelMediaRelay(configuration)
      .then(() => {
        //console.log("startChannelMediaRelay success");
      })
      .catch((e) => {
        //console.log("startChannelMediaRelay failed", e);
      });
  }
}

function testCustom() {
  var stream = mainCanvas.captureStream(25); // 25 FPS
  var CustomVideoTrackInitConfig = {
    bitrateMax: 400,
    bitrateMin: 200,
    mediaStreamTrack: stream,
    optimizationMode: "detail",
  };
  //console.log(CustomVideoTrackInitConfig);
}

// test raw data
async function testRawData() {
  if (localTracks.videoTrack) {
    //ImageData
    var imgdt = localTracks.videoTrack.getCurrentFrameData();
    //console.log(imgdt);
  }
}
