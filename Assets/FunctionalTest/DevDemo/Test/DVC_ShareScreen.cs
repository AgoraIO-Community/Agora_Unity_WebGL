using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;
using UnityEngine.UI;
using System.Globalization;
using System.Runtime.InteropServices;
using System;

public class DVC_ShareScreen : MonoBehaviour {

    //public Dropdown dropDownAngle2;
    Texture2D mTexture;
	Rect mRect;
	int i = 100;

    List<string> m_DropOptions = new List<string>();
    // Use this for initialization
    void Start () {
        
        
    }

    void Awake()
    {
        //m_DropOptions.Add("0");
        //m_DropOptions.Add("90");
        //m_DropOptions.Add("180");
        //dropDownAngle2.ClearOptions();
        ////Add the options created in the List above
        //dropDownAngle2.AddOptions(m_DropOptions);
    }

    public Text txtInfo;
    public Text txtUsingWidth;
    public Text txtUsingHeight;
    public Slider sliderWidth;
    public Slider sliderHeight;

    public InputField shareWidth;
    public InputField shareHeight;

	public void StartScreenSharing()
    {
		

        int sw = 0;
        int sh = 0;

        try
        {
            sw = int.Parse(shareWidth.text);
            sh = int.Parse(shareHeight.text);

            mRect = new Rect(0, 0, sw, sh);
            mTexture = new Texture2D((int)mRect.width, (int)mRect.height, TextureFormat.RGBA32, false);
            Debug.Log("StartScreenSharing: DONE ");

            IRtcEngine engine = IRtcEngine.QueryEngine();
            engine.SetExternalVideoSource(true, false);

            txtInfo.text = "Screen.width = " + sw + ",  Screen.height: " + sh;
            // Creates a rectangular region of the screen.
            //mRect = new Rect(0, 0, Screen.width, Screen.height);

        }
        catch (Exception e)
        {

        }

        
	}

    public void SliderWidthValueChanged()
    {
        //txtUsingWidth.text = "" + sliderWidth.value;
        //mRect = new Rect(0, 0, sliderWidth.value, sliderHeight.value);
        //mTexture = new Texture2D((int)mRect.width, (int)mRect.height, TextureFormat.RGBA32, false);
    }

    public void SliderHeightValueChanged()
    {
        //txtUsingHeight.text = "" + sliderHeight.value;
        //mRect = new Rect(0, 0, sliderWidth.value, sliderHeight.value);
        //mTexture = new Texture2D((int)mRect.width, (int)mRect.height, TextureFormat.RGBA32, false);
    }

    bool sharingOn = false;

    // called by button to share one frame on press
    public void StartSharingNow()
    {
        sharingOn = true;
        //StartCoroutine(shareScreen());
        Debug.Log("sharing screen one frame "+ sharingOn);
        //int dvalue = dropDownAngle2.value;
        //Debug.Log("dvalue = " + dropDownAngle2.value);
    }

    public void StopScreenSharing()
    {
        sharingOn = false;
        IRtcEngine engine = IRtcEngine.QueryEngine();
        engine.SetExternalVideoSource(false, false);
    }

    void Update()
    {
        if (sharingOn)
        {
            StartCoroutine(shareScreen());
            //sharingOn = false;
        }
    }

   
    //public InputField inpCropLeft;
    //public InputField inpCropTop;
    //public InputField inpCropRight;
    //public InputField inpCropBottom;
    // Starts to share the screen.
    IEnumerator shareScreen()
    {
        yield return new WaitForEndOfFrame();
        //Debug.Log("sharing screen one frame " + dropDownAngle2.value);

        // Reads the Pixels of the rectangle you create.
        mTexture.ReadPixels(mRect, 0, 0);
        // Applies the Pixels read from the rectangle to the texture.
        mTexture.Apply();
        // Gets the Raw Texture data from the texture and apply it to an array of bytes.
        byte[] bytes = mTexture.GetRawTextureData();
        // Gives enough space for the bytes array.
        int size = Marshal.SizeOf(bytes[0]) * bytes.Length;
        // Checks whether the IRtcEngine instance is existed.
        IRtcEngine rtc = IRtcEngine.QueryEngine();
        if (rtc != null)
        {
            // Creates a new external video frame.
            ExternalVideoFrame externalVideoFrame = new ExternalVideoFrame();
            // Sets the buffer type of the video frame.
            externalVideoFrame.type = ExternalVideoFrame.VIDEO_BUFFER_TYPE.VIDEO_BUFFER_RAW_DATA;
            // Sets the format of the video pixel.
            externalVideoFrame.format = ExternalVideoFrame.VIDEO_PIXEL_FORMAT.VIDEO_PIXEL_RGBA;
            // Applies raw data.
            externalVideoFrame.buffer = bytes;
            // Sets the width (pixel) of the video frame.
            externalVideoFrame.stride = (int)mRect.width;
            // Sets the height (pixel) of the video frame.
            externalVideoFrame.height = (int)mRect.height;
            // Removes pixels from the sides of the frame
            //externalVideoFrame.cropLeft = int.Parse(inpCropLeft.text);
            //externalVideoFrame.cropTop = int.Parse(inpCropTop.text);
            //externalVideoFrame.cropRight = int.Parse(inpCropRight.text);
            //externalVideoFrame.cropBottom = int.Parse(inpCropBottom.text);

            externalVideoFrame.cropLeft = 0;
            externalVideoFrame.cropTop = 0;
            externalVideoFrame.cropRight = 0;
            externalVideoFrame.cropBottom = 0;

            // Rotates the video frame (0, 90, 180, or 270)
            externalVideoFrame.rotation = 180;

            //int dvalue = dropDownAngle.value;
            //if( dvalue == 0 )
            //{
            //    externalVideoFrame.rotation = 0;
            //}
            //else if (dvalue == 1)
            //{
            //    externalVideoFrame.rotation = 90;
            //}
            //else if (dvalue == 2)
            //{
            //    externalVideoFrame.rotation = 180;
            //}

            
            // Increments i with the video timestamp.
            externalVideoFrame.timestamp = i++;
            // Pushes the external video frame with the frame you create.
            int a = rtc.PushVideoFrame(externalVideoFrame);
            Debug.Log("Pushed this video frame");
        }
    }

    


    public void playEffect()
    {
        IRtcEngine engine = IRtcEngine.QueryEngine();
        int value = engine.GetAudioEffectManager().PlayEffect(101, "audio.mp3", 1);
        Debug.Log("PlayEffect " + value);
    }

}
