using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Types;
using agora_gaming_rtc;


namespace Agora_RTC_Plugin.API_Example.Examples.Advanced.SpatialAudioWithUsers
{
    /// <summary>
    ///   This scene shows a quick demo of the spatial audio effect on a remote
    /// user with respect to the local user.  Move the slider to change remote
    /// user's X value in the coordinate system.
    /// </summary>
    public class SpatialAudioWithUsers : MonoBehaviour
    {

        [SerializeField]
        private AppInfoObject appInfo;

        // Fill in your channel name.
        [SerializeField]
        string _channelName = "";

        // Fill in your app ID.
        string _appID = "";
        // Fill in the temporary token you obtained from Agora Console.
        string _token = "";

        // A variable to save the remote user uid.
        private uint remoteUid;
        internal VideoSurface LocalView;
        internal VideoSurface RemoteView;
        internal IRtcEngine RtcEngine;
        private ILocalSpatialAudioEngine localSpatial;
        // Slider control for spatial audio.
        private Slider distanceSlider;

        // Start is called before the first frame update
        void Start()
        {
            bool appid_OK = CheckAppId();
            Debug.Assert(appid_OK, "App ID NOT GOOD! Please check your AppIDInput asset");
            if (!appid_OK) { return; }

            _appID = appInfo.appID;
            _token = appInfo.token;

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
            RtcEngine.OnJoinChannelSuccess += EngineOnJoinChannelSuccessHandler;
            RtcEngine.OnUserJoined += EngineOnUserJoinedHandler;
            RtcEngine.OnUserOffline += EngineOnUserOfflineHandler;
            RtcEngine.OnLeaveChannel += EngineOnLeaveChannelHandler;
        }


        void EngineOnJoinChannelSuccessHandler(string channelId, uint uid, int elapsed)
        {
            Debug.Log($"Joined Channel {channelId} uid:{uid}");
        }

        void EngineOnUserJoinedHandler(uint uid, int elapsed)
        {
            Debug.Log($"User {uid} joined. ");
            remoteUid = uid;
            RemoteView.SetForUser(uid);
            RemoteView.SetEnable(true);
        }

        void EngineOnLeaveChannelHandler(RtcStats stats)
        {
            Debug.Log($"Left channel. ");
        }

        void EngineOnUserOfflineHandler(uint uid, USER_OFFLINE_REASON reason)
        {
            Debug.Log($"User {uid} is offline now:{reason}. ");
            RemoteView.SetEnable(false);
        }

        private void ConfigureSpatialAudioEngine()
        {
            RtcEngine.EnableAudio();
            localSpatial = RtcEngine.GetLocalSpatialAudioEngine();
            localSpatial.Initialize();

            // Set the audio reception range, in meters, of the local user
            localSpatial.SetAudioRecvRange(50);
            // Set the length, in meters, of unit distance
            localSpatial.SetDistanceUnit(1);
            // Update self position
            float[] pos = new float[] { 0.0F, 0.0F, 0.0F };
            float[] forward = new float[] { 1.0F, 0.0F, 0.0F };
            float[] right = new float[] { 0.0F, 1.0F, 0.0F };
            float[] up = new float[] { 0.0F, 0.0F, 1.0F };
            localSpatial.UpdateSelfPosition(pos, forward, right, up);
        }


        public void updateSpatialAudioPosition(float sourceDistance)
        {
            // Set the coordinates in the world coordinate system.
            // This parameter is an array of length 3
            // The three values represent the front, right, and top coordinates
            float[] position = new float[] { sourceDistance, 4.0F, 0.0F };
            // Set the unit vector of the x axis in the coordinate system.
            // This parameter is an array of length 3,
            // The three values represent the front, right, and top coordinates
            float[] forward = new float[] { sourceDistance, 0.0F, 0.0F };
            // Update the spatial position of the specified remote user
            RemoteVoicePositionInfo remotePosInfo = new RemoteVoicePositionInfo
            {
                position = position,
                forward = forward
            };
            var rc = localSpatial.UpdateRemotePosition((uint)remoteUid, remotePosInfo);
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

            // Reference the slider from the UI
            go = GameObject.Find("distanceSlider");
            distanceSlider = go.GetComponent<Slider>();
            // Specify a minimum and maximum value for slider.
            distanceSlider.maxValue = 10;
            distanceSlider.minValue = 0;
            // Add a listener to the slider and which invokes distanceSlider when the slider is dragged left or right.
            distanceSlider.onValueChanged.AddListener(delegate { updateSpatialAudioPosition((int)distanceSlider.value); });

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

    }
}
