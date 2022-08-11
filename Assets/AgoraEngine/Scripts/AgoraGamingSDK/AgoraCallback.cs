
namespace agora_gaming_rtc
{
        /** Occurs when a user joins a channel.
        *
        * This callback notifies the application that a user joins a specified channel when the application calls the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method.
        *
        * The channel name assignment is based on `channelName` specified in the `JoinChannelByKey` method.
        *
        * If the `uid` is not specified in the `JoinChannelByKey` method, the server automatically assigns a `uid`.
        *
        * @param channelName The name of the channel that you join.
        * @param uid The user ID of the user joining the channel.
        * @param elapsed Time elapsed (ms) from the user calling the `JoinChannelByKey` method until the SDK triggers this callback.
        */
        public delegate void OnJoinChannelSuccessHandler(string channelName, uint uid, int elapsed);

        /** Occurs when a user leaves the channel.
        *
        * This callback notifies the application that a user leaves the channel when the application calls the {@link agora_gaming_rtc.IRtcEngine.LeaveChannel LeaveChannel} method.
        *
        * The application retrieves information, such as the call duration and statistics.
        *
        * @param stats The statistics of the call: RtcStats.
        */
        public delegate void OnLeaveChannelHandler(RtcStats stats);

        /** Occurs when a user rejoins the channel after disconnection due to network problems.
        *
        * When a user loses connection with the server because of network problems, the SDK automatically tries to reconnect and triggers this callback upon reconnection.
        *
        * @param channelName The name of the channel that you rejoin.
        * @param uid The user ID of the user rejoining the channel.
        * @param elapsed The time elapsed (ms) from starting to reconnect until the SDK triggers this callback.
        */
        public delegate void OnReJoinChannelSuccessHandler(string channelName, uint uid, int elapsed);

        /** Occurs when the SDK cannot reconnect to Agora's edge server 10 seconds after its connection to the server is interrupted.
        *
        * The SDK triggers this callback when it cannot connect to the server 10 seconds after calling the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method, whether or not it is in the channel.
        *
        * This callback is different from {@link agora_gaming_rtc.OnConnectionInterruptedHandler OnConnectionInterruptedHandler}:
        * - The SDK triggers the `OnConnectionInterruptedHandler` callback when it loses connection with the server for more than four seconds after it successfully joins the channel.
        * - The SDK triggers the `OnConnectionLostHandler` callback when it loses connection with the server for more than 10 seconds, whether or not it joins the channel.
        *
        * If the SDK fails to rejoin the channel 20 minutes after being disconnected from Agora's edge server, the SDK stops rejoining the channel.
        */
        public delegate void OnConnectionLostHandler();

        /** Occurs when the connection between the SDK and the server is interrupted.
        *
        * @deprecated Deprecated as of v2.3.2. Replaced by the {@link agora_gaming_rtc.OnConnectionStateChangedHandler OnConnectionStateChangedHandler} callback.
        *
        * The SDK triggers this callback when it loses connection with the server for more than four seconds after the connection is established.
        *
        * After triggering this callback, the SDK tries reconnecting to the server. You can use this callback to implement pop-up reminders.
        *
        * This callback is different from {@link agora_gaming_rtc.OnConnectionLostHandler OnConnectionLostHandler}:
        * - The SDK triggers the `OnConnectionInterruptedHandler` callback when it loses connection with the server for more than four seconds after it successfully joins the channel.
        * - The SDK triggers the `OnConnectionLostHandler` callback when it loses connection with the server for more than 10 seconds, whether or not it joins the channel.
        *
        * If the SDK fails to rejoin the channel 20 minutes after being disconnected from Agora's edge server, the SDK stops rejoining the channel.
        */
        public delegate void OnConnectionInterruptedHandler();

        /** Occurs when the token expires.
        *
        * After a token is specified by calling the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method, if the SDK losses connection with the Agora server due to network issues, the token may expire after a certain period of time and a new token may be required to reconnect to the server.
        *
        * This callback notifies the app to generate a new token and call `JoinChannelByKey` to rejoin the channel with the new token.
        */
        public delegate void OnRequestTokenHandler();

        /** Occurs when a remote user (Communication)/ host (Live Broadcast) joins the channel.
        *
        * - Communication profile: This callback notifies the application that another user joins the channel. If other users are already in the channel, the SDK also reports to the application on the existing users.
        * - Live-broadcast profile: This callback notifies the application that the host joins the channel. If other hosts are already in the channel, the SDK also reports to the application on the existing hosts. We recommend limiting the number of hosts to 17.
        *
        * The SDK triggers this callback under one of the following circumstances:
        * - A remote user/host joins the channel by calling the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method.
        * - A remote user switches the user role to the host by calling the {@link agora_gaming_rtc.IRtcEngine.SetClientRole SetClientRole} method after joining the channel.
        * - A remote user/host rejoins the channel after a network interruption.
        * - The host injects an online media stream into the channel by calling the {@link agora_gaming_rtc.IRtcEngine.AddInjectStreamUrl AddInjectStreamUrl} method.
        *
        * @note
        * In the Live-broadcast profile:
        * - The host receives this callback when another host joins the channel.
        * - The audience in the channel receives this callback when a new host joins the channel.
        * - When a web application joins the channel, the SDK triggers this callback as long as the web application publishes streams.
        *
        * @param uid The user ID of the user or host joining the channel.
        * @param elapsed Time delay (ms) from the local user calling the `JoinChannelByKey` method until the SDK triggers this callback.
        */
        public delegate void OnUserJoinedHandler(uint uid, int elapsed);

        /** Occurs when a remote user (Communication)/host (Live Broadcast) leaves the channel.
        *
        * Reasons why the user is offline:
        *
        * - Leave the channel: When the user/host leaves the channel, the user/host sends a goodbye message. When the message is received, the SDK assumes that the user/host leaves the channel.
        * - Drop offline: When no data packet of the user or host is received for a certain period of time (20 seconds for the Communication profile, and more for the Live-broadcast profile), the SDK assumes that the user/host drops offline. Unreliable network connections may lead to false detections, so we recommend using the Agora RTM SDK for more reliable offline detection.
        *
        * @param uid The user ID of the user leaving the channel or going offline.
        * @param reason The reason why the user is offline: #USER_OFFLINE_REASON.
        */
        public delegate void OnUserOfflineHandler(uint uid, USER_OFFLINE_REASON reason);

        /** Reports the volume information of users.
         *
         * By default, this callback is disabled. You can enable it by calling {@link agora_gaming_rtc.IRtcEngine.EnableAudioVolumeIndication EnableAudioVolumeIndication}.
         * Once this callback is enabled and users send streams in the channel, the SDK triggers the `OnVolumeIndicationHandler` callback
         * at the time interval set in `EnableAudioVolumeIndication`.
         *
         * The SDK triggers two independent `OnVolumeIndicationHandler` callbacks simultaneously, which separately report the
         * volume information of the local user who sends a stream and the remote users (up to three) whose instantaneous
         * volumes are the highest.
         *
         * @note After you enable this callback, calling {@link agora_gaming_rtc.IRtcEngine.MuteLocalAudioStream MuteLocalAudioStream}
         * affects the SDK's behavior as follows:
         * - If the local user calls `MuteLocalAudioStream`, the SDK stops triggering the local user's callback.
         * - 20 seconds after a remote user whose volume is one of the three highest calls `MuteLocalAudioStream`, the
         * remote users' callback excludes this remote user's information; 20 seconds after all remote users call
         * `MuteLocalAudioStream`, the SDK stops triggering the remote users' callback.
         *
         * @param speakers The volume information of users. See AudioVolumeInfo.
         *
         * An empty speakers array in the callback indicates that no remote user is in the channel or sending a stream at the moment.
         * @param speakerNumber Total number of users.
         * - In the local user's callback, when the local user sends a stream, `speakerNumber` = `1`.
         * - In the remote users' callback, the value ranges between `0` and `3`. If the number of remote users who send
         * streams is greater than or equal to three, `speakerNumber` = `3`.
         * @param totalVolume Total volume after audio mixing. The value ranges between 0 (lowest volume) and 255 (highest volume).
         * - In the local user's callback, `totalVolume` is the volume of the local user who sends a stream.
         * - In the remote users' callback, `totalVolume` is the sum of the volume of all remote users (up to three) whose
         * instantaneous volumes are the highest.
         *
         * If the user calls {@link agora_gaming_rtc.IRtcEngine.StartAudioMixing StartAudioMixing}, `totalVolume` is the sum of
         * the voice volume and audio-mixing volume.
         */
        public delegate void OnVolumeIndicationHandler(AudioVolumeInfo[] speakers, int speakerNumber, int totalVolume);
        /** Reports the voice pitch of the local user.
         * @since 3.7.0
         *
         * After the local audio capture is enabled and you call {@link agora_gaming_rtc.IRtcEngine.EnableLocalVoicePitchCallback EnableLocalVoicePitchCallback}, the SDK triggers this callback at the time interval set in `EnableLocalVoicePitchCallback`.
         * @note After this callback is enabled, if the user disables the local audio capture, for example, by calling `EnableLocalAudio(false)`, the SDK immediately stops sending the `OnLocalVoicePitchInHzHandler` callback.
         * @param pitchInHz The voice pitch (Hz) of the local user.
         */
        public delegate void OnLocalVoicePitchInHzHandler(int pitchInHz);

