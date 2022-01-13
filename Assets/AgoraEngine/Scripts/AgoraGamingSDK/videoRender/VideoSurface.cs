using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;
namespace agora_gaming_rtc
{
    /* This example script demonstrates how to attach
    * video content to a GameObject
    * 
    * Agora engine outputs one local preview video and some
    * remote user video. User ID (int) is used to identify
    * these video streams. 0 is used for local preview video
    * stream, and other value stands for remote user video
    * stream.
    */

    /** The definition of AgoraVideoSurfaceType.
    */
    public enum AgoraVideoSurfaceType
    {
        /** 0: (Default) The renderer for rendering 3D GameObject, such as Cube、Cylinder and Plane.*/
        Renderer = 0,
        /** 1: The renderer for rendering Raw Image of the UI components. */
        RawImage = 1,
    };

    /** The definition of VideoSurface. */
    public class VideoSurface : MonoBehaviour
    {

        private System.IntPtr data = Marshal.AllocHGlobal(1920 * 1080 * 4);
        private int defWidth = 0;
        private int defHeight = 0;
        private Texture2D nativeTexture;
        private bool initRenderMode = false;
        private VideoRender videoRender = null;
        private uint videoFilter = 15; // 15 fix me according to the real video frame rate.
        private uint updateVideoFrameCount = 0;
        public bool isMultiChannelWant = false;
        /* only one of the following should be set, depends on VideoSurfaceType */
        private Renderer mRenderer = null;
        private RawImage mRawImage = null;
        private bool _initialized = false;
        public bool _enableFlipHorizontal = false;
        public bool _enableFlipVertical = false;
        public uint videoFps = 30;
        [SerializeField]
        AgoraVideoSurfaceType VideoSurfaceType = AgoraVideoSurfaceType.Renderer;
#if !UNITY_EDITOR && UNITY_WEBGL
        // new addition for WebGL
        // required for getting textures from webgl
        private InSurfaceRenderer inSurfaceRenderer  = new InSurfaceRenderer();
#endif

        // Used to identify user. 0 is for self and non-zero is for remote user
        private uint mUid = 0;
        private string mChannelId = "_0_";

        // Controls the rendering. False for nothing to be rendered.
        private bool mEnable = true;

        void Start()
        {
            // render video
            if (VideoSurfaceType == AgoraVideoSurfaceType.Renderer)
            {
                mRenderer = GetComponent<Renderer>();
            }

            if (mRenderer == null || VideoSurfaceType == AgoraVideoSurfaceType.RawImage)
            {
                mRawImage = GetComponent<RawImage>();
                if (mRawImage != null)
                {
                    // the variable may have been set to default enum but actually it is a RawImage
                    VideoSurfaceType = AgoraVideoSurfaceType.RawImage;
                }
            }

            if (mRawImage == null && mRenderer == null)
            {
                _initialized = false;
                Debug.LogError("Unable to find surface render in VideoSurface component.");
            }
            else
            {
#if UNITY_EDITOR
                // Used to add more light to material if it is too dark.
                UpdateShader();
#endif
                _initialized = true;
            }
        }

        void Update()
        {

#if !UNITY_EDITOR && UNITY_WEBGL
            uint uid = mUid;
            if (mEnable)
            {
                if (IsBlankTexture())
                {
                    nativeTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                    nativeTexture.wrapMode = TextureWrapMode.Clamp;
                    nativeTexture.Apply(false, false);
                    ApplyTexture(nativeTexture);
                }
                
                if (uid != 0)
                {
                    if(isMultiChannelWant)
                    {
                        inSurfaceRenderer.UpdateRemoteTextureMC(mChannelId, uid, nativeTexture);
                    }
                    else
                    {
                        inSurfaceRenderer.UpdateRemoteTexture(uid, nativeTexture);
                    }
                }
                else
                {
                    inSurfaceRenderer.UpdateTexture(nativeTexture);
                }
            }
            else
            {
                if (!IsBlankTexture())
                {
                    ApplyTexture(null);
                }
            }
#else
            UpdateOther();
#endif
        }

        void UpdateOther()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE
            if (updateVideoFrameCount >= videoFps / videoFilter)
            {
                updateVideoFrameCount = 0;
            }
            else
            {
                ++updateVideoFrameCount;
                return;
            }
            // process engine messages (TODO: put in some other place)
            IRtcEngine engine = GetEngine();

            if (engine == null || !_initialized || videoRender == null)
                return;

