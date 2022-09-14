using System.Runtime.InteropServices;
using System;
using AOT;

namespace agora_gaming_rtc
{
    public class IRtcEngineNative
    {
        /**
        EngineEvent is only for engine, not for user,Please do not call this function.
        */
        protected delegate void EngineEventOnCaptureVideoFrame(int videoFrameType, int width, int height, int yStride, IntPtr yBuffer, int rotation, long renderTimeMs);

        protected delegate void EngineEventOnRenderVideoFrame(uint uid, int videoFrameType, int width, int height, int yStride, IntPtr yBuffer, int rotation, long renderTimeMs);

        protected delegate void EngineEventOnRecordAudioFrame(int type, int samples, int bytesPerSample, int channels, int samplesPerSec, IntPtr buffer, long renderTimeMs, int avsync_type);

        protected delegate void EngineEventOnPlaybackAudioFrame(int type, int samples, int bytesPerSample, int channels, int samplesPerSec, IntPtr buffer, long renderTimeMs, int avsync_type);

        protected delegate void EngineEventOnMixedAudioFrame(int type, int samples, int bytesPerSample, int channels, int samplesPerSec, IntPtr buffer, long renderTimeMs, int avsync_type);

        protected delegate void EngineEventOnPlaybackAudioFrameBeforeMixing(uint uid, int type, int samples, int bytesPerSample, int channels, int samplesPerSec, IntPtr buffer, long renderTimeMs, int avsync_type);

        protected delegate void EngineEventOnPullAudioFrameHandler(int type, int samples, int bytesPerSample, int channels, int samplesPerSec, IntPtr buffer, long renderTimeMs, int avsync_type);

        protected delegate void EngineEventOnLeaveChannelHandler(uint duration, uint txBytes, uint rxBytes, uint txAudioBytes, uint txVideoBytes, uint rxAudioBytes, uint rxVideoBytes, ushort txKBitRate, ushort rxKBitRate, ushort rxAudioKBitRate, ushort txAudioKBitRate, ushort rxVideoKBitRate, ushort txVideoKBitRate, ushort lastmileDelay, ushort txPacketLossRate, ushort rxPacketLossRate, uint userCount, double cpuAppUsage, double cpuTotalUsage, int gatewayRtt, double memoryAppUsageRatio, double memoryTotalUsageRatio, int memoryAppUsageInKbytes);

        protected delegate void EngineEventOnUserOfflineHandler(uint uid, int offLineReason);

        protected delegate void EngineEventOnAudioVolumeIndicationHandler(string volumeInfo, int speakerNumber, int totalVolume);

        protected delegate void EngineEventOnLocalVoicePitchInHzHandler(int pitchInHz);

        protected delegate void EngineEventOnRtcStatsHandler(uint duration, uint txBytes, uint rxBytes, uint txAudioBytes, uint txVideoBytes, uint rxAudioBytes, uint rxVideoBytes, ushort txKBitRate, ushort rxKBitRate, ushort rxAudioKBitRate, ushort txAudioKBitRate, ushort rxVideoKBitRate, ushort txVideoKBitRate, ushort lastmileDelay, ushort txPacketLossRate, ushort rxPacketLossRate, uint userCount, double cpuAppUsage, double cpuTotalUsage, int gatewayRtt, double memoryAppUsageRatio, double memoryTotalUsageRatio, int memoryAppUsageInKbytes);

        protected delegate void EngineEventOnAudioRouteChangedHandler(int route);

        protected delegate void EngineEventOnLocalVideoStatsHandler(int sentBitrate, int sentFrameRate, int encoderOutputFrameRate, int rendererOutputFrameRate, int targetBitrate, int targetFrameRate, int qualityAdaptIndication, int encodedBitrate, int encodedFrameWidth, int encodedFrameHeight, int encodedFrameCount, int codecType, ushort txPacketLossRate, int captureFrameRate, int captureBrightnessLevel);

        protected delegate void EngineEventOnRemoteVideoStatsHandler(uint uid, int delay, int width, int height, int receivedBitrate, int decoderOutputFrameRate, int rendererOutputFrameRate, int packetLossRate, int rxStreamType, int totalFrozenTime, int frozenRate, int totalActiveTime, int publishDuration);

        protected delegate void EngineEventOnRemoteAudioStatsHandler(uint uid, int quality, int networkTransportDelay, int jitterBufferDelay, int audioLossRate, int numChannels, int receivedSampleRate, int receivedBitrate, int totalFrozenTime, int frozenRate, int totalActiveTime, int publishDuration, int qoeQuality, int qualityChangedReason, int mosValue);

        protected delegate void EngineEventOnAudioDeviceVolumeChangedHandler(int deviceType, int volume, bool muted);

        protected delegate void EngineEventOnAudioMixingStateChangedHandler(int state, int errorCode);

        protected delegate void EngineEventOnRtmpStreamingStateChangedHandler(string url, int state, int errCode);

        protected delegate void EngineEventOnNetworkTypeChangedHandler(int type);

        protected delegate void EngineEventOnLastmileProbeResultHandler(int state, uint upLinkPacketLossRate, uint upLinkjitter, uint upLinkAvailableBandwidth, uint downLinkPacketLossRate, uint downLinkJitter, uint downLinkAvailableBandwidth, uint rtt);

        protected delegate void EngineEventOnUserInfoUpdatedHandler(uint uid, uint userUid, string userAccount);

        protected delegate void EngineEventOnLocalAudioStateChangedHandler(int state, int error);

        protected delegate void EngineEventOnRemoteAudioStateChangedHandler(uint uid, int state, int reason, int elapsed);

        protected delegate void EngineEventOnLocalAudioStatsHandler(int numChannels, int sentSampleRate, int sentBitrate, ushort txPacketLossRate);

        protected delegate void EngineEventOnChannelMediaRelayStateChangedHandler(int state, int code);

        protected delegate void EngineEventOnChannelMediaRelayEventHandler(int events);

        protected delegate bool EngineEventOnReceiveAudioPacketHandler(IntPtr buffer, IntPtr size);

        protected delegate bool EngineEventOnReceiveVideoPacketHandler(IntPtr buffer, IntPtr size);

        protected delegate bool EngineEventOnSendAudioPacketHandler(IntPtr buffer, IntPtr size);

        protected delegate bool EngineEventOnSendVideoPacketHandler(IntPtr buffer, IntPtr size);

        protected delegate void EngineEventOnMediaMetaDataReceived(uint uid, uint size, IntPtr buffer, long timeStampMs);

        protected delegate void EngineEventOnConnectionStateChanged(int state, int reason);

        protected delegate bool EngineEventOnReadyToSendMetadata();

        protected delegate int EngineEventOnGetMaxMetadataSize();

        protected delegate void EngineEventOnClientRoleChanged(int oldRole, int newRole);

        protected delegate void EngineEventOnRemoteVideoStateChanged(uint uid, int state, int reason, int elapsed);
        // audio and video raw data

        protected delegate void EngineEventOnLocalVideoStateChanged(int localVideoState, int error);

        protected delegate void EngineEventOnFacePositionChanged(int imageWidth, int imageHeight, int x, int y, int width, int height, int vecDistance, int numFaces);

