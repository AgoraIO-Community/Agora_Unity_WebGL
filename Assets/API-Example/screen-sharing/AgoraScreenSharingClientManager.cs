using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using agora_utilities;

public class AgoraScreenSharingClientManager : MonoBehaviour
{
    [SerializeField] private AppInfoObject appInfo;

    [SerializeField] private string CHANNEL_NAME = "YOUR_CHANNEL_NAME_1";

    [SerializeField] private uint SCREEN_SHARE_ID = 1000;

    public Text logText;
    public Text screenShareIDText;
    private Logger logger;
    private IRtcEngine mRtcEngine = null;
    private const float Offset = 100;

    public Button joinButton, leaveButton;
    public Button startScreenShareButton, stopScreenShareButton;


    public bool useToken = false;
    public Toggle loopbackAudioToggle, newScreenShareToggle;
    public Toggle publishCameraToggle;
    public InputField screenShareIDInput;

    [Header("Mute Function Buttons")]
    public Text muteLocalVideoText;
    public Text muteRemoteVideoText;
    public Text muteLocalAudioText, muteRemoteAudioText;

    bool localVideoMuted, remoteVideoMuted, localAudioMuted, remoteAudioMuted;
    bool useNewScreenShare = false;
    bool useScreenShareAudio = false;
    bool joinedChannel = false;

    GameObject localView;

    private void Awake()
    {
        if (RootMenuControl.instance)
        {
            CHANNEL_NAME = RootMenuControl.instance.channel;
        }
    }

    // Use this for initialization
    void Start()
    {
        if (!CheckAppId())
        {
            return;
        }

        InitEngine();

        newScreenShareToggle.isOn = useNewScreenShare;
        loopbackAudioToggle.isOn = useScreenShareAudio;
        updateScreenShareNew();
        Debug.Log("ScreenShare UID = " + SCREEN_SHARE_ID);
        screenShareIDInput.text = SCREEN_SHARE_ID.ToString();
    }

    public void updateScreenShareNew()
    {
        useNewScreenShare = newScreenShareToggle.isOn;
        startScreenShareButton.onClick.RemoveAllListeners();
        stopScreenShareButton.onClick.RemoveAllListeners();
        if (!useNewScreenShare)
        {
            startScreenShareButton.onClick.AddListener(delegate { startScreenShare(useScreenShareAudio); });
            stopScreenShareButton.onClick.AddListener(delegate { stopScreenShare(); });
        }
        else
        {
            startScreenShareButton.onClick.AddListener(delegate { startNewScreenShare(useScreenShareAudio); });
            stopScreenShareButton.onClick.AddListener(delegate { stopNewScreenShare(); });
        }


    }

    void Update()
    {
        PermissionHelper.RequestMicrophontPermission();
        PermissionHelper.RequestCameraPermission();

        useScreenShareAudio = loopbackAudioToggle.isOn;

        muteLocalVideoText.text = localVideoMuted ? "Unmute Local Video" : "Mute Local Video";
        muteRemoteVideoText.text = remoteVideoMuted ? "Unmute Remote Video" : "Mute Remote Video";
        muteLocalAudioText.text = localAudioMuted ? "Unmute Local Audio" : "Mute Local Audio";
        muteRemoteAudioText.text = remoteAudioMuted ? "Unmute Remote Audio" : "Mute Remote Audio";

        if (joinedChannel)
        {
            joinButton.interactable = false;
            leaveButton.interactable = true;
        }
        else
        {
            joinButton.interactable = true;
            leaveButton.interactable = false;
        }
    }

    bool CheckAppId()
    {
        logger = new Logger(logText);
        logger.DebugAssert(appInfo.appID.Length > 10, "<color=red>[STOP] Please fill in your appId in your AppIDInfo Object!!!! \n (Assets/API-Example/_AppIDInfo/AppIDInfo)</color>");
        return (appInfo.appID.Length > 10);
    }

    public void updateScreenShareID()
    {
        uint.TryParse(screenShareIDInput.text, out SCREEN_SHARE_ID);
    }

    //for muting/unmuting local video through IRtcEngine class.
    public void setLocalMuteVideo()
    {
        localVideoMuted = !localVideoMuted;
        mRtcEngine.MuteLocalVideoStream(localVideoMuted);
    }

