using UnityEngine;
using agora_gaming_rtc;

namespace agora_gs_test
{
    public class ImplTester : MonoBehaviour
    {
        public TestHome helloUnityVideoObj;

        string msg1 = "";
        //string strFilePath = "https://web-demos-static.agora.io/agora/smlt.flac";		
        string strFilePath = "sample_mp3.mp3";
        [SerializeField] private Vector2 scrollPosition = Vector2.zero;
        [SerializeField] private string FBURL = "rtmps://live-api-s.facebook.com:443/rtmp/761881614728913?s_bl=1&s_psm=1&s_sc=761881671395574&s_sw=0&s_vt=api-s&a=AbxzSme10reUuODr";
        [SerializeField] private string audioEffectURL = "SET FROM INSPECTOR";
        string vdc = "";
        string adc = "";
        string cvdc = "";

        bool uploadLogStatus = false;

        string src_uid_str = "src uid :";
        string dest_uid_str = "dest uid :";
        string dest_uid_str2 = "dest uid 2:";
        int src_uid = 0, dest_uid = 0, dest_uid2 = 0;
        string src_token = "006b16bc9253a014a10a5c8d246601ff843IACchFus76wPyzDAuZkiyl4UZ6spRHnNiXKf/l/SYlV514H/KYQAAAAAEACkGjsKvQmVYAEAAQC9CZVg";
        string dest_token = "006b16bc9253a014a10a5c8d246601ff843IACMjVR0+7ZMsPRXylBh5BoS9Z7v0RTzQtntp3CURt3cisDOMp0AAAAAEACkGjsK4gmVYAEAAQDiCZVg";
        string dest_token2 = "006b16bc9253a014a10a5c8d246601ff843IACxev5HXS/QmIwjL5zWsJLkKN2d7zgP7v/6vwU/z3WJ8RNl5CEAAAAAEACkGjsKIzOVYAEAAQAjM5Vg";


        void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(300), GUILayout.Height(600));

            TestDrawAudio();

            TestDraw1();

            TestMuteStreams();

            TestAudioEffects();

            TestDualStreams();

            src_uid_str = GUILayout.TextField(src_uid_str);
            int.TryParse(src_uid_str, out src_uid);
            GUILayout.Label("unity3d");
            src_token = GUILayout.TextArea(src_token);

            dest_uid_str = GUILayout.TextField(dest_uid_str);
            int.TryParse(dest_uid_str, out dest_uid);
            GUILayout.Label("unity2d");
            dest_token = GUILayout.TextArea(dest_token);

