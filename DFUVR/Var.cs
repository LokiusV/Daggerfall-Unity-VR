using BepInEx;
using DaggerfallWorkshop;
using DaggerfallWorkshop.AudioSynthesis.Synthesis;
using DaggerfallWorkshop.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using static DaggerfallWorkshop.Game.InputManager;
using UnityEngine;
using UnityEngine.UI;

namespace DFUVR
{
    //This class just stores alot of (semi-)important static/global variables
    //I created this class to make things a bit more organized
    public class Var : MonoBehaviour
    {
        //Axis1: Horizontal
        //Axis2: Vertical
        //Axis3: Left&Right Trigger->-1=Left, +1=Right
        //Axis4: Right Stick Horizontal
        //Axis5: Right Stick Vertical
        //axis 11: left controller grip
        //axis 12: right controller grip

        public static List<Actions> actionList = new List<Actions>();
        public static Slot slot1;

        public static int activeWindowCount = 0;
        public static int debugInt2 = 0;
        public static bool isFirst = true;
        public static Camera VRCamera;
        public static bool charControllerCalibrated=false;
        public static bool isCalibrated=false;
        public static bool uiActive = true;
        public static int windowHeight = 1080;
        public static int windowWidth = 1920;
        public static double heightOffset;
        public static Vector3 sheathOffset;

        public static bool leftHanded;
        

        public static GameObject sphereObject;
        //Default Bindings
        public static KeyCode gripButton = KeyCode.JoystickButton5;
        //public static KeyCode gripButton = KeyCode.JoystickButton5;
        public static KeyCode indexButton = KeyCode.Joystick2Button15;
        public static KeyCode rStickButton = KeyCode.JoystickButton9;
        public static KeyCode acceptButton = KeyCode.JoystickButton1;
        public static KeyCode jumpButton = KeyCode.JoystickButton9;
        public static KeyCode cancelButton = KeyCode.JoystickButton0;
        public static KeyCode left1Button = KeyCode.JoystickButton2;
        public static KeyCode left2Button = KeyCode.JoystickButton3;
        public static KeyCode lStickButton = KeyCode.JoystickButton8;
        public static KeyCode lGripButton = KeyCode.JoystickButton4;

        public static bool isNotOculus=false;

        public static string lThumbStickHorizontal = "Axis1";
        public static string lThumbStickVertical = "Axis2";
        public static string triggers = "Axis3";
        public static string rThumbStickHorizontal = "Axis4";
        public static string rThumbStickVertical = "Axis5";
        public static string placeholder=null;
        public static bool fStartMenu = false;

        public static Camera mainCamera;
        public static Camera uiCamera;
        public static GameObject rightHand;
        public static GameObject leftHand;
        public static Camera handCam;
        public static GameObject body;
        public static WeaponManager weaponManager;
        public static ClimbingMotor climbingMotor;
        public static GameObject VRParent;
        public static GameObject debugSphere;

        public static Dictionary<WeaponTypes, HandObject> handObjects = new Dictionary<WeaponTypes, HandObject>();
        public static Dictionary<string, HandObject> handObjectsByName = new Dictionary<string, HandObject>();
        public static GameObject weaponObject;
        public static string currentWeaponName;

        public static GameObject sheathObject;

        public static int connectedJoysticks;


        public static GameObject keyboard;

        public static float active;

        public static bool started;

        public static int debugInt;
        public static GameObject fSpawnMenu;
        public static int controllerAmount;
        public static GameObject cMenu0;
        public static GameObject mMenu;
        public static GameObject cMenu1;
        public static GameObject cMenu2;
        public static GameObject cMenu3;
        public static GameObject fSpawnDoneButton;
        public static Text turnOptionsText;
        public static Text handOptionsText;
        public static int calibrationInt;
        public static bool smoothTurn;
        public static bool noTurn;
        public static bool toggleRun=false;

        public static bool rTriggerDone;
        public static bool lTriggerDone;
        public static bool snapDone;
        public static float lastSnapTime;
        public static float snapCooldown = 0.45f;
        public static float crouchCooldown = 0.45f;
        public static float lastCrouchTime;

        public static GameObject playerGameObject;
        public static CharacterController characterController=null;

        public static volatile bool skyboxToggle = true;
        //Load weapon and other models from the Asset bundles
        public static void InitModels()
        {
            HandObjectLoadList.LoadAllHandObjects(handObjects, handObjectsByName);

            characterController = GameObject.Find("PlayerAdvanced").GetComponent<CharacterController>();

            InitKeyboard();
        }

