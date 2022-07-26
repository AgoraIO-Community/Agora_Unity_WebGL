# Agora Unity WebGL 插件和示例

*[English](README.md) | 中文*

此项目是Unity WebGL的开源项目，包装了Agora Web SDK4.x版本。

**请注意到此项目还处于Beta阶段，不能保证所有功能没有错误和非预期行为，可能存在跟Agora Unity SDK版本行为不一致的情况。在下载和正式使用之前，请仔细阅读README文件。如果您喜欢这个项目，请点击Star，谢谢！** 

## 前置条件
- Unity 2017 以上版本
- Agora 开发者账号
- 理解如何部署、调试Unity WebGL项目
- Google Chrome 浏览器

## Unity WebGL Demo 截图

![webgldemo](https://user-images.githubusercontent.com/1261195/180123220-ca7e96e2-bff5-48d7-b5bd-30a37003bdc9.png)


## 快速开始

这一部分介绍如何准备、编译和运行示例程序
 

### 获取 App ID

在您编译和运行任何Agora项目时，必须获取App ID和做相关的配置。请移步至 [开发者账号的 项目管理](https://console.agora.io/projects), 创建新的App ID 或者拷贝一个已有的项目App ID。
**注意** 对于生产环境项目，您必须使用开启了Token验证的项目App ID。当然，如果仅仅是测试，可以忽略使用Token，使用测试模式，可以在POC集成阶段节省大量时间.
![enter image description here](https://user-images.githubusercontent.com/1261195/110023464-11eb0480-7ce2-11eb-99d6-031af60715ab.png)

  

### 运行Demo程序

1. 即可以克隆此仓库,  也可以直接下载发布的unity package文件, 移步至 [发布页面](https://github.com/AgoraIO-Community/Agora_Unity_WebGL/releases) ，下载最新版本.
2. [可选] 下载并导入Agora Video SDK
3. 导入WebGL SDK 包(Unity Editor-> Assets -> Import Package -> Custom Package). 如果第2步已经做了，这里会覆盖之前的所有文件。
4. 在 Project 窗口, 打开 Asset/AgoraEngine/Demo/Main.scene
5. 下一步, 找到 Hierarchy window 并选择  ****GameController****, 在Inspector 中添加 ****App ID****  到  ****AppID****  输入框.

### API-Examples
这个仓库包括了API-Examples的的子集，其编译版本来自 [主板SDK版本](https://github.com/AgoraIO/Agora-Unity-Quickstart/tree/master/API-Example-Unity).
请使用Canvas game object来填写App ID，而不是GameController.

### [可选] 使用Editor测试
您必须在第2步下载Agora Video SDK

### 编译和运行

 1. 打开  ****File****  >  ****Builds****  >  ****Platform****  ，然后 切换至 WebGL 平台
 2. 确保 "Resolution and Presentation"设置，选择了****AgoraTemplate**** 或者 ****AgoraTemplate2020****
![webgl_template](https://user-images.githubusercontent.com/1261195/130500369-53dca294-2cf5-4a0d-a875-a8ab8fbabd70.png)

模板的差别: 如果是Unity Editor 2020以及以上版本，使用 AgoraTemplate2020模板.

 3. 对于Unity Editor 2020以及以上版本，您可以直接编译和运行。对于Unity 2017,需要先编译，然后从本地Http服务器上运行。




## 资源
  
请点击 [Wiki页面](https://github.com/AgoraIO-Community/Agora_Unity_WebGL/wiki) 查阅更多信息

## 许可
The MIT License (MIT).  [查阅文件内容](LICENSE)