            if (GUILayout.Button("StartChannelMediaRelay"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();

                ChannelMediaRelayConfiguration mediaRelayConfiguration = new ChannelMediaRelayConfiguration();
                mediaRelayConfiguration.srcInfo.uid = (uint)src_uid;
                mediaRelayConfiguration.srcInfo.channelName = "unity3d";
                mediaRelayConfiguration.srcInfo.token = src_token;//"006b16bc9253a014a10a5c8d246601ff843IACchFus76wPyzDAuZkiyl4UZ6spRHnNiXKf/l/SYlV514H/KYQAAAAAEACkGjsKvQmVYAEAAQC9CZVg";

                mediaRelayConfiguration.destCount = 1;

                mediaRelayConfiguration.destInfos.uid = (uint)dest_uid;
                mediaRelayConfiguration.destInfos.channelName = "unity2d";
                mediaRelayConfiguration.destInfos.token = dest_token;// "006b16bc9253a014a10a5c8d246601ff843IACMjVR0+7ZMsPRXylBh5BoS9Z7v0RTzQtntp3CURt3cisDOMp0AAAAAEACkGjsK4gmVYAEAAQDiCZVg";

                int res = engine.StartChannelMediaRelay(mediaRelayConfiguration);
                Debug.Log("StartChannelMediaRelay = " + res);
            }

            dest_uid_str2 = GUILayout.TextField(dest_uid_str2);
            int.TryParse(dest_uid_str2, out dest_uid2);
            GUILayout.Label("unity2021");
            dest_token2 = GUILayout.TextArea(dest_token2);

            if (GUILayout.Button("UpdateChannelMediaRelay"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();

                ChannelMediaRelayConfiguration mediaRelayConfiguration = new ChannelMediaRelayConfiguration();
                mediaRelayConfiguration.srcInfo.uid = (uint)src_uid;
                mediaRelayConfiguration.srcInfo.channelName = "unity3d";
                mediaRelayConfiguration.srcInfo.token = src_token;// "006b16bc9253a014a10a5c8d246601ff843IACchFus76wPyzDAuZkiyl4UZ6spRHnNiXKf/l/SYlV514H/KYQAAAAAEACkGjsKvQmVYAEAAQC9CZVg";

                mediaRelayConfiguration.destCount = 1;

                mediaRelayConfiguration.destInfos.uid = (uint)dest_uid2;
                mediaRelayConfiguration.destInfos.channelName = "unity2021";
                mediaRelayConfiguration.destInfos.token = dest_token2;// "006b16bc9253a014a10a5c8d246601ff843IACxev5HXS/QmIwjL5zWsJLkKN2d7zgP7v/6vwU/z3WJ8RNl5CEAAAAAEACkGjsKIzOVYAEAAQAjM5Vg";

                int res = engine.UpdateChannelMediaRelay(mediaRelayConfiguration);
                Debug.Log("UpdateChannelMediaRelay = " + res);
            }

            if (GUILayout.Button("StopChannelMediaRelay"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                int res = engine.StopChannelMediaRelay();
                Debug.Log("UpdateChannelMediaRelay = " + res);
            }

            if (GUILayout.Button("SetRemoteUserPriority"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.SetRemoteUserPriority(TestHome.app.myUID, PRIORITY_TYPE.PRIORITY_HIGH);
                //Debug.Log("Play Devices Count = " + ardm.GetAudioRecordingDeviceCount());
                //adc = "" + ardm.GetAudioRecordingDeviceCount();
            }

            if (GUILayout.Button("Get Playback Devices Count " + adc))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                AudioRecordingDeviceManager ardm = engine.TestGetAudioRecordingDeviceManager();
                Debug.Log("Play Devices Count = " + ardm.GetAudioRecordingDeviceCount());
                adc = "" + ardm.GetAudioRecordingDeviceCount();
            }

            if (GUILayout.Button("GetVideoDeviceCount " + vdc))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                VideoDeviceManager vdm = engine.TestGetVideoDeviceManager();
                Debug.Log("Video devices Count = " + vdm.GetVideoDeviceCount());
                vdc = "" + vdm.GetVideoDeviceCount();
            }

            if (GUILayout.Button("GetCurrentVideoDevice " + cvdc))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                VideoDeviceManager vdm = engine.TestGetVideoDeviceManager();

                vdm.GetCurrentVideoDevice(ref cvdc);
            }



            if (GUILayout.Button("set client role : HOST"))
            {
                agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
                int id = engine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);

                msg1 = "client role set: " + id;

            }

            if (GUILayout.Button("set client role : AUDIENCE"))
            {
                agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
                int id = engine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE);