        /** Occurs when a remote user's audio stream playback pauses/resumes.
        *
        * @deprecated This method is deprecated from v3.0.1, use the {@link agora_gaming_rtc.OnRemoteAudioStateChangedHandler OnRemoteAudioStateChangedHandler} callback instead.
        *
        * The SDK triggers this callback when the remote user stops or resumes sending the audio stream by calling the {@link agora_gaming_rtc.IRtcEngine.MuteLocalAudioStream MuteLocalAudioStream} method.
        *
        * @note This callback does not work properly when the number of users (in the `COMMUNICATION` profile) or hosts (in the `LIVE_BROADCASTING` profile) in the channel exceeds 17.
        *
        * @param uid The user ID of the remote user.
        * @param muted Whether the remote user's audio stream is muted/unmuted:
        * - true: Muted.
        * - false: Unmuted.
        */
        public delegate void OnUserMutedAudioHandler(uint uid, bool muted);

        /** Reports a warning during SDK runtime.
        *
        * In most cases, the application can ignore the warning reported by the SDK because the SDK can usually fix the issue and resume running. For example, when losing connection with the server, the SDK may report `WARN_LOOKUP_CHANNEL_TIMEOUT(104)` and automatically try to reconnect.
        *
        * @param warn The warning code, see [Warning Code](./index.html#warn).
        * @param msg The warning message.
        */
        public delegate void OnSDKWarningHandler(int warn, string msg);

        /** Reports an error during SDK runtime.
        *
        * In most cases, the SDK cannot fix the issue and resume running. The SDK requires the application to take action or informs the user about the issue.
        *
        * For example, the SDK reports an `ERR_START_CALL(1002)` error when failing to initialize a call. The application informs the user that the call initialization failed and invokes the {@link agora_gaming_rtc.IRtcEngine.LeaveChannel LeaveChannel} method to leave the channel.
        *
        * @param error The error code, see [Error Code](./index.html#error).
        * @param msg The error message.
        */
        public delegate void OnSDKErrorHandler(int error, string msg);

        /** Reports the statistics of the current call session once every two seconds.
        *
        * @param stats The RTC engine statistics: RtcStats.
        */
        public delegate void OnRtcStatsHandler(RtcStats stats);

        /** Occurs when the audio mixing file playback finishes.
        *
        * @deprecated use {@link agora_gaming_rtc.OnAudioMixingStateChangedHandler OnAudioMixingStateChangedHandler} instead.
        *
        * You can start an audio mixing file playback by calling the {@link agora_gaming_rtc.IRtcEngine.StartAudioMixing StartAudioMixing} method. The SDK triggers this callback when the audio mixing file playback finishes.
        *
        * If the `StartAudioMixing` method call fails, an error code returns in the {@link agora_gaming_rtc.OnSDKErrorHandler OnSDKErrorHandler} callback.
        */
        public delegate void OnAudioMixingFinishedHandler();


        /** Occurs when the local audio route changes.
        *
        * The SDK triggers this callback when the local audio route switches to an earpiece, speakerphone, headset, or Bluetooth device.
        *
        * @param route The current audio routing. See: #AUDIO_ROUTE.
        */
        public delegate void OnAudioRouteChangedHandler(AUDIO_ROUTE route);

        /** Occurs when the first remote video frame is received and decoded.
        * 
        * @deprecated Use the {@link agora_gaming_rtc.OnRemoteVideoStateChangedHandler OnRemoteVideoStateChangedHandler} callback with the following parameters instead:
        * - {@link agora_gaming_rtc.REMOTE_VIDEO_STATE#REMOTE_VIDEO_STATE_STARTING REMOTE_VIDEO_STATE_STARTING(1)}
        * - {@link agora_gaming_rtc.REMOTE_VIDEO_STATE#REMOTE_VIDEO_STATE_DECODING REMOTE_VIDEO_STATE_DECODING(2)}
        * 
        * This callback is triggered in either of the following scenarios:
        * - The remote user joins the channel and sends the video stream.
        * - The remote user stops sending the video stream and re-sends it after 15 seconds. Reasons for such an interruption include:
        *       - The remote user leaves the channel.
        *       - The remote user drops offline.
        *       - The remote user calls the {@link agora_gaming_rtc.IRtcEngine.MuteLocalVideoStream MuteLocalVideoStream} method to stop sending the video stream.
        *       - The remote user calls the {@link agora_gaming_rtc.IRtcEngine.DisableVideo DisableVideo} method to disable video.
        * The application can configure the user view settings in this callback.
        *
        * @param uid The user ID of the remote user sending the video stream.
        * @param width The width (px) of the video stream.
        * @param height The height (px) of the video stream.
        * @param elapsed Time elapsed (ms) from the local user calling the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method until the SDK triggers this callback.
        */
        public delegate void OnFirstRemoteVideoDecodedHandler(uint uid, int width, int height, int elapsed);

        /** Occurs when the video size or rotation of a specified user changes.
        *
        * @param uid The user ID of the remote user or local user (0) whose video size or rotation changes.
        * @param width The new width (pixels) of the video.
        * @param height The new height (pixels) of the video.
        * @param rotation The new rotation of the video [0 to 360).
        */
        public delegate void OnVideoSizeChangedHandler(uint uid, int width, int height, int rotation);

        /** Occurs when the user role switches successfully in the interactive live streaming.
        *
        * In the `LIVE_BROADCASTING` channel profile, when the local user successfully calls `setClientRole`
        * to switch their user role after joining the channel, for example, from a host to an audience member
        * or vice versa, the SDK triggers this callback to report the user role before and after the switch.
        *
        * @param oldRole Role that the user switches from: #CLIENT_ROLE_TYPE.
        * @param newRole Role that the user switches to: #CLIENT_ROLE_TYPE.
        */
        public delegate void OnClientRoleChangedHandler(CLIENT_ROLE_TYPE oldRole, CLIENT_ROLE_TYPE newRole);

        /** Occurs when a remote user's video stream playback pauses/resumes.
        *
        * You can also use the {@link agora_gaming_rtc.OnRemoteVideoStateChangedHandler OnRemoteVideoStateChangedHandler} callback with the following parameters:
        * - {@link agora_gaming_rtc.REMOTE_VIDEO_STATE#REMOTE_VIDEO_STATE_STOPPED REMOTE_VIDEO_STATE_STOPPED(0)} and {@link agora_gaming_rtc.REMOTE_VIDEO_STATE_REASON#REMOTE_VIDEO_STATE_REASON_REMOTE_MUTED REMOTE_VIDEO_STATE_REASON_REMOTE_MUTED(5)}.
        * - {@link agora_gaming_rtc.REMOTE_VIDEO_STATE#REMOTE_VIDEO_STATE_DECODING REMOTE_VIDEO_STATE_DECODING(2)} and {@link agora_gaming_rtc.REMOTE_VIDEO_STATE_REASON#REMOTE_VIDEO_STATE_REASON_REMOTE_UNMUTED REMOTE_VIDEO_STATE_REASON_REMOTE_UNMUTED(6)}.
        *
        * The SDK triggers this callback when the remote user stops or resumes sending the video stream by calling the {@link agora_gaming_rtc.IRtcEngine.MuteLocalVideoStream MuteLocalVideoStream} method.
        *
        * @note This callback does not work properly when the number of users (in the `COMMUNICATION` profile) or hosts (in the `LIVE_BROADCASTING` profile) in the channel exceeds 17.
        *
        * @param uid The user ID of the remote user.
        * @param muted Whether the remote user's video stream playback is paused/resumed:
        * - true: Paused.
        * - false: Resumed.
        */
        public delegate void OnUserMuteVideoHandler(uint uid, bool muted);

