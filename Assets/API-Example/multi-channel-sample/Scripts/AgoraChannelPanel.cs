﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;
using UnityEngine.UI;
using agora_utilities;

public class AgoraChannelPanel : MonoBehaviour
{
    [SerializeField] private string channelName;
    [SerializeField] uint ClientUID;

    [Tooltip("Use token with the TokenClient Object")]
    [SerializeField] bool UseToken;

    [SerializeField] private Transform videoSpawnPoint;
    [SerializeField] private RectTransform panelContentWindow;
    [SerializeField] private bool AudienceMode;
    [SerializeField] Text InfoText;

    private bool IsPublishing { get; set; }
    private bool InChannel { get; set; }

    private AgoraChannel mChannel;
    private List<GameObject> userVideos;

    public Text txtLocaluserId;
    public Text txtInfo;
    public Text ChannelLabel;

    private const float SPACE_BETWEEN_USER_VIDEOS = 150f;

    public ToggleStateButton JoinChannelButton;
    public ToggleStateButton PublishButton;
    public ToggleStateButton ScreenShareButton;
    public ToggleStateButton MuteAudioButton;
    public ToggleStateButton MuteVideoButton;
    public ToggleStateButton ClientRoleButton;
    public Toggle subscribeAudio;
    public Toggle subscribeVideo;
    public Toggle publishAudio;
    public Toggle publishVideo;

    List<ToggleStateButton> ButtonCollection = new List<ToggleStateButton>();
    List<Toggle> ToggleCollection = new List<Toggle>();

    private string channelToken;

    void Start()
    {
        ChannelLabel.text = channelName;
        userVideos = new List<GameObject>();

        if (JoinChannelButton != null)
        {
            JoinChannelButton.Setup(initOnOff: false,
                onStateText: "Join Channel", offStateText: "Leave Channel",
                callOnAction: Button_JoinChannel,
                callOffAction: Button_LeaveChannel
            );
            ButtonCollection.Add(JoinChannelButton);
        }

        if (PublishButton != null)
        {
            PublishButton.Setup(initOnOff: false,
                onStateText: "Publish", offStateText: "Unpublish",
                callOnAction: Button_PublishToPartyChannel,
                callOffAction: Button_CancelPublishFromChannel
            );
            ButtonCollection.Add(PublishButton);
        }

        if (MuteAudioButton != null)
        {
            MuteAudioButton.Setup(initOnOff: true,
                onStateText: "Unmute Audio", offStateText: "Mute Audio",
                callOnAction: Button_MuteLocalAudio,
                callOffAction: Button_MuteLocalAudio
            );
            ButtonCollection.Add(MuteAudioButton);
        }

        if (MuteVideoButton != null)
        {
            MuteVideoButton.Setup(initOnOff: true,
                onStateText: "Unmute Video", offStateText: "Mute Video",
                callOnAction: Button_MuteLocalVideo,
                callOffAction: Button_MuteLocalVideo
            );
            ButtonCollection.Add(MuteVideoButton);
        }

        if (ScreenShareButton != null)
        {
            ScreenShareButton.Setup(initOnOff: false,
                onStateText: "Share Screen (Web)", offStateText: "Stop ScreenShare",
                callOnAction: Button_ShareScreen,
                callOffAction: Button_ShareScreen
            );
            ButtonCollection.Add(ScreenShareButton);
        }

        SetupRoleButton(isHost: !AudienceMode);

        SetButtonsState(false, false, false, false);

        InfoText.text = AudienceMode ? "Audience" : "Broadcaster";

        if (publishVideo != null) { ToggleCollection.Add(publishVideo); }
        if (publishAudio != null) { ToggleCollection.Add(publishAudio); }
        if (subscribeVideo != null) { ToggleCollection.Add(subscribeVideo); }
        if (subscribeAudio != null) { ToggleCollection.Add(subscribeAudio); }
    }

