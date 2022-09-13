using UnityEngine;
using System.Collections;

using agora_gaming_rtc;
//using agora_rtm;

namespace agora_utilities
{
    /// <summary>
    ///     publisher = BROADCASTER role in LiveStreaming mode, or a host in Communication mode
    ///     subscriber = AUDIENCE role in LiveStreaming mode.
    /// </summary>
    public enum ClientType
    {
        publisher,
        subscriber
    }

    public delegate void OnTokensReceivedHandler(string rtcToken, string rtmToken);
    public delegate void OnSingleTokenReceivedHandler(string token);


    /// <summary>
    ///    Token Client takes care of the token acquiry and renew the token when time expires
    ///     Caller must provide instance to Rtc and Rtm engine so the event can be handled.
    /// </summary>
    public class TokenClient : MonoBehaviour
    {

        [SerializeField] ClientType clientType;

        [SerializeField] string serverURL;

        [SerializeField] int ExpirationSecs = 3600;

        IRtcEngine mRtcEngine;

        // Caller class is responsible setting this property
        public IRtcEngine RtcEngine
        {
            get { return mRtcEngine; }
            set
            {
                mRtcEngine = value;
                mRtcEngine.OnTokenPrivilegeWillExpire = OnTokenPrivilegeWillExpireHandler;
                mRtcEngine.OnTokenPrivilegeDidExpire = OnTokenPrivilegeDidExpireHandler;
                mRtcEngine.OnClientRoleChanged += OnClientRoleChangedHandler;
            }
        }

        //public RtmClient RtmClient { get; set; }

        uint UID { get; set; }
        string ChannelName { get; set; }

        public static TokenClient Instance { get; protected set; }
        public bool IsInitialized { get; protected set; }

        void Awake()
        {
            // Ensure Singleton pattern for this class
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
                //throw new System.Exception("An instance of this singleton already exists.");
            }
            else
            {
                Instance = this;
            }

            // keep this alive across scenes
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            // requirements for this module considered initialized
            if (!serverURL.StartsWith("http"))
            {
                Debug.LogWarning("Please specify server URL in the " + gameObject.name + " gameobject, if you wish to use token server.");
                return;
            }

            /*
            // This has issue running on local server.
            // e.g. Eorr: Access to XMLHttpRequest at 'http://localhost:8080/ping' from origin 'http://localhost:3001'
	        // has been blocked by CORS policy: No 'Access-Control-Allow-Origin' header is present on the requested resource.
            StartCoroutine(TokenRequestHelper.PingServer(serverURL, (result) =>
            {
                if (result == null)
                {
                    Debug.LogWarning("Token server ping failure!");
                    return;
                }
                if (result.message == "pong")
                {
                    Debug.Log("Pinging token server successful!");
                    IsInitialized = true;
                }
            }));
            */
            IsInitialized = true;
        }

        public void SetClient(ClientType type)
        {
            clientType = type;
        }

        public void SetMultiChannelInstance(AgoraChannel channel)
        {
            channel.ChannelOnTokenPrivilegeWillExpire = ChannelOnTokenPrivilegeWillExpireHandler;
            channel.ChannelOnTokenPrivilegeDidExpire = ChannelOnTokenPrivilegeDidExpireHandler;
            channel.ChannelOnClientRoleChanged += ChannelOnClientRoleChangedHandler;
        }

        public void SetExpirationSecs(int secs)
        {
            ExpirationSecs = secs;
        }

        public void SetRtcEngineInstance(IRtcEngine engine)
        {
            mRtcEngine = engine;
            mRtcEngine.OnTokenPrivilegeWillExpire = OnTokenPrivilegeWillExpireHandler;
            mRtcEngine.OnTokenPrivilegeDidExpire = OnTokenPrivilegeDidExpireHandler;
            mRtcEngine.OnClientRoleChanged += OnClientRoleChangedHandler;
        }

