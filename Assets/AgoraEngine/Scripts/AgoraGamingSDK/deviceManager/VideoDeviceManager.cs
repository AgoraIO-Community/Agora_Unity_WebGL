using System.Runtime.InteropServices;
using System;
using AOT;

namespace agora_gaming_rtc
{
    public abstract class IVideoDeviceManager : IRtcEngineNative
    {

        public abstract bool CreateAVideoDeviceManager();

        public abstract int ReleaseAVideoDeviceManager();

        public abstract int StartVideoDeviceTest(IntPtr hwnd);

        public abstract int StopVideoDeviceTest();
        
        public abstract int GetVideoDeviceCount();

        public abstract int GetVideoDevice(int index, ref string deviceName, ref string deviceId);

        public abstract int SetVideoDevice(string deviceId);

        public abstract int GetCurrentVideoDevice(ref string deviceId);
    }

    /** The definition of the VideoDeviceManager. The APIs of this class are only available on Windows and macOS. */
    public sealed class VideoDeviceManager : IVideoDeviceManager
    {
        private IRtcEngine _mEngine = null;
        private static VideoDeviceManager _videoDeviceManagerInstance = null;

		private VideoDeviceManager (IRtcEngine rtcEngine)
		{
			_mEngine = rtcEngine;
		}

        ~VideoDeviceManager() {
            
        }

        public static VideoDeviceManager GetInstance(IRtcEngine rtcEngine)
        {
            if (_videoDeviceManagerInstance == null)
            {
                _videoDeviceManagerInstance = new VideoDeviceManager(rtcEngine);
            }
            return _videoDeviceManagerInstance;
        }

        public static void ReleaseInstance()
		{
			_videoDeviceManagerInstance = null;
		}

		// used internally
		public void SetEngine (IRtcEngine engine)
		{
			_mEngine = engine;
		}

        /** Create a VideoDeviceManager instance.
        *
        * @note 
        * - Ensure that you call this method after {@link agora_gaming_rtc.IRtcEngine.EnableVideo EnableVideo} or {@link agora_gaming_rtc.IRtcEngine.EnableVideoObserver EnableVideoObserver}.
        * - Ensure that you call {@link agora_gaming_rtc.VideoDeviceManager.ReleaseAVideoDeviceManager ReleaseAVideoDeviceManager} to release this instance after calling this method.
        *
        * @return 
        * - true: Success.
        * - false: Failure.
        */
        public override bool CreateAVideoDeviceManager()
        {
            if (_mEngine == null)
				return false;

            return IRtcEngineNative.createAVideoDeviceManager();
        }
       
