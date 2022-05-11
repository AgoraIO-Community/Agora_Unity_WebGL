using System.Runtime.InteropServices;
using System;
using AOT;

namespace agora_gaming_rtc
{
    public abstract class IAudioRawDataManager : IRtcEngineNative
	{

        public abstract int SetOnRecordAudioFrameCallback(AudioRawDataManager.OnRecordAudioFrameHandler action);

        public abstract int SetOnPlaybackAudioFrameCallback(AudioRawDataManager.OnPlaybackAudioFrameHandler action);
        
        public abstract int SetOnMixedAudioFrameCallback(AudioRawDataManager.OnMixedAudioFrameHandler action);

        public abstract int SetOnPlaybackAudioFrameBeforeMixingCallback(AudioRawDataManager.OnPlaybackAudioFrameBeforeMixingHandler action);

        public abstract int RegisterAudioRawDataObserver();
        
        public abstract int UnRegisterAudioRawDataObserver();

        public abstract int PullAudioFrame(IntPtr audioBuffer, int type, int samples, int bytesPerSample, int channels, int samplesPerSec, long renderTimeMs, int avsync_type);

        public abstract int EnableRawDataPtrCallback(bool enable);
    }

    /** The definition of AudioRawDataManager. */
    public sealed class AudioRawDataManager : IAudioRawDataManager
    {
        private static IRtcEngine _irtcEngine = null;
        private static AudioRawDataManager _audioRawDataManagerInstance = null;
        /** Retrieves the recorded audio frame.
         * 
         * The SDK triggers this callback once every 10 ms.
         * 
         * @param audioFrame See AudioFrame.
         */
        public delegate void OnRecordAudioFrameHandler(AudioFrame audioFrame);
        private OnRecordAudioFrameHandler OnRecordAudioFrame;
        /** Retrieves the audio playback frame.
         * 
         * The SDK triggers this callback once every 10 ms.
         * 
         * @param audioFrame See AudioFrame.
         */
        public delegate void OnPlaybackAudioFrameHandler(AudioFrame audioFrame);
        private OnPlaybackAudioFrameHandler OnPlaybackAudioFrame;
        /** Retrieves the mixed recorded and playback audio frame.
         * 
         * The SDK triggers this callback once every 10 ms.
         * 
         * @param audioFrame See AudioFrame.
         */
        public delegate void OnMixedAudioFrameHandler(AudioFrame audioFrame);
        private OnMixedAudioFrameHandler OnMixedAudioFrame;
        /** Retrieves the audio frame of a specified user before mixing.
         * 
         * The SDK triggers this callback once every 10 ms.
         * 
         * @param uid The user ID.
         * @param audioFrame See AudioFrame.
         */
        public delegate void OnPlaybackAudioFrameBeforeMixingHandler(uint uid, AudioFrame audioFrame);
        private OnPlaybackAudioFrameBeforeMixingHandler OnPlaybackAudioFrameBeforeMixing;

        private static bool enableRawDataPtr = false;

        private AudioRawDataManager(IRtcEngine irtcEngine)
        {
            _irtcEngine = irtcEngine;
        }

        public static AudioRawDataManager GetInstance(IRtcEngine irtcEngine)
        {
            if (_audioRawDataManagerInstance == null)
            {
                _audioRawDataManagerInstance = new AudioRawDataManager(irtcEngine);
            }
            return _audioRawDataManagerInstance;
        }

        public static void ReleaseInstance()
		{
			_audioRawDataManagerInstance = null;
		}

        public void SetEngine(IRtcEngine irtcEngine)
        {
            _irtcEngine = irtcEngine;
        }