            // render video
            uint uid = mUid;
            if (mEnable)
            {
                // create texture if not existent
                if (IsBlankTexture())
                {
                    int tmpi = -1;
                    if (isMultiChannelWant)
                    {
                        if (uid == 0)
                        {
                            tmpi = videoRender.UpdateVideoRawData(uid, data, ref defWidth, ref defHeight);
                        }
                        else
                        {
                            tmpi = videoRender.UpdateVideoRawData(mChannelId, uid, data, ref defWidth, ref defHeight);
                        }
                    }
                    else
                    {
                        tmpi = videoRender.UpdateVideoRawData(uid, data, ref defWidth, ref defHeight);
                    }

                    if (tmpi == -1)
                        return;

                    if (defWidth > 0 && defHeight > 0)
                    {
                        try
                        {
                            // create Texture in the first time update data
                            nativeTexture = new Texture2D((int)defWidth, (int)defHeight, TextureFormat.RGBA32, false);
                            nativeTexture.LoadRawTextureData(data, (int)defWidth * (int)defHeight * 4);
                            ApplyTexture(nativeTexture);
                            nativeTexture.Apply();
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogError("Exception e = " + e);
                        }
                    }
                }
                else
                {
                    if (nativeTexture == null)
                    {
                        Debug.LogError("You didn't initialize native texture, please remove native texture and initialize it by agora.");
                        return;
                    }

                    int width = 0;
                    int height = 0;
                    int tmpi = -1;
                    if (isMultiChannelWant)
                    {
                        if (uid == 0)
                        {
                            tmpi = videoRender.UpdateVideoRawData(uid, data, ref width, ref height);
                        }
                        else
                        {
                            tmpi = videoRender.UpdateVideoRawData(mChannelId, mUid, data, ref width, ref height);
                        }
                    }
                    else
                    {
                        tmpi = videoRender.UpdateVideoRawData(uid, data, ref width, ref height);
                    }

                    if (tmpi == -1)
                        return;

                    try
                    {
                        // Condition to check the width & height and set the texture according to necessity.
                        if (width == defWidth && height == defHeight)
                        {

                            nativeTexture.LoadRawTextureData(data, (int)width * (int)height * 4);
                            nativeTexture.Apply();
                        }
                        else
                        {

                            defWidth = width;
                            defHeight = height;
                            nativeTexture.Resize(defWidth, defHeight);
                            nativeTexture.LoadRawTextureData(data, (int)width * (int)height * 4);
                            nativeTexture.Apply();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception e = " + e);
                    }
                }
            }
            else
            {
                if (!IsBlankTexture())
                {
                    ApplyTexture(null);
                }
            }
#endif
        }

        void OnDestroy()
        {
            if (videoRender != null && IRtcEngine.QueryEngine() != null)
            {
                if (isMultiChannelWant)
                {
                    if (mUid == 0)
                    {
                        videoRender.RemoveUserVideoInfo(mUid);
                    }
                    else
                    {
                        videoRender.RemoveUserVideoInfo(mChannelId, mUid);
                    }
                }
                else
                {
                    videoRender.RemoveUserVideoInfo(mUid);
                }
            }

            if (data != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(data);
                data = IntPtr.Zero;
            }

            if (nativeTexture != null)
            {
                Destroy(nativeTexture);
                nativeTexture = null;
            }
            mRenderer = null;
        }

        /** (Future API) Sets the video rendering frame rate.
        * 
        * @note 
        *   High-level logic only.  Non-effective until native code updated in the future.
        * - Ensure that you call this method in the main thread.
        * - Ensure that you call this method before binding VideoSurface.cs.
        * 
        * @param fps The real video refreshing frame rate of the program.
        */
        [Obsolete("Invoking future API will have no effect")]
        public void SetGameFps(uint fps)
        {
            videoFps = fps;
        }

        // call this to render video stream from uid on this game object
        /** Sets the local or remote video.
        * 
        * @note 
        * - Do not call this method and {@link agora_gaming_rtc.VideoSurface.SetForMultiChannelUser SetForMultiChannelUser} together.
        * - Ensure that you call this method in the main thread.
        * - Ensure that you call this method before binding VideoSurface.cs.
        * 
        * @param uid The ID of the remote user, which is retrieved from {@link agora_gaming_rtc.OnUserJoinedHandler OnUserJoinedHandler}. The default value is 0, which means you can see the local video.
        */
        public void SetForUser(uint uid)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            mUid = uid;
#else
            if (IRtcEngine.QueryEngine() != null)
            {
                mUid = uid;
                IRtcEngine.QueryEngine().GetVideoRender().AddUserVideoInfo(mUid, 0);
            }
#endif
        }


        /** Sets the local or remote video of users in multiple channels.
         * 
         * @since v3.0.1
         *
         * @note 
         * - This method only applies to the multi-channel feature.
         * - Do not call this method and {@link agora_gaming_rtc.VideoSurface.SetForUser SetForUser} together.
         * - Ensure that you call this method in the main thread.
         * - Ensure that you call this method before binding VideoSurface.cs.
         * 
         * @param channelId The channel name.
         * @param uid The ID of the remote user, which is retrieved from {@link agora_gaming_rtc.OnUserJoinedHandler OnUserJoinedHandler}. The default value is 0, which means you can see the local video.
         */

