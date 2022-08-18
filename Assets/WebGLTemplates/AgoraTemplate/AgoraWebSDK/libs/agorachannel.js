// Agora Channel for multi clients
class AgoraChannel {
  constructor() {
    //super();
    this.client = undefined; // first client
    this.options = {
      appid: null,
      channel: null,
      uid: null,
      token: null,
    };
    this.videoEnabled = false; // if true then camera is created, if false then not
    this.audioEnabled = false; // if true then mic access is created, if false then not
    this.videoSubscribing = true;
    this.audioSubscribing = true;

    this.is_publishing = false;
    this.is_screensharing = false;
    this.remoteUsers = {};
    this.remoteUsersAudioMuted = {};
    this.remoteUsersVideoMuted = {};
    this.enableLoopbackAudio = false;
    this.muteAllAudio = false;
    this.muteAllVideo = false;
    this.channelId = "";
    this.mode = "rtc";
    this.client_role = 1; // default is host, 2 is audience
    this.liveTranscodingConfig = null;
    this.volumeIndicationOn = false;
    this.tempLocalTracks = null;
    this.userJoinedHandle = this.handleUserJoined.bind(this);
    this.userPublishedHandle = this.handleUserPublished.bind(this);
    this.userUnpublishedHandle = this.handleUserUnpublished.bind(this);
    this.userLeftHandle = this.handleUserLeft.bind(this);
    this.userExceptionHandle = this.handleException.bind(this);
    this.userErrorHandle = this.handleError.bind(this);
    this.userVolumeHandle = this.handleVolumeIndicator.bind(this);
    this.userStreamHandle = this.handleStreamMessage.bind(this);
    this.userTokenWillExpireHandle = this.handleTokenPrivilegeWillExpire.bind(this);
    this.userTokenDidExpireHandle = this.handleTokenPrivilegeDidExpire.bind(this);
  }

  setOptions(channelkey, channelName, uid) {
    this.options.token = channelkey;
    this.options.channel = channelName;
    this.options.uid = uid;
  }

  setAVControl(subAudio, subVideo, pubAudio, pubVideo) {
    this.audioEnabled = pubAudio;
    this.videoEnabled = pubVideo;
    this.audioSubscribing = subAudio;
    this.videoSubscribing = subVideo;
  }

  getConnectionState() {
    if (this.client) {
      return this.client.connectionState;
    }
    return "NONE";
  }

  createClient() {
    if (this.client == undefined) {
      this.client = AgoraRTC.createClient({ mode: this.mode, codec: "vp8" });
      this.options.appid = client_manager.getMainAppId();
      if (this.volumeIndicationOn) {
        this.client.enableAudioVolumeIndicator();
      }
      return true;
    }
  }

  createClient_Live() {
    if (this.client == undefined) {
      this.mode = "live";
      this.client = AgoraRTC.createClient({ mode: this.mode, codec: "vp8" });
      this.options.appid = client_manager.getMainAppId();
      if (this.volumeIndicationOn) {
        this.client.enableAudioVolumeIndicator();
      }
      return true;
    }
  }

  handleUserJoined(user, mediaType) {
    const id = user.uid;
    event_manager.raiseChannelOnUserJoined_MC(id, this.options.channel);
    event_manager.raiseCustomMsg("New User Joined: " + id);
  }

  async handleUserPublished(user, mediaType) {
    const id = user.uid;
    this.remoteUsers[id] = user;

    if (this.muteAllAudio) {
      this.remoteUsersAudioMuted[id] = true;
    }

    if (this.muteAllVideo) {
      this.remoteUsersVideoMuted[id] = true;
    }

    var userAudioMuted = this.remoteUsersAudioMuted[id] != null && this.remoteUsersAudioMuted[id] == true;
    var userVideoMuted = this.remoteUsersVideoMuted[id] != null && this.remoteUsersVideoMuted[id] == true;
    if ((mediaType == "audio" && !userAudioMuted || mediaType == "video" && !userVideoMuted)) {
      if (mediaType == "video" || (mediaType == "audio" && this.screenShareClient == null
        || mediaType == "audio" && this.screenShareClient != null
        && id != this.screenShareClient.uid)) {
        await this.subscribe_remoteuser(user, mediaType);
      }
    }
    if (mediaType == "video") {
      this.getRemoteVideoStatsMC(id);
    }
  }

