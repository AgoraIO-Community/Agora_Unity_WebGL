let processor = null;
let extension = null;
let imgElement = null;
let videoElement = null;
var localTracks = {
  videoTrack: null,
  audioTrack: null
};

// Initialization
async function getProcessorInstance(videoTrack, enabled, backgroundSourceType, color, source, blurDegree) {

  console.log("background source type", backgroundSourceType);

  if (extension == null) {
    // Create a VirtualBackgroundExtension instance
    extension = new VirtualBackgroundExtension();
    // Register the extension
    AgoraRTC.registerExtensions([extension]);
  }

  if (!processor && videoTrack) {
    // Create a VirtualBackgroundProcessor instance
    processor = extension.createProcessor();

      try {
        // Initialize the extension and pass in the URL of the Wasm file
        await processor.init("./assets/agora-wasm");
        } catch(e) {
          console.log("Fail to load WASM resource!");return null;
          }
    // Inject the extension into the video processing pipeline in the SDK
    videoTrack.pipe(processor).pipe(videoTrack.processorDestination);
  }

  if(backgroundSourceType == 3){
    setBackgroundBlurring(videoTrack, blurDegree);
  } else if(backgroundSourceType == 1){
    setBackgroundColor(videoTrack, color);
  } else if(backgroundSourceType == 2){
    setBackgroundImage(videoTrack, source);
  } else if(backgroundSourceType == 4){
    setBackgroundVideo(videoTrack, source);
  }

  if(enabled){
    processor.enable();
  } else {
    processor.disable();
  }

  return processor;
}

// Set a solid color as the background
async function setBackgroundColor(videoTrack, hexColor) {
  if (videoTrack) {

    if(videoElement != null){
      videoElement.pause();
      videoElement.currentTime = 0;
      videoElement = null;
    }

    console.log(hexColor);

    try {
      processor.setOptions({type: 'color', color: '#' + Math.abs(hexColor).toString(16)});
    } finally {
    }

    virtualBackgroundEnabled = true;
  }
}

// Blur the user's actual background
async function setBackgroundBlurring(videoTrack, myBlur) {
  if (videoTrack) {

    if(videoElement != null){
      videoElement.pause();
      videoElement.currentTime = 0;
      videoElement = null;
    }

    console.log(myBlur);

    try {
      processor.setOptions({type: 'blur', blurDegree: myBlur});
    } finally {
    }

    virtualBackgroundEnabled = true;
  }
}

// Set an image as the background
async function setBackgroundImage(videoTrack, imgFile) {

    if(videoElement != null){
      videoElement.pause();
      videoElement.currentTime = 0;
      videoElement = null;
    }

    imgElement = document.createElement('img');

    imgElement.onload = async() => {

      try {
        processor.setOptions({type: 'img', source: imgElement});
      } finally {
      }

      virtualBackgroundEnabled = true;
    }
    imgElement.src = './AgoraWebSDK/assets/images/' + imgFile;
}

async function setBackgroundVideo(videoTrack, videoFile) {
  videoElement = document.createElement('video');
  
  console.log('./AgoraWebSDK/assets/videos/' + videoFile);

  videoElement.oncanplay = async() => {

    try {
      processor.setOptions({type: 'video', source: videoElement});
      console.log("processor enabled");
    } catch(e) {
    }

    virtualBackgroundEnabled = true;
  }
  videoElement.src = 'AgoraWebSDK/assets/videos/' + videoFile;
  videoElement.type = "video/mp4";
  videoElement.width = 800;
  videoElement.height = 600;
  videoElement.play();
}