# Daggerfall-Unity-VR/
![Bat_Sword - Copy1](https://github.com/user-attachments/assets/562c49e8-9d7b-4ac5-b5f4-c321ea68654c)
 DFUVR is a VR mod for the open source recreation of the classic 1996 RPG Daggerfall in the Unity engine [Daggerfall Unity](https://github.com/Interkarma/daggerfall-unity)

### This project is currently in early access. 
Bugs are to be expected. If you find any, please either create an issue directly here on GitHub or in the Daggerfall Unity VR category on the [Flat2VR discord server](http://flat2vr.com/). This is a hobby project of mine, so please don't expect me to immediately fix them.
There is currently no left handed mode.
###Please read through this README carefully before installing and playing!
## Installation

#### You'll need:
- A working [Daggerfall Unity](https://github.com/Interkarma/daggerfall-unity) installation
- A 1080p monitor or higher
- (Obviously) A VR headset and a SteamVR compatible computer
- [SteamVR](https://store.steampowered.com/app/250820/SteamVR/) installed
- [Daggerfall DOS](https://store.steampowered.com/app/1812390/The_Elder_Scrolls_II_Daggerfall/) installed


#### Step 0:
Start Daggerfall Unity atleast once and go through its installation process. 

If you have previously played Daggerfall Unity, please backup your KeyBinds.txt and Settings.ini(preferably the whole folder) located under 
%USERPROFILE%\AppData\LocalLow\Daggerfall Workshop\Daggerfall Unity\

The mod WILL irreversibly override your key bindings if you don't!

Additionally, I'd recommend to try this mod first without any other additional mods installed.

#### Step 1:
Download the latest release and unzip all of the folder's content into your Daggerfall Unity root folder

#### Step 2:
Set all of your connected monitors to 1920x1080. Failing to do so will make the in-game UI unusuable, with it outright not rendering in most cases.

#### Step 3:
Start SteamVR.

#### Step 4: 
Start Daggerfall Unity.

#### Step 5: 
Go through the initial setup menu. Under "Controller type", pick "Oculus/Meta" if you use oculus touch controllers or pico 4 controllers. If you use Vive wands, pick "HTC Vive Wands". For any other controllers pick "other". Please note, that other controllers/such as the index knuckles) are currently not fully supported.

#### Step 5.5:
Restart the game

#### Step 6:
Start a new game or load a save. Afterwards go into calibration mode to set your height and the position of the scabbard. You can adjust your height by moving the right controller thumbstick up/down.

## Controller bindings:
### Currently, only Oculus/Meta Touch, Pico 4 controllers and Vive wands are supported. Other controllers may work, but I can't guarantee it.
#### Touch/Pico Controllers:
![Oculus Touch bindings](https://github.com/LokiusV/Daggerfall-Unity-VR/blob/main/docs/TouchControllers_DFUVR_Bindings_fin.webp?raw=true)
#### HTC Vive Wands:
![HTC Vive wand bindings](https://github.com/LokiusV/Daggerfall-Unity-VR/blob/main/docs/Wands_DFUVR_bindings.webp?raw=true)

## Important tips
- PLEASE ALWAYS KEEP YOUR GAME WINDOW IN FOCUS!!!!
After spending way too much time even getting the UI to show up, I got lazy and took the easy way of interacting with the ui, by just simply making the laser click your actual mouse cursor on your monitor. This is not only the reason why the pointer precision gets worse the closer you get to the edges of the UI but also why you should always keep the game window focused in order not to accidentally click on anything you don't want to click on.
The same goes for the keyboard. It just emulates a keyboard press, so having any other application(like discord) in focus, even just on your second monitor, will press it in that application
- It's best to do the Daggerfall Unity first setup and the creation of a new character on flat screen. Not that it's not possible to do in VR but it can feel a bit awkward


# Building DFUVR from source:
## You do not need to build the mod from source to play it. Please just follow the installation instructions above. It's only necessary to build it if you want to change something in the mod!

#### You'll need:
- BepInEx v. 5.4-> please follow the [BepInEx installation instructions](https://docs.bepinex.dev/articles/user_guide/installation/index.html)
- Visual Studio 2022

#### Step 0:

Backup your Daggerfall Unity installation, including all configuration files(bindings, settings, etc.).

#### Step 1:
open the project in Visual Studio 2022 and fix any missing assembly references(if any)

#### Step 2:
Compile the mod

#### Step 3:
move the now newly created "DFUVR.dll"(located under (yourLocationToTheRepo)/DFUVR/bin/Debug) to (YourGameInstallationRootDirectory)/BepInEx/plugins

#### Step4
move everything from (yourLocationToTheRepo)/DFUVR/GameFolderFiles to (YourGameInstallationRootDirectory)

#### Step 5:
Adjust settings.txt to your liking-> First line is your height(this can be left at zero and then later calibrated in-game); Second line should be your VR headsets current refresh rate; Third line is your Controller profile(currently only Oculus/Meta Touch controllers are supported); Fourth line is the offsett of the Sheath. Just leave that at the default value for now and calibrate it in-game

#### Step 6:
Enjoy!(Or if something doesn't work, create a GitHub issue because I probably forgot a step)

### Currently known issues:
 - UI is only visible when monitor resolution is set to 1920x1080
 - The VR laser-pointer is not very precise when clicking on the outer edges of the UI

## Special thanks to:
All my beta testers! Including but not limited to:
- WealthyPixelCollector
- WurmVR
- lewisviper
- slogvapes
- Martin  
## Additional Assets used
I have used the following Unity Asset Store Standard License Assets as weapon models for this project:
- https://assetstore.unity.com/packages/3d/props/weapons/ukit-medieval-hammer-free-97625
- https://assetstore.unity.com/packages/3d/props/weapons/fantasy-staff-lite-54999
- https://assetstore.unity.com/packages/3d/long-sword-with-sheath-67467
- https://assetstore.unity.com/packages/3d/props/weapons/steel-dagger-pbr-86188 
- https://assetstore.unity.com/packages/3d/props/weapons/low-poly-weapons-71680

