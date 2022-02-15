// multi client manager
class EventManager {
  constructor() {}

  manipulate() {}

  raiseLastMileQuality(quality) {
    unityInstance.SendMessage("AgoraEventHandler", "LastMileQuality", quality);
  }

  raiseonCamerasListing(fstr) {
    unityInstance.SendMessage("AgoraEventHandler", "onCamerasListing", fstr);
  }

  raiseonRecordingDevicesListing(fstr) {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "onRecordingDevicesListing",
      fstr
    );
  }

  raiseonPlaybackDevicesListing(fstr) {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "onPlaybackDevicesListing",
      fstr
    );
  }

  raiseGetCurrentAudioDevice(currentAudioDevice) {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "OnCurrentChanges",
      "GetCurrentAudioDevice=" + currentAudioDevice
    );
  }

  raiseGetCurrentVideoDevice(currentVideoDevice) {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "OnCurrentChanges",
      "GetCurrentVideoDevice=" + currentVideoDevice
    );
  }

  raiseGetCurrentPlayBackDevice(currentPlayBackDevice) {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "OnCurrentChanges",
      "GetCurrentPlayBackDevice=" + currentPlayBackDevice
    );
  }

  isNumber(n) { return /^-?[\d.]+(?:e-?\d+)?$/.test(n); } 

  raiseJoinChannelSuccess(userId, channel) {
    var uid = 0;
    if (this.isNumber(userId)) {
	uid = userId.toString();
    }
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "onJoinChannelSuccess",
      channel + "|" + uid
    );
  }

  raiseOnLeaveChannel() {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "OnLeaveChannel",
      wrapper.buildStats()
    );
  }

  raiseOnLeaveChannel_MC(channel) {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "OnLeaveChannel_MC",
      channel
    );
  }

  raiseOnRemoteUserJoined(strUID) {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "onRemoteUserJoined",
      strUID
    );
  }

  raiseOnRemoteUserLeaved(strUID, reason) {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "onRemoteUserLeaved",
      strUID + "|" + reason
    );
  }

  raiseOnRemoteUserMuted(strUID, mediaType, muted) {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "onRemoteUserMuted",
      strUID + "|" + mediaType + "|" + muted
    ); 
  }

  raiseGetCurrentVideoDevice() {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "OnCurrentChanges",
      "GetCurrentVideoDevice=" + currentVideoDevice
    );
  }

  raiseGetCurrentAudioDevice() {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "OnCurrentChanges",
      "GetCurrentAudioDevice=" + currentAudioDevice
    );
  }

  raiseGetCurrentPlayBackDevice() {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "OnCurrentChanges",
      "GetCurrentPlayBackDevice=" + currentPlayBackDevice
    );
  }

  // multi channel events, AgoraChannel binding
  raiseJoinChannelSuccess_MC(userId, channel) {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "onJoinChannelSuccess_MC",
      channel + "|" + userId
    );
  }

  raiseChannelOnUserJoined_MC(userId, channel) {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "onChannelOnUserJoined_MC",
      channel + "|" + userId
    );
  }

  raiseChannelOnUserLeft_MC(userId, channel) {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "onChannelOnUserLeft_MC",
      channel + "|" + userId
    );
  }

  raiseChannelOnUserPublished_MC(channel, userId) {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "onChannelOnUserPublished_MC",
      channel + "|" + userId
    );
  }

  raiseChannelOnUserUnPublished_MC(channel, userId) {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "onChannelOnUserUnPublished_MC",
      channel + "|" + userId
    );
  }

  // TEMP EVENTS, JUST FOR TESTING
  raiseCustomMsg(msg) {
    unityInstance.SendMessage("AgoraEventHandler", "CustomMsg", msg);
  }

  raiseChannelOnTranscodingUpdated(channel) {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "ChannelOnTranscodingUpdated",
      channel
    );
  }

  raiseChannelOnClientRoleChanged(channel, old_role, new_role) {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "ChannelOnClientRoleChanged",
      channel + "|" + old_role + "|" + new_role
    );
  }

  raiseHandleChannelError(channel, err, message) {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "HandleChannelError",
      channel + "|" + err + "|" + message
    );
  }

  raiseFrameHandler(data_chunk) {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "HandleFrameHandler",
      data_chunk
    );
  }

  raiseVolumeIndicator(volumeInfo, speakers, total)
  {
    unityInstance.SendMessage(
      "AgoraEventHandler",
      "OnVolumeIndication",
      volumeInfo + "|" + speakers + "|" + total
    );
  }
}
