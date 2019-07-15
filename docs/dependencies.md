# Package Dependencies
### TextMeshPro

# Asset Dependencies

### LeapMotion Orion by [LeapMotion](https://github.com/leapmotion/UnityModules)

[Package Download](https://github.com/leapmotion/UnityModules/releases)

Used as an input system, interaction system and gesture system for LeapMotion and OSVR controls.  (Essential to Function)

Free, Open-Source asset.  LeapMotion controller functionality dependent on closed source drivers currently only available on Windows.  OSVR functionality works without the closed source driver.

- [x] Core SDK
- [x] Interaction Engine
- [ ] UI
- [ ] Graphics
- [x] Hands


### LeapMotion App Modules by [LeapMotion](https://github.com/leapmotion/AppExperiments)

[Package Download](https://github.com/maine-imre/LM-AppExperiments/releases)  *

Free and Open-Source. Used as a main control interface. (Essential to function).  This asset requires some modification in order for it to compile.  We hope to document this process (or include a copy of  the source) in the future.

*The package download is from a fork of LeapMotion's origional repository, that we have modified to ensure compatability with our repository.

***The package is temporarily a derivative of [LeapPaint](https://github.com/maine-imre/Paint/releases)***

### Photon PUN2 by [Exit Games](https://www.photonengine.com/en/pun)

[Package Download](https://assetstore.unity.com/packages/tools/network/pun-2-free-119922)

Paid Asset. Used to add multiplayer support. (Essential to function)

### Discord Rich Presence by [Discord](https://discordapp.com/developers/docs/rich-presence/how-to)

[Package Download](https://github.com/maine-imre/discord-rpc/releases)

Free and Open-Source.  Used to configure PUN networking.


# Optional Add-Ons whose integration has been teseted.
The use of these closed-source assets is restricted to (1) a fork of the repository or (2) a local copy of the repository.  No code dependant on these solutions should be included in the main repository.


### BlueprintReality - MixCast SDK
[BluePrintReality](https://mixcast.me/mixcast-download/)

Free Asset. Used to record mixed reality footage. (Non-essential to function)

### TiltBrush Toolkit
[Google](https://github.com/googlevr/tilt-brush-toolkit/releases)

Free Asset. Used to import TiltBrush drawings into the project. (Non-essential to function)

### HTC Unity Plugin
[HTC Github](https://github.com/ViveSoftware/ViveInputUtility-Unity/releases)

Free Asset. Will be used for spacial audio and locomotion. (Non-essential to function)

### Viveport
[HTC Vive](https://developer.viveport.com/documents/sdk/en/download.html)

Free Asset. Will be used for telemetrics. (Non-essential to function)

### Pool Manager
[Path-o-logical Games](http://u3d.as/1Z4)

Paid Asset. Object pooling for GeoObjects and GeoUI. (Essential to function)
We have used this in the past for instancing objects, but are working to be independent of it.
PoolManager may offer some performance gains, and can be dropped into the application by simply reomving the PoolManager.cs script in the Assets/Scripts folder.

### SteamVR
[Valve](http://u3d.as/cjo)

Free Asset. 

### Vive Pro Hand Tracking
[Vive Hand Tracking](https://developer.vive.com/resources/knowledgebase/vive-hand-tracking-sdk/)

Free, early access. Used for allowing gesture controls through the camera of the Vive Pro.
