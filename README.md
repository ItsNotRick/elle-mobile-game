##### Website
http://endlesslearner.com/
##### Main Github Page
https://github.com/ItsNotRick/elle

# ELLE Mobile
ELLE Mobile is a pair of Unity games created by the second ELLE senior design group at UCF.


## ELLE Mobile3D
This game has two gamemodes, one matches the gameplay of the original ELLE 1.0 over-the-shoulder mode and the other is an augmented reality scavenger hunt for translated words. It connects to [this API](https://github.com/ItsNotRick/elle-web-service) to download language packs and stores them locally.

### For readers wishing to contribute to ELLEMobile3D
#### Build Environment
Install [Unity  2018.3.0f2](https://store.unity.com/download), I recommend using Unity Hub to keep track of multiple versions of unity. Additionally you will need some [android tools](https://developer.android.com/studio) in order to build APKs. Personally I use the command line tools to save space but the full studio package should work as well. Currently we do not support IL2CPP builds but if you're trying to tackle that make sure you have the [correct version of the NDK](https://developer.android.com/ndk/downloads/revision_history) for your version of Unity. There are a few dlls being used that might need some TLC to get working in your specific environment. Feel free to create an issue here on github if you're having trouble.\
Don't forget to keep adding files and folders related to your specific build configuration to the gitignore.

Read these C# files:
###### Language Pack Code
* LoginMangager.cs
* SessionManager.cs
* LanguagePackInterface.cs
###### Game Logic
* GroundGeneration.cs
* PlayerMovementControls.cs
### Potential Additions:
* Add desktop controls, support more platforms (iOS!)
* Music, menu music, in-game music, etc.
* More efficient language pack downloading
* Speed up the gameplay as the player successfully dodges obstacles
* More obvious, easier to see obstacles
* Better performance data collection
* A disabled speech based gamemode could be reworked and re-integrated with the game
* Audio as a presented word format

## ELLE Mobile2D
Most of the Language pack code is the same as Mobile3D, the gameplay is a bit simplistic and there's a huge amount of time the player spends not answering questions. This game could benefit from a gameplay overhaul.
