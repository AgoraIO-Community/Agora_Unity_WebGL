using UnityEngine;
using System;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using AOT;

/* class IRtcEngine provides c# API for Unity 3D
* app. Use IRtcEngine to access underlying Agora
* sdk.
* 
* Agora sdk only supports single instance by now. So here
* provides GetEngine() and Destroy() to create/delete the
* only instance.
*/

namespace agora_gaming_rtc
{
    /** The definition of IRtcEngine.
    */
    public sealed class IRtcEngine : IRtcEngineNative
    {
        #region set callback here for user
        // Find more efer to AgoraCallback.cs.
        public OnJoinChannelSuccessHandler OnJoinChannelSuccess;

        public OnReJoinChannelSuccessHandler OnReJoinChannelSuccess;

        public OnConnectionLostHandler OnConnectionLost;

        public OnConnectionInterruptedHandler OnConnectionInterrupted;

        public OnRequestTokenHandler OnRequestToken;

        public OnUserJoinedHandler OnUserJoined;

        public OnUserOfflineHandler OnUserOffline;

        public OnLeaveChannelHandler OnLeaveChannel;

        public OnVolumeIndicationHandler OnVolumeIndication;

        public OnUserMutedAudioHandler OnUserMutedAudio;

        public OnSDKWarningHandler OnWarning;

        public OnSDKErrorHandler OnError;

        public OnRtcStatsHandler OnRtcStats;

        public OnAudioMixingFinishedHandler OnAudioMixingFinished;

        public OnAudioRouteChangedHandler OnAudioRouteChanged;

        public OnFirstRemoteVideoDecodedHandler OnFirstRemoteVideoDecoded;

        public OnVideoSizeChangedHandler OnVideoSizeChanged;

        public OnClientRoleChangedHandler OnClientRoleChanged;

        public OnUserMuteVideoHandler OnUserMuteVideo;

        public OnMicrophoneEnabledHandler OnMicrophoneEnabled;

        public OnFirstRemoteAudioFrameHandler OnFirstRemoteAudioFrame;

        public OnFirstLocalAudioFrameHandler OnFirstLocalAudioFrame;

        public OnApiExecutedHandler OnApiExecuted;

        public OnLastmileQualityHandler OnLastmileQuality;

        public OnAudioQualityHandler OnAudioQuality;

        public OnStreamInjectedStatusHandler OnStreamInjectedStatus;

        public OnStreamUnpublishedHandler OnStreamUnpublished;

        public OnStreamPublishedHandler OnStreamPublished;

        public OnStreamMessageErrorHandler OnStreamMessageError;

        public OnStreamMessageHandler OnStreamMessage;

        public OnConnectionBannedHandler OnConnectionBanned;

        public OnConnectionStateChangedHandler OnConnectionStateChanged;

        public OnTokenPrivilegeWillExpireHandler OnTokenPrivilegeWillExpire;

        public OnActiveSpeakerHandler OnActiveSpeaker;

        public OnVideoStoppedHandler OnVideoStopped;

        public OnFirstLocalVideoFrameHandler OnFirstLocalVideoFrame;

        public OnFirstRemoteVideoFrameHandler OnFirstRemoteVideoFrame;

        public OnUserEnableVideoHandler OnUserEnableVideo;

        public OnUserEnableLocalVideoHandler OnUserEnableLocalVideo;

        public OnRemoteVideoStateChangedHandler OnRemoteVideoStateChanged;

        public OnLocalPublishFallbackToAudioOnlyHandler OnLocalPublishFallbackToAudioOnly;

        public OnRemoteSubscribeFallbackToAudioOnlyHandler OnRemoteSubscribeFallbackToAudioOnly;

        public OnNetworkQualityHandler OnNetworkQuality;

        public OnLocalVideoStatsHandler OnLocalVideoStats;

        public OnRemoteVideoStatsHandler OnRemoteVideoStats;

        public OnRemoteAudioStatsHandler OnRemoteAudioStats;

        public OnAudioDeviceStateChangedHandler OnAudioDeviceStateChanged;

        public OnCameraReadyHandler OnCameraReady;

        public OnCameraFocusAreaChangedHandler OnCameraFocusAreaChanged;

        public OnCameraExposureAreaChangedHandler OnCameraExposureAreaChanged;
       
        public OnRemoteAudioMixingBeginHandler OnRemoteAudioMixingBegin;

        public OnRemoteAudioMixingEndHandler OnRemoteAudioMixingEnd;

        public OnAudioEffectFinishedHandler OnAudioEffectFinished;

        public OnVideoDeviceStateChangedHandler OnVideoDeviceStateChanged;

        public OnRemoteVideoTransportStatsHandler OnRemoteVideoTransportStats;

        public OnRemoteAudioTransportStatsHandler OnRemoteAudioTransportStats;
    
        public OnTranscodingUpdatedHandler OnTranscodingUpdated;

        public OnAudioDeviceVolumeChangedHandler OnAudioDeviceVolumeChanged;

        public OnMediaEngineStartCallSuccessHandler OnMediaEngineStartCallSuccess;

        public OnMediaEngineLoadSuccessHandler OnMediaEngineLoadSuccess;

        public OnAudioMixingStateChangedHandler OnAudioMixingStateChanged;

        public OnFirstRemoteAudioDecodedHandler OnFirstRemoteAudioDecoded;

        public OnLocalVideoStateChangedHandler OnLocalVideoStateChanged;

        public OnRtmpStreamingStateChangedHandler OnRtmpStreamingStateChanged;

        public OnNetworkTypeChangedHandler OnNetworkTypeChanged;

        public OnLastmileProbeResultHandler OnLastmileProbeResult;

        public OnLocalUserRegisteredHandler OnLocalUserRegistered;

        public OnUserInfoUpdatedHandler OnUserInfoUpdated;

        public OnLocalAudioStateChangedHandler OnLocalAudioStateChanged;

        public OnRemoteAudioStateChangedHandler OnRemoteAudioStateChanged;

        public OnLocalAudioStatsHandler OnLocalAudioStats;

        public OnChannelMediaRelayEventHandler OnChannelMediaRelayEvent;

        public OnChannelMediaRelayStateChangedHandler OnChannelMediaRelayStateChanged;

        public OnFacePositionChangedHandler OnFacePositionChanged;

        public OnRtmpStreamingEventHandler OnRtmpStreamingEvent;

        public OnAudioPublishStateChangedHandler OnAudioPublishStateChanged;

        public OnVideoPublishStateChangedHandler OnVideoPublishStateChanged;

        public OnAudioSubscribeStateChangedHandler OnAudioSubscribeStateChanged;

        public OnVideoSubscribeStateChangedHandler OnVideoSubscribeStateChanged;

        public OnFirstLocalAudioFramePublishedHandler OnFirstLocalAudioFramePublished;

        public OnFirstLocalVideoFramePublishedHandler OnFirstLocalVideoFramePublished;

        public OnUserSuperResolutionEnabledHandler OnUserSuperResolutionEnabled;

        public OnUploadLogResultHandler OnUploadLogResult;

        public OnVirtualBackgroundSourceEnabledHandler OnVirtualBackgroundSourceEnabled;

        #endregion  set callback here for user

        private readonly AudioEffectManagerImpl mAudioEffectM;
        private readonly AudioRecordingDeviceManager audioRecordingDeviceManager;
        private readonly AudioPlaybackDeviceManager audioPlaybackDeviceManager;
        private readonly VideoDeviceManager videoDeviceManager;
        private readonly AudioRawDataManager audioRawDataManager;
        private readonly VideoRawDataManager videoRawDataManager;
        private readonly VideoRender videoRender;
        private readonly bool initStatus = false;
        private const string agoraGameObjectName = "agora_engine_CallBackGamObject";
        // private static GameObject agoraGameObject = null;
        // private static AgoraCallbackQueue ._AgoraCallbackObject = null;
        private AgoraCallbackObject _AgoraCallbackObject = null;

        private IRtcEngine(string appId)
        {


#if !UNITY_EDITOR && UNITY_WEBGL
#else
            InitGameObject();
            InitEngineCallback();
#endif
            int retCode = IRtcEngineNative.createEngine(appId);
            if (retCode != 0) {
                Debug.LogError("Create engine failed, error code: " + retCode);
                initStatus = false;
            } 

#if !UNITY_EDITOR && UNITY_WEBGL

            audioRecordingDeviceManager = AudioRecordingDeviceManager.GetInstance(this);
            videoDeviceManager = VideoDeviceManager.GetInstance(this);
            mAudioEffectM = AudioEffectManagerImpl.GetInstance(this);
            audioPlaybackDeviceManager = AudioPlaybackDeviceManager.GetInstance(this);
            videoDeviceManager = VideoDeviceManager.GetInstance(this);
            audioRawDataManager = AudioRawDataManager.GetInstance(this);
            videoRawDataManager = VideoRawDataManager.GetInstance(this);
            videoRender = VideoRender.GetInstance(this);
#else
            mAudioEffectM = AudioEffectManagerImpl.GetInstance(this);
            audioRecordingDeviceManager = AudioRecordingDeviceManager.GetInstance(this);
            audioPlaybackDeviceManager = AudioPlaybackDeviceManager.GetInstance(this);
            videoDeviceManager = VideoDeviceManager.GetInstance(this);
            audioRawDataManager = AudioRawDataManager.GetInstance(this);
            videoRawDataManager = VideoRawDataManager.GetInstance(this);
            videoRender = VideoRender.GetInstance(this);
#endif
        }

        // custom testing function -- AR
        public AudioRecordingDeviceManager TestGetAudioRecordingDeviceManager()
        {
            return audioRecordingDeviceManager;
        }

        public AudioPlaybackDeviceManager TestGetAudioPlaybackDeviceManager()
        {
            return audioPlaybackDeviceManager;
        }

        public VideoDeviceManager TestGetVideoDeviceManager()
        {
            return videoDeviceManager;
        }

        private IRtcEngine(RtcEngineConfig engineConfig)
        {
            InitGameObject();
            InitEngineCallback();
            int retCode = IRtcEngineNative.createEngine2(engineConfig.appId, (uint)engineConfig.areaCode, engineConfig.logConfig.filePath, engineConfig.logConfig.fileSize, (int)engineConfig.logConfig.level);
            if (retCode != 0) {
                Debug.LogError("Create engine failed, error code: " + retCode);
                initStatus = false;
            } 
            mAudioEffectM = AudioEffectManagerImpl.GetInstance(this);
            audioRecordingDeviceManager = AudioRecordingDeviceManager.GetInstance(this);
            audioPlaybackDeviceManager = AudioPlaybackDeviceManager.GetInstance(this);
            videoDeviceManager = VideoDeviceManager.GetInstance(this);
            audioRawDataManager = AudioRawDataManager.GetInstance(this);
            videoRawDataManager = VideoRawDataManager.GetInstance(this);
            videoRender = VideoRender.GetInstance(this);       
        }

        private void InitGameObject()
        {
            _AgoraCallbackObject = new AgoraCallbackObject(agoraGameObjectName);
        }

        private void DeInitGameObject()
        {
            _AgoraCallbackObject.Release();
            _AgoraCallbackObject = null;
        }

        public bool GetInitStatus()
        {
            return initStatus;
        }

        public string doFormat(string format, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }

         /** Gets the SDK version.
         *
         * @return The version of the current SDK in the string format. For example, 2.9.1.
         */
        public static string GetSdkVersion()
        {
            return Marshal.PtrToStringAnsi(IRtcEngineNative.getSdkVersion());
        }

        /** Sets the channel profile of the Agora IRtcEngine.
         *
         * The Agora IRtcEngine differentiates channel profiles and applies optimization algorithms accordingly.
         * For example, it prioritizes smoothness and low latency for a video call, and prioritizes video quality for a video broadcast.
         *
         * @warning
         * - To ensure the quality of real-time communication, we recommend that all users in a channel use the same channel profile.
         * - Call this method before calling {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey joins a channel}. You cannot set the channel profile once you have joined the channel.
         * - The default audio route and video encoding bitrate are different in different channel profiles. For details, see
         * {@link agora_gaming_rtc.IRtcEngine.SetDefaultAudioRouteToSpeakerphone SetDefaultAudioRouteToSpeakerphone} and {@link agora_gaming_rtc.IRtcEngine.SetVideoEncoderConfiguration SetVideoEncoderConfiguration}.
         *
         * @param profile The channel profile of the Agora IRtcEngine. See {@link agora_gaming_rtc.CHANNEL_PROFILE CHANNEL_PROFILE}.
         *
         * @return
         * - 0(ERR_OK): Success.
         * - < 0: Failure.
         *  - -2(ERR_INVALID_ARGUMENT): The parameter is invalid.
         *  - -7(ERR_NOT_INITIALIZED): The SDK is not initialized.
         */
        public int SetChannelProfile(CHANNEL_PROFILE profile)
        {
            return IRtcEngineNative.setChannelProfile((int)profile);
        }

        /** Sets the role of the user, such as a host or an audience (default), before joining a channel in the interactive live streaming.
         * 
         * This method can be used to switch the user role in the interactive live streaming after the user joins a channel.
         * 
         * In the Live Broadcast profile, when a user switches user roles after joining a channel, a successful `SetClientRole` method call triggers the following callbacks:
         * - The local client: {@link agora_gaming_rtc.OnClientRoleChangedHandler OnClientRoleChangedHandler}
         * - The remote client: {@link agora_gaming_rtc.OnUserJoinedHandler OnUserJoinedHandler} or {@link agora_gaming_rtc.OnUserOfflineHandler OnUserOfflineHandler} (BECOME_AUDIENCE)
         * 
         * @note This method applies only to the Live-broadcast profile.
         * 
         * @param role Sets the role of the user. See {@link agora_gaming_rtc.CLIENT_ROLE_TYPE CLIENT_ROLE_TYPE}.
         * 
         * @return
         * - 0(ERR_OK): Success.
         * - < 0: Failure.
         *  - -1(ERR_FAILED): A general error occurs (no specified reason).
         *  - -2(ERR_INALID_ARGUMENT): The parameter is invalid.
         *  - -7(ERR_NOT_INITIALIZED): The SDK is not initialized.
         */
        public int SetClientRole(CLIENT_ROLE_TYPE role)
        {
            return IRtcEngineNative.setClientRole((int)role);
        }

        /** Sets the output log level of the SDK.
         * 
         * @deprecated This method is deprecated from v3.3.1. Use `logConfig` in the 
         * {@link agora_gaming_rtc.IRtcEngine.GetEngine(RtcEngineConfig engineConfig) GetEngine} method instead.
         *
         * You can use one or a combination of the log filter levels. The log level follows the sequence of OFF, CRITICAL, ERROR, WARNING, INFO, and DEBUG. Choose a level to see the logs preceding that level.
         * 
         * For example, when you set the log level to WARNING, you can see the logs within levels CRITICAL, ERROR, and WARNING.
         *
         * @see {@link agora_gaming_rtc.IRtcEngine.SetLogFile SetLogFile}
         * @see {@link agora_gaming_rtc.IRtcEngine.SetLogFileSize SetLogFileSize}
         *
         * @param filter Sets the log filter level. See {@link agora_gaming_rtc.LOG_FILTER LOG_FILTER}.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetLogFilter(LOG_FILTER filter)
        {
            return IRtcEngineNative.setLogFilter((uint)filter);
        }

        /** Sets the log files that the SDK outputs.
         *
         * @deprecated This method is deprecated from v3.3.1. Use `logConfig` in the 
         * {@link agora_gaming_rtc.IRtcEngine.GetEngine(RtcEngineConfig engineConfig) GetEngine} method instead.
         *
         * By default, the SDK outputs five log files, `agorasdk.log`, `agorasdk_1.log`, `agorasdk_2.log`, `agorasdk_3.log`, `agorasdk_4.log`, each with a default size of 1024 KB.
         * These log files are encoded in UTF-8. The SDK writes the latest logs in `agorasdk.log`. When `agorasdk.log` is full, the SDK deletes the log file with the earliest
         * modification time among the other four, renames `agorasdk.log` to the name of the deleted log file, and create a new `agorasdk.log` to record latest logs.
         *
         * @note Ensure that you call this method immediately after calling {@link agora_gaming_rtc.IRtcEngine.GetEngine GetEngine}, otherwise the output logs may not be complete.
         *
         * @see {@link agora_gaming_rtc.IRtcEngine.SetLogFileSize SetLogFileSize}
         * @see {@link agora_gaming_rtc.IRtcEngine.SetLogFilter SetLogFilter}
         *
         * @param filePath The absolute path of log files. The default file path is `C: \Users\<user_name>\AppData\Local\Agora\<process_name>\agorasdk.log`.
         * Ensure that the directory for the log files exists and is writable. You can use this parameter to rename the log files.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetLogFile(string filePath)
        {
            return IRtcEngineNative.setLogFile(filePath);
        }

        public int SetDefaultEngineSettings()
        {
            return IRtcEngineNative.setDefaultEngineSettings();
        }

        /** Allows a user to join a channel.
         * 
         * Users in the same channel can talk to each other, and multiple users in the same channel can start a group chat. Users with different App IDs cannot call each other.
         * 
         * You must call the {@link agora_gaming_rtc.IRtcEngine.LeaveChannel LeaveChannel} method to exit the current call before entering another channel.
         * 
         * A successful `JoinChannel` method call triggers the following callbacks:
         * - The local client: {@link agora_gaming_rtc.OnJoinChannelSuccessHandler OnJoinChannelSuccessHandler}
         * - The remote client: {@link agora_gaming_rtc.OnUserJoinedHandler OnUserJoinedHandler}, if the user joining the channel is in the Communication profile, or is a BROADCASTER in the Live Broadcast profile.
         * When the connection between the client and Agora's server is interrupted due to poor network conditions, the SDK tries reconnecting to the server. When the local client successfully rejoins the channel, the SDK triggers the {@link agora_gaming_rtc.OnReJoinChannelSuccessHandler OnReJoinChannelSuccessHandler} callback on the local client.
         * 
         * @note A channel does not accept duplicate uids, such as two users with the same uid. If you set `uid` as 0, the system automatically assigns a uid. If you want to join a channel from different devices, ensure that each device has a different uid.
         * 
         * @warning Ensure that the App ID used for creating the token is the same App ID used by the {@link agora_gaming_rtc.IRtcEngine.GetEngine GetEngine} method for initializing the IRtcEngine. Otherwise, the CDN live streaming may fail.
         * 
         * @param channelName The unique channel name for the Agora RTC session in the string format smaller than 64 bytes. Supported characters:
         * - The 26 lowercase English letters: a to z
         * - The 26 uppercase English letters: A to Z
         * - The 10 numbers: 0 to 9
         * - The space
         * - "!", "#", "$", "%", "&", "(", ")", "+", "-", ":", ";", "<", "=", ".", ">", "?", "@", "[", "]", "^", "_", " {", "}", "|", "~", ","
         * @param info (Optional) Additional information about the channel. This parameter can be set to `null` or contain channel related information. Other users in the channel will not receive this message.
         * @param uid (Optional) User ID. A 32-bit unsigned integer with a value ranging from 1 to 2<sup>32</sup>-1. The uid must be unique. If a uid is not assigned (or set to 0), the SDK assigns and returns a uid in the {@link agora_gaming_rtc.OnJoinChannelSuccessHandler OnJoinChannelSuccessHandler} callback. Your application must record and maintain the returned uid since the SDK does not do so.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         *   - `ERR_INVALID_ARGUMENT(-2)`
         *   - `ERR_NOT_READY(-3)`
         *   - `ERR_REFUSED(-5)`
         */
        public int JoinChannel(string channelName, string info = "", uint uid = 0)
        {
            return JoinChannelByKey(null, channelName, info, uid);
        }

        /** Allows a user to join a channel with token.
         * 
         * Users in the same channel can talk to each other, and multiple users in the same channel can start a group chat. Users with different App IDs cannot call each other.
         * 
         * You must call the {@link agora_gaming_rtc.IRtcEngine.LeaveChannel LeaveChannel} method to exit the current call before entering another channel.
         * 
         * A successful `JoinChannelByKey` method call triggers the following callbacks:
         * - The local client: {@link agora_gaming_rtc.OnJoinChannelSuccessHandler OnJoinChannelSuccessHandler}
         * - The remote client: {@link agora_gaming_rtc.OnUserJoinedHandler OnUserJoinedHandler}, if the user joining the channel is in the Communication profile, or is a BROADCASTER in the Live Broadcast profile.
         * 
         * When the connection between the client and Agora's server is interrupted due to poor network conditions, the SDK tries reconnecting to the server. When the local client successfully rejoins the channel, the SDK triggers the {@link agora_gaming_rtc.OnReJoinChannelSuccessHandler OnReJoinChannelSuccessHandler} callback on the local client.
         * 
         * Once the user joins the channel, the user subscribes to the audio and video streams of all the other users in the channel by default, giving rise to usage and billing calculation. If you do not want to subscribe to a specified stream or all remote streams, call the `mute` methods accordingly.
         *
         * @note A channel does not accept duplicate uids, such as two users with the same uid. If you set `uid` as 0, the system automatically assigns a uid. If you want to join a channel from different devices, ensure that each device has a different uid.
         * 
         * @warning Ensure that the App ID used for creating the token is the same App ID used by the {@link agora_gaming_rtc.IRtcEngine.GetEngine GetEngine} method for initializing the IRtcEngine. Otherwise, the CDN live streaming may fail.
         * 
         * @param channelKey The token generated by the application server. In most circumstances, a static App ID suffices. For added security, use a Channel Key.
         * - If the user uses a static App ID, token is optional and can be set as `null`.
         * - If the user uses a Channel Key, Agora issues an additional App Certificate for you to generate a user key based on the algorithm and App Certificate for user authentication on the server.
         * @param channelName The unique channel name for the Agora RTC session in the string format smaller than 64 bytes. Supported characters:
         * - The 26 lowercase English letters: a to z
         * - The 26 uppercase English letters: A to Z
         * - The 10 numbers: 0 to 9
         * - The space
         * - "!", "#", "$", "%", "&", "(", ")", "+", "-", ":", ";", "<", "=", ".", ">", "?", "@", "[", "]", "^", "_", " {", "}", "|", "~", ","
         * @param info (Optional) Additional information about the channel. This parameter can be set to `null` or contain channel related information. Other users in the channel will not receive this message.
         * @param uid (Optional) User ID. A 32-bit unsigned integer with a value ranging from 1 to 2<sup>32</sup>-1. The uid must be unique. If a uid is not assigned (or set to 0), the SDK assigns and returns a uid in the {@link agora_gaming_rtc.OnJoinChannelSuccessHandler OnJoinChannelSuccessHandler} callback. Your application must record and maintain the returned uid since the SDK does not do so.
         * 
         * @return
         * - 0(ERR_OK): Success.
         * - < 0: Failure.
         *   - -2(ERR_INALID_ARGUMENT): The parameter is invalid.
         * - -3(ERR_NOT_READY): The SDK fails to be initialized. You can try re-initializing the SDK.
         * - -5(ERR_REFUSED): The request is rejected. This may be caused by the following:
         *     - You have created an `AgoraChannel` object with the same channel name.
         *     - You have joined and published a stream in a channel created by the `AgoraChannel` object.
         */
        public int JoinChannelByKey(string channelKey, string channelName, string info = "", uint uid = 0)
        {
            return IRtcEngineNative.joinChannel(channelKey, channelName, info, uid);
        }

        /** Gets a new token when the current token expires after a period of time.
         * 
         * The `token` expires after a period of time once the token schema is enabled when:
         * - The SDK triggers the {@link agora_gaming_rtc.OnTokenPrivilegeWillExpireHandler OnTokenPrivilegeWillExpireHandler} callback, or
         * - The {@link agora_gaming_rtc.OnConnectionStateChangedHandler OnConnectionStateChangedHandler} reports {@link agora_gaming_rtc.CONNECTION_CHANGED_REASON_TYPE#CONNECTION_CHANGED_TOKEN_EXPIRED CONNECTION_CHANGED_TOKEN_EXPIRED(9)}.
         * 
         * The application should call this method to get the new `token`. Failure to do so will result in the SDK disconnecting from the server.
         * 
         * @param token The new token.
         * 
         * @return
         * - 0(ERR_OK): Success.
         * - < 0: Failure.
         *   - -1(ERR_FAILED): A general error occurs (no specified reason).
         *   - -2(ERR_INALID_ARGUMENT): The parameter is invalid.
         *   - -7(ERR_NOT_INITIALIZED): The SDK is not initialized.
         */
        public int RenewToken(string token)
        {
            // save parameters
            return IRtcEngineNative.renewToken(token);
        }

         /** Allows a user to leave a channel, such as hanging up or exiting a call.
         * 
         * After joining a channel, the user must call the `LeaveChannel` method to end the call before joining another channel.
         * 
         * This method returns 0 if the user leaves the channel and destroys all resources related to the call.
         * 
         * This method call is asynchronous, and the user has not left the channel when the method call returns. Once the user leaves the channel, the SDK triggers the {@link agora_gaming_rtc.OnLeaveChannelHandler OnLeaveChannelHandler} callback.
         * 
         * A successful `LeaveChannel` method call triggers the following callbacks:
         * - The local client: `OnLeaveChannelHandler`
         * - The remote client: {@link agora_gaming_rtc.OnUserOfflineHandler OnUserOfflineHandler}, if the user leaving the channel is in the Communication channel, or is a BROADCASTER in the Live Broadcast profile.
         * 
         * @note
         * - If you call the {@link agora_gaming_rtc.IRtcEngine.Destroy Destroy} method immediately after the `LeaveChannel` method, the `LeaveChannel` process interrupts, and the `OnLeaveChannelHandler` callback is not triggered.
         * - If you call the `LeaveChannel` method during a CDN live streaming, the SDK triggers the {@link agora_gaming_rtc.IRtcEngine.RemovePublishStreamUrl RemovePublishStreamUrl} method.
         * 
         * 
         * @return
         * - 0(ERR_OK): Success.
         * - < 0: Failure.
         *   - -1(ERR_FAILED): A general error occurs (no specified reason).
         *   - -2(ERR_INALID_ARGUMENT): The parameter is invalid.
         *   - -7(ERR_NOT_INITIALIZED): The SDK is not initialized.
         */
        public int LeaveChannel()
        {
            return IRtcEngineNative.leaveChannel(); // leave uncondionally
        }

        public void Pause()
        {
            Debug.Log("Pause engine");
            DisableAudio();
            DisableVideo();
        }

        public void Resume()
        {
            Debug.Log("Resume engine");
            EnableAudio();
            EnableVideo();
        }

        /** Provides the technical preview functionalities or special customizations by configuring the SDK with JSON options.
         *
         * @param parameters The set parameters in a JSON string.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetParameters(string parameters)
        {
            return IRtcEngineNative.setParameters(parameters);
        }

        public int SetParameter(string parameter, int value)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return IRtcEngineNative.setWebParametersInt(parameter, value);
#else
            string parameters = doFormat("{{\"{0}\": {1}}}", parameter, value);
            return IRtcEngineNative.setParameters(parameters);
#endif
        }

        public int SetParameter(string parameter, double value)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return IRtcEngineNative.setWebParametersDouble(parameter, value);
#else
            string parameters = doFormat("{{\"{0}\": {1}}}", parameter, value);
            return IRtcEngineNative.setParameters(parameters);
#endif
        }

        public int SetParameter(string parameter, bool value)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return IRtcEngineNative.setWebParametersBool(parameter, value);
#else
            string boolValue = value ? "true" : "false";
            string parameters = doFormat("{{\"{0}\": {1}}}", parameter, boolValue);
            return IRtcEngineNative.setParameters(parameters);
#endif
        }

#if !UNITY_EDITOR && UNITY_WEBGL
        public int SetParameterString(string parameter, string value)
        {
            return IRtcEngineNative.setWebParametersString(parameter, value);
        }
#endif

        /** Retrieves the current call ID.
         * 
         * When a user joins a channel on a client, a `callId` is generated to identify the call from the client. Feedback methods, such as {@link agora_gaming_rtc.IRtcEngine.Rate Rate} and {@link agora_gaming_rtc.IRtcEngine.Complain Complain}, must be called after the call ends to submit feedback to the SDK.
         * 
         * The `Rate` and `Complain` methods require the `callId` parameter retrieved from the `GetCallId` method during a call. `callId` is passed as an argument into the `Rate` and `Complain` methods after the call ends.
         * 
         * @note Ensure that you call this method after joining a channel.
         *
         * @return
         * - &ge; 0: The current call ID, if this method call succeeds.
         * - < 0: Failure.
         */
        public string GetCallId()
        {
            string s = null;
            IntPtr res = IRtcEngineNative.getCallId();
            if (res != IntPtr.Zero)
            {
                s = Marshal.PtrToStringAnsi(res);
                IRtcEngineNative.freeObject(res);
            }
            return s;
        }

        /** Allows a user to rate a call after the call ends.
         *
         * @note Ensure that you call this method after joining a channel.
         *
         * @param callId The ID of the call, retrieved from the {@link agora_gaming_rtc.IRtcEngine.GetCallId GetCallId} method.
         * @param rating  Rating of the call. The value is between 1 (lowest score) and 5 (highest score). If you set a value out of this range, the `ERR_INVALID_ARGUMENT(2)` error returns.
         * @param desc (Optional) The description of the rating, with a string length of less than 800 bytes.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int Rate(string callId, int rating, string desc = "")
        {
            return IRtcEngineNative.rate(callId, rating, desc);
        }

        /** Allows a user to complain about the call quality after a call ends.
         *
         * @note Ensure that you call this method after joining a channel.
         *
         * @param callId The ID of the call, retrieved from the {@link agora_gaming_rtc.IRtcEngine.GetCallId GetCallId} method.
         * @param desc (Optional) The description of the rating, with a string length of less than 800 bytes.
         *         
         * @return
         * - 0: Success.
         * - < 0: Failure.        
         */
        public int Complain(string callId, string desc = "")
        {
            return IRtcEngineNative.complain(callId, desc);
        }

