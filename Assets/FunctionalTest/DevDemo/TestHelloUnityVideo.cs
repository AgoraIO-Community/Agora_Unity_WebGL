using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using agora_utilities;
using System.Collections.Generic;

namespace agora_gs_test
{
    // this is an example of using Agora Unity SDK
    // It demonstrates:
    // How to enable video
    // How to join/leave channel
    // 
    public class TestHelloUnityVideo
    {

        // instance of agora engine
        private IRtcEngine mRtcEngine;
        private Text MessageText;

        public List<uint> listOfUsers = new List<uint>();
        public uint myUID;
        public string myChannelName = string.Empty;
        // load agora engine
        public void loadEngine(string appId)
        {
            // start sdk
            Debug.Log("initializeEngine");

            if (mRtcEngine != null)
            {
                Debug.Log("Engine exists. Please unload it first!");
                return;
            }

            // init engine
            mRtcEngine = IRtcEngine.GetEngine(appId);

            // enable log
            mRtcEngine.SetLogFilter(LOG_FILTER.DEBUG | LOG_FILTER.INFO | LOG_FILTER.WARNING | LOG_FILTER.ERROR | LOG_FILTER.CRITICAL);
        }

        public void join(string channel)
        {
            Debug.Log("calling join (channel = " + channel + ")");

            if (mRtcEngine == null)
                return;

            // set callbacks (optional)
            mRtcEngine.OnJoinChannelSuccess = onJoinChannelSuccess;
            mRtcEngine.OnUserJoined = onUserJoined;
            mRtcEngine.OnUserOffline = onUserOffline;
            mRtcEngine.OnLeaveChannel = OnLeaveChannelHandler;


            mRtcEngine.OnWarning = (int warn, string msg) =>
            {
                // Debug.LogWarningFormat("Warning code:{0} msg:{1}", warn, IRtcEngine.GetErrorDescription(warn));
            };
            mRtcEngine.OnError = HandleError;

            // enable video
            mRtcEngine.EnableVideo();
            // allow camera output callback
            mRtcEngine.EnableVideoObserver();

            // join channel
            mRtcEngine.JoinChannel(channel, "", 0);
            //mRtcEngine.JoinChannelByKey("006b16bc9253a014a10a5c8d246601ff843IACB/6GKJn6FlNbvItSFFHYRIMoJMdmkLyzpyOQwn1P074H/KYQAAAAAEABsKo0tnmPpYAEAAQCdY+lg", channel, "", 0);
            //mRtcEngine.JoinChannelWithUserAccount("", channel, "1256523654");

        }

        void OnLeaveChannelHandler(RtcStats stats)
        {
            Debug.Log("OnLeaveChannelHandler called");
            Debug.Log(stats.userCount);
        }


        public string getSdkVersion()
        {
            string ver = IRtcEngine.GetSdkVersion();
            return ver;
        }

        public void leave()
        {
            Debug.Log("calling leave");

            if (mRtcEngine == null)
                return;

            // leave channel
            mRtcEngine.LeaveChannel();
            // deregister video frame observers in native-c code
            mRtcEngine.DisableVideoObserver();
        }

        // unload agora engine
        public void unloadEngine()
        {
            Debug.Log("calling unloadEngine");

            // delete
            if (mRtcEngine != null)
            {
                IRtcEngine.Destroy();  // Place this call in ApplicationQuit
                mRtcEngine = null;
            }
        }

        public void EnableVideo(bool pauseVideo)
        {
            if (mRtcEngine != null)
            {
                if (!pauseVideo)
                {
                    mRtcEngine.EnableVideo();
                }
                else
                {
                    mRtcEngine.DisableVideo();
                }
            }
        }

        // accessing GameObject in Scnene1
        // set video transform delegate for statically created GameObject
        public void onSceneHelloVideoLoaded()
        {
            //Attach the SDK Script VideoSurface for video rendering

            GameObject text = GameObject.Find("MessageText");
            if (!ReferenceEquals(text, null))
            {
                MessageText = text.GetComponent<Text>();
            }

            //join("unity3d");

        }

