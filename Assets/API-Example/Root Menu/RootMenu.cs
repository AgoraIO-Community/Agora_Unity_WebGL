using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class RootMenu : MonoBehaviour
{

    public GameObject menuItemPrefab;
    public GameObject content;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            int index = i;
            if (SceneManager.GetSceneByBuildIndex(i).name != "RootMenuScene")
            {
                GameObject menuItem = (GameObject)Instantiate(menuItemPrefab, content.transform);
                Button b = menuItem.transform.GetChild(0).GetComponent<Button>();
                TMP_Text t = menuItem.transform.GetChild(1).GetComponent<TMP_Text>();
                
                string path = SceneUtility.GetScenePathByBuildIndex(i);
                string[] dirs = path.Split('/');
                string dir = dirs[2];
                string name = dirs[dirs.Length - 1];
                //string name = path.Substring(slash + 1);
                //int dotA = name.LastIndexOf('/');
                t.text = dir;
                
                b.onClick.AddListener(() =>
                {
                    RootMenuControl.rootMenuOn = true;
                    SceneManager.LoadScene(index);
                });
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }


}
