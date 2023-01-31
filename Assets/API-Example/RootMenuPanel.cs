using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void hidePanel()
    {
        panel.SetActive(false);
        hideButton.SetActive(true);
    }

    public void showPanel()
    {
        panel.SetActive(true);
        hideButton.SetActive(false);
    }
}
