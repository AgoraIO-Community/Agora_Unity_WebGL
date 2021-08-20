using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;

public class WebScreenShare : MonoBehaviour
{


    [SerializeField]
    private string APP_ID = "YOUR_APPID";

    [SerializeField]
    private string TOKEN = "";

    [SerializeField]
    private string CHANNEL_NAME = "YOUR_CHANNEL_NAME";

    [SerializeField]
    private Button button;

    public Text logText;
    private Logger logger;
    private IRtcEngine mRtcEngine;
    bool _isSharing = false;

    // Use this for initialization
    void Start()
    {
        bool appReady = CheckAppId();
        if (!appReady) return;

        InitEngine();
        SetupUI();
        JoinChannel();
    }

    bool CheckAppId()
    {
        logger = new Logger(logText);
        return logger.DebugAssert(APP_ID.Length > 10, "<color=red>[STOP] Please fill in your appId in Canvas!!!!</color>");
    }

    void InitEngine()
    {
        mRtcEngine = IRtcEngine.GetEngine(APP_ID);
        mRtcEngine.SetLogFile("log.txt");
        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);
        mRtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        mRtcEngine.EnableVideo();
        mRtcEngine.EnableVideoObserver();
        mRtcEngine.OnJoinChannelSuccess += OnJoinChannelSuccessHandler;
        mRtcEngine.OnLeaveChannel += OnLeaveChannelHandler;
        mRtcEngine.OnWarning += OnSDKWarningHandler;
        mRtcEngine.OnError += OnSDKErrorHandler;
        mRtcEngine.OnConnectionLost += OnConnectionLostHandler;
        mRtcEngine.OnUserJoined += OnUserJoinedHandler;
        mRtcEngine.OnUserOffline += OnUserOfflineHandler;
    }

    void JoinChannel()
    {
        mRtcEngine.JoinChannelByKey(TOKEN, CHANNEL_NAME, "", 0);
    }


    #region -- UI ---
    void SetupUI()
    {
        GameObject go = GameObject.Find("MyView");
        if (go != null)
        {
            VideoSurface videoSurface = go.AddComponent<VideoSurface>();
            videoSurface.SetEnable(true);
            videoSurface.SetForUser(0);
        }

        button.onClick.AddListener(ToggleScreenShare);
    }

    public void ToggleScreenShare()
    {
        _isSharing = !_isSharing;
        button.GetComponentInChildren<Text>().text = _isSharing ? "Stop Sharing" : "Share Screen";
        if (_isSharing)
        {
            logger.UpdateLog("Start screen capture for web...");
            mRtcEngine.StartScreenCaptureForWeb();
        }
        else
        {
            logger.UpdateLog("Stop screen capture");
            mRtcEngine.StopScreenCapture();
        }
    }

    #endregion

    void OnJoinChannelSuccessHandler(string channelName, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("sdk version: ${0}", IRtcEngine.GetSdkVersion()));
        logger.UpdateLog(string.Format("onJoinChannelSuccess channelName: {0}, uid: {1}, elapsed: {2}", channelName,
            uid, elapsed));
    }

    void OnLeaveChannelHandler(RtcStats stats)
    {
        logger.UpdateLog("OnLeaveChannelSuccess");
    }

    void OnUserJoinedHandler(uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("OnUserJoined uid: ${0} elapsed: ${1}", uid, elapsed));
    }

    void OnUserOfflineHandler(uint uid, USER_OFFLINE_REASON reason)
    {
        logger.UpdateLog(string.Format("OnUserOffLine uid: ${0}, reason: ${1}", uid, (int)reason));
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
        logger.UpdateLog("OnConnectionLost ");
    }


    void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        if (mRtcEngine != null)
        {
            mRtcEngine.LeaveChannel();
            mRtcEngine.DisableVideoObserver();
            IRtcEngine.Destroy();
        }

    }
}
