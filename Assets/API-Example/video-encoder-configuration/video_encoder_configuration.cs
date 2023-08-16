using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using agora_utilities;

public class video_encoder_configuration : MonoBehaviour
{
    [SerializeField]
    private AppInfoObject appInfo;

    [SerializeField]
    private string CHANNEL_NAME = "YOUR_CHANNEL_NAME";


    public Text logText;
    public Button statsButton;

    private Logger logger;
    private IRtcEngine mRtcEngine = null;
    private const float Offset = 100;

    [SerializeField]
    bool LogEnableUpload = false;

    // A list of dimensions for swithching
    VideoDimensions[] dimensions = new VideoDimensions[]{
        new VideoDimensions { width = 640, height = 480 },
        new VideoDimensions { width = 480, height = 480 },
        new VideoDimensions { width = 480, height = 240 }
    };

    private void Awake()
    {
        if (RootMenuControl.instance)
        {
            CHANNEL_NAME = RootMenuControl.instance.channel;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        bool appReady = CheckAppId();
        if (!appReady) return;

        InitUI();
        InitEngine();
        SetVideoEncoderConfiguration(); // use default one
        JoinChannel();
    }

    void InitUI()
    {
        statsButton.onClick.AddListener(() =>
        {
            Debug.Log("Calling GetRemoteVideoStats, this API will trigger OnVideoSizeChanged for WebGL only.");
            mRtcEngine.GetRemoteVideoStats();
        });

    }
    // Update is called once per frame
    void Update()
    {
        PermissionHelper.RequestMicrophontPermission();
        PermissionHelper.RequestCameraPermission();
    }

    bool CheckAppId()
    {
        logger = new Logger(logText);
        return logger.DebugAssert(appInfo.appID.Length > 10, "<color=red>[STOP] Please fill in your appId in your AppIDInfo Object!!!! \n (Assets/API-Example/_AppIDInfo/AppIDInfo)</color>");
    }

    void InitEngine()
    {
        mRtcEngine = IRtcEngine.GetEngine(appInfo.appID);
        mRtcEngine.SetLogFile("log.txt");
        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);
        mRtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        mRtcEngine.EnableAudio();
        mRtcEngine.EnableVideo();
        mRtcEngine.EnableVideoObserver();
        mRtcEngine.OnJoinChannelSuccess += OnJoinChannelSuccessHandler;
        mRtcEngine.OnLeaveChannel += OnLeaveChannelHandler;
        mRtcEngine.OnWarning += OnSDKWarningHandler;
        mRtcEngine.OnError += OnSDKErrorHandler;
        mRtcEngine.OnConnectionLost += OnConnectionLostHandler;
        mRtcEngine.OnUserJoined += OnUserJoinedHandler;
        mRtcEngine.OnUserOffline += OnUserOfflineHandler;
        mRtcEngine.OnVideoSizeChanged += OnVideoSizeChangedHandler;
    }

    void JoinChannel()
    {
        if (LogEnableUpload)
        {
            mRtcEngine.EnableLogUpload();
            logger.UpdateLog("Enabled Console log upload, before joining");
        }
        mRtcEngine.JoinChannelByKey(appInfo.token, CHANNEL_NAME, "", 0);
    }


    /// <summary>
    ///   Setting the Encoder Configuration.  Possbily called from UI button
    /// </summary>
    /// <param name="dim"></param>
    public void SetVideoEncoderConfiguration(int dim = 0)
    {
        if (dim >= dimensions.Length)
        {
            Debug.LogError("Invalid dimension choice!");
            return;
        }

        string logtext = string.Format("Setting dimension w,h = {0},{1}", dimensions[dim].width, dimensions[dim].height);
        logger.UpdateLog(logtext);

        VideoEncoderConfiguration config = new VideoEncoderConfiguration
        {
            dimensions = dimensions[dim],
            frameRate = FRAME_RATE.FRAME_RATE_FPS_15,
            minFrameRate = -1,
            bitrate = 0,
            minBitrate = 1,
            orientationMode = ORIENTATION_MODE.ORIENTATION_MODE_ADAPTIVE,
            degradationPreference = DEGRADATION_PREFERENCE.MAINTAIN_FRAMERATE,
            mirrorMode = VIDEO_MIRROR_MODE_TYPE.VIDEO_MIRROR_MODE_AUTO
        };
        mRtcEngine.SetVideoEncoderConfiguration(config);
    }

    void OnVideoSizeChangedHandler(uint uid, int width, int height, int rotation)
    {
        logger.UpdateLog(string.Format("OnVideoSizeChanged - uid:{0}, w:{1}, h:{2}, r:{3}", uid, width, height, rotation));
    }

    void OnJoinChannelSuccessHandler(string channelName, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("sdk version: {0}", IRtcEngine.GetSdkVersion()));
        logger.UpdateLog(string.Format("onJoinChannelSuccess channelName: {0}, uid: {1}, elapsed: {2}", channelName, uid, elapsed));
        makeVideoView(0);
    }

    void OnLeaveChannelHandler(RtcStats stats)
    {
        logger.UpdateLog("OnLeaveChannelSuccess");
        DestroyVideoView(0);
    }

    void OnUserJoinedHandler(uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("OnUserJoined uid: {0} elapsed: {1}", uid, elapsed));
        makeVideoView(uid);
    }

    void OnUserOfflineHandler(uint uid, USER_OFFLINE_REASON reason)
    {
        logger.UpdateLog(string.Format("OnUserOffLine uid: {0}, reason: {1}", uid, (int)reason));
        DestroyVideoView(uid);
    }

    void OnSDKWarningHandler(int warn, string msg)
    {
        logger.UpdateLog(string.Format("OnSDKWarning warn: {0}, msg: {1}", warn, msg));
    }

    void OnSDKErrorHandler(int error, string msg)
    {
        logger.UpdateLog(string.Format("OnSDKError error: {0}, msg: {1}", error, msg));
    }

    void OnConnectionLostHandler()
    {
        logger.UpdateLog(string.Format("OnConnectionLost "));
    }

    void OnDestroy()
    {
        Debug.Log("OnApplicationQuit");
        if (mRtcEngine != null)
        {
            mRtcEngine.LeaveChannel();
            mRtcEngine.DisableVideoObserver();
            IRtcEngine.Destroy();
        }
    }

    private void DestroyVideoView(uint uid)
    {
        GameObject go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            Object.Destroy(go);
        }
    }

    private void makeVideoView(uint uid)
    {
        GameObject go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            return; // reuse
        }

        // create a GameObject and assign to this new user
        VideoSurface videoSurface = makeImageSurface(uid.ToString());
        if (!ReferenceEquals(videoSurface, null))
        {
            // configure videoSurface
            videoSurface.SetForUser(uid);
            videoSurface.SetEnable(true);
            videoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
        }
    }

    private VideoSurface makeImageSurface(string goName)
    {
        GameObject go = new GameObject();

        if (go == null)
        {
            return null;
        }

        go.name = goName;
        // to be renderered onto
        go.AddComponent<RawImage>();
        // make the object draggable
        go.AddComponent<UIElementDrag>();
        GameObject canvas = GameObject.Find("VideoCanvas");
        if (canvas != null)
        {
            go.transform.parent = canvas.transform;
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
        go.transform.localScale = Vector3.one;

        // configure videoSurface
        VideoSurface videoSurface = go.AddComponent<VideoSurface>();
        return videoSurface;
    }
}
