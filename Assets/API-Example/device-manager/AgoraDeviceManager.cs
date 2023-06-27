﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using System.Linq;
using agora_utilities;

public class AgoraDeviceManager : MonoBehaviour
{
    [SerializeField]
    private AppInfoObject appInfo;
    [SerializeField]
    private string CHANNEL_NAME = "YOUR_CHANNEL_NAME";

    public Text LogText, videoDeviceText;
    private Logger _logger;
    private IRtcEngine _rtcEngine = null;
    private AudioRecordingDeviceManager _audioRecordingDeviceManager = null;
    private AudioPlaybackDeviceManager _audioPlaybackDeviceManager = null;
    private VideoDeviceManager _videoDeviceManager = null;
    private Dictionary<int, string> _audioRecordingDeviceDic = new Dictionary<int, string>();
    private Dictionary<int, string> _audioPlaybackDeviceDic = new Dictionary<int, string>();
    private Dictionary<int, string> _videoDeviceManagerDic = new Dictionary<int, string>();
    private Dictionary<int, string> _audioRecordingDeviceNamesDic = new Dictionary<int, string>();
    private Dictionary<int, string> _audioPlaybackDeviceNamesDic = new Dictionary<int, string>();
    private Dictionary<int, string> _videoDeviceManagerNamesDic = new Dictionary<int, string>();
    public Dropdown videoDropdown, recordingDropdown, playbackDropdown;
    [SerializeField]
    private int _recordingDeviceIndex = 0,
    _playbackDeviceIndex = 0,
    _videoDeviceIndex = 0,
    currentVideoDeviceIndex = 0;
    private List<uint> remoteClientIDs = new List<uint>();
    private const float Offset = 100;
    GameObject LastRemote = null;
    public Slider recordingVolume, playbackVolume;
    public bool joinedChannel, previewing;
    public Button videoDeviceButton, joinChannelButton, leaveChannelButton, startPreviewButton, stopPreviewButton, cacheVideoButton;
    // Start is called before the first frame update
    private void Awake()
    {
        if (RootMenuControl.instance)
        {
            CHANNEL_NAME = RootMenuControl.instance.channel;
        }
    }

    void Start()
    {
        if (CheckAppId())
        {
            InitRtcEngine();
            
        }
    }

    void Update()
    {
        PermissionHelper.RequestMicrophontPermission();
        PermissionHelper.RequestCameraPermission();

        List<MediaDeviceInfo> devices = AgoraWebGLEventHandler.GetCachedCameras();
        List<string> videoDeviceLabels = new List<string>();

        if (devices.Count > 0)
        {

            if (_videoDeviceManagerNamesDic.Count != devices.Count)
            {
                //GetVideoDeviceManager();
            }



            foreach (MediaDeviceInfo info in devices)
            {
                bool hasLabel = false;
                foreach (Dropdown.OptionData data in videoDropdown.options)
                {
                    if (data.text == info.label)
                        hasLabel = true;
                }
                if (!hasLabel)
                    videoDeviceLabels.Add(info.label);
            }

            if (videoDropdown.options.Count == 0)
            {
                videoDropdown.AddOptions(videoDeviceLabels);
            }

        }

        //recordingDropdown.interactable = numberOfMicrophones > 0;
        //playbackDropdown.interactable = numberOfPlaybacks > 0;
        videoDeviceButton.interactable = (currentVideoDeviceIndex != _videoDeviceIndex);
        videoDropdown.interactable = devices.Count > 0;
        videoDeviceText.gameObject.SetActive(!joinedChannel);
        joinChannelButton.interactable = !joinedChannel && devices.Count > 0;
        leaveChannelButton.interactable = joinedChannel;
        startPreviewButton.interactable = (!previewing && !joinedChannel && devices.Count > 0);
        stopPreviewButton.interactable = (previewing && !joinedChannel && devices.Count > 0);

    }

    public void cacheVideoDevices()
    {
        _rtcEngine.CacheVideoDevices();
        Invoke("GetVideoDeviceManager", .2f);
    }

    public void cacheRecordingDevices()
    {
        _rtcEngine.CacheRecordingDevices();
        Invoke("GetAudioRecordingDevice", .2f);
    }

   

    public void cachePlaybackDevices()
    {
        _rtcEngine.CachePlaybackDevices();
        Invoke("GetAudioPlaybackDevice", .2f);
    }

