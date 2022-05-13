using System;
using UnityEngine;
using agora_gaming_rtc;

namespace agora_gs_test
{
    public class TestUI : MonoBehaviour
    {


        public string FBURL = "rtmp://a.rtmp.youtube.com/live2/aewv-2v77-59tb-jg39-423r"; //private string FBURL = "rtmp://a.rtmp.youtube.com/live2/aewv-2v77-59tb-jg39-423r";

        private void OnGUI()
        {
            TestTranscoding();
        }

        void TestTranscoding()
        {
            if (GUI.Button(new Rect(10, 10, 250, 30), "Start Live Transcoding " + TestHome.app.myUID))
            {
                const int HOSTVIEW_WIDTH = 360;
                const int HOSTVIEW_HEIGHT = 640;
                IRtcEngine engine = IRtcEngine.QueryEngine();
                LiveTranscoding live = new LiveTranscoding();

                TranscodingUser me = new TranscodingUser();
                me.uid = TestHome.app.myUID;
                me.x = 0;
                me.y = 0;
                me.width = HOSTVIEW_WIDTH;
                me.height = HOSTVIEW_HEIGHT;
                me.audioChannel = 0;
                me.alpha = 1;

                live.transcodingUsers = new TranscodingUser[] { me };
                live.userCount = 1;

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

                live.watermark.url = "https://via.placeholder.com/150";
                live.watermark.x = 10;
                live.watermark.y = 10;
                live.watermark.width = 150;
                live.watermark.height = 150;

                LiveStreamAdvancedFeature[] liveStreamAdvancedFeatures1 = new LiveStreamAdvancedFeature[2];
                liveStreamAdvancedFeatures1[0].featureName = "User";
                //liveStreamAdvancedFeatures1[1].featureName = "User1";
                live.liveStreamAdvancedFeatures = liveStreamAdvancedFeatures1;

                engine.SetLiveTranscoding(live);

                engine.OnTranscodingUpdated += HandleTranscodingCallback;
                int rc = engine.AddPublishStreamUrl(url: FBURL, transcodingEnabled: true);
            }

            if (GUI.Button(new Rect(10, 60, 250, 30), "Stop Live Transcoding"))
            {
                IRtcEngine engine = IRtcEngine.QueryEngine();
                engine.RemovePublishStreamUrl(FBURL);
            }
        }
        void HandleTranscodingCallback()
        {
            Debug.Log("Transcoding Handle Call back");
        }
    }
}
