// AUdio recording system seperation
var mediaRecorder;
var audioChunks = [];
var dest; /// empty audio dest
var audioCtx = new AudioContext();

const kSampleRate = 44100; // Other sample rates might not work depending on the your browser's AudioContext
const kNumSamples = 16834;
const kFrequency = 440;
const kPI_2 = Math.PI * 2;
var listFreq = [
  262, 262, 294, 262, 349, 330, 262, 294, 262, 392, 349, 262, 262, 524, 440,
  349, 330, 294, 466, 466, 440, 349, 392, 349, 262, 262, 294, 262, 349, 330,
  262, 294, 262, 392, 349, 262, 262, 524, 440, 349, 330, 294, 466, 466, 440,
  349, 392, 349, 262, 262, 294, 262, 349, 330, 262, 294, 262, 392, 349, 262,
  262, 524, 440, 349, 330, 294, 466, 466, 440, 349, 392, 349, 262, 262, 294,
  262, 349, 330, 262, 294, 262, 392, 349, 262, 262, 524, 440, 349, 330, 294,
  466, 466, 440, 349, 392, 349, 262, 262, 294, 262, 349, 330, 262, 294, 262,
  392, 349, 262, 262, 524, 440, 349, 330, 294, 466, 466, 440, 349, 392, 349,
  262, 262, 294, 262, 349, 330, 262, 294, 262, 392, 349, 262, 262, 524, 440,
  349, 330, 294, 466, 466, 440, 349, 392, 349,
];
var play = false;

function startAudioRecording_WGL(filePath, quality) {
  if (localTracks.audioTrack) {
    var ctx = new AudioContext();
    var source = ctx.createMediaStreamSource(
      localTracks.audioTrack._source.sourceNode.mediaStream
    );
    var dest = ctx.createMediaStreamDestination();
    var gainNode = ctx.createGain();

    source.connect(gainNode);
    gainNode.connect(dest);
    gainNode.gain.value = wrapper.savedSettings.recordingSignalVolume / 100;

    mediaRecorder = new MediaRecorder(dest.stream);
    mediaRecorder.start();

    mediaRecorder.addEventListener("dataavailable", (event) => {
      audioChunks.push(event.data);
    });

    mediaRecorder.addEventListener("stop", () => {
      const audioBlob = new Blob(audioChunks);
      const audioUrl = URL.createObjectURL(audioBlob);

      var a = document.createElement("a");
      document.body.appendChild(a);
      a.style = "display: none";
      a.href = audioUrl;

      a.download = filePath;
      a.click();
      window.URL.revokeObjectURL(audioUrl);
    });
  }
}

function stopAudioRecording_WGL() {
  if (localTracks.audioTrack) {
    localTracks.audioTrack.setVolume(
      wrapper.savedSettings.localAudioTrackVolume
    );
  }
  audioChunks = [];
  mediaRecorder.stop();
}

function sleep(ms) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}

function playBeep(freq, destination) {
  var buffer = audioCtx.createBuffer(1, kNumSamples, kSampleRate);
  var buf = buffer.getChannelData(0);
  for (i = 0; i < kNumSamples; ++i) {
    buf[i] = Math.sin((freq * kPI_2 * i) / kSampleRate);
  }

  var node = audioCtx.createBufferSource(0);
  node.buffer = buffer;
  node.connect(destination);
  //node.start(audioCtx.currentTime + 0.5);
  node.start(0);
}

function play_buffersource() {
  var buffer = audioCtx.createBuffer(1, kNumSamples, kSampleRate);
  var buf = buffer.getChannelData(0);
  for (i = 0; i < kNumSamples; ++i) {
    buf[i] = Math.sin((kFrequency * kPI_2 * i) / kSampleRate);
  }

  var node = audioCtx.createBufferSource(0);
  node.buffer = buffer;
  node.connect(dest);
  //node.start(audioCtx.currentTime + 0.5);
  node.start(0);
}
// plays music using buffer
async function playMusc() {
  play = true;
  // Sleep in loop
  for (let i = 0; i < listFreq.length; i++) {
    if (play) {
      playBeep(listFreq[i], dest);
      await sleep(500);
    }
  }
}

function pushAudioFrame_WGL(buff, byteArray) {
  /*
  var buffer = audioCtx.createBuffer(1, kNumSamples, kSampleRate);
  var buf = buffer.getChannelData(0);
  for (i = 0; i < kNumSamples; ++i) {
    buf[i] = Math.sin((kFrequency * kPI_2 * i) / kSampleRate);
  }*/
  /* 
  var arrayBuffer = new ArrayBuffer(byteArray.length);
  var bufferView = new Uint8Array(arrayBuffer);
  for (i = 0; i < byteArray.length; i++) {
    bufferView[i] = byteArray[i];
  }*/
  /*
  audioCtx.decodeAudioData(buff, function (buffer) {
    var node = audioCtx.createBufferSource();
    node.buffer = buffer;
    node.connect(dest);
    //node.start(audioCtx.currentTime + 0.5);
    node.start(0);
  });*/
  /*
  audioCtx.decodeAudioData(arrayBuffer, function (buffer) {
    //play();
    var node = audioCtx.createBufferSource(0);
    node.buffer = buffer;
    node.connect(dest);
    node.start(0);
  });*/
}

async function setExternalAudioSource_WGL(enabled, sampleRate, channels) {
  client_manager.setExternalAudioSource_WGL(enabled, sampleRate, channels);
}

// Constants for Audio Profile
var AUDIO_PROFILE_TYPE = [
  // sample rate, bit rate, mono/stereo, speech/music codec
  { label: 0, value: "music_standard" },
  { label: 1, value: "speech_standard" },
  { label: 2, value: "music_standard" },
  { label: 3, value: "standard_stereo" },
  { label: 4, value: "high_quality" },
  { label: 5, value: "high_quality_stereo" },
  { label: 6, value: "speech_low_quality" },
  { label: 7, value: "music_standard" },
];

// Sets the audio parameters and application scenarios.
function SetAudioProfile(profile) {
  AgoraRTC.createMicrophoneAudioTrack({
    encoderConfig: AUDIO_PROFILE_TYPE[profile].value,
  }).then(() => {
    //console.log("JS Microphone audio track is set");
  });
}
