using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;

namespace agora_gaming_rtc 
{
    /** The AgoraChannel class. */
    public sealed class AgoraChannel : IRtcEngineNative
    {
        private static Dictionary<string, AgoraChannel> _channelDictionary = new Dictionary<string, AgoraChannel>();
        private static Dictionary<string, AgoraCallbackObject> _AgoraCallbackObjectDictionary = new Dictionary<string, AgoraCallbackObject>();
        private IntPtr _channelHandler = IntPtr.Zero;
        private string _channelId = null;
        private string agoraChannelCallbackName = "agoraChannelGameObject";
        private IRtcEngine _rtcEngine = null;
        public ChannelOnWarningHandler ChannelOnWarning;
        public ChannelOnErrorHandler ChannelOnError;
        public ChannelOnJoinChannelSuccessHandler ChannelOnJoinChannelSuccess;
        public ChannelOnReJoinChannelSuccessHandler ChannelOnReJoinChannelSuccess;
        public ChannelOnLeaveChannelHandler ChannelOnLeaveChannel;
        public ChannelOnClientRoleChangedHandler ChannelOnClientRoleChanged;
        public ChannelOnUserJoinedHandler ChannelOnUserJoined;
        public ChannelOnUserOffLineHandler ChannelOnUserOffLine;
        public ChannelOnConnectionLostHandler ChannelOnConnectionLost;
        public ChannelOnRequestTokenHandler ChannelOnRequestToken;
        public ChannelOnTokenPrivilegeWillExpireHandler ChannelOnTokenPrivilegeWillExpire;
        public ChannelOnRtcStatsHandler ChannelOnRtcStats;
        public ChannelOnNetworkQualityHandler ChannelOnNetworkQuality;
        public ChannelOnRemoteVideoStatsHandler ChannelOnRemoteVideoStats;
        public ChannelOnRemoteAudioStatsHandler ChannelOnRemoteAudioStats;
        public ChannelOnRemoteAudioStateChangedHandler ChannelOnRemoteAudioStateChanged;
        public ChannelOnActiveSpeakerHandler ChannelOnActiveSpeaker;
        public ChannelOnVideoSizeChangedHandler ChannelOnVideoSizeChanged;
        public ChannelOnRemoteVideoStateChangedHandler ChannelOnRemoteVideoStateChanged;		
        public ChannelOnStreamMessageHandler ChannelOnStreamMessage;		
        public ChannelOnStreamMessageErrorHandler ChannelOnStreamMessageError;		
        public ChannelOnMediaRelayStateChangedHandler ChannelOnMediaRelayStateChanged;   
        public ChannelOnMediaRelayEventHandler ChannelOnMediaRelayEvent;		
        public ChannelOnRtmpStreamingStateChangedHandler ChannelOnRtmpStreamingStateChanged;    
        public ChannelOnTranscodingUpdatedHandler ChannelOnTranscodingUpdated;		
        public ChannelOnStreamInjectedStatusHandler ChannelOnStreamInjectedStatus;		
        public ChannelOnRemoteSubscribeFallbackToAudioOnlyHandler ChannelOnRemoteSubscribeFallbackToAudioOnly;		
        public ChannelOnConnectionStateChangedHandler ChannelOnConnectionStateChanged;
        public ChannelOnLocalPublishFallbackToAudioOnlyHandler ChannelOnLocalPublishFallbackToAudioOnly;
        public ChannelOnRtmpStreamingEventHandler ChannelOnRtmpStreamingEvent;
        public ChannelOnAudioPublishStateChangedHandler ChannelOnAudioPublishStateChanged;
        public ChannelOnVideoPublishStateChangedHandler ChannelOnVideoPublishStateChanged;
        public ChannelOnAudioSubscribeStateChangedHandler ChannelOnAudioSubscribeStateChanged;
        public ChannelOnVideoSubscribeStateChangedHandler ChannelOnVideoSubscribeStateChanged;
        public ChannelOnUserSuperResolutionEnabledHandler ChannelOnUserSuperResolutionEnabled;

        /** Creates and gets an `AgoraChannel` object.
         *
         * To join more than one channel, call this method multiple times to create as many `AgoraChannel` objects as needed, and
         * call the {@link agora_gaming_rtc.AgoraChannel.JoinChannel JoinChannel} method of each created `AgoraChannel` object.
         *
         * After joining multiple channels, you can simultaneously subscribe to streams of all the channels, but publish a stream in only one channel at one time.
         * 
         * @param rtcEngine IRtcEngine.
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
         * - An empty pointer `NULL`, if the method call fails.
         * - `ERR_REFUSED(5)`, if you set `channelId` as the empty string "".
         */
        public static AgoraChannel CreateChannel(IRtcEngine rtcEngine, string channelId)
        {
            if (_channelDictionary.ContainsKey(channelId))
            {
                return _channelDictionary[channelId];
            }
            return new AgoraChannel(rtcEngine, channelId);
        }

        /** The AgoraChannel class.  */
        public AgoraChannel(IRtcEngine rtcEngine, string channelId)
        {
            _channelId = channelId;
            InitGameObject(agoraChannelCallbackName + channelId, channelId);
            _rtcEngine = rtcEngine;
            CreateChannelNative(channelId);
            _channelDictionary.Add(_channelId, this);
            initChannelEvent();

#if !UNITY_EDITOR && UNITY_WEBGL
            AgoraWebGLEventHandler.AddClient(channelId, this);
#endif
        }

        private void InitGameObject(string objectName, string channelId)
        {
            DeInitGameObject(objectName, channelId);
            AgoraCallbackObject agoraCallback = new AgoraCallbackObject(objectName);
            _AgoraCallbackObjectDictionary.Add(channelId, agoraCallback);
        }

        private void DeInitGameObject(string objectName, string channelId)
        {
            if (_AgoraCallbackObjectDictionary.ContainsKey(channelId))
            {
                AgoraCallbackObject agoraCallbackObject = _AgoraCallbackObjectDictionary[channelId];
                if (!ReferenceEquals(agoraCallbackObject, null))
                {
                    agoraCallbackObject.Release();
                    _AgoraCallbackObjectDictionary.Remove(channelId); 
                    agoraCallbackObject = null;
                }
            }
        }

        private int CreateChannelNative(string channelId)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            _channelHandler = IRtcEngineNative.createChannel(channelId);
            return (int)ERROR_CODE.ERROR_OK;
        }
        /** Releases all AgoraChannel resources.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         *    - `ERR_NOT_INITIALIZED(7)`: The SDK is not initialized before calling this method.
         */
        
        public int ReleaseChannel()
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            int ret = IRtcEngineNative.ReleaseChannel(_channelHandler);
            if (_channelDictionary.ContainsKey(_channelId))
            {
                _channelDictionary.Remove(_channelId);
            }
            _channelHandler = IntPtr.Zero;
            DeInitGameObject(agoraChannelCallbackName + _channelId, _channelId);
            ReleaseCallback();
            return ret;
        }

        private void ReleaseCallback()
        {
            _rtcEngine = null;
            _channelId = null;
            ChannelOnWarning = null;
            ChannelOnError = null;
            ChannelOnJoinChannelSuccess = null;
            ChannelOnReJoinChannelSuccess = null;
            ChannelOnLeaveChannel = null;
            ChannelOnClientRoleChanged = null;
            ChannelOnUserJoined = null;
            ChannelOnUserOffLine = null;
            ChannelOnConnectionLost = null;
            ChannelOnRequestToken = null;
            ChannelOnTokenPrivilegeWillExpire = null;
            ChannelOnRtcStats = null;
            ChannelOnNetworkQuality = null;
            ChannelOnRemoteVideoStats = null;
            ChannelOnRemoteAudioStats = null;
            ChannelOnRemoteAudioStateChanged = null;
            ChannelOnActiveSpeaker = null;
            ChannelOnVideoSizeChanged = null;
            ChannelOnRemoteVideoStateChanged = null;
            ChannelOnStreamMessage = null;
            ChannelOnStreamMessageError = null;		
            ChannelOnMediaRelayStateChanged = null;
            ChannelOnMediaRelayEvent = null;
            ChannelOnRtmpStreamingStateChanged = null;    
            ChannelOnTranscodingUpdated = null;
            ChannelOnStreamInjectedStatus = null;
            ChannelOnRemoteSubscribeFallbackToAudioOnly = null;		
            ChannelOnConnectionStateChanged = null;
            ChannelOnLocalPublishFallbackToAudioOnly = null;
            ChannelOnRtmpStreamingEvent = null;
            ChannelOnAudioPublishStateChanged = null;
            ChannelOnVideoPublishStateChanged = null;
            ChannelOnAudioSubscribeStateChanged = null;
            ChannelOnVideoSubscribeStateChanged = null;
            ChannelOnUserSuperResolutionEnabled = null;
        }

        public static int Release()
        {
            _channelDictionary.Clear();
            return 0;
        }

         /** Joins the channel with a user ID.
         *
         * This method differs from the `JoinChannel` method in the `IRtcEngine` class in the following aspects:
         *
         * | AgoraChannel::JoinChannel                                                                                                                    | IRtcEngine::JoinChannel                                                                                      |
         * |------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------|
         * | Does not contain the `channelName` parameter, because the channel name is specified when creating the `AgoraChannel` object.                              | Contains the `channelName` parameter, which specifies the channel to join.                                       |
         * | Contains the `channelMediaOptions` parameter, which decide whether to subscribe to audio or video streams before joining the channel.                            | Does not contain the `channelMediaOptions` parameter. By default, users subscribe to all streams when joining the channel. |
         * | Users can join multiple channels simultaneously by creating multiple `AgoraChannel` objects and calling the `JoinChannel` method of each object. | Users can join only one channel.                                                                             |
         * | By default, the SDK does not publish any stream after the user joins the channel. You need to call the {@link agora_gaming_rtc.AgoraChannel.Publish Publish} method to do that.        | By default, the SDK publishes streams once the user joins the channel.                                       |
         *
         * Once the user joins the channel, the user subscribes to the audio and video streams of all the other users in the channel by default, giving rise to usage and billing calculation. If you do not want to subscribe to a specified stream or all remote streams, call the `mute` methods accordingly.
         *
         * @note
         * - If you are already in a channel, you cannot rejoin it with the same `uid`.
         * - We recommend using different UIDs for different channels.
         * - If you want to join the same channel from different devices, ensure that the UIDs in all devices are different.
         * - Ensure that the app ID you use to generate the token is the same with the app ID used when creating the `IRtcEngine` object.
         *
         * @param token The token for authentication:
         * - In situations not requiring high security: You can use the temporary token generated at Console. For details, see [Get a temporary token](https://docs.agora.io/en/Agora%20Platform/token?platfor%20*%20m=All%20Platforms#get-a-temporary-token).
         * - In situations requiring high security: Set it as the token generated at your server. For details, see [Generate a token](https://docs.agora.io/en/Interactive%20Broadcast/token_server).
         * @param info (Optional) Additional information about the channel. This parameter can be set as null. Other users in the channel do not receive this information.
         * @param uid The user ID. A 32-bit unsigned integer with a value ranging from 1 to (2<sup>32</sup>-1). This parameter must be unique. If `uid` is not assigned (or set as `0`), the SDK assigns a `uid` and reports it in the {@link agora_gaming_rtc.OnJoinChannelSuccessHandler OnJoinChannelSuccessHandler} callback. The app must maintain this user ID.
         * @param channelMediaOptions The channel media options: ChannelMediaOptions.
         *
         * @return
         * - 0(ERR_OK): Success.
         * - < 0: Failure.
         *    - -2(ERR_INALID_ARGUMENT): The parameter is invalid.
         *    - -3(ERR_NOT_READY): The SDK fails to be initialized. You can try re-initializing the SDK.
         *    - -5(ERR_REFUSED): The request is rejected. This may be caused by the following:
         *       - You have created an `AgoraChannel` object with the same channel name.
         *       - You have joined and published a stream in a channel created by the `AgoraChannel` object.
         */
        public int JoinChannel(string token, string info, uint uid, ChannelMediaOptions channelMediaOptions)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
            return IRtcEngineNative.joinChannel2(_channelId, token, info, uid, channelMediaOptions.autoSubscribeAudio, channelMediaOptions.autoSubscribeVideo);
