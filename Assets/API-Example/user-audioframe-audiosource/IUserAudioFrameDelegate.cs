using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;

public interface IUserAudioFrameDelegate
{
    AudioRawDataManager.OnPlaybackAudioFrameBeforeMixingHandler HandleAudioFrameForUser { get; set; }
}