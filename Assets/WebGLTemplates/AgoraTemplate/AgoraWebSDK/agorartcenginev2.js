// Single Channel functions
async function createIRtcEngine(appID) {
  return client_manager.createEngine(appID);
}

async function createIRtcEngine2(appID, areaCode) {
  return client_manager.createEngine2(appID, areaCode);
}

//Allows a user to join a channel.
async function wglw_joinChannel(channelkey, channelName, info, uid) {
  client_manager.setOptions(channelkey, channelName, uid);
  await client_manager.joinAgoraChannel(uid);
  wrapper.initStats();
  cacheDevices();
}

async function wglw_joinChannel_withOption(
  token_str,
  channelId_str,
  info,
  uid,
  subscribeAudio, subscribeVideo,
  publishAudio, publishVideo
) {
  client_manager.setAVControl(subscribeAudio, subscribeVideo, publishAudio, publishVideo);
  await wglw_joinChannel(token_str, channelId_str, info, uid);
}

async function joinChannelWithUserAccount_WGL(
  token_str,
  channelId_str,
  userAccount_str
) {
  client_manager.setOptions(token_str, channelId_str);
  await client_manager.joinAgoraChannel(userAccount_str);
  wrapper.initStats();
  cacheDevices();
}

async function joinChannelWithUserAccount_engine_WGL(
  token_str,
  channelId_str,
  userAccount_str,
  subscribeAudio, subscribeVideo,
  publishAudio, publishVideo
) {
  client_manager.setAVControl(subscribeAudio, subscribeVideo, publishAudio, publishVideo);
  await joinChannelWithUserAccount_WGL(token_str, channelId_str, userAccount_str);
}

// Allows a user to leave a channel, such as hanging up or exiting a call.
async function wglw_leaveChannel() {
  await client_manager.leave();
}

function setChannelProfile(profile) {
  client_manager.setChannelProfile(profile);
}

function switchChannel_WGL(token_str, channelId_str) {
  client_manager.switchChannel(token_str, channelId_str);
}

function switchChannel2_WGL(token_str, channelId_str,
  subscribeAudio, subscribeVideo,
  publishAudio, publishVideo
) {
  client_manager.setAVControl(subscribeAudio, subscribeVideo, publishAudio, publishVideo);
  client_manager.switchChannel(token_str, channelId_str);
}

function setMLocal(val) {
  mlocal = val;
  $("#info_mirror_local").text("Local mirror: " + mlocal);
}

function setMRemote(val) {
  mremote = val;
  $("#info_mirror_remote").text("Remote mirror: " + mremote);
}

// set beauty effect On
async function setBeautyEffectOn(
  lighteningLevel,
  rednessLevel,
  smoothnessLevel
) {
  client_manager.setBeautyEffectOn(
    lighteningLevel,
    rednessLevel,
    smoothnessLevel
  );
}

// set beauty effect Off
async function setBeautyEffectOff() {
  client_manager.setBeautyEffectOff();
}

function setCameraCapturerConfiguration(preference, cameraDirection) {
  AgoraRTC.createCameraVideoTrack({ encoderConfig: curVideoProfile.value });
}

function getUserInfoByUid_WGL(uid) {
  if (typeof remoteUsers[uid] === "undefined") {
    return 0;
  } else {
    return uid;
  }
}

// Stops/Resumes sending the local video stream.
async function enableLocalVideo(enabled) {
  client_manager.enableLocalVideo(enabled);
}

// Starts the local video preview before joining the channel
async function startPreview() {
  client_manager.startPreview();
}

// Stops the local video preview and disables video.
function stopPreview() {
  client_manager.stopPreview();
}

// Gets a new token when the current token expires after a period of time.
async function renewToken(token) {
  client_manager.renewToken(token);
}

//enables the audio volume indicator so now we are geting event of volume indications
function enableAudioVolumeIndicator() {
  client_manager.enableAudioVolumeIndicator();
}

