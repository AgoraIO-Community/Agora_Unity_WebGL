using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using agora_gaming_rtc;
using agora_utilities;


// this is an example of using Agora Unity SDK
// It demonstrates:
// How to enable video
// How to join/leave channel
// 
public class TestHelloUnityVideo
{

    // instance of agora engine
    private IRtcEngine mRtcEngine { get; set; }
    private Text MessageText { get; set; }

    private AudioVideoStates AudioVideoState = new AudioVideoStates();

    private List<GameObject> remoteUserDisplays = new List<GameObject>();

    private string mChannelName { get; set; }
    private Text ChannelNameLabel { get; set; }
    private CLIENT_ROLE_TYPE ClientRole { get; set; }

    private ToggleStateButton MuteAudioButton { get; set; }
    private ToggleStateButton MuteVideoButton { get; set; }
    private ToggleStateButton RoleButton { get; set; }
    private ToggleStateButton ChannelButton { get; set; }
    protected Dictionary<uint, VideoSurface> UserVideoDict = new Dictionary<uint, VideoSurface>();

    private bool UserEnableVideo = true;
    // Testing Volume Indication
    private bool TestVolumeIndication = false;

    // load agora engine
    public void loadEngine(string appId)
    {
        // start sdk
        Debug.Log("initializeEngine");

        if (mRtcEngine != null)
        {
            Debug.Log("Engine exists. Please unload it first!");
            return;
        }

        // init engine
        mRtcEngine = IRtcEngine.GetEngine(appId);

        // enable log
        mRtcEngine.SetLogFilter(LOG_FILTER.DEBUG | LOG_FILTER.INFO | LOG_FILTER.WARNING | LOG_FILTER.ERROR | LOG_FILTER.CRITICAL);
    }

    public void SetupInitState()
    {
        GameObject avObj = GameObject.Find("AVToggles");
        if (avObj != null)
        {
            var av = avObj.GetComponent<AudioVideoStateControl>();
            AudioVideoState.pubAudio = av.togglePubAudio.isOn;
            AudioVideoState.pubVideo = av.togglePubVideo.isOn;
            AudioVideoState.subAudio = av.toggleSubAudio.isOn;
            AudioVideoState.subVideo = av.toggleSubVideo.isOn;
        }
        else
        {
            Debug.Log("AV NULL");
        }
    }