        public void GetRtcToken(string channelName, uint uid, OnSingleTokenReceivedHandler handleToken)
        {
            if (!serverURL.StartsWith("http"))
            {
                Debug.LogError(gameObject.name + " is not initialized! Please check if URL is set.");
            }
            else
            {
                StartCoroutine(CoGetTokens(channelName, uid, handleToken));
            }
        }

        IEnumerator CoGetTokens(string channelName, uint uid, OnSingleTokenReceivedHandler handleToken)
        {
            yield return new WaitUntil(() => IsInitialized);

            ChannelName = channelName;
            UID = uid;

            StartCoroutine(TokenRequestHelper.FetchRtcToken(
                        url: serverURL,
                        channel: channelName,
                        userId: uid,
                        role: clientType.ToString(),
                        expireSecs: ExpirationSecs,
                        callback: (token) =>
                        {
                            handleToken(token);
                        }
                        ));
        }

        void OnTokenPrivilegeWillExpireHandler(string token)
        {
            Debug.Log("Token will expire soon, renewing .... ");
            StartCoroutine(TokenRequestHelper.FetchRtcToken(serverURL, ChannelName, UID, clientType.ToString(), ExpirationSecs,
                        (myToken) =>
                        {

                            if (mRtcEngine != null)
                            {
                                mRtcEngine.RenewToken(myToken);
                            }
                        }));
        }

        void OnTokenPrivilegeDidExpireHandler(string token)
        {
            Debug.Log("Token has expired, please rejoin to get another token.... ");

        }

        void ChannelOnTokenPrivilegeWillExpireHandler(string channelId, string token)
        {
            Debug.Log("Channel Token will expire soon for " + channelId + ", renewing .... ");
            StartCoroutine(TokenRequestHelper.FetchRtcToken(serverURL, channelId, UID, clientType.ToString(), ExpirationSecs,
                        (myToken) =>
                        {
                            var channel = AgoraChannel.GetChannel(channelId);
                            if (channel != null)
                            {
                                channel.RenewToken(myToken);
                            }
                        }));
        }

        void ChannelOnTokenPrivilegeDidExpireHandler(string channelId, string token)
        {
            Debug.Log("Channel Token has expired for " + channelId + ", join again to renew token");
        }

        void ChannelOnClientRoleChangedHandler(string channelId, CLIENT_ROLE_TYPE oldRole, CLIENT_ROLE_TYPE newRole)
        {
            ClientType client_type = newRole == CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER ? ClientType.publisher : ClientType.subscriber;
            Debug.Log("Channel Token will change for " + channelId + ", renewing from " + oldRole + " to " + newRole);
            StartCoroutine(TokenRequestHelper.FetchRtcToken(serverURL, channelId, UID, client_type.ToString(), ExpirationSecs,
                        (token) =>
                        {
                            var channel = AgoraChannel.GetChannel(channelId);
                            if (channel != null)
                            {
                                channel.RenewToken(token);
                            }
                        }));
        }

        void OnClientRoleChangedHandler(CLIENT_ROLE_TYPE oldRole, CLIENT_ROLE_TYPE newRole)
        {
            clientType = newRole == CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER ? ClientType.publisher : ClientType.subscriber;
            Debug.Log("Client Token will change for " + ChannelName + ", renewing from " + oldRole + " to " + newRole);
            StartCoroutine(TokenRequestHelper.FetchRtcToken(serverURL, ChannelName, UID, clientType.ToString(), ExpirationSecs,
                        this.RenewToken)); ;
        }

        void RenewToken(string token)
        {

            if (RtcEngine != null) RtcEngine.RenewToken(token);
            Debug.Log("RTC token has been renewed.");
        }

        void OnDestroy()
        {
            mRtcEngine = IRtcEngine.QueryEngine();
            if (mRtcEngine != null)
            {
                IRtcEngine.Destroy();
            }
        }
    }
}
