using BepInEx;
using DaggerfallWorkshop.Game.Items;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.XR;

namespace DFUVR
{
    public class SheathController : MonoBehaviour
    {
        public GameObject sheath;
        public GameObject sphere;
        public SphereCollider sphereCollider;
        public GameObject sheathOB;
        public SheathCollision sheathCollision;

        public bool isOffHandSheath = false;

        private bool isGripPressed = false;
        private bool alreadyGripped = false;

        public GameObject weaponObj;
        public DaggerfallUnityItem weaponItem;
        public bool isWeaponSheathed = true;

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

            if (isOffHandSheath)
            {
                Var.offHandSphereSheathObject = sphere;
                Var.offHandSheathObject = sheathOB;
                MirrorSheathPosition();
            }
            else
            {
                Var.mainHandSphereSheathObject = sphere;
                Var.mainHandSheathObject = sheathOB;
            }

            // set initial state
            SheathWeapon(isOffHandSheath ? Hands.offHandLabel : Hands.mainHandLabel, null);
        }

        //this will handle all interaction with the Sheath
        void Update()
        {
            foreach (var hand in sheathCollision.handsInside.Values)
            {
                if (hand == null)
                    continue;

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
                    ToggleSheath(hand);
                }
            }
        }

        private void MirrorSheathPosition()
        {
            Vector3 localPos = sphere.transform.localPosition;
            localPos.x = -localPos.x;
            sphere.transform.localPosition = localPos;
        }

        private void ToggleSheath(HandLabel hand)
        {
            Plugin.LoggerInstance.LogInfo("ToggleSheath");

            if (isWeaponSheathed)
                UnSheathWeapon(hand);
            else if (hand.weaponObject != null)
                SheathWeapon(hand, hand.weaponItem);
        }

        private void UnSheathWeapon(HandLabel hand)
        {
            if (weaponObj != null)
                Destroy(weaponObj);
            weaponObj = null;

            var newWeaponObj = Weapon.GetWeaponObjectForHandObject(weaponItem, out HandObject currentHandObject);
            if (currentHandObject == null || newWeaponObj == null)
                return;

            if (newWeaponObj != null)
            {
                
                hand.SetWeapon(weaponItem, newWeaponObj, currentHandObject);
            }

            if (sheathOB != null)
            {
                MeshRenderer meshRenderer = sheathOB.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                    meshRenderer.enabled = true;
            }

            isWeaponSheathed = false;
            weaponItem = null;

            hand.freeHandObject.SetActive(false);
        }

        private void SheathWeapon(HandLabel hand, DaggerfallUnityItem weaponItem)
        {
            // can't sheath if we have something on the sheath already
            if (weaponObj != null)
                return;

            weaponObj = Weapon.GetWeaponObjectForHandObject(weaponItem, out HandObject currentHandObject);
            if (currentHandObject == null || weaponObj == null)
                return;

            SetWeapon(weaponItem, currentHandObject);

            hand.RemoveHeldWeapon();

            hand.freeHandObject.SetActive(true);
        }

        /// <summary>
        /// This method will replace the weapon that is currently sheathed, if any, by the one sent.
        /// The object has to have been created already. Use SheathWeapon if not.
        /// </summary>
        public void ReplaceWeapon(DaggerfallUnityItem weaponItem, GameObject weaponObject, HandObject handObject)
        {
            if (weaponObj != null)
                Destroy(weaponObj);

            weaponObj = weaponObject;

            SetWeapon(weaponItem, handObject);

            weaponObj.SetActive(true);
        }

        private void SetWeapon(DaggerfallUnityItem weaponItem, HandObject currentHandObject)
        {
            Collider collider = weaponObj.GetComponent<Collider>();
            if (collider != null)
                collider.enabled = false;

            if (sheathOB != null)
                weaponObj.transform.SetParent(sheathOB.transform);

            weaponObj.transform.localPosition = currentHandObject.sheatedPositionOffset;
            weaponObj.transform.localRotation = currentHandObject.sheatedRotationOffset;

            if (sheathOB != null)
            {
                MeshRenderer meshRenderer = sheathOB.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                    meshRenderer.enabled = currentHandObject.renderSheated;
            }

            isWeaponSheathed = true;
            this.weaponItem = weaponItem;
        }
    }
}
