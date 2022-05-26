using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;
using UnityEngine.UI;
using System.Globalization;
using System.Runtime.InteropServices;
using System;

public class FrameHandlers : MonoBehaviour {

	// Gets the VideoRawDataManager object.
	VideoRawDataManager videoRawDataManager;
	// Use this for initialization
	void Start () {
		
	}
	
	public void SetOnCaptureVideoFrameCallback_Test()
    {
		IRtcEngine engine = IRtcEngine.QueryEngine();
		// Gets the VideoRawDataManager object.
		// Enables the video observer.
		engine.EnableVideoObserver();
		
		videoRawDataManager = VideoRawDataManager.GetInstance(engine);
		//videoRawDataManager.RegisterVideoRawDataObserver();
		videoRawDataManager.SetOnCaptureVideoFrameCallback(OnCaptureVideoFrameCallback);
		//videoRawDataManager.SetOnRenderVideoFrameCallback(OnCaptureRenderVideoFrameCallback);

    }
	
	public void SetOnCaptureVideoFrameCallback_Test2()
	{
		IRtcEngine engine = IRtcEngine.QueryEngine();
		// Gets the VideoRawDataManager object.
		// Enables the video observer.
		engine.EnableVideoObserver();
		
		videoRawDataManager = VideoRawDataManager.GetInstance(engine);
		//videoRawDataManager.RegisterVideoRawDataObserver();
		//videoRawDataManager.SetOnCaptureVideoFrameCallback(OnCaptureVideoFrameCallback);
		videoRawDataManager.SetOnRenderVideoFrameCallback(OnCaptureRenderVideoFrameCallback);

	}
	
	public GameObject cubeTester;
	int i = 0;
	public void OnCaptureVideoFrameCallback(VideoFrame frame)
	{
		Debug.Log("frame.width: " + frame.width + ", buf length: " + frame.buffer.Length);
		Debug.Log("ystried: " + frame.yStride);
		
		Texture2D tex = new Texture2D(frame.width, frame.height, TextureFormat.PVRTC_RGBA4, false);
		// Load data into the texture and upload it to the GPU.
		tex.LoadRawTextureData(frame.buffer);
		tex.Apply();
		// Assign texture to renderer's material.
		cubeTester.GetComponent<Renderer>().material.mainTexture = tex;
		//i++;
		
	}
	
	
	public void OnCaptureRenderVideoFrameCallback(uint uid, VideoFrame frame)
    {
		Debug.Log("frame.width: " + frame.width + ", uid: " + uid);
		//Debug.Log("frame.width: " + frame.buffer);
		/*foreach (byte b in frame.buffer)
		{
			if (i < 10)
			{
				Debug.Log(b);
			}
			i++;
		}*/
		
		Texture2D tex = new Texture2D(frame.width, frame.height, TextureFormat.PVRTC_RGBA4, false);
		// Load data into the texture and upload it to the GPU.
		tex.LoadRawTextureData(frame.buffer);
		tex.Apply();
		// Assign texture to renderer's material.
		cubeTester.GetComponent<Renderer>().material.mainTexture = tex;
		
		//if (i == 0)
		//{
	//		Texture2D tex = new Texture2D(frame.width, frame.height, TextureFormat.PVRTC_RGBA4, false);
	//		// Load data into the texture and upload it to the GPU.
	//		tex.LoadRawTextureData(frame.buffer);
	//		tex.Apply();
	//		// Assign texture to renderer's material.
	//		cubeTester.GetComponent<Renderer>().material.mainTexture = tex;
	//	}
	}
	const int SAMPLE_RATE = 44100;
	public int AudioChannels = 1;

	public void TestPushAudioFrame()
    {
		IRtcEngine engine = IRtcEngine.QueryEngine();
		engine.SetExternalAudioSource(true, SAMPLE_RATE, AudioChannels);
	}
}
