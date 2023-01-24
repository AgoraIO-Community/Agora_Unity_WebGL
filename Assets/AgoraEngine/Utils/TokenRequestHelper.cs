using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


namespace agora_utilities
{
    [Serializable]
    public class TokenObject
    {
        public string rtcToken;
        public string rtmToken;
    }

    [Serializable]
    public class RtcToken
    {
        public string rtcToken;
    }

    [Serializable]
    public class TokenServerPingResult
    {
        public string message;
    }

    /// <summary>
    ///    The helper class gets the token from a server endpoint conformed to
    /// format like this:
    ///   http://localhost:8080/rte/testing/publisher/uid/1234/?3600
    /// See the GitHub project for compatible server code in GoLang:
    ///   https://github.com/AgoraIO-Community/agora-token-service
    /// </summary>
    public static class TokenRequestHelper
    {
        const string RteTokenEndPointFormatter = "{0}/rte/{1}/{2}/uid/{3}/?expiry={4}";
        const string RtcTokenEndPointFormatter = "{0}/rtc/{1}/{2}/uid/{3}/?expiry={4}";
        const string ServerPingRequest = "{0}/ping";

        /// <summary>
        ///    Fetch both RTC and RTM tokens
        /// </summary>
        /// <param name="url">Token Server URL</param>
        /// <param name="channel">channel name</param>
        /// <param name="userId">uid</param>
        /// <param name="role">Publisher or Subscriber</param>
        /// <param name="expireSecs">time to expire in seconds</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static IEnumerator FetchTokens(
            string url, string channel, uint userId, string role, int expireSecs, Action<string, string> callback = null
        )
        {
            var query_url = string.Format(RteTokenEndPointFormatter, url, channel, role, userId, expireSecs);
            Debug.Log("Query:" + query_url);
            UnityWebRequest request = UnityWebRequest.Get(query_url);

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                callback(null, null);
                yield break;
            }

            TokenObject tokenInfo = JsonUtility.FromJson<TokenObject>(
              request.downloadHandler.text
            );

            callback(tokenInfo.rtcToken, tokenInfo.rtmToken);
        }

        public static IEnumerator FetchRtcToken(
    string url, string channel, uint userId, string role, int expireSecs, Action<string> callback = null
)
        {
            var query_url = string.Format(RtcTokenEndPointFormatter, url, channel, role, userId, expireSecs);
            Debug.Log("Query:" + query_url);
            UnityWebRequest request = UnityWebRequest.Get(query_url);

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                callback(null);
                yield break;
            }

            RtcToken tokenInfo = JsonUtility.FromJson<RtcToken>(
              request.downloadHandler.text
            );

            callback(tokenInfo.rtcToken);
        }

        public static IEnumerator PingServer(string url, Action<TokenServerPingResult> callback)
        {
            UnityWebRequest request = UnityWebRequest.Get(string.Format(ServerPingRequest, url));
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                callback(null);
                yield break;
            }
            TokenServerPingResult result = JsonUtility.FromJson<TokenServerPingResult>(
                              request.downloadHandler.text
            );

            callback(result);
        }
    }

}
