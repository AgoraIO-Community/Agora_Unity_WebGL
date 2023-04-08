using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;

namespace agora_sample_code
{
    /// <summary>
    ///   Demo for sending individual audio stream into audio source instance.
    ///   This demo does not manage local user's camera for simplicity.
    /// </summary>
    public class UserAudioFrame2SourceSample : MonoBehaviour, IUserAudioFrameDelegate
    {
        [SerializeField]
        private string APP_ID = "";

        [SerializeField]
        private string TOKEN = "";

        [SerializeField]
        private string CHANNEL_NAME = "YOUR_CHANNEL_NAME";
        public Text _logText;
        private Logger _logger;
        private IRtcEngine _rtcEngine = null;

        [SerializeField]
        public Transform RootSpace;
        [SerializeField]
        public GameObject UserPrefab;

        private IAudioRawDataManager _audioRawDataManager;
        public AudioRawDataManager.OnPlaybackAudioFrameBeforeMixingHandler HandleAudioFrameForUser
        {
            get; set;
        }


        private Queue<System.Action> _queue;
        private Dictionary<uint, GameObject> _remoteUserObject = new Dictionary<uint, GameObject>();
        private HashSet<uint> _remoteUserConfigured = new HashSet<uint>();

        private void Awake()
        {
            _queue = new Queue<System.Action>();
            if (UserPrefab == null)
            {
                Debug.LogWarning("User prefab wasn't assigned, generating primitive object as prefab.");
                MakePrefab();
            }
        }

        void Start()
        {
            PermissionHelper.RequestMicrophontPermission();
            PermissionHelper.RequestCameraPermission();
            if (CheckAppId())
            {
                InitEngine();
                JoinChannel();
            }
        }

        void Update()
        {
            lock (_queue)
            {
                if (_queue.Count > 0)
                {
                    var action = _queue.Dequeue();
                    action();
                }
            }
        }

        bool CheckAppId()
        {
            _logger = new Logger(_logText);
            return _logger.DebugAssert(APP_ID.Length > 10, "Please fill in your appId in VideoCanvas!!!!!");
        }

        void InitEngine()
        {
            _rtcEngine = IRtcEngine.GetEngine(APP_ID);
            _rtcEngine.SetLogFile("log.txt");
            //_rtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);
            //_rtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
            _rtcEngine.OnJoinChannelSuccess += OnJoinChannelSuccessHandler;
            _rtcEngine.OnLeaveChannel += OnLeaveChannelHandler;
            _rtcEngine.OnWarning += OnSDKWarningHandler;
            _rtcEngine.OnError += OnSDKErrorHandler;
            _rtcEngine.OnConnectionLost += OnConnectionLostHandler;
            _rtcEngine.OnUserJoined += OnUserJoinedHandler;
            _rtcEngine.OnUserOffline += OnUserOfflineHandler;
            _audioRawDataManager = AudioRawDataManager.GetInstance(_rtcEngine);
            _audioRawDataManager.RegisterAudioRawDataObserver();
            _rtcEngine.SetDefaultAudioRouteToSpeakerphone(true);
            _rtcEngine.SetParameter("che.audio.external_render", true);
        }

        void JoinChannel()
        {
            _rtcEngine.EnableAudio();
            _rtcEngine.EnableVideo();
            _rtcEngine.EnableLocalVideo(false);
            _rtcEngine.EnableVideoObserver();
            _rtcEngine.JoinChannelByKey(TOKEN, CHANNEL_NAME, "", 0);

        }
        void OnJoinChannelSuccessHandler(string channelName, uint uid, int elapsed)
        {
            _logger.UpdateLog(string.Format("sdk version: ${0}", IRtcEngine.GetSdkVersion()));
            _logger.UpdateLog(string.Format("onJoinChannelSuccess channelName: {0}, uid: {1}, elapsed: {2}", channelName, uid, elapsed));
            // makeVideoView(0);

            _audioRawDataManager.SetOnPlaybackAudioFrameBeforeMixingCallback(OnPlaybackAudioFrameBeforeMixingHandler);

        }

        void OnLeaveChannelHandler(RtcStats stats)
        {
            _logger.UpdateLog("OnLeaveChannelSuccess");
        }

