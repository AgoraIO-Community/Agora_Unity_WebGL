using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
#if(UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
#endif
using System.Collections;

/// <summary>
///    TestHome serves a game controller object for this application.
/// </summary>
public class TestHome : MonoBehaviour
{

    // Use this for initialization
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList();
#endif
    static TestHelloUnityVideo app = null;

    private string HomeSceneName = "SceneHome";

    private string PlaySceneName = "SceneHelloVideo";

    // PLEASE KEEP THIS App ID IN SAFE PLACE
    // Get your own App ID at https://dashboard.agora.io/
    [SerializeField]
    private string AppID = "your_appid";

    private string ChannelName
    {
        get
        {
            string cached = PlayerPrefs.GetString("ChannelName");
            if (string.IsNullOrEmpty(cached))
            {
                cached = inputField.text;
            }

            return cached;
        }

        set
        {
            PlayerPrefs.SetString("ChannelName", value);
        }
    }

    [SerializeField]
    private InputField inputField;

    void Awake()
    {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
		permissionList.Add(Permission.Microphone);         
		permissionList.Add(Permission.Camera);               
#endif
        // keep this alive across scenes
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        inputField.text = ChannelName;
        CheckAppId();
    }

    void Update()
    {
        CheckPermissions();
    }

    bool _showAppIdPopup = false;

    string newAppID = "";
    private void OnGUI()
    {
        if (_showAppIdPopup)
        {
            GUI.skin.textField.fontSize = 30;
            GUI.skin.button.fontSize = 30;
            int ypos = Screen.height - 80;
            newAppID = GUI.TextField(new Rect(300, ypos, 500, 50), newAppID);
            if (GUI.Button(new Rect(10, ypos, 100, 50), "OK"))
            {
                AppID = newAppID;
                CheckAppId();
                _showAppIdPopup = false;
                newAppID = "";
            }
            if (GUI.Button(new Rect(130, ypos, 150, 50), "Cancel"))
            {
                _showAppIdPopup = false;
            }
        }
    }

    public void NewAppIDPopup()
    {
        _showAppIdPopup = true;
    }

    private void CheckAppId()
    {
        Debug.Assert(AppID.Length > 10, "Please fill in your AppId first on Game Controller object.");
        GameObject go = GameObject.Find("AppIDText");
        if (go != null)
        {
            Text appIDText = go.GetComponent<Text>();
            if (appIDText != null)
            {
                if (string.IsNullOrEmpty(AppID))
                {
                    appIDText.text = "AppID: " + "UNDEFINED!";
                }
                else
                {
                    appIDText.text = "AppID: " + AppID.Substring(0, 4) + "********" + AppID.Substring(AppID.Length - 4, 4);
                }
            }
        }
    }

    /// <summary>
    ///   Checks for platform dependent permissions.
    /// </summary>
    private void CheckPermissions()
    {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
        foreach(string permission in permissionList)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {                 
				Permission.RequestUserPermission(permission);
			}
        }
#endif
    }

    public void onJoinAudience()
    {
        // create app if nonexistent
        if (ReferenceEquals(app, null))
        {
            app = new TestHelloUnityVideo(); // create app
            app.loadEngine(AppID); // load engine
        }

        ChannelName = inputField.text;
        app.joinAudience(ChannelName);
        SceneManager.sceneLoaded += OnLevelFinishedLoading; // configure GameObject after scene is loaded
        SceneManager.LoadScene(PlaySceneName, LoadSceneMode.Single);
    }

    public void onJoinButtonClicked(bool enableVideo, bool muted = false)
    {
        // create app if nonexistent
        if (ReferenceEquals(app, null))
        {
            app = new TestHelloUnityVideo(); // create app
            app.loadEngine(AppID); // load engine
        }

        ChannelName = inputField.text;

        // join channel and jump to next scene
        app.join(ChannelName, enableVideo, muted);
        SceneManager.sceneLoaded += OnLevelFinishedLoading; // configure GameObject after scene is loaded
        SceneManager.LoadScene(PlaySceneName, LoadSceneMode.Single);
    }

    public void onLeaveButtonClicked()
    {
        if (!ReferenceEquals(app, null))
        {
            app.leave(); // leave channel
            app.unloadEngine(); // delete engine
            app = null; // delete app
            SceneManager.LoadScene(HomeSceneName, LoadSceneMode.Single);
        }
        Destroy(gameObject);
    }

    public void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == PlaySceneName)
        {
            if (!ReferenceEquals(app, null))
            {
                app.onSceneHelloVideoLoaded(); // call this after scene is loaded
            }
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }
    }

    void OnApplicationPause(bool paused)
    {
        if (!ReferenceEquals(app, null))
        {
            app.EnableVideo(paused);
        }
    }

    void OnApplicationQuit()
    {
        if (!ReferenceEquals(app, null))
        {
            app.unloadEngine();
        }
    }
}