#else
            return IRtcEngineNative.joinChannel2(_channelHandler, token, info, uid, channelMediaOptions.autoSubscribeAudio, channelMediaOptions.autoSubscribeVideo, channelMediaOptions.publishLocalAudio, channelMediaOptions.publishLocalVideo);
#endif
        }

         /** Joins the channel with a user account.
         *
         * This method differs from the `JoinChannelWithUserAccount` method in the `IRtcEngine` class in the following aspects:
         *
         * | AgoraChannel::JoinChannelWithUserAccount                                                                                                                    | IRtcEngine::JoinChannelWithUserAccount                                                                                      |
         * |------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------|
         * | Does not contain the `channelId` parameter, because the channel name is specified when creating the `AgoraChannel` object.                              | Contains the `channelId` parameter, which specifies the channel to join.                                       |
         * | Contains the `autoSubscribeAudio` and `autoSubscribeVideo` parameters, which decide whether to subscribe to audio or video streams before joining the channel.                            | Does not contain the `autoSubscribeAudio` or `autoSubscribeVideo` parameter. By default, users subscribe to all streams when joining the channel. |
         * | Users can join multiple channels simultaneously by creating multiple `AgoraChannel` objects and calling the `JoinChannelWithUserAccount` method of each object. | Users can join only one channel.                                                                             |
         * | By default, the SDK does not publish any stream after the user joins the channel. You need to call the {@link agora_gaming_rtc.AgoraChannel.Publish Publish} method to do that.        | By default, the SDK publishes streams once the user joins the channel.                                       |
         *
         * Once the user joins the channel, the user subscribes to the audio and video streams of all the other users in the channel by default, giving rise to usage and billing calculation. If you do not want to subscribe to a specified stream or all remote streams, call the `mute` methods accordingly.
         *
         * @note
         * - If you are already in a channel, you cannot rejoin it with the same `uid`.
         * - We recommend using different userAccount for different channels.
         * - If you want to join the same channel from different devices, ensure that the userAccount in all devices are different.
         * - Ensure that the app ID you use to generate the token is the same with the app ID used when creating the `IRtcEngine` object.
         *
         * @param token The token for authentication:
         * - In situations not requiring high security: You can use the temporary token generated at Console. For details, see [Get a temporary token](https://docs.agora.io/en/Agora%20Platform/token?platfor%20*%20m=All%20Platforms#get-a-temporary-token).
         * - In situations requiring high security: Set it as the token generated at your server. For details, see [Generate a token](https://docs.agora.io/en/Interactive%20Broadcast/token_server).
         * @param userAccount The user account. The maximum length of this parameter is 255 bytes. Ensure that you set this parameter and do not set it as null. Supported character scopes are:
         * - All lowercase English letters: a to z.
         * - All uppercase English letters: A to Z.
         * - All numeric characters: 0 to 9.
         * - The space character.
         * - Punctuation characters and other symbols, including: "!", "#", "$", "%", "&", "(", ")", "+", "-", ":", ";", "<", "=", ".", ">", "?", "@", "[", "]", "^", "_", " {", "}", "|", "~", ",".
         * @param channelMediaOptions The channel media options: ChannelMediaOptions.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         *    - `ERR_INVALID_ARGUMENT(-2)`
         *    - `ERR_NOT_READY(-3)`
         *    - `ERR_REFUSED(-5)`
         */
        public int JoinChannelWithUserAccount(string token, string userAccount, ChannelMediaOptions channelMediaOptions)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
            return IRtcEngineNative.joinChannelWithUserAccount2(_channelId, token, userAccount, channelMediaOptions.autoSubscribeAudio, channelMediaOptions.autoSubscribeVideo);
#else
            return IRtcEngineNative.joinChannelWithUserAccount2(_channelHandler, token, userAccount, channelMediaOptions.autoSubscribeAudio, channelMediaOptions.autoSubscribeVideo, channelMediaOptions.publishLocalAudio, channelMediaOptions.publishLocalVideo);
#endif
        }

        /** Allows a user to leave a channel, such as hanging up or exiting a call.
         *
         * After joining a channel, the user must call the `LeaveChannel` method to end the call before joining another channel.
         *
         * This method returns `0` if the user leaves the channel and releases all resources related to the call.
         *
         * This method call is asynchronous, and the user has not left the channel when the method call returns. Once the user leaves the channel, the SDK triggers the {@link agora_gaming_rtc.ChannelOnLeaveChannelHandler ChannelOnLeaveChannelHandler} callback.
         *
         * A successful `LeaveChannel` method call triggers the following callbacks:
         * - The local client: `ChannelOnLeaveChannelHandler`
         * - The remote client: {@link agora_gaming_rtc.ChannelOnUserOffLineHandler ChannelOnUserOffLineHandler}, if the user leaving the channel is in the Communication channel, or is a host in the Live Broadcast profile.
         *
         * @note
         * - If you call the {@link agora_gaming_rtc.AgoraChannel.ReleaseChannel ReleaseChannel} method immediately after the `LeaveChannel` method, the `LeaveChannel` process interrupts, and the `ChannelOnLeaveChannelHandler` callback is not triggered.
         * - If you call the `LeaveChannel` method during a CDN live streaming, the SDK triggers the {@link agora_gaming_rtc.IRtcEngine.RemovePublishStreamUrl RemovePublishStreamUrl} method.
         *
         * @return
         * - 0(ERR_OK): Success.
         * - < 0: Failure.
         *    - -1(ERR_FAILED): A general error occurs (no specified reason).
         *    - -2(ERR_INALID_ARGUMENT): The parameter is invalid.
         *    - -7(ERR_NOT_INITIALIZED): The SDK is not initialized.
         */
        public int LeaveChannel()
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
            return IRtcEngineNative.leaveChannel2(_channelId);
#else 
            return IRtcEngineNative.leaveChannel2(_channelHandler);
#endif

        }

        /** Publishes the local stream to the channel.
         *
         * You must keep the following restrictions in mind when calling this method. Otherwise, the SDK returns the `ERR_REFUSED(-5)`:
         * - This method publishes one stream only to the channel corresponding to the current `AgoraChannel` object.
         * - In a Live Broadcast channel, only a host can call this method. To switch the client role, call {@link agora_gaming_rtc.AgoraChannel.SetClientRole SetClientRole} of the current `AgoraChannel` object.
         * - You can publish a stream to only one channel at a time.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         *    - `ERR_REFUSED(-5)`: The method call is refused.
         */
        public int Publish()
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
            return IRtcEngineNative.publish(_channelId);
#else
            return IRtcEngineNative.publish(_channelHandler);
#endif
        }

        /** Stops publishing a stream to the channel.
         *
         * If you call this method in a channel where you are not publishing streams, the SDK returns `ERR_REFUSED(5)`.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         *    - `ERR_REFUSED(5)`: The method call is refused.
         */
        public int Unpublish()
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
            return IRtcEngineNative.unpublish(_channelId);
#else
            return IRtcEngineNative.unpublish(_channelHandler);
