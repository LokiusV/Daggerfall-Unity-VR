using DaggerfallWorkshop.Game.Items;
using UnityEngine;
using UnityEngine.XR;

namespace DFUVR
{
    public class HandLabel : MonoBehaviour
    {
        public bool isMainHand;
        public XRNode xrHandNode;
        public InputDevice inputDevice;
        public KeyCode grabButton;
        public GameObject freeHandObject;

        public DaggerfallUnityItem weaponItem;
        public GameObject weaponObject;

        public void Init(bool isMainHand, XRNode xrHandNode, InputDevice inputDevice, KeyCode grabButton)
        {
            this.isMainHand = isMainHand;
            this.xrHandNode = xrHandNode;
            this.inputDevice = inputDevice;
            this.grabButton = grabButton;
        }

        void Start()
        {
            Plugin.LoggerInstance.LogInfo("added Handlabel");
        }

        public void RemoveHeldWeapon()
        {
            SetWeapon(null, null, null);
        }

        public void SetWeapon(DaggerfallUnityItem weaponItem, GameObject weaponObject, HandObject handObject)
        {
            if (this.weaponObject != null)
                Destroy(this.weaponObject);

            this.weaponObject = weaponObject;
            this.weaponItem = weaponItem;

            if (weaponObject != null)
            {
                weaponObject.transform.SetParent(gameObject.transform);

                if (handObject != null)
                {
                    weaponObject.transform.localPosition = handObject.unsheatedPositionOffset;
                    weaponObject.transform.localRotation = handObject.unsheatedRotationOffset;
                }

                weaponObject.SetActive(true);
            }
        }
    }
}
