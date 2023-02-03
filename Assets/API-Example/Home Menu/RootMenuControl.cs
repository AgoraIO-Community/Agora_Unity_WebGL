using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RootMenuControl : MonoBehaviour
{
    public static bool rootMenuOn;
    public GameObject controlPrefab;
    public static RootMenuControl instance;
    public string channel;

    // Start is called before the first frame update
    void Start()
    {

        if (instance)
            Destroy(gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject prefab;
        if(scene.name != "HomeMenu" && !GameObject.Find("RootMenuControl"))
            prefab = (GameObject)Instantiate(controlPrefab, FindObjectOfType<Canvas>().transform);
    }

    
}
