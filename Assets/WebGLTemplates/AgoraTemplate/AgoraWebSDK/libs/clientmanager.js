class ClientManager {
  constructor() {
    this.client = undefined; // first client
    this.options = {
      appid: null,
      channel: null,
      uid: null,
      token: null,
    };
    this.videoEnabled = false; // if true then camera is created, if false then not
    this.client_role = 1; // default is host, 2 is audience
    this._storedChannelProfile = 0; // channel profile saved before join is called
  }

  manipulate() {}

  setVideoEnabled(enabled) {
    this.videoEnabled = enabled;
  }

  getMainAppId() {
    return this.options.appid;
  }

  getClient() {
    return this.client;
  }

  setChannelProfile(profile) {
    _logger("in client manager, setChannelProfile: " + profile);
    this._storedChannelProfile = profile;
    var mode = this.getChannelProfileMode();
    this.client = null;
    _logger("created new client");
    this.client = AgoraRTC.createClient({ mode: mode, codec: "vp8" });
  }

  getChannelProfileMode = function () {
    _logger("in getChannelProfileMode client manager");
    if (this._storedChannelProfile == 0) {
      return "rtc";
    } else if (this._storedChannelProfile == 1) {
      return "live";
    }
  };

  setOptions(channelkey, channelName, uid) {
    this.options.token = channelkey;
    this.options.channel = channelName;
    this.options.uid = uid;
  }

  createEngine(appID) {
    if (this.client == undefined) {
      var mode = this.getChannelProfileMode();
      wrapper.log("using mode: " + mode);
      this.client = AgoraRTC.createClient({ mode: mode, codec: "vp8" });
      this.options.appid = appID;

      wrapper.setup(this.client);
      audioEffects.initialize(this.client);
      cacheDevices();

      return true;
    } else {
      wrapper.setup(this.client);
      audioEffects.initialize(this.client);
      cacheDevices();
      return false;
    }
  }

  // subscribe
  async subscribe_remoteuser(user, mediaType) {
    const uid = user.uid;
    // subscribe to a remote user
    await this.client.subscribe(user, mediaType);
    if (mediaType === "video") {
      user.videoTrack.play(`player-${uid}`, { fit: "cover", mirror: mremote });
      var strUID = uid.toString();
      event_manager.raiseOnRemoteUserJoined(strUID);
    }
    if (mediaType === "audio") {
      user.audioTrack.play();
    }
  }

  async handleUserPublished(user, mediaType) {
    const id = user.uid;
    remoteUsers[id] = user;
    await this.subscribe_remoteuser(user, mediaType);
  }

  handleUserJoined(user, mediaType) {
    const id = user.uid;
  }

  handleUserUnpublished(user) {
    const id = user.uid;
    delete remoteUsers[id];
    $(`#player-wrapper-${id}`).remove();
    var strUID = id.toString();
    event_manager.raiseOnRemoteUserLeaved(strUID);
  }

  async leave() {
    for (var trackName in localTracks) {
      var track = localTracks[trackName];
      if (track) {
        track.stop();
        track.close();
        localTracks[trackName] = null;
      }
    }

    this.videoEnabled = false; // set to default
    localTracks.audioMixingTrack = null;

    if (_isCopyVideoToMainCanvasOn) {
      _isCopyVideoToMainCanvasOn = false;
      customVideoTrack.stop();
      customVideoTrack.close();
    }

    if (mixingStatus == true) {
      try {
        // stop audio mixing track
        if (localTracks.audioMixingTrack) {
          await this.client.unpublish(localTracks.audioMixingTrack);
          localTracks.audioMixingTrack.stopProcessAudioBuffer();
          localTracks.audioMixingTrack.stop();
          localTracks.audioMixingTrack.close();
          localTracks.audioMixingTrack = null;
        }
      } catch (error) {
        //console.log("StopAudioMixing error " + error);
      }
      mixingStatus = false;
    }

    audioEffects._stopAllEffects();

    wrapper.destructStats();

    // remove remote users and player views
    remoteUsers = {};

    event_manager.raiseOnLeaveChannel();

    // leave the channel
    await this.client.leave();
  }

  handleException(e) {
    console.log(e);
  }

  handleError(e) {
    console.log(e);
  }

  async switchChannel(token_str, channelId_str) {
    await this.leave();

    this.options.token = token_str;
    this.options.channel = channelId_str;

    if (this.videoEnabled) {
      [this.options.uid, localTracks.audioTrack, localTracks.videoTrack] =
        await Promise.all([
          this.client.join(
            this.options.appid,
            this.options.channel,
            this.options.token || null
          ),
          AgoraRTC.createMicrophoneAudioTrack(),
          AgoraRTC.createCameraVideoTrack(),
        ]);

      currentVideoDevice = wrapper.getCameraDeviceIdFromDeviceName(
        localTracks.videoTrack._deviceName
      );

      currentAudioDevice = wrapper.getMicrophoneDeviceIdFromDeviceName(
        localTracks.audioTrack._deviceName
      );

      event_manager.raiseGetCurrentVideoDevice();
      event_manager.raiseGetCurrentAudioDevice();
      event_manager.raiseGetCurrentPlayBackDevice();

      localTracks.videoTrack.play("local-player", {
        fit: "cover",
        mirror: mlocal,
      });

      event_manager.raiseJoinChannelSuccess(
        this.options.uid.toString(),
        this.options.channel
      );

      $("#local-player-name").text(`localVideo(${this.options.uid})`);

      if (this.client_role == 1) {
        await this.client.publish(
          Object.values(localTracks).filter((track) => track !== null)
        );
      }
    } else {
      [this.options.uid, localTracks.audioTrack] = await Promise.all([
        this.client.join(
          this.options.appid,
          this.options.channel,
          this.options.token || null
        ),
        AgoraRTC.createMicrophoneAudioTrack(),
      ]);

      currentAudioDevice = wrapper.getMicrophoneDeviceIdFromDeviceName(
        localTracks.audioTrack._deviceName
      );

      event_manager.raiseGetCurrentAudioDevice();
      event_manager.raiseGetCurrentPlayBackDevice();

      event_manager.raiseJoinChannelSuccess(
        this.options.uid.toString(),
        this.options.channel
      );

      if (this.client_role == 1) {
        await this.client.publish(
          Object.values(localTracks).filter((track) => track !== null)
        );
      }
    }
  }

  isHosting() {
    if (this._storedChannelProfile == 0) { return true; }
    if (this._storedChannelProfile == 1) {
	return (this.client_role == 1);
    }
    return false;
  }

  async joinChannelWithUserAccount_WGL(
    token_str,
    channelId_str,
    userAccount_str
  ) {
    // setting options
    this.options.channel = channelId_str;
    this.options.token = token_str;

    this.client.on("user-published", this.handleUserPublished.bind(this));
    this.client.on("user-joined", this.handleUserJoined.bind(this));
    this.client.on("user-unpublished", this.handleUserUnpublished.bind(this));
    this.client.on("exception", this.handleException.bind(this));
    this.client.on("error", this.handleError.bind(this));

    if (this.videoEnabled && this.isHosting()) {
      [this.options.uid, localTracks.audioTrack, localTracks.videoTrack] =
        await Promise.all([
          this.client.join(
            this.options.appid,
            this.options.channel,
            this.options.token || null,
            userAccount_str.toString() || null
          ),
          AgoraRTC.createMicrophoneAudioTrack(),
          AgoraRTC.createCameraVideoTrack(),
        ]);

      currentVideoDevice = wrapper.getCameraDeviceIdFromDeviceName(
        localTracks.videoTrack._deviceName
      );

      currentAudioDevice = wrapper.getMicrophoneDeviceIdFromDeviceName(
        localTracks.audioTrack._deviceName
      );

      event_manager.raiseGetCurrentVideoDevice();
      event_manager.raiseGetCurrentAudioDevice();
      event_manager.raiseGetCurrentPlayBackDevice();

      localTracks.videoTrack.play("local-player", {
        fit: "cover",
        mirror: mlocal,
      });

      event_manager.raiseJoinChannelSuccess(
        this.options.uid.toString(),
        this.options.channel
      );

      if (this.client_role == 1) {
        await this.client.publish(
          Object.values(localTracks).filter((track) => track !== null)
        );
      }
    } else {
      [this.options.uid, localTracks.audioTrack] = await Promise.all([
        this.client.join(
          this.options.appid,
          this.options.channel,
          this.options.token || null,
          userAccount_str.toString() || null
        ),
        AgoraRTC.createMicrophoneAudioTrack(),
      ]);

      currentAudioDevice = wrapper.getMicrophoneDeviceIdFromDeviceName(
        localTracks.audioTrack._deviceName
      );

      event_manager.raiseGetCurrentAudioDevice();
      event_manager.raiseGetCurrentPlayBackDevice();

      event_manager.raiseJoinChannelSuccess(
        this.options.uid.toString(),
        this.options.channel
      );

      if (this.client_role == 1) {
        await this.client.publish(
          Object.values(localTracks).filter((track) => track !== null)
        );
      }
    }
  }

  async joinChannel() {
    this.client.on("user-published", this.handleUserPublished.bind(this));
    this.client.on("user-joined", this.handleUserJoined.bind(this));
    this.client.on("user-unpublished", this.handleUserUnpublished.bind(this));
    this.client.on("exception", this.handleException.bind(this));
    this.client.on("error", this.handleError.bind(this));

    if (this.videoEnabled && this.isHosting()) {
      [this.options.uid, localTracks.audioTrack, localTracks.videoTrack] =
        await Promise.all([
          this.client.join(
            this.options.appid,
            this.options.channel,
            this.options.token || null,
            this.options.uid || null
          ),
          AgoraRTC.createMicrophoneAudioTrack(),
          AgoraRTC.createCameraVideoTrack(),
        ]);

      currentVideoDevice = wrapper.getCameraDeviceIdFromDeviceName(
        localTracks.videoTrack._deviceName
      );

      currentAudioDevice = wrapper.getMicrophoneDeviceIdFromDeviceName(
        localTracks.audioTrack._deviceName
      );

      event_manager.raiseGetCurrentVideoDevice();
      event_manager.raiseGetCurrentAudioDevice();
      event_manager.raiseGetCurrentPlayBackDevice();

      localTracks.videoTrack.play("local-player", {
        fit: "cover",
        mirror: mlocal,
      });

      event_manager.raiseJoinChannelSuccess(
        this.options.uid.toString(),
        this.options.channel
      );

      $("#local-player-name").text(`localVideo(${this.options.uid})`);

    } else {
      // video is not enabled
      [this.options.uid, localTracks.audioTrack] = await Promise.all([
        this.client.join(
          this.options.appid,
          this.options.channel,
          this.options.token || null,
          this.options.uid || null
        ),
        AgoraRTC.createMicrophoneAudioTrack(),
      ]);

      currentAudioDevice = wrapper.getMicrophoneDeviceIdFromDeviceName(
        localTracks.audioTrack._deviceName
      );

      event_manager.raiseGetCurrentAudioDevice();
      event_manager.raiseGetCurrentPlayBackDevice();

      event_manager.raiseJoinChannelSuccess(
        this.options.uid.toString(),
        this.options.channel
      );

    }

    if (this.client_role == 1) {
        await this.client.publish(
          Object.values(localTracks).filter((track) => track !== null)
        );
    }
  }

  async setClientRole(role) {
    if (this.client) {
      this.client_role = role;
      if (role === 1) {
        this.client.setClientRole("host", function (e) {
          if (!e) {
            this.client_role = 1;
          }
        });
      } else if (role === 2) {
        await this.client.unpublish(localTracks.videoTrack);
        this.client.setClientRole("audience", function (e) {
          if (!e) {
            this.client_role = 2;
          }
        });
      }
    }
  }

  async enableLocalVideo(enabled) {
    if (this.client) {
      if (enabled == false) {
        localTracks.videoTrack.stop();
        localTracks.videoTrack.close();
        await this.client.unpublish(localTracks.videoTrack);
      } else {
        [localTracks.videoTrack] = await Promise.all([
          AgoraRTC.createCameraVideoTrack(),
        ]);

        localTracks.videoTrack.play("local-player");

        await this.client.publish(localTracks.videoTrack);
      }
    }
  }

  async startPreview() {
    [localTracks.videoTrack] = await Promise.all([
      AgoraRTC.createCameraVideoTrack(),
    ]);
    localTracks.videoTrack.play("local-player");
  }

  stopPreview() {
    if (localTracks.videoTrack) {
      localTracks.videoTrack.stop();
      localTracks.videoTrack.close();
    }
  }

  async setBeautyEffectOn(lighteningLevel, rednessLevel, smoothnessLevel) {
    if (localTracks.videoTrack) {
      await localTracks.videoTrack.setBeautyEffect(true, {
        lighteningLevel: lighteningLevel,
        rednessLevel: rednessLevel,
        smoothnessLevel: smoothnessLevel,
      });
    }
  }

  async setBeautyEffectOff() {
    if (localTracks.videoTrack) {
      await localTracks.videoTrack.setBeautyEffect(false);
    }
  }

  async renewToken(token) {
    if (this.client) {
      await this.client.renewToken(token);
    }
  }

  enableAudioVolumeIndicator() {
    if (this.client) {
      this.client.enableAudioVolumeIndicator();
    }
  }

  setEncryptionSecret(secret) {
    if (this.client) {
      if (savedEncryptionMode != "") {
        this.client.setEncryptionConfig(savedEncryptionMode, secret);
      } else {
        console.log("Please call setEncryptionMode first");
      }
    }
  }

  async setExternalAudioSource_WGL(enabled, sampleRate, channels) {
    if (enabled == 1) {
      localTracks.audioTrack.stop();
      localTracks.audioTrack.close();

      await this.client.unpublish(localTracks.audioTrack);
      dest = audioCtx.createMediaStreamDestination();

      var CustomVideoTrackInitConfig = {
        mediaStreamTrack: dest.stream.getAudioTracks()[0],
      };

      [localTracks.audioTrack] = await Promise.all([
        AgoraRTC.createCustomAudioTrack(CustomVideoTrackInitConfig),
      ]);

      // play local video track
      await this.client.publish(localTracks.audioTrack);

      playMusc();
    } else if (enabled == 0) {
      localTracks.audioTrack.stop();
      localTracks.audioTrack.close();
      play = false;

      await this.client.unpublish(localTracks.audioTrack);

      [localTracks.audioTrack] = await Promise.all([
        AgoraRTC.createMicrophoneAudioTrack(),
      ]);

      await this.client.publish(localTracks.audioTrack);
    }
  }

  async setExternalVideoSource_WGL(enable) {
    if (enable == 1) {
      if (localTracks.videoTrack) {
        localTracks.videoTrack.stop();
        localTracks.videoTrack.close();
        await this.client.unpublish(localTracks.videoTrack);
      }

      var stream = mainCanvas.captureStream(15); // 25 FPS
      var CustomVideoTrackInitConfig = {
        bitrateMax: 400,
        bitrateMin: 200,
        mediaStreamTrack: stream.getTracks()[0],
        optimizationMode: "detail",
      };

      [localTracks.videoTrack] = await Promise.all([
        AgoraRTC.createCustomVideoTrack(CustomVideoTrackInitConfig),
      ]);
      localTracks.videoTrack.play("local-player");
      await this.client.publish(localTracks.videoTrack);
    } else if (enable == 0) {
      localTracks.videoTrack.stop();
      localTracks.videoTrack.close();

      await this.client.unpublish(localTracks.videoTrack);

      [localTracks.videoTrack] = await Promise.all([
        AgoraRTC.createCameraVideoTrack(),
      ]);

      localTracks.videoTrack.play("local-player");
      await this.client.publish(localTracks.videoTrack);
    }
  }

  async clearVideoWatermarks_WGL() {
    _isCopyVideoToMainCanvasOn = false;
    customVideoTrack.stop();
    customVideoTrack.close();

    localTracks.videoTrack.stop();
    localTracks.videoTrack.close();
    await this.client.unpublish(localTracks.videoTrack);

    [localTracks.videoTrack] = await Promise.all([
      AgoraRTC.createCameraVideoTrack(),
    ]);
    localTracks.videoTrack.play("local-player");
    await this.client.publish(localTracks.videoTrack);
    wrapper.savedSettings.watermarkOn = false;
    listingOfWatermarks = Array();
  }

  async startWaterMark_WGL(url, x, y, width, height) {
    listingOfWatermarks.push({
      url: url,
      x: x,
      y: y,
      width: width,
      height: height,
    });

    if (wrapper.savedSettings.watermarkOn == false) {
      localTracks.videoTrack.stop();
      localTracks.videoTrack.close();
      await this.client.unpublish(localTracks.videoTrack);

      [customVideoTrack] = await Promise.all([
        AgoraRTC.createCameraVideoTrack(),
      ]);
      customVideoTrack.play("custom_track");

      _isCopyVideoToMainCanvasOn = true;
      copyVideoToMainCanvas();

      wmConfig.url = url;
      wmConfig.x = x;
      wmConfig.y = y;
      wmConfig.width = width;
      wmConfig.height = height;
      this.watermarkPublishNow();
    }
    wrapper.savedSettings.watermarkOn = true;
  }

  async watermarkPublishNow() {
    var stream = mainCanvas.captureStream(25); // 25 FPS
    var CustomVideoTrackInitConfig = {
      bitrateMax: 400,
      bitrateMin: 200,
      mediaStreamTrack: stream.getTracks()[0],
      optimizationMode: "detail",
    };

    [localTracks.videoTrack] = await Promise.all([
      AgoraRTC.createCustomVideoTrack(CustomVideoTrackInitConfig),
    ]);
    localTracks.videoTrack.play("local-player");
    await this.client.publish(localTracks.videoTrack);
  }

  async startScreenCaptureForWeb() {
    localTracks.videoTrack.stop();
    localTracks.videoTrack.close();
    await this.client.unpublish(localTracks.videoTrack);

    [localTracks.videoTrack] = await Promise.all([
      AgoraRTC.createScreenVideoTrack(),
    ]);
    localTracks.videoTrack.play("local-player");
    await this.client.publish(localTracks.videoTrack);
  }

  // Shares the whole or part of a screen by specifying the display ID.
  async startScreenCaptureByDisplayId(
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

    console.error("Note this API should be replaced by startScreenCaptureForWeb instead.");
    localTracks.videoTrack.stop();
    localTracks.videoTrack.close();
    await this.client.unpublish(localTracks.videoTrack);

    [localTracks.videoTrack] = await Promise.all([
      AgoraRTC.createScreenVideoTrack(),
    ]);
    localTracks.videoTrack.play("local-player");
    await this.client.publish(localTracks.videoTrack);
  }

  // Stop screen sharing.
  async stopScreenCapture() {
    localTracks.videoTrack.stop();
    localTracks.videoTrack.close();
    await this.client.unpublish(localTracks.videoTrack);

    [localTracks.videoTrack] = await Promise.all([
      AgoraRTC.createCameraVideoTrack(),
    ]);
    localTracks.videoTrack.play("local-player");
    await this.client.publish(localTracks.videoTrack);
  }

  // Starts the last-mile network probe test.
  enableLastMile(enabled) {
    if (enabled == true) {
      this.client.getRemoteNetworkQuality();
      this.client.on("network-quality", this.handleNetworkQuality);
    } else {
      this.client.off("network-quality", this.handleNetworkQuality);
    }
  }

  handleNetworkQuality(quality) {
    event_manager.raiseLastMileQuality(quality);
  }

  async subscribe_mv(user, mediaType) {
    try {
      await this.client.subscribe(user, mediaType);
      if (mediaType === "video") {
        user.videoTrack.play(`player-${user.uid}`);
      } else if (mediaType === "audio") {
        user.audioTrack.play();
      }
    } catch (error) {
      console.log("subscribe error ", error);
    }
  }

  async unsubscribe(user, mediaType) {
    try {
      await this.client.unsubscribe(user, mediaType);
    } catch (error) {
      console.log("unsubscribe error ", error);
    }
  }

  // Sets the stream type of the remote video
  async SetRemoteVideoSTreamType(usersIdStream, streamType) {
    try {
      await this.client.setRemoteVideoStreamType(usersIdStream, streamType);
    } catch (error) {
      console.log("error from SetRemoteVideoSTreamType", error);
    }
  }

  async enableDualStream_WGL(enabled) {
    if (enabled == true) {
      await this.client.enableDualStream();
    } else {
      await this.client.disableDualStream();
    }
  }

  async setLocalPublishFallbackOption_WGL(option) {
    try {
      await this.client.setStreamFallbackOption(this.client.uid, option);
    } catch (error) {
      console.log("error from setLocalPublishFallbackOption", error);
    }
  }

  SetRemoteUserPriority(uid, userPriority) {
    if (userPriority == 50 || userPriority == 0)
      this.client.setRemoteVideoStreamType(uid, 0);
    else if (userPriority == 100 || userPriority == 1)
      this.client.setRemoteVideoStreamType(uid, 1);
  }

  setRemoteSubscribeFallbackOption_WGL(option) {
    Object.keys(remoteUsers).forEach((uid2) => {
      this.client.setRemoteVideoStreamType(uid2, option);
    });
  }

  enableLogUpload() {
    AgoraRTC.enableLogUpload();
    console.log("----------- log upload to server enabled -------- ");
  }

  disableLogUpload() {
    AgoraRTC.disableLogUpload();
    console.log("----------- log upload to server disabled -------- ");
  }
}