        int userCount = 0;
        void OnUserJoinedHandler(uint uid, int elapsed)
        {
            _logger.UpdateLog(string.Format("OnUserJoined uid: ${0} elapsed: ${1}", uid, elapsed));
            GameObject go = Instantiate(UserPrefab);
            _remoteUserObject[uid] = go;
            go.transform.SetParent(RootSpace);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = new Vector3(userCount * 2, 0, 0);

            VideoSurface v = go.AddComponent<VideoSurface>();
            v.SetForUser(uid);
            v.SetEnable(true);
            v.SetVideoSurfaceType(AgoraVideoSurfaceType.Renderer);
            userCount++;
        }

        void OnUserOfflineHandler(uint uid, USER_OFFLINE_REASON reason)
        {
            _logger.UpdateLog("User " + uid + " went offline, reason:" + reason);
            Dispatch(() => { _logger.UpdateLog("Dispatched log + OFFLINE reason = " + reason); });

            lock (_remoteUserConfigured)
            {
                if (_remoteUserObject.ContainsKey(uid))
                {
                    Destroy(_remoteUserObject[uid]);
                    _remoteUserObject.Remove(uid);
                }

                if (_remoteUserConfigured.Contains(uid))
                {
                    _remoteUserConfigured.Remove(uid);
                }
            }
        }

        void OnSDKWarningHandler(int warn, string msg)
        {
            _logger.UpdateLog(string.Format("OnSDKWarning warn: {0}, msg: {1}", warn, IRtcEngine.GetErrorDescription(warn)));
        }

        void OnSDKErrorHandler(int error, string msg)
        {
            _logger.UpdateLog(string.Format("OnSDKError error: {0}, msg: {1}", error, IRtcEngine.GetErrorDescription(error)));
        }

        void OnConnectionLostHandler()
        {
            _logger.UpdateLog(string.Format("OnConnectionLost "));
        }

        public void Dispatch(System.Action action)
        {
            lock (_queue)
            {
                _queue.Enqueue(action);
            }
        }

        int count = 0;
        const int MAXAUC = 5;
        void OnPlaybackAudioFrameBeforeMixingHandler(uint uid, AudioFrame audioFrame)
        {
            // limited log
            if (count < MAXAUC)
                Debug.LogWarning("count(" + count + "): OnPlaybackAudioFrameBeforeMixingHandler =============+> " + audioFrame);
            count++;

            // The audio stream info contains in this audioframe, we will use this construct the AudioClip
            lock (_remoteUserConfigured)
            {
                if (!_remoteUserConfigured.Contains(uid) && _remoteUserObject.ContainsKey(uid))
                {
                    if (count < MAXAUC)
                        Dispatch(() =>
                        {
                            _logger.UpdateLog("Uid:" + uid + " setting up audio frame handler....");
                        });

                    GameObject go = _remoteUserObject[uid];
                    if (go != null)
                    {
                        Dispatch(() =>
                        {
                            UserAudioFrameHandler userAudio = go.GetComponent<UserAudioFrameHandler>();
                            if (userAudio == null)
                            {
                                userAudio = go.AddComponent<UserAudioFrameHandler>();
                                userAudio.Init(uid, this, audioFrame);
                                _remoteUserConfigured.Add(uid);
                            }
                            go.SetActive(true);
                        });
                    }
                    else
                    {
                        Dispatch(() =>
                        {
                            _logger.UpdateLog("Uid: " + uid + " setting up audio frame handler._<> no go");
                        });
                    }
                }
            }

            if (HandleAudioFrameForUser != null)
            {
                HandleAudioFrameForUser(uid, audioFrame);
            }
        }

        void OnApplicationQuit()
        {
            Debug.Log("OnApplicationQuit");
            if (_rtcEngine != null)
            {
                _rtcEngine.LeaveChannel();
                _rtcEngine.DisableVideoObserver();
                if (_audioRawDataManager != null)
                {
                    AudioRawDataManager.ReleaseInstance();
                }
                IRtcEngine.Destroy();
            }
        }

        protected virtual void MakePrefab()
        {
            Debug.LogWarning("Generating cube as prefab.");
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UserPrefab = go;
            go.transform.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 45f, 45f));
            MeshRenderer mesh = GetComponent<MeshRenderer>();
            if (mesh != null)
            {
                mesh.material = new Material(Shader.Find("Unlit/Texture"));
            }
            go.SetActive(false);
        }
    }
}