        //Loads the keyboard from the AssetBundle, sets actions, creates new keys. 
        public static void InitKeyboard()
        {
            string assetBundlePath = Path.Combine(Paths.PluginPath, "AssetBundles/keyboard");


            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);

            keyboard = Instantiate(assetBundle.LoadAsset<GameObject>("kbd"));
            GameObject backspace = keyboard.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(11).gameObject;
            backspace.SetActive(true);

            Button[]buttons=keyboard.GetComponentsInChildren<Button>();

            foreach (Button button in buttons) 
            { 
                BoxCollider boxcollider= button.gameObject.AddComponent<BoxCollider>();
                Vector2 buttonSize = button.gameObject.GetComponent<RectTransform>().sizeDelta;
                boxcollider.size= new Vector3(buttonSize.x, buttonSize.y, 0.1f);
            }

            keyboard.transform.GetChild(0).gameObject.transform.rotation = Quaternion.identity;

            backspace.name = "Back";

            GameObject enterButton = Instantiate(backspace);

            RectTransform backspaceRect = backspace.GetComponent<RectTransform>();
            RectTransform enterRect = enterButton.GetComponent<RectTransform>();

            Vector3 newPosition = backspaceRect.localPosition;
            newPosition.y -= (backspaceRect.sizeDelta.y + 10);
            enterRect.localPosition = newPosition;
            enterButton.name = "Enter";
            enterButton.GetComponentInChildren<Text>().text = ">";

            enterButton.SetActive(true);

            enterButton.transform.SetParent(backspace.transform.parent, false);

            keyboard.AddComponent<KeyboardController>();


            //The numbers get placed out of order on the keyboard... why?!?!
            Transform keysParent = keyboard.transform.GetChild(0).GetChild(0);
            int numKeys = 10;
            for (int i = 0; i < numKeys; i++)
            {
                
                Plugin.LoggerInstance.LogInfo("started Button " + i.ToString());
                GameObject clonedKey = Instantiate(keysParent.GetChild(i).gameObject);
                //clonedKey.transform.SetSiblingIndex(i);
                RectTransform originalKeyRect = keysParent.GetChild(i).GetComponent<RectTransform>();
                RectTransform clonedKeyRect = clonedKey.GetComponent<RectTransform>();

                Vector3 newPositio = originalKeyRect.localPosition;
                newPositio.y += (originalKeyRect.sizeDelta.y + 10);

                clonedKeyRect.localPosition = newPositio;
                
                clonedKeyRect.ForceUpdateRectTransforms();
                string number = (i + 1).ToString();
                if (i == 9) number = "0";
                clonedKey.name = "D" + number;
                clonedKey.GetComponentInChildren<Text>().text = number;
                //Plugin.LoggerInstance.LogInfo($"Before Parenting {clonedKey.name}: {clonedKeyRect.anchoredPosition}");
                clonedKey.transform.SetParent(keysParent, false);
                //Plugin.LoggerInstance.LogInfo($"After Parenting {clonedKey.name}: {clonedKeyRect.anchoredPosition}");
                //clonedKey.transform.SetSiblingIndex(i);

                clonedKey.SetActive(true);
                //LayoutRebuilder.ForceRebuildLayoutImmediate(keysParent.GetComponent<RectTransform>());
                Plugin.LoggerInstance.LogInfo("finished Button " + i.ToString());
            }
            //for (int i = 0; i < keysParent.childCount; i++)
            //{
            //    Plugin.LoggerInstance.LogInfo($"{i}: {keysParent.GetChild(i).name}");
            //}

            //GameObject vrui = GameObject.Find("VRUI");