                msg1 = "client role set: " + id;

            }

            if (GUILayout.Button("SetWebParameters"))
            {
                agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
#if !UNITY_EDITOR && UNITY_WEBGL
				uploadLogStatus = !uploadLogStatus;
				int id = engine.SetParameter("UPLOAD_LOG", uploadLogStatus);
				engine.SetParameter("UPLOAD_LOG", 1);
				engine.SetParameter("UPLOAD_LOG", 5.0f);
				engine.SetParameterString("UPLOAD_LOG", "Test");
#endif

                //msg1 = "SetParameter called " + id;

            }

            if (GUILayout.Button("SetVoiceOnlyMode"))
            {
                agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
                int id = engine.GetAudioEffectManager().SetVoiceOnlyMode(true);

                msg1 = "client role set: " + id;

            }

            TestDraw2();

            //strFilePath = GUILayout.TextField(strFilePath);

            TestAudioMixing();

            TestTranscoding();


            GUILayout.EndScrollView();
            //SetCameraCapturerConfiguration(CameraCapturerConfiguration cameraCaptureConfiguration)

            FBURL = GUILayout.TextField(FBURL);
            GUI.Label(new Rect(50, 220, 800, 30), msg1);
        }

        void TestMuteStreams()
        {
            if (GUILayout.Button("Mute all remote VIDEO streams"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.MuteAllRemoteVideoStreams(true);
            }
            if (GUILayout.Button("Unmute all video VIDEO streams"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.MuteAllRemoteVideoStreams(false);
            }
            if (GUILayout.Button("Mute all remote AUDIO streams"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.MuteAllRemoteAudioStreams(true);
            }
            if (GUILayout.Button("Unmute all remote AUDIO streams"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.MuteAllRemoteAudioStreams(false);
            }
            if (GUILayout.Button("Mute Local VIDEO Stream"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.MuteLocalVideoStream(true);
            }
            if (GUILayout.Button("Unmute Local VIDEO Stream"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.MuteLocalVideoStream(false);
            }
            if (GUILayout.Button("Mute VIDEO User 1 "))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                Debug.Log("Remote User Count " + TestHome.app.listOfUsers.Count);
                engine.MuteRemoteVideoStream(TestHome.app.listOfUsers[0], true);
            }
            if (GUILayout.Button("Unmute VIDEO User 1 "))
            {
                Debug.Log("Remote User Count " + TestHome.app.listOfUsers.Count);
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.MuteRemoteVideoStream(TestHome.app.listOfUsers[0], false);
            }
            if (GUILayout.Button("Mute AUDIO User 1 "))
            {
                Debug.Log("Remote User Count " + TestHome.app.listOfUsers.Count);
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.MuteRemoteAudioStream(TestHome.app.listOfUsers[0], true);
            }
            if (GUILayout.Button("Unmute AUDIO User 1 "))
            {
                Debug.Log("Remote User Count " + TestHome.app.listOfUsers.Count);
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.MuteRemoteAudioStream(TestHome.app.listOfUsers[0], false);
            }
        }

        void TestDualStreams()
        {
            if (GUILayout.Button("Enable Dual Stream"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.EnableDualStreamMode(true);
            }
            if (GUILayout.Button("Disable Dual Stream"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.EnableDualStreamMode(false);
            }
            if (GUILayout.Button("SetLocalPublishFallbackOption"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.SetLocalPublishFallbackOption(STREAM_FALLBACK_OPTIONS.STREAM_FALLBACK_OPTION_VIDEO_STREAM_LOW);
            }
            if (GUILayout.Button("SetRemoteSubscribeFallbackOption"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.SetRemoteSubscribeFallbackOption(STREAM_FALLBACK_OPTIONS.STREAM_FALLBACK_OPTION_VIDEO_STREAM_LOW);
            }
            if (GUILayout.Button("SetVideoRemoteStream HIGH User 1"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                Debug.Log("Remote User Id " + TestHome.app.listOfUsers[0].ToString());
                engine.SetRemoteVideoStreamType(TestHome.app.listOfUsers[0], REMOTE_VIDEO_STREAM_TYPE.REMOTE_VIDEO_STREAM_HIGH);
            }
            if (GUILayout.Button("SetVideoRemoteStream Low User 1"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                Debug.Log("Remote User Id " + TestHome.app.listOfUsers[0].ToString());
                engine.SetRemoteVideoStreamType(TestHome.app.listOfUsers[0], REMOTE_VIDEO_STREAM_TYPE.REMOTE_VIDEO_STREAM_LOW);
            }
            if (GUILayout.Button("SetDefaultVideoRemoteStream"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.SetRemoteDefaultVideoStreamType(REMOTE_VIDEO_STREAM_TYPE.REMOTE_VIDEO_STREAM_LOW);
            }
        }

        void TestAudioMixing()
        {
            if (GUILayout.Button("Start Audio Mixing"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                Debug.Log(strFilePath);
                engine.StartAudioMixing(strFilePath, false, false, -1);
            }
            if (GUILayout.Button("StopAudioMixing"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.StopAudioMixing();
            }
            if (GUILayout.Button("Pause Audio Mixing"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.PauseAudioMixing();
            }
            if (GUILayout.Button("Resume Audio Mixing"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.ResumeAudioMixing();
            }
            if (GUILayout.Button("GetAudioMixingDuration"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                float duration = engine.GetAudioMixingDuration();
                Debug.Log("Duration C# " + duration);
            }
            if (GUILayout.Button("GetAudioMixingCurrentPosition"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                float position = engine.GetAudioMixingCurrentPosition();
                Debug.Log("Position C# " + position);
            }
            if (GUILayout.Button("AdjustAudioMixingPlayoutVolume"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.AdjustAudioMixingPlayoutVolume(70);
            }
            if (GUILayout.Button("GetAudioMixingPlayoutVolume"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                float volume = engine.GetAudioMixingPlayoutVolume();
                Debug.Log("playout volume C# " + volume);
            }
            if (GUILayout.Button("AdjustAudioMixingPublishVolume"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.AdjustAudioMixingPublishVolume(15);
            }
            if (GUILayout.Button("GetAudioMixingPublishVolume"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                float volume = engine.GetAudioMixingPublishVolume();
                Debug.Log("published volume C# " + volume);
            }
            if (GUILayout.Button("SetAudioMixingPosition"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.SetAudioMixingPosition(1);
            }







            if (GUILayout.Button("Mute Local Audio Stream TRUE"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.MuteLocalAudioStream(true);
            }
            if (GUILayout.Button("Mute Local Audio Stream FALSE"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.MuteLocalAudioStream(false);
            }
        }

        void TestTranscoding()
        {
            if (GUILayout.Button("Start Live Transcoding"))
            {
                const int HOSTVIEW_WIDTH = 360;
                const int HOSTVIEW_HEIGHT = 640;
                IRtcEngine engine = IRtcEngine.QueryEngine();
                LiveTranscoding live = new LiveTranscoding();

                TranscodingUser user = new TranscodingUser();
                if (TestHome.app.listOfUsers.Count > 0)
                    user.uid = TestHome.app.listOfUsers[0];
                else
                    user.uid = TestHome.app.myUID;
                user.x = 0;
                user.y = 0;
                user.width = HOSTVIEW_WIDTH;
                user.height = HOSTVIEW_HEIGHT;
                user.audioChannel = 0;
                user.alpha = 1;

                TranscodingUser me = user;
                me.uid = TestHome.app.myUID;
                me.x = me.width;

                live.transcodingUsers = new TranscodingUser[] { me, user };
                live.userCount = 2;

                live.width = 2 * HOSTVIEW_WIDTH;
                live.height = HOSTVIEW_HEIGHT;
                live.videoBitrate = 400;
                live.videoCodecProfile = VIDEO_CODEC_PROFILE_TYPE.VIDEO_CODEC_PROFILE_HIGH;
                live.videoGop = 30;
                live.videoFramerate = 24;
                live.lowLatency = false;

                live.audioSampleRate = AUDIO_SAMPLE_RATE_TYPE.AUDIO_SAMPLE_RATE_44100;
                live.audioBitrate = 48;
                live.audioChannels = 1;
                live.audioCodecProfile = AUDIO_CODEC_PROFILE_TYPE.AUDIO_CODEC_PROFILE_LC_AAC;

                LiveStreamAdvancedFeature[] liveStreamAdvancedFeatures1 = new LiveStreamAdvancedFeature[2];
                liveStreamAdvancedFeatures1[0].featureName = "User";
                liveStreamAdvancedFeatures1[1].featureName = "User1";
                live.liveStreamAdvancedFeatures = liveStreamAdvancedFeatures1;

                engine.SetLiveTranscoding(live);

                engine.OnTranscodingUpdated += HandleTranscodingCallback;
                //int rc = mRtcEngine.AddPublishStreamUrl(url: YTURL, transcodingEnabled: true);
                //Debug.Assert(rc == 0, " error in adding " + YTURL);
                int rc = engine.AddPublishStreamUrl(url: FBURL, transcodingEnabled: true);
            }

            if (GUILayout.Button("Stop Live Transcoding"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.RemovePublishStreamUrl(FBURL);
            }
        }
        void TestDraw2()
        {


            if (GUILayout.Button("Set Audio Profile"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.SetAudioProfile(AUDIO_PROFILE_TYPE.AUDIO_PROFILE_IOT, AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_MEETING);
            }
            if (GUILayout.Button("EnableLocalVideo = false"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.EnableLocalVideo(false);
            }
            if (GUILayout.Button("EnableLocalVideo = true"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.EnableLocalVideo(true);
            }


            if (GUILayout.Button("Video Encoder config"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                VideoEncoderConfiguration videoConfigEncode = new VideoEncoderConfiguration();
                videoConfigEncode.dimensions.width = 120;
                videoConfigEncode.dimensions.height = 640;
                videoConfigEncode.frameRate = FRAME_RATE.FRAME_RATE_FPS_60;
                videoConfigEncode.minFrameRate = (int)FRAME_RATE.FRAME_RATE_FPS_15;
                videoConfigEncode.bitrate = 100;
                videoConfigEncode.minBitrate = 50;
                videoConfigEncode.orientationMode = ORIENTATION_MODE.ORIENTATION_MODE_ADAPTIVE;
                videoConfigEncode.degradationPreference = DEGRADATION_PREFERENCE.MAINTAIN_BALANCED;
                videoConfigEncode.mirrorMode = VIDEO_MIRROR_MODE_TYPE.VIDEO_MIRROR_MODE_AUTO;

                engine.SetVideoEncoderConfiguration(videoConfigEncode);
                /*
				configuration.dimensions.width, configuration.dimensions.height, (int)configuration.frameRate, configuration.minFrameRate, configuration.bitrate, 
					configuration.minBitrate, (int)configuration.orientationMode, (int)configuration.degradationPreference, (int)configuration.mirrorMode);
				*/

            }
            if (GUILayout.Button("Enable last mile Test"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                Debug.Log("Last mile test result : " + engine.EnableLastmileTest());

            }

            if (GUILayout.Button("Disable last mile Test"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.DisableLastmileTest();
            }

        }

        void TestDrawAudio()
        {


            if (GUILayout.Button("## EnableLocalAudio = false"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.EnableLocalAudio(false);
            }
            if (GUILayout.Button("## EnableLocalAudio = true"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.EnableLocalAudio(true);
            }

            if (GUILayout.Button("# Enable Local Audio"))
            {
                Debug.Log("enable local audio");
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.EnableAudio();
            }

            if (GUILayout.Button("# Disable Local Audio"))
            {
                Debug.Log("disable local audio");
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.DisableAudio();
            }
        }

        void TestDraw1()
        {

            if (GUILayout.Button("setLogFilter"))
            {
                agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
                //uint rnd = (uint)Random.Range(0, 4);
                engine.SetLogFilter(LOG_FILTER.INFO);
            }

            if (GUILayout.Button("Connection State"))
            {
                agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
                //uint rnd = (uint)Random.Range(0, 4);
                CONNECTION_STATE_TYPE conState = engine.GetConnectionState();
                msg1 = "CONNECTION_STATE_TYPE: " + conState;

            }

            if (GUILayout.Button("Set Beauty effect On"))
            {
                agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
                //uint rnd = (uint)Random.Range(0, 4);
                BeautyOptions bo = new BeautyOptions();
                bo.lighteningContrastLevel = BeautyOptions.LIGHTENING_CONTRAST_LEVEL.LIGHTENING_CONTRAST_HIGH;
                bo.lighteningLevel = 1;
                bo.smoothnessLevel = 1;

                int id = engine.SetBeautyEffectOptions(true, bo);
                msg1 = "Set beauty effect: " + id;

            }

            if (GUILayout.Button("Set Beauty effect Off"))
            {
                agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();

                BeautyOptions bo = new BeautyOptions();
                bo.lighteningContrastLevel = BeautyOptions.LIGHTENING_CONTRAST_LEVEL.LIGHTENING_CONTRAST_HIGH;
                bo.lighteningLevel = 1;
                bo.smoothnessLevel = 1;

                int id = engine.SetBeautyEffectOptions(false, bo);
                msg1 = "Set beauty effect off: " + id;

            }

            if (GUILayout.Button("Enable Audio volumen indicator"))
            {
                agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
                int id = engine.EnableAudioVolumeIndication(1000, 1, false);

                msg1 = "new token set: " + id;

            }

            /*if (GUILayout.Button( "Renew Token"))
			{
				agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
				int id = engine.RenewToken("006b16bc9253a014a10a5c8d246601ff843IAAX6t+4HReVyQ7hDn9j2Up9Ydltn+muR0qQUef+ajW+OYH/KYQAAAAAEAAH/Ych28UPYAEAAQDbxQ9g");

				msg1 = "new token set: " + id;

			}*/


            //setEncryptionMode2(IntPtr channel, string encryptionMode)
            if (GUILayout.Button("Set encryption, call before join"))
            {
                agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
                int id = engine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
                engine.SetEncryptionMode("aes-128-xts");
                //engine.setEncryptionMode2(ENCRYPTION_MODE.AES_128_ECB, "test21#$dsDedf");
                engine.SetEncryptionSecret("test21#$dsDedf");

                msg1 = "Encryption mode set";


            }

            if (GUILayout.Button("Create video track"))
            {
                agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
                CameraCapturerConfiguration ccc = new CameraCapturerConfiguration();
                ccc.cameraDirection = CAMERA_DIRECTION.CAMERA_FRONT;
                ccc.preference = CAPTURER_OUTPUT_PREFERENCE.CAPTURER_OUTPUT_PREFERENCE_AUTO;
                int id = engine.SetCameraCapturerConfiguration(ccc);

                //engine.SetScreenCaptureContentHint
                //engine.StartScreenCaptureByDisplayId
                //engine.StartScreenCaptureByScreenRect
                //engine.StartScreenCaptureByWindowId
                //engine.StopScreenCapture


            }

            if (GUILayout.Button("Start Screen Capture"))
            {
                agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
                Rectangle rt = new Rectangle();
                rt.x = 0;
                rt.y = 0;
                rt.width = 100;
                rt.height = 100;
                ScreenCaptureParameters scp = new ScreenCaptureParameters();
                scp.captureMouseCursor = true;
                scp.bitrate = 100;
                scp.frameRate = 30;
                scp.dimensions.height = 100;
                scp.dimensions.width = 100;

                // stop local surface for now
                //TestHome.app.StopVideoForLocalSurface();

                engine.StartScreenCaptureByDisplayId(1, rt, scp);

                //engine.SwitchCamera();


                //engine.SetScreenCaptureContentHint
                //engine.StartScreenCaptureByDisplayId
                //engine.StartScreenCaptureByScreenRect
                //engine.StartScreenCaptureByWindowId
                //engine.StopScreenCapture


            }



            if (GUILayout.Button("Stop Screen Capture"))
            {
                agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();

                // stop local surface for now
                //TestHome.app.StopVideoForLocalSurface();
                engine.StopScreenCapture();


            }



            if (GUILayout.Button("Switch Camera"))
            {
                agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();

                engine.SwitchCamera();


                //engine.SetScreenCaptureContentHint
                //engine.StartScreenCaptureByDisplayId
                //engine.StartScreenCaptureByScreenRect
                //engine.StartScreenCaptureByWindowId
                //engine.StopScreenCapture


            }


        }

        void TestAudioEffects()
        {
            if (GUILayout.Button("# PlayEffect 101"))
            {
                //Debug.Log("PlayEffect");
                //IRtcEngine engine = IRtcEngine.QueryEngine();
                //engine.DisableAudio();

                IRtcEngine engine = IRtcEngine.QueryEngine();
                int value = engine.GetAudioEffectManager().PlayEffect(101, "sample_mp3.mp3", 2);
                Debug.Log("PlayEffect " + value);
            }

            if (GUILayout.Button("# PlayEffect 102"))
            {
                //Debug.Log("PlayEffect");
                //IRtcEngine engine = IRtcEngine.QueryEngine();
                //engine.DisableAudio();

                IRtcEngine engine = IRtcEngine.QueryEngine();
                int value = engine.GetAudioEffectManager().PlayEffect(102, "sample1_mp3.mp3", 1);
                Debug.Log("PlayEffect " + value);
            }

            if (GUILayout.Button("# GetEffectsVolume"))
            {
                //Debug.Log("GetEffectsVolume");
                //IRtcEngine engine = IRtcEngine.QueryEngine();
                //engine.DisableAudio();

                IRtcEngine engine = IRtcEngine.QueryEngine();
                double vol = engine.GetAudioEffectManager().GetEffectsVolume();
                Debug.Log("GetEffectsVolume " + vol);
            }

            if (GUILayout.Button("# SetEffectsVolume"))
            {
                //Debug.Log("SetEffectsVolume");
                //IRtcEngine engine = IRtcEngine.QueryEngine();
                //engine.DisableAudio();

                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.GetAudioEffectManager().SetEffectsVolume(1000);
                double vol = engine.GetAudioEffectManager().GetEffectsVolume();
                Debug.Log("SetEffectsVolume " + vol);
            }

            if (GUILayout.Button("# SetVolumeOfEffect 102"))
            {
                //Debug.Log("SetEffectsVolume");
                //IRtcEngine engine = IRtcEngine.QueryEngine();
                //engine.DisableAudio();

                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.SetVolumeOfEffect(102, 1000);
                double vol = engine.GetAudioEffectManager().GetEffectsVolume();
                Debug.Log("SetEffectsVolume " + vol);
            }



            if (GUILayout.Button("# Stop Effect 102 "))
            {
                //Debug.Log("SetEffectsVolume");
                //IRtcEngine engine = IRtcEngine.QueryEngine();
                //engine.DisableAudio();

                IRtcEngine engine = IRtcEngine.QueryEngine();
                double retvalue = engine.GetAudioEffectManager().StopEffect(102);

                Debug.Log("StopEffect " + retvalue);
            }

            if (GUILayout.Button("# Stop All Effects"))
            {
                //Debug.Log("SetEffectsVolume");
                //IRtcEngine engine = IRtcEngine.QueryEngine();
                //engine.DisableAudio();

                IRtcEngine engine = IRtcEngine.QueryEngine();
                double retvalue = engine.GetAudioEffectManager().StopAllEffects();
                Debug.Log("StopAllEffects " + retvalue);
            }

            if (GUILayout.Button("# Pause All Effects"))
            {
                Debug.Log("PauseAllEffects");
                //IRtcEngine engine = IRtcEngine.QueryEngine();
                //engine.DisableAudio();

                IRtcEngine engine = IRtcEngine.QueryEngine();
                double retvalue = engine.GetAudioEffectManager().PauseAllEffects();
                Debug.Log("PauseAllEffects " + retvalue);
            }

            if (GUILayout.Button("# Resume All Effects"))
            {
                Debug.Log("ResumeAllEffects");
                //IRtcEngine engine = IRtcEngine.QueryEngine();
                //engine.DisableAudio();

                IRtcEngine engine = IRtcEngine.QueryEngine();
                double retvalue = engine.GetAudioEffectManager().ResumeAllEffects();
                Debug.Log("ResumeAllEffects " + retvalue);
            }

            if (GUILayout.Button("# Pause Effects 1 102"))
            {
                //Debug.Log("SetEffectsVolume");
                //IRtcEngine engine = IRtcEngine.QueryEngine();
                //engine.DisableAudio();

                IRtcEngine engine = IRtcEngine.QueryEngine();
                double retvalue = engine.GetAudioEffectManager().PauseEffect(102);
                Debug.Log("PauseEffect " + retvalue);
            }

            if (GUILayout.Button("# Resume Effects 1 102 "))
            {
                //Debug.Log("SetEffectsVolume");
                //IRtcEngine engine = IRtcEngine.QueryEngine();
                //engine.DisableAudio();

                IRtcEngine engine = IRtcEngine.QueryEngine();
                double retvalue = engine.GetAudioEffectManager().ResumeEffect(102);
                Debug.Log("ResumeEffect " + retvalue);
            }

            if (GUILayout.Button("# Preload Effect 101"))
            {
                //Debug.Log("SetEffectsVolume");
                //IRtcEngine engine = IRtcEngine.QueryEngine();
                //engine.DisableAudio();

                IRtcEngine engine = IRtcEngine.QueryEngine();
                double retvalue = engine.GetAudioEffectManager().PreloadEffect(101, "sample_mp3.mp3");
                Debug.Log("PreloadEffect " + retvalue);
            }
            if (GUILayout.Button("# Unload Effect 101"))
            {
                //Debug.Log("SetEffectsVolume");
                //IRtcEngine engine = IRtcEngine.QueryEngine();
                //engine.DisableAudio();

                IRtcEngine engine = IRtcEngine.QueryEngine();
                double retvalue = engine.GetAudioEffectManager().UnloadEffect(101);
                Debug.Log("UnloadEffect " + retvalue);
            }

        }

        void HandleTranscodingCallback()
        {
            Debug.Log("Transcoding Handle Call back");
        }
    }


}