  handleUserLeft(user) {
    const id = user.uid;
    delete this.remoteUsers[id];
    event_manager.raiseChannelOnUserLeft_MC(id, this.options.channel);
    event_manager.raiseCustomMsg("User Left: " + id);
  }

  // subscribe
  async subscribe_remoteuser(user, mediaType) {
    const uid = user.uid;
    // subscribe to a remote user
    await this.client.subscribe(user, mediaType);
    if (mediaType === "video") {
      user.videoTrack.play(`player-${uid}`);
      var strUID = uid.toString();
      event_manager.raiseChannelOnUserPublished_MC(
        this.options.channel,
        strUID
      );
    }
    if (mediaType === "audio") {
      user.audioTrack.play();
    }
  }

  async publish() {
    // parameter is not local track
    if (this.is_publishing == false) {
      await this.setupLocalVideoTrack();
      if (localTracks.videoTrack != undefined) {
        await this.client.publish(localTracks.videoTrack);
      }
      await this.setupLocalAudioTrack();
      if (localTracks.audioTrack != undefined) {
        await this.client.publish(localTracks.audioTrack);
      }
      this.is_publishing = true;
      event_manager.raiseCustomMsg("Publish Success");
      console.log("Publish successfully, channel:" + this.channelId);
    }
  }

  async renewToken2_mc(token_str) {
    if (this.client) {
      await this.client.renewToken(token_str);
    }
  }

  async setupLocalVideoTrack() {
    if (localTracks != undefined && localTracks.videoTrack == undefined) {
      [localTracks.videoTrack] = await Promise.all([
        AgoraRTC.createCameraVideoTrack().catch(error => {
          event_manager.raiseHandleChannelError(this.channelId, error.code, error.message);
        }),
      ]);
      if(localTracks.videoTrack){
        localTracks.videoTrack.play("local-player");
      }
    }

  }

  async setupLocalAudioTrack() {
    if (localTracks != undefined && localTracks.audioTrack == undefined) {
      [localTracks.audioTrack] = await Promise.all([
        AgoraRTC.createMicrophoneAudioTrack().catch(e => {
          event_manager.raiseHandleChannelError(e.code, e.message);
        }),
      ]);
    }
  }

  async unpublish() {
    if (this.is_publishing == true) {
      for (var trackName in localTracks) {
        var track = localTracks[trackName];
        if (track) {
          await this.client.unpublish(track);
        }
      }
      this.is_publishing = false;
      event_manager.raiseCustomMsg("Unpublish Success");
    }
  }

  handleUserUnpublished(user) {
    const id = user.uid;
    delete remoteUsers[id];
    var strUID = id.toString();

    event_manager.raiseChannelOnUserUnPublished_MC(
      this.options.channel,
      strUID
    );
    event_manager.raiseCustomMsg("New User Published: " + id);
  }

  async handleVolumeIndicator(result) {
    var total = 0;
    var count = result.length;
    var info = "";
    const vad = 0;
    const channel_str = this.channelId;
    result.forEach(function (volume, index) {
      console.log(`${index} UID ${volume.uid} Level ${volume.level}`);

      if (volume.level > total) {
        total = volume.level;
      }
      var level = volume.level.toFixed();
      info += `\t${volume.uid}\t${level}\t${vad}\t${channel_str}`;
    });
    event_manager.raiseVolumeIndicator(info, count, total.toFixed());
  }

  async handleStreamMessage(uid, data) {
    UnityHooks.InvokeStreamMessageCallback(uid, data, data.length);
  }

