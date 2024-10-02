using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SpatialTracking;

namespace DFUVR
{
    public class Hands:MonoBehaviour
    {
        public static GameObject rHand;
        public static GameObject lHand;
        public static void Spawn()
        {
            //creating Hands
            Var.rightHand = new GameObject("RightHand");
            Var.leftHand = new GameObject("LeftHand");

            Var.rightHand.transform.parent = GameObject.Find("VRParent").transform;
            Var.leftHand.transform.parent = GameObject.Find("VRParent").transform;


            TrackedPoseDriver rightTracker=Var.rightHand.AddComponent<TrackedPoseDriver>();
            TrackedPoseDriver leftTracker = Var.leftHand.AddComponent<TrackedPoseDriver>();

            rightTracker.SetPoseSource(TrackedPoseDriver.DeviceType.GenericXRController, TrackedPoseDriver.TrackedPose.RightPose);
            leftTracker.SetPoseSource(TrackedPoseDriver.DeviceType.GenericXRController, TrackedPoseDriver.TrackedPose.LeftPose);

            SphereCollider rCollider=Var.rightHand.AddComponent<SphereCollider>();
            SphereCollider lCollider = Var.leftHand.AddComponent<SphereCollider>();

            rCollider.radius = 0.0668935f;
            lCollider.radius = 0.0668935f;
            rCollider.isTrigger = true;
            lCollider.isTrigger=true;
            Var.rightHand.AddComponent<HandLabel>().rightHand=true;
            Var.leftHand.AddComponent<HandLabel>().rightHand = false;

            
            //Rigidbody rHandBody=Var.rightHand.AddComponent<Rigidbody>();
            //Rigidbody lHandBody = Var.leftHand.AddComponent<Rigidbody>();

            //rHandBody.useGravity=false;
            //lHandBody.useGravity=false;

            string pluginFolderPath = Paths.PluginPath;


            string assetBundlePath = Path.Combine(pluginFolderPath, "AssetBundles/assetbundle2");


            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);

            GameObject controllerPrefab = assetBundle.LoadAsset<GameObject>("vr_controller_01_mrhat");

            rHand = Instantiate(controllerPrefab);
            lHand= Instantiate(controllerPrefab);
            //Plugin.LoggerInstance.LogInfo("Almost...");
            Var.InitModels();
            //Plugin.LoggerInstance.LogInfo("We'Re Here");
            rHand.transform.parent=Var.rightHand.transform;
            rHand.transform.localPosition = new Vector3(0,0,0);
            rHand.transform.Rotate(0,180,0);
            //Plugin.LoggerInstance.LogInfo("WE'Re here");
            lHand.transform.parent = Var.leftHand.transform;
            lHand.transform.localPosition = new Vector3(0, 0, 0);
            lHand.transform.Rotate(0, 180, 0);

            Var.handCam=Var.rightHand.AddComponent<Camera>();
            Var.handCam.stereoTargetEye=StereoTargetEyeMask.None;
            //Var.weaponObject = new GameObject("weapon");
            Var.body = new GameObject("Body");
            Var.body.AddComponent<BodyRotationController>();

            Var.body.AddComponent<SheathController>();
            assetBundle.Unload(false);

        }
    }
}