        /** Enables the audio module.
         * 
         * The audio mode is enabled by default.
         * 
         * @note
         * - This method affects the audio module and can be called after the {@link agora_gaming_rtc.IRtcEngine.LeaveChannel LeaveChannel} method. You can call this method either before or after joining a channel.
         * - This method enables the audio module and takes some time to take effect. Agora recommend using the following API methods to control the audio modules separately:
         *  - {@link agora_gaming_rtc.IRtcEngine.EnableLocalAudio EnableLocalAudio}: Whether to enable the microphone to create the local audio stream.
         *  - {@link agora_gaming_rtc.IRtcEngine.MuteLocalAudioStream MuteLocalAudioStream}: Whether to publish the local audio stream.
         *  - {@link agora_gaming_rtc.IRtcEngine.MuteRemoteAudioStream MuteRemoteAudioStream}: Whether to subscribe to and play the remote audio stream.
         *  - {@link agora_gaming_rtc.IRtcEngine.MuteAllRemoteAudioStreams MuteAllRemoteAudioStreams}: Whether to subscribe to and play all remote audio streams.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int EnableAudio()
        {
            return IRtcEngineNative.enableAudio();
        }

        /** Disables the audio module.
         * 
         * @note
         * - This method affects the internal engine and can be called after the {@link agora_gaming_rtc.IRtcEngine.LeaveChannel LeaveChannel} method. You can call this method either before or after joining a channel.
         * - This method resets the internal engine and takes some time to take effect. We recommend using the following API methods to control the audio engine modules separately:
         *  - {@link agora_gaming_rtc.IRtcEngine.EnableLocalAudio EnableLocalAudio}: Whether to enable the microphone to create the local audio stream.
         *  - {@link agora_gaming_rtc.IRtcEngine.MuteLocalAudioStream MuteLocalAudioStream}: Whether to publish the local audio stream.
         *  - {@link agora_gaming_rtc.IRtcEngine.MuteRemoteAudioStream MuteRemoteAudioStream}: Whether to subscribe to and play the remote audio stream.
         *  - {@link agora_gaming_rtc.IRtcEngine.MuteAllRemoteAudioStreams MuteAllRemoteAudioStreams}: Whether to subscribe to and play all remote audio streams.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int DisableAudio()
        {
            return IRtcEngineNative.disableAudio();
        }

        /** Stops or resumes publishing the local audio stream.
         * 
         * @note
         * - When `mute` is set as true, this method does not affect any ongoing audio recording, because it does not disable the microphone.
         * - You can call this method either before or after joining a channel. If you call {@link agora_gaming_rtc.IRtcEngine.SetChannelProfile SetChannelProfile} after this method, the SDK resets whether or not to stop publishing the local audio according to the channel profile and user role. Therefore, we recommend calling this method after the `SetChannelProfile` method.
         *
         * @param mute Sets whether to stop publishing the local audio stream.
         * - true: Stops publishing the local audio stream.
         * - false: (Default) Resumes publishing the local audio stream.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int MuteLocalAudioStream(bool mute)
        {
            return IRtcEngineNative.muteLocalAudioStream(mute);
        }

        /** Stops or resumes subscribing to the audio streams of all remote users.
         * 
         * As of v3.3.1, after successfully calling this method, the local user stops or resumes subscribing to the 
         * audio streams of all remote users, including all subsequent users.
         *
         * @note
         * - Call this method after joining a channel.
         * - See recommended settings in *Set the Subscribing Status*.
         *
         * @param mute Sets whether to stop subscribing to the audio streams of all remote users.
         * - true: Stop subscribing to the audio streams of all remote users.
         * - false: (Default) Resume subscribing to the audio streams of all remote users.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int MuteAllRemoteAudioStreams(bool mute)
        {
            return IRtcEngineNative.muteAllRemoteAudioStreams(mute);
        }

        /** Stops or resumes subscribing to the audio stream of a specified user.
         * 
         * @note
         * - Call this method after joining a channel.
         * - See recommended settings in *Set the Subscribing State*.
         * 
         * @param uid The user ID of the specified remote user.
         * @param mute Sets whether to stop subscribing to the audio stream of a specified user.
         * - true: Stop subscribing to the audio stream of a specified user.
         * - false: (Default) Resume subscribing to the audio stream of a specified user.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int MuteRemoteAudioStream(uint uid, bool mute)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.muteRemoteAudioStream_WGLM(""+uid, mute);
            return 0;
#else
            return IRtcEngineNative.muteRemoteAudioStream(uid, mute);
#endif

        }

        /** Enables/Disables the audio playback route to the speakerphone.
         * 
         * This method sets whether the audio is routed to the speakerphone or earpiece.
         * 
         * See the default audio route explanation in the {@link agora_gaming_rtc.IRtcEngine.SetDefaultAudioRouteToSpeakerphone SetDefaultAudioRouteToSpeakerphone} method and check whether it is necessary to call this method.
         * 
         * On Android, settings of {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile} and {@link agora_gaming_rtc.IRtcEngine.SetChannelProfile SetChannelProfile} affect the call result of `SetEnableSpeakerphone`. The following are scenarios where `SetEnableSpeakerphone` does not take effect:
         * - If you set `scenario` as `AUDIO_SCENARIO_GAME_STREAMING`, no user can change the audio playback route.
         * - If you set `scenario` as `AUDIO_SCENARIO_DEFAULT` or `AUDIO_SCENARIO_SHOWROOM`, the audience cannot change the audio playback route. If there is only one host is in the channel, the host cannot change the audio playback route either.
         * - If you set `scenario` as `AUDIO_SCENARIO_EDUCATION`, the audience cannot change the audio playback route.
         *
         * @note 
         * - This method is for Android and iOS only.
         * - Ensure that you have successfully called the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method before calling this method.
         * - After calling this method, the SDK returns the {@link agora_gaming_rtc.OnAudioRouteChangedHandler OnAudioRouteChangedHandler} callback to indicate the changes.
         * - This method does not take effect if a headset is used.
         * 
         * @param speakerphone Sets whether to route the audio to the speakerphone or earpiece:
         * - true: Route the audio to the speakerphone. If the playback device connects to the earpiece or Bluetooth, the audio cannot be routed to the speakerphone.
         * - false: Route the audio to the earpiece. If a headset is plugged in, the audio is routed to the headset.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetEnableSpeakerphone(bool speakerphone)
        {
            return IRtcEngineNative.setEnableSpeakerphone(speakerphone);
        }

        /** Sets the default audio playback route.
        *
        * This method sets whether the received audio is routed to the earpiece or speakerphone by default before joining a channel. If a user does not call this method, the audio is routed to the earpiece by default. If you need to change the default audio route after joining a channel, call the {@link agora_gaming_rtc.IRtcEngine.SetEnableSpeakerphone SetEnableSpeakerphone} method.
        * 
        * The default setting for each profile:
        * - Communication: 
        *   - In a voice call, the default audio route is the earpiece. 
        *   - In a video call, the default audio route is the speakerphone. If a user who is in the Communication profile calls the {@link agora_gaming_rtc.IRtcEngine.DisableVideo DisableVideo} method or if the user calls the {@link agora_gaming_rtc.IRtcEngine.MuteLocalVideoStream MuteLocalVideoStream} and {@link agora_gaming_rtc.IRtcEngine.MuteAllRemoteVideoStreams MuteAllRemoteVideoStreams} methods, the default audio route switches back to the earpiece automatically.
        * - Live Broadcast: Speakerphone.
        * 
        * @note 
        * - This method is for Android and iOS only.
        * - This method is applicable only to the Communication profile.
        * - For iOS, this method only works in a voice call.
        * - Call this method before calling the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method.
        * 
        * @param speakerphone Sets the default audio route:
        * - true: Route the audio to the speakerphone. If the playback device connects to the earpiece or Bluetooth, the audio cannot be routed to the speakerphone.
        * - false: (Default) Route the audio to the earpiece. If a headset is plugged in, the audio is routed to the headset.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public int SetDefaultAudioRouteToSpeakerphone(bool speakerphone)
        {
            return IRtcEngineNative.setDefaultAudioRoutetoSpeakerphone(speakerphone);
        }

        /** Checks whether the speakerphone is enabled.
         *
         * @note
         * - This method is for Android and iOS only.
         * - You can call this method either before or after joining a channel.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public bool IsSpeakerphoneEnabled()
        {
            return IRtcEngineNative.isSpeakerphoneEnabled();
        }

        /** Switches between front and rear cameras.
        * 
        * @note
        * - This method is for Android and iOS only.
        * - Ensure that you call this method after the camera starts, for example, by
        * calling {@link agora_gaming_rtc.IRtcEngine.StartPreview StartPreview} or {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey}.
        *
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public int SwitchCamera()
        {
            return IRtcEngineNative.switchCamera();
        }

        /** Sets the video profile.
        * 
        * @deprecated This method is deprecated as of v2.3. Use the {@link agora_gaming_rtc.IRtcEngine.SetVideoEncoderConfiguration SetVideoEncoderConfiguration} method instead.
        * 
        * Each video profile includes a set of parameters, such as the resolution, frame rate, and bitrate. If the camera device does not support the specified resolution, the SDK automatically chooses a suitable camera resolution, keeping the encoder resolution specified by the `setVideoProfile` method.
        * 
        * @note
        * - If you do not need to set the video profile after joining the channel, call this method before the {@link agora_gaming_rtc.IRtcEngine.EnableVideo EnableVideo} method to reduce the render time of the first video frame.
        * - Always set the video profile before calling the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} or {@link agora_gaming_rtc.IRtcEngine.StartPreview StartPreview} method.
        * - Since the landscape or portrait mode of the output video can be decided directly by the video profile, We recommend setting *swapWidthAndHeight* to *false* (default).
        * 
        * @param profile Sets the video profile. See #VIDEO_PROFILE_TYPE.
        * @param swapWidthAndHeight Sets whether to swap the width and height of the video stream:
        * - true: Swap the width and height.
        * - false: (Default) Do not swap the width and height. The width and height of the output video are consistent with the set video profile.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public int SetVideoProfile(VIDEO_PROFILE_TYPE profile, bool swapWidthAndHeight = false)
        {
            return IRtcEngineNative.setVideoProfile((int)profile, swapWidthAndHeight);
        }

        /** Stops or resumes publishing the local video stream.
         * 
         * A successful `MuteLocalVideoStream` method call triggers the {@link agora_gaming_rtc.OnUserMuteVideoHandler OnUserMuteVideoHandler} callback on the remote client.
         * 
         * @note 
         * - This method executes faster than the {@link agora_gaming_rtc.IRtcEngine.EnableLocalVideo EnableLocalVideo} method, which controls the sending of the local video stream.
         * - When `mute` is set as `true`, this method does not affect any ongoing video recording, because it does not disable the camera.
         * - You can call this method either before or after joining a channel. If you call {@link agora_gaming_rtc.IRtcEngine.SetChannelProfile SetChannelProfile} after this method, the SDK resets whether or not to stop publishing the local video according to the channel profile and user role. Therefore, Agora recommends calling this method after the `SetChannelProfile` method.
         * 
         * @param mute Sets whether to stop publishing the local video stream.
         * - true: Stops publishing the local video stream.
         * - false: (Default) Resumes publishing the local video stream.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int MuteLocalVideoStream(bool mute)
        {
            return IRtcEngineNative.muteLocalVideoStream(mute);
        }

        /** Stops or resumes subscribing to the video streams of all remote users.
         * 
         * As of v3.3.1, after successfully calling this method, the local user stops or resumes subscribing to the 
         * video streams of all remote users, including all subsequent users.
         *
         * @note
         * - Call this method after joining a channel.
         * - See recommended settings in *Set the Subscribing State*.
         *
         * @param mute Sets whether to stop subscribing to the video streams of all remote users.
         * - true: Stop subscribing to the video streams of all remote users.
         * - false: (Default) Resume subscribing to the video streams of all remote users.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int MuteAllRemoteVideoStreams(bool mute)
        {
            return IRtcEngineNative.muteAllRemoteVideoStreams(mute);
        }

        /** Stops or resumes subscribing to the video stream of a specified user.
         * 
         * @note
         * - Call this method after joining a channel.
         * - See recommended settings in *Set the Subscribing State*.
         * 
         * @param uid The user ID of the specified remote user.
         * @param mute Sets whether to stop subscribing to the video stream of a specified user.
         * - true: Stop subscribing to the video stream of a specified user.
         * - false: (Default) Resume subscribing to the video stream of a specified user.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int MuteRemoteVideoStream(uint uid, bool mute)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return IRtcEngineNative.muteRemoteVideoStream_WGLM(""+uid, mute);
            return 0;
#else
            return IRtcEngineNative.muteRemoteVideoStream(uid, mute);
#endif
        }

        /** Sets the stream mode to the single-stream (default) or dual-stream mode. (Interactive live streaming only.)
         *
         * If the dual-stream mode is enabled, the receiver can choose to receive the high stream (high-resolution and high-bitrate video stream), or the low stream (low-resolution and low-bitrate video stream).
         * 
         * @param enabled Sets the stream mode:
         * - true: Dual-stream mode.
         * - false: (Default) Single-stream mode.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.        
         */
        public int EnableDualStreamMode(bool enabled)
        {
            return IRtcEngineNative.enableDualStreamMode(enabled);
        }

        /** Sets the built-in encryption mode.
         * 
         * @deprecated Deprecated as of v3.2.0. Use the {@link agora_gaming_rtc.IRtcEngine.EnableEncryption EnableEncryption} instead.
         *
         * The Agora RTC SDK supports built-in encryption, which is set to the `aes-128-xts` mode by default. Call this method to use other encryption modes.
         * 
         * All users in the same channel must use the same encryption mode and password.
         * 
         * Refer to the information related to the AES encryption algorithm on the differences between the encryption modes.
         * 
         * @note Call the {@link agora_gaming_rtc.IRtcEngine.SetEncryptionSecret SetEncryptionSecret} method to enable the built-in encryption function before calling this method.
         * 
         * @param encryptionMode The set encryption mode:
         * - "aes-128-xts": (Default) 128-bit AES encryption, XTS mode.
         * - "aes-128-ecb": 128-bit AES encryption, ECB mode.
         * - "aes-256-xts": 256-bit AES encryption, XTS mode.
         * - "": When encryptionMode is set as `null`, the encryption mode is set as "aes-128-xts" by default.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetEncryptionMode(string encryptionMode)
        {
            return IRtcEngineNative.setEncryptionMode(encryptionMode);
        }

        /** Enables built-in encryption with an encryption password before users join a channel.
         * 
         * @deprecated Deprecated as of v3.2.0. Use the {@link agora_gaming_rtc.IRtcEngine.EnableEncryption EnableEncryption} instead.
         *
         * All users in a channel must use the same encryption password. The encryption password is automatically cleared once a user leaves the channel.
         * 
         * If an encryption password is not specified, the encryption functionality will be disabled.
         * 
         * @note 
         * - Do not use this method for CDN live streaming.
         * - For optimal transmission, ensure that the encrypted data size does not exceed the original data size + 16 bytes. 16 bytes is the maximum padding size for AES encryption.
         * 
         * @param secret The encryption password.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetEncryptionSecret(string secret)
        {
            return IRtcEngineNative.setEncryptionSecret(secret);
        }

        /** Creates a data stream.
         * 
         * @deprecated This method is deprecated from v3.3.1. Use the 
         * {@link agora_gaming_rtc.IRtcEngine.CreateDataStream(DataStreamConfig config) CreateDataStream}2 
         * method instead.
         *
         * Each user can create up to five data streams during the lifecycle of the IRtcEngine.
         * 
         * @note 
         * - Do not set `reliable` as `true` while setting `ordered` as `false`.
         * - Ensure that you call this method after joining a channel.
         * 
         * @param reliable Sets whether or not the recipients are guaranteed to receive the data stream from the sender within five seconds:
         * - true: The recipients receive the data stream from the sender within five seconds. If the recipient does not receive the data stream within five seconds, an error is reported to the application.
         * - false: There is no guarantee that the recipients receive the data stream within five seconds and no error message is reported for any delay or missing data stream.
         * @param ordered Sets whether or not the recipients receive the data stream in the sent order:
         * - true: The recipients receive the data stream in the sent order.
         * - false: The recipients do not receive the data stream in the sent order.
         * 
         * @return
         * - &ge; 0: The ID of the data stream, if this method call succeeds.
         * - < 0: Failure.
         */
        public int CreateDataStream(bool reliable, bool ordered)
        {
            return IRtcEngineNative.createDataStream(reliable, ordered);
        }

        /** Creates a data stream.
         *
         * @since v3.3.1
         *
         * Each user can create up to five data streams in a single channel.
         *
         * This method does not support data reliability. If the receiver receives a data packet five
         * seconds or more after it was sent, the SDK directly discards the data.
         *
         * @param config The configurations for the data stream: 
         * {@link agora_gaming_rtc.DataStreamConfig DataStreamConfig}.
         *
         * @return
         * - &ge; 0: The ID of the data stream, if this method call succeeds.
         * - < 0: Fails to create the data stream.
         */
        public int CreateDataStream(DataStreamConfig config)
        {
            return IRtcEngineNative.createDataStream_engine(config.syncWithAudio, config.ordered);
        }

        /** Sends data stream messages to all users in a channel.
         * 
         * The SDK has the following restrictions on this method:
         * - Up to 30 packets can be sent per second in a channel with each packet having a maximum size of 1 kB.
         * - Each client can send up to 6 kB of data per second.
         * - Each user can have up to five data streams simultaneously.
         * 
         * A successful {@link agora_gaming_rtc.IRtcEngine.SendStreamMessage SendStreamMessage} method call triggers the {@link agora_gaming_rtc.OnStreamMessageHandler OnStreamMessageHandler} callback on the remote client, from which the remote user gets the stream message.
         * 
         * A failed `SendStreamMessage` method call triggers the `OnStreamMessageHandler` callback on the remote client.
         * 
         * @note 
         * - This method applies only to the Communication profile or to the hosts in the Live-broadcast profile. If an audience in the Live-broadcast profile calls this method, the audience may be switched to a host.
         * - Ensure that you have created the data stream using {@link agora_gaming_rtc.IRtcEngine.CreateDataStream CreateDataStream} before calling this method.
         *
         * @param streamId ID of the sent data stream, returned in the `CreateDataStream` method.
         * @param data The sent data.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SendStreamMessage(int streamId, byte[] data)
        {
            return IRtcEngineNative.sendStreamMessage(streamId, data, data.Length);
        }

        /** Set the volume of the speaker. (macOS only.)
         * 
         * @deprecated This method is deprecated as of v2.3.0. Use {@link agora_gaming_rtc.IRtcEngine.AdjustRecordingSignalVolume AdjustRecordingSignalVolume} and {@link agora_gaming_rtc.IRtcEngine.AdjustPlaybackSignalVolume AdjustPlaybackSignalVolume} instead.
         * 
         * @param volume Sets the speakerphone volume. The value ranges between 0 (lowest volume) and 255 (highest volume).
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetSpeakerphoneVolume(int volume)
        {
            return IRtcEngineNative.setSpeakerphoneVolume(volume);
        }

            //only for interactive live streaming
        /** Sets the preferences for the high-quality video. (Interactive live streaming only).
         * 
         * @deprecated This method is deprecated as of v2.4.0.
         * 
         * @param preferFrameRateOverImageQuality Sets the video quality preference:
         * - true: Frame rate over image quality.
         * - false: (Default) Image quality over frame rate.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetVideoQualityParameters(bool preferFrameRateOverImageQuality)
        {
            return IRtcEngineNative.setVideoQualityParameters(preferFrameRateOverImageQuality);
        }

        /** Starts an audio call test.
         * 
         * @deprecated This method is deprecated as of v2.4.0.
         * 
         * This method starts an audio call test to check whether the audio devices (for example, headset and speaker) and the network connection are working properly.
         * 
         * To conduct the test:
         * - The user speaks and the capturing is played back within 10 seconds.
         * - If the user can hear the capturing within 10 seconds, the audio devices and network connection are working properly.
         * 
         * @note
         * - After calling this method, always call the {@link agora_gaming_rtc.IRtcEngine.StopEchoTest StopEchoTest} method to end the test. Otherwise, the application cannot run the next echo test.
         * - In the Live-broadcast profile, only the hosts can call this method. If the user switches from the Communication to Live-broadcast profile, the user must call the {@link agora_gaming_rtc.IRtcEngine.SetClientRole SetClientRole} method to change the user role from the audience (default) to the host before calling this method.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int StartEchoTest()
        {
            return IRtcEngineNative.startEchoTest();
        }

        /** Starts an audio call test.
         * 
         * This method starts an audio call test to determine whether the audio devices (for example, headset and speaker) and the network connection are working properly.
         * 
         * In the audio call test, you record your voice. If the capturing plays back within the set time interval, the audio devices and the network connection are working properly.
         * 
         * @note 
         * - Call this method before joining a channel.
         * - After calling this method, call the {@link agora_gaming_rtc.IRtcEngine.StopEchoTest StopEchoTest} method to end the test. Otherwise, the app cannot run the next echo test, or call the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method.
         * - In the Live-broadcast profile, only a host can call this method.
         * 
         * @param intervalInSeconds The time interval (sec) between when you speak and when the capturing plays back.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int StartEchoTest(int intervalInSeconds)
        {
            return IRtcEngineNative.startEchoTest2(intervalInSeconds);
        }

        /** Stops the audio call test.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int StopEchoTest()
        {
            return IRtcEngineNative.stopEchoTest();
        }

        /** Starts the last-mile network probe test before joining a channel to get the uplink and downlink last-mile network statistics, including the bandwidth, packet loss, jitter, and round-trip time (RTT).
         * 
         * Once this method is enabled, the SDK returns the following callbacks:
         * - {@link agora_gaming_rtc.OnLastmileQualityHandler OnLastmileQualityHandler}: the SDK triggers this callback within two seconds depending on the network conditions. This callback rates the network conditions and is more closely linked to the user experience.
         * - {@link agora_gaming_rtc.OnLastmileProbeResultHandler OnLastmileProbeResultHandler}: the SDK triggers this callback within 30 seconds depending on the network conditions. This callback returns the real-time statistics of the network conditions and is more objective.
         * 
         * Call this method to check the uplink network quality before users join a channel or before an audience switches to a host.
         *
         * @note 
         * - This method consumes extra network traffic and may affect communication quality. We do not recommend calling this method together with {@link agora_gaming_rtc.IRtcEngine.EnableLastmileTest EnableLastmileTest}. 
         * - Do not call other methods before receiving the `OnLastmileQualityHandler` and `OnLastmileProbeResultHandler` callbacks. Otherwise, the callbacks may be interrupted.
         * - In the Live-broadcast profile, a host should not call this method after joining a channel.
         * 
         * @param lastmileProbeConfig Sets the configurations of the last-mile network probe test. See LastmileProbeConfig.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int StartLastmileProbeTest(LastmileProbeConfig lastmileProbeConfig)
        {
            return IRtcEngineNative.startLastmileProbeTest(lastmileProbeConfig.probeUplink, lastmileProbeConfig.probeDownlink, lastmileProbeConfig.expectedUplinkBitrate, lastmileProbeConfig.expectedDownlinkBitrate);
        }

        /** Stops the last-mile network probe test.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int StopLastmileProbeTest()
        {
            return IRtcEngineNative.stopLastmileProbeTest();
        }

        /** Adds a watermark image to the local video or CDN live stream.
         * 
         * @deprecated This method is deprecated from v2.9.1. Use {@link agora_gaming_rtc.IRtcEngine.AddVideoWatermark(string watermarkUrl, WatermarkOptions watermarkOptions) AddVideoWatermark} instead.
         * 
         * This method adds a PNG watermark image to the local video stream for the capturing device, channel audience, and CDN live audience to view and capture.
         * 
         * To add the PNG file to the CDN live publishing stream, see the {@link agora_gaming_rtc.IRtcEngine.SetLiveTranscoding SetLiveTranscoding} method.
         * 
         * @note
         * - The URL descriptions are different for the local video and CDN live streams:
         *     - In a local video stream, `url` in RtcImage refers to the absolute path of the added watermark image file in the local video stream.
         *     - In a CDN live stream, `url` in RtcImage refers to the URL address of the added watermark image in the CDN live broadcast.
         * - The source file of the watermark image must be in the PNG file format. If the width and height of the PNG file differ from your settings in this method, the PNG file will be cropped to conform to your settings.
         * - The Agora RTC SDK supports adding only one watermark image onto a local video or CDN live stream. The newly added watermark image replaces the previous one.
         * 
         * @param rtcImage The watermark image to be added to the local video stream. See RtcImage.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int AddVideoWatermark(RtcImage rtcImage)
        {
            return IRtcEngineNative.addVideoWatermark(rtcImage.url, rtcImage.x, rtcImage.y, rtcImage.width, rtcImage.height);
        }

        /** Adds a watermark image to the local video.
         * 
         * This method adds a PNG watermark image to the local video in the interactive live streaming. Once the watermark image is added, all the audience in the channel (CDN audience included), 
         * and the capturing device can see and capture it. Agora supports adding only one watermark image onto the local video, and the newly watermark image replaces the previous one.
         * 
         * The watermark position depends on the settings in the {@link agora_gaming_rtc.IRtcEngine.SetVideoEncoderConfiguration SetVideoEncoderConfiguration} method:
         * - If the orientation mode of the encoding video is {@link agora_gaming_rtc.ORIENTATION_MODE#ORIENTATION_MODE_FIXED_LANDSCAPE ORIENTATION_MODE_FIXED_LANDSCAPE(1)}, or the landscape mode in {@link agora_gaming_rtc.ORIENTATION_MODE#ORIENTATION_MODE_ADAPTIVE ORIENTATION_MODE_ADAPTIVE(0)}, the watermark uses the landscape orientation.
         * - If the orientation mode of the encoding video is {@link agora_gaming_rtc.ORIENTATION_MODE#ORIENTATION_MODE_FIXED_PORTRAIT ORIENTATION_MODE_FIXED_PORTRAIT(2)}, or the portrait mode in {@link agora_gaming_rtc.ORIENTATION_MODE#ORIENTATION_MODE_ADAPTIVE ORIENTATION_MODE_ADAPTIVE(0)}, the watermark uses the portrait orientation.
         * - When setting the watermark position, the region must be less than the dimensions set in the `setVideoEncoderConfiguration` method. Otherwise, the watermark image will be cropped.
         * 
         * @note
         * - Ensure that you have called the {@link agora_gaming_rtc.IRtcEngine.EnableVideo EnableVideo} method to enable the video module before calling this method.
         * - If you only want to add a watermark image to the local video for the audience in the CDN live broadcast channel to see and capture, you can call this method or the {@link agora_gaming_rtc.IRtcEngine.SetLiveTranscoding SetLiveTranscoding} method.
         * - This method supports adding a watermark image in the PNG file format only. Supported pixel formats of the PNG image are RGBA, RGB, Palette, Gray, and Alpha_gray.
         * - If the dimensions of the PNG image differ from your settings in this method, the image will be cropped or zoomed to conform to your settings.
         * - If you have enabled the local video preview by calling the {@link agora_gaming_rtc.IRtcEngine.StartPreview StartPreview} method, you can use the `visibleInPreview` member in the WatermarkOptions class to set whether or not the watermark is visible in preview.
         * - If you have enabled the mirror mode for the local video, the watermark on the local video is also mirrored. To avoid mirroring the watermark, Agora recommends that you do not use the mirror and watermark functions for the local video at the same time. You can implement the watermark function in your application layer.
         *
         * @param watermarkUrl The local file path of the watermark image to be added. This method supports adding a watermark image from the local absolute or relative file path.
         * @param watermarkOptions The watermark's options to be added. See WatermarkOptions for more infomation.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int AddVideoWatermark(string watermarkUrl, WatermarkOptions watermarkOptions)
        {
            return IRtcEngineNative.addVideoWatermark2(watermarkUrl, watermarkOptions.visibleInPreview, watermarkOptions.positionInLandscapeMode.x, watermarkOptions.positionInLandscapeMode.y, watermarkOptions.positionInLandscapeMode.width, watermarkOptions.positionInLandscapeMode.height, watermarkOptions.positionInPortraitMode.x, watermarkOptions.positionInPortraitMode.y, watermarkOptions.positionInPortraitMode.width, watermarkOptions.positionInPortraitMode.height);
        }

        /** Removes the watermark image from the video stream added by the {@link agora_gaming_rtc.IRtcEngine.AddVideoWatermark(string watermarkUrl, WatermarkOptions watermarkOptions) AddVideoWatermark} method.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int ClearVideoWatermarks()
        {
            return IRtcEngineNative.clearVideoWatermarks();
        }

        /** Sets the stream type of the remote video.
         * 
         * Under limited network conditions, if the publisher has not disabled the dual-stream mode using {@link agora_gaming_rtc.IRtcEngine.EnableDualStreamMode EnableDualStreamMode(false)}, the receiver can choose to receive either the high-quality video stream (the high resolution, and high bitrate video stream) or the low-video stream (the low resolution, and low bitrate video stream).
         * 
         * By default, users receive the high-quality video stream. Call this method if you want to switch to the low-video stream. This method allows the app to adjust the corresponding video stream type based on the size of the video window to reduce the bandwidth and resources.
         * 
         * The aspect ratio of the low-video stream is the same as the high-quality video stream. Once the resolution of the high-quality video stream is set, the system automatically sets the resolution, frame rate, and bitrate of the low-video stream.
         * 
         * The method result returns in the {@link agora_gaming_rtc.OnApiExecutedHandler OnApiExecutedHandler} callback.
         * 
         * @note You can call this method either before or after joining a channel. If you call both `SetRemoteVideoStreamType` and
         * {@link agora_gaming_rtc.IRtcEngine.SetRemoteDefaultVideoStreamType SetRemoteDefaultVideoStreamType}, the SDK applies the settings in
         * the `SetRemoteVideoStreamType`.
         *
         * @param uid ID of the remote user sending the video stream.
         * @param streamType  Sets the video-stream type. See #REMOTE_VIDEO_STREAM_TYPE.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetRemoteVideoStreamType(uint uid, REMOTE_VIDEO_STREAM_TYPE streamType)
        {
            return IRtcEngineNative.setRemoteVideoStreamType(uid, (int)streamType);
        }

        /** Sets the mixed audio format for the {@link agora_gaming_rtc.AudioRawDataManager.OnMixedAudioFrameHandler OnMixedAudioFrameHandler} callback.
         * 
         * @note 
         * - The SDK calculates the sample interval according to the value of the `channels` of `AudioFrame`, `sampleRate`, and `samplesPerCall` parameters you set in this method. Sample interval (sec) = `samplePerCall`/(`sampleRate` &times; `channels`). Ensure that the value of sample interval is no less than 0.01. The SDK triggers the `OnMixedAudioFrameHandler` callback according to the sample interval.
         * - Ensure that you call this method before joining a channel.
         * 
         * @param sampleRate Sets the sample rate (`samplesPerSec`) returned in the `OnMixedAudioFrameHandler` callback, which can be set as 8000, 16000, 32000, 44100, or 48000 Hz.
         * @param samplesPerCall Sets the number of samples the `OnMixedAudioFrameHandler` callback returns. Set it as 1024 for RTMP or RTMPS streaming.        
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetMixedAudioFrameParameters(int sampleRate, int samplesPerCall)
        {
            return IRtcEngineNative.setMixedAudioFrameParameters(sampleRate, samplesPerCall);
        }

        /** Sets the playback position of the music file to a different starting position (the default plays from the beginning).
         * 
         * @note Ensure that this method is called after {@link agora_gaming_rtc.IRtcEngine.StartAudioMixing StartAudioMixing}.
         *
         * @param pos The playback starting position (ms) of the music file.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetAudioMixingPosition(int pos)
        {
            return IRtcEngineNative.setAudioMixingPosition(pos);
        }

        /** Enables the reporting of users' volume indication.
         * 
         * This method enables the SDK to regularly report the volume information of the local user who sends a stream and
         * remote users (up to three) whose instantaneous volumes are the highest to the app. Once you call this method and
         * users send streams in the channel, the SDK triggers the
         * {@link agora_gaming_rtc.OnVolumeIndicationHandler OnVolumeIndicationHandler} callback at the time interval set
         * in this method.
         *
         * @note You can call this method either before or after joining a channel.
         *
         * @param interval Sets the time interval between two consecutive volume indications:
         * - &le; 0: Disables the volume indication.
         * - > 0: Time interval (ms) between two consecutive volume indications. We recommend setting `interval` > 200 ms. Do not set `interval` < 10 ms, or the `OnVolumeIndicationHandler` callback will not be triggered.
         * @param smooth Smoothing factor sets the sensitivity of the audio volume indicator. The value ranges between 0 and 10. The greater the value, the more sensitive the indicator. The recommended value is 3.
         * @param report_vad 
         * - true: Enable the voice activity detection of the local user. Once it is enabled, the `vad` parameter of the `OnVolumeIndicationHandler` callback reports the voice activity status of the local user.
         * - false: (Default) Disable the voice activity detection of the local user. Once it is disabled, the `vad` parameter of the `OnVolumeIndicationHandler` callback does not report the voice activity status of the local user, except for the scenario where the engine automatically detects the voice activity of the local user.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int EnableAudioVolumeIndication(int interval, int smooth, bool report_vad = false)
        {
            return IRtcEngineNative.enableAudioVolumeIndication(interval, smooth, report_vad);
        }

        /** Adjusts adjust the capturing signal volume.
         *
         * @note You can call this method either before or after joining a channel.
         *
         * @param volume Recording volume. To avoid echoes and
         * improve call quality, Agora recommends setting the value of `volume` between
         * 0 and 100. If you need to set the value higher than 100, contact
         * support@agora.io first.
         * - 0: Mute.
         * - 100: Original volume.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int AdjustRecordingSignalVolume(int volume)
        {
            return IRtcEngineNative.adjustRecordingSignalVolume(volume);
        }

        /** Adjusts the playback signal volume of all remote users.
         * 
         * @note
         * - This method adjusts the playback signal volume which is mixed volume of all remote users.
         * - Since v2.3.2, to mute the local audio playback, call both `AdjustPlaybackSignalVolume` and {@link agora_gaming_rtc.IRtcEngine.AdjustAudioMixingVolume AdjustAudioMixingVolume}, and set `volume` as 0.
         * - You can call this method either before or after joining a channel.
         *
         * @param volume the playback signal volume of all remote users. To avoid echoes and improve call quality, Agora recommends setting the value of volume between 0 and 100. If you need to set the value higher than 100, contact support@agora.io first.
         * - 0: Mute.
         * - 100: Original volume.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int AdjustPlaybackSignalVolume(int volume)
        {
            return IRtcEngineNative.adjustPlaybackSignalVolume(volume);
        }

        /** Starts playing and mixing the music file.
         * 
         * This method mixes the specified local audio file with the audio stream from the microphone, or replaces the microphone's audio stream with the specified local audio file. You can choose whether the other user can hear the local audio playback and specify the number of playback loops. This method also supports online music playback.
         * 
         * When the audio mixing file playback finishes after calling this method, the SDK triggers the {@link agora_gaming_rtc.OnAudioMixingFinishedHandler OnAudioMixingFinishedHandler} callback.
         * 
         * A successful `StartAudioMixing` method call triggers the {@link agora_gaming_rtc.OnAudioMixingStateChangedHandler OnAudioMixingStateChangedHandler(PLAYING)} callback on the local client.
         * 
         * When the audio mixing file playback finishes, the SDK triggers the `OnAudioMixingStateChangedHandler(STOPPED)` callback on the local client.
         * 
         * @note
         * - Call this method when you are in a channel.
         * - If the local audio mixing file does not exist, or if the SDK does not support the file format or cannot access the music file URL, the SDK returns `WARN_AUDIO_MIXING_OPEN_ERROR(701)`.
         * 
         * @param filePath The absolute path (including the suffixes of the filename) of the local or online audio file to mix. Supported audio formats: mp3, mp4, m4a, aac, 3gp, mkv and wav. For more information, see [Supported Media Formats in Media Foundation](https://docs.microsoft.com/en-us/windows/win32/medfound/supported-media-formats-in-media-foundation).
         * @param loopback Sets which user can hear the audio mixing:
         * - true: Only the local user can hear the audio mixing.
         * - false: Both users can hear the audio mixing.
         * @param replace Sets the audio mixing content:
         * - true: Only the specified audio file is published; the audio stream received by the microphone is not published.
         * - false: The local audio file is mixed with the audio stream from the microphone.
         * @param cycle Sets the number of playback loops:
         * - Positive integer: Number of playback loops.
         * - -1: Infinite playback loops.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int StartAudioMixing(string filePath, bool loopback, bool replace, int cycle)
        {
            return IRtcEngineNative.startAudioMixing(filePath, loopback, replace, cycle);
        }

        public int StartAudioMixing(string filePath, bool loopback, bool replace, int cycle, int startPos)
        {
            return IRtcEngineNative.startAudioMixing2(filePath, loopback, replace, cycle, startPos);
        }

        /** Stops playing and mixing the music file.
         *
         * Call this method when you are in a channel.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int StopAudioMixing()
        {
            return IRtcEngineNative.stopAudioMixing();
        }

        /** Pauses playing and mixing the music file.
         *
         * Call this method when you are in a channel.
         *
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int PauseAudioMixing()
        {
            return IRtcEngineNative.pauseAudioMixing();
        }

        /** Resumes playing and mixing the music file.
         * 
         * Call this method when you are in a channel.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int ResumeAudioMixing()
        {
            return IRtcEngineNative.resumeAudioMixing();
        }

        /** Adjusts the volume during audio mixing.
         * 
         * Call this method when you are in a channel.
         * 
         * @note
         * - Calling this method does not affect the volume of audio effect file playback invoked by the {@link agora_gaming_rtc.AudioEffectManagerImpl.PlayEffect PlayEffect} method.
         * - Ensure that this method is called after {@link agora_gaming_rtc.IRtcEngine.StartAudioMixing StartAudioMixing}.
         * 
         * @param volume Audio mixing volume. The value ranges between 0 and 100.
         * - 0: Mute.
         * - 100: Original volume.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int AdjustAudioMixingVolume(int volume)
        {
            return IRtcEngineNative.adjustAudioMixingVolume(volume);
        }

        /** Retrieves the duration (ms) of the music file.
         * 
         * Call this method when you are in a channel.
         * 
         * @return 
         * - &ge; 0: The audio mixing duration, if this method call succeeds.
         * - < 0: Failure.
         */
        public int GetAudioMixingDuration()
        {
            return IRtcEngineNative.getAudioMixingDuration();
        }

