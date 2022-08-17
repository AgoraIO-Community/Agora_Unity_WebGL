using System.Collections.Generic;
using UnityEngine;
using System;

namespace agora_gaming_rtc
{

#if UNITY_WEBGL || UNITY_EDITOR
    /*
     * Data item used by DataBuilder
     */
    public class WGL_DataSet
    {
        public string key = "";
        public string value = "";
        public string subKey1 = "";
        public string subKey2 = "";
        public string subKey3 = "";
        public string subKey4 = "";

        public uint getUintValue()
        {
            return uint.Parse(value);
        }

        public int getIntValue()
        {
            return int.Parse(value);
        }

        public ushort getUshortValue()
        {
            return ushort.Parse(value);
        }

    }

    /*
     * Data builder for building custom data from strings received from webgl SendMessage
     */
    public class WGL_DataBuilder
    {
        public List<WGL_DataSet> _list = new List<WGL_DataSet>();

        public void clear()
        {
            _list.Clear();
            _list.TrimExcess();
        }

        public WGL_DataSet getData(string key)
        {
            foreach (WGL_DataSet ds in _list)
            {
                if (ds.key == key)
                {
                    return ds;
                }
            }

            return null;
        }

        public void add(string key, string value)
        {
            WGL_DataSet ds = new WGL_DataSet();
            ds.key = key;
            ds.value = value;

            // check for sub tokens in key
            // in case of specialized keys
            string[] stokens = ds.key.Split('_');
            if (stokens.Length > 0)
            {
                ds.subKey1 = stokens[0];
                if (stokens.Length > 1)
                {
                    ds.subKey2 = stokens[1];
                }

                if (stokens.Length > 2)
                {
                    ds.subKey3 = stokens[2];
                }

                if (stokens.Length > 4)
                {
                    ds.subKey4 = stokens[4];
                }
            }

            _list.Add(ds);
        }

    }

    /*
     * Data item for cache manager to store devices information
     */
    public class MediaDeviceInfo
    {
        public string deviceId;
        public string kind;
        public string groupId;
        public string label;
    }

    /**
     * stores media devices in separate listing
     * stores playback, recording and camera Devices
     */
    public class CacheManagerWebGL
    {
        // caching playback devices
        List<MediaDeviceInfo> _playbackDevicesList = new List<MediaDeviceInfo>();

        // caching recording devices
        List<MediaDeviceInfo> _recordingDevicesList = new List<MediaDeviceInfo>();

        // caching camera listing
        List<MediaDeviceInfo> _cameraDevicesList = new List<MediaDeviceInfo>();
        string currentVideoDevice = "";
        string currentAudioDevice = "";
        string currentPlayBackDevice = "";

        public void setCurrentAudioDevice(string deviceID)
        {
            currentAudioDevice = deviceID;
        }

        public string getCurrentAudioDevice()
        {
            return currentAudioDevice;
        }

        public void setCurrentPlaybackDevice(string deviceID)
        {
            currentPlayBackDevice = deviceID;
        }

        public string getCurrentPlaybackDevice()
        {
            return currentPlayBackDevice;
        }

        public MediaDeviceInfo getPlaybackDeviceByDeviceId(string deviceId)
        {
            foreach (MediaDeviceInfo info in _playbackDevicesList)
            {
                if (info.deviceId == deviceId)
                {
                    return info;
                }
            }

            return null;
        }

        // returns a mediadevice of recording based on deviceID
        public MediaDeviceInfo getAudioDeviceByDeviceId(string deviceId)
        {
            foreach (MediaDeviceInfo info in _recordingDevicesList)
            {
                if (info.deviceId == deviceId)
                {
                    return info;
                }
            }

            return null;
        }


        public void setCurrentVideoDevice(string deviceID)
        {
            currentVideoDevice = deviceID;
        }

        public string getCurrentVideoDevice()
        {
            return currentVideoDevice;
        }

        public int GetPlayBackDeviceCount()
        {
            return _playbackDevicesList.Count;
        }

        public void GetPlaybackAudioDevice(int index, ref string deviceName, ref string deviceId)
        {
            if (index < _playbackDevicesList.Count)
            {
                deviceName = _playbackDevicesList[index].label;
                deviceId = _playbackDevicesList[index].deviceId;
            }
        }

        public int GetCameraDeviceCount()
        {
            return _cameraDevicesList.Count;
        }

        public List<MediaDeviceInfo> GetCachedCameras()
        {
            return _cameraDevicesList;
        }

        public void GetVideoDevice(int index, ref string deviceName, ref string deviceId)
        {
            if (index < _cameraDevicesList.Count)
            {
                deviceName = _cameraDevicesList[index].label;
                deviceId = _cameraDevicesList[index].deviceId;
            }
        }

        public List<MediaDeviceInfo> GetCachedRecordingDevices()
        {
            return _recordingDevicesList;
        }

        public int GetRecordingDeviceCount()
        {
            return _recordingDevicesList.Count;
        }

        public void GetRecordingAudioDevice(int index, ref string deviceName, ref string deviceId)
        {
            if (index < _recordingDevicesList.Count)
            {
                deviceName = _recordingDevicesList[index].label;
                deviceId = _recordingDevicesList[index].deviceId;
            }
        }

        public void ProcessRecordingDevices(string listing)
        {
            _recordingDevicesList.Clear();
            _recordingDevicesList.TrimExcess();

            if (listing.Length > 10)
            {
                string[] list1 = listing.Split('|');
                foreach (string s in list1)
                {
                    string[] item = s.Split(',');
                    MediaDeviceInfo mdi = new MediaDeviceInfo();
                    mdi.deviceId = item[0];
                    mdi.kind = item[1];
                    mdi.label = item[2];
                    mdi.groupId = item[3];
                    _recordingDevicesList.Add(mdi);
                }
            }
        }

        public void ProcessPlayBackDevices(string listing)
        {
            _playbackDevicesList.Clear();
            _playbackDevicesList.TrimExcess();

            if (listing.Length > 10)
            {
                string[] list1 = listing.Split('|');
                foreach (string s in list1)
                {
                    string[] item = s.Split(',');

                    MediaDeviceInfo mdi = new MediaDeviceInfo();
                    mdi.deviceId = item[0];
                    mdi.kind = item[1];
                    mdi.label = item[2];
                    mdi.groupId = item[3];
                    _playbackDevicesList.Add(mdi);
                }
            }


        }

        public void ProcessCameraDevices(string listing)
        {
            _cameraDevicesList.Clear();
            _cameraDevicesList.TrimExcess();

            if (listing.Length > 10)
            {
                string[] list1 = listing.Split('|');
                foreach (string s in list1)
                {
                    string[] item = s.Split(',');

                    MediaDeviceInfo mdi = new MediaDeviceInfo();
                    mdi.deviceId = item[0];
                    mdi.kind = item[1];
                    mdi.label = item[2];
                    mdi.groupId = item[3];
                    _cameraDevicesList.Add(mdi);
                }
            }

        }

    }

