using System.Runtime.InteropServices;
using AOT;

namespace agora_gaming_rtc
{
	public abstract class IAudioRecordingDeviceManager : IRtcEngineNative
	{
        
		public abstract bool CreateAAudioRecordingDeviceManager();

		public abstract int ReleaseAAudioRecordingDeviceManager();

	 	public abstract int GetAudioRecordingDeviceCount();

        public abstract int GetAudioRecordingDevice(int index, ref string audioRecordingDeviceName, ref string audioRecordingDeviceId);

        public abstract int SetAudioRecordingDevice(string deviceId);

 		public abstract int StartAudioRecordingDeviceTest(int indicationInterval);

 		public abstract int StopAudioRecordingDeviceTest();
        
		public abstract int GetCurrentRecordingDevice(ref string deviceId);
   
	    public abstract	int SetAudioRecordingDeviceVolume(int volume);

        public abstract int GetAudioRecordingDeviceVolume();

        public abstract int SetAudioRecordingDeviceMute(bool mute);

        public abstract bool IsAudioRecordingDeviceMute();
        
		public abstract int GetCurrentRecordingDeviceInfo(ref string deviceName, ref string deviceId);
	}

    /** The definition of AudioRecordingDeviceManager. */
	public sealed class AudioRecordingDeviceManager : IAudioRecordingDeviceManager
    {
		
		private IRtcEngine mEngine = null;
		private static AudioRecordingDeviceManager _audioRecordingDeviceManagerInstance;

		private AudioRecordingDeviceManager(IRtcEngine rtcEngine)
		{
			mEngine = rtcEngine;
		}

		~AudioRecordingDeviceManager() 
		{

		}

		public static AudioRecordingDeviceManager GetInstance(IRtcEngine rtcEngine)
		{
			if (_audioRecordingDeviceManagerInstance == null)
			{
				_audioRecordingDeviceManagerInstance = new AudioRecordingDeviceManager(rtcEngine);
			}
			return _audioRecordingDeviceManagerInstance;
		}

     	public static void ReleaseInstance()
		{
			_audioRecordingDeviceManagerInstance = null;
		}

		// used internally
		public void SetEngine (IRtcEngine engine)
		{
			mEngine = engine;
		}

        /** Create an AudioRecordingDeviceManager instance.
        *
        * @note Ensure that you call {@link agora_gaming_rtc.AudioRecordingDeviceManager.ReleaseAAudioRecordingDeviceManager ReleaseAAudioRecordingDeviceManager} to release this instance after calling this method.
        * 
        * @return 
        * - true: Success.
        * - false: Failure.
        */
		public override bool CreateAAudioRecordingDeviceManager()
		{
			if (mEngine == null)
				return false;

			return IRtcEngineNative.creatAAudioRecordingDeviceManager();
		}
        /** Release an AudioRecordingDeviceManager instance.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
		public override int ReleaseAAudioRecordingDeviceManager()
		{
			if (mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

			return IRtcEngineNative.releaseAAudioRecordingDeviceManager();
		}

        /** Retrieves the total number of the indexed audio capturing devices in the system.
        * 
        * @return Total number of the indexed audio capturing devices.
        */
	 	public override int GetAudioRecordingDeviceCount()
		{
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

#if !UNITY_EDITOR && UNITY_WEBGL
            return AgoraWebGLEventHandler.GetCacheManager().GetRecordingDeviceCount();
#else
            return IRtcEngineNative.getAudioRecordingDeviceCount();
#endif
		}