        /** Occurs when the microphone is enabled/disabled.
        *
        * @deprecated Use {@link agora_gaming_rtc.LOCAL_AUDIO_STREAM_STATE#LOCAL_AUDIO_STREAM_STATE_STOPPED LOCAL_AUDIO_STREAM_STATE_STOPPED(0)} or {@link agora_gaming_rtc.LOCAL_AUDIO_STREAM_STATE#LOCAL_AUDIO_STREAM_STATE_RECORDING LOCAL_AUDIO_STREAM_STATE_RECORDING(1)} in the {@link agora_gaming_rtc.OnLocalAudioStateChangedHandler OnLocalAudioStateChangedHandler} callback instead.
        *
        * The SDK triggers this callback when the local user resumes or stops capturing the local audio stream by calling the {@link agora_gaming_rtc.IRtcEngine.EnableLocalAudio EnableLocalAudio} method.
        *
        * @param isEnabled Whether the microphone is enabled/disabled:
        * - true: Enabled.
        * - false: Disabled.
        */
        public delegate void OnMicrophoneEnabledHandler(bool isEnabled);

        /** Occurs when a method is executed by the SDK.
        *
        * @param err The [Error Code](./index.html#error). returned by the SDK when a method call fails. If the SDK returns 0, then the method call is successful.
        * @param api The method executed by the SDK.
        * @param result The result of the method call.
        */
        public delegate void OnApiExecutedHandler(int err, string api, string result);

        /** Reports the last mile network quality of the local user once every two seconds before the user joins the channel.
        *
        * Last mile refers to the connection between the local device and Agora's edge server. After the application calls the {@link agora_gaming_rtc.IRtcEngine.EnableLastmileTest EnableLastmileTest} method, this callback reports once every two seconds the uplink and downlink last mile network conditions of the local user before the user joins the channel.
        *
        * @param quality The last mile network quality: #QUALITY_TYPE.
        */
        public delegate void OnLastmileQualityHandler(int quality);

        /** Occurs when the engine sends the first local audio frame.
        *
        * @deprecated Deprecated as of v3.2.0. Use the {@link agora_gaming_rtc.OnFirstLocalAudioFramePublishedHandler OnFirstLocalAudioFramePublishedHandler} callback instead.
        *
        * @param elapsed The time elapsed (ms) from the local user calling {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} until the SDK triggers this callback.
        */
        public delegate void OnFirstLocalAudioFrameHandler(int elapsed);

        /** Occurs when the engine receives the first audio frame from a specific remote user.
        *
        * @deprecated v3.0.1 This callback is deprecated. Use {@link agora_gaming_rtc.OnRemoteAudioStateChangedHandler OnRemoteAudioStateChangedHandler} instead.
        *
        * @param userId The user ID of the remote user.
        * @param elapsed The time elapsed (ms) from the remote user calling {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} until the SDK triggers this callback.
        */
        public delegate void OnFirstRemoteAudioFrameHandler(uint userId, int elapsed);

        /** Reports the statistics of the audio stream from each remote user/host.
        *
        * @deprecated Deprecated as of v2.3.2. Use the {@link agora_gaming_rtc.OnRemoteAudioStatsHandler OnRemoteAudioStatsHandler} callback instead.
        *
        * The SDK triggers this callback once every two seconds to report the audio quality of each remote user/host sending an audio stream. If a channel has multiple users/hosts sending audio streams, the SDK triggers this callback as many times.
        *
        * @param userId The user ID of the speaker.
        * @param quality The audio quality of the user: #QUALITY_TYPE.
        * @param delay The time delay (ms) of sending the audio packet from the sender to the receiver, including the time delay of audio sampling pre-processing, transmission, and the jitter buffer.
        * @param lost The packet loss rate (%) of the audio packet sent from the sender to the receiver.
        */
        public delegate void OnAudioQualityHandler(uint userId, int quality, ushort delay, ushort lost);

        /** Occurs when a voice or video stream URL address is added to the interactive live streaming.
         *
         * @warning Agora will soon stop the service for injecting online media streams on the client. If you have not implemented this service, Agora recommends that you do not use it.
         *
         * @param url The URL address of the externally injected stream.
         * @param userId The user ID.
         * @param status The state of the externally injected stream: #INJECT_STREAM_STATUS.
         */
        public delegate void OnStreamInjectedStatusHandler(string url, uint userId, int status);

        /** Reports the result of calling the {@link agora_gaming_rtc.IRtcEngine.RemovePublishStreamUrl RemovePublishStreamUrl} method. (CDN live only.)
         *
         * @deprecated v3.0.1 This method is deprecated. Use {@link agora_gaming_rtc.OnRtmpStreamingStateChangedHandler OnRtmpStreamingStateChangedHandler} instead.
         *
         * This callback indicates whether you have successfully removed an RTMP or RTMPS stream from the CDN.
         *
         * @param url The CDN streaming URL.
         */
        public delegate void OnStreamUnpublishedHandler(string url);

        /** Reports the result of calling the {@link agora_gaming_rtc.IRtcEngine.AddPublishStreamUrl AddPublishStreamUrl} method. (CDN live only.)
         *
         * @deprecated v3.0.1 This method is deprecated. Use {@link agora_gaming_rtc.OnRtmpStreamingStateChangedHandler OnRtmpStreamingStateChangedHandler} instead.
         *
         * @param url The CDN streaming URL.
         * @param error Error code: [Error Code](./index.html#error).
         * - `ERR_OK(0)`: The publishing succeeds.
         * - `ERR_FAILED(1)`: The publishing fails.
         * - `ERR_INVALID_ARGUMENT(2)`: Invalid argument used. If, for example, you did not call {@link agora_gaming_rtc.IRtcEngine.SetLiveTranscoding SetLiveTranscoding} to configure LiveTranscoding before calling `AddPublishStreamUrl`, the SDK reports `ERR_INVALID_ARGUMENT(2)`.
         * - `ERR_TIMEDOUT(10)`: The publishing timed out.
         * - `ERR_ALREADY_IN_USE(19)`: The chosen URL address is already in use for CDN live streaming.
         * - `ERR_RESOURCE_LIMITED(22)`: The backend system does not have enough resources for the CDN live streaming.
         * - `ERR_ENCRYPTED_STREAM_NOT_ALLOWED_PUBLISH(130)`: You cannot publish an encrypted stream.
         * - `ERR_PUBLISH_STREAM_CDN_ERROR(151)`
         * - `ERR_PUBLISH_STREAM_NUM_REACH_LIMIT(152)`
         * - `ERR_PUBLISH_STREAM_NOT_AUTHORIZED(153)`
         * - `ERR_PUBLISH_STREAM_INTERNAL_SERVER_ERROR(154)`
         * - `ERR_PUBLISH_STREAM_FORMAT_NOT_SUPPORTED(156)`
         */
        public delegate void OnStreamPublishedHandler(string url, int error);

        /** Occurs when the local user does not receive the data stream from the remote user within five seconds.
         *
         * The SDK triggers this callback when the local user fails to receive the stream message that the remote user sends by calling the {@link agora_gaming_rtc.IRtcEngine.SendStreamMessage SendStreamMessage} method.
         *
         * @param userId The user ID of the remote user sending the message.
         * @param streamId The stream ID.
         * @param code The error code: [Error Code](./index.html#error).
         * @param missed The number of lost messages.
         * @param cached The number of incoming cached messages when the data stream is interrupted.
         */
        public delegate void OnStreamMessageErrorHandler(uint userId, int streamId, int code, int missed, int cached);

        /** Occurs when the local user receives the data stream from the remote user within five seconds.
         *
         * The SDK triggers this callback when the local user receives the stream message that the remote user sends by calling the {@link agora_gaming_rtc.IRtcEngine.SendStreamMessage SendStreamMessage} method.
         *
         * @param userId The user ID of the remote user sending the message.
         * @param streamId The stream ID.
         * @param data The data received by the local user.
         * @param length The length of the data in bytes.
         */
        public delegate void OnStreamMessageHandler(uint userId, int streamId, byte[] data, int length);

        /** Occurs when your connection is banned by the Agora Server.
         *
         * @deprecated from v2.3.2. Replaced by the {@link agora_gaming_rtc.OnConnectionStateChangedHandler OnConnectionStateChangedHandler} callback.
         */
        public delegate void OnConnectionBannedHandler();

        /** Occurs when the connection state between the SDK and the server changes.
         *
         * @param state See #CONNECTION_STATE_TYPE.
         * @param reason See #CONNECTION_CHANGED_REASON_TYPE.
         */
        public delegate void OnConnectionStateChangedHandler(CONNECTION_STATE_TYPE state, CONNECTION_CHANGED_REASON_TYPE reason);