// create a data stream
function createDataStream(needRetry) {
  client_manager.createDataStream(needRetry);
}
// sends a stream message of byte array
function sendStreamMessage(data) {
  return client_manager.sendDataStream(data);
}

// Sets the built-in encryption mode.
function setEncryptionMode(mode) {
  var modes = [
    "aes-128-xts",
    "aes-256-xts",
    "aes-128-ecb",
    "sm4-128-ecb",
    "none",
  ];
  var n = modes.includes(mode);
  if (n) {
    savedEncryptionMode = mode;
  }
}

// Enables built-in encryption with an encryption password before users join a channel.
function setEncryptionSecret(secret) {
  client_manager.setEncryptionSecret(secret);
}

//  Sets the role of the user, such as a host or an audience (default), before joining a channel in the interactive live streaming.
async function setClientRole(role) {
  client_manager.setClientRole(role, null);
}
async function setClientRole1(role, audienceLatencyLevel) {
  client_manager.setClientRole(role, audienceLatencyLevel);
}

async function setMirrorApplied_WGL(apply) {
  if (apply == 1) {
    wrapper.savedSettings.mirrorOptions = { fit: "cover", mirror: true };
    if (localTracks.videoTrack) {
      localTracks.videoTrack.stop();
      localTracks.videoTrack.play(
        "local-player",
        wrapper.savedSettings.mirrorOptions
      );
    }
  } else if (apply == 0) {
    wrapper.savedSettings.mirrorOptions = { fit: "cover", mirror: false };
    if (localTracks.videoTrack) {
      localTracks.videoTrack.stop();
      localTracks.videoTrack.play(
        "local-player",
        wrapper.savedSettings.mirrorOptions
      );
    }
  }
}

function setAudioRecordingDeviceVolume(volume) {
  if (localTracks.audioTrack) {
    localTracks.audioTrack.setVolume(volume);
  }
  wrapper.savedSettings.localAudioTrackVolume = volume;
}

// adjust volume of all remote users
function adjustPlaybackSignalVolume_WGL(volume) {
  Object.keys(remoteUsers).forEach((uid) => {
    var audioTrack = remoteUsers[uid]._audioTrack;
    if(audioTrack) {
      audioTrack.setVolume(volume);
    }
  });
}

function adjustUserPlaybackSignalVolume_WGL(uid, volume) {
  Object.keys(remoteUsers).forEach((uid_in) => {
    if (uid_in == uid) {
      var audioTrack = remoteUsers[uid_in]._audioTrack;
      if(audioTrack) {
        audioTrack.setVolume(volume);
      }
    }
  });
}

function setAudioPlaybackDeviceVolume(volume) {
  wrapper.savedSettings.playbackVolume = volume;
  Object.keys(remoteUsers).forEach((uid) => {
    var audioTrack = remoteUsers[uid]._audioTrack;
    if(audioTrack) {
      audioTrack.setVolume(volume);
    }
  });
  if (localTracks.audioMixingTrack) {
    localTracks.audioMixingTrack.setVolume(volume);
  }
}

function adjustRecordingSignalVolume_WGL(volume) {
  if (wrapper) {
    wrapper.savedSettings.recordingSignalVolume = volume;
  }
}

function setAudioPlaybackDeviceMute(mute) {
  if (mute == 1) {
    Object.keys(remoteUsers).forEach((uid) => {
      var audioTrack = remoteUsers[uid]._audioTrack;
      if(audioTrack) {
        audioTrack._mediaStreamTrack.enabled = false;
      }
    });
    if (localTracks.audioMixingTrack) {
      localTracks.audioMixingTrack._mediaStreamTrack.enabled = false;
    }
    pd_muted = 1;
  } else {
    Object.keys(remoteUsers).forEach((uid) => {
      var audioTrack = remoteUsers[uid]._audioTrack;
      if(audioTrack) {
        audioTrack._mediaStreamTrack.enabled = true;
      }
    });
    if (localTracks.audioMixingTrack) {
      localTracks.audioMixingTrack._mediaStreamTrack.enabled = true;
    }
    pd_muted = 0;
  }
}