  handleException(e) {
    console.log(e);
  }

  handleError(e) {
    event_manager.raiseHandleChannelError()
  }

  async handleStopScreenShare() {
    stopScreenCapture2();
  }

  async handleStopNewScreenShare() {
    stopNewScreenCaptureForWeb2();
  }

  async handleTokenPrivilegeWillExpire(){
    console.log("token will expire pretty soon...");
    event_manager.raiseChannelOnTokenPrivilegeWillExpire(this.options.channel, this.options.token);
  }

  async handleTokenPrivilegeDidExpire(){
    console.log("token has officially expired...");
    event_manager.raiseChannelOnTokenPrivilegeDidExpire(this.options.channel, this.options.token);
  }
  //============================================================================== 
  async joinChannel2(
    channel_str,
    token_str,
    userAccount_str
  ) {
    this.options.token = token_str;
    this.options.channel = channel_str;
    this.channelId = channel_str;

    //reset event listeners.
    // this.client.removeAllListeners("user-joined");
    // this.client.removeAllListeners("user-published");
    // this.client.removeAllListeners("user-unpublished");
    // this.client.removeAllListeners("user-left");
    // this.client.removeAllListeners("exception");
    // this.client.removeAllListeners("error");
    // this.client.removeAllListeners("volume-indicator");
    // this.client.removeAllListeners("stream-message");

    // add event listener to play remote tracks when remote user publishs.
    this.client.on("user-joined", this.userJoinedHandle);
    this.client.on("user-published", this.userPublishedHandle);
    this.client.on("user-unpublished", this.userUnpublishedHandle);
    this.client.on("user-left", this.userLeftHandle);
    this.client.on("exception", this.userExceptionHandle);
    this.client.on("error", this.userErrorHandle);
    this.client.on("volume-indicator", this.userVolumeHandle);
    this.client.on("stream-message", this.userStreamHandle);
    this.client.on("token-privilege-will-expire", this.userTokenWillExpireHandle);
    this.client.on("token-privilege-did-expire", this.userTokenDidExpireHandle);

    [this.options.uid] = await Promise.all([
      this.client.join(
        this.options.appid,
        this.options.channel,
        this.options.token || null,
        userAccount_str
      ),
    ]);

    if (this.client_role === 1 && this.videoEnabled) {
      await this.setupLocalVideoTrack();
      if (localTracks != undefined && localTracks.videoTrack != undefined) {
        localTracks.videoTrack.play("local-player");
        await this.client.publish(localTracks.videoTrack);
      } 
      this.is_publishing = true;
    }

    if (this.client_role === 1 && this.audioEnabled) {
      await this.setupLocalAudioTrack();
      if (localTracks != undefined && localTracks.audioTrack != undefined) {
        await this.client.publish(localTracks.audioTrack);
      }
      this.is_publishing = true;
    }

    multiclient_connections++;
    event_manager.raiseJoinChannelSuccess_MC(
      this.options.uid.toString(),
      this.options.channel
    );
    event_manager.raiseCustomMsg("Channel Joined With user Account");
  }

