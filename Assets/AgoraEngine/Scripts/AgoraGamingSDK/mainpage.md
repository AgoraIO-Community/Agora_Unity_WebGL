Agora provides ensured quality of experience (QoE) for worldwide Internet-based voice and video communications through a virtual global network optimized for real-time web and mobile-to-mobile applications.

> During API calls, the SDK may returns error codes and warning codes. See [Error Codes and Warning Codes](../../error_rtc?platform=Unity).

- The {@link agora_gaming_rtc.IRtcEngine IRtcEngine} class is the entry point of the Agora RTC SDK providing API methods for applications to quickly start a voice/video communication or interactive broadcast.
- The {@link agora_gaming_rtc.OnJoinChannelSuccessHandler AgoraCallback} reports runtime events to the applications.
- The {@link agora_gaming_rtc.AgoraChannel AgoraChannel} class provides methods that enable real-time communications
in a specified channel. By creating multiple RtcChannel instances, users can join multiple channels.
- The {@link agora_gaming_rtc.AudioEffectManagerImpl AudioEffectManagerImpl} class provides APIs that set the audio effects.
- The {@link agora_gaming_rtc.AudioPlaybackDeviceManager AudioPlaybackDeviceManager} class provides APIs that set the audio playback device, and retrieves the information of the audio playback device.
- The {@link agora_gaming_rtc.AudioRecordingDeviceManager AudioRecordingDeviceManager} class provides APIs that set the audio capturing device, and retrieves the information of the audio capturing device.
- The {@link agora_gaming_rtc.VideoDeviceManager VideoDeviceManager} class provides APIs that set the video capturing device, and retrieves the information of the video capturing device.
- The {@link agora_gaming_rtc.MetadataObserver MetadataObserver} class provides APIs that register the MetadataObserver and report the status of the metadata.
- The {@link agora_gaming_rtc.PacketObserver PacketObserver} class provides APIs that register the PacketObserver and report the status of the audio packet.
- The {@link agora_gaming_rtc.AudioRawDataManager AudioRawDataManager} class provides APIs that register the audio raw data observer and report the status of the audio raw data.
- The {@link agora_gaming_rtc.VideoRawDataManager VideoRawDataManager} class provides APIs that register the video raw data observer and report the status of the video raw data.
- The {@link agora_gaming_rtc.VideoSurface VideoSurface} class provides APIs that set the video renderer type and the local/remote video.

### Core Methods

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.GetEngine(string appId) GetEngine}1</td>
<td>Initializes the IRtcEngine.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.GetEngine(RtcEngineConfig engineConfig) GetEngine}2</td>
<td>Initializes the IRtcEngine and specifies the connection area.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.Destroy Destroy}</td>
<td>Destroys all IRtcEngine resources.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetChannelProfile SetChannelProfile}</td>
<td>Sets the channel profile of the Agora IRtcEngine.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetClientRole(CLIENT_ROLE_TYPE role) SetClientRole}1</td>
<td>Sets the role of the user in interactive live streaming.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetClientRole(CLIENT_ROLE_TYPE role, ClientRoleOptions audienceLatencyLevel) SetClientRole}2</td>
<td>Sets the role and level of the user in interactive live streaming.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey}</td>
<td>Allows a user to join a channel with token.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.JoinChannel(string token, string channelId, string info, uint uid, ChannelMediaOptions options) JoinChannel}</td>
<td>Allows a user to join a channel and set the subscribing state.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SwitchChannel(string token, string channelId) SwitchChannel}</td>
<td>Switches to a different channel in interactive live streaming.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SwitchChannel(string token, string channelId, ChannelMediaOptions options) SwitchChannel}2</td>
<td>Switches to a different channel and sets the subscribing state in the interactive live streaming.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.LeaveChannel LeaveChannel}</td>
<td>Allows a user to leave a channel.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.RenewToken RenewToken}</td>
<td>Renews the Token.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.GetConnectionState GetConnectionState}</td>
<td>Gets the current connection state of the SDK.</td>
</tr>
</table>

### Core Events

<table>
<tr>
<th>Event</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnConnectionStateChangedHandler OnConnectionStateChangedHandler}</td>
<td>Occurs when the connection state between the SDK and the server changes.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnJoinChannelSuccessHandler OnJoinChannelSuccessHandler}</td>
<td>Occurs when a user joins a channel.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnReJoinChannelSuccessHandler OnReJoinChannelSuccessHandler}</td>
<td>Occurs when a user rejoins the channel.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnLeaveChannelHandler OnLeaveChannelHandler}</td>
<td>Occurs when a user leaves the channel.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnClientRoleChangedHandler OnClientRoleChangedHandler}</td>
<td>Occurs when the user role switches in interactive live streaming.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnUserJoinedHandler OnUserJoinedHandler}</td>
<td>Occurs when a remote user (`COMMUNICATION`)/ host (`LIVE_BROADCASTING`) joins the channel.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnUserOfflineHandler OnUserOfflineHandler}</td>
<td>Occurs when a remote user (`COMMUNICATION`)/ host (`LIVE_BROADCASTING`) leaves the channel.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnNetworkTypeChangedHandler OnNetworkTypeChangedHandler}</td>
<td>Occurs when the local network type changes.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnConnectionLostHandler OnConnectionLostHandler}</td>
<td>Occurs when the SDK cannot reconnect to Agora's edge server 10 seconds after its connection to the server is interrupted.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnTokenPrivilegeWillExpireHandler OnTokenPrivilegeWillExpireHandler}</td>
<td>Occurs when the token expires in 30 seconds.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnRequestTokenHandler OnRequestTokenHandler}</td>
<td>Occurs when the token expires.</td>
</tr>
</table>

