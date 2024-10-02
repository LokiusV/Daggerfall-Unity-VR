using UnityEngine;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Entity;
using DFUVR;
using HarmonyLib;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.Questing;
namespace DFUVR
{
    public class WeaponCollision : MonoBehaviour
    {
        //public float velocityThreshold = 2.0f;

        //private Rigidbody rb;

        void Start()
        {
            //rb = GetComponent<Rigidbody>();
        }
        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.Y))
        //    {

        //        WeaponManager weaponManager=GameObject.Find("PlayerAdvanced").GetComponent<WeaponManager>();
        //        weaponManager.ToggleSheath();

        //    }
        //}

        private void OnTriggerEnter(Collider other)
        {


            //Plugin.LoggerInstance.LogInfo("Hit something");

            if (other.GetComponent<DaggerfallEntityBehaviour>())
            {
                //Debug.Log("hit");
                //Vector3 relativeVelocity = rb.velocity;
                //if (relativeVelocity.magnitude > velocityThreshold)
                //{

                //Plugin.LoggerInstance.LogInfo("Hit Entity");

                //    Vector3 hitDirection = other.transform.position - transform.position;
                //    hitDirection = hitDirection.normalized;
                //    Debug.Log("Hit Direction: " + hitDirection);
                //}
                Vector3 hitDirection = other.transform.position - transform.position;
                Transform hitTransform = other.transform;
                WeaponManager weaponManager = GameObject.Find("PlayerAdvanced").GetComponent<WeaponManager>();
                DaggerfallUnityItem currentRightHandWeapon = (DaggerfallUnityItem)AccessTools.Field(typeof(WeaponManager), "currentRightHandWeapon").GetValue(weaponManager);
                hitDirection = hitDirection.normalized;
                //Debug.Log("Hit Direction: " + hitDirection);

                weaponManager.WeaponDamage(currentRightHandWeapon, false, false, hitTransform, hitTransform.localPosition, hitDirection);

            }
            else if (other.GetComponent<DaggerfallAction>())
            {
                Plugin.LoggerInstance.LogInfo("Hit action");
                DaggerfallAction action= other.GetComponent<DaggerfallAction>();
                if (Var.weaponManager != null)
                {
                    GameObject player = (GameObject)AccessTools.Field(typeof(WeaponManager), "player").GetValue(Var.weaponManager);
                    action.Receive(player, DaggerfallAction.TriggerTypes.Attack);
                }
                else
                {
                    Plugin.LoggerInstance.LogError("null");
                }

            }
            else if (other.GetComponent<DaggerfallActionDoor>()) 
            {
                //Plugin.LoggerInstance.LogInfo("Hit door");
                DaggerfallActionDoor actionDoor = other.GetComponent<DaggerfallActionDoor>();
                if (actionDoor)
                {
                    actionDoor.AttemptBash(true);
                }


            }

            Haptics.TriggerHapticFeedback(UnityEngine.XR.XRNode.RightHand, 0.6f);
            //else if()

        }


    }
}