  async leave() {
    _logger("leaving in agorachannel");

    if(this.screenShareClient && this.screenShareClient.uid != null){
      this.handleUserLeft(this.screenShareClient);
      await stopNewScreenCaptureForWeb2();
    }

    if (multiclient_connections <= 1) {
      if (localTracks != undefined && localTracks.videoTrack != undefined) {
        localTracks.videoTrack.stop();
        localTracks.videoTrack.close();
        this.client.unpublish(localTracks.videoTrack);
        localTracks.videoTrack = undefined;
      }
      if (localTracks != undefined) {

        for (var i = 0; i < localTracks.length; i++) {
          localTracks[i].stop();
          localTracks[i].close();
          this.client.unpublish(localTracks[i])
        }
        localTracks = undefined;
      }

      
    }

    this.is_screensharing = false;
    this.is_publishing = false;
    // remove remote users and player views
    this.remoteUsers = {};
    multiclient_connections--;
    this.client.leave();
    this.client.off("user-joined", this.userJoinedHandle);
    this.client.off("user-published", this.userPublishedHandle);
    this.client.off("user-unpublished", this.userUnpublishedHandle);
    this.client.off("user-left", this.userLeftHandle);
    this.client.off("exception", this.userExceptionHandle);
    this.client.off("error", this.userErrorHandle);
    this.client.off("volume-indicator", this.userVolumeHandle);
    this.client.off("stream-message", this.userStreamHandle);
    _logger("leave successfull");
    event_manager.raiseOnLeaveChannel_MC(this.options.channel);
    //event_manager.raiseCustomMsg("Channel Left");
  }

  updateChannelMediaRelay_MC(
    srcChannelName,
    srcToken,
    srcUid,
    destChannelName,
    destToken,
    destUid,
    destCount
  ) {
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

    event_manager.raiseCustomMsg("updateChannelMediaRelay Doing...");
    this.client
      .updateChannelMediaRelay(configuration)
      .then(() => {
        event_manager.raiseCustomMsg("updateChannelMediaRelay success");
      })
      .catch((e) => {
        event_manager.raiseCustomMsg("updateChannelMediaRelay failed");
      });
  }

  stopChannelMediaRelay_MC() {
    this.client
      .stopChannelMediaRelay()
      .then(() => {
        event_manager.raiseCustomMsg("stopChannelMediaRelay success");
      })
      .catch((e) => {
        event_manager.raiseCustomMsg("stopChannelMediaRelay failed");
      });
  }

  startChannelMediaRelay_MC(
    srcChannelName,
    srcToken,
    srcUid,
    destChannelName,
    destToken,
    destUid,
    destCount
  ) {
    if (this.client != undefined) {
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

      this.client
        .startChannelMediaRelay(configuration)
        .then(() => {
          event_manager.raiseCustomMsg("startChannelMediaRelay success");
        })
        .catch((e) => {
          event_manager.raiseCustomMsg("startChannelMediaRelay failed");
        });
    }
  }

  SetLiveTranscoding(
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

    // .equals property throws error at runtime.
    if (StrWatermarkRtcImageUrl == "") {
      StrWatermarkRtcImageUrl = DEFAULT_IMAGE_URL_TRANSCODING;
    }

    if (StrBackgroundImageRtcImageUrl == "") {
      StrBackgroundImageRtcImageUrl = DEFAULT_BG_URL_TRANSCODING;
    }

    //  configuration of pushing stream to cdn
    this.liveTranscodingConfig = {
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
      // userConfigExtraInfo: {}, // Can be added at later stage. Information on documentation is missing.
      backgroundColor: BackgroundColor,
      watermark: {
        url: StrWatermarkRtcImageUrl,
        x: WatermarkRtcImageX,
        y: WatermarkRtcImageY,
        width: WatermarkRtcImageWidth == 0 ? 200 : WatermarkRtcImageWidth,
        height: WatermarkRtcImageHeight == 0 ? 200 : WatermarkRtcImageHeight,
      },
      backgroundImage: {
        url: StrBackgroundImageRtcImageUrl,
        x: BackgroundImageRtcImageX,
        y: BackgroundImageRtcImageY,
        width:
          BackgroundImageRtcImageWidth == 0
            ? 1080
            : BackgroundImageRtcImageWidth,
        height:
          BackgroundImageRtcImageHeight == 0
            ? 520
            : BackgroundImageRtcImageHeight,
      },
      transcodingUsers,
    };
  }

