
using System;
using System.Runtime.InteropServices;

namespace agora_gaming_rtc
{
    #region some enum and struct types

    /** Video rendering mode. */
    public enum VIDEO_RENDER_MODE
    {
        /** 100: (Default) RawData.*/
        RENDER_RAWDATA = 100,
        /** 101: OpenGLES 2. */
        REDNER_OPENGL_ES2 = 101,
        /** 102: Unity low level interface. */
        RENDER_UNITY_LOW_LEVEL_INTERFACE = 102,
    };

    /** Error code, see more in [Error Code](./index.html#error).
    */
    public enum ERROR_CODE
    {
        /** -7: The SDK is not initialized before calling this method. */
        ERROR_NOT_INIT_ENGINE = -7,
        /** 0: No error occurs. */
        ERROR_OK = 0,
        /** -2: An invalid parameter is used. For example, the specific channel name includes illegal characters. */
        ERROR_INVALID_ARGUMENT = -2,
        /** -100: No device is plugged.*/
        ERROR_NO_DEVICE_PLUGIN = -100,
    };

    /** Remote video stream types. */
    public enum REMOTE_VIDEO_STREAM_TYPE
    {
        /** 0: High-stream video. */
        REMOTE_VIDEO_STREAM_HIGH = 0,
        /** 1: Low-stream video. */
        REMOTE_VIDEO_STREAM_LOW = 1,
    };

    /** The state of the remote video. */
    public enum REMOTE_VIDEO_STATE
    {
        /** 0: The remote video is in the default state, probably due to `REMOTE_VIDEO_STATE_REASON_LOCAL_MUTED(3)`, `REMOTE_VIDEO_STATE_REASON_REMOTE_MUTED(5)`, or `REMOTE_VIDEO_STATE_REASON_REMOTE_OFFLINE(7)`.
        */
        REMOTE_VIDEO_STATE_STOPPED = 0,

        /** 1: The first remote video packet is received.
        */
        REMOTE_VIDEO_STATE_STARTING = 1,

        /** 2: The remote video stream is decoded and plays normally, probably due to `REMOTE_VIDEO_STATE_REASON_NETWORK_RECOVERY(2)`, `REMOTE_VIDEO_STATE_REASON_LOCAL_UNMUTED(4)`, `REMOTE_VIDEO_STATE_REASON_REMOTE_UNMUTED(6)`, or `REMOTE_VIDEO_STATE_REASON_AUDIO_FALLBACK_RECOVERY(9)`.
        */
        REMOTE_VIDEO_STATE_DECODING = 2,

        /** 3: The remote video is frozen, probably due to `REMOTE_VIDEO_STATE_REASON_NETWORK_CONGESTION(1)` or `REMOTE_VIDEO_STATE_REASON_AUDIO_FALLBACK(8)`.
        */
        REMOTE_VIDEO_STATE_FROZEN = 3,

        /** 4: The remote video fails to start, probably due to `REMOTE_VIDEO_STATE_REASON_INTERNAL(0)`.
        */
        REMOTE_VIDEO_STATE_FAILED = 4
    };

    /** The reason of audio mixing state change.
    */
    public enum AUDIO_MIXING_REASON_TYPE {
        /** 701: The SDK cannot open the audio mixing file.
        */
        AUDIO_MIXING_REASON_CAN_NOT_OPEN = 701,
        /** 702: The SDK opens the audio mixing file too frequently.
        */
        AUDIO_MIXING_REASON_TOO_FREQUENT_CALL = 702,
        /** 703: The audio mixing file playback is interrupted.
        */
        AUDIO_MIXING_REASON_INTERRUPTED_EOF = 703,
        /** 720: The audio mixing is started by user.
        */
        AUDIO_MIXING_REASON_STARTED_BY_USER = 720,
        /** 721: The audio mixing file is played once.
        */
        AUDIO_MIXING_REASON_ONE_LOOP_COMPLETED = 721,
        /** 722: The audio mixing file is playing in a new loop.
        */
        AUDIO_MIXING_REASON_START_NEW_LOOP = 722,
        /** 723: The audio mixing file is all played out.
        */
        AUDIO_MIXING_REASON_ALL_LOOPS_COMPLETED = 723,
        /** 724: Playing of audio file is stopped by user.
        */
        AUDIO_MIXING_REASON_STOPPED_BY_USER = 724,
        /** 725: Playing of audio file is paused by user.
        */
        AUDIO_MIXING_REASON_PAUSED_BY_USER = 725,
        /** 726: Playing of audio file is resumed by user.
        */
        AUDIO_MIXING_REASON_RESUMED_BY_USER = 726,
    };

    /** Reasons for a user being offline. */
    public enum USER_OFFLINE_REASON
    {
        /** 0: The user quits the call. */
        QUIT = 0,
        /** 1: The SDK times out and the user drops offline because no data packet is received within a certain period of time. If the user quits the call and the message is not passed to the SDK (due to an unreliable channel), the SDK assumes the user dropped offline. */
        DROPPED = 1,
        /** 2: (Interactive live streaming only.) The client role switched from the host to the audience. */
        BECOME_AUDIENCE = 2,
    };
    /** Output log filter level. */
    public enum LOG_FILTER
    {
        /** 0: Do not output any log information. */
        OFF = 0,
        /** 0x80f: Output all log information. Set your log filter as debug if you want to get the most complete log file. */
        DEBUG = 0x80f,
        /** 0x0f: Output CRITICAL, ERROR, WARNING, and INFO level log information. We recommend setting your log filter as this level. */
        INFO = 0x0f,
        /** 0x0e: Outputs CRITICAL, ERROR, and WARNING level log information. */
        WARNING = 0x0e,
        /** 0x0c: Outputs CRITICAL and ERROR level log information. */
        ERROR = 0x0c,
        /** 0x08: Outputs CRITICAL level log information. */
        CRITICAL = 0x08,
    };

     /** The channel profile. */
    public enum CHANNEL_PROFILE
    {
        /** 0: (Default) Communication. This profile applies to scenarios such as an audio call or video call,
         * where all users can publish and subscribe to streams.
         */
        CHANNEL_PROFILE_COMMUNICATION = 0,
        /** 1: Live streaming. In this profile, uses have roles, namely, host and audience (default).
         * A host both publishes and subscribes to streams, while an audience subscribes to streams only.
         * This profile applies to scenarios such as a chat room or interactive video streaming.
         */
        CHANNEL_PROFILE_LIVE_BROADCASTING = 1,
        /** 2: Gaming. This profile uses a codec with a lower bitrate and consumes less power. Applies to the gaming scenario, where all game players can talk freely.
        *
        * @note Agora does not recommend using this setting.
        */
        CHANNEL_PROFILE_GAME = 2,
    };


    /** The role of a user in interactive live streaming. */
    public enum CLIENT_ROLE_TYPE
    {
        /** 1: Host. A host can both send and receive streams. */
        CLIENT_ROLE_BROADCASTER = 1,
        /** 2: (Default) Audience, the default role. An audience member can only receive streams. */
        CLIENT_ROLE_AUDIENCE = 2,
    };

    /** The audio recording quality, which is set in {@link agora_gaming_rtc.IRtcEngine.StartAudioRecording(AudioRecordingConfiguration config) StartAudioRecording}. */
    public enum AUDIO_RECORDING_QUALITY_TYPE
    {
        /** 0: Low quality. For example, the size of an AAC file with a sample rate of 32,000 Hz and a 10-minute recording is approximately 1.2 MB. */
        AUDIO_RECORDING_QUALITY_LOW = 0,
        /** 1: (Default) Medium quality. For example, the size of an AAC file with a sample rate of 32,000 Hz and a 10-minute recording is approximately 2 MB. */
        AUDIO_RECORDING_QUALITY_MEDIUM = 1,
        /** 2: High quality. For example, the size of an AAC file with a sample rate of 32,000 Hz and a 10-minute recording is approximately 3.75 MB. */
        AUDIO_RECORDING_QUALITY_HIGH = 2,
        /** 3: Ultra high quality. For example, the size of an AAC file with a sample rate
        * of 32,000 Hz and a 10-minute recording is approximately 7.5 MB.
        */
        AUDIO_RECORDING_QUALITY_ULTRA_HIGH = 3,
    };
    /** Audio output routing. */
    public enum AUDIO_ROUTE
    {
        /** Default.
        */
        AUDIO_ROUTE_DEFAULT = -1,
        /** Headset.
        */
        AUDIO_ROUTE_HEADSET = 0,
        /** Earpiece.
        */
        AUDIO_ROUTE_EARPIECE = 1,
        /** Headset with no microphone.
        */
        AUDIO_ROUTE_HEADSET_NO_MIC = 2,
        /** Speakerphone.
        */
        AUDIO_ROUTE_SPEAKERPHONE = 3,
        /** Loudspeaker.
        */
        AUDIO_ROUTE_LOUDSPEAKER = 4,
        /** Bluetooth headset.
        */
        AUDIO_ROUTE_BLUETOOTH = 5,
        /** USB peripheral (macOS only).
        */
        AUDIO_ROUTE_USB = 6,
        /** HDMI peripheral (macOS only).
        */
        AUDIO_ROUTE_HDMI = 7,
        /** DisplayPort peripheral (macOS only).
        */
        AUDIO_ROUTE_DISPLAYPORT = 8,
        /** Apple AirPlay (macOS only).
        */
        AUDIO_ROUTE_AIRPLAY = 9
    };

    /** Connection states. */
    public enum CONNECTION_STATE_TYPE
    {
        /** 1: The SDK is disconnected from Agora's edge server.
         * - This is the initial state before calling the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method.
         * - The SDK also enters this state when the application calls the {@link agora_gaming_rtc.IRtcEngine.LeaveChannel LeaveChannel} method.
         */
        CONNECTION_STATE_DISCONNECTED = 1,

        /** 2: The SDK is connecting to Agora's edge server.
         * - When the application calls the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method, the SDK starts to establish a connection to the specified channel, triggers the {@link agora_gaming_rtc.OnConnectionStateChangedHandler OnConnectionStateChangedHandler} callback, and switches to the `CONNECTION_STATE_CONNECTING(2)` state.
         * - When the SDK successfully joins the channel, it triggers the `OnConnectionStateChangedHandler` callback and switches to the `CONNECTION_STATE_CONNECTED(3)` state.
         * - After the SDK joins the channel and when it finishes initializing the media engine, the SDK triggers the {@link agora_gaming_rtc.OnJoinChannelSuccessHandler OnJoinChannelSuccessHandler} callback.
         */
        CONNECTION_STATE_CONNECTING = 2,

         /** 3: The SDK is connected to Agora's edge server and has joined a channel. You can now publish or subscribe to a media stream in the channel.
         * If the connection to the channel is lost because, for example, if the network is down or switched, the SDK automatically tries to reconnect and triggers:
         * - The {@link agora_gaming_rtc.OnConnectionInterruptedHandler OnConnectionInterruptedHandler} callback (deprecated).
         * - The {@link agora_gaming_rtc.OnConnectionStateChangedHandler OnConnectionStateChangedHandler} callback and switches to the `CONNECTION_STATE_RECONNECTING(4)` state.
         */
        CONNECTION_STATE_CONNECTED = 3,

         /** 4: The SDK keeps rejoining the channel after being disconnected from a joined channel because of network issues.
         * - If the SDK cannot rejoin the channel within 10 seconds after being disconnected from Agora's edge server, the SDK triggers the {@link agora_gaming_rtc.OnConnectionLostHandler OnConnectionLostHandler} callback, stays in the `CONNECTION_STATE_RECONNECTING(4)` state, and keeps rejoining the channel.
         * - If the SDK fails to rejoin the channel 20 minutes after being disconnected from Agora's edge server, the SDK triggers the {@link agora_gaming_rtc.OnConnectionStateChangedHandler OnConnectionStateChangedHandler} callback, switches to the `CONNECTION_STATE_FAILED(5)` state, and stops rejoining the channel.
         */
        CONNECTION_STATE_RECONNECTING = 4,

        /** 5: The SDK fails to connect to Agora's edge server or join the channel.
         * You must call the {@link agora_gaming_rtc.IRtcEngine.LeaveChannel LeaveChannel} method to leave this state, and call the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method again to rejoin the channel.
         * If the SDK is banned from joining the channel by Agora's edge server (through the RESTful API), the SDK triggers the {@link agora_gaming_rtc.OnConnectionBannedHandler OnConnectionBannedHandler} (deprecated) and {@link agora_gaming_rtc.OnConnectionStateChangedHandler OnConnectionStateChangedHandler} callbacks.
         */
        CONNECTION_STATE_FAILED = 5
    };

    /** Reasons for a connection state change. */
    public enum CONNECTION_CHANGED_REASON_TYPE
    {
        /** 0: The SDK is connecting to Agora's edge server. */
        CONNECTION_CHANGED_CONNECTING = 0,
        /** 1: The SDK has joined the channel successfully. */
        CONNECTION_CHANGED_JOIN_SUCCESS = 1,
        /** 2: The connection between the SDK and Agora's edge server is interrupted. */
        CONNECTION_CHANGED_INTERRUPTED = 2,
        /** 3: The user is banned by the server. This error occurs when the user is kicked out the channel from the server. */
        CONNECTION_CHANGED_BANNED_BY_SERVER = 3,
        /** 4: The SDK fails to join the channel for more than 20 minutes and stops reconnecting to the channel. */
        CONNECTION_CHANGED_JOIN_FAILED = 4,
        /** 5: The SDK has left the channel. */
        CONNECTION_CHANGED_LEAVE_CHANNEL = 5,
        /** 6: The connection failed since Appid is not valid. */
        CONNECTION_CHANGED_INVALID_APP_ID = 6,
        /** 7: The connection failed since channel name is not valid. */
        CONNECTION_CHANGED_INVALID_CHANNEL_NAME = 7,
        /** 8: The connection failed since token is not valid, possibly because:
         * - The App Certificate for the project is enabled in Dashboard, but you do not use Token when joining the channel. If you enable the App Certificate, you must use a token to join the channel.
         * - The `uid` that you specify in the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method is different from the `uid` that you pass for generating the token.
         */
        CONNECTION_CHANGED_INVALID_TOKEN = 8,
        /** 9: The connection failed since token is expired. */
        CONNECTION_CHANGED_TOKEN_EXPIRED = 9,
        /** 10: The connection is rejected by server. This error usually occurs in the following situations:
         * - When the user is already in the channel, and still calls the method to join the channel, for example,
         * {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey}.
         * - When the user tries to join a channel during {@link agora_gaming_rtc.IRtcEngine.StartEchoTest(int intervalInSeconds) StartEchoTest}. Once you
         * call `StartEchoTest`, you need to call {@link agora_gaming_rtc.IRtcEngine.StopEchoTest StopEchoTest} before joining a channel.
         */
        CONNECTION_CHANGED_REJECTED_BY_SERVER = 10,
        /** 11: The connection changed to reconnecting since SDK has set a proxy server. */
        CONNECTION_CHANGED_SETTING_PROXY_SERVER = 11,
        /** 12: When SDK is in connection failed, the renew token operation will make it connecting. */
        CONNECTION_CHANGED_RENEW_TOKEN = 12,
        /** 13: The IP Address of SDK client has changed. i.e., Network type or IP/Port changed by network operator might change client IP address. */
        CONNECTION_CHANGED_CLIENT_IP_ADDRESS_CHANGED = 13,
        /** 14: Timeout for the keep-alive of the connection between the SDK and Agora's edge server. The connection state changes to `CONNECTION_STATE_RECONNECTING(4)`. */
        CONNECTION_CHANGED_KEEP_ALIVE_TIMEOUT = 14,
        /** 19: Join the same channel from different devices using the same user ID. */
        CONNECTION_CHANGED_SAME_UID_LOGIN = 19,
        /** 20: The number of hosts in the channel is already at the upper limit.
         * @note This enumerator is reported only when the support for 128 users is enabled. The maximum number of hosts is based on the actual number of hosts configured when you enable the 128-user feature.
         */
        CONNECTION_CHANGED_TOO_MANY_BROADCASTERS = 20,
    };

    /** Stream fallback options. */
    public enum STREAM_FALLBACK_OPTIONS
    {
        /** 0: No fallback behavior for the local/remote video stream when the uplink/downlink network conditions are poor. The quality of the stream is not guaranteed. */
        STREAM_FALLBACK_OPTION_DISABLED = 0,
        /** 1: Under poor downlink network conditions, the remote video stream, to which you subscribe, falls back to the low-stream (low resolution and low bitrate) video. You can set this option only in the {@link agora_gaming_rtc.IRtcEngine.SetRemoteSubscribeFallbackOption SetRemoteSubscribeFallbackOption} method. Nothing happens when you set this in the {@link agora_gaming_rtc.IRtcEngine.SetLocalPublishFallbackOption SetLocalPublishFallbackOption} method. */
        STREAM_FALLBACK_OPTION_VIDEO_STREAM_LOW = 1,
        /** 2: Under poor uplink network conditions, the locally published video stream falls back to audio only.
         * Under poor downlink network conditions, the remote video stream, to which you subscribe, first falls back to the low-stream (low resolution and low bitrate) video; and then to an audio-only stream if the network conditions worsen.
         */
        STREAM_FALLBACK_OPTION_AUDIO_ONLY = 2,
    };

    /** Content hints for screen sharing.
    */
    public enum VideoContentHint
    {
        /** 0: (Default) No content hint.
        */
        CONTENT_HINT_NONE = 0,
        /** 1: Motion-intensive content. Choose this option if you prefer smoothness or when you are sharing a video clip, movie, or video game.
        */
        CONTENT_HINT_MOTION = 1,
        /** 2: Motionless content. Choose this option if you prefer sharpness or when you are sharing a picture, PowerPoint slide, or text.
        */
        CONTENT_HINT_DETAILS = 2,
    };


    /** The reason of the remote video state change. */
    public enum REMOTE_VIDEO_STATE_REASON
    {
        /** 0: The SDK reports this reason when the video state changes.
        */
        REMOTE_VIDEO_STATE_REASON_INTERNAL = 0,

        /** 1: Network congestion.
        */
        REMOTE_VIDEO_STATE_REASON_NETWORK_CONGESTION = 1,

        /** 2: Network recovery.
        */
        REMOTE_VIDEO_STATE_REASON_NETWORK_RECOVERY = 2,

        /** 3: The local user stops receiving the remote video stream or disables the video module.
        */
        REMOTE_VIDEO_STATE_REASON_LOCAL_MUTED = 3,

        /** 4: The local user resumes receiving the remote video stream or enables the video module.
        */
        REMOTE_VIDEO_STATE_REASON_LOCAL_UNMUTED = 4,

        /** 5: The remote user stops sending the video stream or disables the video module.
        */
        REMOTE_VIDEO_STATE_REASON_REMOTE_MUTED = 5,

        /** 6: The remote user resumes sending the video stream or enables the video module.
        */
        REMOTE_VIDEO_STATE_REASON_REMOTE_UNMUTED = 6,

        /** 7: The remote user leaves the channel.
        */
        REMOTE_VIDEO_STATE_REASON_REMOTE_OFFLINE = 7,

        /** 8: The remote audio-and-video stream falls back to the audio-only stream due to poor network conditions.
        */
        REMOTE_VIDEO_STATE_REASON_AUDIO_FALLBACK = 8,

        /** 9: The remote audio-only stream switches back to the video stream after the network conditions improve.
        */
        REMOTE_VIDEO_STATE_REASON_AUDIO_FALLBACK_RECOVERY = 9,

        /** 10: The remote user sdk(only for iOS) in background.
        */
        REMOTE_VIDEO_STATE_REASON_SDK_IN_BACKGROUND = 10

    };

    /** Local video state types.
    */
    public enum LOCAL_VIDEO_STREAM_STATE
    {
        /** 0: Initial state. */
        LOCAL_VIDEO_STREAM_STATE_STOPPED = 0,
        /** 1: The local video capturing device starts successfully.
         * The SDK also reports this state when you share a maximized window by calling {@link agora_gaming_rtc.IRtcEngine.StartScreenCaptureByWindowId StartScreenCaptureByWindowId}.
         */
        LOCAL_VIDEO_STREAM_STATE_CAPTURING = 1,
        /** 2: The first video frame is successfully encoded. */
        LOCAL_VIDEO_STREAM_STATE_ENCODING = 2,
        /** 3: The local video fails to start. */
        LOCAL_VIDEO_STREAM_STATE_FAILED = 3
    };

    /** Local video state error codes
    */
    public enum LOCAL_VIDEO_STREAM_ERROR {
        /** 0: The local video is normal. */
        LOCAL_VIDEO_STREAM_ERROR_OK = 0,
        /** 1: No specified reason for the local video failure. */
        LOCAL_VIDEO_STREAM_ERROR_FAILURE = 1,
        /** 2: No permission to use the local video capturing device. */
        LOCAL_VIDEO_STREAM_ERROR_DEVICE_NO_PERMISSION = 2,
        /** 3: The local video capturing device is in use. */
        LOCAL_VIDEO_STREAM_ERROR_DEVICE_BUSY = 3,
        /** 4: The local video capture fails. Check whether the capturing device is working properly. */
        LOCAL_VIDEO_STREAM_ERROR_CAPTURE_FAILURE = 4,
        /** 5: The local video encoding fails. */
        LOCAL_VIDEO_STREAM_ERROR_ENCODE_FAILURE = 5,
        /** 6: (iOS only) The application is in the background.
        *
        * @since v3.3.0
        */
        LOCAL_VIDEO_STREAM_ERROR_CAPTURE_INBACKGROUND = 6,
        /** 7: (iOS only) The application is running in Slide Over, Split View, or Picture in Picture mode.
        *
        * @since v3.3.0
        */
        LOCAL_VIDEO_STREAM_ERROR_CAPTURE_MULTIPLE_FOREGROUND_APPS = 7,
        /** 8:capture not found*/
        LOCAL_VIDEO_STREAM_ERROR_DEVICE_NOT_FOUND = 8,
        /**
        * 9: (macOS only) The external camera currently in use is disconnected
        * (such as being unplugged).
        *
        * @since v3.5.0
        */
        LOCAL_VIDEO_STREAM_ERROR_DEVICE_DISCONNECTED = 9,
        /**
        * 10: (macOS and Windows only) The SDK cannot find the video device in the video device list. Check whether the ID of the video device is valid.
        *
        * @since v3.6.1.1
        */
        LOCAL_VIDEO_STREAM_ERROR_DEVICE_INVALID_ID = 10,
          /**
         * 11: The shared window is minimized when you call
         * {@link agora_gaming_rtc.IRtcEngine.StartScreenCaptureByWindowId StartScreenCaptureByWindowId}
         * to share a window.
         */
        LOCAL_VIDEO_STREAM_ERROR_SCREEN_CAPTURE_WINDOW_MINIMIZED = 11,
        /** 12: The error code indicates that a window shared by the window ID has been closed, or a full-screen window
        * shared by the window ID has exited full-screen mode.
        * After exiting full-screen mode, remote users cannot see the shared window. To prevent remote users from seeing a
        * black screen, Agora recommends that you immediately stop screen sharing.
        *
        * Common scenarios for reporting this error code:
        * - When the local user closes the shared window, the SDK reports this error code.
        * - The local user shows some slides in full-screen mode first, and then shares the windows of the slides. After
        * the user exits full-screen mode, the SDK reports this error code.
        * - The local user watches web video or reads web document in full-screen mode first, and then shares the window of
        * the web video or document. After the user exits full-screen mode, the SDK reports this error code.
        */
        LOCAL_VIDEO_STREAM_ERROR_SCREEN_CAPTURE_WINDOW_CLOSED = 12,
        /**
        * 13: (Windows only) The window being shared is overlapped by another window, so the overlapped area is blacked out by the SDK during window sharing.
        *
        * @since v3.6.1.1
        */
        LOCAL_VIDEO_STREAM_ERROR_SCREEN_CAPTURE_WINDOW_OCCLUDED = 13,
        /**
        * 20: (Windows only) The SDK does not support sharing this type of window.
        *
        * @since v3.6.1.1
        */
        LOCAL_VIDEO_STREAM_ERROR_SCREEN_CAPTURE_WINDOW_NOT_SUPPORTED = 20,
    };

