using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;

public class Test_AudioMixing : MonoBehaviour {

	public Text txtMsg;
	public Dropdown volumeDropdown;
	List<string> m_DropOptions = new List<string>();
	public static uint savedRemoteUserId = 0;

	// Use this for initialization
	void Start () {
		int i = 0;
		while (i < 100)
		{
			m_DropOptions.Add(" " + i);
			i++;
		}
		volumeDropdown.ClearOptions();
		//Add the options created in the List above
		volumeDropdown.AddOptions(m_DropOptions);
	}
	
	public void A_StartAudioMixing()
    {
		IRtcEngine engine = IRtcEngine.QueryEngine();
		engine.StartAudioMixing("audio2.mp3", false, false, -1);
		txtMsg.text = "Started audio mixing file - audio2.mp3";
	}

	public void A_StopAudioMixing()
    {
		txtMsg.text = "Stopped audio mixing";
		IRtcEngine engine = IRtcEngine.QueryEngine();
		engine.StopAudioMixing();
	}

	public void A_AdjustAudioMixingVolume()
    {
		int sv = int.Parse(volumeDropdown.options[volumeDropdown.value].text);
		txtMsg.text = "AdjustAudioMixingVolume - Volume - " + sv;
		IRtcEngine engine = IRtcEngine.QueryEngine();
		engine.AdjustAudioMixingVolume(sv);
	}

	public void A_GetAudioMixingDuration()
    {
		IRtcEngine engine = IRtcEngine.QueryEngine();
		float duration = engine.GetAudioMixingDuration();
		txtMsg.text = "Duration - " + duration;
	}

	public void A_GetAudioMixingPosition()
    {
		IRtcEngine engine = IRtcEngine.QueryEngine();
		float position = engine.GetAudioMixingCurrentPosition();
		//Debug.Log("Position C# " + position);
		txtMsg.text = "Position - " + position;
	}

	public void A_AdjustAudioMixingPlayoutVolume()
    {
		int sv = int.Parse(volumeDropdown.options[volumeDropdown.value].text);
		txtMsg.text = "AdjustAudioMixingPlayoutVolume - Volume - " + sv;

		IRtcEngine engine = IRtcEngine.QueryEngine();
		engine.AdjustAudioMixingPlayoutVolume(sv);
	}

    public void A_AdjustAudioMixingPublishVolume()
    {
		int sv = int.Parse(volumeDropdown.options[volumeDropdown.value].text);
		txtMsg.text = "AdjustAudioMixingPublishVolume - Volume - " + sv;

		IRtcEngine engine = IRtcEngine.QueryEngine();
		engine.AdjustAudioMixingPublishVolume(sv);
	}

	public void A_SetAudioMixingPosition()
    {
		IRtcEngine engine = IRtcEngine.QueryEngine();
		engine.SetAudioMixingPosition(1);
		txtMsg.text = "SetAudioMixingPosition, Setting positon to 1";
	}

	public void B_GetAudioMixingPlayoutVolume()
    {
		IRtcEngine engine = IRtcEngine.QueryEngine();
		float volume = engine.GetAudioMixingPlayoutVolume();
		txtMsg.text = "GetAudioMixingPlayoutVolume, Volume - " + volume;
	}

	public void B_GetAudioMixingPublishVolume()
    {
		IRtcEngine engine = IRtcEngine.QueryEngine();
		float volume = engine.GetAudioMixingPublishVolume();
		txtMsg.text = "GetAudioMixingPublishVolume, Volume - " + volume;
	}

	public void C_GetUserInfoByUid()
    {
		IRtcEngine engine = IRtcEngine.QueryEngine();
		UserInfo info = engine.GetUserInfoByUid(savedRemoteUserId);
		txtMsg.text = "GetUserInfoByUid, account - " + info.userAccount;
	}

	public void C_SetRemoteUserPriority_HIGH()
    {
		IRtcEngine engine = IRtcEngine.QueryEngine();
		engine.SetRemoteUserPriority(savedRemoteUserId, PRIORITY_TYPE.PRIORITY_HIGH);
		txtMsg.text = "SetRemoteUserPriority_HIGH, for user id - " + savedRemoteUserId;
	}

	public void C_SetRemoteUserPriority_NORMAL()
	{
		IRtcEngine engine = IRtcEngine.QueryEngine();
		engine.SetRemoteUserPriority(savedRemoteUserId, PRIORITY_TYPE.PRIORITY_NORMAL);
		txtMsg.text = "SetRemoteUserPriority_NORMAL, for user id - " + savedRemoteUserId;
	}

	public InputField errorCode;

	public void D_GetErrorDescription()
    {
		string code = errorCode.text;
		IRtcEngine engine = IRtcEngine.QueryEngine();
		string v = IRtcEngine.GetErrorDescription(int.Parse(code));
		txtMsg.text = "Error result - " + v;
	}


}
