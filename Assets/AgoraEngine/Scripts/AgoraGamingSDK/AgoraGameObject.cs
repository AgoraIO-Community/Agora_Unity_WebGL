using UnityEngine;
using System.Collections;

namespace agora_gaming_rtc
{
    public class AgoraGameObject : MonoBehaviour
    {
        void OnApplicationQuit()
        {
            IRtcEngine.Destroy();
        }
    }
}