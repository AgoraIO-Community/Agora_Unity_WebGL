using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using agora_utilities;

/// <summary>
///   This demo shows the use of token and TokenClient prefab
/// to renew tokens.  Please make sure to fill in your token server's
/// URL address in the TokenClient's Inspector field. 
/// </summary>
public class ClientManagerTokenUse : MonoBehaviour
{
    [SerializeField] private AppInfoObject appInfo;

    [SerializeField] private string CHANNEL_NAME = "YOUR_CHANNEL_NAME_1";

    public Text logText;
    private Logger logger;
    private IRtcEngine mRtcEngine = null;
    private const float Offset = 100;

    public Button joinButton, leaveButton;
    public bool joinedChannel = false;
    public bool useToken = false;

    private List<uint> remoteClientIDs;

    public static ClientManagerTokenUse instance;

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
        logger = new Logger(logText);
        //channel setup.
        remoteClientIDs = new List<uint>();
        instance = this;

        SetupToggle();
    }

    void Update()
    {
        PermissionHelper.RequestMicrophontPermission();
        PermissionHelper.RequestCameraPermission();

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

    /// <summary>
    ///   Set up the functionality of the toggle.  The purpose of the toggle
    /// is to demonstrate OnTokenPrivilegeDidExpire will happen if no renewal is in place.
    /// </summary>
    void SetupToggle()
    {
        GameObject go = GameObject.Find("Toggle");
        if (go != null)
        {
            var toggle = go.GetComponent<Toggle>();
            if (toggle != null)
            {
                toggle.onValueChanged.AddListener((toggled) =>
                {
                    if (!toggled)
                    {
                        // throw away the handler so token will expire without renewal
                        logger.UpdateLog("OnTokenPrivilegeWillExpire will be off for now.");
                        mRtcEngine.OnTokenPrivilegeWillExpire = null;
                        toggle.interactable = false;
                    }
                });
            }
        }
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

        mRtcEngine.OnUserJoined += EngineOnUserJoinedHandler;
        mRtcEngine.OnUserOffline += EngineOnUserOfflineHandler;

        mRtcEngine.OnError += EngineOnErrorHandler;

    }

    public void JoinChannel()
    {
        ChannelMediaOptions options = new ChannelMediaOptions(true, true, true, true);

        if (!useToken)
        {
            mRtcEngine.JoinChannel(appInfo.token, CHANNEL_NAME, "", 0, options);
        }
        else
        {
            /* <--------------------- Token client usage here ------------------------> */
            TokenClient.Instance.SetRtcEngineInstance(mRtcEngine);
            TokenClient.Instance.GetRtcToken(CHANNEL_NAME, 0, (token) =>
            {
                appInfo.token = token;
                Debug.Log(gameObject.name + " Got rtc token:" + appInfo.token);
                mRtcEngine.JoinChannel(appInfo.token, CHANNEL_NAME, "", 0, options);
            });
        }
        joinedChannel = true;
    }

    public void LeaveChannel()
    {
        mRtcEngine.LeaveChannel();
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

    #region -- EVENT HANDLERS --

    void EngineOnJoinChannelSuccessHandler(string channelId, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("sdk version: {0}", IRtcEngine.GetSdkVersion()));
        logger.UpdateLog(string.Format("EngineOnJoinChannelSuccess channelId: {0}, uid: {1}, elapsed: {2}", CHANNEL_NAME, uid,
            elapsed));
        RespawnLocal(CHANNEL_NAME);
    }

    void EngineOnLeaveChannelHandler(RtcStats rtcStats)
    {
        logger.UpdateLog(string.Format("OnLeaveChannelHandler channelId: {0}", CHANNEL_NAME));
        DestroyVideoView(CHANNEL_NAME, 0);
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
        remoteClientIDs.Add(uid);
    }

    void EngineOnUserOfflineHandler(uint uid, USER_OFFLINE_REASON reason)
    {
        logger.UpdateLog(string.Format("OnUserOffLine uid: ${0}, reason: ${1}", uid, (int)reason));
        DestroyVideoView(CHANNEL_NAME, uid);
        remoteClientIDs.Remove(uid);
    }

    #endregion

    #region -- USER VIEWS --
    public void RespawnLocal(string channelName)
    {
        GameObject go = GameObject.Find(channelName + "_0");
        if (go != null)
        {
            go.name = "Destroying";
            Destroy(go);
            makeVideoView(channelName, 0);
        }
        else
        {
            makeVideoView(channelName, 0);
        }
    }


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
    #endregion
}