            assetBundle.Unload(false);
            keyboard.SetActive(false);

        }
        //Initializes player height, bindings, sheath position, initial spawn menu and headset refresh rate 
        public static void Initialize()
        {
            started = false;
            Debug.Log("Reading Controller Settings");
            string filePath = Path.Combine(Paths.PluginPath, "Settings.txt");
            

            try //to read the Settings.txt file
            {
                string fileContent = FileReader.ReadFromFile(filePath);
                string[] lines = fileContent.Split('\n');
                Debug.Log(lines[2].Trim());
                //using only a bool makes the settings file too hard to understand for most people
                //bool.TryParse(lines[6], out leftHanded);
                if (lines[6].Trim() == "left") { Var.leftHanded = true; }

                //Set the bindings to the default Oculus Touch bindings
                //This is not necessary. The default values are already set up for the Touch Controllers
                //only if the player ist left handed, change the grip buttons
                if (lines[2].Trim() == "Oculus/Meta")
                {
                    if (Var.leftHanded)
                    {
                        gripButton = KeyCode.JoystickButton4;

                    }
                    else
                    {
                        gripButton = KeyCode.JoystickButton5;
                    }
                    //    gripButton = KeyCode.Joystick2Button5;
                    //    indexButton = KeyCode.Joystick2Button15;
                    //    acceptButton = KeyCode.JoystickButton1;
                    //    jumpButton = KeyCode.JoystickButton9;
                    //    cancelButton = KeyCode.JoystickButton0;
                    //    left1Button = KeyCode.JoystickButton2;
                    //    left2Button = KeyCode.JoystickButton3;
                    //    Plugin.LoggerInstance.LogInfo("Set bindings for Oculus Touch.");

                }
                //Set the bindings to the default HTC Vive Wand bindings
                if (lines[2].Trim() == "HTC Vive Wands")
                {
                    left2Button = KeyCode.JoystickButton4;
                    left1Button = KeyCode.JoystickButton2;
                    gripButton = KeyCode.JoystickButton5;
                    indexButton = KeyCode.JoystickButton15;
                    acceptButton = KeyCode.JoystickButton0;

                    jumpButton = KeyCode.JoystickButton9;
                    rStickButton = KeyCode.JoystickButton9;
                    lStickButton = KeyCode.JoystickButton8;
                    cancelButton = KeyCode.JoystickButton0;
                    lGripButton = KeyCode.Quote;
                    //lGripButton = KeyCode.JoystickButton4;

                }
                //FOr everything else, we try to let Unity figure it out. Doesn't work very well though and manual controller profiles are necessary for a playable experience
                else if (lines[2].Trim() == "Other")
                {
                    isNotOculus = true;

                    gripButton = KeyCode.Quote;
                    indexButton = KeyCode.Quote;
                    acceptButton = KeyCode.Quote;
                    cancelButton = KeyCode.Quote;
                    jumpButton = KeyCode.Quote;
                    rStickButton = KeyCode.Quote;
                    left1Button = KeyCode.Quote;
                    left2Button = KeyCode.Quote;
                    lStickButton = KeyCode.Quote;
                    lGripButton = KeyCode.Quote;

                    Plugin.LoggerInstance.LogInfo(gripButton.ToString());


                }
                //set the refresh rate of the game to the refresh rate specified in settings.txt
                float targetTimeStep;
                try 
                {
                    fileContent = FileReader.ReadFromFile(filePath);
                    //lines = fileContent.Split('\n');
                    Debug.Log("Line1:" + lines[0].Trim());
                    Debug.Log("Line2:" + lines[1].Trim());
                    Var.heightOffset = float.Parse(lines[0].Trim(),CultureInfo.InvariantCulture);
                    Plugin.LoggerInstance.LogInfo(Var.heightOffset);
                    targetTimeStep = 1f / float.Parse(lines[1].Trim());
                    Plugin.LoggerInstance.LogInfo(targetTimeStep);

                }
                catch (Exception e)//if it doesn't work, set it to an emergency default value
                {
                    Plugin.LoggerInstance.LogError("Made a fucky wucky while reading the file, oopsie! Error: " + e);
                    targetTimeStep = 1f / 90f;
                }
                try
                {
                    //deprecated. Should remove all occurences of ReadAxis when I have time.
                    ReadAxis();
                }
                catch
                {
                    //deprecated. Was used for the old Unity Input system, however I have since switched to the Unity XR input system for the Joysticks/Touchpads
                    Plugin.LoggerInstance.LogError("Failed to initialize controller axis. Returning to defaults");
                    lThumbStickHorizontal = "Axis1";
                    lThumbStickVertical = "Axis2";
                    triggers = "Axis3";
                    rThumbStickHorizontal = "Axis4";
                    rThumbStickVertical = "Axis5";
                    placeholder = null;

                }
                Time.fixedDeltaTime = targetTimeStep;
                Plugin.LoggerInstance.LogInfo(Time.fixedDeltaTime);

                string rawLine3 = lines[3].Trim();
                Plugin.LoggerInstance.LogInfo(rawLine3);
                string[] sheathVector = rawLine3.Split(',');
                float x = float.Parse(sheathVector[0], CultureInfo.InvariantCulture);
                float y = float.Parse(sheathVector[1], CultureInfo.InvariantCulture);
                float z = float.Parse(sheathVector[2], CultureInfo.InvariantCulture);
                Plugin.LoggerInstance.LogInfo(x);
                Var.sheathOffset=new Vector3(x,y,z);
                bool.TryParse(lines[5], out fStartMenu);
                //bool.TryParse(lines[6],out leftHanded);
                Plugin.LoggerInstance.LogInfo("Offsett: "+Var.sheathOffset.ToString());
                if (lines[7].Trim() == "smooth") { Var.smoothTurn = true;}
                Plugin.LoggerInstance.LogInfo("Smooth turn: "+Var.smoothTurn);
                if (lines[7].Trim() == "none") { Var.noTurn = true; }
                bool.TryParse(lines[8],out toggleRun);


            }
            catch (Exception e)
            {
                Plugin.LoggerInstance.LogError("Error: "+e.Message);
                return;
            }
        }
        //THis saves the players height in Settings.txt. Gets called after exiting calibration mode
        public static void SaveHeight()
        {
            string filePath = Path.Combine(Paths.PluginPath, "Settings.txt");
            string[] lines = File.ReadAllLines(filePath);

            lines[0] = string.Format(CultureInfo.InvariantCulture,"{0}", Var.heightOffset);//Var.heightOffset.ToString();
            lines[3] = string.Format(CultureInfo.InvariantCulture, "{0},{1},{2}", Var.sphereObject.transform.localPosition.x, Var.sphereObject.transform.localPosition.y, Var.sphereObject.transform.localPosition.z);//Var.sphereObject.transform.localPosition.ToString();//Var.sheathOffset.ToString();
            lines[4] = connectedJoysticks.ToString();

            File.WriteAllLines(filePath, lines);
        }
        //deprecated. Ignore.
        public static void SaveAxis()
        {
            string filePath = Path.Combine(Paths.PluginPath, "Axis.txt");
            string[] lines = File.ReadAllLines(filePath);

            lines[0] = string.Format(CultureInfo.InvariantCulture, "Joystick{0}Axis1", Var.lThumbStickHorizontal);
            lines[1] = string.Format(CultureInfo.InvariantCulture, "Joystick{0}Axis2", Var.lThumbStickVertical);
            lines[2] = string.Format(CultureInfo.InvariantCulture, "Axis{0}", Var.rThumbStickHorizontal);
            lines[3] = string.Format(CultureInfo.InvariantCulture, "Axis{0}", Var.rThumbStickVertical);
            lines[4] = string.Format(CultureInfo.InvariantCulture, "Axis{0}", Var.triggers);
            File.WriteAllLines(filePath, lines);

        }
        //deprecated. Ignore.
        static void ReadAxis()
        {
            //string filePath = Path.Combine(Paths.PluginPath, "Axis.txt");
            //string[] lines = File.ReadAllLines(filePath);

            //Var.lThumbStickHorizontal = lines[0];
            //Var.lThumbStickVertical = lines[1];
            //Var.rThumbStickHorizontal = lines[2];
            //Var.rThumbStickVertical = lines[3];
            //Var.triggers = lines[4];

        }
        //Gets all connected Gamepads. Deprecated.
        static int GetConnectedGamepadsCount()
        {
            string[] joystickNames = Input.GetJoystickNames();
            Plugin.LoggerInstance.LogInfo("joystickNames.Length");
            int count = 0;

            foreach (string name in joystickNames)
            {
                Plugin.LoggerInstance.LogInfo(name);
                if (!string.IsNullOrEmpty(name))
                {
                    count++;
                }
            }

            return count;
        }
        //static void InternalGetJoystick()
        //{
        //    connectedJoysticks = GetConnectedGamepadsCount();
        //    Plugin.LoggerInstance.LogInfo("Connected Joysticks: " + connectedJoysticks.ToString() + " Previous: " + controllerAmount);
        //    if (connectedJoysticks != controllerAmount)
        //    {
        //        Var.fStartMenu = true;
        //    }
        //}
        //public static IEnumerator WaitForInitialization()
        //{
        //    yield return new WaitForEndOfFrame(); // Or WaitForSeconds(0.5f) if needed
        //    string[] joystickNames = Input.GetJoystickNames();
        //    Debug.Log("Connected gamepads: " + joystickNames.Length);
        //}

    }



}