using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSettings : MonoBehaviour
{

    public SpatialAudioDemoManager.NPCEffectParams tempParams;
    public static NPCSettings instance;

    public Text avatarText;
    public Toggle blur, airAbsorb;
    public Slider attenuation;

    public GameObject panel;
    public bool isOn;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (panel.activeInHierarchy)
        {
            
        }
    }

    public void enableParamWindow(bool enabled, SpatialAudioDemoManager.NPCEffectParams temp)
    {
        isOn = !isOn;
        if (isOn)
        {
            tempParams = temp;
            panel.gameObject.SetActive(isOn);
            avatarText.text = "Avatar: " + tempParams.name;
            blur.isOn = tempParams.blur;
            airAbsorb.isOn = tempParams.airAbsorb;
            attenuation.value = (float)tempParams.attenuation;
        } else
        {
            SpatialAudioDemoManager.demo.updateNPC(tempParams);
            panel.gameObject.SetActive(isOn);
        }
    }

    public void enableParamWindow(bool enabled)
    {
        isOn = !isOn;
        if (isOn)
        {
            panel.gameObject.SetActive(isOn);
            avatarText.text = "Avatar: " + tempParams.name;
            blur.isOn = tempParams.blur;
            airAbsorb.isOn = tempParams.airAbsorb;
            attenuation.value = (float)tempParams.attenuation;
        }
        else
        {
            tempParams.blur = blur.isOn;
            tempParams.airAbsorb = airAbsorb.isOn;
            tempParams.attenuation = attenuation.value;
            SpatialAudioDemoManager.demo.updateNPC(tempParams);
            panel.gameObject.SetActive(isOn);
        }
        Debug.Log("Enabling Window...." + isOn.ToString());
    }

    public void disableParamWindow()
    {
        isOn = false;
        Debug.Log("Enabling Window...." + isOn.ToString());
        tempParams.blur = blur.isOn;
        tempParams.airAbsorb = airAbsorb.isOn;
        tempParams.attenuation = attenuation.value;
        SpatialAudioDemoManager.demo.updateNPC(tempParams);
        panel.gameObject.SetActive(false);
    }

    public void cancelParamWindow()
    {
        isOn = false;
        panel.gameObject.SetActive(false);
    }


}