        /** Release a VideoDeviceManager instance.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int ReleaseAVideoDeviceManager()
        {
            if (_mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.releaseAVideoDeviceManager();
        }

        /** Starts the video capturing device test.
        * 
        * This method tests whether the video capturing device works properly. Before calling this method, ensure that you have already called the {@link agora_gaming_rtc.IRtcEngine.EnableVideo EnableVideo} method, and the window handle (`hwnd`) parameter is valid.
        *  
        * @note 
        * Ensure that you call {@link agora_gaming_rtc.VideoDeviceManager.StopVideoDeviceTest StopVideoDeviceTest} after calling this method.
        * 
        * @param hwnd The window handle used to display the screen.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int StartVideoDeviceTest(IntPtr hwnd)
        {
            if (_mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.startVideoDeviceTest(hwnd);
        }

        /** Stops the video capturing device test.
        * 
        * @note Ensure that you call this method to stop the test after calling {@link agora_gaming_rtc.VideoDeviceManager.StartVideoDeviceTest StartVideoDeviceTest}.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int StopVideoDeviceTest()
        {
            if (_mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.stopVideoDeviceTest();
        }

        /** Retrieves the total number of the indexed video capturing devices in the system.
        * 
        * @return Total number of the indexed video capturing devices.
        */
        public override int GetVideoDeviceCount()
        {

            if (_mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

#if !UNITY_EDITOR && UNITY_WEBGL
            return AgoraWebGLEventHandler.GetCameraDeviceCount();
#else
     
            return IRtcEngineNative.getVideoDeviceCollectionCount();
#endif
        }

        /** Retrieves the video capturing device associated with the index.
        *         
        * After calling this method, the SDK retrieves the device name and device ID according to the index.
        * 
        * @note Call {@link agora_gaming_rtc.VideoDeviceManager.GetVideoDeviceCount GetVideoDeviceCount} before this method.
        * 
        * @param index The index of the capturing device in the system. The value of `index` is associated with the number of the capturing device which is retrieved from `GetVideoDeviceCount`. For example, when the number of capturing devices is 3, the value range of `index` is [0,2].
        * @param deviceName The name of the capturing device for the corresponding index.
        * @param deviceId The ID of the capturing device for the corresponding index.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int GetVideoDevice(int index, ref string deviceName, ref string deviceId)
        {
            if (_mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

#if !UNITY_EDITOR && UNITY_WEBGL

            if (index >= 0 && index < GetVideoDeviceCount())
            {
                AgoraWebGLEventHandler.GetVideoDevice(index, ref deviceName, ref deviceId);
                return 0;
            }
            else
            {
                return (int)ERROR_CODE.ERROR_INVALID_ARGUMENT;
            }
#else
            if (index >= 0 && index < GetVideoDeviceCount())
            {
                System.IntPtr videoDeviceName = Marshal.AllocHGlobal(512);
                System.IntPtr videoDeviceId = Marshal.AllocHGlobal(512);
                int ret = IRtcEngineNative.getVideoDeviceCollectionDevice(index, videoDeviceName, videoDeviceId);
                deviceName = Marshal.PtrToStringAnsi(videoDeviceName);
                deviceId = Marshal.PtrToStringAnsi(videoDeviceId);
                Marshal.FreeHGlobal(videoDeviceName);
                Marshal.FreeHGlobal(videoDeviceId);
                return ret;
            }
            else
            {
                return (int)ERROR_CODE.ERROR_INVALID_ARGUMENT;
            }
#endif
        }
        
        /** Retrieves the device ID of the current video capturing device.
        * 
        * @param deviceId The device ID of the current video capturing device.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int GetCurrentVideoDevice(ref string deviceId)
        {
            if (_mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

#if !UNITY_EDITOR && UNITY_WEBGL
            if (GetVideoDeviceCount() > 0)
            {
                string vd = AgoraWebGLEventHandler.GetCurrentVideoDevice();
                deviceId = vd;
                return 0;
            }
            else
            {
                return (int)ERROR_CODE.ERROR_NO_DEVICE_PLUGIN;
            }
#else


            if (GetVideoDeviceCount() > 0)
            {
                System.IntPtr videoDeviceId = Marshal.AllocHGlobal(512);
                int ret = IRtcEngineNative.getCurrentVideoDevice(videoDeviceId);
                deviceId = Marshal.PtrToStringAnsi(videoDeviceId);
                Marshal.FreeHGlobal(videoDeviceId);
                return ret;
            }
            else
            {
                return (int)ERROR_CODE.ERROR_NO_DEVICE_PLUGIN;
            }
#endif

        }

        /** Sets the video capturing device using the device ID.
        * 
        * @note 
        * - Call {@link agora_gaming_rtc.VideoDeviceManager.GetVideoDevice GetVideoDevice} before this method.
        * - Plugging or unplugging the video device does not change the device ID.
        * 
        * @param deviceId Device ID of the video capturing device, retrieved by calling `GetVideoDevice`.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int SetVideoDevice(string deviceId)
        {
            if (_mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setVideoDeviceCollectionDeviceWGL(deviceId);
            return 0;
#else

            return IRtcEngineNative.setVideoDeviceCollectionDevice(deviceId);
#endif

        }
    }
}