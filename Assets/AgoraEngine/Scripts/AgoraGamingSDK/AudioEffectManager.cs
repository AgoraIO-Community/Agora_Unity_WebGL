using UnityEngine;

namespace agora_gaming_rtc
{
	public abstract class IAudioEffectManager : IRtcEngineNative
	{
		public abstract double GetEffectsVolume ();

		public abstract int SetEffectsVolume (int volume);

		public abstract int PlayEffect (int soundId, string filePath,
			int loopCount,
			double pitch = 1.0D,
			double pan = 0.0D,
			int gain = 100,
			bool publish = false
		);

        public abstract int PlayEffect(int soundId, string filePath, int loopCount, double pitch, double pan, int gain, bool publish, int startPos);
        
        public abstract int GetEffectDuration(string filePath);
        
        public abstract int SetEffectPosition(int soundId, int pos);
        
        public abstract int GetEffectCurrentPosition(int soundId);

		public abstract int StopEffect (int soundId);

		public abstract int StopAllEffects ();

		public abstract int PreloadEffect (int soundId, string filePath);

		public abstract int UnloadEffect (int soundId);

		public abstract int PauseEffect (int soundId);

		public abstract int PauseAllEffects ();

		public abstract int ResumeEffect (int soundId);

		public abstract int ResumeAllEffects ();

		public abstract int SetVoiceOnlyMode (bool enable);

		public abstract int SetRemoteVoicePosition (uint uid, double pan, double gain);

		public abstract int SetLocalVoicePitch (double pitch);
	}
    /** The definition of AudioEffectManagerImpl. */
	public sealed class  AudioEffectManagerImpl : IAudioEffectManager
	{
		private IRtcEngine _mEngine = null;
		private static AudioEffectManagerImpl _audioEffectManagerImplInstance = null;

		private AudioEffectManagerImpl (IRtcEngine rtcEngine)
		{		
			_mEngine = rtcEngine;
		}

		public static AudioEffectManagerImpl GetInstance(IRtcEngine rtcEngine)
		{
			if (_audioEffectManagerImplInstance == null)
			{
				_audioEffectManagerImplInstance = new AudioEffectManagerImpl(rtcEngine);
			}

			return _audioEffectManagerImplInstance;
		}

		public static void ReleaseInstance()
		{
			_audioEffectManagerImplInstance = null;
		}

		// used internally
		public void SetEngine (IRtcEngine engine)
		{
			_mEngine = engine;
		}

        /** Retrieves the volume of the audio effects.
         * 
         * The value ranges between 0.0 and 100.0.
         *
         * @note Ensure that this method is called after {@link agora_gaming_rtc.AudioEffectManagerImpl.PlayEffect PlayEffect}.
         * 
         * @return
         * - &ge; 0: Volume of the audio effects, if this method call succeeds.
         * - < 0: Failure.
         */
		public override double GetEffectsVolume ()
		{
			if (_mEngine == null)
				return (double)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

			return IRtcEngineNative.getEffectsVolume();
		}