  async addPublishStreamUrl2_MC(channel, transcodingUrl, transcodingEnabled) {
    if (!transcodingUrl) {
      console.error("you should input liveStreaming URL");
      return;
    }
    try {
      // To monitor errors in the middle of the push, please refer to the API documentation for the list of error codes
      this.client.on("live-streaming-error", (url, err) => {
        console.error("url", url, "live streaming error!", err.code);
        event_manager.raiseHandleChannelError(err.code, "live-streaming-error");
      });

      // set live streaming transcode configuration,
      await this.client.setLiveTranscoding(this.liveTranscodingConfig);
      // then start live streaming.
      await this.client.startLiveStreaming(transcodingUrl, transcodingEnabled);
      event_manager.raiseChannelOnTranscodingUpdated(this.channelId);
    } catch (error) {
      console.error("live streaming error:", error.message);
      event_manager.raiseHandleChannelError(500, error.message);
    }
  }

  async removePublishStreamUrl2_MC(channel, url) {
    this.client.stopLiveStreaming(url).then(() => {
      event_manager.raiseChannelOnTranscodingUpdated(this.channelId);
    });
  }

  muteAllRemoteAudioStreams(mute) {
    Object.keys(this.remoteUsers).forEach((uid) => {
      if (mute == true) {
        this.unsubscribe(this.remoteUsers[uid], "audio");
        this.remoteUsersAudioMuted[uid] = true;
      } else {
        this.subscribe_mv(this.remoteUsers[uid], "audio");
        this.remoteUsersAudioMuted[uid] = false;
      }
    });
    this.muteAllAudio = mute;
  }

  async unsubscribe(user, mediaType) {
    try {
      await this.client.unsubscribe(user, mediaType);
    } catch (error) {
      console.log("unsubscribe error ", error);
    }
  }

  async subscribe_mv(user, mediaType) {
    try {
      await this.client.subscribe(user, mediaType);
      if (mediaType === "video") {
        const player = $(`
        <div id="player-wrapper-${user.uid}">
        <p class="player-name">remoteUser(${user.uid})</p>
        <div id="player-${user.uid}" class="player"></div>
        </div>
      `);
        $("#remote-playerlist").append(player);
        user.videoTrack.play(`player-${user.uid}`);
      } else if (mediaType === "audio") {
        user.audioTrack.play();
      }
    } catch (error) {
      console.log("subscribe error ", error);
      event_manager.raiseHandleChannelError(this.channelId, error.code, error.message);
    }
  }

  muteAllRemoteVideoStreams(mute) {
    Object.keys(this.remoteUsers).forEach((uid) => {
      if (mute == true) {
        this.unsubscribe(this.remoteUsers[uid], "video");
        this.remoteUsersVideoMuted[uid] = true;
      } else {
        if (this.remoteUsers[uid].hasVideo) {
          this.subscribe_mv(this.remoteUsers[uid], "video");
        }
        this.remoteUsersVideoMuted[uid] = false;
      }
    });
    this.muteAllVideo = mute;
  }

  // Must/Unmute local audio (mic)
  async muteLocalAudioStream(mute) {
    if (mute) {
      if (localTracks.audioTrack) {
        await this.client.unpublish(localTracks.audioTrack);
      }
    } else {
      if (localTracks.audioTrack) {
        await this.client.publish(localTracks.audioTrack);
      }
    }
    this.audioEnabled = !mute;
  }

  // Stops/Resumes sending the local video stream.
  async muteLocalVideoStream(mute) {
    if (this.client && !this.is_screensharing) {
      if (mute) {
        if (localTracks.videoTrack) {
         await localTracks.videoTrack.setMuted(true);
        }
        
      } else {
        
        if (localTracks.videoTrack) {
          await localTracks.videoTrack.setMuted(false);
        }
        
        await localTracks.videoTrack.setMuted(false);
      }
      this.videoEnabled = !mute;
    }
  }
  muteRemoteAudioStream(uid, mute) {
    Object.keys(this.remoteUsers).forEach((uid2) => {

      if (uid2 == uid) {
        if (mute == true) {
          this.unsubscribe(this.remoteUsers[uid], "audio");
          this.remoteUsersAudioMuted[uid] = true;
        } else {
          this.subscribe_mv(this.remoteUsers[uid], "audio");
          this.remoteUsersAudioMuted[uid] = false;
        }
      }
    });
  }

