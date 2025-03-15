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
                    string watchBundlePath = Path.Combine(Paths.PluginPath, "AssetBundles/watchandfonts");

                    AssetBundle watchBundle = AssetBundle.LoadFromFile(watchBundlePath);

                    GameObject watch = Instantiate(watchBundle.LoadAsset<GameObject>("Watch"));
                    watch.transform.position = new Vector3(1000, 1000, 1000);
                    //watch.AddComponent<WatchController>();

                    watchBundle.Unload(false);
                    Plugin.LoggerInstance.LogInfo("1");
                    AssetBundle fMenuassetBundle = AssetBundle.LoadFromFile(fMenuassetBundlePath);


                    Var.fSpawnMenu = Instantiate(fMenuassetBundle.LoadAsset<GameObject>("FirstSpawnCanvas_New"));

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
                    Var.fSpawnMenu.transform.position = Camera.main.transform.position + new Vector3(0, 0, 2f);



                    UnityEngine.UI.Button[] buttons = Var.fSpawnMenu.GetComponentsInChildren<UnityEngine.UI.Button>();

                    foreach (UnityEngine.UI.Button button in buttons)
                    {
                        BoxCollider boxcollider = button.gameObject.AddComponent<BoxCollider>();
                        Vector2 buttonSize = button.gameObject.GetComponent<RectTransform>().sizeDelta;
                        boxcollider.size = new Vector3(buttonSize.x, buttonSize.y, 0.1f);
                    }
                    UnityEngine.UI.Dropdown[] dropdowns = Var.fSpawnMenu.GetComponentsInChildren<UnityEngine.UI.Dropdown>();

                    foreach (UnityEngine.UI.Dropdown dropdown in dropdowns)
                    {
                        BoxCollider boxcollider = dropdown.gameObject.AddComponent<BoxCollider>();
                        Vector2 dropdownSize = dropdown.gameObject.GetComponent<RectTransform>().sizeDelta;
                        boxcollider.size = new Vector3(dropdownSize.x, dropdownSize.y, 0.1f);
                    }
                    Text[] texts = Var.fSpawnMenu.GetComponentsInChildren<Text>();
                    foreach (Text text in texts)
                    {
                        text.font=watch.GetComponent<TextMesh>().font;
                    }
                    Var.cMenu0 = GameObject.Find("MainMenu"); //shitty name.It's not the main menu. It's the main menu of the setup menu. will fix later

                    Var.cMenu1 = GameObject.Find("HSetup");

                    Var.mMenu = GameObject.Find("Monitor");


                    Toggle HToggle = GameObject.Find("HSetupToggle").GetComponent<Toggle>();
                    HToggle.interactable = false;
                    HToggle.isOn = false;
                    Toggle MToggle = GameObject.Find("MSetupToggle").GetComponent<Toggle>();
                    MToggle.interactable = false;
                    MToggle.isOn = false;
                    //Main Menu "finished Setup" button
                    //GameObject.Find("DoneButton").GetComponent<UnityEngine.UI.Button>().interactable = true;

                    Var.fSpawnDoneButton = GameObject.Find("DoneButton");
                    GameObject.Find("DoneButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ButtonHandler.Done);
                    Var.fSpawnDoneButton.GetComponent<UnityEngine.UI.Button>().interactable = false;

                    //Next Step buttons

                    GameObject.Find("HSetupButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ButtonHandler.MenuTransition(Var.cMenu0, Var.cMenu1, false, false));

                    GameObject.Find("MSetupButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ButtonHandler.MenuTransition(Var.cMenu0, Var.mMenu, false, false));

                    GameObject.Find("HDoneButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ButtonHandler.SaveButtonClick(Var.cMenu1, Var.cMenu0, true, false));

                    GameObject.Find("MDoneButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ButtonHandler.MenuTransition(Var.mMenu, Var.cMenu0, false, true));

                    GameObject.Find("MQuitButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Application.Quit);

                    //Setting up next and previous axis selection
                    //left

                    //trigger
                    Var.cMenu1.SetActive(false);
                    //Var.cMenu2.SetActive(false);


                    Var.mMenu.SetActive(false);
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


                fMenuassetBundle.Unload(false);



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

                
                Material newMaterial = new Material(Shader.Find("Sprites/Default"));
                newMaterial.SetFloat("_Mode", 1);

                newMaterial.color = Color.grey;

                //actually, the material less magenta is a pretty good choice for this
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


