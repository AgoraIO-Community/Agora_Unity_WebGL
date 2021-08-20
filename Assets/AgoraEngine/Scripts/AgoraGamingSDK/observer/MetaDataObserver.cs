using System.Runtime.InteropServices;
using System;
using AOT;

namespace agora_gaming_rtc
{
    public abstract class IMetadataObserver : IRtcEngineNative
    {
        public abstract int RegisterMediaMetadataObserver(METADATA_TYPE metaDataType);
       
        public abstract int UnRegisterMediaMetadataObserver();
    }

    /** The definition of MetadataObserver.
    */
    public sealed class MetadataObserver : IMetadataObserver
    {
        public const int defaultMaxMetaDataSize = 1024;

        /** Occurs when the local user receives the metadata.
         * 
         * @param metadata The received Metadata.
         */
        public delegate void OnMediaMetaDataReceivedHandler(Metadata metadata);
        public OnMediaMetaDataReceivedHandler _OnMediaMetaDataReceived;

        /** Occurs when the SDK is ready to receive and send metadata.
         * 
         * @note Ensure that the size of the metadata does not exceed the value set in the {@link agora_gaming_rtc.MetadataObserver.OnGetMaxMetadataSizeHandler OnGetMaxMetadataSizeHandler} callback.
         * 
         * @param metadata The Metadata to be sent.
         * 
         * @return
         * - true: Send.
         * - false: Do not send.
         */
        public delegate bool OnReadyToSendMetadataHandler(ref Metadata metadata);
        public OnReadyToSendMetadataHandler _OnReadyToSendMetadata;

        /** Occurs when the SDK requests the maximum size of the metadata.
         * 
         * The metadata includes the following parameters:
         * 
         * - `uid`: The ID of the user who sent the metadata.
         * - `size`: The buffer size of the sent or received metadata.
         * - `buffer`: The buffer address of the sent or received metadata.
         * - `timeStampMs`: Time statmp of the frame following the metadata.
         * 
         * The SDK triggers this callback after you successfully call the {@link MetadataObserver.RegisterMediaMetadataObserver RegisterMediaMetadataObserver} method. You need to specify the maximum size of the metadata in the return value of this callback.
         * 
         * @return The maximum size of the buffer of the metadata that you set. The highest value is 1024 bytes. Ensure that you set the return value, if not, `OnGetMaxMetadataSizeHandler` returns 1024, which is the default maximum size of the metadata.
         */
        public delegate int OnGetMaxMetadataSizeHandler();
        public OnGetMaxMetadataSizeHandler _OnGetMaxMetadataSize;

        private static IRtcEngine _irtcEngine = null;
        public static MetadataObserver _metaDataObserver = null;

        private MetadataObserver(IRtcEngine irtcEngine)
        {
            _irtcEngine = irtcEngine;
        }

        public static MetadataObserver GetInstance(IRtcEngine irtcEngine)
        {
            if (_metaDataObserver == null)
                _metaDataObserver = new MetadataObserver(irtcEngine);

            return _metaDataObserver;
        }

        public static void releaseInstance()
        {
            _metaDataObserver = null;
        }

        public void SetEngine(IRtcEngine irtcEngine)
        {
            _irtcEngine = irtcEngine;
        }

        /** Registers a metadata observer.
         * 
         * You need to implement the MetadataObserver class and specify the metadata type in this method. A successful call of this method triggers the {@link agora_gaming_rtc.MetadataObserver.OnGetMaxMetadataSizeHandler OnGetMaxMetadataSizeHandler} callback. This method enables you to add synchronized metadata in the video stream for more diversified interactive live streaming interactions, such as sending shopping links, digital coupons, and online quizzes.
         * 
         * @note 
         * - Call this method before the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method.
         * - This method applies to the Live-broadcast channel profile.
         * 
         * @param metaDataType See {@link agora_gaming_rtc.METADATA_TYPE METADATA_TYPE}. The SDK supports `VIDEO_METADATA(0)` only for now.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public override int RegisterMediaMetadataObserver(METADATA_TYPE metaDataType)
        {
            if (_irtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            IRtcEngineNative.initEventOnMetaDataCallback(OnMediaMetaDataReceivedCallback, OnReadyToSendMetadataCallback, OnGetMaxMetadataSizeCallback);
            return IRtcEngineNative.registerMediaMetadataObserver((int)metaDataType);
        }
        
        /** UnRegisters the metadata observer.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public override int UnRegisterMediaMetadataObserver()
        {
            if (_irtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            int rc = IRtcEngineNative.unRegisterMediaMetadataObserver();
            IRtcEngineNative.initEventOnMetaDataCallback(null, null, null);
            return rc;
        }


        [MonoPInvokeCallback(typeof(EngineEventOnMediaMetaDataReceived))]
        private static void OnMediaMetaDataReceivedCallback(uint uid, uint size, IntPtr buffer, long timeStampMs)
        {
            if (_irtcEngine != null && _metaDataObserver != null && _metaDataObserver._OnMediaMetaDataReceived != null)
            {
                Metadata metadata = new Metadata();
                metadata.uid = uid;
                metadata.size = size;
                byte[] byteBuffer = new byte[size];
                Marshal.Copy(buffer, byteBuffer, 0,(int)size);
                metadata.buffer = byteBuffer;
                metadata.timeStampMs = timeStampMs;
                _metaDataObserver._OnMediaMetaDataReceived(metadata);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnReadyToSendMetadata))]
        private static bool OnReadyToSendMetadataCallback()
        {
            if (_irtcEngine != null && _metaDataObserver != null && _metaDataObserver._OnReadyToSendMetadata != null)
            {
                Metadata metadata = new Metadata();
                bool rc = _metaDataObserver._OnReadyToSendMetadata(ref metadata);
                if (metadata.size != 0 && metadata.buffer != null)
                {
                    IRtcEngineNative.sendMetadata(metadata.uid, metadata.size, metadata.buffer, metadata.timeStampMs);
                }
                return rc;
            }
            return true;
        }

        [MonoPInvokeCallback(typeof(EngineEventOnGetMaxMetadataSize))]
        private static int OnGetMaxMetadataSizeCallback()
        {
            if (_irtcEngine != null && _metaDataObserver != null && _metaDataObserver._OnGetMaxMetadataSize != null)
            {
                return _metaDataObserver._OnGetMaxMetadataSize();
            }
            return defaultMaxMetaDataSize;
        }
    }
}