function setAudioRecordingDeviceMute(mute) {
  if (mute == 1) {
    if (localTracks.audioTrack) {
      localTracks.audioTrack._mediaStreamTrack.enabled = false;
    }
  } else {
    if (localTracks.audioTrack) {
      localTracks.audioTrack._mediaStreamTrack.enabled = true;
    }
  }
}

async function setLocalAudioTrackMicrophone(deviceId) {
  if (localTracks.audioTrack) {
    localTracks.audioTrack.setDevice(deviceId);
  }
}

async function setLocalTrackCamera(deviceId) {
  if (localTracks.videoTrack) {
    localTracks.videoTrack.setDevice(deviceId);
  }
}

async function setVideoDeviceCollectionDeviceWGL(deviceId) {
  if (currentVideoDevice === "") {
    currentVideoDevice = deviceId;
    event_manager.raiseGetCurrentVideoDevice(currentVideoDevice);
  } else {
    currentVideoDevice = deviceId;
    event_manager.raiseGetCurrentVideoDevice(currentVideoDevice);
    setLocalTrackCamera(currentVideoDevice);
  }
}

async function setPlaybackCollectionDeviceWGL(deviceId) {
  currentPlayBackDevice = deviceId;
  Object.keys(remoteUsers).forEach((uid) => {
    var audioTrack = remoteUsers[uid]._audioTrack;
    if(audioTrack) {
      audioTrack.setPlaybackDevice(deviceId);
    }
  });
  if (localTracks.audioMixingTrack) {
    localTracks.audioMixingTrack.setPlaybackDevice(deviceId);
  }

  event_manager.raiseGetCurrentPlayBackDevice(currentPlayBackDevice);
}

async function setAudioRecordingCollectionDeviceWGL(deviceId) {
  currentAudioDevice = deviceId;
  event_manager.raiseGetCurrentAudioDevice(currentAudioDevice);
  setLocalAudioTrackMicrophone(deviceId);
}

function handleConnectionStateChange(curState, revState, reason) { }

async function startScreenCaptureForWeb(enableAudio) {
  client_manager.startScreenCapture(enableAudio);
}

function startNewScreenCaptureForWeb(uid, enableAudio) {
  console.log("agora engine startNewScreenCaptureForWeb");
  client_manager.startNewScreenCaptureForWeb(uid, enableAudio);
}

function stopNewScreenCaptureForWeb(){
  console.log("agora engine stopNewScreenCaptureForWeb");
  client_manager.stopNewScreenCaptureForWeb();
}

function setRemoteUserSpatialAudioParams(uid, azimuth, elevation, distance, orientation, attenuation, blur, airAbsorb){
  client_manager.setRemoteUserSpatialAudioParams(uid, azimuth, elevation, distance, orientation, attenuation, blur, airAbsorb);
}

async function startScreenCaptureByDisplayId(
  displayId,
  x,
  y,
  width,
  height,
  screenCaptureVideoDimenWidth,
  screenCaptureVideoDimenHeight,
  screenCaptureFrameRate,
  screenCaptureBitrate,
  screenCaptureCaptureMouseCursor
) {
  client_manager.startScreenCaptureByDisplayId(
    displayId,
    x,
    y,
    width,
    height,
    screenCaptureVideoDimenWidth,
    screenCaptureVideoDimenHeight,
    screenCaptureFrameRate,
    screenCaptureBitrate,
    screenCaptureCaptureMouseCursor
  );
}

async function stopScreenCapture() {
  client_manager.stopScreenCapture();
}

