// Shows either a welcome message, only once per session.
//#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace agora_gaming_rtc
{
    static class Welcome
    {
        [InitializeOnLoadMethod]
        static void OnInitializeOnLoad()
        {
            // InitializeOnLoad is called on start and after each rebuild,
            // but we only want to show this once per editor session.
            if (!SessionState.GetBool("AGORA_WELCOME", false))
            {
                SessionState.SetBool("AGORA_WELCOME", true);
                Debug.Log("<color=#099DFD>Agora IO</color> | webgl.agoraguru.net | See Wiki On GibHub");
            }
        }
    }
}
//#endif
