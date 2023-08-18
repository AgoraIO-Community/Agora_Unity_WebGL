using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using agora_utilities;

namespace agora_sample_code
{
    /// <summary>
    ///   Demo for sending individual audio stream into audio source instance.
    ///   This demo does not manage local user's camera for simplicity.
    /// </summary>
    public class UserAudioFrame2SourceSample : MonoBehaviour, IUserAudioFrameDelegate
    {
        public AppInfoObject appInfo;

        [SerializeField]
        private string CHANNEL_NAME = "YOUR_CHANNEL_NAME";
        public Text _logText;
        private Logger _logger;
        private IRtcEngine _rtcEngine = null;

        [SerializeField]
        public Transform RootSpace;

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
            return _logger.DebugAssert(appInfo.appID.Length > 10, "<color=red>[STOP] Please fill in your appId in your AppIDInfo Object!!!! \n (Assets/API-Example/_AppIDInfo/AppIDInfo)</color>");
        }


        void InitEngine()
        {
            _rtcEngine = IRtcEngine.GetEngine(appInfo.appID);
            _rtcEngine.SetLogFile("log.txt");
            _rtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);
            _rtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
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
            //_rtcEngine.EnableLocalVideo(true);  // not needed for WebGL
            _rtcEngine.EnableVideoObserver();
            _rtcEngine.JoinChannelByKey(appInfo.token, CHANNEL_NAME, "", 0);

        }
        void OnJoinChannelSuccessHandler(string channelName, uint uid, int elapsed)
        {
            _logger.UpdateLog(string.Format("sdk version: ${0}", IRtcEngine.GetSdkVersion()));
            _logger.UpdateLog(string.Format("onJoinChannelSuccess channelName: {0}, uid: {1}, elapsed: {2}", channelName, uid, elapsed));

            _audioRawDataManager.SetOnPlaybackAudioFrameBeforeMixingCallback(OnPlaybackAudioFrameBeforeMixingHandler);
            makeVideoView(CHANNEL_NAME, 0);
        }

        void OnLeaveChannelHandler(RtcStats stats)
        {
            _logger.UpdateLog("OnLeaveChannelSuccess");
        }

        int userCount = 0;
        void OnUserJoinedHandler(uint uid, int elapsed)
        {
            _logger.UpdateLog(string.Format("OnUserJoinedHandler channelId: {0} uid: ${1} elapsed: ${2}", CHANNEL_NAME,
                uid, elapsed));
            _remoteUserObject[uid] = makeVideoView(CHANNEL_NAME, uid);
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

        void Dispatch(System.Action action)
        {
            lock (_queue)
            {
                _queue.Enqueue(action);
            }
        }

        int count = 0;
        const int MAXAUC = 5; // debug print count
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

        void OnDestroy()
        {
            if (_rtcEngine != null)
            {
                _rtcEngine.LeaveChannel();
                _rtcEngine.DisableVideoObserver();
                if (_audioRawDataManager != null)
                {
                    AudioRawDataManager.ReleaseInstance();
                }
                IRtcEngine.Destroy();
                _rtcEngine = null;
            }
        }


        private GameObject makeVideoView(string channelId, uint uid)
        {
            string objName = channelId + "_" + uid.ToString();
            GameObject go = GameObject.Find(objName);
            if (!ReferenceEquals(go, null))
            {
                return go; // reuse
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
            }
            videoSurface.transform.SetParent(RootSpace);
            return videoSurface.gameObject;
        }
        private const float Offset = 100;
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

            GameObject canvas = GameObject.Find("VideoPanel");
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
            Vector2 pos = AgoraUIUtils.GetRandomPosition(60);
            go.transform.localPosition = new Vector3(pos.x, pos.y, 0f);
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
}