    /** The reording content, set in {@link agora_gaming_rtc.IRtcEngine.StartAudioRecording(AudioRecordingConfiguration config) StartAudioRecording}. */
    public enum AUDIO_RECORDING_POSITION {
        /** 0: (Default) Records the mixed audio of the local user and all remote users. */
        AUDIO_RECORDING_POSITION_MIXED_RECORDING_AND_PLAYBACK = 0,
        /** 1: Records the audio of the local user only. */
        AUDIO_RECORDING_POSITION_RECORDING = 1,
        /** 2: Records the audio of all remote users only. */
        AUDIO_RECORDING_POSITION_MIXED_PLAYBACK = 2,
    };

    /** Media device types. */
    public enum MEDIA_DEVICE_TYPE
    {
        /** -1: Unknown device type. */
        UNKNOWN_AUDIO_DEVICE = -1,
        /** 0: Audio playback device. */
        AUDIO_PLAYOUT_DEVICE = 0,
        /** 1: Audio capturing device. */
        AUDIO_RECORDING_DEVICE = 1,
        /** 2: Video renderer. */
        VIDEO_RENDER_DEVICE = 2,
        /** 3: Video capturer. */
        VIDEO_CAPTURE_DEVICE = 3,
        /** 4: Application audio playback device. */
        AUDIO_APPLICATION_PLAYOUT_DEVICE = 4,
    };

    /** Use modes of the {@link agora_gaming_rtc.AudioRawDataManager.OnRecordAudioFrameHandler OnRecordAudioFrameHandler} and {@link agora_gaming_rtc.AudioRawDataManager.OnPlaybackAudioFrameHandler OnPlaybackAudioFrameHandler} callbacks. */
    public enum RAW_AUDIO_FRAME_OP_MODE_TYPE
    {
        /** 0: Read-only mode: Users only read the AudioFrame data without modifying anything. For example, when users acquire the data with the Agora RTC SDK, then push the RTMP or RTMPS streams. */
        RAW_AUDIO_FRAME_OP_MODE_READ_ONLY = 0,
        /** 1: Write-only mode: Users replace the AudioFrame data with their own data and pass the data to the SDK for encoding. For example, when users acquire the data. */
        RAW_AUDIO_FRAME_OP_MODE_WRITE_ONLY = 1,
        /** 2: Read and write mode: Users read the data from AudioFrame, modify it, and then play it. For example, when users have their own sound-effect processing module and perform some voice pre-processing, such as a voice change. */
        RAW_AUDIO_FRAME_OP_MODE_READ_WRITE = 2,
    };

    /** Audio profiles.
     *
     * Sets the sample rate, bitrate, encoding mode, and the number of channels.
     */
    public enum AUDIO_PROFILE_TYPE // sample rate, bit rate, mono/stereo, speech/music codec
    {
        /** 0: Default audio profile.
         * - For the `LIVE_BROADCASTING` profile: A sample rate of 48 KHz, music encoding, mono, and a bitrate of up to 64 Kbps.
         * - For the `COMMUNICATION` profile:
         *   - Windows: A sample rate of 16 KHz, music encoding, mono, and a bitrate of up to 16 Kbps.
         *   - Android/iOS/macOS: A sample rate of 32 KHz, music encoding, mono, and a bitrate of up to 18 Kbps.
         */
        AUDIO_PROFILE_DEFAULT = 0,
        /** 1: A sample rate of 32 KHz, audio encoding, mono, and a bitrate of up to 18 Kbps. */
        AUDIO_PROFILE_SPEECH_STANDARD = 1,
        /** 2: A sample rate of 48 kHz, music encoding, mono, and a bitrate of up to 64 Kbps. */
        AUDIO_PROFILE_MUSIC_STANDARD = 2,
        /** 3: A sample rate of 48 kHz, music encoding, stereo, and a bitrate of up to 80 Kbps. */
        AUDIO_PROFILE_MUSIC_STANDARD_STEREO = 3,
        /** 4: A sample rate of 48 kHz, music encoding, mono, and a bitrate of up to 96 Kbps. */
        AUDIO_PROFILE_MUSIC_HIGH_QUALITY = 4,
        /** 5: A sample rate of 48 kHz, music encoding, stereo, and a bitrate of up to 128 Kbps. */
        AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO = 5,
        /** 6: A sample rate of 16 kHz, audio encoding, mono, and Acoustic Echo Cancellation (AES) enabled.  */
        AUDIO_PROFILE_IOT = 6,
        /** The number of elements in the enumeration. */
        AUDIO_PROFILE_NUM = 7,
    };

    /** Audio application scenarios.*/
    public enum AUDIO_SCENARIO_TYPE // set a suitable scenario for your app type
    {
        /** 0: Default audio scenario. */
        AUDIO_SCENARIO_DEFAULT = 0,
        /** 1: Entertainment scenario, supporting voice during gameplay. */
        AUDIO_SCENARIO_CHATROOM_ENTERTAINMENT = 1,
        /** 2: Education scenario where users need to frequently switch the user role. */
        AUDIO_SCENARIO_EDUCATION = 2,
        /** 3: High-quality audio chatroom scenario where hosts mainly play music. */
        AUDIO_SCENARIO_GAME_STREAMING = 3,
        /** 4: Showroom scenario where a single host wants high-quality audio. */
        AUDIO_SCENARIO_SHOWROOM = 4,
        /** 5: Gaming scenario for group chat that only contains the human voice. */
        AUDIO_SCENARIO_CHATROOM_GAMING = 5,
        /** 6: IoT (Internet of Things) scenario where users use IoT devices with low power consumption. */
        AUDIO_SCENARIO_IOT = 6,

        /** 8: Meeting scenario that mainly contains the human voice.
         *
         * @since v3.2.0
         */
        AUDIO_SCENARIO_MEETING = 8,
        /** The number of elements in the enumeration. */
        AUDIO_SCENARIO_NUM = 10,
    };

    /** Video codec profile types. */
    public enum VIDEO_CODEC_PROFILE_TYPE
    {
        /** 66: Baseline video codec profile. Generally used in video calls on mobile phones. */
        VIDEO_CODEC_PROFILE_BASELINE = 66,
        /** 77: Main video codec profile. Generally used in mainstream electronics such as MP4 players, portable video players, PSP, and iPads. */
        VIDEO_CODEC_PROFILE_MAIN = 77,
        /**  100: (Default) High video codec profile. Generally used in high-resolution broadcasts or television. */
        VIDEO_CODEC_PROFILE_HIGH = 100,
    };

    /** Audio-sample rates. */
    public enum AUDIO_SAMPLE_RATE_TYPE
    {
        /** 32000: 32 kHz */
        AUDIO_SAMPLE_RATE_32000 = 32000,
        /** 44100: 44.1 kHz */
        AUDIO_SAMPLE_RATE_44100 = 44100,
        /** 48000: 48 kHz */
        AUDIO_SAMPLE_RATE_48000 = 48000,
    };

    /** The states of the local user's audio mixing file. */
    public enum AUDIO_MIXING_STATE_TYPE
    {
        /** 710: The audio mixing file is playing.
         * This state indicates that the SDK is in the following stages:
         * - Call {@link agora_gaming_rtc.IRtcEngine.StartAudioMixing StartAudioMixing} successfully.
         * - Call {@link agora_gaming_rtc.IRtcEngine.ResumeAudioMixing ResumeAudioMixing} successfully.
         */
        AUDIO_MIXING_STATE_PLAYING = 710,
        /** 711: The audio mixing file pauses playing.
         * This state indicates that the SDK calls {@link agora_gaming_rtc.IRtcEngine.PauseAudioMixing PauseAudioMixing} successfully.
         */
        AUDIO_MIXING_STATE_PAUSED = 711,
        /** 713: The audio mixing file stops playing.
         * This state indicates that the SDK calls {@link agora_gaming_rtc.IRtcEngine.StopAudioMixing StopAudioMixing} successfully.
         */
        AUDIO_MIXING_STATE_STOPPED = 713,
        /** 714: An exception occurs during the playback of the audio mixing file. See error types in the `errorCode` for details.
        */
        AUDIO_MIXING_STATE_FAILED = 714,
    };

    /** The error codes of the local user's audio mixing file.
     *
     * @deprecated Deprecated from v3.4.2, Use #AUDIO_MIXING_REASON_TYPE
     * instead.
     */
    public enum AUDIO_MIXING_ERROR_TYPE
    {
        /** 701: The SDK cannot open the audio mixing file.
        */
        AUDIO_MIXING_ERROR_CAN_NOT_OPEN = 701,
        /** 702: The SDK opens the audio mixing file too frequently.
        */
        AUDIO_MIXING_ERROR_TOO_FREQUENT_CALL = 702,
        /** 703: The opening of the audio mixing file is interrupted.
        */
        AUDIO_MIXING_ERROR_INTERRUPTED_EOF = 703,
        /** 0: The SDK can open the audio mixing file.
        */
        AUDIO_MIXING_ERROR_OK = 0,
    };

    /** States of the RTMP or RTMPS streaming.
    */
    public enum RTMP_STREAM_PUBLISH_STATE
    {
        /** 0: The RTMP or RTMPS streaming has not started or has ended.
        */
        RTMP_STREAM_PUBLISH_STATE_IDLE = 0,
        /** 1: The SDK is connecting to Agora's streaming server and the RTMP server. This state is triggered after you call the {@link agora_gaming_rtc.IRtcEngine.AddPublishStreamUrl AddPublishStreamUrl} method.
        */
        RTMP_STREAM_PUBLISH_STATE_CONNECTING = 1,
        /** 2: The RTMP or RTMPS streaming publishes. The SDK successfully publishes the RTMP or RTMPS streaming and returns this state.
        */
        RTMP_STREAM_PUBLISH_STATE_RUNNING = 2,
        /** 3: The RTMP or RTMPS streaming is recovering. When exceptions occur to the CDN, or the streaming is interrupted, the SDK tries to resume RTMP or RTMPS streaming and returns this state.
        * - If the SDK successfully resumes the streaming, `RTMP_STREAM_PUBLISH_STATE_RUNNING(2)` returns.
        * - If the streaming does not resume within 60 seconds or server errors occur, `RTMP_STREAM_PUBLISH_STATE_FAILURE(4)` returns. You can also reconnect to the server by calling the {@link agora_gaming_rtc.IRtcEngine.RemovePublishStreamUrl RemovePublishStreamUrl} and {@link agora_gaming_rtc.IRtcEngine.AddPublishStreamUrl AddPublishStreamUrl} methods.
        */
        RTMP_STREAM_PUBLISH_STATE_RECOVERING = 3,
        /** 4: The RTMP or RTMPS streaming fails. See the errCode parameter for the detailed error information. You can also call the {@link agora_gaming_rtc.IRtcEngine.AddPublishStreamUrl AddPublishStreamUrl} method to publish the RTMP or RTMPS streaming again.
        */
        RTMP_STREAM_PUBLISH_STATE_FAILURE = 4,
        /**
        * 5: The SDK is disconnecting from the Agora streaming server and CDN. When you call `Remove` or `Stop` to stop the streaming normally, the SDK reports the streaming state as `DISCONNECTING`, `IDLE` in sequence.
        *
        * @since v3.6.1.1
        */
        RTMP_STREAM_PUBLISH_STATE_DISCONNECTING = 5,
    };

    /** Error codes of the RTMP or RTMPS streaming.
    */
    public enum RTMP_STREAM_PUBLISH_ERROR_TYPE
    {
        /** The RTMP or RTMPS streaming publishes successfully. */
        RTMP_STREAM_PUBLISH_ERROR_OK = 0,
        /** Invalid argument used. If, for example, you do not call the `SetLiveTranscoding` method to configure the LiveTranscoding parameters before calling the addPublishStreamUrl method, the SDK returns this error. Check whether you set the parameters in the *setLiveTranscoding* method properly. */
        RTMP_STREAM_PUBLISH_ERROR_INVALID_ARGUMENT = 1,
        /** The RTMP or RTMPS streaming is encrypted and cannot be published. */
        RTMP_STREAM_PUBLISH_ERROR_ENCRYPTED_STREAM_NOT_ALLOWED = 2,
        /** Timeout for the RTMP or RTMPS streaming. Call the `AddPublishStreamUrl` method to publish the streaming again. */
        RTMP_STREAM_PUBLISH_ERROR_CONNECTION_TIMEOUT = 3,
        /** An error occurs in Agora's streaming server. Call the `AddPublishStreamUrl` method to publish the streaming again. */
        RTMP_STREAM_PUBLISH_ERROR_INTERNAL_SERVER_ERROR = 4,
        /** An error occurs in the CDN server. */
        RTMP_STREAM_PUBLISH_ERROR_RTMP_SERVER_ERROR = 5,
        /** The RTMP or RTMPS streaming publishes too frequently. */
        RTMP_STREAM_PUBLISH_ERROR_TOO_OFTEN = 6,
        /** The host publishes more than 10 URLs. Delete the unnecessary URLs before adding new ones. */
        RTMP_STREAM_PUBLISH_ERROR_REACH_LIMIT = 7,
        /** The host manipulates other hosts' URLs. Check your app logic. */
        RTMP_STREAM_PUBLISH_ERROR_NOT_AUTHORIZED = 8,
        /** Agora's server fails to find the RTMP or RTMPS streaming. */
        RTMP_STREAM_PUBLISH_ERROR_STREAM_NOT_FOUND = 9,
        /** The format of the RTMP or RTMPS streaming URL is not supported. Check whether the URL format is correct. */
        RTMP_STREAM_PUBLISH_ERROR_FORMAT_NOT_SUPPORTED = 10,
        /**
        * 11: The user role is not host, so the user cannot use the CDN live streaming function. Check your application code logic.
        *
        * @since v3.6.1.1
        */
        RTMP_STREAM_PUBLISH_ERROR_NOT_BROADCASTER = 11,
        /**
        * 13: {@link agora_gaming_rtc.IRtcEngine.UpdateRtmpTranscoding UpdateRtmpTranscoding} or {@link agora_gaming_rtc.IRtcEngine.SetLiveTranscoding SetLiveTranscoding} method is called to update the transcoding configuration in a scenario where there is streaming without transcoding. Check your application code logic.
        *
        * @since v3.6.1.1
        */
        RTMP_STREAM_PUBLISH_ERROR_TRANSCODING_NO_MIX_STREAM = 13,
        /**
        * 14: Errors occurred in the host's network.
        *
        * @since v3.6.1.1
        */
        RTMP_STREAM_PUBLISH_ERROR_NET_DOWN = 14,
        /**
        * 15: Your App ID does not have permission to use the CDN live streaming function. Refer to [Prerequisites](https://docs.agora.io/en/Interactive%20Broadcast/cdn_streaming_unity?platform=Unity#prerequisites) to enable the CDN live streaming permission.
        *
        * @since v3.6.1.1
        */
        RTMP_STREAM_PUBLISH_ERROR_INVALID_APPID = 15,
        /**
        * 100: The streaming has been stopped normally. After you call
        * {@link agora_gaming_rtc.IRtcEngine.RemovePublishStreamUrl RemovePublishStreamUrl}
        * to stop streaming, the SDK returns this value.
        *
        * @since v3.4.5
        */
        RTMP_STREAM_UNPUBLISH_ERROR_OK = 100,
    };

    /** Network type. */
    public enum NETWORK_TYPE
    {
        /** -1: The network type is unknown. */
        NETWORK_TYPE_UNKNOWN = -1,
        /** 0: The SDK disconnects from the network. */
        NETWORK_TYPE_DISCONNECTED = 0,
        /** 1: The network type is LAN. */
        NETWORK_TYPE_LAN = 1,
        /** 2: The network type is Wi-Fi. */
        NETWORK_TYPE_WIFI = 2,
        /** 3: The network type is mobile 2G. */
        NETWORK_TYPE_MOBILE_2G = 3,
        /** 4: The network type is mobile 3G. */
        NETWORK_TYPE_MOBILE_3G = 4,
        /** 5: The network type is mobile 4G. */
        NETWORK_TYPE_MOBILE_4G = 5,
        /**
         * 6: The network type is mobile 5G.
         *
         * @since v3.6.1.1
         */
        NETWORK_TYPE_MOBILE_5G = 6,
    };

    /** Local voice beautifier options.
     * @deprecated Deprecated as of v3.2.0. Use #VOICE_BEAUTIFIER_PRESET instead.
     */
    public enum VOICE_CHANGER_PRESET
    {
        /**
        * The original voice (no local voice beautifier).
        */
        VOICE_CHANGER_OFF = 0x00000000, //Turn off the voice changer
        /**
        * The voice of an old man (for male-sounding voice only).
        */
        VOICE_CHANGER_OLDMAN = 0x00000001,
        /**
        * The voice of a little boy (for male-sounding voice only).
        */
        VOICE_CHANGER_BABYBOY = 0x00000002,
        /**
        * The voice of a little girl (for female-sounding voice only).
        */
        VOICE_CHANGER_BABYGIRL = 0x00000003,
        /**
        * The voice of Zhu Bajie, a character in Journey to the West who has a voice like that of a growling bear.
        */
        VOICE_CHANGER_ZHUBAJIE = 0x00000004,
        /**
        * The ethereal voice.
        */
        VOICE_CHANGER_ETHEREAL = 0x00000005,
        /**
        * The voice of Hulk.
        */
        VOICE_CHANGER_HULK = 0x00000006,
        /**
        * A more vigorous voice.
        */
        VOICE_BEAUTY_VIGOROUS = 0x00100001,//7,
        /**
        * A deeper voice.
        */
        VOICE_BEAUTY_DEEP = 0x00100002,
        /**
        * A mellower voice.
        */
        VOICE_BEAUTY_MELLOW = 0x00100003,
        /**
        * Falsetto.
        */
        VOICE_BEAUTY_FALSETTO = 0x00100004,
        /**
        * A fuller voice.
        */
        VOICE_BEAUTY_FULL = 0x00100005,
        /**
        * A clearer voice.
        */
        VOICE_BEAUTY_CLEAR = 0x00100006,
        /**
        * A more resounding voice.
        */
        VOICE_BEAUTY_RESOUNDING = 0x00100007,
        /**
        * A more ringing voice.
        */
        VOICE_BEAUTY_RINGING = 0x00100008,
        /**
        * A more spatially resonant voice.
        */
        VOICE_BEAUTY_SPACIAL = 0x00100009,
        /**
        * (For male only) A more magnetic voice. Do not use it when the speaker is a female; otherwise, voice distortion occurs.
        */
        GENERAL_BEAUTY_VOICE_MALE_MAGNETIC = 0x00200001,
        /**
        * (For female only) A fresher voice. Do not use it when the speaker is a male; otherwise, voice distortion occurs.
        */
        GENERAL_BEAUTY_VOICE_FEMALE_FRESH = 0x00200002,
        /**
        * (For female only) A more vital voice. Do not use it when the speaker is a male; otherwise, voice distortion occurs.
        */
        GENERAL_BEAUTY_VOICE_FEMALE_VITALITY = 0x00200003

    };

    /** Local voice reverberation presets.
     * @deprecated Deprecated as of v3.2.0. Use #AUDIO_EFFECT_PRESET instead.
     */
    public enum AUDIO_REVERB_PRESET
    {
        /**
        * Turn off local voice reverberation, that is, to use the original voice.
        */
        AUDIO_REVERB_OFF = 0x00000000, // Turn off audio reverb
        /**
        * The reverberation style typical of a KTV venue (enhanced).
        */
        AUDIO_REVERB_FX_KTV = 0x00100001,
        /**
        * The reverberation style typical of a concert hall (enhanced).
        */
        AUDIO_REVERB_FX_VOCAL_CONCERT = 0x00100002,
        /**
        * The reverberation style typical of an uncle's voice.
        */
        AUDIO_REVERB_FX_UNCLE = 0x00100003,
        /**
        * The reverberation style typical of a sister's voice.
        */
        AUDIO_REVERB_FX_SISTER = 0x00100004,
        /**
        * The reverberation style typical of a recording studio (enhanced).
        */
        AUDIO_REVERB_FX_STUDIO = 0x00100005,
        /**
        * The reverberation style typical of popular music (enhanced).
        */
        AUDIO_REVERB_FX_POPULAR = 0x00100006,
        /**
        * The reverberation style typical of R&B music (enhanced).
        */
        AUDIO_REVERB_FX_RNB = 0x00100007,
        /**
        * The reverberation style typical of the vintage phonograph.
        */
        AUDIO_REVERB_FX_PHONOGRAPH = 0x00100008,
        /**
        * The reverberation style typical of popular music.
        */
        AUDIO_REVERB_POPULAR = 0x00000001,
        /**
        * The reverberation style typical of R&B music.
        */
        AUDIO_REVERB_RNB = 0x00000002,
        /**
        * The reverberation style typical of rock music.
        */
        AUDIO_REVERB_ROCK = 0x00000003,
        /**
        * The reverberation style typical of hip-hop music.
        */
        AUDIO_REVERB_HIPHOP = 0x00000004,
        /**
        * The reverberation style typical of a concert hall.
        */
        AUDIO_REVERB_VOCAL_CONCERT = 0x00000005,
        /**
        * The reverberation style typical of a KTV venue.
        */
        AUDIO_REVERB_KTV = 0x00000006,
        /**
        * The reverberation style typical of a recording studio.
        */
        AUDIO_REVERB_STUDIO = 0x00000007,
        /**
        * The reverberation of the virtual stereo. The virtual stereo is an effect that renders the monophonic
        * audio as the stereo audio, so that all users in the channel can hear the stereo voice effect.
        * To achieve better virtual stereo reverberation, Agora recommends setting `profile` in `SetAudioProfile`
        * as `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)`.
        */
        AUDIO_VIRTUAL_STEREO = 0x00200001,
    };