        public void SetForMultiChannelUser(string channelId, uint uid)
        {

#if !UNITY_EDITOR && UNITY_WEBGL

            mUid = uid;
            isMultiChannelWant = true;
            mChannelId = channelId;
            mEnable = true;
#else
            if (IRtcEngine.QueryEngine() != null)
            {
                isMultiChannelWant = true;
                mUid = uid;
                mChannelId = channelId;
                IRtcEngine.QueryEngine().GetVideoRender().AddUserVideoInfo(mChannelId, mUid, 0);
            }
            else
            {
                Debug.LogError("Please init agora engine first");
            }
#endif
        }

        /** Enables/Disables the mirror mode when renders the Texture.
        * 
        * @note 
        * - Ensure that you call this method in the main thread.
        * - Ensure that you call this method before binding VideoSurface.cs.
        * 
        * @param enableFlipHorizontal Whether to enable the horizontal mirror mode of Texture.
        * - true: Enable.
        * - false: (Default) Disable.
        * @param enableFlipVertical Whether to enable the vertical mirror mode of Texture.
        * - true: Enable.
        * - false: (Default) Disable.
        */
        public void EnableFilpTextureApply(bool enableFlipHorizontal, bool enableFlipVertical)
        {
            if (_enableFlipHorizontal != enableFlipHorizontal)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                _enableFlipHorizontal = enableFlipHorizontal;
            }
            if (_enableFlipVertical != enableFlipVertical)
            {
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
                _enableFlipVertical = enableFlipVertical;
            }
        }

        /** Set the video renderer type.
        * 
        * @param agoraVideoSurfaceType The renderer type, see AgoraVideoSurfaceType.
        */
        public void SetVideoSurfaceType(AgoraVideoSurfaceType agoraVideoSurfaceType)
        {
            VideoSurfaceType = agoraVideoSurfaceType;
        }

        /** Starts/Stops the video rendering.
        * 
        * @param enable Whether to start/stop the video rendering.
        * - true: (Default) Start.
        * - false: Stop.
        */
        public void SetEnable(bool enable)
        {
            mEnable = enable;
#if !UNITY_EDITOR && UNITY_WEBGL
            uint uid = mUid;
            if(mEnable)
            {
                if (uid != 0)
                {
                    inSurfaceRenderer.initRemote(uid);
                }
            }
#endif
        }

        private IRtcEngine GetEngine()
        {
            agora_gaming_rtc.IRtcEngine engine = agora_gaming_rtc.IRtcEngine.QueryEngine();
            if (!initRenderMode && engine != null)
            {
                videoRender = (VideoRender)engine.GetVideoRender();
                videoRender.SetVideoRenderMode(VIDEO_RENDER_MODE.RENDER_RAWDATA);
                videoRender.AddUserVideoInfo(mUid, 0);
                initRenderMode = true;
            }
            return engine;
        }

        private bool IsBlankTexture()
        {
            if (VideoSurfaceType == AgoraVideoSurfaceType.Renderer)
            {
                // if never assigned or assigned texture is not Texture2D, we will consider it blank and create a new one
                return (mRenderer.material.mainTexture == null || !(mRenderer.material.mainTexture is Texture2D));
            }
            else if (VideoSurfaceType == AgoraVideoSurfaceType.RawImage)
            {
                return (mRawImage.texture == null);
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        ///  Fetch the texture from Native and display Image Data. Method posts the relevant information to Surface renderer.
        /// </summary>
        private void ApplyTexture(Texture2D texture)
        {
            if (VideoSurfaceType == AgoraVideoSurfaceType.Renderer)
            {
                // in case of its called before start like we are doing now
                // we are attaching it dynamically and enabling it instantly
                // in that case start is called after apply texture and it fails
                // because cant find renderer
#if !UNITY_EDITOR && UNITY_WEBGL
                if (mRenderer == null)
                {
                    mRenderer = GetComponent<Renderer>();
                }
#endif
                mRenderer.material.mainTexture = texture;
            }
            else if (VideoSurfaceType == AgoraVideoSurfaceType.RawImage)
            {
#if !UNITY_EDITOR && UNITY_WEBGL
                if( mRawImage == null )
                {
                    mRawImage = GetComponent<RawImage>();
                }
#endif
                mRawImage.texture = texture;
            }
        }


        // Used to adjust light of Material in case visibility is too dark
        private void UpdateShader()
        {
            MeshRenderer mesh = GetComponent<MeshRenderer>();
            if (mesh != null)
            {
                mesh.material = new Material(Shader.Find("Unlit/Texture"));
            }
        }
    }
}