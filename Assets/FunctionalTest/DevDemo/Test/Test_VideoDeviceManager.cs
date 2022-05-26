using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;

public class Test_VideoDeviceManager : MonoBehaviour {

    public Text txtCurDeviceId;
    // video device manager testing functions
    public AMT_Item[] listDevices;

    // Use this for initialization
    void Start()
    {
        foreach (AMT_Item item in listDevices)
        {
            item.gameObject.SetActive(false);
        }
    }

    #region Video Device Manager
    
    public void GetVideoRecordingDeviceCount()
    {


        IRtcEngine mEngine = IRtcEngine.QueryEngine();

        VideoDeviceManager apm = (VideoDeviceManager)mEngine.GetVideoDeviceManager();
        apm.SetEngine(mEngine);
        apm.CreateAVideoDeviceManager();

        int deviceCount = apm.GetVideoDeviceCount();
        Debug.Log("AudioPlaybackDeviceManager = " + deviceCount);
        //txtVideoDeviceCount.text = "Playback Devices Count: " + deviceCount;

        string deviceName, deviceId;
        deviceName = deviceId = string.Empty;
        for (int i = 0; i < deviceCount; i++)
        {
            apm.GetVideoDevice(i, ref deviceName, ref deviceId);

            AMT_Item item = listDevices[i];
            item.gameObject.SetActive(true);
            item.txtName.text = deviceName;
            item.txtDeviceId.text = deviceId;
            item.itemIndex = i;
            item.tvdm = this;

            Debug.Log("deviceName: " + deviceName + ", deviceId: " + deviceId);

        }
    }

    public void SetVideoDeviceWithIndex(int index)
    {

        IRtcEngine mEngine = IRtcEngine.QueryEngine();

        VideoDeviceManager apm = (VideoDeviceManager)mEngine.GetVideoDeviceManager();
        apm.SetEngine(mEngine);
        apm.CreateAVideoDeviceManager();


        string deviceName, deviceId;
        deviceName = deviceId = string.Empty;
        apm.GetVideoDevice(index, ref deviceName, ref deviceId);

        apm.SetVideoDevice(deviceId);

    }

    public void GetCurrentVideoDevice()
    {
        IRtcEngine mEngine = IRtcEngine.QueryEngine();

        VideoDeviceManager apm = (VideoDeviceManager)mEngine.GetVideoDeviceManager();
        apm.SetEngine(mEngine);
        apm.CreateAVideoDeviceManager();

        string deviceId = "";
        apm.GetCurrentVideoDevice(ref deviceId);
        txtCurDeviceId.text = "Current Device: " + deviceId;
    }


    #endregion
}
