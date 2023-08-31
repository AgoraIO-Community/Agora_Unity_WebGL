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
    private ILocalSpatialAudioEngine spatialAudio;

    public Button joinButton, leaveButton;
    public bool joinedChannel = false;

    public double azimuth = 0f;
    public double elevation = 0f;
    public double distance = 1f;
    public int orientation = 0;
    public double attenuation = 0f;

    public bool spatialBlur = false;
    public bool spatialAirAbsorb = false;

    public string[] soundFiles;

    public InputField appIdText, tokenText, channelNameText;

    public Transform peter;

    public Transform[] NPCs;
    public float[] NPCDistanceMod, NPCSoundRange;

    [SerializeField]
    private List<uint> remoteClientIDs = new List<uint>();

    public GameObject loginScreen, player;

    public SpatialAudioAvatar avatar;

    Vector3[] directions = { Vector3.right, Vector3.down, Vector3.left, Vector3.up };

    public static bool isHoveringOverButton;

    public Dictionary<uint, NPCEffectParams> NPCEffects = new Dictionary<uint, NPCEffectParams>();

    public bool NPCSettingsEnabled = false;

    public NPCSettings npcSettings;

    public static SpatialAudioDemoManager demo;

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
        demo = this;
        InitEngine();



        //channel setup.
        appIdText.text = APP_ID;
        tokenText.text = TOKEN_1;
        channelNameText.text = CHANNEL_NAME_1;

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
            mRtcEngine.updateSelfPosition(player.transform.position, -player.transform.forward, player.transform.right, player.transform.up);
            for (int x = 0; x < soundFiles.Length; x++)
            {
                if (NPCs.Length > x)
                {
                    mRtcEngine.updatePlayerPositionInfo((1000 + x).ToString(), NPCs[x].position * NPCDistanceMod[x], (NPCs[x].transform.position - player.transform.position).normalized);
                    float posDiff = Vector3.Distance(player.transform.position, NPCs[x].transform.position);
                    if (posDiff > NPCSoundRange[x])
                        mRtcEngine.muteLocalMediaSpatialAudio((uint)(1000 + x), true);
                    else
                        mRtcEngine.muteLocalMediaSpatialAudio((uint)(1000 + x), false);
                }
            }
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

        mRtcEngine.EnableAudio();
        spatialAudio = mRtcEngine.GetLocalSpatialAudioEngine();
        spatialAudio.Initialize();
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
        string TEST_URL = "./AgoraWebSDK/libs/resources/DemoResources/paul/ToddEmbleyDemo.mp3";
        mRtcEngine.JoinChannel(TOKEN_1, CHANNEL_NAME_1, "", 0, new ChannelMediaOptions(true, true, true, true));
        for (uint i = 0; i < soundFiles.Length; i++)
        {
            mRtcEngine.StartLocalMediaSpatialAudio(((uint)1000 + i), soundFiles[i]);
            NPCEffects.Add((uint)1000 + i, new NPCEffectParams(NPCs[i].gameObject.name, (uint)1000 + i));
        }
        joinedChannel = true;
        loginScreen.SetActive(false);
        player.SetActive(true);
        peter.gameObject.SetActive(true);
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
        mRtcEngine.SetRemoteUserSpatialAudioParams(0, azimuth, elevation, distance, orientation, attenuation, spatialBlur, spatialAirAbsorb);
    }

    public void updateIsHovering(bool setting)
    {
        isHoveringOverButton = setting;
    }

    public void updateNPCSettings(int index)
    {
        Debug.Log(NPCEffects[(uint)index]);
        NPCSettingsEnabled = !NPCSettingsEnabled;
        NPCSettings.instance.enableParamWindow(NPCSettingsEnabled, NPCEffects[(uint)index]);
    }

    public void updateSpatialAudioParams(NPCEffectParams newParams)
    {
        mRtcEngine.SetRemoteUserSpatialAudioAttenuation(newParams.uid, newParams.attenuation);
        mRtcEngine.SetRemoteUserSpatialAudioBlur(newParams.uid, newParams.blur);
        mRtcEngine.SetRemoteUserSpatialAudioAirAbsorb(newParams.uid, newParams.airAbsorb);
    }

    public void updateNPC(NPCEffectParams par)
    {
        NPCEffects[(uint)par.uid] = par;
        updateSpatialAudioParams(par);
    }

    public class NPCEffectParams
    {
        public uint uid;
        public string name;
        public double attenuation = .5f;
        public bool blur = false;
        public bool airAbsorb = false;

        public NPCEffectParams(string n, uint u)
        {
            name = n;
            uid = u;
        }
    }
}