#endif
        }

        /** Gets the channel ID of the current `AgoraChannel` object.
        *
        * @return
        * - The channel ID of the current `AgoraChannel` object, if the method call succeeds.
        * - The empty string "", if the method call fails.
        */
#if UNITY_WEBGL
            [Obsolete("This API is not supported for WebGL")]
#endif
        public string ChannelId()
        {
            if(Application.platform == RuntimePlatform.WebGLPlayer) {
                return ""; 
	        }

            if (_rtcEngine == null)
                return ERROR_CODE.ERROR_NOT_INIT_ENGINE + "";

            return Marshal.PtrToStringAnsi(IRtcEngineNative.channelId(_channelHandler));
        }

        /** Retrieves the current call ID.
         * 
         *  When a user joins a channel on a client, a call ID is generated to identify the call from the client.
         *  Feedback methods, such as {@link agora_gaming_rtc.IRtcEngine.Rate Rate} and {@link agora_gaming_rtc.IRtcEngine.Complain Complain}, must be called after the call ends to submit feedback to the SDK.
         * 
         *  The `Rate` and `Complain` methods require the call ID retrieved from the `GetCallId` method during a call. The call ID is passed as an argument into the `Rate` and `Complain` methods after the call ends.
         * 
         *  @return
         *  - &ge; 0: The current call ID, if this method call succeeds.
         *  - < 0: Failure.
         */
#if UNITY_WEBGL
            [Obsolete("This API is not supported for WebGL")]
#endif
        public string GetCallId()
        {
            if (_rtcEngine == null)
                return ERROR_CODE.ERROR_NOT_INIT_ENGINE + "";

            string callIdString = "";
            IntPtr callId = IRtcEngineNative.getCallId2(_channelHandler);
            if (callId != IntPtr.Zero)
            {
                callIdString = Marshal.PtrToStringAnsi(callId);
                IRtcEngineNative.freeObject(callId);
            }
            return callIdString;
        }

        /** Gets a new token when the current token expires after a period of time.
         *
         * The `token` expires after a period of time once the token schema is enabled when:
         *
         * - The SDK triggers the {@link agora_gaming_rtc.ChannelOnTokenPrivilegeWillExpireHandler ChannelOnTokenPrivilegeWillExpireHandler} callback, or
         * - The {@link agora_gaming_rtc.ChannelOnConnectionStateChangedHandler ChannelOnConnectionStateChangedHandler} reports `CONNECTION_CHANGED_TOKEN_EXPIRED(9)`.
         *
         * The application should call this method to get the new `token`. Failure to do so will result in the SDK disconnecting from the server.
         *
         * @param token Pointer to the new token.
         * @return
         * - 0(ERR_OK): Success.
         * - < 0: Failure.
         *    - -1(ERR_FAILED): A general error occurs (no specified reason).
         *    - -2(ERR_INALID_ARGUMENT): The parameter is invalid.
         *    - -7(ERR_NOT_INITIALIZED): The SDK is not initialized.
         */
        public int RenewToken(string token)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
            return IRtcEngineNative.renewToken2(_channelId, token);
#else
            return IRtcEngineNative.renewToken2(_channelHandler, token);
