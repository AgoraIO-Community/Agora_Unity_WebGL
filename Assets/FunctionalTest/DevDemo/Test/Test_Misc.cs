using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;
using agora_utilities;
using UnityEngine.UI;
using System;

public class Test_Misc : MonoBehaviour
{

    public Text txtmsg;

    public void StartRecording()
    {
        IRtcEngine mRtcEngine = IRtcEngine.QueryEngine();
        mRtcEngine.StartAudioRecording("test.webm", AUDIO_RECORDING_QUALITY_TYPE.AUDIO_RECORDING_QUALITY_HIGH);
        txtmsg.text = "Recording...";
    }

    public void StopRecording()
    {
        IRtcEngine mRtcEngine = IRtcEngine.QueryEngine();
        mRtcEngine.StopAudioRecording();
        txtmsg.text = "Stopped";
    }

    public void AdjustSignalVolume()
    {
        IRtcEngine mRtcEngine = IRtcEngine.QueryEngine();
        int rnd = UnityEngine.Random.Range(10, 99);
        txtmsg.text = "Random Vol: " + rnd;
        mRtcEngine.AdjustRecordingSignalVolume(rnd);
    }

    public void _EnableDualStreamMode_TRUE()
    {
        IRtcEngine mRtcEngine = IRtcEngine.QueryEngine();
        txtmsg.text = "Enabled DSM - true";
        mRtcEngine.EnableDualStreamMode(true);
    }

    public void _EnableDualStreamMode_FALSE()
    {
        IRtcEngine mRtcEngine = IRtcEngine.QueryEngine();
        txtmsg.text = "Enabled DSM - false";
        mRtcEngine.EnableDualStreamMode(false);
    }

    public void _SRS_SetRemoteSubscribeFallbackOption_AO()
    {
        // TODO, TO CHECK
        //https://docs.agora.io/en/Video/fallback_android?platform=Android
        IRtcEngine mRtcEngine = IRtcEngine.QueryEngine();
        mRtcEngine.SetRemoteSubscribeFallbackOption(STREAM_FALLBACK_OPTIONS.STREAM_FALLBACK_OPTION_AUDIO_ONLY);
    }

    public void _SRS_SetRemoteSubscribeFallbackOption_DI()
    {
        IRtcEngine mRtcEngine = IRtcEngine.QueryEngine();
        mRtcEngine.SetRemoteSubscribeFallbackOption(STREAM_FALLBACK_OPTIONS.STREAM_FALLBACK_OPTION_DISABLED);
    }

    public void _SRS_SetRemoteSubscribeFallbackOption_VSL()
    {
        IRtcEngine mRtcEngine = IRtcEngine.QueryEngine();
        mRtcEngine.SetRemoteSubscribeFallbackOption(STREAM_FALLBACK_OPTIONS.STREAM_FALLBACK_OPTION_VIDEO_STREAM_LOW);
    }

    public void _SLPF_SetLocalPublishFallbackOption_AO()
    {
        IRtcEngine mRtcEngine = IRtcEngine.QueryEngine();
        mRtcEngine.SetLocalPublishFallbackOption(STREAM_FALLBACK_OPTIONS.STREAM_FALLBACK_OPTION_AUDIO_ONLY);
    }

    public void _SLPF_SetLocalPublishFallbackOption_DI()
    {
        IRtcEngine mRtcEngine = IRtcEngine.QueryEngine();
        mRtcEngine.SetLocalPublishFallbackOption(STREAM_FALLBACK_OPTIONS.STREAM_FALLBACK_OPTION_DISABLED);
    }

    public void _SLPF_SetLocalPublishFallbackOption_VSL()
    {
        IRtcEngine mRtcEngine = IRtcEngine.QueryEngine();
        mRtcEngine.SetLocalPublishFallbackOption(STREAM_FALLBACK_OPTIONS.STREAM_FALLBACK_OPTION_VIDEO_STREAM_LOW);
    }

    public InputField inpTokenSource;
    public InputField inpTokenDest;
    public InputField inpSourceUID;
    public InputField inpDestUID;

    public void A_StartMediaRelay()
    {
        IRtcEngine engine = IRtcEngine.QueryEngine();

        ChannelMediaRelayConfiguration mediaRelayConfiguration = new ChannelMediaRelayConfiguration();
        uint src_uid = uint.Parse(inpSourceUID.text);
        mediaRelayConfiguration.srcInfo.uid = src_uid;
        mediaRelayConfiguration.srcInfo.channelName = "unity3d";
        mediaRelayConfiguration.srcInfo.token = inpTokenSource.text;

        mediaRelayConfiguration.destCount = 1;
        mediaRelayConfiguration.destInfos = new ChannelMediaInfo[1];
        uint dest_uid = uint.Parse(inpDestUID.text);
        mediaRelayConfiguration.destInfos[0].uid = dest_uid;
        mediaRelayConfiguration.destInfos[0].channelName = "unity2d";
        mediaRelayConfiguration.destInfos[0].token = inpTokenDest.text;

        int res = engine.StartChannelMediaRelay(mediaRelayConfiguration);
        Debug.Log("StartChannelMediaRelay = " + res);
    }

    public void A_UpdateMediaRelay()
    {
        IRtcEngine engine = IRtcEngine.QueryEngine();

        ChannelMediaRelayConfiguration mediaRelayConfiguration = new ChannelMediaRelayConfiguration();
        uint src_uid = uint.Parse(inpSourceUID.text);
        mediaRelayConfiguration.srcInfo.uid = src_uid;
        mediaRelayConfiguration.srcInfo.channelName = "unity3d";
        mediaRelayConfiguration.srcInfo.token = inpTokenSource.text;

        mediaRelayConfiguration.destCount = 1;
        mediaRelayConfiguration.destInfos = new ChannelMediaInfo[1];
        uint dest_uid = uint.Parse(inpDestUID.text);
        mediaRelayConfiguration.destInfos[0].uid = dest_uid;
        mediaRelayConfiguration.destInfos[0].channelName = "unity2d";
        mediaRelayConfiguration.destInfos[0].token = inpTokenDest.text;

        int res = engine.UpdateChannelMediaRelay(mediaRelayConfiguration);
        Debug.Log("UpdateChannelMediaRelay = " + res);
    }

    public void A_StopMediaRelay()
    {
        IRtcEngine engine = IRtcEngine.QueryEngine();
        int res = engine.StopChannelMediaRelay();
        Debug.Log("UpdateChannelMediaRelay = " + res);
    }

    public void D_SwitchChannel()
    {
        IRtcEngine engine = IRtcEngine.QueryEngine();
        engine.SwitchChannel(null, "unity2d");

        Debug.Log("Switching channel to unity2d");
    }
    public void D_SetParameter()
    {
        IRtcEngine engine = IRtcEngine.QueryEngine();
        string send = "UPLOAD_LOG";
        engine.SetParameter(send, true);
    }
}
