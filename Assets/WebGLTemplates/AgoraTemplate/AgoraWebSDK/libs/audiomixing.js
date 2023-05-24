var audioSourceOptions = {
  loop: false,
  cycle: 1,
  startPlayTime: 0,
};
var mixingStatus = false;
//
// Starts playing and mixing the music file.
async function StartAudioMixing(FilePath, LoopBack, Replace, Cycle) {
  var client = client_manager.getClient();
  if (client) {
    if (mixingStatus == false) {
      mixingStatus = true;

      try {
        if (localTracks.audioMixingTrack) {
          await client.unpublish(localTracks.audioMixingTrack);
        }
        // start audio mixing with local file or the preset file
        localTracks.audioMixingTrack =
          await AgoraRTC.createBufferSourceAudioTrack({
            source: FilePath,
          });

        audioSourceOptions.loop = Cycle == -1;
        audioSourceOptions.cycle = Cycle;

        localTracks.audioMixingTrack.startProcessAudioBuffer(
          audioSourceOptions
        );

        if (Replace == false) {
          localTracks.audioTrack = await AgoraRTC.createMicrophoneAudioTrack();
          if (LoopBack == false)
            await client.publish([
              localTracks.audioTrack,
              localTracks.audioMixingTrack,
            ]);
        } else {
          if (LoopBack == false)
            await client.publish(localTracks.audioMixingTrack);
        }
        localTracks.audioMixingTrack.play();
      } catch (error) {
        mixingStatus = false;
      }
    }
  }
}

// Stops playing and mixing the music file.
async function StopAudioMixing() {
  var client = client_manager.getClient();
  if (client) {
    if (mixingStatus == true) {
      try {
        if (localTracks.audioMixingTrack) {
          await client.unpublish(localTracks.audioMixingTrack);
          localTracks.audioMixingTrack.stopProcessAudioBuffer();
          localTracks.audioMixingTrack.stop();
          localTracks.audioMixingTrack.close();
        }
      } catch (error) {
        //console.log("StopAudioMixing error " + error);
      }
      mixingStatus = false;
    }
  }
}

// Pauses playing and mixing the music file.
function PauseAudioMixing() {
  try {
    if (localTracks.audioMixingTrack)
      localTracks.audioMixingTrack.pauseProcessAudioBuffer();
  } catch (error) {
    //console.log("Pause Audio Mixing Error " + error);
  }
}

// Resumes playing and mixing the music file
function ResumeAudioMixing() {
  try {
    if (localTracks.audioMixingTrack)
      localTracks.audioMixingTrack.resumeProcessAudioBuffer();
  } catch (error) {
    //console.log("Resume Audio Mixing Error " + error);
  }
}
