var LibraryAgoraWebGLSDK = {
  $remoteVideoInstances: [],
  $localVideo: null,

  createEngine: function (appID) {
    var app_id = Pointer_stringify(appID);
    return createIRtcEngine(app_id);
  },
  createLocalTexture: function () {
    if(localTracks.videoTrack) {
      localVideo = document.getElementById("video_" + localTracks.videoTrack._ID);
    }
  },
  setVideoDeviceCollectionDeviceWGL: function (deviceID) {
    var deviceID_Str = Pointer_stringify(deviceID);
    setVideoDeviceCollectionDeviceWGL(deviceID_Str);
  },
  setAudioRecordingCollectionDeviceWGL: function (deviceID) {
    var deviceID_Str = Pointer_stringify(deviceID);
    setAudioRecordingCollectionDeviceWGL(deviceID_Str);
  },
  setPlaybackCollectionDeviceWGL: function (deviceID) {
    var deviceID_Str = Pointer_stringify(deviceID);
    setPlaybackCollectionDeviceWGL(deviceID_Str);
  },
  isLocalVideoReady: function () {
    var lVid = undefined; // set null initially
    if (localTracks != undefined) {
      if (localTracks.videoTrack != undefined) {
        if (localTracks.videoTrack._player != undefined) {
          lVid = localTracks.videoTrack._player.videoElement;
          return true;
        }
      }
    }
    return false;
  },
  updateLocalTexture: function (tex) {
    var lVid = undefined; // set null initially
    if (localTracks != undefined) {
      if (localTracks.videoTrack != undefined) {
        if (localTracks.videoTrack._player != undefined) {
          lVid = localTracks.videoTrack._player.videoElement;
        }
      }
    }
    if (lVid == undefined) {
      return;
    }

    localVideo = lVid;

    if (localVideo == null) {
      return false;
    }

    var v = localVideo;

    if (!(v.videoWidth > 0 && v.videoHeight > 0)) {
      return false;
    }

    GLctx.deleteTexture(GL.textures[tex]);
    var t = GLctx.createTexture();
    t.name = tex;
    GL.textures[tex] = t;
    // target, texture
    GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[tex]);

    GLctx.texParameteri(
      GLctx.TEXTURE_2D,
      GLctx.TEXTURE_WRAP_S,
      GLctx.CLAMP_TO_EDGE
    );
    GLctx.texParameteri(
      GLctx.TEXTURE_2D,
      GLctx.TEXTURE_WRAP_T,
      GLctx.CLAMP_TO_EDGE
    );
    GLctx.texParameteri(
      GLctx.TEXTURE_2D,
      GLctx.TEXTURE_MIN_FILTER,
      GLctx.LINEAR
    );
    GLctx.texImage2D(
      GLctx.TEXTURE_2D,
      0,
      GLctx.RGBA,
      GLctx.RGBA,
      GLctx.UNSIGNED_BYTE,
      v
    );

    return true;
  },
  
  createRemoteTexture: function (userId) {
    var ch_userId = Pointer_stringify(userId);

    // approximate 1~2 frames time delay to avoid race condition
    setTimeout(function(){
        var remoteUser = remoteUsers[ch_userId];
        if (remoteUser && remoteUser.videoTrack) {
          video = document.getElementById(
            "video_" + remoteUser.videoTrack._ID
          );
          remoteVideoInstances[ch_userId] = video;
        }
    }, 200);
    return 1;
  }, 

  updateRemoteTexture: function (userId, tex) {
    var ch_userId = Pointer_stringify(userId);

    var lVid = undefined; // set null initially

    if (remoteUsers[ch_userId] != undefined) {
      if (remoteUsers[ch_userId].videoTrack != undefined) {
        if (remoteUsers[ch_userId].videoTrack._player != undefined) {
          lVid = remoteUsers[ch_userId].videoTrack._player.videoElement;
        }
      }
    }

    if (lVid == undefined) {
      return;
    }

    var v = lVid;

    if (!(v.videoWidth > 0 && v.videoHeight > 0)) return false;

    //if (v.lastUpdateTextureTime === v.currentTime) return false;

    //v.lastUpdateTextureTime = v.currentTime;

    if (
      1
      //v.previousUploadedWidth != v.videoWidth ||
      //v.previousUploadedHeight != v.videoHeight
    ) {
      GLctx.deleteTexture(GL.textures[tex]);
      var t = GLctx.createTexture();
      t.name = tex;
      GL.textures[tex] = t;
      GLctx.bindTexture(GLctx.TEXTURE_2D, t);
      GLctx.texParameteri(
        GLctx.TEXTURE_2D,
        GLctx.TEXTURE_WRAP_S,
        GLctx.CLAMP_TO_EDGE
      );
      GLctx.texParameteri(
        GLctx.TEXTURE_2D,
        GLctx.TEXTURE_WRAP_T,
        GLctx.CLAMP_TO_EDGE
      );
      GLctx.texParameteri(
        GLctx.TEXTURE_2D,
        GLctx.TEXTURE_MIN_FILTER,
        GLctx.LINEAR
      );
      GLctx.texImage2D(
        GLctx.TEXTURE_2D,
        0,
        GLctx.RGBA,
        GLctx.RGBA,
        GLctx.UNSIGNED_BYTE,
        v
      );

      v.previousUploadedWidth = v.videoWidth;
      v.previousUploadedHeight = v.videoHeight;
//    } else {
//      console.log("bindTexture////////");
//      GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[tex]);
//      GLctx.texImage2D(
//        GLctx.TEXTURE_2D,
//        0,
//        GLctx.RGBA,
//        GLctx.RGBA,
//        GLctx.UNSIGNED_BYTE,
//       v
//      );
    }

    return true;
  },

  updateRemoteTexture_MC: function (channel, userId, tex) {
    var ch_userId = Pointer_stringify(userId);
    var channelId_str = Pointer_stringify(channel);

    var clientmc = find_mc_client(channelId_str);

    var lVid = undefined; // set null initially
    if (clientmc != null) {
      if (clientmc.remoteUsers[ch_userId] != undefined) {
        if (clientmc.remoteUsers[ch_userId].videoTrack != undefined) {
          if (clientmc.remoteUsers[ch_userId].videoTrack._player != undefined) {
            lVid =
              clientmc.remoteUsers[ch_userId].videoTrack._player.videoElement;
          }
        }
      }
    }

    if (lVid == undefined) {
      return;
    }

    var v = lVid;

    if (!(v.videoWidth > 0 && v.videoHeight > 0)) return false;

    // if (v.lastUpdateTextureTime === v.currentTime) return false;

    // v.lastUpdateTextureTime = v.currentTime;

    if ( 1 
      // v.previousUploadedWidth != v.videoWidth ||
      // v.previousUploadedHeight != v.videoHeight
    ) {
      GLctx.deleteTexture(GL.textures[tex]);
      var t = GLctx.createTexture();
      t.name = tex;
      GL.textures[tex] = t;
      GLctx.bindTexture(GLctx.TEXTURE_2D, t);
      GLctx.texParameteri(
        GLctx.TEXTURE_2D,
        GLctx.TEXTURE_WRAP_S,
        GLctx.CLAMP_TO_EDGE
      );
      GLctx.texParameteri(
        GLctx.TEXTURE_2D,
        GLctx.TEXTURE_WRAP_T,
        GLctx.CLAMP_TO_EDGE
      );
      GLctx.texParameteri(
        GLctx.TEXTURE_2D,
        GLctx.TEXTURE_MIN_FILTER,
        GLctx.LINEAR
      );
      GLctx.texImage2D(
        GLctx.TEXTURE_2D,
        0,
        GLctx.RGBA,
        GLctx.RGBA,
        GLctx.UNSIGNED_BYTE,
        v
      );
      v.previousUploadedWidth = v.videoWidth;
      v.previousUploadedHeight = v.videoHeight;
    } else {
      GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[tex]);
      GLctx.texImage2D(
        GLctx.TEXTURE_2D,
        0,
        GLctx.RGBA,
        GLctx.RGBA,
        GLctx.UNSIGNED_BYTE,
        v
      );
    }

    return true;
  },

  isRemoteVideoReady_MC: function (channelId, userId) {
    var ch_userId = Pointer_stringify(userId);
    var channelId_str = Pointer_stringify(channelId);

    var clientmc = find_mc_client(channelId_str);
    if (clientmc != null) {
      var lVid = undefined; // set null initially
      if (clientmc.remoteUsers[ch_userId] != undefined) {
        if (clientmc.remoteUsers[ch_userId].videoTrack != undefined) {
          if (clientmc.remoteUsers[ch_userId].videoTrack._player != undefined) {
            lVid =
              clientmc.remoteUsers[ch_userId].videoTrack._player.videoElement;
          }
        }
      }
      if (lVid == undefined) {
        return false;
      }
      return true;
    } else {
      return false;
    }
  },

  isRemoteVideoReady: function (userId) {
    var ch_userId = Pointer_stringify(userId);
    var lVid = undefined; // set null initially
    if (remoteUsers[ch_userId] != undefined) {
      if (remoteUsers[ch_userId].videoTrack != undefined) {
        if (remoteUsers[ch_userId].videoTrack._player != undefined) {
          lVid = remoteUsers[ch_userId].videoTrack._player.videoElement;
        }
      }
    }
    if (lVid == undefined) {
      return false;
    }
    return true;
  },

  leaveChannel: function () {
    wglw_leaveChannel();
  },

  enableVideo: function () {
    // for webgl do nothing
    client_manager.setVideoEnabled(true);
  },

  enableVideoObserver: function () {
    // for webgl do nothing
  },

  disableVideoObserver: function () {
    // for webgl do nothing
  },

  renewToken: function (token) {
    var ch_token = Pointer_stringify(token);
    renewToken(ch_token);
  },

  setCameraCapturerConfiguration: function (preference, cameraDirection) {
    console.log("setCameraCapturerConfiguration: preference = " + preference);
    console.log(
      "setCameraCapturerConfiguration: cameraDirection = " + cameraDirection
    );
    setCameraCapturerConfiguration(preference, cameraDirection);
  },

  setEncryptionMode: function (mode) {
    var ch_mode = Pointer_stringify(mode);
    setEncryptionMode(ch_mode);
  },

  setEncryptionSecret: function (secret) {
    var ch_secret = Pointer_stringify(secret);
    setEncryptionSecret(ch_secret);
  },

  setClientRole: function (role) {
    setClientRole(role);
  },
  setClientRole_1: function (role, audienceLatencyLevel) {
    setClientRole1(role, audienceLatencyLevel);
  },
  setClientRole2: function (channel, role) {
    setClientRole2_MC(role);
  },
  setClientRole_2: function (channel, role, audienceLatencyLevel) {
    setClientRole2_MC(role, audienceLatencyLevel);
  },
  enableAudioVolumeIndication: function (interval, smooth, report_vad) {
    enableAudioVolumeIndicator();
  },
  enableAudioVolumeIndication2: function () {
    enableAudioVolumeIndicator2();
  },

  setBeautyEffectOptions: function (
    enabled,
    lighteningContrastLevel,
    lighteningLevel,
    smoothnessLevel,
    rednessLevel
  ) {
    if (enabled == 1) {
      setBeautyEffectOn(lighteningLevel, rednessLevel, smoothnessLevel);
      return 0;
    } else if (enabled == 0) {
      setBeautyEffectOff();
      return 0;
    }
    return 1;
  },
  // switches to another camera
  switchCamera: function () {
    switchCamera();
  },

  joinChannel: function (channelKey, channelName, info, uid) {
    var channel_key = Pointer_stringify(channelKey);
    var channel_name = Pointer_stringify(channelName);
    var channel_info = Pointer_stringify(info);
    wglw_joinChannel(channel_key, channel_name, channel_info, uid);
  },

  getConnectionState: function () {
    var con_state = wrapper.getConnectionState();

    var conIndex = 1;
    if (con_state == "DISCONNECTED") {
      conIndex = 1;
    } else if (con_state == "CONNECTING") {
      conIndex = 2;
    } else if (con_state == "RECONNECTING") {
      conIndex = 4;
    } else if (con_state == "CONNECTED") {
      conIndex = 3;
    }
    return conIndex;
  },

  startScreenCaptureByDisplayId: function (
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
    startScreenCaptureByDisplayId(
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
  },

  startScreenCaptureForWeb: function(enableAudio) {
      startScreenCaptureForWeb(enableAudio);
  },

  startScreenCaptureForWeb2: function(enableAudio) {
      startScreenCaptureForWeb2(enableAudio);
  },

  startNewScreenCaptureForWeb: function(uid, enableAudio) {
     startNewScreenCaptureForWeb(uid, enableAudio);
  },

  stopNewScreenCaptureForWeb: function() {
    console.log("SDK stopNewScreenCaptureForWeb");
      stopNewScreenCaptureForWeb();
  },

  startNewScreenCaptureForWeb2: function(uid, audioEnabled) {
    console.log("SDK startNewScreenCaptureForWeb2");
     startNewScreenCaptureForWeb2(uid, audioEnabled);
  },

  stopNewScreenCaptureForWeb2: function() {
      console.log("SDK stopNewScreenCaptureForWeb2");
      stopNewScreenCaptureForWeb2();
  },

  stopScreenCapture2 : function() {
    stopScreenCapture2();
  },

  startPreview: function () {
    startPreview();
  },

  stopPreview: function () {
    stopPreview();
  },

  // Enables/Disables the local video capture.
  enableLocalVideo: function (enabled) {
    enableLocalVideo(enabled);
  },

  enableLocalAudio: function (enabled) {
    enableLocalAudio(enabled);
  },

  stopScreenCapture: function () {
    stopScreenCapture();
  },

  //Enables the audio module.
  enableAudio: function () {
    enableAudio(true);
  },

  disableAudio: function () {
    enableAudio(false);
  },

  muteLocalAudioStream: function (muteStream) {
    muteLocalAudioStream(muteStream);
  },

  muteLocalVideoStream: function (enable) {
    muteLocalVideoTrack(enable);
  },

  setRemoteVideoStreamType: function (userid, streamType) {
    SetRemoteVideoSTreamType(userid, streamType);
  },

  setLogFilter: function (filter) {
    AgoraRTC.setLogLevel(filter);
  },

  setAudioProfile: function (audioProfile, scenario) {
    SetAudioProfile(audioProfile);
  },

  setVideoEncoderConfiguration: function (
    width,
    height,
    frameRate,
    minFrameRate,
    bitrate,
    minBitrate,
    orientationMode,
    degradationPreference,
    videoMirrorMode
  ) {
    SetVideoEncoderConfiguration(
      width,
      height,
      frameRate,
      minFrameRate,
      bitrate,
      minBitrate,
      orientationMode,
      degradationPreference,
      videoMirrorMode
    );
  },

  enableLastmileTest: function () {
    enableLastMile(true);
  },

  disableLastmileTest: function () {
    enableLastMile(false);
  },

  startAudioMixing: function (filePath, loopBack, replace, cycle) {
    var strFilePath = Pointer_stringify(filePath);
    StartAudioMixing(strFilePath, loopBack, replace, cycle);
  },

  stopAudioMixing: function () {
    StopAudioMixing();
  },

  muteAllRemoteVideoStreams: function (mute) {
    muteAllRemoteVideoStreams(mute);
  },

  pauseAudioMixing: function () {
    PauseAudioMixing();
  },

  resumeAudioMixing: function () {
    ResumeAudioMixing();
  },

  setLiveTranscoding: function (
    width,
    height,
    videoBitrate,
    videoFramerate,
    lowLatency,
    videoGroup,
    video_codec_profile,
    backgroundColor,
    userCount,
    transcodingUserInfo,
    transcodingExtraInfo,
    metaData,
    watermarkRtcImageUrl,
    watermarkRtcImageX,
    watermarkRtcImageY,
    watermarkRtcImageWidth,
    watermarkRtcImageHeight,
    backgroundImageRtcImageUrl,
    backgroundImageRtcImageX,
    backgroundImageRtcImageY,
    backgroundImageRtcImageWidth,
    backgroundImageRtcImageHeight,
    audioSampleRate,
    audioBitrate,
    audioChannels,
    audioCodecProfile,
    advancedFeatures,
    advancedFeatureCount
  ) {
    var strTranscodingUserInfo = Pointer_stringify(transcodingUserInfo);
    var strTranscodingExtraInfo = Pointer_stringify(transcodingExtraInfo);
    var strMetaData = Pointer_stringify(metaData);
    var strWatermarkRtcImageUrl = Pointer_stringify(watermarkRtcImageUrl);
    var strBackgroundImageRtcImageUrl = Pointer_stringify(
      backgroundImageRtcImageUrl
    );
    var strAdvancedFeatures = Pointer_stringify(advancedFeatures);

    SetLiveTranscoding(
      width,
      height,
      videoBitrate,
      videoFramerate,
      lowLatency,
      videoGroup,
      video_codec_profile,
      backgroundColor,
      userCount,
      strTranscodingUserInfo,
      strTranscodingExtraInfo,
      strMetaData,
      strWatermarkRtcImageUrl,
      watermarkRtcImageX,
      watermarkRtcImageY,
      watermarkRtcImageWidth,
      watermarkRtcImageHeight,
      strBackgroundImageRtcImageUrl,
      backgroundImageRtcImageX,
      backgroundImageRtcImageY,
      backgroundImageRtcImageWidth,
      backgroundImageRtcImageHeight,
      audioSampleRate,
      audioBitrate,
      audioChannels,
      audioCodecProfile,
      strAdvancedFeatures,
      advancedFeatureCount
    );
  },

  addPublishStreamUrl: function (url, transcodingEnabled) {
    var strUrl = Pointer_stringify(url);
    StartLiveTranscoding(strUrl, transcodingEnabled);
  },

  removePublishStreamUrl: function (url) {
    var strUrl = Pointer_stringify(url);
    sStopLiveTranscoding(strUrl);
  },

  setChannelProfile: function (profile) {
    setChannelProfile(profile);
  },

  removeRemoteVideo: function (video_id) {},

  resetVideoTextureData: function () {},

  deleteEngine: function () {
    return false;
  },
  playEffect: function (
    soundId,
    filePath,
    loopCount,
    pitch,
    pan,
    gain,
    publish
  ) {
    var strFilePath = Pointer_stringify(filePath);
    PlayEffect(soundId, strFilePath, loopCount, pitch, pan, gain, publish);
  },
  stopEffect: function (soundId) {
    StopEffect(soundId);
  },
  stopAllEffects: function () {
    StopAllEffects();
  },
  preloadEffect: function (soundId, filePath) {
    var strFilePath = Pointer_stringify(filePath);
    PreloadEffect(soundId, strFilePath);
  },

  unloadEffect: function (soundId) {
    UnloadEffect(soundId);
  },

  getEffectsVolume: function () {
    return GetEffectsVolume();
  },

  pauseAllEffects: function () {
    PauseAllEffects();
  },

  pauseEffect: function (soundId) {
    PauseEffect(soundId);
  },

  resumeAllEffects: function () {
    ResumeAllEffects();
  },

  resumeEffect: function (soundId) {
    ResumeEffect(soundId);
  },

  setEffectsVolume: function (volume) {
    SetEffectsVolume(volume);
  },

  setVolumeOfEffect: function (soundId, volume) {
    SetVolumeOfEffect(soundId, volume);
  },

  adjustAudioMixingVolume: function (level) {
    AdjustAudioMixingVolume(level);
  },

  setAudioMixingPosition: function (position) {
    SetAudioMixingPosition(position);
  },

  muteRemoteVideoStream: function (uid, mute) {},

  muteRemoteVideoStream_WGLM: function (uid, mute) {
    var uid_Str = Pointer_stringify(uid);
    MuteRemoteVideoStream(uid_Str, mute);
  },

  muteRemoteAudioStream: function (uid, mute) {},
  muteRemoteAudioStream_WGLM: function (uid, mute) {
    var uid_Str = Pointer_stringify(uid);
    MuteRemoteAudioStream(uid_Str, mute);
  },
  getSdkVersion: function () {
    var bufferSize = lengthBytesUTF8(SDK_VERSION) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(SDK_VERSION, buffer, bufferSize);
    return buffer;
  },

  addTrack: function (track) {},

  muteAllRemoteAudioStreams: function (mute) {
    muteAllRemoteAudioStreams(mute);
  },

  disableVideo: function () {
    client_manager.setVideoEnabled(false);
  },

  getAudioMixingCurrentPosition: function () {
    var position = GetAudioMixingCurrentPosition();
    return position;
  },
  getAudioMixingDuration: function () {
    var duration = GetAudioMixingDuration();
    return duration;
  },

  pushVideoFrameWGL: function (
    videoBuffer,
    size,
    stride,
    height,
    rotation,
    cropLeft,
    cropTop,
    cropRight,
    cropBottom
  ) {
    var buf = new ArrayBuffer(Math.trunc(stride) * Math.trunc(height) * 4);
    var arr = new Uint8Array(buf);
    for (var i = 0; i < size; i++) {
      arr[i] = HEAPU8[videoBuffer + i];
    }
    pushVideoFrame_WGL(
      arr,
      buf,
      size,
      stride,
      height,
      rotation,
      cropLeft,
      cropTop,
      cropRight,
      cropBottom
    );
  },

  pushVideoFrame: function (
    type,
    format,
    videoBuffer,
    stride,
    height,
    cropLeft,
    cropTop,
    cropRight,
    cropBottom,
    rotation,
    timestamp
  ) {},
  setLocalVoiceChanger: function (voiceChanger) {},
  removeInjectStreamUrl2: function (channel, url) {},
  removeInjectStreamUrl: function (url) {},
  setupLocalVideo: function (hwnd, renderMode, uid, priv) {},
  startAudioRecording: function (filePath, quality) {
    var filePath_Str = Pointer_stringify(filePath);
    startAudioRecording_WGL(filePath_Str, quality);
  },
  setSpeakerphoneVolume: function (volume) {},
  setEncryptionSecret2: function (channel, secret) {},
  configPublisher: function (
    width,
    height,
    framerate,
    bitrate,
    defaultLayout,
    lifecycle,
    owner,
    injectStreamWidth,
    injectStreamHeight,
    injectStreamUrl,
    publishUrl,
    rawStreamUrl,
    extraInfo
  ) {},
  releaseAAudioRecordingDeviceManager: function () {},
  setAudioSessionOperationRestriction: function (restriction) {},

  getCurrentRecordingDeviceInfo: function (deviceName, deviceId) {},
  setDefaultAudioRoutetoSpeakerphone: function (enabled) {},
  setRecordingAudioFrameParameters: function (
    sampleRate,
    channel,
    mode,
    samplesPerCall
  ) {},
  rate: function (callId, rating, desc) {},
  initEventOnMixedAudioFrame: function (onMixedAudioFrame) {},
  setDefaultMuteAllRemoteVideoStreams: function (channel, mute) {},
  setDefaultMuteAllRemoteVideoStreams2: function (channel, mute) {},
  setVoiceBeautifierPreset: function (preset) {},
  setExternalAudioSource: function (enabled, sampleRate, channels) {
    setExternalAudioSource_WGL(enabled, sampleRate, channels);
  },
  setMixedAudioFrameParameters: function (sampleRate, samplesPerCall) {},

  registerMediaMetadataObserver: function (metaDataType) {},
  setVideoProfile: function (profile, swapWidthAndHeight) {},
  setHighQualityAudioParametersWithFullband: function (
    fullband,
    stereo,
    fullBitrate
  ) {},
  getUserInfoByUid: function (uid) {},
  getUserInfoByUid_WGL: function (uid) {
    var uid_Str = Pointer_stringify(uid);
    var uinfo = getUserInfoByUid_WGL(uid_Str);
    uinfo = uinfo.toString();
    var bufferSize = lengthBytesUTF8(uinfo) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(uinfo, buffer, bufferSize);
    return buffer;
  },
  removePublishStreamUrl2: function (channel, url) {},
  getVideoDeviceCollectionDevice: function (index, deviceName, deviceId) {},
  registerPacketObserver: function () {},
  updateTexture: function () {},
  unRegisterVideoRawDataObserver: function () {},
  initEventOnPullAudioFrame: function (onPullAudioFrame) {},

  setRemoteVoicePosition: function (uid, pan, gain) {},
  isAudioRecordingDeviceMute: function () {
    return localTracks.audioTrack._mediaStreamTrack.enabled;
  },
  setLocalVoiceReverbPreset: function (audioReverbPreset) {},
  enableRemoteSuperResolution: function (userId, enable) {},
  setMirrorApplied: function (wheatherApply) {
    setMirrorApplied_WGL(wheatherApply);
  },
  setLocalRenderMode: function (renderMode) {},
  complain: function (callId, desc) {},
  freeObject: function (obj) {},
  createAVideoDeviceManager: function () {},
  createMediaRecorder: function () {
    SendNotImplementedError()
  },
  enableContentInspect: function (enabled, extraInfo, modulesInfo, modulesCount) {
    SendNotImplementedError()
  },
  enableLocalVoicePitchCallback: function (interval) {
    SendNotImplementedError()
  },
  enableSpatialAudio: function (enabled) {
    enableSpatialAudio(enabled);
  },
  enableSpatialAudio_MC: function (enabled) {
    enableSpatialAudio_MC(enabled);
  },
  enableWirelessAccelerate: function (enabled) {
    SendNotImplementedError()
  },
  followSystemPlaybackDevice: function (enabled) {
    SendNotImplementedError()
  },
  followSystemRecordingDevice: function (enabled) {
    SendNotImplementedError()
  },
  getAudioFileInfo: function (filePath) {
    SendNotImplementedError()
  },
  getAudioPlaybackDefaultDevice: function (deviceName, deviceId) {
    SendNotImplementedError()
  },
  getAudioRecordingDefaultDevice: function (deviceName, deviceId) {
    SendNotImplementedError()
  },
  getAudioTrackCount: function () {
    SendNotImplementedError()
  },
  getCameraMaxZoomFactor: function () {
    SendNotImplementedError()
  },
  getScreenCaptureIconImage: function () {
    SendNotImplementedError()
  },
  getScreenCaptureIsPrimaryMonitor: function (index) {
    SendNotImplementedError()
  },
  getScreenCaptureSourceId : function (index) {
    SendNotImplementedError()
  },
  getScreenCaptureSourceName : function (index) {
    SendNotImplementedError()
  },
  getScreenCaptureSourceProcessPath : function (index) {
    SendNotImplementedError()
  },
  getScreenCaptureSourceTitle : function (index) {
    SendNotImplementedError()
  },
  getScreenCaptureSourceType : function (index) {
    SendNotImplementedError()
  },
  getScreenCaptureSources : function (thumbHeight, thumbWidth, iconHeight, iconWidth, includeScreen) {
    SendNotImplementedError()
  },
  getScreenCaptureSourcesCount: function () {
    SendNotImplementedError()
  },
  getScreenCaptureThumbImage: function (index, buffer) {
    SendNotImplementedError()
  },
  isCameraAutoFocusFaceModeSupported: function () {
    SendNotImplementedError()
  },
  isCameraExposurePositionSupported: function () {
    SendNotImplementedError()
  },
  isCameraFocusSupported: function () {
    SendNotImplementedError()
  },
  isCameraZoomSupported: function () {
    SendNotImplementedError()
  },
  pauseAllChannelMediaRelay: function () {
    SendNotImplementedError()
  },
  pauseAllChannelMediaRelay: function () {
    SendNotImplementedError()
  },
  pushAudioFrame3_: function (sourcePos, audioFrameType, samples, bytesPerSample, channels, samplesPerSec, buffer, renderTimeMs, avsync_type) {
    SendNotImplementedError()
  },
  pushVideoFrame2: function (type, format, bufferPtr, stride, height, cropLeft, cropTop, cropRight, cropBottom, rotation, timestamp) {
    SendNotImplementedError()
  },
  releaseMediaRecorder: function () {
    SendNotImplementedError()
  },
  resumeAllChannelMediaRelay: function () {
    SendNotImplementedError()
  },
  initEventOnMediaRecorderCallback: function (onRecorderStateChanged, onRecorderInfoUpdated) {
    SendNotImplementedError()
  },
  selectAudioTrack : function (index) {
    SendNotImplementedError()
  },
  setAVSyncSource : function (channelId, uid) {
    SendNotImplementedError()
  },
  setAVSyncSource2 : function (channel, channelId, uid) {
    SendNotImplementedError()
  },
  setAudioMixingDualMonoMode : function (mode) {
    SendNotImplementedError()
  },
  setAudioMixingDualMonoMode : function (speed) {
    SendNotImplementedError()
  },
  setAudioMixingDualMonoMode : function (speed) {
    SendNotImplementedError()
  },
  setCameraAutoFocusFaceModeEnabled : function (enabled) {
    SendNotImplementedError()
  },
  setCameraExposurePosition : function (positionXinView, positionYinView) {
    SendNotImplementedError()
  },
  setCameraExposurePosition : function (positionXinView, positionYinView) {
    SendNotImplementedError()
  },
  setCameraFocusPositionInPreview : function (positionX, positionY) {
    SendNotImplementedError()
  },
  setAudioMixingPlaybackSpeed : function (speed) {
    SendNotImplementedError()
  },
  setAudioMixingPlaybackSpeed : function (speed) {
    SendNotImplementedError()
  },
  setCameraZoomFactor : function (factor) {
    SendNotImplementedError()
  },
  setColorEnhanceOptions : function (enabled, strengthLevel, skinProtectLevel) {
    SendNotImplementedError()
  },
  setExternalAudioSourceVolume : function (sourcePos, volume) {
    SendNotImplementedError()
  },
  setLowlightEnhanceOptions : function (enabled, mode, level) {
    SendNotImplementedError()
  },
  setLowlightEnhanceOptions : function (enabled, mode, level) {
    SendNotImplementedError()
  },
  setRemoteUserSpatialAudioParams : function (uid, speaker_azimuth, speaker_elevation, speaker_distance, speaker_orientation, speaker_attenuation, enable_blur, enable_air_absorb) {
    uid_Str = Pointer_stringify(uid);
    newUID = parseInt(uid_Str);
    blur = enable_blur == 0 ? false : true;
    airAbsorb = enable_air_absorb == 0 ? false : true;
    setRemoteUserSpatialAudioParams(newUID, speaker_azimuth, speaker_elevation, speaker_distance, speaker_orientation, speaker_attenuation, blur, airAbsorb);
  },
  setRemoteUserSpatialAudioParams2 : function (uid, speaker_azimuth, speaker_elevation, speaker_distance, speaker_orientation, speaker_attenuation, enable_blur, enable_air_absorb) {
    uid_Str = Pointer_stringify(uid);
    newUID = parseInt(uid_Str);
    blur = enable_blur == 0 ? false : true;
    airAbsorb = enable_air_absorb == 0 ? false : true;
    setRemoteUserSpatialAudioParams2(newUID, speaker_azimuth, speaker_elevation, speaker_distance, speaker_orientation, speaker_attenuation, blur, airAbsorb);
  },
  setScreenCaptureScenario : function () {
    SendNotImplementedError()
  },
  setVideoDenoiserOptions : function (enabled, mode, level) {
    SendNotImplementedError()
  },
  startAudioDeviceLoopbackTest : function (indicationInterval) {
    SendNotImplementedError()
  },
  startEchoTest3  : function (view, enableAudio, enableVideo, token, channelId) {
    SendNotImplementedError()
  },
  startRecording  : function () {
    SendNotImplementedError()
  },
  startRtmpStreamWithTranscoding  : function (url, width, height, videoBitrate, videoFramerate, lowLatency, videoGroup, video_codec_profile, backgroundColor, userCount, transcodingUserInfo, transcodingExtraInfo, metaData, watermarkRtcImageUrl, watermarkRtcImageX, watermarkRtcImageY, watermarkRtcImageWidth, watermarkRtcImageHeight, watermarkImageZorder, watermarkImageAlpha, watermarkCount, backgroundImageRtcImageUrl, backgroundImageRtcImageX, backgroundImageRtcImageY, backgroundImageRtcImageWidth, backgroundImageRtcImageHeight, backgroundImageRtcImageZorder, backgroundImageRtcImageAlpha, backgroundImageRtcImageCount, audioSampleRate, audioBitrate, audioChannels, audioCodecProfile, advancedFeatures, advancedFeatureCount) {
    SendNotImplementedError()
  },
  startRtmpStreamWithTranscoding2  : function (channel, url, width, height, videoBitrate, videoFramerate, lowLatency, videoGroup, video_codec_profile, backgroundColor, userCount, transcodingUserInfo, transcodingExtraInfo, metaData, watermarkRtcImageUrl, watermarkRtcImageX, watermarkRtcImageY, watermarkRtcImageWidth, watermarkRtcImageHeight, watermarkImageZorder, watermarkImageAlpha, watermarkCount, backgroundImageRtcImageUrl, backgroundImageRtcImageX, backgroundImageRtcImageY, backgroundImageRtcImageWidth, backgroundImageRtcImageHeight, backgroundImageRtcImageZorder, backgroundImageRtcImageAlpha, backgroundImageRtcImageCount, audioSampleRate, audioBitrate, audioChannels, audioCodecProfile, advancedFeatures, advancedFeatureCount) {
    SendNotImplementedError()
  },
  startRtmpStreamWithoutTranscoding  : function (url) {
    SendNotImplementedError()
  },
  startRtmpStreamWithoutTranscoding2  : function (channel, url) {
    SendNotImplementedError()
  },
  stopAudioDeviceLoopbackTest  : function () {
    SendNotImplementedError()
  },
  stopRecording  : function () {
    SendNotImplementedError()
  },
  stopRtmpStream  : function (url) {
    SendNotImplementedError()
  },
  stopRtmpStream2  : function (channel, url) {
    SendNotImplementedError()
  },
  takeSnapshot  : function (channel, uid, filePath) {
    SendNotImplementedError()
  },
  updateRtmpTranscoding  : function (width, height, videoBitrate, videoFramerate, lowLatency, videoGroup, video_codec_profile, backgroundColor, userCount, transcodingUserInfo, transcodingExtraInfo, metaData, watermarkRtcImageUrl, watermarkRtcImageX, watermarkRtcImageY, watermarkRtcImageWidth, watermarkRtcImageHeight, watermarkImageZorder, watermarkImageAlpha, watermarkCount, backgroundImageRtcImageUrl, backgroundImageRtcImageX, backgroundImageRtcImageY, backgroundImageRtcImageWidth, backgroundImageRtcImageHeight, backgroundImageRtcImageZorder, backgroundImageRtcImageAlpha, backgroundImageRtcImageCount, audioSampleRate, audioBitrate, audioChannels, audioCodecProfile, advancedFeatures, advancedFeatureCount) {
    SendNotImplementedError()
  },
  updateRtmpTranscoding2  : function (channel, width, height, videoBitrate, videoFramerate, lowLatency, videoGroup, video_codec_profile, backgroundColor, userCount, transcodingUserInfo, transcodingExtraInfo, metaData, watermarkRtcImageUrl, watermarkRtcImageX, watermarkRtcImageY, watermarkRtcImageWidth, watermarkRtcImageHeight, watermarkImageZorder, watermarkImageAlpha, watermarkCount, backgroundImageRtcImageUrl, backgroundImageRtcImageX, backgroundImageRtcImageY, backgroundImageRtcImageWidth, backgroundImageRtcImageHeight, backgroundImageRtcImageZorder, backgroundImageRtcImageAlpha, backgroundImageRtcImageCount, audioSampleRate, audioBitrate, audioChannels, audioCodecProfile, advancedFeatures, advancedFeatureCount) {
    SendNotImplementedError()
  },
  adjustAudioMixingPlayoutVolume: function (volume) {
    AdjustAudioMixingPlayoutVolume(volume);
  },
  adjustAudioMixingPublishVolume: function (volume) {
    AdjustAudioMixingPublishVolume(volume);
  },
  
  creatAAudioPlaybackDeviceManager: function () {},
  initEventOnPacketCallback: function (
    onReceiveAudioPacket,
    onReceiveVideoPacket,
    onSendAudioPacket,
    onSendVideoPacket
  ) {},
  stopChannelMediaRelay2: function (channel) {
    stopChannelMediaRelay_MC();
  },
  startAudioRecordingDeviceTest: function (indicationInterval) {},
  initEventOnPlaybackAudioFrameBeforeMixing: function (
    onPlaybackAudioFrameBeforeMixing
  ) {},
  setAudioPlaybackDeviceMute: function (mute) {
    setAudioPlaybackDeviceMute(mute);
  },

  initChannelEventCallback: function (
    channel,
    onWarning,
    onError,
    onJoinChannelSuccess,
    onRejoinChannelSuccess,
    onLeaveChannel,
    onClientRoleChanged,
    onUserJoined,
    onUserOffline,
    onConnectionLost,
    onRequestToken,
    onTokenPrivilegeWillExpire,
    onRtcStats,
    onNetworkQuality,
    onRemoteVideoStats,
    onRemoteAudioStats,
    onRemoteAudioStateChanged,
    onActiveSpeaker,
    onVideoSizeChanged,
    onRemoteVideoStateChanged,
    onStreamMessage,
    onStreamMessageError,
    onMediaRelayStateChanged,
    onMediaRelayEvent,
    onRtmpStreamingStateChanged,
    onTranscodingUpdated,
    onStreamInjectedStatus,
    onRemoteSubscribeFallbackToAudioOnly,
    onConnectionStateChanged,
    onLocalPublishFallbackToAudioOnly,
    onRtmpStreamingEvent,
    onAudioPublishStateChange,
    onVideoPublishStateChange,
    onAudioSubscribeStateChange,
    onVideoSubscribeStateChange,
    onUserSuperResolutionEnabled
  ) {},
  setPlaybackAudioFrameParameters: function (
    sampleRate,
    channel,
    mode,
    samplesPerCall
  ) {},
  setLiveTranscoding2: function (
    channel,
    width,
    height,
    videoBitrate,
    videoFramerate,
    lowLatency,
    videoGroup,
    video_codec_profile,
    backgroundColor,
    userCount,
    transcodingUserInfo,
    transcodingExtraInfo,
    metaData,
    watermarkRtcImageUrl,
    watermarkRtcImageX,
    watermarkRtcImageY,
    watermarkRtcImageWidth,
    watermarkRtcImageHeight,
    backgroundImageRtcImageUrl,
    backgroundImageRtcImageX,
    backgroundImageRtcImageY,
    backgroundImageRtcImageWidth,
    backgroundImageRtcImageHeight,
    audioSampleRate,
    audioBitrate,
    audioChannels,
    audioCodecProfile,
    advancedFeatures,
    advancedFeatureCount
  ) {},
  enableInEarMonitoring: function (enabled) {},
  setLogFile: function (filePath) {},
  registerVideoRawDataObserver: function () {},
  setEncryptionMode2: function (channel, encryptionMode) {},
  initEventOnRenderVideoFrame: function (onRenderVideoFrame) {},
  setInEarMonitoringVolume: function (volume) {},
  pullAudioFrame_: function (
    audioBuffer,
    type,
    samples,
    bytesPerSample,
    channels,
    samplesPerSec,
    renderTimeMs,
    avsync_type
  ) {},
  getCallId: function () {},
  initEventOnMetaDataCallback: function (
    onMetadataReceived,
    onReadyToSendMetadata,
    onGetMaxMetadataSize
  ) {},
  setAudioMixingPitch: function (pitch) {},
  adjustUserPlaybackSignalVolume: function (uid, volume) {},
  adjustUserPlaybackSignalVolume_WGLM: function (uid, volume) {
    var uid_Str = Pointer_stringify(uid);
    adjustUserPlaybackSignalVolume_WGL(uid_Str, volume);
  },
  setAudioPlaybackDevice: function (deviceId) {},
  setLocalPublishFallbackOption: function (option) {
    setLocalPublishFallbackOption_WGL(option);
  },
  stopAudioRecordingDeviceTest: function () {},
  registerAudioRawDataObserver: function () {},
  publish: function (channel) {
    publish_mc_WGL();
  },
  stopVideoDeviceTest: function () {},
  unpublish: function (channel) {
    unpublish_mc_WGL();
  },
  muteAllRemoteAudioStreams2: function (channel, mute) {
    muteAllRemoteAudioStreams2_mc_WGL(mute);
  },
  muteAllRemoteVideoStreams2: function (channel, mute) {
    muteAllRemoteVideoStreams2_mc_WGL(mute);
  },

  muteRemoteAudioStream2: function (channel, userId, mute) {
    muteRemoteAudioStream2_mc_WGL(userId, mute);
  },

  muteRemoteAudioStream2_WGLM: function (channel, userId, mute) {
    var userId_Str = Pointer_stringify(userId);
    muteRemoteAudioStream2_mc_WGL(userId_Str, mute);
  },

  muteRemoteVideoStream2: function (channel, userId, mute) {
    muteRemoteVideoStream2_mc_WGL(userId, mute);
  },

  muteRemoteVideoStream2_WGLM: function (channel, userId, mute) {
    var userId_Str = Pointer_stringify(userId);
    muteRemoteVideoStream2_mc_WGL(userId_Str, mute);
  },
  setRemoteVideoStreamType2: function (channel, userId, streamType) {
    setRemoteVideoStreamType2_mc_WGL(userId, streamType);
  },
  setRemoteVideoStreamType2_WGLM: function (channel, userId, streamType) {
    var userId_Str = Pointer_stringify(userId);
    setRemoteVideoStreamType2_mc_WGL(userId_Str, streamType);
  },

  adjustUserPlaybackSignalVolume2: function (channel, userId, volume) {
    adjustUserPlaybackSignalVolume2_mc_WGL(userId, volume);
  },

  adjustUserPlaybackSignalVolume2_WGLM: function (channel, userId, volume) {
    var userId_Str = Pointer_stringify(userId);
    adjustUserPlaybackSignalVolume2_mc_WGL(userId_Str, volume);
  },

  setRemoteDefaultVideoStreamType2: function (channel, streamType) {
    setRemoteDefaultVideoStreamType2_mc_WGL(streamType);
  },

  setRemoteUserPriority2: function (channel, userId, userPriority) {
    setRemoteUserPriority2_mc_WGL(userId, userPriority);
  },

  setRemoteUserPriority2_WGLM: function (channel, userId, userPriority) {
    var userId_Str = Pointer_stringify(userId);
    setRemoteUserPriority2_mc_WGL(userId_Str, userPriority);
  },

  setEncryptionMode2: function (channel, encryptionMode) {
    setEncryptionMode2_mc_WGL(encryptionMode);
  },
  setEncryptionSecret2: function (channel, secret) {
    var userId_Str = Pointer_stringify(secret);
    setEncryptionSecret2_mc_WGL(userId_Str);
  },

  enableEncryption2: function (
    channel,
    enabled,
    encryptionKey,
    encryptionMode
  ) {
    var encryptionKey_Str = Pointer_stringify(encryptionKey);
    enableEncryption2_mc(enabled, encryptionKey_Str, encryptionMode);
  },

  setDefaultMuteAllRemoteAudioStreams: function (channel, mute) {
    console.log("Deprecated from v3.3.1");
  },
  unRegisterPacketObserver: function () {},
  createChannel: function (channelId) {
    var channelId_Str = Pointer_stringify(channelId);
    wgl_mc_createChannel(channelId_Str);
  },

  setVoiceOnlyMode: function (enable) {
    SetVoiceOnlyMode(enable);
  },
  stopRecordingService: function (recordingKey) {},
  stopAudioRecording: function () {
    stopAudioRecording_WGL();
  },
  setRemoteSubscribeFallbackOption: function (option) {
    setRemoteSubscribeFallbackOption_WGL(option);
  },
  releaseAVideoDeviceManager: function () {},
  addPublishStreamUrl2: function (channel, url, transcodingEnabled) {},
  setRemoteDefaultVideoStreamType: function (remoteVideoStreamType) {
    setRemoteDefaultVideoStreamType(remoteVideoStreamType);
  },
  setLocalVoiceReverb: function (reverbKey, value) {},

  createEngine2: function (appID, areaCode, filePath, fileSize, level) {
    var app_id = Pointer_stringify(appID);
    return createIRtcEngine2(app_id, areaCode);
  },

  setPlaybackDeviceVolume: function (volume) {},
  updateScreenCaptureParameters: function (
    screenCaptureVideoDimenWidth,
    screenCaptureVideoDimenHeight,
    screenCaptureFrameRate,
    screenCaptureBitrate,
    screenCaptureCaptureMouseCursor
  ) {},
  startEchoTest: function () {},
  leaveChannel2: function (channel) {
    leaveChannel2_WGL();
  },

  updateVideoRawData: function (data, channelId, uid) {},
  setAudioEffectPreset: function (preset) {},
  updateVideoRawData2: function (data, channelId, uid) {},
  getUserInfoByUserAccount: function (userAccount) {},
  getErrorDescription: function (code) {},
  removeUserVideoInfo: function (userId) {},
  getAudioRecordingDevice: function (index, deviceName, deviceId) {},
  setEnableSpeakerphone: function (enabled) {},
  startAudioRecording2: function (filePath, sampleRate, quality) {
    var filePath_Str = Pointer_stringify(filePath);
    startAudioRecording_WGL(filePath_Str, quality);
  },
  updateChannelMediaRelay2: function (
    channel,
    srcChannelName,
    srcToken,
    srcUid,
    destChannelName,
    destToken,
    destUid,
    destCount
  ) {},
  updateChannelMediaRelay2_WEBGL: function (
    channel,
    srcChannelName,
    srcToken,
    srcUid,
    destChannelName,
    destToken,
    destUid,
    destCount
  ) {
    var srcChannelNameStr = Pointer_stringify(srcChannelName);
    var srcTokenStr = Pointer_stringify(srcToken);
    var destChannelNameStr = Pointer_stringify(destChannelName);
    var destTokenStr = Pointer_stringify(destToken);

    var srcUid_Str = Pointer_stringify(srcUid);
    var destUid_Str = Pointer_stringify(destUid);

    updateChannelMediaRelay_MC(
      srcChannelNameStr,
      srcTokenStr,
      srcUid_Str,
      destChannelNameStr,
      destTokenStr,
      destUid_Str,
      destCount
    );
  },
  pushAudioFrame_: function (
    audioFrameType,
    samples,
    bytesPerSample,
    channels,
    samplesPerSec,
    buffer,
    renderTimeMs,
    avsync_type
  ) {
    var arsize = samples * (channels * bytesPerSample);
    var buf = new ArrayBuffer(arsize);
    var arr = new Uint8Array(buf);

    for (var i = 0; i < arsize; i++) {
      arr[i] = HEAPU8[buffer + i];
    }
  },
  stopLastmileProbeTest: function () {},
  adjustPlaybackSignalVolume: function (volume) {
    adjustPlaybackSignalVolume_WGL(volume);
  },
  startEchoTest2: function (intervalInSeconds) {},
  addUserVideoInfo2: function (channelId, _userId, _textureId) {},
  getCurrentPlaybackDevice: function (deviceName, deviceId) {},
  getVideoDeviceCollectionCount: function () {},
  setRemoteRenderMode: function (channel, userId, renderMode, mirrorMode) {},
  addVideoWatermark2: function (
    watermarkUrl,
    visibleInPreview,
    positionInLandscapeX,
    positionInLandscapeY,
    positionInLandscapeWidth,
    positionInLandscapeHeight,
    positionInPortraitX,
    positionInPortraitY,
    positionInPortraitWidth,
    positionInPortraitHeight
  ) {
    var url_Str = Pointer_stringify(watermarkUrl);
    startWaterMark_WGL(
      url_Str,
      positionInLandscapeX,
      positionInLandscapeY,
      positionInLandscapeWidth,
      positionInLandscapeHeight
    );
  },
  setLocalVoiceEqualization: function (bandFrequency, bandGain) {},
  stopAudioPlaybackDeviceTest: function () {},
  setAudioPlaybackDeviceVolume: function (volume) {
    setAudioPlaybackDeviceVolume(volume);
  },
  initEventOnPlaybackAudioFrame: function (onPlaybackAudioFrame) {},
  removeUserVideoInfo2: function (channelId, _userId) {},
  setVideoDeviceCollectionDevice: function (deviceId) {},
  registerLocalUserAccount: function (appId, userAccount) {},
  addInjectStreamUrl: function (
    url,
    width,
    height,
    videoGop,
    videoFramerate,
    videoBitrate,
    audioSampleRate,
    audioBitrate,
    audioChannels
  ) {},
  refreshRecordingServiceStatus: function () {},
  unRegisterAudioRawDataObserver: function () {},
  startVideoDeviceTest: function (hwnd) {},
  setScreenCaptureContentHint: function (videoContentHint) {},
  setupRemoteVideo: function (hwnd, renderMode, uid, priv) {},

  setDefaultMuteAllRemoteAudioStreams2: function (channel, mute) {},
  setLocalVideoMirrorMode: function (mirrorMode) {},
  getCurrentRecordingDevice: function (deviceId) {},
  startScreenCaptureByScreenRect: function (
    screenRectX,
    screenRectY,
    screenRectWidth,
    screenRectHeight,
    regionRectX,
    regionRectY,
    regionRectWidth,
    regionRectHeight,
    screenCaptureVideoDimenWidth,
    screenCaptureVideoDimenHeight,
    screenCaptureFrameRate,
    screenCaptureBitrate,
    screenCaptureCaptureMouseCursor
  ) {},
  enableRemoteSuperResolution2: function (channel, userId, enable) {},
  enableEncryption: function (enabled, encryptionKey, encryptionMode) {},
  unRegisterMediaMetadataObserver: function () {},
  setMultiChannelWant: function (multiChannelWant) {
    setMultiChannelWant_MC(multiChannelWant);
  },
  isAudioPlaybackDeviceMute: function () {
    return pd_muted;
  },
  stopEchoTest: function () {},
  creatAAudioRecordingDeviceManager: function () {},
  createDataStream: function (reliable, ordered) { 
    createDataStream(ordered);
    return 0;
  },
  createDataStream_engine: function(syncWithAudio, ordered) {
    createDataStream(ordered);
  },
  initEventOnCaptureVideoFrame: function (onCaptureVideoFrame) {},
  getCurrentVideoDevice: function (deviceId) {},
  setAudioRecordingDeviceMute: function (mute) {
    setAudioRecordingDeviceMute(mute);
  },
  enableSoundPositionIndication: function (enabled) {},
  setAudioEffectParameters: function (preset, param1, param2) {},
  setExternalAudioSink: function (enabled, sampleRate, channels) {},
  setRemoteRenderMode2: function (channel, userId, renderMode, mirrorMode) {},
  setRemoteVoicePosition2: function (channel, uid, pan, gain) {},
  channelId: function (channel) {},
  sendStreamMessage: function (streamId, data, length) {
      // there is no notion of streamId 
      const newArray =new ArrayBuffer(length);
      const newByteArray = new Uint8Array(newArray);
      for(var i = 0; i < length; i++) {
        newByteArray[i]=HEAPU8[data + i];
      }
      return sendStreamMessage(newByteArray);
  },
  sendMetadata: function (uid, size, buffer, timeStampMs) {},
  initEventOnRecordAudioFrame: function (onRecordAudioFrame) {},
  getAudioMixingPublishVolume: function () {},
  enableWebSdkInteroperability: function (enabled) {},
  renewToken2: function (channel, token) {
    var token_str = Pointer_stringify(token);
    renewToken2_mc(token_str);
  },
  joinChannelWithUserAccount: function (token, channelId, userAccount) {
    var token_str = Pointer_stringify(token);
    var channelId_str = Pointer_stringify(channelId);
    var userAccount_str = Pointer_stringify(userAccount);
    joinChannelWithUserAccount_WGL(token_str, channelId_str, userAccount_str);
  },
  joinChannelWithUserAccount_engine: function (token, channelId, userAccount, 
      autoSubscribeAudio, autoSubscribeVideo,
      publishLocalAudio, publishLocalVideo) {
    var token_str = Pointer_stringify(token);
    var channelId_str = Pointer_stringify(channelId);
    var userAccount_str = Pointer_stringify(userAccount); 
    joinChannelWithUserAccount_engine_WGL(token_str,channelId_str, userAccount_str, 
      autoSubscribeAudio, autoSubscribeVideo, publishLocalAudio, publishLocalVideo );
  },

  joinChannelWithMediaOption : function (token, channelId, info, uid, 
      autoSubscribeAudio, autoSubscribeVideo,
      publishLocalAudio, publishLocalVideo) {
    var token_str = Pointer_stringify(token);
    var channelId_str = Pointer_stringify(channelId);
    wglw_joinChannel_withOption(token_str,channelId_str, info, uid, 
      autoSubscribeAudio, autoSubscribeVideo, publishLocalAudio, publishLocalVideo );
  },

  setDefaultEngineSettings: function () {},
  setVideoQualityParameters: function (preferFrameRateOverImageQuality) {},
  getCurrentPlaybackDeviceInfo: function (deviceName, deviceId) {},
  setAudioRecordingDeviceVolume: function (volume) {
    setAudioRecordingDeviceVolume(volume);
  },
  setCurrentChannel_WGL: function (channelId) {
    var channelId_Str = Pointer_stringify(channelId);
    setCurrentChannel_WGL(channelId_Str);
  },

muteLocalVideoStream_channel: function(channel, mute) {
  var str_chan = Pointer_stringify(channel);
  muteLocalVideoStream2_mc_WGL(str_chan, mute);
},
muteLocalAudioStream_channel: function(channel, mute) {
  var str_chan = Pointer_stringify(channel);
  muteLocalAudioStream2_mc_WGL(str_chan, mute);
},
  joinChannelWithUserAccount2: function (
    channel,
    token,
    userAccount,
    autoPublishAudio, autoPublishVideo,
    publishLocalAudio, publishLocalVideo
  ) {
    _logger("joinChannelWithUserAccount2 in jslib");
    var token_str = Pointer_stringify(token);
    var userAccount_str = Pointer_stringify(userAccount);
    wgl_mc_joinChannel2(
      token_str,
      userAccount_str,
      autoPublishAudio, autoPublishVideo,
      publishLocalAudio, publishLocalVideo
    );
  },
  joinChannel2: function (
    channel,
    token,
    info,
    uid,
    autoSubscribeAudio, autoSubscribeVideo,
    publishLocalAudio, publishLocalVideo
  ) {
    var token_Str = Pointer_stringify(token);
    var info_Str = Pointer_stringify(info);
    wgl_mc_joinChannel2(
      token_Str,
      uid,
      autoSubscribeAudio, autoSubscribeVideo,
      publishLocalAudio, publishLocalVideo
    );
  },
  ReleaseChannel: function (channel) {
    var channel_str = Pointer_stringify(channel);
    wgl_mc_releaseChannel(channel_str);
  },
  adjustRecordingSignalVolume: function (volume) {
    adjustRecordingSignalVolume_WGL(volume);
  },
  enableDualStreamMode: function (enabled) {
    enableDualStream_WGL(enabled);
  },
  enableLogUpload: function () {
    enableLogUpload();
    return 1;
  },
  disableLogUpload: function () {
    disableLogUpload();
    return 1;
  },

  getAudioPlaybackDeviceVolume: function () {
    return wrapper.savedSettings.playbackVolume;
  },
  setLogFileSize: function (fileSizeInKBytes) {},
  setExternalVideoSource: function (enable, useTexture) {
    setExternalVideoSource_WGL(enable);
  },
  getAudioRecordingDeviceCount: function () {},
  deleteTexture: function (tex) {},
  getAudioMixingPlayoutVolume: function () {
    var volume = GetAudioMixingPlayoutVolume();
    return volume;
  },
  getAudioMixingPublishVolume: function () {
    var volume = GetAudioMixingPublishVolume();
    return volume;
  },

  getAudioPlaybackDevice: function (index, deviceName, deviceId) {},
  enableLoopbackRecording: function (enabled, deviceName) {},

  addUserVideoInfo: function (userId, textureId) {},
  sendCustomReportMessage: function (id, category, events, label, value) {},
  getMultiChannelWanted: function () {
    return multiChannelWant_MC;
  },
  startLastmileProbeTest: function (
    probeUplink,
    probeDownlink,
    expectedUplinkBitrate,
    expectedDownlinkBitrate
  ) {},
  setRenderMode: function (renderMode) {},
  switchChannel: function (token, channelId) {
    var token_str = Pointer_stringify(token);
    var channelId_str = Pointer_stringify(channelId);
    switchChannel_WGL(token_str, channelId_str);
  },
  setAudioRecordingDevice: function (deviceId) {},
  sendStreamMessage2: function (channel, streamId, data, length) {},
  isSpeakerphoneEnabled: function () {},
  setRemoteUserPriority: function (uid, userPriority) {},
  setRemoteUserPriority_WGL: function (uid, userPriority) {
    var uid_Str = Pointer_stringify(uid);
    SetRemoteUserPriority(uid_Str, userPriority);
  },
  startAudioPlaybackDeviceTest: function (testAudioFilePath) {},

  setParameters: function (options) {},

  setWebParametersInt: function (key, value) {
    var key_Str = Pointer_stringify(key);
    setWebParametersInt(key_Str, value);
  },

  setWebParametersDouble: function (key, value) {
    var key_Str = Pointer_stringify(key);
    setWebParametersDouble(key_Str, value);
  },

  setWebParametersBool: function (key, value) {
    var key_Str = Pointer_stringify(key);
    setWebParametersBool(key_Str, value);
  },

  setWebParametersString: function (key, value) {
    var key_Str = Pointer_stringify(key);
    var value_Str = Pointer_stringify(value);
    setWebParametersString(key_Str, value_Str);
  },

  muteRemoteVideoStream2: function (channel, userId, mute) {},

  startScreenCaptureByWindowId: function (
    windowId,
    regionRectX,
    regionRectY,
    regionRectWidth,
    regionRectHeight,
    screenCaptureVideoDimenWidth,
    screenCaptureVideoDimenHeight,
    screenCaptureFrameRate,
    screenCaptureBitrate,
    screenCaptureCaptureMouseCursor
  ) {},
  getRemoteVideoStats_WGL: function() {
    getRemoteVideoStats();
  },
  getRemoteVideoStats_MC: function() {
    getRemoteVideoStatsMC();
  },
  generateNativeTexture: function () {},
  setLocalVoicePitch: function (pitch) {},
  addInjectStreamUrl2: function (
    channel,
    url,
    width,
    height,
    videoGop,
    videoFramerate,
    videoBitrate,
    audioSampleRate,
    audioBitrate,
    audioChannels
  ) {},
  enableFaceDetection: function (enable) {},

  getConnectionState2: function (channel) {
    return getConnectionState2_MC();
  },

  initEventOnEngineCallback: function (OnJoinChannelSuccessCallback,
                                      OnReJoinChannelSuccessCallback,
                                      OnConnectionLostCallback,
                                      OnLeaveChannelCallback,
                                      OnConnectionInterruptedCallback,
                                      OnRequestTokenCallback,
                                      OnUserJoinedCallback,
                                      OnUserOfflineCallback,
                                      OnAudioVolumeIndicationCallback,
                                      OnUserMuteAudioCallback,
                                      OnSDKWarningCallback,
                                      OnSDKErrorCallback,
                                      OnRtcStatsCallback,
                                      OnAudioMixingFinishedCallback,
                                      OnAudioRouteChangedCallback,
                                      OnFirstRemoteVideoDecodedCallback,
                                      OnVideoSizeChangedCallback,
                                      OnClientRoleChangedCallback,
                                      OnUserMuteVideoCallback,
                                      OnMicrophoneEnabledCallback,
                                      OnApiExecutedCallback,
                                      OnFirstLocalAudioFrameCallback,
                                      OnFirstRemoteAudioFrameCallback,
                                      OnLastmileQualityCallback,
                                      OnAudioQualityCallback,
                                      OnStreamInjectedStatusCallback,
                                      OnStreamUnpublishedCallback,
                                      OnStreamPublishedCallback,
                                      OnStreamMessageErrorCallback,
                                      OnStreamMessageCallback,
                                      OnConnectionBannedCallback,
                                      OnVideoStoppedCallback,
                                      OnTokenPrivilegeWillExpireCallback,
                                      OnNetworkQualityCallback,
                                      OnLocalVideoStatsCallback,
                                      OnRemoteVideoStatsCallback,
                                      OnRemoteAudioStatsCallback,
                                      OnFirstLocalVideoFrameCallback,
                                      OnFirstRemoteVideoFrameCallback,
                                      OnUserEnableVideoCallback,
                                      OnAudioDeviceStateChangedCallback,
                                      OnCameraReadyCallback,
                                      OnCameraFocusAreaChangedCallback,
                                      OnCameraExposureAreaChangedCallback,
                                      OnRemoteAudioMixingBeginCallback,
                                      OnRemoteAudioMixingEndCallback,
                                      OnAudioEffectFinishedCallback,
                                      OnVideoDeviceStateChangedCallback,
                                      OnRemoteVideoStateChangedCallback,
                                      OnUserEnableLocalVideoCallback,
                                      OnLocalPublishFallbackToAudioOnlyCallback,
                                      OnRemoteSubscribeFallbackToAudioOnlyCallback,
                                      OnConnectionStateChangedCallback,
                                      OnRemoteVideoTransportStatsCallback,
                                      OnRemoteAudioTransportStatsCallback,
                                      OnTranscodingUpdatedCallback,
                                      OnAudioDeviceVolumeChangedCallback,
                                      OnActiveSpeakerCallback,
                                      OnMediaEngineStartCallSuccessCallback,
                                      OnMediaEngineLoadSuccessCallback,
                                      OnAudioMixingStateChangedCallback,
                                      OnFirstRemoteAudioDecodedCallback,
                                      OnLocalVideoStateChangedCallback,
                                      OnRtmpStreamingStateChangedCallback,
                                      OnNetworkTypeChangedCallback,
                                      OnLastmileProbeResultCallback,
                                      OnLocalUserRegisteredCallback,
                                      OnUserInfoUpdatedCallback,
                                      OnLocalAudioStateChangedCallback,
                                      OnRemoteAudioStateChangedCallback,
                                      OnLocalAudioStatsCallback,
                                      OnChannelMediaRelayStateChangedCallback,
                                      OnChannelMediaRelayEventCallback,
                                      OnFacePositionChangedCallback,
                                      OnRtmpStreamingEventCallback,
                                      OnAudioPublishStateChangeCallback,
                                      OnVideoPublishStateChangeCallback,
                                      OnAudioSubscribeStateChangeCallback,
                                      OnVideoSubscribeStateChangeCallback,
                                      OnFirstLocalAudioFramePublishedCallback,
                                      OnFirstLocalVideoFramePublishedCallback,
                                      OnUserSuperResolutionEnabledCallback,
                                      OnUploadLogResultCallback,
                                      OnVirtualBackgroundSourceEnabledCallback,
                                      OnRequestAudioFileInfo,
                                      OnContentInspectResult,
                                      OnSnapshotTaken,
                                      OnClientRoleChangeFailed,
                                      OnAudioDeviceTestVolumeIndication,
                                      OnProxyConnected,
                                      OnWlAccMessage,
                                      OnWlAccStats,
                                      OnScreenCaptureInfoUpdated

  ) {
    UnityHooks.OnStreamMessageCallback = OnStreamMessageCallback;
    UnityHooks.InvokeStreamMessageCallback = function(uid, bytes, length) {
      if (UnityHooks.data) {
        _free(UnityHooks.data);
      }
      var data = _malloc(length);
      for(var i=0; i<length; i++) {
        HEAPU8[data+i] = bytes[i];
      }
      UnityHooks.data = data;
      //Runtime.dynCall('viiii', UnityHooks.OnStreamMessageCallback, [uid, 0, data, length]);
      Module['dynCall_viiiii'](UnityHooks.OnStreamMessageCallback, uid, 0, data, length);
    };

    UnityHooks.OnVideoSizeChangedCallback = OnVideoSizeChangedCallback;
    UnityHooks.InvokeVideoSizeChangedCallback = function(uid, width, height) {
      //Runtime.dynCall('viiii', UnityHooks.OnVideoSizeChangedCallback, [uid, width, height, 0]);
      Module['dynCall_viiiii'](UnityHooks.OnStreamMessageCallback, uid, 0, data, length);
    };

    UnityHooks.isLoaded = true;
  },

  getCallId2: function (channel) {},
  getAudioPlaybackDeviceCount: function () {},
  updateScreenCaptureRegion: function (x, y, width, height) {},
  startRecordingService: function (recordingKey) {},
  clearVideoWatermarks: function () {
    clearVideoWatermarks_WGL();
  },
  startChannelMediaRelay: function (
    srcChannelName,
    srcToken,
    srcUid,
    destChannelName,
    destToken,
    destUid,
    destCount
  ) {},

  startChannelMediaRelay_WEBGL: function (
    srcChannelName,
    srcToken,
    srcUid,
    destChannelName,
    destToken,
    destUid,
    destCount
  ) {
    var srcChannelNameStr = Pointer_stringify(srcChannelName);
    var srcTokenStr = Pointer_stringify(srcToken);
    var destChannelNameStr = Pointer_stringify(destChannelName);
    var destTokenStr = Pointer_stringify(destToken);

    var srcUid_Str = Pointer_stringify(srcUid);
    var destUid_Str = Pointer_stringify(destUid);

    startChannelMediaRelay(
      srcChannelNameStr,
      srcTokenStr,
      srcUid_Str,
      destChannelNameStr,
      destTokenStr,
      destUid_Str,
      destCount
    );
  },

  updateChannelMediaRelay: function (
    srcChannelName,
    srcToken,
    srcUid,
    destChannelName,
    destToken,
    destUid,
    destCount
  ) {},

  updateChannelMediaRelay_WEBGL: function (
    srcChannelName,
    srcToken,
    srcUid,
    destChannelName,
    destToken,
    destUid,
    destCount
  ) {
    var srcChannelNameStr = Pointer_stringify(srcChannelName);
    var srcTokenStr = Pointer_stringify(srcToken);
    var destChannelNameStr = Pointer_stringify(destChannelName);
    var destTokenStr = Pointer_stringify(destToken);

    var srcUid_Str = Pointer_stringify(srcUid);
    var destUid_Str = Pointer_stringify(destUid);

    updateChannelMediaRelay(
      srcChannelNameStr,
      srcTokenStr,
      srcUid_Str,
      destChannelNameStr,
      destTokenStr,
      destUid_Str,
      destCount
    );
  },

  stopChannelMediaRelay: function () {
    stopChannelMediaRelay();
  },

  addVideoWatermark: function (url, x, y, width, height) {
    var url_Str = Pointer_stringify(url);
    startWaterMark_WGL(url_Str, x, y, width, height);
  },
  startChannelMediaRelay2: function (
    channel,
    srcChannelName,
    srcToken,
    srcUid,
    destChannelName,
    destToken,
    destUid,
    destCount
  ) {},
  startChannelMediaRelay2_WEBGL: function (
    channel,
    srcChannelName,
    srcToken,
    srcUid,
    destChannelName,
    destToken,
    destUid,
    destCount
  ) {
    var srcChannelNameStr = Pointer_stringify(srcChannelName);
    var srcTokenStr = Pointer_stringify(srcToken);
    var destChannelNameStr = Pointer_stringify(destChannelName);
    var destTokenStr = Pointer_stringify(destToken);

    var srcUid_Str = Pointer_stringify(srcUid);
    var destUid_Str = Pointer_stringify(destUid);

    startChannelMediaRelay_MC(
      srcChannelNameStr,
      srcTokenStr,
      srcUid_Str,
      destChannelNameStr,
      destTokenStr,
      destUid_Str,
      destCount
    );
  },
  getAudioRecordingDeviceVolume: function () {
    return wrapper.savedSettings.localAudioTrackVolume;
  },

  releaseAAudioPlaybackDeviceManager: function () {},

  // Stubs for Unity 2021.2.x, see https://github.com/AgoraIO-Community/Agora_Unity_WebGL/issues/17
  adjustLoopbackRecordingSignalVolume: function() {},
  createDataStream2: function (channel, reliable, ordered) {},
  createDataStream_channel: function(channel, syncWithAudio, ordered) {},

  enableDeepLearningDenoise: function() {},
  enableVirtualBackground: function(enabled, backgroundSourceType, color, source, blurDegree, mute, loop) {
    enable = enabled == 0 ? false : true;
    muted = mute == 0 ? false : true;
    looped = loop == 0 ? false : true;
    source_Str = Pointer_stringify(source);
    initVirtualBackground(enable, backgroundSourceType, color, source_Str, blurDegree, muted, looped);
  },
  setVirtualBackgroundBlur: function(blurDegree) {
    setVirtualBackgroundBlur(blurDegree);
  },
  setVirtualBackgroundColor: function(hexColor) {
    var myColor = Pointer_stringify(hexColor);
    setVirtualBackgroundColor(myColor);
  },
  setVirtualBackgroundImage: function(imageFile) {
    var myImg = Pointer_stringify(imageFile);
    setVirtualBackgroundImage(myImg);
  },
  setVirtualBackgroundVideo: function(videoFile) {
    var myVideo = Pointer_stringify(videoFile);
    setVirtualBackgroundVideo(myVideo);
  },
  initVirtualBackground_MC: function(enabled, backgroundSourceType, color, source, blurDegree, mute, loop) {
    enable = enabled == 0 ? false : true;
    muted = mute == 0 ? false : true;
    looped = loop == 0 ? false : true;
    source_Str = Pointer_stringify(source);
    initVirtualBackground_MC(enabled, backgroundSourceType, color, source_Str, blurDegree, mute, loop);
  },
  setVirtualBackgroundBlur_MC: function(blurDegree) {
    setVirtualBackgroundBlur_MC(blurDegree);
  },
  setVirtualBackgroundColor_MC: function(hexColor) {
    var myColor = Pointer_stringify(hexColor);
    setVirtualBackgroundColor_MC(myColor);
  },
  setVirtualBackgroundImage_MC: function(imageFile) {
    var myImg = Pointer_stringify(imageFile);
    setVirtualBackgroundImage_MC(myImg);
  },
  setVirtualBackgroundVideo_MC: function(videoFile) {
    var myVideo = Pointer_stringify(videoFile);
    setVirtualBackgroundVideo_MC(myVideo);
  },
  getAudioMixingDuration2: function() {},
  getEffectCurrentPosition: function() {},
  getEffectDuration: function() {},
  isCameraTorchSupported: function() {},
  playEffect2: function() {},
  setCameraTorchOn: function() {},
  setCloudProxy: function() {},
  setEffectPosition: function() {},
  setLocalAccessPoint: function() {},
  setVoiceBeautifierParameters: function() {},
  setVoiceConversionPreset: function() {},
  startAudioMixing2: function() {},
  startAudioRecordingWithConfig: function() {},
  switchChannel2: function() {},
  uploadLogFile: function() {},
  setCameraCaptureRotation: function (rotation) {}
};

autoAddDeps(LibraryAgoraWebGLSDK, "$localVideo");
autoAddDeps(LibraryAgoraWebGLSDK, "$remoteVideoInstances");
mergeInto(LibraryManager.library, LibraryAgoraWebGLSDK);
