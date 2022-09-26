using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using agora_utilities;
using agora_gaming_rtc;

public class VirtualBackgroundMultichannelDemo : MonoBehaviour
{
    [SerializeField] private string APP_ID = "YOUR_APPID";

    [SerializeField] private string TOKEN_1 = "";

    [SerializeField] private string CHANNEL_NAME_1 = "YOUR_CHANNEL_NAME_1";

    [SerializeField] private string TOKEN_2 = "";
    public Text logText;
    private Logger logger;
    private IRtcEngine mRtcEngine = null;
    private const float Offset = 100;

    public Button joinButton, leaveButton, blurButton, colorButton, imageButton, videoButton, enableButton, disableButton;
    public bool joinedChannel = false;
    public bool virtualBackgroundOn = false;
    public bool useToken = false;
    public bool mute, loop;
    public VirtualBackgroundSource myVirtualBackground;
    public BACKGROUND_SOURCE_TYPE background = BACKGROUND_SOURCE_TYPE.BACKGROUND_BLUR;
    public BACKGROUND_BLUR_DEGREE blur;

    private List<uint> remoteClientIDs;

    public int blurDegrees = 2;
    public int hexIndex = 0;
    public string[] hexColors = { "#FF1111", "#11FF11", "#1111FF" };
    public Dropdown hexDropdown, blurDropdown;
    public Toggle muteToggle, loopToggle;
    public string imgFile = "bedroom.jpg";
    public string videoFile = "outside.mp4";

    public AgoraChannel channel;

    // Use this for initialization
    void Start()
    {
        if (!CheckAppId())
        {
            return;
        }

        InitEngine();

        //channel setup.
        remoteClientIDs = new List<uint>();
        myVirtualBackground = new VirtualBackgroundSource();
        myVirtualBackground.background_source_type = background;
        myVirtualBackground.blur_degree = (BACKGROUND_BLUR_DEGREE)blurDegrees;
        blurDropdown.value = blurDegrees;
        uint colorValue = (uint)int.Parse(hexColors[hexIndex], System.Globalization.NumberStyles.HexNumber);
        myVirtualBackground.color = colorValue;
        blurDropdown.onValueChanged.AddListener(delegate { updateBlur(); });
        hexDropdown.onValueChanged.AddListener(delegate { updateHex(); });
        myVirtualBackground.source = imgFile;
        Debug.Log("Background Source C#....." + myVirtualBackground.background_source_type.ToString());
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

        if (virtualBackgroundOn)
        {
            enableButton.interactable = false;
            disableButton.interactable = true;
        }
        else
        {
            enableButton.interactable = true;
            disableButton.interactable = false;
        }

        mute = muteToggle.isOn;
        loop = loopToggle.isOn;

        if (background == BACKGROUND_SOURCE_TYPE.BACKGROUND_BLUR)
        {
            blurButton.interactable = false;
            colorButton.interactable = true;
            imageButton.interactable = true;
            videoButton.interactable = true;
        }
        else if (background == BACKGROUND_SOURCE_TYPE.BACKGROUND_COLOR)
        {
            blurButton.interactable = true;
            colorButton.interactable = false;
            imageButton.interactable = true;
            videoButton.interactable = true;
        }
        else if (background == BACKGROUND_SOURCE_TYPE.BACKGROUND_IMG)
        {
            blurButton.interactable = true;
            colorButton.interactable = true;
            imageButton.interactable = false;
            videoButton.interactable = true;
        }
        else if (background == BACKGROUND_SOURCE_TYPE.BACKGROUND_VIDEO)
        {
            blurButton.interactable = true;
            colorButton.interactable = true;
            imageButton.interactable = true;
            videoButton.interactable = false;
        }
    }

    bool CheckAppId()
    {
        logger = new Logger(logText);
        logger.DebugAssert(APP_ID.Length > 10, "Please fill in your appId in VideoCanvas!!!!!");
        return (APP_ID.Length > 10);
    }

    void InitEngine()
    {
        mRtcEngine = IRtcEngine.GetEngine(APP_ID);
        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);

        mRtcEngine.EnableAudio();
        mRtcEngine.EnableVideo();
        mRtcEngine.EnableVideoObserver();
        mRtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        mRtcEngine.SetMultiChannelWant(true);