        public int GetAudioMixingDuration(string filePath)
        {
            return IRtcEngineNative.getAudioMixingDuration2(filePath);
        }

        /** Retrieves the playback position (ms) of the music file.
         * 
         * Call this method when you are in a channel.
         *
         * @return 
         * - &ge; 0: The current playback position of the audio mixing, if this method call succeeds.
         * - < 0: Failure.
         */
        public int GetAudioMixingCurrentPosition()
        {
            return IRtcEngineNative.getAudioMixingCurrentPosition();
        }

        /** Starts an audio recording.
         *
         * @deprecated Use {@link agora_gaming_rtc.IRtcEngine.StartAudioRecording(string filePath, int sampleRate, AUDIO_RECORDING_QUALITY_TYPE quality) StartAudioRecording2} instead.
         * 
         * The SDK allows recording during a call. Supported formats:
         * 
         * - .wav: Large file size with high fidelity.
         * - .aac: Small file size with low fidelity.
         * 
         * This method has a fixed sample rate of 32 kHz.
         * 
         * - Ensure that the directory to save the recording file exists and is writable.
         * - This method is usually called after the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method.
         * - The recording automatically stops when the {@link agora_gaming_rtc.IRtcEngine.LeaveChannel LeaveChannel} method is called.
         * 
         * @param filePath Pointer to the absolute file path of the recording file. The string of the file name is in UTF-8.
         * @param quality Sets the audio recording quality. See #AUDIO_RECORDING_QUALITY_TYPE.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int StartAudioRecording(string filePath, AUDIO_RECORDING_QUALITY_TYPE quality)
        {
            return IRtcEngineNative.startAudioRecording(filePath, (int)quality);
        }

        /** Starts an audio recording on the client.
         * 
         * The SDK allows recording during a call. After successfully calling this method, you can record the audio of all the users in the channel and get an audio recording file. 
         * Supported formats of the recording file are as follows:
         * - .wav: Large file size with high fidelity.
         * - .aac: Small file size with low fidelity.
         * 
         * @note
         * - Ensure that the directory you use to save the recording file exists and is writable.
         * - This method is usually called after the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method. The recording automatically stops when you call the {@link agora_gaming_rtc.IRtcEngine.LeaveChannel LeaveChannel} method.
         * - For better recording effects, set quality as {@link agora_gaming_rtc.AUDIO_RECORDING_QUALITY_TYPE#AUDIO_RECORDING_QUALITY_MEDIUM AUDIO_RECORDING_QUALITY_MEDIUM(1)} or {@link agora_gaming_rtc.AUDIO_RECORDING_QUALITY_TYPE#AUDIO_RECORDING_QUALITY_HIGH AUDIO_RECORDING_QUALITY_HIGH(2)} when `sampleRate` is 44100 Hz or 48000 Hz.
         * 
         * @param filePath Pointer to the absolute file path of the recording file. The string of the file name is in UTF-8.
         * @param sampleRate Sample rate (Hz) of the recording file. Supported values are as follows:
         * - 16000
         * - (Default) 32000
         * - 44100
         * - 48000
         * @param quality Sets the audio recording quality. See #AUDIO_RECORDING_QUALITY_TYPE.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int StartAudioRecording(string filePath, int sampleRate, AUDIO_RECORDING_QUALITY_TYPE quality)
        {
            return IRtcEngineNative.startAudioRecording2(filePath, sampleRate, (int)quality);
        }

        /** Stops an audio recording on the client.
         * 
         * You can call this method before calling the {@link agora_gaming_rtc.IRtcEngine.LeaveChannel LeaveChannel} method else, the recording automatically stops when the `LeaveChannel` method is called.
         *
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int StopAudioRecording()
        {
            return IRtcEngineNative.stopAudioRecording();
        }

        /** Retrieves the AudioEffectManagerImpl object.
         * 
         * @return The AudioEffectManagerImpl object.
         */
        public IAudioEffectManager GetAudioEffectManager()
        {
            return mAudioEffectM;
        }

        /** Retrieves the AudioRecordingDeviceManager object.
         * 
         * @return The AudioRecordingDeviceManager object.
         */
        public IAudioRecordingDeviceManager GetAudioRecordingDeviceManager()
        {
            return audioRecordingDeviceManager;
        }

        /** Retrieves the AudioPlaybackDeviceManager object.
         * 
         * @return The AudioPlaybackDeviceManager object.
         */
        public IAudioPlaybackDeviceManager GetAudioPlaybackDeviceManager()
        {
            return audioPlaybackDeviceManager;
        }

        /** Retrieves the VideoDeviceManager object.
         * 
         * @return The VideoDeviceManager object.
         */
        public IVideoDeviceManager GetVideoDeviceManager()
        {
            return videoDeviceManager;
        }

        /** Retrieves the AudioRawDataManager object.
         * 
         * @return The AudioRawDataManager object.
         */
        public IAudioRawDataManager GetAudioRawDataManager()
        {
            return audioRawDataManager;
        }

        /** Retrieves the VideoRawDataManager object.
         * 
         * @return The VideoRawDataManager object.
         */
        public IVideoRawDataManager GetVideoRawDataManager()
        {
            return videoRawDataManager;
        }

        /** Retrieves the VideoRender object.
         * 
         * @return The VideoRender object.
         */
        internal IVideoRender GetVideoRender()
        {
            return videoRender;
        }

        /** Enables the video module.
         * 
         * Call this method either before joining a channel or during a call. If this method is called before joining a channel, the call starts in the video mode. If this method is called during an audio call, the audio mode switches to the video mode. To disable the video module, call the {@link agora_gaming_rtc.IRtcEngine.DisableVideo DisableVideo} method.
         * 
         * A successful `EnableVideo` method call triggers the {@link agora_gaming_rtc.OnUserEnableVideoHandler OnUserEnableVideoHandler(true)} callback on the remote client.
         * 
         * @note 
         * - To get video raw data, call both `EnableVideo` and {@link agora_gaming_rtc.IRtcEngine.EnableVideoObserver EnableVideoObserver} methods.
         * - This method affects the internal engine and can be called after the {@link agora_gaming_rtc.IRtcEngine.LeaveChannel LeaveChannel} method.
         * - This method resets the internal engine and takes some time to take effect. We recommend using the following API methods to control the video engine modules separately:
         *  - {@link agora_gaming_rtc.IRtcEngine.EnableLocalVideo EnableLocalVideo}: Whether to enable the camera to create the local video stream.
         *  - {@link agora_gaming_rtc.IRtcEngine.MuteLocalVideoStream MuteLocalVideoStream}: Whether to publish the local video stream.
         *  - {@link agora_gaming_rtc.IRtcEngine.MuteRemoteVideoStream MuteRemoteVideoStream}: Whether to subscribe to and play the remote video stream.
         *  - {@link agora_gaming_rtc.IRtcEngine.MuteAllRemoteVideoStreams MuteAllRemoteVideoStreams}: Whether to subscribe to and play all remote video streams.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int EnableVideo()
        {
            return IRtcEngineNative.enableVideo();
        }

        /** Disables the video module.
         * 
         * This method can be called before joining a channel or during a call. If this method is called before joining a channel, the call starts in audio mode. If this method is called during a video call, the video mode switches to the audio mode. To enable the video module, call the {@link agora_gaming_rtc.IRtcEngine.EnableVideo EnableVideo} method.
         * 
         * A successful `DisableVideo` method call triggers the {@link agora_gaming_rtc.OnUserEnableVideoHandler OnUserEnableVideoHandler(false)} callback on the remote client.
         * 
         * @note
         * - To stop getting video raw data, call both `DisableVideo` and {@link agora_gaming_rtc.IRtcEngine.DisableVideoObserver DisableVideoObserver} methods.
         * - This method affects the internal engine and can be called after the {@link agora_gaming_rtc.IRtcEngine.LeaveChannel LeaveChannel} method.
         * - This method resets the internal engine and takes some time to take effect. We recommend using the following API methods to control the video engine modules separately:
         *  - {@link agora_gaming_rtc.IRtcEngine.EnableLocalVideo EnableLocalVideo}: Whether to enable the camera to create the local video stream.
         *  - {@link agora_gaming_rtc.IRtcEngine.MuteLocalVideoStream MuteLocalVideoStream}: Whether to publish the local video stream.
         *  - {@link agora_gaming_rtc.IRtcEngine.MuteRemoteVideoStream MuteRemoteVideoStream}: Whether to subscribe to and play the remote video stream.
         *  - {@link agora_gaming_rtc.IRtcEngine.MuteAllRemoteVideoStreams MuteAllRemoteVideoStreams}: Whether to subscribe to and play all remote video streams.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int DisableVideo()
        {
            return IRtcEngineNative.disableVideo();
        }

        /** Enables/Disables the local video capture.
         * 
         * This method disables or re-enables the local video capturer, and does not affect receiving the remote video stream.
         * 
         * After you call the {@link agora_gaming_rtc.IRtcEngine.EnableVideo EnableVideo} method, the local video capturer is enabled by default. You can call `EnableLocalVideo(false)` to disable the local video capturer. If you want to re-enable it, call `EnableLocalVideo(true)`.
         * 
         * After the local video capturer is successfully disabled or re-enabled, the SDK triggers the {@link agora_gaming_rtc.OnUserEnableLocalVideoHandler OnUserEnableLocalVideoHandler} callback on the remote client.
         * 
         * @note
         * - You can call this method either before or after joining a channel.
         * - This method affects the internal engine and can be called after the {@link agora_gaming_rtc.IRtcEngine.LeaveChannel LeaveChannel} method.
         * 
         * @param enabled Sets whether to disable/re-enable the local video, including the capturer, renderer, and sender:
         * - true: (Default) Re-enable the local video.
         * - false: Disable the local video. Once the local video is disabled, the remote users can no longer receive the video stream of this user, while this user can still receive the video streams of the other remote users.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int EnableLocalVideo(bool enabled)
        {
            return IRtcEngineNative.enableLocalVideo(enabled);
        }

        /** Disables/Re-enables the local audio function.
         * 
         * The audio function is enabled by default. This method disables or re-enables the local audio function, that is, to stop or restart local audio capturing.
         * 
         * This method does not affect receiving or playing the remote audio streams, and `EnableLocalAudio(false)` is applicable to scenarios where the user wants to receive remote audio streams without sending any audio stream to other users in the channel.
         * 
         * Once the local audio function is disabled or re-enabled, the SDK triggers the {@link agora_gaming_rtc.OnLocalAudioStateChangedHandler OnLocalAudioStateChangedHandler} callback,
         * which reports `LOCAL_AUDIO_STREAM_STATE_STOPPED(0)` or `LOCAL_AUDIO_STREAM_STATE_RECORDING(1)`.
         *
         * @note
         * - This method is different from the {@link agora_gaming_rtc.IRtcEngine.MuteLocalAudioStream MuteLocalAudioStream} method:
         *   - `EnableLocalAudio`: Disables/Re-enables the local audio capturing and processing. If you disable or re-enable local audio capturing using the `EnableLocalAudio` method, the local user may hear a pause in the remote audio playback.
         *   - `MuteLocalAudioStream`: Sends/Stops sending the local audio streams.
         * - You can call this method either before or after joining a channel.
         *
         * @param enabled Sets whether to disable/re-enable the local audio function:
         * - true: (Default) Re-enable the local audio function, that is, to start the local audio capturing device (for example, the microphone).
         * - false: Disable the local audio function, that is, to stop local audio capturing.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int EnableLocalAudio(bool enabled)
        {
            return IRtcEngineNative.enableLocalAudio(enabled);
        }

        /*
        public int SetupLocalVideo(VideoCanvas videoCanvas)
        {
            return IRtcEngineNative.setupLocalVideo(videoCanvas.hwnd, (int)videoCanvas.renderMode, uid, priv);
        }

        public int SetupRemoteVideo(VideoCanvas videoCanvas)
        {
            return IRtcEngineNative.setupLocalVideo(videoCanvas.hwnd, (int)videoCanvas.renderMode, uid, priv);
        }

        public int SetLocalRenderMode(RENDER_MODE_TYPE renderMode)
        {
            return IRtcEngineNative.setLocalRenderMode((int)renderMode);
        }
       
       
        public int SetRemoteRenderMode(uint userId, RENDER_MODE_TYPE renderMode)
        {
            return IRtcEngineNative.setRemoteRenderMode(userId, (int)renderMode);
        }
       
        public int setLocalVideoMirrorMode(VIDEO_MIRROR_MODE_TYPE mirrorMode)
        {
            return IRtcEngineNative.setLocalVideoMirrorMode((int)VIDEO_MIRROR_MODE_TYPE);
        }
         */

        /** Starts the local video preview before joining the channel.
         * 
         * Before calling this method, you must call the {@link agora_gaming_rtc.IRtcEngine.EnableVideo EnableVideo} method to enable video.
         * 
         * @note Once the `StartPreview` method is called to start the local video preview, if you leave the channel by calling the {@link agora_gaming_rtc.IRtcEngine.LeaveChannel LeaveChannel} method, the local video preview remains until you call the {@link agora_gaming_rtc.IRtcEngine.StopPreview StopPreview} method to disable it.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int StartPreview()
        {
            return IRtcEngineNative.startPreview();
        }

        /** Stops the local video preview and disables video.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int StopPreview()
        {
            return IRtcEngineNative.stopPreview();
        }

        /** Enables the video observer.
         * 
         * This method sends the video pictures directly to the app instead of to the traditional view renderer.
         * 
         * @note 
         * - To get video raw data, call both {@link agora_gaming_rtc.IRtcEngine.EnableVideo EnableVideo} and `EnableVideoObserver` methods.
         * - Call this method before joining a channel.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int EnableVideoObserver()
        {
            return IRtcEngineNative.enableVideoObserver();
        }

        /** Disables the video observer.
         * 
         * This method disables sending video directly to the app.
         * 
         * @note 
         * - To stop getting video raw data, call both {@link agora_gaming_rtc.IRtcEngine.DisableVideo DisableVideo} and `DisableVideoObserver` methods.
         * - Call this method after leaving the channel.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int DisableVideoObserver()
        {
            return IRtcEngineNative.disableVideoObserver();
        }

        /** Stops or resumes subscribing to the audio streams of all remote users by default.
         * 
         * @deprecated This method is deprecated from v3.3.1.
         *
         * Call this method after joining a channel. After successfully calling this method, the local user stops or 
         * resumes subscribing to the audio streams of all subsequent users.
         * 
         * @note If you need to resume subscribing to the audio streams of remote users in the channel after 
         * calling `SetDefaultMuteAllRemoteAudioStreams(true)`, do the following.
         * - If you need to resume subscribing to the audio stream of a specified user, call 
         * {@link agora_gaming_rtc.IRtcEngine.MuteRemoteAudioStream MuteRemoteAudioStream(false)}, and specify the user ID.
         * - If you need to resume subscribing to the audio stream of multiple remote users, call 
         * {@link agora_gaming_rtc.IRtcEngine.MuteRemoteAudioStream MuteRemoteAudioStream(false)} multiple times.
         *
         * @param mute Sets whether to stop subscribing to the audio streams of all remote users by default.
         * - true: Stop subscribing to the audio streams of all remote users by default.
         * - false: (Default) Resume subscribing to the audio streams of all remote users by default.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetDefaultMuteAllRemoteAudioStreams(bool mute)
        {
            return IRtcEngineNative.setDefaultMuteAllRemoteAudioStreams(mute);
        }

        /** Stops or resumes subscribing to the video streams of all remote users by default.
         * 
         * @deprecated This method is deprecated from v3.3.1.
         *
         * Call this method after joining a channel. After successfully calling this method, the local user stops or 
         * resumes subscribing to the video streams of all subsequent users.
         * 
         * @note If you need to resume subscribing to the video streams of remote users in the channel after 
         * calling `SetDefaultMuteAllRemoteVideoStreams(true)`, do the following.
         * - If you need to resume subscribing to the video stream of a specified user, call 
         * {@link agora_gaming_rtc.IRtcEngine.MuteRemoteVideoStream MuteRemoteVideoStream(false)}, and specify the user ID.
         * - If you need to resume subscribing to the video stream of multiple remote users, call 
         * {@link agora_gaming_rtc.IRtcEngine.MuteRemoteVideoStream MuteRemoteVideoStream(false)} multiple times.
         *
         * @param mute Sets whether to stop subscribing to the video streams of all remote users by default.
         * - true: Stop subscribing to the video streams of all remote users by default.
         * - false: (Default) Resume subscribing to the video streams of all remote users by default.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetDefaultMuteAllRemoteVideoStreams(bool mute)
        {
            return IRtcEngineNative.setDefaultMuteAllRemoteVideoStreams(mute);
        }

        /**  Enables the network connection quality test.
         * 
         * This method tests the quality of the users' network connections and is disabled by default.
         * 
         * Before a user joins a channel or before an audience switches to a host, call this method to check the uplink network quality.
         * 
         * This method consumes additional network traffic, and hence may affect communication quality.
         * 
         * Call the {@link agora_gaming_rtc.IRtcEngine.DisableLastmileTest DisableLastmileTest} method to disable this test after receiving the {@link agora_gaming_rtc.OnLastmileQualityHandler OnLastmileQualityHandler} callback, and before joining a channel.
         * 
         * @note
         * - Do not call any other methods before receiving the `OnLastmileQualityHandler` callback. Otherwise, the callback may be interrupted by other methods, and hence may not be triggered.
         * - A host should not call this method after joining a channel (when in a call).
         * - If you call this method to test the last-mile quality, the SDK consumes the bandwidth of a video stream, whose bitrate corresponds to the bitrate you set in the {@link agora_gaming_rtc.IRtcEngine.SetVideoEncoderConfiguration SetVideoEncoderConfiguration} method. After you join the channel, whether you have called the {@link agora_gaming_rtc.IRtcEngine.DisableLastmileTest DisableLastmileTest} method or not, the SDK automatically stops consuming the bandwidth.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int EnableLastmileTest()
        {
            return IRtcEngineNative.enableLastmileTest();
        }

        /** Disables the network connection quality test.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int DisableLastmileTest()
        {
            return IRtcEngineNative.disableLastmileTest();
        }

        /** Retrieves the connection state of the SDK.
         * 
         * @note You can call this method either before or after joining a channel.
         *
         * @return #CONNECTION_STATE_TYPE
         */
        public CONNECTION_STATE_TYPE GetConnectionState()
        {
            return (CONNECTION_STATE_TYPE)IRtcEngineNative.getConnectionState();
        }

        /** Sets the audio parameters and application scenarios.
        * 
        * @note
        * - The `SetAudioProfile` method must be called before the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method.
        * - In the Communication and Live-broadcast profiles, the bitrate may be different from your settings due to network self-adaptation.
        * - In scenarios requiring high-quality audio, we recommend setting profile as {@link agora_gaming_rtc.AUDIO_PROFILE_TYPE#AUDIO_PROFILE_MUSIC_HIGH_QUALITY AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)} and scenario as {@link agora_gaming_rtc.AUDIO_SCENARIO_TYPE#AUDIO_SCENARIO_GAME_STREAMING AUDIO_SCENARIO_GAME_STREAMING(3)}. For example, for music education scenarios.
        * 
        * @param audioProfile Sets the sample rate, bitrate, encoding mode, and the number of channels. See #AUDIO_PROFILE_TYPE.
        * @param scenario Sets the audio application scenario. See #AUDIO_SCENARIO_TYPE. Under different audio scenarios, the device uses different volume tracks, i.e. either the in-call volume or the media volume. For details, see [What is the difference between the in-call volume and the media volume?](https://docs.agora.io/en/faq/system_volume).
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public int SetAudioProfile(AUDIO_PROFILE_TYPE audioProfile, AUDIO_SCENARIO_TYPE scenario)
        {
            return IRtcEngineNative.setAudioProfile((int)audioProfile, (int)scenario);
        }

        /** Sets the video encoder configuration.
         *
         * Each video encoder configuration corresponds to a set of video parameters, including the resolution, frame rate, bitrate, and video orientation.
         *
         * The parameters specified in this method are the maximum values under ideal network conditions. If the video engine cannot render the video using the specified parameters due to poor network conditions, the parameters further down the list are considered until a successful configuration is found.
         * 
         * @note
         * - You can call this method either before or after joining a channel.
         * - If you do not need to set the video encoder configuration after joining the channel, you can call this method before the {@link agora_gaming_rtc.IRtcEngine.EnableVideo EnableVideo} method to reduce the render time of the first video frame.
         *
         * @param configuration Sets the local video encoder configuration. See VideoEncoderConfiguration.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetVideoEncoderConfiguration(VideoEncoderConfiguration configuration)
        {                                                                      
            return IRtcEngineNative.setVideoEncoderConfiguration(configuration.dimensions.width, configuration.dimensions.height, (int)configuration.frameRate, configuration.minFrameRate, configuration.bitrate, configuration.minBitrate, (int)configuration.orientationMode, (int)configuration.degradationPreference, (int)configuration.mirrorMode);
        }

        /** Adjusts the audio mixing volume for local playback.
         * 
         * @note Ensure that this method is called after {@link agora_gaming_rtc.IRtcEngine.StartAudioMixing StartAudioMixing}.
         * 
         * @param volume Audio mixing volume for local playback. The value ranges between 0 and 100 (default).
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int AdjustAudioMixingPlayoutVolume(int volume)
        {
            return IRtcEngineNative.adjustAudioMixingPlayoutVolume(volume);
        }

        /** Adjusts the audio mixing volume for publishing (for remote users).
         * 
         * @note Ensure that this method is called after {@link agora_gaming_rtc.IRtcEngine.StartAudioMixing StartAudioMixing}.
         * 
         * @param volume Audio mixing volume for publishing. The value ranges between 0 and 100 (default).
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int AdjustAudioMixingPublishVolume(int volume)
        {
            return IRtcEngineNative.adjustAudioMixingPublishVolume(volume);
        }

        /** Sets the volume of a specified audio effect.
         * 
         * @note Ensure that this method is called after {@link agora_gaming_rtc.AudioEffectManagerImpl.PlayEffect PlayEffect}.
         * 
         * @param soundId ID of the audio effect. Each audio effect has a unique ID.
         * @param volume Sets the volume of the specified audio effect. The value ranges between 0 and 100 (default).
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetVolumeOfEffect(int soundId, int volume)
        {
            return IRtcEngineNative.setVolumeOfEffect(soundId, volume);
        }

        /** Sets the audio capturing format for the {@link agora_gaming_rtc.AudioRawDataManager.OnRecordAudioFrameHandler OnRecordAudioFrameHandler} callback.
         * 
         * @note 
         * - The SDK calculates the sample interval according to the value of the `sampleRate`, `channel`, and `samplesPerCall` parameters you set in this method. Sample interval (sec) = `samplePerCall`/(`sampleRate` &times; `channel`). Ensure that the value of sample interval is no less than 0.01. The SDK triggers the `OnRecordAudioFrameHandler` callback according to the sample interval.
         * - Ensure that you call this method before joining a channel.
         *
         * @param sampleRate Sets the sample rate returned in the `OnRecordAudioFrameHandler` callback, which can be set as 8000, 16000, 32000, 44100, or 48000 Hz.
         * @param channel Sets the number of audio channels returned in the `OnRecordAudioFrameHandler` callback:
         * - 1: Mono
         * - 2: Stereo
         * @param mode Sets the use mode (see #RAW_AUDIO_FRAME_OP_MODE_TYPE) of the `OnRecordAudioFrameHandler` callback.
         * @param samplesPerCall Sets the number of samples the `OnRecordAudioFrameHandler` callback returns. Set it as 1024 for RTMP or RTMPS streaming.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetRecordingAudioFrameParameters(int sampleRate, int channel, RAW_AUDIO_FRAME_OP_MODE_TYPE mode, int samplesPerCall)
        {
            return IRtcEngineNative.setRecordingAudioFrameParameters(sampleRate, channel, (int)mode, samplesPerCall);
        }

         /** Sets the audio playback format for the {@link agora_gaming_rtc.AudioRawDataManager.OnPlaybackAudioFrameHandler OnPlaybackAudioFrameHandler} callback.
         * 
         * @note 
         * - The SDK calculates the sample interval according to the value of the `sampleRate`, `channel`, and `samplesPerCall` parameters you set in this method. Sample interval (sec) = `samplePerCall`/(`sampleRate` &times; `channel`). Ensure that the value of sample interval is no less than 0.01. The SDK triggers the `OnPlaybackAudioFrameHandler` callback according to the sample interval.
         * - Ensure that you call this method before joining a channel.
         *
         * @param sampleRate Sets the sample rate returned in the `OnPlaybackAudioFrameHandler` callback, which can be set as 8000, 16000, 32000, 44100, or 48000 Hz.
         * @param channel Sets the number of channels returned in the `OnPlaybackAudioFrameHandler` callback:
         * - 1: Mono
         * - 2: Stereo
         * @param mode Sets the use mode (see #RAW_AUDIO_FRAME_OP_MODE_TYPE) of the `OnPlaybackAudioFrameHandler` callback.
         * @param samplesPerCall Sets the number of samples the `OnPlaybackAudioFrameHandler` callback returns. Set it as 1024 for RTMP or RTMPS streaming.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetPlaybackAudioFrameParameters(int sampleRate, int channel, RAW_AUDIO_FRAME_OP_MODE_TYPE mode, int samplesPerCall)
        {
            return IRtcEngineNative.setPlaybackAudioFrameParameters(sampleRate, channel, (int)mode, samplesPerCall);
        }  

         /** Sets the fallback option for the locally published video stream based on the network conditions.
         * 
         * If `option` is set as {@link agora_gaming_rtc.STREAM_FALLBACK_OPTIONS#STREAM_FALLBACK_OPTION_AUDIO_ONLY STREAM_FALLBACK_OPTION_AUDIO_ONLY(2)}, the SDK will:
         * 
         * - Disable the upstream video but enable audio only when the network conditions deteriorate and cannot support both video and audio.
         * - Re-enable the video when the network conditions improve.
         * 
         * When the locally published video stream falls back to audio only or when the audio-only stream switches back to the video, the SDK triggers the {@link agora_gaming_rtc.OnLocalPublishFallbackToAudioOnlyHandler OnLocalPublishFallbackToAudioOnlyHandler} callback.
         * 
         * @note
         * - Agora does not recommend using this method for CDN live streaming, because the remote CDN live user will have a noticeable lag when the locally published video stream falls back to audio only.
         * - Ensure that you call this method before joining a channel.
         *
         * @param option Sets the fallback option for the locally published video stream. See #STREAM_FALLBACK_OPTIONS.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetLocalPublishFallbackOption(STREAM_FALLBACK_OPTIONS option)
        {
            return IRtcEngineNative.setLocalPublishFallbackOption((int)option);
        }

        /** Sets the fallback option for the remotely subscribed video stream based on the network conditions.
         * 
         * The default setting for `option` is {@link agora_gaming_rtc.STREAM_FALLBACK_OPTIONS#STREAM_FALLBACK_OPTION_VIDEO_STREAM_LOW STREAM_FALLBACK_OPTION_VIDEO_STREAM_LOW(1)}, where the remotely subscribed video stream falls back to the low-stream video (low resolution and low bitrate) under poor downlink network conditions.
         * 
         * If `option` is set as {@link agora_gaming_rtc.STREAM_FALLBACK_OPTIONS#STREAM_FALLBACK_OPTION_AUDIO_ONLY STREAM_FALLBACK_OPTION_AUDIO_ONLY(2)}, the SDK automatically switches the video from a high-stream to a low-stream, or disables the video when the downlink network conditions cannot support both audio and video to guarantee the quality of the audio. The SDK monitors the network quality and restores the video stream when the network conditions improve.
         * 
         * When the remotely subscribed video stream falls back to audio only or when the audio-only stream switches back to the video stream, the SDK triggers the {@link agora_gaming_rtc.OnRemoteSubscribeFallbackToAudioOnlyHandler OnRemoteSubscribeFallbackToAudioOnlyHandler} callback.
         * 
         * @note Ensure that you call this method before joining a channel.
         * 
         * @param option Sets the fallback option for the remotely subscribed video stream. See #STREAM_FALLBACK_OPTIONS.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */   
        public int SetRemoteSubscribeFallbackOption(STREAM_FALLBACK_OPTIONS option)
        {
            return IRtcEngineNative.setRemoteSubscribeFallbackOption((int)option);
        }