async function switchCamera() {
  var curCamId = localTracks.videoTrack.getMediaStreamTrack().label;
  var otherCams = new Array();

  AgoraRTC.getCameras()
    .then((cameras) => {
      cameras.forEach((cam) => {
        if (cam.label == curCamId) {
          //console.log("found cur camera");
        } else {
          otherCams.push(cam.deviceId);
        }
      });

      if (otherCams.length > 0) {
        localTracks.videoTrack.setDevice(otherCams[0]);
      }
    })
    .catch((e) => {
      console.log("get cameras error!", e);
    });
}

// Starts the last-mile network probe test.
function enableLastMile(enabled) {
  client_manager.enableLastMile(enabled);
}

// Stops/Resumes receiving all remote users' audio streams.
async function muteAllRemoteAudioStreams(mute) {
  Object.keys(remoteUsers).forEach((uid) => {
    if (mute == true) {
      unsubscribe(remoteUsers[uid], "audio");
    } else {
      subscribe_mv(remoteUsers[uid], "audio");
    }
  });
}

// Stops/Resumes receiving all remote users' video streams.
async function muteAllRemoteVideoStreams(mute) {
  Object.keys(remoteUsers).forEach((uid) => {
    //unsubscribe not working so trying another approach
    if (mute == true) {
      unsubscribe(remoteUsers[uid], "video");
    } else {
      subscribe_mv(remoteUsers[uid], "video");
    }
  });
}

// Stops/Resumes receiving the video stream from a specified remote user.
async function MuteRemoteVideoStream(uid, mute) {
  Object.keys(remoteUsers).forEach((uid2) => {
    if (uid2 == uid) {
      if (mute == true) {
        unsubscribe(remoteUsers[uid], "video");
      } else {
        subscribe_mv(remoteUsers[uid], "video");
      }
    }
  });
}

// Stops/Resumes receiving the audio stream from a specified remote user.
async function MuteRemoteAudioStream(uid, mute) {
  Object.keys(remoteUsers).forEach((uid2) => {
    if (uid2 == uid) {
      if (mute == true) {
        unsubscribe(remoteUsers[uid], "audio");
      } else {
        subscribe_mv(remoteUsers[uid], "audio");
      }
    }
  });
}

async function subscribe_mv(user, mediaType) {
  client_manager.subscribe_mv(user, mediaType);
}

async function unsubscribe(user, mediaType) {
  client_manager.unsubscribe(user, mediaType);
}

async function enableAudio(enabled) {
  client_manager.enableAudio(enabled);
}

async function enableLocalAudio(enabled) {
  client_manager.enableLocalAudio(enabled);
}

// Disables/Re-enables the local audio function.
async function enableDisableAudio(enabled) {
  if (localTracks.audioTrack) {
    if (enabled == false) {
      if (localTracks.audioTrack) {
        localTracks.audioTrack.setVolume(0);
      }
    } else {
      if (localTracks.audioTrack) {
        localTracks.audioTrack.setVolume(100);
      }
    }
  }
}

function muteLocalAudioStream(mute) {
  client_manager.muteLocalAudioStream(mute);
}

// Stops/Resumes sending the local video stream.
function muteLocalVideoTrack(mute) {
  client_manager.muteLocalVideoStream(mute);
}

// Sets the stream type for all remote users
async function setRemoteDefaultVideoStreamType(streamType) {
  Object.keys(remoteUsers).forEach((uid2) => {
    client_manager.SetRemoteVideoSTreamType(uid2, streamType);
  });
}

async function enableDualStream_WGL(enabled) {
  client_manager.enableDualStream_WGL(enabled);
}

async function setLocalPublishFallbackOption_WGL(option) {
  client_manager.setLocalPublishFallbackOption_WGL(option);
}

function SetRemoteUserPriority(uid, userPriority) {
  client_manager.SetRemoteUserPriority(uid, userPriority);
}

function setRemoteSubscribeFallbackOption_WGL(option) {
  client_manager.setRemoteSubscribeFallbackOption_WGL(option);
}

function enableLogUpload() {
  client_manager.enableLogUpload();
}