        channel = mRtcEngine.CreateChannel(CHANNEL_NAME_1);
        channel.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);

        channel.ChannelOnJoinChannelSuccess = EngineOnJoinChannelSuccessHandler;
        channel.ChannelOnLeaveChannel = EngineOnLeaveChannelHandler;

        channel.ChannelOnError += EngineOnErrorHandler;

    }

    public void updateHex()
    {
        hexIndex = hexDropdown.value;
        setVirtualBackgroundColor();
    }

    public void updateBlur()
    {
        blur = (BACKGROUND_BLUR_DEGREE)blurDropdown.value + 1;
        setVirtualBackgroundBlur();
    }

    public void enableVirtualBackground(bool onoff)
    {
        if (onoff)
        {
            myVirtualBackground.background_source_type = background;
        }
        virtualBackgroundOn = onoff;
        channel.enableVirtualBackground(virtualBackgroundOn, myVirtualBackground);
    }

    public void setVirtualBackgroundBlur()
    {
        background = BACKGROUND_SOURCE_TYPE.BACKGROUND_BLUR;
        myVirtualBackground.background_source_type = background;
        myVirtualBackground.blur_degree = blur;
        channel.enableVirtualBackground(virtualBackgroundOn, myVirtualBackground);
    }

    public void setVirtualBackgroundColor()
    {
        background = BACKGROUND_SOURCE_TYPE.BACKGROUND_COLOR;
        myVirtualBackground.background_source_type = background;
        uint colorValue = (uint)int.Parse(hexColors[hexIndex], System.Globalization.NumberStyles.HexNumber);
        myVirtualBackground.color = colorValue;
        channel.enableVirtualBackground(virtualBackgroundOn, myVirtualBackground);
    }

    public void setVirtualBackgroundImage()
    {
        background = BACKGROUND_SOURCE_TYPE.BACKGROUND_IMG;
        myVirtualBackground.background_source_type = background;
        myVirtualBackground.source = imgFile;
        channel.enableVirtualBackground(virtualBackgroundOn, myVirtualBackground);
    }

    public void setVirtualBackgroundVideo()
    {
        background = BACKGROUND_SOURCE_TYPE.BACKGROUND_VIDEO;
        myVirtualBackground.background_source_type = background;
        myVirtualBackground.source = videoFile;
        myVirtualBackground.mute = mute;
        myVirtualBackground.loop = loop;
        channel.enableVirtualBackground(virtualBackgroundOn, myVirtualBackground);
    }

    public void JoinChannel()
    {
        if (!useToken)
        {
            channel.JoinChannel(TOKEN_1, "", 0, new ChannelMediaOptions(true, true, true, true));
        }
        else
        {
            TokenClient.Instance.RtcEngine = mRtcEngine;
            TokenClient.Instance.GetRtcToken(CHANNEL_NAME_1, 0, (token) =>
            {
                TOKEN_1 = token;
                Debug.Log(gameObject.name + " Got rtc token:" + TOKEN_1);
                channel.JoinChannel(TOKEN_1, "", 0, new ChannelMediaOptions(true, true, true, true));
            });
        }
        joinedChannel = true;
    }

    public void LeaveChannel()
    {
        channel.LeaveChannel();
        joinedChannel = false;
    }

    void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        if (mRtcEngine != null)
        {

            mRtcEngine.DisableVideoObserver();
            IRtcEngine.Destroy();
        }
    }

    void EngineOnJoinChannelSuccessHandler(string channelId, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("sdk version: ${0}", IRtcEngine.GetSdkVersion()));
        logger.UpdateLog(string.Format("ChannelOnJoinChannelSuccess channelId: {0}, uid: {1}, elapsed: {2}", CHANNEL_NAME_1, uid,
            elapsed));
        makeVideoView(channelId, 0);
    }

    void EngineOnLeaveChannelHandler(string channelId, RtcStats rtcStats)
    {
        logger.UpdateLog(string.Format("OnLeaveChannelHandler channelId: {0}, rtcStats: {1}", channelId, rtcStats));
    }

    void EngineOnErrorHandler(string channelId, int err, string message)
    {
        logger.UpdateLog(string.Format("UserErrorHandler err: {0}, message: {1}, channel: {2}", err,
            message, channelId));
    }

    void EngineOnUserJoinedHandler(string channelId, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("OnUserJoinedHandler channelId: {0} uid: ${1} elapsed: ${2}", channelId,
            uid, elapsed));
        makeVideoView(CHANNEL_NAME_1, uid);
        remoteClientIDs.Add(uid);
    }

    void EngineOnUserOfflineHandler(uint uid, USER_OFFLINE_REASON reason)
    {
        logger.UpdateLog(string.Format("OnUserOffLine uid: ${0}, reason: ${1}", uid, (int)reason));
        DestroyVideoView(CHANNEL_NAME_1, uid);
        remoteClientIDs.Remove(uid);
    }

    public void RespawnLocal(string channelName)
    {
        GameObject go = GameObject.Find(channelName + "_0");
        if (go != null)
        {
            go.name = "Destroying";
            Destroy(go);
            makeVideoView(channelName, 0);
        }
    }

    public void RespawnRemote()
    {
        if (LastRemote != null)
        {
            string[] strs = LastRemote.name.Split('_');
            string channel = strs[0];
            uint uid = uint.Parse(strs[1]);
            LastRemote.name = "_Destroyer";
            Destroy(LastRemote);
            Debug.LogWarningFormat("Remaking video surface for  uid:{0} channel:{1}", uid, channel);
            makeVideoView(channel, uid);
        }
    }

    GameObject LastRemote = null;

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

            if (uid != 0)
            {
                LastRemote = videoSurface.gameObject;
            }
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
        float xPos = Random.Range(Offset - Screen.width / 2f, Screen.width / 2f - Offset);
        float yPos = Random.Range(Offset, Screen.height / 2f - Offset);
        Debug.Log("position x " + xPos + " y: " + yPos);
        go.transform.localPosition = new Vector3(xPos, yPos, 0f);
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
}