         /** Listens for the {@link agora_gaming_rtc.AudioRawDataManager.OnRecordAudioFrameHandler OnRecordAudioFrameHandler} delegate.
         * 
         * @note 
         * - Call this method before calling {@link agora_gaming_rtc.AudioRawDataManager.RegisterAudioRawDataObserver RegisterAudioRawDataObserver}.
         * - If you want to unregister the `OnRecordAudioFrameHandler` delegate, call {@link agora_gaming_rtc.AudioRawDataManager.UnRegisterAudioRawDataObserver UnRegisterAudioRawDataObserver} before calling this method, and set `action` as `null` when calls this method.
         * 
         * @param action The implementation of the `OnRecordAudioFrameHandler` delegate.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public override int SetOnRecordAudioFrameCallback(OnRecordAudioFrameHandler action)
        {
            if (_irtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            if (action == null)
            {
                OnRecordAudioFrame = null;
                IRtcEngineNative.initEventOnRecordAudioFrame(null);
            }
            else
            {
                OnRecordAudioFrame = action;
                IRtcEngineNative.initEventOnRecordAudioFrame(OnRecordAudioFrameCallback);
            }
            return (int)ERROR_CODE.ERROR_OK;
        }

        /** Listens for the {@link agora_gaming_rtc.AudioRawDataManager.OnPlaybackAudioFrameHandler OnPlaybackAudioFrameHandler} delegate.
         * 
         * @note 
         * - Call this method before calling {@link agora_gaming_rtc.AudioRawDataManager.RegisterAudioRawDataObserver RegisterAudioRawDataObserver}.
         * - If you want to unregister the `OnPlaybackAudioFrameHandler` delegate, call {@link agora_gaming_rtc.AudioRawDataManager.UnRegisterAudioRawDataObserver UnRegisterAudioRawDataObserver} before calling this method, and set `action` as `null` when calls this method.
         * 
         * @param action The implementation of the `OnPlaybackAudioFrameHandler` delegate.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public override int SetOnPlaybackAudioFrameCallback(OnPlaybackAudioFrameHandler action)
        {
            if (_irtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            if (action == null)
            {
                OnPlaybackAudioFrame = null;
                IRtcEngineNative.initEventOnPlaybackAudioFrame(null);
            }
            else
            {
                OnPlaybackAudioFrame = action;
                IRtcEngineNative.initEventOnPlaybackAudioFrame(OnPlaybackAudioFrameCallback);
            }
            return (int)ERROR_CODE.ERROR_OK;
        }

        /** Listens for the {@link agora_gaming_rtc.AudioRawDataManager.OnMixedAudioFrameHandler OnMixedAudioFrameHandler} delegate.
         *
         * @note 
         * - Call this method before calling {@link agora_gaming_rtc.AudioRawDataManager.RegisterAudioRawDataObserver RegisterAudioRawDataObserver}.
         * - If you want to unregister the `OnMixedAudioFrameHandler` delegate, call {@link agora_gaming_rtc.AudioRawDataManager.UnRegisterAudioRawDataObserver UnRegisterAudioRawDataObserver} before calling this method, and set `action` as `null` when calls this method.
         * 
         * @param action The implementation of the `OnMixedAudioFrameHandler` delegate.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public override int SetOnMixedAudioFrameCallback(OnMixedAudioFrameHandler action)
        {
            if (_irtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE; 

            if (action == null)
            {
                OnMixedAudioFrame = null;
                IRtcEngineNative.initEventOnMixedAudioFrame(null);
            }
            else
            {
                OnMixedAudioFrame = action;
                IRtcEngineNative.initEventOnMixedAudioFrame(OnMixedAudioFrameCallback);
            }
            return (int)ERROR_CODE.ERROR_OK;
        }

         /** Listens for the {@link agora_gaming_rtc.AudioRawDataManager.OnPlaybackAudioFrameBeforeMixingHandler OnPlaybackAudioFrameBeforeMixingHandler} delegate.
         *
         * @note 
         * - Call this method before calling {@link agora_gaming_rtc.AudioRawDataManager.RegisterAudioRawDataObserver RegisterAudioRawDataObserver}.
         * - If you want to unregister the `OnPlaybackAudioFrameBeforeMixingHandler` delegate, call {@link agora_gaming_rtc.AudioRawDataManager.UnRegisterAudioRawDataObserver UnRegisterAudioRawDataObserver} before calling this method, and set `action` as `null` when calls this method.
         * 
         * @param action The implementation of the `OnPlaybackAudioFrameBeforeMixingHandler` delegate.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public override int SetOnPlaybackAudioFrameBeforeMixingCallback(OnPlaybackAudioFrameBeforeMixingHandler action)
        {
            if (_irtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            if (action == null)
            {
                OnPlaybackAudioFrameBeforeMixing = null;
                IRtcEngineNative.initEventOnPlaybackAudioFrameBeforeMixing(null);
            }
            else
            {
                OnPlaybackAudioFrameBeforeMixing = action;
                IRtcEngineNative.initEventOnPlaybackAudioFrameBeforeMixing(OnPlaybackAudioFrameBeforeMixingCallback);
            }
            return (int)ERROR_CODE.ERROR_OK;
        }

         /** Registers an audio raw data observer.
         * 
         * @note Ensure that you call this method before joining a channel.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public override int RegisterAudioRawDataObserver()
        {
            if (_irtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.registerAudioRawDataObserver();
        }
    
        /** UnRegisters the audio raw data observer.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public override int UnRegisterAudioRawDataObserver()
        {
            if (_irtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.unRegisterAudioRawDataObserver();
        }

        /** Pulls the remote audio data.
         * 
         * Before calling this method, call the {@link agora_gaming_rtc.IRtcEngine.SetExternalAudioSink SetExternalAudioSink(enabled: true)} method to enable and set the external audio sink.
         * 
         * After a successful method call, the app pulls the decoded and mixed audio data for playback.
         * 
         * @note
         * - Once you call the `PullAudioFrame` method successfully, the app will not retrieve any audio data from the {@link agora_gaming_rtc.AudioRawDataManager.OnPlaybackAudioFrameHandler OnPlaybackAudioFrameHandler} callback.
         * - The difference between the `OnPlaybackAudioFrameHandler` callback and the `PullAudioFrame` method is as follows:
         *   - `OnPlaybackAudioFrameHandler`: The SDK sends the audio data to the app through this callback. Any delay in processing the audio frames may result in audio jitter.
         *   - `PullAudioFrame`: The app pulls the remote audio data. After setting the audio data parameters, the SDK adjusts the frame buffer and avoids problems caused by jitter in the external audio playback.
         * 
         * @param audioBuffer The data buffer of the audio frame. When the audio frame uses a stereo channel, the data buffer is interleaved. The size of the data buffer is as follows: `audioBuffer` = `samples` &times; `channels` &times; `bytesPerSample`.
         * @param type The type of the audio frame. See #AUDIO_FRAME_TYPE.
         * @param samples The number of samples per channel in the audio frame.
         * @param bytesPerSample The number of bytes per audio sample, which is usually 16-bit (2-byte).
         * @param channels The number of audio channels.
         * - 1: Mono
         * - 2: Stereo (the data is interleaved)
         * @param samplesPerSec The sample rate.
         * @param renderTimeMs The timestamp of the external audio frame. You can use this parameter for the following purposes:
         * - Restore the order of the captured audio frame.
         * - Synchronize audio and video frames in video-related scenarios, including where external video sources are used.
         * @param avsync_type The reserved parameter.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public override int PullAudioFrame(IntPtr audioBuffer, int type, int samples, int bytesPerSample, int channels, int samplesPerSec, long renderTimeMs, int avsync_type)
        {
            if (_irtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.pullAudioFrame_(audioBuffer, type, samples, bytesPerSample, channels, samplesPerSec, renderTimeMs, avsync_type);
        }

        public override int EnableRawDataPtrCallback(bool enable)
        {
            if (_irtcEngine == null)
                return (int)ERROR_CODE.ERROR_OK;
            enableRawDataPtr = enable;
            return 0;
        }
        
        [MonoPInvokeCallback(typeof(EngineEventOnRecordAudioFrame))]
        private static void OnRecordAudioFrameCallback(int type, int samples, int bytesPerSample, int channels, int samplesPerSec, IntPtr buffer, long renderTimeMs, int avsync_type)
        {
            if (_irtcEngine != null && _audioRawDataManagerInstance != null && _audioRawDataManagerInstance.OnRecordAudioFrame != null)
            {
                AudioFrame audioFrame = new AudioFrame();
                audioFrame.type = (AUDIO_FRAME_TYPE)type;
                audioFrame.samples = samples;
                audioFrame.bytesPerSample = bytesPerSample;
                audioFrame.channels = channels;
                audioFrame.samplesPerSec = samplesPerSec;

                if (!enableRawDataPtr)
                {
                    byte[] byteBuffer = new byte[bytesPerSample * channels * samples];
                    Marshal.Copy(buffer, byteBuffer, 0, bytesPerSample * channels * samples);
                    audioFrame.buffer = byteBuffer;
                }
                
                audioFrame.bufferPtr = buffer;
                audioFrame.renderTimeMs = renderTimeMs;
                audioFrame.avsync_type = avsync_type;
                _audioRawDataManagerInstance.OnRecordAudioFrame(audioFrame);
            }
        } 

        [MonoPInvokeCallback(typeof(EngineEventOnPlaybackAudioFrame))]
        private static void OnPlaybackAudioFrameCallback(int type, int samples, int bytesPerSample, int channels, int samplesPerSec, IntPtr buffer, long renderTimeMs, int avsync_type)
        {
            if (_irtcEngine != null && _audioRawDataManagerInstance != null && _audioRawDataManagerInstance.OnPlaybackAudioFrame != null)
            {
                AudioFrame audioFrame = new AudioFrame();
                audioFrame.type = (AUDIO_FRAME_TYPE)type;
                audioFrame.samples = samples;
                audioFrame.bytesPerSample = bytesPerSample;
                audioFrame.channels = channels;
                audioFrame.samplesPerSec = samplesPerSec;

                if (!enableRawDataPtr)
                {
                    byte[] byteBuffer = new byte[bytesPerSample * channels * samples];
                    Marshal.Copy(buffer, byteBuffer, 0, bytesPerSample * channels * samples);
                    audioFrame.buffer = byteBuffer;
                }
                
                audioFrame.bufferPtr = buffer;
                audioFrame.renderTimeMs = renderTimeMs;
                audioFrame.avsync_type = avsync_type;
                _audioRawDataManagerInstance.OnPlaybackAudioFrame(audioFrame);
            }
        } 

        [MonoPInvokeCallback(typeof(EngineEventOnMixedAudioFrame))]
        private static void OnMixedAudioFrameCallback(int type, int samples, int bytesPerSample, int channels, int samplesPerSec, IntPtr buffer, long renderTimeMs, int avsync_type)
        {
            if (_irtcEngine != null && _audioRawDataManagerInstance != null && _audioRawDataManagerInstance.OnMixedAudioFrame != null)
            {
                AudioFrame audioFrame = new AudioFrame();
                audioFrame.type = (AUDIO_FRAME_TYPE)type;
                audioFrame.samples = samples;
                audioFrame.bytesPerSample = bytesPerSample;
                audioFrame.channels = channels;
                audioFrame.samplesPerSec = samplesPerSec;

                if (!enableRawDataPtr)
                {
                    byte[] byteBuffer = new byte[bytesPerSample * channels * samples];
                    Marshal.Copy(buffer, byteBuffer, 0, bytesPerSample * channels * samples);
                    audioFrame.buffer = byteBuffer;
                }
                
                audioFrame.bufferPtr = buffer;
                audioFrame.renderTimeMs = renderTimeMs;
                audioFrame.avsync_type = avsync_type;
                _audioRawDataManagerInstance.OnMixedAudioFrame(audioFrame);
            }
        } 

        [MonoPInvokeCallback(typeof(EngineEventOnPlaybackAudioFrameBeforeMixing))]
        private static void OnPlaybackAudioFrameBeforeMixingCallback(uint uid, int type, int samples, int bytesPerSample, int channels, int samplesPerSec, IntPtr buffer, long renderTimeMs, int avsync_type)
        {
            if (_irtcEngine != null && _audioRawDataManagerInstance != null && _audioRawDataManagerInstance.OnPlaybackAudioFrameBeforeMixing != null)
            {
                AudioFrame audioFrame = new AudioFrame();
                audioFrame.type = (AUDIO_FRAME_TYPE)type;
                audioFrame.samples = samples;
                audioFrame.bytesPerSample = bytesPerSample;
                audioFrame.channels = channels;
                audioFrame.samplesPerSec = samplesPerSec;

                if (!enableRawDataPtr)
                {
                    byte[] byteBuffer = new byte[bytesPerSample * channels * samples];
                    Marshal.Copy(buffer, byteBuffer, 0, bytesPerSample * channels * samples);
                    audioFrame.buffer = byteBuffer;
                }

                audioFrame.bufferPtr = buffer;
                audioFrame.renderTimeMs = renderTimeMs;
                audioFrame.avsync_type = avsync_type;
                _audioRawDataManagerInstance.OnPlaybackAudioFrameBeforeMixing(uid, audioFrame);
            }
        } 
    }
}