### Audio Management

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.EnableAudio EnableAudio}</td>
<td>Enables the audio module.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.DisableAudio DisableAudio}</td>
<td>Disables the audio module.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetAudioProfile SetAudioProfile}</td>
<td>Sets the audio parameters and application scenarios.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.AdjustRecordingSignalVolume AdjustRecordingSignalVolume}</td>
<td>Adjusts adjust the capturing signal volume.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.AdjustUserPlaybackSignalVolume AdjustUserPlaybackSignalVolume}</td>
<td>Adjusts the playback signal volume of a specified remote user.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.AdjustPlaybackSignalVolume AdjustPlaybackSignalVolume}</td>
<td>Adjusts the playback signal volume of all remote users.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.EnableLocalAudio EnableLocalAudio}</td>
<td>Enables/Disables the local audio sampling.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.MuteLocalAudioStream MuteLocalAudioStream}</td>
<td>Stops or resumes publishing the local audio stream.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.MuteRemoteAudioStream MuteRemoteAudioStream}</td>
<td>Stops or resumes subscribing to the audio stream of a specified user.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.MuteAllRemoteAudioStreams MuteAllRemoteAudioStreams}</td>
<td>Stops or resumes subscribing to the audio streams of all remote users.</td>
</tr>
</table>

### Audio Volume Indication

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.EnableAudioVolumeIndication EnableAudioVolumeIndication}</td>
<td>Enables the {@link agora_gaming_rtc.OnVolumeIndicationHandler OnVolumeIndicationHandler} callback at a set time interval to report on which users are speaking and the speakers' volume.</td>
</tr>
</table>

<table>
<tr>
<th>Event</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnVolumeIndicationHandler OnVolumeIndicationHandler}</td>
<td>Reports which users are speaking, the speakers' volumes, and whether the local user is speaking.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnActiveSpeakerHandler OnActiveSpeakerHandler}</td>
<td>Reports which user is the loudest speaker.</td>
</tr>
</table>

### Face detection
<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.EnableFaceDetection EnableFaceDetection}</td>
<td>Enables/Disables face detection for the local user.</td>
</tr>
</table>

<table>
<tr>
<th>Event</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnFacePositionChangedHandler OnFacePositionChangedHandler}</td>
<td>Reports the face detection result of the local user.</td>
</tr>
</table>

### Video Management

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.EnableVideo EnableVideo}</td>
<td>Enables the video module.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.EnableVideoObserver EnableVideoObserver}</td>
<td>Enables the video observer.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.DisableVideo DisableVideo}</td>
<td>Disables the video module.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.DisableVideoObserver DisableVideoObserver}</td>
<td>Disables the video observer.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetVideoEncoderConfiguration SetVideoEncoderConfiguration}</td>
<td>Sets the video encoder configuration.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.VideoSurface.SetForUser SetForUser}</td>
<td>Sets the local/remote video.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.StartPreview StartPreview}</td>
<td>Starts the local video preview before joining the channel.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.StopPreview StopPreview}</td>
<td>Stops the local video preview and disables video.</td>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.EnableLocalVideo EnableLocalVideo}</td>
<td>Enables/Disables the local video capture.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.MuteLocalVideoStream MuteLocalVideoStream}</td>
<td>Stops or resumes publishing the local video stream.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.MuteRemoteVideoStream MuteRemoteVideoStream}</td>
<td>Stops or resumes subscribing to the video stream of a specified user.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.MuteAllRemoteVideoStreams MuteAllRemoteVideoStreams}</td>
<td>Stops or resumes subscribing to the video streams of all remote users.</td>
</tr>
</table>

### Audio Effect Playback

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioEffectManagerImpl.GetEffectsVolume GetEffectsVolume}</td>
<td>Gets the volume of the audio effects.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioEffectManagerImpl.SetEffectsVolume SetEffectsVolume}</td>
<td>Sets the volume of the audio effects.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetVolumeOfEffect SetVolumeOfEffect}</td>
<td>Sets the volume of a specified audio effect.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioEffectManagerImpl.PlayEffect PlayEffect}</td>
<td>Plays a specified audio effect.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioEffectManagerImpl.StopEffect StopEffect}</td>
<td>Stops playing a specified audio effect.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioEffectManagerImpl.StopAllEffects StopAllEffects}</td>
<td>Stops playing all audio effects.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioEffectManagerImpl.PreloadEffect PreloadEffect}</td>
<td>Preloads a specified audio effect file into the memory.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioEffectManagerImpl.UnloadEffect UnloadEffect}</td>
<td>Releases a specified audio effect from the memory.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioEffectManagerImpl.PauseEffect PauseEffect}</td>
<td>Pauses a specified audio effect.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioEffectManagerImpl.PauseAllEffects PauseAllEffects}</td>
<td>Pauses all audio effects.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioEffectManagerImpl.ResumeEffect ResumeEffect}</td>
<td>Resumes playing a specified audio effect.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioEffectManagerImpl.ResumeAllEffects ResumeAllEffects}</td>
<td>Resumes playing all audio effects.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioEffectManagerImpl.SetVoiceOnlyMode SetVoiceOnlyMode}</td>
<td>Sets the voice-only mode.</td>
</tr>
</table>
<table>
<tr>
<th>Event</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnAudioEffectFinishedHandler OnAudioEffectFinishedHandler}</td>
<td>Occurs when the local audio effect playback finishes.</td>
</tr>
</table>

