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
        public abstract int SetVideoRenderMode(VIDEO_RENDER_MODE _renderMode);

         // load data to texture
        public abstract int UpdateTexture(int tex, uint uid, IntPtr data, ref int width, ref int height);

        public abstract int UpdateVideoRawData(uint uid, IntPtr data, ref int width, ref int height);   

        /**
         * create Native texture and return textureId.
         */
        public abstract int GenerateNativeTexture();
        
        /**
         * Delete native texture according to the textureId.
         */
        public abstract void DeleteTexture(int tex);

        public abstract void AddUserVideoInfo(uint userId, uint textureId);

        public abstract void RemoveUserVideoInfo(uint _userId);

        public abstract void AddUserVideoInfo(string channelId, uint _userId, uint _textureId);
        
        public abstract void RemoveUserVideoInfo(string channelId, uint _userId);

        public abstract int UpdateVideoRawData(string channelId, uint uid, IntPtr data, ref int width, ref int height);

        public abstract bool GetMultiChannelWanted();
    }

    public sealed class VideoRender : IVideoRender
    {
        private static VideoRender _videoRenderInstance = null;
        private IRtcEngine _rtcEngine;

        private VideoRender(IRtcEngine rtcEngine)
        {
            _rtcEngine = rtcEngine;
        }

        public static VideoRender GetInstance(IRtcEngine rtcEngine)
        {
            if (_videoRenderInstance == null)
            {
                _videoRenderInstance = new VideoRender(rtcEngine);
            }
            return _videoRenderInstance;
        }

        public static void ReleaseInstance()
		{
			_videoRenderInstance = null;
		}

        public void SetEngine(IRtcEngine rtcEngine)
        {
            _rtcEngine = rtcEngine;
        }

        public override int SetVideoRenderMode(VIDEO_RENDER_MODE _renderMode)
        {
            if (_rtcEngine == null)
			    return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.setRenderMode((int)_renderMode);
        }

        public override int UpdateVideoRawData(uint uid, IntPtr data, ref int width, ref int height)
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

        public override int UpdateVideoRawData(string channelId, uint uid, IntPtr data, ref int width, ref int height)
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
        public override int UpdateTexture(int tex, uint uid, IntPtr data, ref int width, ref int height)
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

        public override void AddUserVideoInfo(uint userId, uint textureId)
        {
            if (_rtcEngine == null)
                    return;

            IRtcEngineNative.addUserVideoInfo(userId, textureId);
        }

        public override void RemoveUserVideoInfo(uint _userId)
        {
            if (_rtcEngine == null)
                return;

            IRtcEngineNative.removeUserVideoInfo(_userId);
        }

        public override void AddUserVideoInfo(string channelId, uint _userId, uint _textureId)
        {
           if (_rtcEngine == null)
                return;

            IRtcEngineNative.addUserVideoInfo2(channelId, _userId, _textureId);
        }

        public override void RemoveUserVideoInfo(string channelId, uint _userId)
        {
           if (_rtcEngine == null)
                return;

            IRtcEngineNative.removeUserVideoInfo2(channelId, _userId);
        }

        public override int GenerateNativeTexture()
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.generateNativeTexture();
        }

        public override void DeleteTexture(int tex)
        {
            if (_rtcEngine == null)
                return;
                
            IRtcEngineNative.deleteTexture(tex);
        }

        public override bool GetMultiChannelWanted()
        {
            if (_rtcEngine == null)
                return false;

            return IRtcEngineNative.getMultiChannelWanted();
        }
    }
}