    /** Audio equalization band frequencies. */
    public enum AUDIO_EQUALIZATION_BAND_FREQUENCY
    {
        /** 0: 31 Hz */
        AUDIO_EQUALIZATION_BAND_31 = 0,
        /** 1: 62 Hz */
        AUDIO_EQUALIZATION_BAND_62 = 1,
        /** 2: 125 Hz */
        AUDIO_EQUALIZATION_BAND_125 = 2,
        /** 3: 250 Hz */
        AUDIO_EQUALIZATION_BAND_250 = 3,
        /** 4: 500 Hz */
        AUDIO_EQUALIZATION_BAND_500 = 4,
        /** 5: 1 kHz */
        AUDIO_EQUALIZATION_BAND_1K = 5,
        /** 6: 2 kHz */
        AUDIO_EQUALIZATION_BAND_2K = 6,
        /** 7: 4 kHz */
        AUDIO_EQUALIZATION_BAND_4K = 7,
        /** 8: 8 kHz */
        AUDIO_EQUALIZATION_BAND_8K = 8,
        /** 9: 16 kHz */
        AUDIO_EQUALIZATION_BAND_16K = 9,
    };

    /** Quality change of the local video in terms of target frame rate and target bit rate since last count.
    */
    public enum QUALITY_ADAPT_INDICATION
    {
        /** The quality of the local video stays the same. */
        ADAPT_NONE = 0,
        /** The quality improves because the network bandwidth increases. */
        ADAPT_UP_BANDWIDTH = 1,
        /** The quality worsens because the network bandwidth decreases. */
        ADAPT_DOWN_BANDWIDTH = 2,
    };

    /** Audio reverberation types. */
    public enum AUDIO_REVERB_TYPE
    {
        /** 0: The level of the dry signal (db). The value is between -20 and 10. */
        AUDIO_REVERB_DRY_LEVEL = 0,
        /** 1: The level of the early reflection signal (wet signal) (dB). The value is between -20 and 10. */
        AUDIO_REVERB_WET_LEVEL = 1,
        /** 2: The room size of the reflection. The value is between 0 and 100. */
        AUDIO_REVERB_ROOM_SIZE = 2,
        /** 3: The length of the initial delay of the wet signal (ms). The value is between 0 and 200. */
        AUDIO_REVERB_WET_DELAY = 3,
        /** 4: The reverberation strength. The value is between 0 and 100. */
        AUDIO_REVERB_STRENGTH = 4,
    };

    /** Audio codec profile types. The default value is LC_ACC. */
    public enum AUDIO_CODEC_PROFILE_TYPE
    {
        /** 0: LC-AAC, which is the low-complexity audio codec type. */
        AUDIO_CODEC_PROFILE_LC_AAC = 0,
        /** 1: HE-AAC, which is the high-efficiency audio codec type. */
        AUDIO_CODEC_PROFILE_HE_AAC = 1,
        /**
        * 2: HE-AACv2, which is the high-efficiency audio codec type.
        *
        * @since v3.6.1.1
        */
        AUDIO_CODEC_PROFILE_HE_AAC_V2 = 2,
    };

    /** Video codec types */
    public enum VIDEO_CODEC_TYPE
    {
        /** 1: Standard VP8. */
        VIDEO_CODEC_VP8 = 1,
        /** 2: Standard H264. */
        VIDEO_CODEC_H264 = 2,
        /** 3: Enhanced VP8. */
        VIDEO_CODEC_EVP = 3,
        /** 4: Enhanced H264. */
        VIDEO_CODEC_E264 = 4,
    };

    /** Regions for connetion. */
    public enum AREA_CODE : uint {
        /**
        * Mainland China.
        */
        AREA_CODE_CN = 0x00000001,
        /**
        * North America.
        */
        AREA_CODE_NA = 0x00000002,
        /**
        * Europe.
        */
        AREA_CODE_EU = 0x00000004,
        /**
        * Asia, excluding Mainland China.
        */
        AREA_CODE_AS = 0x00000008,
        /**
        * Japan.
        */
        AREA_CODE_JP = 0x00000010,
        /**
        * India.
        */
        AREA_CODE_IN = 0x00000020,
        /**
        * (Default) Global.
        */
        AREA_CODE_GLOB = 0xFFFFFFFF
    };

    /** The output log level of the SDK.
     *
     * @since v3.3.1
     */
    public enum LOG_LEVEL {
        /** 0: Do not output any log. */
        LOG_LEVEL_NONE = 0x0000,
        /** 0x0001: (Default) Output logs of the `FATAL`, `ERROR`, `WARN` and `INFO` level. We recommend setting your
         * log filter as this level.
         */
        LOG_LEVEL_INFO = 0x0001,
        /** 0x0002: Output logs of the `FATAL`, `ERROR` and `WARN` level.
         */
        LOG_LEVEL_WARN = 0x0002,
        /** 0x0004: Output logs of the `FATAL` and `ERROR` level.
         */
        LOG_LEVEL_ERROR = 0x0004,
        /** 0x0008: Output logs of the `FATAL` level.
         */
        LOG_LEVEL_FATAL = 0x0008,
    };

    /** The configuration of the log files.
     *
     * @since v3.3.1
     */
    public struct LogConfig
    {
        /** The absolute path of log files.
         *
         * The default file path is:
         * - Android: `/storage/emulated/0/Android/data/<package name>/files/agorasdk.log`
         * - iOS: `App Sandbox/Library/caches/agorasdk.log`
         * - macOS:
         *  - Sandbox enabled: `App Sandbox/Library/Logs/agorasdk.log`, such as `/Users/<username>/Library/Containers/<App Bundle Identifier>/Data/Library/Logs/agorasdk.log`.
         *  - Sandbox disabled: `～/Library/Logs/agorasdk.log`.
         * - Windows: `C:\Users\<user_name>\AppData\Local\Agora\<process_name>\agorasdk.log`
         *
         * Ensure that the directory for the log files exists and is writable. You can use this parameter to rename the log files.
         */
        public string filePath;
        /** The size (KB) of a log file. The default value is 1024 KB. If you set `fileSize` to 1024 KB, the SDK
         * outputs at most 5 MB log files; if you set it to less than 1024 KB, the setting is invalid, and the
         * maximum size of a log file is still 1024 KB.
         */
        public int fileSize;
        /** The output log level of the SDK. See {@link agora_gaming_rtc.LOG_LEVEL LOG_LEVEL}.
         *
         * For example, if you set the log level to `LOG_LEVEL_WARN`, the SDK outputs the logs within levels `FATAL`,
         * `ERROR`, and `WARN`.
         */
        public LOG_LEVEL level;
    };

    /** Configurations for the `IRtcEngine` instance.
     */
    public struct RtcEngineConfig
    {
        /** The App ID issued to you by Agora. See [How to get the App ID](https://docs.agora.io/en/Agora%20Platform/token#getappid).
         * Only users in apps with the same App ID can join the same channel and communicate with each other. Use an
         * App ID to initialize only one `IRtcEngine` instance. To change your App ID, call
         * {@link agora_gaming_rtc.IRtcEngine.Destroy Destroy} to destroy the current `IRtcEngine` instance and then
         * call this method to initialize an `IRtcEngine` instance with the new App ID.
         */
        public string appId {
            get;
            set;
        }

        /** The region for connection. This advanced feature applies to scenarios that have regional restrictions.
         *
         * For the regions that Agora supports, see #AREA_CODE. After specifying the region, the SDK connects to the
         * Agora servers within that region.
         */
        public AREA_CODE areaCode {
            get;
            set;
        }

        /** The configuration of the log files that the SDK outputs. See {@link agora_gaming_rtc.LogConfig LogConfig}.
         *
         * @since v3.3.1
         *
         * By default, the SDK outputs five log files, `agorasdk.log`, `agorasdk_1.log`,
         * `agorasdk_2.log`, `agorasdk_3.log`, `agorasdk_4.log`, each with a default
         * size of 1024 KB. These log files are encoded in UTF-8. The SDK writes the
         * latest logs in `agorasdk.log`. When `agorasdk.log` is full, the SDK deletes
         * the log file with the earliest modification time among the other four,
         * renames `agorasdk.log` to the name of the deleted log file, and creates a
         * new `agorasdk.log` to record latest logs.
         */
        public LogConfig logConfig {
            get;
            set;
        }

        /** Configurations for the `IRtcEngine` instance.
         */
        public RtcEngineConfig(string mAppId, LogConfig config, AREA_CODE mAreaCode = AREA_CODE.AREA_CODE_GLOB) {
            appId = mAppId;
            areaCode = mAreaCode;
            logConfig = config;
        }
    }

    /** Statistics of the channel. */
    public struct RtcStats
    {
        /** Call duration (s), represented by an aggregate value.
        */
        public uint duration;
        /** Total number of bytes transmitted, represented by an aggregate value.
        */
        public uint txBytes;
        /** Total number of bytes received, represented by an aggregate value.
        */
        public uint rxBytes;
        /** Total number of audio bytes sent (bytes), represented
        * by an aggregate value.
        */
        public uint txAudioBytes;
        /** Total number of video bytes sent (bytes), represented
        * by an aggregate value.
        */
        public uint txVideoBytes;
        /** Total number of audio bytes received (bytes) before
        * network countermeasures, represented by an aggregate value.
        */
        public uint rxAudioBytes;
        /** Total number of video bytes received (bytes),
        * represented by an aggregate value.
        */
        public uint rxVideoBytes;

        /** Transmission bitrate (Kbps), represented by an instantaneous value.
        */
        public uint txKBitRate;
        /** Receive bitrate (Kbps), represented by an instantaneous value.
        */
        public uint rxKBitRate;
        /** Audio receive bitrate (Kbps), represented by an instantaneous value.
        */
        public uint rxAudioKBitRate;
        /** Audio transmission bitrate (Kbps), represented by an instantaneous value.
        */
        public uint txAudioKBitRate;
        /** Video receive bitrate (Kbps), represented by an instantaneous value.
        */
        public uint rxVideoKBitRate;
        /** Video transmission bitrate (Kbps), represented by an instantaneous value.
        */
        public uint txVideoKBitRate;
        /** Client-server latency (ms)
        */
        public ushort lastmileDelay;
        /** The packet loss rate (%) from the local client to Agora's edge server,
        * before using the anti-packet-loss method.
        */
        public ushort txPacketLossRate;
        /** The packet loss rate (%) from Agora's edge server to the local client,
        * before using the anti-packet-loss method.
        */
        public ushort rxPacketLossRate;
        /** Number of users in the channel.
        * - Communication profile: The number of users in the channel.
        * - Interactive live streaming profile:
        *   -  If the local user is an audience: The number of users in the channel = The number of hosts in the channel + 1.
        *   -  If the user is a host: The number of users in the channel = The number of hosts in the channel.
        */
        public uint userCount;
        /** Application CPU usage (%).
        */
        public double cpuAppUsage;
        /** System CPU usage (%).<p>In the multi-kernel environment, this member represents the average CPU usage. The value = 100 - <b>System Idle Progress</b> in <b>Task Manager</b> (%).</p>
        */
        public double cpuTotalUsage;
        /** The round-trip time delay from the client to the local router.
         * @note On iOS, this parameter is disabled by default. See [FAQ](https://docs.agora.io/en/faq/local_network_privacy) for details. If you need to enable this parameter, contact support@agora.io.
         */
        public int gatewayRtt;
        /** The memory usage ratio of the app (%).
        * @note This value is for reference only. Due to system limitations, you may not get the value of this member.
        */
        public double memoryAppUsageRatio;
        /** The memory usage ratio of the system (%).
        * @note This value is for reference only. Due to system limitations, you may not get the value of this member.
        */
        public double memoryTotalUsageRatio;
        /** The memory usage ratio of the app (KB).
        * @note This value is for reference only. Due to system limitations, you may not get the value of this member.
        */
        public int memoryAppUsageInKbytes;
    };
    /** The volume information of users.
     */
    public struct AudioVolumeInfo
    {
        /** The user ID.
         * - In the local user's callback, `uid = 0`.
         * - In the remote users' callback, `uid` is the ID of a remote user whose instantaneous volume is one of the three highest.
         */
        public uint uid;
        /** The volume of each user after audio mixing. The value ranges between 0 (lowest volume) and 255 (highest volume).
         * In the local user's callback, `volume = totalVolume`.
         */
        public uint volume;
        /** Voice activity status of the local user.
        * - `0`: The local user is not speaking.
        * - `1`: The local user is speaking.
        *
        * @note
        * - The `vad` parameter cannot report the voice activity status of remote users.
        * In the remote users' callback, `vad` is always `0`.
        * - To use this parameter, you must set the `report_vad` parameter to `true` when calling {@link agora_gaming_rtc.IRtcEngine.EnableAudioVolumeIndication EnableAudioVolumeIndication}.
        */
        public uint vad;

        /** The name of the channel where the user is in.
         */
        public string channelId;
    };

    /** The channel media options. */
    public class ChannelMediaOptions
    {
        public ChannelMediaOptions(bool _autoSubscribeAudio = true, bool _autoSubscribeVideo = false, bool _publishLocalAudio = true, bool _publishLocalVideo = true) {
            autoSubscribeAudio = _autoSubscribeAudio;
            autoSubscribeVideo = _autoSubscribeVideo;
            publishLocalAudio = _publishLocalAudio;
            publishLocalVideo = _publishLocalVideo;
        }

        /** Determines whether to subscribe to audio streams when the user joins the channel:
         * - true: (Default) Subscribe.
         * - false: Do not subscribe.
         *
         * This member serves a similar function to the {@link agora_gaming_rtc.AgoraChannel.MuteAllRemoteAudioStreams MuteAllRemoteAudioStreams} method. After joining the channel,
         * you can call the `MuteAllRemoteAudioStreams` method to set whether to subscribe to audio streams in the channel.
         */
        public bool autoSubscribeAudio;
        /** Determines whether to subscribe to video streams when the user joins the channel:
         * - true: (Default) Subscribe.
         * - false: Do not subscribe.
         *
         * This member serves a similar function to the {@link agora_gaming_rtc.IRtcEngine.MuteAllRemoteVideoStreams MuteAllRemoteVideoStreams} method. After joining the channel,
         * you can call the `MuteAllRemoteVideoStreams` method to set whether to subscribe to video streams in the channel.
         */
        public bool autoSubscribeVideo;
        /** whether to publish the local audio stream when the user joins a channel:
         *
         * - true: (Default) Publish.
         * -false: Do not publish.
         *
         * This member serves a similar function to the `MuteLocalAudioStream` method. After the user joins the channel, you can call the `MuteLocalAudioStream` method to set whether to publish the local audio stream in the channel.
         */
        public bool publishLocalAudio;
        /** whether to publish the local video stream when the user joins a channel:
         *
         * - true: (Default) Publish.
         * -false: Do not publish.
         *
         * This member serves a similar function to the `MuteLocalVideoStream` method. After the user joins the channel, you can call the `MuteLocalVideoStream` method to set whether to publish the local video stream in the channel.
         */
        public bool publishLocalVideo;
    }

    /** Statistics of the local video stream. */
    public struct LocalVideoStats
    {
        /** Bitrate (Kbps) sent in the reported interval, which does not include
        * the bitrate of the retransmission video after packet loss.
        */
        public int sentBitrate;
        /** Frame rate (fps) sent in the reported interval, which does not include
        * the frame rate of the retransmission video after packet loss.
        */
        public int sentFrameRate;
        /** The encoder output frame rate (fps) of the local video.
        */
        public int encoderOutputFrameRate;
        /** The render output frame rate (fps) of the local video.
        */
        public int rendererOutputFrameRate;
        /** The target bitrate (Kbps) of the current encoder. This value is estimated by the SDK based on the current network conditions.
        */
        public int targetBitrate;
        /** The target frame rate (fps) of the current encoder.
        */
        public int targetFrameRate;
        /** Quality change of the local video in terms of target frame rate and
        * target bit rate in this reported interval. See #QUALITY_ADAPT_INDICATION.
        */
        public QUALITY_ADAPT_INDICATION qualityAdaptIndication;
        /** The encoding bitrate (Kbps), which does not include the bitrate of the
        * re-transmission video after packet loss.
        */
        public int encodedBitrate;
        /** The width of the encoding frame (px).
        */
        public int encodedFrameWidth;
        /** The height of the encoding frame (px).
        */
        public int encodedFrameHeight;
        /** The value of the sent frames, represented by an aggregate value.
        */
        public int encodedFrameCount;
        /** The codec type of the local video:
        * - VIDEO_CODEC_VP8 = 1: VP8.
        * - VIDEO_CODEC_H264 = 2: (Default) H.264.
        */
        public VIDEO_CODEC_TYPE codecType;
        /** The video packet loss rate (%) from the local client to the Agora edge server before applying the anti-packet loss strategies.
         *
         * @since v3.2.0
         */
        public ushort txPacketLossRate;
        /** The capture frame rate (fps) of the local video.
         *
         * @since v3.2.0
         */
        public int captureFrameRate;

        /** The brightness level of the video image captured by the local camera.
         * See {@link agora_gaming_rtc.CAPTURE_BRIGHTNESS_LEVEL_TYPE CAPTURE_BRIGHTNESS_LEVEL_TYPE}.
         *
         * @since v3.3.1
         */
        public CAPTURE_BRIGHTNESS_LEVEL_TYPE captureBrightnessLevel;
    };

    /** Statistics of the remote video stream. */
    public struct RemoteVideoStats
    {
        /** User ID of the remote user sending the video streams. */
        public uint uid;
        /** @deprecated Time delay (ms).<p>In scenarios where audio and video is synchronized, you can use the value of `networkTransportDelay` and `jitterBufferDelay` in `RemoteAudioStats` to know the delay statistics of the remote video.</p> */
        public int delay;
        /** Width (pixels) of the video stream. */
        public int width;
        /** Height (pixels) of the video stream. */
        public int height;
        /** Bitrate (Kbps) received since the last count. */
        public int receivedBitrate;
        /** The decoder output frame rate (fps) of the remote video. */
        public int decoderOutputFrameRate;
        /** The render output frame rate (fps) of the remote video. */
        public int rendererOutputFrameRate;
        /** Packet loss rate (%) of the remote video stream after using the anti-packet-loss method. */
        public int packetLossRate;
        /** The type of the remote video stream: #REMOTE_VIDEO_STREAM_TYPE*/
        public REMOTE_VIDEO_STREAM_TYPE rxStreamType;
        /** The total freeze time (ms) of the remote video stream after the remote user joins the channel.
         * In a video session where the frame rate is set to no less than 5 fps, video freeze occurs when the time interval between two adjacent renderable video frames is more than 500 ms.
         */
        public int totalFrozenTime;
        /** The total video freeze time as a percentage (%) of the total time when the video is available. */
        public int frozenRate;
        /** The total time (ms) when the remote user in the Communication profile or the remote host in the Live-broadcast profile neither stops sending the video stream nor disables the video module after joining the channel.
         *
         * @since v3.0.1
         */
        public int totalActiveTime;
        /** The total publish duration (ms) of the remote video stream.
         *
         * @since v3.2.0
         */
        public int publishDuration;
    };

    /** The UserInfo class.*/
    public struct UserInfo
    {
        /** The user ID.*/
        public uint uid;
        /** The user account.*/
        public string userAccount;
    }

    /** Audio statistics of a remote user */
    public struct RemoteAudioStats
    {
        /** User ID of the remote user sending the audio streams. */
        public uint uid;
        /** Audio quality received by the user. */
        public int quality;
        /** Network delay (ms) from the sender to the receiver. */
        public int networkTransportDelay;
        /** Network delay (ms) from the receiver to the jitter buffer. */
        public int jitterBufferDelay;
        /** The audio frame loss rate in the reported interval. */
        public int audioLossRate;
        /** The number of channels. */
        public int numChannels;
        /** The sample rate (Hz) of the received audio stream in the reported interval. */
        public int receivedSampleRate;
        /** The average bitrate (Kbps) of the received audio stream in the reported interval. */
        public int receivedBitrate;
        /** The total freeze time (ms) of the remote audio stream after the remote user joins the channel. In a session, audio freeze occurs when the audio frame loss rate reaches 4%.
         */
        public int totalFrozenTime;
        /** The total audio freeze time as a percentage (%) of the total time when the audio is available. */
        public int frozenRate;
        /** The total time (ms) when the remote user in the Communication profile or the remote host in the Live-broadcast profile neither stops sending the audio stream nor disables the audio module after joining the channel.
         *
         * @since v3.0.1
         */
        public int totalActiveTime;
        /** The total publish duration (ms) of the remote audio stream.
         *
         * @since v3.2.0
         */
        public int publishDuration;
        /**
         * Quality of experience (QoE) of the local user when receiving a remote audio stream.
         * See #EXPERIENCE_QUALITY_TYPE.
         *
         * @since v3.3.1
         */
        public int qoeQuality;
        /**
         * The reason for poor QoE of the local user when receiving a remote audio stream.
         * See #EXPERIENCE_POOR_REASON.
         *
         * @since v3.3.1
         */
        public int qualityChangedReason;
        /**
         * The quality of the remote audio stream as determined by the Agora
         * real-time audio MOS (Mean Opinion Score) measurement method in the
         * reported interval. The return value ranges from `0` to `500`. Dividing the
         * return value by 100 gets the MOS score, which ranges from 0 to 5. The
         * higher the score, the better the audio quality.
         *
         * @since v3.3.1
         *
         * The subjective perception of audio quality corresponding to the Agora
         * real-time audio MOS scores is as follows:
         *
         * | MOS score       | Perception of audio quality                                                                                                                                 |
         * |-----------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------|
         * | Greater than 4  | Excellent. The audio sounds clear and smooth.                                                                                                               |
         * | From 3.5 to 4   | Good. The audio has some perceptible impairment, but still sounds clear.                                                                                    |
         * | From 3 to 3.5   | Fair. The audio freezes occasionally and requires attentive listening.                                                                                      |
         * | From 2.5 to 3   | Poor. The audio sounds choppy and requires considerable effort to understand.                                                                               |
         * | From 2 to 2.5   | Bad. The audio has occasional noise. Consecutive audio dropouts occur, resulting in some information loss. The users can communicate only with difficulty.  |
         * | Less than 2     | Very bad. The audio has persistent noise. Consecutive audio dropouts are frequent, resulting in severe information loss. Communication is nearly impossible. |
         */
        public int mosValue;
    };


    /** The options of the watermark image to be added. */
    public struct WatermarkOptions
    {
        /** Sets whether or not the watermark image is visible in the local video preview:
         * - true: The watermark image is visible in preview.
         * - false: The watermark image is not visible in preview.
         */
        public bool visibleInPreview;
        /** The watermark position in the landscape mode. See Rectangle.
         * For detailed information on the landscape mode, see *Rotate the video*.
         */
        public Rectangle positionInLandscapeMode;
        /** The watermark position in the portrait mode. See Rectangle.
         * For detailed information on the portrait mode, see *Rotate the video*.
         */
        public Rectangle positionInPortraitMode;
    };

