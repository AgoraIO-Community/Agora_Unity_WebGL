var isAudioMute = true;
var isVideoMute = true;
var isRemoteAudioMute = true;
var isRemoteVideoMute = true;

function setMultiChannelWant_MC(multiChannelWant) {
  if (multiChannelWant) {
    multiChannelWant_MC = true;
  } else {
    multiChannelWant_MC = false;
  }
}

var mc2 = false;
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
    if (mc2 == false) {
      c.createClient();
    } else {
      c.createClient_Live();
    }
  } else {
    throw "Cannot create Channel as Multi Channel want is set to False"; // throw a text
  }
}

function joinChannelWithUserAccount_MC(
  token_str,
  userAccount_str,
  autoSubscribeAudio,
  autoSubscribeVideo
) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  } else {
    var c = clients[selectedCurrentChannel];
    c.joinChannelWithUserAccount_MC(
      token_str,
      userAccount_str,
      autoSubscribeAudio,
      autoSubscribeVideo
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

function setClientRole2_MC(role) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  } else {
    var c = clients[selectedCurrentChannel];
    c.setClientRole2_MC(role);
  }
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
  if (isAudioMute) {
    isAudioMute = false;
    var c = clients[selectedCurrentChannel];
    c.muteAllRemoteAudioStreams(true);
  } else {
    isAudioMute = true;
    var c = clients[selectedCurrentChannel];
    c.muteAllRemoteAudioStreams(false);
  }
}

function muteAllRemoteVideoStreams2_mc_WGL(mute) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  }
  if (isVideoMute) {
    isVideoMute = false;
    var c = clients[selectedCurrentChannel];
    c.muteAllRemoteVideoStreams(true);
  } else {
    isVideoMute = true;
    var c = clients[selectedCurrentChannel];
    c.muteAllRemoteVideoStreams(false);
  }
}

function muteRemoteAudioStream2_mc_WGL(userId, mute) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  }
  if (isRemoteAudioMute) {
    isRemoteAudioMute = false;
    var c = clients[selectedCurrentChannel];
    c.muteRemoteAudioStream(userId, true);
  } else {
    isRemoteAudioMute = true;
    var c = clients[selectedCurrentChannel];
    c.muteRemoteAudioStream(userId, false);
  }
}

function muteRemoteVideoStream2_mc_WGL(userId, mute) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  }
  if (isRemoteVideoMute) {
    isRemoteVideoMute = false;
    var c = clients[selectedCurrentChannel];
    c.muteRemoteVideoStream(userId, true);
  } else {
    isRemoteVideoMute = true;
    var c = clients[selectedCurrentChannel];
    c.muteRemoteVideoStream(userId, false);
  }
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
  channel,
  token,
  info,
  uid,
  autoSubscribeAudio,
  autoSubscribeVideo
) {
  if (typeof clients[selectedCurrentChannel] === "undefined") {
    return 0;
  } else {
    var c = clients[selectedCurrentChannel];
    c.setOptions(token, selectedCurrentChannel, uid);
    c.joinChannel();
  }
}
// NEW MULTI CLIENT API's ENDS
