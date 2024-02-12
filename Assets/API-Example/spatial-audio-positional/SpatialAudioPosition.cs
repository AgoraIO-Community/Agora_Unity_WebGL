using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;


namespace Agora_RTC_Plugin.API_Example.Examples.Advanced
{
    /// <summary>
    ///   This scene shows a quick demo of the spatial audio effect on a remote
    /// user with respect to the local user.  Move the slider to change remote
    /// user's X value in the coordinate system.
    /// </summary>
    public class SpatialAudioPosition : MonoBehaviour
    {

        [Header("Application Info")]
        [SerializeField]
        private AppInfoObject appInfo;

        // Fill in your channel name.
        [SerializeField]
        string _channelName = "";

        // Fill in your app ID.
        string _appID = "";
        // Fill in the temporary token you obtained from Agora Console.
        string _token = "";

        [SerializeField] uint MediaUID = 666666;
        [SerializeField] string MediaURL;

        [Header("UI Controls")]
        [SerializeField] Button DecreaseBtn;
        [SerializeField] Button IncreaseBtn;
        [SerializeField] Button UserOffBtn;
        [SerializeField] Text DistanceLabel;
        [SerializeField] Toggle MediaToggle;
        [SerializeField] Slider DistanceSlider;
        [SerializeField] GameObject MediaObject;

        // A variable to save the remote user uid.
        uint RemoteUid { get; set; }

        VideoSurface LocalView;
        VideoSurface RemoteView;

        IRtcEngine RtcEngine;
        ILocalSpatialAudioEngine localSpatial;

        bool MediaPlaying { get { return MediaToggle.isOn; } }
        float XDistance { get; set; }
        const float MAX_DISTANCE = 50;


        // Start is called before the first frame update
        void Start()
        {
            bool appid_OK = CheckAppId();
            Debug.Assert(appid_OK, "App ID NOT GOOD! Please check your AppIDInput asset");
            if (!appid_OK) { return; }

            _appID = appInfo.appID;
            _token = appInfo.token;

            if (RootMenuControl.instance)
            {
                _channelName = RootMenuControl.instance.channel;
            }
            SetupVideoSDKEngine();
            ConfigureSpatialAudioEngine();

            SetupUI();
        }


        bool CheckAppId()
        {
            return (appInfo.appID.Length > 10);
        }


        void Update()
        {
            PermissionHelper.RequestMicrophontPermission();
        }

        void OnDestroy()
        {
            if (RtcEngine != null)
            {
                Leave();
                if (localSpatial != null)
                {
                    localSpatial.Release();
                }

                if (RtcEngine != null)
                {
                    RtcEngine.DisableVideoObserver();
                    IRtcEngine.Destroy();
                }
                RtcEngine = null;
            }
        }

        private void SetupVideoSDKEngine()
        {
            RtcEngine = IRtcEngine.GetEngine(_appID);
            RtcEngine.SetLogFile("agora_log.txt");
            RtcEngine.OnJoinChannelSuccess += EngineOnJoinChannelSuccessHandler;
            RtcEngine.OnUserJoined += EngineOnUserJoinedHandler;
            RtcEngine.OnUserOffline += EngineOnUserOfflineHandler;
            RtcEngine.OnLeaveChannel += EngineOnLeaveChannelHandler;
        }

        #region --- Agora Event Handlers ---

        void EngineOnJoinChannelSuccessHandler(string channelId, uint uid, int elapsed)
        {
            Debug.Log($"Joined Channel {channelId} uid:{uid}");
            MediaToggle.interactable = false;
            if (MediaPlaying)
            {
                RtcEngine.StartLocalMediaSpatialAudio(MediaUID, MediaURL);
                RemoteUid = MediaUID;
                EnableDistanceControl(true);
                updateSpatialAudioPosition(0);
            }
        }

        void EngineOnUserJoinedHandler(uint uid, int elapsed)
        {
            Debug.Log($"User {uid} joined. ");
            if (RemoteUid != 0 && uid != RemoteUid)
            {
                Debug.LogWarning($"User {uid} joined, but this test is already ongoing with another user ${RemoteUid}");
                return;
            }
            RemoteUid = uid;
            RemoteView.SetForUser(uid);
            RemoteView.SetEnable(true);
            EnableDistanceControl(true);
        }

        void EngineOnLeaveChannelHandler(RtcStats stats)
        {
            RemoteUid = 0;
            MediaToggle.interactable = true;
        }

        void EngineOnUserOfflineHandler(uint uid, USER_OFFLINE_REASON reason)
        {
            Debug.Log($"User {uid} is offline now:{reason}. ");
            RemoteView.SetEnable(false);
            EnableDistanceControl(false);
            RemoteUid = 0;
        }
        #endregion

        private void ConfigureSpatialAudioEngine()
        {
            int rc = 0;
            RtcEngine.EnableAudio();
            localSpatial = RtcEngine.GetLocalSpatialAudioEngine();
            localSpatial.Initialize();

            // Set the audio reception range, in meters, of the local user
            rc = localSpatial.SetAudioRecvRange(50);
            Debug.Assert(rc == 0, "SetAudioRecvRange ------> " + rc);
            // Set the length, in meters, of unit distance
            rc = localSpatial.SetDistanceUnit(1);
            Debug.Assert(rc == 0, "SetDistanceUnit ------> " + rc);
            // Update self position
            float[] pos = new float[] { 0.0F, 0.0F, 0.0F };
            float[] forward = new float[] { 1.0F, 0.0F, 0.0F };
            float[] right = new float[] { 0.0F, 1.0F, 0.0F };
            float[] up = new float[] { 0.0F, 0.0F, 1.0F };
            rc = localSpatial.UpdateSelfPosition(pos, forward, right, up);
            Debug.Assert(rc == 0, "SetDistanceUnit ------> " + rc);
        }