        protected delegate void ChannelEngineEventOnLeaveChannelHandler(string channelId, uint duration, uint txBytes, uint rxBytes, uint txAudioBytes, uint txVideoBytes, uint rxAudioBytes, uint rxVideoBytes, ushort txKBitRate, ushort rxKBitRate, ushort rxAudioKBitRate, ushort txAudioKBitRate, ushort rxVideoKBitRate, ushort txVideoKBitRate, ushort lastmileDelay, ushort txPacketLossRate, ushort rxPacketLossRate, uint userCount, double cpuAppUsage, double cpuTotalUsage, int gatewayRtt, double memoryAppUsageRatio, double memoryTotalUsageRatio, int memoryAppUsageInKbytes);

        protected delegate void ChannelEngineEventOnRtcStatsHandler(string channelId, uint duration, uint txBytes, uint rxBytes, uint txAudioBytes, uint txVideoBytes, uint rxAudioBytes, uint rxVideoBytes, ushort txKBitRate, ushort rxKBitRate, ushort rxAudioKBitRate, ushort txAudioKBitRate, ushort rxVideoKBitRate, ushort txVideoKBitRate, ushort lastmileDelay, ushort txPacketLossRate, ushort rxPacketLossRate, uint userCount, double cpuAppUsage, double cpuTotalUsage, int gatewayRtt, double memoryAppUsageRatio, double memoryTotalUsageRatio, int memoryAppUsageInKbytes);

        protected delegate void ChannelEngineEventOnRemoteVideoStatsHandler(string channelId, uint uid, int delay, int width, int height, int receivedBitrate, int decoderOutputFrameRate, int rendererOutputFrameRate, int packetLossRate, int rxStreamType, int totalFrozenTime, int frozenRate, int totalActiveTime, int publishDuration);

        protected delegate void ChannelEngineEventOnRemoteAudioStatsHandler(string channelId, uint uid, int quality, int networkTransportDelay, int jitterBufferDelay, int audioLossRate, int numChannels, int receivedSampleRate, int receivedBitrate, int totalFrozenTime, int frozenRate, int totalActiveTime, int publishDuration, int qoeQuality, int qualityChangedReason, int mosValue);

        protected delegate void EngineEventOnStreamMessageHandler(uint userId, int streamId, IntPtr data, int length);

        protected delegate void EngineEventOnRequestAudioFileInfo(string filePath, int durationMs, int error);

        protected delegate void EngineEventOnRecorderStateChanged(int state, int error);

        protected delegate void EngineEventOnRecorderInfoUpdated(string fileName, uint durationMs, uint fileSize);