        /** Occurs when the token expires in 30 seconds.
         *
         * The user becomes offline if the token used in the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method expires. The SDK triggers this callback 30 seconds before the token expires to remind the application to get a new token. Upon receiving this callback, generate a new token on the server and call the {@link agora_gaming_rtc.IRtcEngine.RenewToken RenewToken} method to pass the new token to the SDK.
         *
         * @param token The token that expires in 30 seconds.
         */
        public delegate void OnTokenPrivilegeWillExpireHandler(string token);

        /** Occurs when the token has expired.
         *
         * The user becomes offline if the token used in the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method expires.
         *
         * @param token The token that expires in 30 seconds.
         */
        public delegate void OnTokenPrivilegeDidExpireHandler(string token);

        /** Occurs when the most active speaker is detected.
         *
         * After a successful call of {@link agora_gaming_rtc.IRtcEngine.EnableAudioVolumeIndication EnableAudioVolumeIndication},
         * the SDK continuously detects which remote user has the loudest volume. During the current period, the remote user,
         * who is detected as the loudest for the most times, is the most active user.
         *
         * When the number of user is no less than two and an active speaker exists, the SDK triggers this callback and reports the `uid` of the most active speaker.
         * - If the most active speaker is always the same user, the SDK triggers this callback only once.
         * - If the most active speaker changes to another user, the SDK triggers this callback again and reports the `uid` of the new active speaker.
         *
         * @param uid The user ID of the most active speaker.
         */
        public delegate void OnActiveSpeakerHandler(uint uid);

        /** Occurs when the video stops playing.
         *
         * @deprecated Deprecated as of v2.4.1. Use {@link agora_gaming_rtc.LOCAL_VIDEO_STREAM_STATE#LOCAL_VIDEO_STREAM_STATE_STOPPED LOCAL_VIDEO_STREAM_STATE_STOPPED(0)} in the {@link agora_gaming_rtc.OnLocalVideoStateChangedHandler OnLocalVideoStateChangedHandler} callback instead.
         *
         * The application can use this callback to change the configuration of the view (for example, displaying other pictures in the view) after the video stops playing.
         */
        public delegate void OnVideoStoppedHandler();

        /** Occurs when the first local video frame is displayed/rendered on the local video view.
         *
         * @param width Width (px) of the first local video frame.
         * @param height Height (px) of the first local video frame.
         * @param elapsed Time elapsed (ms) from the local user calling the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method until the SDK triggers this callback. If you call the {@link agora_gaming_rtc.IRtcEngine.StartPreview StartPreview} method before calling the `JoinChannelByKey` method, then `elapsed` is the time elapsed from calling the `StartPreview` method until the SDK triggers this callback.
         */
        public delegate void OnFirstLocalVideoFrameHandler(int width, int height, int elapsed);

        /** Occurs when the first remote video frame is received and decoded.
         *
         * The SDK triggers this callback when the first frame of the remote video is displayed in the user's video window. The application can retrieve the time elapsed from a user joining the channel until the first video frame is displayed.
         *
         * @param uid User ID of the remote user sending the video stream.
         * @param width Width (px) of the video frame.
         * @param height Height (px) of the video stream.
         * @param elapsed Time elapsed (ms) from the local user calling the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method until the SDK triggers this callback.
         */
        public delegate void OnFirstRemoteVideoFrameHandler(uint uid, int width, int height, int elapsed);

        /** Occurs when a specific remote user enables/disables the video module.
         *
         * @deprecated This callback is deprecated and replaced by the {@link agora_gaming_rtc.OnRemoteVideoStateChangedHandler OnRemoteVideoStateChangedHandler} callback with the following parameters:
         * - {@link agora_gaming_rtc.REMOTE_VIDEO_STATE#REMOTE_VIDEO_STATE_STOPPED REMOTE_VIDEO_STATE_STOPPED(0)} and {@link agora_gaming_rtc.REMOTE_VIDEO_STATE_REASON#REMOTE_VIDEO_STATE_REASON_REMOTE_MUTED REMOTE_VIDEO_STATE_REASON_REMOTE_MUTED(5)}.
         * - {@link agora_gaming_rtc.REMOTE_VIDEO_STATE#REMOTE_VIDEO_STATE_DECODING REMOTE_VIDEO_STATE_DECODING(2)} and {@link agora_gaming_rtc.REMOTE_VIDEO_STATE_REASON#REMOTE_VIDEO_STATE_REASON_REMOTE_UNMUTED REMOTE_VIDEO_STATE_REASON_REMOTE_UNMUTED(6)}.
         *
         * Once the video module is disabled, the remote user can only use a voice call. The remote user cannot send or receive any video from other users.
         *
         * The SDK triggers this callback when the remote user enables or disables the video module by calling the {@link agora_gaming_rtc.IRtcEngine.EnableVideo EnableVideo} or {@link agora_gaming_rtc.IRtcEngine.DisableVideo DisableVideo} method.
         *
         * @note This callback returns invalid when the number of users in a channel exceeds 20.
         *
         * @param uid User ID of the remote user.
         * @param enabled Whether the specified remote user enables/disables the local video capturing function:
         * - true: Enable. The remote user can enter a video session.
         * - false: Disable. The remote user can only enter a voice session, and cannot send or receive any video stream.
         */
        public delegate void OnUserEnableVideoHandler(uint uid, bool enabled);

        /** Occurs when a specified remote user enables/disables the local video capturing function.
         *
         * @deprecated This callback is deprecated and replaced by the {@link agora_gaming_rtc.OnRemoteVideoStateChangedHandler OnRemoteVideoStateChangedHandler} callback with the following parameters:
         * - {@link agora_gaming_rtc.REMOTE_VIDEO_STATE#REMOTE_VIDEO_STATE_STOPPED REMOTE_VIDEO_STATE_STOPPED(0)} and {@link agora_gaming_rtc.REMOTE_VIDEO_STATE_REASON#REMOTE_VIDEO_STATE_REASON_REMOTE_MUTED REMOTE_VIDEO_STATE_REASON_REMOTE_MUTED(5)}.
         * - {@link agora_gaming_rtc.REMOTE_VIDEO_STATE#REMOTE_VIDEO_STATE_DECODING REMOTE_VIDEO_STATE_DECODING(2)} and {@link agora_gaming_rtc.REMOTE_VIDEO_STATE_REASON#REMOTE_VIDEO_STATE_REASON_REMOTE_UNMUTED REMOTE_VIDEO_STATE_REASON_REMOTE_UNMUTED(6)}.
         *
         * This callback is only applicable to the scenario when the user only wants to watch the remote video without sending any video stream to the other user.
         *
         * The SDK triggers this callback when the remote user resumes or stops capturing the video stream by calling the {@link agora_gaming_rtc.IRtcEngine.EnableLocalVideo EnableLocalVideo} method.
         *
         * @param uid User ID of the remote user.
         * @param enabled Whether the specified remote user enables/disables the local video capturing function:
         * - true: Enable. Other users in the channel can see the video of this remote user.
         * - false: Disable. Other users in the channel can no longer receive the video stream from this remote user, while this remote user can still receive the video streams from other users.
         */
        public delegate void OnUserEnableLocalVideoHandler(uint uid, bool enabled);

        /** Occurs when the remote video state changes.
         *
         * @note This callback does not work properly when the number of users (in the `COMMUNICATION` profile) or hosts (in the `LIVE_BROADCASTING` profile) in the channel exceeds 17.
         *
         * @param uid The ID of the remote user whose video state changes.
         * @param state The state of the remote video. See #REMOTE_VIDEO_STATE.
         * @param reason The reason of the remote video state change. See #REMOTE_VIDEO_STATE_REASON.
         * @param elapsed The time elapsed (ms) from the local user calling the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method until the SDK triggers this callback.
         */
        public delegate void OnRemoteVideoStateChangedHandler(uint uid, REMOTE_VIDEO_STATE state, REMOTE_VIDEO_STATE_REASON reason, int elapsed);

        /** Occurs when the locally published media stream falls back to an audio-only stream due to poor network conditions or switches back to the video after the network conditions improve.
         *
         * If you call {@link agora_gaming_rtc.IRtcEngine.SetLocalPublishFallbackOption SetLocalPublishFallbackOption} and set `option` as {@link agora_gaming_rtc.STREAM_FALLBACK_OPTIONS#STREAM_FALLBACK_OPTION_AUDIO_ONLY STREAM_FALLBACK_OPTION_AUDIO_ONLY(2)}, the SDK triggers this callback when the locally published stream falls back to audio-only mode due to poor uplink conditions, or when the audio stream switches back to the video after the uplink network condition improves.
         *
         * @param isFallbackOrRecover Whether the locally published stream falls back to audio-only or switches back to the video:
         * - true: The locally published stream falls back to audio-only due to poor network conditions.
         * - false: The locally published stream switches back to the video after the network conditions improve.
         */
        public delegate void OnLocalPublishFallbackToAudioOnlyHandler(bool isFallbackOrRecover);