        Vector3 RemotePosition = new Vector3(0, 4, 0);

        public void updateSpatialAudioPosition(float sourceDistance)
        {
            RemotePosition = new Vector3(sourceDistance, RemotePosition.y, RemotePosition.z);
            // Set the coordinates in the world coordinate system.
            // This parameter is an array of length 3
            // The three values represent the front, right, and top coordinates
            float[] position = new float[] { RemotePosition.x, RemotePosition.y, RemotePosition.z };
            // Set the unit vector of the x axis in the coordinate system.
            // This parameter is an array of length 3,
            // The three values represent the front, right, and top coordinates
            float[] forward = new float[] { 1, 0.0F, 0.0F };
            // Update the spatial position of the specified remote user
            RemoteVoicePositionInfo remotePosInfo = new RemoteVoicePositionInfo
            {
                position = position,
                forward = forward
            };
            var rc = localSpatial.UpdateRemotePosition(RemoteUid, remotePosInfo);
            Debug.Log("Remote user spatial position updated, rc = " + rc);
        }


        private void SetupUI()
        {
            GameObject go = GameObject.Find("LocalView");
            LocalView = go.AddComponent<VideoSurface>();
            LocalView.SetEnable(false);
            go.transform.Rotate(0.0f, 0.0f, 180.0f);
            go = GameObject.Find("RemoteView");
            RemoteView = go.AddComponent<VideoSurface>();
            RemoteView.SetEnable(false);
            go.transform.Rotate(0.0f, 0.0f, 180.0f);
            go = GameObject.Find("Leave");
            go.GetComponent<Button>().onClick.AddListener(Leave);
            go = GameObject.Find("Join");
            go.GetComponent<Button>().onClick.AddListener(Join);

            DecreaseBtn.onClick.AddListener(DecreaseDistance);
            IncreaseBtn.onClick.AddListener(IncreaseDistance);
            UserOffBtn.onClick.AddListener(RemoveRemoteUserSpatial);

            EnableDistanceControl(false);
            DistanceSlider.maxValue = MAX_DISTANCE;
            DistanceSlider.minValue = -MAX_DISTANCE;
            XDistance = 0;
            UpdateUI();

            // Reference the slider from the UI
            // Add a listener to the slider and which invokes distanceSlider when the slider is dragged left or right.
            DistanceSlider.onValueChanged.AddListener(delegate
            {
                var dist = (int)DistanceSlider.value;
                updateSpatialAudioPosition(dist);
                XDistance = dist;
                DistanceLabel.text = RemotePosition.ToString();
            });

            MediaToggle.onValueChanged.AddListener(delegate
            {
                MediaObject.SetActive(MediaToggle.isOn);
            });
        }

        void UpdateUI()
        {
            DistanceSlider.value = XDistance;
            DistanceLabel.text = RemotePosition.ToString();
        }

        void EnableDistanceControl(bool enabled)
        {
            DecreaseBtn.interactable = enabled;
            IncreaseBtn.interactable = enabled;
            UserOffBtn.interactable = enabled;
            DistanceSlider.interactable = enabled;
        }
        void Join()
        {
            // Enable the video module.
            RtcEngine.EnableVideo();
            RtcEngine.EnableLocalVideo(true);
            RtcEngine.EnableVideoObserver();
            // Set the user role as broadcaster.
            RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
            // Set the local video view.
            LocalView.SetForUser(0);
            // Start rendering local video.
            LocalView.SetEnable(true);
            // Join a channel.
            RtcEngine.JoinChannelByKey(appInfo.token, _channelName, "", 0);
        }

        void Leave()
        {
            // Leaves the channel.
            RtcEngine.LeaveChannel();
            // Disable the video modules.
            RtcEngine.DisableVideo();
            // Stops rendering the remote video.
            RemoteView.SetEnable(false);
            // Stops rendering the local video.
            LocalView.SetEnable(false);
        }

        const float DIST_DELTA = 10;
        void DecreaseDistance()
        {
            XDistance -= DIST_DELTA;
            if (XDistance < -MAX_DISTANCE)
            {
                XDistance = -MAX_DISTANCE;
            }
            updateSpatialAudioPosition(XDistance);
            UpdateUI();
        }

        void IncreaseDistance()
        {
            XDistance += DIST_DELTA;
            if (XDistance > MAX_DISTANCE)
            {
                XDistance = MAX_DISTANCE;
            }
            updateSpatialAudioPosition(XDistance);
            UpdateUI();
        }

        void RemoveRemoteUserSpatial()
        {
            Debug.Log($"Removing user {RemoteUid} from Spatial Audio Positional effect");
            localSpatial.RemoveRemotePosition(RemoteUid);
            XDistance = 0;
            RemotePosition = Vector3.zero;
            UpdateUI();
            EnableDistanceControl(false);
        }

    }
}
