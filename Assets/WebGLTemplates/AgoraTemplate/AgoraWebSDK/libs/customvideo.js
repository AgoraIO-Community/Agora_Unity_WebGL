var customVideoTrack;
var listingOfWatermarks = Array();
var _isCopyVideoToMainCanvasOn = false;
var requestAnimationFrame_CVTMC =
  window.requestAnimationFrame ||
  window.mozRequestAnimationFrame ||
  window.webkitRequestAnimationFrame ||
  window.msRequestAnimationFrame;
var wmConfig = { url: "", x: 0, y: 0, width: 0, height: 0 };

async function setExternalVideoSource_WGL(enable) {
  client_manager.setExternalVideoSource_WGL(enable);
}

function pushVideoFrame_WGL(
  arr,
  buf,
  size,
  stride,
  height,
  rotation,
  cropLeft,
  cropTop,
  cropRight,
  cropBottom
) {
  var array = new Uint8ClampedArray(arr);
  var image = new ImageData(array, stride, height);

  inMemCanvas.width = stride; // in pixels
  inMemCanvas.height = height; // in pixels
  mainCanvas.width = stride; // in pixels
  mainCanvas.height = height; // in pixels

  inMemContext.clearRect(0, 0, inMemCanvas.width, inMemCanvas.height);
  inMemContext.putImageData(image, 0, 0);

  let imageData = inMemContext.getImageData(
    cropLeft,
    cropTop,
    inMemCanvas.width - cropRight,
    inMemCanvas.height - cropBottom
  );

  inMemContext.clearRect(0, 0, inMemCanvas.width, inMemCanvas.height);
  inMemContext.putImageData(imageData, 0, 0); // here we can shift image, left and right padding put at x and y

  mainContext.clearRect(0, 0, mainCanvas.width, mainCanvas.height);
  mainContext.save();
  mainContext.translate(mainCanvas.width / 2, mainCanvas.height / 2);
  mainContext.rotate(rotation * TO_RADIANS);
  mainContext.drawImage(
    inMemCanvas,
    (-1 * mainCanvas.width) / 2,
    (-1 * mainCanvas.height) / 2
  );
  mainContext.restore();
}

function copyVideoToMainCanvas() {
  if (_isCopyVideoToMainCanvasOn) {
    var cvideo = customVideoTrack._player.videoElement;
    mainCanvas.width = cvideo.videoWidth;
    mainCanvas.height = cvideo.videoHeight;
    mainContext.drawImage(cvideo, 0, 0, mainCanvas.width, mainCanvas.height);

    listingOfWatermarks.forEach(function (item) {
      var _myImage = new Image();
      _myImage.src = item.url;
      mainContext.drawImage(_myImage, item.x, item.y, item.width, item.height);
    });
  }
  if (_isCopyVideoToMainCanvasOn) {
    requestAnimationFrame_CVTMC(copyVideoToMainCanvas);
  }
}

async function clearVideoWatermarks_WGL() {
  client_manager.clearVideoWatermarks_WGL();
}

async function startWaterMark_WGL(url, x, y, width, height) {
  client_manager.startWaterMark_WGL(url, x, y, width, height);
}