    //for muting/unmuting remote video through IRtcEngine class.
    public void setRemoteMuteVideo()
    {
        remoteVideoMuted = !remoteVideoMuted;
        mRtcEngine.MuteAllRemoteVideoStreams(remoteVideoMuted);
    }

    //for muting/unmuting local video through IRtcEngine class.
    public void setLocalMuteAudio()
    {
        localAudioMuted = !localAudioMuted;
        mRtcEngine.MuteLocalAudioStream(localAudioMuted);
    }

    //for muting/unmuting local video through IRtcEngine class.
    public void setRemoteMuteAudio()
    {
        remoteAudioMuted = !remoteAudioMuted;
        mRtcEngine.MuteAllRemoteAudioStreams(remoteAudioMuted);
    }

    //for starting/stopping a new screen share through IRtcEngine class.
    public void startNewScreenShare(bool audioEnabled)
    {
        updateScreenShareID();
        mRtcEngine.StartNewScreenCaptureForWeb(SCREEN_SHARE_ID, audioEnabled, appInfo.screenShareToken);
    }

    public void stopNewScreenShare()
    {
        mRtcEngine.StopNewScreenCaptureForWeb();
    }

    //for starting/stopping a screen share through IRtcEngine class.
    public void startScreenShare(bool audioEnabled)
    {
#if UNITY_EDITOR
        mRtcEngine.StartScreenCaptureByDisplayId(0, default, default);
#else
        mRtcEngine.StartScreenCaptureForWeb(audioEnabled);
#endif
    }

    public void stopScreenShare()
    {
        mRtcEngine.StopScreenCapture();
    }

    void InitEngine()
    {
        mRtcEngine = IRtcEngine.GetEngine(appInfo.appID);
        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);