    bool CheckAppId()
    {
        _logger = new Logger(LogText);
        return _logger.DebugAssert(appInfo.appID.Length > 10, "<color=red>[STOP] Please fill in your appId in your AppIDInfo Object!!!! \n (Assets/API-Example/_AppIDInfo/AppIDInfo)</color>");
    }

    public void playbackUpdate()
    {
        _playbackDeviceIndex = playbackDropdown.value;
        SetAndReleasePlaybackDevice();
    }

    public void recordingUpdate()
    {
        _recordingDeviceIndex = recordingDropdown.value;
        SetAndReleaseRecordingDevice();
    }

    public void videoUpdate()
    {
        _videoDeviceIndex = videoDropdown.value;
        _videoDeviceManager.SetVideoDevice(_videoDeviceManagerDic[_videoDeviceIndex]);
    }

    public void startPreview()
    {
        previewing = true;
        makeVideoView(CHANNEL_NAME, 0);
        _videoDeviceIndex = videoDropdown.value;
        _logger.UpdateLog(_videoDeviceManagerDic[_videoDeviceIndex]);
        _videoDeviceManager.SetVideoDevice(_videoDeviceManagerDic[_videoDeviceIndex]);
    }

    public void stopPreview()
    {
        previewing = false;
        DestroyVideoView(CHANNEL_NAME, 0);
        _rtcEngine.StopPreview();
        _rtcEngine.DisableVideoObserver();
        //videoDropdown.value = _videoDeviceManagerDic.Count;
        //videoDropdown.RefreshShownValue();
    }

    public void volumeUpdate()
    {
        SetCurrentDeviceVolume();
    }