        /** Retrieves the audio capturing device associated with the index.
        *         
        * After calling this method, the SDK retrieves the device name and device ID according to the index.
        * 
        * @note Call {@link agora_gaming_rtc.AudioRecordingDeviceManager.GetAudioRecordingDeviceCount GetAudioRecordingDeviceCount} before this method.
        * 
        * @param index The index of the capturing device in the system. The value of `index` is associated with the number of the capturing device which is retrieved from `GetAudioRecordingDeviceCount`. For example, when the number of capturing devices is 3, the value range of `index` is [0,2].
        * @param audioRecordingDeviceName The name of the capturing device for the corresponding index.
        * @param audioRecordingDeviceId The ID of the capturing device for the corresponding index.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
		public override int GetAudioRecordingDevice(int index, ref string audioRecordingDeviceName, ref string audioRecordingDeviceId)
		{
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

#if !UNITY_EDITOR && UNITY_WEBGL

            if (index >= 0 && index < GetAudioRecordingDeviceCount())
            {   
                AgoraWebGLEventHandler.GetCacheManager().GetRecordingAudioDevice(index, ref audioRecordingDeviceName, ref audioRecordingDeviceId);
                return 0;
            }
            else
            {
                return (int)ERROR_CODE.ERROR_INVALID_ARGUMENT;
            }
#else

            if (index >= 0 && index < GetAudioRecordingDeviceCount())
            {
                System.IntPtr audioRecordingDeviceNamePtr = Marshal.AllocHGlobal(512);
                System.IntPtr audioRecordingDeviceIdPtr = Marshal.AllocHGlobal(512);
                int ret = IRtcEngineNative.getAudioRecordingDevice(index, audioRecordingDeviceNamePtr, audioRecordingDeviceIdPtr);
                audioRecordingDeviceName = Marshal.PtrToStringAnsi(audioRecordingDeviceNamePtr);
                audioRecordingDeviceId = Marshal.PtrToStringAnsi(audioRecordingDeviceIdPtr);
                Marshal.FreeHGlobal(audioRecordingDeviceNamePtr);
                Marshal.FreeHGlobal(audioRecordingDeviceIdPtr);
                return ret;
            }
            else
            {
                return (int)ERROR_CODE.ERROR_INVALID_ARGUMENT;
            }
#endif


        }

        /** Retrieves the device ID of the current audio capturing device.
        * 
        * @param deviceId The device ID of the current audio capturing device.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
		public override int GetCurrentRecordingDevice(ref string deviceId)
        {
			if (mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;


#if !UNITY_EDITOR && UNITY_WEBGL
            deviceId = AgoraWebGLEventHandler.GetCacheManager().getCurrentAudioDevice();
            return 0;
#else

            if (GetAudioRecordingDeviceCount() > 0)
            {
                System.IntPtr recordingDeviceId = Marshal.AllocHGlobal(512);
                int ret = getCurrentRecordingDevice(recordingDeviceId);
                deviceId = Marshal.PtrToStringAnsi(recordingDeviceId);
                Marshal.FreeHGlobal(recordingDeviceId);
                return ret;
            }
            else
            {
                return (int)ERROR_CODE.ERROR_NO_DEVICE_PLUGIN;
            }
#endif

        }
		
        /** Sets the volume of the current audio capturing device.
        * 
        * @param volume The volume of the current audio capturing device. The value ranges between 0 (lowest volume) and 255 (highest volume).
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
		public override	int SetAudioRecordingDeviceVolume(int volume)
		{
			if (mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setAudioRecordingDeviceVolume(volume);
            return 0;
#else

            return IRtcEngineNative.setAudioRecordingDeviceVolume(volume);
#endif

        }

        /** Retrieves the volume of the current audio capturing device.
        * 
        * @return
        * - &ge; 0: The volume of the current audio capturing device, if this method call succeeds.
        * - < 0: Failure.
        */
        public override int GetAudioRecordingDeviceVolume()
		{
			if (mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

#if !UNITY_EDITOR && UNITY_WEBGL
            int f = IRtcEngineNative.getAudioRecordingDeviceVolume();
            return f;
#else

            return IRtcEngineNative.getAudioRecordingDeviceVolume();
#endif

        }

        /** Sets whether to stop audio capturing.
        * 
        * @param mute Sets whether to stop audio capturing.
        * - true: Stops.
        * - false: Doesn't stop.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int SetAudioRecordingDeviceMute(bool mute)
		{
			if (mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setAudioRecordingDeviceMute(mute);
            return 0;
#else

            return IRtcEngineNative.setAudioRecordingDeviceMute(mute);
#endif

        }

        /** Gets the status of the current audio capturing device.
        * 
        * @return Whether the current audio capturing device stops audio capturing.
        * - true: Stops.
        * - false: Doesn't stop.
        */
        public override bool IsAudioRecordingDeviceMute()
		{
			if (mEngine == null)
				return false;

#if !UNITY_EDITOR && UNITY_WEBGL
            //IRtcEngineNative.setAudioRecordingDeviceMute(mute);
            bool f = IRtcEngineNative.isAudioRecordingDeviceMute();
            //UnityEngine.Debug.Log("IsAudioRecordingDeviceMute: " + f);
            return !f;
#else

            return IRtcEngineNative.isAudioRecordingDeviceMute();
#endif


        }

        /** Sets the audio capturing device using the device ID.
        * 
        * @note 
        * - Call {@link agora_gaming_rtc.AudioRecordingDeviceManager.GetAudioRecordingDevice GetAudioRecordingDevice} before this method.
        * - Plugging or unplugging the audio device does not change the device ID.
        * 
        * @param deviceId Device ID of the audio capturing device, retrieved by calling `GetAudioRecordingDevice`.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
		public override int SetAudioRecordingDevice(string deviceId)
		{	
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setAudioRecordingCollectionDeviceWGL(deviceId);
            return 0;
#else

            return IRtcEngineNative.setAudioRecordingDevice(deviceId);
#endif

        }

        /** Starts the test of the current audio capturing device.
        * 
        * This method tests whether the audio sampling device works properly.
        * 
        * After calling this method, the SDK triggers the
        * {@link agora_gaming_rtc.OnVolumeIndicationHandler OnVolumeIndicationHandler} callback at the time interval set
        * in this method, which reports `uid = 0` and the volume of the sampling device.
        * 
        * @note 
        * - Ensure that you call {@link agora_gaming_rtc.AudioRecordingDeviceManager.StopAudioRecordingDeviceTest StopAudioRecordingDeviceTest} after calling this method.
        * - Call this method before joining a channel.
        * - This method is for Windows and macOS only.
        * 
        * @param indicationInterval The time interval (ms) at which the `OnVolumeIndicationHandler` callback returns. We
        * recommend a setting greater than 200 ms. This value must not be less than 10 ms; otherwise, you cannot receive
        * the `OnVolumeIndicationHandler` callback.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int StartAudioRecordingDeviceTest(int indicationInterval)
		{
			if (mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

			return IRtcEngineNative.startAudioRecordingDeviceTest(indicationInterval);
		}

        /** Stops the test of the current audio capturing device.
        * 
        * @note Ensure that you call this method to stop the test after calling {@link agora_gaming_rtc.AudioRecordingDeviceManager.StartAudioRecordingDeviceTest StartAudioRecordingDeviceTest}.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
 		public override int StopAudioRecordingDeviceTest()
		{
			if (mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

			return IRtcEngineNative.stopAudioRecordingDeviceTest();
		}

        /** Retrieves the device information of the current audio capturing device.
        * 
        * @param deviceName The device name of the current audio capturing device.
        * @param deviceId The device ID of the current audio capturing device.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
		public override int GetCurrentRecordingDeviceInfo(ref string deviceName, ref string deviceId)
		{
			if (mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;


#if !UNITY_EDITOR && UNITY_WEBGL
            
            string cdeviceId = AgoraWebGLEventHandler.GetCacheManager().getCurrentAudioDevice();
            MediaDeviceInfo info = AgoraWebGLEventHandler.GetCacheManager().getAudioDeviceByDeviceId(cdeviceId);
            if( info != null )
            {
                deviceId = cdeviceId;
                deviceName = info.label;
            }
            return 0;
            
#else
            if (GetAudioRecordingDeviceCount() > 0)
            {
                System.IntPtr audioRecordingDeviceNamePtr = Marshal.AllocHGlobal(512);
                System.IntPtr audioRecordingDeviceIdPtr = Marshal.AllocHGlobal(512);
                int ret = IRtcEngineNative.getCurrentRecordingDeviceInfo(audioRecordingDeviceNamePtr, audioRecordingDeviceIdPtr);
                deviceName = Marshal.PtrToStringAnsi(audioRecordingDeviceNamePtr);
                deviceId = Marshal.PtrToStringAnsi(audioRecordingDeviceIdPtr);
                Marshal.FreeHGlobal(audioRecordingDeviceNamePtr);
                Marshal.FreeHGlobal(audioRecordingDeviceIdPtr);
                return ret;
            }
            else
            {
                return (int)ERROR_CODE.ERROR_NO_DEVICE_PLUGIN;
            }
#endif


        }
	}
}