### Voice Effect

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetVoiceBeautifierPreset SetVoiceBeautifierPreset}</td>
<td>Sets an SDK preset voice beautifier effect.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetVoiceBeautifierParameters SetVoiceBeautifierParameters}</td>
<td>Sets parameters for SDK preset voice beautifier effects.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetAudioEffectPreset SetAudioEffectPreset}</td>
<td>Sets an SDK preset audio effect.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetAudioEffectParameters SetAudioEffectParameters}</td>
<td>Sets parameters for SDK preset audio effects.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetVoiceConversionPreset SetVoiceConversionPreset}</td>
<td>Sets an SDK preset voice conversion effect.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioEffectManagerImpl.SetLocalVoicePitch SetLocalVoicePitch}</td>
<td>Changes the voice pitch of the local speaker.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceEqualization SetLocalVoiceEqualization}</td>
<td>Sets the local voice equalization effect.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetLocalVoiceReverb SetLocalVoiceReverb}</td>
<td>Sets the local voice reverberation.</td>
</tr>
</table>

### Sound Position Indication

<table>
<tr>
<th>Method</th>
<th>Description</th>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.EnableSoundPositionIndication EnableSoundPositionIndication}</td>
<td>Enables/Disables stereo panning for remote users.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioEffectManagerImpl.SetRemoteVoicePosition SetRemoteVoicePosition}</td>
<td>Sets the sound position and gain of a remote user.</td>
</tr>
</table>


### Audio Routing Control

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetDefaultAudioRouteToSpeakerphone SetDefaultAudioRouteToSpeakerphone}</td>
<td>Sets the default audio playback route. (For Android and iOS only)</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetEnableSpeakerphone SetEnableSpeakerphone}</td>
<td>Enables/Disables the audio playback route to the speakerphone. (For Android and iOS only)</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.IsSpeakerphoneEnabled IsSpeakerphoneEnabled}</td>
<td>Checks whether the speakerphone is enabled. (For Android and iOS only)</td>
</tr>
</table>

<table>
<tr>
<th>Event</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnAudioRouteChangedHandler OnAudioRouteChangedHandler}</td>
<td>Occurs when the local audio route changes.</td>
</tr>
</table>

### Local Media Events

<table>
<tr>
<th>Event</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnLocalAudioStateChangedHandler OnLocalAudioStateChangedHandler}</td>
<td>Occurs when the local audio state changes.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnLocalVideoStateChangedHandler OnLocalVideoStateChangedHandler}</td>
<td>Occurs when the local video state changes.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnFirstLocalAudioFramePublishedHandler OnFirstLocalAudioFramePublishedHandler}</td>
<td>Occurs when the first audio frame is published.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnFirstLocalVideoFramePublishedHandler OnFirstLocalVideoFramePublishedHandler}</td>
<td>Occurs when the first video frame is published.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnFirstLocalVideoFrameHandler OnFirstLocalVideoFrameHandler}</td>
<td>Occurs when the first local video frame is rendered.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnAudioPublishStateChangedHandler OnAudioPublishStateChangedHandler}</td>
<td>Occurs when the audio publishing state changes.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnVideoPublishStateChangedHandler OnVideoPublishStateChangedHandler}</td>
<td>Occurs when the video publishing state changes.</td>
</tr>
</table>

### Remote Media Events

<table>
<tr>
<th>Event</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnRemoteAudioStateChangedHandler OnRemoteAudioStateChangedHandler}</td>
<td>Occurs when the remote audio state changes.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnRemoteVideoStateChangedHandler OnRemoteVideoStateChangedHandler}</td>
<td>Occurs when the remote video state changes.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnFirstRemoteVideoFrameHandler OnFirstRemoteVideoFrameHandler}</td>
<td>Occurs when the first remote video frame is rendered.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnAudioSubscribeStateChangedHandler OnAudioSubscribeStateChangedHandler}</td>
<td>Occurs when the audio subscribing state changes.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnVideoSubscribeStateChangedHandler OnVideoSubscribeStateChangedHandler}</td>
<td>Occurs when the video subscribing state changes.</td>
</tr>
</table>

### Statistics Events

> After joining a channel, SDK triggers this group of callbacks once every two seconds.

