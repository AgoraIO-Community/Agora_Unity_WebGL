var WglWrapper = function () {
  var self = {
    client: undefined,
    statsInterval: "",
    clientStatsStored: undefined,
    localStatsStored: "",
    remoteTracksStatsStored: "",
    connectionState: "",
    savedCameras: Array(),
    savedMicrophones: Array(),
    savedPlayBackDevices: Array(),
    savedSettings: {
      localAudioTrackVolume: 100,
      playbackVolume: 100,
      recordingSignalVolume: 100,
      audioMixingPlayoutVolume: 100,
      audioMixingPublishVolume: 100,
      mirrorOptions: { fit: "cover", mirror: true },
      watermarkOn: false,
    },
  };

  self.saveCameras = function (cameras) {
    self.savedCameras = Array();
    cameras.forEach((cam) => {
      self.savedCameras.push(cam);
    });
  };

  self.saveMicrophones = function (cameras) {
    self.savedMicrophones = Array();
    cameras.forEach((cam) => {
      self.savedMicrophones.push(cam);
    });
  };

  self.savePlayBackDevices = function (cameras) {
    self.savedPlayBackDevices = Array();
    cameras.forEach((cam) => {
      self.savedPlayBackDevices.push(cam);
    });
  };

  self.getCameraDeviceIdFromDeviceName = function (deviceName) {
    var retValue = "";
    self.savedCameras.forEach((cam) => {
      if (cam.label == deviceName) {
        retValue = cam.deviceId;
      }
    });
    return retValue;
  };

  self.getMicrophoneDeviceIdFromDeviceName = function (deviceName) {
    var retValue = "";
    self.savedMicrophones.forEach((cam) => {
      if (cam.label == deviceName) {
        retValue = cam.deviceId;
      }
    });
    return retValue;
  };

  self.getSecondTestCamera = function () {
    return self.savedCameras[1];
  };

  self.setup = function (client) {
    self.client = client;
  };

  self.getConnectionState = function () {
    return self.connectionState;
  };

  self.initStats = function () {
    this.statsInterval = setInterval(this.flushStats, 1000);
  };

  self.destructStats = function () {
    clearInterval(this.statsInterval);
  };

  self.buildStats = function () {
    if (self.clientStatsStored == undefined) {
      return "";
    }
    dataBuilder.add("UserCount", self.clientStatsStored.UserCount);
    dataBuilder.add("Duration", self.clientStatsStored.Duration);
    dataBuilder.add("RecvBitrate", self.clientStatsStored.RecvBitrate);
    dataBuilder.add("SendBitrate", self.clientStatsStored.SendBitrate);
    dataBuilder.add("RecvBytes", self.clientStatsStored.RecvBytes);
    dataBuilder.add("SendBytes", self.clientStatsStored.SendBytes);
    // i fixed it, what happened?
    if (self.clientStatsStored.OutgoingAvailableBandwidth) {
      dataBuilder.add(
        "OutgoingAvailableBandwidth",
        self.clientStatsStored.OutgoingAvailableBandwidth.toFixed(3)
      );
    } else {
      dataBuilder.add("OutgoingAvailableBandwidth", 0);
    }

    dataBuilder.add("RTT", self.clientStatsStored.RTT);

    dataBuilder.add("a_sendBitrate", self.localStatsStored.audio.sendBitrate);
    dataBuilder.add("a_sendBytes", self.localStatsStored.audio.sendBytes);
    dataBuilder.add("a_sendPackets", self.localStatsStored.audio.sendPackets);
    dataBuilder.add(
      "a_sendPacketsLost",
      self.localStatsStored.audio.sendPacketsLost
    );

    if (self.localStatsStored.video) {
      dataBuilder.add(
        "v_captureResolutionHeight",
        self.localStatsStored.video.captureResolutionHeight
      );
      dataBuilder.add(
        "v_captureResolutionWidth",
        self.localStatsStored.video.captureResolutionWidth
      );

      dataBuilder.add(
        "v_sendResolutionHeight",
        self.localStatsStored.video.sendResolutionHeight
      );
      dataBuilder.add(
        "v_sendResolutionWidth",
        self.localStatsStored.video.sendResolutionWidth
      );
      dataBuilder.add(
        "v_encodeDelay",
        Number(self.localStatsStored.video.encodeDelay).toFixed(2)
      );

      dataBuilder.add("v_sendBitrate", self.localStatsStored.video.sendBitrate);
      dataBuilder.add("v_sendBytes", self.localStatsStored.video.sendBytes);
      dataBuilder.add("v_sendPackets", self.localStatsStored.video.sendPackets);
      dataBuilder.add(
        "v_sendPacketsLost",
        self.localStatsStored.video.sendPacketsLost
      );
      dataBuilder.add(
        "v_totalDuration",
        self.localStatsStored.video.totalDuration
      );
      dataBuilder.add(
        "v_totalFreezeTime",
        self.localStatsStored.video.totalFreezeTime
      );
    }

    Object.keys(self.remoteTracksStatsStored).forEach((uid) => {
      var video = self.remoteTracksStatsStored[uid].video;
      var audio = self.remoteTracksStatsStored[uid].audio;
      var num = undefined;
      if (audio) {
        num =  Number(audio.receiveDelay) ?? 0;
        dataBuilder.add(
          "ru_" + uid + "_" + "a_receiveDelay",
          num.toFixed(2)
        );
        num =  Number(audio.receiveBytes) ?? 0;
        dataBuilder.add("ru_" + uid + "_" + "a_receiveBytes", num.toFixed(3));
        num =  Number(audio.receivePackets) ?? 0;
        dataBuilder.add(
          "ru_" + uid + "_" + "a_receivePackets",
          num.toFixed(3)
        );
        num =  Number(audio.receivePacketsLost) ?? 0;
        dataBuilder.add(
          "ru_" + uid + "_" + "a_receivePacketsLost",
          num.toFixed(3)
        );
        num =  Number(audio.packetLossRate) ?? 0;
        dataBuilder.add(
          "ru_" + uid + "_" + "a_",
          num.toFixed(3)
        );
      }

      if (video) {
        num =  Number(video.receiveDelay) ?? 0;
        dataBuilder.add(
          "ru_" + uid + "_" + "v_receiveDelay",
          num.toFixed(2)
        );

        num =  Number(video.receiveResolutionHeight) ?? 0;
        dataBuilder.add(
          "ru_" + uid + "_" + "v_receiveResolutionHeight",
          num.toFixed(2)
        );

        num =  Number(video.receiveResolutionWidth) ?? 0;
        dataBuilder.add(
          "ru_" + uid + "_" + "v_receiveResolutionWidth",
          num.toFixed(2)
        );

        num =  Number(video.receiveBitrate) ?? 0;
        dataBuilder.add(
          "ru_" + uid + "_" + "v_receiveBitrate",
          num.toFixed(2)
        );

        num =  Number(video.receiveBytes) ?? 0;
        dataBuilder.add("ru_" + uid + "_" + "v_receiveBytes", num.toFixed(2));

        num =  Number(video.receivePackets) ?? 0;
        dataBuilder.add(
          "ru_" + uid + "_" + "v_receivePackets",
          num.toFixed(2)
        );

        num =  Number(video.receivePacketsLost) ?? 0;
        dataBuilder.add(
          "ru_" + uid + "_" + "v_receivePacketsLost",
          num.toFixed(2)
        );

        num =  Number(video.receivePacketsLost) ?? 0;
        dataBuilder.add(
          `ru_${uid}_v_receivePacketsLost`,
          num.toFixed(3)
        );

        num =  Number(video.totalDuration) ?? 0;
        dataBuilder.add(`ru_${uid}_v_totalDuration`, num.toFixed(2));

        num =  Number(video.totalFreezeTime) ?? 0;
        dataBuilder.add(`ru_${uid}_v_totalFreezeTime`, num.toFixed(2));

        num =  Number(video.freezeRate) ?? 0;
        dataBuilder.add(
          `ru_${uid}_v_freezeRate`,
          num.toFixed(3)
        );
      }
    });

    return dataBuilder.build();
  };

  self.flushStats = function () {
    self.connectionState = self.client.connectionState;

    const clientStats = self.client.getRTCStats();
    self.clientStatsStored = clientStats;

    const localStats = {
      video: self.client.getLocalVideoStats(),
      audio: self.client.getLocalAudioStats(),
    };
    self.localStatsStored = localStats;

    self.remoteTracksStatsStored = Array();

    Object.keys(remoteUsers).forEach((uid) => {
      const remoteTracksStats = {
        video: self.client.getRemoteVideoStats()[uid],
        audio: self.client.getRemoteAudioStats()[uid],
      };
      self.remoteTracksStatsStored[uid] = remoteTracksStats;
    });
  };

  self.log = function (msg) {};

  return self;
};
