# Daggerfall-Unity-VR
 A VR mod for the open source recreation of the classic 1996 RPG Daggerfall in the Unity engine, called 'Daggerfall Unity'

# This repository is still a work in progress. 
I'm currently only doing closed beta tests. If you want to play the mod in it's (still pretty early) current state, please either contact me or build the mod yourself.
## To build the mod you'll need:
- BepInEx v. 5.4-> please follow the BepInEx installation instructions under https://docs.bepinex.dev/articles/user_guide/installation/index.html
- Visual Studio 2022
### Step 1:
open the project in visual studio and fix any missing assembly references(if any)

### Step 2:
Compile the mod

### Step 3:
move the now newly created "DFUVR.dll"(located under (yourLocationToTheRepo)/DFUVR/bin/Debug) to (YourGameInstallationRootDirectory)/BepInEx/plugins

### Step4
move everything from (yourLocationToTheRepo)/DFUVR/GameFolderFiles to (YourGameInstallationRootDirectory)

### Step 5: Take a look at the source code to find out how the controls work because I didn't yet create an overview of the bindings(sorry. Will do that later)

### Step 6:
Enjoy!(Or if something doesn't work, create a GitHub issue because I probably forgot a step)

Currently known issues:
 - UI is only visible when monitor resolution is set to 1920x1080
 - The VR laser-pointer is not very precise when clicking on the outer edges of the UI
 - Climbing does not work
 - the UI occasionally spawns directly in the face of the user

