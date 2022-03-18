using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;

/// <summary>
///  This demo shows MultiChannel video streaming.
///  A major different between Native(including Editor) vs WebGL:
///     You may only broadcast on one channel on Native platforms while you
///  may broadcast to multiple channels.  There is no limitation of audience.
/// </summary>
public class MultiChannelSceneCtrl : MonoBehaviour
{
    [SerializeField]
    string appID;

    [SerializeField]
    Text logText;

    static IRtcEngine mRtcEngine;
    static string APPID { get; set; }

    public static MultiChannelSceneCtrl Instance { get; private set; }

    private void Awake()
    {
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
        APPID = appID;
        SetupEngine();
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
        mRtcEngine.SetMultiChannelWant(true);
        mRtcEngine.EnableVideo();
        mRtcEngine.EnableAudio();
        mRtcEngine.EnableVideoObserver();
    }

    private void OnApplicationQuit()
    {
        mRtcEngine = null;
        IRtcEngine.Destroy();
    }

    bool CheckAppId()
    {
        Logger logger = new Logger(logText);
        return logger.DebugAssert(appID.Length > 10, "<color=red>[STOP] Please fill in your appId in Canvas!!!!</color>");
    }
}