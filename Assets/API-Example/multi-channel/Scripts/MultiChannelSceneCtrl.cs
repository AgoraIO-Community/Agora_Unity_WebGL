using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
#if(UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
#endif
using System.Collections;

/// <summary>
///  This demo shows MultiChannel video streaming.
///  A major different between Native(including Editor) vs WebGL:
///     You may only broadcast on one channel on Native platforms while you
///  may broadcast to multiple channels.  There is no limitation of audience.
/// </summary>
public class MultiChannelSceneCtrl : MonoBehaviour
{
    [SerializeField]
    AppInfoObject appInfo;

    [SerializeField]
    Text logText;
    // Use this for initialization
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList();
#endif
    static IRtcEngine mRtcEngine;
    static string APPID { get; set; }

    public static MultiChannelSceneCtrl Instance { get; private set; }

    private void Awake()
    {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
        permissionList.Add(Permission.Microphone);
        permissionList.Add(Permission.Camera);
# endif
        if(!RootMenuControl.instance)
            DontDestroyOnLoad(gameObject);

        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;


    }

    void Start()
    {
        if (!CheckAppId())
        {
            return;
        }
        APPID = appInfo.appID;
        SetupEngine();
    }

    private void Update()
    {
        CheckPermissions();
    }
    /// <summary>
    ///   Checks for platform dependent permissions.
    /// </summary>
    private void CheckPermissions()
    {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
        foreach (string permission in permissionList)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                Permission.RequestUserPermission(permission);
            }
        }
#endif
    }
    public void SetupEngine(bool resetting = false)
    {
        if (mRtcEngine == null || resetting)
        {
            mRtcEngine = IRtcEngine.GetEngine(APPID);
        }


        if (mRtcEngine == null)
        {
            Debug.Log("engine is null");
            return;
        }
        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);
        mRtcEngine.SetAudioProfile(AUDIO_PROFILE_TYPE.AUDIO_PROFILE_DEFAULT, AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_GAME_STREAMING);
        mRtcEngine.SetMultiChannelWant(true);
        mRtcEngine.EnableVideo();
        mRtcEngine.EnableAudio();
        mRtcEngine.EnableVideoObserver();
    }

    private void OnDestroy()
    {
        mRtcEngine = null;
        IRtcEngine.Destroy();
    }

    bool CheckAppId()
    {
        Logger logger = new Logger(logText);
        return logger.DebugAssert(appInfo.appID.Length > 10, "<color=red>[STOP] Please fill in your appId in your AppIDInfo Object!!!! \n (Assets/API-Example/_AppIDInfo/AppIDInfo)</color>");
    }
}