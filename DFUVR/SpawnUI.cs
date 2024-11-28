using BepInEx;
using BepInEx.Logging;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.UI;
using uWindowCapture;
using HarmonyLib;
using System.Reflection;
using HarmonyLib.Tools;
using System;
namespace DFUVR
{
    public class UI : MonoBehaviour
    {

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public static IEnumerator Spawn()
        {
            
            string assetBundlePath = Path.Combine(Paths.PluginPath, "AssetBundles", "assetbundle0");


            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);

            Mesh mesh = assetBundle.LoadAsset<Mesh>("uWC_Board");
            Material wMat = assetBundle.LoadAsset<Material>("uWC_Unlit");


            assetBundle.Unload(false);
            GameObject vrui = new GameObject("VRUI");
            MeshFilter mFilter = vrui.AddComponent<MeshFilter>();
            mFilter.mesh = mesh;
            MeshRenderer meshRenderer = vrui.AddComponent<MeshRenderer>();
            meshRenderer.material = wMat;
            MeshCollider collider = vrui.AddComponent<MeshCollider>();
            collider.convex = true;
            if (!Var.fStartMenu)
            {
                if (Var.isFirst)
                {
                    GameObject parentObject = new GameObject("CamParent");

                    GameObject cam = Camera.main.gameObject;
                    GameObject tracker = new GameObject("Tracker");
                    TrackedPoseDriver trackedPoseDriver = tracker.AddComponent<TrackedPoseDriver>();
                    trackedPoseDriver.SetPoseSource(TrackedPoseDriver.DeviceType.GenericXRDevice, TrackedPoseDriver.TrackedPose.Head);
                    //parentObject.transform.position = cam.transform.position-new Vector3(0,-1.2f,0);
                    //parentObject.transform.position = new Vector3(-33-cam.transform.localPosition.x,-10, -54.41f-cam.transform.localPosition.z);
                    //cam.transform.Rotate(0, -90, 0);
                    cam.transform.parent = parentObject.transform;
                    //Debug.Log(tracker.transform.position.x);
                    //parentObject.transform.position = new Vector3(-33 - cam.transform.localPosition.x, -10, -54.41f - cam.transform.localPosition.z);
                    yield return new WaitForSecondsRealtime(1);
                    //Plugin.LoggerInstance.LogInfo(tracker.transform.position);
                    parentObject.transform.position = new Vector3(-cam.transform.localPosition.x + tracker.transform.localPosition.x, -10, -cam.transform.localPosition.z + tracker.transform.localPosition.z);
                    parentObject.transform.RotateAround(new Vector3(cam.transform.position.x, 0, cam.transform.position.z), Vector3.up, -90f);
                    //parentObject.transform.position = new Vector3(-cam.);
                    //cam.transform.position = Vector3.zero;



                    vrui.transform.position = Camera.main.transform.position + new Vector3(0, 0, 2f);



                    UwcWindowTexture wTexture = vrui.AddComponent<UwcWindowTexture>();
                    vrui.AddComponent<UwcWindowTextureChildrenManager>();
                    wTexture.partialWindowTitle = "Daggerfall";
                    wTexture.childWindowPrefab = vrui;

                    try
                    {
                        GameObject menuWorld = GameObject.Find("DaggerfallBlock [CUSTAA06.RMB]");
                        menuWorld.transform.localPosition = new Vector3(55, 0, -40);
                        menuWorld.transform.localRotation = Quaternion.Euler(0, 270, 0);
                    }
                    catch
                    {
                        Plugin.LoggerInstance.LogError("Background world not found");
                    }

                }
                else
                {
                    GameObject cameraObject = new GameObject("VRCamera");
                    GameObject vrparent = new GameObject("VRParent");
                    vrparent.transform.parent = GameObject.Find("SmoothFollower").transform;

                    //cameraObject.transform.parent = GameObject.Find("SmoothFollower").transform;
                    cameraObject.transform.parent = vrparent.transform;

                    Camera.main.stereoTargetEye = StereoTargetEyeMask.None;
                    Camera.main.gameObject.GetComponent<AudioListener>().enabled = false;
                    Var.VRCamera = cameraObject.AddComponent<Camera>();
                    Var.VRCamera.stereoTargetEye = StereoTargetEyeMask.Both;
                    Var.VRCamera.nearClipPlane = 0.01f;
                    Var.VRCamera.gameObject.AddComponent<AudioListener>().enabled = true;

                    int automapLayer = LayerMask.NameToLayer("Automap");
                    Var.VRCamera.cullingMask = ~(1 << automapLayer);

                    Var.VRCamera.farClipPlane = Camera.main.farClipPlane;

                    vrparent.transform.localPosition = new Vector3(0, (float)Var.heightOffset, 0);


                    //GameObject laserPointer = GameObject.Find("LaserPointer");
                    //laserPointer.transform.parent = vrparent.transform;

                    //vrui.transform.position = Var.VRCamera.transform.position + new Vector3(0, 1.5f, 2f);

                    //vrui.transform.parent = GameObject.Find("SmoothFollower").transform;
                    vrui.transform.parent = vrparent.transform;
                    vrui.transform.localScale = new Vector3((float)Var.windowWidth / 1000f, (float)Var.windowHeight / 1000f, 1);

                    //Var.keyboard.
                    Hands.Spawn();

                    Var.keyboard.transform.SetParent(vrui.transform);
                    Var.keyboard.transform.localPosition = new Vector3(0, -0.7f, 0);
                    Var.keyboard.transform.localRotation = Quaternion.Euler(45, 0, 0);

                    Var.keyboard.SetActive(true);




                }
            }
            else
            {
                Plugin.LoggerInstance.LogInfo("FirstSpawn.");
                string fMenuassetBundlePath = Path.Combine(Paths.PluginPath, "AssetBundles/firstspawnmenu");

                Plugin.LoggerInstance.LogInfo("1");
                AssetBundle fMenuassetBundle = AssetBundle.LoadFromFile(fMenuassetBundlePath);


                Var.fSpawnMenu = Instantiate(fMenuassetBundle.LoadAsset<GameObject>("FirstSpawnCanvas"));

                GameObject parentObject = new GameObject("CamParent");

                GameObject cam = Camera.main.gameObject;
                GameObject tracker = new GameObject("Tracker");
                TrackedPoseDriver trackedPoseDriver = tracker.AddComponent<TrackedPoseDriver>();
                trackedPoseDriver.SetPoseSource(TrackedPoseDriver.DeviceType.GenericXRDevice, TrackedPoseDriver.TrackedPose.Head);
                //parentObject.transform.position = cam.transform.position-new Vector3(0,-1.2f,0);
                //parentObject.transform.position = new Vector3(-33-cam.transform.localPosition.x,-10, -54.41f-cam.transform.localPosition.z);
                //cam.transform.Rotate(0, -90, 0);
                cam.transform.parent = parentObject.transform;
                //Debug.Log(tracker.transform.position.x);
                //parentObject.transform.position = new Vector3(-33 - cam.transform.localPosition.x, -10, -54.41f - cam.transform.localPosition.z);
                yield return new WaitForSecondsRealtime(1);
                //Plugin.LoggerInstance.LogInfo(tracker.transform.position);
                parentObject.transform.position = new Vector3(-cam.transform.localPosition.x + tracker.transform.localPosition.x, -10, -cam.transform.localPosition.z + tracker.transform.localPosition.z);
                parentObject.transform.RotateAround(new Vector3(cam.transform.position.x, 0, cam.transform.position.z), Vector3.up, -90f);

                //Var.fSpawnMenu.transform.position = Var.VRCamera.transform.position;
                //Var.VRCamera.transform.position = new Vector3(0, 0, 0);
                Var.fSpawnMenu.transform.position= Camera.main.transform.position + new Vector3(0, 0, 2f); 
                Plugin.LoggerInstance.LogInfo("2");


                UnityEngine.UI.Button[] buttons = Var.fSpawnMenu.GetComponentsInChildren<UnityEngine.UI.Button>();

                foreach (UnityEngine.UI.Button button in buttons)
                {
                    BoxCollider boxcollider = button.gameObject.AddComponent<BoxCollider>();
                    Vector2 buttonSize = button.gameObject.GetComponent<RectTransform>().sizeDelta;
                    boxcollider.size = new Vector3(buttonSize.x, buttonSize.y, 0.1f);
                }
                Plugin.LoggerInstance.LogInfo("3");
                Var.cMenu0 = GameObject.Find("MainMenu"); //shitty name.It's not the main menu. It's the main menu of the setup menu. will fix later

                Var.cMenu1 = GameObject.Find("LController");
                Var.cMenu2 = GameObject.Find("RController");
                Var.cMenu3 = GameObject.Find("TController");

                Var.mMenu = GameObject.Find("Monitor");
                Plugin.LoggerInstance.LogInfo("4");
                GameObject.Find("LCube").AddComponent<CubeController>();
                GameObject.Find("RCube").AddComponent<CubeController>();
                GameObject.Find("TCube").AddComponent<CubeController>();
                Plugin.LoggerInstance.LogInfo("5");
                Toggle CToggle = GameObject.Find("CSetupToggle").GetComponent<Toggle>();
                CToggle.interactable = false;

                if(Var.connectedJoysticks == Var.controllerAmount)
                {
                    CToggle.isOn = true;
                }
                else
                {
                    CToggle.isOn= false;
                }
                Plugin.LoggerInstance.LogInfo("6");
                //Main Menu "finished Setup" button
                GameObject.Find("DoneButton").GetComponent<UnityEngine.UI.Button>().interactable = true;
                Plugin.LoggerInstance.LogInfo("7");
                
                GameObject.Find("DoneButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ButtonHandler.Done);
                 
                Plugin.LoggerInstance.LogInfo("7.5");
                //Next Step buttons
                
                    GameObject.Find("CSetupButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ButtonHandler.MenuTransition(Var.cMenu0, Var.cMenu1, false, false, ref Var.placeholder));
                Plugin.LoggerInstance.LogInfo("7.6");
                GameObject.Find("MSetupButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ButtonHandler.MenuTransition(Var.cMenu0, Var.mMenu, false, false, ref Var.placeholder));
                Plugin.LoggerInstance.LogInfo("7.7");
                GameObject.Find("ClDoneButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ButtonHandler.MenuTransition(Var.cMenu1, Var.cMenu2, false, false, ref Var.lThumbStickHorizontal, ref Var.lThumbStickVertical));
                Plugin.LoggerInstance.LogInfo("7.8");
                GameObject.Find("CrDoneButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ButtonHandler.MenuTransition(Var.cMenu2, Var.cMenu3, false, false, ref Var.rThumbStickHorizontal, ref Var.rThumbStickVertical));
                Plugin.LoggerInstance.LogInfo("7.9");
                GameObject.Find("TDoneButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ButtonHandler.MenuTransition(Var.cMenu3, Var.cMenu0, true, false, ref Var.triggers));
                Plugin.LoggerInstance.LogInfo("8.0");
                GameObject.Find("MDoneButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ButtonHandler.MenuTransition(Var.mMenu, Var.cMenu0, false, true, ref Var.placeholder));
                Plugin.LoggerInstance.LogInfo("8");
                GameObject.Find("MQuitButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Application.Quit);
                Plugin.LoggerInstance.LogInfo("9");
                    //Setting up next and previous axis selection
                    //left
                GameObject.Find("NlAxisButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ButtonHandler.nextInt);
                Plugin.LoggerInstance.LogInfo("9.1");
                GameObject.Find("PlAxisButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ButtonHandler.prevInt);
                Plugin.LoggerInstance.LogInfo("9.2");
                //right
                GameObject.Find("NrAxisButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ButtonHandler.nextInt);
                GameObject.Find("PrAxisButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ButtonHandler.prevInt);
                
                Plugin.LoggerInstance.LogInfo("10");
                //trigger
                try
                {
                    GameObject.Find("NtAxisButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ButtonHandler.nextInt);
                    GameObject.Find("PtAxisButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ButtonHandler.nextInt);
                }
                catch {
                    Plugin.LoggerInstance.LogError("You forgot to include the newer version of the assetbundle because you went to get some food while it was exporting.");
                
                }
                Plugin.LoggerInstance.LogInfo("11");
                Var.cMenu1.SetActive(false);
                Plugin.LoggerInstance.LogInfo("12");
                Var.cMenu2.SetActive(false);
                Plugin.LoggerInstance.LogInfo("13");
                try
                {
                    Var.cMenu3.SetActive(false);
                }
                catch(Exception e){
                    Plugin.LoggerInstance.LogError(e.ToString()+Var.cMenu3.name);
                
                }
                Plugin.LoggerInstance.LogInfo("14");
                Var.mMenu.SetActive(false);
                Plugin.LoggerInstance.LogInfo("15");
                fMenuassetBundle.Unload(false);
                Plugin.LoggerInstance.LogInfo("16");

            }





            CreateMenuController();
            if (!Var.fStartMenu)
            {
                Var.isFirst = false;
            }



        }
        public static IEnumerator Calibrate()
        {

            string assetBundlePath = Path.Combine(Paths.PluginPath, "AssetBundles", "assetbundle1");


            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);

            Shader uIShader = assetBundle.LoadAsset<Shader>("UniShade");



            assetBundle.Unload(false);
            //Debug.Log("Entered Coroutine");
            GameObject vrui = GameObject.Find("VRUI");
            //UwcWindowTexture wTexture = vrui.GetComponent<UwcWindowTexture>();
            UserInterfaceRenderTarget newTarget = vrui.AddComponent<UserInterfaceRenderTarget>();
            //wTexture.type = WindowTextureType.Desktop;
            
            //wTexture.desktopIndex = 1;
            yield return new WaitForSecondsRealtime(1);

            //wTexture.desktopIndex = 0;
            //yield return new WaitForSecondsRealtime(0.1f);
            //wTexture.type = WindowTextureType.Window;
            DaggerfallUI daggerfallUI = GameObject.Find("DaggerfallUI").GetComponent<DaggerfallUI>();
            FieldInfo directionField = AccessTools.Field(typeof(DaggerfallUI), "customRenderTarget");//customRenderTarget is private so I have to get it using AccessTools.Field
            if (directionField != null)
            {
                directionField.SetValue(daggerfallUI, newTarget);
            }
            //Plugin.LoggerInstance.LogInfo(directionField.GetValue(daggerfallUI));
            //daggerfallUI.customRenderTarget = newTarget;
            sTx stx = daggerfallUI.gameObject.GetComponent<sTx>();

            MeshRenderer meshRenderer = vrui.GetComponent<MeshRenderer>();
            Material cMat = new Material(uIShader);
            cMat.mainTexture = stx.sTxx;
            //cMat.mainTexture = newTarget.TargetTexture;
            //stx.sTxx=newTarget.TargetTexture;
            //Shader shader = cMat.shader;

            meshRenderer.material = cMat;
            //vrui.SetActive(true);
            Var.isCalibrated = true;



            //Debug.Log("I hope this worked");

        }
        public static void CreateMenuController()
        {
            try
            {
                GameObject vrparent = GameObject.Find("VRParent");


                GameObject emptyObject = new GameObject("LaserPointer");
                if (!Var.isFirst && !Var.fStartMenu)
                {
                    emptyObject.transform.parent = vrparent.transform;

                }
                //emptyObject.transform.parent=GameObject.Find("CamParent").transform;
                LineRenderer lineRenderer = emptyObject.AddComponent<LineRenderer>();
                GraphicRaycaster raycaster = emptyObject.AddComponent<GraphicRaycaster>();
                lineRenderer.useWorldSpace = true;
                lineRenderer.startWidth = 0.01f;
                lineRenderer.endWidth = 0.01f;
                lineRenderer.positionCount = 2;

                //removed because I don't have the willpower to mess with layers right now
                //Material newMaterial = new Material(Shader.Find("Sprites/Default"));
                //newMaterial.SetFloat("_Mode", 1);

                //newMaterial.color = Color.white; 


                //lineRenderer.material = newMaterial;




                TrackedPoseDriver trackedPoseDriver = emptyObject.AddComponent<TrackedPoseDriver>();
                trackedPoseDriver.SetPoseSource(TrackedPoseDriver.DeviceType.GenericXRController, TrackedPoseDriver.TrackedPose.RightPose);
                SPC spcScript = emptyObject.AddComponent<SPC>();
                spcScript.Initialize(lineRenderer, raycaster, trackedPoseDriver);
            }
            catch (Exception e)
            {
                Plugin.LoggerInstance.LogError(e.ToString());
            }
        }
        public static void HideUI()
        {
            GameObject.Find("VRUI").SetActive(false);
        }
        public static void ShowUI() 
        {
            GameObject.Find("VRUI").SetActive(true);
        }
    }
}