function disableLogUpload() {
  client_manager.disableLogUpload();
}
// Sets the video encoder configuration.
async function SetVideoEncoderConfiguration(
  Width,
  Height,
  FrameRate,
  MinFrameRate,
  Bitrate,
  MinBitrate,
  OrientationMode,
  DegradationPreference,
  VideoMirrorMode
) {
  var updatedConfig;
  updatedConfig = {
    width: Width,
    height: Height,
    frameRate: FrameRate,
    bitrateMin: MinBitrate,
    bitrateMax: Bitrate,
    orientationMode: OrientationMode,
    //degradationPreference: DegradationPreference,
    //videoMirrorMode: VideoMirrorMode,
  };

  if (localTracks.videoTrack &&!localTracks.videoTrack.customVideoEnabled) {
    localTracks.videoTrack && await localTracks.videoTrack.setEncoderConfiguration(updatedConfig);
  }
  
  client_manager.setVideoConfiguration(updatedConfig);
}

// Setting Live Transcoding Configuration
function SetLiveTranscoding(
  Width,
  Height,
  VideoBitrate,
  VideoFramerate,
  LowLatency,
  VideoGroup,
  Video_codec_profile,
  BackgroundColor,
  UserCount,
  StrTranscodingUserInfo,
  StrTranscodingExtraInfo,
  StrMetaData,
  StrWatermarkRtcImageUrl,
  WatermarkRtcImageX,
  WatermarkRtcImageY,
  WatermarkRtcImageWidth,
  WatermarkRtcImageHeight,
  StrBackgroundImageRtcImageUrl,
  BackgroundImageRtcImageX,
  BackgroundImageRtcImageY,
  BackgroundImageRtcImageWidth,
  BackgroundImageRtcImageHeight,
  AudioSampleRate,
  AudioBitrate,
  AudioChannels,
  AudioCodecProfile,
  StrAdvancedFeatures,
  AdvancedFeatureCount
) {
  const arrUserInfo = StrTranscodingUserInfo.split("\t");
  var transcodingUsers = [];
  var i = 0;
  while (i < arrUserInfo.length) {
    const transcodingUser = {
      uid: Number(arrUserInfo[i++]),
      x: Number(arrUserInfo[i++]),
      y: Number(arrUserInfo[i++]),
      width: Number(arrUserInfo[i++]),
      height: Number(arrUserInfo[i++]),
      zOrder: Number(arrUserInfo[i++]),
      alpha: Number(arrUserInfo[i++]),
      audioChannel: Number(arrUserInfo[i++]),
    };
    transcodingUsers.push(transcodingUser);
  }
  // Removing last blank object
  transcodingUsers.pop();

  //  configuration of pushing stream to cdn
  liveTranscodingConfig = {
    width: Width,
    height: Height,
    videoBitrate: VideoBitrate,
    videoFramerate: VideoFramerate,
    audioSampleRate: AudioSampleRate,
    audioBitrate: AudioBitrate,
    audioChannels: AudioChannels,
    videoGop: VideoGroup,
    videoCodecProfile: Video_codec_profile,
    userCount: UserCount,
    backgroundColor: BackgroundColor,
    transcodingUsers,
  };

  // .equals property throws error at runtime.
  if (StrWatermarkRtcImageUrl == "") {
    StrWatermarkRtcImageUrl = DEFAULT_IMAGE_URL_TRANSCODING;
  } else {
    liveTranscodingConfig.watermark = {
      url: StrWatermarkRtcImageUrl,
      x: WatermarkRtcImageX,
      y: WatermarkRtcImageY,
      width: WatermarkRtcImageWidth == 0 ? 200 : WatermarkRtcImageWidth,
      height: WatermarkRtcImageHeight == 0 ? 200 : WatermarkRtcImageHeight,
    };
  }

  if (StrBackgroundImageRtcImageUrl == "") {
    StrBackgroundImageRtcImageUrl = DEFAULT_BG_URL_TRANSCODING;
  } else {
    liveTranscodingConfig.backgroundImage = {
      url: StrBackgroundImageRtcImageUrl,
      x: BackgroundImageRtcImageX,
      y: BackgroundImageRtcImageY,
      width:
        BackgroundImageRtcImageWidth == 0 ? 1080 : BackgroundImageRtcImageWidth,
      height:
        BackgroundImageRtcImageHeight == 0
          ? 520
          : BackgroundImageRtcImageHeight,
    };
  }
}