  muteRemoteVideoStream(uid, mute) {
    Object.keys(this.remoteUsers).forEach((uid2) => {
      if (uid2 == uid) {
        if (mute == true) {
            this.remoteUsersVideoMuted[uid] = true;
            this.unsubscribe(this.remoteUsers[uid], "video");
        } else {
            this.remoteUsersVideoMuted[uid] = false;
            if(this.remoteUsers[uid].hasVideo){
              this.subscribe_mv(this.remoteUsers[uid], "video");
            }
        }
      }
    });
  }

  adjustUserPlaybackSignalVolume(uid, volume) {
    Object.keys(this.remoteUsers).forEach((uid_in) => {
      if (uid_in == uid) {
        var audioTrack = this.remoteUsers[uid_in]._audioTrack;
        audioTrack?.setVolume(volume);
      }
    });
  }

  setRemoteDefaultVideoStreamType(streamType) {
    Object.keys(this.remoteUsers).forEach((uid2) => {
      this.setRemoteVideoStreamType(uid2, streamType);
    });
  }

  async setRemoteVideoStreamType(usersIdStream, streamType) {
    try {
      await this.client.setRemoteVideoStreamType(usersIdStream, streamType);
    } catch (error) {
      console.log("error from SetRemoteVideoSTreamType", error);
    }
  }

  async startScreenCapture(enableAudio) {
    var enableAudioStr = enableAudio ? "auto" : "disable";
    this.is_screensharing = true;
    var screenShareTrack = null;
    this.tempLocalTracks = localTracks;
    screenShareTrack = await AgoraRTC.createScreenVideoTrack({}, enableAudioStr).catch(error => {
      event_manager.raiseScreenShareCanceled_MC(this.options.channel, this.options.uid);
      this.is_screensharing = false;
    });
    if (this.is_screensharing) {
      if (Array.isArray(screenShareTrack)) {
        if (localTracks.videoTrack) {
          localTracks.videoTrack.stop();
          localTracks.videoTrack.close();
          await this.client.unpublish(localTracks.videoTrack);
        }
        [localTracks.videoTrack] = screenShareTrack;
        localTracks.videoTrack.on("track-ended", this.handleStopScreenShare.bind());
        localTracks.videoTrack.play("local-player");
        this.tempLocalTracks = screenShareTrack;
        await this.client.publish(this.tempLocalTracks[0]);
        await this.client.publish(this.tempLocalTracks[1]);
        this.enableLoopbackAudio = true;
        event_manager.raiseScreenShareStarted_MC(this.options.channel, this.options.uid);
      } else {
        if (localTracks.videoTrack) {
          localTracks.videoTrack.stop();
          localTracks.videoTrack.close();
          await this.client.unpublish(localTracks.videoTrack);
        }
        localTracks.videoTrack = screenShareTrack;
        localTracks.videoTrack.on("track-ended", this.handleStopScreenShare.bind());
        localTracks.videoTrack.play("local-player");
        await this.client.publish(localTracks.videoTrack);
        event_manager.raiseScreenShareStarted_MC(this.options.channel, this.options.uid);
      }
    }
  }

