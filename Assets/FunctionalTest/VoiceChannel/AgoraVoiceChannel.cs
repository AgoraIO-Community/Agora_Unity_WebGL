using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using agora_utilities;

public class AgoraVoiceChannel : MonoBehaviour
{
    [SerializeField] private AppInfoObject appInfo;

    [SerializeField] private string CHANNEL_NAME_1 = "YOUR_CHANNEL_NAME_1";
    public Text logText;
    private Logger logger;
    private IRtcEngine mRtcEngine = null;
    private AgoraChannel channel1 = null;
    private const float Offset = 100;


    public Button[] joinChannelButtons, muteButtons;

    protected Dictionary<uint, VideoSurface> UserVideoDict = new Dictionary<uint, VideoSurface>();
    private List<GameObject> remoteUserDisplays = new List<GameObject>();

    private void Awake()
    {
        if (RootMenuControl.instance)
        {
            CHANNEL_NAME_1 = RootMenuControl.instance.channel;
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

    }

    void Update()
    {
        PermissionHelper.RequestMicrophontPermission();
        //PermissionHelper.RequestCameraPermission();

    }

    bool CheckAppId()
    {
        logger = new Logger(logText);
        logger.DebugAssert(appInfo.appID.Length > 10, "Please fill in your appId in VideoCanvas!!!!!");
        return (appInfo.appID.Length > 10);
    }

    void InitEngine()
    {
        mRtcEngine = IRtcEngine.GetEngine(appInfo.appID);
        mRtcEngine.SetMultiChannelWant(true);
        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);
        mRtcEngine.EnableAudio();
        mRtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);

        //channel1 = mRtcEngine.CreateChannel(CHANNEL_NAME_1);
        //channel1.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);

        //channel1.ChannelOnJoinChannelSuccess = Channel1OnJoinChannelSuccessHandler;
        //channel1.ChannelOnLeaveChannel = Channel1OnLeaveChannelHandler;
        //channel1.ChannelOnUserJoined = Channel1OnUserJoinedHandler;
        //channel1.ChannelOnError = Channel1OnErrorHandler;
        //channel1.ChannelOnUserOffLine = ChannelOnUserOfflineHandler;
        //channel1.ChannelOnVideoSizeChanged = onVideoSizeChanged_MCHandler;

    }

    public void JoinChannel1()
    {
        mRtcEngine.JoinChannel(appInfo.token, CHANNEL_NAME_1, "", 0, new ChannelMediaOptions(true, false, true, false));
        //channel1.JoinChannel(appInfo.token, "", 0, new ChannelMediaOptions(true, false, true, false));
        if (joinChannelButtons.Length > 0 && joinChannelButtons[0])
        {
            joinChannelButtons[0].interactable = false;
        }
    }

    public void LeaveChannel1()
    {
        mRtcEngine.LeaveChannel();
        //channel1.LeaveChannel();
        if (joinChannelButtons.Length > 0 && joinChannelButtons[0])
        {
            joinChannelButtons[0].interactable = true;
        }
    }

    void OnDestoy()
    {
        Debug.Log("OnApplicationQuit");
        if (mRtcEngine != null)
        {
            channel1.LeaveChannel();
            channel1.ReleaseChannel();

            mRtcEngine.DisableVideoObserver();
            IRtcEngine.Destroy();
        }
    }

    float EnforcingViewLength = 180f;
    void onVideoSizeChanged_MCHandler(string channelID, uint uid, int width, int height, int rotation)
    {

        logger.UpdateLog(string.Format("channelOnVideoSizeChanged channelID: {3}, uid: {0}, width: {1}, height: {2}", uid,
            width, height, channelID));
        if (UserVideoDict.ContainsKey(uid))
        {
            GameObject go = UserVideoDict[uid].gameObject;
            Vector2 v2 = new Vector2(width, height);
            RawImage image = go.GetComponent<RawImage>();
            v2 = AgoraUIUtils.GetScaledDimension(width, height, EnforcingViewLength);

            if (rotation == 90 || rotation == 270)
            {
                v2 = new Vector2(v2.y, v2.x);
            }
            image.rectTransform.sizeDelta = v2;
        }
    }

    void Channel1OnJoinChannelSuccessHandler(string channelId, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("sdk version: ${0}", IRtcEngine.GetSdkVersion()));
        logger.UpdateLog(string.Format("onJoinChannelSuccess channelId: {0}, uid: {1}, elapsed: {2}", channelId, uid,
            elapsed));
    }

    void EngineOnJoinChannelSuccessHandler(string channelId, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("sdk version: ${0}", IRtcEngine.GetSdkVersion()));
        logger.UpdateLog(string.Format("EngineOnJoinChannelSuccess channelId: {0}, uid: {1}, elapsed: {2}", CHANNEL_NAME_1, uid,
            elapsed));
    }

    void Channel1OnLeaveChannelHandler(string channelId, RtcStats rtcStats)
    {
        logger.UpdateLog(string.Format("Channel1OnLeaveChannelHandler channelId: {0}", channelId));

    }

    void Channel2OnLeaveChannelHandler(string channelId, RtcStats rtcStats)
    {
        logger.UpdateLog(string.Format("Channel2OnLeaveChannelHandler channelId: {0}", channelId));
    }

    void EngineOnLeaveChannelHandler(RtcStats rtcStats)
    {
        logger.UpdateLog(string.Format("EngineOnLeaveChannelHandler channelId: {0}", CHANNEL_NAME_1));
    }

    void Channel1OnErrorHandler(string channelId, int err, string message)
    {
        logger.UpdateLog(string.Format("Channel1OnErrorHandler channelId: {0}, err: {1}, message: {2}", channelId, err,
            message));
    }

    void EngineOnErrorHandler(int err, string message)
    {
        logger.UpdateLog(string.Format("EngineOnErrorHandler channelId: {0}, err: {1}, message: {2}", CHANNEL_NAME_1, err,
            message));
    }


    void Channel1OnUserJoinedHandler(string channelId, uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("Channel1OnUserJoinedHandler channelId: {0} uid: ${1} elapsed: ${2}", channelId,
            uid, elapsed));
    }

    void EngineOnUserJoinedHandler(uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("EngineOnUserJoinedHandler channelId: {0} uid: ${1} elapsed: ${2}", CHANNEL_NAME_1,
            uid, elapsed));
    }

    void ChannelOnUserOfflineHandler(string channelId, uint uid, USER_OFFLINE_REASON reason)
    {
        logger.UpdateLog(string.Format("OnUserOffLine uid: ${0}, reason: ${1}", uid, (int)reason));
    }

    void EngineOnUserOfflineHandler(uint uid, USER_OFFLINE_REASON reason)
    {
        logger.UpdateLog(string.Format("OnUserOffLine uid: ${0}, reason: ${1}", uid, (int)reason));
    }

    public void RespawnLocal(string channelName)
    {
        GameObject go = GameObject.Find(channelName + "_0");
        if (go != null)
        {
            go.name = "Destroying";
            Destroy(go);
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
            remoteUserDisplays.Remove(LastRemote);
        }
    }

    GameObject LastRemote = null;

    
}