<table>
<tr>
<th>Event</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnRtcStatsHandler OnRtcStatsHandler}</td>
<td>Reports the statistics of the current call session.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnNetworkQualityHandler OnNetworkQualityHandler}</td>
<td>Reports the network quality of each user.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnLocalAudioStatsHandler OnLocalAudioStatsHandler}</td>
<td>Reports the statistics of the local audio stream.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnLocalVideoStatsHandler OnLocalVideoStatsHandler}</td>
<td>Reports the statistics of the local video stream.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnRemoteAudioStatsHandler OnRemoteAudioStatsHandler}</td>
<td>Reports the statistics of the audio stream from each remote user/host.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnRemoteVideoStatsHandler OnRemoteVideoStatsHandler}</td>
<td>Reports the statistics of the video stream from each remote user/host.</td>
</tr>
</table>

### Video Pre-process and Post-process

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetBeautyEffectOptions SetBeautyEffectOptions}</td>
<td>Sets the image enhancement options. (This method applies to Android and iOS only.)</td>
</tr>
</table>

### Multi-channel management

> We provide an advanced guide on the applicable scenarios, implementation and considerations for this group of methods. For details, see [Join multiple channels](https://docs.agora.io/en/Interactive%20Broadcast/multiple_channel_unity).

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.CreateChannel CreateChannel}</td>
<td>Creates and gets an `AgoraChannel` object. To join multiple channels, create multiple `AgoraChannel` objects.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetMultiChannelWant SetMultiChannelWant}</td>
<td>Sets whether to enable the multi-channel mode.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.VideoSurface.SetForMultiChannelUser SetForMultiChannelUser}</td>
<td>Sets the local or remote video of users in multiple channels.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AgoraChannel AgoraChannel}</td>
<td>Provides methods that enable real-time communications in a specified channel.</td>
</tr>
</table>

### Screen Capture

> This group of methods applies to Windows or macOS only.

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.StartScreenCaptureByDisplayId StartScreenCaptureByDisplayId}</td>
<td>Shares the whole or part of a screen by specifying the display ID.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.StartScreenCaptureByScreenRect StartScreenCaptureByScreenRect}</td>
<td>Shares the whole or part of a screen by specifying the screen rect.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.StartScreenCaptureByWindowId StartScreenCaptureByWindowId}</td>
<td>Shares the whole or part of a window by specifying the window ID.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetScreenCaptureContentHint SetScreenCaptureContentHint}</td>
<td>Sets the content hint for screen sharing.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.UpdateScreenCaptureParameters UpdateScreenCaptureParameters}</td>
<td>Updates the screen sharing parameters.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.UpdateScreenCaptureRegion UpdateScreenCaptureRegion}</td>
<td>Updates the screen sharing region.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.StopScreenCapture StopScreenCapture}</td>
<td>Stops screen sharing.</td>
</tr>
</table>

### Audio File Playback and Mixing

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.StartAudioMixing StartAudioMixing}</td>
<td>Starts playing and mixing the music file.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.StopAudioMixing StopAudioMixing}</td>
<td>Stops playing and mixing the music file.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.PauseAudioMixing PauseAudioMixing}</td>
<td>Pauses playing and mixing the music file.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.ResumeAudioMixing ResumeAudioMixing}</td>
<td>Resumes playing and mixing the music file.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.AdjustAudioMixingVolume AdjustAudioMixingVolume}</td>
<td>Adjusts the volume during audio mixing.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.AdjustAudioMixingPlayoutVolume AdjustAudioMixingPlayoutVolume}</td>
<td>Adjusts the volume of audio mixing for local playback.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.AdjustAudioMixingPublishVolume AdjustAudioMixingPublishVolume}</td>
<td>Adjusts the volume of audio mixing for remote playback.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetAudioMixingPitch SetAudioMixingPitch}</td>
<td>Sets the pitch of the local music file.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.GetAudioMixingPlayoutVolume GetAudioMixingPlayoutVolume}</td>
<td>Gets the audio mixing volume for local playback.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.GetAudioMixingPublishVolume GetAudioMixingPublishVolume}</td>
<td>Gets the audio mixing volume for publishing.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.GetAudioMixingDuration GetAudioMixingDuration}</td>
<td>Gets the duration (ms) of the music file.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.GetAudioMixingCurrentPosition GetAudioMixingCurrentPosition}</td>
<td>Gets the playback position (ms) of the music file.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetAudioMixingPosition SetAudioMixingPosition}</td>
<td>Sets the playback position of the music file.</td>
</tr>
</table>

<table>
<tr>
<th>Event</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnAudioMixingStateChangedHandler OnAudioMixingStateChangedHandler}</td>
<td>Occurs when the state of the local user's audio mixing file changes.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnRemoteAudioMixingBeginHandler OnRemoteAudioMixingBeginHandler}</td>
<td>Occurs when a remote user starts audio mixing.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnRemoteAudioMixingEndHandler OnRemoteAudioMixingEndHandler}</td>
<td>Occurs when a remote user finishes audio mixing.</td>
</tr>
</table>

### CDN Publisher

> This group of methods apply to interactive live streaming only.

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetLiveTranscoding SetLiveTranscoding}</td>
<td>Sets the video layout and audio for CDN live.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.AddPublishStreamUrl AddPublishStreamUrl}</td>
<td>Adds a CDN stream address.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.RemovePublishStreamUrl RemovePublishStreamUrl}</td>
<td>Removes a CDN stream address.</td>
</tr>
</table>

