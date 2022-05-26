using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;

public class Test_PlaybackDeviceManager : MonoBehaviour {

    public Text txtCurDeviceId;
    // video device manager testing functions
    public AMT_Item[] listDevices;
    public Dropdown dropdown_Volume;
    List<string> m_DropOptions = new List<string>();

    // Use this for initialization
    void Start()
    {

        foreach (AMT_Item item in listDevices)
        {
            item.gameObject.SetActive(false);
        }

        int i = 0;
        while (i < 100)
        {
            m_DropOptions.Add(" " + i);
            i++;
        }
        dropdown_Volume.ClearOptions();
        //Add the options created in the List above
        dropdown_Volume.AddOptions(m_DropOptions);

    }

    #region Audio Device Manager
    public void GetVolume()
    {
        IRtcEngine mEngine = IRtcEngine.QueryEngine();

        AudioPlaybackDeviceManager apm = (AudioPlaybackDeviceManager)mEngine.GetAudioPlaybackDeviceManager();
        apm.SetEngine(mEngine);
        apm.CreateAAudioPlaybackDeviceManager();
        int v = apm.GetAudioPlaybackDeviceVolume();
        txtCurDeviceId.text = "Volume = " + v;
    }

    public void SetVolume()
    {
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        int sv = int.Parse(dropdown_Volume.options[dropdown_Volume.value].text);
        AudioPlaybackDeviceManager apm = (AudioPlaybackDeviceManager)mEngine.GetAudioPlaybackDeviceManager();
        apm.SetEngine(mEngine);
        apm.CreateAAudioPlaybackDeviceManager();
        apm.SetAudioPlaybackDeviceVolume(sv);
        txtCurDeviceId.text = "Set Volume = " + sv;

    }

    public void GetAudioRecordingDeviceCount()
    {
        IRtcEngine mEngine = IRtcEngine.QueryEngine();

        AudioPlaybackDeviceManager apm = (AudioPlaybackDeviceManager)mEngine.GetAudioPlaybackDeviceManager();
        apm.SetEngine(mEngine);
        apm.CreateAAudioPlaybackDeviceManager();
        int deviceCount = apm.GetAudioPlaybackDeviceCount();
        Debug.Log("AudioPlaybackDeviceManager = " + deviceCount);
        //txtVideoDeviceCount.text = "Playback Devices Count: " + deviceCount;

        string deviceName, deviceId;
        deviceName = deviceId = string.Empty;
        for (int i = 0; i < deviceCount; i++)
        {
            Debug.Log("i = " + i + " deviceName = " + deviceName + "deviceId = " + deviceId);
            apm.GetAudioPlaybackDevice(i, ref deviceName, ref deviceId);

            AMT_Item item = listDevices[i];
            item.gameObject.SetActive(true);
            item.txtName.text = deviceName;
            item.txtDeviceId.text = deviceId;
            item.itemIndex = i;
            item.tpdm = this;

            Debug.Log("deviceName: " + deviceName + ", deviceId: " + deviceId);

        }
    }

    public void SetAudioRecordingDeviceWithIndex(int index)
    {

        IRtcEngine mEngine = IRtcEngine.QueryEngine();

        AudioPlaybackDeviceManager apm = (AudioPlaybackDeviceManager)mEngine.GetAudioPlaybackDeviceManager();
        apm.SetEngine(mEngine);
        apm.CreateAAudioPlaybackDeviceManager();

        string deviceName, deviceId;
        deviceName = deviceId = string.Empty;
        apm.GetAudioPlaybackDevice(index, ref deviceName, ref deviceId);

        apm.SetAudioPlaybackDevice(deviceId);

        //txtVideoDeviceCount.text = "Setting deviceName := " + deviceName + ", deviceId: " + deviceId;

    }

    public void GetCurrentRecordingDevice()
    {
        //GetCurrentRecordingDevice

        IRtcEngine mEngine = IRtcEngine.QueryEngine();

        AudioPlaybackDeviceManager apm = (AudioPlaybackDeviceManager)mEngine.GetAudioPlaybackDeviceManager();
        apm.SetEngine(mEngine);
        apm.CreateAAudioPlaybackDeviceManager();

        string deviceId = "";
        apm.GetCurrentPlaybackDevice(ref deviceId);
        //txtVideoDeviceCount.text = "Setting deviceName := " + deviceName + ", deviceId: " + deviceId;
        txtCurDeviceId.text = "Current Device: " + deviceId;
    }

    public void GetCurrentRecordingDeviceInfo()
    {

        IRtcEngine mEngine = IRtcEngine.QueryEngine();

        AudioPlaybackDeviceManager apm = (AudioPlaybackDeviceManager)mEngine.GetAudioPlaybackDeviceManager();
        apm.SetEngine(mEngine);
        apm.CreateAAudioPlaybackDeviceManager();

        string deviceName = "";
        string deviceId = "";
        apm.GetCurrentPlaybackDeviceInfo(ref deviceName, ref deviceId);
        txtCurDeviceId.text = "Setting deviceName := " + deviceName + ", deviceId: " + deviceId;
    }

    public void MuteCurrentDevice()
    {
        IRtcEngine mEngine = IRtcEngine.QueryEngine();

        AudioPlaybackDeviceManager apm = (AudioPlaybackDeviceManager)mEngine.GetAudioPlaybackDeviceManager();
        apm.SetEngine(mEngine);
        apm.CreateAAudioPlaybackDeviceManager();

        apm.SetAudioPlaybackDeviceMute(true);
        txtCurDeviceId.text = "Muted Current Device.";
    }

    public void UnMuteCurrentDevice()
    {

        IRtcEngine mEngine = IRtcEngine.QueryEngine();

        AudioPlaybackDeviceManager apm = (AudioPlaybackDeviceManager)mEngine.GetAudioPlaybackDeviceManager();
        apm.SetEngine(mEngine);
        apm.CreateAAudioPlaybackDeviceManager();

        apm.SetAudioPlaybackDeviceMute(false);
        txtCurDeviceId.text = "UnMuted Current Device.";
    }

    public void IsMute()
    {

        IRtcEngine mEngine = IRtcEngine.QueryEngine();

        AudioPlaybackDeviceManager apm = (AudioPlaybackDeviceManager)mEngine.GetAudioPlaybackDeviceManager();
        apm.SetEngine(mEngine);
        apm.CreateAAudioPlaybackDeviceManager();

        bool v = apm.IsAudioPlaybackDeviceMute();
        Debug.Log("IsMute Returned status: " + v);
        txtCurDeviceId.text = "Mute Status: " + v;
    }

    #endregion
}
