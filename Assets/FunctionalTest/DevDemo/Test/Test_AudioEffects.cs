using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;

public class Test_AudioEffects : MonoBehaviour {

    public Dropdown fileInfo;
    public Dropdown volumeDropdown;
    List<string> m_DropOptions = new List<string>();

    private int afileId = 101;
    private string afileName = "audio.mp3";

    void Start()
    {
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

    void ProcessInput()
    {
        int v = fileInfo.value;
        
        if( v == 0)
        {
            afileId = 101;
            afileName = "audio.mp3";
        }
        else if( v == 1 )
        {
            afileId = 102;
            afileName = "audio2.mp3";
        }

        Debug.Log("afileId = " + afileId);
        Debug.Log("afileName = " + afileName);

    }

	public void A_PlayEffect()
    {
        ProcessInput();
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.GetAudioEffectManager().PlayEffect(afileId, afileName, 1, 1, 0 , 0 , true);
    }

    public void A_StopEffect()
    {
        ProcessInput();
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.GetAudioEffectManager().StopEffect(afileId);
    }

    public void A_StopAllEffects()
    {
        ProcessInput();
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.GetAudioEffectManager().StopAllEffects();
    }

    public void A_PreloadEffect()
    {
        ProcessInput();
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.GetAudioEffectManager().PreloadEffect(afileId, afileName);
    }

    public void A_UnloadEffect()
    {
        ProcessInput();
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.GetAudioEffectManager().UnloadEffect(afileId);
    }

    public void A_PauseEffect()
    {
        ProcessInput();
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.GetAudioEffectManager().PauseEffect(afileId);
    }

    public void A_PauseAllEffects()
    {
        ProcessInput();
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.GetAudioEffectManager().PauseAllEffects();
    }

    public void A_ResumeEffect()
    {
        ProcessInput();
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.GetAudioEffectManager().ResumeEffect(afileId);
    }

    public void A_ResumeAllEffects()
    {
        ProcessInput();
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.GetAudioEffectManager().ResumeAllEffects();
    }

    public void A_StopVolumeOfEffect()
    {
        ProcessInput();
        int sv = int.Parse(volumeDropdown.options[volumeDropdown.value].text);
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.SetVolumeOfEffect(afileId, sv);
    }

    public Text txtVolume;
    public void B_GetEffectsVolume()
    {
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        double vol = mEngine.GetAudioEffectManager().GetEffectsVolume();
        txtVolume.text = "v = " + vol; 
    }

    public void B_SetEffectsVolume()
    {
        int sv = int.Parse(volumeDropdown.options[volumeDropdown.value].text);
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.GetAudioEffectManager().SetEffectsVolume(sv);
    }

    public void C_SetVoiceOnlyModeTrue()
    {
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.GetAudioEffectManager().SetVoiceOnlyMode(true);
    }

    public void C_SetVoiceOnlyModeFalse()
    {
        IRtcEngine mEngine = IRtcEngine.QueryEngine();
        mEngine.GetAudioEffectManager().SetVoiceOnlyMode(false);
    }

}
