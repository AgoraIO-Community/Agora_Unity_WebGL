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

    try {
      processor.setOptions({type: 'color', color: hexColor});
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
async function setBackgroundImage(videoTrack) {
    const imgElement = document.createElement('img');

    imgElement.onload = async() => {
      document.getElementById("loading").style.display = "block";

      let processor = await getProcessorInstance(videoTrack);

      try {
        processor.setOptions({type: 'img', source: imgElement});
        await processor.enable();
      } finally {
        document.getElementById("loading").style.display = "none";
      }

      virtualBackgroundEnabled = true;
    }
    imgElement.src = '/images/background.png';
}