    /** Audio statistics of the local user. */
    public struct LocalAudioStats
    {
        /** The number of channels.
        */
        public int numChannels;
        /** The sample rate (Hz).
        */
        public int sentSampleRate;
        /** The average sending bitrate (Kbps).
        */
        public int sentBitrate;
        /** The audio packet loss rate (%) from the local client to the Agora edge server before applying the anti-packet loss strategies.
        *
        * @since v3.2.0
        */
        public ushort txPacketLossRate;
    };

    /** Video encoder configurations. */
    public struct VideoEncoderConfiguration
    {
        /** The video frame dimension used to specify the video quality and measured by the total number of pixels along a frame's width and height: VideoDimensions.
         */
        public VideoDimensions dimensions;
        /** The frame rate of the video: #FRAME_RATE. The default value is 15.
         * Note that we do not recommend setting this to a value greater than 30.
         */
        public FRAME_RATE frameRate;
        /** The minimum frame rate of the video. The default value is -1.
         */
        public int minFrameRate;
        /** The video encoding bitrate (Kbps).
         */
        public int bitrate;
        /** The minimum encoding bitrate (Kbps).
         *
         * The SDK automatically adjusts the encoding bitrate to adapt to the network conditions. Using a value greater than the default value forces the video encoder to output high-quality images but may cause more packet loss and hence sacrifice the smoothness of the video transmission. That said, unless you have special requirements for image quality, Agora does not recommend changing this value.
         *
         * @note This parameter applies only to the Live-broadcast profile.
         */
        public int minBitrate;
        /** The video orientation mode of the video: #ORIENTATION_MODE.
         */
        public ORIENTATION_MODE orientationMode;
        /** The video encoding degradation preference under limited bandwidth: #DEGRADATION_PREFERENCE.
         */
        public DEGRADATION_PREFERENCE degradationPreference;
        /** Sets the mirror mode of the published local video stream. It only affects the video that the remote user sees. See #VIDEO_MIRROR_MODE_TYPE
         * @since v3.0.0
         * @note The SDK disables the mirror mode by default.
         */
        public VIDEO_MIRROR_MODE_TYPE mirrorMode;
    };

    /** Video dimensions. */
    public struct VideoDimensions
    {
        /** Width (pixels) of the video. */
        public int width;
        /** Height (pixels) of the video. */
        public int height;
    };

    /** The video properties of the user displaying the video in the CDN live. Agora supports a maximum of 17 transcoding users in a CDN streaming channel. */
    public struct TranscodingUser
    {
        /** User ID of the user displaying the video in the CDN live.
        */
        public uint uid;

        /** Horizontal position from the top left corner of the video frame.
*/
        public int x;
        /** Vertical position from the top left corner of the video frame.
        */
        public int y;
        /** Width of the video frame. The default value is 360.
        */
        public int width;
        /** Height of the video frame. The default value is 640.
        */
        public int height;

        /** The layer index of the video frame. An integer. The value range is [0, 100].
        * - 0: (Default) Bottom layer.
        * - 100: Top layer.
        *
        * @note
        * - If zOrder is beyond this range, the SDK reports the `ERR_INVALID_ARGUMENT(-2)`.
        * - As of v2.3, the SDK supports zOrder = 0.
        */
        public int zOrder;
        /**  The transparency level of the user's video. The value ranges between 0 and 1.0:
         * - 0: Completely transparent
         * - 1.0: (Default) Opaque
         */
        public double alpha;
        /** The audio channel of the sound. The default value is 0:
         * - 0: (Default) Supports dual channels at most, depending on the upstream of the host.
         * - 1: The audio stream of the host uses the FL audio channel. If the upstream of the host uses multiple audio channels, these channels will be mixed into mono first.
         * - 2: The audio stream of the host uses the FC audio channel. If the upstream of the host uses multiple audio channels, these channels will be mixed into mono first.
         * - 3: The audio stream of the host uses the FR audio channel. If the upstream of the host uses multiple audio channels, these channels will be mixed into mono first.
         * - 4: The audio stream of the host uses the BL audio channel. If the upstream of the host uses multiple audio channels, these channels will be mixed into mono first.
         * - 5: The audio stream of the host uses the BR audio channel. If the upstream of the host uses multiple audio channels, these channels will be mixed into mono first.
         *
         * @note If your setting is not 0, you may need a specialized player.
         */
        public int audioChannel;
    };

    /** Image properties.
     *
     * The properties of the watermark and background images.
     */
    public struct RtcImage
    {
        /** HTTP/HTTPS URL address of the image on the broadcasting video. The maximum length of this parameter is 1024 bytes. */
        public string url;
        /** Horizontal position of the image from the upper left of the broadcasting video. */
        public int x;
        /** Vertical position of the image from the upper left of the broadcasting video. */
        public int y;
        /** Width of the image on the broadcasting video. */
        public int width;
        /** Height of the image on the broadcasting video. */
        public int height;
        /**
        * The layer number of the watermark or background image. The value range is [0,255]:
        * - 0: (Default) Bottom layer.
        * - 255: Top layer.
        *
        * @since v3.6.1.1
        */
        public int zOrder;
        /**
         * The transparency of the watermark or background image. The value range is [0.0,1.0]:
         * - 0.0: Completely transparent
         * - 1.0: (Default) Opaque
         *
         * @since v3.6.1.1
         */
        public double alpha;
    }

    /// @cond
    /** The configuration for advanced features of the RTMP or RTMPS streaming with transcoding.
     */
    public struct LiveStreamAdvancedFeature {

        /** The advanced feature for high-quality video with a lower bitrate. */
        public const string LBHQ = "lbhq";
        /** The advanced feature for the optimized video encoder. */
        public const string VEO = "veo";

        /** The name of the advanced feature. It contains `LBHQ` and `VEO`.
        */
        public string featureName;

        /** Whether to enable the advanced feature:
        * - true: Enable the advanced feature.
        * - false: (Default) Disable the advanced feature.
        */
        public bool opened;

    }
    /// @endcond

    /** A struct for managing CDN live audio/video transcoding settings. */
    public struct LiveTranscoding
    {
        /** Width of the video. The default value is 360.
         * - If you push video streams to the CDN, set the value of width &times; height to at least 64 &times; 64 (px), or the SDK will adjust it to 64 &times; 64 (px).
         * - If you push audio streams to the CDN, set the value of width &times; height to 0 &times; 0 (px).
         */
        public int width;
        /** Height of the video. The default value is 640.
         * - If you push video streams to the CDN, set the value of width &times; height to at least 64 &times; 64 (px), or the SDK will adjust it to 64 &times; 64 (px).
         * - If you push audio streams to the CDN, set the value of width &times; height to 0 &times; 0 (px).
         */
        public int height;
        /** Bitrate of the CDN live output video stream. The default value is 400 Kbps.
         *
         * Set this parameter according to the Video Bitrate Table. If you set a bitrate beyond the proper range, the SDK automatically adapts it to a value within the range.
         */
        public int videoBitrate;
        /** Frame rate of the output video stream set for the CDN interactive live streaming. The default value is 15 fps, and the value range is (0,30].
         *
         * @note Agora adjusts all values over 30 to 30.
         */
        public int videoFramerate;

        /** @deprecated Latency mode:
         * - true: Low latency with unassured quality.
         * - false: (Default) High latency with assured quality.
         */
        public bool lowLatency;

        /** Video GOP in frames. The default value is 30 fps.
        */
        public int videoGop;
        /** Self-defined video codec profile: #VIDEO_CODEC_PROFILE_TYPE.
         *
         * @note If you set this parameter to other values, Agora adjusts it to the default value of 100.
         */
        public VIDEO_CODEC_PROFILE_TYPE videoCodecProfile;
        /** The background color in RGB hex value. Value only, do not include a #. For example, 0xFFB6C1 (light pink). The default value is 0x000000 (black).
         */
        public uint backgroundColor;
        /** The number of users in the interactive live streaming.
         */
        public uint userCount;
        /** TranscodingUser.
        */
        public TranscodingUser[] transcodingUsers;
        /** Reserved property. Extra user-defined information to send SEI for the H.264/H.265 video stream to the CDN live client. Maximum length: 4096 Bytes.
         *
         * For more information on SEI frame, see [SEI-related questions](https://docs.agora.io/en/faq/sei).
         */
        public string transcodingExtraInfo;

        /** @deprecated The metadata sent to the CDN live client defined by the RTMP or FLV metadata.
         */
        public string metadata;
        /** The watermark image added to the CDN live publishing stream.
         *
         * Ensure that the format of the image is PNG. Once a watermark image is added, the audience of the CDN live publishing stream can see the watermark image. See RtcImage.
         */
        public RtcImage watermark;
        /**
        * The number of watermarks on the live video. The value range is [0,100]. This parameter is used in conjunction with watermark.
        *
        * @since v3.6.1.1
        */
        public uint watermarkCount;
        /** The background image added to the CDN live publishing stream.
         *
         * Once a background image is added, the audience of the CDN live publishing stream can see the background image. See RtcImage.
         */
        public RtcImage backgroundImage;
        /**
        * The number of background images on the live video. The value range is [0,100]. This parameter is used in conjunction with `backgroundImage`.
        *
        * @since v3.6.1.1
        */
        public uint backgroundImageCount;
        /** Self-defined audio-sample rate: #AUDIO_SAMPLE_RATE_TYPE.
        */
        public AUDIO_SAMPLE_RATE_TYPE audioSampleRate;
        /** Bitrate of the CDN live audio output stream. The default value is 48 Kbps, and the highest value is 128.
         */
        public int audioBitrate;
        /** Agora's self-defined audio-channel types. We recommend choosing option 1 or 2. A special player is required if you choose option 3, 4, or 5:
         * - 1: (Default) Mono
         * - 2: Two-channel stereo
         * - 3: Three-channel stereo
         * - 4: Four-channel stereo
         * - 5: Five-channel stereo
         */
        public int audioChannels;
        /** Self-defined audio codec profile: #AUDIO_CODEC_PROFILE_TYPE.
         */
        public AUDIO_CODEC_PROFILE_TYPE audioCodecProfile;

        /// @cond
        /** Advanced features of the RTMP or RTMPS streaming with transcoding. See LiveStreamAdvancedFeature.
         *
         * @since v3.2.0
         */
        public LiveStreamAdvancedFeature[] liveStreamAdvancedFeatures;
        /// @endcond
    };

    /** Video frame rates. */
    public enum FRAME_RATE
    {
        /** 1: 1 fps */
        FRAME_RATE_FPS_1 = 1,
        /** 7: 7 fps */
        FRAME_RATE_FPS_7 = 7,
        /** 10: 10 fps */
        FRAME_RATE_FPS_10 = 10,
        /** 15: 15 fps */
        FRAME_RATE_FPS_15 = 15,
        /** 24: 24 fps */
        FRAME_RATE_FPS_24 = 24,
        /** 30: 30 fps */
        FRAME_RATE_FPS_30 = 30,
        /** 60: 60 fps (Windows and macOS only) */
        FRAME_RATE_FPS_60 = 60,
    };

    /** Video output orientation modes.
    */
    public enum ORIENTATION_MODE
    {
        /** 0: (Default) Adaptive mode.
         * The video encoder adapts to the orientation mode of the video input device.
         * - If the width of the captured video from the SDK is greater than the height, the encoder sends the video in landscape mode. The encoder also sends the rotational information of the video, and the receiver uses the rotational information to rotate the received video.
         * - When you use a custom video source, the output video from the encoder inherits the orientation of the original video. If the original video is in portrait mode, the output video from the encoder is also in portrait mode. The encoder also sends the rotational information of the video to the receiver.
         */
        ORIENTATION_MODE_ADAPTIVE = 0,
        /** 1: Landscape mode.
         * The video encoder always sends the video in landscape mode. The video encoder rotates the original video before sending it and the rotational infomation is 0. This mode applies to scenarios involving CDN live streaming.
         */
        ORIENTATION_MODE_FIXED_LANDSCAPE = 1,
        /** 2: Portrait mode.
         * The video encoder always sends the video in portrait mode. The video encoder rotates the original video before sending it and the rotational infomation is 0. This mode applies to scenarios involving CDN live streaming.
         */
        ORIENTATION_MODE_FIXED_PORTRAIT = 2,
    };

    /** Video degradation preferences when the bandwidth is a constraint. */
    public enum DEGRADATION_PREFERENCE
    {
        /** 0: (Default) Degrade the frame rate in order to maintain the video quality. */
        MAINTAIN_QUALITY = 0,
        /** 1: Degrade the video quality in order to maintain the frame rate. */
        MAINTAIN_FRAMERATE = 1,
        /** 2: (For future use) Maintain a balance between the frame rate and video quality. */
        MAINTAIN_BALANCED = 2,
    };

    /** The external video frame. */
    public struct ExternalVideoFrame
    {
        /** The video buffer type.
         */
        public enum VIDEO_BUFFER_TYPE
        {
            /** 1: The video buffer in the format of raw data.
             */
            VIDEO_BUFFER_RAW_DATA = 1,
        };

        /** The video pixel format.
         * @note The SDK does not support the alpha channel, and discards any alpha value passed to the SDK.
         */
        public enum VIDEO_PIXEL_FORMAT
        {
            /** 0: The video pixel format is unknown.
             */
            VIDEO_PIXEL_UNKNOWN = 0,
            /** 1: The video pixel format is I420.
             */
            VIDEO_PIXEL_I420 = 1,
            /** 2: The video pixel format is BGRA.
             */
            VIDEO_PIXEL_BGRA = 2,
            /** 3: The video pixel format is NV21.
            */
            VIDEO_PIXEL_NV21 = 3,
            /** 4: The video pixel format is RGBA.
            */
            VIDEO_PIXEL_RGBA = 4,
            /** 5: The video pixel format is IMC2.
            */
            VIDEO_PIXEL_IMC2 = 5,
            /** 7: The video pixel format is ARGB.
            */
            VIDEO_PIXEL_ARGB = 7,
            /** 8: The video pixel format is NV12.
             */
            VIDEO_PIXEL_NV12 = 8,
            /** 16: The video pixel format is I422.
             */
            VIDEO_PIXEL_I422 = 16,
            /** 17: The video pixel format is GL_TEXTURE_2D.
            */
            VIDEO_TEXTURE_2D = 17,
            /** 18: The video pixel format is GL_TEXTURE_OES.
            */
            VIDEO_TEXTURE_OES = 18,
        };

        /** The buffer type. See #VIDEO_BUFFER_TYPE.
         */
        public VIDEO_BUFFER_TYPE type;
        /** The pixel format. See #VIDEO_PIXEL_FORMAT.
         */
        public VIDEO_PIXEL_FORMAT format;
        /** The video buffer.
         */
        public byte[] buffer;

        public IntPtr bufferPtr;
        /** Line spacing of the incoming video frame, which must be in pixels instead of bytes. For textures, it is the width of the texture.
         */
        public int stride;
        /** Height of the incoming video frame.
         */
        public int height;
        /** [Raw data related parameter] The number of pixels trimmed from the left. The default value is 0.
         */
        public int cropLeft;
        /** [Raw data related parameter] The number of pixels trimmed from the top. The default value is 0.
         */
        public int cropTop;
        /** [Raw data related parameter] The number of pixels trimmed from the right. The default value is 0.
         */
        public int cropRight;
        /** [Raw data related parameter] The number of pixels trimmed from the bottom. The default value is 0.
         */
        public int cropBottom;
        /** [Raw data related parameter] The clockwise rotation of the video frame. You can set the rotation angle as 0, 90, 180, or 270. The default value is 0.
         */
        public int rotation;
        /** Timestamp of the incoming video frame (ms). An incorrect timestamp results in frame loss or unsynchronized audio and video.
         */
        public long timestamp;
    };

    /** The video frame type. */
    public enum VIDEO_FRAME_TYPE
    {
        /** 0: YUV420. */
        FRAME_TYPE_YUV420 = 0,  //YUV 420 format
        /** 1: RGBA. */
        FRAME_TYPE_RGBA = 1, //RGBA
    };

    /** Video mirror modes. */
    public enum VIDEO_MIRROR_MODE_TYPE
    {
        /** 0: The default mirror mode is determined by the SDK. */
        VIDEO_MIRROR_MODE_AUTO = 0,//determined by SDK
            /** 1: Enable mirror mode. */
        VIDEO_MIRROR_MODE_ENABLED = 1,//enabled mirror
            /** 2: Disable mirror mode. */
        VIDEO_MIRROR_MODE_DISABLED = 2,//disable mirror
    };


    /** Video frame containing the Agora RTC SDK's encoded video data. */
    public struct VideoFrame
    {
        /** The video frame type: #VIDEO_FRAME_TYPE. */
        public VIDEO_FRAME_TYPE type;
        /** Width (pixel) of the video frame.*/
        public int width;
        /** Height (pixel) of the video frame. */
        public int height;
        /** Line span of the Y buffer within the video data. */
        public int yStride;  //stride of  data buffer
        /** The buffer of the RGBA data. */
        public byte[] buffer;  //rgba data buffer
        public IntPtr bufferPtr;
        /** Set the rotation of this frame before rendering the video. Supports 0, 90, 180, 270 degrees clockwise.
         */
        public int rotation; // rotation of this frame (0, 90, 180, 270)
        /** The timestamp of the external audio frame. It is mandatory. You can use this parameter for the following purposes:
         * - Restore the order of the captured audio frame.
         * - Synchronize audio and video frames in video-related scenarios, including scenarios where external video sources are used.
         * @note This timestamp is for rendering the video stream, and not for capturing the video stream.
         */
        public long renderTimeMs;
        /** Reserved for future use. */
        public int avsync_type;
    };

    /** The audio frame type. */
    public enum AUDIO_FRAME_TYPE
    {
        /** 0: PCM16. */
        FRAME_TYPE_PCM16 = 0,  //PCM 16bit little endian
    };

    /** Definition of AudioFrame. */
    public struct AudioFrame
    {
        /** The type of the audio frame. See #AUDIO_FRAME_TYPE
         */
        public AUDIO_FRAME_TYPE type;
        /** The number of samples per channel in the audio frame.
         */
        public int samples;  //number of samples in this frame
        /** The number of bytes per audio sample, which is usually 16-bit (2-byte).
         */
        public int bytesPerSample;  //number of bytes per sample: 2 for PCM16
        /** The number of audio channels.
         * - 1: Mono
         * - 2: Stereo (the data is interleaved)
         */
        public int channels;  //number of channels (data are interleaved if stereo)
        /** The sample rate.
         */
        public int samplesPerSec;  //sampling rate
        /** The data buffer of the audio frame. When the audio frame uses a stereo channel, the data buffer is interleaved.
         * The size of the data buffer is as follows: `buffer` = `samples` × `channels` × `bytesPerSample`.
         */
        public byte[] buffer;  //data buffer
        public IntPtr bufferPtr;
        /** The timestamp of the external audio frame. You can use this parameter for the following purposes:
         * - Restore the order of the captured audio frame.
         * - Synchronize audio and video frames in video-related scenarios, including where external video sources are used.
         */
        public long renderTimeMs;
        /** Reserved for future use.
         */
        public int avsync_type;
    };

    /** @deprecated Type of audio device.
    */
    public enum MEDIA_SOURCE_TYPE
    {
        /** 0: Audio playback device.
        */
        AUDIO_PLAYOUT_SOURCE = 0,
        /** 1: Microphone.
         */
        AUDIO_RECORDING_SOURCE = 1,
    };

    /** States of the last-mile network probe test. */
    public enum LASTMILE_PROBE_RESULT_STATE
    {
        /** 1: The last-mile network probe test is complete. */
        LASTMILE_PROBE_RESULT_COMPLETE = 1,
        /** 2: The last-mile network probe test is incomplete and the bandwidth estimation is not available, probably due to limited test resources. */
        LASTMILE_PROBE_RESULT_INCOMPLETE_NO_BWE = 2,
        /** 3: The last-mile network probe test is not carried out, probably due to poor network conditions. */
        LASTMILE_PROBE_RESULT_UNAVAILABLE = 3
    };

    /** The uplink or downlink last-mile network probe test result. */
    public struct LastmileProbeOneWayResult
    {
        /** The packet loss rate (%). */
        public uint packetLossRate;
        /** The network jitter (ms). */
        public uint jitter;
        /** The estimated available bandwidth (bps). */
        public uint availableBandwidth;
    };

    /** The uplink and downlink last-mile network probe test result. */
    public struct LastmileProbeResult
    {
        /** The state of the probe test. */
        public LASTMILE_PROBE_RESULT_STATE state;
        /** The uplink last-mile network probe test result. */
        public LastmileProbeOneWayResult uplinkReport;
        /** The downlink last-mile network probe test result. */
        public LastmileProbeOneWayResult downlinkReport;
        /** The round-trip delay time (ms). */
        public uint rtt;
    };

    /**
     * Sets the camera direction.
     */
    public enum CAMERA_DIRECTION
    {
        /**
         * 0: Uses the rear camera.
         */
        CAMERA_REAR = 0,
        /**
         * 1: Uses the front camera.
         */
        CAMERA_FRONT = 1,
    };

     /** Camera capturer configuration.
     */
    public struct CameraCapturerConfiguration
    {
        /** Camera capturer preference settings.See: #CAPTURER_OUTPUT_PREFERENCE. */
        public CAPTURER_OUTPUT_PREFERENCE preference;
        /** Camera direction settings (for Android/iOS only). See: #CAMERA_DIRECTION. */
        public CAMERA_DIRECTION cameraDirection;
        /** The width (px) of the video image captured by the local camera.
         * To customize the width of the video image, set `preference` as `CAPTURER_OUTPUT_PREFERENCE_MANUAL(3)` first,
         * and then use `captureWidth`.
         *
         * @since v3.3.1
         */
        public int captureWidth;
        /** The height (px) of the video image captured by the local camera.
         * To customize the height of the video image, set `preference` as `CAPTURER_OUTPUT_PREFERENCE_MANUAL(3)` first,
         * and then use `captureHeight`.
         *
         * @since v3.3.1
         */
        public int captureHeight;
    };

     /** Camera capturer configuration.
 */
    public enum CAPTURER_OUTPUT_PREFERENCE
    {
        /** 0: (Default) self-adapts the camera output parameters to the system performance and network conditions to balance CPU consumption and video preview quality.
        */
        CAPTURER_OUTPUT_PREFERENCE_AUTO = 0,
        /** 1: Prioritizes the system performance. The SDK chooses the dimension and frame rate of the local camera capture closest to those set by {@link agora_gaming_rtc.IRtcEngine.SetVideoEncoderConfiguration SetVideoEncoderConfiguration}.
        */
        CAPTURER_OUTPUT_PREFERENCE_PERFORMANCE = 1,
        /** 2: Prioritizes the local preview quality. The SDK chooses higher camera output parameters to improve the local video preview quality. This option requires extra CPU and RAM usage for video pre-processing.
        */
        CAPTURER_OUTPUT_PREFERENCE_PREVIEW = 2,
        /** 3: Allows you to customize the width and height of the video image captured by the local camera.
         *
         * @since v3.3.1
         */
        CAPTURER_OUTPUT_PREFERENCE_MANUAL = 3,
    };


