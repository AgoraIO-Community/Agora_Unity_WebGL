using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RootMenu : MonoBehaviour
{

    public GameObject menuItemPrefab;
    public GameObject content;
    public AppInfoObject appInfo;
    public InputField tokenInput, channelInput;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            int index = i;
            if (SceneManager.GetSceneByBuildIndex(i).name != "HomeMenu")
            {
                GameObject menuItem = (GameObject)Instantiate(menuItemPrefab, content.transform);
                Button b = menuItem.transform.GetChild(0).GetComponent<Button>();
                Text t = menuItem.transform.GetChild(1).GetComponent<Text>();
                
                string path = SceneUtility.GetScenePathByBuildIndex(i);
                string[] dirs = path.Split('/');
                string dir = dirs[2];
                string name = dirs[dirs.Length - 1];
                //string name = path.Substring(slash + 1);
                //int dotA = name.LastIndexOf('/');
                t.text = dir;
                
                b.onClick.AddListener(() =>
                {
                    appInfo.token = tokenInput.text;
                    RootMenuControl.instance.channel = channelInput.text;
                    RootMenuControl.rootMenuOn = true;
                    SceneManager.LoadScene(index);
                });
            }
        }

        tokenInput.text = appInfo.token;
        channelInput.text = FindObjectOfType<RootMenuControl>().channel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }


}
