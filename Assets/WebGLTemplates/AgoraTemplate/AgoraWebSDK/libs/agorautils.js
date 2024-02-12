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
  console.log("caching Playback Devices");
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
  console.log("caching Microphones Devices");
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
  console.log("caching Video Devices");
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

function hex2ascii(hexx)
{
  const hex = hexx.toString();//force conversion
  let str = '';
  for (let i = 0; i < hex.length; i += 2)
    str += String.fromCharCode(parseInt(hex.substr(i, 2), 16));
  return str;
}
function base64ToUint8Array(base64Str)
{
  const raw = window.atob(base64Str);
  const result = new Uint8Array(new ArrayBuffer(raw.length));
  for (let i = 0; i < raw.length; i += 1)
  {
    result[i] = raw.charCodeAt(i);
  }
  return result;
}

const ENCRYPTION_MODE = {
  1:"aes-128-xts", 
  2:"aes-128-ecb",
  3:"aes-256-xts",
  4:"sm4-128-ecb",
  5:"aes-128-gcm",
  6:"aes-256-gcm",
  7:"aes-128-gcm2",
  8:"aes-256-gcm2"
}