    public class AgoraWebGLEventHandler : MonoBehaviour
    {
        private static AgoraWebGLEventHandler _webglEventHandlerInstance = null; // singleton instance
        private WGL_DataBuilder dataBuilder = new WGL_DataBuilder();
        private CacheManagerWebGL _cacheManager = new CacheManagerWebGL(); // storage for media devices
        public Dictionary<string, string> ErrorDescription = new Dictionary<string, string>(); // collection of errors for web
        public Dictionary<string, AgoraChannel> _clientsList = new Dictionary<string, AgoraChannel>(); // clientlist for multi channel

        // for video raw data manager
        //public delegate void OnRenderVideoFrameHandler(uint uid, VideoFrame videoFrame);
        //private OnRenderVideoFrameHandler OnRenderVideoFrame;

        void Awake()
        {
            if (_webglEventHandlerInstance == null)
            {
                _webglEventHandlerInstance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }

            PrepareErrors();

        }

        private VideoRawDataManager _rawD;

        public static void SetOnRenderVideoFrameCallback(VideoRawDataManager rawD)
        {
            if (rawD == null)
            {
                GetInstance()._rawD = null;
            }
            else
            {
                if (GetInstance().nativeTexture == null)
                {
                    GetInstance().nativeTexture = new Texture2D(200, 200, TextureFormat.ARGB32, false);
                    GetInstance().nativeTexture.wrapMode = TextureWrapMode.Clamp;
                    GetInstance().nativeTexture.Apply(false, false);
                }

                GetInstance()._rawD = rawD;

            }
        }

        private VideoRawDataManager _rawDOnCapture;

        public static void SetOnCaptureVideoFrameCallback(VideoRawDataManager rawD)
        {
            if (rawD == null)
            {
                GetInstance()._rawDOnCapture = null;
            }
            else
            {
                if (GetInstance().nativeTexture == null)
                {
                    GetInstance().nativeTexture = new Texture2D(200, 200, TextureFormat.ARGB32, false);
                    GetInstance().nativeTexture.wrapMode = TextureWrapMode.Clamp;
                    GetInstance().nativeTexture.Apply(false, false);
                }

                GetInstance()._rawDOnCapture = rawD;

            }
        }



        // MULTI CLIENT MANAGEMENT
        public static void AddClient(string channelId, AgoraChannel channel)
        {
            //GetInstance()._clientsList.Add(channelId, channel);
            GetInstance()._clientsList[channelId] = channel;
        }

        // encapsulate all video thing in one object
        // and then get this object in code for info

        public static void GetVideoDevice(int index, ref string deviceName, ref string deviceId)
        {
            GetInstance()._cacheManager.GetVideoDevice(index, ref deviceName, ref deviceId);
        }

        public static string GetErrorDescription(string code)
        {
            if (GetInstance().ErrorDescription.ContainsKey(code))
            {
                string value = GetInstance().ErrorDescription[code];
                return value;
            }

            return "<ERROR_NO_SUCH_CODE_EXISTS>";
        }

        public static CacheManagerWebGL GetCacheManager()
        {
            return GetInstance()._cacheManager;
        }

        public static string GetCurrentVideoDevice()
        {
            return GetInstance()._cacheManager.getCurrentVideoDevice();
        }

        public static AgoraWebGLEventHandler GetInstance()
        {
            return _webglEventHandlerInstance;
        }

        public static int GetPlaybackDeviceCount()
        {
            return GetInstance()._cacheManager.GetPlayBackDeviceCount();
        }

        public static int GetCameraDeviceCount()
        {
            return GetInstance()._cacheManager.GetCameraDeviceCount();
        }

        public static int GetAudioRecordingDeviceCount()
        {
            return 0;
        }

        public static List<MediaDeviceInfo> GetCachedCameras()
        {
            return GetInstance()._cacheManager.GetCachedCameras();
        }


        public void onJoinChannelSuccess_MC(string eventData)
        {
            string[] events = eventData.Split('|');
            string channel = events[0];
            string userId = events[1];

            if (GetInstance()._clientsList.ContainsKey(channel))
            {
                AgoraChannel ch = GetInstance()._clientsList[channel];
                if (ch.ChannelOnJoinChannelSuccess != null)
                {
                    ch.ChannelOnJoinChannelSuccess(channel, uint.Parse(userId), 0);
                }
            }
        }

        // called from web
        public void onJoinChannelSuccess(string eventData)
        {
            string[] events = eventData.Split('|');

            string channel = events[0];
            string userId = events[1];

            agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
            if (engine.OnJoinChannelSuccess != null)
            {
                engine.OnJoinChannelSuccess(channel, uint.Parse(userId), 1);
            }
        }

        public void onScreenShareStarted(string eventData)
        {
            string[] events = eventData.Split('|');

            string channel = events[0];
            string userId = events[1];

            agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
            if (engine.OnScreenShareStarted != null)
            {
                engine.OnScreenShareStarted(channel, uint.Parse(userId), 1);
            }
        }

        public void onScreenShareStopped(string eventData)
        {
            string[] events = eventData.Split('|');

            string channel = events[0];
            string userId = events[1];

            agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
            if (engine.OnScreenShareStopped != null)
            {
                engine.OnScreenShareStopped(channel, uint.Parse(userId), 1);
            }
        }

        public void onScreenShareCanceled(string eventData)
        {
            string[] events = eventData.Split('|');

            string channel = events[0];
            string userId = events[1];

            agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
            if (engine.OnScreenShareCanceled != null)
            {
                engine.OnScreenShareCanceled(channel, uint.Parse(userId), 1);
            }
        }

        public void onScreenShareStarted_MC(string eventData)
        {
            string[] events = eventData.Split('|');
            string channel = events[0];
            string userId = events[1];

            if (GetInstance()._clientsList.ContainsKey(channel))
            {
                AgoraChannel ch = GetInstance()._clientsList[channel];
                if (ch.ChannelOnScreenShareStarted != null)
                {
                    ch.ChannelOnScreenShareStarted(channel, uint.Parse(userId), 0);
                }
            }
        }

        public void onScreenShareStopped_MC(string eventData)
        {
            string[] events = eventData.Split('|');
            string channel = events[0];
            string userId = events[1];

            if (GetInstance()._clientsList.ContainsKey(channel))
            {
                AgoraChannel ch = GetInstance()._clientsList[channel];
                if (ch.ChannelOnScreenShareStopped != null)
                {
                    ch.ChannelOnScreenShareStopped(channel, uint.Parse(userId), 0);
                }
            }
        }

        public void onScreenShareCanceled_MC(string eventData)
        {
            string[] events = eventData.Split('|');
            string channel = events[0];
            string userId = events[1];

            if (GetInstance()._clientsList.ContainsKey(channel))
            {
                AgoraChannel ch = GetInstance()._clientsList[channel];
                if (ch.ChannelOnScreenShareCanceled != null)
                {
                    ch.ChannelOnScreenShareCanceled(channel, uint.Parse(userId), 0);
                }
            }
        }

