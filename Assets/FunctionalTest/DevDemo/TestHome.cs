using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
#if(UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
#endif
using System.Collections;

namespace agora_gs_test
{
    /// <summary>
    ///    TestHome serves a game controller object for this application.
    /// </summary>
    public class TestHome : MonoBehaviour
    {

        // Use this for initialization
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList();
#endif
        public static TestHelloUnityVideo app = null;

        private string HomeSceneName = "SceneHome2";

        private string PlaySceneName = "SceneFuncTests";

        // PLEASE KEEP THIS App ID IN SAFE PLACE
        // Get your own App ID at https://dashboard.agora.io/
        [SerializeField]
        private AppInfoObject appInfo;

        void Awake()
        {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
		permissionList.Add(Permission.Microphone);         
		permissionList.Add(Permission.Camera);               
#endif
            // keep this alive across scenes
            if (!RootMenuControl.instance)
                DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            CheckAppId();
        }

        void Update()
        {
            CheckPermissions();
        }

        private void CheckAppId()
        {
            Debug.Assert(appInfo.appID.Length > 10, "<color=red>[STOP] Please fill in your appId in your AppIDInfo Object!!!! \n (Assets/API-Example/_AppIDInfo/AppIDInfo)</color>");
            GameObject go = GameObject.Find("AppIDText");
            if (go != null)
            {
                Text appIDText = go.GetComponent<Text>();
                if (appIDText != null)
                {
                    if (string.IsNullOrEmpty(appInfo.appID))
                    {
                        appIDText.text = "AppID: " + "UNDEFINED!";
                    }
                    else
                    {
                        appIDText.text = "AppID: " + appInfo.appID.Substring(0, 4) + "********" + appInfo.appID.Substring(appInfo.appID.Length - 4, 4);
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

        public void onJoinButtonClicked()
        {
            // get parameters (channel name, channel profile, etc.)
            GameObject go = GameObject.Find("ChannelName");
            InputField field = go.GetComponent<InputField>();

            // create app if nonexistent
            if (ReferenceEquals(app, null))
            {
                app = new TestHelloUnityVideo(); // create app
                app.loadEngine(appInfo.appID); // load engine
            }

            // join channel and jump to next scene
            app.join(field.text);

            SceneManager.sceneLoaded += OnLevelFinishedLoading; // configure GameObject after scene is loaded
            SceneManager.LoadScene(PlaySceneName, LoadSceneMode.Single);
        }

        public void onLeaveButtonClicked()
        {
            Debug.Log("Leave Button Clicked " + app);
            if (!ReferenceEquals(app, null))
            {
                app.leave(); // leave channel
                app.unloadEngine(); // delete engine
                app = null; // delete app
                SceneManager.LoadScene(HomeSceneName, LoadSceneMode.Single);
            }
            //Destroy(gameObject);
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
}
