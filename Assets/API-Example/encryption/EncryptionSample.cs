using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using agora_utilities;

public class EncryptionSample : MonoBehaviour
{
    [Header("Agora Inputs")]
    [SerializeField]
    private AppInfoObject appInfo;

    [SerializeField]
    private string CHANNEL_NAME = "YOUR_CHANNEL_NAME";

    public Text logText;
    private Logger logger;
    private IRtcEngine mRtcEngine = null;

    [Header("UI Components")]
    [SerializeField]
    Button ApplyButton;
    [SerializeField]
    Button JoinButton;
    [SerializeField]
    Button LeaveButton;


    [SerializeField]
    InputField EncryptionKeyField;
    [SerializeField]
    InputField SaltField;
    [SerializeField]
    Dropdown ModeDropdown;

    [Header("Encryption Settings")]
    [SerializeField]
    [Tooltip("      The secret must contain at least 1 lowercase alphabetical character,\n      The secret must contain at least 1 uppercase alphabetical character,\n      The secret must contain at least 1 numeric character,\n      The secret must contain at least one special character,\n      The secret must be eight characters or longer.")]
    /*
      encrytionKey is a.k.a "secret", rules:
      The secret must contain at least 1 lowercase alphabetical character,
      The secret must contain at least 1 uppercase alphabetical character,
      The secret must contain at least 1 numeric character,
      The secret must contain at least one special character,
      The secret must be eight characters or longer.
    */
    string encryptionKey;

    [SerializeField]
    // Generate this by: openssl rand -hex 32
    string encryptionSaltBase64;

    [SerializeField]
    ENCRYPTION_MODE encryptionMode = ENCRYPTION_MODE.AES_128_ECB;

    bool _joinedChannel = false;

