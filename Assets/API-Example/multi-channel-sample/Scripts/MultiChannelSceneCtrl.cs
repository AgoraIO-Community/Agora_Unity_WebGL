using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;

public class MultiChannelSceneCtrl : MonoBehaviour
{
    [SerializeField]
    string appID;

    [SerializeField]
    Text logText;

    static IRtcEngine mRtcEngine;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (!CheckAppId())
        {
            return;
        }

        if (mRtcEngine == null)
        {
            mRtcEngine = IRtcEngine.GetEngine(appID);
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