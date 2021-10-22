using System.Collections;
using UnityEngine;
using agora_gaming_rtc;
using UnityEngine.UI;
using agora_utilities;

public class AgoraScreenShare : MonoBehaviour 
{

    [SerializeField]
    private string APP_ID = "YOUR_APPID";

    [SerializeField]
    private string TOKEN = "";

    [SerializeField]
    private string CHANNEL_NAME = "YOUR_CHANNEL_NAME";

    [SerializeField]
    Toggle ScreenToggle;

   	public Text logText;
    private Logger logger;
	public IRtcEngine mRtcEngine = null;
	private const float Offset = 100;
	private Texture2D mTexture;
    private Rect mRect;	
    public RawImage rawImage;
    public RawImage textureImage;
	public Vector2 cameraSize = new Vector2(640, 480);
	public int cameraFPS = 15;
    bool running = false;
    int timestamp = 0;
    bool _sharingImage = false;

    // Use this for initialization
    void Start () 
	{
        bool appReady = CheckAppId();
        if (!appReady) return;

		InitEngine();
        InitTexture();

#if !UNITY_WEBGL || UNITY_EDITOR
        // native order
        logger.UpdateLog("<color=lime>ScreenShare starts automatically...</color>");
        EnableShareScreen();
        JoinChannel();
        StartSharing();
#else
        // webgl order (join first, EnableShareScreen second)
        JoinChannel();
        logger.UpdateLog("<color=lime>Press Enable and Then Start Sharing...</color>");
        // will call startSharing from UI
#endif
    }

    #region -- UI Respondant --
    public void StartSharing()
    {
        if (running == false)
        {
            running = true;
            HandleToggleValueChange(!_sharingImage);
            StartCoroutine(shareScreen());
        }
    }

    public void EnableShareScreen()
    {
        // Very Important to make this app work
        mRtcEngine.SetExternalVideoSource(true, false);
        Debug.Log("ScreenShare Enabled");
    }

    public void DisableShareScreen()
    {
        StopSharing();
        Debug.Log("ScreenShare Deactivated");
    }

