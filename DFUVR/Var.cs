﻿using UnityEngine;
using System.Collections;

using System.IO;
using BepInEx;
using System;
using UnityEngine.UI;
using DaggerfallWorkshop.AudioSynthesis.Synthesis;
using DaggerfallWorkshop.Game;
using System.Globalization;

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
        public static int activeWindowCount = 0;

        public static bool isFirst = true;
        public static Camera VRCamera;
        public static bool charControllerCalibrated=false;
        public static bool isCalibrated=false;
        public static bool uiActive = true;
        public static int windowHeight = 1080;
        public static int windowWidth = 1920;
        public static double heightOffset;
        public static Vector3 sheathOffset;

        public static GameObject sphereObject;
        //Default Bindings
        public static KeyCode gripButton = KeyCode.Joystick2Button5;
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
        

        public static Camera mainCamera;
        public static Camera uiCamera;
        public static GameObject rightHand;
        public static GameObject leftHand;
        public static Camera handCam;
        public static GameObject body;
        public static WeaponManager weaponManager;
        public static GameObject VRParent;
        public static GameObject debugSphere;

        public static GameObject weaponObject;
        public static GameObject sword;
        public static GameObject dagger;
        public static GameObject battleaxe;
        public static GameObject elseA;
        public static GameObject sheathObject;
        public static GameObject mace;
        public static GameObject flail;
        public static GameObject hammer;
        public static GameObject staff;
        public static GameObject bow;



        public static GameObject keyboard;

        public static float active;

        public static bool started;

        public static int debugInt;

        public static bool rTriggerDone;
        public static bool lTriggerDone;
        public static bool snapDone;
        public static float lastSnapTime;
        public static float snapCooldown = 0.45f;

        public static GameObject playerGameObject;
        public static CharacterController characterController=null;
        
        public static void InitModels()
        {
            string assetBundlePath = Path.Combine(Paths.PluginPath, "AssetBundles/weapons");


            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);

            sword = Instantiate(assetBundle.LoadAsset<GameObject>("Sword"));
            sword.transform.position = Vector3.zero;
            sword.transform.rotation = Quaternion.identity;
            sword.AddComponent<WeaponCollision>();
            sword.GetComponent<MeshCollider>().isTrigger=true;
            //sword.AddComponent<Rigidbody>().useGravity = false;


            dagger = Instantiate(assetBundle.LoadAsset<GameObject>("Steel_Dagger_512"));
            
            //dagger.transform.rotation = Quaternion.identity;
            
            dagger.AddComponent<WeaponCollision>();
            dagger.GetComponent<MeshCollider>().isTrigger = true;

            //dagger.AddComponent<Rigidbody>().useGravity=false;


            battleaxe = Instantiate(assetBundle.LoadAsset<GameObject>("MM_Axe_01_01_lod2"));
            
            battleaxe.AddComponent<WeaponCollision>();
            battleaxe.GetComponent<MeshCollider>().isTrigger = true;
            //battleaxe.AddComponent<Rigidbody>().useGravity = false;
            elseA = Instantiate(assetBundle.LoadAsset<GameObject>("Sheath"));
            elseA.transform.position = Vector3.zero;
            elseA.transform.rotation = Quaternion.identity;

            mace = Instantiate(assetBundle.LoadAsset<GameObject>("mace"));
            mace.AddComponent<WeaponCollision>();
            mace.GetComponent<MeshCollider>().isTrigger = true;
            //flail and mace are the same model for now
            flail = Instantiate(assetBundle.LoadAsset<GameObject>("mace"));
            flail.AddComponent<WeaponCollision>();
            flail.GetComponent<MeshCollider>().isTrigger=true;

            hammer = Instantiate(assetBundle.LoadAsset<GameObject>("Warhammer_1"));
            hammer.AddComponent<WeaponCollision>();
            hammer.GetComponent<BoxCollider>().isTrigger=true;

            staff = Instantiate(assetBundle.LoadAsset<GameObject>("Staff_1"));
            staff.AddComponent<WeaponCollision>();
            staff.GetComponent<BoxCollider>().isTrigger = true;

            bow = Instantiate(assetBundle.LoadAsset<GameObject>("Crossbow"));
            bow.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
            bow.AddComponent<SphereCollider>().isTrigger=true;


            assetBundle.Unload(false);
            //Plugin.LoggerInstance.LogWarning("Exited Method_Init");
            Var.characterController=GameObject.Find("PlayerAdvanced").GetComponent<CharacterController>();


            sword.SetActive(false);
            dagger.SetActive(false);
            battleaxe.SetActive(false);
            elseA.SetActive(false);
            mace.SetActive(false);
            flail.SetActive(false);
            hammer.SetActive(false);
            bow.SetActive(false);
            staff.SetActive(false);

            InitKeyboard();

        }

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



            Transform keysParent = keyboard.transform.GetChild(0).GetChild(0);
            int numKeys = 10;
            for (int i = 0; i < numKeys; i++)
            {
                GameObject clonedKey = Instantiate(keysParent.GetChild(i).gameObject);

                RectTransform originalKeyRect = keysParent.GetChild(i).GetComponent<RectTransform>();
                RectTransform clonedKeyRect = clonedKey.GetComponent<RectTransform>();

                Vector3 newPositio = originalKeyRect.localPosition;
                newPositio.y += (originalKeyRect.sizeDelta.y + 10);
                clonedKeyRect.localPosition = newPositio;

                string number = (i + 1).ToString();
                if (i == 9) number = "0";
                clonedKey.name = "D"+number;
                clonedKey.GetComponentInChildren<Text>().text = number;
                clonedKey.transform.SetParent(keysParent, false);
                clonedKey.SetActive(true);
            }
            //GameObject vrui = GameObject.Find("VRUI");

            assetBundle.Unload(false);
            keyboard.SetActive(false);

        }
        //Sets up the correct bindings
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
                //Set the bindings to the default Oculus Touch bindings
                if (lines[2].Trim() == "Oculus/Meta")
                {
                    gripButton = KeyCode.Joystick2Button5;
                    indexButton = KeyCode.Joystick2Button15;
                    acceptButton = KeyCode.JoystickButton1;
                    jumpButton = KeyCode.JoystickButton9;
                    cancelButton = KeyCode.JoystickButton0;
                    left1Button = KeyCode.JoystickButton2;
                    left2Button = KeyCode.JoystickButton3;
                    Plugin.LoggerInstance.LogInfo("Set bindings for Oculus Touch.");

                }
                //Set the bindings to the default HTC Vive Wand bindings
                else if (lines[2].Trim() == "Vive Wand")
                {
                    gripButton = KeyCode.Joystick2Button5;
                    indexButton = KeyCode.Joystick2Button15;
                    acceptButton = KeyCode.Joystick2Button15;
                    jumpButton = KeyCode.JoystickButton17;
                    cancelButton = KeyCode.Joystick2Button5;
                    Plugin.LoggerInstance.LogInfo("Set bindings for HTC Vive Wands or Pimax Sword Controllers");

                }
                else if (lines[2].Trim() == "Custom")
                {
                    string filePath2 = Path.Combine(Paths.PluginPath, "Bindings.txt");
                    try //to read the Bindings.txt file
                    {
                        string fileContent2 = FileReader.ReadFromFile(filePath2);
                        string[] lines2 = fileContent2.Split('\n');
                        gripButton = (KeyCode)Enum.Parse(typeof(KeyCode), lines2[0].Trim());
                        indexButton = (KeyCode)Enum.Parse(typeof(KeyCode), lines2[1].Trim());
                        acceptButton = (KeyCode)Enum.Parse(typeof(KeyCode), lines2[2].Trim());
                        cancelButton = (KeyCode)Enum.Parse(typeof(KeyCode), lines2[3].Trim());
                        jumpButton = (KeyCode)Enum.Parse(typeof(KeyCode), lines2[4].Trim());
                    }
                    catch (Exception e)//if it doesn't work, set it to emergency default values(height gets set somewhere else)
                    {
                        Plugin.LoggerInstance.LogError("Error: "+e.Message);
                        return;
                    }

                }

                //Screen.SetResolution(1920, 1080, true);
                float targetTimeStep;
                try //to read the Settings.txt file
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
                catch (Exception e)//if it doesn't work, set it to emergency default values(height gets set somewhere else)
                {
                    Plugin.LoggerInstance.LogError("Made a fucky wucky while reading the file, oopsie! Error: " + e);
                    targetTimeStep = 1f / 90f;
                }
                Time.fixedDeltaTime = targetTimeStep;
                Plugin.LoggerInstance.LogInfo(Time.fixedDeltaTime);

                string rawLine3 = lines[3].Trim();
                //rawLine3=rawLine3.Substring(1, rawLine3.Length - 2);
                Plugin.LoggerInstance.LogInfo(rawLine3);
                string[] sheathVector = rawLine3.Split(',');
                float x = float.Parse(sheathVector[0], CultureInfo.InvariantCulture);
                float y = float.Parse(sheathVector[1], CultureInfo.InvariantCulture);
                float z = float.Parse(sheathVector[2], CultureInfo.InvariantCulture);
                Plugin.LoggerInstance.LogInfo(x);
                Var.sheathOffset=new Vector3(x,y,z);
                Plugin.LoggerInstance.LogInfo("Offsett: "+Var.sheathOffset.ToString());


                //Set the keybindings to the custom bindings specifiec in Bindings.txt


            }
            catch (Exception e)//if it doesn't work, set it to emergency default values(height gets set somewhere else)
            {
                Plugin.LoggerInstance.LogError("Error: "+e.Message);
                return;
            }
        }
        
        public static void SaveHeight()
        {
            string filePath = Path.Combine(Paths.PluginPath, "Settings.txt");
            string[] lines = File.ReadAllLines(filePath);

            lines[0] = string.Format(CultureInfo.InvariantCulture,"{0}", Var.heightOffset);//Var.heightOffset.ToString();
            lines[3] = string.Format(CultureInfo.InvariantCulture, "{0},{1},{2}", Var.sphereObject.transform.localPosition.x, Var.sphereObject.transform.localPosition.y, Var.sphereObject.transform.localPosition.z);//Var.sphereObject.transform.localPosition.ToString();//Var.sheathOffset.ToString();
            File.WriteAllLines(filePath, lines);
        }

    }

}