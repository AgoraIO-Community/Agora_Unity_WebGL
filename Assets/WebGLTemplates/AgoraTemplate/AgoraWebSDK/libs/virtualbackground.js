let extension = null;
let imgElement = null;
let videoElement = null;


// Initialization
async function getVirtualBackgroundProcessor(videoTrack, enabled, backgroundSourceType, color, source, blurDegree, mute, loop) {

  if (extension == null) {
    // Create a VirtualBackgroundExtension instance
    extension = new VirtualBackgroundExtension();
    // Register the extension
    AgoraRTC.registerExtensions([extension]);
  }

  let processor = extension.createProcessor();

  try {
      // Initialize the extension and pass in the URL of the Wasm file
      await processor.init("./assets/agora-wasm");
    } catch(e) {
        console.log("Fail to load WASM resource!");return null;
    }

  if (videoTrack != undefined) {
    // Create a VirtualBackgroundProcessor instance
    
    // Inject the extension into the video processing pipeline in the SDK
    videoTrack.pipe(processor).pipe(videoTrack.processorDestination);
  }

  if(backgroundSourceType == 3){
   processor = await setBackgroundBlurring(processor, videoTrack, blurDegree);
  } else if(backgroundSourceType == 1){
   processor = await setBackgroundColor(processor, videoTrack, color);
  } else if(backgroundSourceType == 2){
   processor = await setBackgroundImage(processor, videoTrack, source);
  } else if(backgroundSourceType == 4){
   processor = await setBackgroundVideo(processor, videoTrack, source, mute, loop);
  }

  console.log(processor);

  if(enabled == true){
   await processor.enable();
  } else {
   await processor.disable();
  }

  return processor;
}

async function setVirtualBackgroundProcessor(processor, videoTrack, enabled, backgroundSourceType, color, source, blurDegree, mute, loop) {

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

    if(videoElement != null){
      videoElement.pause();
      videoElement.currentTime = 0;
      videoElement = null;
    }


    try {
      processor.setOptions({type: 'color', color: '#' + Math.abs(hexColor).toString(16)});
    } finally {
    }

    virtualBackgroundEnabled = true;
  }

  return processor;
}

// Blur the user's actual background
async function setBackgroundBlurring(processor, videoTrack, myBlur) {
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

  return processor;
}

// Set an image as the background
async function setBackgroundImage(processor, videoTrack, imgFile) {
if(videoTrack){
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

    return processor;
}

async function setBackgroundVideo(processor, videoTrack, videoFile, mute, loop) {
  if (videoTrack) {
    videoElement = document.createElement('video');

    console.log('./AgoraWebSDK/assets/videos/' + videoFile);

    videoElement.oncanplay = async () => {

      try {
        processor.setOptions({ type: 'video', source: videoElement });
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
  }

  return processor;
}