<table>
<tr>
<th>Event</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnRtmpStreamingStateChangedHandler OnRtmpStreamingStateChangedHandler}</td>
<td>Occurs when the state of the RTMP or RTMPS streaming changes.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnRtmpStreamingEventHandler OnRtmpStreamingEventHandler}</td>
<td>Reports events during the RTMP or RTMPS streaming.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnTranscodingUpdatedHandler OnTranscodingUpdatedHandler}</td>
<td>Occurs when the publisher's transcoding settings are updated.</td>
</tr>
</table>

### Media Stream Relay Across Channels

> This group of methods apply to interactive live streaming only.

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.StartChannelMediaRelay StartChannelMediaRelay}</td>
<td>Starts to relay media streams across channels.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.UpdateChannelMediaRelay UpdateChannelMediaRelay}</td>
<td>Updates the channels for media stream relay.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.StopChannelMediaRelay StopChannelMediaRelay}</td>
<td>Stops the media stream relay.</td>
</tr>
</table>

<table>
<tr>
<th>Event</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnChannelMediaRelayStateChangedHandler OnChannelMediaRelayStateChangedHandler}</td>
<td>Occurs when the state of the media stream relay changes.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnChannelMediaRelayEventHandler OnChannelMediaRelayEventHandler}</td>
<td>Reports events during the media stream relay.</td>
</tr>
</table>

### In-ear Monitoring

> This group of methods applies to Android and iOS only.

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.EnableInEarMonitoring EnableInEarMonitoring}</td>
<td>Enables in-ear monitoring.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetInEarMonitoringVolume SetInEarMonitoringVolume}</td>
<td>Sets the volume of the in-ear monitor.</td>
</tr>
</table>

### Dual Video Stream Mode

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.EnableDualStreamMode EnableDualStreamMode}</td>
<td>Enables/disables dual video stream mode.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetRemoteVideoStreamType SetRemoteVideoStreamType}</td>
<td>Sets the remote user’s video stream type received by the local user when the remote user sends dual streams.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetRemoteDefaultVideoStreamType SetRemoteDefaultVideoStreamType}</td>
<td>Sets the default video-stream type for the video received by the local user when the remote user sends dual streams.</td>
</tr>
</table>

### Stream Fallback

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetLocalPublishFallbackOption SetLocalPublishFallbackOption}</td>
<td>Sets the fallback option for the published video stream under unreliable network conditions.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetRemoteSubscribeFallbackOption SetRemoteSubscribeFallbackOption}</td>
<td>Sets the fallback option for the remote stream under unreliable network conditions.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetRemoteUserPriority SetRemoteUserPriority}</td>
<td>Prioritizes a remote user's stream. </td>
</tr>
</table>

<table>
<tr>
<th>Event</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnLocalPublishFallbackToAudioOnlyHandler OnLocalPublishFallbackToAudioOnlyHandler}</td>
<td>Occurs: <p><ul><li>When the published media stream falls back to an audio-only stream due to poor network conditions.</li><li>When the published media stream switches back to the video after the network conditions improve.</li></ul></p></td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnRemoteSubscribeFallbackToAudioOnlyHandler OnRemoteSubscribeFallbackToAudioOnlyHandler}</td>
<td>Occurs: <p><ul><li>When the remote media stream falls back to audio-only due to poor network conditions. </li><li>When the remote media stream switches back to the video after the network conditions improve.</li></ul></p></td>
</tr>
</table>

### Pre-call Network Test

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.StartEchoTest(int intervalInSeconds) StartEchoTest}</td>
<td>Starts an audio call test.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.StopEchoTest StopEchoTest}</td>
<td>Stops the audio call test.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.EnableLastmileTest EnableLastmileTest}</td>
<td>Enables the network connection quality test.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.DisableLastmileTest DisableLastmileTest}</td>
<td>Disables the network connection quality test.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.StartLastmileProbeTest StartLastmileProbeTest}</td>
<td>Starts the last-mile network probe test.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.StopLastmileProbeTest StopLastmileProbeTest}</td>
<td>Stops the last-mile network probe test.</td>
</tr>
</table>

<table>
<tr>
<th>Event</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnLastmileQualityHandler OnLastmileQualityHandler}</td>
<td>Reports the last mile network quality of the local user before the user joins the channel.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnLastmileProbeResultHandler OnLastmileProbeResultHandler}</td>
<td>Reports the last-mile network probe result.</td>
</tr>
</table>

### External Video Data (Push-mode only)

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetExternalVideoSource SetExternalVideoSource}</td>
<td>Configures the external video source.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.PushVideoFrame PushVideoFrame}</td>
<td>Pushes the external video frame.</td>
</tr>
</table>

### External Audio Data (Push-mode only)

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetExternalAudioSource SetExternalAudioSource}</td>
<td>Configures the external audio source.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.PushAudioFrame PushAudioFrame}</td>
<td>Pushes the external audio frame.</td>
</tr>
</table>


### External Audio Sink (Pull-mode only)

> This group of methods applies to Windows only.

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetExternalAudioSink SetExternalAudioSink}</td>
<td>Sets the external audio sink.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRawDataManager.PullAudioFrame PullAudioFrame}</td>
<td>Pulls the external audio frame.</td>
</tr>
</table>