        /** Sets the volume of the audio effects.
         *
         * @note Ensure that this method is called after {@link agora_gaming_rtc.AudioEffectManagerImpl.PlayEffect PlayEffect}.
         * 
         * @param volume Sets the volume of the audio effects. The value ranges between 0 and 100 (default).
         * 
         * @return 
         * - 0: Success.
         * - < 0: Failure.
         */
		public override int SetEffectsVolume (int volume)
		{
			if (_mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

			return IRtcEngineNative.setEffectsVolume(volume);
		}

        /** Plays a specified local or online audio effect file.
         * 
         * This method allows you to set the loop count, pitch, pan, and gain of the audio effect file, as well as whether the remote user can hear the audio effect.
         * 
         * To play multiple audio effect files simultaneously, call this method multiple times with different soundIds and filePaths. We recommend playing no more than three audio effect files at the same time.
         * 
         * @note
         * - If the audio effect is preloaded into the memory through the {@link agora_gaming_rtc.AudioEffectManagerImpl.PreloadEffect PreloadEffect} method, the value of `soundId` must be the same as that in the `PreloadEffect` method.
         * - Playing multiple online audio effect files simultaneously is not supported on macOS and Windows.
         * - Ensure that you call this method after joining a channel.
         * 
         * @param soundId ID of the specified audio effect. Each audio effect has a unique ID.
         * @param filePath Specifies the absolute path (including the suffixes of the filename) to the local audio effect file or the URL of the online audio effect file. Supported audio formats: mp3, mp4, m4a, aac, 3gp, mkv and wav.
         * @param loopCount Sets the number of times the audio effect loops:
         * - 0: Play the audio effect once.
         * - 1: Play the audio effect twice.
         * - -1: Play the audio effect in an indefinite loop until the {@link agora_gaming_rtc.AudioEffectManagerImpl.StopEffect StopEffect} or {@link agora_gaming_rtc.AudioEffectManagerImpl.StopAllEffects StopAllEffects} method is called.
         * @param pitch Sets the pitch of the audio effect. The value ranges between 0.5 and 2. The default value is 1 (no change to the pitch). The lower the value, the lower the pitch.
         * @param pan Sets the spatial position of the audio effect. The value ranges between -1.0 and 1.0:
         * - 0.0: The audio effect displays ahead.
         * - 1.0: The audio effect displays to the right.
         * - -1.0: The audio effect displays to the left.
         * @param gain  Sets the volume of the audio effect. The value ranges between 0 and 100 (default). The lower the value, the lower the volume of the audio effect.
         * @param publish Sets whether or not to publish the specified audio effect to the remote stream:
         * - true: The locally played audio effect is published to the Agora Cloud and the remote users can hear it.
         * - false: The locally played audio effect is not published to the Agora Cloud and the remote users cannot hear it.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
		public override int PlayEffect (int soundId, string filePath,
			int loopCount,
			double pitch = 1.0D,
			double pan = 0.0D,
			int gain = 100,
			bool publish = false)
		{
			if (_mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

			return IRtcEngineNative.playEffect(soundId, filePath, loopCount, pitch, pan, gain, publish);
		}

        public override int PlayEffect(int soundId, string filePath, int loopCount, double pitch, double pan, int gain, bool publish, int startPos)
        {
            if (_mEngine  == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.playEffect2(soundId, filePath, loopCount, pitch, pan, gain, publish, startPos);
        }
        
        public override int GetEffectDuration(string filePath)
        {
            if (_mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.getEffectDuration(filePath);
        }
        
        public override int SetEffectPosition(int soundId, int pos)
        {
            if (_mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.setEffectPosition(soundId, pos);
        }
        
        public override int GetEffectCurrentPosition(int soundId)
        {
            if (_mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return IRtcEngineNative.getEffectCurrentPosition(soundId);
        }

        /** Stops playing a specified audio effect.
         *
         * @param soundId ID of the audio effect to stop playing. Each audio effect has a unique ID.
         * 
         * @return 
         * - 0: Success.
         * - < 0: Failure.
         */
		public override int StopEffect (int soundId)
		{
			if (_mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

			return IRtcEngineNative.stopEffect(soundId);
		}

        /** Stops playing all audio effects.
         *
         * @return 
         * - 0: Success.
         * - < 0: Failure.
         */
		public override int StopAllEffects ()
		{
			if (_mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

			return IRtcEngineNative.stopAllEffects();
		}

         /** Preloads a specified audio effect file into the memory.
         *
         * To ensure smooth communication, limit the size of the audio effect file. We recommend using this method to preload the audio effect before calling the {@link agora_gaming_rtc.IRtcEngine.JoinChannelByKey JoinChannelByKey} method.
         * 
         * Supported audio formats: mp3, aac, m4a, 3gp, and wav.
         * 
         * @note This method does not support online audio effect files.
         * 
         * @param soundId ID of the audio effect. Each audio effect has a unique ID.
         * @param filePath The absolute path of the audio effect file.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
		public override int PreloadEffect (int soundId, string filePath)
		{
			if (_mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

			return IRtcEngineNative.preloadEffect(soundId, filePath);
		}

        /** Releases a specified preloaded audio effect from the memory.
         *
         * @param soundId ID of the audio effect. Each audio effect has a unique ID.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
		public override int UnloadEffect (int soundId)
		{
			if (_mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

			return IRtcEngineNative.unloadEffect(soundId);
		}

        /** Pauses a specified audio effect.
         *
         * @param soundId ID of the audio effect. Each audio effect has a unique ID.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
		public override int PauseEffect (int soundId)
		{
			if (_mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

			return IRtcEngineNative.pauseEffect(soundId);
		}

        /** Pauses all audio effects.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
		public override int PauseAllEffects ()
		{
			if (_mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

			return IRtcEngineNative.pauseAllEffects();
		}

        /** Resumes playing a specified audio effect.
         *
         * @param soundId ID of the audio effect. Each audio effect has a unique ID.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
		public override int ResumeEffect (int soundId)
		{
			if (_mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

			return IRtcEngineNative.resumeEffect(soundId);
		}

        /** Resumes playing all audio effects.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
		public override int ResumeAllEffects ()
		{
			if (_mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

			return IRtcEngineNative.resumeAllEffects();
		}

        /** Sets the voice-only mode (transmit the audio stream only), and the other streams will be ignored; for example the sound of the keyboard strokes.
         *
         * @param enable Whether to enable the voice-only mode.
         * - true: Enable.
         * - false: Disable.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
		public override int SetVoiceOnlyMode (bool enable)
		{
			if (_mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

			return IRtcEngineNative.setVoiceOnlyMode(enable);
		}

        /** Sets the sound position and gain of a remote user.
         * 
         * When the local user calls this method to set the sound position of a remote user, the sound difference between the left and right channels allows the local user to track the real-time position of the remote user, creating a real sense of space. This method applies to massively multiplayer online games, such as Battle Royale games.
         * 
         * @note
         * - For this method to work, enable stereo panning for remote users by calling the {@link agora_gaming_rtc.IRtcEngine.EnableSoundPositionIndication EnableSoundPositionIndication} method before joining a channel.
         * - This method requires hardware support. For the best sound positioning, we recommend using a stereo speaker.
         * - Ensure that you call this method after joining a channel.
         * 
         * @param uid The ID of the remote user.
         * @param pan The sound position of the remote user. The value ranges from -1.0 to 1.0:
         * - 0.0: the remote sound comes from the front.
         * - -1.0: the remote sound comes from the left.
         * - 1.0: the remote sound comes from the right.
         * @param gain Gain of the remote user. The value ranges from 0.0 to 100.0. The default value is 100.0 (the original gain of the remote user). The smaller the value, the less the gain.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
		public override int SetRemoteVoicePosition (uint uid, double pan, double gain)
		{
			if (_mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

			return IRtcEngineNative.setRemoteVoicePosition(uid, pan, gain);
		}

        /** Changes the voice pitch of the local speaker.
         *
         * @note You can call this method either before or after joining a channel.
         *
         * @param pitch Sets the voice pitch. The value ranges between 0.5 and 2.0. The lower the value, the lower the voice pitch. The default value is 1.0 (no change to the local voice pitch).
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
		public override int SetLocalVoicePitch (double pitch)
		{
			if (_mEngine == null)
				return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
				
			return IRtcEngineNative.setLocalVoicePitch(pitch);
		}
	}
}
