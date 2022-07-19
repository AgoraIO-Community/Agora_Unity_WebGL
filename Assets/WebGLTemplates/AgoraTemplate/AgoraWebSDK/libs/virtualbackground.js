let processor = null;
let extension = null;
var localTracks = {
  videoTrack: null,
  audioTrack: null
};

// Initialization
async function getProcessorInstance(videoTrack) {

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
  return processor;
}

// Set a solid color as the background
async function setBackgroundColor(videoTrack, hexColor) {
  if (videoTrack) {

    let processor = await getProcessorInstance(videoTrack);

    console.log(hexColor);

    try {
      processor.setOptions({type: 'color', color: hexColor.toString()});
      await processor.enable();
    } finally {
    }

    virtualBackgroundEnabled = true;
  }
}

// Blur the user's actual background
async function setBackgroundBlurring(videoTrack, myBlur) {
  if (videoTrack) {

    let processor = await getProcessorInstance(videoTrack);

    try {
      processor.setOptions({type: 'blur', blurDegree: myBlur});
      await processor.enable();
    } finally {
    }

    virtualBackgroundEnabled = true;
  }
}

// Set an image as the background
async function setBackgroundImage(videoTrack, imgFile) {
    const imgElement = document.createElement('img');

    imgElement.onload = async() => {

      let processor = await getProcessorInstance(videoTrack);

      try {
        processor.setOptions({type: 'img', source: imgElement});
        await processor.enable();
      } finally {
      }

      virtualBackgroundEnabled = true;
    }
    imgElement.src = './AgoraWebSDK/assets/images/' + imgFile;
}

async function setBackgroundVideo(videoTrack, videoFile) {
  const videoElement = document.createElement('video');
  
  console.log('./AgoraWebSDK/assets/videos/' + videoFile);

  videoElement.oncanplay = async() => {

    let processor = await getProcessorInstance(videoTrack);

    try {
      processor.setOptions({type: 'video', source: videoElement});
      await processor.enable();
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
  console.log(videoElement);
}