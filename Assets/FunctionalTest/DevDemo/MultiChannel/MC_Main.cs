using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;
using agora_utilities;
using UnityEngine.UI;

public class MC_Main : MonoBehaviour {


	// NOTE: This feature applies to the live streaming channel profile. Do not use it for the communication profile.
	[SerializeField]
	private string AppID = "your_appid";
	private IRtcEngine mRtcEngine;
    public uint myUID;
    public string myChannelName = string.Empty;
    public GameObject localVideoRawImage;

    void Start()
    {
        userVideos = new List<GameObject>();
    }

    public void CreateClient_1()
    {
        if (mRtcEngine != null)
        {
            Debug.Log("Engine exists. Please unload it first!");
            return;
        }

        // init engine
        mRtcEngine = IRtcEngine.GetEngine(AppID);

        // enable log
        mRtcEngine.SetLogFilter(LOG_FILTER.DEBUG | LOG_FILTER.INFO | LOG_FILTER.WARNING | LOG_FILTER.ERROR | LOG_FILTER.CRITICAL);
    }

    public void SetChannelProfile_1()
    {
        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);
    }

    public void JoinAndPublish_1()
    {
        // set client role first
        mRtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);

        mRtcEngine.OnJoinChannelSuccess = onJoinChannelSuccess;
        //mRtcEngine.OnUserJoined = onUserJoined;
        //mRtcEngine.OnUserOffline = onUserOffline;
        //mRtcEngine.OnLeaveChannel = onLeaveChannelHandler;

        // enable video
        mRtcEngine.EnableVideo();
        // allow camera output callback
        mRtcEngine.EnableVideoObserver();
        // join channel
        mRtcEngine.JoinChannel("unity3d", "", 0);
    }

    private void onJoinChannelSuccess(string channelName, uint uid, int elapsed)
    {
        Debug.Log("JoinChannelSuccessHandler: uid = " + uid);
        
        myUID = uid;
        myChannelName = channelName;
        
        VideoSurface vs = localVideoRawImage.AddComponent<VideoSurface>();
        vs.SetEnable(true);
    }

    AgoraChannel channel2;
    public void CreateClient_2()
    {
        channel2 = mRtcEngine.CreateChannel("unity2d");
        channel2.ChannelOnJoinChannelSuccess = onJoinChannelSuccessHandler2;
    }

    private void onJoinChannelSuccessHandler2(string channelID, uint uid, int elapsed)
    {
        Debug.Log("JoinChannelSuccessHandler: uid = " + uid);

        MakeImageSurface(channelID, uid, true);
    }

    public void JoinChannel_2()
    {
        channel2.JoinChannel("", null, 0, new ChannelMediaOptions(true, true));
    }

    [SerializeField] private Transform videoSpawnPoint;
    [SerializeField] private RectTransform panelContentWindow;
    private List<GameObject> userVideos;
    private const float SPACE_BETWEEN_USER_VIDEOS = 150f;

    void MakeImageSurface(string channelID, uint uid, bool isLocalUser = false)
    {
        if (GameObject.Find(uid.ToString()) != null)
        {
            Debug.Log("A video surface already exists with this uid: " + uid.ToString());
            return;
        }

        // Create my new image surface
        GameObject go = new GameObject();
        go.name = uid.ToString();
        RawImage userVideo = go.AddComponent<RawImage>();
        go.transform.localScale = new Vector3(1, -1, 1);

        // Child it inside the panel scroller
        if (videoSpawnPoint != null)
        {
            go.transform.SetParent(videoSpawnPoint);
        }

        // Update the layout of the panel scrollers
        panelContentWindow.sizeDelta = new Vector2(0, userVideos.Count * SPACE_BETWEEN_USER_VIDEOS);
        float spawnY = userVideos.Count * SPACE_BETWEEN_USER_VIDEOS * -1;

        userVideos.Add(go);

        go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, spawnY);

        VideoSurface videoSurface = go.AddComponent<VideoSurface>();
        if (isLocalUser == false)
        {
            videoSurface.SetForMultiChannelUser(channelID, uid);
        }
    }


}
