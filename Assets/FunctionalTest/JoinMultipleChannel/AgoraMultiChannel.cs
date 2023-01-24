using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using agora_utilities;

public class AgoraMultiChannel : MonoBehaviour
{
    [SerializeField] private string APP_ID = "YOUR_APPID";

    [SerializeField] private string TOKEN_1 = "";

    [SerializeField] private string CHANNEL_NAME_1 = "YOUR_CHANNEL_NAME_1";

    [SerializeField] private string TOKEN_2 = "";

    [SerializeField] private string CHANNEL_NAME_2 = "YOUR_CHANNEL_NAME_2";
    public Text logText;
    private Logger logger;
    private IRtcEngine mRtcEngine = null;
    private AgoraChannel channel1 = null;
    private AgoraChannel channel2 = null;
    private const float Offset = 100;

    // Use this for initialization
    void Start()
    {
        CheckAppId();
        InitEngine();
        JoinChannel();
    }

    void Update()
    {
        PermissionHelper.RequestMicrophontPermission();
        PermissionHelper.RequestCameraPermission();
    }

    void CheckAppId()
    {
        logger = new Logger(logText);
        logger.DebugAssert(APP_ID.Length > 10, "Please fill in your appId in VideoCanvas!!!!!");
    }

    void InitEngine()
    {
        mRtcEngine = IRtcEngine.GetEngine(APP_ID);
        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);
        // If you want to user Multi Channel Video, please call "SetMultiChannleWant to true"
        mRtcEngine.SetMultiChannelWant(true);
        mRtcEngine.EnableAudio();
        mRtcEngine.EnableVideo();
        mRtcEngine.EnableVideoObserver();

        channel1 = mRtcEngine.CreateChannel(CHANNEL_NAME_1);
        channel2 = mRtcEngine.CreateChannel(CHANNEL_NAME_2);
        channel1.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        channel2.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE);

        channel1.ChannelOnJoinChannelSuccess = Channel1OnJoinChannelSuccessHandler;
        channel1.ChannelOnLeaveChannel = Channel1OnLeaveChannelHandler;
        channel1.ChannelOnUserJoined = Channel1OnUserJoinedHandler;
        channel1.ChannelOnError = Channel1OnErrorHandler;
        channel1.ChannelOnUserOffLine = ChannelOnUserOfflineHandler;
        channel2.ChannelOnJoinChannelSuccess = Channel2OnJoinChannelSuccessHandler;
        channel2.ChannelOnLeaveChannel = Channel2OnLeaveChannelHandler;
        channel2.ChannelOnUserJoined = Channel2OnUserJoinedHandler;
        channel2.ChannelOnError = Channel2OnErrorHandler;
        channel2.ChannelOnUserOffLine = ChannelOnUserOfflineHandler;
    }

    void JoinChannel()
    {
        channel1.JoinChannel(TOKEN_1, "", 0, new ChannelMediaOptions(true, true));
        channel2.JoinChannel(TOKEN_2, "", 0, new ChannelMediaOptions(true, true, false, false));
    }

    void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        if (mRtcEngine != null)
        {
            channel1.LeaveChannel();
            channel2.LeaveChannel();
            channel1.ReleaseChannel();
            channel2.ReleaseChannel();

            mRtcEngine.DisableVideoObserver();
            IRtcEngine.Destroy();
        }
    }

    void Channel1OnJoinChannelSuccessHandler(string channelId, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("sdk version: ${0}", IRtcEngine.GetSdkVersion()));
        logger.UpdateLog(string.Format("onJoinChannelSuccess channelId: {0}, uid: {1}, elapsed: {2}", channelId, uid,
            elapsed));
        makeVideoView(channelId, 0);
    }

    void Channel2OnJoinChannelSuccessHandler(string channelId, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("sdk version: ${0}", IRtcEngine.GetSdkVersion()));
        logger.UpdateLog(string.Format("onJoinChannelSuccess channelId: {0}, uid: {1}, elapsed: {2}", channelId, uid,
            elapsed));
        makeVideoView(channelId, 0);
    }

    void Channel1OnLeaveChannelHandler(string channelId, RtcStats rtcStats)
    {
        logger.UpdateLog(string.Format("Channel1OnLeaveChannelHandler channelId: {0}", channelId));

    }

    void Channel2OnLeaveChannelHandler(string channelId, RtcStats rtcStats)
    {
        logger.UpdateLog(string.Format("Channel1OnLeaveChannelHandler channelId: {0}", channelId));
    }

    void Channel1OnErrorHandler(string channelId, int err, string message)
    {
        logger.UpdateLog(string.Format("Channel1OnErrorHandler channelId: {0}, err: {1}, message: {2}", channelId, err,
            message));
    }

    void Channel2OnErrorHandler(string channelId, int err, string message)
    {
        logger.UpdateLog(string.Format("Channel2OnErrorHandler channelId: {0}, err: {1}, message: {2}", channelId, err,
            message));
    }

    void Channel1OnUserJoinedHandler(string channelId, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("Channel1OnUserJoinedHandler channelId: {0} uid: ${1} elapsed: ${2}", channelId,
            uid, elapsed));
        makeVideoView(channelId, uid);
    }

    void Channel2OnUserJoinedHandler(string channelId, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("Channel2OnUserJoinedHandler channelId: {0} uid: ${1} elapsed: ${2}", channelId,
            uid, elapsed));
        makeVideoView(channelId, uid);
    }

    void ChannelOnUserOfflineHandler(string channelId, uint uid, USER_OFFLINE_REASON reason)
    {
        logger.UpdateLog(string.Format("OnUserOffLine uid: ${0}, reason: ${1}", uid, (int)reason));
        DestroyVideoView(channelId, uid);
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
            videoSurface.SetForMultiChannelUser(channelId, uid);
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