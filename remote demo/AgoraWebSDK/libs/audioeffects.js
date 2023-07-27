var AudioEffects = function () {
  var self = {
    client: null,
    _effects: Array(),
    effectsDataList: Array(),
    effectsTrackList: Array(),
    _globalEFVolume: 100,
  };

  var AudioEffect = {
    soundId: 0,
    filePath: "",
    loopCount: 1,
    pitch: 0,
    pan: 0,
    gain: 0,
    publish: false,
    preload: false,
  };

  var audioBufferConfig = {
    loop: false,
    cycle: 1,
    startPlayTime: 0,
  };

  var bufferSourceAudioTrackInitConfig = {
    cacheOnlineFile: false,
    encoderConfig: "music_standard",
    source: "audio.mp3",
  };

  self.initialize = function (client) {
    self.effectsDataList = Array();
    self.effectsTrackList = Array();
    self.client = client;
  };

  self._unloadEffect = function (soundId) {
    if (typeof self._effects[soundId] === "undefined") {
      // if no such file exists
      return undefined;
    } else {
      self._print();
      unpublish(self._effects[soundId]);
      self._effects.hasOwnProperty(soundId); // true
      delete self._effects[soundId];
      self._print();
    }
  };

  async function unpublish(effectTrack) {
    await self.client.unpublish(effectTrack);
    effectTrack.stopProcessAudioBuffer();
    effectTrack.stop();
  }

  self._pauseEffect = function (a_soundId) {
    if (typeof self._effects[a_soundId] === "undefined") {
      return undefined;
    } else {
      self._effects[a_soundId].pauseProcessAudioBuffer();
    }
  };

  self._resumeEffect = function (a_soundId) {
    if (typeof self._effects[a_soundId] === "undefined") {
      return undefined;
    } else {
      self._effects[a_soundId].resumeProcessAudioBuffer();
    }
  };

  self._pauseAllEffects = function () {
    self._effects.forEach((effect) => {
      effect.pauseProcessAudioBuffer();
    });
  };

  self._resumeAllEffects = function () {
    self._effects.forEach((effect) => {
      effect.resumeProcessAudioBuffer();
    });
  };

  self._setEffectsVolume = function (volume) {
    self._globalEFVolume = volume;
    self._effects.forEach((effect) => {
      effect.setVolume(volume);
    });
  };

  self._PlayEffect = function (a_soundId, filePath, loopCount, publish) {
    if (typeof self._effects[a_soundId] === "undefined") {
      _createEffect(a_soundId, filePath, loopCount, publish);
    } else {
      _playEffectSound(a_soundId, publish);
    }
  };

  self._print = function () {
    /*self._effects.forEach((effect) => {
      console.log(effect);
    });*/
  };

  self._stopEffect = function (a_soundId) {
    if (typeof self._effects[a_soundId] === "undefined") {
      return undefined;
    } else {
      self._effects[a_soundId].stop();
      self._effects[a_soundId].stopProcessAudioBuffer();
    }
  };

  self._getEffectsVolume = function () {
    return self._globalEFVolume;
  };

  self._setVolumeOfEffect = function (a_soundId, volume) {
    if (typeof self._effects[a_soundId] === "undefined") {
      return undefined;
    } else {
      self._effects[a_soundId].setVolume(volume);
    }
  };

  self._stopAllEffects = function () {
    self._effects.forEach((effect) => {
      if (effect != undefined) {
        effect.stop();
        effect.stopProcessAudioBuffer();
      }
    });
  };

  async function _createEffect(a_soundId, filePath, loopCount, publish) {
    try {
      bufferSourceAudioTrackInitConfig.source = filePath;

      var effectTrack = await AgoraRTC.createBufferSourceAudioTrack(
        bufferSourceAudioTrackInitConfig
      );

      audioBufferConfig.cycle = loopCount;

      if (loopCount == -1) audioBufferConfig.loop = true;

      effectTrack.startProcessAudioBuffer({ audioBufferConfig });

      self._effects[a_soundId] = effectTrack;

      _playEffectSound(a_soundId, publish);
    } catch (error) {
      //console.error("PlayEffect error:", error.message);
    }
  }

  async function _playEffectSound(a_soundId, publish) {
    if (typeof self._effects[a_soundId] === "undefined") {
      return undefined;
    }
    if (publish) {
      await self.client.publish(self._effects[a_soundId]);
      self._effects[a_soundId].startProcessAudioBuffer({ audioBufferConfig });
      self._effects[a_soundId].play();
    } else {
      //console.log("in play but publish is false");
    }
  }

  return self;
};