  // Stop screen sharing.
  async stopScreenCapture() {
    if (this.is_screensharing) {
      if (localTracks.videoTrack) {
        localTracks.videoTrack.stop();
        localTracks.videoTrack.close();
        await this.client.unpublish(localTracks.videoTrack);
      }
      this.is_screensharing = false;
      this.enableLoopbackAudio = false;
      if (this.tempLocalTracks != null) {
        for (var i = 0; i < this.tempLocalTracks.length; i++) {
          this.tempLocalTracks[i].stop();
          this.tempLocalTracks[i].close();
          await this.client.unpublish(this.tempLocalTracks[i]);
        }
      }
      this.tempLocalTracks = null;

      if (this.videoEnabled) {
        [localTracks.videoTrack] = await Promise.all([
          AgoraRTC.createCameraVideoTrack().catch(
            async e => { 
              event_manager.raiseHandleChannelError(this.options.channel, e.code, e.message); 
            }),
        ]);
      if (localTracks.videoTrack != null) {
        localTracks.videoTrack.play("local-player");
        await this.client.publish(localTracks.videoTrack);
      }
      }
      event_manager.raiseScreenShareStopped_MC(this.options.channel, this.options.uid);
    }
  }

  async startNewScreenCaptureForWeb2(uid, enableAudio) {
    var screenShareTrack = null;
    var enableAudioStr = enableAudio ? "auto" : "disable";
    if (!this.is_screensharing) {
      this.screenShareClient = AgoraRTC.createClient({ mode: "rtc", codec: "vp8" });
      AgoraRTC.createScreenVideoTrack({
        encoderConfig: "1080p_1", optimizationMode: "detail"
      }, enableAudioStr
      ).then(localVideoTrack => {
        if (Array.isArray(localVideoTrack)) {
          this.is_screensharing = true;
          screenShareTrack = localVideoTrack;
          screenShareTrack[0].on("track-ended", this.handleStopNewScreenShare.bind());
          this.enableLoopbackAudio = enableAudio;
          this.tempLocalTracks = screenShareTrack;
          var screenShareUID = uid + this.client.uid;
            if(this.remoteUsers && this.remoteUsers[screenShareUID] !== undefined){
              screenShareTrack = null;
              event_manager.raiseScreenShareCanceled_MC(this.options.channel, screenShareUID);
              return;
            }
          this.screenShareClient.join(this.options.appid, this.options.channel, null, screenShareUID).then(u => {
            this.screenShareClient.publish(screenShareTrack);
            event_manager.raiseScreenShareStarted_MC(this.options.channel, screenShareUID);

          });
        } else {
          this.is_screensharing = true;
          screenShareTrack = localVideoTrack;
          screenShareTrack.on("track-ended", this.handleStopNewScreenShare.bind());
          this.enableLoopbackAudio = enableAudio;
          this.tempLocalTracks = screenShareTrack;
          var screenShareUID = uid + this.client.uid;
            if(this.remoteUsers && this.remoteUsers[screenShareUID] !== undefined){
              screenShareTrack = null;
              event_manager.raiseScreenShareCanceled_MC(this.options.channel, screenShareUID);
              return;
            }
          this.screenShareClient.join(this.options.appid, this.options.channel, null, screenShareUID).then(u => {
            this.screenShareClient.publish(screenShareTrack);
            event_manager.raiseScreenShareStarted_MC(this.options.channel, screenShareUID);
          });
        }
      }).catch(error => {
        event_manager.raiseScreenShareCanceled_MC(this.options.channel, this.options.uid);
      });
    } else {
      window.alert("SCREEN IS ALREADY BEING SHARED!\nPlease stop current ScreenShare before\nstarting a new one.");
    }
  }

  async stopNewScreenCaptureForWeb2() {
    if (this.is_screensharing) {
      if (this.tempLocalTracks !== null) {
        if (Array.isArray(this.tempLocalTracks)) {
          for (var i = 0; i < this.tempLocalTracks.length; i++) {
            this.tempLocalTracks[i].stop();
            this.tempLocalTracks[i].close();
            this.screenShareClient.unpublish(this.tempLocalTracks[i]);
          }
        } else {
          console.log(this.tempLocalTracks);
          this.tempLocalTracks.stop();
          this.tempLocalTracks.close();
          this.screenShareClient.unpublish(this.tempLocalTracks);
        }
      }

      this.screenShareClient.leave();
      this.is_screensharing = false;
      if (localTracks.audioTrack) {
        this.client.publish(localTracks.audioTrack);
      }
      event_manager.raiseScreenShareStopped_MC(this.options.channel, this.options.uid);
    }
  }

