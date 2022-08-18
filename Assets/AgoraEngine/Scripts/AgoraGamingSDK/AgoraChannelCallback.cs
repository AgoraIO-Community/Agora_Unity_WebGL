namespace agora_gaming_rtc {	
        /** Reports a warning during SDK runtime.
        *
        * In most cases, the application can ignore the warning reported by the SDK because the SDK can usually fix the issue and resume running. For example, when losing connection with the server, the SDK may report `WARN_LOOKUP_CHANNEL_TIMEOUT(104)` and automatically try to reconnect.
        *
        * @param channelId The name of the channel that you join.
        * @param warn The warning code, see [Warning Code](./index.html#warn).
        * @param message The warning message.
        */
        public delegate void ChannelOnWarningHandler(string channelId, int warn, string message);

        /** Reports an error code of `AgoraChannel`.
        *
        * In most cases, the SDK cannot fix the issue and resume running. The SDK requires the application to take action or informs the user about the issue.
        *
        * For example, the SDK reports an `ERR_START_CALL(1002)` error when failing to initialize a call. The application informs the user that the call initialization failed and invokes the {@link agora_gaming_rtc.AgoraChannel.LeaveChannel LeaveChannel} method to leave the channel.
        *
        * @param channelId The name of the channel that you join.
        * @param err The error code, see [Error Code](./index.html#error).
        * @param message The error message.
        */
        public delegate void ChannelOnErrorHandler(string channelId, int err, string message);

        /** Occurs when a user joins a channel.
        *
        * This callback notifies the application that a user joins a specified channel when the application calls the {@link agora_gaming_rtc.AgoraChannel.JoinChannel JoinChannel} method.
        *
        * The channel name assignment is based on `channelId` specified in the `JoinChannel` method.
        *
        * If the `uid` is not specified in the `JoinChannel` method, the server automatically assigns a `uid`.
        *
        * @param channelId The name of the channel that you join.
        * @param uid The user ID of the user joining the channel.
        * @param elapsed Time elapsed (ms) from the user calling the `JoinChannel` method until the SDK triggers this callback.
        */
        public delegate void ChannelOnJoinChannelSuccessHandler(string channelId, uint uid, int elapsed);

        /** Occurs when a user rejoins the channel after disconnection due to network problems.
        *
        * When a user loses connection with the server because of network problems, the SDK automatically tries to reconnect and triggers this callback upon reconnection.
        *
        * @param channelId The name of the channel that you rejoin.
        * @param uid The user ID of the user rejoining the channel.
        * @param elapsed The time elapsed (ms) from starting to reconnect until the SDK triggers this callback.
        */
        public delegate void ChannelOnReJoinChannelSuccessHandler(string channelId, uint uid, int elapsed);

        /** Occurs when a user leaves the channel.
        *
        * This callback notifies the application that a user leaves the channel when the application calls the {@link agora_gaming_rtc.AgoraChannel.LeaveChannel LeaveChannel} method.
        *
        * The application retrieves information, such as the call duration and statistics.
        *
        * @param channelId The name of the channel that you join.
        * @param rtcStats The statistics of the call: RtcStats.
        */
        public delegate void ChannelOnLeaveChannelHandler(string channelId, RtcStats rtcStats);

        /** Occurs when the user role switches in the interactive live streaming. For example, from a host to an audience or vice versa.
        *
        * This callback notifies the application of a user role switch when the application calls the {@link agora_gaming_rtc.AgoraChannel.SetClientRole SetClientRole} method.
        *
        * The SDK triggers this callback when the local user switches the user role by calling the `SetClientRole` method after joining the channel.
        *
        * @param channelId The name of the channel that you join.
        * @param oldRole Role that the user switches from: #CLIENT_ROLE_TYPE.
        * @param newRole Role that the user switches to: #CLIENT_ROLE_TYPE.
        */
        public delegate void ChannelOnClientRoleChangedHandler(string channelId, CLIENT_ROLE_TYPE oldRole, CLIENT_ROLE_TYPE newRole);

        /** Occurs when a remote user (Communication) or host (Live Broadcast) joins the channel.
        *
        * - Communication profile: This callback notifies the application that another user joins the channel. If other users are already in the channel, the SDK also reports to the application on the existing users.
        * - Live-broadcast profile: This callback notifies the application that the host joins the channel. If other hosts are already in the channel, the SDK also reports to the application on the existing hosts. We recommend limiting the number of hosts to 17.
        *
        * The SDK triggers this callback under one of the following circumstances:
        * - A remote user or host joins the channel by calling the {@link agora_gaming_rtc.AgoraChannel.JoinChannel JoinChannel} method.
        * - A remote user switches the user role to the host by calling the {@link agora_gaming_rtc.AgoraChannel.SetClientRole SetClientRole} method after joining the channel.
        * - A remote user or host rejoins the channel after a network interruption.
        * - The host injects an online media stream into the channel by calling the {@link agora_gaming_rtc.AgoraChannel.AddInjectStreamUrl AddInjectStreamUrl} method.
        *
        * @note
        * In the Live-broadcast profile:
        * - The host receives this callback when another host joins the channel.
        * - The audience in the channel receives this callback when a new host joins the channel.
        * - When a web application joins the channel, the SDK triggers this callback as long as the web application publishes streams.
        *
        * @param channelId The name of the channel that you join.
        * @param uid The user ID of the user or host joining the channel.
        * @param elapsed Time delay (ms) from the local user calling the `JoinChannel` method until the SDK triggers this callback.
        */
        public delegate void ChannelOnUserJoinedHandler(string channelId, uint uid, int elapsed);

        /** Occurs when a remote user (Communication) or host (Live Broadcast) leaves the channel.
        *
        * Reasons why the user is offline:
        *
        * - Leave the channel: When the user or host leaves the channel, the user or host sends a goodbye message. When the message is received, the SDK assumes that the user or host leaves the channel.
        * - Drop offline: When no data packet of the user or host is received for a certain period of time (20 seconds for the Communication profile, and more for the Live-broadcast profile), the SDK assumes that the user or host drops offline. Unreliable network connections may lead to false detections, so we recommend using the Agora RTM SDK for more reliable offline detection.
        *
        * @param channelId The name of the channel that you join.
        * @param uid The user ID of the user leaving the channel or going offline.
        * @param reason The reason why the user is offline: #USER_OFFLINE_REASON.
        */
        public delegate void ChannelOnUserOffLineHandler(string channelId, uint uid, USER_OFFLINE_REASON reason);

        /** Occurs when the SDK cannot reconnect to Agora's edge server 10 seconds after its connection to the server is interrupted.
        *
        * The SDK triggers this callback when it cannot connect to the server 10 seconds after calling the {@link agora_gaming_rtc.AgoraChannel.JoinChannel JoinChannel} method, whether or not it is in the channel.
        *
        * This callback is different from {@link agora_gaming_rtc.OnConnectionInterruptedHandler OnConnectionInterruptedHandler}:
        * - The SDK triggers the `OnConnectionInterruptedHandler` callback when it loses connection with the server for more than four seconds after it successfully joins the channel.
        * - The SDK triggers the `ChannelOnConnectionLostHandler` callback when it loses connection with the server for more than 10 seconds, whether or not it joins the channel.
        *
        * If the SDK fails to rejoin the channel 20 minutes after being disconnected from Agora's edge server, the SDK stops rejoining the channel.
        *
        * @param channelId The name of the channel that you join.
        */
        public delegate void ChannelOnConnectionLostHandler(string channelId);

        /** Occurs when the token expires.
        *
        * After a token is specified by calling the {@link agora_gaming_rtc.AgoraChannel.JoinChannel JoinChannel} method, if the SDK losses connection with the Agora server due to network issues, the token may expire after a certain period of time and a new token may be required to reconnect to the server.
        *
        * This callback notifies the app to generate a new token and call `JoinChannel` to rejoin the channel with the new token.
        *
        * @param channelId The name of the channel that you join.
        */
        public delegate void ChannelOnRequestTokenHandler(string channelId);

        /** Occurs when the token expires in 30 seconds.
         *
         * The user becomes offline if the token used in the {@link agora_gaming_rtc.AgoraChannel.JoinChannel JoinChannel} method expires. The SDK triggers this callback 30 seconds before the token expires to remind the application to get a new token. Upon receiving this callback, generate a new token on the server and call the {@link agora_gaming_rtc.AgoraChannel.RenewToken RenewToken} method to pass the new token to the SDK.
         *
         * @param channelId The name of the channel that you join.
         * @param token The token that expires in 30 seconds.
         */
        public delegate void ChannelOnTokenPrivilegeWillExpireHandler(string channelId, string token);

        /** Occurs when the token expires.
         *
         *
         * @param channelId The name of the channel that you joined.
         * @param token The token that expired.
         */
        public delegate void ChannelOnTokenPrivilegeDidExpireHandler(string channelId, string token);

        /** Reports the statistics of the current call session once every two seconds.
        *
        * @param channelId The name of the channel that you join.
        * @param rtcStats The AgoraChannel engine statistics: RtcStats.
        */
        public delegate void ChannelOnRtcStatsHandler(string channelId, RtcStats rtcStats);

        /** Reports the last mile network quality of each user in the channel once every two seconds.
         *
         * Last mile refers to the connection between the local device and the Agora edge server. This callback reports once every two seconds the last mile network conditions of each user in the channel. If a channel includes multiple users, the SDK triggers this callback as many times.
         *
         * @param channelId The name of the channel that you join.
         * @param uid User ID. The network quality of the user with this `uid` is reported. If `uid` is 0, the local network quality is reported.
         * @param txQuality Uplink transmission quality rating of the user in terms of the transmission bitrate, packet loss rate, average RTT (Round-Trip Time), and jitter of the uplink network. `txQuality` is a quality rating helping you understand how well the current uplink network conditions can support the selected VideoEncoderConfiguration. For example, a 1000 Kbps uplink network may be adequate for video frames with a resolution of 640 × 480 and a frame rate of 15 fps in the Live-broadcast profile, but may be inadequate for resolutions higher than 1280 × 720. See #QUALITY_TYPE.
         * @param rxQuality Downlink network quality rating of the user in terms of the packet loss rate, average RTT, and jitter of the downlink network. See #QUALITY_TYPE.
         */
        public delegate void ChannelOnNetworkQualityHandler(string channelId, uint uid, int txQuality, int rxQuality);

        /** Reports the statistics of the video stream from each remote user or host.
         *
         * The SDK triggers this callback once every two seconds for each remote user or host. If a channel includes multiple remote users, the SDK triggers this callback as many times.
         *
         * @param channelId The name of the channel that you join.
         * @param remoteVideoStats The statistics of the remote video stream. See RemoteVideoStats.
         */
        public delegate void ChannelOnRemoteVideoStatsHandler(string channelId, RemoteVideoStats remoteVideoStats);

        /** Reports the statistics of the audio stream from each remote user or host.
         *
         * This callback replaces the {@link agora_gaming_rtc.OnAudioQualityHandler OnAudioQualityHandler} callback.
         *
         * The SDK triggers this callback once every two seconds for each remote user or host. If a channel includes multiple remote users, the SDK triggers this callback as many times.
         *
         * @param channelId The name of the channel that you join.
         * @param remoteAudioStats The statistics of the received remote audio streams. See RemoteAudioStats.
         */
        public delegate void ChannelOnRemoteAudioStatsHandler(string channelId, RemoteAudioStats remoteAudioStats);

        /** Occurs when the remote audio state changes.
         *
         * This callback indicates the state change of the remote audio stream.
         *
         * @note This callback does not work properly when the number of users (in the `COMMUNICATION` profile) or hosts (in the `LIVE_BROADCASTING` profile) in the channel exceeds 17.
         *
         * @param channelId The name of the channel that you join.
         * @param uid The ID of the remote user whose audio state changes.
         * @param state The state of the remote audio. See #REMOTE_AUDIO_STATE.
         * @param reason The reason of the remote audio state change. See #REMOTE_AUDIO_STATE_REASON.
         * @param elapsed Time elapsed (ms) from the local user calling the {@link agora_gaming_rtc.AgoraChannel.JoinChannel JoinChannel} method until the SDK triggers this callback.
         */
        public delegate void ChannelOnRemoteAudioStateChangedHandler(string channelId, uint uid, REMOTE_AUDIO_STATE state, REMOTE_AUDIO_STATE_REASON reason, int elapsed);

        /** Reports which user is the loudest speaker.
         *
         * If the user enables the audio volume indication by calling the {@link agora_gaming_rtc.IRtcEngine.EnableAudioVolumeIndication EnableAudioVolumeIndication} method, this callback returns the `uid` of the active speaker detected by the audio volume detection module of the SDK.
         *
         * @note
         * - To receive this callback, you need to call the `EnableAudioVolumeIndication` method.
         * - This callback returns the user ID of the user with the highest voice volume during a period of time, instead of at the moment.
         *
         * @param channelId The name of the channel that you join.
         * @param uid The user ID of the active speaker. A `uid` of 0 represents the local user.
         */
        public delegate void ChannelOnActiveSpeakerHandler(string channelId, uint uid);

        /** Occurs when the video size or rotation of a specified user changes.
        *
        * @param channelId The name of the channel that you join.
        * @param uid The user ID of the remote user or local user (0) whose video size or rotation changes.
        * @param width The new width (pixels) of the video.
        * @param height The new height (pixels) of the video.
        * @param rotation The new rotation of the video [0 to 360).
        */
        public delegate void ChannelOnVideoSizeChangedHandler(string channelId, uint uid, int width, int height, int rotation);

        /** Occurs when the remote video state changes.
         *
         * @note This callback does not work properly when the number of users (in the `COMMUNICATION` profile) or hosts (in the `LIVE_BROADCASTING` profile) in the channel exceeds 17.
         *
         * @param channelId The name of the channel that you join.
         * @param uid The ID of the remote user whose video state changes.
         * @param state The state of the remote video. See #REMOTE_VIDEO_STATE.
         * @param reason The reason of the remote video state change. See #REMOTE_VIDEO_STATE_REASON.
         * @param elapsed The time elapsed (ms) from the local user calling the {@link agora_gaming_rtc.AgoraChannel.JoinChannel JoinChannel} method until the SDK triggers this callback.
         */
        public delegate void ChannelOnRemoteVideoStateChangedHandler(string channelId, uint uid, REMOTE_VIDEO_STATE state, REMOTE_VIDEO_STATE_REASON reason, int elapsed);

        /** Occurs when the local user receives the data stream from the remote user within five seconds.
         *
         * The SDK triggers this callback when the local user receives the stream message that the remote user sends by calling the {@link agora_gaming_rtc.AgoraChannel.SendStreamMessage SendStreamMessage} method.
         *
         * @param channelId The name of the channel that you join.
         * @param uid The user ID of the remote user sending the message.
         * @param streamId The stream ID.
         * @param data The data received by the local user.
         * @param length The length of the data in bytes.
         */
        public delegate void ChannelOnStreamMessageHandler(string channelId, uint uid, int streamId, string data, int length);

        /** Occurs when the local user does not receive the data stream from the remote user within five seconds.
         *
         * The SDK triggers this callback when the local user fails to receive the stream message that the remote user sends by calling the {@link agora_gaming_rtc.AgoraChannel.SendStreamMessage SendStreamMessage} method.
         *
         * @param channelId The name of the channel that you join.
         * @param uid The user ID of the remote user sending the message.
         * @param streamId The stream ID.
         * @param code The error code: [Error Code](./index.html#error).
         * @param missed The number of lost messages.
         * @param cached The number of incoming cached messages when the data stream is interrupted.
         */
        public delegate void ChannelOnStreamMessageErrorHandler(string channelId, uint uid, int streamId, int code, int missed, int cached);

        /** Occurs when the state of the media stream relay changes.
         *
         * The SDK returns the state of the current media relay with any error message.
         *
         * @param channelId The name of the channel that you join.
         * @param state The state code in #CHANNEL_MEDIA_RELAY_STATE.
         * @param code The error code in #CHANNEL_MEDIA_RELAY_ERROR.
         */
        public delegate void ChannelOnMediaRelayStateChangedHandler(string channelId, CHANNEL_MEDIA_RELAY_STATE state, CHANNEL_MEDIA_RELAY_ERROR code);

        /** Reports events during the media stream relay.
         *
         * @param channelId The name of the channel that you join.
         * @param events The event code in #CHANNEL_MEDIA_RELAY_EVENT.
         */
        public delegate void ChannelOnMediaRelayEventHandler(string channelId, CHANNEL_MEDIA_RELAY_EVENT events);

        /** Occurs when the state of the RTMP or RTMPS streaming changes.
         *
         * The SDK triggers this callback to report the result of the local user calling the {@link agora_gaming_rtc.AgoraChannel.AddPublishStreamUrl AddPublishStreamUrl} or {@link agora_gaming_rtc.AgoraChannel.RemovePublishStreamUrl RemovePublishStreamUrl} method.
         *
         * This callback indicates the state of the RTMP or RTMPS streaming. When exceptions occur, you can troubleshoot issues by referring to the detailed error descriptions in the `errCode` parameter.
         *
         * @param channelId The name of the channel that you join.
         * @param url The CDN streaming URL.
         * @param state The RTMP or RTMPS streaming state. See: #RTMP_STREAM_PUBLISH_STATE.
         * @param errCode The detailed error information for streaming. See: #RTMP_STREAM_PUBLISH_ERROR_TYPE.
         */
        public delegate void ChannelOnRtmpStreamingStateChangedHandler(string channelId, string url, RTMP_STREAM_PUBLISH_STATE state, RTMP_STREAM_PUBLISH_ERROR_TYPE errCode);

        /** Occurs when the publisher's transcoding is updated.
         *
         * When the LiveTranscoding class in the {@link agora_gaming_rtc.AgoraChannel.SetLiveTranscoding SetLiveTranscoding} method updates, the SDK triggers the `ChannelOnTranscodingUpdatedHandler` callback to report the update information to the local host.
         *
         * @note If you call the `SetLiveTranscoding` method to set the `LiveTranscoding` class for the first time, the SDK does not trigger the `ChannelOnTranscodingUpdatedHandler` callback.
         *
         * @param channelId The name of the channel that you join.
         */
        public delegate void ChannelOnTranscodingUpdatedHandler(string channelId);

        /** Occurs when a voice or video stream URL address is added to the interactive live streaming.
         *
         * @warning Agora will soon stop the service for injecting online media streams on the client. If you have not implemented this service, Agora recommends that you do not use it.
         *
         * @param channelId The name of the channel that you join.
         * @param url The URL address of the externally injected stream.
         * @param uid The user ID.
         * @param status The state of the externally injected stream: #INJECT_STREAM_STATUS.
         */
        public delegate void ChannelOnStreamInjectedStatusHandler(string channelId, string url, uint uid, int status);

        /** Occurs when the remote media stream falls back to audio-only stream due to poor network conditions or switches back to the video stream after the network conditions improve.
         *
         * If you call {@link agora_gaming_rtc.IRtcEngine.SetRemoteSubscribeFallbackOption SetRemoteSubscribeFallbackOption} and set `option` as {@link agora_gaming_rtc.STREAM_FALLBACK_OPTIONS#STREAM_FALLBACK_OPTION_AUDIO_ONLY STREAM_FALLBACK_OPTION_AUDIO_ONLY(2)}, the SDK triggers this callback when the remote media stream falls back to audio-only mode due to poor uplink conditions, or when the remote media stream switches back to the video after the uplink network condition improves.
         *
         * @note Once the remotely subscribed media stream switches to the low stream due to poor network conditions, you can monitor the stream switch between a high and low stream in the RemoteVideoStats of the {@link agora_gaming_rtc.ChannelOnRemoteVideoStatsHandler ChannelOnRemoteVideoStatsHandler} callback.
         *
         * @param channelId The name of the channel that you join.
         * @param uid ID of the remote user sending the stream.
         * @param isFallbackOrRecover Whether the remotely subscribed media stream falls back to audio-only or switches back to the video:
         * - true: The remotely subscribed media stream falls back to audio-only due to poor network conditions.
         * - false: The remotely subscribed media stream switches back to the video stream after the network conditions improved.
         */
        public delegate void ChannelOnRemoteSubscribeFallbackToAudioOnlyHandler(string channelId, uint uid, bool isFallbackOrRecover);

        /** Occurs when the connection state between the SDK and the server changes.
         *
         * @param channelId The name of the channel that you join.
         * @param state See #CONNECTION_STATE_TYPE.
         * @param reason See #CONNECTION_CHANGED_REASON_TYPE.
         */
        public delegate void ChannelOnConnectionStateChangedHandler(string channelId, CONNECTION_STATE_TYPE state, CONNECTION_CHANGED_REASON_TYPE reason);

        /** Occurs when the locally published media stream falls back to an audio-only stream due to poor network conditions or switches back to the video after the network conditions improve.
         *
         * If you call {@link agora_gaming_rtc.IRtcEngine.SetLocalPublishFallbackOption SetLocalPublishFallbackOption} and set `option` as {@link agora_gaming_rtc.STREAM_FALLBACK_OPTIONS#STREAM_FALLBACK_OPTION_AUDIO_ONLY STREAM_FALLBACK_OPTION_AUDIO_ONLY(2)}, the SDK triggers this callback when the locally published stream falls back to audio-only mode due to poor uplink conditions, or when the audio stream switches back to the video after the uplink network condition improves.
         *
         * @param channelId The name of the channel that you join.
         * @param isFallbackOrRecover Whether the locally published stream falls back to audio-only or switches back to the video:
         * - true: The locally published stream falls back to audio-only due to poor network conditions.
         * - false: The locally published stream switches back to the video after the network conditions improve.
         */
        public delegate void ChannelOnLocalPublishFallbackToAudioOnlyHandler(string channelId, bool isFallbackOrRecover);

        /** Reports events during the RTMP or RTMPS streaming.
         *
         * @since v3.2.0
         *
         * @param channelId The name of the channel that you join.
         * @param url The RTMP or RTMPS streaming URL.
         * @param eventCode The event code. See #RTMP_STREAMING_EVENT
         */
        public delegate void ChannelOnRtmpStreamingEventHandler(string channelId, string url, RTMP_STREAMING_EVENT eventCode);

        /** Occurs when the audio publishing state changes.
         *
         * @since v3.2.0
         *
         * This callback indicates the publishing state change of the local audio stream.
         *
         * @param channelId The name of the channel that you join.
         * @param oldState The previous publishing state. For details, see #STREAM_PUBLISH_STATE.
         * @param newState The current publishing state. For details, see #STREAM_PUBLISH_STATE.
         * @param elapseSinceLastState The time elapsed (ms) from the previous state to the current state.
         */
        public delegate void ChannelOnAudioPublishStateChangedHandler(string channelId, STREAM_PUBLISH_STATE oldState, STREAM_PUBLISH_STATE newState, int elapseSinceLastState);

        /** Occurs when the video publishing state changes.
         *
         * @since v3.2.0
         *
         * This callback indicates the publishing state change of the local video stream.
         *
         * @param channelId The name of the channel that you join.
         * @param oldState The previous publishing state. For details, see #STREAM_PUBLISH_STATE.
         * @param newState The current publishing state. For details, see #STREAM_PUBLISH_STATE.
         * @param elapseSinceLastState The time elapsed (ms) from the previous state to the current state.
         */
        public delegate void ChannelOnVideoPublishStateChangedHandler(string channelId, STREAM_PUBLISH_STATE oldState, STREAM_PUBLISH_STATE newState, int elapseSinceLastState);

        /** Occurs when the audio subscribing state changes.
         *
         * @since v3.2.0
         *
         * This callback indicates the subscribing state change of a remote audio stream.
         *
         * @param channelId The name of the channel that you join.
         * @param uid The ID of the remote user.
         * @param oldState The previous subscribing state. For details, see #STREAM_SUBSCRIBE_STATE.
         * @param newState The current subscribing state. For details, see #STREAM_SUBSCRIBE_STATE.
         * @param elapseSinceLastState The time elapsed (ms) from the previous state to the current state.
         */
        public delegate void ChannelOnAudioSubscribeStateChangedHandler(string channelId, uint uid, STREAM_SUBSCRIBE_STATE oldState, STREAM_SUBSCRIBE_STATE newState, int elapseSinceLastState);

        /** Occurs when the audio subscribing state changes.
         *
         * @since v3.2.0
         *
         * This callback indicates the subscribing state change of a remote video stream.
         *
         * @param channelId The name of the channel that you join.
         * @param uid The ID of the remote user.
         * @param oldState The previous subscribing state. For details, see #STREAM_SUBSCRIBE_STATE.
         * @param newState The current subscribing state. For details, see #STREAM_SUBSCRIBE_STATE.
         * @param elapseSinceLastState The time elapsed (ms) from the previous state to the current state.
         */
        public delegate void ChannelOnVideoSubscribeStateChangedHandler(string channelId, uint uid, STREAM_SUBSCRIBE_STATE oldState, STREAM_SUBSCRIBE_STATE newState, int elapseSinceLastState);


        /** Reports whether the super resolution feature is successfully enabled. (beta feature)
         *
         * @since v3.6.1.1
         *
         * After calling {@link agora_gaming_rtc.AgoraChannel.EnableRemoteSuperResolution EnableRemoteSuperResolution}, the SDK triggers this
         * callback to report whether the super-resolution algorithm is successfully enabled. If not successfully enabled,
         * you can use `reason` for troubleshooting.
         *
         * @param channelId The name of the channel that you join.
         * @param uid The user ID of the remote user.
         * @param enabled Whether super resolution is successfully enabled:
         * - true: Super resolution is successfully enabled.
         * - false: Super resolution is not successfully enabled.
         * @param reason The reason why super resolution is not successfully enabled. See #SUPER_RESOLUTION_STATE_REASON.
         */
        public delegate void ChannelOnUserSuperResolutionEnabledHandler(string channelId, uint uid, bool enabled, SUPER_RESOLUTION_STATE_REASON reason);

        /** Occurs when the user role switch fails in the interactive live streaming.
         * @since 3.7.0
         *
         * In the `LIVE_BROADCASTING` channel profile, when the local user calls `SetClientRole`
         * to switch their user role after joining the channel but the switch fails, the SDK
         * triggers this callback to report the reason for the failure and the current user role.
         * @param channelId The channel name.
         * @param reason The reason for the user role switch failure. See #CLIENT_ROLE_CHANGE_FAILED_REASON.
         * @param currentRole The current user role. See #CLIENT_ROLE_TYPE.
         */
        public delegate void ChannelOnClientRoleChangeFailedHandler(string channelId, CLIENT_ROLE_CHANGE_FAILED_REASON reason, CLIENT_ROLE_TYPE currentRole);
        /**
         * Occurs when the first remote video frame is rendered.
         * 
         * The SDK triggers this callback when the first frame of the remote video is displayed in the user's video window.
         * The application can get the time elapsed from a user joining the channel until the first video frame is displayed.
         * @param channelId The name of the current channel.
         * @param uid User ID of the remote user sending the video stream.
         * @param width Width (px) of the video frame.
         * @param height Height (px) of the video stream.
         * @param elapsed Time elapsed (ms) from the local user calling `joinChannel` until the SDK triggers this callback.
         */
        public delegate void ChannelOnFirstRemoteVideoFrameHandler(string channelId, uint uid, int width, int height, int elapsed);
        /**
         * Reports the proxy connection state.
         * @since 3.7.0
         *
         * You can use this callback to listen for the state of the SDK connecting to a proxy. For example, when a user calls
         * {@link agora_gaming_rtc.IRtcEngine.SetCloudProxy SetCloudProxy} and joins a channel successfully, the SDK triggers
         * this callback to report the user ID, the proxy type connected, and the time elapsed from the user calling `joinChannel`
         * until this callback is triggered.
         * @param channelId The channel name.
         * @param uid The user ID.
         * @param proxyType The proxy type. See #PROXY_TYPE.
         * @param localProxyIp Reserved for future use.
         * @param elapsed The time elapsed (ms) from the user calling `joinChannel` until this callback is triggered.
         */
        public delegate void ChannelOnChannelProxyConnectedHandler(string channelId, uint uid, PROXY_TYPE proxyType, string localProxyIp, int elapsed);

}