        mRtcEngine.EnableAudio();
        mRtcEngine.EnableVideo();
        mRtcEngine.EnableVideoObserver();
        mRtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);

        mRtcEngine.OnJoinChannelSuccess = EngineOnJoinChannelSuccessHandler;
        mRtcEngine.OnLeaveChannel = EngineOnLeaveChannelHandler;
        mRtcEngine.OnScreenShareStarted += screenShareStartedHandler;
        mRtcEngine.OnScreenShareStopped += screenShareStoppedHandler;
        mRtcEngine.OnScreenShareCanceled += screenShareCanceledHandler;

        mRtcEngine.OnUserJoined += EngineOnUserJoinedHandler;
        mRtcEngine.OnUserOffline += EngineOnUserOfflineHandler;

        mRtcEngine.OnError += EngineOnErrorHandler;

    }

    public void JoinChannel()
    {

        ChannelMediaOptions options = new ChannelMediaOptions
        {
            autoSubscribeAudio = true,
            autoSubscribeVideo = true,
            publishLocalAudio = true,
            publishLocalVideo = this.publishCameraToggle.isOn
        };

        if (!useToken)
        {
            mRtcEngine.JoinChannel(appInfo.token, CHANNEL_NAME, "", 0, options);
        }
        else
        {
            TokenClient.Instance.RtcEngine = mRtcEngine;
            TokenClient.Instance.GetRtcToken(CHANNEL_NAME, 0, (token) =>
            {
                appInfo.token = token;
                Debug.Log(gameObject.name + " Got rtc token:" + appInfo.token);
                mRtcEngine.JoinChannelByKey(appInfo.token, CHANNEL_NAME);

                mRtcEngine.JoinChannel(appInfo.token, CHANNEL_NAME, "", 0, options);
            });
        }
        joinedChannel = true;
    }

    public void LeaveChannel()
    {
        mRtcEngine.LeaveChannel();
        Destroy(LastRemote);
        joinedChannel = false;
    }

    void OnDestroy()
    {
        Debug.Log("OnApplicationQuit");
        if (mRtcEngine != null)
        {
            LeaveChannel();
            mRtcEngine.DisableVideoObserver();
            IRtcEngine.Destroy();
        }
    }

    void userVideoMutedHandler(uint uid, bool muted)
    {
        logger.UpdateLog(string.Format("onUserMuteHandler uid: {0}, muted: {1}", uid, muted));
    }

    void screenShareStartedHandler(string channelId, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("onScreenShareStarted channelId: {0}, uid: {1}, elapsed: {2}", channelId, uid,
            elapsed));
    }

    void screenShareStoppedHandler(string channelId, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("onScreenShareStopped channelId: {0}, uid: {1}, elapsed: {2}", channelId, uid,
            elapsed));
    }

    void screenShareCanceledHandler(string channelId, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("onScreenShareCanceled channelId: {0}, uid: {1}, elapsed: {2}", channelId, uid,
            elapsed));
    }

    void screenShareStartedHandler_MC(string channelId, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("onScreenShareStartedMC channelId: {0}, uid: {1}, elapsed: {2}", channelId, uid,
            elapsed));
    }

    void screenShareStoppedHandler_MC(string channelId, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("onScreenShareStoppedMC channelId: {0}, uid: {1}, elapsed: {2}", channelId, uid,
            elapsed));

    }

    void screenShareCanceledHandler_MC(string channelId, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("onScreenShareCanceledMC channelId: {0}, uid: {1}, elapsed: {2}", channelId, uid,
            elapsed));
    }

    void EngineOnJoinChannelSuccessHandler(string channelId, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("sdk version: ${0}", IRtcEngine.GetSdkVersion()));
        logger.UpdateLog(string.Format("EngineOnJoinChannelSuccess channelId: {0}, uid: {1}, elapsed: {2}", CHANNEL_NAME, uid,
            elapsed));
        makeVideoView(channelId, 0);
    }

    void EngineOnLeaveChannelHandler(RtcStats rtcStats)
    {
        logger.UpdateLog(string.Format("OnLeaveChannelHandler channelId: {0}", CHANNEL_NAME));
        Destroy(localView);
    }

    void EngineOnErrorHandler(int err, string message)
    {
        logger.UpdateLog(string.Format("UserErrorHandler err: {0}, message: {1}", err,
            message));
    }

    void EngineOnUserJoinedHandler(uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("OnUserJoinedHandler channelId: {0} uid: ${1} elapsed: ${2}", CHANNEL_NAME,
            uid, elapsed));
        makeVideoView(CHANNEL_NAME, uid);
    }

    void EngineOnUserOfflineHandler(uint uid, USER_OFFLINE_REASON reason)
    {
        logger.UpdateLog(string.Format("OnUserOffLine uid: ${0}, reason: ${1}", uid, (int)reason));
        DestroyVideoView(CHANNEL_NAME, uid);
    }


    GameObject LastRemote = null;

    private void makeVideoView(string channelId, uint uid)
    {
        string objName = channelId + "_" + uid.ToString();
        GameObject go = GameObject.Find(objName);
        if (!ReferenceEquals(go, null))
        {
            return; // reuse
        }


        // create a GameObject and assign to this new user
        VideoSurface videoSurface = makeImageSurface(objName);
        if (!ReferenceEquals(videoSurface, null))
        {
            // configure videoSurface
            videoSurface.SetForUser(uid);
            videoSurface.SetEnable(true);
            videoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
            // make the object draggable
            videoSurface.gameObject.AddComponent<UIElementDragger>();

            if (uid == 0)
            {
                localView = videoSurface.gameObject;
            }
            else
            {
                LastRemote = videoSurface.gameObject;
            }
        }
    }

    // Video TYPE 2: RawImage
    public VideoSurface makeImageSurface(string goName)
    {
        GameObject go = new GameObject();

        if (go == null)
        {
            return null;
        }

        go.name = goName;
        // make the object draggable
        go.AddComponent<UIElementDrag>();
        // to be renderered onto
        go.AddComponent<RawImage>();

        GameObject canvas = GameObject.Find("VideoCanvas");
        if (canvas != null)
        {
            go.transform.SetParent(canvas.transform);
            Debug.Log("add video view");
        }
        else
        {
            Debug.Log("Canvas is null video view");
        }

        // set up transform
        go.transform.Rotate(0f, 0.0f, 180.0f);
        Vector2 pos = AgoraUIUtils.GetRandomPosition(60);
        go.transform.localPosition = new Vector3(pos.x, pos.y, 0f);
        go.transform.localScale = new Vector3(1.5f, 1f, 1f);

        // configure videoSurface
        VideoSurface videoSurface = go.AddComponent<VideoSurface>();
        return videoSurface;
    }

    private void DestroyVideoView(string channelId, uint uid)
    {
        string objName = channelId + "_" + uid.ToString();
        GameObject go = GameObject.Find(objName);
        if (!ReferenceEquals(go, null))
        {
            Object.Destroy(go);
        }
    }
}