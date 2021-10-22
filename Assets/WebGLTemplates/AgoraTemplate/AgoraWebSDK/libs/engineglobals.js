// conversion to new engine
var SDK_VERSION = "0.3.5.0.463100";

let client_manager = new ClientManager();
let dataBuilder = DataBuilder();
let wrapper = WglWrapper();
let audioEffects = AudioEffects();
let event_manager = new EventManager();
var autopublish = true; // if true then after join auto publishes, else have to manually publish
var client_role = 1; // default is host, 2 is audience
// saved devices information
var currentVideoDevice = "";
var currentAudioDevice = "";
var currentPlayBackDevice = "";

var localTracks = {
  videoTrack: null,
  audioTrack: null,
  audioMixingTrack: null,
  audioEffectTrack: null,
};
// we can check the number of joins and leave, based on it we decided
// if we need to delete local track
var multiclient_connections = 0; // how many multi clients are joined

var remoteUsers = {};
var savedEncryptionMode = "";
var pd_muted = 0; // playback devices muted or not
var mlocal = true;
var mremote = false;
var isAudioMute = true;
var isVideoMute = true;

var isRemoteAudioMute = true;
var isRemoteVideoMute = true;
var TO_RADIANS = Math.PI / 180;
// multi clients
var clients = {};
var selectedCurrentChannel = "";
var multiChannelWant_MC = false;

// URLs used for Transcoding feature.
const DEFAULT_IMAGE_URL_TRANSCODING =
  "https://agoraio-community.github.io/AgoraWebSDK-NG/img/logo.png";
const DEFAULT_BG_URL_TRANSCODING =
  "https://agoraio-community.github.io/AgoraWebSDK-NG/img/sd_rtn.jpg";
//Live Transcoding
var liveTranscodingConfig;

// you can find all the agora preset video profiles here https://docs.agora.io/cn/Voice/API%20Reference/web/interfaces/agorartc.stream.html#setvideoprofile
var videoProfiles = [
  { label: "480p_1", detail: "640×480, 15fps, 500Kbps", value: "480p_1" },
  { label: "480p_2", detail: "640×480, 30fps, 1000Kbps", value: "480p_2" },
  { label: "720p_1", detail: "1280×720, 15fps, 1130Kbps", value: "720p_1" },
  { label: "720p_2", detail: "1280×720, 30fps, 2000Kbps", value: "720p_2" },
  { label: "1080p_1", detail: "1920×1080, 15fps, 2080Kbps", value: "1080p_1" },
  { label: "1080p_2", detail: "1920×1080, 30fps, 3000Kbps", value: "1080p_2" },
  {
    label: "200×640",
    detail: "200×640, 30fps",
    value: { width: 200, height: 640, frameRate: 30 },
  }, // custom video profile
];

var curVideoProfile = videoProfiles[0];

var preloadedTrack;
var preloadConfig = {
  cacheOnlineFile: false,
  encoderConfig: "music_standard",
  source: "audio.mp3",
};

function _logger(msg) {
  /*console.log(
    "%c " + msg,
    "color: pink; font-weight: bold; background-color: black;"
  );*/
}

function _throw(msg) {
  /*console.log(
    "%c " + msg,
    "color: white; font-weight: bold; background-color: red;"
  );*/
}
/*
setInterval(function () {
  $("#info").html("multiclient_connections: " + multiclient_connections);
}, 3000);*/