        /** Sets the default video-stream type for the video received by the local user when the remote user sends dual streams.
         * 
         * - If the dual-stream mode is enabled by calling the {@link agora_gaming_rtc.IRtcEngine.EnableDualStreamMode EnableDualStreamMode} method, the user receives the high-stream video by default. The `SetRemoteDefaultVideoStreamType` method allows the application to adjust the corresponding video-stream type according to the size of the video window, reducing the bandwidth and resources.
         * - If the dual-stream mode is not enabled, the user receives the high-stream video by default.
         * 
         * The result after calling this method is returned in the {@link agora_gaming_rtc.OnApiExecutedHandler OnApiExecutedHandler} callback. The Agora RTC SDK receives the high-stream video by default to reduce the bandwidth. If needed, users can switch to the low-stream video through this method.
         * 
         * @note You can call this method either before or after joining a channel. If you call both `SetRemoteDefaultVideoStreamType` and
         * {@link agora_gaming_rtc.IRtcEngine.SetRemoteVideoStreamType SetRemoteVideoStreamType}, the SDK applies the settings in
         * the `SetRemoteVideoStreamType`.
         *
         * @param remoteVideoStreamType Sets the default video-stream type. See #REMOTE_VIDEO_STREAM_TYPE.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetRemoteDefaultVideoStreamType(REMOTE_VIDEO_STREAM_TYPE remoteVideoStreamType)
        {
            return IRtcEngineNative.setRemoteDefaultVideoStreamType((int)remoteVideoStreamType);
        }

        /** Publishes the local stream to a specified CDN streaming URL. (CDN live only.)
         * 
         * The SDK returns the result of this method call in the {@link agora_gaming_rtc.OnStreamPublishedHandler OnStreamPublishedHandler} callback.
         * 
         * The `AddPublishStreamUrl` method call triggers the {@link agora_gaming_rtc.OnRtmpStreamingStateChangedHandler OnRtmpStreamingStateChangedHandler} callback on the local client to report the state of adding a local stream to the CDN.
         * 
         * @note
         * - Ensure that the user joins the channel before calling this method.
         * - Ensure that you enable the RTMP Converter service before using this function.
         * - This method adds only one stream CDN streaming URL each time it is called.
         * - Agora supports pushing media streams in RTMPS protocol to the CDN only when you enable transcoding.
         * 
         * @param url The CDN streaming URL in the RTMP or RTMPS format. The maximum length of this parameter is 1024 bytes. The CDN streaming URL must not contain special characters, such as Chinese language characters.
         * @param transcodingEnabled Sets whether transcoding is enabled/disabled:
         * - true: Enable transcoding. To [transcode](https://docs.agora.io/en/Agora%20Platform/terms?platform=All%20Platforms#transcoding) the audio or video streams when publishing them to CDN live, often used for combining the audio and video streams of multiple hosts in CDN live. If you set this parameter as `true`, ensure that you call the {@link agora_gaming_rtc.IRtcEngine.SetLiveTranscoding SetLiveTranscoding} method before this method.
         * - false: Disable transcoding.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         *     - `ERR_INVALID_ARGUMENT(-2)`: The CDN streaming URL is `null` or has a string length of 0.
         *     - `ERR_NOT_INITIALIZED(-7)`: You have not initialized the RTC engine when publishing the stream.
         */
        public int AddPublishStreamUrl(string url, bool transcodingEnabled)
        {
            return IRtcEngineNative.addPublishStreamUrl(url, transcodingEnabled);
        }

        /** Removes an RTMP or RTMPS stream from the CDN. (CDN live only.)
         * 
         * This method removes the CDN streaming URL (added by the {@link agora_gaming_rtc.IRtcEngine.AddPublishStreamUrl AddPublishStreamUrl} method) from a CDN live stream. The SDK returns the result of this method call in the {@link agora_gaming_rtc.OnStreamUnpublishedHandler OnStreamUnpublishedHandler} callback.
         * 
         * The `RemovePublishStreamUrl` method call triggers the {@link agora_gaming_rtc.OnRtmpStreamingStateChangedHandler OnRtmpStreamingStateChangedHandler} callback on the local client to report the state of removing an RTMP or RTMPS stream from the CDN.
         * 
         * @note
         * - This method removes only one CDN streaming URL each time it is called.
         * - The CDN streaming URL must not contain special characters, such as Chinese language characters.
         * - This method applies to Live Broadcast only.
         * 
         * @param url The CDN streaming URL to be removed. The maximum length of this parameter is 1024 bytes.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */   
        public int RemovePublishStreamUrl(string url)
        {
            return IRtcEngineNative.removePublishStreamUrl(url);
        }

        /** Retrieves the description of a warning or error code.
         *
         * @param code The warning or error code that the {@link agora_gaming_rtc.OnSDKWarningHandler OnSDKWarningHandler} or {@link agora_gaming_rtc.OnSDKErrorHandler OnSDKErrorHandler} callback returns.
         *  
         * @return [Warning Code](./index.html#warn) or [Error Code](./index.html#error).
         */
        public static string GetErrorDescription(int code)
        {

#if !UNITY_EDITOR && UNITY_WEBGL
            string value = AgoraWebGLEventHandler.GetErrorDescription(code + "");
            return value;
#else
            return Marshal.PtrToStringAnsi(IRtcEngineNative.getErrorDescription(code));
#endif

        }

        /** Enables interoperability with the Agora Web SDK.
         * 
         * @deprecated This method is deprecated. As of v3.0.1, the Unity SDK automatically enables interoperability with the Web SDK, so you no longer need to call this method.
         *
         * @note 
         * - This method applies only to the Live-broadcast profile. In the Communication profile, interoperability with the Agora Web SDK is enabled by default.
         * - If the channel has Web SDK users, ensure that you call this method, or the video of the Unity user will be a black screen for the Web user.
         * 
         * @param enabled Sets whether to enable/disable interoperability with the Agora Web SDK:
         * - true: Enable.
         * - false: (Default) Disable.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int EnableWebSdkInteroperability(bool enabled)
        {
            return IRtcEngineNative.enableWebSdkInteroperability(enabled);
        }

        /** Sets the video layout and audio settings for CDN live. (CDN live only.)
         * 
         * The SDK triggers the {@link agora_gaming_rtc.OnTranscodingUpdatedHandler OnTranscodingUpdatedHandler} callback when you call the `SetLiveTranscoding` method to update the transcoding setting.
         * 
         * @note
         * - This method applies to Live Broadcast only.
         * - Ensure that you enable the RTMP Converter service before using this function.
         * - If you call the `SetLiveTranscoding` method to update the transcoding setting for the first time, the SDK does not trigger the `OnTranscodingUpdatedHandler` callback.
         * - Ensure that you call this method after joining a channel.
         * - Agora supports pushing media streams in RTMPS protocol to the CDN only when you enable transcoding.
         *
         * @param transcoding Sets the CDN live audio/video transcoding settings. See LiveTranscoding.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetLiveTranscoding(LiveTranscoding transcoding)
        {
            String transcodingUserInfo = "";
            if (transcoding.userCount != 0 && transcoding.transcodingUsers != null) {
                for (int i = 0; i < transcoding.userCount; i ++) {
                    transcodingUserInfo += transcoding.transcodingUsers[i].uid;
                    transcodingUserInfo += "\t";
                    transcodingUserInfo += transcoding.transcodingUsers[i].x;
                    transcodingUserInfo += "\t";
                    transcodingUserInfo += transcoding.transcodingUsers[i].y;
                    transcodingUserInfo += "\t";
                    transcodingUserInfo += transcoding.transcodingUsers[i].width;
                    transcodingUserInfo += "\t";
                    transcodingUserInfo += transcoding.transcodingUsers[i].height;
                    transcodingUserInfo += "\t";
                    transcodingUserInfo += transcoding.transcodingUsers[i].zOrder;
                    transcodingUserInfo += "\t";
                    transcodingUserInfo += transcoding.transcodingUsers[i].alpha;
                    transcodingUserInfo += "\t";
                    transcodingUserInfo += transcoding.transcodingUsers[i].audioChannel;
                    transcodingUserInfo += "\t";
                }
            }

            String liveStreamAdvancedFeaturesStr = "";
            if (transcoding.liveStreamAdvancedFeatures.Length > 0) {
                for (int i = 0; i < transcoding.liveStreamAdvancedFeatures.Length; i++) {
                    liveStreamAdvancedFeaturesStr += transcoding.liveStreamAdvancedFeatures[i].featureName;
                    liveStreamAdvancedFeaturesStr += "\t";
                    liveStreamAdvancedFeaturesStr += transcoding.liveStreamAdvancedFeatures[i].opened;
                    liveStreamAdvancedFeaturesStr += "\t";
                }
            }

            // Debug.Log("transcodingUserInfo  " + transcodingUserInfo + "liveStreamAdvancedFeaturesStr" + liveStreamAdvancedFeaturesStr);
            return IRtcEngineNative.setLiveTranscoding(transcoding.width, transcoding.height, transcoding.videoBitrate, transcoding.videoFramerate, transcoding.lowLatency, transcoding.videoGop, (int)transcoding.videoCodecProfile, transcoding.backgroundColor, transcoding.userCount, transcodingUserInfo, transcoding.transcodingExtraInfo, transcoding.metadata, transcoding.watermark.url, transcoding.watermark.x, transcoding.watermark.y, transcoding.watermark.width, transcoding.watermark.height, transcoding.backgroundImage.url, transcoding.backgroundImage.x, transcoding.backgroundImage.y, transcoding.backgroundImage.width, transcoding.backgroundImage.height, (int)transcoding.audioSampleRate, transcoding.audioBitrate, transcoding.audioChannels, (int)transcoding.audioCodecProfile, liveStreamAdvancedFeaturesStr, (uint)transcoding.liveStreamAdvancedFeatures.Length);
        }

        /** Pushes the video frame using the {@link agora_gaming_rtc.ExternalVideoFrame ExternalVideoFrame} and passes the video frame to the Agora RTC SDK.
         * 
         * @note This method does not support video frames in the Texture format.
         * 
         * @param externalVideoFrame Video frame to be pushed. See {@link agora_gaming_rtc.ExternalVideoFrame ExternalVideoFrame}.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int PushVideoFrame(ExternalVideoFrame externalVideoFrame)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.pushVideoFrameWGL(externalVideoFrame.buffer, externalVideoFrame.buffer.Length, externalVideoFrame.stride, externalVideoFrame.height, externalVideoFrame.rotation, externalVideoFrame.cropLeft, externalVideoFrame.cropTop, externalVideoFrame.cropRight, externalVideoFrame.cropBottom);
            return 0;
#else
            return IRtcEngineNative.pushVideoFrame((int)externalVideoFrame.type, (int)externalVideoFrame.format, externalVideoFrame.buffer, externalVideoFrame.stride, externalVideoFrame.height, externalVideoFrame.cropLeft, externalVideoFrame.cropTop, externalVideoFrame.cropRight, externalVideoFrame.cropBottom, externalVideoFrame.rotation, externalVideoFrame.timestamp);
#endif

        }

        /** Configures the external video source.
         * 
         * @note Ensure that you call this method before joining a channel.
         *
         * @param enable Sets whether to use the external video source:
         * - true: Use the external video source.
         * - false: (Default) Do not use the external video source.
         * @param useTexture Sets whether to use texture as an input (Agora does not support texture now, please use `false`):
         * - true: Use texture as an input.
         * - false: (Default) Do not use texture as an input.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetExternalVideoSource(bool enable, bool useTexture = false)
        {
            return IRtcEngineNative.setExternalVideoSource(enable, useTexture);
        }

        /** Sets the external audio source. Please call this method before {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey}.
         * 
         * @param enabled Sets whether to enable/disable the external audio source:
         * - true: Enables the external audio source.
         * - false: (Default) Disables the external audio source.
         * @param sampleRate Sets the sample rate (Hz) of the external audio source, which can be set as 8000, 16000, 32000, 44100, or 48000 Hz.
         * @param channels Sets the number of audio channels of the external audio source:
         * - 1: Mono.
         * - 2: Stereo.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetExternalAudioSource(bool enabled, int sampleRate, int channels)
        {
            return IRtcEngineNative.setExternalAudioSource(enabled, sampleRate, channels);
        }

        /** Pushes the external audio frame.
         * 
         * @param audioFrame The audio frame: {@link agora_gaming_rtc.AudioFrame AudioFrame}.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int PushAudioFrame(AudioFrame audioFrame)
        {
            return IRtcEngineNative.pushAudioFrame_((int)audioFrame.type, audioFrame.samples, audioFrame.bytesPerSample, audioFrame.channels, audioFrame.samplesPerSec, audioFrame.buffer, audioFrame.renderTimeMs, audioFrame.avsync_type);
        }

        /** Retrieves the audio mixing volume for local playback.
         * 
         * This method helps troubleshoot audio volume related issues.
         * 
         * @note Call this method when you are in a channel.
         * 
         * @return
         * - &ge; 0: The audio mixing volume, if this method call succeeds. The value range is [0,100].
         * - < 0: Failure.
         */
        public int GetAudioMixingPlayoutVolume()
        {
            return IRtcEngineNative.getAudioMixingPlayoutVolume();
        }   
        
        /** Retrieves the audio mixing volume for publishing.
         * 
         * This method helps troubleshoot audio volume related issues.
         * 
         * @note Call this method when you are in a channel.
         * 
         * @return
         * - &ge; 0: The audio mixing volume for publishing, if this method call succeeds. The value range is [0,100].
         * - < 0: Failure.
         */
        public int GetAudioMixingPublishVolume()
        {
            return IRtcEngineNative.getAudioMixingPublishVolume();
        }

        /** Enables/Disables stereo panning for remote users.
         * 
         * Ensure that you call this method before {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} to enable stereo panning for remote users so that the local user can track the position of a remote user by calling {@link agora_gaming_rtc.AudioEffectManagerImpl.SetRemoteVoicePosition SetRemoteVoicePosition}.
         * 
         * @param enabled Sets whether or not to enable stereo panning for remote users:
         * - true: enables stereo panning.
         * - false: disables stereo panning.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int EnableSoundPositionIndication(bool enabled)
        {
            return IRtcEngineNative.enableSoundPositionIndication(enabled);
        }

        /** Sets the local voice changer option.
         * 
         * @deprecated Deprecated from v3.2.0. Use the following methods instead:
         * - {@link agora_gaming_rtc.IRtcEngine.SetAudioEffectPreset SetAudioEffectPreset}: Audio effects.
         * - {@link agora_gaming_rtc.IRtcEngine.SetVoiceBeautifierPreset SetVoiceBeautifierPreset}: Voice beautifier effects.
         * - {@link agora_gaming_rtc.IRtcEngine.SetVoiceConversionPreset SetVoiceConversionPreset}: Voice conversion effects.
         * This method can be used to set the local voice effect for users in a communication channel or hosts in the interactive live streaming channel.
         * Voice changer options include the following voice effects:
         * 
         * - `VOICE_CHANGER_XXX`: Changes the local voice to an old man, a little boy, or the Hulk. Applies to the voice talk scenario.
         * - `VOICE_BEAUTY_XXX`: Beautifies the local voice by making it sound more vigorous, resounding, or adding spacial resonance. Applies to the voice talk and singing scenario.
         * - `GENERAL_BEAUTY_VOICE_XXX`: Adds gender-based beautification effect to the local voice. Applies to the voice talk scenario.
         *   - For a male voice: Adds magnetism to the voice.
         *   - For a female voice: Adds freshness or vitality to the voice.
         * 
         * @note
         * - To achieve better voice effect quality, Agora recommends setting the profile parameter in {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile} as `AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)` or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)`.
         * - This method works best with the human voice, and Agora does not recommend using it for audio containing music and a human voice.
         * - Do not use this method with {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceReverbPreset SetLocalVoiceReverbPreset}, because the method called later overrides the one called earlier.
         * - You can call this method either before or after joining a channel.
         *
         * @param voiceChanger Sets the local voice changer option. The default value is `VOICE_CHANGER_OFF`, which means the original voice. See details in #VOICE_CHANGER_PRESET. 
         * Gender-based beatification effect works best only when assigned a proper gender:
         * - For male: `GENERAL_BEAUTY_VOICE_MALE_MAGNETIC`
         * - For female: `GENERAL_BEAUTY_VOICE_FEMALE_FRESH` or `GENERAL_BEAUTY_VOICE_FEMALE_VITALITY`
         * Failure to do so can lead to voice distortion.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure. Check if the enumeration is properly set.
         */
        public int SetLocalVoiceChanger(VOICE_CHANGER_PRESET voiceChanger)
        {
            return IRtcEngineNative.setLocalVoiceChanger((int)voiceChanger);
        }

        /** Sets the local voice reverberation option, including the virtual stereo.
         * 
         * @deprecated Deprecated from v3.2.0. Use {@link agora_gaming_rtc.IRtcEngine.SetAudioEffectPreset SetAudioEffectPreset} or
         * {@link agora_gaming_rtc.IRtcEngine.SetVoiceBeautifierPreset SetVoiceBeautifierPreset} instead.
         *
         * This method sets the local voice reverberation for users in a communication channel or hosts in a Live-broadcast channel.
         * After successfully calling this method, all users in the channel can hear the voice with reverberation.
         * 
         * @note
         * - When calling this method with enumerations that begin with `AUDIO_REVERB_FX`, ensure that you set profile in {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile} 
         * as `AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)` or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)`; otherwise, this methods cannot set the corresponding voice reverberation option.
         * - When calling this method with `AUDIO_VIRTUAL_STEREO`, Agora recommends setting the `profile` parameter in `SetAudioProfile` as `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)`.
         * - This method works best with the human voice, and Agora does not recommend using it for audio containing music and a human voice.
         * - Do not use this method with {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceChanger SetLocalVoiceChanger}, because the method called later overrides the one called earlier.
         * For detailed considerations, see the advanced guide *Voice Enhancement and Effects*.
         * - You can call this method either before or after joining a channel.
         *
         * @param audioReverbPreset The local voice reverberation option. The default value is `AUDIO_REVERB_OFF`,
         * which means the original voice. See #AUDIO_REVERB_PRESET.
         * To achieve better voice effects, Agora recommends the enumeration whose name begins with `AUDIO_REVERB_FX`.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetLocalVoiceReverbPreset(AUDIO_REVERB_PRESET audioReverbPreset)
        {
            return IRtcEngineNative.setLocalVoiceReverbPreset((int)audioReverbPreset);
        }

        /** Changes the voice pitch of the local speaker.
         * 
         * @param pitch Sets the voice pitch. The value ranges between 0.5 and 2.0. The lower the value, the lower the voice pitch. The default value is 1.0 (no change to the local voice pitch).
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetLocalVoicePitch(double pitch)
        {
            return IRtcEngineNative.setLocalVoicePitch(pitch);
        }

        /** Sets the local voice equalization effect.
         * 
         * @note You can call this method either before or after joining a channel.
         *
         * @param bandFrequency Sets the band frequency. The value ranges between 0 and 9, representing the respective 10-band center frequencies of the voice effects, including 31, 62, 125, 500, 1k, 2k, 4k, 8k, and 16k Hz. See #AUDIO_EQUALIZATION_BAND_FREQUENCY.
         * @param bandGain Sets the gain of each band in dB. The value ranges between -15 and 15.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetLocalVoiceEqualization(AUDIO_EQUALIZATION_BAND_FREQUENCY bandFrequency, int bandGain)
        {
            return IRtcEngineNative.setLocalVoiceEqualization((int)bandFrequency, bandGain);
        }

        /** Sets the local voice reverberation.
         * 
         * v2.4.0 adds the {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceReverbPreset SetLocalVoiceReverbPreset} method, a more user-friendly method for setting the local voice reverberation. You can use this method to set the local reverberation effect, such as pop music, R&B, rock music, and hip-hop.
         * 
         * @note You can call this method either before or after joining a channel.
         *
         * @param reverbKey Sets the reverberation key. See #AUDIO_REVERB_TYPE.
         * @param value Sets the value of the reverberation key.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetLocalVoiceReverb(AUDIO_REVERB_TYPE reverbKey, int value)
        {
            return IRtcEngineNative.setLocalVoiceReverb((int)reverbKey, value);
        }

        /** Sets the camera capture configuration.
         * 
         * For a video call or interactive live streaming, generally the SDK controls the camera output parameters. When the default camera capturer settings do not meet special requirements or cause performance problems, we recommend using this method to set the camera capturer configuration:
         * - If the resolution or frame rate of the captured raw video data are higher than those set by {@link agora_gaming_rtc.IRtcEngine.SetVideoEncoderConfiguration SetVideoEncoderConfiguration}, processing video frames requires extra CPU and RAM usage and degrades performance. We recommend setting config as {@link agora_gaming_rtc.CAPTURER_OUTPUT_PREFERENCE#CAPTURER_OUTPUT_PREFERENCE_PERFORMANCE CAPTURER_OUTPUT_PREFERENCE_PERFORMANCE(1)} to avoid such problems.
         * - If you do not need local video preview or are willing to sacrifice preview quality, we recommend setting config as {@link agora_gaming_rtc.CAPTURER_OUTPUT_PREFERENCE#CAPTURER_OUTPUT_PREFERENCE_PERFORMANCE CAPTURER_OUTPUT_PREFERENCE_PERFORMANCE(1)} to optimize CPU and RAM usage.
         * - If you want better quality for the local video preview, we recommend setting config as {@link agora_gaming_rtc.CAPTURER_OUTPUT_PREFERENCE#CAPTURER_OUTPUT_PREFERENCE_PREVIEW CAPTURER_OUTPUT_PREFERENCE_PREVIEW(2)}.
         * - To customize the width and height of the video image captured by the local camera, set the camera capture configuration as {@link agora_gaming_rtc.CAPTURER_OUTPUT_PREFERENCE#CAPTURER_OUTPUT_PREFERENCE_MANUAL CAPTURER_OUTPUT_PREFERENCE_MANUAL(3)}.
         * 
         * @note Call this method before enabling the local camera. That said, you can call this method before calling {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey}, {@link agora_gaming_rtc.IRtcEngine.EnableVideo EnableVideo}, or {@link agora_gaming_rtc.IRtcEngine.EnableLocalVideo EnableLocalVideo}, depending on which method you use to turn on your local camera.
         * 
         * @param cameraCaptureConfiguration Sets the camera capturer configuration. See {@link agora_gaming_rtc.CameraCapturerConfiguration CameraCapturerConfiguration}.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetCameraCapturerConfiguration(CameraCapturerConfiguration cameraCaptureConfiguration)
        {
            return IRtcEngineNative.setCameraCapturerConfiguration((int)cameraCaptureConfiguration.preference, (int)cameraCaptureConfiguration.cameraDirection, cameraCaptureConfiguration.captureWidth, cameraCaptureConfiguration.captureHeight);
        }

        /** Prioritizes a remote user's stream.
         * 
         * Use this method with the {@link agora_gaming_rtc.IRtcEngine.SetRemoteSubscribeFallbackOption SetRemoteSubscribeFallbackOption} method. If the fallback function is enabled for a subscribed stream, the SDK ensures the high-priority user gets the best possible stream quality.
         * 
         * @note 
         * - The Agora RTC SDK supports setting `userPriority` as high for one user only.
         * - Ensure that you call this method before joining a channel.
         * 
         * @param uid The ID of the remote user.
         * @param userPriority Sets the priority of the remote user. See #PRIORITY_TYPE.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetRemoteUserPriority(uint uid, PRIORITY_TYPE userPriority)
        {

#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setRemoteUserPriority_WGL(uid + "", (int)userPriority);
            return 0;
#else
            return IRtcEngineNative.setRemoteUserPriority(uid, (int)userPriority);
#endif

        }

        /** Sets the size of a log file that the SDK outputs.
         *
         * @deprecated This method is deprecated from v3.3.1. Use `logConfig` in the 
         * {@link agora_gaming_rtc.IRtcEngine.GetEngine(RtcEngineConfig engineConfig) GetEngine} method instead.
         *
         * @note If you want to set the log file size, ensure that you call
         * this method before {@link agora_gaming_rtc.IRtcEngine.SetLogFile SetLogFile}, or the logs are cleared.
         *
         * By default, the SDK outputs five log files, `agorasdk.log`, `agorasdk_1.log`, `agorasdk_2.log`, `agorasdk_3.log`, `agorasdk_4.log`, each with a default size of 1024 KB.
         * These log files are encoded in UTF-8. The SDK writes the latest logs in `agorasdk.log`. When `agorasdk.log` is full, the SDK deletes the log file with the earliest
         * modification time among the other four, renames `agorasdk.log` to the name of the deleted log file, and create a new `agorasdk.log` to record latest logs.
         *
         * @see {@link agora_gaming_rtc.IRtcEngine.SetLogFile SetLogFile}
         * @see {@link agora_gaming_rtc.IRtcEngine.SetLogFilter SetLogFilter}
         *
         * @param fileSizeInKBytes The size (KB) of a log file. The default value is 1024 KB. If you set `fileSizeInKByte` to 1024 KB,
         * the SDK outputs at most 5 MB log files; if you set it to less than 1024 KB, the maximum size of a log file is still 1024 KB.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetLogFileSize(uint fileSizeInKBytes)
        {
            return IRtcEngineNative.setLogFileSize(fileSizeInKBytes);
        }

        /** Sets the external audio sink. This method applies to scenarios where you want to use external audio data for playback. After enabling the external audio sink, you can call the {@link agora_gaming_rtc.AudioRawDataManager.PullAudioFrame PullAudioFrame} method to pull the remote audio data, process it, and play it with the audio effects that you want.
         * 
         * @note 
         * - Once you enable the external audio sink, the app will not retrieve any audio data from the {@link agora_gaming_rtc.AudioRawDataManager.OnPlaybackAudioFrameHandler OnPlaybackAudioFrameHandler} callback.
         * - Ensure that you call this method before joining a channel.
         *
         * @param enabled
         * - true: Enables the external audio sink.
         * - false: (Default) Disables the external audio sink.
         * @param sampleRate Sets the sample rate (Hz) of the external audio sink, which can be set as 16000, 32000, 44100 or 48000.
         * @param channels Sets the number of audio channels of the external audio sink:
         * - 1: Mono.
         * - 2: Stereo.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetExternalAudioSink(bool enabled, int sampleRate, int channels)
        {
            return IRtcEngineNative.setExternalAudioSink(enabled, sampleRate, channels);
        }

        /** Registers a user account.
         * 
         * Once registered, the user account can be used to identify the local user when the user joins the channel. After the user successfully registers a user account, the SDK triggers the {@link agora_gaming_rtc.OnLocalUserRegisteredHandler OnLocalUserRegisteredHandler} callback on the local client, reporting the user ID and user account of the local user.
         * 
         * To join a channel with a user account, you can choose either of the following:
         * - Call the `RegisterLocalUserAccount` method to create a user account, and then the {@link agora_gaming_rtc.IRtcEngine.JoinChannelWithUserAccount JoinChannelWithUserAccount} method to join the channel.
         * - Call the `JoinChannelWithUserAccount` method to join the channel.
         * 
         * The difference between the two is that for the former, the time elapsed between calling the `JoinChannelWithUserAccount` method and joining the channel is shorter than the latter.
         * 
         * @note 
         * - Ensure that you set the `userAccount` parameter. Otherwise, this method does not take effect.
         * - Ensure that the value of the `userAccount` parameter is unique in the channel.
         * - To ensure smooth communication, use the same parameter type to identify the user. For example, if a user joins the channel with a user ID, then ensure all the other users use the user ID too. The same applies to the user account. If a user joins the channel with the Agora Web SDK, ensure that the uid of the user is set to the same parameter type.
         * 
         * @param appId The App ID of your project.
         * @param userAccount The user account. The maximum length of this parameter is 255 bytes. Ensure that you set this parameter and do not set it as null. Supported character scopes are:
         * - All lowercase English letters: a to z.
         * - All uppercase English letters: A to Z.
         * - All numeric characters: 0 to 9.
         * - The space character.
         * - Punctuation characters and other symbols, including: "!", "#", "$", "%", "&", "(", ")", "+", "-", ":", ";", "<", "=", ".", ">", "?", "@", "[", "]", "^", "_", " {", "}", "|", "~", ",".
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int RegisterLocalUserAccount(string appId, string userAccount)
        {
            return IRtcEngineNative.registerLocalUserAccount(appId, userAccount);
        }

        /** Joins the channel with a user account.
         * 
         * After the user successfully joins the channel, the SDK triggers the following callbacks:
         * - The local client: {@link agora_gaming_rtc.OnLocalUserRegisteredHandler OnLocalUserRegisteredHandler} and {@link agora_gaming_rtc.OnJoinChannelSuccessHandler OnJoinChannelSuccessHandler}. 
         * - The remote client: {@link agora_gaming_rtc.OnUserJoinedHandler OnUserJoinedHandler} and {@link agora_gaming_rtc.OnUserInfoUpdatedHandler OnUserInfoUpdatedHandler}, if the user joining the channel is in the Communication profile, or is a host in the Live Broadcast profile.
         * 
         * Once the user joins the channel, the user subscribes to the audio and video streams of all the other users in the channel by default, giving rise to usage and billing calculation. If you do not want to subscribe to a specified stream or all remote streams, call the `mute` methods accordingly.
         *
         * @note To ensure smooth communication, use the same parameter type to identify the user. For example, if a user joins the channel with a user ID, then ensure all the other users use the user ID too. The same applies to the user account. If a user joins the channel with the Agora Web SDK, ensure that the uid of the user is set to the same parameter type.
         * 
         * @param token The token generated at your server:
         * - For low-security requirements: You can use the temporary token generated at Console. For details, see [Get a temporary toke](https://docs.agora.io/en/Agora%20Platform/token?platform=All%20Platforms#get-a-temporary-token).
         * - For high-security requirements: Set it as the token generated at your server. For details, see [Get a token](https://docs.agora.io/en/Agora%20Platform/token?platform=All%20Platforms#get-a-token).
         * @param channelId The channel name. The maximum length of this parameter is 64 bytes. Supported character scopes are:
         * - All lowercase English letters: a to z.
         * - All uppercase English letters: A to Z.
         * - All numeric characters: 0 to 9.
         * - The space character.
         * - Punctuation characters and other symbols, including: "!", "#", "$", "%", "&", "(", ")", "+", "-", ":", ";", "<", "=", ".", ">", "?", "@", "[", "]", "^", "_", " {", "}", "|", "~", ",".
         * @param userAccount The user account. The maximum length of this parameter is 255 bytes. Ensure that you set this parameter and do not set it as null. Supported character scopes are:
         * - All lowercase English letters: a to z.
         * - All uppercase English letters: A to Z.
         * - All numeric characters: 0 to 9.
         * - The space character.
         * - Punctuation characters and other symbols, including: "!", "#", "$", "%", "&", "(", ")", "+", "-", ":", ";", "<", "=", ".", ">", "?", "@", "[", "]", "^", "_", " {", "}", "|", "~", ",".
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int JoinChannelWithUserAccount(string token, string channelId, string userAccount)
        {
            return IRtcEngineNative.joinChannelWithUserAccount(token, channelId, userAccount);
        }

        /** Joins the channel with a user account, and configures whether to automatically subscribe to audio or 
         * video streams after joining the channel.
         *
         * @since v3.3.1
         *
         * After the user successfully joins the channel, the SDK triggers the following callbacks:
         * - The local client: {@link agora_gaming_rtc.OnLocalUserRegisteredHandler OnLocalUserRegisteredHandler} and {@link agora_gaming_rtc.OnJoinChannelSuccessHandler OnJoinChannelSuccessHandler}.
         * - The remote client: {@link agora_gaming_rtc.OnUserJoinedHandler OnUserJoinedHandler} and {@link agora_gaming_rtc.OnUserInfoUpdatedHandler OnUserInfoUpdatedHandler}, if the user joining the channel is in the `COMMUNICATION` profile, or is a host in the `LIVE_BROADCASTING` profile.
         *
         * @note
         * - Compared with {@link agora_gaming_rtc.IRtcEngine.JoinChannelWithUserAccount(string token, string channelId, string userAccount) JoinChannelWithUserAccount}1,
         * this method has the options parameter to configure whether the end user automatically subscribes to all remote audio and video streams in a
         * channel when joining the channel. By default, the user subscribes to the audio and video streams of all the other users in the channel, thus
         * incurring all associated usage costs. To unsubscribe, set the `options` parameter or call the `mute` methods accordingly.
         * - To ensure smooth communication, use the same parameter type to identify the user. For example, if a user joins the channel with a user ID, then ensure all
         *  the other users use the user ID too. The same applies to the user account. If a user joins the channel with the Agora Web SDK, ensure that the
         * uid of the user is set to the same parameter type.
         *
         * @param token The token generated at your server. For details, see [Generate a token](https://docs.agora.io/en/Interactive%20Broadcast/token_server?platform=Windows).
         * @param channelId The channel name. The maximum length of this parameter is 64 bytes. Supported character scopes are:
         * - All lowercase English letters: a to z.
         * - All uppercase English letters: A to Z.
         * - All numeric characters: 0 to 9.
         * - The space character.
         * - Punctuation characters and other symbols, including: "!", "#", "$", "%", "&", "(", ")", "+", "-", ":", ";", "<", "=", ".", ">", "?", "@", "[", "]", "^", "_", " {", "}", "|", "~", ",".
         * @param userAccount The user account. The maximum length of this parameter is 255 bytes. Ensure that the user account is unique and do not set it as null. Supported character scopes are:
         * - All lowercase English letters: a to z.
         * - All uppercase English letters: A to Z.
         * - All numeric characters: 0 to 9.
         * - The space character.
         * - Punctuation characters and other symbols, including: "!", "#", "$", "%", "&", "(", ")", "+", "-", ":", ";", "<", "=", ".", ">", "?", "@", "[", "]", "^", "_", " {", "}", "|", "~", ",".
         * @param options The channel media options: {@link agora_gaming_rtc.ChannelMediaOptions ChannelMediaOptions}.
         * @return
         * - 0: Success.
         * - < 0: Failure.
         *    - ERR_INVALID_ARGUMENT (-2)
         *    - ERR_NOT_READY (-3)
         *    - ERR_REFUSED (-5)
         */
        public int JoinChannelWithUserAccount(string token, string channelId, string userAccount, ChannelMediaOptions options)
        {
            return IRtcEngineNative.joinChannelWithUserAccount_engine(token, channelId, userAccount, options.autoSubscribeAudio, options.autoSubscribeVideo, options.publishLocalAudio, options.publishLocalVideo);
        }

