using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;

// playbackdevice
public class Test_ADM : MonoBehaviour {

    public GameObject panelVDM;
    public GameObject panelRDM;
    public GameObject panelPDM;
    public GameObject panelShareScreen;
    public GameObject panelAudioEffects;
    public GameObject panelMuteStreams;
    public GameObject panelShareAudio;
    public GameObject panelMediaRelay;
    public GameObject shareScreenPanel;
    public GameObject audioMixingPanel;
    public GameObject panelWatermark;
    
    public void HideAll()
    {
        panelVDM.SetActive(false);
        panelRDM.SetActive(false);
        panelPDM.SetActive(false);
        panelShareScreen.SetActive(false);
        panelAudioEffects.SetActive(false);
        panelMuteStreams.SetActive(false);
        panelShareAudio.SetActive(false);
        panelMediaRelay.SetActive(false);
        shareScreenPanel.SetActive(false);
        audioMixingPanel.SetActive(false);
        panelWatermark.SetActive(false);
    }

    public void A_ShowpanelAudioEffects()
    {
        HideAll();
        panelAudioEffects.SetActive(true);
    }

    public void showPanelVideoDeviceManager()
    {
        HideAll();
        panelVDM.SetActive(true);
    }

    public void showPanelRecordingDeviceManager()
    {
        HideAll();
        panelRDM.SetActive(true);
    }

    public void showPanelPlaybackDeviceManager()
    {
        HideAll();
        panelPDM.SetActive(true);
    }

    public void showPanelShareScreen()
    {
        HideAll();
        panelShareScreen.SetActive(true);
    }

    public void showPanelMuteStreams()
    {
        HideAll();
        panelMuteStreams.SetActive(true);
    }

    public void showPanelShareAudio()
    {
        HideAll();
        panelShareAudio.SetActive(true);
    }

    public void showPanelMediaRelay()
    {
        HideAll();
        panelMediaRelay.SetActive(true);
    }

    public void showPanelScreenSharing()
    {
        HideAll();
        shareScreenPanel.SetActive(true);
    }

    public void showPanelAudioMixing()
    {
        HideAll();
        audioMixingPanel.SetActive(true);
    }

    public void showPanelWatermark()
    {
        HideAll();
        panelWatermark.SetActive(true);
    }

}