    /** Network quality types. */
    public enum QUALITY_TYPE
    {
        /** 0: The network quality is unknown. */
        QUALITY_UNKNOWN = 0,
        /** 1: The network quality is excellent. */
        QUALITY_EXCELLENT = 1,
        /** 2: The network quality is quite good, but the bitrate may be slightly lower than excellent. */
        QUALITY_GOOD = 2,
        /** 3: Users can feel the communication slightly impaired. */
        QUALITY_POOR = 3,
        /** 4: Users cannot communicate smoothly. */
        QUALITY_BAD = 4,
        /** 5: The network is so bad that users can barely communicate. */
        QUALITY_VBAD = 5,
        /** 6: The network is down and users cannot communicate at all. */
        QUALITY_DOWN = 6,
        /** 7: Users cannot detect the network quality. (Not in use.) */
        QUALITY_UNSUPPORTED = 7,
        /** 8: Detecting the network quality. */
        QUALITY_DETECTING = 8,
    };

    /** Media device states.
    */
    public enum MEDIA_DEVICE_STATE_TYPE
    {
        /** 0: The device is idle.
        */
        MEDIA_DEVICE_STATE_IDLE = 0,
        /** 1: The device is active.
        */
        MEDIA_DEVICE_STATE_ACTIVE = 1,
        /** 2: The device is disabled.
        */
        MEDIA_DEVICE_STATE_DISABLED = 2,
        /** 4: The device is not present.
        */
        MEDIA_DEVICE_STATE_NOT_PRESENT = 4,
        /** 8: The device is unplugged.
        */
        MEDIA_DEVICE_STATE_UNPLUGGED = 8,
        /** 16: The device is not recommended.
        */
        MEDIA_DEVICE_STATE_UNRECOMMENDED = 16,
    };

    /** States of importing an external video stream in the interactive live streaming. */
    public enum INJECT_STREAM_STATUS
    {
        /** 0: The external video stream imported successfully. */
        INJECT_STREAM_STATUS_START_SUCCESS = 0,
        /** 1: The external video stream already exists. */
        INJECT_STREAM_STATUS_START_ALREADY_EXISTS = 1,
        /** 2: The external video stream to be imported is unauthorized. */
        INJECT_STREAM_STATUS_START_UNAUTHORIZED = 2,
        /** 3: Import external video stream timeout. */
        INJECT_STREAM_STATUS_START_TIMEDOUT = 3,
        /** 4: Import external video stream failed. */
        INJECT_STREAM_STATUS_START_FAILED = 4,
        /** 5: The external video stream stopped importing successfully. */
        INJECT_STREAM_STATUS_STOP_SUCCESS = 5,
        /** 6: No external video stream is found. */
        INJECT_STREAM_STATUS_STOP_NOT_FOUND = 6,
        /** 7: The external video stream to be stopped importing is unauthorized. */
        INJECT_STREAM_STATUS_STOP_UNAUTHORIZED = 7,
        /** 8: Stop importing external video stream timeout. */
        INJECT_STREAM_STATUS_STOP_TIMEDOUT = 8,
        /** 9: Stop importing external video stream failed. */
        INJECT_STREAM_STATUS_STOP_FAILED = 9,
        /** 10: The external video stream is corrupted. */
        INJECT_STREAM_STATUS_BROKEN = 10,
    };

    /** The priority of the remote user.
    */
    public enum PRIORITY_TYPE
    {
        /** 50: The user's priority is high.
        */
        PRIORITY_HIGH = 50,
        /** 100: (Default) The user's priority is normal.
        */
        PRIORITY_NORMAL = 100,
    };

    /** Configurations of the last-mile network probe test. */
    public struct LastmileProbeConfig
    {
        /** Sets whether or not to test the uplink network. Some users, for example, the audience in a Live-broadcast channel, do not need such a test:
         * - true: test.
         * - false: do not test.
         */
        public bool probeUplink;
        /** Sets whether or not to test the downlink network:
         * - true: test.
         * - false: do not test.
         */
        public bool probeDownlink;
        /** The expected maximum sending bitrate (bps) of the local user. The value ranges between 100000 and 5000000. We recommend setting this parameter according to the bitrate value set by {@link agora_gaming_rtc.IRtcEngine.SetVideoEncoderConfiguration SetVideoEncoderConfiguration}. */
        public uint expectedUplinkBitrate;
        /** The expected maximum receiving bitrate (bps) of the local user. The value ranges between 100000 and 5000000. */
        public uint expectedDownlinkBitrate;
    };

    /** Definition of Packet. */
    public struct Packet
	{
        /** Buffer address of the sent or received data.
         * @note Agora recommends that the value of buffer is more than 2048 bytes, otherwise, you may meet undefined behaviors such as a crash.
         */
		public IntPtr buffer;
        /** Buffer size of the sent or received data.
         */
		public IntPtr size;
	};

    /** Local audio state types.
    */
    public enum LOCAL_AUDIO_STREAM_STATE
    {
        /** 0: The local audio is in the initial state.
        */
        LOCAL_AUDIO_STREAM_STATE_STOPPED = 0,
        /** 1: The capturing device starts successfully.
        */
        LOCAL_AUDIO_STREAM_STATE_RECORDING = 1,
        /** 2: The first audio frame encodes successfully.
        */
        LOCAL_AUDIO_STREAM_STATE_ENCODING = 2,
        /** 3: The local audio fails to start.
        */
        LOCAL_AUDIO_STREAM_STATE_FAILED = 3
    };

    /** Local audio state error codes.
    */
    public enum LOCAL_AUDIO_STREAM_ERROR {
        /** 0: The local audio is normal.
        */
        LOCAL_AUDIO_STREAM_ERROR_OK = 0,
        /** 1: No specified reason for the local audio failure.
        */
        LOCAL_AUDIO_STREAM_ERROR_FAILURE = 1,
        /** 2: No permission to use the local audio device.
        */
        LOCAL_AUDIO_STREAM_ERROR_DEVICE_NO_PERMISSION = 2,
        /** 3: The microphone is in use.
        */
        LOCAL_AUDIO_STREAM_ERROR_DEVICE_BUSY = 3,
        /** 4: The local audio capturing fails. Check whether the capturing device
        * is working properly.
        */
        LOCAL_AUDIO_STREAM_ERROR_RECORD_FAILURE = 4,
        /** 5: The local audio encoding fails.
        */
        LOCAL_AUDIO_STREAM_ERROR_ENCODE_FAILURE = 5,
        /** 6: No recording audio device.
        */
        LOCAL_AUDIO_STREAM_ERROR_NO_RECORDING_DEVICE = 6,
        /** 7: No playout audio device.
        */
        LOCAL_AUDIO_STREAM_ERROR_NO_PLAYOUT_DEVICE = 7,
        /**
        * 8: The local audio capturing is interrupted by the system call.
        */
        LOCAL_AUDIO_STREAM_ERROR_INTERRUPTED = 8,
        /**
        * 9: An invalid audio capture device ID.
        *
        * @since v3.6.1.1
        */
        LOCAL_AUDIO_STREAM_ERROR_RECORD_INVALID_ID = 9,
        /**
        * 10: An invalid audio playback device ID.
        *
        * @since v3.6.1.1
        */
        LOCAL_AUDIO_STREAM_ERROR_PLAYOUT_INVALID_ID = 10,
    };


    /** Remote audio states.
    */
    public enum REMOTE_AUDIO_STATE
    {
        /** 0: The remote audio is in the default state, probably due to
        * `REMOTE_AUDIO_REASON_LOCAL_MUTED(3)`,
        * `REMOTE_AUDIO_REASON_REMOTE_MUTED(5)`, or
        * `REMOTE_AUDIO_REASON_REMOTE_OFFLINE(7)`.
        */
        REMOTE_AUDIO_STATE_STOPPED = 0,  // Default state, audio is started or remote user disabled/muted audio stream
        /** 1: The first remote audio packet is received.
        */
        REMOTE_AUDIO_STATE_STARTING = 1,  // The first audio frame packet has been received
        /** 2: The remote audio stream is decoded and plays normally, probably
        * due to `REMOTE_AUDIO_REASON_NETWORK_RECOVERY(2)`,
        * `REMOTE_AUDIO_REASON_LOCAL_UNMUTED`(4)`, or
        * `REMOTE_AUDIO_REASON_REMOTE_UNMUTED(6)`.
        */
        REMOTE_AUDIO_STATE_DECODING = 2,  // The first remote audio frame has been decoded or fronzen state ends
        /** 3: The remote audio is frozen, probably due to
        * `REMOTE_AUDIO_REASON_NETWORK_CONGESTION(1)`.
        */
        REMOTE_AUDIO_STATE_FROZEN = 3,    // Remote audio is frozen, probably due to network issue
        /** 4: The remote audio fails to start, probably due to
        * `REMOTE_AUDIO_REASON_INTERNAL(0)`.
        */
        REMOTE_AUDIO_STATE_FAILED = 4,    // Remote audio play failed
    };

    /** Remote audio state reasons.
    */
    public enum REMOTE_AUDIO_STATE_REASON
    {
        /** 0: The SDK reports this reason when the audio state changes.
        */
        REMOTE_AUDIO_REASON_INTERNAL = 0,
        /** 1: Network congestion.
        */
        REMOTE_AUDIO_REASON_NETWORK_CONGESTION = 1,
        /** 2: Network recovery.
        */
        REMOTE_AUDIO_REASON_NETWORK_RECOVERY = 2,
        /** 3: The local user stops receiving the remote audio stream or
        * disables the audio module.
        */
        REMOTE_AUDIO_REASON_LOCAL_MUTED = 3,
        /** 4: The local user resumes receiving the remote audio stream or
        * enables the audio module.
        */
        REMOTE_AUDIO_REASON_LOCAL_UNMUTED = 4,
        /** 5: The remote user stops sending the audio stream or disables the
        * audio module.
        */
        REMOTE_AUDIO_REASON_REMOTE_MUTED = 5,
        /** 6: The remote user resumes sending the audio stream or enables the
        * audio module.
        */
        REMOTE_AUDIO_REASON_REMOTE_UNMUTED = 6,
        /** 7: The remote user leaves the channel.
        */
        REMOTE_AUDIO_REASON_REMOTE_OFFLINE = 7,
    };

    /** Image enhancement options. */
    public struct BeautyOptions {
        /** The contrast level, used with the @p lightening parameter.
        */
        public enum LIGHTENING_CONTRAST_LEVEL
        {
            /** Low contrast level. */
            LIGHTENING_CONTRAST_LOW = 0,
            /** (Default) Normal contrast level. */
            LIGHTENING_CONTRAST_NORMAL,
            /** High contrast level. */
            LIGHTENING_CONTRAST_HIGH
        };

        /** The contrast level, used with the `lightening` parameter.
        */
        public LIGHTENING_CONTRAST_LEVEL lighteningContrastLevel;

        /** The brightening level, in the range [0.0,1.0], where 0.0 means the original brightening. The default value is 0.6. The higher the value, the greater the brightening level. */
        public float lighteningLevel;

        /** The sharpness level. The value ranges between 0 (original) and 1. This parameter is usually used to remove blemishes.
        */
        public float smoothnessLevel;

        /** The redness level. The value ranges between 0 (original) and 1. This parameter adjusts the red saturation level.
        */
        public float rednessLevel;
        /** The sharpness level, in the range [0.0,1.0], where 0.0 means the original sharpness. The default value is 0.3. The higher the value, the greater the sharpness level.
        *
        * @since v3.6.1.1
        */
        public float sharpnessLevel;
    }

    /** The relative location of the region to the screen or window. */
    public struct Rectangle
    {
        /** The horizontal offset from the top-left corner.
        */
        public int x;
        /** The vertical offset from the top-left corner.
        */
        public int y;
        /** The width of the region.
        */
        public int width;
        /** The height of the region.
        */
        public int height;
    };

    /** Screen sharing encoding parameters.
    */
    public struct ScreenCaptureParameters
    {
        /** The maximum encoding dimensions of the shared region in terms of width &times; height.
         *
         * The default value is 1920 &times; 1080 pixels, that is, 2073600 pixels. Agora uses the value of this parameter to calculate the charges.
         *
         * If the aspect ratio is different between the encoding dimensions and screen dimensions, Agora applies the following algorithms for encoding. Suppose the encoding dimensions are 1920 x 1080:
         * - If the value of the screen dimensions is lower than that of the encoding dimensions, for example, 1000 &times; 1000, the SDK uses 1000 &times; 1000 for encoding.
         * - If the value of the screen dimensions is higher than that of the encoding dimensions, for example, 2000 &times; 1500, the SDK uses the maximum value under 1920 &times; 1080 with the aspect ratio of the screen dimension (4:3) for encoding, that is, 1440 &times; 1080.
         */
        public VideoDimensions dimensions;
        /** The frame rate (fps) of the shared region.
         *
         * The default value is 5. We do not recommend setting this to a value greater than 15.
         */
        public int frameRate;
        /** The bitrate (Kbps) of the shared region.
         *
         * The default value is 0 (the SDK works out a bitrate according to the dimensions of the current screen).
         */
        public int bitrate;
        /** Sets whether or not to capture the mouse for screen sharing:
         * - true: (Default) Capture the mouse.
         * - false: Do not capture the mouse.
         */
        public bool captureMouseCursor;
        /** Whether to bring the window to the front when calling {@link agora_gaming_rtc.IRtcEngine.StartScreenCaptureByWindowId StartScreenCaptureByWindowId} to share the window:
         * - true: Bring the window to the front.
         * - false: (Default) Do not bring the window to the front.
         */
        public bool windowFocus;
        /** A list of IDs of windows to be blocked.
         *
         * When calling {@link agora_gaming_rtc.IRtcEngine.StartScreenCaptureByScreenRect StartScreenCaptureByScreenRect} or {@link agora_gaming_rtc.IRtcEngine.StartScreenCaptureByWindowId StartScreenCaptureByWindowId} to start screen sharing, you can use this parameter to block the specified windows. When calling {@link agora_gaming_rtc.IRtcEngine.UpdateScreenCaptureParameters UpdateScreenCaptureParameters} to update the configuration for screen sharing, you can use this parameter to dynamically block the specified windows during screen sharing.
         */
        public string[] excludeWindowList;
        /** The number of windows to be blocked.
        */
        public int excludeWindowCount;
        /** (macOS only) The width (px) of the border. Defaults to 0, and the value range is [0,50].
         * @since 3.7.0
        */
        public int highLightWidth;
        /** (macOS only) The color of the border in RGBA format. The default value is 0xFF8CBF26.
         * @since 3.7.0
        */
        public uint highLightColor;
        /** (macOS only) Determines whether to place a border around the shared window or screen:
        - true: Place a border.
        - false: (Default) Do not place a border.
        * @since 3.7.0
        * @note When you share a part of a window or screen, the SDK places a border around the entire window or screen if you set `enableHighLight` as true.
        */
        public bool enableHighLight;
    };

    /** Configuration of the injected media stream.
    */
    public struct InjectStreamConfig
    {
        /** Width of the injected stream in the interactive live streaming. The default value is 0 (same width as the original stream).
         */
        public int width;
        /** Height of the injected stream in the interactive live streaming. The default value is 0 (same height as the original stream).
         */
        public int height;
        /** Video GOP (in frames) of the injected stream in the interactive live streaming. The default value is 30 fps.
        */
        public int videoGop;
        /** Video frame rate of the injected stream in the interactive live streaming. The default value is 15 fps.
        */
        public int videoFramerate;
        /** Video bitrate of the injected stream in the interactive live streaming. The default value is 400 Kbps.
        *
        * @note The setting of the video bitrate is closely linked to the resolution. If the video bitrate you set is beyond a reasonable range, the SDK sets it within a reasonable range.
        */
        public int videoBitrate;
        /** Audio-sample rate of the injected stream in the interactive live streaming: #AUDIO_SAMPLE_RATE_TYPE. The default value is 48000 Hz.
        *
        * @note We recommend setting the default value.
        */
        public AUDIO_SAMPLE_RATE_TYPE audioSampleRate;
        /** Audio bitrate of the injected stream in the interactive live streaming. The default value is 48.
        *
        * @note We recommend setting the default value.
        */
        public int audioBitrate;
        /** Audio channels in the interactive live streaming.
        * - 1: (Default) Mono
        * - 2: Two-channel stereo
        *
        * @note We recommend setting the default value.
        */
        public int audioChannels;
    };

    /** Audio session restriction. */
    public enum AUDIO_SESSION_OPERATION_RESTRICTION
    {
        /** No restriction, the SDK has full control of the audio session operations. */
        AUDIO_SESSION_OPERATION_RESTRICTION_NONE = 0,
        /** The SDK does not change the audio session category. */
        AUDIO_SESSION_OPERATION_RESTRICTION_SET_CATEGORY = 1,
        /** The SDK does not change any setting of the audio session (category, mode, categoryOptions). */
        AUDIO_SESSION_OPERATION_RESTRICTION_CONFIGURE_SESSION = 1 << 1,
        /** The SDK keeps the audio session active when leaving a channel. */
        AUDIO_SESSION_OPERATION_RESTRICTION_DEACTIVATE_SESSION = 1 << 2,
        /** The SDK does not configure the audio session anymore. */
        AUDIO_SESSION_OPERATION_RESTRICTION_ALL = 1 << 7,
    };

    /** The definition of ChannelMediaRelayConfiguration.
    */
    public struct ChannelMediaRelayConfiguration
    {
        /** The information of the source channel: ChannelMediaInfo. It contains the following members:
         * - `channelName`: The name of the source channel. The default value is `null`, which means the SDK applies the name of the current channel.
         * - `uid`: The unique ID to identify the relay stream in the source channel. The default value is 0, which means the SDK generates a random UID. You must set it as 0.
         * - `token`: The token for joining the source channel. It is generated with the `channelName` and `uid` you set in `srcInfo`.
         *   - If you have not enabled the App Certificate, set this parameter as the default value `null`, which means the SDK applies the App ID.
         *   - If you have enabled the App Certificate, you must use the `token` generated with the `channelName` and `uid`, and the `uid` must be set as 0.
         */
        public ChannelMediaInfo srcInfo;
        /** The information of the destination channel: ChannelMediaInfo. It contains the following members:
         * - `channelName`: The name of the destination channel.
         * - `uid`: The unique ID to identify the relay stream in the destination channel. The value ranges from 0 to (2<sup>32</sup>-1). To avoid UID conflicts, this `uid` must be different from any other UIDs in the destination channel. The default value is 0, which means the SDK generates a random UID.
         * Do not set this parameter as the `uid` of the host in the destination channel, and ensure that this `uid` is different from any other `uid` in the channel.
         * - `token`: The token for joining the destination channel. It is generated with the `channelName` and `uid` you set in `destInfos`.
         *   - If you have not enabled the App Certificate, set this parameter as the default value `null`, which means the SDK applies the App ID.
         *   - If you have enabled the App Certificate, you must use the `token` generated with the `channelName` and `uid`.
         */
        public ChannelMediaInfo destInfos;
        /** The number of destination channels. The default value is 0, and the
        * value range is [0,4]. Ensure that the value of this parameter
        * corresponds to the number of ChannelMediaInfo structs you define in
        * `destInfos`.
        */
        public int destCount;
    };

    /** The definition of ChannelMediaInfo.
    */
    public struct ChannelMediaInfo
    {
        /** The channel name.
        */
        public string channelName;
        /** The token that enables the user to join the channel.
        */
        public string token;
        /** The user ID.
        */
        public uint uid;
    };

    /** The event code in CHANNEL_MEDIA_RELAY_EVENT. */
    public enum CHANNEL_MEDIA_RELAY_EVENT
    {
        /** 0: The user disconnects from the server due to poor network
        * connections.
        */
        RELAY_EVENT_NETWORK_DISCONNECTED = 0,
        /** 1: The network reconnects.
        */
        RELAY_EVENT_NETWORK_CONNECTED = 1,
        /** 2: The user joins the source channel.
        */
        RELAY_EVENT_PACKET_JOINED_SRC_CHANNEL = 2,
        /** 3: The user joins the destination channel.
        */
        RELAY_EVENT_PACKET_JOINED_DEST_CHANNEL = 3,
        /** 4: The SDK starts relaying the media stream to the destination channel.
        */
        RELAY_EVENT_PACKET_SENT_TO_DEST_CHANNEL = 4,
        /** 5: The server receives the video stream from the source channel.
        */
        RELAY_EVENT_PACKET_RECEIVED_VIDEO_FROM_SRC = 5,
        /** 6: The server receives the audio stream from the source channel.
        */
        RELAY_EVENT_PACKET_RECEIVED_AUDIO_FROM_SRC = 6,
        /** 7: The destination channel is updated.
        */
        RELAY_EVENT_PACKET_UPDATE_DEST_CHANNEL = 7,
        /** 8: The destination channel update fails due to internal reasons.
        */
        RELAY_EVENT_PACKET_UPDATE_DEST_CHANNEL_REFUSED = 8,
        /** 9: The destination channel does not change, which means that the
        * destination channel fails to be updated.
        */
        RELAY_EVENT_PACKET_UPDATE_DEST_CHANNEL_NOT_CHANGE = 9,
        /** 10: The destination channel name is `null`.
        */
        RELAY_EVENT_PACKET_UPDATE_DEST_CHANNEL_IS_NULL = 10,
        /** 11: The video profile is sent to the server.
        */
        RELAY_EVENT_VIDEO_PROFILE_UPDATE = 11,
        /**
        * 12: The SDK successfully pauses relaying the media stream to destination channels.
        *
        * @since v3.6.1.1
        */
        RELAY_EVENT_PAUSE_SEND_PACKET_TO_DEST_CHANNEL_SUCCESS = 12,
        /**
        * 13: The SDK fails to pause relaying the media stream to destination channels.
        *
        * @since v3.6.1.1
        */
        RELAY_EVENT_PAUSE_SEND_PACKET_TO_DEST_CHANNEL_FAILED = 13,
        /**
        * 14: The SDK successfully resumes relaying the media stream to destination channels.
        *
        * @since v3.6.1.1
        */
        RELAY_EVENT_RESUME_SEND_PACKET_TO_DEST_CHANNEL_SUCCESS = 14,
        /**
        * 15: The SDK fails to resume relaying the media stream to destination channels.
        *
        * @since v3.6.1.1
        */
        RELAY_EVENT_RESUME_SEND_PACKET_TO_DEST_CHANNEL_FAILED = 15,
    };

    /** The state code in CHANNEL_MEDIA_RELAY_STATE. */
    public enum CHANNEL_MEDIA_RELAY_STATE
    {
        /** 0: The initial state. After you successfully stop the channel media
         * relay by calling {@link agora_gaming_rtc.IRtcEngine.StopChannelMediaRelay StopChannelMediaRelay},
         * the {@link agora_gaming_rtc.OnChannelMediaRelayStateChangedHandler OnChannelMediaRelayStateChangedHandler} callback returns this state.
         */
        RELAY_STATE_IDLE = 0,
        /** 1: The SDK tries to relay the media stream to the destination channel.
        */
        RELAY_STATE_CONNECTING = 1,
        /** 2: The SDK successfully relays the media stream to the destination
        * channel.
        */
        RELAY_STATE_RUNNING = 2,
        /** 3: A failure occurs. See the details in code.
        */
        RELAY_STATE_FAILURE = 3,
    };