        /** Gets the user information by passing in the user account.
         * 
         * After a remote user joins the channel, the SDK gets the user ID and user account of the remote user, caches them in a mapping table object (`userInfo`), and triggers the {@link agora_gaming_rtc.OnUserInfoUpdatedHandler OnUserInfoUpdatedHandler} callback on the local client.
         * 
         * After receiving the `OnUserInfoUpdatedHandler` callback, you can call this method to get the user ID of the remote user from the userInfo object by passing in the user account.
         * 
         * @param account The user account of the user. Ensure that you set this parameter.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public UserInfo GetUserInfoByUserAccount(string account)
        {
            int uid = IRtcEngineNative.getUserInfoByUserAccount(account);
            UserInfo userInfo = new UserInfo();
            userInfo.userAccount = account;
            userInfo.uid = (uint)uid;
            return userInfo;
        }
        
        /** Gets the user information by passing in the user ID.
         * 
         * After a remote user joins the channel, the SDK gets the user ID and user account of the remote user, caches them in a mapping table object (`userInfo`), and triggers the {@link agora_gaming_rtc.OnUserInfoUpdatedHandler OnUserInfoUpdatedHandler} callback on the local client.
         * 
         * After receiving the `OnUserInfoUpdatedHandler` callback, you can call this method to get the user account of the remote user from the userInfo object by passing in the user ID.
         * 
         * @param uid The user ID of the remote user. Ensure that you set this parameter.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public UserInfo GetUserInfoByUid(uint uid)
        {


#if !UNITY_EDITOR && UNITY_WEBGL
            IntPtr account = IRtcEngineNative.getUserInfoByUid_WGL(uid+"");
            UserInfo userInfo = new UserInfo();
            userInfo.uid = uid;
            if (account != IntPtr.Zero)
            {
                userInfo.userAccount = Marshal.PtrToStringAnsi(account);
                IRtcEngineNative.freeObject(account);
            }
            return userInfo;
#else
            IntPtr account = IRtcEngineNative.getUserInfoByUid(uid);
            UserInfo userInfo = new UserInfo();
            userInfo.uid = uid;
            if (account != IntPtr.Zero)
            {
                userInfo.userAccount = Marshal.PtrToStringAnsi(account);
                IRtcEngineNative.freeObject(account);
            }
            return userInfo;
#endif

        }

        /** Enables/Disables image enhancement and sets the options.
         * 
         * @since v3.0.1
         *
         * @note Call this method after calling {@link agora_gaming_rtc.IRtcEngine.EnableVideo EnableVideo}.
         * 
         * @param enabled Sets whether or not to enable image enhancement:
         * - true: Enables image enhancement.
         * - false: Disables image enhancement.
         * @param beautyOptions Sets the image enhancement option. See {@link agora_gaming_rtc.BeautyOptions BeautyOptions}.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetBeautyEffectOptions(bool enabled, BeautyOptions beautyOptions)
        {
            return IRtcEngineNative.setBeautyEffectOptions(enabled, (int)beautyOptions.lighteningContrastLevel, beautyOptions.lighteningLevel, beautyOptions.smoothnessLevel, beautyOptions.rednessLevel);
        }

        /** Shares the whole or part of a screen by specifying the display ID.
         * 
         * @note
         * - This method is for macOS only.
         * - Ensure that you call this method after joining a channel.
         * 
         * @param displayId The display ID of the screen to be shared. This parameter specifies which screen you want to share.
         * @param rectangle (Optional) Sets the relative location of the region to the screen. `null` means sharing the whole screen. See Rectangle. If the specified region overruns the screen, the SDK shares only the region within it; if you set width or height as 0, the SDK shares the whole screen.
         * @param screenCaptureParameters Sets the screen sharing encoding parameters. The screen sharing encoding parameters. The default video dimension is 1920 x 1080, that is, 2,073,600 pixels. Agora uses the value of `videoDimension` to calculate the charges.
         * For details, see descriptions in ScreenCaptureParameters.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         *     - `ERR_INVALID_STATE`: the screen sharing state is invalid, probably because another screen or window is being shared. Call {@link agora_gaming_rtc.IRtcEngine.StopScreenCapture StopScreenCapture} to stop the current screen sharing.
         *     - `ERR_INVALID_ARGUMENT(-2)`: the argument is invalid.
         */ 
        public int StartScreenCaptureByDisplayId(uint displayId, Rectangle rectangle, ScreenCaptureParameters screenCaptureParameters)
        {
            return IRtcEngineNative.startScreenCaptureByDisplayId(displayId, rectangle.x, rectangle.y, rectangle.width, rectangle.height, screenCaptureParameters.dimensions.width, screenCaptureParameters.dimensions.height, screenCaptureParameters.frameRate, screenCaptureParameters.bitrate, screenCaptureParameters.captureMouseCursor);
        }

        public void StartScreenCaptureForWeb()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.startScreenCaptureForWeb();
#else 
            Debug.LogWarning("StartScreenCaptureForWeb runs for WebGL only.");
#endif
        }
        /** Shares the whole or part of a screen by specifying the screen rect.
         * 
         * @note
         * - This method is for Windows only.
         * - Ensure that you call this method after joining a channel.
         *
         * @param screenRectangle Sets the relative location of the screen to the virtual screen.
         * @param regionRectangle (Optional) Sets the relative location of the region to the screen. `null` means sharing the whole screen. See Rectangle. If the specified region overruns the screen, the SDK shares only the region within it; if you set width or height as 0, the SDK shares the whole screen.
         * @param screenCaptureParameters Sets the screen sharing encoding parameters. The screen sharing encoding parameters. The default video dimension is 1920 x 1080, that is, 2,073,600 pixels. Agora uses the value of `videoDimension` to calculate the charges.
         * For details, see descriptions in ScreenCaptureParameters.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         *     - `ERR_INVALID_STATE`: the screen sharing state is invalid, probably because another screen or window is being shared. Call {@link agora_gaming_rtc.IRtcEngine.StopScreenCapture StopScreenCapture} to stop the current screen sharing.
         *     - `ERR_INVALID_ARGUMENT(-2)`: the argument is invalid.
         */
        public int StartScreenCaptureByScreenRect(Rectangle screenRectangle, Rectangle regionRectangle, ScreenCaptureParameters screenCaptureParameters)
        {
            return IRtcEngineNative.startScreenCaptureByScreenRect(screenRectangle.x, screenRectangle.y, screenRectangle.width, screenRectangle.height, regionRectangle.x, regionRectangle.y, regionRectangle.width, regionRectangle.height, screenCaptureParameters.dimensions.width, screenCaptureParameters.dimensions.height, screenCaptureParameters.frameRate, screenCaptureParameters.bitrate, screenCaptureParameters.captureMouseCursor);
        }

        /** Sets the content hint for screen sharing.
         * 
         * A content hint suggests the type of the content being shared, so that the SDK applies different optimization algorithm to different types of content.
         * 
         * @note
         * - This method is for Windows and macOS only.
         * - You can call this method either before or after you start screen sharing.
         *
         * @param videoContentHint Sets the content hint for screen sharing. See VideoContentHint.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetScreenCaptureContentHint(VideoContentHint videoContentHint)
        {
            return IRtcEngineNative.setScreenCaptureContentHint((int)videoContentHint);
        }

        /** Updates the screen sharing parameters.
         * 
         * @note This method is for Windows and macOS only.
         *
         * @param screenCaptureParameters Sets the screen sharing encoding parameters. The screen sharing encoding parameters. The default video dimension is 1920 x 1080, that is, 2,073,600 pixels. Agora uses the value of `videoDimension` to calculate the charges.
         * For details, see descriptions in ScreenCaptureParameters.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         *     - `ERR_NOT_READY(-3)`: no screen or windows is being shared.
         */
        public int UpdateScreenCaptureParameters(ScreenCaptureParameters screenCaptureParameters)
        {
            return IRtcEngineNative.updateScreenCaptureParameters(screenCaptureParameters.dimensions.width, screenCaptureParameters.dimensions.height, screenCaptureParameters.frameRate, screenCaptureParameters.bitrate, screenCaptureParameters.captureMouseCursor);
        }

        /** Updates the screen sharing region.
         * 
         * @note This method is for Windows and macOS only.
         *
         * @param rectangle Sets the relative location of the region to the screen or window. `null` means sharing the whole screen or window. See Rectangle. If the specified region overruns the screen or window, the SDK shares only the region within it; if you set width or height as 0, the SDK shares the whole screen or window.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         *     - `ERR_NOT_READY(-3)`: no screen or windows is being shared.
         */
        public int UpdateScreenCaptureRegion(Rectangle rectangle)
        {
            return IRtcEngineNative.updateScreenCaptureRegion(rectangle.x, rectangle.y, rectangle.width, rectangle.height);
        }

        /** Stop screen sharing.
         * 
         * @note This method is for Windows and macOS only.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int StopScreenCapture()
        {
            return IRtcEngineNative.stopScreenCapture();
        }

        /** Adds a voice or video stream URL address to the interactive live streaming.
         * 
         * The {@link agora_gaming_rtc.OnStreamPublishedHandler OnStreamPublishedHandler} callback returns the inject status. If this method call is successful, the server pulls the voice or video stream and injects it into a live channel. This is applicable to scenarios where all audience members in the channel can watch a live show and interact with each other.
         * 
         * The `AddInjectStreamUrl` method call triggers the following callbacks:
         * - The local client:
         *     - {@link agora_gaming_rtc.OnStreamInjectedStatusHandler OnStreamInjectedStatusHandler} , with the state of the injecting the online stream.
         *     - {@link agora_gaming_rtc.OnUserJoinedHandler OnUserJoinedHandler} (uid: 666), if the method call is successful and the online media stream is injected into the channel.
         * - The remote client:
         *     - `OnUserJoinedHandler` (uid: 666), if the method call is successful and the online media stream is injected into the channel.
         * 
         * @warning Agora will soon stop the service for injecting online media streams on the client. If you have not implemented this service, Agora recommends that you do not use it.
         *
         * @note 
         * - Ensure that you enable the RTMP Converter service before using this function.
         * - This method applies to the Live-Broadcast profile only.
         * - You can inject only one media stream into the channel at the same time.
         * 
         * @param url The URL address which is added to the ongoing interactive live streaming. Valid protocols are RTMP, HLS, and HTTP-FLV.
         * - Supported FLV audio codec type: AAC.
         * - Supported FLV video codec type: H264 (AVC).
         * @param streamConfig The InjectStreamConfig object that contains the configuration of the added voice or video stream.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         *     -`ERR_INVALID_ARGUMENT(-2)`: The injected URL does not exist. Call this method again to inject the stream and ensure that the URL is valid.
         *     -`ERR_NOT_READY(-3)`: The user is not in the channel.
         *     -`ERR_NOT_SUPPORTED(-4)`: The channel profile is not interactive live streaming. Call the {@link agora_gaming_rtc.IRtcEngine.SetChannelProfile SetChannelProfile} method and set the channel profile to interactive live streaming before calling this method.
         *     -`ERR_NOT_INITIALIZED(-7)`: The SDK is not initialized. Ensure that the IRtcEngine object is initialized before calling this method.
         */
        public int AddInjectStreamUrl(string url, InjectStreamConfig streamConfig)
        {
            return IRtcEngineNative.addInjectStreamUrl(url, streamConfig.width, streamConfig.height, streamConfig.videoGop, streamConfig.videoFramerate, streamConfig.videoBitrate, (int)streamConfig.audioSampleRate, streamConfig.audioBitrate, streamConfig.audioChannels);
        }
        
        /** Removes the voice or video stream URL address from the interactive live streaming.
         * 
         * This method removes the URL address (added by the {@link agora_gaming_rtc.IRtcEngine.AddInjectStreamUrl AddInjectStreamUrl} method) from the interactive live streaming.
         * 
         * @warning Agora will soon stop the service for injecting online media streams on the client. If you have not implemented this service, Agora recommends that you do not use it.
         *
         * @note If this method is called successfully, the SDK triggers the {@link agora_gaming_rtc.OnUserOfflineHandler OnUserOfflineHandler} callback and returns a stream uid of 666.
         * 
         * @param url The URL address of the injected stream to be removed.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int RemoveInjectStreamUrl(string url)
        {
            return IRtcEngineNative.removeInjectStreamUrl(url);
        }

        /** Enables loopback capturing.
         * 
         * If you enable loopback capturing, the output of the sound card is mixed into the audio stream sent to the other end.
         * 
         * @note
         * - This method is for macOS and Windows only.
         * - macOS does not support loopback capturing of the default sound card. If you need to use this method, please use a virtual sound card and pass its name to the deviceName parameter. Agora has tested and recommends using soundflower.
         * 
         * @param enabled Sets whether to enable/disable loopback capturing.
         * - true: Enable loopback capturing.
         * - false: (Default) Disable loopback capturing.
         * @param deviceName The device name of the sound card. The default value is `null` (the default sound card).
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int EnableLoopbackRecording(bool enabled, string deviceName)
        {
            return IRtcEngineNative.enableLoopbackRecording(enabled, deviceName);
        }

        /** Sets the audio session’s operational restriction.
         * 
         * The SDK and the app can both configure the audio session by default. The app may occasionally use other apps or third-party components to manipulate the audio session and restrict the SDK from doing so. This method allows the app to restrict the SDK’s manipulation of the audio session.
         * 
         * You can call this method at any time to return the control of the audio sessions to the SDK.
         * 
         * @note
         * - This method is for iOS only.
         * - This method restricts the SDK’s manipulation of the audio session. Any operation to the audio session relies solely on the app, other apps, or third-party components.
         * - You can call this method either before or after joining a channel.
         *
         * @param restriction The operational restriction (bit mask) of the SDK on the audio session. See #AUDIO_SESSION_OPERATION_RESTRICTION.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetAudioSessionOperationRestriction(AUDIO_SESSION_OPERATION_RESTRICTION restriction)
        {
            return IRtcEngineNative.setAudioSessionOperationRestriction((int)restriction);
        }

        /** Starts to relay media streams across channels.
         * 
         * After a successful method call, the SDK triggers the {@link agora_gaming_rtc.OnChannelMediaRelayStateChangedHandler OnChannelMediaRelayStateChangedHandler} and {@link agora_gaming_rtc.OnChannelMediaRelayEventHandler OnChannelMediaRelayEventHandler} callbacks, and these callbacks return the state and events of the media stream relay.
         * - If the `OnChannelMediaRelayStateChangedHandler` callback returns {@link agora_gaming_rtc.CHANNEL_MEDIA_RELAY_STATE#RELAY_STATE_RUNNING RELAY_STATE_RUNNING(2)} and {@link agora_gaming_rtc.CHANNEL_MEDIA_RELAY_ERROR#RELAY_OK RELAY_OK(0)}, and the `OnChannelMediaRelayEventHandler` callback returns {@link agora_gaming_rtc.CHANNEL_MEDIA_RELAY_EVENT#RELAY_EVENT_PACKET_SENT_TO_DEST_CHANNEL RELAY_EVENT_PACKET_SENT_TO_DEST_CHANNEL(4)}, the host starts sending data to the destination channel.
         * - If the `OnChannelMediaRelayStateChangedHandler` callback returns {@link agora_gaming_rtc.CHANNEL_MEDIA_RELAY_STATE#RELAY_STATE_FAILURE RELAY_STATE_FAILURE(3)}, an exception occurs during the media stream relay.
         * 
         * @note
         * - Call this method after the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method.
         * - This method takes effect only when you are a host in a Live-broadcast channel.
         * - After a successful method call, if you want to call this method again, ensure that you call the {@link agora_gaming_rtc.IRtcEngine.StopChannelMediaRelay StopChannelMediaRelay} method to quit the current relay.
         * 
         * @param mediaRelayConfiguration The configuration of the media stream relay: ChannelMediaRelayConfiguration.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int StartChannelMediaRelay(ChannelMediaRelayConfiguration mediaRelayConfiguration)
        {

#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.startChannelMediaRelay_WEBGL(mediaRelayConfiguration.srcInfo.channelName, mediaRelayConfiguration.srcInfo.token, "" + mediaRelayConfiguration.srcInfo.uid, mediaRelayConfiguration.destInfos.channelName, mediaRelayConfiguration.destInfos.token, "" + mediaRelayConfiguration.destInfos.uid, mediaRelayConfiguration.destCount);
            return 0;
#else
            return IRtcEngineNative.startChannelMediaRelay(mediaRelayConfiguration.srcInfo.channelName, mediaRelayConfiguration.srcInfo.token, mediaRelayConfiguration.srcInfo.uid, mediaRelayConfiguration.destInfos.channelName, mediaRelayConfiguration.destInfos.token, mediaRelayConfiguration.destInfos.uid, mediaRelayConfiguration.destCount);
#endif

        }

        /** Updates the channels for media stream relay. After a successful {@link agora_gaming_rtc.IRtcEngine.StartChannelMediaRelay StartChannelMediaRelay} method call, if you want to relay the media stream to more channels, or leave the current relay channel, you can call the `UpdateChannelMediaRelay` method.
         * 
         * After a successful method call, the SDK triggers the {@link agora_gaming_rtc.OnChannelMediaRelayEventHandler OnChannelMediaRelayEventHandler} callback with the {@link agora_gaming_rtc.CHANNEL_MEDIA_RELAY_EVENT#RELAY_EVENT_PACKET_UPDATE_DEST_CHANNEL RELAY_EVENT_PACKET_UPDATE_DEST_CHANNEL(7)} state code.
         * 
         * @note Call this method after the `StartChannelMediaRelay` method to update the destination channel.
         * 
         * @param mediaRelayConfiguration The media stream relay configuration: ChannelMediaRelayConfiguration.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int UpdateChannelMediaRelay(ChannelMediaRelayConfiguration mediaRelayConfiguration)
        {

#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.updateChannelMediaRelay_WEBGL(mediaRelayConfiguration.srcInfo.channelName, mediaRelayConfiguration.srcInfo.token, "" + mediaRelayConfiguration.srcInfo.uid, mediaRelayConfiguration.destInfos.channelName, mediaRelayConfiguration.destInfos.token, "" + mediaRelayConfiguration.destInfos.uid, mediaRelayConfiguration.destCount);
            return 0;
#else
            return IRtcEngineNative.updateChannelMediaRelay(mediaRelayConfiguration.srcInfo.channelName, mediaRelayConfiguration.srcInfo.token, mediaRelayConfiguration.srcInfo.uid, mediaRelayConfiguration.destInfos.channelName, mediaRelayConfiguration.destInfos.token, mediaRelayConfiguration.destInfos.uid, mediaRelayConfiguration.destCount);
#endif

        }

        /** Stops the media stream relay.
         * 
         * Once the relay stops, the host quits all the destination channels.
         * 
         * After a successful method call, the SDK triggers the {@link agora_gaming_rtc.OnChannelMediaRelayStateChangedHandler OnChannelMediaRelayStateChangedHandler} callback. If the callback returns {@link agora_gaming_rtc.CHANNEL_MEDIA_RELAY_STATE#RELAY_STATE_IDLE RELAY_STATE_IDLE(0)} and {@link agora_gaming_rtc.CHANNEL_MEDIA_RELAY_ERROR#RELAY_OK RELAY_OK(0)}, the host successfully stops the relay.
         * 
         * @note If the method call fails, the SDK triggers the `OnChannelMediaRelayStateChangedHandler` callback with the {@link agora_gaming_rtc.CHANNEL_MEDIA_RELAY_ERROR#RELAY_ERROR_SERVER_NO_RESPONSE RELAY_ERROR_SERVER_NO_RESPONSE(2)} or {@link agora_gaming_rtc.CHANNEL_MEDIA_RELAY_ERROR#RELAY_ERROR_SERVER_CONNECTION_LOST RELAY_ERROR_SERVER_CONNECTION_LOST(8)} state code. You can leave the channel by calling the {@link agora_gaming_rtc.IRtcEngine.LeaveChannel LeaveChannel} method, and the media stream relay automatically stops.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int StopChannelMediaRelay()
        {
            return IRtcEngineNative.stopChannelMediaRelay();
        }

        /** Switches to a different channel.
         * 
         * This method allows the audience of a Live-broadcast channel to switch to a different channel.
         * 
         * After the user successfully switches to another channel, the {@link agora_gaming_rtc.OnLeaveChannelHandler OnLeaveChannelHandler} and {@link agora_gaming_rtc.OnJoinChannelSuccessHandler OnJoinChannelSuccessHandler} callbacks are triggered to indicate that the user has left the original channel and joined a new one.
         * 
         * Once the user switches to another channel, the user subscribes to the audio and video streams of all the other users in the channel by default, giving rise to usage and billing calculation. If you do not want to subscribe to a specified stream or all remote streams, call the `mute` methods accordingly.
         *
         * @note This method applies to the audience role in a Live-broadcast channel only.
         * 
         * @param token The token generated at your server:
         * - For low-security requirements: You can use the temporary token generated in Console. For details, see [Get a temporary token](https://docs.agora.io/en/Agora%20Platform/token?platform=All%20Platforms#generate-a-token).
         * - For high-security requirements: Use the token generated at your server. For details, see [Get a token](https://docs.agora.io/en/Interactive%20Broadcast/token_server?platform=All%20Platforms).
         * @param channelId Unique channel name for the AgoraRTC session in the string format. The string length must be less than 64 bytes. Supported character scopes are:
         * - All lowercase English letters: a to z.
         * - All uppercase English letters: A to Z.
         * - All numeric characters: 0 to 9.
         * - The space character.
         * - Punctuation characters and other symbols, including: "!", "#", "$", "%", "&", "(", ")", "+", "-", ":", ";", "<", "=", ".", ">", "?", "@", "[", "]", "^", "_", " {", "}", "|", "~", ",".
         * 
         * @return
         * - 0(ERR_OK): Success.
         * - < 0: Failure.
         *  - -1(ERR_FAILED): A general error occurs (no specified reason).
         *  - -2(ERR_INALID_ARGUMENT): The parameter is invalid.
         *  - -5(ERR_REFUSED): The request is rejected, probably because the user is not an audience.
         *  - -7(ERR_NOT_INITIALIZED): The SDK is not initialized.
         *  - -102(ERR_INVALID_CHANNEL_NAME): The channel name is invalid.
         *  - -113(ERR_NOT_IN_CHANNEL): The user is not in the channel.
         */
        public int SwitchChannel(string token, string channelId)
        {
            return IRtcEngineNative.switchChannel(token, channelId);
        }

