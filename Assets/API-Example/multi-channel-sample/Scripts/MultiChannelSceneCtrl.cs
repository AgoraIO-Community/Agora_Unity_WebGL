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

        if(mRtcEngine == null)
        {
            mRtcEngine = IRtcEngine.GetEngine(appID);
        }

        mRtcEngine.SetMultiChannelWant(true);

        if (mRtcEngine == null)
        {
            Debug.Log("engine is null");
            return;
        }

        mRtcEngine.EnableVideo();
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