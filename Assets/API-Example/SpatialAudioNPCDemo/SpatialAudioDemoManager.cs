using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using agora_gaming_rtc;
using System.Linq;

public class SpatialAudioDemoManager : MonoBehaviour
{
    [SerializeField] private string APP_ID = "YOUR_APPID";
    public AppInfoObject appInfo;

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

    public InputField appIdText, nicknameText, channelNameText;

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

    public List<string> names = new List<string>();
    public string name;
    public Text nameText;
    public NPCInfo playerInfo;

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

        Random.InitState((int)System.DateTime.Now.Ticks);
        int index = Random.Range(0, names.Count);

        nicknameText.text = names[index];
        

        //channel setup.
        appIdText.text = appInfo.appID;
        //tokenText.text = TOKEN_1;
        channelNameText.text = CHANNEL_NAME_1;

    }

    public void updateAppID()
    {
        APP_ID = appIdText.text;
    }

    public void updateToken()
    {
        //TOKEN_1 = tokenText.text;
    }

    public void updateChannelName()
    {
        CHANNEL_NAME_1 = channelNameText.text;
    }

    public void updateNickname()
    {
        name = nicknameText.text;
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

            for(int x = 0; x < NPCInfoWindow.instance.NPCInfos.Count; x++)
            {
                NPCInfoWindow.instance.updateNPC(NPCEffects[(uint)(1000 + x)], NPCs[x]);
            }



            nameText.text = name;
            playerInfo.info.name = name;
            playerInfo.info.attenuation = .5f;
            playerInfo.info.position = player.transform.position;
            playerInfo.info.forward = player.transform.forward;
            playerInfo.info.right = player.transform.right;
            playerInfo.info.top = player.transform.up;

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
        return (appInfo.appID.Length > 10);
    }

    void InitEngine()
    {
        mRtcEngine = IRtcEngine.GetEngine(appInfo.appID);
        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);
        
        spatialAudio = mRtcEngine.GetLocalSpatialAudioEngine();
        spatialAudio.Initialize();
        mRtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE);
        mRtcEngine.OnJoinChannelSuccess += EngineOnJoinChannelSuccessHandler;
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
        name = nicknameText.text;
        nameText.text = name;
        string TEST_URL = "./AgoraWebSDK/libs/resources/DemoResources/paul/ToddEmbleyDemo.mp3";
        Debug.Log(mRtcEngine);
        mRtcEngine.JoinChannel(appInfo.token, appInfo.appID, "", 0, new ChannelMediaOptions(true, true, true, true));
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

    void OnDestroy()
    {
        for (int x = 0; x < NPCs.Length; x++)
        {
            if (mRtcEngine != null)
                mRtcEngine.RemoveRemotePosition((uint)(1000 + x));
        }
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

