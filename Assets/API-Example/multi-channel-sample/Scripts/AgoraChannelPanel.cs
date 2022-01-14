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
    [SerializeField] private bool isPublishing;

    private AgoraChannel mChannel;
    private List<GameObject> userVideos;

    public Text txtLocaluserId;
    public Text txtInfo;
    public Text ChannelLabel;

    private const float SPACE_BETWEEN_USER_VIDEOS = 150f;

    public GameObject joinButton;
    public GameObject leaveButton;
    public GameObject publishButton;
    public GameObject unpublishButton;

    private VideoSurface localVideoSurface;
    private string channelToken;

    void Start()
    {
        ChannelLabel.text = channelName;
        userVideos = new List<GameObject>();
        SetButtonsState(true, false, false, false);
        if (UseToken)
        {
            TokenClient.Instance.GetTokens(channelName, ClientUID, (token, rtm) =>
        {
            channelToken = token;
            Debug.Log(gameObject.name + " Got rtc token:" + token);
        });
        }
    }

    void SetButtonsState(bool joinButtonFlag, bool leaveButtonFlag, bool publishButtonFlag, bool unpublishButtonFlag)
    {
        joinButton.SetActive(joinButtonFlag);
        publishButton.SetActive(publishButtonFlag);
        unpublishButton.SetActive(unpublishButtonFlag);
        leaveButton.SetActive(leaveButtonFlag);
    }

    #region Buttons

    public void Button_JoinChannel()
    {

        if (mChannel == null)
        {
            mChannel = IRtcEngine.QueryEngine().CreateChannel(channelName);

            mChannel.ChannelOnJoinChannelSuccess += OnJoinChannelSuccessHandler;
            mChannel.ChannelOnUserJoined += OnUserJoinedHandler;
            mChannel.ChannelOnLeaveChannel += OnLeaveHandler;
            mChannel.ChannelOnUserOffLine += OnUserLeftHandler;
            mChannel.ChannelOnRemoteVideoStats += OnRemoteVideoStatsHandler;
            mChannel.ChannelOnError += HandleChannelError;
            mChannel.ChannelOnClientRoleChanged += handleChannelOnClientRoleChangedHandler;
        }

        mChannel.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        mChannel.JoinChannel(channelToken, gameObject.name, ClientUID, new ChannelMediaOptions(true, true));
        Debug.Log("Joining channel: " + channelName);

    }

    public void Button_LeaveChannel()
    {
        if (mChannel != null)
        {
            isPublishing = false;
            mChannel.LeaveChannel();
            Debug.Log("Leaving channel: " + channelName);
            SetButtonsState(true, false, false, false);
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

        if (isPublishing == false)
        {
            int publishResult = mChannel.Publish();
            if (publishResult == 0)
            {
                isPublishing = true;
            }
            SetButtonsState(false, true, false, true);
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

        if (isPublishing == true)
        {
            int unpublishResult = mChannel.Unpublish();
            if (unpublishResult == 0)
            {
                isPublishing = false;
            }
            SetButtonsState(false, true, true, false);
            Debug.Log("Unpublish from channel: " + channelName + " result: " + unpublishResult);
        }
        else
        {
            Debug.Log("Not published to any channel");
        }
    }

    bool isLocalAudioMuted = false;
    bool isLocalVideoMuted = false;
    public void Button_MuteLocalAudio(Text buttonText)
    {
        isLocalAudioMuted = !isLocalAudioMuted;
        mChannel.MuteLocalAudioStream(isLocalAudioMuted);
        buttonText.text = isLocalAudioMuted ? "Unmute LocalAudio" : "Mute LocalAudio";
    }

    public void Button_MuteLocalVideo(Text buttonText)
    {
        isLocalVideoMuted = !isLocalVideoMuted;
        mChannel.MuteLocalVideoStream(isLocalVideoMuted);
        buttonText.text = isLocalVideoMuted ? "Unmute LocalVideo" : "Mute LocalVideo";
        localVideoSurface.SetEnable(!isLocalVideoMuted);
    }

    #endregion

    #region Callbacks
    void OnJoinChannelSuccessHandler(string channelID, uint uid, int elapsed)
    {
        Debug.Log("Join party channel success - channel: " + channelID + " uid: " + uid);
        txtLocaluserId.text = "Local user Id: " + uid;
        MakeImageSurface(channelID, uid, true);
        SetButtonsState(false, true, true, false);
    }

    void OnUserJoinedHandler(string channelID, uint uid, int elapsed)
    {
        Debug.Log("User: " + uid + "joined channel: + " + channelID);
    }

    void OnLeaveHandler(string channelID, RtcStats stats)
    {
        Debug.Log("You left the party channel.");
        foreach (GameObject player in userVideos)
        {
            Destroy(player);
        }

        userVideos.Clear();
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
    #endregion

    #region UI Handling
    void MakeImageSurface(string channelID, uint uid, bool isLocalUser = false)
    {
        Debug.Log("in MakeImageSurface");
        if (GameObject.Find(uid.ToString()) != null)
        {
            Debug.Log("A video surface already exists with this uid: " + uid.ToString());
            return;
        }

        // Create my new image surface
        GameObject go = new GameObject();
        go.name = uid.ToString();
        go.AddComponent<RawImage>();

        // Child it inside the panel scroller
        if (videoSpawnPoint != null)
        {
            go.transform.SetParent(videoSpawnPoint);
        }

        Debug.Log("in MakeImageSurface 2");

        go.transform.localScale = new Vector3(1, -1, 1);
        // Update the layout of the panel scrollers
        panelContentWindow.sizeDelta = new Vector2(0, userVideos.Count * SPACE_BETWEEN_USER_VIDEOS);
        float spawnY = userVideos.Count * SPACE_BETWEEN_USER_VIDEOS * -1;

        userVideos.Add(go);

        Debug.Log("in MakeImageSurface 3");

        go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, spawnY);
        Debug.Log("MakeImageSurface - before adding video surface");
        VideoSurface videoSurface = go.AddComponent<VideoSurface>();
        if (isLocalUser == false)
        {
            Debug.Log("MakeImageSurface - isLocalUser == false");
            videoSurface.SetForMultiChannelUser(channelID, uid);
            Debug.Log("MakeImageSurface - SetForMultiChannelUser after");
        }
        else
        {
            localVideoSurface = videoSurface;
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