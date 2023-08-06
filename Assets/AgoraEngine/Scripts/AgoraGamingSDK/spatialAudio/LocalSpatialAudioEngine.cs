using System.Runtime.InteropServices;
using AOT;

namespace agora_gaming_rtc
{
    public abstract class ILocalSpatialAudioEngine : IRtcEngineNative
    {
        public abstract int Initialize();

        public abstract int Initialize_MC();

        public abstract int UpdateRemotePosition(uint uid, RemoteVoicePositionInfo info);

        public abstract int RemoveRemotePosition(uint uid);

        public abstract int ClearRemotePositions();

        public abstract void Release();

        public abstract int SetMaxAudioRecvCount(int maxCount);

        public abstract int SetAudioRecvRange(float range);

        public abstract int SetDistanceUnit(float unit);

        public abstract int UpdateSelfPosition(float[] position, float[] axisForward, float[] axisRight, float[] axisUp);

        public abstract int SetParameters(string @params);

        public abstract int MuteLocalAudioStream(bool mute);

        public abstract int MuteAllRemoteAudioStreams(bool mute);
    }

    public sealed class LocalSpatialAudioEngine : ILocalSpatialAudioEngine
    {
        private IRtcEngine mEngine = null;
        private static LocalSpatialAudioEngine _localSpatialAudioEngine;

        private LocalSpatialAudioEngine(IRtcEngine rtcEngine)
        {
            mEngine = rtcEngine;
        }

        ~LocalSpatialAudioEngine()
        {

        }

        public static LocalSpatialAudioEngine GetInstance(IRtcEngine rtcEngine)
        {
            if (_localSpatialAudioEngine == null)
            {
                _localSpatialAudioEngine = new LocalSpatialAudioEngine(rtcEngine);
            }
            return _localSpatialAudioEngine;
        }

        public static void ReleaseInstance()
        {
            _localSpatialAudioEngine = null;
        }

        // used internally
        public void SetEngine(IRtcEngine engine)
        {
            mEngine = engine;
        }

        public override int Initialize()
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.localSpatialAudio_initialize();
        }

        public override int Initialize_MC()
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.localSpatialAudio_initialize_mc();
        }

        public override int UpdateRemotePosition(uint uid, RemoteVoicePositionInfo info)
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.updateRemotePosition(uid, info.position, info.forward);
        }

        public override int RemoveRemotePosition(uint uid)
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.removeRemotePosition(uid);
        }

        public override int ClearRemotePositions()
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.clearRemotePositions();
        }

        public override void Release()
        {
            if (mEngine == null)
                return;

            IRtcEngineNative.localSpatialAudio_release();
        }

        public override int SetMaxAudioRecvCount(int maxCount)
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.setMaxAudioRecvCount(maxCount);
        }

        public override int SetAudioRecvRange(float range)
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.setAudioRecvRange(range);
        }

        public override int SetDistanceUnit(float unit)
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.setDistanceUnit(unit);
        }

        public override int UpdateSelfPosition(float[] position, float[] axisForward, float[] axisRight, float[] axisUp)
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            UnityEngine.Debug.Log("Upddate selfPosition (LocalSpatialAudio)");
            return IRtcEngineNative.updateSelfPosition(position, axisForward, axisRight, axisUp);
        }

        public override int SetParameters(string @params)
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.localSpatialAudio_setParameters(@params);
        }

        public override int MuteLocalAudioStream(bool mute)
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.muteLocalAudioStream(mute);
        }

        public override int MuteAllRemoteAudioStreams(bool mute)
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.muteAllRemoteAudioStreams(mute);
        }
    }
}