### <a name="rawaudio"></a >Raw Audio Data

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRawDataManager.RegisterAudioRawDataObserver RegisterAudioRawDataObserver}</td>
<td>Registers an audio frame observer object.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRawDataManager.UnRegisterAudioRawDataObserver UnRegisterAudioRawDataObserver}</td>
<td>UnRegisters the audio raw data observer.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRawDataManager.SetOnRecordAudioFrameCallback SetOnRecordAudioFrameCallback}</td>
<td>Listens for the {@link agora_gaming_rtc.AudioRawDataManager.OnRecordAudioFrameHandler OnRecordAudioFrameHandler} delegate.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRawDataManager.SetOnPlaybackAudioFrameCallback SetOnPlaybackAudioFrameCallback}</td>
<td>Listens for the {@link agora_gaming_rtc.AudioRawDataManager.OnPlaybackAudioFrameHandler OnPlaybackAudioFrameHandler} delegate.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRawDataManager.SetOnPlaybackAudioFrameBeforeMixingCallback SetOnPlaybackAudioFrameBeforeMixingCallback}</td>
<td>Listens for the {@link agora_gaming_rtc.AudioRawDataManager.OnPlaybackAudioFrameBeforeMixingHandler OnPlaybackAudioFrameBeforeMixingHandler} delegate.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRawDataManager.SetOnMixedAudioFrameCallback SetOnMixedAudioFrameCallback}</td>
<td>Listens for the {@link agora_gaming_rtc.AudioRawDataManager.OnMixedAudioFrameHandler OnMixedAudioFrameHandler} delegate.</td>
</tr>
</table>

<table>
<tr>
<th>Event</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRawDataManager.OnRecordAudioFrameHandler OnRecordAudioFrameHandler}</td>
<td>Retrieves the recorded audio frame.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRawDataManager.OnPlaybackAudioFrameHandler OnPlaybackAudioFrameHandler}</td>
<td>Retrieves the audio playback frame.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRawDataManager.OnPlaybackAudioFrameBeforeMixingHandler OnPlaybackAudioFrameBeforeMixingHandler}</td>
<td>Retrieves the audio frame of a specified user before mixing.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRawDataManager.OnMixedAudioFrameHandler OnMixedAudioFrameHandler}</td>
<td>Retrieves the mixed recorded and playback audio frame.</td>
</tr>
</table>


### <a name="rawvideo"></a >Raw Video Data

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.VideoRawDataManager.RegisterVideoRawDataObserver RegisterVideoRawDataObserver}</td>
<td>Registers a video frame observer object.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.VideoRawDataManager.UnRegisterVideoRawDataObserver UnRegisterVideoRawDataObserver}</td>
<td>UnRegisters the video raw data observer.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.VideoRawDataManager.SetOnCaptureVideoFrameCallback SetOnCaptureVideoFrameCallback}</td>
<td>Listens for the {@link agora_gaming_rtc.VideoRawDataManager.OnCaptureVideoFrameHandler OnCaptureVideoFrameHandler} delegate.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.VideoRawDataManager.SetOnRenderVideoFrameCallback SetOnRenderVideoFrameCallback}</td>
<td>Listens for the {@link agora_gaming_rtc.VideoRawDataManager.OnRenderVideoFrameHandler OnRenderVideoFrameHandler} delegate.</td>
</tr>
</table>

<table>
<tr>
<th>Event</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.VideoRawDataManager.OnCaptureVideoFrameHandler OnCaptureVideoFrameHandler}</td>
<td>Occurs when the camera captured image is received.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.VideoRawDataManager.OnRenderVideoFrameHandler OnRenderVideoFrameHandler}</td>
<td>Processes the received image of the specified user (post-processing).</td>
</tr>
</table>


### Media Metadata

> This group of methods apply to interactive live streaming only.

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.MetadataObserver.RegisterMediaMetadataObserver RegisterMediaMetadataObserver}</td>
<td>Registers a metadata observer.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.MetadataObserver.UnRegisterMediaMetadataObserver UnRegisterMediaMetadataObserver}</td>
<td>Unregisters a metadata observer.</td>
</tr>
</table>

<table>
<tr>
<th>Event</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.MetadataObserver.OnGetMaxMetadataSizeHandler OnGetMaxMetadataSizeHandler}</td>
<td>Occurs when the SDK requests the maximum size of the metadata.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.MetadataObserver.OnReadyToSendMetadataHandler OnReadyToSendMetadataHandler}</td>
<td>Occurs when the SDK is ready to receive and send metadata.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.MetadataObserver.OnMediaMetaDataReceivedHandler OnMediaMetaDataReceivedHandler}</td>
<td>Occurs when the local user receives the metadata.</td>
</tr>
</table>


### Watermark

> This group of methods apply to interactive live streaming only.

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.AddVideoWatermark(string watermarkUrl, WatermarkOptions watermarkOptions) AddVideoWatermark}</td>
<td>Adds a watermark image to the local video stream.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.ClearVideoWatermarks ClearVideoWatermarks}</td>
<td>Removes the added watermark image from the video stream.</td>
</tr>
</table>