#endif
        }

        /** Enables built-in encryption with an encryption password before users join a channel.
         * 
         * @deprecated Deprecated as of v3.2.0. Use the {@link agora_gaming_rtc.AgoraChannel.EnableEncryption EnableEncryption} instead.
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
        [Obsolete("deprecated Deprecated as of v3.2.0. Use EnableEncryption instead")]
        public int SetEncryptionSecret(string secret)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
#if !UNITY_EDITOR && UNITY_WEBGL
            // do nothing for WebGL since deprecation
            return -1;
#else 
            return IRtcEngineNative.setEncryptionSecret2(_channelHandler, secret);
#endif
        }

        /** Sets the built-in encryption mode.
         * 
         * @deprecated Deprecated as of v3.2.0. Use the {@link agora_gaming_rtc.AgoraChannel.EnableEncryption EnableEncryption} instead.
         *
         * The Agora Unity SDK supports built-in encryption, which is set to the `aes-128-xts` mode by default. Call this method to use other encryption modes.
         * 
         * All users in the same channel must use the same encryption mode and password.
         * 
         * Refer to the information related to the AES encryption algorithm on the differences between the encryption modes.
         * 
         * @note Call the {@link agora_gaming_rtc.AgoraChannel.SetEncryptionSecret SetEncryptionSecret} method to enable the built-in encryption function before calling this method.
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
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
            return IRtcEngineNative.setEncryptionMode2(_channelId, encryptionMode);
#else
            return IRtcEngineNative.setEncryptionMode2(_channelHandler, encryptionMode);
#endif
        }

        /** Sets the role of the user, such as a host or an audience (default), before joining a channel in an interactive live streaming.
         * 
         * This method can be used to switch the user role in the interactive live streaming after the user joins a channel.
         * 
         * In the Live Broadcast profile, when a user switches user roles after joining a channel, a successful `SetClientRole` method call triggers the following callbacks:
         * - The local client: {@link agora_gaming_rtc.ChannelOnClientRoleChangedHandler ChannelOnClientRoleChangedHandler}
         * - The remote client: {@link agora_gaming_rtc.ChannelOnUserJoinedHandler ChannelOnUserJoinedHandler} or {@link agora_gaming_rtc.ChannelOnUserOffLineHandler ChannelOnUserOffLineHandler} (BECOME_AUDIENCE)
         * 
         * @note This method applies only to the Live-broadcast profile.
         * 
         * @param role Sets the role of the user. See {@link agora_gaming_rtc.CLIENT_ROLE_TYPE CLIENT_ROLE_TYPE}.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetClientRole(CLIENT_ROLE_TYPE role)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
#if !UNITY_EDITOR && UNITY_WEBGL
            return IRtcEngineNative.setClientRole2(_channelId, (int)role);
#else
            return IRtcEngineNative.setClientRole2(_channelHandler, (int)role);
#endif
        }        

        /** Prioritizes a remote user's stream.
         * 
         * Use this method with the {@link agora_gaming_rtc.IRtcEngine.SetRemoteSubscribeFallbackOption SetRemoteSubscribeFallbackOption} method. If the fallback function is enabled for a subscribed stream, the SDK ensures the high-priority user gets the best possible stream quality.
         * 
         * @note
         * 
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
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
            IRtcEngineNative.setRemoteUserPriority2_WGLM(_channelHandler, ""+uid, (int)userPriority);
            return 0;
#else
            return IRtcEngineNative.setRemoteUserPriority2(_channelHandler, uid, (int)userPriority);
#endif

        }

        /** Sets the sound position and gain of a remote user.
         * 
         * When the local user calls this method to set the sound position of a remote user, the sound difference between the left and right channels allows the local user to track the real-time position of the remote user, creating a real sense of space. This method applies to massively multiplayer online games, such as Battle Royale games.
         * 
         * @note
         * - For this method to work, enable stereo panning for remote users by calling the {@link agora_gaming_rtc.IRtcEngine.EnableSoundPositionIndication EnableSoundPositionIndication} method before joining a channel.
         * - This method requires hardware support. For the best sound positioning, we recommend using a stereo speaker.
         * 
         * @param uid The ID of the remote user.
         * @param pan The sound position of the remote user. The value ranges from -1.0 to 1.0:
         * - 0.0: the remote sound comes from the front.
         * - -1.0: the remote sound comes from the left.
         * - 1.0: the remote sound comes from the right.
         * @param gain Gain of the remote user. The value ranges from 0.0 to 100.0. The default value is 100.0 (the original gain of the remote user). The smaller the value, the less the gain.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetRemoteVoicePosition(uint uid, double pan, double gain)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.setRemoteVoicePosition2(_channelHandler, uid, pan, gain);
        }

        public int SetRemoteRenderMode(uint userId, int renderMode, int mirrorMode)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.setRemoteRenderMode2(_channelHandler, userId, renderMode, mirrorMode);
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
         * {@link agora_gaming_rtc.AgoraChannel.MuteRemoteAudioStream MuteRemoteAudioStream(false)}, and specify the user ID.
         * - If you need to resume subscribing to the audio stream of multiple remote users, call 
         * {@link agora_gaming_rtc.AgoraChannel.MuteRemoteAudioStream MuteRemoteAudioStream(false)} multiple times.
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
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.setDefaultMuteAllRemoteAudioStreams2(_channelHandler, mute);
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
         * {@link agora_gaming_rtc.AgoraChannel.MuteRemoteVideoStream MuteRemoteVideoStream(false)}, and specify the user ID.
         * - If you need to resume subscribing to the video stream of multiple remote users, call 
         * {@link agora_gaming_rtc.AgoraChannel.MuteRemoteVideoStream MuteRemoteVideoStream(false)} multiple times.
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
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.setDefaultMuteAllRemoteVideoStreams2(_channelHandler, mute);
        }

        /** Stops or resumes subscribing to the audio streams of all remote users.
         * 
         * As of v3.3.1, after successfully calling this method, the local user stops or resumes subscribing to the 
         * audio streams of all remote users, including all subsequent users.
         *
         * @note
         * - Call this method after joining a channel.
         * - See recommended settings in *Set the Subscribing State*.
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
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
#endif
            return IRtcEngineNative.muteAllRemoteAudioStreams2(_channelHandler, mute);
        }

        /** Adjusts the playback signal volume of a specified remote user.
         * 
         * You can call this method as many times as necessary to adjust the playback signal volume of different remote users, or to repeatedly adjust the playback signal volume of the same remote user.
         * 
         * @note
         * - Call this method after joining a channel.
         * - The playback signal volume here refers to the mixed volume of a specified remote user.
         * - This method can only adjust the playback signal volume of one specified remote user at a time. To adjust the playback signal volume of different remote users, call the method as many times, once for each remote user.
         * 
         * @param userId The ID of the remote user.
         * @param volume The playback signal volume of the specified remote user. The value ranges from 0 to 100:
         * - 0: Mute.
         * - 100: Original volume.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int AdjustUserPlaybackSignalVolume(uint userId, int volume)
        {
           if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
#if !UNITY_EDITOR && UNITY_WEBGL
             IRtcEngineNative.setCurrentChannel_WGL(_channelId);
             IRtcEngineNative.adjustUserPlaybackSignalVolume2_WGLM(_channelHandler, ""+userId, volume);
             return 0;
#else
            return IRtcEngineNative.adjustUserPlaybackSignalVolume2(_channelHandler, userId, volume);
#endif
        }

        /** Stops or resumes subscribing to the audio stream of a specified user.
         * 
         * @note
         * - Call this method after joining a channel.
         * - See recommended settings in *Set the Subscribing State*.
         * 
         * @param userId The user ID of the specified remote user.
         * @param mute Sets whether to stop subscribing to the audio stream of a specified user.
         * - true: Stop subscribing to the audio stream of a specified user.
         * - false: (Default) Resume subscribing to the audio stream of a specified user.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int MuteRemoteAudioStream(uint userId, bool mute)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
             IRtcEngineNative.muteRemoteAudioStream2_WGLM(_channelHandler, ""+userId, mute);
             return 0;
#else
            return IRtcEngineNative.muteRemoteAudioStream2(_channelHandler, userId, mute);
#endif


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
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
#endif
            return IRtcEngineNative.muteAllRemoteVideoStreams2(_channelHandler, mute);                    
        }

        /** Stops or resumes subscribing to the video stream of a specified user.
         * 
         * @note
         * - Call this method after joining a channel.
         * - See recommended settings in *Set the Subscribing State*.
         * 
         * @param userId The user ID of the specified remote user.
         * @param mute Sets whether to stop subscribing to the video stream of a specified user.
         * - true: Stop subscribing to the video stream of a specified user.
         * - false: (Default) Resume subscribing to the video stream of a specified user.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int MuteRemoteVideoStream(uint userId, bool mute)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
           IRtcEngineNative.muteRemoteVideoStream2_WGLM(_channelHandler, ""+userId, mute);
             return 0;
#else
            return IRtcEngineNative.muteRemoteVideoStream2(_channelHandler, userId, mute);
#endif
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
         * {@link agora_gaming_rtc.AgoraChannel.SetRemoteDefaultVideoStreamType SetRemoteDefaultVideoStreamType}, the SDK applies the settings in
         * the `SetRemoteVideoStreamType`.
         *
         * @param userId ID of the remote user sending the video stream.
         * @param streamType  Sets the video-stream type. See #REMOTE_VIDEO_STREAM_TYPE.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetRemoteVideoStreamType(uint userId, REMOTE_VIDEO_STREAM_TYPE streamType)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
             IRtcEngineNative.setRemoteVideoStreamType2_WGLM(_channelHandler, ""+userId, (int)streamType);
             return 0;
#else
            return IRtcEngineNative.setRemoteVideoStreamType2(_channelHandler, userId, (int)streamType);
#endif
        }

        /** Sets the default video-stream type for the video received by the local user when the remote user sends dual streams.
         * 
         * - If the dual-stream mode is enabled by calling the {@link agora_gaming_rtc.IRtcEngine.EnableDualStreamMode EnableDualStreamMode} method, the user receives the high-stream video by default. The `SetRemoteDefaultVideoStreamType` method allows the application to adjust the corresponding video-stream type according to the size of the video window, reducing the bandwidth and resources.
         * - If the dual-stream mode is not enabled, the user receives the high-stream video by default.
         * 
         * The result after calling this method is returned in the {@link agora_gaming_rtc.OnApiExecutedHandler OnApiExecutedHandler} callback. The Agora RTC SDK receives the high-stream video by default to reduce the bandwidth. If needed, users can switch to the low-stream video through this method.
         * 
         * @note You can call this method either before or after joining a channel. If you call both `SetRemoteDefaultVideoStreamType` and
         * {@link agora_gaming_rtc.AgoraChannel.SetRemoteVideoStreamType SetRemoteVideoStreamType}, the SDK applies the settings in
         * the `SetRemoteVideoStreamType`.
         *
         * @param streamType Sets the default video stream type. See #REMOTE_VIDEO_STREAM_TYPE.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetRemoteDefaultVideoStreamType(REMOTE_VIDEO_STREAM_TYPE streamType)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
#endif
            return IRtcEngineNative.setRemoteDefaultVideoStreamType2(_channelHandler, (int)streamType);             
        }

        /** Creates a data stream.
         * 
         * @deprecated This method is deprecated from v3.3.1. Use the 
         * {@link agora_gaming_rtc.AgoraChannel.CreateDataStream(DataStreamConfig config) CreateDataStream}2 
         * method instead.
         * 
         * Each user can create up to five data streams during the lifecycle of the AgoraChannel.
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
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.createDataStream2(_channelHandler, reliable, ordered);        
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
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.createDataStream_channel(_channelHandler, config.syncWithAudio, config.ordered);        
        }

        /** Sends data stream messages to all users in a channel.
         * 
         * The SDK has the following restrictions on this method:
         * - Up to 30 packets can be sent per second in a channel with each packet having a maximum size of 1 kB.
         * - Each client can send up to 6 KB of data per second.
         * - Each user can have up to five data streams simultaneously.
         * 
         * A successful `SendStreamMessage` method call triggers the {@link agora_gaming_rtc.ChannelOnStreamMessageHandler ChannelOnStreamMessageHandler} callback on the remote client, from which the remote user gets the stream message.
         * 
         * A failed `SendStreamMessage` method call triggers the `ChannelOnStreamMessageHandler` callback on the remote client.
         * 
         * @note 
         * - This method applies only to the Communication profile or to the hosts in the Live-broadcast profile. If an audience in the Live-broadcast profile calls this method, the audience may be switched to a host.
         * - Ensure that you have created the data stream using {@link agora_gaming_rtc.AgoraChannel.CreateDataStream CreateDataStream} before calling this method.
         *
         * @param streamId The ID of the sent data stream, returned in the `CreateDataStream` method.
         * @param data The sent data.
         * @param length The data length.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SendStreamMessage(int streamId, string data, Int64 length)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.sendStreamMessage2(_channelHandler, streamId, data, length);            
        }    

        /** Publishes the local stream to a specified CDN streaming URL. (CDN live only.)
         * 
         * The SDK returns the result of this method call in the {@link agora_gaming_rtc.OnStreamPublishedHandler OnStreamPublishedHandler} callback.
         * 
         * The `AddPublishStreamUrl` method call triggers the {@link agora_gaming_rtc.ChannelOnRtmpStreamingStateChangedHandler ChannelOnRtmpStreamingStateChangedHandler} callback on the local client to report the state of adding a local stream to the CDN.
         * 
         * @note
         * - Ensure that the user joins the channel before calling this method.
         * - Ensure that you enable the RTMP Converter service before using this function.
         * - This method adds only one stream CDN streaming URL each time it is called.
         * - This method applies to Live Broadcast only.
         * 
         * @param url The CDN streaming URL in the RTMP or RTMPS format. The maximum length of this parameter is 1024 bytes. The CDN streaming URL must not contain special characters, such as Chinese language characters.
         * @param transcodingEnabled Sets whether transcoding is enabled or disabled:
         * - true: Enable transcoding. To [transcode](https://docs.agora.io/en/Agora%20Platform/terms?platform=All%20Platforms#transcoding) the audio or video streams when publishing them to CDN live, often used for combining the audio and video streams of multiple hosts in CDN live. If you set this parameter as `true`, ensure that you call the {@link agora_gaming_rtc.AgoraChannel.SetLiveTranscoding SetLiveTranscoding} method before this method.
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
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
#endif
            return IRtcEngineNative.addPublishStreamUrl2(_channelHandler, url, transcodingEnabled);            
        }

        /** Removes an RTMP or RTMPS stream from the CDN. (CDN live only.)
         * 
         * This method removes the CDN streaming URL (added by the {@link agora_gaming_rtc.AgoraChannel.AddPublishStreamUrl AddPublishStreamUrl} method) from a CDN live stream. The SDK returns the result of this method call in the {@link agora_gaming_rtc.OnStreamUnpublishedHandler OnStreamUnpublishedHandler} callback.
         * 
         * The `RemovePublishStreamUrl` method call triggers the {@link agora_gaming_rtc.ChannelOnRtmpStreamingStateChangedHandler ChannelOnRtmpStreamingStateChangedHandler} callback on the local client to report the state of removing an RTMP or RTMPS stream from the CDN.
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
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
#endif
            return IRtcEngineNative.removePublishStreamUrl2(_channelHandler, url);
        }

        /** Sets the video layout and audio settings for CDN live. (CDN live only.)
         * 
         * The SDK triggers the {@link agora_gaming_rtc.ChannelOnTranscodingUpdatedHandler ChannelOnTranscodingUpdatedHandler} callback when you call the `SetLiveTranscoding` method to update the transcoding setting.
         * 
         * @note
         * - Ensure that you enable the RTMP Converter service before using this function.
         * - If you call the `SetLiveTranscoding` method to update the transcoding setting for the first time, the SDK does not trigger the `ChannelOnTranscodingUpdatedHandler` callback.
         * - Ensure that you call this method after joining a channel.
         * - Agora supports pushing media streams in RTMPS protocol to the CDN only when you enable transcoding.
         *
         * @param liveTranscoding Sets the CDN live audio or video transcoding settings. See LiveTranscoding.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int SetLiveTranscoding(LiveTranscoding liveTranscoding)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
#endif

            String transcodingUserInfo = "";
            if (liveTranscoding.userCount != 0 && liveTranscoding.transcodingUsers != null) {
                for (int i = 0; i < liveTranscoding.userCount; i ++) {
                    transcodingUserInfo += liveTranscoding.transcodingUsers[i].uid;
                    transcodingUserInfo += "\t";
                    transcodingUserInfo += liveTranscoding.transcodingUsers[i].x;
                    transcodingUserInfo += "\t";
                    transcodingUserInfo += liveTranscoding.transcodingUsers[i].y;
                    transcodingUserInfo += "\t";
                    transcodingUserInfo += liveTranscoding.transcodingUsers[i].width;
                    transcodingUserInfo += "\t";
                    transcodingUserInfo += liveTranscoding.transcodingUsers[i].height;
                    transcodingUserInfo += "\t";
                    transcodingUserInfo += liveTranscoding.transcodingUsers[i].zOrder;
                    transcodingUserInfo += "\t";
                    transcodingUserInfo += liveTranscoding.transcodingUsers[i].alpha;
                    transcodingUserInfo += "\t";
                    transcodingUserInfo += liveTranscoding.transcodingUsers[i].audioChannel;
                    transcodingUserInfo += "\t";
                }
            }

            String liveStreamAdvancedFeaturesStr = "";
            if (liveTranscoding.liveStreamAdvancedFeatures.Length > 0) {
                for (int i = 0; i < liveTranscoding.liveStreamAdvancedFeatures.Length; i++) {
                    liveStreamAdvancedFeaturesStr += liveTranscoding.liveStreamAdvancedFeatures[i].featureName;
                    liveStreamAdvancedFeaturesStr += "\t";
                    liveStreamAdvancedFeaturesStr += liveTranscoding.liveStreamAdvancedFeatures[i].opened;
                    liveStreamAdvancedFeaturesStr += "\t";
                }
            }
            return IRtcEngineNative.setLiveTranscoding2(_channelHandler, liveTranscoding.width, liveTranscoding.height, liveTranscoding.videoBitrate, liveTranscoding.videoFramerate, liveTranscoding.lowLatency, liveTranscoding.videoGop, (int)liveTranscoding.videoCodecProfile, liveTranscoding.backgroundColor, liveTranscoding.userCount, transcodingUserInfo.ToString(),liveTranscoding.transcodingExtraInfo, liveTranscoding.metadata, liveTranscoding.watermark.url, liveTranscoding.watermark.x, liveTranscoding.watermark.y, liveTranscoding.watermark.width, liveTranscoding.watermark.height, liveTranscoding.backgroundImage.url, liveTranscoding.backgroundImage.x, liveTranscoding.backgroundImage.y, liveTranscoding.backgroundImage.width, liveTranscoding.backgroundImage.height, (int)liveTranscoding.audioSampleRate, liveTranscoding.audioBitrate, liveTranscoding.audioChannels, (int)liveTranscoding.audioCodecProfile, liveStreamAdvancedFeaturesStr, (uint)liveTranscoding.liveStreamAdvancedFeatures.Length);
        }

        /** Adds a voice or video stream URL address to the interactive live streaming.
         * 
         * The {@link agora_gaming_rtc.OnStreamPublishedHandler OnStreamPublishedHandler} callback returns the inject status. If this method call is successful, the server pulls the voice or video stream and injects it into a live channel. This is applicable to scenarios where all audience members in the channel can watch a live show and interact with each other.
         * 
         * The `AddInjectStreamUrl` method call triggers the following callbacks:
         * - The local client:
         *     - {@link agora_gaming_rtc.ChannelOnStreamInjectedStatusHandler ChannelOnStreamInjectedStatusHandler} , with the state of the injecting the online stream.
         *     - {@link agora_gaming_rtc.ChannelOnUserJoinedHandler ChannelOnUserJoinedHandler} (uid: 666), if the method call is successful and the online media stream is injected into the channel.
         * - The remote client:
         *     - `ChannelOnUserJoinedHandler` (uid: 666), if the method call is successful and the online media stream is injected into the channel.
         * 
         * @warning Agora will soon stop the service for injecting online media streams on the client. If you have not implemented this service, Agora recommends that you do not use it.
         *
         * @note 
         * - Ensure that you enable the RTMP Converter service before using this function.
         * - This method applies to the Live-Broadcast profile only.
         * - You can inject only one media stream into the channel at the same time.
         * 
         * @param url The URL address which is added to the ongoing interactive live streaming. Valid protocols are RTMP, HLS, and FLV.
         * - Supported FLV audio codec type: AAC.
         * - Supported FLV video codec type: H264 (AVC).
         * @param config The InjectStreamConfig object that contains the configuration of the added voice or video stream.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         *     -`ERR_INVALID_ARGUMENT(-2)`: The injected URL does not exist. Call this method again to inject the stream and ensure that the URL is valid.
         *     -`ERR_NOT_READY(-3)`: The user is not in the channel.
         *     -`ERR_NOT_SUPPORTED(-4)`: The channel profile is not interactive live streaming. Call the {@link agora_gaming_rtc.IRtcEngine.SetChannelProfile SetChannelProfile} method and set the channel profile to interactive live streaming before calling this method.
         *     -`ERR_NOT_INITIALIZED(-7)`: The SDK is not initialized. Ensure that the IRtcEngine object is initialized before calling this method.
         */
        public int AddInjectStreamUrl(string url, InjectStreamConfig config)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.addInjectStreamUrl2(_channelHandler, url, config.width, config.height, config.videoGop, config.videoFramerate, config.videoBitrate, (int)config.audioSampleRate, config.audioBitrate, config.audioChannels);             
        }

        /** Removes the voice or video stream URL address from the interactive live streaming.
         * 
         * This method removes the URL address (added by the {@link agora_gaming_rtc.AgoraChannel.AddInjectStreamUrl AddInjectStreamUrl} method) from the interactive live streaming.
         * 
         * @warning Agora will soon stop the service for injecting online media streams on the client. If you have not implemented this service, Agora recommends that you do not use it.
         *
         * @note If this method is called successfully, the SDK triggers the {@link agora_gaming_rtc.ChannelOnUserOffLineHandler ChannelOnUserOffLineHandler} callback and returns a stream uid of 666.
         * 
         * @param url The URL address of the added stream to be removed.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int RemoveInjectStreamUrl(string url)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.removeInjectStreamUrl2(_channelHandler, url);
        }

        /** Starts to relay media streams across channels.
         * 
         * After a successful method call, the SDK triggers the {@link agora_gaming_rtc.ChannelOnMediaRelayStateChangedHandler ChannelOnMediaRelayStateChangedHandler} and {@link agora_gaming_rtc.ChannelOnMediaRelayEventHandler ChannelOnMediaRelayEventHandler} callbacks, and these callbacks return the state and events of the media stream relay.
         * - If the `ChannelOnMediaRelayStateChangedHandler` callback returns {@link agora_gaming_rtc.CHANNEL_MEDIA_RELAY_STATE#RELAY_STATE_RUNNING RELAY_STATE_RUNNING(2)} and {@link agora_gaming_rtc.CHANNEL_MEDIA_RELAY_ERROR#RELAY_OK RELAY_OK(0)}, and the `ChannelOnMediaRelayEventHandler` callback returns {@link agora_gaming_rtc.CHANNEL_MEDIA_RELAY_EVENT#RELAY_EVENT_PACKET_SENT_TO_DEST_CHANNEL RELAY_EVENT_PACKET_SENT_TO_DEST_CHANNEL(4)}, the host starts sending data to the destination channel.
         * - If the `ChannelOnMediaRelayStateChangedHandler` callback returns {@link agora_gaming_rtc.CHANNEL_MEDIA_RELAY_STATE#RELAY_STATE_FAILURE RELAY_STATE_FAILURE(3)}, an exception occurs during the media stream relay.
         * 
         * @note
         * - Call this method after the {@link agora_gaming_rtc.AgoraChannel.JoinChannel JoinChannel} method.
         * - This method takes effect only when you are a host in a Live-broadcast channel.
         * - After a successful method call, if you want to call this method again, ensure that you call the {@link agora_gaming_rtc.AgoraChannel.StopChannelMediaRelay StopChannelMediaRelay} method to quit the current relay.
         * 
         * @param channelMediaRelayConfiguration The configuration of the media stream relay: ChannelMediaRelayConfiguration.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int StartChannelMediaRelay(ChannelMediaRelayConfiguration channelMediaRelayConfiguration)
        {            
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
            IRtcEngineNative.startChannelMediaRelay2_WEBGL(_channelHandler, channelMediaRelayConfiguration.srcInfo.channelName, channelMediaRelayConfiguration.srcInfo.token, channelMediaRelayConfiguration.srcInfo.uid + "", channelMediaRelayConfiguration.destInfos.channelName, channelMediaRelayConfiguration.destInfos.token, channelMediaRelayConfiguration.destInfos.uid + "", channelMediaRelayConfiguration.destCount);
            return 0;
#else
            return IRtcEngineNative.startChannelMediaRelay2(_channelHandler, channelMediaRelayConfiguration.srcInfo.channelName, channelMediaRelayConfiguration.srcInfo.token , channelMediaRelayConfiguration.srcInfo.uid, channelMediaRelayConfiguration.destInfos.channelName, channelMediaRelayConfiguration.destInfos.token, channelMediaRelayConfiguration.destInfos.uid, channelMediaRelayConfiguration.destCount);
#endif
        }

        /** Updates the channels for media stream relay. After a successful {@link agora_gaming_rtc.AgoraChannel.StartChannelMediaRelay StartChannelMediaRelay} method call, if you want to relay the media stream to more channels, or leave the current relay channel, you can call the `UpdateChannelMediaRelay` method.
         * 
         * After a successful method call, the SDK triggers the {@link agora_gaming_rtc.ChannelOnMediaRelayEventHandler ChannelOnMediaRelayEventHandler} callback with the {@link agora_gaming_rtc.CHANNEL_MEDIA_RELAY_EVENT#RELAY_EVENT_PACKET_UPDATE_DEST_CHANNEL RELAY_EVENT_PACKET_UPDATE_DEST_CHANNEL(7)} state code.
         * 
         * @note Call this method after the `StartChannelMediaRelay` method to update the destination channel.
         * 
         * @param channelMediaRelayConfiguration The media stream relay configuration: ChannelMediaRelayConfiguration.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int UpdateChannelMediaRelay(ChannelMediaRelayConfiguration channelMediaRelayConfiguration)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;


#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
            IRtcEngineNative.updateChannelMediaRelay2_WEBGL(_channelHandler, channelMediaRelayConfiguration.srcInfo.channelName, channelMediaRelayConfiguration.srcInfo.token, channelMediaRelayConfiguration.srcInfo.uid + "", channelMediaRelayConfiguration.destInfos.channelName, channelMediaRelayConfiguration.destInfos.token, channelMediaRelayConfiguration.destInfos.uid + "", channelMediaRelayConfiguration.destCount);
            return 0;
#else
            return IRtcEngineNative.updateChannelMediaRelay2(_channelHandler, channelMediaRelayConfiguration.srcInfo.channelName, channelMediaRelayConfiguration.srcInfo.token, channelMediaRelayConfiguration.srcInfo.uid, channelMediaRelayConfiguration.destInfos.channelName, channelMediaRelayConfiguration.destInfos.token, channelMediaRelayConfiguration.destInfos.uid, channelMediaRelayConfiguration.destCount);
#endif

        }

        /** Stops the media stream relay.
         * 
         * Once the relay stops, the host quits all the destination channels.
         * 
         * After a successful method call, the SDK triggers the {@link agora_gaming_rtc.ChannelOnMediaRelayStateChangedHandler ChannelOnMediaRelayStateChangedHandler} callback. If the callback returns {@link agora_gaming_rtc.CHANNEL_MEDIA_RELAY_STATE#RELAY_STATE_IDLE RELAY_STATE_IDLE(0)} and {@link agora_gaming_rtc.CHANNEL_MEDIA_RELAY_ERROR#RELAY_OK RELAY_OK(0)}, the host successfully stops the relay.
         * 
         * @note If the method call fails, the SDK triggers the `ChannelOnMediaRelayStateChangedHandler` callback with the {@link agora_gaming_rtc.CHANNEL_MEDIA_RELAY_ERROR#RELAY_ERROR_SERVER_NO_RESPONSE RELAY_ERROR_SERVER_NO_RESPONSE(2)} or {@link agora_gaming_rtc.CHANNEL_MEDIA_RELAY_ERROR#RELAY_ERROR_SERVER_CONNECTION_LOST RELAY_ERROR_SERVER_CONNECTION_LOST(8)} state code. You can leave the channel by calling the {@link agora_gaming_rtc.AgoraChannel.LeaveChannel LeaveChannel} method, and the media stream relay automatically stops.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public int StopChannelMediaRelay()
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
#endif
            return IRtcEngineNative.stopChannelMediaRelay2(_channelHandler);

        }
        
        /** Retrieves the connection state of the SDK.
         * 
         * @note You can call this method either before or after joining a channel.
         *
         * @return #CONNECTION_STATE_TYPE.
         */
        public CONNECTION_STATE_TYPE GetConnectionState()
        {
            if (_rtcEngine == null)
                return CONNECTION_STATE_TYPE.CONNECTION_STATE_FAILED;
#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
#endif
            return (CONNECTION_STATE_TYPE)IRtcEngineNative.getConnectionState2(_channelHandler);
        }
        /** Sets the role of a user in interactive live streaming.
         *
         * @since v3.2.0
         *
         * You can call this method either before or after joining the channel to set the user role as audience or host. If
         * you call this method to switch the user role after joining the channel, the SDK triggers the following callbacks:
         * - The local client: {@link agora_gaming_rtc.ChannelOnClientRoleChangedHandler ChannelOnClientRoleChangedHandler}.
         * - The remote client: {@link agora_gaming_rtc.ChannelOnUserJoinedHandler ChannelOnUserJoinedHandler} 
         * or {@link agora_gaming_rtc.ChannelOnUserOffLineHandler ChannelOnUserOffLineHandler}.
         *
         * @note
         * - This method applies to the `LIVE_BROADCASTING` profile only.
         * - The difference between this method and {@link agora_gaming_rtc.AgoraChannel.SetClientRole(CLIENT_ROLE_TYPE role) SetClientRole}1 is that
         * this method can set the user level in addition to the user role.
         *  - The user role determines the permissions that the SDK grants to a user, such as permission to send local
         * streams, receive remote streams, and push streams to a CDN address.
         *  - The user level determines the level of services that a user can enjoy within the permissions of the user's
         * role. For example, an audience can choose to receive remote streams with low latency or ultra low latency. Levels
         * affect prices.
         *
         * @param role The role of a user in interactive live streaming. See #CLIENT_ROLE_TYPE.
         * @param audienceLatencyLevel The detailed options of a user, including user level. See {@link agora_gaming_rtc.ClientRoleOptions ClientRoleOptions}.
         *
         * @return
         * - 0(ERR_OK): Success.
         * - < 0: Failure.
         *  - -1(ERR_FAILED): A general error occurs (no specified reason).
         *  - -2(ERR_INALID_ARGUMENT): The parameter is invalid.
         *  - -7(ERR_NOT_INITIALIZED): The SDK is not initialized.
         */
        public int SetClientRole(CLIENT_ROLE_TYPE role, ClientRoleOptions audienceLatencyLevel)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
