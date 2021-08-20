using System.Runtime.InteropServices;
using AOT;

namespace agora_gaming_rtc
{
    public abstract class IAudioPlaybackDeviceManager : IRtcEngineNative
    {
        
        public abstract bool CreateAAudioPlaybackDeviceManager();
        
        public abstract int ReleaseAAudioPlaybackDeviceManager();
        
        public abstract int GetAudioPlaybackDeviceCount();
        
        public abstract int GetAudioPlaybackDevice(int index, ref string deviceName, ref string deviceId);
        
        public abstract int SetAudioPlaybackDevice(string deviceId);
           
        public abstract int SetAudioPlaybackDeviceVolume(int volume);
        
        public abstract int GetAudioPlaybackDeviceVolume();
        
        public abstract int SetAudioPlaybackDeviceMute(bool mute);
        
        public abstract bool IsAudioPlaybackDeviceMute();
        
        public abstract int StartAudioPlaybackDeviceTest(string testAudioFilePath);
        
        public abstract int StopAudioPlaybackDeviceTest();
        
        public abstract int GetCurrentPlaybackDevice(ref string deviceId);
        
        public abstract int GetCurrentPlaybackDeviceInfo(ref string deviceName, ref string deviceId);
    }

    /** The definition of AudioPlaybackDeviceManager. */
    public sealed class AudioPlaybackDeviceManager : IAudioPlaybackDeviceManager
    {
        private IRtcEngine mEngine = null;
        private static AudioPlaybackDeviceManager _audioPlaybackDeviceManagerInstance;
        private AudioPlaybackDeviceManager(IRtcEngine rtcEngine)
        {
            mEngine = rtcEngine;
        }

        ~AudioPlaybackDeviceManager() {
        }

        public static AudioPlaybackDeviceManager GetInstance(IRtcEngine rtcEngine)
        {
            if (_audioPlaybackDeviceManagerInstance == null)
            {
                _audioPlaybackDeviceManagerInstance = new AudioPlaybackDeviceManager (rtcEngine);
            }
            return _audioPlaybackDeviceManagerInstance;
        }

        public static void ReleaseInstance()
		{
			_audioPlaybackDeviceManagerInstance = null;
		}

        // used internally
        public void SetEngine(IRtcEngine engine)
        {
            mEngine = engine;
        }
        
        /** Create an AudioPlaybackDeviceManager instance.
        *
        * @note Ensure that you call {@link agora_gaming_rtc.AudioPlaybackDeviceManager.ReleaseAAudioPlaybackDeviceManager ReleaseAAudioPlaybackDeviceManager} to release this instance after calling this method.
        * 
        * @return 
        * - true: Success.
        * - false: Failure.
        */
        public override bool CreateAAudioPlaybackDeviceManager()
        {
            if (mEngine == null)
                return false;
 
            return IRtcEngineNative.creatAAudioPlaybackDeviceManager();
        }

        /** Release an AudioPlaybackDeviceManager instance.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int ReleaseAAudioPlaybackDeviceManager()
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.releaseAAudioPlaybackDeviceManager();
        }

        /** Retrieves the total number of the indexed audio playback devices in the system.
        * 
        * @return Total number of the indexed audio playback devices.
        */
        public override int GetAudioPlaybackDeviceCount()
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

#if !UNITY_EDITOR && UNITY_WEBGL
            UnityEngine.Debug.Log("GetAudioRecordingDeviceCount in 1");
            return AgoraWebGLEventHandler.GetCacheManager().GetPlayBackDeviceCount();
            UnityEngine.Debug.Log("GetAudioRecordingDeviceCount in 2");
#else
            return IRtcEngineNative.getAudioPlaybackDeviceCount();
#endif


        }