### Encryption

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.EnableEncryption EnableEncryption}</td>
<td>Enables/Disables the built-in encryption.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.PacketObserver.RegisterPacketObserver RegisterPacketObserver}</td>
<td>Registers a packet observer.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.PacketObserver.UnRegisterPacketObserver UnRegisterPacketObserver}</td>
<td>UnRegisters the packet observer.</td>
</tr>
</table>

### Audio Recorder

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.StartAudioRecording(string filePath, int sampleRate, AUDIO_RECORDING_QUALITY_TYPE quality) StartAudioRecording}</td>
<td>Starts an audio recording on the client.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.StopAudioRecording StopAudioRecording}</td>
<td>Stops an audio recording on the client.</td>
</tr>
</table>

### Camera Control

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SwitchCamera SwitchCamera}</td>
<td>Switches between front and rear cameras (for Android and iOS only).</td>
</tr>
</table>

### Audio Device Manager

> This group of methods applies to Windows and macOS only.

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioPlaybackDeviceManager.CreateAAudioPlaybackDeviceManager CreateAAudioPlaybackDeviceManager}</td>
<td>Creates an AudioPlaybackDeviceManager instance.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioPlaybackDeviceManager.ReleaseAAudioPlaybackDeviceManager ReleaseAAudioPlaybackDeviceManager}</td>
<td>Releases an AudioPlaybackDeviceManager instance.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRecordingDeviceManager.CreateAAudioRecordingDeviceManager CreateAAudioRecordingDeviceManager}</td>
<td>Creates an AudioRecordingDeviceManager instance.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRecordingDeviceManager.ReleaseAAudioRecordingDeviceManager ReleaseAAudioRecordingDeviceManager}</td>
<td>Releases an AudioRecordingDeviceManager instance.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioPlaybackDeviceManager.GetAudioPlaybackDeviceCount GetAudioPlaybackDeviceCount}</td>
<td>Retrieves the total number of the indexed audio playback devices in the system.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRecordingDeviceManager.GetAudioRecordingDeviceCount GetAudioRecordingDeviceCount}</td>
<td>Retrieves the total number of the indexed audio capturing devices in the system.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioPlaybackDeviceManager.GetAudioPlaybackDevice GetAudioPlaybackDevice}</td>
<td>Retrieves the audio playback device associated with the index.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRecordingDeviceManager.GetAudioRecordingDevice GetAudioRecordingDevice}</td>
<td>Retrieves the audio capturing device associated with the index.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioPlaybackDeviceManager.SetAudioPlaybackDevice SetAudioPlaybackDevice}</td>
<td>Sets the audio playback device using the device ID.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRecordingDeviceManager.SetAudioRecordingDevice SetAudioRecordingDevice}</td>
<td>Sets the audio capturing device using the device ID.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioPlaybackDeviceManager.StartAudioPlaybackDeviceTest StartAudioPlaybackDeviceTest}</td>
<td>Starts the test of the current audio playback device.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioPlaybackDeviceManager.StopAudioPlaybackDeviceTest StopAudioPlaybackDeviceTest}</td>
<td>Stops the test of the current audio playback device.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRecordingDeviceManager.StartAudioRecordingDeviceTest StartAudioRecordingDeviceTest}</td>
<td>Starts the test of the current audio capturing device.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRecordingDeviceManager.StopAudioRecordingDeviceTest StopAudioRecordingDeviceTest}</td>
<td>Stops the test of the current audio capturing device.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioPlaybackDeviceManager.SetAudioPlaybackDeviceVolume SetAudioPlaybackDeviceVolume}</td>
<td>Sets the volume of the current audio playback device.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioPlaybackDeviceManager.GetAudioPlaybackDeviceVolume GetAudioPlaybackDeviceVolume}</td>
<td>Retrieves the volume of the current audio playback device.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRecordingDeviceManager.SetAudioRecordingDeviceVolume SetAudioRecordingDeviceVolume}</td>
<td>Sets the volume of the current audio capturing device.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRecordingDeviceManager.GetAudioRecordingDeviceVolume GetAudioRecordingDeviceVolume}</td>
<td>Retrieves the volume of the current audio capturing device.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioPlaybackDeviceManager.SetAudioPlaybackDeviceMute SetAudioPlaybackDeviceMute}</td>
<td>Sets whether to stop audio playback.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioPlaybackDeviceManager.IsAudioPlaybackDeviceMute IsAudioPlaybackDeviceMute}</td>
<td>Retrieves the status of the current audio playback device.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRecordingDeviceManager.SetAudioRecordingDeviceMute SetAudioRecordingDeviceMute}</td>
<td>Sets whether to stop audio capturing.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRecordingDeviceManager.IsAudioRecordingDeviceMute IsAudioRecordingDeviceMute}</td>
<td>Gets the status of the current audio capturing device.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioPlaybackDeviceManager.GetCurrentPlaybackDevice GetCurrentPlaybackDevice}</td>
<td>Retrieves the device ID of the current audio playback device.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRecordingDeviceManager.GetCurrentRecordingDevice GetCurrentRecordingDevice}</td>
<td>Retrieves the device ID of the current audio capturing device.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioPlaybackDeviceManager.GetCurrentPlaybackDeviceInfo GetCurrentPlaybackDeviceInfo}</td>
<td>Retrieves the device information of the current audio playback device.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.AudioRecordingDeviceManager.GetCurrentRecordingDeviceInfo GetCurrentRecordingDeviceInfo}</td>
<td>Retrieves the device information of the current audio capturing device.</td>
</tr>
</table>