#if !UNITY_EDITOR && UNITY_WEBGL
            return SetClientRole(role);
#endif
            return IRtcEngineNative.setClientRole_2(_channelHandler, (int)role, (int)audienceLatencyLevel.audienceLatencyLevel);
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
         *  - -7(ERR_NOT_INITIALIZED): The SDK is not initialized. Initialize the `AgoraChannel` instance before calling this method.
         */
        public int EnableEncryption(bool enabled, EncryptionConfig encryptionConfig)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setCurrentChannel_WGL(_channelId);
            return IRtcEngineNative.enableEncryption2(_channelId, enabled, encryptionConfig.encryptionKey, (int)encryptionConfig.encryptionMode);
#else
            return IRtcEngineNative.enableEncryption2(_channelHandler, enabled, encryptionConfig.encryptionKey, (int)encryptionConfig.encryptionMode, encryptionConfig.encryptionKdfSalt);
#endif
        }

        public int MuteLocalVideoStream(bool mute)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;  

            return IRtcEngineNative.muteLocalVideoStream_channel(_channelHandler, mute);
        }

        public int MuteLocalAudioStream(bool mute)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
            
            return IRtcEngineNative.muteLocalAudioStream_channel(_channelHandler, mute);
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
         * {@link agora_gaming_rtc.ChannelOnUserSuperResolutionEnabledHandler ChannelOnUserSuperResolutionEnabledHandler} callback to report
         * whether you have successfully enabled the super-resolution algorithm.
         *
         * @warning The super-resolution algorithm requires extra system resources.
         * To balance the visual experience and system usage, the SDK poses the following restrictions:
         * - The algorithm can only be used for a single user at a time.
         * - On the Android platform, the original resolution of the remote video must not exceed 640 × 360 pixels.
         * - On the iOS platform, the original resolution of the remote video must not exceed 640 × 480 pixels.
         * If you exceed these limitations, the SDK triggers the {@link agora_gaming_rtc.ChannelOnWarningHandler ChannelOnWarningHandler}
         * callback with the corresponding warning codes:
         * - `WARN_SUPER_RESOLUTION_STREAM_OVER_LIMITATION (1610)`: The origin resolution of the remote video is beyond the range where the super-resolution algorithm can be applied.
         * - `WARN_SUPER_RESOLUTION_USER_COUNT_OVER_LIMITATION (1611)`: Another user is already using the super-resolution algorithm.
         * - `WARN_SUPER_RESOLUTION_DEVICE_NOT_SUPPORTED (1612)`: The device does not support the super-resolution algorithm.
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
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.enableRemoteSuperResolution2(_channelHandler, userId, enable);     
        }
        /// @endcond

        [MonoPInvokeCallback(typeof(ChannelOnWarningHandler))]
        private static void OnWarningCallback(string channelId, int warn, string message)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnWarning != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                        if (_channelDictionary.ContainsKey(channelId))
                        {
                            AgoraChannel ch = _channelDictionary[channelId];
                            if (ch != null && channel.ChannelOnWarning != null)
                            {
                                ch.ChannelOnWarning(channelId, warn, message);
                            }
                        }
                    });
                    }
                }
            }
        }

        [MonoPInvokeCallback(typeof(ChannelOnErrorHandler))]
        private static void OnErrorCallback(string channelId, int err, string message)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnError != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnError != null)
                                {
                                    ch.ChannelOnError(channelId, err, message);
                                }
                            }
                        });
                    }
                }
            }
        }

        [MonoPInvokeCallback(typeof(ChannelOnJoinChannelSuccessHandler))]
        private static void OnJoinChannelSuccessCallback(string channelId, uint uid, int elapsed)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnJoinChannelSuccess != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnJoinChannelSuccess != null)
                                {
                                    ch.ChannelOnJoinChannelSuccess(channelId, uid, elapsed);
                                }   
                            }
                        });
                    }
                }
            }
        }

        [MonoPInvokeCallback(typeof(ChannelOnReJoinChannelSuccessHandler))]
        private static void OnReJoinChannelSuccessCallback(string channelId, uint uid, int elapsed)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnReJoinChannelSuccess != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnReJoinChannelSuccess != null)
                                {
                                    ch.ChannelOnJoinChannelSuccess(channelId, uid, elapsed);
                                }   
                            }
                        });
                    }
                }
            }
        }

        [MonoPInvokeCallback(typeof(ChannelEngineEventOnLeaveChannelHandler))]
        private static void OnLeaveChannelCallback(string channelId, uint duration, uint txBytes, uint rxBytes, uint txAudioBytes, 
        uint txVideoBytes, uint rxAudioBytes, uint rxVideoBytes, ushort txKBitRate, ushort rxKBitRate, ushort rxAudioKBitRate, ushort txAudioKBitRate, ushort rxVideoKBitRate, ushort txVideoKBitRate, ushort lastmileDelay, ushort txPacketLossRate, ushort rxPacketLossRate, uint userCount, double cpuAppUsage, double cpuTotalUsage, int gatewayRtt, double memoryAppUsageRatio, double memoryTotalUsageRatio, int memoryAppUsageInKbytes)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnLeaveChannel != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnLeaveChannel != null)
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
                                    ch.ChannelOnLeaveChannel(channelId, rtcStats);
                                }   
                            }
                        });
                    }
                }
            }
        }

        [MonoPInvokeCallback(typeof(ChannelOnClientRoleChangedHandler))]
        private static void OnClientRoleChangedCallback(string channelId, CLIENT_ROLE_TYPE oldRole, CLIENT_ROLE_TYPE newRole)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnClientRoleChanged != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnClientRoleChanged != null)
                                {
                                    ch.ChannelOnClientRoleChanged(channelId, oldRole, newRole);
                                }   
                            }
                        });
                    }
                }
            }
        }

        [MonoPInvokeCallback(typeof(ChannelOnUserJoinedHandler))]
        private static void OnUserJoinedCallback(string channelId, uint uid, int elapsed)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnUserJoined != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnUserJoined != null)
                                {
                                    ch.ChannelOnUserJoined(channelId, uid, elapsed);
                                }   
                            }
                        });
                    }
                }
            }
        }

        [MonoPInvokeCallback(typeof(ChannelOnUserOffLineHandler))]
        private static void OnUserOffLineCallback(string channelId, uint uid, USER_OFFLINE_REASON reason)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnUserOffLine != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnUserOffLine != null)
                                {
                                    ch.ChannelOnUserOffLine(channelId, uid, reason);
                                }   
                            }
                        });
                    }
                }
            }
        }

        [MonoPInvokeCallback(typeof(ChannelOnConnectionLostHandler))]
        private static void OnConnectionLostCallback(string channelId)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnConnectionLost != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnConnectionLost != null)
                                {
                                    ch.ChannelOnConnectionLost(channelId);
                                }   
                            }
                        });
                    }
                }
            }
        }

        [MonoPInvokeCallback(typeof(ChannelOnRequestTokenHandler))]
        private static void OnRequestTokenCallback(string channelId)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnRequestToken != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnRequestToken != null)
                                {
                                    ch.ChannelOnRequestToken(channelId);
                                }   
                            }
                        });
                    }
                }
            }
        }

        [MonoPInvokeCallback(typeof(ChannelOnTokenPrivilegeWillExpireHandler))]
        private static void OnTokenPrivilegeWillExpireCallback(string channelId, string token)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnTokenPrivilegeWillExpire != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnTokenPrivilegeWillExpire != null)
                                {
                                    ch.ChannelOnTokenPrivilegeWillExpire(channelId, token);
                                }   
                            }
                        });
                    }
                }
            }
        }    


        [MonoPInvokeCallback(typeof(ChannelEngineEventOnRtcStatsHandler))]
        private static void OnRtcStatsCallback(string channelId, uint duration, uint txBytes, uint rxBytes, uint txAudioBytes, 
        uint txVideoBytes, uint rxAudioBytes, uint rxVideoBytes, ushort txKBitRate, ushort rxKBitRate, ushort rxAudioKBitRate, ushort txAudioKBitRate, ushort rxVideoKBitRate, ushort txVideoKBitRate, ushort lastmileDelay, ushort txPacketLossRate, ushort rxPacketLossRate, uint userCount, double cpuAppUsage, double cpuTotalUsage, int gatewayRtt, double memoryAppUsageRatio, double memoryTotalUsageRatio, int memoryAppUsageInKbytes)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnRtcStats != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnRtcStats != null)
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
                                    ch.ChannelOnRtcStats(channelId, rtcStats);   
                                }
                            }
                        });
                    }
                }
            }
        } 

        [MonoPInvokeCallback(typeof(ChannelOnNetworkQualityHandler))]
        private static void OnNetworkQualityCallback(string channelId, uint uid, int txQuality, int rxQuality)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnNetworkQuality != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnNetworkQuality != null)
                                {
                                    ch.ChannelOnNetworkQuality(channelId, uid, txQuality, rxQuality);
                                }   
                            }
                        });
                    }
                }
            }
        } 

        [MonoPInvokeCallback(typeof(ChannelEngineEventOnRemoteVideoStatsHandler))]
        private static void OnRemoteVideoStatsCallback(string channelId, uint uid, int delay, int width, int height, int receivedBitrate, int decoderOutputFrameRate, int rendererOutputFrameRate, int packetLossRate, int rxStreamType, int totalFrozenTime, int frozenRate, int totalActiveTime, int publishDuration)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnRemoteVideoStats != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnRemoteVideoStats != null)
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
                                    ch.ChannelOnRemoteVideoStats(channelId, remoteVideoStats);
                                }   
                            }
                        });
                    }
                }
            }
        } 

        [MonoPInvokeCallback(typeof(ChannelEngineEventOnRemoteAudioStatsHandler))]
        private static void OnRemoteAudioStatsCallback(string channelId, uint uid, int quality, int networkTransportDelay, int jitterBufferDelay, int audioLossRate, int numChannels, int receivedSampleRate, int receivedBitrate, int totalFrozenTime, int frozenRate, int totalActiveTime, int publishDuration, int qoeQuality, int qualityChangedReason, int mosValue)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnRemoteAudioStats != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnRemoteAudioStats != null)
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
                                    ch.ChannelOnRemoteAudioStats(channelId, remoteAudioStats);
                                }   
                            }
                        });
                    }
                }
            }
        } 


        [MonoPInvokeCallback(typeof(ChannelOnRemoteAudioStateChangedHandler))]
        private static void OnRemoteAudioStatChangedCallback(string channelId, uint uid, REMOTE_AUDIO_STATE state, REMOTE_AUDIO_STATE_REASON reason, int elapsed)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnRemoteAudioStateChanged != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnRemoteAudioStateChanged != null)
                                {
                                    ch.ChannelOnRemoteAudioStateChanged(channelId, uid, state, reason, elapsed);
                                }   
                            }
                        });
                    }
                }
            }
        } 		

        [MonoPInvokeCallback(typeof(ChannelOnActiveSpeakerHandler))]
        private static void OnActiveSpeakerCallback(string channelId, uint uid)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnActiveSpeaker != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnActiveSpeaker != null)
                                {
                                    ch.ChannelOnActiveSpeaker(channelId, uid);
                                }   
                            }
                        });
                    }
                }
            }
        } 

        [MonoPInvokeCallback(typeof(ChannelOnVideoSizeChangedHandler))]
        private static void OnVideoSizeChangedCallback(string channelId, uint uid, int width, int height, int rotation)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnVideoSizeChanged != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnVideoSizeChanged != null)
                                {
                                    ch.ChannelOnVideoSizeChanged(channelId, uid, width, height, rotation);
                                }   
                            }
                        });
                    }
                }
            }
        }

       [MonoPInvokeCallback(typeof(ChannelOnRemoteVideoStateChangedHandler))]
        private static void OnRemoteVideoStateChangedCallback(string channelId, uint uid, REMOTE_VIDEO_STATE state, REMOTE_VIDEO_STATE_REASON reason, int elapsed)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnRemoteVideoStateChanged != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnRemoteVideoStateChanged != null)
                                {
                                    ch.ChannelOnRemoteVideoStateChanged(channelId, uid, state, reason, elapsed);
                                }   
                            }
                        });
                    }
                }
            }
        }  

        [MonoPInvokeCallback(typeof(ChannelOnStreamMessageHandler))]
        private static void OnStreamMessageCallback(string channelId, uint uid, int streamId, string data, int length)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnStreamMessage != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnStreamMessage != null)
                                {
                                    ch.ChannelOnStreamMessage(channelId, uid, streamId, data, length);
                                }   
                            }
                        });
                    }
                }
            }
        }  
		
        [MonoPInvokeCallback(typeof(ChannelOnStreamMessageErrorHandler))]
        private static void OnStreamMessageErrorCallback(string channelId, uint uid, int streamId, int code, int missed, int cached)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnStreamMessageError != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnStreamMessageError != null)
                                {
                                    ch.ChannelOnStreamMessageError(channelId, uid, streamId, code, missed, cached);
                                }   
                            }
                        });
                    }
                }
            }
        }  

        [MonoPInvokeCallback(typeof(ChannelOnMediaRelayStateChangedHandler))]
        private static void OnMediaRelayStateChangedCallback(string channelId, CHANNEL_MEDIA_RELAY_STATE state, CHANNEL_MEDIA_RELAY_ERROR code)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnMediaRelayStateChanged != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnMediaRelayStateChanged != null)
                                {
                                    ch.ChannelOnMediaRelayStateChanged(channelId, state, code);
                                }
                            }
                        });
                    }
                }
            }
        }

        [MonoPInvokeCallback(typeof(ChannelOnMediaRelayEventHandler))]
        private static void OnMediaRelayEventCallback(string channelId, CHANNEL_MEDIA_RELAY_EVENT code)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnMediaRelayEvent != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnMediaRelayEvent != null)
                                {
                                    ch.ChannelOnMediaRelayEvent(channelId, code);
                                }   
                            }
                        });
                    }
                }
            }
        }

        [MonoPInvokeCallback(typeof(ChannelOnRtmpStreamingStateChangedHandler))]
        private static void OnRtmpStreamingStateChangedCallback(string channelId, string url, RTMP_STREAM_PUBLISH_STATE state, RTMP_STREAM_PUBLISH_ERROR errCode)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnRtmpStreamingStateChanged != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnRtmpStreamingStateChanged != null)
                                {
                                    ch.ChannelOnRtmpStreamingStateChanged(channelId, url, state, errCode);
                                }   
                            }
                        });
                    }
                }
            }
        }
		
        [MonoPInvokeCallback(typeof(ChannelOnTranscodingUpdatedHandler))]
        private static void OnTranscodingUpdatedCallback(string channelId)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnTranscodingUpdated != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnTranscodingUpdated != null)
                                {
                                    ch.ChannelOnTranscodingUpdated(channelId);
                                }   
                            }
                        });
                    }
                }
            }
        }

        [MonoPInvokeCallback(typeof(ChannelOnStreamInjectedStatusHandler))]
        private static void OnStreamInjectedStatusCallback(string channelId, string url, uint uid, int status)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnStreamInjectedStatus != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnStreamInjectedStatus != null)
                                {
                                    ch.ChannelOnStreamInjectedStatus(channelId, url, uid, status);
                                }   
                            }
                        });
                    }
                }
            }
        }

        [MonoPInvokeCallback(typeof(ChannelOnRemoteSubscribeFallbackToAudioOnlyHandler))]
        private static void OnRemoteSubscribeFallbackToAudioOnlyCallback(string channelId, uint uid, bool isFallbackOrRecover)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnRemoteSubscribeFallbackToAudioOnly != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnRemoteSubscribeFallbackToAudioOnly != null)
                                {
                                    ch.ChannelOnRemoteSubscribeFallbackToAudioOnly(channelId, uid, isFallbackOrRecover);
                                }   
                            }
                        });
                    }
                }
            }
        }

       [MonoPInvokeCallback(typeof(ChannelOnConnectionStateChangedHandler))]
        private static void OnConnectionStateChangedCallback(string channelId, CONNECTION_STATE_TYPE state, CONNECTION_CHANGED_REASON_TYPE reason)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnConnectionStateChanged != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnConnectionStateChanged != null)
                                {
                                    ch.ChannelOnConnectionStateChanged(channelId, state, reason);
                                }  
                            }
                        });
                    }
                }
            }
        }

        [MonoPInvokeCallback(typeof(ChannelOnLocalPublishFallbackToAudioOnlyHandler))]
        private static void OnLocalPublishFallbackToAudioOnlyCallback(string channelId, bool isFallbackOrRecover)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnLocalPublishFallbackToAudioOnly != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnLocalPublishFallbackToAudioOnly != null)
                                {
                                    ch.ChannelOnLocalPublishFallbackToAudioOnly(channelId, isFallbackOrRecover);
                                }  
                            }
                        });
                    }
                }
            }
        }
 
        [MonoPInvokeCallback(typeof(ChannelOnRtmpStreamingEventHandler))]
        private static void OnRtmpStreamingEventCallback(string channelId, string url, RTMP_STREAMING_EVENT eventCode)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnRtmpStreamingEvent != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnRtmpStreamingEvent != null)
                                {
                                    ch.ChannelOnRtmpStreamingEvent(channelId, url, eventCode);
                                }  
                            }
                        });
                    }
                }
            }
        }
 
        [MonoPInvokeCallback(typeof(ChannelOnAudioPublishStateChangedHandler))]
        private static void OnAudioPublishStateChangedCallback(string channelId, STREAM_PUBLISH_STATE oldState, STREAM_PUBLISH_STATE newState, int elapseSinceLastState)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnAudioPublishStateChanged != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnAudioPublishStateChanged != null)
                                {
                                    ch.ChannelOnAudioPublishStateChanged(channelId, oldState, newState, elapseSinceLastState);
                                }  
                            }
                        });
                    }
                }
            }
        }

        [MonoPInvokeCallback(typeof(ChannelOnVideoPublishStateChangedHandler))]
        private static void OnVideoPublishStateChangedCallback(string channelId, STREAM_PUBLISH_STATE oldState, STREAM_PUBLISH_STATE newState, int elapseSinceLastState)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnVideoPublishStateChanged != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnVideoPublishStateChanged != null)
                                {
                                    ch.ChannelOnVideoPublishStateChanged(channelId, oldState, newState, elapseSinceLastState);
                                }  
                            }
                        });
                    }
                }
            }
        }

        [MonoPInvokeCallback(typeof(ChannelOnAudioSubscribeStateChangedHandler))]
        private static void OnAudioSubscribeStateChangedCallback(string channelId, uint uid, STREAM_SUBSCRIBE_STATE oldState, STREAM_SUBSCRIBE_STATE newState, int elapseSinceLastState)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnAudioSubscribeStateChanged != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnAudioSubscribeStateChanged != null)
                                {
                                    ch.ChannelOnAudioSubscribeStateChanged(channelId, uid, oldState, newState, elapseSinceLastState);
                                }  
                            }
                        });
                    }
                }
            }
        }

        [MonoPInvokeCallback(typeof(ChannelOnVideoSubscribeStateChangedHandler))]
        private static void OnVideoSubscribeStateChangedCallback(string channelId, uint uid, STREAM_SUBSCRIBE_STATE oldState, STREAM_SUBSCRIBE_STATE newState, int elapseSinceLastState)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnVideoSubscribeStateChanged != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnVideoSubscribeStateChanged != null)
                                {
                                    ch.ChannelOnVideoSubscribeStateChanged(channelId, uid, oldState, newState, elapseSinceLastState);
                                }  
                            }
                        });
                    }
                }
            }
        }

       [MonoPInvokeCallback(typeof(ChannelOnUserSuperResolutionEnabledHandler))]
        private static void OnUserSuperResolutionEnabledCallback(string channelId, uint uid, bool enabled, SUPER_RESOLUTION_STATE_REASON reason)
        {
            AgoraChannel channel = null;
            if (_channelDictionary.ContainsKey(channelId))
            {
                channel = _channelDictionary[channelId];
                if (channel != null && channel.ChannelOnUserSuperResolutionEnabled != null && _AgoraCallbackObjectDictionary[channelId] != null)
                {
                    AgoraCallbackQueue queue = _AgoraCallbackObjectDictionary[channelId]._CallbackQueue;
                    if (queue != null)
                    {
                        queue.EnQueue(()=> {
                            if (_channelDictionary.ContainsKey(channelId))
                            {
                                AgoraChannel ch = _channelDictionary[channelId];
                                if (ch != null && channel.ChannelOnUserSuperResolutionEnabled != null)
                                {
                                    ch.ChannelOnUserSuperResolutionEnabled(channelId, uid, enabled, reason);
                                }  
                            }
                        });
                    }
                }
            }
        }

        private void initChannelEvent()
        {
            IRtcEngineNative.initChannelEventCallback(_channelHandler, OnWarningCallback,
                                                    OnErrorCallback,
                                                    OnJoinChannelSuccessCallback,
                                                    OnReJoinChannelSuccessCallback,
                                                    OnLeaveChannelCallback,
                                                    OnClientRoleChangedCallback,
                                                    OnUserJoinedCallback,
                                                    OnUserOffLineCallback,
                                                    OnConnectionLostCallback,
                                                    OnRequestTokenCallback,
                                                    OnTokenPrivilegeWillExpireCallback,
                                                    OnRtcStatsCallback,
                                                    OnNetworkQualityCallback,
                                                    OnRemoteVideoStatsCallback,
                                                    OnRemoteAudioStatsCallback,
                                                    OnRemoteAudioStatChangedCallback,
                                                    OnActiveSpeakerCallback,
                                                    OnVideoSizeChangedCallback,
                                                    OnRemoteVideoStateChangedCallback,
                                                    OnStreamMessageCallback,
                                                    OnStreamMessageErrorCallback,
                                                    OnMediaRelayStateChangedCallback,
                                                    OnMediaRelayEventCallback,
                                                    OnRtmpStreamingStateChangedCallback,
                                                    OnTranscodingUpdatedCallback,
                                                    OnStreamInjectedStatusCallback,
                                                    OnRemoteSubscribeFallbackToAudioOnlyCallback,
                                                    OnConnectionStateChangedCallback,
                                                    OnLocalPublishFallbackToAudioOnlyCallback,
                                                    OnRtmpStreamingEventCallback,
                                                    OnAudioPublishStateChangedCallback,
                                                    OnVideoPublishStateChangedCallback,
                                                    OnAudioSubscribeStateChangedCallback,
                                                    OnVideoSubscribeStateChangedCallback,
                                                    OnUserSuperResolutionEnabledCallback);
        }
    }
}