        /** Sets whether to enable the multi-channel mode.
         *
         * @since v3.0.1
         *
         * In multi-channel video scenarios, you must call this method to enable the multi-channel mode. Otherwise, a user cannot receive video frames from multiple channels.
         * 
         * @note 
         * - Call this method before joining a channel.
         * - In voice-only scenarios, you do not need to call this method.
         *
         * @param multiChannelWant Whether to enable the multi-channel mode:
         * - `true`: Enable the multi-channel mode
         * - `false`: (Default) Disable the multi-channel mode.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetMultiChannelWant(bool multiChannelWant)
        {
            return IRtcEngineNative.setMultiChannelWant(multiChannelWant);
        }

        /** Sets whether to enable the mirror mode of both local video and remote video.
         *
         * @note Call this method before {@link agora_gaming_rtc.IRtcEngine.EnableVideoObserver EnableVideoObserver}.
         *
         * @param wheatherApply Sets whether to enable the mirror mode of both local video and remote video.
         * - true: Enable the mirror mode. 
         * - false: (Default) Disable the mirror mode.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetMirrorApplied(bool wheatherApply)
        {
            return IRtcEngineNative.setMirrorApplied(wheatherApply);
        }

        /** Sets the volume of the in-ear monitor.
         * 
         * @note 
         * - This method is for Android and iOS only.
         * - Users must use wired earphones to hear their own voices.
         * - You can call this method either before or after joining a channel.
         *
         * @param volume Sets the volume of the in-ear monitor. The value ranges between 0 and 100 (default).
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetInEarMonitoringVolume(int volume)
        {
            return IRtcEngineNative.setInEarMonitoringVolume(volume);
        }

        /** Shares the whole or part of a window by specifying the window ID.
         * 
         * @note
         * - Ensure that you call this method after joining a channel.
         * - Applies to the macOS and Windows platforms only.
         *
         * Since v3.0.0, this method supports window sharing of UWP (Universal Windows Platform) applications.
         *
         * Agora tests the mainstream UWP applications by using the lastest SDK, see details as follows:
         *
         * <table>
         *     <tr>
         *         <td><b>OS version</b></td>
         *         <td><b>Software</b></td>
         *         <td><b>Software name</b></td>
         *         <td><b>Whether support</b></td>
         *     </tr>
         *     <tr>
         *         <td rowspan="8">win10</td>
         *         <td >Chrome</td>
         *         <td>76.0.3809.100</td>
         *         <td>No</td>
         *     </tr>
         *     <tr>
         *         <td>Office Word</td>
         *         <td rowspan="3">18.1903.1152.0</td>
         *         <td>Yes</td>
         *     </tr>
         *         <tr>
         *         <td>Office Excel</td>
         *         <td>No</td>
         *     </tr>
         *     <tr>
         *         <td>Office PPT</td>
         *         <td>Yes</td>
         *     </tr>
         *  <tr>
         *         <td>WPS Word</td>
         *         <td rowspan="3">11.1.0.9145</td>
         *         <td rowspan="3">Yes</td>
         *     </tr>
         *         <tr>
         *         <td>WPS Excel</td>
         *     </tr>
         *     <tr>
         *         <td>WPS PPT</td>
         *     </tr>
         *         <tr>
         *         <td>Media Player (come with the system)</td>
         *         <td>All</td>
         *         <td>Yes</td>
         *     </tr>
         *      <tr>
         *         <td rowspan="8">win8</td>
         *         <td >Chrome</td>
         *         <td>All</td>
         *         <td>Yes</td>
         *     </tr>
         *     <tr>
         *         <td>Office Word</td>
         *         <td rowspan="3">All</td>
         *         <td rowspan="3">Yes</td>
         *     </tr>
         *         <tr>
         *         <td>Office Excel</td>
         *     </tr>
         *     <tr>
         *         <td>Office PPT</td>
         *     </tr>
         *  <tr>
         *         <td>WPS Word</td>
         *         <td rowspan="3">11.1.0.9098</td>
         *         <td rowspan="3">Yes</td>
         *     </tr>
         *         <tr>
         *         <td>WPS Excel</td>
         *     </tr>
         *     <tr>
         *         <td>WPS PPT</td>
         *     </tr>
         *         <tr>
         *         <td>Media Player(come with the system)</td>
         *         <td>All</td>
         *         <td>Yes</td>
         *     </tr>
         *   <tr>
         *         <td rowspan="8">win7</td>
         *         <td >Chrome</td>
         *         <td>73.0.3683.103</td>
         *         <td>No</td>
         *     </tr>
         *     <tr>
         *         <td>Office Word</td>
         *         <td rowspan="3">All</td>
         *         <td rowspan="3">Yes</td>
         *     </tr>
         *         <tr>
         *         <td>Office Excel</td>
         *     </tr>
         *     <tr>
         *         <td>Office PPT</td>
         *     </tr>
         *  <tr>
         *         <td>WPS Word</td>
         *         <td rowspan="2">11.1.0.9098</td>
         *         <td rowspan="2">No</td>
         *     </tr>
         *         <tr>
         *         <td>WPS Excel</td>
         *     </tr>
         *     <tr>
         *         <td>WPS PPT</td>
         *         <td>11.1.0.9098</td>
         *         <td>Yes</td>
         *     </tr>
         *         <tr>
         *         <td>Media Player(come with the system)</td>
         *         <td>All</td>
         *         <td>No</td>
         *     </tr>
         * </table>
         *
         * @param  windowId The ID of the window to be shared. For information on how to get the windowId, see the advanced guide *Share Screen*.
         * @param  regionRect (Optional) The relative location of the region to the window. NULL/NIL means sharing the whole window. See Rectangle. If the specified region overruns the window, the SDK shares only the region within it; if you set width or height as 0, the SDK shares the whole window.
         * @param  screenCaptureParameters The screen sharing encoding parameters. The default video dimension is 1920 x 1080, that is, 2,073,600 pixels. Agora uses the value of `videoDimension` to calculate the charges. For details, see descriptions in ScreenCaptureParameters.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure:
         *  - `ERR_INVALID_ARGUMENT`: The argument is invalid.
         */
        public int StartScreenCaptureByWindowId(int windowId, Rectangle regionRect, ScreenCaptureParameters screenCaptureParameters)
        {
            return IRtcEngineNative.startScreenCaptureByWindowId(windowId, regionRect.x, regionRect.y, regionRect.width, regionRect.height, screenCaptureParameters.dimensions.width, screenCaptureParameters.dimensions.height, screenCaptureParameters.frameRate, screenCaptureParameters.bitrate, screenCaptureParameters.captureMouseCursor);
        }

        /** Enables in-ear monitoring.
         * 
         * @note
         * - This method is only for Android and iOS.
         * - Users must use wired earphones to hear their own voices.
         * - You can call this method either before or after joining a channel.
         *
         * @param enabled Sets whether to enable/disable in-ear monitoring:
         * - true: Enable.
         * - false: (Default) Disable.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int EnableInEarMonitoring(bool enabled)
        {
            return IRtcEngineNative.enableInEarMonitoring(enabled);
        }

        /** Adjusts the playback signal volume of a specified remote user.
         * 
         * @since v3.0.1
         *
         * You can call this method as many times as necessary to adjust the playback signal volume of different remote users, or to repeatedly adjust the playback signal volume of the same remote user.
         * 
         * @note
         * - Call this method after joining a channel.
         * - The playback signal volume here refers to the mixed volume of a specified remote user.
         * - This method can only adjust the playback signal volume of one specified remote user at a time. To adjust the playback signal volume of different remote users, call the method as many times, once for each remote user.
         * 
         * @param uid The ID of the remote user.
         * @param volume the playback signal volume of the specified remote user. The value ranges from 0 to 100:
         * - 0: Mute.
         * - 100: Original volume.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int AdjustUserPlaybackSignalVolume(uint uid, int volume)
        {

#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.adjustUserPlaybackSignalVolume_WGLM(""+uid, volume);
            return 0;
#else
            return IRtcEngineNative.adjustUserPlaybackSignalVolume(uid, volume);
#endif
        }

        /** Creates and gets an `AgoraChannel` object.
         * 
         * @since v3.0.1
         *
         * To join more than one channel, call this method multiple times to create as many `AgoraChannel` objects as needed, and
         * call the {@link agora_gaming_rtc.AgoraChannel.JoinChannel JoinChannel} method of each created `AgoraChannel` object.
         * 
         * After joining multiple channels, you can simultaneously subscribe to streams of all the channels, but publish a stream in only one channel at one time.
         * @param channelId The unique channel name for an Agora RTC session. It must be in the string format and not exceed 64 bytes in length. Supported character scopes are:
         * - All lowercase English letters: a to z.
         * - All uppercase English letters: A to Z.
         * - All numeric characters: 0 to 9.
         * - The space character.
         * - Punctuation characters and other symbols, including: "!", "#", "$", "%", "&", "(", ")", "+", "-", ":", ";", "<", "=", ".", ">", "?", "@", "[", "]", "^", "_", " {", "}", "|", "~", ",".
         * 
         * @note
         * - This parameter does not have a default value. You must set it.
         * - Do not set it as the empty string "". Otherwise, the SDK returns `ERR_REFUSED(5)`.
         * 
         * @return
         * - The `AgoraChannel` object, if the method call succeeds.
         * - `null`, if the method call fails.
         * - `ERR_REFUSED(5)`, if you set channelId as the empty string "".
         */
        public AgoraChannel CreateChannel(string channelId)
        {
            return AgoraChannel.CreateChannel(this, channelId);
        }

        /** Enables/Disables face detection for the local user.
         * 
         * @since v3.0.1
         *
         * Once face detection is enabled, the SDK triggers the {@link agora_gaming_rtc.OnFacePositionChangedHandler OnFacePositionChangedHandler} callback
         * to report the face information of the local user, which includes the following aspects:
         * - The width and height of the local video.
         * - The position of the human face in the local video.
         * - The distance between the human face and the device screen.
         *
         * @note
         * - Applies to Android and iOS only.
         * - You can call this method either before or after joining a channel.
         *
         * @param enable Determines whether to enable the face detection function for the local user:
         * - true: Enable face detection.
         * - false: (Default) Disable face detection.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int EnableFaceDetection(bool enable)
        {
            return IRtcEngineNative.enableFaceDetection(enable);
        }

        /** Sets the pitch of the local music file.
         *
         * @since v3.0.1
         *
         * When a local music file is mixed with a local human voice, call this method to set the pitch of the local music file only.
         *
         * @note
         * Call this method after calling {@link agora_gaming_rtc.IRtcEngine.StartAudioMixing StartAudioMixing}.
         *
         * @param pitch Sets the pitch of the local music file by chromatic scale. The default value is 0,
         * which means keeping the original pitch. The value ranges from -12 to 12, and the pitch value between
         * consecutive values is a chromatic value. The greater the absolute value of this parameter, the
         * higher or lower the pitch of the local music file.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetAudioMixingPitch(int pitch)
        {
            return IRtcEngineNative.setAudioMixingPitch(pitch);
        }

        /** Enables/Disables the built-in encryption.
         *
         * @since v3.2.0
         *
         * In scenarios requiring high security, Agora recommends calling this method to enable the built-in encryption before joining a channel.
         *
         * All users in the same channel must use the same encryption mode and encryption key. After a user leaves the 
         * channel, the SDK automatically disables the built-in encryption. To enable the built-in encryption, call 
         * this method before the user joins the channel again.
         *
         * @note If you enable the built-in encryption, you cannot use the RTMP or RTMPS streaming function.
         * 
         * @param enabled Whether to enable the built-in encryption:
         * - true: Enable the built-in encryption.
         * - false: Disable the built-in encryption.
         * @param encryptionConfig Configurations of built-in encryption schemas. See EncryptionConfig.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         *  - -2(ERR_INVALID_ARGUMENT): An invalid parameter is used. Set the parameter with a valid value.
         *  - -4(ERR_NOT_SUPPORTED): The encryption mode is incorrect or the SDK fails to load the external encryption library. Check the enumeration or reload the external encryption library.
         *  - -7(ERR_NOT_INITIALIZED): The SDK is not initialized. Initialize the `IRtcEngine` instance before calling this method.
         */
        public int EnableEncryption(bool enabled, EncryptionConfig encryptionConfig)
        {
            return IRtcEngineNative.enableEncryption(enabled, encryptionConfig.encryptionKey, (int)encryptionConfig.encryptionMode, encryptionConfig.encryptionKdfSalt);
        }

        /// @cond
        /** Enables/Disables the super-resolution algorithm for a remote user's video stream.
         *
         * @since v3.2.0
         *
         * The algorithm effectively improves the resolution of the specified remote user's video stream. When the original
         * resolution of the remote video stream is a × b pixels, you can receive and render the stream at a higher
         * resolution (2a × 2b pixels) by enabling the algorithm.
         *
         * After calling this method, the SDK triggers the
         * {@link agora_gaming_rtc.OnUserSuperResolutionEnabledHandler OnUserSuperResolutionEnabledHandler} callback to report
         * whether you have successfully enabled the super-resolution algorithm.
         *
         * @warning The super-resolution algorithm requires extra system resources.
         * To balance the visual experience and system usage, the SDK poses the following restrictions:
         * - The algorithm can only be used for a single user at a time.
         * - On the Android platform, the original resolution of the remote video must not exceed 640 × 360 pixels.
         * - On the iOS platform, the original resolution of the remote video must not exceed 640 × 480 pixels.
         * If you exceed these limitations, the SDK triggers the {@link agora_gaming_rtc.OnSDKWarningHandler OnSDKWarningHandler}
         * callback with the corresponding warning codes:
         * - #WARN_SUPER_RESOLUTION_STREAM_OVER_LIMITATION (1610): The origin resolution of the remote video is beyond the range where the super-resolution algorithm can be applied.
         * - #WARN_SUPER_RESOLUTION_USER_COUNT_OVER_LIMITATION (1611): Another user is already using the super-resolution algorithm.
         * - #WARN_SUPER_RESOLUTION_DEVICE_NOT_SUPPORTED (1612): The device does not support the super-resolution algorithm.
         *
         * @note
         * - This method applies to Android and iOS only.
         * - Requirements for the user's device:
         *  - Android: The following devices are known to support the method:
         *    - VIVO: V1821A, NEX S, 1914A, 1916A, and 1824BA
         *    - OPPO: PCCM00
         *    - OnePlus: A6000
         *    - Xiaomi: Mi 8, Mi 9, MIX3, and Redmi K20 Pro
         *    - SAMSUNG: SM-G9600, SM-G9650, SM-N9600, SM-G9708, SM-G960U, and SM-G9750
         *    - HUAWEI: SEA-AL00, ELE-AL00, VOG-AL00, YAL-AL10, HMA-AL00, and EVR-AN00
         *  - iOS: This method is supported on devices running iOS 12.0 or later. The following
         * device models are known to support the method:
         *      - iPhone XR
         *      - iPhone XS
         *      - iPhone XS Max
         *      - iPhone 11
         *      - iPhone 11 Pro
         *      - iPhone 11 Pro Max
         *      - iPad Pro 11-inch (3rd Generation)
         *      - iPad Pro 12.9-inch (3rd Generation)
         *      - iPad Air 3 (3rd Generation)
         *
         * @param userId The ID of the remote user.
         * @param enable Whether to enable the super-resolution algorithm:
         * - true: Enable the super-resolution algorithm.
         * - false: Disable the super-resolution algorithm.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int EnableRemoteSuperResolution(uint userId, bool enable)
        {
            return IRtcEngineNative.enableRemoteSuperResolution(userId, enable);
        }
        /// @endcond

        /** Sets the role of a user in interactive live streaming.
         *
         * @since v3.2.0
         *
         * You can call this method either before or after joining the channel to set the user role as audience or host. If
         * you call this method to switch the user role after joining the channel, the SDK triggers the following callbacks:
         * - The local client: {@link agora_gaming_rtc.OnClientRoleChangedHandler OnClientRoleChangedHandler}.
         * - The remote client: {@link agora_gaming_rtc.OnUserJoinedHandler OnUserJoinedHandler}
         * or {@link agora_gaming_rtc.OnUserOfflineHandler OnUserOfflineHandler}.
         *
         * @note
         * - This method applies to the `LIVE_BROADCASTING` profile only (when the `profile` parameter in
         * {@link agora_gaming_rtc.IRtcEngine.SetChannelProfile SetChannelProfile} is set as `CHANNEL_PROFILE_LIVE_BROADCASTING`).
         * - The difference between this method and {@link agora_gaming_rtc.IRtcEngine.SetClientRole(CLIENT_ROLE_TYPE role) SetClientRole}1 is that
         * this method can set the user level in addition to the user role.
         *  - The user role determines the permissions that the SDK grants to a user, such as permission to send local
         * streams, receive remote streams, and push streams to a CDN address.
         *  - The user level determines the level of services that a user can enjoy within the permissions of the user's
         * role. For example, an audience can choose to receive remote streams with low latency or ultra low latency. Levels
         * affect prices.
         *
         * @param role The role of a user in interactive live streaming. See #CLIENT_ROLE_TYPE.
         * @param clientRoleOptions The detailed options of a user, including user level. See {@link agora_gaming_rtc.ClientRoleOptions ClientRoleOptions}.
         *
         * @return
         * - 0(ERR_OK): Success.
         * - < 0: Failure.
         *  - -1(ERR_FAILED): A general error occurs (no specified reason).
         *  - -2(ERR_INALID_ARGUMENT): The parameter is invalid.
         *  - -7(ERR_NOT_INITIALIZED): The SDK is not initialized.
         */
        public int SetClientRole(CLIENT_ROLE_TYPE role, ClientRoleOptions clientRoleOptions)
        {
            return IRtcEngineNative.setClientRole_1((int)role, (int)clientRoleOptions.audienceLatencyLevel);
        }

        /** Sets an SDK preset voice beautifier effect.
         *
         * @since v3.2.0
         *
         * Call this method to set an SDK preset voice beautifier effect for the local user who sends an audio stream. After
         * setting a voice beautifier effect, all users in the channel can hear the effect.
         *
         * You can set different voice beautifier effects for different scenarios. See *Set the Voice Effect*.
         *
         * To achieve better audio effect quality, Agora recommends calling {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile} and
         * setting the `scenario` parameter to `AUDIO_SCENARIO_GAME_STREAMING(3)` and the `profile` parameter to
         * `AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)` or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)` before calling this method.
         *
         * @note
         * - You can call this method either before or after joining a channel.
         * - Do not set the `profile` parameter of `SetAudioProfile` to `AUDIO_PROFILE_SPEECH_STANDARD(1)`
         * or `AUDIO_PROFILE_IOT(6)`; otherwise, this method call does not take effect.
         * - This method works best with the human voice. Agora does not recommend using this method for audio containing music.
         * - After calling this method, Agora recommends not calling the following methods, because they can override `SetVoiceBeautifierPreset`:
         *  - {@link agora_gaming_rtc.IRtcEngine.SetAudioEffectPreset SetAudioEffectPreset}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetAudioEffectParameters SetAudioEffectParameters}
​         *  - {@link agora_gaming_rtc.IRtcEngine.SetVoiceBeautifierParameters SetVoiceBeautifierParameters}
​         *  - {@link agora_gaming_rtc.IRtcEngine.SetVoiceConversionPreset SetVoiceConversionPreset}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceReverbPreset SetLocalVoiceReverbPreset}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceChanger SetLocalVoiceChanger}
         *  - {@link agora_gaming_rtc.AudioEffectManagerImpl.SetLocalVoicePitch SetLocalVoicePitch}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceEqualization SetLocalVoiceEqualization}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceReverb SetLocalVoiceReverb}
         *
         * @param preset The options for SDK preset voice beautifier effects: #VOICE_BEAUTIFIER_PRESET.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetVoiceBeautifierPreset(VOICE_BEAUTIFIER_PRESET preset)
        {
            return IRtcEngineNative.setVoiceBeautifierPreset((int)preset);
        }

        /** Sets an SDK preset audio effect.
         *
         * @since v3.2.0
         *
         * Call this method to set an SDK preset audio effect for the local user who sends an audio stream. This audio effect
         * does not change the gender characteristics of the original voice. After setting an audio effect, all users in the
         * channel can hear the effect.
         *
         * You can set different audio effects for different scenarios. See *Set the Voice Effect*.
         *
         * To achieve better audio effect quality, Agora recommends calling {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile}
         * and setting the `scenario` parameter to `AUDIO_SCENARIO_GAME_STREAMING(3)` before calling this method.
         *
         * @note
         * - You can call this method either before or after joining a channel.
         * - Do not set the profile `parameter` of `setAudioProfile` to `AUDIO_PROFILE_SPEECH_STANDARD(1)` or `AUDIO_PROFILE_IOT(6)`;
         * otherwise, this method call does not take effect.
         * - This method works best with the human voice. Agora does not recommend using this method for audio containing music.
         * - If you call this method and set the `preset` parameter to enumerators except `ROOM_ACOUSTICS_3D_VOICE` or `PITCH_CORRECTION`,
         * do not call {@link agora_gaming_rtc.IRtcEngine.SetAudioEffectParameters SetAudioEffectParameters}; otherwise, `setAudioEffectParameters`
         * overrides this method.
         * - After calling this method, Agora recommends not calling the following methods, because they can override `SetAudioEffectPreset`:
         *  - {@link agora_gaming_rtc.IRtcEngine.SetVoiceBeautifierPreset SetVoiceBeautifierPreset}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetVoiceBeautifierParameters SetVoiceBeautifierParameters}
​         *  - {@link agora_gaming_rtc.IRtcEngine.SetVoiceConversionPreset SetVoiceConversionPreset}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceReverbPreset SetLocalVoiceReverbPreset}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceChanger SetLocalVoiceChanger}
         *  - {@link agora_gaming_rtc.AudioEffectManagerImpl.SetLocalVoicePitch SetLocalVoicePitch}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceEqualization SetLocalVoiceEqualization}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceReverb SetLocalVoiceReverb}
         *
         * @param preset The options for SDK preset audio effects. See #AUDIO_EFFECT_PRESET.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetAudioEffectPreset(AUDIO_EFFECT_PRESET preset)
        {
            return IRtcEngineNative.setAudioEffectPreset((int)preset);
        }

        /** Sets parameters for SDK preset audio effects.
         *
         * @since v3.2.0
         *
         * Call this method to set the following parameters for the local user who send an audio stream:
         * - 3D voice effect: Sets the cycle period of the 3D voice effect.
         * - Pitch correction effect: Sets the basic mode and tonic pitch of the pitch correction effect. Different songs
         * have different modes and tonic pitches. Agora recommends bounding this method with interface elements to enable
         * users to adjust the pitch correction interactively.
         *
         * After setting parameters, all users in the channel can hear the relevant effect.
         *
         * You can call this method directly or after {@link agora_gaming_rtc.IRtcEngine.SetAudioEffectPreset SetAudioEffectPreset}. If you
         * call this method after `SetAudioEffectPreset`, ensure that you set the preset
         * parameter of `SetAudioEffectPreset` to `ROOM_ACOUSTICS_3D_VOICE` or `PITCH_CORRECTION` and then call this method
         * to set the same enumerator; otherwise, this method overrides `SetAudioEffectPreset`.
         *
         * @note
         * - You can call this method either before or after joining a channel.
         * - To achieve better audio effect quality, Agora recommends calling {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile}
         * and setting the `scenario` parameter to `AUDIO_SCENARIO_GAME_STREAMING(3)` before calling this method.
         * - Do not set the `profile` parameter of `SetAudioProfile` to `AUDIO_PROFILE_SPEECH_STANDARD(1)` or
         * `AUDIO_PROFILE_IOT(6)`; otherwise, this method call does not take effect.
         * - This method works best with the human voice. Agora does not recommend using this method for audio containing music.
         * - After calling this method, Agora recommends not calling the following methods, because they can override `setAudioEffectParameters`:
         *  - {@link agora_gaming_rtc.IRtcEngine.SetAudioEffectPreset SetAudioEffectPreset}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetVoiceBeautifierPreset SetVoiceBeautifierPreset}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetVoiceBeautifierParameters SetVoiceBeautifierParameters}
​         *  - {@link agora_gaming_rtc.IRtcEngine.SetVoiceConversionPreset SetVoiceConversionPreset}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceReverbPreset SetLocalVoiceReverbPreset}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceChanger SetLocalVoiceChanger}
         *  - {@link agora_gaming_rtc.AudioEffectManagerImpl.SetLocalVoicePitch SetLocalVoicePitch}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceEqualization SetLocalVoiceEqualization}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceReverb SetLocalVoiceReverb}
         *
         * @param preset The options for SDK preset audio effects:
         * - `ROOM_ACOUSTICS_3D_VOICE`: 3D voice effect.
         *  - Call `SetAudioProfile` and set the `profile` parameter to `AUDIO_PROFILE_MUSIC_STANDARD_STEREO(3)`
         * or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)` before setting this enumerator; otherwise, the enumerator setting does not take effect.
         *  - If the 3D voice effect is enabled, users need to use stereo audio playback devices to hear the anticipated voice effect.
         * - `PITCH_CORRECTION`: Pitch correction effect. To achieve better audio effect quality, Agora recommends calling
         * `SetAudioProfile` and setting the `profile` parameter to `AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)` or
         * `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)` before setting this enumerator.
         * @param param1
         * - If you set `preset` to `ROOM_ACOUSTICS_3D_VOICE`, the `param1` sets the cycle period of the 3D voice effect.
         * The value range is [1,60] and the unit is a second. The default value is 10 seconds, indicating that the voice moves
         * around you every 10 seconds.
         * - If you set `preset` to `PITCH_CORRECTION`, `param1` sets the basic mode of the pitch correction effect:
         *  - `1`: (Default) Natural major scale.
         *  - `2`: Natural minor scale.
         *  - `3`: Japanese pentatonic scale.
         * @param param2
         * - If you set `preset` to `ROOM_ACOUSTICS_3D_VOICE`, you need to set `param2` to `0`.
         * - If you set `preset` to `PITCH_CORRECTION`, `param2` sets the tonic pitch of the pitch correction effect:
         *  - `1`: A
         *  - `2`: A#
         *  - `3`: B
         *  - `4`: (Default) C
         *  - `5`: C#
         *  - `6`: D
         *  - `7`: D#
         *  - `8`: E
         *  - `9`: F
         *  - `10`: F#
         *  - `11`: G
         *  - `12`: G#
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetAudioEffectParameters(AUDIO_EFFECT_PRESET preset, int param1, int param2)
        {
            return IRtcEngineNative.setAudioEffectParameters((int)preset, param1, param2);
        }

        /** Agora supports reporting and analyzing customized messages.
         *
         * @since v3.2.0
         *
         * This function is in the beta stage with a free trial. The ability provided in its beta test version is reporting a maximum of 10 message pieces within 6 seconds, with each message piece not exceeding 256 bytes and each string not exceeding 100 bytes.
         * To try out this function, contact [support@agora.io](mailto:support@agora.io) and discuss the format of customized messages with us.
         */
        public int SendCustomReportMessage(string id, string category, string events, string label, int value)
        {
            return IRtcEngineNative.sendCustomReportMessage(id, category, events, label, value);
        }

        /** Sets parameters for SDK preset voice beautifier effects.
         *
         * @since v3.3.1
         *
         * Call this method to set a gender characteristic and a reverberation effect for the singing beautifier 
         * effect. This method sets parameters for the local user who sends an audio stream.
         * See *Set the Voice Effect*.
         *
         * After you call this method successfully, all users in the channel can hear the relevant effect.
         *
         * To achieve better audio effect quality, before you call this method, Agora recommends calling 
         * {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile}, and setting the `scenario` parameter
         * as `AUDIO_SCENARIO_GAME_STREAMING(3)` and the `profile` parameter as `AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)` 
         * or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)`.
         *
         * @note
         * - You can call this method either before or after joining a channel.
         * - Do not set the `profile` parameter of `SetAudioProfile` as `AUDIO_PROFILE_SPEECH_STANDARD(1)` or 
         * `AUDIO_PROFILE_IOT(6)`; otherwise, this method call does not take effect.
         * - This method works best with the human voice. Agora does not recommend using this method for audio 
         * containing music.
         * - After you call this method, Agora recommends not calling the following methods, because they can override 
         * `SetVoiceBeautifierParameters`:
         *  - {@link agora_gaming_rtc.IRtcEngine.SetAudioEffectPreset SetAudioEffectPreset}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetAudioEffectParameters SetAudioEffectParameters}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetVoiceBeautifierPreset SetVoiceBeautifierPreset}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetVoiceConversionPreset SetVoiceConversionPreset}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceReverbPreset SetLocalVoiceReverbPreset}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceChanger SetLocalVoiceChanger}
         *  - {@link agora_gaming_rtc.AudioEffectManagerImpl.SetLocalVoicePitch SetLocalVoicePitch}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceEqualization SetLocalVoiceEqualization}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceReverb SetLocalVoiceReverb}
         *
         * @param preset The options for SDK preset voice beautifier effects:
         * - `SINGING_BEAUTIFIER`: Singing beautifier effect.
         * @param param1 The gender characteristics options for the singing voice:
         * - `1`: A male-sounding voice.
         * - `2`: A female-sounding voice.
         * @param param2 The reverberation effects options:
         * - `1`: The reverberation effect sounds like singing in a small room.
         * - `2`: The reverberation effect sounds like singing in a large room.
         * - `3`: The reverberation effect sounds like singing in a hall.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetVoiceBeautifierParameters(VOICE_BEAUTIFIER_PRESET preset, int param1, int param2)
        {
            return IRtcEngineNative.setVoiceBeautifierParameters((int)preset, param1, param2);
        }

        /** Enables or disables deep-learning noise reduction.
         *
         * @since v3.3.1
         *
         * The SDK enables traditional noise reduction mode by default to reduce most of the stationary background noise.
         * If you need to reduce most of the non-stationary background noise, Agora recommends enabling deep-learning
         * noise reduction as follows:
         *
         * 1. Integrate the dynamical library under the libs folder to your project:
         *  - Android: `libagora_ai_denoise_extension.so`
         *  - iOS: `AgoraAIDenoiseExtension.framework`
         *  - macOS: `AgoraAIDenoiseExtension.framework`
         *  - Windows: `libagora_ai_denoise_extension.dll`
         * 2. Call `EnableDeepLearningDenoise(true)`.
         *
         * Deep-learning noise reduction requires high-performance devices. For example, the following devices and later
         * models are known to support deep-learning noise reduction:
         * - iPhone 6S
         * - MacBook Pro 2015
         * - iPad Pro (2nd generation)
         * - iPad mini (5th generation)
         * - iPad Air (3rd generation)
         *
         * After successfully enabling deep-learning noise reduction, if the SDK detects that the device performance
         * is not sufficient, it automatically disables deep-learning noise reduction and enables traditional noise 
         * reduction.
         *
         * If you call `EnableDeepLearningDenoise(false)` or the SDK automatically disables deep-learning noise reduction
         * in the channel, when you need to re-enable deep-learning noise reduction, you need to call 
         * {@link agora_gaming_rtc.IRtcEngine.LeaveChannel LeaveChannel} first, and then call 
         * `EnableDeepLearningDenoise(true)`.
         *
         * @note
         * - This method dynamically loads the library, so Agora recommends calling this method before joining a channel.
         * - This method works best with the human voice. Agora does not recommend using this method for audio 
         * containing music.
         *
         * @param enable Sets whether to enable deep-learning noise reduction.
         * - true: (Default) Enables deep-learning noise reduction.
         * - false: Disables deep-learning noise reduction.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         *  - `-157` (`ERR_MODULE_NOT_FOUND`): The dynamical library for enabling deep-learning noise reduction is not integrated.
         */
        public int EnableDeepLearningDenoise(bool enable) 
        {
            return IRtcEngineNative.enableDeepLearningDenoise(enable);
        }