    /// <summary>
    ///   Joining the Channel as Audience.  The user won't publish local video and audio.
    ///   User can switch channels as an audience.
    /// </summary>
    /// <param name="channel"></param>
    public void joinAudience(string channel)
    {
        Debug.Log("calling join (channel = " + channel + ")");

        SetupInitState();

        if (mRtcEngine == null)
            return;
        // set callbacks (optional)
        mRtcEngine.OnJoinChannelSuccess = onJoinChannelSuccess;
        mRtcEngine.OnUserJoined = onUserJoined;
        mRtcEngine.OnUserOffline = onUserOffline;
        mRtcEngine.OnLeaveChannel += OnLeaveChannelHandler;
        mRtcEngine.OnWarning = (int warn, string msg) =>
        {
            Debug.LogWarningFormat("Warning code:{0} msg:{1}", warn, IRtcEngine.GetErrorDescription(warn));
        };
        mRtcEngine.OnError = HandleError;
        mRtcEngine.OnClientRoleChanged += handleOnClientRoleChanged;
        mRtcEngine.OnRemoteVideoStateChanged += handleOnUserEnableVideo;
        mRtcEngine.OnClientRoleChangeFailed += OnClientRoleChangeFailedHandler;
        mRtcEngine.OnVideoSizeChanged += OnVideoSizeChangedHandler;
        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);
        mRtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE);
        mRtcEngine.EnableVideo();
        mRtcEngine.EnableVideoObserver();
        mRtcEngine.JoinChannelByKey("", channel);
        mChannelName = channel;
        ClientRole = CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE;
    }

    /// <summary>
    ///    Joining a channel as a host. This is same as running in Communication mode for other hosts.
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="enableVideoOrNot"></param>
    /// <param name="muted"></param>
    public void join(string channel, bool enableVideoOrNot, bool muted = false)
    {
        Debug.Log("calling join (channel = " + channel + ")");

        if (mRtcEngine == null)
            return;

        UserEnableVideo = enableVideoOrNot;

        SetupInitState();

        // set callbacks (optional)
        mRtcEngine.OnJoinChannelSuccess = onJoinChannelSuccess;
        mRtcEngine.OnUserJoined = onUserJoined;
        mRtcEngine.OnUserOffline = onUserOffline;
        mRtcEngine.OnLeaveChannel += OnLeaveChannelHandler;
        mRtcEngine.OnWarning = (int warn, string msg) =>
        {
            Debug.LogWarningFormat("Warning code:{0} msg:{1}", warn, IRtcEngine.GetErrorDescription(warn));
        };
        mRtcEngine.OnError = HandleError;

        mRtcEngine.OnUserMutedAudio = OnUserMutedAudio;
        mRtcEngine.OnUserMuteVideo = OnUserMutedVideo;
        //   mRtcEngine.OnVolumeIndication = OnVolumeIndicationHandler;
        mRtcEngine.OnClientRoleChanged += handleOnClientRoleChanged;
        mRtcEngine.OnRemoteVideoStateChanged += handleOnUserEnableVideo;
        mRtcEngine.OnClientRoleChangeFailed += OnClientRoleChangeFailedHandler;
        mRtcEngine.OnVideoSizeChanged += OnVideoSizeChangedHandler;

        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);
        mRtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);

        // Turn this on to receive volumenIndication
        if (TestVolumeIndication)
        {
            mRtcEngine.EnableAudioVolumeIndication(500, 8, report_vad: true);
        }

        var _orientationMode = ORIENTATION_MODE.ORIENTATION_MODE_FIXED_LANDSCAPE;

        VideoEncoderConfiguration config = new VideoEncoderConfiguration
        {
            orientationMode = _orientationMode,
            degradationPreference = DEGRADATION_PREFERENCE.MAINTAIN_FRAMERATE,
            mirrorMode = VIDEO_MIRROR_MODE_TYPE.VIDEO_MIRROR_MODE_DISABLED
            // note: mirrorMode is not effective for WebGL
        };
        mRtcEngine.SetVideoEncoderConfiguration(config);

        // enable video
        if (enableVideoOrNot)
        {
            mRtcEngine.EnableVideo();
            // allow camera output callback
            mRtcEngine.EnableVideoObserver();
        }
        else
        {
            mRtcEngine.DisableVideo();
            AudioVideoState.subVideo = false;
            AudioVideoState.pubVideo = false;
        }

        // NOTE, we use the third button to invoke JoinChannelByKey
        // otherwise, it joins using JoinChannelWithUserAccount
        if (muted)
        {
            // mute locally only. still subscribing
            mRtcEngine.EnableLocalAudio(false);
            mRtcEngine.MuteLocalAudioStream(true);
            mRtcEngine.JoinChannelByKey(channelKey: null, channelName: channel, info: "", uid: 0);
        }
        else
        {
            // join channel with string user name
            // ************************************************************************************* 
            // !!!  There is incompatibiity with string Native UID and Web string UIDs !!!
            // !!!  We strongly recommend to use uint uid only !!!!
            // mRtcEngine.JoinChannelWithUserAccount(null, channel, "user" + Random.Range(1, 99999),
            // ************************************************************************************* 
            mRtcEngine.JoinChannel(token: null, channelId: channel, info: "", uid: 0,
                 options: new ChannelMediaOptions(AudioVideoState.subAudio, AudioVideoState.subVideo,
                 AudioVideoState.pubAudio, AudioVideoState.pubVideo));
        }

        mChannelName = channel;
        ClientRole = CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER;
    }

    public string getSdkVersion()
    {
        string ver = IRtcEngine.GetSdkVersion();
        return ver;
    }

    public void leave()
    {
        if (mRtcEngine == null)
            return;

        // leave channel
        mRtcEngine.LeaveChannel();
        // deregister video frame observers in native-c code
        mRtcEngine.DisableVideoObserver();
    }

    // unload agora engine
    public void unloadEngine()
    {
        // delete
        if (mRtcEngine != null)
        {
            IRtcEngine.Destroy();  // Place this call in ApplicationQuit
            mRtcEngine = null;
        }
    }

    public void EnableVideo(bool pauseVideo)
    {
        if (mRtcEngine != null)
        {
            if (!pauseVideo)
            {
                mRtcEngine.EnableVideo();
            }
            else
            {
                mRtcEngine.DisableVideo();
            }
        }
    }

    // accessing GameObject in Scnene1
    // set video transform delegate for statically created GameObject
    public void onSceneHelloVideoLoaded()
    {
        // Attach the SDK Script VideoSurface for video rendering
        GameObject quad = GameObject.Find("Quad");
        if (ReferenceEquals(quad, null))
        {
            Debug.Log("failed to find Quad");
            return;
        }
        else
        {
            quad.AddComponent<VideoSurface>();
        }

        GameObject cube = GameObject.Find("Cube");
        if (ReferenceEquals(cube, null))
        {
            Debug.Log("failed to find Cube");
            return;
        }
        else
        {
            cube.AddComponent<VideoSurface>();
        }

        GameObject text = GameObject.Find("MessageText");
        if (!ReferenceEquals(text, null))
        {
            MessageText = text.GetComponent<Text>();
        }

        SetButtons();
    }

    private void SetButtons()
    {
        MuteAudioButton = GameObject.Find("MuteButton").GetComponent<ToggleStateButton>();
        if (MuteAudioButton != null)
        {
            MuteAudioButton.Setup(initOnOff: false,
                onStateText: "Mute Local Audio", offStateText: "Unmute Local Audio",
                callOnAction: () =>
                {
                    mRtcEngine.MuteLocalAudioStream(true);
                },
                callOffAction: () =>
                {
                    mRtcEngine.MuteLocalAudioStream(false);
                }
            );
        }

        MuteVideoButton = GameObject.Find("CamButton").GetComponent<ToggleStateButton>();
        if (MuteVideoButton != null)
        {
            MuteVideoButton.Setup(initOnOff: false,
                onStateText: "Mute Local video", offStateText: "Unmute Local video",
                callOnAction: () =>
                {
                    Debug.Log("muting video");
                    mRtcEngine.MuteLocalVideoStream(true);
                },
                callOffAction: () =>
                {
                    Debug.Log("unmuting video");
                    mRtcEngine.MuteLocalVideoStream(false);
                }
            );
        }
        MuteVideoButton.gameObject.SetActive(UserEnableVideo);

        ChannelButton = GameObject.Find("ChannelButton").GetComponent<ToggleStateButton>();
        if (ChannelButton != null)
        {
            ChannelButton.Setup(initOnOff: false,
                onStateText: mChannelName + "2", offStateText: mChannelName,
                callOnAction: () =>
                {
                    mRtcEngine.SwitchChannel(null, mChannelName + "2");
                },
                callOffAction: () =>
                {
                    mRtcEngine.SwitchChannel(null, mChannelName);
                }
                );
        }

        ChannelNameLabel = GameObject.Find("ChannelName").GetComponent<Text>();

        RoleButton = GameObject.Find("RoleButton").GetComponent<ToggleStateButton>();
        SetupRoleButton(isHost: ClientRole == CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);

        ChannelButton.GetComponent<Button>().interactable = ClientRole == CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE;
    }

    private void SetupRoleButton(bool isHost)
    {
        if (RoleButton != null)
        {
            RoleButton.Setup(initOnOff: isHost,
                 onStateText: "Host", offStateText: "Audience",
                 callOnAction: () =>
                 {
                     Debug.Log("Switching role to Broadcaster");
                     mRtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
                     ChannelButton.GetComponent<Button>().interactable = false;
                     MuteAudioButton.Reset();
                     MuteVideoButton.Reset();
                     MuteVideoButton.GetComponent<Button>().interactable = true;
                     MuteAudioButton.GetComponent<Button>().interactable = true;
                 },
                 callOffAction: () =>
                 {
                     Debug.Log("Switching role to Audience");
                     mRtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE);
                     ChannelButton.GetComponent<Button>().interactable = true;
                     MuteVideoButton.GetComponent<Button>().interactable = false;
                     MuteAudioButton.GetComponent<Button>().interactable = false;
                 }
                 );

        }
    }
    void handleOnClientRoleChanged(CLIENT_ROLE_TYPE oldRole, CLIENT_ROLE_TYPE newRole)
    {
        Debug.Log("Engine OnClientRoleChanged: " + oldRole + " -> " + newRole);
        if (newRole == CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER)
        {
            if (AudioVideoState.pubVideo)
            {
                mRtcEngine.EnableVideo();
                mRtcEngine.EnableVideoObserver();
                //mRtcEngine.EnableLocalVideo(true);
            }

            if (AudioVideoState.pubAudio)
            {
                mRtcEngine.EnableLocalAudio(true);
            }
        }
        else
        {
            if (AudioVideoState.pubVideo)
            {
                //mRtcEngine.EnableLocalVideo(false);
                mRtcEngine.EnableVideo();
                mRtcEngine.EnableVideoObserver();
            }

            if (AudioVideoState.pubAudio)
            {
                mRtcEngine.EnableLocalAudio(false);
            }
        }
    }

    void handleOnUserEnableVideo(uint uid, REMOTE_VIDEO_STATE state, REMOTE_VIDEO_STATE_REASON reason, int elapsed)
    {
        Debug.Log("remote video state:" + state.ToString());
        if (state == REMOTE_VIDEO_STATE.REMOTE_VIDEO_STATE_STARTING)
        {
            if (!UserVideoDict.ContainsKey(uid))
            {
                // create a GameObject and assign to this new user
                VideoSurface videoSurface = makeImageSurface(uid.ToString());
                if (!ReferenceEquals(videoSurface, null))
                {
                    // configure videoSurface
                    videoSurface.SetForUser(uid);
                    videoSurface.SetEnable(true);
                    videoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);

                    remoteUserDisplays.Add(videoSurface.gameObject);
                    UserVideoDict[uid] = videoSurface;
                }
            }
        }

    }

    void OnClientRoleChangeFailedHandler(CLIENT_ROLE_CHANGE_FAILED_REASON reason, CLIENT_ROLE_TYPE currentRole)
    {
        Debug.Log("Engine OnClientRoleChangeFaile: " + reason + " c-> " + currentRole);
    }
    private void OnUserMutedAudio(uint uid, bool muted)
    {
        Debug.LogFormat("user {0} muted audio:{1}", uid, muted);
    }

    private void OnUserMutedVideo(uint uid, bool muted)
    {
        Debug.LogFormat("user {0} muted video:{1}", uid, muted);
    }

    void OnVolumeIndicationHandler(AudioVolumeInfo[] speakers, int speakerNumber, int totalVolume)
    {
        Debug.Log("OnVolumeIndicationHandler speakerNumber:" + speakerNumber + " totalvolume:" + totalVolume);
        foreach (var sp in speakers)
        {
            Debug.LogFormat("Speaker:{0} level:{1} channel:{2}", sp.uid, sp.volume, sp.channelId);
        }
    }

    /// <summary>
    ///   Leaving the channel.  Clean up the subscription UIs.
    /// </summary>
    /// <param name="stats"></param>
    private void OnLeaveChannelHandler(RtcStats stats)
    {
        Debug.LogFormat("OnLeaveChannelSuccess ---- duration = {0} txVideoBytes:{1} ", stats.duration, stats.txVideoBytes);
        // Clean up the displays
        foreach (var go in remoteUserDisplays)
        {
            GameObject.Destroy(go);
        }
        remoteUserDisplays.Clear();
    }

    // implement engine callbacks
    private void onJoinChannelSuccess(string channelName, uint uid, int elapsed)
    {
        Debug.Log("JoinChannel " + channelName + " Success: uid = " + uid);
        GameObject textVersionGameObject = GameObject.Find("VersionText");
        textVersionGameObject.GetComponent<Text>().text = "SDK Version : " + getSdkVersion();
        ChannelNameLabel.text = channelName;
    }

    // When a remote user joined, this delegate will be called. Typically
    // create a GameObject to render video on it
    private void onUserJoined(uint uid, int elapsed)
    {
        Debug.Log("onUserJoined: uid = " + uid + " elapsed = " + elapsed);
        // this is called in main thread

        // find a game object to render video stream from 'uid'
        GameObject go = GameObject.Find(uid.ToString());
        if (go != null)
        {
            return; // reuse
        }

        // create a GameObject and assign to this new user
        VideoSurface videoSurface = makeImageSurface(uid.ToString());
        if (videoSurface != null)
        {
            // configure videoSurface
            videoSurface.SetForUser(uid);
            videoSurface.SetEnable(true);
            videoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);

            remoteUserDisplays.Add(videoSurface.gameObject);
            UserVideoDict[uid] = videoSurface;
        }

        // This will trigger OnVideoSizeChanged
        mRtcEngine.GetRemoteVideoStats();
    }

    float EnforcingViewLength = 360f;
    void OnVideoSizeChangedHandler(uint uid, int width, int height, int rotation)
    {
        Debug.LogWarning(string.Format("OnVideoSizeChangedHandler, uid:{0}, width:{1}, height:{2}, rotation:{3}", uid, width, height, rotation));


        if (UserVideoDict.ContainsKey(uid))
        {
            GameObject go = UserVideoDict[uid].gameObject;
            Vector2 v2 = new Vector2(width, height);
            RawImage image = go.GetComponent<RawImage>();
            v2 = AgoraUIUtils.GetScaledDimension(width, height, EnforcingViewLength);

            if (rotation == 90 || rotation == 270)
            {
                v2 = new Vector2(v2.y, v2.x);
            }
            image.rectTransform.sizeDelta = v2;

            // if (0,0) we will get a default dimension. but let's still check for the actual dimension
            if (width == 0 && height == 0)
            {
                go.GetComponent<MonoBehaviour>().StartCoroutine(CoGetVideoResolutionDelayed(1));
            }
        }
    }

    IEnumerator CoGetVideoResolutionDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.LogWarning("Triggering GetRemoteVideoStats");
        // This will trigger OnVideoSizeChanged
        mRtcEngine.GetRemoteVideoStats();
    }

    VideoSurface makePlaneSurface(string goName)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Plane);

        if (go == null)
        {
            return null;
        }
        go.name = goName;
        // set up transform
        go.transform.Rotate(-90.0f, 0.0f, 0.0f);
        float yPos = Random.Range(3.0f, 5.0f);
        float xPos = Random.Range(-2.0f, 2.0f);
        go.transform.position = new Vector3(xPos, yPos, 0f);
        go.transform.localScale = new Vector3(0.25f, 0.5f, .5f);

        // configure videoSurface
        VideoSurface videoSurface = go.AddComponent<VideoSurface>();
        return videoSurface;
    }

    private const float Offset = 100;
    public VideoSurface makeImageSurface(string goName)
    {
        GameObject go = new GameObject();

        if (go == null)
        {
            return null;
        }
        Debug.Log("Making GameObject:" + goName);
        go.name = goName;

        // to be renderered onto
        go.AddComponent<RawImage>();

        // make the object draggable
        go.AddComponent<UIElementDragger>();
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            go.transform.SetParent(canvas.transform);
        }
        // set up transform
        go.transform.Rotate(0f, 0.0f, 180.0f);
        Vector2 pos = AgoraUIUtils.GetRandomPosition(60);
        go.transform.localPosition = new Vector3(pos.x, pos.y, 0f);

        // configure videoSurface
        VideoSurface videoSurface = go.AddComponent<VideoSurface>();
        return videoSurface;
    }


    // When remote user is offline, this delegate will be called. Typically
    // delete the GameObject for this user
    private void onUserOffline(uint uid, USER_OFFLINE_REASON reason)
    {
        // remove video stream
        Debug.Log("onUserOffline: uid = " + uid + " reason = " + reason);
        // this is called in main thread
        GameObject go = GameObject.Find(uid.ToString());
        if (go != null)
        {
            Object.Destroy(go);
        }
        else
        {
            Debug.Log(uid + " not found");
        }
    }

    #region Error Handling
    private int LastError { get; set; }
    private void HandleError(int error, string msg)
    {
        if (error == LastError)
        {
            return;
        }

        msg = string.Format("Error code:{0} msg:{1}", error, IRtcEngine.GetErrorDescription(error));

        switch (error)
        {
            case 101:
                msg += "\nPlease make sure your AppId is valid and it does not require a certificate for this demo.";
                break;
        }

        Debug.LogError(msg);
        if (MessageText != null)
        {
            if (MessageText.text.Length > 0)
            {
                msg = "\n" + msg;
            }
            MessageText.text += msg;
        }

        LastError = error;
    }

    #endregion
}