        /** Occurs when the remote media stream falls back to audio-only stream due to poor network conditions or switches back to the video stream after the network conditions improve.
         *
         * If you call {@link agora_gaming_rtc.IRtcEngine.SetRemoteSubscribeFallbackOption SetRemoteSubscribeFallbackOption} and set `option` as {@link agora_gaming_rtc.STREAM_FALLBACK_OPTIONS#STREAM_FALLBACK_OPTION_AUDIO_ONLY STREAM_FALLBACK_OPTION_AUDIO_ONLY(2)}, the SDK triggers this callback when the remote media stream falls back to audio-only mode due to poor uplink conditions, or when the remote media stream switches back to the video after the uplink network condition improves.
         *
         * @note Once the remotely subscribed media stream switches to the low stream due to poor network conditions, you can monitor the stream switch between a high and low stream in the RemoteVideoStats of the {@link agora_gaming_rtc.OnRemoteVideoStatsHandler OnRemoteVideoStatsHandler} callback.
         *
         * @param uid ID of the remote user sending the stream.
         * @param isFallbackOrRecover Whether the remotely subscribed media stream falls back to audio-only or switches back to the video:
         * - true: The remotely subscribed media stream falls back to audio-only due to poor network conditions.
         * - false: The remotely subscribed media stream switches back to the video stream after the network conditions improved.
         */
        public delegate void OnRemoteSubscribeFallbackToAudioOnlyHandler(uint uid, bool isFallbackOrRecover);

        /** Reports the last mile network quality of each user in the channel once every two seconds.
         *
         * Last mile refers to the connection between the local device and the Agora edge server. This callback reports once every two seconds the last mile network conditions of each user in the channel. If a channel includes multiple users, the SDK triggers this callback as many times.
         *
         * @param uid User ID. The network quality of the user with this `uid` is reported. If `uid` is 0, the local network quality is reported.
         * @param txQuality Uplink transmission quality rating of the user in terms of the transmission bitrate, packet loss rate, average RTT (Round-Trip Time), and jitter of the uplink network. `txQuality` is a quality rating helping you understand how well the current uplink network conditions can support the selected VideoEncoderConfiguration. For example, a 1000 Kbps uplink network may be adequate for video frames with a resolution of 640 × 480 and a frame rate of 15 fps in the Live-broadcast profile, but may be inadequate for resolutions higher than 1280 × 720. See #QUALITY_TYPE.
         * @param rxQuality Downlink network quality rating of the user in terms of the packet loss rate, average RTT, and jitter of the downlink network. See #QUALITY_TYPE.
         */
        public delegate void OnNetworkQualityHandler(uint uid, int txQuality, int rxQuality);

        /** Reports the statistics of the local video stream.
         *
         * The SDK triggers this callback once every two seconds for each user/host. If there are multiple users/hosts in the channel, the SDK triggers this callback as many times.
         *
         * @note
         * If you have called the {@link agora_gaming_rtc.IRtcEngine.EnableDualStreamMode EnableDualStreamMode} method, the `OnLocalVideoStatsHandler` callback reports the statistics of the high-video stream (high bitrate, and high-resolution video stream).
         *
         * @param localVideoStats The statistics of the local video stream. See LocalVideoStats.
         */
        public delegate void OnLocalVideoStatsHandler(LocalVideoStats localVideoStats);

        /** Reports the statistics of the video stream from each remote user/host.
         *
         * The SDK triggers this callback once every two seconds for each remote user/host. If a channel includes multiple remote users, the SDK triggers this callback as many times.
         *
         * @param remoteVideoStats The statistics of the remote video stream. See RemoteVideoStats.
         */
        public delegate void OnRemoteVideoStatsHandler(RemoteVideoStats remoteVideoStats);

        /** Reports the statistics of the audio stream from each remote user/host.
         *
         * This callback replaces the {@link agora_gaming_rtc.OnAudioQualityHandler OnAudioQualityHandler} callback.
         *
         * The SDK triggers this callback once every two seconds for each remote user/host. If a channel includes multiple remote users, the SDK triggers this callback as many times.
         *
         * @param remoteAudioStats The statistics of the received remote audio streams. See RemoteAudioStats.
         */
        public delegate void OnRemoteAudioStatsHandler(RemoteAudioStats remoteAudioStats);

        /** Occurs when the audio device state changes.
         *
         * This callback notifies the application that the system's audio device state is changed. For example, a headset is unplugged from the device.
         *
         * @note This method applies to Windows and macOS only.
         *
         * @param deviceId The device ID.
         * @param deviceType The device type: #MEDIA_DEVICE_TYPE.
         * @param deviceState The device state: #MEDIA_DEVICE_STATE_TYPE.
         */
        public delegate void OnAudioDeviceStateChangedHandler(string deviceId, int deviceType, int deviceState);

        /** Occurs when the camera turns on and is ready to capture the video.
         *
         * @deprecated Deprecated as of v2.4.1. Use {@link agora_gaming_rtc.LOCAL_VIDEO_STREAM_STATE#LOCAL_VIDEO_STREAM_STATE_CAPTURING LOCAL_VIDEO_STREAM_STATE_CAPTURING(1)} in the {@link agora_gaming_rtc.OnLocalVideoStateChangedHandler OnLocalVideoStateChangedHandler} callback instead.
         *
         * If the camera fails to turn on, fix the error reported in the {@link agora_gaming_rtc.OnSDKErrorHandler OnSDKErrorHandler} callback.
         */
        public delegate void OnCameraReadyHandler();

        /** Occurs when the camera focus area changes.
         *
         * The SDK triggers this callback when the local user changes the camera focus position by calling the setCameraFocusPositionInPreview method.
         *
         * @note This callback is for Android and iOS only.
         *
         * @param x x coordinate of the changed camera focus area.
         * @param y y coordinate of the changed camera focus area.
         * @param width Width of the changed camera focus area.
         * @param height Height of the changed camera focus area.
         */
        public delegate void OnCameraFocusAreaChangedHandler(int x, int y, int width, int height);

        /** Occurs when the camera exposure area changes.
         *
         * The SDK triggers this callback when the local user changes the camera exposure position by calling the setCameraExposurePosition method.
         *
         * @note This callback is for Android and iOS only.
         *
         * @param x x coordinate of the changed camera exposure area.
         * @param y y coordinate of the changed camera exposure area.
         * @param width Width of the changed camera exposure area.
         * @param height Height of the changed camera exposure area.
         */
        public delegate void OnCameraExposureAreaChangedHandler(int x, int y, int width, int height);

        /** Occurs when a remote user starts audio mixing.
         *
         * When a remote user calls {@link agora_gaming_rtc.IRtcEngine.StartAudioMixing StartAudioMixing} to play the background music, the SDK reports this callback.
         */
        public delegate void OnRemoteAudioMixingBeginHandler();

        /** Occurs when a remote user finishes audio mixing.
         */
        public delegate void OnRemoteAudioMixingEndHandler();

        /** Occurs when the local audio effect playback finishes.
         *
         * The SDK triggers this callback when the local audio effect file playback finishes.
         *
         * @param soundId ID of the local audio effect. Each local audio effect has a unique ID.
         */
        public delegate void OnAudioEffectFinishedHandler(int soundId);

        /** Occurs when the video device state changes.
         *
         * @note On a Windows device with an external camera for video capturing, the video disables once the external camera is unplugge
         *
         * @param deviceId The device ID of the video device that changes state.
         * @param deviceType The device type: #MEDIA_DEVICE_TYPE.
         * @param deviceState The device state: #MEDIA_DEVICE_STATE_TYPE.
         */
        public delegate void OnVideoDeviceStateChangedHandler(string deviceId, int deviceType, int deviceState);

        /** Reports the transport-layer statistics of each remote video stream.
         *
         * @deprecated This callback is deprecated and replaced by the {@link agora_gaming_rtc.OnRemoteVideoStatsHandler OnRemoteVideoStatsHandler} callback.
         *
         * This callback reports the transport-layer statistics, such as the packet loss rate and network time delay, once every two seconds after the local user receives a video packet from a remote user.
         *
         * @param uid User ID of the remote user sending the video packet.
         * @param delay Network time delay (ms) from the remote user sending the video packet to the local user.
         * @param lost Packet loss rate (%) of the video packet sent from the remote user.
         * @param rxKBitRate Received bitrate (Kbps) of the video packet sent from the remote user.
         */
        public delegate void OnRemoteVideoTransportStatsHandler(uint uid, ushort delay, ushort lost, ushort rxKBitRate);