        /** Joins a channel with the user ID, and configures whether to automatically subscribe to the audio or video streams.
         *
         * @since v3.3.1
         *
         * Users in the same channel can talk to each other, and multiple users in the same channel can start a group chat. 
         * Users with different App IDs cannot call each other.
         *
         * You must call the {@link agora_gaming_rtc.IRtcEngine.LeaveChannel LeaveChannel} method to exit the current call 
         * before entering another channel.
         *
         * A successful `JoinChannel` method call triggers the following callbacks:
         * - The local client: {@link agora_gaming_rtc.OnJoinChannelSuccessHandler OnJoinChannelSuccessHandler}.
         * - The remote client: {@link agora_gaming_rtc.OnUserJoinedHandler OnUserJoinedHandler}, if the user joining the 
         * channel is in the `COMMUNICATION` profile, or is a host in the `LIVE_BROADCASTING` profile.
         *
         * When the connection between the client and the Agora server is interrupted due to poor network conditions, the 
         * SDK tries reconnecting to the server.
         * When the local client successfully rejoins the channel, the SDK triggers the 
         * {@link agora_gaming_rtc.OnReJoinChannelSuccessHandler OnReJoinChannelSuccessHandler} callback on the local client.
         *
         * @note
         * - Compared with {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey}, this method
         * has the options parameter which configures whether the user automatically subscribes to all remote audio and 
         * video streams in the channel when joining the channel. By default, the user subscribes to the audio and video 
         * streams of all the other users in the channel, thus incurring all associated usage costs. To unsubscribe, set 
         * the `options` parameter or call the `mute` methods accordingly.
         * - Ensure that the App ID used for generating the token is the same App ID used in the 
         * {@link agora_gaming_rtc.IRtcEngine.GetEngine(string appId) GetEngine} method for initializing an `IRtcEngine` object.
         *
         * @param token The token generated at your server. For details, see [Generate a token](https://docs.agora.io/en/Interactive%20Broadcast/token_server?platform=Windows).
         * @param channelId Pointer to the unique channel name for the Agora RTC session in the string format smaller than 64 bytes. Supported characters:
         * - All lowercase English letters: a to z.
         * - All uppercase English letters: A to Z.
         * - All numeric characters: 0 to 9.
         * - The space character.
         * - Punctuation characters and other symbols, including: "!", "#", "$", "%", "&", "(", ")", "+", "-", ":", ";", "<", "=", ".", ">", "?", "@", "[", "]", "^", "_", " {", "}", "|", "~", ",".
         * @param info (Optional) Reserved for future use.
         * @param uid (Optional) User ID. A 32-bit unsigned integer with a value ranging from 1 to 2<sup>32</sup>-1. The @p uid must be unique. If a @p uid is
         * not assigned (or set to 0), the SDK assigns and returns a @p uid in the `OnJoinChannelSuccessHandler` callback.
         * Your application must record and maintain the returned `uid`, because the SDK does not do so. **Note**: The ID of each user in the channel should be unique.
         * If you want to join the same channel from different devices, ensure that the user IDs in all devices are different.
         * @param options The channel media options: {@link agora_gaming_rtc.ChannelMediaOptions ChannelMediaOptions}.
         *
         * @return
         * - 0(ERR_OK): Success.
         * - < 0: Failure.
         *    - -2(ERR_INALID_ARGUMENT): The parameter is invalid.
         *    - -3(ERR_NOT_READY): The SDK fails to be initialized. You can try re-initializing the SDK.
         *    - -5(ERR_REFUSED): The request is rejected. This may be caused by the following:
         *        - You have created an `AgoraChannel` object with the same channel name.
         *        - You have joined and published a stream in a channel created by the `AgoraChannel` object. When you join a channel created by the `IRtcEngine` object, the SDK publishes the local audio and video streams to that channel by default. Because the SDK does not support publishing a local stream to more than one channel simultaneously, an error occurs in this occasion.
         *    - -7(ERR_NOT_INITIALIZED): The SDK is not initialized before calling this method.
         */
        public int JoinChannel(string token, string channelId, string info, uint uid, ChannelMediaOptions options)
        {
            return IRtcEngineNative.joinChannelWithMediaOption(token, channelId, info, uid, options.autoSubscribeAudio, options.autoSubscribeVideo, options.publishLocalAudio, options.publishLocalVideo);
        }

        /** Switches to a different channel, and configures whether to automatically subscribe to audio or video 
         * streams in the target channel.
         *
         * @since v3.3.1
         *
         * This method allows the audience of a `LIVE_BROADCASTING` channel to switch to a different channel.
         *
         * After the user successfully switches to another channel, the 
         * {@link agora_gaming_rtc.OnLeaveChannelHandler OnLeaveChannelHandler}
         * and {@link agora_gaming_rtc.OnJoinChannelSuccessHandler OnJoinChannelSuccessHandler} callbacks are triggered 
         * to indicate that the user has left the original channel and joined a new one.
         *
         * @note
         * - This method applies to the audience role in a `LIVE_BROADCASTING` channel only.
         * - The difference between this method and 
         * {@link agora_gaming_rtc.IRtcEngine.SwitchChannel(string token, string channelId) SwitchChannel}1
         * is that the former adds the options parameter to configure whether the user automatically subscribes to all 
         * remote audio and video streams in the target channel.
         * By default, the user subscribes to the audio and video streams of all the other users in the target channel, 
         * thus incurring all associated usage costs.
         * To unsubscribe, set the `options` parameter or call the `mute` methods accordingly.
         *
         * @param token The token generated at your server. For details, see [Generate a token](https://docs.agora.io/en/Interactive%20Broadcast/token_server?platform=Windows).
         * @param channelId Unique channel name for the AgoraRTC session in the
         * string format. The string length must be less than 64 bytes. Supported
         * character scopes are:
         * - All lowercase English letters: a to z.
         * - All uppercase English letters: A to Z.
         * - All numeric characters: 0 to 9.
         * - The space character.
         * - Punctuation characters and other symbols, including: "!", "#", "$", "%", "&", "(", ")", "+", "-", ":", ";", "<", "=", ".", ">", "?", "@", "[", "]", "^", "_", " {", "}", "|", "~", ",".
         * @param options The channel media options: {@link agora_gaming_rtc.ChannelMediaOptions ChannelMediaOptions}.
         *
         * @return
         * - 0(ERR_OK): Success.
         * - < 0: Failure.
         *  - -1(ERR_FAILED): A general error occurs (no specified reason).
         *  - -2(ERR_INALID_ARGUMENT): The parameter is invalid.
         *  - -5(ERR_REFUSED): The request is rejected, probably because the user is not an audience.
         *  - -7(ERR_NOT_INITIALIZED): The SDK is not initialized.
         *  - -102(ERR_INVALID_CHANNEL_NAME): The channel name is invalid.
         *  - -113(ERR_NOT_IN_CHANNEL): The user is not in the channel.
         */
        public int SwitchChannel(string token, string channelId, ChannelMediaOptions options)
        {
            return IRtcEngineNative.switchChannel2(token, channelId, options.autoSubscribeAudio, options.autoSubscribeVideo, options.publishLocalAudio, options.publishLocalVideo);
        }

        /** Sets an SDK preset voice conversion effect.
         *
         * @since v3.3.1
         *
         * Call this method to set an SDK preset voice conversion effect for the
         * local user who sends an audio stream. After setting a voice conversion
         * effect, all users in the channel can hear the effect.
         *
         * You can set different voice conversion effects for different scenarios.
         * See *Set the Voice Effect*.
         *
         * To achieve better voice effect quality, Agora recommends calling
         * {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile} and setting the
         * `profile` parameter to #AUDIO_PROFILE_MUSIC_HIGH_QUALITY (4) or
         * `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)` and the `scenario`
         * parameter to `AUDIO_SCENARIO_GAME_STREAMING(3)` before calling this
         * method.
         *
         * @note
         * - You can call this method either before or after joining a channel.
         * - Do not set the `profile` parameter of `SetAudioProfile` to
         * `AUDIO_PROFILE_SPEECH_STANDARD(1)` or `AUDIO_PROFILE_IOT(6)`; 
         * otherwise, this method call does not take effect.
         * - This method works best with the human voice. Agora does not recommend
         * using this method for audio containing music.
         * - After calling this method, Agora recommends not calling the following
         * methods, because they can override `SetVoiceConversionPreset`:
         *  - {@link agora_gaming_rtc.IRtcEngine.SetAudioEffectPreset SetAudioEffectPreset}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetAudioEffectParameters SetAudioEffectParameters}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetVoiceBeautifierPreset SetVoiceBeautifierPreset}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetVoiceBeautifierParameters SetVoiceBeautifierParameters}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceReverbPreset SetLocalVoiceReverbPreset}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceChanger SetLocalVoiceChanger}
         *  - {@link agora_gaming_rtc.AudioEffectManagerImpl.SetLocalVoicePitch SetLocalVoicePitch}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceEqualization SetLocalVoiceEqualization}
         *  - {@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceReverb SetLocalVoiceReverb}
         *
         * @param preset The options for SDK preset voice conversion effects: 
         * {@link agora_gaming_rtc.VOICE_CONVERSION_PRESET VOICE_CONVERSION_PRESET}.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetVoiceConversionPreset(VOICE_CONVERSION_PRESET preset)
        {
            return IRtcEngineNative.setVoiceConversionPreset((int)preset);
        }

        /// @cond
        /** Uploads all SDK log files.
         *
         * @since v3.3.1
         *
         * Uploads all SDK log files from the client to the Agora server.
         * After a successful method call, the SDK triggers the 
         * {@link agora_gaming_rtc.OnUploadLogResultHandler OnUploadLogResultHandler} callback
         * to report whether the log files are successfully uploaded to the Agora server.
         *
         * For easier debugging, Agora recommends that you bind this method to the UI element of your App, so as to 
         * instruct the user to upload a log file when a quality issue occurs.
         *
         * @note Do not call this method more than once per minute, otherwise the SDK reports `ERR_TOO_OFTEN(12)`.
         *
         * @return
         * - &ge; 0: The request ID, if this method call succeeds. This request ID is the same as requestId in the 
         * `OnUploadLogResultHandler` callback, and you can use the request ID to match a specific upload with a callback.
         * - < 0: Failure.
         *   - -12(ERR_TOO_OFTEN): The call frequency exceeds the limit.
         */
        public string UploadLogFile()
        {
            string requestId = null;
            IntPtr res = IRtcEngineNative.uploadLogFile();
            if (res != IntPtr.Zero)
            {
                requestId = Marshal.PtrToStringAnsi(res);
                IRtcEngineNative.freeObject(res);
            }
            return requestId;
        }
        /// @endcond


        /** Sets the Agora cloud proxy service.
         *
         * @since v3.3.1
         *
         * When the user's firewall restricts the IP address and port, refer to *Use Cloud Proxy* to add the specific
         * IP addresses and ports to the firewall whitelist; then, call this method to enable the cloud proxy and set
         * the `proxyType` parameter as `UDP_PROXY(1)`, which is the cloud proxy for the UDP protocol.
         *
         * After a successfully cloud proxy connection, the SDK triggers the 
         * {@link agora_gaming_rtc.OnConnectionStateChangedHandler OnConnectionStateChangedHandler}`(CONNECTION_STATE_CONNECTING, CONNECTION_CHANGED_SETTING_PROXY_SERVER)` callback.
         *
         * To disable the cloud proxy that has been set, call `SetCloudProxy(NONE_PROXY)`. To change the cloud proxy 
         * type that has been set, call `SetCloudProxy(NONE_PROXY)` first, and then call `SetCloudProxy`, and pass the 
         * value that you expect in `proxyType`.
         *
         * @note
         * - Agora recommends that you call this method before joining the channel or after leaving the channel.
         * - When you use the cloud proxy for the UDP protocol, the services for pushing streams to CDN and co-hosting 
         * across channels are not available.
         *
         * @param proxyType The cloud proxy type, see {@link agora_gaming_rtc.CLOUD_PROXY_TYPE CLOUD_PROXY_TYPE}. 
         * This parameter is required, and the SDK reports an error if you do not pass in a value.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         *  - `-2（ERR_INVALID_ARGUMENT)`: The parameter is invalid.
         *  - `-7(ERR_NOT_INITIALIZED)`: The SDK is not initialized.
         */
        public int SetCloudProxy(CLOUD_PROXY_TYPE proxyType) {
            return IRtcEngineNative.setCloudProxy((int)proxyType);
        }

        public int AdjustLoopbackRecordingSignalVolume(int volume) {
            return IRtcEngineNative.adjustLoopbackRecordingSignalVolume(volume);
        }

        public int StartAudioRecording(AudioRecordingConfiguration config) {
            return IRtcEngineNative.startAudioRecordingWithConfig(config.filePath, (int)config.recordingQuality, (int)config.recordingPosition, config.recordingSampleRate);
        }

        public int SetLocalAccessPoint(string[] ips, string domain) {

            StringBuilder ipsStr = new StringBuilder();
            for (int i = 0; i < ips.Length; i ++) {
                ipsStr.Append(ips[i]);
                ipsStr.Append("\t");
            }
            return IRtcEngineNative.setLocalAccessPoint(ipsStr.ToString(), ips.Length, domain);
        }

        public int EnableVirtualBackground(bool enabled, VirtualBackgroundSource source) {
            return IRtcEngineNative.enableVirtualBackground(enabled, (int)source.background_source_type, source.color, source.source);
        }

        public int SetCameraTorchOn(bool on) {
            return IRtcEngineNative.setCameraTorchOn(on);
        }

        public bool IsCameraTorchSupported() {
            return IRtcEngineNative.isCameraTorchSupported();
        }

        /** Initializes an IRtcEngine instance.
         * 
         * Unless otherwise specified, all the methods provided by the IRtcEngine class are executed asynchronously. Agora recommends calling these methods in the same thread.
         *
         * @note 
         * - You must initialize the IRtcEngine instance before calling any other method.
         * - You can initialize an IRtcEngine instance either by calling this method or by calling {@link agora_gaming_rtc.IRtcEngine.GetEngine(RtcEngineConfig engineConfig) GetEngine2}. The difference between `GetEngine2` and this method is that `GetEngine2` enables you to specify the connection area.
         * - The Agora RTC Unity SDK supports initializing only one IRtcEngine instance for an app for now.
         * 
         * @param appId The App ID issued to you by Agora. See [How to get the App ID](https://docs.agora.io/en/Agora%20Platform/token#getappid).
         * Only users in apps with the same App ID can join the same channel and communicate with each other. Use an App ID to initialize only
         * one `IRtcEngine` instance. To change your App ID, call {@link agora_gaming_rtc.IRtcEngine.Destroy Destroy} to destroy the current `IRtcEngine` instance and then call this method to initialize an `IRtcEngine` instance with the new App ID.
         * 
         * @return 
         * - The IRtcEngine instance, if this method call succeeds.
         * - The error code, if this method call fails.
         *   - `ERR_INVALID_APP_ID (101)`: The App ID is invalid. Check if your App ID is in the correct format.
         */
        public static IRtcEngine GetEngine(string appId)
        {
            if (instance == null)
            {
                instance = new IRtcEngine(appId);
            }
            return instance;
        }

        /** Initializes an IRtcEngine instance.
         * 
         * Unless otherwise specified, all the methods provided by the IRtcEngine class are executed asynchronously. Agora recommends calling these methods in the same thread.
         *
         * @note 
         * - You must initialize the IRtcEngine instance before calling any other method.
         * - You can initialize an IRtcEngine instance either by calling this method or by calling {@link agora_gaming_rtc.IRtcEngine.GetEngine(string appId) GetEngine1}. The difference between `GetEngine1` and this method is that this method enables you to specify the connection area.
         * - The Agora RTC Unity SDK supports initializing only one IRtcEngine instance for an app for now.
         * 
         * @param engineConfig Configurations for the IRtcEngine instance. For details, see {@link agora_gaming_rtc.RtcEngineConfig RtcEngineConfig}.
         * 
         * @return 
         * - The IRtcEngine instance, if this method call succeeds.
         * - The error code, if this method call fails.
         *   - `ERR_INVALID_APP_ID (101)`: The App ID is invalid. Check if your App ID is in the correct format.
         */
        public static IRtcEngine GetEngine(RtcEngineConfig engineConfig)
        {
            if (instance == null)
            {
                instance = new IRtcEngine(engineConfig);
            }
            return instance;
        }
        
        /** Initializes the IRtcEngine.
         *
         * @deprecated Use {@link agora_gaming_rtc.IRtcEngine.GetEngine GetEngine} instead.
         * 
         * @param appId The App ID of your project.
         * 
         * @return The IRtcEngine instance.
         */
        public static IRtcEngine getEngine(string appId)
        {
            return GetEngine(appId);
        }

        /** Destroys the `IRtcEngine` instance and releases all resources used by the Agora RTC SDK.
         * 
         * Use this method for apps in which users occasionally make voice or video calls. When users do not make calls, you
         * can free up resources for other operations. Once you call `Destroy` to destroy the created `IRtcEngine` instance,
         * you cannot use any method or callback in the SDK any more. If you want to use the real-time communication functions
         * again, you must call {@link agora_gaming_rtc.IRtcEngine.GetEngine(string appId) GetEngine}
         * to Initialize a new `IRtcEngine` instance.
         * 
         * @note
         * - Because `Destroy` is a synchronous method and the app cannot move on to another task until the execution completes, Agora suggests calling this method in a sub-thread to avoid congestion in the main thread. Besides, you **cannot** call `Destroy` in any method or callback of the SDK. Otherwise, the SDK cannot release the resources occupied by the `IRtcEngine` instance until the callbacks return results, which may result in a deadlock.
         * - If you want to create a new `IRtcEngine` instance after destroying the current one, ensure that you wait till the `destroy` method completes executing.
         */
        public static void Destroy()
        {
            if (instance != null)
            {
                // break the connection with mAudioEffectM
                AudioEffectManagerImpl am = (AudioEffectManagerImpl)instance.GetAudioEffectManager();
                if (am != null)
                {
                    am.SetEngine(null);
                    AudioEffectManagerImpl.ReleaseInstance();
                }
                
                AudioRecordingDeviceManager adm = (AudioRecordingDeviceManager)instance.GetAudioRecordingDeviceManager();
                if (adm != null)
                {
                    adm.SetEngine(null);
                    AudioRecordingDeviceManager.ReleaseInstance();
                }

                AudioPlaybackDeviceManager apm = (AudioPlaybackDeviceManager)instance.GetAudioPlaybackDeviceManager();
                if (apm != null)
                {
                    apm.SetEngine(null);
                    AudioPlaybackDeviceManager.ReleaseInstance();
                }

                VideoDeviceManager vdm = (VideoDeviceManager)instance.GetVideoDeviceManager();
                if (vdm != null)
                {
                    vdm.SetEngine(null);
                    VideoDeviceManager.ReleaseInstance();
                }

                AudioRawDataManager ardm = (AudioRawDataManager)instance.GetAudioRawDataManager();
                if (ardm != null)
                {
                    ardm.SetEngine(null);
                    AudioRawDataManager.ReleaseInstance();
                }

                VideoRawDataManager vrdm = (VideoRawDataManager)instance.GetVideoRawDataManager();
                if (vrdm != null)
                {
                    vrdm.SetEngine(null);
                    VideoRawDataManager.ReleaseInstance();
                }

                VideoRender vr = (VideoRender)instance.GetVideoRender();
                if (vr != null)
                {
                    vr.SetEngine(null);
                    VideoRender.ReleaseInstance();
                } 
            }
            
            IRtcEngineNative.deleteEngine();
            AgoraChannel.Release();
            
            if (instance != null)
            {
#if !UNITY_EDITOR && UNITY_WEBGL
#else
                instance.DeInitGameObject();
#endif
            }

            instance = null;
            GC.Collect();
        }

        // only query, do not create 
        /** Query the IRtcEngine instance.
         *
         * @note Call this method after calling {@link agora_gaming_rtc.IRtcEngine.GetEngine(string appId) GetEngine}.
         *
         * @return The IRtcEngine instance.
         */
        public static IRtcEngine QueryEngine()
        {
            return instance;
        }
        
        private static IRtcEngine instance = null;

