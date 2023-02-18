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
    public InputField appIdInput, tokenInput, channelInput;
    public Text appIdHint;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            int index = i;
            var scene = SceneManager.GetSceneByBuildIndex(i);
            string sceneName = scene.name;
            if (sceneName != "HomeMenu")
            {
                string path = SceneUtility.GetScenePathByBuildIndex(i);
                string[] dirs = path.Split('/');
                string dir = dirs[2];

                if (dir == "Demo")
                {
                    if (dirs[3] == "SceneHelloVideo.unity")
                    {
                        // dual scene demo
                        continue;
                    }
                }

                GameObject menuItem = (GameObject)Instantiate(menuItemPrefab, content.transform);
                Button b = menuItem.transform.GetChild(0).GetComponent<Button>();
                Text t = menuItem.transform.GetChild(1).GetComponent<Text>();

                //string name = dirs[dirs.Length - 1];
                t.text = dir;

                b.onClick.AddListener(() =>
                {
                    if (tokenInput.text != "") appInfo.token = tokenInput.text;
                    if (appIdInput.text != "") appInfo.appID = appIdInput.text;
                    RootMenuControl.instance.channel = channelInput.text;
                    RootMenuControl.rootMenuOn = true;
                    SceneManager.LoadScene(index);
                });
            }
        }
        if (string.IsNullOrEmpty(appInfo.appID))
        {
            appIdHint.text = "<color=red>No AppID</color>";
        }
        else
        {
            appIdHint.text = "<color=green>" + appInfo.appID.Substring(0, 4) + "*************" + "</color>";
        }
        tokenInput.text = appInfo.token;
        channelInput.text = FindObjectOfType<RootMenuControl>().channel;
    }
}