        // implement engine callbacks
        private void onJoinChannelSuccess(string channelName, uint uid, int elapsed)
        {

            Debug.Log("#################### onJoinChannelSuccess ###################" + uid);
            Debug.Log("JoinChannelSuccessHandler: uid = " + uid);
            GameObject textVersionGameObject = GameObject.Find("VersionText");
            myUID = uid;
            myChannelName = channelName;

            //textVersionGameObject.GetComponent<Text>().text = "SDK Version : " + getSdkVersion();


            GameObject quad = GameObject.Find("Quad");
            if (ReferenceEquals(quad, null))
            {
                Debug.Log("failed to find Quad");
                return;
            }
            else
            {
                if (quad.GetComponent<VideoSurface>())
                {
                    quad.GetComponent<VideoSurface>().SetEnable(true);
                }
                else
                {
                    VideoSurface vs = quad.AddComponent<VideoSurface>();
                    vs.SetEnable(true);
                    Debug.Log("#################### create videosurface ###################" + uid);
                }

            }

            GameObject cube = GameObject.Find("Cube");
            if (ReferenceEquals(cube, null))
            {
                Debug.Log("failed to find Quad2");
                return;
            }
            else
            {
                if (cube.GetComponent<VideoSurface>())
                {
                    cube.GetComponent<VideoSurface>().SetEnable(true);
                }
                else
                {
                    VideoSurface vs = cube.AddComponent<VideoSurface>();
                    vs.SetEnable(true);
                    Debug.Log("#################### create videosurface ###################" + uid);
                }

            }


        }

        // When a remote user joined, this delegate will be called. Typically
        // create a GameObject to render video on it
        private void onUserJoined(uint uid, int elapsed)
        {
            Debug.Log("onUserJoined: uid = " + uid + " elapsed = " + elapsed);
            // this is called in main thread
            Test_AudioMixing.savedRemoteUserId = uid;

            // find a game object to render video stream from 'uid'
            GameObject go = GameObject.Find(uid.ToString());
            if (!ReferenceEquals(go, null))
            {
                return; // reuse
            }

            // create a GameObject and assign to this new user
            VideoSurface videoSurface = makeImageSurface(uid.ToString());
            listOfUsers.Add(uid);
            if (!ReferenceEquals(videoSurface, null))
            {
                // configure videoSurface
                videoSurface.SetForUser(uid);
                videoSurface.SetEnable(true);
                // TODO: call this before set enable in webgl, creates error
                videoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
                videoSurface.SetGameFps(30);
            }
        }

        public VideoSurface makePlaneSurface(string goName)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Plane);

            if (go == null)
            {
                return null;
            }
            go.name = goName;
            // set up transform
            go.transform.Rotate(-90.0f, 0.0f, 0.0f);
            float yPos = Random.Range(3.0f, 5.0f);
            float xPos = Random.Range(-2.0f, 2.0f);
            go.transform.position = new Vector3(xPos, yPos, 0f);
            go.transform.localScale = new Vector3(0.25f, 0.5f, .5f);

            // configure videoSurface
            VideoSurface videoSurface = go.AddComponent<VideoSurface>();
            return videoSurface;
        }

        private const float Offset = 100;
        public VideoSurface makeImageSurface(string goName)
        {
            GameObject go = new GameObject();

            if (go == null)
            {
                return null;
            }

            go.name = goName;

            // to be renderered onto
            go.AddComponent<RawImage>();

            // make the object draggable
            go.AddComponent<UIElementDragger>();
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas != null)
            {
                go.transform.parent = canvas.transform;
            }
            // set up transform
            go.transform.Rotate(0f, 0.0f, 180.0f);
            //float xPos = Random.Range(Offset - Screen.width / 2f, Screen.width / 2f - Offset);
            //float yPos = Random.Range(Offset, Screen.height / 2f - Offset);
            //go.transform.localPosition = new Vector3(xPos, yPos, 0f);
            // for testing disabled above so i can see screen always
            go.transform.localPosition = new Vector3(10, 10, 0f);
            go.transform.localScale = new Vector3(3f, 4f, 1f);
            //go.transform.localScale = new Vector3(6f, 4f, 1f);

            // configure videoSurface
            VideoSurface videoSurface = go.AddComponent<VideoSurface>();
            return videoSurface;
        }
        // When remote user is offline, this delegate will be called. Typically
        // delete the GameObject for this user
        private void onUserOffline(uint uid, USER_OFFLINE_REASON reason)
        {
            // remove video stream
            Debug.Log("onUserOffline: uid = " + uid + " reason = " + reason);
            // this is called in main thread
            GameObject go = GameObject.Find(uid.ToString());
            if (!ReferenceEquals(go, null))
            {
                Object.Destroy(go);
            }
            listOfUsers.Remove(uid);
        }

        #region Error Handling
        private int LastError { get; set; }
        private void HandleError(int error, string msg)
        {
            if (error == LastError)
            {
                return;
            }

            msg = string.Format("Error code:{0} msg:{1}", error, IRtcEngine.GetErrorDescription(error));

            switch (error)
            {
                case 101:
                    msg += "\nPlease make sure your AppId is valid and it does not require a certificate for this demo.";
                    break;
            }

            Debug.LogError(msg);
            if (MessageText != null)
            {
                if (MessageText.text.Length > 0)
                {
                    msg = "\n" + msg;
                }
                MessageText.text += msg;
            }

            LastError = error;
        }

        #endregion
    }
}
