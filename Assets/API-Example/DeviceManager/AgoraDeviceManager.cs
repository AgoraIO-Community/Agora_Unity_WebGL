using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using System.Linq;

public class AgoraDeviceManager : MonoBehaviour
{
    [SerializeField]
    private string APP_ID = "YOUR_APPID";
    [SerializeField]
    private string TOKEN = "";
    [SerializeField]
    private string CHANNEL_NAME = "YOUR_CHANNEL_NAME";

    public Text LogText;
    private Logger _logger;
    private IRtcEngine _rtcEngine = null;
    private AudioRecordingDeviceManager _audioRecordingDeviceManager = null;
    private AudioPlaybackDeviceManager _audioPlaybackDeviceManager = null;
    private VideoDeviceManager _videoDeviceManager = null;
    private Dictionary<int, string> _audioRecordingDeviceDic = new Dictionary<int, string>();
    private Dictionary<int, string> _audioPlaybackDeviceDic = new Dictionary<int, string>();
    private Dictionary<int, string> _videoDeviceManagerDic = new Dictionary<int, string>();
    public Dropdown videoDropdown, recordingDropdown, playbackDropdown;
    [SerializeField]
    private int _recordingDeviceIndex = 0,
    _playbackDeviceIndex = 0,
    _videoDeviceIndex = 0;
    private List<uint> remoteClientIDs;

    // Start is called before the first frame update
    void Start()
    {
        if (CheckAppId())
        {
            InitRtcEngine();
            JoinChannel();
        }
    }

    void Update() 
    {
        PermissionHelper.RequestMicrophontPermission();
    }

    bool CheckAppId()
    {
        _logger = new Logger(LogText);
        return _logger.DebugAssert(APP_ID.Length > 10, "Please fill in your appId in Canvas!!!!!");
    }

    public void playbackUpdate(){
        _playbackDeviceIndex = playbackDropdown.value;
        SetCurrentDevice();
        ReleaseDeviceManager();
    }

    public void recordingUpdate(){
        _playbackDeviceIndex = recordingDropdown.value;
        SetCurrentDevice();
        ReleaseDeviceManager();
    }

    public void videoUpdate(){
        _playbackDeviceIndex = videoDropdown.value;
        SetCurrentDevice();
        ReleaseDeviceManager();
    }

    void InitRtcEngine()
    {
        _rtcEngine = IRtcEngine.GetEngine(APP_ID);
        _rtcEngine.SetLogFile("log.txt");
        _rtcEngine.EnableAudio();
        _rtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);
        _rtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        _rtcEngine.OnJoinChannelSuccess += OnJoinChannelSuccessHandler;
        _rtcEngine.OnUserJoined += EngineOnUserJoinedHandler;
        _rtcEngine.OnLeaveChannel += OnLeaveChannelHandler;
        _rtcEngine.OnWarning += OnSDKWarningHandler;
        _rtcEngine.OnError += OnSDKErrorHandler;
        _rtcEngine.OnConnectionLost += OnConnectionLostHandler;
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
        for(int i = 0; i < count ; i ++) {
            _audioRecordingDeviceManager.GetAudioRecordingDevice(i, ref audioRecordingDeviceName, ref audioRecordingDeviceId);
            _audioRecordingDeviceDic.Add(i, audioRecordingDeviceId);
            _logger.UpdateLog(string.Format("AudioRecordingDevice device index: {0}, name: {1}, id: {2}", i, audioRecordingDeviceName, audioRecordingDeviceId));
        }

        recordingDropdown.AddOptions(_audioRecordingDeviceDic.Values.ToList());
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
        for(int i = 0; i < count ; i ++) {
            _audioPlaybackDeviceManager.GetAudioPlaybackDevice(i, ref audioPlaybackDeviceName, ref audioPlaybackDeviceId);
            _audioPlaybackDeviceDic.Add(i, audioPlaybackDeviceId);
            _logger.UpdateLog(string.Format("AudioPlaybackDevice device index: {0}, name: {1}, id: {2}", i, audioPlaybackDeviceName, audioPlaybackDeviceId));
        }

         playbackDropdown.AddOptions(_audioPlaybackDeviceDic.Values.ToList());
    }

    void GetVideoDeviceManager()
    {
        string videoDeviceName = "";
        string videoDeviceId = "";
		/// If you want to getVideoDeviceManager, you need to call startPreview() first;
		_rtcEngine.StartPreview();
        _videoDeviceManager = (VideoDeviceManager)_rtcEngine.GetVideoDeviceManager();
        _videoDeviceManager.CreateAVideoDeviceManager();
        int count = _videoDeviceManager.GetVideoDeviceCount();
        _logger.UpdateLog(string.Format("VideoDeviceManager count: {0}", count));
        videoDropdown.ClearOptions();
        for(int i = 0; i < count ; i ++) {
            _videoDeviceManager.GetVideoDevice(i, ref videoDeviceName, ref videoDeviceId);
            _videoDeviceManagerDic.Add(i, videoDeviceId);
            _logger.UpdateLog(string.Format("VideoDeviceManager device index: {0}, name: {1}, id: {2}", i, videoDeviceName, videoDeviceId));
        }
        videoDropdown.AddOptions(_videoDeviceManagerDic.Values.ToList());
    }

    void SetCurrentDevice()
    {
        _audioRecordingDeviceManager.SetAudioRecordingDevice(_audioRecordingDeviceDic[_recordingDeviceIndex]);
        _audioPlaybackDeviceManager.SetAudioPlaybackDevice(_audioPlaybackDeviceDic[_playbackDeviceIndex]);
        _videoDeviceManager.SetVideoDevice(_videoDeviceManagerDic[_videoDeviceIndex]);
    }

    void SetCurrentDeviceVolume()
    {
        _audioRecordingDeviceManager.SetAudioRecordingDeviceVolume(100);
        _audioPlaybackDeviceManager.SetAudioPlaybackDeviceVolume(100);
    }

    void ReleaseDeviceManager()
    {
        _audioPlaybackDeviceManager.ReleaseAAudioPlaybackDeviceManager();
        _audioRecordingDeviceManager.ReleaseAAudioRecordingDeviceManager();
        _videoDeviceManager.ReleaseAVideoDeviceManager();
    }

    void JoinChannel()
    {
        _rtcEngine.JoinChannelByKey(TOKEN, CHANNEL_NAME, "", 0);
    }

    void EngineOnUserJoinedHandler(uint uid, int elapsed)
    {
        _logger.UpdateLog(string.Format("OnUserJoinedHandler channelId: {0} uid: ${1} elapsed: ${2}", CHANNEL_NAME,
            uid, elapsed));
        remoteClientIDs.Add(uid);
    }

    void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        if (_rtcEngine != null)
        {
            IRtcEngine.Destroy();
        }
    }

    void OnJoinChannelSuccessHandler(string channelName, uint uid, int elapsed)
    {
        _logger.UpdateLog(string.Format("sdk version: {0}", IRtcEngine.GetSdkVersion()));
        _logger.UpdateLog(string.Format("onJoinChannelSuccess channelName: {0}, uid: {1}, elapsed: {2}", channelName, uid, elapsed));
        GetAudioPlaybackDevice();
        GetVideoDeviceManager();
        GetAudioRecordingDevice();
        //SetCurrentDevice();
        //SetCurrentDeviceVolume();
        //ReleaseDeviceManager();
    }

    void OnLeaveChannelHandler(RtcStats stats)
    {
        _logger.UpdateLog("OnLeaveChannelSuccess");
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
}