    private void SetupRoleButton(bool isHost)
    {
        if (ClientRoleButton != null)
        {
            ClientRoleButton.Setup(initOnOff: isHost,
                 onStateText: "To Host", offStateText: "To Audience",
                 callOnAction: () =>
                 {
                     Debug.LogWarning("Switching role to Broadcaster");
                     if (mChannel != null)
                     {
                         mChannel.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
                     }
                     AudienceMode = false;
                     InfoText.text = "Broadcaster";
                     // change role to Broadcaster will automatically publish
                     if (InChannel)
                     {
                         SetButtonsState(true, true, true, true);
                         // set to unpublish
                         PublishButton.SetState(true);
                         IsPublishing = true;
                     }
                     else
                     {
                         SetButtonsState(false, false, false, false);
                     }
                 },
                 callOffAction: () =>
                 {
                     Debug.LogWarning("Switching role to Audience");
                     if (mChannel != null)
                     {
                         mChannel.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE);
                     }
                     AudienceMode = true;
                     InfoText.text = "Audience";
                     if (IsPublishing)
                     {
                         Button_CancelPublishFromChannel();
                     }
                     SetButtonsState(false, false, false, false);
                 }
                 );
        }
    }

    void SetButtonsState(bool publishButtonFlag, bool screenShareButtonFlag, bool muteAudioFlag, bool muteVideoFlag)
    {
        if (PublishButton)
        {
            PublishButton.gameObject.SetActive(publishButtonFlag);
        }
        if (ScreenShareButton)
        {
            ScreenShareButton.gameObject.SetActive(screenShareButtonFlag);
        }
        if (MuteAudioButton)
        {
            MuteAudioButton.gameObject.SetActive(muteAudioFlag);
        }
        if (MuteVideoButton)
        {
            MuteVideoButton.gameObject.SetActive(muteVideoFlag);
        }
    }

    void ResetButtons()
    {
        foreach (var btn in ButtonCollection)
        {
            btn.Reset();
        }
    }

    void ActiveToggles(bool on)
    {
        foreach (var tog in ToggleCollection)
        {
            tog.interactable = on;
        }
    }
    #region Buttons

    public void Restart()
    {
        StartCoroutine(CoRestart());
    }

    IEnumerator CoRestart()
    {
        Button_LeaveChannel();
        yield return new WaitForSeconds(0.5f);

        if (mChannel != null)
        {
            mChannel.ReleaseChannel();
            mChannel = null;
        }
        IRtcEngine.Destroy();

        yield return new WaitForFixedUpdate();
        MultiChannelSceneCtrl.Instance.SetupEngine(resetting: true);
    }

    public void Button_JoinChannel()
    {
        var engine = IRtcEngine.QueryEngine();
        if (engine != null)
        {
            engine.OnVolumeIndication += OnVolumeIndicationHandler;

            if (mChannel != null)
            {
                mChannel.ReleaseChannel();
            }
            mChannel = engine.CreateChannel(channelName);

            mChannel.EnableAudioVolumeIndicator();
            mChannel.ChannelOnJoinChannelSuccess += OnJoinChannelSuccessHandler;
            mChannel.ChannelOnUserJoined += OnUserJoinedHandler;
            mChannel.ChannelOnLeaveChannel = OnLeaveHandler;
            mChannel.ChannelOnUserOffLine += OnUserLeftHandler;
            mChannel.ChannelOnRemoteVideoStats += OnRemoteVideoStatsHandler;
            mChannel.ChannelOnError += HandleChannelError;
            mChannel.ChannelOnClientRoleChanged += handleChannelOnClientRoleChangedHandler;



        }


        ChannelMediaOptions channelMediaOptions = new ChannelMediaOptions(
            subscribeAudio == null ? true : subscribeAudio.isOn,
            subscribeVideo == null ? true : subscribeVideo.isOn,
            publishAudio == null ? false : publishAudio.isOn,
            publishVideo == null ? false : publishVideo.isOn
        );

        if (AudienceMode)
        {
            mChannel.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE);
        }
        else
        {
            mChannel.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        }
        if (UseToken)
        {
            TokenClient.Instance.SetMultiChannelInstance(mChannel);
            TokenClient.Instance.GetTokens(channelName, ClientUID, (token, rtm) =>
            {
                channelToken = token;
                Debug.Log(gameObject.name + " Got rtc token:" + token);
                mChannel.JoinChannel(channelToken, gameObject.name, ClientUID, channelMediaOptions);
            });
        }
        else
        {
            channelToken = null;
            mChannel.JoinChannel(channelToken, gameObject.name, ClientUID, channelMediaOptions);
        }

        Debug.Log("Joining channel: " + channelName);
    }

    public void Button_LeaveChannel()
    {
        if (mChannel != null)
        {
            Debug.Log("Leaving channel: " + channelName);

            IsPublishing = false;
            var engine = IRtcEngine.QueryEngine();
            engine.OnVolumeIndication -= OnVolumeIndicationHandler;
            mChannel.ChannelOnJoinChannelSuccess -= OnJoinChannelSuccessHandler;
            mChannel.ChannelOnUserJoined -= OnUserJoinedHandler;
            mChannel.ChannelOnUserOffLine -= OnUserLeftHandler;
            mChannel.ChannelOnRemoteVideoStats -= OnRemoteVideoStatsHandler;
            mChannel.ChannelOnError -= HandleChannelError;
            mChannel.ChannelOnClientRoleChanged -= handleChannelOnClientRoleChangedHandler;

            mChannel.LeaveChannel();
            SetButtonsState(false, false, false, false);
            ResetButtons();
            ActiveToggles(true);
        }
        else
        {
            Debug.LogWarning("Channel: " + channelName + " hasn't been created yet.");
        }
    }

    public void Button_PublishToPartyChannel()
    {
        if (mChannel == null)
        {
            Debug.LogError("New channel isn't created yet.");
            return;
        }

        if (IsPublishing == false)
        {
            int publishResult = mChannel.Publish();
            if (publishResult == 0)
            {
                IsPublishing = true;
            }
            SetButtonsState(true, true, true, true);
            Debug.Log("Publishing to channel: " + channelName + " result: " + publishResult);
        }
        else
        {
            Debug.Log("Already publishing to a channel.");
        }
    }

    public void Button_CancelPublishFromChannel()
    {
        if (mChannel == null)
        {
            Debug.Log("New channel isn't created yet.");
            return;
        }

        if (IsPublishing == true)
        {
            int unpublishResult = mChannel.Unpublish();
            if (unpublishResult == 0)
            {
                IsPublishing = false;
            }
            SetButtonsState(true, false, false, false);
            Debug.Log("Unpublish from channel: " + channelName + " result: " + unpublishResult);
        }
        else
        {
            Debug.Log("Not published to any channel");
        }
    }

    public void Button_MuteLocalAudio()
    {
        // on means muted
        mChannel.MuteLocalAudioStream(!MuteAudioButton.OnOffState);
    }

    public void Button_MuteLocalVideo()
    {
        // on means muted
        mChannel.MuteLocalVideoStream(!MuteVideoButton.OnOffState);
    }

    public void Button_ShareScreen()
    {
        if (ScreenShareButton.OnOffState)
        {
            Debug.Log("Screen sharing button.... share");
            mChannel.StartScreenCaptureForWeb(false);
        }
        else
        {
            Debug.Log("Screen sharing button.... stop");
            mChannel.StopScreenCapture();
        }
    }

    #endregion

    #region Callbacks
    void OnJoinChannelSuccessHandler(string channelID, uint uid, int elapsed)
    {
        Debug.Log("Join party channel success - channel: " + channelID + " uid: " + uid);
        txtLocaluserId.text = "Local user Id: " + uid;
        MakeImageSurface(channelID, uid, true);
        if ((publishAudio != null && publishAudio.isOn) || (publishVideo != null && publishVideo.isOn))
        {
            SetButtonsState(true, true, true, true);
            PublishButton.SetState(true);
            IsPublishing = true;
        }
        else
        {
            SetButtonsState(!AudienceMode, false, false, false);
        }
        InChannel = true;
        ActiveToggles(false);
    }

    void OnUserJoinedHandler(string channelID, uint uid, int elapsed)
    {
        Debug.Log("User: " + uid + "joined channel: + " + channelID);
        MakeImageSurface(channelID, uid);
    }

    void OnLeaveHandler(string channelID, RtcStats stats)
    {
        Debug.Log("You left the party channel.");
        Debug.LogFormat("OnLeaveChannelSuccess ---- duration = {0} txVideoBytes:{1} ", stats.duration, stats.txVideoBytes);

        foreach (GameObject player in userVideos)
        {
            Debug.LogWarning("Destroying " + player.name);
            Destroy(player);
        }

        userVideos.Clear();
        InChannel = false;
    }

    void OnUserLeftHandler(string channelID, uint uid, USER_OFFLINE_REASON reason)
    {
        Debug.Log("User: " + uid + " left party - channel: + " + uid + "for reason: " + reason);
        RemoveUserVideoSurface(uid);
    }


    void handleChannelOnClientRoleChangedHandler(string channelId, CLIENT_ROLE_TYPE oldRole, CLIENT_ROLE_TYPE newRole)
    {
        //txtConState.text = "ChannelOnClientRoleChanged -> " + oldRole + " to " + newRole;
    }

    void OnRemoteVideoStatsHandler(string channelID, RemoteVideoStats remoteStats)
    {
        return;
        Debug.Log("UNITY -> OnRemoteVideoStatsHandler = channelID: " + channelID
            + ", remoteStats.receivedBitrate: " + remoteStats.receivedBitrate
            + ", remoteStats.uid: " + remoteStats.uid);
        // If user is publishing...
        if (remoteStats.receivedBitrate > 0)
        {
            bool needsToMakeNewImageSurface = true;
            foreach (GameObject user in userVideos)
            {
                if (remoteStats.uid.ToString() == user.name)
                {
                    needsToMakeNewImageSurface = false;
                    break;
                }
            }
            // ... and their video surface isn't currently displaying ...
            if (needsToMakeNewImageSurface == true)
            {
                // ... display their feed.
                MakeImageSurface(channelID, remoteStats.uid);
            }
        }
        // If user isn't publishing ...
        else if (remoteStats.receivedBitrate == 0)
        {
            bool needsToRemoveUser = false;
            foreach (GameObject user in userVideos)
            {
                // ... but their video stream is currently displaying...
                if (remoteStats.uid.ToString() == user.name)
                {
                    needsToRemoveUser = true;
                }
            }

            if (needsToRemoveUser == true)
            {
                // ... remove their feed.
                RemoveUserVideoSurface(remoteStats.uid);
            }
        }
    }

    void HandleChannelError(string channelId, int err, string message)
    {
        Debug.LogWarning("Channel Error: " + IRtcEngine.GetErrorDescription(err));
    }

    void OnVolumeIndicationHandler(AudioVolumeInfo[] speakers, int speakerNumber, int totalVolume)
    {
        Debug.Log("OnVolumeIndicationHandler speakerNumber:" + speakerNumber + " totalvolume:" + totalVolume);
        foreach (var sp in speakers)
        {
            Debug.LogFormat("Speaker:{0} level:{1} channel:{2}", sp.uid, sp.volume, sp.channelId);
        }
    }
    #endregion

    #region UI Handling
    void MakeImageSurface(string channelID, uint uid, bool isLocalUser = false)
    {
        string objName = isLocalUser ? "LocalUser" : uid.ToString();
        if (GameObject.Find(objName) != null)
        {
            Debug.Log("A video surface already exists with this uid: " + uid.ToString());
            return;
        }

        // Create my new image surface
        GameObject go = new GameObject();
        go.name = objName;
        go.AddComponent<RawImage>();

        // Child it inside the panel scroller
        if (videoSpawnPoint != null)
        {
            go.transform.SetParent(videoSpawnPoint);
        }


        go.transform.localScale = new Vector3(1, -1, 1);
        // Update the layout of the panel scrollers
        panelContentWindow.sizeDelta = new Vector2(0, userVideos.Count * SPACE_BETWEEN_USER_VIDEOS);
        float spawnY = userVideos.Count * SPACE_BETWEEN_USER_VIDEOS * -1;

        userVideos.Add(go);


        go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, spawnY);
        VideoSurface videoSurface = go.AddComponent<VideoSurface>();
        if (isLocalUser == false)
        {
            videoSurface.SetForMultiChannelUser(channelID, uid);
        }
    }


    private void UpdatePlayerVideoPostions()
    {
        for (int i = 0; i < userVideos.Count; i++)
        {
            userVideos[i].GetComponent<RectTransform>().anchoredPosition = Vector2.down * SPACE_BETWEEN_USER_VIDEOS * i;
        }
    }

    private void RemoveUserVideoSurface(uint deletedUID)
    {
        foreach (GameObject user in userVideos)
        {
            if (user.name == deletedUID.ToString())
            {
                userVideos.Remove(user);
                Destroy(user);

                UpdatePlayerVideoPostions();

                Vector2 oldContent = panelContentWindow.sizeDelta;
                panelContentWindow.sizeDelta = oldContent + Vector2.down * SPACE_BETWEEN_USER_VIDEOS;
                panelContentWindow.anchoredPosition = Vector2.zero;
                break;
            }
        }
    }
    #endregion

    private void OnApplicationQuit()
    {
        if (mChannel != null)
        {
            mChannel.LeaveChannel();
            mChannel.ReleaseChannel();
        }
    }


}