    /** @deprecated Video profiles. */
    public enum VIDEO_PROFILE_TYPE
    {
        /** 0: 160 &times; 120, frame rate 15 fps, bitrate 65 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_120P = 0,
        /** 2: 120 &times; 120, frame rate 15 fps, bitrate 50 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_120P_3 = 2,
        /** 10: 320&times;180, frame rate 15 fps, bitrate 140 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_180P = 10,
        /** 12: 180 &times; 180, frame rate 15 fps, bitrate 100 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_180P_3 = 12,
        /** 13: 240 &times; 180, frame rate 15 fps, bitrate 120 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_180P_4 = 13,
        /** 20: 320 &times; 240, frame rate 15 fps, bitrate 200 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_240P = 20,
        /** 22: 240 &times; 240, frame rate 15 fps, bitrate 140 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_240P_3 = 22,
        /** 23: 424 &times; 240, frame rate 15 fps, bitrate 220 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_240P_4 = 23,
        /** 30: 640 &times; 360, frame rate 15 fps, bitrate 400 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_360P = 30,
        /** 32: 360 &times; 360, frame rate 15 fps, bitrate 260 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_360P_3 = 32,
        /** 33: 640 &times; 360, frame rate 30 fps, bitrate 600 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_360P_4 = 33,
        /** 35: 360 &times; 360, frame rate 30 fps, bitrate 400 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_360P_6 = 35,
        /** 36: 480 &times; 360, frame rate 15 fps, bitrate 320 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_360P_7 = 36,
        /** 37: 480 &times; 360, frame rate 30 fps, bitrate 490 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_360P_8 = 37,
        /** 38: 640 &times; 360, frame rate 15 fps, bitrate 800 Kbps.
         * @note Interactive live streaming profile only.
         */
        VIDEO_PROFILE_LANDSCAPE_360P_9 = 38,
        /** 39: 640 &times; 360, frame rate 24 fps, bitrate 800 Kbps.
         * @note Interactive live streaming profile only.
         */
        VIDEO_PROFILE_LANDSCAPE_360P_10 = 39,
        /** 100: 640 &times; 360, frame rate 24 fps, bitrate 1000 Kbps.
         * @note Interactive live streaming profile only.
         */
        VIDEO_PROFILE_LANDSCAPE_360P_11 = 100,
        /** 40: 640 &times; 480, frame rate 15 fps, bitrate 500 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_480P = 40,
        /** 42: 480 &times; 480, frame rate 15 fps, bitrate 400 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_480P_3 = 42,
        /** 43: 640 &times; 480, frame rate 30 fps, bitrate 750 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_480P_4 = 43,
        /** 45: 480 &times; 480, frame rate 30 fps, bitrate 600 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_480P_6 = 45,
        /** 47: 848 &times; 480, frame rate 15 fps, bitrate 610 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_480P_8 = 47,
        /** 48: 848 &times; 480, frame rate 30 fps, bitrate 930 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_480P_9 = 48,
        /** 49: 640 &times; 480, frame rate 10 fps, bitrate 400 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_480P_10 = 49,
        /** 50: 1280 &times; 720, frame rate 15 fps, bitrate 1130 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_720P = 50,
        /** 52: 1280 &times; 720, frame rate 30 fps, bitrate 1710 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_720P_3 = 52,
        /** 54: 960 &times; 720, frame rate 15 fps, bitrate 910 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_720P_5 = 54,
        /** 55: 960 &times; 720, frame rate 30 fps, bitrate 1380 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_720P_6 = 55,
        /** 60: 1920 &times; 1080, frame rate 15 fps, bitrate 2080 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_1080P = 60,
        /** 62: 1920 &times; 1080, frame rate 30 fps, bitrate 3150 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_1080P_3 = 62,
        /** 64: 1920 &times; 1080, frame rate 60 fps, bitrate 4780 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_1080P_5 = 64,
        /** 66: 2560 &times; 1440, frame rate 30 fps, bitrate 4850 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_1440P = 66,
        /** 67: 2560 &times; 1440, frame rate 60 fps, bitrate 6500 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_1440P_2 = 67,
        /** 70: 3840 &times; 2160, frame rate 30 fps, bitrate 6500 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_4K = 70,
        /** 72: 3840 &times; 2160, frame rate 60 fps, bitrate 6500 Kbps. */
        VIDEO_PROFILE_LANDSCAPE_4K_3 = 72,
        /** 1000: 120 &times; 160, frame rate 15 fps, bitrate 65 Kbps. */
        VIDEO_PROFILE_PORTRAIT_120P = 1000,
        /** 1002: 120 &times; 120, frame rate 15 fps, bitrate 50 Kbps. */
        VIDEO_PROFILE_PORTRAIT_120P_3 = 1002,
        /** 1010: 180 &times; 320, frame rate 15 fps, bitrate 140 Kbps. */
        VIDEO_PROFILE_PORTRAIT_180P = 1010,
        /** 1012: 180 &times; 180, frame rate 15 fps, bitrate 100 Kbps. */
        VIDEO_PROFILE_PORTRAIT_180P_3 = 1012,
        /** 1013: 180 &times; 240, frame rate 15 fps, bitrate 120 Kbps. */
        VIDEO_PROFILE_PORTRAIT_180P_4 = 1013,
        /** 1020: 240 &times; 320, frame rate 15 fps, bitrate 200 Kbps. */
        VIDEO_PROFILE_PORTRAIT_240P = 1020,
        /** 1022: 240 &times; 240, frame rate 15 fps, bitrate 140 Kbps. */
        VIDEO_PROFILE_PORTRAIT_240P_3 = 1022,
        /** 1023: 240 &times; 424, frame rate 15 fps, bitrate 220 Kbps. */
        VIDEO_PROFILE_PORTRAIT_240P_4 = 1023,
        /** 1030: 360 &times; 640, frame rate 15 fps, bitrate 400 Kbps. */
        VIDEO_PROFILE_PORTRAIT_360P = 1030,
        /** 1032: 360 &times; 360, frame rate 15 fps, bitrate 260 Kbps. */
        VIDEO_PROFILE_PORTRAIT_360P_3 = 1032,
        /** 1033: 360 &times; 640, frame rate 30 fps, bitrate 600 Kbps. */
        VIDEO_PROFILE_PORTRAIT_360P_4 = 1033,
        /** 1035: 360 &times; 360, frame rate 30 fps, bitrate 400 Kbps. */
        VIDEO_PROFILE_PORTRAIT_360P_6 = 1035,
        /** 1036: 360 &times; 480, frame rate 15 fps, bitrate 320 Kbps. */
        VIDEO_PROFILE_PORTRAIT_360P_7 = 1036,
        /** 1037: 360 &times; 480, frame rate 30 fps, bitrate 490 Kbps. */
        VIDEO_PROFILE_PORTRAIT_360P_8 = 1037,
        /** 1038: 360 &times; 640, frame rate 15 fps, bitrate 800 Kbps.
         * @note Interactive live streaming profile only.
         */
        VIDEO_PROFILE_PORTRAIT_360P_9 = 1038,
        /** 1039: 360 &times; 640, frame rate 24 fps, bitrate 800 Kbps.
         * @note Interactive live streaming profile only.
         */
        VIDEO_PROFILE_PORTRAIT_360P_10 = 1039,
        /** 1100: 360 &times; 640, frame rate 24 fps, bitrate 1000 Kbps.
         * @note Interactive live streaming profile only.
         */
        VIDEO_PROFILE_PORTRAIT_360P_11 = 1100,
        /** 1040: 480 &times; 640, frame rate 15 fps, bitrate 500 Kbps. */
        VIDEO_PROFILE_PORTRAIT_480P = 1040,
        /** 1042: 480 &times; 480, frame rate 15 fps, bitrate 400 Kbps. */
        VIDEO_PROFILE_PORTRAIT_480P_3 = 1042,
        /** 1043: 480 &times; 640, frame rate 30 fps, bitrate 750 Kbps. */
        VIDEO_PROFILE_PORTRAIT_480P_4 = 1043,
        /** 1045: 480 &times; 480, frame rate 30 fps, bitrate 600 Kbps. */
        VIDEO_PROFILE_PORTRAIT_480P_6 = 1045,
        /** 1047: 480 &times; 848, frame rate 15 fps, bitrate 610 Kbps. */
        VIDEO_PROFILE_PORTRAIT_480P_8 = 1047,
        /** 1048: 480 &times; 848, frame rate 30 fps, bitrate 930 Kbps. */
        VIDEO_PROFILE_PORTRAIT_480P_9 = 1048,
        /** 1049: 480 &times; 640, frame rate 10 fps, bitrate 400 Kbps. */
        VIDEO_PROFILE_PORTRAIT_480P_10 = 1049,
        /** 1050: 720 &times; 1280, frame rate 15 fps, bitrate 1130 Kbps. */
        VIDEO_PROFILE_PORTRAIT_720P = 1050,
        /** 1052: 720 &times; 1280, frame rate 30 fps, bitrate 1710 Kbps. */
        VIDEO_PROFILE_PORTRAIT_720P_3 = 1052,
        /** 1054: 720 &times; 960, frame rate 15 fps, bitrate 910 Kbps. */
        VIDEO_PROFILE_PORTRAIT_720P_5 = 1054,
        /** 1055: 720 &times; 960, frame rate 30 fps, bitrate 1380 Kbps. */
        VIDEO_PROFILE_PORTRAIT_720P_6 = 1055,
        /** 1060: 1080 &times; 1920, frame rate 15 fps, bitrate 2080 Kbps. */
        VIDEO_PROFILE_PORTRAIT_1080P = 1060,
        /** 1062: 1080 &times; 1920, frame rate 30 fps, bitrate 3150 Kbps. */
        VIDEO_PROFILE_PORTRAIT_1080P_3 = 1062,
        /** 1064: 1080 &times; 1920, frame rate 60 fps, bitrate 4780 Kbps. */
        VIDEO_PROFILE_PORTRAIT_1080P_5 = 1064,
        /** 1066: 1440 &times; 2560, frame rate 30 fps, bitrate 4850 Kbps. */
        VIDEO_PROFILE_PORTRAIT_1440P = 1066,
        /** 1067: 1440 &times; 2560, frame rate 60 fps, bitrate 6500 Kbps. */
        VIDEO_PROFILE_PORTRAIT_1440P_2 = 1067,
        /** 1070: 2160 &times; 3840, frame rate 30 fps, bitrate 6500 Kbps. */
        VIDEO_PROFILE_PORTRAIT_4K = 1070,
        /** 1072: 2160 &times; 3840, frame rate 60 fps, bitrate 6500 Kbps. */
        VIDEO_PROFILE_PORTRAIT_4K_3 = 1072,
        /** Default 640 &times; 360, frame rate 15 fps, bitrate 400 Kbps. */
        VIDEO_PROFILE_DEFAULT = VIDEO_PROFILE_LANDSCAPE_360P,
    };

    /** The definition of #CHANNEL_MEDIA_RELAY_ERROR. */
    public enum CHANNEL_MEDIA_RELAY_ERROR
    {
        /** 0: The state is normal.
        */
        RELAY_OK = 0,
        /** 1: An error occurs in the server response.
        */
        RELAY_ERROR_SERVER_ERROR_RESPONSE = 1,
        /** 2: No server response. You can call the
        * {@link agora_gaming_rtc.IRtcEngine.LeaveChannel LeaveChannel} method to
        * leave the channel.
        *
        * This error can also occur if your project has not enabled co-host token
        * authentication. Contact support@agora.io to enable the co-host token
        * authentication service before starting a channel media relay.
        */
        RELAY_ERROR_SERVER_NO_RESPONSE = 2,
        /** 3: The SDK fails to access the service, probably due to limited
        * resources of the server.
        */
        RELAY_ERROR_NO_RESOURCE_AVAILABLE = 3,
        /** 4: Fails to send the relay request.
        */
        RELAY_ERROR_FAILED_JOIN_SRC = 4,
        /** 5: Fails to accept the relay request.
        */
        RELAY_ERROR_FAILED_JOIN_DEST = 5,
        /** 6: The server fails to receive the media stream.
        */
        RELAY_ERROR_FAILED_PACKET_RECEIVED_FROM_SRC = 6,
        /** 7: The server fails to send the media stream.
        */
        RELAY_ERROR_FAILED_PACKET_SENT_TO_DEST = 7,
        /** 8: The SDK disconnects from the server due to poor network
        * connections. You can call the {@link agora_gaming_rtc.IRtcEngine.LeaveChannel LeaveChannel} method to leave the channel.
        */
        RELAY_ERROR_SERVER_CONNECTION_LOST = 8,
        /** 9: An internal error occurs in the server.
        */
        RELAY_ERROR_INTERNAL_ERROR = 9,
        /** 10: The token of the source channel has expired.
        */
        RELAY_ERROR_SRC_TOKEN_EXPIRED = 10,
        /** 11: The token of the destination channel has expired.
        */
        RELAY_ERROR_DEST_TOKEN_EXPIRED = 11,
    };

    /** Metadata type of the observer.
     * @note We only support video metadata for now.
     */
    public enum METADATA_TYPE
    {
        /** -1: The metadata type is unknown.
         */
        UNKNOWN_METADATA = -1,
        /** 0: The metadata type is video.
         */
        VIDEO_METADATA = 0,
    };

    /** The definition of Metadata. */
    public struct Metadata
    {
        /** The User ID.
         * - For the receiver: The ID of the user who sent the metadata.
         * - For the sender: Ignore it.
         */
        public uint uid;
        /** The buffer size of the sent or received metadata.
         */
        public uint size;
        /** The buffer address of the sent or received metadata.
         */
        public byte[] buffer;
        /** Time statmp of the frame following the metadata.
        */
        public long timeStampMs;
    };

    /** Video display settings of the VideoCanvas class.
    */
    public struct VideoCanvas
    {
        /** Video display window (view).
        */
        public int hwnd;
        /** The rendering mode of the video view. See RENDER_MODE_TYPE.
        */
        public RENDER_MODE_TYPE renderMode;
        /** The user ID. */
        public uint uid;
        public IntPtr priv; // private data (underlying video engine denotes it)
    };

    /** Video display modes. */
    public enum RENDER_MODE_TYPE
    {
        /** 1: Uniformly scale the video until it fills the visible boundaries (cropped). One dimension of the video may have clipped contents.
        */
        RENDER_MODE_HIDDEN = 1,
        /** 2: Uniformly scale the video until one of its dimension fits the boundary (zoomed to fit). Areas that are not filled due to disparity in the aspect ratio are filled with black.
        */
        RENDER_MODE_FIT = 2,
        /** @deprecated 3: This mode is deprecated.
        */
        RENDER_MODE_ADAPTIVE = 3,
    };

    /** Encryption mode.
     */
    public enum ENCRYPTION_MODE
    {
        /** 1: (Default) 128-bit AES encryption, XTS mode.
         */
        AES_128_XTS = 1,
        /** 2: 128-bit AES encryption, ECB mode.
         */
        AES_128_ECB = 2,
        /** 3: 256-bit AES encryption, XTS mode.
         */
        AES_256_XTS = 3,
        /** 4: 128-bit SM4 encryption, ECB mode.
         */
        SM4_128_ECB = 4,
        /** 5: 128-bit AES encryption, GCM mode.
         *
         * @since v3.3.1
         */
        AES_128_GCM = 5,
        /** 6: 256-bit AES encryption, GCM mode.
         *
         * @since v3.3.1
         */
        AES_256_GCM = 6,
        /** 7: (Default) 128-bit AES encryption, GCM mode, with custom KDF salt.
        *
        * @since v3.4.5
        */
        AES_128_GCM2 = 7,
        /** 8: 256-bit AES encryption, GCM mode, with custom KDF salt.
        *
        * @since v3.4.5
        */
        AES_256_GCM2 = 8,
        /** Enumerator boundary.
         */
        MODE_END,
    };

    /** Configurations of built-in encryption schemas. */
    public class EncryptionConfig{
        /**
        * Encryption mode. The default encryption mode is `AES_128_XTS`. See #ENCRYPTION_MODE.
        */
        public ENCRYPTION_MODE encryptionMode {
            get;
            set;
        }
        /**
        * Encryption key in string type.
        *
        * @note If you do not set an encryption key or set it as `NULL`, you cannot use the built-in encryption, and the SDK returns `ERR_INVALID_ARGUMENT (-2)`.
        */
        public string encryptionKey {
            get;
            set;
        }
        /** The salt with the length of 32 bytes. Agora recommends using OpenSSL to generate the salt on your server.
        *
        * @note his parameter is only valid when you set the encryption mode as `AES_128_GCM2` or `AES_256_GCM2`. In this case, ensure that this parameter is not `0`.
        */
        public byte[] encryptionKdfSalt {
            get;
            set;
        }

        public EncryptionConfig() {
            encryptionMode = ENCRYPTION_MODE.AES_128_XTS;
            encryptionKey = "";
            encryptionKdfSalt = new byte[32];
        }
    };

    /** Events during the RTMP or RTMPS streaming. */
    public enum RTMP_STREAMING_EVENT
    {
        /** An error occurs when you add a background image or a watermark image to the RTMP or RTMPS stream.
        */
        RTMP_STREAMING_EVENT_FAILED_LOAD_IMAGE = 1,
        /** The chosen URL address is already in use for CDN live streaming.
        */
        RTMP_STREAMING_EVENT_URL_ALREADY_IN_USE = 2,
        /**
        * 3: The feature is not supported.
        *
        * @since v3.6.1.1
        */
        RTMP_STREAMING_EVENT_ADVANCED_FEATURE_NOT_SUPPORT = 3,
        /**
        * 4: Reserved.
        *
        * @since v3.6.1.1
        */
        RTMP_STREAMING_EVENT_REQUEST_TOO_OFTEN = 4,
    };

    /** The publishing state.
    */
    public enum STREAM_PUBLISH_STATE {
        /** 0: The initial publishing state after joining the channel.
        */
        PUB_STATE_IDLE = 0,
        /** 1: Fails to publish the local stream. Possible reasons:
        * - The local user calls {@link agora_gaming_rtc.IRtcEngine.MuteLocalAudioStream MuteLocalAudioStream}(true) or {@link agora_gaming_rtc.IRtcEngine.MuteLocalVideoStream MuteLocalVideoStream}(true) to stop sending local streams.
        * - The local user calls {@link agora_gaming_rtc.IRtcEngine.DisableAudio DisableAudio} or {@link agora_gaming_rtc.IRtcEngine.DisableVideo DisableVideo} to disable the entire audio or video module.
        * - The local user calls {@link agora_gaming_rtc.IRtcEngine.EnableLocalAudio EnableLocalAudio}(false) or {@link agora_gaming_rtc.IRtcEngine.EnableLocalVideo EnableLocalVideo}(false) to disable the local audio sampling or video capturing.
        * - The role of the local user is `AUDIENCE`.
        */
        PUB_STATE_NO_PUBLISHED = 1,
        /** 2: Publishing.
        */
        PUB_STATE_PUBLISHING = 2,
        /** 3: Publishes successfully.
        */
        PUB_STATE_PUBLISHED = 3
    };
    /** The subscribing state.
    */
    public enum STREAM_SUBSCRIBE_STATE {
        /** 0: The initial subscribing state after joining the channel.
        */
        SUB_STATE_IDLE = 0,
        /** 1: Fails to subscribe to the remote stream. Possible reasons:
        * - The remote user:
        *  - Calls {@link agora_gaming_rtc.IRtcEngine.MuteLocalAudioStream MuteLocalAudioStream}(true) or {@link agora_gaming_rtc.IRtcEngine.MuteLocalVideoStream MuteLocalVideoStream}(true) to stop sending local streams.
        *  - Calls {@link agora_gaming_rtc.IRtcEngine.DisableAudio DisableAudio} or {@link agora_gaming_rtc.IRtcEngine.DisableVideo DisableVideo} to disable the entire audio or video modules.
        *  - Calls {@link agora_gaming_rtc.IRtcEngine.EnableLocalAudio EnableLocalAudio}(false) or {@link agora_gaming_rtc.IRtcEngine.EnableLocalVideo EnableLocalVideo}(false) to disable the local audio sampling or video capturing.
        *  - The role of the remote user is `AUDIENCE`.
        * - The local user calls the following methods to stop receiving remote streams:
        *  - Calls {@link agora_gaming_rtc.IRtcEngine.MuteRemoteAudioStream MuteRemoteAudioStream}(true), {@link agora_gaming_rtc.IRtcEngine.MuteAllRemoteAudioStreams MuteAllRemoteAudioStreams}(true), or {@link agora_gaming_rtc.IRtcEngine.SetDefaultMuteAllRemoteAudioStreams SetDefaultMuteAllRemoteAudioStreams}(true) to stop receiving remote audio streams.
        *  - Calls {@link agora_gaming_rtc.IRtcEngine.MuteRemoteVideoStream MuteRemoteVideoStream}(true), {@link agora_gaming_rtc.IRtcEngine.MuteAllRemoteVideoStreams MuteAllRemoteVideoStreams}(true), or {@link agora_gaming_rtc.IRtcEngine.SetDefaultMuteAllRemoteVideoStreams SetDefaultMuteAllRemoteVideoStreams}(true) to stop receiving remote video streams.
        */
        SUB_STATE_NO_SUBSCRIBED = 1,
        /** 2: Subscribing.
        */
        SUB_STATE_SUBSCRIBING = 2,
        /** 3: Subscribes to and receives the remote stream successfully.
        */
        SUB_STATE_SUBSCRIBED = 3
    };

    /** The latency level of an audience member in interactive live streaming.
     * @note Takes effect only when the user role is `CLIENT_ROLE_AUDIENCE`.
     */
    public enum AUDIENCE_LATENCY_LEVEL_TYPE
    {
        /** 1: Low latency. */
        AUDIENCE_LATENCY_LEVEL_LOW_LATENCY = 1,
        /** 2: (Default) Ultra low latency. */
        AUDIENCE_LATENCY_LEVEL_ULTRA_LOW_LATENCY = 2,
    };

    /** The detailed options of a user.
    */
    public struct ClientRoleOptions
    {
        /** The latency level of an audience member in a interactive live streaming. See #AUDIENCE_LATENCY_LEVEL_TYPE.
        */
        public AUDIENCE_LATENCY_LEVEL_TYPE audienceLatencyLevel {
            get;
            set;
        }
    };

