function setMultiChannelWant_MC(multiChannelWant) {
  if (multiChannelWant) {
    multiChannelWant_MC = true;
  } else {
    multiChannelWant_MC = false;
  }
}

var mc2 = false; // indicates live or rtc profile
var roles = {}; // dictionary saving role of the user in this channel
function setClientMode_RTC() {
  mc2 = false;
}

function setClientMode_LIVE() {
  mc2 = true;
}

function wgl_mc_createChannel(channelId) {
  if (multiChannelWant_MC) {
    var c = new AgoraChannel();
    c.channelId = channelId;
    clients[channelId] = c;

    selectedCurrentChannel = channelId;

    if (mc2 == false) {
      c.createClient();
    } else {
      c.createClient_Live();
      if (roles[channelId] != undefined) {
        c.setClientRole2_MC(roles[channelId]);
      }
    }
  } else {
    throw "Cannot create Channel as Multi Channel want is set to False"; // throw a text
  }
}

function joinChannelWithUserAccount_MC(
  token_str,
  userAccount_str,
  autoPublishAudio,
  autoPublishVideo
) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  } else {
    var c = clients[selectedCurrentChannel];
    c.joinChannelWithUserAccount_MC(
      token_str,
      userAccount_str,
      autoPublishAudio,
      autoPublishVideo
    );
  }
}

function renewToken2_mc(token_str) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  } else {
    var c = clients[selectedCurrentChannel];
    c.renewToken2_mc(token_str);
  }
}

function setCurrentChannel_WGL(channelId) {
  selectedCurrentChannel = channelId;
}

function setClientRole2_MC(role, optionLevel) {
  roles[selectedCurrentChannel] = role;
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return -1;
  } else {
    var c = clients[selectedCurrentChannel];
    c.setClientRole2_MC(role, optionLevel);
  }
}

function startScreenCaptureForWeb2(enableAudio) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 1;
  } else {
    var c = clients[selectedCurrentChannel]; 
    c.startScreenCapture(enableAudio);
  }
}

function stopScreenCapture2() {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 1;
  } else {
    var c = clients[selectedCurrentChannel]; 
    c.stopScreenCapture();
  }
}

function startNewScreenCaptureForWeb2(uid, audioEnabled) {
  console.log("Multichannel startNewScreenCaptureForWeb2");
  var c = clients[selectedCurrentChannel];
  c.startNewScreenCaptureForWeb2(uid, audioEnabled);
}

function stopNewScreenCaptureForWeb2(){
  console.log("Multichannel stopNewScreenCaptureForWeb2");
  var c = clients[selectedCurrentChannel];
  c.stopNewScreenCaptureForWeb2();
}

function getConnectionState2_MC() {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 1;
  } else {
    var c = clients[selectedCurrentChannel];
    var state = c.getConnectionState();
    if (state == "DISCONNECTED") {
      return 1;
    } else if (state == "CONNECTED") {
      return 3;
    } else if (state == "CONNECTING") {
      return 2;
    } else if (state == "RECONNECTING") {
      return 4;
    } else if (state == "ABORTED") {
      return 5;
    } else {
      return 1;
    }
  }
}

function removePublishStreamUrl2_MC(channel, url) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  } else {
    var c = clients[selectedCurrentChannel];
    c.removePublishStreamUrl2_MC(channel, url);
  }
}

function addPublishStreamUrl2_MC(channel, url, transcodingEnabled) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  } else {
    var c = clients[selectedCurrentChannel];
    c.addPublishStreamUrl2_MC(channel, url, transcodingEnabled);
  }
}

function SetLiveTranscoding_MC(
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
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  } else {
    var c = clients[selectedCurrentChannel];
    c.SetLiveTranscoding(
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
    );
  }
}

function stopChannelMediaRelay_MC() {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  } else {
    var c = clients[selectedCurrentChannel];
    c.stopChannelMediaRelay_MC();
  }
}

function updateChannelMediaRelay_MC(
  srcChannelNameStr,
  srcTokenStr,
  srcUid_Str,
  destChannelNameStr,
  destTokenStr,
  destUid_Str,
  destCount
) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  } else {
    var c = clients[selectedCurrentChannel];
    c.updateChannelMediaRelay_MC(
      srcChannelNameStr,
      srcTokenStr,
      srcUid_Str,
      destChannelNameStr,
      destTokenStr,
      destUid_Str,
      destCount
    );
  }
}

function startChannelMediaRelay_MC(
  srcChannelNameStr,
  srcTokenStr,
  srcUid_Str,
  destChannelNameStr,
  destTokenStr,
  destUid_Str,
  destCount
) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  } else {
    var c = clients[selectedCurrentChannel];
    c.startChannelMediaRelay_MC(
      srcChannelNameStr,
      srcTokenStr,
      srcUid_Str,
      destChannelNameStr,
      destTokenStr,
      destUid_Str,
      destCount
    );
  }
}

function publish_mc_WGL() {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  } else {
    var c = clients[selectedCurrentChannel];
    c.publish();
  }
}

function unpublish_mc_WGL() {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  } else {
    var c = clients[selectedCurrentChannel];
    c.unpublish();
  }
}

