using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace agora_gaming_rtc
{
	public class ImplTesterBeforeJoin : MonoBehaviour
	{
		public GameObject quad;
		private IRtcEngine mRtcEngine;
		public string appId = "";

		string msg1 = "";
		private Vector2 scrollPosition = new Vector2(10,100);

		List<string> _list = new List<string>();
		public void onCamerasListing(string listing)
        {
			Debug.Log(listing);
			string[] list1 = listing.Split('|');
			foreach (string s in list1)
			{
				string[] item = s.Split(',');
				Debug.Log("# " + "deviceId: " + item[0] + ", " + "kind: " + item[1] + ", " + "label: " + item[2] + ", " + "groupId: " + item[3]);
				_list.Add("# " + "deviceId: " + item[0] + ", " + "kind: " + item[1] + ", " + "groupId: " + item[3]);
			}
		}

		void OnGUI()
		{

			GUILayout.BeginArea(new Rect(100, 10, 700, 800));
			GUILayout.Box("---- event data ----");
			foreach (string s in _list)
			{
				GUILayout.Box(s);
			}
			GUILayout.EndArea();

			scrollPosition = GUILayout.BeginScrollView(scrollPosition);


			GUILayout.Box("Set channel Profile call before join, then join");

			if (GUILayout.Button("Set Channel Profile - COMMUNICATION"))
			{
				IRtcEngine engine = IRtcEngine.QueryEngine();
				engine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_COMMUNICATION);
			}

			if (GUILayout.Button("Set Channel Profile - LIVE_BROADCASTING"))
			{
				IRtcEngine engine = IRtcEngine.QueryEngine();
				engine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);
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

			if (GUILayout.Button("startPreview"))
			{

				Debug.Log("initializeEngine");

				if (mRtcEngine == null)
				{
					// init engine
					mRtcEngine = IRtcEngine.GetEngine(appId);

					// enable log
					mRtcEngine.SetLogFilter(LOG_FILTER.DEBUG | LOG_FILTER.INFO | LOG_FILTER.WARNING | LOG_FILTER.ERROR | LOG_FILTER.CRITICAL);
				}


				// LA IMPORTANT: PLEASE IMPLEMENT ENABLE VIDEO BECAUSE ITS NEED TO BE CALLED BEFORE CALLING PREVIEW
				// its implemented somewhere, so dont enable it
				//mRtcEngine.EnableAudio();

				//StartCoroutine(_startPreview());

				mRtcEngine.StartPreview();

				if (quad.GetComponent<VideoSurface>())
				{
					quad.GetComponent<VideoSurface>().SetEnable(true);
				}
				else
				{
					VideoSurface vs = quad.AddComponent<VideoSurface>();
					vs.SetEnable(true);
				}



			}

			if (GUILayout.Button("Stop Preview"))
			{

				Debug.Log("initializeEngine");
				// LA IMPORTANT: PLEASE IMPLEMENT ENABLE VIDEO BECAUSE ITS NEED TO BE CALLED BEFORE CALLING PREVIEW
				// its implemented somewhere, so dont enable it
				//mRtcEngine.EnableAudio();

				//StartCoroutine(_startPreview());

				mRtcEngine.StopPreview();
				quad.GetComponent<VideoSurface>().SetEnable(false);

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

			GUILayout.EndScrollView();
			//SetCameraCapturerConfiguration(CameraCapturerConfiguration cameraCaptureConfiguration)


			GUI.Label(new Rect(50, 220, 800, 30), msg1);

		}

		void LastmileQualityHandler(int quality)
		{
			Debug.Log("From Unity : Quality of last mile " + quality);
		}

		IEnumerator _startPreview()
        {
			mRtcEngine.StartPreview();

			yield return new WaitForSeconds(3);

			VideoSurface vs = quad.AddComponent<VideoSurface>();
			vs.SetEnable(true);

			yield return 0;
		}

	}

}