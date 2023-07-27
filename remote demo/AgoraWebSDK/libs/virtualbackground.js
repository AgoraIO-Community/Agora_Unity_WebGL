let extension = null;
let imgElement = null;
let videoElement = null;
let myProcessor = null;
let backgroundType = null;

// Initialization
async function getVirtualBackgroundProcessor(videoTrack, enabled, backgroundSourceType, color, source, blurDegree, mute, loop) {
  backgroundType = backgroundSourceType;
  if (extension == null) {
    // Create a VirtualBackgroundExtension instance
    extension = new VirtualBackgroundExtension();
    // Register the extension
    AgoraRTC.registerExtensions([extension]);
  }

  if(myProcessor === null){
    myProcessor = await extension.createProcessor();
  }

  try {
      // Initialize the extension and pass in the URL of the Wasm file
      await myProcessor.init("./assets/agora-wasm");
    } catch(e) {
        console.log("Fail to load WASM resource!");return null;
    }

  if (videoTrack != undefined) {
    // Create a VirtualBackgroundProcessor instance
    
    // Inject the extension into the video processing pipeline in the SDK
    videoTrack.pipe(myProcessor).pipe(videoTrack.processorDestination);
  }

  if(backgroundSourceType == 3){
    myProcessor = await setBackgroundBlurring(myProcessor, videoTrack, blurDegree);
  } else if(backgroundSourceType == 1){
    myProcessor = await setBackgroundColor(myProcessor, videoTrack, color);
  } else if(backgroundSourceType == 2){
    myProcessor = await setBackgroundImage(myProcessor, videoTrack, source);
  } else if(backgroundSourceType == 4){
    myProcessor = await setBackgroundVideo(myProcessor, videoTrack, source, mute, loop);
  }

  if(enabled == true){
   await myProcessor.enable();
  } else {
   await myProcessor.disable();
  }

  return myProcessor;
}

async function setVirtualBackgroundProcessor(processor, videoTrack, enabled, backgroundSourceType, color, source, blurDegree, mute, loop) {
  backgroundType = backgroundSourceType;
  if(backgroundSourceType == 3){
    processor = await setBackgroundBlurring(processor, videoTrack, blurDegree);
  } else if(backgroundSourceType == 1){
    processor = await setBackgroundColor(processor, videoTrack, color);
  } else if(backgroundSourceType == 2){
    processor = await setBackgroundImage(processor, videoTrack, source);
  } else if(backgroundSourceType == 4){
    processor = await setBackgroundVideo(processor, videoTrack, source, mute, loop);
  }

  if(enabled == true){
    await processor.enable();
  } else {
    await processor.disable();
  }

  return processor;
}

// Set a solid color as the background
async function setBackgroundColor(processor, videoTrack, hexColor) {
  if (videoTrack) {
    backgroundType = 1;
    if(videoElement != null){
      videoElement.pause();
      videoElement.currentTime = 0;
      //videoElement = null;
    }

    myColor = hexColor.toString(16);
    console.log(myColor);

    try {
      processor.setOptions({type: 'color', color: "#" + myColor});
    } finally {
    }

    virtualBackgroundEnabled = true;
  }

  return processor;

}

// Blur the user's actual background
async function setBackgroundBlurring(processor, videoTrack, myBlur) {
  if (videoTrack) {
    backgroundType = 3;
    if(videoElement != null){
      videoElement.pause();
      videoElement.currentTime = 0;
      //videoElement = null;
    }

    console.log(processor);

    try {
      processor.setOptions({type: 'blur', blurDegree: myBlur});
    } finally {
    }

    virtualBackgroundEnabled = true;
  }


  return processor;
}

// Set an image as the background
async function setBackgroundImage(processor, videoTrack, imgFile) {
if(videoTrack){
    backgroundType = 2;
    if(videoElement != null){
      videoElement.pause();
      videoElement.currentTime = 0;
      //videoElement = null;
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

  return processor;
}

async function setBackgroundVideo(processor, videoTrack, videoFile, mute, loop) {
  if (videoTrack) {
    backgroundType = 4;
    videoElement = document.createElement('video');

    videoElement.oncanplay = async () => {
      console.log(backgroundType);
      try {
        if(backgroundType === 4){
          processor.setOptions({ type: 'video', source: videoElement });
        }
      } catch (e) {
      }

      virtualBackgroundEnabled = true;
    }
    videoElement.src = 'AgoraWebSDK/assets/videos/' + videoFile;
    videoElement.type = "video/mp4";
    videoElement.width = 800;
    videoElement.height = 600;
    videoElement.loop = loop;
    videoElement.muted = mute;
    videoElement.play();
    console.log("setting video");
  }

  return processor;

}