        public void onRemoteUserJoined(string userId)
        {
            _remoteUserListing[uint.Parse(userId)] = uint.Parse(userId);

            agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
            if (engine.OnUserJoined != null)
            {
                engine.OnUserJoined(uint.Parse(userId), 1);
            }
        }

        public void onRemoteUserLeaved(string eventData)
        {
            string[] events = eventData.Split('|');
            if (events.Length < 2)
            {
                Debug.LogError("Unexpected value for onRemoteUserLeaved:" + eventData);
                return;
            }
            uint uid = uint.Parse(events[0]);
            _remoteUserListing.Remove(uid);

            USER_OFFLINE_REASON reason = (USER_OFFLINE_REASON)int.Parse(events[1]);

            agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
            if (engine.OnUserOffline != null)
            {
                engine.OnUserOffline(uid, reason);
            }
        }

        public void onRemoteUserMuted(string eventData)
        {
            string[] events = eventData.Split('|');
            if (events.Length < 3)
            {
                Debug.LogError("Unexpected value for onRemoteUserMuted:" + eventData);
                return;
            }

            uint userId = uint.Parse(events[0]);
            string mediaType = events[1];
            bool muted = events[2].Equals("1");
            agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
            if (engine == null) return;
            if (mediaType == "video" && engine.OnUserMuteVideo != null)
            {
                engine.OnUserMuteVideo(userId, muted);
            }
            else if (mediaType == "audio" && engine.OnUserMutedAudio != null)
            {
                engine.OnUserMutedAudio(userId, muted);
            }
        }

