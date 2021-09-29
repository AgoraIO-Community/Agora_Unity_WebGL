using UnityEngine;

namespace agora_gaming_rtc
{
    /* Internal script used to render WebglVideo to a VideoSurface */
#if UNITY_WEBGL || UNITY_EDITOR
    public sealed class InSurfaceRenderer : IRtcEngineNative
    {
        public InSurfaceRenderer() { }

        // update local texture of local user
        // takes texture from localVideoTrack
        public void UpdateTexture(Texture tex)
        {
            if (!isLocalVideoReady())
                return;
            if (tex != null)
            {
                updateLocalTexture(tex.GetNativeTexturePtr());
            }
        }

        // test function, unused for now
        public void testMC(string channel, uint userid)
        {
            if(isRemoteVideoReady_MC(channel, userid + ""))
            {
                //Debug.Log("UNITY - > remote video multichannel ready");
            }
            else
            {
                //Debug.Log("UNITY - > remote video multichannel not ready");
            }
        }

        // Update texture of remote user for a multichannel video surface
        public void UpdateRemoteTextureMC(string channel, uint uid, Texture tex)
        {
            if (isRemoteVideoReady_MC(channel, uid + ""))
            {
                updateRemoteTexture_MC(channel, "" + uid, tex.GetNativeTexturePtr());
            }
        }

        // update texture of remote user for single channel video surface
        public void UpdateRemoteTexture(uint uid, Texture tex)
        {
            if (!isRemoteVideoReady(""+uid))
                return;
            updateRemoteTexture(""+uid, tex.GetNativeTexturePtr());
        }

        // initialize remote video surface
        public void initRemote(uint uid)
        {
            createRemoteTexture(""+uid);
        }

        // initialize local video surface
        public void initLocal()
        {
            createLocalTexture();
        }
    }
#endif
}