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
    this.is_publishing = false;
    this.remoteUsers = {};
    this.channelId = "";
    this.mode = "rtc";
    this.client_role = 1; // default is host, 2 is audience
    this.liveTranscodingConfig = null;
  }

  setOptions(channelkey, channelName, uid) {
    this.options.token = channelkey;
    this.options.channel = channelName;
    this.options.uid = uid;
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
      return true;
    }
  }

  createClient_Live() {
    if (this.client == undefined) {
      this.mode = "live";
      this.client = AgoraRTC.createClient({ mode: this.mode, codec: "vp8" });
      this.options.appid = client_manager.getMainAppId();
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
    await this.subscribe_remoteuser(user, mediaType);
  }

  handleUserLeft(user) {
    const id = user.uid;
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
      if (localTracks.videoTrack != undefined) {
        await this.client.publish(localTracks.videoTrack);
      }
      if (localTracks.audioTrack != undefined) {
        await this.client.publish(localTracks.audioTrack);
      }
      this.is_publishing = true;
      event_manager.raiseCustomMsg("Publish Success");
    }
  }

  async renewToken2_mc(token_str) {
    if (this.client) {
      await this.client.renewToken(token_str);
    }
  }

  async unpublish() {
    if (this.is_publishing == true) {
      if (localTracks.videoTrack != undefined) {
        await this.client.unpublish(localTracks.videoTrack);
      }
      if (localTracks.audioTrack != undefined) {
        await this.client.unpublish(localTracks.audioTrack);
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

  async joinChannelWithUserAccount_MC(
    token_str,
    userAccount_str,
    autoSubscribeAudio,
    autoSubscribeVideo
  ) {
    this.options.token = token_str;
    this.options.channel = this.channelId;

    // add event listener to play remote tracks when remote user publishs.
    this.client.on("user-joined", this.handleUserJoined.bind(this));
    this.client.on("user-published", this.handleUserPublished.bind(this));
    this.client.on("user-unpublished", this.handleUserUnpublished.bind(this));
    this.client.on("user-left", this.handleUserLeft.bind(this));

    if (localTracks.videoTrack == undefined) {
      [localTracks.videoTrack] = await Promise.all([
        AgoraRTC.createCameraVideoTrack(),
      ]);
    }

    if (localTracks.audioTrack == undefined) {
      [localTracks.audioTrack] = await Promise.all([
        AgoraRTC.createMicrophoneAudioTrack(),
      ]);
    }

    [this.options.uid] = await Promise.all([
      this.client.join(
        this.options.appid,
        this.options.channel,
        this.options.token || null,
        userAccount_str
      ),
    ]);

    if (localTracks.videoTrack != undefined) {
      localTracks.videoTrack.play("local-player");
    }
    multiclient_connections++;
    event_manager.raiseJoinChannelSuccess_MC(
      this.options.uid.toString(),
      this.options.channel
    );
    event_manager.raiseCustomMsg("Channel Joined With user Account");
  }

  async joinChannel() {
    // add event listener to play remote tracks when remote user publishs.
    this.client.on("user-joined", this.handleUserJoined.bind(this));
    this.client.on("user-published", this.handleUserPublished.bind(this));
    this.client.on("user-unpublished", this.handleUserUnpublished.bind(this));
    this.client.on("user-left", this.handleUserLeft.bind(this));

    if (localTracks.videoTrack == undefined) {
      [localTracks.videoTrack] = await Promise.all([
        AgoraRTC.createCameraVideoTrack(),
      ]);
    }

    if (localTracks.audioTrack == undefined) {
      [localTracks.audioTrack] = await Promise.all([
        AgoraRTC.createMicrophoneAudioTrack(),
      ]);
    }

    [this.options.uid] = await Promise.all([
      this.client.join(
        this.options.appid,
        this.options.channel,
        this.options.token || null
      ),
    ]);

    if (localTracks.videoTrack != undefined) {
      localTracks.videoTrack.play("local-player");
    }

    event_manager.raiseJoinChannelSuccess_MC(
      this.options.uid.toString(),
      this.options.channel
    );
    multiclient_connections++;
    event_manager.raiseCustomMsg("Channel Joined");
  }

  async leave() {
    _logger("leaving in agorachannel");
    if (multiclient_connections <= 1) {
      if (localTracks.videoTrack != undefined) {
        localTracks.videoTrack.stop();
        localTracks.videoTrack.close();
        localTracks.videoTrack = undefined;
      }
      if (localTracks.audioTrack != undefined) {
        localTracks.audioTrack.stop();
        localTracks.audioTrack.close();
        localTracks.audioTrack = undefined;
      }
    }

    this.is_publishing = false;
    // remove remote users and player views
    this.remoteUsers = {};
    multiclient_connections--;
    await this.client.leave();
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
      } else {
        this.subscribe_mv(this.remoteUsers[uid], "audio");
      }
    });
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
    }
  }

  muteAllRemoteVideoStreams(mute) {
    Object.keys(this.remoteUsers).forEach((uid) => {
      if (mute == true) {
        this.unsubscribe(this.remoteUsers[uid], "video");
      } else {
        this.subscribe_mv(this.remoteUsers[uid], "video");
      }
    });
  }

  muteRemoteAudioStream(uid, mute) {
    Object.keys(this.remoteUsers).forEach((uid2) => {
      if (uid2 == uid) {
        if (mute == true) {
          this.unsubscribe(this.remoteUsers[uid], "audio");
        } else {
          this.subscribe_mv(this.remoteUsers[uid], "audio");
        }
      }
    });
  }

  muteRemoteVideoStream(uid, mute) {
    Object.keys(this.remoteUsers).forEach((uid2) => {
      if (uid2 == uid) {
        if (mute == true) {
          this.unsubscribe(this.remoteUsers[uid], "video");
        } else {
          this.subscribe_mv(this.remoteUsers[uid], "video");
        }
      }
    });
  }

  adjustUserPlaybackSignalVolume(uid, volume) {
    Object.keys(this.remoteUsers).forEach((uid_in) => {
      if (uid_in == uid) {
        var audioTrack = this.remoteUsers[uid_in]._audioTrack;
        audioTrack.setVolume(volume);
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

  setClientRole2_MC(role) {
    if (this.client) {
      if (role === 1) {
        this.client.setClientRole("host", function (e) {
          if (!e) {
            this.client_role = 1;
          }
        });
      } else if (role === 2) {
        this.client.setClientRole("audience", function (e) {
          if (!e) {
            this.client_role = 2;
          }
        });
      }
    }
  }
}