        public void OnLeaveChannel(string statData)
        {
            BuildData(statData);

            RtcStats stat = new RtcStats();

            WGL_DataSet ds = dataBuilder.getData("UserCount");
            if (ds != null)
            {
                stat.userCount = ds.getUintValue();
            }

            ds = dataBuilder.getData("Duration");
            if (ds != null)
            {
                stat.duration = ds.getUintValue();
            }

            ds = dataBuilder.getData("RecvBitrate");
            if (ds != null)
            {
                stat.rxKBitRate = ds.getUintValue();
            }

            ds = dataBuilder.getData("SendBitrate");
            if (ds != null)
            {
                stat.txKBitRate = ds.getUintValue();
            }

            ds = dataBuilder.getData("RecvBytes");
            if (ds != null)
            {
                stat.rxBytes = ds.getUintValue();
            }

            ds = dataBuilder.getData("SendBytes");
            if (ds != null)
            {
                stat.txBytes = ds.getUintValue();
            }

            ds = dataBuilder.getData("RTT");
            if (ds != null)
            {
                stat.gatewayRtt = ds.getIntValue();
            }

            ds = dataBuilder.getData("a_sendBitrate");
            if (ds != null)
            {
                stat.txAudioKBitRate = ds.getUintValue();
            }

            ds = dataBuilder.getData("a_sendBytes");
            if (ds != null)
            {
                stat.txAudioBytes = ds.getUintValue();
            }

            ds = dataBuilder.getData("a_sendPacketsLost");
            if (ds != null)
            {
                stat.txPacketLossRate = ds.getUshortValue();
            }


            agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();

            try
            {
                if (engine.OnLeaveChannel != null)
                {
                    engine.OnLeaveChannel(stat);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("exception " + e.Message);
            }

        }

        private Dictionary<uint, uint> _remoteUserListing = new Dictionary<uint, uint>();

        public void onChannelOnUserJoined_MC(string eventData)
        {
            string[] events = eventData.Split('|');

            string channel = events[0];
            string userId = events[1];

            if (GetInstance()._clientsList.ContainsKey(channel))
            {
                AgoraChannel ch = GetInstance()._clientsList[channel];
                uint uid = uint.Parse(userId);
                if(ch.ChannelOnUserJoined != null) {
                    ch.ChannelOnUserJoined(channel, uid, 0);
                }
            }
        }


        public void onChannelOnUserLeft_MC(string eventData)
        {
            string[] events = eventData.Split('|');

            string channel = events[0];
            string userId = events[1];

            if (GetInstance()._clientsList.ContainsKey(channel))
            {
                AgoraChannel ch = GetInstance()._clientsList[channel];
                uint uid = uint.Parse(userId);
                // TODO: consider more reasons parsed from the Web side
                if (ch.ChannelOnUserOffLine != null)
                {
                    ch.ChannelOnUserOffLine(channel, uid, USER_OFFLINE_REASON.QUIT);
                }
            }
        }

        public void onChannelOnUserPublished_MC(string eventData)
        {
            string[] events = eventData.Split('|');

            string channel = events[0];
            string userId = events[1];

            if (GetInstance()._clientsList.ContainsKey(channel))
            {
                AgoraChannel ch = GetInstance()._clientsList[channel];
                RemoteVideoStats remoteStats = new RemoteVideoStats();
                remoteStats.receivedBitrate = 1024; // to make it visible, put more than 0
                remoteStats.uid = uint.Parse(userId);
                if (ch.ChannelOnRemoteVideoStats != null)
                {
                    ch.ChannelOnRemoteVideoStats(channel, remoteStats);
                }
            }
        }


        public void onChannelOnUserUnPublished_MC(string eventData)
        {
            string[] events = eventData.Split('|');

            string channel = events[0];
            string userId = events[1];

            if (GetInstance()._clientsList.ContainsKey(channel))
            {
                AgoraChannel ch = GetInstance()._clientsList[channel];
                RemoteVideoStats remoteStats = new RemoteVideoStats();
                remoteStats.receivedBitrate = 0; // to make it visible, put more than 0
                remoteStats.uid = uint.Parse(userId);
                if (ch.ChannelOnRemoteVideoStats != null)
                {
                    ch.ChannelOnRemoteVideoStats(channel, remoteStats);
                }
            }
        }

        public void OnLeaveChannel_MC(string channel)
        {
            if (GetInstance()._clientsList.ContainsKey(channel))
            {
                RtcStats stat = new RtcStats();
                AgoraChannel ch = GetInstance()._clientsList[channel];
                if (ch.ChannelOnLeaveChannel != null)
                {
                    ch.ChannelOnLeaveChannel(channel, stat);
                }
            }
        }

        public void ChannelOnTranscodingUpdated(string channel)
        {
            if (GetInstance()._clientsList.ContainsKey(channel))
            {
                AgoraChannel ch = GetInstance()._clientsList[channel];
                ch.ChannelOnTranscodingUpdated(channel);
            }
        }

        // Audio frame handler
        public void HandleFrameHandler(string audioData)
        {
            Debug.Log(audioData);
            // TODO: this is where we need to encode data received to proper byte[] array
        }


        public void HandleChannelError(string eventData)
        {
            string[] events = eventData.Split('|');

            
            string channel = events[0];
            string errCode = events[1];
            string msg = events[2];
            int results = 0;
            int.TryParse(errCode, out results);
            

            if (GetInstance()._clientsList.ContainsKey(channel))
            {
                AgoraChannel ch = GetInstance()._clientsList[channel];
                if (ch.ChannelOnError != null)
                {
                    ch.ChannelOnError(channel, results, msg);
                }
            }
        }

        public void HandleUserError(string eventData)
        {
            string[] events = eventData.Split('|');
            
            string errCode = events[0];
            string msg = events[1];
            Debug.Log(errCode);
            Debug.Log(msg);
            int results = 0;
            int.TryParse(errCode, out results);
            agora_gaming_rtc.IRtcEngine engine = IRtcEngine.QueryEngine();
            if (engine != null)
            {
                
                if (engine.OnError != null)
                {
                    engine.OnError(results, msg);
                }
            }
        }

        public void ChannelOnClientRoleChanged(string eventData)
        {
            string[] events = eventData.Split('|');

            string channel = events[0];
            string oldRole_s = events[1];
            string newRole_s = events[2];

            if (GetInstance()._clientsList.ContainsKey(channel))
            {
                AgoraChannel ch = GetInstance()._clientsList[channel];

                CLIENT_ROLE_TYPE oldRole = CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE; // 2
                CLIENT_ROLE_TYPE newRole = CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER; // 1
                if (oldRole_s == "1")
                {
                    oldRole = CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER;
                }
                else
                {
                    oldRole = CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE;
                }

                if (newRole_s == "1")
                {
                    newRole = CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER;
                }
                else
                {
                    newRole = CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE;
                }
                if (ch.ChannelOnClientRoleChanged != null)
                {
                    ch.ChannelOnClientRoleChanged(channel, oldRole, newRole);
                }

            }
        }

        public void OnClientRoleChanged(string eventData)
        {
            string[] events = eventData.Split('|');

            string oldRole_s = events[0];
            string newRole_s = events[1];

            IRtcEngine engine = IRtcEngine.QueryEngine();
            if (engine == null) return;

            CLIENT_ROLE_TYPE oldRole = CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE; // 2
            CLIENT_ROLE_TYPE newRole = CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER; // 1
            if (oldRole_s == "1")
            {
                oldRole = CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER;
            }
            else
            {
                oldRole = CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE;
            }

            if (newRole_s == "1")
            {
                newRole = CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER;
            }
            else
            {
                newRole = CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE;
            }
            if (engine.OnClientRoleChanged != null)
            {
                engine.OnClientRoleChanged(oldRole, newRole);
            }

        }

        public void ClientOnVideoSizeChanged(string eventData)
        {
            string[] events = eventData.Split('|');

            try
            {
                uint uid = uint.Parse(events[0]);
                int width = int.Parse(events[1]);
                int height = int.Parse(events[2]);

                IRtcEngine engine = IRtcEngine.QueryEngine();
                if (engine == null) return;
                if (engine.OnVideoSizeChanged != null)
                {
                    engine.OnVideoSizeChanged(uid, width, height, 0);
                }

            }
            catch
            {
                Debug.LogWarning("Error processing ClientOnVideoSizeChanged:" + eventData);
            }
        }

        public void ChannelOnVideoSizeChanged(string eventData)
        {
            string[] events = eventData.Split('|');
            
            try
            {
                uint uid = uint.Parse(events[0]);

                int width = int.Parse(events[1]);
                int height = int.Parse(events[2]);
                string channel = events[3];
                AgoraChannel myChannel = GetInstance()._clientsList[channel];
                
                if (myChannel == null) return;
                if (myChannel.ChannelOnVideoSizeChanged != null)
                {
                    myChannel.ChannelOnVideoSizeChanged(channel, uid, width, height, 0);
                }

            }
            catch
            {
                Debug.LogWarning("Error processing ChannelOnVideoSizeChanged:" + eventData);
            }
        }

        #region Testing functions, remove later

        public void CustomMsg(string msg)
        {
            //PopupGUI gi = new PopupGUI();
            //gi.msg = msg;
            //_msgs.Add(gi);
        }

        // used for fetching raw data
        private RawDataFetcher _rawDataFetcher = new RawDataFetcher();
        private Texture2D nativeTexture = null; // for raw data

        void Update()
        {
            /*foreach (PopupGUI gi in _msgs)
            {
                if (gi.isExpired == false)
                {
                    gi.Update();
                }
            }*/
            // if video raw data manager is assigned
            if (_rawD != null)
            {
                foreach (var key in _remoteUserListing.Keys)
                {
                    _rawDataFetcher.UpdateRemoteTexture(key, nativeTexture);
                    var data = nativeTexture.GetRawTextureData();
                    _rawD.RaiseEvent_OnRender(key, data);
                }
            }

            if (_rawDOnCapture != null)
            {
                _rawDataFetcher.UpdateTexture(nativeTexture);
                var data = nativeTexture.GetRawTextureData();
                _rawDOnCapture.RaiseEvent_OnCapture(data);
            }

        }


        #endregion

        // server expires token after some time
        // you need to call setToken again otherwise server will disconnect
        public void tokenPrivilegeWillExpire(string token)
        {
            agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
            if (engine.OnTokenPrivilegeWillExpire != null)
            {
                engine.OnTokenPrivilegeWillExpire(token);
            }

        }

        // server expires token after some time
        // you need to call setToken again otherwise server will disconnect
        public void tokenPrivilegeDidExpire(string token)
        {
            agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
            if (engine.OnTokenPrivilegeDidExpire != null)
            {
                engine.OnTokenPrivilegeDidExpire(token);
            }

        }

        // server expires token after some time
        // you need to call setToken again otherwise server will disconnect
        public void channelTokenPrivilegeWillExpire(string eventData)
        {
            string[] events = eventData.Split('|');
            string channel = events[0];
            string token = events[1];
           
            Debug.Log(GetInstance()._clientsList.ContainsKey(channel));
            if (GetInstance()._clientsList.ContainsKey(channel))
            {
                AgoraChannel ch = GetInstance()._clientsList[channel];
                
                if (ch.ChannelOnTokenPrivilegeWillExpire != null)
                {
                    
                    ch.ChannelOnTokenPrivilegeWillExpire(channel, token);
                }
            }

        }

        // server expires token after some time
        // you need to call setToken again otherwise server will disconnect
        public void channelTokenPrivilegeDidExpire(string eventData)
        {
            string[] events = eventData.Split('|');
            string channel = events[0];
            string token = events[1];

            if (GetInstance()._clientsList.ContainsKey(channel))
            {
                AgoraChannel ch = GetInstance()._clientsList[channel];
                if (ch.ChannelOnTokenPrivilegeDidExpire != null)
                {
                    ch.ChannelOnTokenPrivilegeDidExpire(channel, token);
                }
            }

        }

        // sending to OnAudioVolumeIndicationCallback(string volumeInfo, int speakerNumber, int totalVolume)
        // volumn info is <uid volume vad channel>* 
        // in native, it seems the speaker number is always 1, even with multiple people in the channel
        // vad is not used for WebGL, so it is hardcoded to 0 
        // WebGL will pass back  <uid volume vad channel>* | speakerNumber | totalVolume
        public void OnVolumeIndication(string data)
        {
            string[] events = data.Split('|');
            IRtcEngine.OnAudioVolumeIndicationCallback(events[0], int.Parse(events[1]), int.Parse(events[2]));
        }

        public void refreshLocalCamera()
        {
            //agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
            //engine.OnActiveSpeaker(1); // for testing using this one
        }

        public void onRecordingDevicesListing(string listing)
        {
            _cacheManager.ProcessRecordingDevices(listing);
        }

        public void onPlaybackDevicesListing(string listing)
        {
            _cacheManager.ProcessPlayBackDevices(listing);
        }

        public void onCamerasListing(string listing)
        {
            _cacheManager.ProcessCameraDevices(listing);
        }

        public void OnCurrentChanges(string msg)
        {
            string[] st = msg.Split('=');
            if (st[0] == "GetCurrentVideoDevice")
            {
                _cacheManager.setCurrentVideoDevice(st[1]);
            }
            else if (st[0] == "GetCurrentAudioDevice")
            {
                _cacheManager.setCurrentAudioDevice(st[1]);
            }
            else if (st[0] == "GetCurrentPlayBackDevice")
            {
                _cacheManager.setCurrentPlaybackDevice(st[1]);
            }
        }

        void BuildData(string inStr)
        {
            dataBuilder.clear();
            if (inStr.Length > 20)
            {
                string[] list1 = inStr.Split('|');
                foreach (string s in list1)
                {
                    string[] item = s.Split(';');
                    dataBuilder.add(item[0], item[1]);
                }
            }

        }

        void LastMileQuality(string data)
        {
            agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
            if (engine.OnLastmileQuality != null)
            {
                engine.OnLastmileQuality(int.Parse(data));
            }
        }

        void PrepareErrors()
        {
            ErrorDescription.Add("0", "No error occurs.");
            ErrorDescription.Add("1", "A general error occurs (no specified reason). Try calling the method again.");
            ErrorDescription.Add("2",
                "An invalid parameter is used. For example, the specified channel name includes illegal characters. Please reset the parameters.");
            ErrorDescription.Add("3",
                "The SDK is not ready due to one of the following reasons:RtcEngine fails to initialize.Please re - initialize RtcEngine.No user has joined the channel when the method is called.Please check your code logic.Users have not left the channel when the rate and complain methods are called.Please check your code logic.The audio module is disabled.The program is not complete.");
            ErrorDescription.Add("4",
                "	RtcEngine does not support the request due to one of the following reasons:The setBeautyEffectOptions method is called on devices running versions earlier than Android 4.4.Please check the Android version.The built -in encryption mode is incorrect or the SDK fails to load the external encryption library.Please check the encryption mode setting or reload the external encryption library.");
            ErrorDescription.Add("5",
                "The request is rejected due to one of the following reasons:RtcEngine fails to initialize.Please re - initialize RtcEngine.The channel name is set as the empty string  when creating the RtcChannel instance.Please reset the channel name.When the joinChannel method is called to join one of multiple channels, the specified channel name is already in use.Please reset the channel name.The joinChannel method in RtcEngine is called to join another channel after an RtcChannel instance has been created to join a channel and a stream has been published in the channel.A user whose role is not audience calls the switchChannel method.Ensure that the user role is audience before calling the switchChannel method.");
            ErrorDescription.Add("6", "The buffer size is insufficient to store the returned data.");
            ErrorDescription.Add("7",
                "A method is called before the initialization of RtcEngine. Ensure that the RtcEngineinstance is created and initialized before calling the method.");
            ErrorDescription.Add("9",
                "Permission to access is not granted. Check whether your app has access to the audio and video device.");
            ErrorDescription.Add("10",
                "A timeout occurs. Some API calls require the SDK to return the execution result. This error occurs if the SDK takes too long (more than 10 seconds) to return the result.");
            ErrorDescription.Add("17",
                "The request to join the channel is rejected. Typical causes include:The user is already in the channel and still calls a method(for example, joinChannel) to join the channel. Stop calling the method to clear this error.The user tries to join a channel during a call test.To join a channel, the call test must be ended by calling stopEchoTest.");
            ErrorDescription.Add("18",
                "The request to leave the channel is rejected. Typical causes include:The user has left the channel but still calls a method(for example, leaveChannel) to leave the channel. Stop calling the method to clear this error.The user is not in the channel. In this case, no extra operation is needed.");
            ErrorDescription.Add("19", "Resources are already in use.");
            ErrorDescription.Add("20",
                "The request is abandoned by the SDK, possibly because the request has been sent too frequently.");
            ErrorDescription.Add("21",
                "RtcEngine fails to initialize and has crashed because of specific Windows firewall settings.");
            ErrorDescription.Add("22",
                "The SDK fails to allocate resources because your app uses too many system resources or system resources are insufficient.");
            ErrorDescription.Add("101",
                "The specified App ID is invalid. Please rejoin the channel with a valid App ID.");
            ErrorDescription.Add("102",
                "The specified channel name is invalid, possibly because the data types of some parameters are incorrect. Please rejoin the channel with a valid channel name.");
            ErrorDescription.Add("103",
                "RtcEngine fails to get server resources in the specified region. Try another region when initializing RtcEngine.");
            ErrorDescription.Add("109",
                "The current token has expired. Please apply for a new token on the server and call renewToken.");
            ErrorDescription.Add("110",
                "The token is invalid due to one of the following reasons:App Certificate is enabled in Agora Console, but the code still uses App ID for authentication.Once App Certificate is enabled for a project, you must use token - based authentication.The uid used to generate the token is not the same as the one used to join the channel.");
            ErrorDescription.Add("111",
                "The network connection is interrupted. This error occurs when the SDK has connected to the server but lost connection for more than 4 seconds.");
            ErrorDescription.Add("112",
                "The network connection is lost. This error occurs when the connection is interrupted and the SDK cannot reconnect to the server within 10 seconds.");
            ErrorDescription.Add("113", "The user is not in the channel when calling the method.");
            ErrorDescription.Add("114", "The data size exceeds 1024 bytes when calling the sendStreamMessage method.");
            ErrorDescription.Add("115", "The data bitrate exceeds 6 Kbps when calling the sendStreamMessage method.");
            ErrorDescription.Add("116",
                "More than five data streams are created when calling the createDataStream method.");
            ErrorDescription.Add("117", "The data stream transmission times out.");
            ErrorDescription.Add("119", "Switching roles fails.");
            ErrorDescription.Add("120",
                "Decryption fails. The user may have entered an incorrect password to join the channel. Check the entered password or tell the user to try rejoining the channel.");
            ErrorDescription.Add("123",
                "The user is banned from the server. This error occurs when the user is kicked out of the channel from the server.");
            ErrorDescription.Add("134",
                "The user account is invalid, possibly because it contains invalid parameters.");

            ErrorDescription.Add("124", "Incorrect watermark file parameter.");
            ErrorDescription.Add("125", "Incorrect watermark file path.");
            ErrorDescription.Add("126",
                "Incorrect watermark file format. The SDK only supports adding PNG files as the watermark image.");
            ErrorDescription.Add("127", "Incorrect watermark file information.");
            ErrorDescription.Add("128", "Incorrect watermark file data format.");
            ErrorDescription.Add("129", "	An error occurs when reading the watermark file.");

            ErrorDescription.Add("130",
                "Stream encryption is enabled when the user calls the addPublishStreamUrl method. The SDK does not support pushing encrypted streams to CDN.");
            ErrorDescription.Add("151",
                "An error occurs when pushing streams to CDN. Please remove the current URL address by calling the removePublishStreamUrl method, and then add a new address by calling the addPublishStreamUrl method.");
            ErrorDescription.Add("152",
                "The host has published more than 10 URLs. Please delete the unnecessary URLs before adding new ones.");
            ErrorDescription.Add("153",
                "The host is making changes to other hosts' URLs, such as updating parameters and disabling a URL. Please check your app logic.");
            ErrorDescription.Add("154",
                "An error occurs on Agora's streaming server. Call the addPublishStreamUrl method to push the stream again.");
            ErrorDescription.Add("155", "The server fails to find the stream.");
            ErrorDescription.Add("156", "The URL format is incorrect. Please check the format.");

            ErrorDescription.Add("160",
                "The client is already recording audio. To start a new recording, call stopAudioRecording to stop the current recording first, and then call startAudioRecording.");
            ErrorDescription.Add("1005",
                "A general error occurs (no specified reason). Check whether the audio device is already in use by another app, or try rejoining the channel.");
            ErrorDescription.Add("1006",
                "An error occurs when using Java resources. Check whether the audio device storage is sufficient, or restart the audio device.");
            ErrorDescription.Add("1007", "The sampling frequency setting is incorrect.");
            ErrorDescription.Add("1008",
                "An error occurs when initializing the playback device. Check whether the playback device is already in use by another app, or try rejoining the channel.");
            ErrorDescription.Add("1009",
                "An error occurs when starting the playback device. Check the playback device, or try rejoining the channel.");
            ErrorDescription.Add("1010", "An error occurs when stopping the playback device.");
            ErrorDescription.Add("1011",
                "An error occurs when initializing the recording device. Check the recording device, or try rejoining the channel.");
            ErrorDescription.Add("1012",
                "An error occurs when starting the recording device. Check the recording device, or try rejoining the channel.");
            ErrorDescription.Add("1013", "An error occurs when stopping the recording device.");
            ErrorDescription.Add("1015",
                "A playback error occurs. Check the playback device, or try rejoining the channel.");
            ErrorDescription.Add("1017",
                "A recording error occurs. Check the recording device, or try rejoining the channel.");
            ErrorDescription.Add("1018", "Recording fails.");
            ErrorDescription.Add("1022", "An error occurs when initializing the loopback device.");
            ErrorDescription.Add("1023", "An error occurs when starting the loopback device.");
            ErrorDescription.Add("1027",
                "Permission to record is not granted. Check whether permission to record is granted or whether the recording device is already in use by another app.");
            ErrorDescription.Add("1033", "The recording device is already in use.");
            ErrorDescription.Add("1101",
                "An error occurs when using Java resources. Check whether the audio device storage is sufficient, or restart the audio device.");
            ErrorDescription.Add("1108",
                "The audio recording frequency is lower than 50 Hz. In this case, the frequency is often 0, which indicates that the recording has not started. Agora recommends that you check whether permission to record is granted.");
            ErrorDescription.Add("1109",
                "The audio playback frequency is lower than 50 Hz. In this case, the frequency is often 0, which indicates that the playback has not started. Agora recommends that you check whether too many AudioTrack instances have been created.");
            ErrorDescription.Add("1111",
                "Recording fails to start, and a ROM system error occurs. Possible solutions:Restart your app.Restart the device on which your app is running.Check whether permission to record is granted.");
            ErrorDescription.Add("1112",
                "Playback fails to start and a ROM system error occurs. Possible solutions:Restart the app.Restart the device on which your app is running.Check whether permission to record is granted.");
            ErrorDescription.Add("1115",
                "Recording fails. Check whether there is permission to record or whether there is a problem with the network connection.");
            ErrorDescription.Add("1201",
                "The current device does not support audio input, possibly because the configuration of the Audio Session category is incorrect, or because the device is already in use. Agora recommends terminating all background apps and rejoining the channel.");
            ErrorDescription.Add("1206", "Audio Session fails to launch. Check your recording settings.");
            ErrorDescription.Add("1210",
                "An error occurs when initializing the audio device, usually because some audio device parameters are incorrect.");
            ErrorDescription.Add("1213",
                "An error occurs when re-initializing the audio device, usually because some audio device parameters are incorrect.");
            ErrorDescription.Add("1214",
                "An error occurs when restarting the audio device, usually because the Audio Session category setting is not compatible with the audio device settings.");
            ErrorDescription.Add("1301",
                "The audio device module fails to initialize. Disable and re-enable the audio device, or restart the device on which your app is running.");
            ErrorDescription.Add("1303",
                "The audio device module fails to terminate. Disable and re-enable the audio device, or restart the device on which your app is running.");
            ErrorDescription.Add("1306",
                "The playback device fails to initialize. Disable and re-enable the audio device, or restart the device on which your app is running.");
            ErrorDescription.Add("1307",
                "Initialization fails because no audio playback device is available. Ensure that a proper audio device is connected.");
            ErrorDescription.Add("1309",
                "Recording fails to start. Disable and re-enable the audio device, or restart the device on which your app is running.");
            ErrorDescription.Add("1311",
                "The system fails to create a recording thread, possibly because the device storage or performance is insufficient. Restart the device or use a different one.");
            ErrorDescription.Add("1314",
                "Recording fails to start. Possible solutions:Disable and re - enable the audio device.Restart the device on which your app is running.Update the sound card driver.");
            ErrorDescription.Add("1319",
                "The system fails to create a playback thread, possibly because the device storage or performance is insufficient. Restart the device, or use a different one.");
            ErrorDescription.Add("1320",
                "Audio playback fails to start. Possible solutions:Disable and re - enable the audio device.Restart the device on which your app is running.Upgrade the sound card driver.");
            ErrorDescription.Add("1322",
                "No recording device is available. Ensure that a proper audio device is connected.");
            ErrorDescription.Add("1323",
                "No playback device is available. Ensure that a proper audio device is connected.");
            ErrorDescription.Add("1351",
                "The audio device module fails to initialize. Possible solutions:Disable and re - enable the audio device.Restart the device on which your app is running.Upgrade the sound card driver.");
            ErrorDescription.Add("1353",
                "The recording device fails to initialize. Possible solutions:Disable and re - enable the audio device.Restart the device on which your app is running.Upgrade the sound card driver.");
            ErrorDescription.Add("1354",
                "The microphone fails to initialize. Possible solutions:Disable and re - enable the audio device.Restart the device on which your app is running.Upgrade the sound card driver.");
            ErrorDescription.Add("1355",
                "The playback device fails to initialize. Possible solutions:Disable and re - enable the audio device.Restart the device on which your app is running.Upgrade the sound card driver.");
            ErrorDescription.Add("1356",
                "The speaker fails to initialize. Possible solutions:Disable and re - enable the audio device.Restart the device on which your app is running.Upgrade the audio card driver.");
            ErrorDescription.Add("1357",
                "Recording fails to start. Possible solutions:Disable and re - enable the audio device.Restart the device on which your app is running.Upgrade the sound card driver.");
            ErrorDescription.Add("1358",
                "Audio playback fails to start. Possible solutions:Disable and re - enable the audio device.Restart the device on which your app is running.Upgrade the sound card driver.");
            ErrorDescription.Add("1359",
                "No recording device is available. Check whether the recording device is connected or whether it is already in use by another app.");
            ErrorDescription.Add("1360",
                "No playback device is available. Check whether the playback device is connected or whether it is already in use by another app.");

            ErrorDescription.Add("1003",
                "The camera fails to start. Check whether the camera is already in use by another app, or try rejoining the channel.");
            ErrorDescription.Add("1004", "The video rendering module fails to start.");
            ErrorDescription.Add("1510",
                "Permission to access the camera is not granted. Check whether permission to access the camera permission is granted.");
            ErrorDescription.Add("1512", "The camera is already in use.");
            ErrorDescription.Add("1600", "An unknown error occurs.");
            ErrorDescription.Add("1601", "Video encoding initialization fails. Try rejoining the channel.");
            ErrorDescription.Add("1602", "Video encoding fails. Try rejoining the channel.");
            ErrorDescription.Add("1603", "Video encoding settings fail to be applied.");

            ErrorDescription.Add("157",
                "The extension library is not integrated, such as the library for enabling deep-learning noise reduction.");
            ErrorDescription.Add("1001", "The SDK fails to load the media engine.");
            ErrorDescription.Add("1002",
                "The SDK fails to start an audio/video call after launching the media engine. Try rejoining the channel.");

            ErrorDescription.Add("8",
                "The specified view is invalid. The video call function requires a specified view.");
            ErrorDescription.Add("16",
                "The SDK fails to initialize the video call function, possibly due to a lack of resources. When this warning occurs, users cannot make a video call, but the voice call function is not affected.");
            //		ErrorDescription.Add("20", "The request is pending, usually because some modules are not ready, causing the SDK to postpone processing the request.");
            //		ErrorDescription.Add("103", "No channel resources are available, possibly because the server fails to allocate channel resources.");
            ErrorDescription.Add("104",
                "A timeout occurs when the SDK is searching for a specified channel. When receiving a request to join a channel, the SDK searches for the channel first. This warning usually occurs when the network connection is too poor for the SDK to connect to the server.");
            ErrorDescription.Add("105",
                "The server rejects the request to search for the channel because the server cannot process the request or the request is illegal.");
            ErrorDescription.Add("106",
                "A timeout occurs when joining the channel. Once the specified channel is found, the SDK starts joining the channel. This warning usually occurs when the network connection is too poor for the SDK to connect to the server.");
            ErrorDescription.Add("107",
                "The server rejects the request to join the channel because the server cannot process this request or the request is illegal.");
            //		ErrorDescription.Add("111", "A timeout occurs when switching to the live video.");
            ErrorDescription.Add("118", "A timeout occurs when setting user roles in the live-streaming profile.");
            ErrorDescription.Add("121", "The ticket to join the channel is invalid.");
            ErrorDescription.Add("122", "The SDK is trying to connect to another server.");
            ErrorDescription.Add("131", "The channel connection cannot be recovered.");
            ErrorDescription.Add("132", "The IP address has changed.");
            ErrorDescription.Add("133", "The port has changed.");
            ErrorDescription.Add("701", "An error occurs when opening the audio-mixing file.");
            ErrorDescription.Add("1014", "A playback device warning occurs.");
            ErrorDescription.Add("1016", "A recording device warning occurs.");
            ErrorDescription.Add("1019",
                "No data is recorded. Possible solutions:Check whether the microphone is already in use.Check whether permission to record is granted.Check whether the recording device works properly.Restart the device.");
            ErrorDescription.Add("1020",
                "The audio playback frequency is abnormal due to high CPU usage. Recommended solutions:Close other apps that are consuming CPU resources.Check whether the audio module is enabled in your app.Try rejoining the channel.");
            ErrorDescription.Add("1021",
                "The audio recording frequency is abnormal due to high CPU usage. Recommended solutions:Close other apps that are consuming CPU resources.Check whether the audio module is enabled in your app.Try rejoining the channel.");
            ErrorDescription.Add("1025",
                "The audio playback or recording is interrupted by system events (such as a phone call).");
            ErrorDescription.Add("1029",
                "The Audio Session category is not set as AVAudioSessionCategoryPlayAndRecord.During a call, RtcEngine monitors the Audio Session category. If the category is modified, this warning occurs, and RtcEngine automatically sets it back to AVAudioSessionCategoryPlayAndRecord");
            ErrorDescription.Add("1031",
                "The recording volume is too low. Check whether the user's microphone is muted or whether the user has enabled microphone augmentation.");
            ErrorDescription.Add("1032",
                "The playback volume is too low. Check whether the user's microphone is muted or whether the user has enabled microphone augmentation.");
            //		ErrorDescription.Add("1033", "The recording device is already in use. Check whether permission to record is granted or whether another app is using the device.");
            ErrorDescription.Add("1040",
                "No audio data is available. Possible solutions:Disable and re - enable the audio device.Restart the device on which your app is running.Update the sound card driver.");
            ErrorDescription.Add("1051",
                "Audio feedback is detected during recording. Ensure that users in the same channel maintain sufficient physical distance between themselves.");
            ErrorDescription.Add("1052",
                "The system threads for recording and playback cannot be arranged due to high CPU usage.");
            ErrorDescription.Add("1053",
                "A residual echo is detected. This may be caused by the delayed scheduling of system threads or a signal overflow.");
            //		ErrorDescription.Add("1323", "No playback device is available. Ensure that a proper audio device is connected.");
            ErrorDescription.Add("1324",
                "The recording device is released improperly. Possible solutions:Disable and re - enable the audio device.Restart the device on which your app is running.Update the sound card driver.");
            ErrorDescription.Add("1610",
                "The original resolution of the remote video is beyond the range (640 × 480) where the super-resolution algorithm can be applied.");
            ErrorDescription.Add("1611", "Another user is using super resolution.");
            ErrorDescription.Add("1612", "The device does not support super resolution.");

            ErrorDescription.Add("UNEXPECTED_ERROR",
                "Unexpected error that the SDK cannot handle. In most cases, this error has a more detailed error message.");
            ErrorDescription.Add("UNEXPECTED_RESPONSE", "The server returns an unexpected response.");
            ErrorDescription.Add("INVALID_PARAMS", "Incorrect parameter.");
            ErrorDescription.Add("NOT_SUPPORTED", "Not supported by the browser.");
            ErrorDescription.Add("INVALID_OPERATION",
                "Illegal operation, usually because you cannot perform this operation in the current state.");
            ErrorDescription.Add("OPERATION_ABORTED",
                "The operation is aborted because the SDK fails to communicate with Agora servers due to poor network conditions or disconnection.");
            ErrorDescription.Add("WEB_SECURITY_RESTRICT", "Browser security policy restrictions.");

            ErrorDescription.Add("NETWORK_TIMEOUT",
                "Timeout because the SDK fails to communicate with Agora servers due to poor network conditions or disconnection.");
            ErrorDescription.Add("NETWORK_RESPONSE_ERROR", "An error occurs in the server response.");
            ErrorDescription.Add("NETWORK_ERROR", "Fails to locate the network error.");

            ErrorDescription.Add("WS_ABORT", "	WebSocket disconnected when requesting for the Agora servers.");
            ErrorDescription.Add("WS_DISCONNECT", "WebSocket disconnected before requesting for the Agora servers	");
            ErrorDescription.Add("WS_ERR", "WebSocket connection error.");

            ErrorDescription.Add("ENUMERATE_DEVICES_FAILED",
                "Fails to enumerate local devices, usually due to browser restrictions.");
            ErrorDescription.Add("DEVICE_NOT_FOUND", "Fails to find the specified device.");

            ErrorDescription.Add("TRACK_IS_DISABLED",
                "The track is disabled, usually because you have called Track.setEnabled(false) to disable the track.");
            ErrorDescription.Add("SHARE_AUDIO_NOT_ALLOWED", "The end user does not click Share Audio.");
            ErrorDescription.Add("CHROME_PLUGIN_NO_RESPONSE",
                "No response from Agora's Chrome Extension for Screen Sharing");
            ErrorDescription.Add("CHROME_PLUGIN_NOT_INSTALL",
                "Agora's Chrome Extension for Screen Sharing has not been installed.");
            ErrorDescription.Add("MEDIA_OPTION_INVALID", "Invalid parameter for media capturing");
            ErrorDescription.Add("CONSTRAINT_NOT_SATISFIED", "Invalid parameter for media capturing.");
            ErrorDescription.Add("PERMISSION_DENIED", "Fails to get access to the media device.");
            ErrorDescription.Add("FETCH_AUDIO_FILE_FAILED", "Fails to download the online audio file.");
            ErrorDescription.Add("READ_LOCAL_AUDIO_FILE_ERROR", "Fails to read the local audio file.");
            ErrorDescription.Add("DECODE_AUDIO_FILE_FAILED",
                "Fails to decode the audio file mainly because the codec of the audio file is not supported by WebAudio.");

            ErrorDescription.Add("UID_CONFLICT", "Duplicate UIDs in the channel.");
            ErrorDescription.Add("INVALID_UINT_UID_FROM_STRING_UID",
                "Agora's service for allocating a UID returns an illegal number for UID.");
            ErrorDescription.Add("CAN_NOT_GET_PROXY_SERVER",
                "Fails to get the IP addresses of the cloud proxy service.");
            ErrorDescription.Add("CAN_NOT_GET_GATEWAY_SERVER", "Fails to get the IP addresses of the Agora servers.");

            ErrorDescription.Add("INVALID_LOCAL_TRACK", "The LocalTrack object that you pass is illegal.");
            ErrorDescription.Add("CAN_NOT_PUBLISH_MULTIPLE_VIDEO_TRACKS",
                "Publishes multiple video tracks at the same time.");

            ErrorDescription.Add("INVALID_REMOTE_USER",
                "The remote user may not be in the channel or have not yet published any tracks.");
            ErrorDescription.Add("REMOTE_USER_IS_NOT_PUBLISHED",
                "The remote user has not yet published a track of the media type that you specify. For example, you try to subscribe to a video track, but the remote user only publishes an audio video.");

            ErrorDescription.Add("LIVE_STREAMING_TASK_CONFLICT", "The streaming task exists.");
            ErrorDescription.Add("LIVE_STREAMING_INVALID_ARGUMENT", "Incorrect streaming parameters.");
            ErrorDescription.Add("LIVE_STREAMING_INTERNAL_SERVER_ERROR",
                "An internal error occurs in the dedicated Agora server for pushing streams.");
            ErrorDescription.Add("LIVE_STREAMING_PUBLISH_STREAM_NOT_AUTHORIZED", "The target URL is occupied.");
            ErrorDescription.Add("LIVE_STREAMING_CDN_ERROR", "An error occurs in the target CDN.");
            ErrorDescription.Add("LIVE_STREAMING_INVALID_RAW_STREAM", "Timeout");

            ErrorDescription.Add("CROSS_CHANNEL_WAIT_STATUS_ERROR",
                "An error occurs when triggering the AgoraRTCClient.on(channel - media - relay - state) state.");
            ErrorDescription.Add("CROSS_CHANNEL_FAILED_JOIN_SRC", "Fails to send the relay request.");
            ErrorDescription.Add("CROSS_CHANNEL_FAILED_JOIN_DEST", "Fails to accept the relay request.");
            ErrorDescription.Add("CROSS_CHANNEL_FAILED_PACKET_SENT_TO_DEST",
                "The Agora server fails to receive the media stream.");
            ErrorDescription.Add("CROSS_CHANNEL_SERVER_ERROR_RESPONSE", "An error occurs in the server response.");
        }


        // Testing popup item, auto hides are some time
        class PopupGUI
        {
            float counter = 0;
            float ctime = 4;
            public bool isExpired = false;
            public string msg = "";

            public void Update()
            {
                counter += Time.deltaTime;
                if (counter > ctime)
                {
                    counter = 0;
                    isExpired = true;
                }

            }
        }


    }

    // same as InSurfaceRenderer for videosurface
    // used for observing video renderers in webgl
    public sealed class RawDataFetcher : IRtcEngineNative
    {

        public RawDataFetcher()
        {

        }

        public void UpdateTexture(Texture tex)
        {
            if (!isLocalVideoReady())
                return;
            if (tex != null)
            {
                updateLocalTexture(tex.GetNativeTexturePtr());
            }
        }

        public void UpdateRemoteTexture(uint uid, Texture tex)
        {
            if (!isRemoteVideoReady("" + uid))
                return;
            updateRemoteTexture("" + uid, tex.GetNativeTexturePtr());
        }

    }
#endif
}