    void InitRtcEngine()
    {
        _rtcEngine = IRtcEngine.GetEngine(appInfo.appID);
        _rtcEngine.SetLogFile("log.txt");
        _rtcEngine.EnableAudio();
        _rtcEngine.EnableVideo();
        _rtcEngine.EnableVideoObserver();
        _rtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);
        _rtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        _rtcEngine.OnJoinChannelSuccess += OnJoinChannelSuccessHandler;
        _rtcEngine.OnUserJoined += EngineOnUserJoinedHandler;
        _rtcEngine.OnUserOffline += OnUserLeaveChannelHandler;
        _rtcEngine.OnLeaveChannel += OnLeaveChannelHandler;
        _rtcEngine.OnWarning += OnSDKWarningHandler;
        _rtcEngine.OnError += OnSDKErrorHandler;
        _rtcEngine.OnConnectionLost += OnConnectionLostHandler;
        _rtcEngine.OnCameraChanged += OnCameraChangedHandler;
        _rtcEngine.OnMicrophoneChanged += OnMicrophoneChangedHandler;
        _rtcEngine.OnPlaybackChanged += OnPlaybackChangedHandler;
    }

    void GetAudioRecordingDevice()
    {
        string audioRecordingDeviceName = "";
        string audioRecordingDeviceId = "";
        _audioRecordingDeviceManager = (AudioRecordingDeviceManager)_rtcEngine.GetAudioRecordingDeviceManager();
        _audioRecordingDeviceManager.CreateAAudioRecordingDeviceManager();
        int count = _audioRecordingDeviceManager.GetAudioRecordingDeviceCount();
        _logger.UpdateLog(string.Format("AudioRecordingDevice count: {0}", count));
        recordingDropdown.ClearOptions();
        _audioRecordingDeviceDic.Clear();
        _audioRecordingDeviceNamesDic.Clear();
        for (int i = 0; i < count; i++)
        {
            _audioRecordingDeviceManager.GetAudioRecordingDevice(i, ref audioRecordingDeviceName, ref audioRecordingDeviceId);
            if (!_audioRecordingDeviceDic.ContainsKey(i))
            {
                _audioRecordingDeviceDic.Add(i, audioRecordingDeviceId);
                _audioRecordingDeviceNamesDic.Add(i, audioRecordingDeviceName);
            }
            _logger.UpdateLog(string.Format("AudioRecordingDevice device index: {0}, name: {1}, id: {2}", i, audioRecordingDeviceName, audioRecordingDeviceId));
        }

        recordingDropdown.AddOptions(_audioRecordingDeviceNamesDic.Values.ToList());
        recordingDropdown.value = _audioRecordingDeviceNamesDic.Count - 1;
    }

    void GetAudioPlaybackDevice()
    {
        string audioPlaybackDeviceName = "";
        string audioPlaybackDeviceId = "";
        _audioPlaybackDeviceManager = (AudioPlaybackDeviceManager)_rtcEngine.GetAudioPlaybackDeviceManager();
        _audioPlaybackDeviceManager.CreateAAudioPlaybackDeviceManager();
        int count = _audioPlaybackDeviceManager.GetAudioPlaybackDeviceCount();
        _logger.UpdateLog(string.Format("AudioPlaybackDeviceManager count: {0}", count));
        playbackDropdown.ClearOptions();
        _audioPlaybackDeviceDic.Clear();
        _audioPlaybackDeviceNamesDic.Clear();
        for (int i = 0; i < count; i++)
        {
            _audioPlaybackDeviceManager.GetAudioPlaybackDevice(i, ref audioPlaybackDeviceName, ref audioPlaybackDeviceId);
            if (!_audioPlaybackDeviceDic.ContainsKey(i))
            {
                _audioPlaybackDeviceDic.Add(i, audioPlaybackDeviceId);
                _audioPlaybackDeviceNamesDic.Add(i, audioPlaybackDeviceName);
            }
            _logger.UpdateLog(string.Format("AudioPlaybackDevice device index: {0}, name: {1}, id: {2}", i, audioPlaybackDeviceName, audioPlaybackDeviceId));
        }

        playbackDropdown.AddOptions(_audioPlaybackDeviceNamesDic.Values.ToList());
        playbackDropdown.value = _audioPlaybackDeviceNamesDic.Count - 1;
    }

    void GetVideoDeviceManager()
    {
        string videoDeviceName = "";
        string videoDeviceId = "";
        /// If you want to getVideoDeviceManager, you need to call startPreview() first;
        _rtcEngine.StartPreview();
        if (_videoDeviceManager == null)
        {
            _videoDeviceManager = (VideoDeviceManager)_rtcEngine.GetVideoDeviceManager();
            _videoDeviceManager.CreateAVideoDeviceManager();
            int count = _videoDeviceManager.GetVideoDeviceCount();
            _logger.UpdateLog(string.Format("VideoDevice count: {0}", count));
            videoDropdown.ClearOptions();
            _videoDeviceManagerDic.Clear();
            _videoDeviceManagerNamesDic.Clear();
            for (int i = 0; i < count; i++)
            {
                _videoDeviceManager.GetVideoDevice(i, ref videoDeviceName, ref videoDeviceId);
                if (!_videoDeviceManagerDic.ContainsKey(i))
                {
                    _videoDeviceManagerDic.Add(i, videoDeviceId);
                    _videoDeviceManagerNamesDic.Add(i, videoDeviceName);
                }

                _logger.UpdateLog(string.Format("VideoDeviceManager device index: {0}, name: {1}, id: {2}", i, videoDeviceName, videoDeviceId));
            }

            videoDropdown.AddOptions(_videoDeviceManagerNamesDic.Values.ToList());
        }
        _videoDeviceIndex = videoDropdown.value;
        _videoDeviceManager.SetVideoDevice(_videoDeviceManagerDic[_videoDeviceIndex]);
    }

    void SetCurrentDevice()
    {
        _audioRecordingDeviceManager.SetAudioRecordingDevice(_audioRecordingDeviceDic[_recordingDeviceIndex]);
        _audioPlaybackDeviceManager.SetAudioPlaybackDevice(_audioPlaybackDeviceDic[_playbackDeviceIndex]);
        _videoDeviceManager.SetVideoDevice(_videoDeviceManagerDic[_videoDeviceIndex]);
    }

    public void SetCurrentDeviceVolume()
    {
        _audioRecordingDeviceManager.SetAudioRecordingDeviceVolume((int)recordingVolume.value);
        _audioPlaybackDeviceManager.SetAudioPlaybackDeviceVolume((int)playbackVolume.value);
    }

    void ReleaseDeviceManager()
    {
        _audioPlaybackDeviceManager.ReleaseAAudioPlaybackDeviceManager();
        _audioRecordingDeviceManager.ReleaseAAudioRecordingDeviceManager();
        _videoDeviceManager.ReleaseAVideoDeviceManager();
    }

    public void SetAndReleaseRecordingDevice()
    {
        _audioRecordingDeviceManager.SetAudioRecordingDevice(_audioRecordingDeviceDic[_recordingDeviceIndex]);
        _audioRecordingDeviceManager.ReleaseAAudioRecordingDeviceManager();
    }

    public void SetAndReleasePlaybackDevice()
    {
        _audioPlaybackDeviceManager.SetAudioPlaybackDevice(_audioPlaybackDeviceDic[_playbackDeviceIndex]);
        _audioPlaybackDeviceManager.ReleaseAAudioPlaybackDeviceManager();
    }

    public void SetAndReleaseVideoDevice()
    {
        _videoDeviceManager.ReleaseAVideoDeviceManager();
        _videoDeviceManager.SetVideoDevice(_videoDeviceManagerDic[_videoDeviceIndex]);        
    }

    public void JoinChannel()
    {
        GetVideoDeviceManager();
        //SetAndReleaseVideoDevice();
        Invoke("startJoiningChannel", 1f);
    }

    public void startJoiningChannel()
    {
        _rtcEngine.JoinChannel(CHANNEL_NAME);
        makeVideoView(CHANNEL_NAME, 0);
    }

    public void LeaveChannel()
    {
        _rtcEngine.LeaveChannel();
        DestroyVideoView(CHANNEL_NAME, 0);
    }

    void EngineOnUserJoinedHandler(uint uid, int elapsed)
    {
        _logger.UpdateLog(string.Format("OnUserJoinedHandler channelId: {0} uid: ${1} elapsed: ${2}", CHANNEL_NAME,
            uid, elapsed));
        makeVideoView(CHANNEL_NAME, uid);
        remoteClientIDs.Add(uid);
    }

    void OnCameraChangedHandler(string state, string device)
    {
        _logger.UpdateLog(string.Format("OnCameraChanged state: {0} device: {1}", state, device));
        GetVideoDeviceManager();
    }

    void OnMicrophoneChangedHandler(string state, string device)
    {
        _logger.UpdateLog(string.Format("OnMicrophoneChanged state: {0} device: {1}", state, device));
        GetAudioRecordingDevice();
    }

    void OnPlaybackChangedHandler(string state, string device)
    {
        _logger.UpdateLog(string.Format("OnPlaybackChanged state: {0} device: {1}", state, device));
        GetAudioPlaybackDevice();
    }

    void OnDestroy()
    {
        Debug.Log("OnApplicationQuit");
        if (_rtcEngine != null)
        {
            LeaveChannel();
            _rtcEngine.DisableVideo();
            _rtcEngine.DisableVideoObserver();
            IRtcEngine.Destroy();
        }
    }

    void OnJoinChannelSuccessHandler(string channelName, uint uid, int elapsed)
    {
        _logger.UpdateLog(string.Format("sdk version: {0}", IRtcEngine.GetSdkVersion()));
        _logger.UpdateLog(string.Format("onJoinChannelSuccess channelName: {0}, uid: {1}, elapsed: {2}", channelName, uid, elapsed));
        joinedChannel = true;
        if (!previewing)
        {
            //GetVideoDeviceManager();
            //_rtcEngine.StopPreview();
            //SetAndReleaseVideoDevice();
        }
        else
        {
            //DestroyVideoView(CHANNEL_NAME, uid);
            //_rtcEngine.StopPreview();
            //SetAndReleaseVideoDevice();
        }
        //SetCurrentDeviceVolume();
        //ReleaseDeviceManager();
    }

    void OnLeaveChannelHandler(RtcStats stats)
    {
        _logger.UpdateLog("OnLeaveChannelSuccess");
        DestroyVideoView(CHANNEL_NAME, 0);
        joinedChannel = false;
    }

    void OnUserLeaveChannelHandler(uint uid, USER_OFFLINE_REASON reason)
    {
        DestroyVideoView(CHANNEL_NAME, uid);
    }

    void OnSDKWarningHandler(int warn, string msg)
    {
        _logger.UpdateLog(string.Format("OnSDKWarning warn: {0}, msg: {1}", warn, msg));
    }

    void OnSDKErrorHandler(int error, string msg)
    {
        _logger.UpdateLog(string.Format("OnSDKError error: {0}, msg: {1}", error, msg));
    }

    void OnConnectionLostHandler()
    {
        _logger.UpdateLog(string.Format("OnConnectionLost "));
    }

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
            videoSurface.SetForUser(uid);
            videoSurface.SetEnable(true);
            videoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
            // make the object draggable
            //videoSurface.gameObject.AddComponent<UIElementDragger>();

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