    /** The configurations for the data stream.
     *
     * @since v3.3.1
     *
     * |`syncWithAudio` |`ordered`| SDK behaviors|
     * |--------------|--------|-------------|
     * | false   |  false   |The SDK triggers the {@link agora_gaming_rtc.OnStreamMessageHandler OnStreamMessageHandler} or {@link agora_gaming_rtc.ChannelOnStreamMessageHandler ChannelOnStreamMessageHandler} callback immediately after the receiver receives a data packet.      |
     * | true |  false | <p>If the data packet delay is within the audio delay, the SDK triggers the `OnStreamMessageHandler` or `ChannelOnStreamMessageHandler` callback when the synchronized audio packet is played out.</p><p>If the data packet delay exceeds the audio delay, the SDK triggers the `OnStreamMessageHandler` or `ChannelOnStreamMessageHandler` callback as soon as the data packet is received. In this case, the data packet is not synchronized with the audio packet.</p>   |
     * | false  |  true | <p>If the delay of a data packet is within five seconds, the SDK corrects the order of the data packet.</p><p>If the delay of a data packet exceeds five seconds, the SDK discards the data packet.</p>     |
     * |  true  |  true   | <p>If the delay of a data packet is within the audio delay, the SDK corrects the order of the data packet.</p><p>If the delay of a data packet exceeds the audio delay, the SDK discards this data packet.</p>     |
     */
    public struct DataStreamConfig {
        /** Whether to synchronize the data packet with the published audio packet.
        *
        * - true: Synchronize the data packet with the audio packet.
        * - false: Do not synchronize the data packet with the audio packet.
        *
        * When you set the data packet to synchronize with the audio, then if the data
        * packet delay is within the audio delay, the SDK triggers the
        * {@link agora_gaming_rtc.OnStreamMessageHandler OnStreamMessageHandler} or
        * {@link agora_gaming_rtc.ChannelOnStreamMessageHandler ChannelOnStreamMessageHandler} callback when
        * the synchronized audio packet is played out. Do not set this parameter as `true` if you
        * need the receiver to receive the data packet immediately. Agora recommends that you set
        * this parameter to `true` only when you need to implement specific functions, for example
        * lyric synchronization.
        */
        public bool syncWithAudio;
        /** Whether the SDK guarantees that the receiver receives the data in the sent order.
        *
        * - true: Guarantee that the receiver receives the data in the sent order.
        * - false: Do not guarantee that the receiver receives the data in the sent order.
        *
        * Do not set this parameter to `true` if you need the receiver to receive the data immediately.
        */
        public bool ordered;
    };

    /** The options for SDK preset voice beautifier effects.
     */
    public enum VOICE_BEAUTIFIER_PRESET
    {
        /** Turn off voice beautifier effects and use the original voice.
        */
        VOICE_BEAUTIFIER_OFF = 0x00000000,
        /** A more magnetic voice.
         * @note Agora recommends using this enumerator to process a male-sounding voice; otherwise, you may experience vocal distortion.
         */
        CHAT_BEAUTIFIER_MAGNETIC = 0x01010100,
        /** A fresher voice.
         * @note Agora recommends using this enumerator to process a female-sounding voice; otherwise, you may experience vocal distortion.
         */
        CHAT_BEAUTIFIER_FRESH = 0x01010200,
        /** A more vital voice.
         * @note Agora recommends using this enumerator to process a female-sounding voice; otherwise, you may experience vocal distortion.
         */
        CHAT_BEAUTIFIER_VITALITY = 0x01010300,
        /**
         * @since v3.3.1
         *
         * Singing beautifier effect.
         * - If you call {@link agora_gaming_rtc.IRtcEngine.SetVoiceBeautifierPreset SetVoiceBeautifierPreset(SINGING_BEAUTIFIER)},
         * you can beautify a male-sounding voice and add a reverberation effect that sounds like singing in a small room.
         * Agora recommends not using `SetVoiceBeautifierPreset(SINGING_BEAUTIFIER)` to process a female-sounding voice;
         * otherwise, you may experience vocal distortion.
         * - If you call {@link agora_gaming_rtc.IRtcEngine.SetVoiceBeautifierParameters SetVoiceBeautifierParameters(SINGING_BEAUTIFIER, param1, param2)},
         * you can beautify a male- or female-sounding voice and add a reverberation effect.
         */
        SINGING_BEAUTIFIER = 0x01020100,
        /** A more vigorous voice.
         */
        TIMBRE_TRANSFORMATION_VIGOROUS = 0x01030100,
        /** A deeper voice.
         */
        TIMBRE_TRANSFORMATION_DEEP = 0x01030200,
        /** A mellower voice.
         */
        TIMBRE_TRANSFORMATION_MELLOW = 0x01030300,
        /** A falsetto voice.
         */
        TIMBRE_TRANSFORMATION_FALSETTO = 0x01030400,
        /** A fuller voice.
         */
        TIMBRE_TRANSFORMATION_FULL = 0x01030500,
        /** A clearer voice.
         */
        TIMBRE_TRANSFORMATION_CLEAR = 0x01030600,
        /** A more resounding voice.
         */
        TIMBRE_TRANSFORMATION_RESOUNDING = 0x01030700,
        /** A more ringing voice.
         */
        TIMBRE_TRANSFORMATION_RINGING = 0x01030800
    };

    /** The options for SDK preset audio effects.
     */
    public enum AUDIO_EFFECT_PRESET
    {
        /** Turn off audio effects and use the original voice.
        */
        AUDIO_EFFECT_OFF = 0x00000000,
        /** An audio effect typical of a KTV venue.
         * @note To achieve better audio effect quality, Agora recommends calling {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile}
         * and setting the `profile` parameter to `AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)` or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)`
         * before setting this enumerator.
         */
        ROOM_ACOUSTICS_KTV = 0x02010100,
        /** An audio effect typical of a concert hall.
         * @note To achieve better audio effect quality, Agora recommends calling {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile}
         * and setting the `profile` parameter to `AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)` or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)`
         * before setting this enumerator.
         */
        ROOM_ACOUSTICS_VOCAL_CONCERT = 0x02010200,
        /** An audio effect typical of a recording studio.
         * @note To achieve better audio effect quality, Agora recommends calling {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile}
         * and setting the `profile` parameter to `AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)` or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)`
         * before setting this enumerator.
         */
        ROOM_ACOUSTICS_STUDIO = 0x02010300,
        /** An audio effect typical of a vintage phonograph.
         * @note To achieve better audio effect quality, Agora recommends calling {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile}
         * and setting the `profile` parameter to `AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)` or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)`
         * before setting this enumerator.
         */
        ROOM_ACOUSTICS_PHONOGRAPH = 0x02010400,
        /** A virtual stereo effect that renders monophonic audio as stereo audio.
         * @note Call {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile} and set the `profile` parameter to
         * `AUDIO_PROFILE_MUSIC_STANDARD_STEREO(3)` or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)` before setting this
         * enumerator; otherwise, the enumerator setting does not take effect.
         */
        ROOM_ACOUSTICS_VIRTUAL_STEREO = 0x02010500,
        /** A more spatial audio effect.
         * @note To achieve better audio effect quality, Agora recommends calling {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile}
         * and setting the `profile` parameter to `AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)` or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)`
         * before setting this enumerator.
         */
        ROOM_ACOUSTICS_SPACIAL = 0x02010600,
        /** A more ethereal audio effect.
         * @note To achieve better audio effect quality, Agora recommends calling {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile}
         * and setting the `profile` parameter to `AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)` or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)`
         * before setting this enumerator.
         */
        ROOM_ACOUSTICS_ETHEREAL = 0x02010700,
        /** A 3D voice effect that makes the voice appear to be moving around the user. The default cycle period of the 3D
         * voice effect is 10 seconds. To change the cycle period, call {@link agora_gaming_rtc.IRtcEngine.SetAudioEffectParameters SetAudioEffectParameters}
         * after this method.
         * @note
         * - Call {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile} and set the `profile` parameter to `AUDIO_PROFILE_MUSIC_STANDARD_STEREO(3)`
         * or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)` before setting this enumerator; otherwise, the enumerator setting does not take effect.
         * - If the 3D voice effect is enabled, users need to use stereo audio playback devices to hear the anticipated voice effect.
         */
        ROOM_ACOUSTICS_3D_VOICE = 0x02010800,
        /** The voice of an uncle.
         * @note
         * - Agora recommends using this enumerator to process a male-sounding voice; otherwise, you may not hear the anticipated voice effect.
         * - To achieve better audio effect quality, Agora recommends calling {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile} and
         * setting the `profile` parameter to `AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)` or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)` before
         * setting this enumerator.
         */
        VOICE_CHANGER_EFFECT_UNCLE = 0x02020100,
        /** The voice of an old man.
         * @note
         * - Agora recommends using this enumerator to process a male-sounding voice; otherwise, you may not hear the anticipated voice effect.
         * - To achieve better audio effect quality, Agora recommends calling {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile} and setting
         * the `profile` parameter to `AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)` or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)` before setting
         * this enumerator.
         */
        VOICE_CHANGER_EFFECT_OLDMAN = 0x02020200,
        /** The voice of a boy.
         * @note
         * - Agora recommends using this enumerator to process a male-sounding voice; otherwise, you may not hear the anticipated voice effect.
         * - To achieve better audio effect quality, Agora recommends calling {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile} and setting
         * the `profile` parameter to `AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)` or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)` before
         * setting this enumerator.
         */
        VOICE_CHANGER_EFFECT_BOY = 0x02020300,
        /** The voice of a young woman.
         * @note
         * - Agora recommends using this enumerator to process a female-sounding voice; otherwise, you may not hear the anticipated voice effect.
         * - To achieve better audio effect quality, Agora recommends calling {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile} and setting
         * the `profile` parameter to `AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)` or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)` before
         * setting this enumerator.
         */
        VOICE_CHANGER_EFFECT_SISTER = 0x02020400,
        /** The voice of a girl.
         * @note
         * - Agora recommends using this enumerator to process a female-sounding voice; otherwise, you may not hear the anticipated voice effect.
         * - To achieve better audio effect quality, Agora recommends calling {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile} and setting
         * the `profile` parameter to `AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)` or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)` before
         * setting this enumerator.
         */
        VOICE_CHANGER_EFFECT_GIRL = 0x02020500,
        /** The voice of Pig King, a character in Journey to the West who has a voice like a growling bear.
         * @note To achieve better audio effect quality, Agora recommends calling {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile} and
         * setting the `profile` parameter to `AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)` or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)` before
         * setting this enumerator.
         */
        VOICE_CHANGER_EFFECT_PIGKING = 0x02020600,
        /** The voice of Hulk.
         * @note To achieve better audio effect quality, Agora recommends calling {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile} and
         * setting the `profile` parameter to `AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)` or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)` before
         * setting this enumerator.
         */
        VOICE_CHANGER_EFFECT_HULK = 0x02020700,
        /** An audio effect typical of R&B music.
         * @note Call {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile} and set the `profile` parameter to `AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)`
         * or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)` before setting this enumerator; otherwise, the enumerator setting does not take effect.
         */
        STYLE_TRANSFORMATION_RNB = 0x02030100,
        /** An audio effect typical of popular music.
         * @note Call {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile} and set the `profile` parameter to `AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)`
         * or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)` before setting this enumerator; otherwise, the enumerator setting does not take effect.
         */
        STYLE_TRANSFORMATION_POPULAR = 0x02030200,
        /** A pitch correction effect that corrects the user's pitch based on the pitch of the natural C major scale.
         * To change the basic mode and tonic pitch, call {@link agora_gaming_rtc.IRtcEngine.SetAudioEffectParameters SetAudioEffectParameters} after this method.
         * @note To achieve better audio effect quality, Agora recommends calling {@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile} and
         * setting the `profile` parameter to `AUDIO_PROFILE_MUSIC_HIGH_QUALITY(4)` or `AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO(5)` before
         * setting this enumerator.
         */
        PITCH_CORRECTION = 0x02040100
    };


    /** The reason why super resolution is not successfully enabled or the message that confirms success.
     *
     * @since v3.6.1.1
     */
    public enum SUPER_RESOLUTION_STATE_REASON
    {
        /** 0: Super resolution is successfully enabled.
         */
        SR_STATE_REASON_SUCCESS = 0,
        /** 1: The original resolution of the remote video is beyond the range where super resolution can be applied.
         */
        SR_STATE_REASON_STREAM_OVER_LIMITATION = 1,
        /** 2: Super resolution is already being used to boost another remote user's video.
         */
        SR_STATE_REASON_USER_COUNT_OVER_LIMITATION = 2,
        /** 3: The device does not support using super resolution.
         */
        SR_STATE_REASON_DEVICE_NOT_SUPPORTED = 3,
        /** 4: Insufficient device performance，It is recommended to turn off super resolution.
        */
        SR_STATE_REASON_INSUFFICIENT_PERFORMANCE = 4,
    };

    /** The brightness level of the video image captured by the local camera.
     *
     * @since v3.3.1
     */
    public enum CAPTURE_BRIGHTNESS_LEVEL_TYPE {
        /** -1: The SDK does not detect the brightness level of the video image.
         * Wait a few seconds to get the brightness level from `CAPTURE_BRIGHTNESS_LEVEL_TYPE` in the next callback.
         */
        CAPTURE_BRIGHTNESS_LEVEL_INVALID = -1,
        /** 0: The brightness level of the video image is normal.
         */
        CAPTURE_BRIGHTNESS_LEVEL_NORMAL = 0,
        /** 1: The brightness level of the video image is too bright.
         */
        CAPTURE_BRIGHTNESS_LEVEL_BRIGHT = 1,
        /** 2: The brightness level of the video image is too dark.
         */
        CAPTURE_BRIGHTNESS_LEVEL_DARK = 2,
    };

    /** The proxy type.
     * @since 3.6.2
     */
    public enum PROXY_TYPE {
        /** 0: Reserved for future use.
        */
        NONE_PROXY_TYPE = 0,
        /**
         * 1: The cloud proxy for the UDP protocol, that is, the Force UDP cloud proxy mode. In this mode, the SDK always transmits data over UDP.
        */
        UDP_PROXY_TYPE = 1,
        /**
         * 2: The cloud proxy for the TCP (encryption) protocol, that is, the Force TCP cloud proxy mode. In this mode, the SDK always transmits data over TLS 443.
        */
        TCP_PROXY_TYPE = 2,
        /** 3: Reserved for future use.
        */
        LOCAL_PROXY_TYPE = 3,
        /**
         * 4: The automatic mode. In this mode, the SDK attempts a direct connection to SD-RTN™ and automatically switches to TLS 443 if the attempt fails.
        */
        TCP_PROXY_AUTO_FALLBACK_TYPE = 4,
    };

    /** The options for SDK preset voice conversion effects.
     *
     * @since v3.3.1
     */
    public enum VOICE_CONVERSION_PRESET {
        /** Turn off voice conversion effects and use the original voice.
         */
        VOICE_CONVERSION_OFF = 0x00000000,
        /** A gender-neutral voice. To avoid audio distortion, ensure that you use
         * this enumerator to process a female-sounding voice.
         */
        VOICE_CHANGER_NEUTRAL = 0x03010100,
        /** A sweet voice. To avoid audio distortion, ensure that you use this
         * enumerator to process a female-sounding voice.
         */
        VOICE_CHANGER_SWEET = 0x03010200,
        /** A steady voice. To avoid audio distortion, ensure that you use this
         * enumerator to process a male-sounding voice.
         */
        VOICE_CHANGER_SOLID = 0x03010300,
        /** A deep voice. To avoid audio distortion, ensure that you use this
         * enumerator to process a male-sounding voice.
         */
        VOICE_CHANGER_BASS = 0x03010400
    };

    /// @cond
    /**
     * The reason for the upload failure.
     *
     * @since v3.3.1
     */
    public enum UPLOAD_ERROR_REASON
    {
        /** 0: The log file is successfully uploaded.
         */
        UPLOAD_SUCCESS = 0,
        /**
         * 1: Network error. Check the network connection and call
         * {@link agora_gaming_rtc.IRtcEngine.UploadLogFile UploadLogFile}
         * again to upload the log file.
         */
        UPLOAD_NET_ERROR = 1,
        /**
         * 2: An error occurs in the Agora server. Try uploading the log files later.
         */
        UPLOAD_SERVER_ERROR = 2,
    };
    /// @endcond

    /** Quality of experience (QoE) of the local user when receiving a remote audio stream.
     *
     * @since v3.3.1
     */
    public enum EXPERIENCE_QUALITY_TYPE
    {
        /** 0: QoE of the local user is good.  */
        EXPERIENCE_QUALITY_GOOD = 0,
        /** 1: QoE of the local user is poor.  */
        EXPERIENCE_QUALITY_BAD = 1,
    };

    /**
     * The reason for poor QoE of the local user when receiving a remote audio stream.
     *
     * @since v3.3.1
     */
    public enum EXPERIENCE_POOR_REASON {
        /** 0: No reason, indicating good QoE of the local user.
         */
        EXPERIENCE_REASON_NONE = 0,
        /** 1: The remote user's network quality is poor.
         */
        REMOTE_NETWORK_QUALITY_POOR = 1,
        /** 2: The local user's network quality is poor.
         */
        LOCAL_NETWORK_QUALITY_POOR = 2,
        /** 4: The local user's Wi-Fi or mobile network signal is weak.
         */
        WIRELESS_SIGNAL_POOR = 4,
        /** 8: The local user enables both Wi-Fi and bluetooth, and their signals interfere with each other.
         * As a result, audio transmission quality is undermined.
         */
        WIFI_BLUETOOTH_COEXIST = 8,
    };

    /**
     * Recording configuration, which is set in {@link agora_gaming_rtc.IRtcEngine.StartAudioRecording(AudioRecordingConfiguration config) StartAudioRecording}.
     */
    public struct AudioRecordingConfiguration {
        /**
         * The absolute path (including the filename extensions) of the recording file. For example: `C:\music\audio.aac`.
         * Ensure that the path you specify exists and is writable.
         */
        public string filePath;
        /**
         * Audio recording quality. See #AUDIO_RECORDING_QUALITY_TYPE.
         * @note This parameter applies to AAC files only.
         */
        public AUDIO_RECORDING_QUALITY_TYPE recordingQuality;
        /**
         * Recording content. See #AUDIO_RECORDING_POSITION.
         */
        public AUDIO_RECORDING_POSITION recordingPosition;
        /**
         * Recording sample rate (Hz). The following values are supported:
         * - 16000
         * - (Default) 32000
         * - 44100
         * - 48000
         * @note
         * If this parameter is set to 44100 or 48000, for better recording effects, Agora recommends recording
         * WAV files or AAC files whose `recordingQuality` is `AUDIO_RECORDING_QUALITY_MEDIUM` or `AUDIO_RECORDING_QUALITY_HIGH`.
         */
        public int recordingSampleRate;
        /**
         * The recorded audio channel. The following values are supported:
         * - `1`: (Default) Mono channel.
         * - `2`: Dual channel.
         *
         * @note
         * The actual recorded audio channel is related to the audio channel that you capture. If the captured audio
         * is mono and `recordingChannel` is 2, the recorded audio is the dual-channel data that is copied from mono
         * data, not stereo. If the captured audio is dual channel and `recordingChannel` is 1, the recorded audio is
         * the mono data that is mixed by dual-channel data. The integration scheme also affects the final recorded audio
         * channel. Therefore, to record in stereo, contact technical support for assistance.
         */
        public int recordingChannel;
    };

   /** The type of the custom background image.
    */
    public enum BACKGROUND_SOURCE_TYPE {
        /**
         * 1: (Default) The background image is a solid color.
         */
        BACKGROUND_COLOR = 1,
        /**
         * The background image is a file in PNG or JPG format.
         */
        BACKGROUND_IMG,
        /**
        * The background image is blurred.
        *
        * @since v3.6.1.1
        */
        BACKGROUND_BLUR,
        /**
        * The background image is a mp4 or avi file.
        *
        * @since v3.6.1.1
        */
        BACKGROUND_VIDEO
    };

    /**
    * The degree of blurring applied to the custom background image.
    *
    * @since v3.6.1.1
    */
    public enum BACKGROUND_BLUR_DEGREE {
        /**
        * 1: The degree of blurring applied to the custom background image is low.
        * The user can almost see the background clearly.
        */
        BLUR_DEGREE_LOW = 1,
        /**
        * The degree of blurring applied to the custom background image is medium.
        * It is difficult for the user to recognize details in the background.
        */
        BLUR_DEGREE_MEDIUM,
        /**
        * (Default) The degree of blurring applied to the custom background image is high.
        * The user can barely see any distinguishing features in the background.
        */
        BLUR_DEGREE_HIGH,
    };


    /** The custom background image.
     * @since 3.4.5
     */
    public struct VirtualBackgroundSource {
        /**
         * The type of the custom background image. See #BACKGROUND_SOURCE_TYPE.
        */
        public BACKGROUND_SOURCE_TYPE background_source_type;

        /** The color of the custom background image. The format is a hexadecimal integer defined by RGB,
        * without the # sign, such as 0xFFB6C1 for light pink. The default value is 0xFFFFFF, which signifies
        * white. The value range is [0x000000,0xFFFFFF]. If the value is invalid, the SDK replaces the original
        * background image with a white background image.
        *
        * @note This parameter takes effect only when the type of the custom background image is `BACKGROUND_COLOR`.
        */
        public uint color;

        /** The local absolute path of the custom background image. PNG and JPG formats are supported. If the path is invalid, the SDK replaces the original background image with a white background image.
         *
         * @note This parameter takes effect only when the type of the custom background image is `BACKGROUND_IMG`.
         */
        public string source;

        /** The degree of blurring applied to the custom background image. See #BACKGROUND_BLUR_DEGREE.
         *
         * @note This parameter takes effect only when the type of the custom background image is `BACKGROUND_BLUR`.
         */
        public BACKGROUND_BLUR_DEGREE  blur_degree;

        /** Boolean settings for muting or looping the video virtual background.
         *
         * @note These parameters take effect only when the type of the custom background image is `BACKGROUND_VIDEO`.
         */
        public bool loop, mute;
    };

    /** The reason why the virtual background is not successfully enabled or the message that confirms success.
     */
    public enum VIRTUAL_BACKGROUND_SOURCE_STATE_REASON {
        /**
         * 0: The virtual background is successfully enabled.
         */
        VIRTUAL_BACKGROUND_SOURCE_STATE_REASON_SUCCESS = 0,
        /**
         * 1: The custom background image does not exist. Please check the value of `source` in {@link agora_gaming_rtc.VirtualBackgroundSource VirtualBackgroundSource}.
         */
        VIRTUAL_BACKGROUND_SOURCE_STATE_REASON_IMAGE_NOT_EXIST = 1,
        /**
         * 2: The color format of the custom background image is invalid. Please check the value of `color` in `VirtualBackgroundSource`.
         */
        VIRTUAL_BACKGROUND_SOURCE_STATE_REASON_COLOR_FORMAT_NOT_SUPPORTED = 2,
        /**
         * 3: The device does not support using the virtual background.
         */
        VIRTUAL_BACKGROUND_SOURCE_STATE_REASON_DEVICE_NOT_SUPPORTED = 3,
        /**
        * 4: Insufficient device performance，It is recommended to turn off virtual background.
        */
        VIRTUAL_BACKGROUND_SOURCE_STATE_REASON_INSUFFICIENT_PERFORMANCE = 4,
    };