        /** Retrieves the audio playback device associated with the index.
        *         
        * After calling this method, the SDK retrieves the device name and device ID according to the index.
        * 
        * @note Call {@link agora_gaming_rtc.AudioPlaybackDeviceManager.GetAudioPlaybackDeviceCount GetAudioPlaybackDeviceCount} before this method.
        * 
        * @param index The index of the playback device in the system. The value of `index` is associated with the number of the playback device which is retrieved from `GetAudioPlaybackDeviceCount`. For example, when the number of playback devices is 3, the value range of `index` is [0,2].
        * @param deviceName The name of the playback device for the corresponding index.
        * @param deviceId The ID of the playback device for the corresponding index.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int GetAudioPlaybackDevice(int index, ref string deviceName, ref string deviceId)
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
            
#if !UNITY_EDITOR && UNITY_WEBGL

            if (index >= 0 && index < GetAudioPlaybackDeviceCount())
            {   
                AgoraWebGLEventHandler.GetCacheManager().GetPlaybackAudioDevice(index, ref deviceName, ref deviceId);
                return 0;
            }
            else
            {
                return (int)ERROR_CODE.ERROR_INVALID_ARGUMENT;
            }
#else

            if (index >= 0 && index < GetAudioPlaybackDeviceCount())
            {
                System.IntPtr playbackDeviceName = Marshal.AllocHGlobal(512);
                System.IntPtr playbackDeviceId = Marshal.AllocHGlobal(512);
                int ret = IRtcEngineNative.getAudioPlaybackDevice(index, playbackDeviceName, playbackDeviceId);
                deviceName = Marshal.PtrToStringAnsi(playbackDeviceName);
                deviceId = Marshal.PtrToStringAnsi(playbackDeviceId);
                Marshal.FreeHGlobal(playbackDeviceName);
                Marshal.FreeHGlobal(playbackDeviceId);
                return ret;
            }
            else
            {
                return (int)ERROR_CODE.ERROR_INVALID_ARGUMENT;
            }
#endif


        }

        /** Retrieves the device ID of the current audio playback device.
        * 
        * @param deviceId The device ID of the current audio playback device.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int GetCurrentPlaybackDevice(ref string deviceId)
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;


#if !UNITY_EDITOR && UNITY_WEBGL
            deviceId = AgoraWebGLEventHandler.GetCacheManager().getCurrentPlaybackDevice();
            return 0;
#else

            if (GetAudioPlaybackDeviceCount() > 0)
            {
                System.IntPtr playbackDeviceId = Marshal.AllocHGlobal(512);
                int ret = getCurrentPlaybackDevice(playbackDeviceId);
                deviceId = Marshal.PtrToStringAnsi(playbackDeviceId);
                Marshal.FreeHGlobal(playbackDeviceId);
                return ret;
            }
            else
            {
                return (int)ERROR_CODE.ERROR_NO_DEVICE_PLUGIN;
            }
#endif

        }

        /** Sets the audio playback device using the device ID.
        * 
        * @note 
        * - Call {@link agora_gaming_rtc.AudioPlaybackDeviceManager.GetAudioPlaybackDevice GetAudioPlaybackDevice} before this method.
        * - Plugging or unplugging the audio device does not change the device ID.
        * 
        * @param deviceId Device ID of the audio playback device, retrieved by calling `GetAudioPlaybackDevice`.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int SetAudioPlaybackDevice(string deviceId)
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;


#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setPlaybackCollectionDeviceWGL(deviceId);
            return 0;
#else

            return IRtcEngineNative.setAudioPlaybackDevice(deviceId);
#endif


        }

        /** Sets the volume of the current audio playback device.
        * 
        * @param volume The volume of the current audio playback device. The value ranges between 0 (lowest volume) and 255 (highest volume).
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int SetAudioPlaybackDeviceVolume(int volume)
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setAudioPlaybackDeviceVolume(volume);
            return 0;
#else

            return IRtcEngineNative.setAudioPlaybackDeviceVolume(volume);
#endif


        }

        /** Retrieves the volume of the current audio playback device.
        * 
        * @return
        * - The volume of the current audio playback device, if this method call succeeds.
        * - < 0: Failure.
        */
        public override int GetAudioPlaybackDeviceVolume()
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;


#if !UNITY_EDITOR && UNITY_WEBGL
            int f = IRtcEngineNative.getAudioPlaybackDeviceVolume();
            return f;
#else

