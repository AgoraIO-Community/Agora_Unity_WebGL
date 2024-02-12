using UnityEngine;
using UnityEngine.UI;

public class AudioVideoStates
{
    public bool subAudio;
    public bool subVideo;
    public bool pubAudio;
    public bool pubVideo;
}

public class AudioVideoStateControl : MonoBehaviour
{
    public Toggle toggleSubAudio;
    public Toggle toggleSubVideo;
    public Toggle togglePubAudio;
    public Toggle togglePubVideo;
}
