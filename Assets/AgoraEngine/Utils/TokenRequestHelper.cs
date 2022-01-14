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
        const string ServerPingRequest = "{0}/ping";

        public static IEnumerator FetchToken(
            string url, string channel, uint userId, string role, int expireSecs, Action<TokenObject> callback = null
        )
        {
            UnityWebRequest request = UnityWebRequest.Get(string.Format(
              RteTokenEndPointFormatter, url, channel, role, userId, expireSecs
            ));
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                callback(null);
                yield break;
            }

            TokenObject tokenInfo = JsonUtility.FromJson<TokenObject>(
              request.downloadHandler.text
            );

            callback(tokenInfo);
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
