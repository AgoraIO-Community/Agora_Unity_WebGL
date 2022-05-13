using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;


public class Test_MuteStreams : MonoBehaviour {

    public InputField inputUserId;
    public Text txtinfo;

	public void A_SetRemoteDefaultVideoStreamType_HIGH()
    {
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.SetRemoteDefaultVideoStreamType(REMOTE_VIDEO_STREAM_TYPE.REMOTE_VIDEO_STREAM_HIGH);
    }

    public void A_SetRemoteDefaultVideoStreamType_LOW()
    {
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.SetRemoteDefaultVideoStreamType(REMOTE_VIDEO_STREAM_TYPE.REMOTE_VIDEO_STREAM_LOW);
    }

    public void A_MuteRemoteAudioStream_TRUE()
    {
        uint uid = uint.Parse(inputUserId.text);
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.MuteRemoteAudioStream(uid, true);
        txtinfo.text = "Muted remote audio stream: " + uid;
    }

    public void A_MuteRemoteAudioStream_FALSE()
    {
        uint uid = uint.Parse(inputUserId.text);
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.MuteRemoteAudioStream(uid, false);
        txtinfo.text = "UnMuted remote audio stream: " + uid;
    }

    public void A_MuteRemoteVideoStream_TRUE()
    {
        uint uid = uint.Parse(inputUserId.text);
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.MuteRemoteVideoStream(uid, true);
        txtinfo.text = "Muted remote Video stream: " + uid;
    }

    public void A_MuteRemoteVideoStream_FALSE()
    {
        uint uid = uint.Parse(inputUserId.text);
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.MuteRemoteVideoStream(uid, false);
        txtinfo.text = "UnMuted remote Video stream: " + uid;
    }

    public void A_SetDefaultMuteAllRemoteAudioStreams_TRUE()
    {
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.SetDefaultMuteAllRemoteAudioStreams(true);
    }

    public void A_SetDefaultMuteAllRemoteAudioStreams_FALSE()
    {
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.SetDefaultMuteAllRemoteAudioStreams(false);
    }

    public void A_SetDefaultMuteAllRemoteVideoStreams_TRUE()
    {
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.SetDefaultMuteAllRemoteVideoStreams(true);
    }

    public void A_SetDefaultMuteAllRemoteVideoStreams_FALSE()
    {
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.SetDefaultMuteAllRemoteVideoStreams(false);
    }

    public void F_AdjustPlaybackSignalVolume()
    {
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        int rnd = UnityEngine.Random.Range(10, 99);
        txtinfo.text = "Set Rnd Volume: " + rnd;
        mEngine.AdjustPlaybackSignalVolume(rnd);
    }

    public void F_AdjustUserPlaybackSignalVolume()
    {
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        int rnd = UnityEngine.Random.Range(10, 99);
        uint uid = uint.Parse(inputUserId.text);
        txtinfo.text = "Set Rnd Volume: " + rnd;
        mEngine.AdjustUserPlaybackSignalVolume(uid, rnd);
    }

}
