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

**Note LeapMotion is not open source, so we are moving to remove them from our dependencies.
We are hoping to preserve LeapMotion Core as a sensor input that we support, but are working to make our source
compile without having LM as a dependency.

**We are also considering adding support for ViveSense, Intel RealSense and Valve's SteamVR Skeletal tracking as
alternative data sources. 

### Photon PUN2 by [Exit Games](https://www.photonengine.com/en/pun)

[Package Download](https://assetstore.unity.com/packages/tools/network/pun-2-free-119922)

Paid Asset. Used to add multiplayer support. (Essential to function)

**Note Photon is not open source, so we are moving to remove them from our dependencies.

<!---
# Optional Add-Ons whose integration has been teseted.
The use of these closed-source assets is restricted to (1) a fork of the repository or (2) a local copy of the repository.  No code dependant on these solutions should be included in the main repository.

### Discord Rich Presence by [Discord](https://discordapp.com/developers/docs/rich-presence/how-to)

[Package Download](https://github.com/maine-imre/discord-rpc/releases)

Free and Open-Source.  Used to configure PUN networking.

**support pending

### BlueprintReality - MixCast SDK
[BluePrintReality](https://mixcast.me/mixcast-download/)

Free Asset. Used to record mixed reality footage. (Non-essential to function)
-->
