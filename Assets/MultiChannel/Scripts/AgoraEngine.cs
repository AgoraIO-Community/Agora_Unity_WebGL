using UnityEngine;
using agora_gaming_rtc;

public class AgoraEngine : MonoBehaviour
{
    public string appID;
    public static IRtcEngine mRtcEngine;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
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


	[SerializeField] private string FBURL = "rtmps://live-api-s.facebook.com:443/rtmp/761881614728913?s_bl=1&s_psm=1&s_sc=761881671395574&s_sw=0&s_vt=api-s&a=AbxzSme10reUuODr";
	void TestTranscoding()
	{
		if (GUILayout.Button("Start Live Transcoding"))
		{
			const int HOSTVIEW_WIDTH = 360;
			const int HOSTVIEW_HEIGHT = 640;
			IRtcEngine engine = IRtcEngine.QueryEngine();
			LiveTranscoding live = new LiveTranscoding();

			TranscodingUser user = new TranscodingUser();
			
			user.x = 0;
			user.y = 0;
			user.width = HOSTVIEW_WIDTH;
			user.height = HOSTVIEW_HEIGHT;
			user.audioChannel = 0;
			user.alpha = 1;

			TranscodingUser me = user;
			
			me.x = me.width;

			live.transcodingUsers = new TranscodingUser[] { me, user };
			live.userCount = 2;

			live.width = 2 * HOSTVIEW_WIDTH;
			live.height = HOSTVIEW_HEIGHT;
			live.videoBitrate = 400;
			live.videoCodecProfile = VIDEO_CODEC_PROFILE_TYPE.VIDEO_CODEC_PROFILE_HIGH;
			live.videoGop = 30;
			live.videoFramerate = 24;
			live.lowLatency = false;

			live.audioSampleRate = AUDIO_SAMPLE_RATE_TYPE.AUDIO_SAMPLE_RATE_44100;
			live.audioBitrate = 48;
			live.audioChannels = 1;
			live.audioCodecProfile = AUDIO_CODEC_PROFILE_TYPE.AUDIO_CODEC_PROFILE_LC_AAC;

			LiveStreamAdvancedFeature[] liveStreamAdvancedFeatures1 = new LiveStreamAdvancedFeature[2];
			liveStreamAdvancedFeatures1[0].featureName = "User";
			liveStreamAdvancedFeatures1[1].featureName = "User1";
			live.liveStreamAdvancedFeatures = liveStreamAdvancedFeatures1;

			engine.SetLiveTranscoding(live);

			engine.OnTranscodingUpdated += HandleTranscodingCallback;
			//int rc = mRtcEngine.AddPublishStreamUrl(url: YTURL, transcodingEnabled: true);
			//Debug.Assert(rc == 0, " error in adding " + YTURL);
			int rc = engine.AddPublishStreamUrl(url: FBURL, transcodingEnabled: true);
		}

		if (GUILayout.Button("Stop Live Transcoding"))
		{
			IRtcEngine engine = IRtcEngine.QueryEngine();
			engine.RemovePublishStreamUrl(FBURL);
		}
	}

	void HandleTranscodingCallback()
	{
		Debug.Log("Transcoding Handle Call back");
	}

	public void D_EnableVideoRawDataObserver()
    {

    }

	public void D_UnRegisterVideoRawDataObserver()
	{

	}

}