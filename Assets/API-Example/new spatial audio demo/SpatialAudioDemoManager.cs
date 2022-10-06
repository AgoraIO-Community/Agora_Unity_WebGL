using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using System.Linq;

public class SpatialAudioDemoManager : MonoBehaviour
{
    [SerializeField] private string APP_ID = "YOUR_APPID";

    [SerializeField] private string TOKEN_1 = "";

    [SerializeField] private string CHANNEL_NAME_1 = "YOUR_CHANNEL_NAME_1";
    private IRtcEngine mRtcEngine = null;

    public Button joinButton, leaveButton;
    public bool joinedChannel = false;

    public double azimuth = 0f;
    public double elevation = 0f;
    public double distance = 1f;
    public int orientation = 0;
    public double attenuation = 0f;

    public bool spatialBlur = false;
    public bool spatialAirAbsorb = false;

    public InputField appIdText, tokenText, channelNameText;
    public Dropdown userDropdown;

    [SerializeField]
    private List<uint> remoteClientIDs = new List<uint>();



    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        if (!CheckAppId())
        {
            return;
        }

        InitEngine();
        JoinChannel();
        mRtcEngine.EnableLocalMediaSpatialAudio(true, "");
        //channel setup.
        // appIdText.text = APP_ID;
        // tokenText.text = TOKEN_1;
        // channelNameText.text = CHANNEL_NAME_1;

    }

    public void updateAppID()
    {
        APP_ID = appIdText.text;
    }

    public void updateToken()
    {
        TOKEN_1 = tokenText.text;
    }

    public void updateChannelName()
    {
        CHANNEL_NAME_1 = channelNameText.text;
    }

    void Update()
    {
        PermissionHelper.RequestMicrophontPermission();

        if (joinedChannel)
        {

            // joinButton.interactable = false;
            // leaveButton.interactable = true;
            mRtcEngine.updateLocalSpatialAudioPosition(transform.position, transform.forward);
        }
        else
        {

            // joinButton.interactable = true;
            // leaveButton.interactable = false;

            // appIdText.interactable = true;
            // tokenText.interactable = true;
            // channelNameText.interactable = true;

        }

        
    }

    bool CheckAppId()
    {
        return (APP_ID.Length > 10);
    }

    void InitEngine()
    {
        mRtcEngine = IRtcEngine.GetEngine(APP_ID);
        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);

        //mRtcEngine.EnableAudio();
        mRtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        mRtcEngine.OnJoinChannelSuccess += EngineOnJoinChannelSuccessHandler;
        mRtcEngine.OnUserJoined += EngineOnUserJoinedHandler;
        mRtcEngine.OnUserOffline += EngineOnUserOfflineHandler;
        mRtcEngine.OnLeaveChannel += EngineOnLeaveChannelHandler;
    }

    void EngineOnJoinChannelSuccessHandler(string channelId, uint uid, int elapsed)
    {

    }

    void EngineOnUserJoinedHandler(uint uid, int elapsed)
    {
        remoteClientIDs.Add(uid);
    }

    void EngineOnLeaveChannelHandler(RtcStats stats)
    {
        remoteClientIDs.Clear();
    }

    void EngineOnUserOfflineHandler(uint uid, USER_OFFLINE_REASON reason)
    {
        remoteClientIDs.Remove(uid);
    }

    public void JoinChannel()
    {
        mRtcEngine.JoinChannel(TOKEN_1, CHANNEL_NAME_1, "", 0, new ChannelMediaOptions(true, true, true, true));
        joinedChannel = true;
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

    public void updateSpatialAudio()
    {
        uint uid = remoteClientIDs[userDropdown.value];
        Debug.Log("Updating spatial effect for uid:" + uid);
        mRtcEngine.SetRemoteUserSpatialAudioParams(uid.ToString(), azimuth, elevation, distance, orientation, attenuation, spatialBlur, spatialAirAbsorb);
    }



}