// Publishes the local stream to a specified CDN streaming URL. (CDN live only.)
async function StartLiveTranscoding(transcodingUrl, transcodingEnabled) {
  var client = client_manager.getClient();
  if (client) {
    if (!transcodingUrl) {
      console.error("you should input liveStreaming URL");
      return;
    }
    try {
      // To monitor errors in the middle of the push, please refer to the API documentation for the list of error codes
      client.on("live-streaming-error", (url, err) => {
        console.error("url", url, "live streaming error!", err.code);
      });

      // set live streaming transcode configuration,
      await client.setLiveTranscoding(liveTranscodingConfig);
      // then start live streaming.
      await client.startLiveStreaming(transcodingUrl, transcodingEnabled);
    } catch (error) {
      console.error("live streaming error:", error.message);
    }
  }
}

// Removes an RTMP or RTMPS stream from the CDN. (CDN live only.). There was a conflict on Naming so 's' is used as suffix.
function sStopLiveTranscoding(url) {
  var client = client_manager.getClient();
  if (client) {
    client.stopLiveStreaming(url).then(() => {
      //console.log("stop live streaming success");
    });
  }
}

async function PlayEffect(
  soundId,
  filePath,
  loopCount,
  pitch,
  pan,
  gain,
  publish
) {
  audioEffects._PlayEffect(soundId, filePath, loopCount, publish);
}

function StopEffect(soundId) {
  audioEffects._stopEffect(soundId);
}

function StopAllEffects() {
  audioEffects._stopAllEffects();
}

async function PreloadEffect(soundId, filePath) {
  audioEffects._PlayEffect(soundId, filePath, 1, false);
}

function UnloadEffect(soundId) {
  audioEffects._unloadEffect(soundId);
}

function GetEffectsVolume() {
  return audioEffects._getEffectsVolume();
}

function PauseEffect(soundId) {
  audioEffects._pauseEffect(soundId);
}

function ResumeEffect(soundId) {
  audioEffects._resumeEffect(soundId);
}

function PauseAllEffects() {
  audioEffects._pauseAllEffects();
}

function ResumeAllEffects() {
  audioEffects._resumeAllEffects();
}

function SetEffectsVolume(volume) {
  audioEffects._setEffectsVolume(volume);
}

//Need to re-check once. As per documentation we need stop all other tracks than microphone track
async function SetVoiceOnlyMode(enable) {
  var client = client_manager.getClient();
  if (client) {
    audioEffects._stopAllEffects();
    if (enable) {
      if (localTracks.audioMixingTrack) {
        await client.unpublish(localTracks.audioMixingTrack);
      }
      if (localTracks.videoTrack) {
        await client.unpublish(localTracks.videoTrack);
      }
    } else {
      if (localTracks.audioMixingTrack) {
        await client.publish(localTracks.audioMixingTrack);
      }
      if (localTracks.videoTrack) {
        await client.publish(localTracks.videoTrack);
      }
    }
  }
}

function SetVolumeOfEffect(soundId, volume) {
  audioEffects._setVolumeOfEffect(soundId, volume);
}

function AdjustAudioMixingVolume(volume) {
  if (localTracks.audioMixingTrack)
    localTracks.audioMixingTrack.setVolume(volume);
}

function SetAudioMixingPosition(position) {
  if (localTracks.audioMixingTrack) {
    localTracks.audioMixingTrack.seekAudioBuffer(position);
  }
}