function muteAllRemoteAudioStreams2_mc_WGL(mute) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  }
  var c = clients[selectedCurrentChannel];
  c.muteAllRemoteAudioStreams(mute);
}

function muteAllRemoteVideoStreams2_mc_WGL(mute) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  }
  var c = clients[selectedCurrentChannel];
  c.muteAllRemoteVideoStreams(mute);
}

function muteLocalAudioStream2_mc_WGL(channel, mute) {
  if (typeof clients[channel] === "undefined") {
    return 0;
  } 
  clients[channel].muteLocalAudioStream(mute);
}

function muteLocalVideoStream2_mc_WGL(channel, mute) {
  if (typeof clients[channel] === "undefined") {
    return 0;
  } 
  clients[channel].muteLocalVideoStream(mute);
}

function muteRemoteAudioStream2_mc_WGL(userId, mute) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  }
  var c = clients[selectedCurrentChannel];
  c.muteRemoteAudioStream(userId, mute);
}

function muteRemoteVideoStream2_mc_WGL(userId, mute) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  }
  var c = clients[selectedCurrentChannel];
  c.muteRemoteVideoStream(userId, mute);
}

function setRemoteVideoStreamType2_mc_WGL(userId, streamType) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  }
  var c = clients[selectedCurrentChannel];
  c.setRemoteVideoStreamType(userId, streamType);
}

function adjustUserPlaybackSignalVolume2_mc_WGL(userId, volume) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  }
  var c = clients[selectedCurrentChannel];
  c.adjustUserPlaybackSignalVolume(userId, volume);
}

function setRemoteDefaultVideoStreamType2_mc_WGL(streamType) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  }
  var c = clients[selectedCurrentChannel];
  c.setRemoteDefaultVideoStreamType(streamType);
}

function setRemoteUserPriority2_mc_WGL(uid, userPriority) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  }
  var c = clients[selectedCurrentChannel];
  c.setRemoteUserPriority(uid, userPriority);
}

function setEncryptionMode2_mc_WGL(mode) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  }
  var c = clients[selectedCurrentChannel];
  c.setEncryptionMode(mode);
}

function enableEncryption2_mc(enable, encryptionKey_Str, encryptionMode) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  }
  var c = clients[selectedCurrentChannel];
  c.enableEncryption2_mc(enable, encryptionKey_Str, encryptionMode);
}

function find_mc_client(channelId) {
  if (typeof clients[channelId] === "undefined") {
    return null;
  } else {
    var c = clients[channelId];
    return c;
  }
}

function leaveChannel2_WGL() {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  } else {
    var c = clients[selectedCurrentChannel];
    _logger("Leaving channel: " + selectedCurrentChannel);
    c.leave();
  }
}

function wgl_mc_joinChannel2(
  token,
  uid,
  autoSubscribeAudio, autoSubscribeVideo,
  autoPublishAudio, autoPublishVideo
) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  } else {
    var c = clients[selectedCurrentChannel];
    c.setAVControl(autoSubscribeAudio, autoSubscribeVideo, autoPublishAudio, autoPublishVideo);
    c.joinChannel2(selectedCurrentChannel, token, uid);
  }
}
function wgl_mc_releaseChannel(channel_str) {
  if (typeof clients[channel_str] === "undefined" || clients[channel_str] == null) {
    return 0;
  } else {
    clients[channel_str] = null;
  }
}

function enableAudioVolumeIndicator2() {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  } else {
    var c = clients[selectedCurrentChannel]; 
    c.enableAudioVolumeIndicator2();
  }
}

function getRemoteVideoStatsMC() {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  } else {
    var c = clients[selectedCurrentChannel];
    c.getRemoteVideoStatsMC();
  }
}

function initVirtualBackground_MC(){
  console.log("multichannel working");
  var c = clients[selectedCurrentChannel];
  c.enableVirtualBackground();
}

function setVirtualBackgroundBlur_MC(blurDegree){
  var c = clients[selectedCurrentChannel];
  c.setVirtualBackgroundBlur(blurDegree);
}

function setVirtualBackgroundColor_MC(hexColor){
  var c = clients[selectedCurrentChannel];
  c.setVirtualBackgroundColor(hexColor);
}

function setVirtualBackgroundImage_MC(imgFile){
  var c = clients[selectedCurrentChannel];
  c.setVirtualBackgroundImage(imgFile);
}

function setVirtualBackgroundVideo_MC(videoFile){
  var c = clients[selectedCurrentChannel];
  c.setVirtualBackgroundVideo(videoFile);
}

function enableSpatialAudio_MC(enabled){
  var c = clients[selectedCurrentChannel];
  c.enableSpatialAudio(enabled);
}

function setRemoteUserSpatialAudioParams2(uid, speaker_azimuth, speaker_elevation, speaker_distance, speaker_orientation, enable_blur, enable_air_absorb){
  var c = clients[selectedCurrentChannel];
  c.setRemoteUserSpatialAudioParams(uid, speaker_azimuth, speaker_elevation, speaker_distance, speaker_orientation, enable_blur, enable_air_absorb);
}
// NEW MULTI CLIENT API's ENDS