        protected delegate void EngineEventOnScreenCaptureInfoUpdated(string graphicsCardType, int errCode);
        #region DllImport
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        public const string MyLibName = "agoraSdkCWrapper";
#else
#if UNITY_IOS || UNITY_WEBGL
	        public const string MyLibName = "__Internal";
#else
                public const string MyLibName = "agoraSdkCWrapper";
#endif
#endif
        // standard sdk api
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int createEngine(string appId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int createEngine2(string appId, uint areaCode, string filePath, int fileSize, int level);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern bool deleteEngine();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr getSdkVersion();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int joinChannel(string channelKey, string channelName, string info, uint uid);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setLocalVoicePitch(double pitch);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteVoicePosition(uint uid, double pan, double gain);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setVoiceOnlyMode(bool enable);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int leaveChannel();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableLastmileTest();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int disableLastmileTest();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableVideo();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int disableVideo();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableLocalVideo(bool enabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableLocalAudio(bool enabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setupLocalVideo(int hwnd, int renderMode, uint uid, IntPtr priv);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setupRemoteVideo(int hwnd, int renderMode, uint uid, IntPtr priv);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setLocalRenderMode(int renderMode);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteRenderMode(uint userId, int renderMode);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setLocalVideoMirrorMode(int mirrorMode);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startPreview();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopPreview();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableAudio();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int disableAudio();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setParameters(string options);

#if UNITY_WEBGL
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setWebParametersInt(string key, int value);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setWebParametersDouble(string key, double value);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setWebParametersBool(string key, bool value);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setWebParametersString(string key, string value);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableAudioVolumeIndication2();
#endif

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr getCallId();
        // caller free the returned char * (through freeObject)
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int rate(string callId, int rating, string desc);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int complain(string callId, string desc);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setEnableSpeakerphone(bool enabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern bool isSpeakerphoneEnabled();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setDefaultAudioRoutetoSpeakerphone(bool enabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableAudioVolumeIndication(int interval, int smooth, bool report_vad);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startAudioRecording(string filePath, int quality);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startAudioRecording2(string filePath, int sampleRate, int quality);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopAudioRecording();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startAudioMixing(string filePath, bool loopback, bool replace, int cycle);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startAudioMixing2(string filePath, bool loopback, bool replace, int cycle, int startPos);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopAudioMixing();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int pauseAudioMixing();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int resumeAudioMixing();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int adjustAudioMixingVolume(int volume);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getAudioMixingDuration();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getAudioMixingDuration2(string filePath);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getAudioMixingCurrentPosition();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteLocalAudioStream(bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteAllRemoteAudioStreams(bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteRemoteAudioStream(uint uid, bool mute);

#if UNITY_WEBGL || UNITY_EDITOR
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteRemoteAudioStream_WGLM(string uid, bool mute);
#endif

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int switchCamera();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setVideoProfile(int profile, bool swapWidthAndHeight);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteLocalVideoStream(bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteAllRemoteVideoStreams(bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteRemoteVideoStream(uint uid, bool mute);

#if UNITY_WEBGL || UNITY_EDITOR
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteRemoteVideoStream_WGLM(string uid, bool mute);


        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableLogUpload();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int disableLogUpload();
#endif

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setLogFile(string filePath);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int renewToken(string token);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setChannelProfile(int profile);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setClientRole(int role);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableDualStreamMode(bool enabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setEncryptionMode(string encryptionMode);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setEncryptionSecret(string secret);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startRecordingService(string recordingKey);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopRecordingService(string recordingKey);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int refreshRecordingServiceStatus();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int createDataStream(bool reliable, bool ordered);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int createDataStream_engine(bool syncWithAudio, bool ordered);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int sendStreamMessage(int streamId, byte[] data, Int64 length);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setSpeakerphoneVolume(int volume);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int adjustRecordingSignalVolume(int volume);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int adjustPlaybackSignalVolume(int volume);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setHighQualityAudioParametersWithFullband(int fullband, int stereo, int fullBitrate);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableInEarMonitoring(bool enabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableWebSdkInteroperability(bool enabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setVideoQualityParameters(bool preferFrameRateOverImageQuality);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startEchoTest();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startEchoTest2(int intervalInSeconds);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startEchoTest3(IntPtr view, bool enableAudio, bool enableVideo, string token, string channelId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopEchoTest();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteVideoStreamType(uint uid, int streamType);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setMixedAudioFrameParameters(int sampleRate, int samplesPerCall);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setAudioMixingPosition(int pos);
        // setLogFilter: deprecated
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setLogFilter(uint filter);
        // video texture stuff (extension for gaming)
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableVideoObserver();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int disableVideoObserver();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int generateNativeTexture();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int updateTexture(int tex, IntPtr data, uint uid);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int deleteTexture(int tex);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int updateVideoRawData(IntPtr data, uint uid);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void addUserVideoInfo(uint userId, uint textureId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void removeUserVideoInfo(uint userId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setPlaybackDeviceVolume(int volume);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getEffectsVolume();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setEffectsVolume(int volume);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int playEffect(int soundId, string filePath, int loopCount, double pitch, double pan, int gain, bool publish);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int playEffect2(int soundId, string filePath, int loopCount, double pitch, double pan, int gain, bool publish, int startPos);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getEffectDuration(string filePath);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setEffectPosition(int soundId, int pos);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getEffectCurrentPosition(int soundId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopEffect(int soundId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopAllEffects();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int preloadEffect(int soundId, string filePath);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int unloadEffect(int soundId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int pauseEffect(int soundId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int pauseAllEffects();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int resumeEffect(int soundId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int resumeAllEffects();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setDefaultMuteAllRemoteAudioStreams(bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setDefaultMuteAllRemoteVideoStreams(bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void freeObject(IntPtr obj);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getConnectionState();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setAudioProfile(int audioProfile, int scenario);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setVideoEncoderConfiguration(int width, int height, int frameRate, int minFrameRate, int bitrate, int minBitrate, int orientationMode, int degradationPreference, int videoMirrorMode);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int adjustAudioMixingPlayoutVolume(int volume);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int adjustAudioMixingPublishVolume(int volume);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setVolumeOfEffect(int soundId, int volume);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setRecordingAudioFrameParameters(int sampleRate, int channel, int mode, int samplesPerCall);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setPlaybackAudioFrameParameters(int sampleRate, int channel, int mode, int samplesPerCall);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setLocalPublishFallbackOption(int option);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteSubscribeFallbackOption(int option);
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteDefaultVideoStreamType(int remoteVideoStreamType);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int addPublishStreamUrl(string url, bool transcodingEnabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int removePublishStreamUrl(string url);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr getErrorDescription(int code);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setLiveTranscoding(int width, int height, int videoBitrate, int videoFramerate, bool lowLatency, int videoGroup, int video_codec_profile, uint backgroundColor, uint userCount, string transcodingUserInfo, string transcodingExtraInfo, string metaData, string watermarkRtcImageUrl, int watermarkRtcImageX, int watermarkRtcImageY, int watermarkRtcImageWidth, int watermarkRtcImageHeight, int watermarkImageZorder, double watermarkImageAlpha, uint watermarkCount, string backgroundImageRtcImageUrl, int backgroundImageRtcImageX, int backgroundImageRtcImageY, int backgroundImageRtcImageWidth, int backgroundImageRtcImageHeight, int backgroundImageRtcImageZorder, double backgroundImageRtcImageAlpha, uint backgroundImageRtcImageCount, int audioSampleRate, int audioBitrate, int audioChannels, int audioCodecProfile, string advancedFeatures, uint advancedFeatureCount);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setAVSyncSource(string channelId, uint uid);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startRtmpStreamWithoutTranscoding(string url);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startRtmpStreamWithTranscoding(string url, int width, int height, int videoBitrate, int videoFramerate, bool lowLatency, int videoGroup, int video_codec_profile, uint backgroundColor, uint userCount, string transcodingUserInfo, string transcodingExtraInfo, string metaData, string watermarkRtcImageUrl, int watermarkRtcImageX, int watermarkRtcImageY, int watermarkRtcImageWidth, int watermarkRtcImageHeight, int watermarkImageZorder, double watermarkImageAlpha, uint watermarkCount, string backgroundImageRtcImageUrl, int backgroundImageRtcImageX, int backgroundImageRtcImageY, int backgroundImageRtcImageWidth, int backgroundImageRtcImageHeight, int backgroundImageRtcImageZorder, double backgroundImageRtcImageAlpha, uint backgroundImageRtcImageCount, int audioSampleRate, int audioBitrate, int audioChannels, int audioCodecProfile, string advancedFeatures, uint advancedFeatureCount);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int updateRtmpTranscoding(int width, int height, int videoBitrate, int videoFramerate, bool lowLatency, int videoGroup, int video_codec_profile, uint backgroundColor, uint userCount, string transcodingUserInfo, string transcodingExtraInfo, string metaData, string watermarkRtcImageUrl, int watermarkRtcImageX, int watermarkRtcImageY, int watermarkRtcImageWidth, int watermarkRtcImageHeight, int watermarkImageZorder, double watermarkImageAlpha, uint watermarkCount, string backgroundImageRtcImageUrl, int backgroundImageRtcImageX, int backgroundImageRtcImageY, int backgroundImageRtcImageWidth, int backgroundImageRtcImageHeight, int backgroundImageRtcImageZorder, double backgroundImageRtcImageAlpha, uint backgroundImageRtcImageCount, int audioSampleRate, int audioBitrate, int audioChannels, int audioCodecProfile, string advancedFeatures, uint advancedFeatureCount);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopRtmpStream(string url);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int takeSnapshot(string channel, uint uid, string filePath);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableContentInspect(bool enabled, string extraInfo, string modulesInfo, int moduleCount);

        // video manager
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern bool createAVideoDeviceManager();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int releaseAVideoDeviceManager();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startVideoDeviceTest(IntPtr hwnd);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopVideoDeviceTest();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getVideoDeviceCollectionCount();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getVideoDeviceCollectionDevice(int index, IntPtr deviceName, IntPtr deviceId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setVideoDeviceCollectionDevice(string deviceId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getCurrentVideoDevice(IntPtr deviceId);
        // audio recording device manager
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern bool creatAAudioRecordingDeviceManager();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int releaseAAudioRecordingDeviceManager();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getAudioRecordingDeviceCount();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getAudioRecordingDevice(int index, IntPtr deviceName, IntPtr deviceId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setAudioRecordingDevice(string deviceId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getAudioRecordingDefaultDevice(IntPtr deviceName, IntPtr deviceId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setAudioRecordingDeviceVolume(int volume);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getAudioRecordingDeviceVolume();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setAudioRecordingDeviceMute(bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern bool isAudioRecordingDeviceMute();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getCurrentRecordingDeviceInfo(IntPtr deviceName, IntPtr deviceId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int followSystemRecordingDevice(bool enable);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startAudioDeviceLoopbackTest(int indicationInterval);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopAudioDeviceLoopbackTest();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getCurrentRecordingDevice(IntPtr deviceId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startAudioRecordingDeviceTest(int indicationInterval);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopAudioRecordingDeviceTest();

        //audio playback device manager
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getAudioPlaybackDeviceCount();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern bool creatAAudioPlaybackDeviceManager();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int releaseAAudioPlaybackDeviceManager();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getAudioPlaybackDevice(int index, IntPtr deviceName, IntPtr deviceId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setAudioPlaybackDevice(string deviceId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getAudioPlaybackDefaultDevice(IntPtr deviceName, IntPtr deviceId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setAudioPlaybackDeviceVolume(int volume);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getAudioPlaybackDeviceVolume();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setAudioPlaybackDeviceMute(bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern bool isAudioPlaybackDeviceMute();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startAudioPlaybackDeviceTest(string testAudioFilePath);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopAudioPlaybackDeviceTest();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getCurrentPlaybackDeviceInfo(IntPtr deviceName, IntPtr deviceId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int followSystemPlaybackDevice(bool enable);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getCurrentPlaybackDevice(IntPtr deviceId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int pushVideoFrame(int type, int format, byte[] videoBuffer, int stride, int height, int cropLeft, int cropTop, int cropRight, int cropBottom, int rotation, long timestamp);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int pushVideoFrame2(int type, int format, IntPtr bufferPtr, int stride, int height, int cropLeft, int cropTop, int cropRight, int cropBottom, int rotation, long timestamp);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setExternalVideoSource(bool enable, bool useTexture);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setExternalAudioSource(bool enabled, int sampleRate, int channels);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int pushAudioFrame_(int audioFrameType, int samples, int bytesPerSample, int channels, int samplesPerSec, byte[] buffer, long renderTimeMs, int avsync_type);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int pushAudioFrame3_(int sourcePos, int audioFrameType, int samples, int bytesPerSample, int channels, int samplesPerSec, byte[] buffer, long renderTimeMs, int avsync_type);

        // [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        // protected static extern int pushAudioFrame2_(int mediaSourceType, int audioFrameType, int samples, int bytesPerSample, int channels, int samplesPerSec, byte[] buffer, long renderTimeMs, int avsync_type, bool wrap);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int registerVideoRawDataObserver();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int unRegisterVideoRawDataObserver();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int registerAudioRawDataObserver();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int unRegisterAudioRawDataObserver();
        // render
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setRenderMode(int renderMode);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getAudioMixingPlayoutVolume();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getAudioMixingPublishVolume();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setLocalVoiceChanger(int voiceChanger);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setLocalVoiceReverbPreset(int audioReverbPreset);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableSoundPositionIndication(bool enabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setLocalVoiceEqualization(int bandFrequency, int bandGain);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setLocalVoiceReverb(int reverbKey, int value);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setCameraCapturerConfiguration(int cameraCaptureConfiguration, int cameraDirection, int width, int height);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteUserPriority(uint uid, int userPriority);

#if UNITY_WEBGL || UNITY_EDITOR
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteUserPriority_WGL(string uid, int userPriority);
#endif

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setLogFileSize(uint fileSizeInKBytes);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setExternalAudioSink(bool enabled, int sampleRate, int channels);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int pullAudioFrame_(IntPtr audioBuffer, int type, int samples, int bytesPerSample, int channels, int samplesPerSec, long renderTimeMs, int avsync_type);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startLastmileProbeTest(bool probeUplink, bool probeDownlink, uint expectedUplinkBitrate, uint expectedDownlinkBitrate);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopLastmileProbeTest();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int configPublisher(int width, int height, int framerate, int bitrate, int defaultLayout, int lifecycle, bool owner, int injectStreamWidth, int injectStreamHeight, string injectStreamUrl, string publishUrl, string rawStreamUrl, string extraInfo);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int addVideoWatermark(string url, int x, int y, int width, int height);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int addVideoWatermark2(string watermarkUrl, bool visibleInPreview, int positionInLandscapeX, int positionInLandscapeY, int positionInLandscapeWidth, int positionInLandscapeHeight, int positionInPortraitX, int positionInPortraitY, int positionInPortraitWidth, int positionInPortraitHeight);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int clearVideoWatermarks();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int registerLocalUserAccount(string appId, string userAccount);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int joinChannelWithUserAccount(string token, string channelId, string userAccount);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int joinChannelWithUserAccount_engine(string token, string channelId, string userAccount, bool autoSubscribeAudio, bool autoSubscribeVideo, bool publishLocalAudio, bool publishLocalVideo);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getUserInfoByUserAccount(string userAccount);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr getUserInfoByUid(uint uid);

#if UNITY_WEBGL || UNITY_EDITOR
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr getUserInfoByUid_WGL(string uid);
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void getRemoteVideoStats_WGL();
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void getRemoteVideoStats_MC();
#endif

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setBeautyEffectOptions(bool enabled, int lighteningContrastLevel, float lighteningLevel, float smoothnessLevel, float rednessLevel, float sharpnessLevel);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setInEarMonitoringVolume(int volume);

#if UNITY_WEBGL || UNITY_EDITOR
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void startScreenCaptureForWeb(bool enableAudio);
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void startScreenCaptureForWeb2(bool enableAudio);
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void startNewScreenCaptureForWeb(uint uid, bool enableAudio);
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void stopNewScreenCaptureForWeb();
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void startNewScreenCaptureForWeb2(uint uid, bool audioEnabled);
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void stopNewScreenCaptureForWeb2();
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void stopScreenCapture2();
#endif

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startScreenCaptureByDisplayId(uint displayId, int x, int y, int width, int height, int screenCaptureVideoDimenWidth, int screenCaptureVideoDimenHeight, int screenCaptureFrameRate, int screenCaptureBitrate, bool screenCaptureCaptureMouseCursor, bool windowFocus, string excludeWindowList, int excludeWindowCount, int highLightWidth, uint highLightColor, bool enableHighLight);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startScreenCaptureByScreenRect(int screenRectX, int screenRectY, int screenRectWidth, int screenRectHeight, int regionRectX, int regionRectY, int regionRectWidth, int regionRectHeight, int screenCaptureVideoDimenWidth, int screenCaptureVideoDimenHeight, int screenCaptureFrameRate, int screenCaptureBitrate, bool screenCaptureCaptureMouseCursor, bool windowFocus, string excludeWindowList, int excludeWindowCount, int highLightWidth, uint highLightColor, bool enableHighLight);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setScreenCaptureContentHint(int videoContentHint);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int updateScreenCaptureParameters(int screenCaptureVideoDimenWidth, int screenCaptureVideoDimenHeight, int screenCaptureFrameRate, int screenCaptureBitrate, bool screenCaptureCaptureMouseCursor, bool windowFocus, string excludeWindowList, int excludeWindowCount, int highLightWidth, uint highLightColor, bool enableHighLight);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int updateScreenCaptureRegion(int x, int y, int width, int height);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopScreenCapture();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int addInjectStreamUrl(string url, int width, int height, int videoGop, int videoFramerate, int videoBitrate, int audioSampleRate, int audioBitrate, int audioChannels);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int removeInjectStreamUrl(string url);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableLoopbackRecording(bool enabled, string deviceName);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setAudioSessionOperationRestriction(int restriction);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int switchChannel(string token, string channelId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startChannelMediaRelay(string srcChannelName, string srcToken, uint srcUid, string destChannelName, string destToken, uint destUid, int destCount);

#if UNITY_WEBGL || UNITY_EDITOR
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startChannelMediaRelay_WEBGL(string srcChannelName, string srcToken, string srcUid, string destChannelName, string destToken, string destUid, int destCount);
#endif
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int updateChannelMediaRelay(string srcChannelName, string srcToken, uint srcUid, string destChannelName, string destToken, uint destUid, int destCount);

#if UNITY_WEBGL || UNITY_EDITOR
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int updateChannelMediaRelay_WEBGL(string srcChannelName, string srcToken, string srcUid, string destChannelName, string destToken, string destUid, int destCount);
#endif

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopChannelMediaRelay();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int sendMetadata(uint uid, uint size, byte[] buffer, long timeStampMs);

        // [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        // protected static extern int sendAudioPacket(byte[] buffer, uint size);

        // [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        // protected static extern int sendVideoPacket(byte[] buffer, uint size);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int registerPacketObserver();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int unRegisterPacketObserver();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int registerMediaMetadataObserver(int metaDataType);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int unRegisterMediaMetadataObserver();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setMirrorApplied(bool wheatherApply);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int sendMetadata(uint uid, uint size, string buffer, long timeStamps);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startScreenCaptureByWindowId(int windowId, int regionRectX, int regionRectY, int regionRectWidth, int regionRectHeight, int screenCaptureVideoDimenWidth, int screenCaptureVideoDimenHeight, int screenCaptureFrameRate, int screenCaptureBitrate, bool screenCaptureCaptureMouseCursor, bool windowFocus, string excludeWindowList, int excludeWindowCount, int highLightWidth, uint highLightColor, bool enableHighLight);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setDefaultEngineSettings();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int adjustUserPlaybackSignalVolume(uint uid, int volume);

#if UNITY_WEBGL || UNITY_EDITOR
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void adjustUserPlaybackSignalVolume_WGLM(string uid, int volume);
#endif

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setMultiChannelWant(bool multiChannelWant);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern bool getMultiChannelWanted();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setAudioMixingPitch(int pitch);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnEngineCallback(OnJoinChannelSuccessHandler OnJoinChannelSuccess,
                                      OnReJoinChannelSuccessHandler OnReJoinChannelSuccess,
                                      OnConnectionLostHandler OnConnectionLost,
                                      EngineEventOnLeaveChannelHandler OnLeaveChannel,
                                      OnConnectionInterruptedHandler OnConnectionInterrupted,
                                      OnRequestTokenHandler OnRequestToken,
                                      OnUserJoinedHandler OnUserJoined,
                                      EngineEventOnUserOfflineHandler OnUserOffline,
                                      EngineEventOnAudioVolumeIndicationHandler OnAudioVolumeIndication,
                                      EngineEventOnLocalVoicePitchInHzHandler OnLocalVoicePitchInHz,
                                      OnUserMutedAudioHandler OnUserMuteAudio,
                                      OnSDKWarningHandler OnSDKWarning,
                                      OnSDKErrorHandler OnSDKError,
                                      EngineEventOnRtcStatsHandler OnRtcStats,
                                      OnAudioMixingFinishedHandler OnAudioMixingFinished,
                                      EngineEventOnAudioRouteChangedHandler OnAudioRouteChanged,
                                      OnFirstRemoteVideoDecodedHandler OnFirstRemoteVideoDecoded,
                                      OnVideoSizeChangedHandler OnVideoSizeChanged,
                                      EngineEventOnClientRoleChanged onClientRolteChanged,
                                      OnUserMuteVideoHandler OnUserMuteVideo,
                                      OnMicrophoneEnabledHandler OnMicrophoneEnabled,
                                      OnApiExecutedHandler OnApiExecuted,
                                      OnFirstLocalAudioFrameHandler OnFirstLocalAudioFrame,
                                      OnFirstRemoteAudioFrameHandler OnFirstRemoteAudioFrame,
                                      OnLastmileQualityHandler OnLastmileQuality,
                                      OnAudioQualityHandler onAudioQuality,
                                      OnStreamInjectedStatusHandler onStreamInjectedStatus,
                                      OnStreamUnpublishedHandler onStreamUnpublished,
                                      OnStreamPublishedHandler onStreamPublished,
                                      OnStreamMessageErrorHandler onStreamMessageError,
                                      EngineEventOnStreamMessageHandler onStreamMessage,
                                      OnConnectionBannedHandler onConnectionBanned,
                                      OnVideoStoppedHandler OnVideoStopped,
                                      OnTokenPrivilegeWillExpireHandler onTokenPrivilegeWillExpire,
                                      OnNetworkQualityHandler onNetworkQuality,
                                      EngineEventOnLocalVideoStatsHandler onLocalVideoStats,
                                      EngineEventOnRemoteVideoStatsHandler onRemoteVideoStats,
                                      EngineEventOnRemoteAudioStatsHandler onRemoteAudioStats,
                                      OnFirstLocalVideoFrameHandler OnFirstLocalVideoFrame,
                                      OnFirstRemoteVideoFrameHandler OnFirstRemoteVideoFrame,
                                      OnUserEnableVideoHandler OnUserEnableVideo,
                                      OnAudioDeviceStateChangedHandler onAudioDeviceStateChanged,
                                      OnCameraReadyHandler onCameraReady,
                                      OnCameraFocusAreaChangedHandler onCameraFocusAreaChanged,
                                      OnCameraExposureAreaChangedHandler onCameraExposureAreaChanged,
                                      OnRemoteAudioMixingBeginHandler onRemoteAudioMixingBegin,
                                      OnRemoteAudioMixingEndHandler onRemoteAudioMixingEnd,
                                      OnAudioEffectFinishedHandler onAudioEffectFinished,
                                      OnVideoDeviceStateChangedHandler onVideoDeviceStateChanged,
                                      EngineEventOnRemoteVideoStateChanged OnRemoteVideoStateChanged,
                                      OnUserEnableLocalVideoHandler OnUserEnableLocalVideo,
                                      OnLocalPublishFallbackToAudioOnlyHandler OnLocalPublishFallbackToAudioOnly,
                                      OnRemoteSubscribeFallbackToAudioOnlyHandler onRemoteSubscribeFallbackToAudioOnly,
                                      EngineEventOnConnectionStateChanged onConnectionStateChanged,
                                      OnRemoteVideoTransportStatsHandler onRemoteVideoTransportStats,
                                      OnRemoteAudioTransportStatsHandler onRemoteAudioTransportStats,
                                      OnTranscodingUpdatedHandler onTranscodingUpdated,
                                      EngineEventOnAudioDeviceVolumeChangedHandler onAudioDeviceVolumeChanged,
                                      OnActiveSpeakerHandler onActiveSpeaker,
                                      OnMediaEngineStartCallSuccessHandler onMediaEngineStartCallSuccess,
                                      OnMediaEngineLoadSuccessHandler onMediaEngineLoadSuccess,
                                      EngineEventOnAudioMixingStateChangedHandler onAudioMixingStateChanged,
                                      OnFirstRemoteAudioDecodedHandler onFirstRemoteAudioDecoded,
                                      EngineEventOnLocalVideoStateChanged onLocalVideoStateChanged,
                                      EngineEventOnRtmpStreamingStateChangedHandler onRtmpStreamingStateChanged,
                                      EngineEventOnNetworkTypeChangedHandler onNetworkTypeChanged,
                                      EngineEventOnLastmileProbeResultHandler onLastmileProbeResult,
                                      OnLocalUserRegisteredHandler onLocalUserRegistered,
                                      EngineEventOnUserInfoUpdatedHandler onUserInfoUpdated,
                                      EngineEventOnLocalAudioStateChangedHandler onLocalAudioStateChanged,
                                      EngineEventOnRemoteAudioStateChangedHandler onRemoteAudioStateChanged,
                                      EngineEventOnLocalAudioStatsHandler onLocalAudioStats,
                                      EngineEventOnChannelMediaRelayStateChangedHandler onChannelMediaRelayStateChanged,
                                      EngineEventOnChannelMediaRelayEventHandler onChannelMediaRelayEvent,
                                      EngineEventOnFacePositionChanged onFacePositionChanged,
                                      OnRtmpStreamingEventHandler onRtmpStreamingEvent,
                                      OnAudioPublishStateChangedHandler onAudioPublishStateChange,
                                      OnVideoPublishStateChangedHandler onVideoPublishStateChanged,
                                      OnAudioSubscribeStateChangedHandler onAudioSubscribeStateChanged,
                                      OnVideoSubscribeStateChangedHandler onVideoSubscribeStateChanged,
                                      OnFirstLocalAudioFramePublishedHandler onFirstLocalAudioFramePublished,
                                      OnFirstLocalVideoFramePublishedHandler onFirstLocalVideoFramePublished,
                                      OnUserSuperResolutionEnabledHandler onUserSuperResolutionEnabled,
                                      OnUploadLogResultHandler onUploadLogResult,
                                      OnVirtualBackgroundSourceEnabledHandler onVirtualBackgroundSourceEnabled,
                                      EngineEventOnRequestAudioFileInfo OnRequestAudioFileInfo,
                                      OnContentInspectResultHandler onContentInspectResult,
                                      OnSnapshotTakenHandler onSnapshotTaken,
                                      OnClientRoleChangeFailedHandler onClientRoleChangeFailed,
                                      OnAudioDeviceTestVolumeIndicationHandler onAudioDeviceTestVolumeIndication,
                                      OnProxyConnectedHandler onProxyConnected,
                                      OnWlAccMessageHandler onWlAccMessage,
                                      OnWlAccStatsHandler onWlAccStats,
                                      EngineEventOnScreenCaptureInfoUpdated onScreenCaptureInfoUpdated);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initChannelEventCallback(IntPtr channel, ChannelOnWarningHandler onWarning,
                                        ChannelOnErrorHandler onError,
                                        ChannelOnJoinChannelSuccessHandler onJoinChannelSuccess,
                                        ChannelOnReJoinChannelSuccessHandler onRejoinChannelSuccess,
                                        ChannelEngineEventOnLeaveChannelHandler onLeaveChannel,
                                        ChannelOnClientRoleChangedHandler onClientRoleChanged,
                                        ChannelOnUserJoinedHandler onUserJoined,
                                        ChannelOnUserOffLineHandler onUserOffline,
                                        ChannelOnConnectionLostHandler onConnectionLost,
                                        ChannelOnRequestTokenHandler onRequestToken,
                                        ChannelOnTokenPrivilegeWillExpireHandler onTokenPrivilegeWillExpire,
                                        ChannelEngineEventOnRtcStatsHandler onRtcStats,
                                        ChannelOnNetworkQualityHandler onNetworkQuality,
                                        ChannelEngineEventOnRemoteVideoStatsHandler onRemoteVideoStats,
                                        ChannelEngineEventOnRemoteAudioStatsHandler onRemoteAudioStats,
                                        ChannelOnRemoteAudioStateChangedHandler onRemoteAudioStateChanged,
                                        ChannelOnActiveSpeakerHandler onActiveSpeaker,
                                        ChannelOnVideoSizeChangedHandler onVideoSizeChanged,
                                        ChannelOnRemoteVideoStateChangedHandler onRemoteVideoStateChanged,
                                        ChannelOnStreamMessageHandler onStreamMessage,
                                        ChannelOnStreamMessageErrorHandler onStreamMessageError,
                                        ChannelOnMediaRelayStateChangedHandler onMediaRelayStateChanged,
                                        ChannelOnMediaRelayEventHandler onMediaRelayEvent,
                                        ChannelOnRtmpStreamingStateChangedHandler onRtmpStreamingStateChanged,
                                        ChannelOnTranscodingUpdatedHandler onTranscodingUpdated,
                                        ChannelOnStreamInjectedStatusHandler onStreamInjectedStatus,
                                        ChannelOnRemoteSubscribeFallbackToAudioOnlyHandler onRemoteSubscribeFallbackToAudioOnly,
                                        ChannelOnConnectionStateChangedHandler onConnectionStateChanged,
                                        ChannelOnLocalPublishFallbackToAudioOnlyHandler onLocalPublishFallbackToAudioOnly,
                                        ChannelOnRtmpStreamingEventHandler onRtmpStreamingEvent,
                                        ChannelOnAudioPublishStateChangedHandler onAudioPublishStateChange,
                                        ChannelOnVideoPublishStateChangedHandler onVideoPublishStateChange,
                                        ChannelOnAudioSubscribeStateChangedHandler onAudioSubscribeStateChange,
                                        ChannelOnVideoSubscribeStateChangedHandler onVideoSubscribeStateChange,
                                        ChannelOnUserSuperResolutionEnabledHandler onUserSuperResolutionEnabled,
                                        ChannelOnClientRoleChangeFailedHandler onClientRoleChangeFailed,
                                        ChannelOnFirstRemoteVideoFrameHandler onFirstRemoteVideoFrame,
                                        ChannelOnChannelProxyConnectedHandler onChannelProxyConnected);

        // audio and video raw data
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnCaptureVideoFrame(EngineEventOnCaptureVideoFrame onCaptureVideoFrame);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnRenderVideoFrame(EngineEventOnRenderVideoFrame onRenderVideoFrame);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnRecordAudioFrame(EngineEventOnRecordAudioFrame onRecordAudioFrame);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnPlaybackAudioFrame(EngineEventOnPlaybackAudioFrame onPlaybackAudioFrame);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnMixedAudioFrame(EngineEventOnMixedAudioFrame onMixedAudioFrame);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnPlaybackAudioFrameBeforeMixing(EngineEventOnPlaybackAudioFrameBeforeMixing onPlaybackAudioFrameBeforeMixing);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnPullAudioFrame(EngineEventOnPullAudioFrameHandler onPullAudioFrame);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnPacketCallback(EngineEventOnReceiveAudioPacketHandler onReceiveAudioPacket, EngineEventOnReceiveVideoPacketHandler onReceiveVideoPacket, EngineEventOnSendAudioPacketHandler onSendAudioPacket, EngineEventOnSendVideoPacketHandler onSendVideoPacket);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnMetaDataCallback(EngineEventOnMediaMetaDataReceived onMetadataReceived, EngineEventOnReadyToSendMetadata onReadyToSendMetadata, EngineEventOnGetMaxMetadataSize onGetMaxMetadataSize);

#if UNITY_EDITOR || UNITY_WEBGL
        //WebGL Video Renderer APIs from ASH
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void setVideoDeviceCollectionDeviceWGL(string deviceID);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void setAudioRecordingCollectionDeviceWGL(string deviceID);
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void setPlaybackCollectionDeviceWGL(string deviceID);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int pushVideoFrameWGL(byte[] videoBuffer, int size, int stride, int height, int rotation, int cropLeft, int cropTop, int cropRight, int cropBottom);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void createLocalTexture();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern bool isLocalVideoReady();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void updateLocalTexture(IntPtr texture);
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int createRemoteTexture(string userId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern bool isRemoteVideoReady(string videoID);
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern bool isRemoteVideoReady_MC(string channelId, string videoID);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        public static extern void updateRemoteTexture(string videoID, IntPtr texture);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        public static extern void updateRemoteTexture_MC(string channel, string videoID, IntPtr texture);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        public static extern void setCurrentChannel_WGL(string channelId);
#endif


        // 3.0 multi channel
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr createChannel(string channelId);




#if !UNITY_EDITOR && UNITY_WEBGL
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int joinChannel2(string channel, string token, string info, uint uid, bool autoSubscribeAudio, bool autoSubscribeVideo, bool publishLocalAudio, bool publishLocalVideo);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int joinChannelWithUserAccount2(string channel, string token, string userAccount, bool autoSubscribeAudio, bool autoSubscribeVideo, bool publishLocalAudio, bool publishLocalVideo);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int leaveChannel2(string channel);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int publish(string channel);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int unpublish(string channel);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int ReleaseChannel(string channel);
#else
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int ReleaseChannel(IntPtr channel);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int joinChannel2(IntPtr channel, string token, string info, uint uid, bool autoSubscribeAudio, bool autoSubscribeVideo, bool publishLocalAudio, bool publishLocalVideo);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int joinChannelWithUserAccount2(IntPtr channel, string token, string userAccount, bool autoSubscribeAudio, bool autoSubscribeVideo, bool publishLocalAudio, bool publishLocalVideo);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int leaveChannel2(IntPtr channel);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int publish(IntPtr channel);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int unpublish(IntPtr channel);

#endif
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr channelId(IntPtr channel);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr getCallId2(IntPtr channel);

#if !UNITY_EDITOR && UNITY_WEBGL
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int renewToken2(string channel, string token);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setEncryptionSecret2(string channel, string secret);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setEncryptionMode2(string channel, string encryptionMode);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setClientRole2(string channel, int role);
#else
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int renewToken2(IntPtr channel, string token);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setEncryptionSecret2(IntPtr channel, string secret);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setEncryptionMode2(IntPtr channel, string encryptionMode);

        // virtual int registerPacketObserver(void *channel, IPacketObserver* observer);

        // virtual int registerMediaMetadataObserver(void *channel, IMetadataObserver *observer, IMetadataObserver::METADATA_TYPE type);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setClientRole2(IntPtr channel, int role);
#endif

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteUserPriority2(IntPtr channel, uint uid, int userPriority);

#if UNITY_WEBGL
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteUserPriority2_WGLM(IntPtr channel, string uid, int userPriority);
#endif

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteVoicePosition2(IntPtr channel, uint uid, double pan, double gain);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteRenderMode2(IntPtr channel, uint userId, int renderMode, int mirrorMode);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setDefaultMuteAllRemoteAudioStreams2(IntPtr channel, bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setDefaultMuteAllRemoteVideoStreams2(IntPtr channel, bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteAllRemoteAudioStreams2(IntPtr channel, bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int adjustUserPlaybackSignalVolume2(IntPtr channel, uint userId, int volume);

#if UNITY_WEBGL || UNITY_EDITOR
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int adjustUserPlaybackSignalVolume2_WGLM(IntPtr channel, string userId, int volume);
#endif

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteRemoteAudioStream2(IntPtr channel, uint userId, bool mute);

#if UNITY_WEBGL || UNITY_EDITOR
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteRemoteAudioStream2_WGLM(IntPtr channel, string userId, bool mute);
#endif

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteAllRemoteVideoStreams2(IntPtr channel, bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteRemoteVideoStream2(IntPtr channel, uint userId, bool mute);

#if UNITY_WEBGL || UNITY_EDITOR
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteRemoteVideoStream2_WGLM(IntPtr channel, string userId, bool mute);
#endif

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteVideoStreamType2(IntPtr channel, uint userId, int streamType);

#if UNITY_WEBGL || UNITY_EDITOR
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteVideoStreamType2_WGLM(IntPtr channel, string userId, int streamType);
#endif

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteDefaultVideoStreamType2(IntPtr channel, int streamType);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int createDataStream2(IntPtr channel, bool reliable, bool ordered);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int createDataStream_channel(IntPtr channel, bool syncWithAudio, bool ordered);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int sendStreamMessage2(IntPtr channel, int streamId, string data, Int64 length);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int addPublishStreamUrl2(IntPtr channel, string url, bool transcodingEnabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int removePublishStreamUrl2(IntPtr channel, string url);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setLiveTranscoding2(IntPtr channel, int width, int height, int videoBitrate, int videoFramerate, bool lowLatency, int videoGroup, int video_codec_profile, uint backgroundColor, uint userCount, string transcodingUserInfo, string transcodingExtraInfo, string metaData, string watermarkRtcImageUrl, int watermarkRtcImageX, int watermarkRtcImageY, int watermarkRtcImageWidth, int watermarkRtcImageHeight, int watermarkImageZorder, double watermarkImageAlpha, uint watermarkCount, string backgroundImageRtcImageUrl, int backgroundImageRtcImageX, int backgroundImageRtcImageY, int backgroundImageRtcImageWidth, int backgroundImageRtcImageHeight, int backgroundImageRtcImageZorder, double backgroundImageRtcImageAlpha, uint backgroundImageRtcImageCount, int audioSampleRate, int audioBitrate, int audioChannels, int audioCodecProfile, string advancedFeatures, uint advancedFeatureCount);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startRtmpStreamWithoutTranscoding2(IntPtr channel, string url);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startRtmpStreamWithTranscoding2(IntPtr channel, string url, int width, int height, int videoBitrate, int videoFramerate, bool lowLatency, int videoGroup, int video_codec_profile, uint backgroundColor, uint userCount, string transcodingUserInfo, string transcodingExtraInfo, string metaData, string watermarkRtcImageUrl, int watermarkRtcImageX, int watermarkRtcImageY, int watermarkRtcImageWidth, int watermarkRtcImageHeight, int watermarkImageZorder, double watermarkImageAlpha, uint watermarkCount, string backgroundImageRtcImageUrl, int backgroundImageRtcImageX, int backgroundImageRtcImageY, int backgroundImageRtcImageWidth, int backgroundImageRtcImageHeight, int backgroundImageRtcImageZorder, double backgroundImageRtcImageAlpha, uint backgroundImageRtcImageCount, int audioSampleRate, int audioBitrate, int audioChannels, int audioCodecProfile, string advancedFeatures, uint advancedFeatureCount);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int updateRtmpTranscoding2(IntPtr channel, int width, int height, int videoBitrate, int videoFramerate, bool lowLatency, int videoGroup, int video_codec_profile, uint backgroundColor, uint userCount, string transcodingUserInfo, string transcodingExtraInfo, string metaData, string watermarkRtcImageUrl, int watermarkRtcImageX, int watermarkRtcImageY, int watermarkRtcImageWidth, int watermarkRtcImageHeight, int watermarkImageZorder, double watermarkImageAlpha, uint watermarkCount, string backgroundImageRtcImageUrl, int backgroundImageRtcImageX, int backgroundImageRtcImageY, int backgroundImageRtcImageWidth, int backgroundImageRtcImageHeight, int backgroundImageRtcImageZorder, double backgroundImageRtcImageAlpha, uint backgroundImageRtcImageCount, int audioSampleRate, int audioBitrate, int audioChannels, int audioCodecProfile, string advancedFeatures, uint advancedFeatureCount);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopRtmpStream2(IntPtr channel, string url);
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int addInjectStreamUrl2(IntPtr channel, string url, int width, int height, int videoGop, int videoFramerate, int videoBitrate, int audioSampleRate, int audioBitrate, int audioChannels);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int removeInjectStreamUrl2(IntPtr channel, string url);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startChannelMediaRelay2(IntPtr channel, string srcChannelName, string srcToken, uint srcUid, string destChannelName, string destToken, uint destUid, int destCount);

#if UNITY_WEBGL || UNITY_EDITOR
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startChannelMediaRelay2_WEBGL(IntPtr channel, string srcChannelName, string srcToken, string srcUid, string destChannelName, string destToken, string destUid, int destCount);
#endif

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int updateChannelMediaRelay2(IntPtr channel, string srcChannelName, string srcToken, uint srcUid, string destChannelName, string destToken, uint destUid, int destCount);

#if UNITY_WEBGL || UNITY_EDITOR
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int updateChannelMediaRelay2_WEBGL(IntPtr channel, string srcChannelName, string srcToken, string srcUid, string destChannelName, string destToken, string destUid, int destCount);
#endif

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopChannelMediaRelay2(IntPtr channel);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getConnectionState2(IntPtr channel);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void addUserVideoInfo2(string channelId, uint _userId, uint _textureId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void removeUserVideoInfo2(string channelId, uint _userId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int updateVideoRawData2(IntPtr data, string channelId, uint uid);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableFaceDetection(bool enable);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableEncryption(bool enabled, string encryptionKey, int encryptionMode, byte[] encryptionKdfSalt);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableRemoteSuperResolution(bool enabled, int mode, uint userId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setClientRole_1(int role, int audienceLatencyLevel);


#if !UNITY_EDITOR && UNITY_WEBGL
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setClientRole_2(string channel, int role, int audienceLatencyLevel);
#else
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setClientRole_2(IntPtr channel, int role, int audienceLatencyLevel);
#endif

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setVoiceBeautifierPreset(int preset);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setAudioEffectPreset(int preset);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setAudioEffectParameters(int preset, int param1, int param2);

#if !UNITY_EDITOR && UNITY_WEBGL
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableEncryption2(string channel, bool enabled, string encryptionKey, int encryptionMode);
#else
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableEncryption2(IntPtr channel, bool enabled, string encryptionKey, int encryptionMode, byte[] encryptionKdfSalt);
#endif

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableRemoteSuperResolution2(IntPtr channel, bool enabled, int mode, uint userId);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int sendCustomReportMessage(string id, string category, string events, string label, int value);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setVoiceBeautifierParameters(int preset, int param1, int param2);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableDeepLearningDenoise(bool enable);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int joinChannelWithMediaOption(string token, string channelId, string info, uint uid, bool autoSubscribeAudio, bool autoSubscribeVideo, bool publishLocalAudio, bool publishLocalVideo);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int switchChannel2(string token, string channelId, bool autoSubscribeAudio, bool autoSubscribeVideo, bool publishLocalAudio, bool publishLocalVideo);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr uploadLogFile();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setCloudProxy(int proxyType);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setVoiceConversionPreset(int preset);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int adjustLoopbackRecordingSignalVolume(int volume);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startAudioRecordingWithConfig(string filePath, int recordingQuality, int recordingPosition, int recordingSampleRate, int recordingChannel);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setLocalAccessPoint(string ips, int ipSize, string domainList, int domainListSize, string verifyDomainName, int mode);


#if !UNITY_EDITOR && UNITY_WEBGL
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteLocalAudioStream_channel(string channel, bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteLocalVideoStream_channel(string channel, bool mute);
#else
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteLocalAudioStream_channel(IntPtr channel, bool mute);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int muteLocalVideoStream_channel(IntPtr channel, bool mute);
#endif

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableVirtualBackground(bool enabled, int background_source_type, uint color, string source, int blur_degree, bool mute, bool loop);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setVirtualBackgroundBlur(int blur_degree);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setVirtualBackgroundColor(string hexColor);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setVirtualBackgroundImage(string imgFile);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setVirtualBackgroundVideo(string videoFile);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int initVirtualBackground_MC(bool enabled, int background_source_type, uint color, string source, int blur_degree, bool mute, bool loop);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setVirtualBackgroundBlur_MC(int blur_degree);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setVirtualBackgroundColor_MC(string hexColor);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setVirtualBackgroundImage_MC(string imgFile);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setVirtualBackgroundVideo_MC(string videoFile);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setCameraTorchOn(bool on);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern bool isCameraTorchSupported();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setExternalAudioSourceVolume(int sourcePos, int volume);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setAudioMixingPlaybackSpeed(int speed);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int selectAudioTrack(int index);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getAudioTrackCount();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setAudioMixingDualMonoMode(int mode);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int pauseAllChannelMediaRelay();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int resumeAllChannelMediaRelay();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getAudioFileInfo(string filePath);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setAVSyncSource2(IntPtr channel, string channelId, uint uid);

        //MediaRecorder
        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int createMediaRecorder();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int startRecording(string storagePath, int containerFormat, int streamType, int maxDurationMs, int recorderInfoUpdateInterval);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int stopRecording();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int releaseMediaRecorder();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnMediaRecorderCallback(EngineEventOnRecorderStateChanged onRecorderStateChanged, EngineEventOnRecorderInfoUpdated onRecorderInfoUpdated);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void getScreenCaptureSources(int thumbHeight, int thumbWidth, int iconHeight, int iconWidth, bool includeScreen);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getScreenCaptureSourcesCount();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getScreenCaptureSourceType(uint index);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr getScreenCaptureSourceName(uint index);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr getScreenCaptureSourceProcessPath(uint index);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr getScreenCaptureSourceTitle(uint index);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr getScreenCaptureSourceId(uint index);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern bool getScreenCaptureIsPrimaryMonitor(uint index);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getScreenCaptureThumbImage(uint index, ref ThumbImageBuffer buffer);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getScreenCaptureIconImage(uint index, ref ThumbImageBuffer buffer);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setLowlightEnhanceOptions(bool enabled, int mode, int level);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setVideoDenoiserOptions(bool enabled, int mode, int level);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setColorEnhanceOptions(bool enabled, float strengthLevel, float skinProtectLevel);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableWirelessAccelerate(bool enabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableLocalVoicePitchCallback(int interval);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableSpatialAudio(bool enabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int enableSpatialAudio_MC(bool enabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteUserSpatialAudioParams(string uid, double speaker_azimuth, double speaker_elevation, double speaker_distance, int speaker_orientation, double speaker_attenuation, bool enable_blur, bool enable_air_absorb);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setScreenCaptureScenario(int screenScenario);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteUserSpatialAudioParams2(string uid, double speaker_azimuth, double speaker_elevation, double speaker_distance, int speaker_orientation, double speaker_attenuation, bool enable_blur, bool enable_air_absorb);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setCameraZoomFactor(float factor);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern float getCameraMaxZoomFactor();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern bool isCameraZoomSupported();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern bool isCameraFocusSupported();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern bool isCameraExposurePositionSupported();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern bool isCameraAutoFocusFaceModeSupported();

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setCameraFocusPositionInPreview(float positionX, float positionY);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setCameraExposurePosition(float positionXinView, float positionYinView);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setCameraAutoFocusFaceModeEnabled(bool enabled);

        [DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setCameraCaptureRotation(int rotation);

        #endregion engine callbacks
    }
}