  setRemoteUserPriority(uid, userPriority) {
    if (userPriority == 50 || userPriority == 0)
      this.client.setRemoteVideoStreamType(uid, 0);
    else if (userPriority == 100 || userPriority == 1)
      this.client.setRemoteVideoStreamType(uid, 1);
  }

  setEncryptionMode(mode) {
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

  enableEncryption2_mc(enable, encryptionKey_Str, encryptionMode) {
    var modestr = "none";

    if (enable) {
      if (encryptionMode == 1) {
        modestr = "aes-128-xts";
      } else if (encryptionMode == 2) {
        modestr = "aes-256-xts";
      } else if (encryptionMode == 3) {
        modestr = "aes-128-ecb";
      } else if (encryptionMode == 4) {
        modestr = "sm4-128-ecb";
      }
    } else {
      modestr = "none";
    }
    if (this.client) {
      this.client.setEncryptionConfig(modestr, encryptionKey_Str);
    }
  }

  async setClientRole2_MC(role, optionLevel) {
    if (this.client) {
      // host
      if (role === 1) {
        await this.client.setClientRole("host", optionLevel);
        if (this.channelId === "") {
          // called before join channel
          // do nothing, just mark it at the end
        } else {
          if (this.client_role == 2) {
            // called after join channel
            // and previosly it is audience, default to publish
            await this.publish();
            event_manager.raiseChannelOnClientRoleChanged(this.options.channel, "2", "1");
          }
        }
      } else if (role === 2) {
        // audience
        if (this.client_role != role) {
          await this.unpublish();
        }
        await this.client.setClientRole("audience", optionLevel);
        event_manager.raiseChannelOnClientRoleChanged(this.options.channel, "1", "2");
      }
      this.client_role = role;
    }
  }

  enableAudioVolumeIndicator2() {
    if (this.client) {
      this.client.enableAudioVolumeIndicator();
    } else {
      this.volumeIndicationOn = true;
    }
  }

  async getRemoteVideoStatsMC(uid) {
    let Client = this.client;
    setTimeout(function () {
      var stats = Client.getRemoteVideoStats();
      console.log("agora channel remote video: " + stats);
      if (stats[uid]) {
        const width = stats[uid].receiveResolutionWidth;
        const height = stats[uid].receiveResolutionHeight;
        event_manager.raiseOnClientVideoSizeChanged(uid, width, height);
      }
    }, 2000);
  }


  async enableVirtualBackground(){
    console.log("agora channel working");
    getProcessorInstance(localTracks.videoTrack);
  }
  
  async setVirtualBackgroundBlur(blurDegree){
    setBackgroundBlurring(localTracks.videoTrack, blurDegree);
  }
  
  async setVirtualBackgroundColor(hexColor){
    setBackgroundColor(localTracks.videoTrack, hexColor);
  }
  
  async setVirtualBackgroundImage(imgFile){
    setBackgroundImage(localTracks.videoTrack, imgFile);
  }
  
  async setVirtualBackgroundVideo(videoFile){
    setBackgroundVideo(localTracks.videoTrack, videoFile);
  }

  async enableSpatialAudio(enabled){
    this.client.processor = window.joinSpatialAudioChannel(enabled, this.options.appid, this.options.token, this.options.channel);
  }

  async setRemoteUserSpatialAudioParams(uid, azimuth, elevation, distance, orientation, attenuation, blur, airAbsorb){
    window.updateSpatialAzimuth(azimuth);
    window.updateSpatialElevation(elevation);
    window.updateSpatialDistance(distance);
    window.updateSpatialOrientation(orientation);
    window.updateSpatialAttenuation(attenuation);
    window.updateSpatialBlur(blur);
    window.updateSpatialAirAbsorb(airAbsorb);
  }

}