    /// <summary>
    ///   Saving a list of displaying video views
    /// </summary>
    Dictionary<string, GameObject> _views = new Dictionary<string, GameObject>();

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
        CheckAppId();
        InitUI();
        InitEngine();
    }

    // Update is called once per frame
    void Update()
    {
        PermissionHelper.RequestMicrophontPermission();
        PermissionHelper.RequestCameraPermission();
    }

    void CheckAppId()
    {
        logger = new Logger(logText);
        logger.DebugAssert(appInfo.appID.Length > 10, "<color=red>[STOP] Please fill in your appId in your AppIDInfo Object!!!! \n (Assets/API-Example/_AppIDInfo/AppIDInfo)</color>");
        logger.DebugAssert(appInfo.token.Length > 10, "<color=red>[STOP] Please provide a token in your AppIDInfo Object!!!! \n (Assets/API-Example/_AppIDInfo/AppIDInfo)</color>");
    }

    byte[] GetEncryptionSaltFromServer()
    {
        return System.Convert.FromBase64String(encryptionSaltBase64);
    }


    void EnableEncryption()
    {
        if (IsSaltRequired(encryptionMode))
        {
            if (!logger.DebugAssert(SaltField.text.Length >= 32, $" Encryption mode {encryptionMode} requires salt!"))
            {
                return;
            }
        }

        if (mRtcEngine != null)
        {
            encryptionKey = EncryptionKeyField.text;
            // encryptionMode is either set up at start or changed by value on dropdown event

            // Create an encryption configuration.
            var config = new EncryptionConfig
            {
                // Specify a encyption mode
                encryptionMode = encryptionMode,
                // Assign a secret key.
                encryptionKey = encryptionKey,
            };

            if (IsSaltRequired(encryptionMode))
            {
                encryptionSaltBase64 = SaltField.text;
                config.encryptionKdfSalt = GetEncryptionSaltFromServer();
            }
            // Enable the built-in encryption.
            int rc = mRtcEngine.EnableEncryption(true, config);

            string mySalt = string.Join(",", config.encryptionKdfSalt);
            Debug.Log($"Enable Encryption, key:{encryptionKey} my mode is {encryptionMode}" + " My EncryptSalt is [" + mySalt + "]");
            if (rc == 0)
            {
                logger.UpdateLog("<color=green>Encryption enabled.</color>");
            }
        }
    }

    void InitUI()
    {
        // text fields
        if (!string.IsNullOrEmpty(encryptionKey))
        {
            EncryptionKeyField.text = encryptionKey;
        }
        SaltField.enabled = IsSaltRequired(encryptionMode);
        if (SaltField.enabled && !string.IsNullOrEmpty(encryptionSaltBase64))
        {
            SaltField.text = encryptionSaltBase64;
        }

        // buttons
        JoinButton.onClick.AddListener(JoinChannel);
        LeaveButton.onClick.AddListener(LeaveChannel);
        List<string> modeNames = new List<string>();
        for (ENCRYPTION_MODE mode = ENCRYPTION_MODE.AES_128_XTS; mode < ENCRYPTION_MODE.MODE_END; mode++)
        {
            modeNames.Add(mode.ToString());
        }
        ApplyButton.onClick.AddListener(EnableEncryption);

        // dropdown
        ModeDropdown.ClearOptions();
        ModeDropdown.AddOptions(modeNames);
        ModeDropdown.value = (int)encryptionMode;
        ModeDropdown.onValueChanged.AddListener((v) =>
        {
            encryptionMode = (ENCRYPTION_MODE)(1 + v); // v is 0 based, while the mode is 1 based
            SaltField.enabled = IsSaltRequired(encryptionMode);
        });
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

        logger.UpdateLog(string.Format("sdk version: ${0}", IRtcEngine.GetSdkVersion()));
    }

    void JoinChannel()
    {
        mRtcEngine.JoinChannelByKey(appInfo.token, CHANNEL_NAME, "", 0);
    }

    void LeaveChannel()
    {
        mRtcEngine.LeaveChannel();
    }

    void UpdateUI()
    {
        JoinButton.interactable = !_joinedChannel;
        LeaveButton.interactable = _joinedChannel;
        ApplyButton.interactable = !_joinedChannel;
    }

    bool IsSaltRequired(ENCRYPTION_MODE mode)
    {
        return mode == ENCRYPTION_MODE.AES_128_GCM2 || mode == ENCRYPTION_MODE.AES_256_GCM2;
    }

    #region agora event handlers
    void OnJoinChannelSuccessHandler(string channelName, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("onJoinChannelSuccess channelName: {0}, uid: {1}, elapsed: {2}", channelName, uid, elapsed));
        makeVideoView(0);
        _joinedChannel = true;
        UpdateUI();
    }

    void OnLeaveChannelHandler(RtcStats stats)
    {
        logger.UpdateLog("OnLeaveChannelSuccess");
        DestroyAllViews();
        _joinedChannel = false;
        UpdateUI();
    }

    void OnUserJoinedHandler(uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("OnUserJoined uid: ${0} elapsed: ${1}", uid, elapsed));
        makeVideoView(uid);
    }

    void OnUserOfflineHandler(uint uid, USER_OFFLINE_REASON reason)
    {
        logger.UpdateLog(string.Format("OnUserOffLine uid: ${0}, reason: ${1}", uid, (int)reason));
        DestroyVideoView(uid);
    }

    void OnSDKWarningHandler(int warn, string msg)
    {
        logger.UpdateLog(string.Format("OnSDKWarning warn: {0}, msg: {1}", warn, msg));
    }

    void OnSDKErrorHandler(int error, string msg)
    {
        msg = IRtcEngine.GetErrorDescription(error);
        logger.UpdateLog(string.Format("OnSDKError error: {0}, msg: {1}", error, msg));
    }

    void OnConnectionLostHandler()
    {
        logger.UpdateLog(string.Format("OnConnectionLost "));
    }

    #endregion

    void OnDestroy()
    {
        if (mRtcEngine != null)
        {
            mRtcEngine.LeaveChannel();
            mRtcEngine.DisableVideoObserver();
            IRtcEngine.Destroy();
        }
    }

    #region -- Video view management --
    private void DestroyAllViews()
    {
        foreach (var viewGO in _views.Values)
        {
            Object.Destroy(viewGO);
        }
        _views.Clear();
    }

    private void DestroyVideoView(uint uid)
    {
        GameObject go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            Object.Destroy(go);
            _views.Remove(uid.ToString());
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
            _views[uid.ToString()] = videoSurface.gameObject;
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

    #endregion
}