function GetAudioMixingDuration() {
  if (localTracks.audioMixingTrack)
    return localTracks.audioMixingTrack.duration;
  return 0;
}

function GetAudioMixingCurrentPosition() {
  if (localTracks.audioMixingTrack)
    return localTracks.audioMixingTrack.getCurrentTime();
  return 0;
}

function AdjustAudioMixingPlayoutVolume(volume) {
  if (localTracks.audioMixingTrack) {
    localTracks.audioMixingTrack.setVolume(volume);
    wrapper.savedSettings.audioMixingPlayoutVolume = volume;
  }
}

function GetAudioMixingPlayoutVolume() {
  return wrapper.savedSettings.audioMixingPlayoutVolume;
}

function AdjustAudioMixingPublishVolume(volume) {
  if (localTracks.audioMixingTrack) {
    localTracks.audioMixingTrack.setVolume(volume);
    wrapper.savedSettings.audioMixingPublishVolume = volume;
  }
}

function GetAudioMixingPublishVolume() {
  return wrapper.savedSettings.audioMixingPublishVolume;
}

function startChannelMediaRelay(
  srcChannelName,
  srcToken,
  srcUid,
  destChannelName,
  destToken,
  destUid,
  destCount
) {
  var client = client_manager.getClient();
  if (client) {
    const configuration = AgoraRTC.createChannelMediaRelayConfiguration();
    configuration.setSrcChannelInfo({
      channelName: srcChannelName,
      token: srcToken,
      uid: Number(srcUid),
    });
    configuration.addDestChannelInfo({
      channelName: destChannelName,
      token: destToken,
      uid: Number(destUid),
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

function updateChannelMediaRelay(
  srcChannelName,
  srcToken,
  srcUid,
  destChannelName,
  destToken,
  destUid,
  destCount
) {
  var client = client_manager.getClient();
  if (client) {
    const configuration = AgoraRTC.createChannelMediaRelayConfiguration();
    configuration.setSrcChannelInfo({
      channelName: srcChannelName,
      token: srcToken,
      uid: Number(srcUid),
    });
    configuration.addDestChannelInfo({
      channelName: destChannelName,
      token: destToken,
      uid: Number(destUid),
    });

    client
      .updateChannelMediaRelay(configuration)
      .then(() => {
        //console.log("updateChannelMediaRelay success");
      })
      .catch((e) => {
        //console.log("updateChannelMediaRelay failed", e);
      });
  }
}

function stopChannelMediaRelay() {
  var client = client_manager.getClient();
  if (client) {
    client
      .stopChannelMediaRelay()
      .then(() => {
        //console.log("stopChannelMediaRelay success");
      })
      .catch((e) => {
        //console.log("stopChannelMediaRelay failed", e);
      });
  }
}

function setWebParametersInt(key, value) {
  AgoraRTC.setParameter(key, value);
}
function setWebParametersDouble(key, value) {
  AgoraRTC.setParameter(key, value);
}
function setWebParametersBool(key, value) {
  AgoraRTC.setParameter(key, value);
}
function setWebParametersString(key, value) {
  AgoraRTC.setParameter(key, value);
}

function getRemoteVideoStats() {
  client_manager.getRemoteVideoStats();
}

function initVirtualBackground(){
  client_manager.enableVirtualBackground();
}

function setVirtualBackgroundBlur(blurDegree){
  client_manager.setVirtualBackgroundBlur(blurDegree);
}

function setVirtualBackgroundColor(hexColor){
  client_manager.setVirtualBackgroundColor(hexColor);
}

function setVirtualBackgroundImage(imgFile){
  client_manager.setVirtualBackgroundImage(imgFile);
}

function setVirtualBackgroundVideo(videoFile){
  client_manager.setVirtualBackgroundVideo(videoFile);
}

function enableSpatialAudio(enabled){
  client_manager.enableSpatialAudio(enabled);
}