        /** Reports the transport-layer statistics of each remote audio stream.
         *
         * @deprecated This callback is deprecated and replaced by the {@link agora_gaming_rtc.OnRemoteAudioStatsHandler OnRemoteAudioStatsHandler} callback.
         *
         * This callback reports the transport-layer statistics, such as the packet loss rate and network time delay, once every two seconds after the local user receives an audio packet from a remote user.
         *
         * @param uid  User ID of the remote user sending the audio packet.
         * @param delay Network time delay (ms) from the remote user sending the audio packet to the local user.
         * @param lost Packet loss rate (%) of the audio packet sent from the remote user.
         * @param rxKBitRate  Received bitrate (Kbps) of the audio packet sent from the remote user.
         */
        public delegate void OnRemoteAudioTransportStatsHandler(uint uid, ushort delay, ushort lost, ushort rxKBitRate);

        /** Occurs when the publisher's transcoding is updated.
         *
         * When the LiveTranscoding class in the {@link agora_gaming_rtc.IRtcEngine.SetLiveTranscoding SetLiveTranscoding} method updates, the SDK triggers the `OnTranscodingUpdatedHandler` callback to report the update information to the local host.
         *
         * @note If you call the `SetLiveTranscoding` method to set the `LiveTranscoding` class for the first time, the SDK does not trigger the `OnTranscodingUpdatedHandler` callback.
         */
        public delegate void OnTranscodingUpdatedHandler();

        /** Occurs when the volume of the playback device, microphone, or application changes.
         *
         * @note This method applies to Windows only.
         *
         * @param deviceType Device type: #MEDIA_DEVICE_TYPE.
         * @param volume Volume of the device. The value ranges between 0 and 255.
         * @param muted
         * - true: The audio device is muted.
         * - false: The audio device is not muted.
         */
        public delegate void OnAudioDeviceVolumeChangedHandler(MEDIA_DEVICE_TYPE deviceType, int volume, bool muted);

        /** Occurs when the media engine call starts.*/
        public delegate void OnMediaEngineStartCallSuccessHandler();

        /** Occurs when the media engine loads.*/
        public delegate void OnMediaEngineLoadSuccessHandler();

        /** Occurs when the state of the local user's audio mixing file changes.
         *
         * - When the audio mixing file plays, pauses playing, or stops playing, this callback returns 710, 711, or 713 in `state`, and 0 in `errorCode`.
         * - When exceptions occur during playback, this callback returns 714 in `state` and an error in `errorCode`.
         * - If the local audio mixing file does not exist, or if the SDK does not support the file format or cannot access the music file URL, the SDK returns `WARN_AUDIO_MIXING_OPEN_ERROR(701)`.
         *
         * @param state The state code. See #AUDIO_MIXING_STATE_TYPE.
         * @param reason The error code. See #AUDIO_MIXING_ERROR_TYPE.
         */
        public delegate void OnAudioMixingStateChangedHandler(AUDIO_MIXING_STATE_TYPE state, AUDIO_MIXING_REASON_TYPE reason);

        /** Occurs when the SDK decodes the first remote audio frame for playback.
         *
         * @deprecated v3.0.1 This callback is deprecated. Use {@link agora_gaming_rtc.OnRemoteAudioStateChangedHandler OnRemoteAudioStateChangedHandler} instead.
         *
         * This callback is triggered in either of the following scenarios:
         * - The remote user joins the channel and sends the audio stream.
         * - The remote user stops sending the audio stream and re-sends it after 15 seconds. Reasons for such an interruption include:
         *    - The remote user leaves channel.
         *    - The remote user drops offline.
         *    - The remote user calls the {@link agora_gaming_rtc.IRtcEngine.MuteLocalAudioStream MuteLocalAudioStream} method to stop sending the local audio stream.
         *    - The remote user calls the {@link agora_gaming_rtc.IRtcEngine.DisableAudio DisableAudio} method to disable audio.
         *
         * @param uid User ID of the remote user sending the audio stream.
         * @param elapsed Time elapsed (ms) from the local user calling the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method until the SDK triggers this callback.
         */
        public delegate void OnFirstRemoteAudioDecodedHandler(uint uid, int elapsed);

        /** Occurs when the local video stream state changes.
         *
         * This callback indicates the state of the local video stream, including camera capturing and video encoding, and allows you to troubleshoot issues when exceptions occur.
         *
         * @note For some device models, the SDK will not trigger this callback when the state of the local video changes while the local video capturing device is in use, so you have to make your own timeout judgment.
         *
         * @param localVideoState State type #LOCAL_VIDEO_STREAM_STATE. When the state is {@link agora_gaming_rtc.LOCAL_VIDEO_STREAM_STATE#LOCAL_VIDEO_STREAM_STATE_FAILED LOCAL_VIDEO_STREAM_STATE_FAILED(3)}, see the error parameter for details.
         * @param error The detailed error information. code #LOCAL_VIDEO_STREAM_ERROR.
         */
        public delegate void OnLocalVideoStateChangedHandler(LOCAL_VIDEO_STREAM_STATE localVideoState, LOCAL_VIDEO_STREAM_ERROR error);

        /** Occurs when the state of the RTMP or RTMPS streaming changes.
         *
         * The SDK triggers this callback to report the result of the local user calling the {@link agora_gaming_rtc.IRtcEngine.AddPublishStreamUrl AddPublishStreamUrl} or {@link agora_gaming_rtc.IRtcEngine.RemovePublishStreamUrl RemovePublishStreamUrl} method.
         *
         * This callback indicates the state of the RTMP or RTMPS streaming. When exceptions occur, you can troubleshoot issues by referring to the detailed error descriptions in the `errCode` parameter.
         *
         * @param url The CDN streaming URL.
         * @param state The RTMP or RTMPS streaming state. See: #RTMP_STREAM_PUBLISH_STATE.
         * @param errCode The detailed error information for streaming. See: #RTMP_STREAM_PUBLISH_ERROR_TYPE.
         */
        public delegate void OnRtmpStreamingStateChangedHandler(string url, RTMP_STREAM_PUBLISH_STATE state, RTMP_STREAM_PUBLISH_ERROR_TYPE errCode);

        /** Occurs when the local network type changes.
         *
         * When the network connection is interrupted, this callback indicates whether the interruption is caused by a network type change or poor network conditions.
         *
         * @param type See #NETWORK_TYPE.
         */
        public delegate void OnNetworkTypeChangedHandler(NETWORK_TYPE type);

        /** Reports the last-mile network probe result.
         *
         * The SDK triggers this callback within 30 seconds after the app calls the {@link agora_gaming_rtc.IRtcEngine.StartLastmileProbeTest StartLastmileProbeTest} method.
         *
         * @param result The uplink and downlink last-mile network probe test result. See LastmileProbeResult.
         */
        public delegate void OnLastmileProbeResultHandler(LastmileProbeResult result);

        /** Occurs when the local user successfully registers a user account by calling the {@link agora_gaming_rtc.IRtcEngine.RegisterLocalUserAccount RegisterLocalUserAccount} method or joins a channel by calling the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method.This callback reports the user ID and user account of the local user.
         *
         * @param uid The ID of the local user.
         * @param userAccount The user account of the local user.
         */
        public delegate void OnLocalUserRegisteredHandler(uint uid, string userAccount);

        /** Occurs when the SDK gets the user ID and user account of the remote user.
         *
         * After a remote user joins the channel, the SDK gets the UID and user account of the remote user, caches them in a mapping table object (`userInfo`), and triggers this callback on the local client.
         *
         * @param uid The ID of the remote user.
         * @param userInfo The UserInfo object that contains the user ID and user account of the remote user.
         */
        public delegate void OnUserInfoUpdatedHandler(uint uid, UserInfo userInfo);

        /** Occurs when the local audio state changes.
         *
         * This callback indicates the state change of the local audio stream, including the state of the audio capturing and encoding, and allows you to troubleshoot issues when exceptions occur.
         *
         * @note When the state is {@link agora_gaming_rtc.LOCAL_AUDIO_STREAM_STATE#LOCAL_AUDIO_STREAM_STATE_FAILED LOCAL_AUDIO_STREAM_STATE_FAILED(3)}, see the `error` parameter for details.
         *
         * @param state The state of the local audio. See #LOCAL_AUDIO_STREAM_STATE.
         * @param error The error information of the local audio. See #LOCAL_AUDIO_STREAM_ERROR.
         */
        public delegate void OnLocalAudioStateChangedHandler(LOCAL_AUDIO_STREAM_STATE state, LOCAL_AUDIO_STREAM_ERROR error);

