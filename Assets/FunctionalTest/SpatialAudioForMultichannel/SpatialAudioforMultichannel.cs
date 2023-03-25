﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using agora_utilities;

public class SpatialAudioforMultichannel : MonoBehaviour
{
    [SerializeField] private AppInfoObject appInfo;

    [SerializeField] private string CHANNEL_NAME_1 = "YOUR_CHANNEL_NAME_1";
    private IRtcEngine mRtcEngine = null;
    private const float Offset = 100;

    public Button joinButton, leaveButton;
    public bool joinedChannel = false;

    public double azimuth = 0f;
    public double elevation = 0f;
    public double distance = 1f;
    public int orientation = 0;
    public double attenuation = 0f;

    public bool spatialBlur = false;
    public bool spatialAirAbsorb = false;

    public Slider azimuthSlider, elevationSlider, distanceSlider,
    orientationSlider, attenuationSlider;

    public Toggle blurToggle, airAbsorbToggle, enableToggle;

    public Text azimuthText, elevationText,
    distanceText, orientationText, attenuationText;

    public InputField appIdText, tokenText, channelNameText;

    AgoraChannel spatialAudioChannel;

    [SerializeField]
    private List<uint> remoteClientIDs;

    private void Awake()
    {
        if (RootMenuControl.instance)
        {
            CHANNEL_NAME_1 = RootMenuControl.instance.channel;
        }
    }

    // Use this for initialization
    void Start()
    {
        if (!CheckAppId())
        {
            return;
        }

        InitEngine();

        //channel setup.
        appIdText.text = appInfo.appID;
        tokenText.text = appInfo.token;
        channelNameText.text = CHANNEL_NAME_1;
        remoteClientIDs = new List<uint>();
    }

    public void updateAppID()
    {
        appInfo.appID = appIdText.text;
    }

    public void updateToken()
    {
        appInfo.token = tokenText.text;
    }

    public void updateChannelName()
    {
        CHANNEL_NAME_1 = channelNameText.text;
    }

    void Update()
    {
        PermissionHelper.RequestMicrophontPermission();

        if (joinedChannel)
        {

            joinButton.interactable = false;
            leaveButton.interactable = true;
            appIdText.interactable = false;
            tokenText.interactable = false;
            channelNameText.interactable = false;

            azimuthSlider.interactable = true;
            elevationSlider.interactable = true;
            distanceSlider.interactable = true;
            orientationSlider.interactable = true;
            attenuationSlider.interactable = true;
            blurToggle.interactable = true;
            airAbsorbToggle.interactable = true;
        }
        else
        {

            joinButton.interactable = true;
            leaveButton.interactable = false;
            appIdText.interactable = true;
            tokenText.interactable = true;
            channelNameText.interactable = true;

            azimuthSlider.interactable = false;
            elevationSlider.interactable = false;
            distanceSlider.interactable = false;
            orientationSlider.interactable = false;
            attenuationSlider.interactable = false;
            blurToggle.interactable = false;
            airAbsorbToggle.interactable = false;
        }

        azimuthText.text = azimuthSlider.value.ToString("F2");
        elevationText.text = elevationSlider.value.ToString("F2");
        distanceText.text = distanceSlider.value.ToString("F2");
        orientationText.text = orientationSlider.value.ToString();
        attenuationText.text = attenuationSlider.value.ToString("F2");
    }

    bool CheckAppId()
    {
        return (appInfo.appID.Length > 10);
    }

    void InitEngine()
    {
        mRtcEngine = IRtcEngine.GetEngine(appInfo.appID);
        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);

        mRtcEngine.EnableAudio();
        mRtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        mRtcEngine.SetMultiChannelWant(true);

        spatialAudioChannel = mRtcEngine.CreateChannel(CHANNEL_NAME_1);
        spatialAudioChannel.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);

        spatialAudioChannel.ChannelOnJoinChannelSuccess += OnJoinChannelSuccessHandler;
        spatialAudioChannel.ChannelOnUserJoined += OnUserJoinedHandler;
    }

    void OnJoinChannelSuccessHandler(string channelId, uint uid, int elapsed)
    {

    }

    void OnUserJoinedHandler(string channelId, uint uid, int elapsed)
    {
        remoteClientIDs.Add(uid);
    }

    public void JoinChannel()
    {
        spatialAudioChannel.EnableSpatialAudio_MC(enableToggle.isOn);
        spatialAudioChannel.JoinChannel(appInfo.token, "", 0, new ChannelMediaOptions(true, false, true, false));
        joinedChannel = true;
    }

    public void LeaveChannel()
    {

        spatialAudioChannel.EnableSpatialAudio_MC(false);
        spatialAudioChannel.LeaveChannel();
        joinedChannel = false;

        azimuthSlider.value = 0f;
        elevationSlider.value = 0f;
        distanceSlider.value = 1f;
        orientationSlider.value = 0f;
        attenuationSlider.value = .5f;
        blurToggle.isOn = false;
        airAbsorbToggle.isOn = false;
    }

    void OnDestroy()
    {
        Debug.Log("OnApplicationQuit");
        if (mRtcEngine != null)
        {
            LeaveChannel();
            mRtcEngine.DisableVideoObserver();
            IRtcEngine.Destroy();
        }
    }

    public void updateAzimuth()
    {
        azimuth = azimuthSlider.value;
        updateSpatialAudio();
    }

    public void updateElevation()
    {
        elevation = elevationSlider.value;
        updateSpatialAudio();
    }

    public void updateDistance()
    {
        distance = distanceSlider.value;
        updateSpatialAudio();
    }

    public void updateOrientation()
    {
        orientation = (int)orientationSlider.value;
        updateSpatialAudio();
    }

    public void updateAttenuation()
    {
        attenuation = attenuationSlider.value;
        updateSpatialAudio();
    }

    public void updateBlur()
    {
        spatialBlur = blurToggle.isOn;
        updateSpatialAudio();
    }

    public void updateAirAbsorb()
    {
        spatialAirAbsorb = airAbsorbToggle.isOn;
        updateSpatialAudio();
    }

    public void updateSpatialAudio()
    {
        if (remoteClientIDs.Count > 0 && enableToggle.isOn)
        {
            spatialAudioChannel.SetRemoteUserSpatialAudioParams(remoteClientIDs[0], azimuth, elevation, distance, orientation, attenuation, spatialBlur, spatialAirAbsorb);
        }
    }

}