    public void WebShareScreen()
    {
        Debug.Log("ScreenShare Web API Call");
        mRtcEngine.StartScreenCaptureForWeb();
    }

        
    // On Native only, after stopping screenshare, you need to choose the
    // external source mode again before joining the channel.
    // On Web, it is preferred to enable external source while in the channel
    public void Restart()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        Debug.Log("Leaving Channel....");
        mRtcEngine.LeaveChannel();
        EnableShareScreen();
        Debug.Log("Joining Channel....");
        JoinChannel();
#else
        logger.UpdateLog("Don't need to leave, just Enable/Start/Stop in sequence again.");
#endif
    }

    void HandleToggleValueChange(bool isScreenOn)
    {
        if (isScreenOn)
        {
            mRect = new Rect(0, 0, Screen.width, Screen.height);
            mTexture = new Texture2D((int)mRect.width, (int)mRect.height, TextureFormat.RGBA32, false);
        }
        else
        {
            mRect = new Rect(0, 0, textureImage.texture.width, textureImage.texture.height);
            mTexture = textureImage.texture as Texture2D;
        }

        _sharingImage = !isScreenOn;


        VideoEncoderConfiguration config = new VideoEncoderConfiguration
        {
            dimensions = new VideoDimensions() { width = (int)mRect.width, height = (int)mRect.height },
            frameRate = FRAME_RATE.FRAME_RATE_FPS_15,
            minFrameRate = -1,
            bitrate = 0,
            minBitrate = 1,
            orientationMode = ORIENTATION_MODE.ORIENTATION_MODE_ADAPTIVE,
            degradationPreference = DEGRADATION_PREFERENCE.MAINTAIN_FRAMERATE,
            mirrorMode = VIDEO_MIRROR_MODE_TYPE.VIDEO_MIRROR_MODE_DISABLED 
                // note: mirrorMode is not effective for WebGL
        };
        mRtcEngine.SetVideoEncoderConfiguration(config);


        Debug.Log("texture size = " + mRect.width + " x " + "mRect.height");
    }
    #endregion

    IEnumerator shareScreen()
    {
        while (running)
        {
            yield return new WaitForEndOfFrame();
            //Read the Pixels inside the Rectangle
            if (!_sharingImage)
            {

                mTexture.ReadPixels(mRect, 0, 0);
                //Apply the Pixels read from the rectangle to the texture
                mTexture.Apply();
            }

            // Get the Raw Texture data from the the from the texture and apply it to an array of bytes
            byte[] bytes = mTexture.GetRawTextureData();
            // int size = Marshal.SizeOf(bytes[0]) * bytes.Length;
            // Check to see if there is an engine instance already created
            //if the engine is present
            if (mRtcEngine != null)
            {
                //Create a new external video frame
                ExternalVideoFrame externalVideoFrame = new ExternalVideoFrame();
                //Set the buffer type of the video frame
                externalVideoFrame.type = ExternalVideoFrame.VIDEO_BUFFER_TYPE.VIDEO_BUFFER_RAW_DATA;
                // Set the video pixel format
                //externalVideoFrame.format = ExternalVideoFrame.VIDEO_PIXEL_FORMAT.VIDEO_PIXEL_BGRA;  // V.2.9.x
                externalVideoFrame.format = ExternalVideoFrame.VIDEO_PIXEL_FORMAT.VIDEO_PIXEL_RGBA;  // V.3.x.x
                //apply raw data you are pulling from the rectangle you created earlier to the video frame
                externalVideoFrame.buffer = bytes;
                //Set the width of the video frame (in pixels)
                externalVideoFrame.stride = (int)mRect.width;
                //Set the height of the video frame
                externalVideoFrame.height = (int)mRect.height;
                //Remove pixels from the sides of the frame
                //externalVideoFrame.cropLeft = 10;
                //externalVideoFrame.cropTop = 10;
                //externalVideoFrame.cropRight = 10;
                //externalVideoFrame.cropBottom = 10;
                //Rotate the video frame (0, 90, 180, or 270)
                externalVideoFrame.rotation = 180;
                externalVideoFrame.timestamp = timestamp++;
                //Push the external video frame with the frame we just created
                mRtcEngine.PushVideoFrame(externalVideoFrame);
                if (timestamp % 100 == 0)
                {
                    Debug.Log("Pushed frame = " + timestamp);
                }

            }
        }
    }

    void StopSharing()
    {
        // set the boolean false will cause the shareScreen coRoutine to exit
        running = false;
        mRtcEngine.SetExternalVideoSource(false, false);
    }

    void InitEngine()
	{
        mRtcEngine = IRtcEngine.GetEngine(APP_ID);
		mRtcEngine.SetLogFile("log.txt");
		mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);
		mRtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
		mRtcEngine.EnableAudio();
		mRtcEngine.EnableVideo();
		mRtcEngine.EnableVideoObserver();

        // Web: calling this before joining creates publish error
        // as we publish new created canvas source
		//mRtcEngine.SetExternalVideoSource(true, false);

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
        int ret = mRtcEngine.JoinChannelByKey(TOKEN, CHANNEL_NAME, "", 0);
        // int ret = mRtcEngine.JoinChannel(CHANNEL_NAME, "", 0);
        Debug.Log(string.Format("JoinChannel ret: ${0}", ret));
	}

	bool CheckAppId()
    {
        logger = new Logger(logText);
        return logger.DebugAssert(APP_ID.Length > 10, "<color=red>[STOP] Please fill in your appId in Canvas!!!!</color>");
    }

    void InitTexture()
    {
        ScreenToggle.onValueChanged.AddListener(HandleToggleValueChange);
        textureImage = GameObject.Find("LogoImage").GetComponent<RawImage>();
    }

    #region -- event handlers --
    void OnJoinChannelSuccessHandler(string channelName, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("sdk version: ${0}", IRtcEngine.GetSdkVersion()));
        logger.UpdateLog(string.Format("onJoinChannelSuccess channelName: {0}, uid: {1}, elapsed: {2}", channelName, uid, elapsed));
    }

    void OnLeaveChannelHandler(RtcStats stats)
    {
        logger.UpdateLog("OnLeaveChannelSuccess");
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
        logger.UpdateLog(string.Format("OnSDKError error: {0}, msg: {1}", error, msg));
    }
    
    void OnConnectionLostHandler()
    {
        logger.UpdateLog(string.Format("OnConnectionLost "));
    }

    void OnApplicationQuit()
    {
        if (mRtcEngine != null)
        {
			mRtcEngine.LeaveChannel();
			mRtcEngine.DisableVideoObserver();
            IRtcEngine.Destroy();
            mRtcEngine = null;
        }
    }

    #endregion

    #region -- remote user view handling ---
    private void DestroyVideoView(uint uid)
    {
        GameObject go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            Destroy(go);
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
            logger.UpdateLog("make view view for " + uid);
            // configure videoSurface
            videoSurface.SetForUser(uid);
            videoSurface.SetEnable(true);
            videoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
        }
    }

    public VideoSurface makeImageSurface(string goName)
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
        GameObject canvas = GameObject.Find("Canvas");
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
        float xPos = Random.Range(Offset - Screen.width / 2f, Screen.width / 2f - Offset);
        float yPos = Random.Range(Offset, Screen.height / 2f - Offset);
        Debug.Log("position x " + xPos + " y: " + yPos);
        go.transform.localPosition = new Vector3(xPos, yPos, 0f);
        //go.transform.localPosition = new Vector3(10, 10, 0f);
        go.transform.localScale = new Vector3(3 * 1.6666f, 3f, 1f);

        // configure videoSurface
        VideoSurface videoSurface = go.AddComponent<VideoSurface>();
        return videoSurface;
    }
    #endregion
}