            return IRtcEngineNative.getAudioPlaybackDeviceVolume();
#endif


        }

        /** Sets whether to stop audio playback.
        * 
        * @param mute Sets whether to stop audio playback.
        * - true: Stops.
        * - false: Doesn't stop.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int SetAudioPlaybackDeviceMute(bool mute)
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

#if !UNITY_EDITOR && UNITY_WEBGL
            IRtcEngineNative.setAudioPlaybackDeviceMute(mute);
            return 0;
#else

            return IRtcEngineNative.setAudioPlaybackDeviceMute(mute);
#endif

        }

        /** Retrieves the status of the current audio playback device.
        * 
        * @return Whether the current audio playback device stops audio playback.
        * - true: Stops.
        * - false: Doesn't stop.
        */
        public override bool IsAudioPlaybackDeviceMute()
        {
            if (mEngine == null)
                return false;


#if !UNITY_EDITOR && UNITY_WEBGL

            bool f = IRtcEngineNative.isAudioPlaybackDeviceMute();
            //UnityEngine.Debug.Log("Unity - IsAudioPlaybackDeviceMute = " + f);
            return f;
            
#else

            return IRtcEngineNative.isAudioPlaybackDeviceMute();
#endif


        }

        /** Starts the test of the current audio playback device.
        * 
        * This method tests whether the audio playback device works properly. Once a user starts the test, the SDK plays an audio file 
        * specified by the user. If the user can hear the audio, the playback device works properly.
        * 
        * After calling this method, the SDK triggers the
        * {@link agora_gaming_rtc.OnVolumeIndicationHandler OnVolumeIndicationHandler} callback every 100 ms, which
        * reports `uid = 1` and the volume of the playback device.
        *
        * @note
        * - Ensure that you call {@link agora_gaming_rtc.AudioPlaybackDeviceManager.StopAudioPlaybackDeviceTest StopAudioPlaybackDeviceTest} after calling this method.
        * - Call this method before joining a channel.
        * - This method is for Windows and macOS only.
        * 
        * @param testAudioFilePath The path of the audio file for the audio playback device test in UTF-8:
        * - Supported file formats: wav, mp3, m4a, and aac.
        * - Supported file sample rates: 8000, 16000, 32000, 44100, and 48000 Hz.
        * 
        * @return
        * - 0: Success, and you can hear the sound of the specified audio file.
        * - < 0: Failure.
        */
        public override int StartAudioPlaybackDeviceTest(string testAudioFilePath)
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.startAudioPlaybackDeviceTest(testAudioFilePath);
        }

        /** Stops the test of the current audio playback device.
        * 
        * @note Ensure that you call this method to stop the test after calling {@link agora_gaming_rtc.AudioPlaybackDeviceManager.StartAudioPlaybackDeviceTest StartAudioPlaybackDeviceTest}.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int StopAudioPlaybackDeviceTest()
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
                
            return IRtcEngineNative.stopAudioPlaybackDeviceTest();
        }

        /** Retrieves the device information of the current audio playback device.
        * 
        * @param deviceName The device name of the current audio playback device.
        * @param deviceId The device ID of the current audio playback device.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int GetCurrentPlaybackDeviceInfo(ref string deviceName, ref string deviceId)
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;


#if !UNITY_EDITOR && UNITY_WEBGL
            
            string cdeviceId = AgoraWebGLEventHandler.GetCacheManager().getCurrentPlaybackDevice();
            MediaDeviceInfo info = AgoraWebGLEventHandler.GetCacheManager().getPlaybackDeviceByDeviceId(cdeviceId);
            if( info != null )
            {
                deviceId = cdeviceId;
                deviceName = info.label;
            }
            return 0;
            
#else

            if (GetAudioPlaybackDeviceCount() > 0)
            {
                System.IntPtr playbackDeviceName = Marshal.AllocHGlobal(512);
                System.IntPtr playbackDeviceId = Marshal.AllocHGlobal(512);
                int ret = IRtcEngineNative.getCurrentPlaybackDeviceInfo(playbackDeviceName, playbackDeviceId);
                deviceName = Marshal.PtrToStringAnsi(playbackDeviceName);
                deviceId = Marshal.PtrToStringAnsi(playbackDeviceId);
                Marshal.FreeHGlobal(playbackDeviceName);
                Marshal.FreeHGlobal(playbackDeviceId);
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