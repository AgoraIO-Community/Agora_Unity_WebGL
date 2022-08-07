﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using agora_utilities;

public class SpatialAudioForClientManager : MonoBehaviour
{
    [SerializeField] private string APP_ID = "YOUR_APPID";

    [SerializeField] private string TOKEN_1 = "";

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

    public Toggle blurToggle, airAbsorbToggle;

    public Text azimuthText, elevationText, 
    distanceText, orientationText, attenuationText;

    public InputField appIdText, tokenText, channelNameText;

    void Awake(){
        
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
        appIdText.text = APP_ID;
        tokenText.text = TOKEN_1;
        channelNameText.text = CHANNEL_NAME_1;
        
    }

    public void updateAppID(){
        APP_ID = appIdText.text;
    }

    public void updateToken(){
        TOKEN_1 = tokenText.text;
    }

    public void updateChannelName(){
        CHANNEL_NAME_1 = channelNameText.text;
    }

    void Update()
    {
        PermissionHelper.RequestMicrophontPermission();
        PermissionHelper.RequestCameraPermission();

        if(joinedChannel){
            joinButton.interactable = false;
            leaveButton.interactable = true;
            appIdText.interactable = false;
            tokenText.interactable = false;
            channelNameText.interactable = false;
        } else {
            joinButton.interactable = true;
            leaveButton.interactable = false;
            appIdText.interactable = true;
            tokenText.interactable = true;
            channelNameText.interactable = true;
        }

        azimuthText.text = azimuthSlider.value.ToString("F2");
        elevationText.text = elevationSlider.value.ToString("F2");
        distanceText.text = distanceSlider.value.ToString("F2");
        orientationText.text = orientationSlider.value.ToString();
        attenuationText.text = attenuationSlider.value.ToString("F2");
    }

    bool CheckAppId()
    {
        return (APP_ID.Length > 10);
    }

    void InitEngine()
    {
        mRtcEngine = IRtcEngine.GetEngine(APP_ID);
        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);

        mRtcEngine.EnableAudio();
        mRtcEngine.EnableVideo();
        mRtcEngine.EnableVideoObserver();
        mRtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);

        mRtcEngine.OnJoinChannelSuccess = EngineOnJoinChannelSuccessHandler;
        mRtcEngine.OnLeaveChannel = EngineOnLeaveChannelHandler;

        mRtcEngine.OnError += EngineOnErrorHandler;

    }

    public void JoinChannel()
    {
        
        mRtcEngine.JoinChannel(TOKEN_1, CHANNEL_NAME_1, "", 0, new ChannelMediaOptions(true, true, true, true));
        joinedChannel = true;
        mRtcEngine.EnableSpatialAudio(true);
    }

    public void LeaveChannel()
    {
        mRtcEngine.LeaveChannel();
        mRtcEngine.EnableSpatialAudio(false);
        joinedChannel = false;
    }

    void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        if (mRtcEngine != null)
        {

            mRtcEngine.DisableVideoObserver();
            IRtcEngine.Destroy();
        }
    }

    public void updateAzimuth(){
        azimuth = azimuthSlider.value;
        updateSpatialAudio();
    }

    public void updateElevation(){
        elevation = elevationSlider.value;
        updateSpatialAudio();
    }

    public void updateDistance(){
        distance = distanceSlider.value;
        updateSpatialAudio();
    }

    public void updateOrientation(){
        orientation = (int)orientationSlider.value;
        updateSpatialAudio();
    }

    public void updateAttenuation(){
        attenuation = attenuationSlider.value;
        updateSpatialAudio();
    }

    public void updateBlur(){
        spatialBlur = blurToggle.isOn;
        updateSpatialAudio();
    }

    public void updateAirAbsorb(){
        spatialAirAbsorb = airAbsorbToggle.isOn;
        updateSpatialAudio();
    }

    public void updateSpatialAudio(){
        Debug.Log("Spatial Blur: " + spatialBlur.ToString());
        mRtcEngine.SetRemoteUserSpatialAudioParams(0, azimuth, elevation, distance, orientation, attenuation, spatialBlur, spatialAirAbsorb);
    }

    void EngineOnJoinChannelSuccessHandler(string channelId, uint uid, int elapsed)
    {
        
    }

    void EngineOnLeaveChannelHandler(RtcStats rtcStats)
    {
       
    }

    void EngineOnErrorHandler(int err, string message)
    {

    }

    void EngineOnUserJoinedHandler(uint uid, int elapsed)
    {
        
    }

    void EngineOnUserOfflineHandler(uint uid, USER_OFFLINE_REASON reason)
    {

    }

    
}