<table>
<tr>
<th>Event</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnAudioDeviceStateChangedHandler OnAudioDeviceStateChangedHandler}</td>
<td>Occurs when the audio device state changes.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnAudioDeviceVolumeChangedHandler OnAudioDeviceVolumeChangedHandler}</td>
<td>Occurs when the volume of the playback, microphone, or application changes. (Windows only)</td>
</tr>
</table>

### Video Device Manager

> This group of methods applies to Windows and macOS only.

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.VideoDeviceManager.CreateAVideoDeviceManager CreateAVideoDeviceManager}</td>
<td>Creates an VideoDeviceManager instance.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.VideoDeviceManager.ReleaseAVideoDeviceManager ReleaseAVideoDeviceManager}</td>
<td>Releases an VideoDeviceManager instance.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.VideoDeviceManager.GetVideoDeviceCount GetVideoDeviceCount}</td>
<td>Retrieves the total number of the indexed video capturing devices in the system.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.VideoDeviceManager.GetVideoDevice GetVideoDevice}</td>
<td>Retrieves the video capturing device associated with the index.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.VideoDeviceManager.SetVideoDevice SetVideoDevice}</td>
<td>Sets the video capturing device using the device ID.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.VideoDeviceManager.StartVideoDeviceTest StartVideoDeviceTest}</td>
<td>Starts the video capturing device test.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.VideoDeviceManager.StopVideoDeviceTest StopVideoDeviceTest}</td>
<td>Stops the video capturing device test.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.VideoDeviceManager.GetCurrentVideoDevice GetCurrentVideoDevice}</td>
<td>Retrieves the device ID of the current video capturing device.</td>
</tr>
</table>

<table>
<tr>
<th>Event</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnVideoDeviceStateChangedHandler OnVideoDeviceStateChangedHandler}</td>
<td>Occurs when the video device state changes.</td>
</tr>
</table>



### Stream Message

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.CreateDataStream(DataStreamConfig config) CreateDataStream}</td>
<td>Creates a data stream.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SendStreamMessage SendStreamMessage}</td>
<td>Sends data stream messages to all users in a channel.</td>
</tr>
</table>

<table>
<tr>
<th>Event</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnStreamMessageHandler OnStreamMessageHandler}</td>
<td>Occurs when the local user receives the data stream from the remote user within five seconds.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnStreamMessageErrorHandler OnStreamMessageErrorHandler}</td>
<td>Occurs when the local user does not receive the data stream from the remote user within five seconds.</td>
</tr>
</table>


### Miscellaneous Audio Control

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.EnableLoopbackRecording EnableLoopbackRecording}</td>
<td>Enables loopback capturing (for macOS and Windows only).</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetAudioSessionOperationRestriction SetAudioSessionOperationRestriction}</td>
<td>Sets the audio session’s operational restriction (for iOS only).</td>
</tr>
</table>

### Miscellaneous Video Control
<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetCameraCapturerConfiguration SetCameraCapturerConfiguration}</td>
<td>Sets the camera capturer configuration.</td>
</tr>
</table>


### Miscellaneous Methods
<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.VideoSurface.SetGameFps SetGameFps}</td>
<td>Sets the video rendering frame rate.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.VideoSurface.EnableFilpTextureApply EnableFilpTextureApply}</td>
<td>Enables/Disables the mirror mode when renders the Texture.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.VideoSurface.SetVideoSurfaceType SetVideoSurfaceType}</td>
<td>Set the video renderer type.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.VideoSurface.SetEnable SetEnable}</td>
<td>Starts/Stops the video rendering.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SetCloudProxy SetCloudProxy}</td>
<td>Sets the Agora cloud proxy service.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.EnableDeepLearningDenoise EnableDeepLearningDenoise}</td>
<td>Enables/Disables deep-learning noise reduction.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.SendCustomReportMessage SendCustomReportMessage}</td>
<td>Reports customized messages. </td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.GetCallId GetCallId}</td>
<td>Retrieves the current call ID.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.Rate Rate}</td>
<td>Allows the user to rate the call and is called after the call ends.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.Complain Complain}</td>
<td>Allows a user to complain about the call quality after a call ends.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.GetSdkVersion GetSdkVersion}</td>
<td>Gets the SDK version number.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.IRtcEngine.GetErrorDescription GetErrorDescription}</td>
<td>Retrieves the description of a warning or error code.</td>
</tr>
</table>


### Miscellaneous Events

<table>
<tr>
<th>Event</th>
<th>Description</th>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnSDKWarningHandler OnSDKWarningHandler}</td>
<td>Reports a warning during SDK runtime.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnSDKErrorHandler OnSDKErrorHandler}</td>
<td>Reports an error during SDK runtime.</td>
</tr>
<tr>
<td>{@link agora_gaming_rtc.OnApiExecutedHandler OnApiExecutedHandler}</td>
<td>Occurs when a method is executed by the SDK.</td>
</tr>
</table>
