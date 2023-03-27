using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using agora_gaming_rtc;

public class RootMenuPanel : MonoBehaviour
{

    public GameObject panel, hideButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void goBackToMenu()
    {
        IRtcEngine.Destroy();
        SceneManager.LoadScene("HomeMenu");
    }

}