        /** Occurs when the remote audio state changes.
         *
         * This callback indicates the state change of the remote audio stream.
         *
         * @note This callback does not work properly when the number of users (in the `COMMUNICATION` profile) or hosts (in the `LIVE_BROADCASTING` profile) in the channel exceeds 17.
         *
         * @param uid The ID of the remote user whose audio state changes.
         * @param state The state of the remote audio. See #REMOTE_AUDIO_STATE.
         * @param reason The reason of the remote audio state change. See #REMOTE_AUDIO_STATE_REASON.
         * @param elapsed Time elapsed (ms) from the local user calling the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method until the SDK triggers this callback.
         */
        public delegate void OnRemoteAudioStateChangedHandler(uint uid, REMOTE_AUDIO_STATE state, REMOTE_AUDIO_STATE_REASON reason, int elapsed);

        /** Reports the statistics of the local audio stream.
         *
         * The SDK triggers this callback once every two seconds.
         *
         * @param localAudioStats The statistics of the local audio stream. See LocalAudioStats.
         */
        public delegate void OnLocalAudioStatsHandler(LocalAudioStats localAudioStats);

        /** Occurs when the state of the media stream relay changes.
         *
         * The SDK returns the state of the current media relay with any error message.
         *
         * @param state The state code in #CHANNEL_MEDIA_RELAY_STATE.
         * @param code The error code in #CHANNEL_MEDIA_RELAY_ERROR.
         */
        public delegate void OnChannelMediaRelayStateChangedHandler(CHANNEL_MEDIA_RELAY_STATE state, CHANNEL_MEDIA_RELAY_ERROR code);

        /** Reports events during the media stream relay.
         *
         * @param events The event code in #CHANNEL_MEDIA_RELAY_EVENT.
         */
        public delegate void OnChannelMediaRelayEventHandler(CHANNEL_MEDIA_RELAY_EVENT events);

        /**
         * Reports the face detection result of the local user. Applies to Android and iOS only.
         * @since v3.0.1
         *
         * Once you enable face detection by calling {@link agora_gaming_rtc.IRtcEngine.EnableFaceDetection EnableFaceDetection(true)}, you can get the following information on the local user in real-time:
         * - The width and height of the local video.
         * - The position of the human face in the local video.
         * - The distance between the human face and the device screen. This value is based on the fitting calculation of the local video size and the position of the human face.
         *
         * @note
         * - If the SDK does not detect a face, it reduces the frequency of this callback to reduce power consumption on the local device.
         * - The SDK stops triggering this callback when a human face is in close proximity to the screen.
         * - On Android, the `distance` value reported in this callback may be slightly different from the actual distance. Therefore, Agora does not recommend using it for
         * accurate calculation.
         * @param imageWidth The width (px) of the local video.
         * @param imageHeight The height (px) of the local video.
         * @param vecRectangle The position and size of the human face on the local video:
         * - `x`: The x coordinate (px) of the human face in the local video. Taking the top left corner of the captured video as the origin,
         * the x coordinate represents the relative lateral displacement of the top left corner of the human face to the origin.
         * - `y`: The y coordinate (px) of the human face in the local video. Taking the top left corner of the captured video as the origin,
         * the y coordinate represents the relative longitudinal displacement of the top left corner of the human face to the origin.
         * - `width`: The width (px) of the human face in the captured video.
         * - `height`: The height (px) of the human face in the captured video.
         * @param vecDistance The distance (cm) between the human face and the screen.
         * @param numFaces The number of faces detected. If the value is 0, it means that no human face is detected.
         */
        public delegate void OnFacePositionChangedHandler(int imageWidth, int imageHeight, Rectangle vecRectangle, int vecDistance, int numFaces);

        /** Reports events during the RTMP or RTMPS streaming.
         *
         * @since v3.2.0
         *
         * @param url The RTMP or RTMPS streaming URL.
         * @param eventCode The event code. See #RTMP_STREAMING_EVENT
         */
        public delegate void OnRtmpStreamingEventHandler(string url, RTMP_STREAMING_EVENT eventCode);

        /** Occurs when the audio publishing state changes.
         *
         * @since v3.2.0
         *
         * This callback indicates the publishing state change of the local audio stream.
         *
         * @param channel The channel name.
         * @param oldState The previous publishing state. For details, see #STREAM_PUBLISH_STATE.
         * @param newState The current publishing state. For details, see #STREAM_PUBLISH_STATE.
         * @param elapseSinceLastState The time elapsed (ms) from the previous state to the current state.
         */
        public delegate void OnAudioPublishStateChangedHandler(string channel, STREAM_PUBLISH_STATE oldState, STREAM_PUBLISH_STATE newState, int elapseSinceLastState);

        /** Occurs when the video publishing state changes.
         *
         * @since v3.2.0
         *
         * This callback indicates the publishing state change of the local video stream.
         *
         * @param channel The channel name.
         * @param oldState The previous publishing state. For details, see #STREAM_PUBLISH_STATE.
         * @param newState The current publishing state. For details, see #STREAM_PUBLISH_STATE.
         * @param elapseSinceLastState The time elapsed (ms) from the previous state to the current state.
         */
        public delegate void OnVideoPublishStateChangedHandler(string channel, STREAM_PUBLISH_STATE oldState, STREAM_PUBLISH_STATE newState, int elapseSinceLastState);

        /** Occurs when the audio subscribing state changes.
         *
         * @since v3.2.0
         *
         * This callback indicates the subscribing state change of a remote audio stream.
         *
         * @param channel The channel name.
         * @param uid The ID of the remote user.
         * @param oldState The previous subscribing state. For details, see #STREAM_SUBSCRIBE_STATE.
         * @param newState The current subscribing state. For details, see #STREAM_SUBSCRIBE_STATE.
         * @param elapseSinceLastState The time elapsed (ms) from the previous state to the current state.
         */
        public delegate void OnAudioSubscribeStateChangedHandler(string channel, uint uid, STREAM_SUBSCRIBE_STATE oldState, STREAM_SUBSCRIBE_STATE newState, int elapseSinceLastState);

        /** Occurs when the audio subscribing state changes.
         *
         * @since v3.2.0
         *
         * This callback indicates the subscribing state change of a remote video stream.
         *
         * @param channel The channel name.
         * @param uid The ID of the remote user.
         * @param oldState The previous subscribing state. For details, see #STREAM_SUBSCRIBE_STATE.
         * @param newState The current subscribing state. For details, see #STREAM_SUBSCRIBE_STATE.
         * @param elapseSinceLastState The time elapsed (ms) from the previous state to the current state.
         */
        public delegate void OnVideoSubscribeStateChangedHandler(string channel, uint uid, STREAM_SUBSCRIBE_STATE oldState, STREAM_SUBSCRIBE_STATE newState, int elapseSinceLastState);

        /** Occurs when the first audio frame is published.
         *
         * @since v3.2.0
         *
         * The SDK triggers this callback under one of the following circumstances:
         * - The local client enables the audio module and calls {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} successfully.
         * - The local client calls {@link agora_gaming_rtc.IRtcEngine.MuteLocalAudioStream MuteLocalAudioStream(true)} and `MuteLocalAudioStream(false)` in sequence.
         * - The local client calls {@link agora_gaming_rtc.IRtcEngine.DisableAudio DisableAudio} and {@link agora_gaming_rtc.IRtcEngine.EnableAudio EnableAudio} in sequence.
         *
         * @param elapsed The time elapsed (ms) from the local client calling `JoinChannelByKey` until the SDK triggers this callback.
         */
        public delegate void OnFirstLocalAudioFramePublishedHandler(int elapsed);

        /** Occurs when the first video frame is published.
         *
         * @since v3.2.0
         *
         * The SDK triggers this callback under one of the following circumstances:
         * - The local client enables the video module and calls {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} successfully.
         * - The local client calls {@link agora_gaming_rtc.IRtcEngine.MuteLocalVideoStream MuteLocalVideoStream(true)} and `MuteLocalVideoStream(false)` in sequence.
         * - The local client calls {@link agora_gaming_rtc.IRtcEngine.DisableVideo DisableVideo} and {@link agora_gaming_rtc.IRtcEngine.EnableVideo EnableVideo} in sequence.
         *
         * @param elapsed The time elapsed (ms) from the local client calling `JoinChannelByKey` until the SDK triggers this callback.
         */
        public delegate void OnFirstLocalVideoFramePublishedHandler(int elapsed);

