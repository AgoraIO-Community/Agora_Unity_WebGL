using System.Runtime.InteropServices;
using AOT;

namespace agora_gaming_rtc
{

    public abstract class IMediaRecorder : IRtcEngineNative
    {
        public abstract int StartRecording(MediaRecorderConfiguration config);

        public abstract int StopRecording();

        public abstract void InitMediaRecorderObserver();
    }

    /** The MediaRecorder class, for recording the audio and video on the client.
    * IMediaRecorder can record the following content:
    *
    * - The audio captured by the local microphone and encoded in AAC format.
    * - The video captured by the local camera and encoded by the SDK.
    *
    * Since v3.6.1.1
    *
    * @note
    * In the `COMMUNICATION` channel profile, this function is unavailable when there are users using versions of the SDK earlier than v3.0.1 in the channel.
    */
    public sealed class MediaRecorder : IMediaRecorder
    {
        private static IRtcEngine mEngine = null;
		private static MediaRecorder _mediaRecorderInstance;
        /**
        * Occurs when the recording state changes.
        *
        * @since v3.6.1.1
        *
        * When the local audio and video recording state changes, the SDK triggers this callback to report the current recording state and the reason for the change.
        *
        * @param state The current recording state. See #RecorderState.
        * @param error The reason for the state change. See #RecorderErrorCode.
        */
        public delegate void OnRecorderStateChangedHandler(RecorderState state, RecorderErrorCode error);
        public OnRecorderStateChangedHandler _OnRecorderStateChanged;
        /**
        * Occurs when the recording information is updated.
        *
        * @since v3.6.1.1
        *
        * After you successfully register this callback and enable the local audio and video recording, the SDK periodically triggers the {@link agora_gaming_rtc.MediaRecorder.OnRecorderInfoUpdatedHandler OnRecorderInfoUpdatedHandler} callback based on the set value of `recorderInfoUpdateInterval`. This callback reports the filename, duration, and size of the current recording file.
        *
        * @param info Information for the recording file. See {@link agora_gaming_rtc.RecorderInfo RecorderInfo}.
        */
        public delegate void OnRecorderInfoUpdatedHandler(RecorderInfo info);
        public OnRecorderInfoUpdatedHandler _OnRecorderInfoUpdated;

		private MediaRecorder(IRtcEngine rtcEngine)
		{
			mEngine = rtcEngine;
            createMediaRecording();
		}

		~MediaRecorder()
		{

		}

		public static MediaRecorder GetInstance(IRtcEngine rtcEngine)
		{
			if (_mediaRecorderInstance == null)
			{
				_mediaRecorderInstance = new MediaRecorder(rtcEngine);
			}
			return _mediaRecorderInstance;
		}

        /// @cond
     	public static void ReleaseInstance()
		{
			IRtcEngineNative.releaseMediaRecorder();
			_mediaRecorderInstance = null;
		}

		public void SetEngine (IRtcEngine engine)
		{
			mEngine = engine;
		}
        /// @endcond

        private int createMediaRecording()
        {
            if (mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
            return IRtcEngineNative.createMediaRecorder();
        }
        /**
        * Starts recording the local audio and video.
        *
        * @since v3.6.1
        *
        * After successfully getting the object, you can call this method to enable the recording of the local audio and video.
        *
        * This method can record the following content:
        * - The audio captured by the local microphone and encoded in AAC format.
        * - The video captured by the local camera and encoded by the SDK.
        *
        * The SDK can generate a recording file only when it detects the recordable audio and video streams; when there are
        * no audio and video streams to be recorded or the audio and video streams are interrupted for more than five
        * seconds, the SDK stops recording and triggers the
        * `OnRecorderStateChangedHandler(RECORDER_STATE_ERROR, RECORDER_ERROR_NO_STREAM)`
        * callback.
        *
        * @note Call this method after joining the channel.
        *
        * @param config The recording configurations. See {@link agora_gaming_rtc.MediaRecorderConfiguration MediaRecorderConfiguration}.
        *
        * @return
        * - 0(ERR_OK): Success.
        * - < 0: Failure:
        *    - `-2(ERR_INVALID_ARGUMENT)`: The parameter is invalid. Ensure the following:
        *      - The specified path of the recording file exists and is writable.
        *      - The specified format of the recording file is supported.
        *      - The maximum recording duration is correctly set.
        *    - `-4(ERR_NOT_SUPPORTED)`: IRtcEngine does not support the request due to one of the following reasons:
        *      - The recording is ongoing.
        *      - The recording stops because an error occurs.
        *    - `-7(ERR_NOT_INITIALIZED)`: This method is called before the initialization of IRtcEngine. Ensure that you have
        * called `GetMediaRecorder` before calling `StartRecording`.
        */
        public override int StartRecording(MediaRecorderConfiguration config)
        {
            if (mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
            return IRtcEngineNative.startRecording(config.storagePath, (int)config.containerFormat, (int)config.streamType, config.maxDurationMs, config.recorderInfoUpdateInterval);
        }
        /**
        * Stops recording the local audio and video.
        *
        * @since v3.6.1.1
        *
        * @note Call this method after calling `StartRecording`.
        *
        * @return
        * - 0(ERR_OK): Success.
        * - < 0: Failure.
        */
        public override int StopRecording()
        {
            if (mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
            return IRtcEngineNative.stopRecording();
        }
        /**
         * Initializes the media recorder observer object.
         * @since v3.6.1.1
         * @note This method should be called after you implement the {@link agora_gaming_rtc.MediaRecorder.OnRecorderStateChangedHandler OnRecorderStateChangedHandler}
         * and {@link agora_gaming_rtc.MediaRecorder.OnRecorderInfoUpdatedHandler OnRecorderInfoUpdatedHandler} callbacks and before you call {@link agora_gaming_rtc.MediaRecorder.StartRecording StartRecording}.
         */
        public override void InitMediaRecorderObserver()
        {
	        if (mEngine == null)
		        return;
            IRtcEngineNative.initEventOnMediaRecorderCallback(OnRecorderStateChangedCallback, OnRecorderInfoUpdatedCallback);
        }

        [MonoPInvokeCallback(typeof(EngineEventOnRecorderStateChanged))]
        private static void OnRecorderStateChangedCallback(int state, int error)
        {
            if (mEngine != null && _mediaRecorderInstance != null)
            {
                _mediaRecorderInstance._OnRecorderStateChanged((RecorderState)state, (RecorderErrorCode)error);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnRecorderInfoUpdated))]
        private static void OnRecorderInfoUpdatedCallback(string fileName, uint durationMs, uint fileSize)
        {
            if (mEngine != null && _mediaRecorderInstance != null)
            {
                RecorderInfo info;
                info.fileName = fileName;
                info.durationMs = durationMs;
                info.fileSize = fileSize;
                _mediaRecorderInstance._OnRecorderInfoUpdated(info);
            }
        }
    }
}
