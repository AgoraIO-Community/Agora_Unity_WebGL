// Caching the list of devices for the usage in Device Manager
async function cacheDevices() {
  await AgoraRTC.getPlaybackDevices()
    .then((cameras) => {
      wrapper.savePlayBackDevices(cameras);
      var fstr = "";
      var delim = "";
      cameras.forEach((cam) => {
        fstr +=
          delim +
          cam.deviceId +
          "," +
          cam.kind +
          "," +
          cam.label +
          "," +
          cam.groupId;
        delim = "|";
      });

      event_manager.raiseonPlaybackDevicesListing(fstr);
    })
    .catch((e) => {
      //console.log("get playback devices error!", e);
    });

  await AgoraRTC.getMicrophones()
    .then((mics) => {
      wrapper.saveMicrophones(mics);
      var fstr = "";
      var delim = "";
      mics.forEach((cam) => {
        fstr +=
          delim +
          cam.deviceId +
          "," +
          cam.kind +
          "," +
          cam.label +
          "," +
          cam.groupId;
        delim = "|";
      });

      event_manager.raiseonRecordingDevicesListing(fstr);
    })
    .catch((e) => {
      //console.log("get playback devices error!", e);
    });

  await AgoraRTC.getCameras()
    .then((cameras) => {
      wrapper.saveCameras(cameras);

      var fstr = "";
      var delim = "";
      cameras.forEach((cam) => {
        fstr +=
          delim +
          cam.deviceId +
          "," +
          cam.kind +
          "," +
          cam.label +
          "," +
          cam.groupId;
        delim = "|";
      });

      event_manager.raiseonCamerasListing(fstr);
    })
    .catch((e) => {
      //console.log("get cameras error!", e);
    });
}

async function cachePlaybackDevices(){
  await AgoraRTC.getPlaybackDevices()
    .then((cameras) => {
      wrapper.savePlayBackDevices(cameras);
      var fstr = "";
      var delim = "";
      cameras.forEach((cam) => {
        fstr +=
          delim +
          cam.deviceId +
          "," +
          cam.kind +
          "," +
          cam.label +
          "," +
          cam.groupId;
        delim = "|";
      });

      event_manager.raiseonPlaybackDevicesListing(fstr);
    })
    .catch((e) => {
      //console.log("get playback devices error!", e);
    });
}

async function cacheMicrophones(){
  await AgoraRTC.getMicrophones()
    .then((mics) => {
      wrapper.saveMicrophones(mics);
      var fstr = "";
      var delim = "";
      mics.forEach((cam) => {
        fstr +=
          delim +
          cam.deviceId +
          "," +
          cam.kind +
          "," +
          cam.label +
          "," +
          cam.groupId;
        delim = "|";
      });

      event_manager.raiseonRecordingDevicesListing(fstr);
    })
    .catch((e) => {
      //console.log("get playback devices error!", e);
    });
}

async function cacheVideoDevices(){
  await AgoraRTC.getCameras()
    .then((cameras) => {
      wrapper.saveCameras(cameras);

      var fstr = "";
      var delim = "";
      cameras.forEach((cam) => {
        fstr +=
          delim +
          cam.deviceId +
          "," +
          cam.kind +
          "," +
          cam.label +
          "," +
          cam.groupId;
        delim = "|";
      });

      event_manager.raiseonCamerasListing(fstr);
    })
    .catch((e) => {
      //console.log("get cameras error!", e);
    });
}
