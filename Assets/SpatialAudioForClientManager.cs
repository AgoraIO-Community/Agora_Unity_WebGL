using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using agora_utilities;

public class SpatialAudioForClientManager : MonoBehaviour
{
    [SerializeField] private string APP_ID = "YOUR_APPID";

    [SerializeField] private string TOKEN_1 = "";

    [SerializeField] private string CHANNEL_NAME_1 = "YOUR_CHANNEL_NAME_1";
    public Text logText;
    private Logger logger;
    private IRtcEngine mRtcEngine = null;
    private const float Offset = 100;

    public Button joinButton, leaveButton;
    public bool joinedChannel = false;
    public bool useToken = false;


    // Use this for initialization
    void Start()
    {
        if (!CheckAppId())
        {
            return;
        }

        InitEngine();
        JoinChannel();
        
        
        //channel setup.

        
    }

    

    void Update()
    {
        PermissionHelper.RequestMicrophontPermission();
        PermissionHelper.RequestCameraPermission();

        if(joinedChannel){
            joinButton.interactable = false;
            leaveButton.interactable = true;
        } else {
            joinButton.interactable = true;
            leaveButton.interactable = false;
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

        mRtcEngine.OnJoinChannelSuccess = EngineOnJoinChannelSuccessHandler;
        mRtcEngine.OnLeaveChannel = EngineOnLeaveChannelHandler;


        mRtcEngine.OnError += EngineOnErrorHandler;

    }

    public void JoinChannel()
    {
        if(!useToken){
            mRtcEngine.JoinChannel(TOKEN_1, CHANNEL_NAME_1, "", 0, new ChannelMediaOptions(true, true, true, true));
        } else {
            TokenClient.Instance.SetRtcEngineInstance(mRtcEngine);
            TokenClient.Instance.GetTokens(CHANNEL_NAME_1, 0, (token, rtm) =>
            {
                TOKEN_1 = token;
                Debug.Log(gameObject.name + " Got rtc token:" + TOKEN_1);
                mRtcEngine.JoinChannelByKey(TOKEN_1, CHANNEL_NAME_1);
            });
        }
        joinedChannel = true;
        mRtcEngine.EnableSpatialAudio(true);
    }

    public void LeaveChannel()
    {
        mRtcEngine.LeaveChannel();
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
        logger.UpdateLog(string.Format("EngineOnJoinChannelSuccess channelId: {0}, uid: {1}, elapsed: {2}", CHANNEL_NAME_1, uid,
            elapsed));
    }

    void EngineOnLeaveChannelHandler(RtcStats rtcStats)
    {
        logger.UpdateLog(string.Format("OnLeaveChannelHandler channelId: {0}", CHANNEL_NAME_1));
    }

    void EngineOnErrorHandler(int err, string message)
    {
        logger.UpdateLog(string.Format("UserErrorHandler err: {0}, message: {1}", err,
            message));
    }

    void EngineOnUserJoinedHandler(uint uid, int elapsed)
    {
        logger.UpdateLog(string.Format("OnUserJoinedHandler channelId: {0} uid: ${1} elapsed: ${2}", CHANNEL_NAME_1,
            uid, elapsed));
    }

    void EngineOnUserOfflineHandler(uint uid, USER_OFFLINE_REASON reason)
    {
        logger.UpdateLog(string.Format("OnUserOffLine uid: ${0}, reason: ${1}", uid, (int)reason));
    }

    
}