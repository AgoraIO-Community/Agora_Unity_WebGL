using System;


namespace agora_gaming_rtc
{
    public abstract class IVideoRender : IRtcEngineNative
    {
        /**
		 * choose the rendreMode of video.
		 * 1:  VIDEO_RENDER_MODE.RENDER_RAWDATA
         * this way can support any Unity Graphic API
         *
         * 2: VIDEO_RENDER_MODE.REDNER_OPENGL_ES2
         * this way only support openGLES2 and do not support multiTherad Rendering.
         *
         * 3: VIDEO_RENDER_MODE.RENDER_UNITY_LOW_LEVEL_INTERFACE
         * this way use Unity Low level native Interface to render video.
         *
		 * @return return effect volume
		 */
        internal abstract int SetVideoRenderMode(VIDEO_RENDER_MODE _renderMode);

         // load data to texture
        internal abstract int UpdateTexture(int tex, uint uid, IntPtr data, ref int width, ref int height);

        internal abstract int UpdateVideoRawData(uint uid, IntPtr data, ref int width, ref int height);   

        /**
         * create Native texture and return textureId.
         */
        internal abstract int GenerateNativeTexture();
        
        /**
         * Delete native texture according to the textureId.
         */
        internal abstract void DeleteTexture(int tex);

        internal abstract void AddUserVideoInfo(uint userId, uint textureId);

        internal abstract void RemoveUserVideoInfo(uint _userId);

        internal abstract void AddUserVideoInfo(string channelId, uint _userId, uint _textureId);
        
        internal abstract void RemoveUserVideoInfo(string channelId, uint _userId);

        internal abstract int UpdateVideoRawData(string channelId, uint uid, IntPtr data, ref int width, ref int height);

        internal abstract bool GetMultiChannelWanted();
    }

    /**
    *  The VideoRender class provides internal Unity level of video data processing functions.
    */
    public sealed class VideoRender : IVideoRender
    {
        private static VideoRender _videoRenderInstance = null;
        private IRtcEngine _rtcEngine;

        private VideoRender(IRtcEngine rtcEngine)
        {
            _rtcEngine = rtcEngine;
        }

        internal static VideoRender GetInstance(IRtcEngine rtcEngine)
        {
            if (_videoRenderInstance == null)
            {
                _videoRenderInstance = new VideoRender(rtcEngine);
            }
            return _videoRenderInstance;
        }

        internal static void ReleaseInstance()
		{
			_videoRenderInstance = null;
		}

        internal void SetEngine(IRtcEngine rtcEngine)
        {
            _rtcEngine = rtcEngine;
        }

        internal override int SetVideoRenderMode(VIDEO_RENDER_MODE _renderMode)
        {
            if (_rtcEngine == null)
			    return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.setRenderMode((int)_renderMode);
        }

        internal override int UpdateVideoRawData(uint uid, IntPtr data, ref int width, ref int height)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            int rc = IRtcEngineNative.updateVideoRawData(data, uid);
            if (rc == -1)
                return -1;

            width = (int)rc >> 16;
            height = (int)(rc & 0xffff);
            return 0;
        }

        internal override int UpdateVideoRawData(string channelId, uint uid, IntPtr data, ref int width, ref int height)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            int rc = IRtcEngineNative.updateVideoRawData2(data, channelId, uid);
            if (rc == -1)
                return -1;

            width = (int)rc >> 16;
            height = (int)(rc & 0xffff);
            return 0;
        } 

         // load data to texture
        internal override int UpdateTexture(int tex, uint uid, IntPtr data, ref int width, ref int height)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            int rc = IRtcEngineNative.updateTexture(tex, data, uid);
            if (rc == -1)
                return -1;
            width = (int)rc >> 16;
            height = (int)(rc & 0xffff);
            return 0;
        }

        internal override void AddUserVideoInfo(uint userId, uint textureId)
        {
            if (_rtcEngine == null)
                    return;

            IRtcEngineNative.addUserVideoInfo(userId, textureId);
        }

        internal override void RemoveUserVideoInfo(uint _userId)
        {
            if (_rtcEngine == null)
                return;

            IRtcEngineNative.removeUserVideoInfo(_userId);
        }

        internal override void AddUserVideoInfo(string channelId, uint _userId, uint _textureId)
        {
           if (_rtcEngine == null)
                return;

            IRtcEngineNative.addUserVideoInfo2(channelId, _userId, _textureId);
        }

        internal override void RemoveUserVideoInfo(string channelId, uint _userId)
        {
           if (_rtcEngine == null)
                return;

            IRtcEngineNative.removeUserVideoInfo2(channelId, _userId);
        }

        internal override int GenerateNativeTexture()
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.generateNativeTexture();
        }

        internal override void DeleteTexture(int tex)
        {
            if (_rtcEngine == null)
                return;
                
            IRtcEngineNative.deleteTexture(tex);
        }

        internal override bool GetMultiChannelWanted()
        {
            if (_rtcEngine == null)
                return false;

            return IRtcEngineNative.getMultiChannelWanted();
        }
    }
}