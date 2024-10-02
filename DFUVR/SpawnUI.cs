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
                
                Camera.main.stereoTargetEye=StereoTargetEyeMask.None;
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
                vrui.transform.localScale=new Vector3((float)Var.windowWidth/1000f, (float)Var.windowHeight/1000f,1);

                //Var.keyboard.
                Hands.Spawn();

                Var.keyboard.transform.SetParent(vrui.transform);
                Var.keyboard.transform.localPosition = new Vector3(0, -0.7f, 0);
                Var.keyboard.transform.localRotation = Quaternion.Euler(45,0,0);

                Var.keyboard.SetActive(true);
                



            }

            
            

            
            CreateMenuController();
            Var.isFirst = false;


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
            GameObject vrparent = GameObject.Find("VRParent");


            GameObject emptyObject = new GameObject("LaserPointer");
            if (!Var.isFirst)
            {
                emptyObject.transform.parent=vrparent.transform;

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


