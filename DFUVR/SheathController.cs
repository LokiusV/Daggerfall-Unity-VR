using System.IO;
using UnityEngine;
using BepInEx;
using DaggerfallWorkshop.Game;
using UnityEngine.XR;
using System;

namespace DFUVR
{
    public class SheathController : MonoBehaviour
    {
        public GameObject sheath;
        public GameObject sphere;
        public SphereCollider sphereCollider;
        public GameObject sheathOB;
        public SheathCollision sheathCollision;

        public bool isLeftSheath = false;

        private bool isGripPressed = false;
        private bool alreadyGripped = false;

        void Start()
        {
            sphere = new GameObject("Sphere" + Guid.NewGuid());

            var body = GameObject.Find("Body");
            if (body == null)
                Plugin.LoggerInstance.LogError("Body not found. Are you a ghost? Or did I just fuck smthn up?");
            else
                sphere.transform.parent = body.transform;

            //sphere.transform.localPosition = new Vector3(-0.155f,0,0);
            //sphere.transform.localPosition = Var.sheathOffset;
            //sphere.transform.localPosition = sphere.transform.parent.InverseTransformPoint(Var.sheathOffset);
            //sphere.transform.Rotate(-231.724f, 0, 0);
            sphere.transform.Rotate(-300f, 0, 0);
            sphere.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            sphereCollider = sphere.AddComponent<SphereCollider>();
            sphereCollider.radius = 0.25f;
            sphereCollider.isTrigger = true;

            sheathCollision = sphere.AddComponent<SheathCollision>();

            Rigidbody rb = sphere.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            //DebugSphere.CreateVisualizer(sphereCollider);

            string assetBundlePath = Path.Combine(Paths.PluginPath, "AssetBundles/weapons");
            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);
            GameObject sheathAsset = assetBundle.LoadAsset<GameObject>("Sheath");
            assetBundle.Unload(false);

            GameObject sheath = Instantiate(sheathAsset);
            sheath.transform.parent = sphere.transform;
            sheath.transform.localRotation = Quaternion.identity;

            sheathOB = sheath;
            //GameObject sheath = new GameObject("Sheath");
            //sheath.transform.parent = sphere.transform;
            //sheath.transform.localRotation = Quaternion.identity;

            //sphere.transform.localPosition = sphere.transform.parent.InverseTransformPoint(Var.sheathOffset);
            sphere.transform.localPosition = Var.sheathOffset;

            if (isLeftSheath)
            {
                Var.leftSphereSheathObject = sphere;
                Var.leftSheathObject = sheathOB;
                MirrorSheathPosition();
            }
            else
            {
                Var.rightSphereSheathObject = sphere;
                Var.rightSheathObject = sheathOB;
            }
        }

        //this will handle all interaction with the Sheath
        void Update()
        {
            var hand = sheathCollision.handInside;
            if (hand == null)
                return;
            
            if (Var.isNotOculus)
            {
                hand.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool gripButton);
                if (gripButton)
                {
                    isGripPressed = true;
                }
                else
                {
                    isGripPressed = false;
                    alreadyGripped = false;
                }
            }
            else
            {
                if (Input.GetKeyDown(hand.grabButton))
                    isGripPressed = true;

                if (Input.GetKeyUp(hand.grabButton))
                {
                    isGripPressed = false;
                    alreadyGripped = false;
                }
            }

            if (isGripPressed && !alreadyGripped)
            {
                alreadyGripped = true;
                GameObject.Find("PlayerAdvanced").GetComponent<WeaponManager>().ToggleSheath();
            }
        }

        private void MirrorSheathPosition()
        {
            Vector3 localPos = sphere.transform.localPosition;
            localPos.x = -localPos.x;
            sphere.transform.localPosition = localPos;
        }
    }
}