        /** Reports whether the super-resolution algorithm is enabled.
         *
         * @since v3.5.1
         *
         * After calling {@link agora_gaming_rtc.IRtcEngine.EnableRemoteSuperResolution EnableRemoteSuperResolution}, the SDK triggers this
         * callback to report whether the super-resolution algorithm is successfully enabled. If not successfully enabled,
         * you can use reason for troubleshooting.
         *
         * @param uid The ID of the remote user.
         * @param enabled Whether the super-resolution algorithm is successfully enabled:
         * - true: The super-resolution algorithm is successfully enabled.
         * - false: The super-resolution algorithm is not successfully enabled.
         * @param reason The reason why the super-resolution algorithm is not successfully enabled. See
         * {@link agora_gaming_rtc.SUPER_RESOLUTION_STATE_REASON SUPER_RESOLUTION_STATE_REASON}.
         */
        public delegate void OnUserSuperResolutionEnabledHandler(uint uid, bool enabled, SUPER_RESOLUTION_STATE_REASON reason);

        /// @cond
        /** Reports the result of uploading the SDK log files.
         *
         * @since v3.3.1
         *
         * After the method call of {@link agora_gaming_rtc.IRtcEngine.UploadLogFile UploadLogFile}, the SDK triggers
         * this callback to report the result of uploading the log files. If the upload fails, refer to the `reason`
         * parameter to troubleshoot.
         *
         * @param requestId The request ID. This request ID is the same as `requestId` returned by
         * `UploadLogFile`, and you can use `requestId` to match a specific upload with a callback.
         * @param success Whether the log files are successfully uploaded.
         * - true: Successfully uploads the log files.
         * - false: Fails to upload the log files. For details, see the `reason` parameter.
         * @param reason The reason for the upload failure. See {@link agora_gaming_rtc.UPLOAD_ERROR_REASON UPLOAD_ERROR_REASON}.
         */
        public delegate void OnUploadLogResultHandler(string requestId, bool success, UPLOAD_ERROR_REASON reason);
        /// @endcond

        /** Reports whether the virtual background is successfully enabled.
         *
         * After you call {@link agora_gaming_rtc.IRtcEngine.EnableVirtualBackground EnableVirtualBackground}, the SDK triggers this callback to report whether the virtual background is successfully enabled.
         *
         * @note If the background image customized in the virtual background is in PNG or JPG format, the triggering of this callback is delayed until the image is read.
         * @param enabled Whether the virtual background is successfully enabled:
         * - true: The virtual background is successfully enabled.
         * - false: The virtual background is not successfully enabled.
         * @param reason The reason why the virtual background is not successfully enabled or the message that confirms success. See #VIRTUAL_BACKGROUND_SOURCE_STATE_REASON.
         */
        public delegate void OnVirtualBackgroundSourceEnabledHandler(bool enabled, VIRTUAL_BACKGROUND_SOURCE_STATE_REASON reason);

        public delegate void OnUserScreenShareStarted(string channelName, uint id, int elapsed);

        public delegate void OnUserScreenShareStopped(string channelName, uint id, int elapsed);

        public delegate void OnUserScreenShareCanceled(string channelName, uint id, int elapsed);

        public delegate void OnChannelScreenShareStarted(string channelName, uint id, int elapsed);

        public delegate void OnChannelScreenShareStopped(string channelName, uint id, int elapsed);

        public delegate void OnChannelScreenShareCanceled(string channelName, uint id, int elapsed);



        /**
        * Reports the information of an audio file.
        *
        * @since v3.6.1.1
        *
        * After successfully calling {@link agora_gaming_rtc.IRtcEngine.GetAudioFileInfo GetAudioFileInfo}, the SDK triggers this callback to report the information of the audio file, such as the file path and duration.
        *
        * @param info The information of an audio file, see AudioFileInfo.
        * @param error The information acquisition state, see #AUDIO_FILE_INFO_ERROR.
        */
        public delegate void OnRequestAudioFileInfoHandler(AudioFileInfo info, AUDIO_FILE_INFO_ERROR error);

        /// @cond
        /*
        * Reports result of Content Inspect.
        *
        * @since v3.5.2
        */
        public delegate void OnContentInspectResultHandler(CONTENT_INSPECT_RESULT result);
        /// @endcond

        /** Reports the result of taking a video snapshot.
        *
        * @since v3.6.1.1
        *
        * After a successful {@link agora_gaming_rtc.IRtcEngine.TakeSnapshot TakeSnapshot} method call, the SDK triggers this callback to report whether the snapshot is successfully taken as well as the details for the snapshot taken.
        *
        * @param channel The channel name.
        * @param uid The user ID of the user. A `uid` of 0 indicates the local user.
        * @param filePath The local path of the snapshot.
        * @param width The width (px) of the snapshot.
        * @param height The height (px) of the snapshot.
        * @param errCode The message that confirms success or the reason why the snapshot is not successfully taken:
        * - `0`: Success.
        * - < 0: Failure:
        *   - `-1`: The SDK fails to write data to a file or encode a JPEG image.
        *   - `-2`: The SDK does not find the video stream of the specified user within one second after the {@link agora_gaming_rtc.IRtcEngine.TakeSnapshot TakeSnapshot} method call succeeds.
        */
        public delegate void OnSnapshotTakenHandler(string channel, uint uid, string filePath, int width, int height, int errCode);

        /** Occurs when the user role switch fails in the interactive live streaming.
         * @since 3.7.0
         *
         * In the `LIVE_BROADCASTING` channel profile, when the local user calls `SetClientRole`
         * to switch their user role after joining the channel but the switch fails, the SDK
         * triggers this callback to report the reason for the failure and the current user role.
         * @param reason The reason for the user role switch failure. See #CLIENT_ROLE_CHANGE_FAILED_REASON.
         * @param currentRole The current user role. See #CLIENT_ROLE_TYPE.
         */
        public delegate void OnClientRoleChangeFailedHandler(CLIENT_ROLE_CHANGE_FAILED_REASON reason, CLIENT_ROLE_TYPE currentRole);

        /** Reports the result of an audio device test.
         * @since 3.7.0
         * 
         * After successfully calling {@link agora_gaming_rtc.AudioRecordingDeviceManager.StartAudioRecordingDeviceTest StartAudioRecordingDeviceTest},
         * {@link agora_gaming_rtc.AudioPlaybackDeviceManager.StartAudioPlaybackDeviceTest StartAudioPlaybackDeviceTest}
         * or {@link agora_gaming_rtc.AudioRecordingDeviceManager.StartAudioDeviceLoopbackTest StartAudioDeviceLoopbackTest} to
         * start an audio device test, the SDK triggers this callback at the set time interval to report
         * the volume information of the audio device tested.
         * @param volumeType The volume type. See #AudioDeviceTestVolumeType.
         * @param volume The volume, in the range of [0,255].
         */
        public delegate void OnAudioDeviceTestVolumeIndicationHandler(AudioDeviceTestVolumeType volumeType, int volume);

        /**
         * Reports the proxy connection state.
         * @since 3.6.2
         *
         * You can use this callback to listen for the state of the SDK connecting to a proxy. For example, when a user calls
         * {@link agora_gaming_rtc.IRtcEngine.SetCloudProxy SetCloudProxy} and joins a channel successfully, the SDK triggers
         * this callback to report the user ID, the proxy type connected, and the time elapsed from the user calling `joinChannel`
         * until this callback is triggered.
         * @param channel The channel name.
         * @param uid The user ID.
         * @param proxyType The proxy type. See #PROXY_TYPE.
         * @param localProxyIp Reserved for future use.
         * @param elapsed The time elapsed (ms) from the user calling `joinChannel` until this callback is triggered.
         */
        public delegate void OnProxyConnectedHandler(string channel, uint uid, PROXY_TYPE proxyType, string localProxyIp, int elapsed);
     
        /// @cond
        public delegate void OnWlAccMessageHandler(WLACC_MESSAGE_REASON reason, WLACC_SUGGEST_ACTION action, string wlAccMsg);

        public delegate void OnWlAccStatsHandler(WlAccStats currentStats, WlAccStats averageStats);
        /// @endcond

        /** Occurs when the screen sharing information is updated.
        *
        * @since v3.6.1.1
        *
        * When you call {@link agora_gaming_rtc.IRtcEngine.StartScreenCaptureByDisplayId StartScreenCaptureByDisplayId} or {@link agora_gaming_rtc.IRtcEngine.StartScreenCaptureByScreenRect StartScreenCaptureByScreenRect} to start screen sharing
        * and use the `excludeWindowList` attribute to block the specified window, the SDK triggers this callback if the window blocking fails.
        *
        * @note This callback is for Windows only.
        *
        * @param info The screen sharing information. See {@link agora_gaming_rtc.ScreenCaptureInfo ScreenCaptureInfo}.
        *
        */
        public delegate void OnScreenCaptureInfoUpdatedHandler(ScreenCaptureInfo info);
}
