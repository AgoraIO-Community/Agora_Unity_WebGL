# Agora Unity WebGL Plugin and Demo

This project is the open source WebGL SDK for Unity, a wrapper to the Agora Web SDK 4.x.  

**Please be aware that this is still a beta release.  Do not assume everything works correctly as an official product. Do not expect the same behaviors from the original Unity SDK. You should check the README files to understand this SDK some more before proceeding to download. Do press the Star to show respect to people who work hard on creation of this SDK on your demand.** 

## Prerequisites
- Unity 2017 and up
- Agora Developer Account
- Knowledge of how to deploy and debug WebGL Application from Unity
- Google Chrome

## Updated main Demo

![Demo for WebGL](https://user-images.githubusercontent.com/1261195/130497083-45cc6d5e-fa1e-4581-8ea4-b93a27f2f6cf.png)



## Quick Start

This section shows you how to prepare, build, and run the sample application.
 

### Obtain an App ID

Before you can build and run any Agora project, you will need to add your AppID to the configuration. Go to your [developer account’s project console](https://console.agora.io/projects), create a new AppId or copy the AppId from an existing project. 

**Note** it is important that for a production ready project, you should always use an AppId with token enabled.  However, in testing a demo, you will skip this part.  Using a testing mode AppId can save time for POC integration.
![enter image description here](https://user-images.githubusercontent.com/1261195/110023464-11eb0480-7ce2-11eb-99d6-031af60715ab.png)

  

### Run the Demo Application

1. It is optional to clone this repository,  to obtain the unity package file, go to [the release section](https://github.com/AgoraIO-Community/Agora_Unity_WebGL/releases) and download the latest version.
2. [Optional] Download and import the Agora Video SDK
3. Import the WebGL SDK package (Unity Editor-> Assets -> Import Package -> Custom Package). Overwrite all the files if Step 2 was performed.
4. From Project window, open Asset/AgoraEngine/Demo/Main.scene
5. Next, go into your Hierarchy window and select  ****GameController****, in the Inspector add your  ****App ID****  to to the  ****AppID****  Input field.

### API-Examples
This repository includes a subset of API-Examples that resembles [the main SDK's version](https://github.com/AgoraIO/Agora-Unity-Quickstart/tree/master/API-Example-Unity).
Please use the Canvas game object to fill the AppID instead of GameController.

### [optional] Test in Editor
You must download the Agora Video SDK in Step 2 above.

### Build and Run

 1. Go to  ****File****  >  ****Builds****  >  ****Platform****  and switch to platform WebGL 
 2. Make sure ****AgoraTemplate**** or ****AgoraTemplate2020**** is chosen under "Resolution and Presentation":
![webgl_template](https://user-images.githubusercontent.com/1261195/130500369-53dca294-2cf5-4a0d-a875-a8ab8fbabd70.png)
The difference of the templates: use AgoraTemplate2020 for Unity Editor 2020 and up.
 3. You may do Build And Run for Unity version 2020 and up.  For Unity 2017, you need to build first, then run it from your local http server. 




## Resources
  
Please see [the Wiki page](https://github.com/AgoraIO-Community/Agora_Unity_WebGL/wiki) for more information.

## License
The MIT License (MIT).  [See doc.](LICENSE)