        [MonoPInvokeCallback(typeof(OnJoinChannelSuccessHandler))]
        private static void OnJoinChannelSuccessCallback(string channel, uint uid, int elapsed)
        {
            if (instance != null && instance.OnJoinChannelSuccess != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnJoinChannelSuccess != null)
                        { 
                            instance.OnJoinChannelSuccess(channel, uid, elapsed);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnLeaveChannelHandler))]
        private static void OnLeaveChannelCallback(uint duration, uint txBytes, uint rxBytes, uint txAudioBytes, uint txVideoBytes, uint rxAudioBytes, uint rxVideoBytes, ushort txKBitRate, ushort rxKBitRate, ushort rxAudioKBitRate, ushort txAudioKBitRate, ushort rxVideoKBitRate, ushort txVideoKBitRate, ushort lastmileDelay, ushort txPacketLossRate, ushort rxPacketLossRate, uint userCount, double cpuAppUsage, double cpuTotalUsage, int gatewayRtt, double memoryAppUsageRatio, double memoryTotalUsageRatio, int memoryAppUsageInKbytes)
        {
            if (instance != null && instance.OnLeaveChannel != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnLeaveChannel != null)
                        { 
                            RtcStats rtcStats = new RtcStats();
                            rtcStats.duration = duration;
                            rtcStats.txBytes = txBytes;
                            rtcStats.rxBytes = rxBytes;
                            rtcStats.txAudioBytes = txAudioBytes;
                            rtcStats.txVideoBytes = txVideoBytes;
                            rtcStats.rxAudioBytes = rxAudioBytes;
                            rtcStats.rxVideoBytes = rxVideoBytes;
                            rtcStats.txKBitRate = txKBitRate;
                            rtcStats.rxKBitRate = rxKBitRate;
                            rtcStats.rxAudioKBitRate = rxAudioKBitRate;
                            rtcStats.txAudioKBitRate = txAudioKBitRate;
                            rtcStats.rxVideoKBitRate = rxVideoKBitRate;
                            rtcStats.txVideoKBitRate = txVideoKBitRate;
                            rtcStats.lastmileDelay = lastmileDelay;
                            rtcStats.txPacketLossRate = txPacketLossRate;
                            rtcStats.rxPacketLossRate = rxPacketLossRate;
                            rtcStats.userCount = userCount;
                            rtcStats.cpuAppUsage = cpuAppUsage;
                            rtcStats.cpuTotalUsage = cpuTotalUsage;
                            rtcStats.gatewayRtt = gatewayRtt;
                            rtcStats.memoryAppUsageRatio = memoryAppUsageRatio;
                            rtcStats.memoryTotalUsageRatio = memoryTotalUsageRatio;
                            rtcStats.memoryAppUsageInKbytes = memoryAppUsageInKbytes;
                            instance.OnLeaveChannel(rtcStats);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnReJoinChannelSuccessHandler))]
        private static void OnReJoinChannelSuccessCallback(string channelName, uint uid, int elapsed)
        {
            if (instance != null && instance.OnReJoinChannelSuccess != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnReJoinChannelSuccess != null)
                        { 
                            instance.OnReJoinChannelSuccess(channelName, uid, elapsed);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnConnectionLostHandler))]
        private static void OnConnectionLostCallback()
        {
            if (instance != null && instance.OnConnectionLost != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnConnectionLost != null)
                        { 
                            instance.OnConnectionLost();
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnConnectionInterruptedHandler))]
        private static void OnConnectionInterruptedCallback()
        {
            if (instance != null && instance.OnConnectionInterrupted != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnConnectionInterrupted != null)
                        { 
                            instance.OnConnectionInterrupted();
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnRequestTokenHandler))]
        private static void OnRequestTokenCallback()
        {
            if (instance != null && instance.OnRequestToken != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnRequestToken != null)
                        { 
                            instance.OnRequestToken();
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnUserJoinedHandler))]
        private static void OnUserJoinedCallback(uint uid, int elapsed)
        {
            if (instance != null && instance.OnUserJoined != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnUserJoined != null)
                        { 
                            instance.OnUserJoined(uid, elapsed);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnUserOfflineHandler))]
        private static void OnUserOfflineCallback(uint uid, int reason)
        {
            if (instance != null && instance.OnUserOffline != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnUserOffline != null)
                        { 
                            instance.OnUserOffline(uid, (USER_OFFLINE_REASON)reason);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnAudioVolumeIndicationHandler))]
        private static void OnAudioVolumeIndicationCallback(string volumeInfo, int speakerNumber, int totalVolume)
        {
            if (instance != null && instance.OnVolumeIndication != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnVolumeIndication != null)
                        {
                            string[] sArray = volumeInfo.Split('\t');
                            int j = 1;
                            AudioVolumeInfo[] infos = new AudioVolumeInfo[speakerNumber];
                            if (speakerNumber > 0)
                            {
                                for (int i = 0; i < speakerNumber; i++)
                                {
                                    uint uids = (uint)int.Parse(sArray[j++]);
                                    uint vol = (uint)int.Parse(sArray[j++]);
                                    uint vad = (uint)int.Parse(sArray[j++]);
                                    string channelId = sArray[j++];
                                    infos[i].uid = uids;
                                    infos[i].volume = vol;
                                    infos[i].vad = vad;
                                    infos[i].channelId = channelId;
                                }
                            }
                            instance.OnVolumeIndication(infos, speakerNumber, totalVolume);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnUserMutedAudioHandler))]
        private static void OnUserMuteAudioCallback(uint uid, bool muted)
        {
            if (instance != null && instance.OnUserMutedAudio != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnUserMutedAudio != null)
                        { 
                            instance.OnUserMutedAudio(uid, muted);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnSDKWarningHandler))]
        private static void OnSDKWarningCallback(int warn, string msg)
        {
#if UNITY_2017_4_OR_NEWER
            if (warn == 8 || warn == 16)
                return;
#endif
            if (instance != null && instance.OnWarning != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnWarning != null)
                        {
                            instance.OnWarning(warn, msg);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnSDKErrorHandler))]
        private static void OnSDKErrorCallback(int error, string msg)
        {
            if (instance != null && instance.OnError != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnError != null)
                        { 
                            instance.OnError(error, msg);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnRtcStatsHandler))]
        private static void OnRtcStatsCallback(uint duration, uint txBytes, uint rxBytes, uint txAudioBytes, uint txVideoBytes, uint rxAudioBytes, uint rxVideoBytes, ushort txKBitRate, ushort rxKBitRate, ushort rxAudioKBitRate, ushort txAudioKBitRate, ushort rxVideoKBitRate, ushort txVideoKBitRate, ushort lastmileDelay, ushort txPacketLossRate, ushort rxPacketLossRate, uint userCount, double cpuAppUsage, double cpuTotalUsage, int gatewayRtt, double memoryAppUsageRatio, double memoryTotalUsageRatio, int memoryAppUsageInKbytes)
        {
            if (instance != null && instance.OnRtcStats != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnRtcStats != null)
                        {
                            RtcStats rtcStats = new RtcStats();
                            rtcStats.duration = duration;
                            rtcStats.txBytes = txBytes;
                            rtcStats.rxBytes = rxBytes;
                            rtcStats.txAudioBytes = txAudioBytes;
                            rtcStats.txVideoBytes = txVideoBytes;
                            rtcStats.rxAudioBytes = rxAudioBytes;
                            rtcStats.rxVideoBytes = rxVideoBytes;
                            rtcStats.txKBitRate = txKBitRate;
                            rtcStats.rxKBitRate = rxKBitRate;
                            rtcStats.rxAudioKBitRate = rxAudioKBitRate;
                            rtcStats.txAudioKBitRate = txAudioKBitRate;
                            rtcStats.rxVideoKBitRate = rxVideoKBitRate;
                            rtcStats.txVideoKBitRate = txVideoKBitRate;
                            rtcStats.lastmileDelay = lastmileDelay;
                            rtcStats.txPacketLossRate = txPacketLossRate;
                            rtcStats.rxPacketLossRate = rxPacketLossRate;
                            rtcStats.userCount = userCount;
                            rtcStats.cpuAppUsage = cpuAppUsage;
                            rtcStats.cpuTotalUsage = cpuTotalUsage;
                            rtcStats.gatewayRtt = gatewayRtt;
                            rtcStats.memoryAppUsageRatio = memoryAppUsageRatio;
                            rtcStats.memoryTotalUsageRatio = memoryTotalUsageRatio;
                            rtcStats.memoryAppUsageInKbytes = memoryAppUsageInKbytes;
                            instance.OnRtcStats(rtcStats);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnAudioMixingFinishedHandler))]
        private static void OnAudioMixingFinishedCallback()
        {
            if (instance != null && instance.OnAudioMixingFinished != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnAudioMixingFinished != null)
                        { 
                            instance.OnAudioMixingFinished();
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnAudioRouteChangedHandler))]
        private static void OnAudioRouteChangedCallback(int route)
        {
            if (instance != null && instance.OnAudioRouteChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnAudioRouteChanged != null)
                        { 
                            instance.OnAudioRouteChanged((AUDIO_ROUTE)route);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnFirstRemoteVideoDecodedHandler))]
        private static void OnFirstRemoteVideoDecodedCallback(uint uid, int width, int height, int elapsed)
        {
            if (instance != null && instance.OnFirstRemoteVideoDecoded != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnFirstRemoteVideoDecoded != null)
                        { 
                            instance.OnFirstRemoteVideoDecoded(uid, width, height, elapsed);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnVideoSizeChangedHandler))]
        private static void OnVideoSizeChangedCallback(uint uid, int width, int height, int rotation)
        {
            if (instance != null && instance.OnVideoSizeChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnVideoSizeChanged != null)
                        { 
                            instance.OnVideoSizeChanged(uid, width, height, rotation);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnClientRoleChanged))]
        private static void OnClientRoleChangedCallback(int oldRole, int newRole)
        {
            if (instance != null && instance.OnClientRoleChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnClientRoleChanged != null)
                        { 
                            instance.OnClientRoleChanged((CLIENT_ROLE_TYPE)oldRole, (CLIENT_ROLE_TYPE)newRole);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnUserMuteVideoHandler))]
        private static void OnUserMuteVideoCallback(uint uid, bool muted)
        {
            if (instance != null && instance.OnUserMuteVideo != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnUserMuteVideo != null)
                        { 
                            instance.OnUserMuteVideo(uid, muted);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnMicrophoneEnabledHandler))]
        private static void OnMicrophoneEnabledCallback(bool isEnabled)
        {
            if (instance != null && instance.OnMicrophoneEnabled != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnMicrophoneEnabled != null)
                        { 
                            instance.OnMicrophoneEnabled(isEnabled);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnApiExecutedHandler))]
        private static void OnApiExecutedCallback(int err, string api, string result)
        {
            if (instance != null && instance.OnApiExecuted != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnApiExecuted != null)
                        { 
                            instance.OnApiExecuted(err, api, result);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnFirstLocalAudioFrameHandler))]
        private static void OnFirstLocalAudioFrameCallback(int elapsed)
        {
            if (instance != null && instance.OnFirstLocalAudioFrame != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnFirstLocalAudioFrame != null)
                        { 
                            instance.OnFirstLocalAudioFrame(elapsed);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnFirstRemoteAudioFrameHandler))]
        private static void OnFirstRemoteAudioFrameCallback(uint userId, int elapsed)
        {
            if (instance != null && instance.OnFirstRemoteAudioFrame != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnFirstRemoteAudioFrame != null)
                        { 
                            instance.OnFirstRemoteAudioFrame(userId, elapsed);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnLastmileQualityHandler))]
        private static void OnLastmileQualityCallback(int quality)
        {
            if (instance != null && instance.OnLastmileQuality != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnLastmileQuality != null)
                        { 
                            instance.OnLastmileQuality(quality);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnAudioQualityHandler))]
        private static void OnAudioQualityCallback(uint userId, int quality, ushort delay, ushort lost)
        {
            if (instance != null && instance.OnAudioQuality != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnAudioQuality != null)
                        { 
                            instance.OnAudioQuality(userId, quality, delay, lost);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnStreamInjectedStatusHandler))]
        private static void OnStreamInjectedStatusCallback(string url, uint userId, int status)
        {
            if (instance != null && instance.OnStreamInjectedStatus != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnStreamInjectedStatus != null)
                        {
                            instance.OnStreamInjectedStatus(url, userId, status);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnStreamUnpublishedHandler))]
        private static void OnStreamUnpublishedCallback(string url)
        {
            if (instance != null && instance.OnStreamUnpublished != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnStreamUnpublished != null)
                        { 
                            instance.OnStreamUnpublished(url);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnStreamPublishedHandler))]
        private static void OnStreamPublishedCallback(string url, int error)
        {
            if (instance != null && instance.OnStreamPublished != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnStreamPublished != null)
                        { 
                            instance.OnStreamPublished(url, error);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnStreamMessageErrorHandler))]
        private static void OnStreamMessageErrorCallback(uint userId, int streamId, int code, int missed, int cached)
        {
            if (instance != null && instance.OnStreamMessageError != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnStreamMessageError != null)
                        { 
                            instance.OnStreamMessageError(userId, streamId, code, missed,cached);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnStreamMessageHandler))]
        private static void OnStreamMessageCallback(uint userId, int streamId, IntPtr data, int length)
        {
            if (instance != null && instance.OnStreamMessage != null && instance._AgoraCallbackObject != null)
            {
                byte[] byteBuffer = null;
                if (length > 0 && data != IntPtr.Zero) {
                    byteBuffer = new byte[length];
                    Marshal.Copy(data, byteBuffer, 0, length);
                }
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnStreamMessage != null)
                        { 
                            instance.OnStreamMessage(userId, streamId, byteBuffer, length);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnConnectionBannedHandler))]
        private static void OnConnectionBannedCallback()
        {
            if (instance != null && instance.OnConnectionBanned != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnConnectionBanned != null)
                        {
                            instance.OnConnectionBanned();
                        }    
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnConnectionStateChanged))]
        private static void OnConnectionStateChangedCallback(int state, int reason)
        {
            if (instance != null && instance.OnConnectionStateChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnConnectionStateChanged != null)
                        {
                            instance.OnConnectionStateChanged((CONNECTION_STATE_TYPE)state, (CONNECTION_CHANGED_REASON_TYPE)reason);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnTokenPrivilegeWillExpireHandler))]
        private static void OnTokenPrivilegeWillExpireCallback(string token)
        {
            if (instance != null && instance.OnTokenPrivilegeWillExpire != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnTokenPrivilegeWillExpire != null)
                        { 
                            instance.OnTokenPrivilegeWillExpire(token);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnActiveSpeakerHandler))]
        private static void OnActiveSpeakerCallback(uint uid)
        {
            if (instance != null && instance.OnActiveSpeaker != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnActiveSpeaker != null)
                        {
                            instance.OnActiveSpeaker(uid);
                        }          
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnVideoStoppedHandler))]
        private static void OnVideoStoppedCallback()
        {
            if (instance != null && instance.OnVideoStopped != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnVideoStopped != null)
                        { 
                            instance.OnVideoStopped();
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnFirstLocalVideoFrameHandler))]
        private static void OnFirstLocalVideoFrameCallback(int width, int height, int elapsed)
        {
            if (instance != null && instance.OnFirstLocalVideoFrame != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnFirstLocalVideoFrame != null)
                        { 
                            instance.OnFirstLocalVideoFrame(width, height, elapsed);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnFirstRemoteVideoFrameHandler))]
        private static void OnFirstRemoteVideoFrameCallback(uint uid, int width, int height, int elapsed)
        {
            if (instance != null && instance.OnFirstRemoteVideoFrame != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnFirstRemoteVideoFrame != null)
                        { 
                            instance.OnFirstRemoteVideoFrame(uid, width, height, elapsed);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnUserEnableVideoHandler))]
        private static void OnUserEnableVideoCallback(uint uid, bool enabled)
        {
            if (instance != null && instance.OnUserEnableVideo != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnUserEnableVideo != null)
                        { 
                            instance.OnUserEnableVideo(uid, enabled);
                        }
                    });
                }
            }
        }


        [MonoPInvokeCallback(typeof(OnUserEnableLocalVideoHandler))]
        private static void OnUserEnableLocalVideoCallback(uint uid, bool enabled)
        {
            if (instance != null && instance.OnUserEnableLocalVideo != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnUserEnableLocalVideo != null)
                        { 
                            instance.OnUserEnableLocalVideo(uid, enabled);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnRemoteVideoStateChanged))]
        private static void OnRemoteVideoStateChangedCallback(uint uid, int state, int reason, int elapsed)
        {
            if (instance != null && instance.OnRemoteVideoStateChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnRemoteVideoStateChanged != null)
                        { 
                            instance.OnRemoteVideoStateChanged(uid, (REMOTE_VIDEO_STATE)state, (REMOTE_VIDEO_STATE_REASON)reason, elapsed);
                        }
                    });
                }
            }
        }


        [MonoPInvokeCallback(typeof(OnLocalPublishFallbackToAudioOnlyHandler))]
        private static void OnLocalPublishFallbackToAudioOnlyCallback(bool isFallbackOrRecover)
        {
            if (instance != null && instance.OnLocalPublishFallbackToAudioOnly != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnLocalPublishFallbackToAudioOnly != null)
                        { 
                            instance.OnLocalPublishFallbackToAudioOnly(isFallbackOrRecover);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnRemoteSubscribeFallbackToAudioOnlyHandler))]
        private static void OnRemoteSubscribeFallbackToAudioOnlyCallback(uint uid, bool isFallbackOrRecover)
        {
            if (instance != null && instance.OnRemoteSubscribeFallbackToAudioOnly != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnRemoteSubscribeFallbackToAudioOnly != null)
                        { 
                            instance.OnRemoteSubscribeFallbackToAudioOnly(uid, isFallbackOrRecover);
                        }
                    });
                }
            }
        }


        [MonoPInvokeCallback(typeof(OnNetworkQualityHandler))]
        private static void OnNetworkQualityCallback(uint uid, int txQuality, int rxQuality)
        {
            if (instance != null && instance.OnNetworkQuality != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnNetworkQuality != null)
                        { 
                            instance.OnNetworkQuality(uid, txQuality, rxQuality);
                        }
                    });
                }
            }
        }  

        [MonoPInvokeCallback(typeof(EngineEventOnLocalVideoStatsHandler))]
        private static void OnLocalVideoStatsCallback(int sentBitrate, int sentFrameRate, int encoderOutputFrameRate, int rendererOutputFrameRate, int targetBitrate, int targetFrameRate, int qualityAdaptIndication, int encodedBitrate, int encodedFrameWidth, int encodedFrameHeight, int encodedFrameCount, int codecType, ushort txPacketLossRate, int captureFrameRate, int captureBrightnessLevel)
        {
            if (instance != null && instance.OnLocalVideoStats != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnLocalVideoStats != null)
                        { 
                            LocalVideoStats localVideoStats = new LocalVideoStats();
                            localVideoStats.sentBitrate = sentBitrate;
                            localVideoStats.sentFrameRate = sentFrameRate;
                            localVideoStats.encoderOutputFrameRate = encoderOutputFrameRate;
                            localVideoStats.rendererOutputFrameRate = rendererOutputFrameRate;
                            localVideoStats.targetBitrate = targetBitrate;
                            localVideoStats.targetFrameRate = targetFrameRate;
                            localVideoStats.qualityAdaptIndication = (QUALITY_ADAPT_INDICATION)qualityAdaptIndication;
                            localVideoStats.encodedBitrate = encodedBitrate;
                            localVideoStats.encodedFrameWidth = encodedFrameWidth;
                            localVideoStats.encodedFrameHeight = encodedFrameHeight;
                            localVideoStats.encodedFrameCount = encodedFrameCount;
                            localVideoStats.codecType = (VIDEO_CODEC_TYPE)codecType;
                            localVideoStats.txPacketLossRate = txPacketLossRate;
                            localVideoStats.captureFrameRate = captureFrameRate;
                            localVideoStats.captureBrightnessLevel  = (CAPTURE_BRIGHTNESS_LEVEL_TYPE)captureBrightnessLevel;
                            instance.OnLocalVideoStats(localVideoStats);
                        }
                    });
                }
            }
        }  

        [MonoPInvokeCallback(typeof(EngineEventOnRemoteVideoStatsHandler))]
        private static void OnRemoteVideoStatsCallback(uint uid, int delay, int width, int height, int receivedBitrate, int decoderOutputFrameRate, int rendererOutputFrameRate, int packetLossRate, int rxStreamType, int totalFrozenTime, int frozenRate, int totalActiveTime, int publishDuration)
        {
            if (instance != null && instance.OnRemoteVideoStats != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnRemoteVideoStats != null)
                        { 
                            RemoteVideoStats remoteVideoStats = new RemoteVideoStats();
                            remoteVideoStats.uid = uid;
                            remoteVideoStats.delay = delay;
                            remoteVideoStats.width = width;
                            remoteVideoStats.height = height;
                            remoteVideoStats.receivedBitrate = receivedBitrate;
                            remoteVideoStats.decoderOutputFrameRate = decoderOutputFrameRate;
                            remoteVideoStats.rendererOutputFrameRate = rendererOutputFrameRate;
                            remoteVideoStats.packetLossRate = packetLossRate;
                            remoteVideoStats.rxStreamType = (REMOTE_VIDEO_STREAM_TYPE)rxStreamType;
                            remoteVideoStats.totalFrozenTime = totalFrozenTime;
                            remoteVideoStats.frozenRate = frozenRate;
                            remoteVideoStats.totalActiveTime = totalActiveTime;
                            remoteVideoStats.publishDuration = publishDuration;
                            instance.OnRemoteVideoStats(remoteVideoStats);
                        }
                    });
                }
            }
        } 

        [MonoPInvokeCallback(typeof(EngineEventOnRemoteAudioStatsHandler))]
        private static void OnRemoteAudioStatsCallback(uint uid, int quality, int networkTransportDelay, int jitterBufferDelay, int audioLossRate, int numChannels, int receivedSampleRate, int receivedBitrate, int totalFrozenTime, int frozenRate, int totalActiveTime, int publishDuration, int qoeQuality, int qualityChangedReason, int mosValue)
        {
            if (instance != null && instance.OnRemoteAudioStats != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnRemoteAudioStats != null)
                        {
                            RemoteAudioStats remoteAudioStats = new RemoteAudioStats();
                            remoteAudioStats.uid = uid;
                            remoteAudioStats.quality = quality;
                            remoteAudioStats.networkTransportDelay = networkTransportDelay;
                            remoteAudioStats.jitterBufferDelay = jitterBufferDelay;
                            remoteAudioStats.audioLossRate = audioLossRate;
                            remoteAudioStats.numChannels = numChannels;
                            remoteAudioStats.receivedSampleRate = receivedSampleRate;
                            remoteAudioStats.receivedBitrate = receivedBitrate;
                            remoteAudioStats.totalFrozenTime = totalFrozenTime;
                            remoteAudioStats.frozenRate = frozenRate;
                            remoteAudioStats.totalActiveTime = totalActiveTime;
                            remoteAudioStats.publishDuration = publishDuration;
                            remoteAudioStats.qoeQuality = qoeQuality;
                            remoteAudioStats.qualityChangedReason = qualityChangedReason;
                            remoteAudioStats.mosValue = mosValue;
                            instance.OnRemoteAudioStats(remoteAudioStats);
                        }
                    });
                }
            }
        } 

        [MonoPInvokeCallback(typeof(OnAudioDeviceStateChangedHandler))]
        private static void OnAudioDeviceStateChangedCallback(string deviceId, int deviceType, int deviceState)
        {
            if (instance != null && instance.OnAudioDeviceStateChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnAudioDeviceStateChanged != null)
                        { 
                            instance.OnAudioDeviceStateChanged(deviceId, deviceType, deviceState);
                        }
                    });
                }
            }
        } 

        [MonoPInvokeCallback(typeof(OnCameraReadyHandler))]
        private static void OnCameraReadyCallback()
        {
            if (instance != null && instance.OnCameraReady != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnCameraReady != null)
                        { 
                            instance.OnCameraReady();
                        }
                    });
                }
            }
        } 

        [MonoPInvokeCallback(typeof(OnCameraFocusAreaChangedHandler))]
        private static void OnCameraFocusAreaChangedCallback(int x, int y, int width, int height)
        {
            if (instance != null && instance.OnCameraFocusAreaChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnCameraFocusAreaChanged != null)
                        { 
                            instance.OnCameraExposureAreaChanged(x, y, width, height);
                        }
                    });
                }
            }
        } 

        [MonoPInvokeCallback(typeof(OnCameraExposureAreaChangedHandler))]
        private static void OnCameraExposureAreaChangedCallback(int x, int y, int width, int height)
        {
            if (instance != null && instance.OnCameraExposureAreaChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnCameraExposureAreaChanged != null)
                        { 
                            instance.OnCameraExposureAreaChanged(x, y, width, height);
                        }
                    });
                }
            }
        } 

        [MonoPInvokeCallback(typeof(OnRemoteAudioMixingBeginHandler))]
        private static void OnRemoteAudioMixingBeginCallback()
        {
            if (instance != null && instance.OnRemoteAudioMixingBegin != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnRemoteAudioMixingBegin != null)
                        { 
                            instance.OnRemoteAudioMixingBegin();
                        }
                    });
                }
            }
        } 

        [MonoPInvokeCallback(typeof(OnRemoteAudioMixingEndHandler))]
        private static void OnRemoteAudioMixingEndCallback()
        {
            if (instance != null && instance.OnRemoteAudioMixingEnd != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnRemoteAudioMixingEnd != null)
                        { 
                            instance.OnRemoteAudioMixingEnd();
                        }
                    });
                }
            }
        } 

        [MonoPInvokeCallback(typeof(OnAudioEffectFinishedHandler))]
        private static void OnAudioEffectFinishedCallback(int soundId)
        {
            if (instance != null && instance.OnAudioEffectFinished != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnAudioEffectFinished != null)
                        { 
                            instance.OnAudioEffectFinished(soundId);
                        }
                    });
                }
            }
        } 

       [MonoPInvokeCallback(typeof(OnVideoDeviceStateChangedHandler))]
        private static void OnVideoDeviceStateChangedCallback(string deviceId, int deviceType, int deviceState)
        {
            if (instance != null && instance.OnVideoDeviceStateChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnVideoDeviceStateChanged != null)
                        { 
                            instance.OnVideoDeviceStateChanged(deviceId, deviceType, deviceState);
                        }
                    });
                }
            }
        } 

       [MonoPInvokeCallback(typeof(OnRemoteVideoTransportStatsHandler))]
        private static void OnRemoteVideoTransportStatsCallback(uint uid, ushort delay, ushort lost, ushort rxKBitRate)
        {
            if (instance != null && instance.OnRemoteVideoTransportStats != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnRemoteVideoTransportStats != null)
                        { 
                            instance.OnRemoteVideoTransportStats(uid, delay, lost, rxKBitRate);
                        }
                    });
                }
            }
        } 

        [MonoPInvokeCallback(typeof(OnRemoteAudioTransportStatsHandler))]
        private static void OnRemoteAudioTransportStatsCallback(uint uid, ushort delay, ushort lost, ushort rxKBitRate)
        {
            if (instance != null && instance.OnRemoteAudioTransportStats != null && instance._AgoraCallbackObject != null)
            {
                                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnRemoteAudioTransportStats != null)
                        { 
                            instance.OnRemoteAudioTransportStats(uid, delay, lost, rxKBitRate);
                        }
                    });
                }
            }
        } 

       [MonoPInvokeCallback(typeof(OnTranscodingUpdatedHandler))]
        private static void OnTranscodingUpdatedCallback()
        {
            if (instance != null && instance.OnTranscodingUpdated != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnTranscodingUpdated != null)
                        { 
                            instance.OnTranscodingUpdated();
                        }
                    });
                }
            }
        } 

       [MonoPInvokeCallback(typeof(EngineEventOnAudioDeviceVolumeChangedHandler))]
        private static void OnAudioDeviceVolumeChangedCallback(int deviceType, int volume, bool muted)
        {
            if (instance != null && instance.OnAudioDeviceVolumeChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnAudioDeviceVolumeChanged != null)
                        { 
                            instance.OnAudioDeviceVolumeChanged((MEDIA_DEVICE_TYPE)deviceType, volume, muted);
                        }
                    });
                }
            }
        } 

       [MonoPInvokeCallback(typeof(OnMediaEngineStartCallSuccessHandler))]
        private static void OnMediaEngineStartCallSuccessCallback()
        {
            if (instance != null && instance.OnMediaEngineStartCallSuccess != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnMediaEngineStartCallSuccess != null)
                        { 
                            instance.OnMediaEngineStartCallSuccess();
                        }
                    });
                }
            }
        } 

       [MonoPInvokeCallback(typeof(OnMediaEngineLoadSuccessHandler))]
        private static void OnMediaEngineLoadSuccessCallback()
        {
            if (instance != null && instance.OnMediaEngineLoadSuccess != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnMediaEngineLoadSuccess != null)
                        { 
                            instance.OnMediaEngineLoadSuccess();
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnAudioMixingStateChangedHandler))]
        private static void OnAudioMixingStateChangedCallback(int audioMixingStateType, int audioMixingErrorType)
        {
            if (instance != null && instance.OnAudioMixingStateChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnAudioMixingStateChanged != null)
                        { 
                            instance.OnAudioMixingStateChanged((AUDIO_MIXING_STATE_TYPE)audioMixingStateType, (AUDIO_MIXING_REASON_TYPE)audioMixingErrorType);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnFirstRemoteAudioDecodedHandler))]
        private static void OnFirstRemoteAudioDecodedCallback(uint uid, int elapsed)
        {
            if (instance != null && instance.OnFirstRemoteAudioDecoded != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnFirstRemoteAudioDecoded != null)
                        { 
                            instance.OnFirstRemoteAudioDecoded(uid, elapsed);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnLocalVideoStateChanged))]
        private static void OnLocalVideoStateChangedCallback(int localVideoState, int error)
        {
            if (instance != null && instance.OnLocalVideoStateChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnLocalVideoStateChanged != null)
                        { 
                            instance.OnLocalVideoStateChanged((LOCAL_VIDEO_STREAM_STATE)localVideoState, (LOCAL_VIDEO_STREAM_ERROR)error);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnRtmpStreamingStateChangedHandler))]
        private static void OnRtmpStreamingStateChangedCallback(string url, int state, int errCode)
        {
            if (instance != null && instance.OnRtmpStreamingStateChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnRtmpStreamingStateChanged != null)
                        { 
                            instance.OnRtmpStreamingStateChanged(url, (RTMP_STREAM_PUBLISH_STATE)state, (RTMP_STREAM_PUBLISH_ERROR)errCode);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnNetworkTypeChangedHandler))]
        private static void OnNetworkTypeChangedCallback(int networkType)
        {
            if (instance != null && instance.OnNetworkTypeChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnNetworkTypeChanged != null)
                        { 
                            instance.OnNetworkTypeChanged((NETWORK_TYPE)networkType);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnLastmileProbeResultHandler))]
        private static void OnLastmileProbeResultCallback(int state, uint upLinkPacketLossRate, uint upLinkjitter, uint upLinkAvailableBandwidth, uint downLinkPacketLossRate, uint downLinkJitter, uint downLinkAvailableBandwidth,uint rtt)
        {
            if (instance != null && instance.OnLastmileProbeResult != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnLastmileProbeResult != null)
                        {
                            LastmileProbeResult lastmileProbeResult = new LastmileProbeResult();
                            lastmileProbeResult.state = (LASTMILE_PROBE_RESULT_STATE)state;
                            lastmileProbeResult.uplinkReport.packetLossRate = upLinkPacketLossRate;
                            lastmileProbeResult.uplinkReport.jitter = upLinkjitter;
                            lastmileProbeResult.uplinkReport.availableBandwidth = upLinkAvailableBandwidth;
                            lastmileProbeResult.downlinkReport.packetLossRate = downLinkPacketLossRate;
                            lastmileProbeResult.downlinkReport.jitter = downLinkJitter;
                            lastmileProbeResult.downlinkReport.availableBandwidth = downLinkAvailableBandwidth;
                            lastmileProbeResult.rtt = rtt;
                            instance.OnLastmileProbeResult(lastmileProbeResult);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnUserInfoUpdatedHandler))]
        private static void OnUserInfoUpdatedCallback(uint uid, uint userUid, string userAccount)
        {
            if (instance != null && instance.OnUserInfoUpdated != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnUserInfoUpdated != null)
                        {
                            UserInfo userInfo = new UserInfo();
                            userInfo.uid = userUid;
                            userInfo.userAccount = userAccount;
                            instance.OnUserInfoUpdated(uid, userInfo);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnLocalUserRegisteredHandler))]
        private static void OnLocalUserRegisteredCallback(uint uid, string userAccount)
        {
            if (instance != null && instance.OnLocalUserRegistered != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnLocalUserRegistered != null)
                        {
                            instance.OnLocalUserRegistered(uid, userAccount);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnLocalAudioStateChangedHandler))]
        private static void OnLocalAudioStateChangedCallback(int state, int error)
        {
            if (instance != null && instance.OnLocalAudioStateChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnLocalAudioStateChanged != null)
                        {
                            instance.OnLocalAudioStateChanged((LOCAL_AUDIO_STREAM_STATE)state, (LOCAL_AUDIO_STREAM_ERROR)error);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnRemoteAudioStateChangedHandler))]
        private static void OnRemoteAudioStateChangedCallback(uint uid, int state, int reason, int elapsed)
        {
            if (instance != null && instance.OnRemoteAudioStateChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnRemoteAudioStateChanged != null)
                        {
                            instance.OnRemoteAudioStateChanged(uid, (REMOTE_AUDIO_STATE)state, (REMOTE_AUDIO_STATE_REASON)reason, elapsed);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnLocalAudioStatsHandler))]
        private static void OnLocalAudioStatsCallback(int numChannels, int sentSampleRate, int sentBitrate, ushort txPacketLossRate)
        {
            if (instance != null && instance.OnLocalAudioStats != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnLocalAudioStats != null)
                        {
                            LocalAudioStats localAudioStats = new LocalAudioStats();
                            localAudioStats.numChannels = numChannels;
                            localAudioStats.sentSampleRate = sentSampleRate;
                            localAudioStats.sentBitrate = sentBitrate;
                            localAudioStats.txPacketLossRate = txPacketLossRate;
                            instance.OnLocalAudioStats(localAudioStats);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnChannelMediaRelayStateChangedHandler))]
        private static void OnChannelMediaRelayStateChangedCallback(int state, int code)
        {
            if (instance != null && instance.OnChannelMediaRelayStateChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnChannelMediaRelayStateChanged != null)
                        {
                            instance.OnChannelMediaRelayStateChanged((CHANNEL_MEDIA_RELAY_STATE)state, (CHANNEL_MEDIA_RELAY_ERROR)code);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnChannelMediaRelayEventHandler))]
        private static void OnChannelMediaRelayEventCallback(int events)
        {
            if (instance != null && instance.OnChannelMediaRelayEvent != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnChannelMediaRelayEvent != null)
                        {
                            instance.OnChannelMediaRelayEvent((CHANNEL_MEDIA_RELAY_EVENT)events);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnFacePositionChanged))]
        private static void OnFacePositionChangedCallback(int imageWidth, int imageHeight, int x, int y, int width, int height, int vecDistance, int numFaces)
        {
            if (instance != null && instance.OnFacePositionChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnFacePositionChanged != null)
                        {
                            Rectangle vecRectangle = new Rectangle();
                            vecRectangle.x = x;
                            vecRectangle.y = y;
                            vecRectangle.width = width;
                            vecRectangle.height = height;
                            instance.OnFacePositionChanged(imageWidth, imageHeight, vecRectangle, vecDistance, numFaces);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnRtmpStreamingEventHandler))]
        private static void OnRtmpStreamingEventCallback(string url, RTMP_STREAMING_EVENT eventCode)
        {
            if (instance != null && instance.OnRtmpStreamingEvent != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnRtmpStreamingEvent != null)
                        {
                            instance.OnRtmpStreamingEvent(url, eventCode);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnAudioPublishStateChangedHandler))]
        private static void OnAudioPublishStateChangeCallback(string channel, STREAM_PUBLISH_STATE oldState, STREAM_PUBLISH_STATE newState, int elapseSinceLastState)
        {
            if (instance != null && instance.OnAudioPublishStateChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnAudioPublishStateChanged != null)
                        {
                            instance.OnAudioPublishStateChanged(channel, oldState, newState, elapseSinceLastState);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnVideoPublishStateChangedHandler))]
        private static void OnVideoPublishStateChangeCallback(string channel, STREAM_PUBLISH_STATE oldState, STREAM_PUBLISH_STATE newState, int elapseSinceLastState)
        {
            if (instance != null && instance.OnVideoPublishStateChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnVideoPublishStateChanged != null)
                        {
                            instance.OnVideoPublishStateChanged(channel, oldState, newState, elapseSinceLastState);
                        }
                    });
                }
            }
        }

       [MonoPInvokeCallback(typeof(OnAudioSubscribeStateChangedHandler))]
        private static void OnAudioSubscribeStateChangeCallback(string channel, uint uid, STREAM_SUBSCRIBE_STATE oldState, STREAM_SUBSCRIBE_STATE newState, int elapseSinceLastState)
        {
            if (instance != null && instance.OnAudioSubscribeStateChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnAudioSubscribeStateChanged != null)
                        {
                            instance.OnAudioSubscribeStateChanged(channel, uid, oldState, newState, elapseSinceLastState);
                        }
                    });
                }
            }
        }

       [MonoPInvokeCallback(typeof(OnVideoSubscribeStateChangedHandler))]
        private static void OnVideoSubscribeStateChangeCallback(string channel, uint uid, STREAM_SUBSCRIBE_STATE oldState, STREAM_SUBSCRIBE_STATE newState, int elapseSinceLastState)
        {
            if (instance != null && instance.OnVideoSubscribeStateChanged != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnVideoSubscribeStateChanged != null)
                        {
                            instance.OnVideoSubscribeStateChanged(channel, uid, oldState, newState, elapseSinceLastState);
                        }
                    });
                }
            }
        }

       [MonoPInvokeCallback(typeof(OnFirstLocalAudioFramePublishedHandler))]
        private static void OnFirstLocalAudioFramePublishedCallback(int elapsed)
        {
            if (instance != null && instance.OnFirstLocalAudioFramePublished != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnFirstLocalAudioFramePublished != null)
                        {
                            instance.OnFirstLocalAudioFramePublished(elapsed);
                        }
                    });
                }
            }
        }

       [MonoPInvokeCallback(typeof(OnFirstLocalVideoFramePublishedHandler))]
        private static void OnFirstLocalVideoFramePublishedCallback(int elapsed)
        {
            if (instance != null && instance.OnFirstLocalVideoFramePublished != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnFirstLocalVideoFramePublished != null)
                        {
                            instance.OnFirstLocalVideoFramePublished(elapsed);
                        }
                    });
                }
            }
        }

       [MonoPInvokeCallback(typeof(OnUserSuperResolutionEnabledHandler))]
        private static void OnUserSuperResolutionEnabledCallback(uint uid, bool enabled, SUPER_RESOLUTION_STATE_REASON reason)
        {
            if (instance != null && instance.OnUserSuperResolutionEnabled != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnUserSuperResolutionEnabled != null)
                        {
                            instance.OnUserSuperResolutionEnabled(uid, enabled, reason);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnUploadLogResultHandler))]
        private static void OnUploadLogResultCallback(string requestId, bool success, UPLOAD_ERROR_REASON reason)
        {
            if (instance != null && instance.OnUploadLogResult != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnUploadLogResult != null)
                        {
                            instance.OnUploadLogResult(requestId, success, reason);
                        }
                    });
                }
            }
        }

        [MonoPInvokeCallback(typeof(OnVirtualBackgroundSourceEnabledHandler))]
        private static void OnVirtualBackgroundSourceEnabledCallback(bool enabled, VIRTUAL_BACKGROUND_SOURCE_STATE_REASON reason)
        {
            if (instance != null && instance.OnVirtualBackgroundSourceEnabled != null && instance._AgoraCallbackObject != null)
            {
                AgoraCallbackQueue queue = instance._AgoraCallbackObject._CallbackQueue;
                if (!ReferenceEquals(queue, null))
                {
                    queue.EnQueue(()=> {
                        if (instance != null && instance.OnVirtualBackgroundSourceEnabled != null)
                        {
                            instance.OnVirtualBackgroundSourceEnabled(enabled, reason);
                        }
                    });
                }
            }
        }

        private void InitEngineCallback()
        {   
            IRtcEngineNative.initEventOnEngineCallback(OnJoinChannelSuccessCallback, 
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
                                      OnVirtualBackgroundSourceEnabledCallback);
        }
    }
};