    /**
    * The channel mode. Set in {@link agora_gaming_rtc.IRtcEngine.SetAudioMixingDualMonoMode SetAudioMixingDualMonoMode}.
    *
    * @since v3.6.1.1
    */
    public enum AUDIO_MIXING_DUAL_MONO_MODE {
        /**
        * 0: Original mode.
        */
        AUDIO_MIXING_DUAL_MONO_AUTO = 0,
        /**
        * 1: Left channel mode. This mode replaces the audio of the right channel
        * with the audio of the left channel, which means the user can only hear
        * the audio of the left channel.
        */
        AUDIO_MIXING_DUAL_MONO_L = 1,
        /**
        * 2: Right channel mode. This mode replaces the audio of the left channel with
        * the audio of the right channel, which means the user can only hear the audio
        * of the right channel.
        */
        AUDIO_MIXING_DUAL_MONO_R = 2,
        /**
        * 3: Mixed channel mode. This mode mixes the audio of the left channel and
        * the right channel, which means the user can hear the audio of the left
        * channel and the right channel at the same time.
        */
        AUDIO_MIXING_DUAL_MONO_MIX = 3
    };

    /**
    * The information of an audio file. This struct is reported
    * in {@link agora_gaming_rtc.OnRequestAudioFileInfoHandler OnRequestAudioFileInfoHandler}.
    *
    * @since v3.6.1.1
    */
    public struct AudioFileInfo {
        /** The file path.
        */
        public string filePath;
        /** The file duration (ms).
        */
        public int durationMs;
    };

    /** The information acquisition state. This enum is reported
    * in {@link agora_gaming_rtc.OnRequestAudioFileInfoHandler OnRequestAudioFileInfoHandler}.
    *
    * @since v3.6.1.1
    */
    public enum AUDIO_FILE_INFO_ERROR {
        /** 0: Successfully get the information of an audio file.
        */
        AUDIO_FILE_INFO_ERROR_OK = 0,

        /** 1: Fail to get the information of an audio file.
        */
        AUDIO_FILE_INFO_ERROR_FAILURE = 1
    };


    public enum CONTENT_INSPECT_RESULT {
        CONTENT_INSPECT_NEUTRAL = 1,
        CONTENT_INSPECT_SEXY = 2,
        CONTENT_INSPECT_PORN = 3
    };

    /**
    * The EchoTestConfiguration struct.
    *
    * @since v3.6.1.1
    */
    public struct EchoTestConfiguration {
        /**
        * The view used to render the local user's video. This parameter is only applicable to scenarios testing video devices, that is, when `enableVideo` is `true`.
        */
        public IntPtr view;
        /**
        * Whether to enable the audio device for the call loop test:
        * - true: (Default) Enables the audio device. To test the audio device, set this parameter as true.
        * - false: Disables the audio device.
        */
        public bool enableAudio;
        /**
        * Whether to enable the video device for the call loop test:
        * - true: (Default) Enables the video device. To test the video device, set this parameter as true.
        * - false: Disables the video device.
        */
        public bool enableVideo;
        /**
        * The token used to secure the audio and video call loop test. If you do not enable App Certificate in Agora Console, you do not need to pass a value in this parameter; if you have enabled App Certificate in Agora Console, you must pass a token in this parameter, the uid used when you generate the token must be 0xFFFFFFFF, and the channel name used must be the channel name that identifies each audio and video call loop tested. For server-side token generation, see [Authenticate Your Users with Tokens](https://docs.agora.io/en/Interactive%20Broadcast/token_server?platform=Unity).
        */
        public string token;
        /**
        * The channel name that identifies each audio and video call loop. To ensure proper loop test functionality, the channel name passed in to identify each loop test cannot be the same when users of the same project (App ID) perform audio and video call loop tests on different devices.
        */
        public string channelId;
    };

    /// @cond
    /** Definition of ContentInspectModule.
    *
    * @since v3.6.1.1
    */
    public struct ContentInspectModule {
        /**
        * The content inspect module type.
        * the module type can be 0 to 31.
        * kContentInspectInvalid(0)
        * kContentInspectModeration(1)
        * kContentInspectSupervise(2)
        */
        public int type;
        /**The content inspect frequency, default is 0 second.
        * the frequency <= 0 is invalid.
        */
        public int interval;
    };

    /** Definition of ContentInspectConfig.
    * @since v3.5.2
    */
    public struct ContentInspectConfig {
        /** The extra information, max length of extraInfo is 1024.
        *  The extra information will send to server with content(image).
        */
        public string extraInfo;
        /**The content inspect modules, max length of modules is 32.
        * the content(snapshot of send video stream, image) can be used to max of 32 types functions.
        */
        public ContentInspectModule[] modules;
        /**The content inspect module count.
        */
        public int moduleCount;
    };
    /// @endcond

    public enum AVDATA_TYPE {
        /** 0: the metadata type is unknown.
        */
        AVDATA_UNKNOWN = 0,
        /** 1: the metadata type is video.
        */
        AVDATA_VIDEO = 1,
        /** 2: the metadata type is video.
        */
        AVDATA_AUDIO = 2
    };

    public enum CODEC_VIDEO {
        /** 0: h264 avc codec.
        */
        CODEC_VIDEO_AVC = 0,
        /** 1: h265 hevc codec.
        */
        CODEC_VIDEO_HEVC = 1,
        /** 2: vp8 codec.
        */
        CODEC_VIDEO_VP8 = 2
    };

    public enum CODEC_AUDIO {
        /** 0: PCM audio codec.
        */
        CODEC_AUDIO_PCM = 0,
        /** 1: aac audio codec.
        */
        CODEC_AUDIO_AAC = 1,
        /** 2: G711 audio codec.
        */
        CODEC_AUDIO_G722 = 2
    };

    public class VDataInfo {
        public uint codec;
        public uint width;
        public uint height;
        public int frameType;
        public int rotation;
        public bool equal(VDataInfo vinfo) { return codec == vinfo.codec && width == vinfo.width && height == vinfo.height && rotation == vinfo.rotation; }
    };

    public class ADataInfo {
        public uint codec;
        public uint bitwidth;
        public uint sample_rate;
        public uint channel;
        public uint sample_size;

        public bool equal(ADataInfo ainfo) { return codec == ainfo.codec && bitwidth == ainfo.bitwidth && sample_rate == ainfo.sample_rate && channel == ainfo.channel; }
    };

    public struct AVData {
        /** The User ID. reserved
        - For the receiver: the ID of the user who owns the data.
        */
        public uint uid;
        /**
        - data type, audio / video.
        */
        public AVDATA_TYPE type;
        /** Buffer size of the sent or received Metadata.
        */
        public uint size;
        /** Buffer address of the sent or received Metadata.
        */
        public byte[] buffer;
        /** Time statmp of the frame following the metadata.
        */
        public uint timestamp;
        /**
        * Video frame info
        */
        public VDataInfo vinfo;
        /**
        * Audio frame info
        */
        public ADataInfo ainfo;
    };

    /** The format of the recording file.
    *
    * @since v3.6.1.1
    */
    public enum MediaRecorderContainerFormat {
        /**
        * (Default) MP4.
        */
        FORMAT_MP4 = 1,
        /**
        * Reserved parameter.
        */
        FORMAT_FLV = 2
    };

    /** The recording content.
    *
    * @since v3.6.1.1
    */
    public enum MediaRecorderStreamType {
        /**
        * Only audio.
        */
        STREAM_TYPE_AUDIO = 0x01,
        /**
        * Only video.
        */
        STREAM_TYPE_VIDEO = 0x02,
        /**
        * (Default) Audio and video.
        */
        STREAM_TYPE_BOTH = STREAM_TYPE_AUDIO | STREAM_TYPE_VIDEO
    };

    /** The current recording state.
    *
    * @since v3.6.1.1
    */
    public enum RecorderState {
        /**
        * An error occurs during the recording. See #RecorderErrorCode for the reason.
        */
        RECORDER_STATE_ERROR = -1,
        /**
        * The audio and video recording is started.
        */
        RECORDER_STATE_START = 2,
        /**
        * The audio and video recording is stopped.
        */
        RECORDER_STATE_STOP = 3
    };

    /** The reason for the state change.
    *
    * @since v3.6.1.1
    */
    public enum RecorderErrorCode {
        /**
        * No error occurs.
        */
        RECORDER_ERROR_NONE = 0,
        /**
        * The SDK fails to write the recorded data to a file.
        */
        RECORDER_ERROR_WRITE_FAILED = 1,
        /**
        * The SDK does not detect audio and video streams to be recorded, or audio and video streams are interrupted for more than five seconds during recording.
        */
        RECORDER_ERROR_NO_STREAM = 2,
        /**
        * The recording duration exceeds the upper limit.
        */
        RECORDER_ERROR_OVER_MAX_DURATION = 3,
        /**
        * The recording configuration changes.
        */
        RECORDER_ERROR_CONFIG_CHANGED = 4,
        /**
        * The SDK detects audio and video streams from users using versions of the SDK earlier than v3.0.1 in the `COMMUNICATION` channel profile.
        */
        RECORDER_ERROR_CUSTOM_STREAM_DETECTED = 5
    };

    /** Configurations for the local audio and video recording.
    *
    * @since v3.6.1.1
    */
    public struct MediaRecorderConfiguration {
        /**
        * The absolute path (including the filename extensions) of the recording file. For example, `C:\Users\<user_name>\AppData\Local\Agora\<process_name>\example.mp4` on Windows, `/App Sandbox/Library/Caches/example.mp4` on iOS, `/Library/Logs/example.mp4` on macOS, and `/storage/emulated/0/Android/data/<package name>/files/example.mp4` on Android.
        */
        public string storagePath;
        /**
        * The format of the recording file. See #MediaRecorderContainerFormat.
        */
        public MediaRecorderContainerFormat containerFormat;
        /**
        * The recording content. See #MediaRecorderStreamType.
        */
        public MediaRecorderStreamType streamType;
        /**
        * The maximum recording duration, in milliseconds. The default value is 120000.
        */
        public int maxDurationMs;
        /**
        * The interval (ms) of updating the recording information. The value range is [1000,10000]. Based on the set value of `recorderInfoUpdateInterval`, the SDK triggers the {@link agora_gaming_rtc.MediaRecorder.OnRecorderInfoUpdatedHandler OnRecorderInfoUpdatedHandler} to report the updated recording information.
        */
        public int recorderInfoUpdateInterval;
    };

    /** Information for the recording file.
    *
    * @since v3.6.1.1
    */
    public struct RecorderInfo {
        /**
        * The absolute path of the recording file.
        */
        public string fileName;
        /**
        * The recording duration, in milliseconds.
        */
        public uint durationMs;
        /**
        * The size in bytes of the recording file.
        */
        public uint fileSize;
    };

    /// @cond
    /** The local  proxy mode type. */
    public enum LOCAL_PROXY_MODE {
        /** 0: Connect local proxy with high priority, if not connected to local proxy, fallback to sdrtn.
        */
        ConnectivityFirst = 0,
        /** 1: Only connect local proxy
        */
        LocalOnly = 1,
    };

    public struct LocalAccessPointConfiguration {
        /** local access point ip address list.
        */
        public string[] ipList;
        /** the number of local access point ip address.
        */
        public int ipListSize;
        /** local access point domain list.
        */
        public string[] domainList;
        /** the number of local access point domain.
        */
        public int domainListSize;
        /** certificate domain name installed on specific local access point. pass "" means using sni domain on specific local access point
        */
        public string verifyDomainName;
        /** local proxy connection mode, connectivity first or local only.
        */
        public LOCAL_PROXY_MODE mode;
    };
    /// @endcond

    /**
    * The error code of the window blocking during screen sharing.
    *
    * @since v3.6.1.1
    */
    public enum EXCLUDE_WINDOW_ERROR {
        /**
        * -1: Fails to block the window during screen sharing. The user's graphics card does not support window blocking.
        */
        EXCLUDE_WINDOW_FAIL = -1,
        /**
        * 0: Reserved.
        */
        EXCLUDE_WINDOW_NONE = 0
    };

    /** The volume type.
     * @since 3.6.2
     */
    public enum AudioDeviceTestVolumeType {
        /** 0: The volume of the audio capturing device. */
        AudioTestRecordingVolume = 0,
        /** 1: The volume of the audio playback device. */
        AudioTestPlaybackVolume = 1,
    };

    /**
     * The low-light enhancement mode.
     */
    public enum LOW_LIGHT_ENHANCE_MODE {
        /**
         * 0: (Default) Automatic mode. The SDK automatically enables or disables the low-light enhancement
         * feature according to the ambient light to compensate for the lighting level or prevent overexposure,
         * as necessary. */
        LOW_LIGHT_ENHANCE_AUTO = 0,
        /** Manual mode. Users need to enable or disable the low-light enhancement feature manually. */
        LOW_LIGHT_ENHANCE_MANUAL
    };
    /**
     * The low-light enhancement level.
     */
    public enum LOW_LIGHT_ENHANCE_LEVEL {
        /** 0: (Default) Promotes video quality during low-light enhancement. It processes the brightness,
         * details, and noise of the video image. The performance consumption is moderate, the processing
         * speed is moderate, and the overall video quality is optimal. */
        LOW_LIGHT_ENHANCE_LEVEL_HIGH_QUALITY = 0,
        /** Promotes performance during low-light enhancement. It processes the brightness and details of the video image.
         * The processing speed is faster.*/
        LOW_LIGHT_ENHANCE_LEVEL_FAST
    };

    /** The low-light enhancement options.
     * @since 3.6.2
    */
    public struct LowLightEnhanceOptions {
        /** The low-light enhancement mode. See #LOW_LIGHT_ENHANCE_MODE.
        */
        public LOW_LIGHT_ENHANCE_MODE mode;
        /** The low-light enhancement level. See #LOW_LIGHT_ENHANCE_LEVEL.
        */
        public LOW_LIGHT_ENHANCE_LEVEL level;
    };

    /**
     * The video noise reduction mode.
     */
    public enum VIDEO_DENOISER_MODE {
        /**
         * 0: (Default) Automatic mode. The SDK automatically enables or disables
         * the video noise reduction feature according to the ambient light.
         */
        VIDEO_DENOISER_AUTO = 0,
        /**
         * Manual mode. Users need to enable or disable the video noise reduction feature manually.
         */
        VIDEO_DENOISER_MANUAL
    };

    /**
     * The video noise reduction level.
     */
    public enum VIDEO_DENOISER_LEVEL {
        /**
         * 0: (Default) Promotes video quality during video noise reduction. `HIGH_QUALITY` balances
         * performance consumption and video noise reduction quality. The performance consumption is
         * moderate, the video noise reduction speed is moderate, and the overall video quality is optimal.
         */
        VIDEO_DENOISER_LEVEL_HIGH_QUALITY = 0,
        /**
         * Promotes reducing performance consumption during video noise reduction. `FAST` prioritizes reducing
         * performance consumption over video noise reduction quality. The performance consumption is lower,
         * and the video noise reduction speed is faster. To avoid a noticeable shadowing effect (shadows trailing
         * behind moving objects) in the processed video, Agora recommends that you use `FAST` when the camera is fixed.
         */
        VIDEO_DENOISER_LEVEL_FAST,
        /**
         * Enhanced video noise reduction. `STRENGTH` prioritizes video noise reduction quality over reducing performance
         * consumption. The performance consumption is higher, the video noise reduction speed is slower, and the video
         * noise reduction quality is better. If `HIGH_QUALITY` is not enough for your video noise reduction needs, you
         * can use `STRENGTH`.
         */
        VIDEO_DENOISER_LEVEL_STRENGTH
    };

    /**
     * The video noise reduction options.
     * @since 3.6.2.
     */
    public struct VideoDenoiserOptions {
       /**
        * The video noise reduction mode. See #VIDEO_DENOISER_MODE.
        */
        public VIDEO_DENOISER_MODE mode;

       /**
        * The video noise reduction level. See #VIDEO_DENOISER_LEVEL
        */
        public VIDEO_DENOISER_LEVEL level;
    };

    /** The color enhancement options.
     * @since 3.6.2
    */
    public struct ColorEnhanceOptions {
        /** The level of color enhancement. The value range is [0.0,1.0]. 0.0 is the default value,
        * which means no color enhancement is applied to the video. The higher the value, the higher
        * the level of color enhancement.
        */
        public float strengthLevel;
        /** The level of skin tone protection. The value range is [0.0,1.0]. 0.0 means no skin tone
        * protection. The higher the value, the higher the level of skin tone protection. The default
        * value is 1.0. When the level of color enhancement is higher, the portrait skin tone can be
        * significantly distorted, so you need to set the level of skin tone protection; when the level
        * of skin tone protection is higher, the color enhancement effect can be slightly reduced. Therefore,
        * to get the best color enhancement effect, Agora recommends that you adjust `strengthLevel` and
        * `skinProtectLevel` to get the most appropriate values.
        */
        public float skinProtectLevel;
    };

    /**
    * Reasons for a user role switch failure, reported in {@link agora_gaming_rtc.OnClientRoleChangeFailedHandler OnClientRoleChangeFailedHandler}.
    *
    * @since v3.7.0
    */
    public enum CLIENT_ROLE_CHANGE_FAILED_REASON {
        /** 1: The number of hosts in the channel is already at the upper limit.
         * @note This enumerator is reported only when the support for 128 users is enabled. The maximum number of hosts is based on the actual number
         * of hosts configured when you enable the 128-user feature.
        */
        CLIENT_ROLE_CHANGE_FAILED_BY_TOO_MANY_BROADCASTERS = 1,
        /** 2: The request is rejected by the Agora server. Agora recommends you prompt the user to try to switch their user role again.
        */
        CLIENT_ROLE_CHANGE_FAILED_BY_NOT_AUTHORIZED = 2,
        /** 3: The request is timed out. Agora recommends you prompt the user to check the network connection and try to switch their user role again.
        */
        CLIENT_ROLE_CHANGE_FAILED_BY_REQUEST_TIME_OUT = 3,
        /** 4: The SDK connection fails. You can use reason reported in the onClientRoleChangeFailed callback to troubleshoot the failure.
        */
        CLIENT_ROLE_CHANGE_FAILED_BY_CONNECTION_FAILED = 4,
    };

    /** The reason of notifying the user of a message.
    */
    public enum WLACC_MESSAGE_REASON {
        /** WIFI signal is weak.*/
        WLACC_MESSAGE_REASON_WEAK_SIGNAL = 0,
        /** 2.4G band congestion.*/
        WLACC_MESSAGE_REASON_2G_CHANNEL_CONGESTION = 1,
    };

    /** Suggest an action for the user.
    */
    public enum WLACC_SUGGEST_ACTION {
        /** Please get close to AP.*/
        WLACC_SUGGEST_ACTION_CLOSE_TO_WIFI = 0,
        /** The user is advised to connect to the prompted SSID.*/
        WLACC_SUGGEST_ACTION_CONNECT_5G = 1,
        /** The user is advised to check whether the AP supports 5G band and enable 5G band (the aciton link is attached), or purchases an AP that supports 5G. AP does not support 5G band.*/
        WLACC_SUGGEST_ACTION_CHECK_5G = 2,
        /** The user is advised to change the SSID of the 2.4G or 5G band (the aciton link is attached). The SSID of the 2.4G band AP is the same as that of the 5G band.*/
        WLACC_SUGGEST_ACTION_MODIFY_SSID = 3,
    };

    /** Indicator optimization degree.
    */
    public struct WlAccStats {
        /** End-to-end delay optimization percentage.*/
        public ushort e2eDelayPercent;
        /** Frozen Ratio optimization percentage.*/
        public ushort frozenRatioPercent;
        /** Loss Rate optimization percentage.*/
        public ushort lossRatePercent;
    };

    /** The cloud proxy type.
    *
    * @since v3.3.0
    */
    public enum CLOUD_PROXY_TYPE {
        /** 0: Do not use the cloud proxy.
        */
        NONE_PROXY = 0,
        /** 1: The cloud proxy for the UDP protocol.
        */
        UDP_PROXY = 1,
        /// @cond
        /** 2: The cloud proxy for the TCP (encrypted) protocol.
        */
        TCP_PROXY = 2,
        /// @endcond
    };

    /**
     * The screen sharing scenario, set in {@link agora_gaming_rtc.IRtcEngine.SetScreenCaptureScenario SetScreenCaptureScenario}.
     * @since 3.7.0
     */
    public enum SCREEN_SCENARIO_TYPE {
        /**
         * 1: (Default) Document. This scenario prioritizes the video quality of screen sharing
         * and reduces the latency of the shared video for the receiver. If you share documents,
         * slides, and tables, you can set this scenario.
         */
        SCREEN_SCENARIO_DOCUMENT = 1,
        /**
         * 2: Game. This scenario prioritizes the smoothness of screen sharing. If you share games,
         * you can set this scenario.
         */
        SCREEN_SCENARIO_GAMING = 2,
        /**
         * 3: Video. This scenario prioritizes the smoothness of screen sharing. If you share movies
         * or live videos, you can set this scenario.
         */
        SCREEN_SCENARIO_VIDEO = 3,
        /**
         * 4: Remote control. This scenario prioritizes the video quality of screen sharing and reduces
         * the latency of the shared video for the receiver. If you share the device desktop being remotely
         * controlled, you can set this scenario.
         */
        SCREEN_SCENARIO_RDC = 4,
    };

    /**
    * The image content of the thumbnail or icon.
    *
    * @since v3.5.2
    *
    * @note The default image is in the RGBA format. If you need to use another format, you need to convert the image on
    * your own.
    */
    [StructLayout(LayoutKind.Sequential)]
    public struct ThumbImageBuffer {
        /**
        * The buffer of the thumbnail or icon.
        */
        public IntPtr buffer;
        /**
        * The buffer length (bytes) of the thumbnail or icon.
        */
        public uint length;
        /**
        * The actual width (px) of the thumbnail or icon.
        */
        public uint width;
        /**
        * The actual height (px) of the thumbnail or icon.
        */
        public uint height;
    };

    /**
    * The type of the shared target.
    *
    * @since v3.5.2
    */
    public enum ScreenCaptureSourceType {
        /**
        * -1: Unknown type.
        */
        ScreenCaptureSourceType_Unknown = -1,
        /**
        * 0: The shared target is a window.
        */
        ScreenCaptureSourceType_Window = 0,
        /**
        * 1: The shared target is a screen of a particular monitor.
        */
        ScreenCaptureSourceType_Screen = 1,
        /**
        * 2: Reserved parameter.
        */
        ScreenCaptureSourceType_Custom = 2,
    };

    /** The screen sharing information.
    *
    * @since v3.6.1.1
    *
    */
    public struct ScreenCaptureInfo {
        /** The type of the graphics card, which contains the model information of the graphics card. */
        public string graphicsCardType;
        /** The error code of the window blocking during screen sharing. See #EXCLUDE_WINDOW_ERROR. */
        public EXCLUDE_WINDOW_ERROR errCode;
    };

    /** Super Resolution modes. */
    public enum SR_MODE {
        /** 0: manual select uid to do super resolution */
        SR_MODE_MANUAL = 0,
        /** 1: auto select.*/
        SR_MODE_AUTO,
    };

    #endregion some enum and struct types
}
