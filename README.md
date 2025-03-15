# Daggerfall-Unity-VR/
 DFUVR is a VR mod for the open source recreation of the classic 1996 RPG Daggerfall in the Unity engine [Daggerfall Unity](https://github.com/Interkarma/daggerfall-unity)

## This project is currently in early access. 
Bugs are to be expected. If you find any, please either create an issue directly here on GitHub or in the Daggerfall Unity VR category on the Flat2VR discord server. This is a hobby project of mine, so please don't expect me to immediately fix them.
There is currently no left handed mode.

# Installation
### Step 0:
Start Daggerfall Unity atleast once and go through its installation process. 

If you have previously played Daggerfall Unity, please backup your KeyBinds.txt and Settings.ini(preferably the whole folder) located under 
%USERPROFILE%\AppData\LocalLow\Daggerfall Workshop\Daggerfall Unity\

The mod WILL irreversibly override your key bindings if you don't!

Additionally, I'd recommend to try this mod first without any other additional mods installed.

### Step 1:
Download the latest release and unzip all of the folder's content into your Daggerfall Unity root folder

### Step 2:
Set all of your connected monitors to 1920x1080. Failing to do so will make the in-game UI unusuable, with it outright not rendering in most cases.

### Step 3:
Start SteamVR.

### Step 4: 
Start Daggerfall Unity.

### Step 5: 
Go through the initial setup menu. Under "Controller type", pick "Oculus/Meta" if you use oculus touch controllers or pico 4 controllers. If you use Vive wands, pick "HTC Vive Wands". For any other controllers pick "other". Please note, that other controllers/such as the index knuckles) are currently not fully supported.

### Step 6:
Start a new game or load a save. Afterwards go into calibration mode to set your height and the position of the scabbard. You can adjust your height by moving the right controller thumbstick up/down.

# Controller bindings:
### Currently, only Oculus/Meta Touch, Pico 4 controllers and Vive wands are supported. Other controllers may work, but I can't guarantee it.
#### Touch/Pico Controllers:
![Oculus Touch bindings](https://github.com/LokiusV/Daggerfall-Unity-VR/blob/main/docs/TouchControllers_DFUVR_Bindings_fin.webp?raw=true)
#### HTC Vive Wands:
![HTC Vive wand bindings](https://github.com/LokiusV/Daggerfall-Unity-VR/blob/main/docs/Wands_DFUVR_bindings.webp?raw=true)


# Building DFUVR from source:
## You do not need to build the mod from source to play it. Please just follow the installation instructions above. It's only necessary to build it if you want to change something in the mod!

### You'll need:
- BepInEx v. 5.4-> please follow the BepInEx installation instructions ( https://docs.bepinex.dev/articles/user_guide/installation/index.html )
- Visual Studio 2022

### Step 0:

Backup your Daggerfall Unity installation, including all configuration files(bindings, settings, etc.).

### Step 1:
open the project in Visual Studio 2022 and fix any missing assembly references(if any)

### Step 2:
Compile the mod

### Step 3:
move the now newly created "DFUVR.dll"(located under (yourLocationToTheRepo)/DFUVR/bin/Debug) to (YourGameInstallationRootDirectory)/BepInEx/plugins

### Step4
move everything from (yourLocationToTheRepo)/DFUVR/GameFolderFiles to (YourGameInstallationRootDirectory)

### Step 5:
Adjust settings.txt to your liking-> First line is your height(this can be left at zero and then later calibrated in-game); Second line should be your VR headsets current refresh rate; Third line is your Controller profile(currently only Oculus/Meta Touch controllers are supported); Fourth line is the offsett of the Sheath. Just leave that at the default value for now and calibrate it in-game

### Step 6:
Enjoy!(Or if something doesn't work, create a GitHub issue because I probably forgot a step)

Currently known issues:
 - UI is only visible when monitor resolution is set to 1920x1080
 - The VR laser-pointer is not very precise when clicking on the outer edges of the UI
 - Climbing does not work
 - the UI occasionally spawns directly in the face of the user

