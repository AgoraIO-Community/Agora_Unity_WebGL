using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>
    ///    Token Client takes care of the token acquiry and renew the token when time expires
    ///     Caller must provide instance to Rtc and Rtm engine so the event can be handled.
    /// </summary>
    public class TokenClient : MonoBehaviour
    {

        [SerializeField] ClientType clientType;

        [SerializeField] string AppID;

        [SerializeField] string serverURL;

        [SerializeField] int ExpirationSecs = 3600;

        bool _initialized = false;

        IRtcEngine mRtcEngine;

        // Caller class is responsible setting this property
        public IRtcEngine RtcEngine
        {
            get { return mRtcEngine; }
            set
            {
                mRtcEngine = value;
                mRtcEngine.OnTokenPrivilegeWillExpire += OnTokenPrivilegeWillExpireHandler;
            }
        }

        //public RtmClient RtmClient { get; set; }


        uint UID { get; set; }
        string ChannelName { get; set; }

        public static TokenClient Instance { get; protected set; }

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
            if (!string.IsNullOrEmpty(AppID) && serverURL.StartsWith("http"))
            {
                _initialized = true;
            }

            Debug.Assert(!string.IsNullOrEmpty(AppID), "Please set AppID in " + gameObject.name);
        }

        public void SetClient(ClientType type)
        {
            clientType = type;
        }

        public void GetTokens(string channelName, uint uid, OnTokensReceivedHandler handleTokens)
        {
            if (!_initialized)
            {
                Debug.LogError(gameObject.name + " is not initialized! Please check if URL is set.");
                return;
            }

            ChannelName = channelName;
            UID = uid;

            StartCoroutine(TokenRequestHelper.FetchToken(
                        url: serverURL,
                        channel: channelName,
                        userId: uid,
                        role: clientType.ToString(),
                        expireSecs: ExpirationSecs,
                        callback: (tokens) =>
                        {
                            handleTokens(tokens.rtcToken, tokens.rtmToken);
                        }
                        ));
        }

        void OnTokenPrivilegeWillExpireHandler(string token)
        {
            Debug.Log("Token will expire soon, renewing .... ");
            StartCoroutine(TokenRequestHelper.FetchToken(serverURL, ChannelName, UID, clientType.ToString(), ExpirationSecs,
                        this.RenewToken));
        }

        void RenewToken(TokenObject token)
        {
            if (RtcEngine != null) RtcEngine.RenewToken(token.rtcToken);
            //if (RtmClient != null) RtmClient.RenewToken(token.rtmToken);
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
