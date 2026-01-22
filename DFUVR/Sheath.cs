using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BepInEx;
using DaggerfallWorkshop.Game;
using UnityEngine.XR;


namespace DFUVR
{
    public class SheathController : MonoBehaviour 
    {
        public static GameObject sheath;
        public static GameObject sphere;
        public static SphereCollider sphereCollider;
        public static GameObject sheathOB;
        private static bool gripFlag=false;
        private static bool alreadyGripped=false;
        void Start()
        {
            sphere = new GameObject("Sphere");
            sphere.transform.parent=GameObject.Find("Body").transform;
            if (GameObject.Find("Body") == null) { Plugin.LoggerInstance.LogError("Body not found. Are you a ghost? Or did I just fuck smthn up?"); }
            //sphere.transform.localPosition = new Vector3(-0.155f,0,0);
            //sphere.transform.localPosition = Var.sheathOffset;
            //sphere.transform.localPosition = sphere.transform.parent.InverseTransformPoint(Var.sheathOffset);
            sphere.transform.Rotate(-231.724f, 0, 0);
            
            
            sphere.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            
            sphereCollider = sphere.AddComponent<SphereCollider>();
            sphereCollider.radius = 0.25f;
            sphereCollider.isTrigger = true;
            Var.sphereObject = sphere;
            sphere.AddComponent<SheathCollision>();

            Rigidbody rb=sphere.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity=false;
            //DebugSphere.CreateVisualizer(sphereCollider);

            string assetBundlePath = Path.Combine(Paths.PluginPath, "AssetBundles/weapons");


            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);

            GameObject sheathInstance = assetBundle.LoadAsset<GameObject>("Sheath");

            GameObject sheath = Instantiate(sheathInstance);

            sheath.transform.parent = sphere.transform;
            sheath.transform.localRotation = Quaternion.identity;

            sheathOB = sheath;
            //GameObject sheath = new GameObject("Sheath");
            //sheath.transform.parent = sphere.transform;
            //sheath.transform.localRotation = Quaternion.identity;

            assetBundle.Unload(false);
            Var.sheathObject=sheath;

            //sphere.transform.localPosition = sphere.transform.parent.InverseTransformPoint(Var.sheathOffset);
            sphere.transform.localPosition = Var.sheathOffset;

        }
        //this will handle all interaction with the Sheath

        void Update()
        {
            if (Var.isNotOculus)
            {
                bool gripButton;
                var rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                if (Var.leftHanded) { rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand); }
                rightHand.TryGetFeatureValue(CommonUsages.gripButton, out gripButton);
                //rightHand.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButton);

                if (gripButton) { gripFlag = true; }
                if(!gripButton) { gripFlag= false; alreadyGripped = false; }
                if ((gripFlag && SheathCollision.flag) && !alreadyGripped)
                {
                    alreadyGripped = true;
                    GameObject.Find("PlayerAdvanced").GetComponent<WeaponManager>().ToggleSheath();
                }
            }
            if (Input.GetKeyDown(Var.gripButton))
            {
                gripFlag = true;
            }
            if(Input.GetKeyUp(Var.gripButton)) { gripFlag = false; alreadyGripped = false; }
            if ((gripFlag && SheathCollision.flag)&&!alreadyGripped)
            {
                alreadyGripped = true;
                GameObject.Find("PlayerAdvanced").GetComponent<WeaponManager>().ToggleSheath();
                
                
            }
            

        }
    
    
    }
}
