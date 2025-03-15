using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using DaggerfallConnect;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Formulas;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.MagicAndEffects;
using DaggerfallWorkshop.Game.Questing;
using DaggerfallWorkshop.Game.Serialization;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using HarmonyLib;
using HarmonyLib.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using static DaggerfallWorkshop.Game.InputManager;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.XR;
using UnityEngine.XR.Provider;
using DaggerfallWorkshop.Utility;
using DaggerfallWorkshop.Game.MagicAndEffects.MagicEffects;

namespace DFUVR
{

    
    //initialize UI
    [HarmonyPatch(typeof(DaggerfallUI), "Start")]
    public class MenuPatch : MonoBehaviour
    {
        [HarmonyPostfix]
        static void Postfix(DaggerfallUI __instance)
        {
            __instance.gameObject.AddComponent<sTx>();
            
            __instance.StartCoroutine(UI.Spawn());
            
            

        }
    }
    //needed for UI. attaches a render texture reference to the UI
    [HarmonyPatch(typeof(GameManager), "Start")]
    public class CalibratePatch : MonoBehaviour
    {
        [HarmonyPostfix]
        static void Postfix(GameManager __instance)
        {
            __instance.StartCoroutine(GameObject.Find("DaggerfallUI").GetComponent<sTx>().UICal());

        }
    }

    //this patch fixes the orientation of moving npcs
    [HarmonyPatch(typeof(MobilePersonBillboard), "Start")]
    public class MobileNPCOrientationFix
    {
        [HarmonyPostfix]
        static void Postfix(MobilePersonBillboard __instance)
        {
            AccessTools.Field(typeof(MobilePersonBillboard), "mainCamera").SetValue(__instance, Var.VRCamera);


        }
    }
    //This adjusts the time to be properly formatted on the watch.
    [HarmonyPatch(typeof(DaggerfallDateTime), "ShortTimeString")]
    public class ShortTimeStringPatch
    {
        [HarmonyPrefix]
        static bool Prefix(DaggerfallDateTime __instance, ref string __result)
        {
            __result = string.Format("{0:00}:{1:00} {2:00} {3} 3E{4}", __instance.Hour, __instance.Minute, __instance.Day + 1, __instance.MonthName, __instance.Year);
            return false;

        }
    }

    //fixes arrow colliding with the Weapon models.
    [HarmonyPatch(typeof(DaggerfallMissile), "Start")]
    public class SpawnMissilePatch : MonoBehaviour
    {
        [HarmonyPostfix]
        static void Postfix(DaggerfallMissile __instance)
        {
            GameObject goModel = (GameObject)AccessTools.Field(typeof(DaggerfallMissile), "goModel").GetValue(__instance);
            //Vector3 direction = (Vector3)AccessTools.Field(typeof(DaggerfallMissile), "direction").GetValue(__instance);

            //goModel.transform.localPosition=Var.rightHand.transform.position+new Vector3(1,0,0);
            //goModel.transform.localRotation=Var.rightHand.transform.rotation;
            //Physics.IgnoreCollision(Var.rightHand.GetComponent<Collider>(), goModel.GetComponent<Collider>());

            //goModel.layer = 0;
            //Transform rightHandTransform = Var.rightHand.transform;

            ////// Set the arrow's rotation to match the right hand's rotation
            ////goModel.transform.localRotation = rightHandTransform.rotation;

            //// Set the arrow's position to be 1 meter in front of the hand's position
            //Vector3 spawnOffset = rightHandTransform.right * 1f;  // 1 meter in front of the hand
            //goModel.transform.localPosition = rightHandTransform.position + spawnOffset;
            //goModel.layer = __instance.gameObject.layer;
            //Plugin.LoggerInstance.LogInfo("Pointig towards" + Var.rightHand.transform.forward);
            //Ensure the arrow's collider ignores the hand's collider



            Collider handCollider = Var.rightHand.GetComponent<Collider>();
            Collider arrowCollider = goModel.GetComponent<Collider>();

            //TODO: put all the colliders in a list and for through them instead of this massive block

            Physics.IgnoreCollision(handCollider, arrowCollider);
            Physics.IgnoreCollision(GameObject.Find("PlayerAdvanced").GetComponent<Collider>(),arrowCollider);
            Physics.IgnoreCollision(handCollider, __instance.GetComponent<Collider>());

            //no clue why this is necessary but for some reason it always collides with these objects if I don't specifically ignore them
            Physics.IgnoreCollision(GameObject.Find("VRUI").GetComponent<Collider>(), arrowCollider);
            Physics.IgnoreCollision(Var.mace.GetComponent<Collider>(), arrowCollider);
            Physics.IgnoreCollision(Var.battleaxe.GetComponent<Collider>(), arrowCollider);
            Physics.IgnoreCollision(Var.sword.GetComponent<Collider>(), arrowCollider);
            Physics.IgnoreCollision(Var.staff.GetComponent<Collider>(), arrowCollider);
            Physics.IgnoreCollision(Var.bow.GetComponent<Collider>(), arrowCollider);
            Physics.IgnoreCollision(Var.hammer.GetComponent<Collider>(), arrowCollider);
            Physics.IgnoreCollision(Var.flail.GetComponent<Collider>(), arrowCollider);
            Physics.IgnoreCollision(Var.elseA.GetComponent<Collider>(), arrowCollider);
            Physics.IgnoreCollision(Var.meleeHandR.GetComponent<Collider>(), arrowCollider);

            //Plugin.LoggerInstance.LogInfo("Bowed");
            //Var.debugSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //Var.debugSphere.GetComponent<Collider>().enabled = false;

            //__instance.gameObject.AddComponent <CollisionFixMissile>();

            //AccessTools.Field(typeof(DaggerfallMissile), "direction").SetValue(__instance,Var.rightHand.transform.forward);

        }
    }
    //[HarmonyPatch(typeof(DaggerfallMissile), "Start")]
    //public class SpawnMissileCameraPatch : MonoBehaviour
    //{
    //    [HarmonyPrefix]
    //    static void Prefix(DaggerfallMissile __instance)
    //    {


    //    }
    //}
    //[HarmonyPatch(typeof(DaggerfallMissile), "DoCollision")]
    //public static class DebugImpact
    //{
    //    [HarmonyPrefix]
    //    static void Prefix(DaggerfallMissile __instance, Collision collision, Collider other)
    //    {
    //        Plugin.LoggerInstance.LogInfo("Entered Missile hit");
    //        try
    //        {
    //            Plugin.LoggerInstance.LogInfo("Hit Something: " + other.gameObject.name);
    //        }
    //        catch (Exception e) { }
    //        try
    //        {
    //            Plugin.LoggerInstance.LogInfo("Hit Something: " + collision.gameObject.name);


    //        }
    //        catch { }
    //    }
    //}
    //[HarmonyPatch(typeof(DaggerfallMissile), "DoMissile")]
    //public static class DebugImpactTrigger
    //{
    //    [HarmonyPostfix]
    //    static void Postfix(DaggerfallMissile __instance)
    //    {


    //        GameObject goModel = (GameObject)AccessTools.Field(typeof(DaggerfallMissile), "goModel").GetValue(__instance);
    //        __instance.transform.position = goModel.transform.position;
    //        Plugin.LoggerInstance.LogInfo(__instance.transform.position);
    //    }
    //}
    //[HarmonyPatch(typeof(DaggerfallMissile), "Update")]
    //public static class DebugUpdateMissile
    //{
    //    [HarmonyPostfix]
    //    static void Postfix(DaggerfallMissile __instance)
    //    {
    //        //AccessTools.Field(typeof(DaggerfallMissile),"impactDetected").SetValue(__instance,false);
    //        //GameObject myBillboard = (GameObject)AccessTools.Field(typeof(DaggerfallMissile), "myBillboard").GetValue(__instance);
    //        //Plugin.LoggerInstance.LogInfo(__instance.transform.position);
    //        //__instance.GetComponent<Collider>().transform.position = __instance.transform.position;
    //        //Var.debugSphere.transform.position = __instance.GetComponent<Collider>().transform.position;//__instance.transform.position;
    //    }
    //}
    //[HarmonyPatch(typeof(DaggerfallAction), "UserInputHandler")]
    //public class HideAfterInputPatch : MonoBehaviour
    //{
    //    [HarmonyPostfix]
    //    static void Postfix(UserInterfaceManager __instance)
    //    {
    //        Var.activeWindowCount--;
    //        //Plugin.LoggerInstance.LogInfo(Var.activeWindowCount);
    //        if (Var.activeWindowCount < 2)
    //        {
    //            //Plugin.LoggerInstance.LogInfo("Exited Window");
    //            GameObject vrui = GameObject.Find("VRUI");
    //            GameObject laserPointer = GameObject.Find("LaserPointer");
    //            GameObject idleParent;
    //            if (GameObject.Find("IdleParent") == null)
    //            {
    //                idleParent = new GameObject("IdleParent");
    //                idleParent.transform.position = Vector3.zero;

    //                //This is the HUD
    //                idleParent.AddComponent<HUDSpawner>();
    //            }
    //            else
    //            {
    //                idleParent = GameObject.Find("IdleParent");

    //            }

    //            vrui.transform.SetParent(idleParent.transform);
    //            laserPointer.transform.SetParent(idleParent.transform);
    //            vrui.transform.localPosition = Vector3.zero;
    //            vrui.transform.localRotation = Quaternion.identity;
    //            laserPointer.transform.localPosition = Vector3.zero;
    //        }

    //    }

    //}
    //[HarmonyPatch(typeof(DaggerfallAction), "ShowTextWithInput")]
    //public class InputNotifPatch : MonoBehaviour
    //{
    //    [HarmonyPostfix]
    //    static void Postfix(UserInterfaceManager __instance)
    //    {
    //        //Plugin.LoggerInstance.LogInfo("Entered Window");
    //        GameObject vrui = GameObject.Find("VRUI");
    //        GameObject laserPointer = GameObject.Find("LaserPointer");
    //        GameObject vrParent = GameObject.Find("VRParent");
    //        vrui.transform.SetParent(vrParent.transform);
    //        laserPointer.transform.SetParent(vrParent.transform);
    //        CoroutineRunner.Instance.StartRoutine(PushPatch.Waiter1());
    //        //laserPointer.transform.localPosition = Vector3.zero;
    //        Var.activeWindowCount++;

    //    }

    //}
    //shows UI, recalibrates its position and repositions the player so that enemies can actually hit them
    [HarmonyPatch(typeof(UserInterfaceManager), "PushWindow")]
    public class PushPatch : MonoBehaviour
    {
        private static PushPatch __pInstance; 

        void Awake()
        {
            __pInstance = this;
        }

        [HarmonyPostfix]
        static void Postfix(UserInterfaceManager __instance)
        {
            //Plugin.LoggerInstance.LogInfo("Entered Window");
            GameObject vrui = GameObject.Find("VRUI");
            GameObject laserPointer = GameObject.Find("LaserPointer");
            GameObject vrParent = GameObject.Find("VRParent");
            vrui.transform.SetParent(vrParent.transform);
            laserPointer.transform.SetParent(vrParent.transform);
            CoroutineRunner.Instance.StartRoutine(Waiter1());
            //laserPointer.transform.localPosition = Vector3.zero;
            Var.activeWindowCount++;

        }
        //static IEnumerator Waiter1()
        //{
        //    //Plugin.LoggerInstance.LogInfo("Started Coroutine");
        //    GameObject vrui = GameObject.Find("VRUI");
        //    yield return new WaitForSecondsRealtime(0.2f);
        //    //vrui.transform.localPosition = Var.VRCamera.transform.localPosition + new Vector3(0, 0, 2f);
        //    Transform vrCameraTransform = Var.VRCamera.transform;
        //    Vector3 uiPositionInFront = vrCameraTransform.position + vrCameraTransform.forward * 2f;
        //    vrui.transform.localPosition = vrui.transform.parent.InverseTransformPoint(uiPositionInFront);
        //    vrui.transform.rotation = Quaternion.LookRotation(vrui.transform.position - vrCameraTransform.position);
        //}
        public static IEnumerator Waiter1()
        {
            GameObject vrui = GameObject.Find("VRUI");
            //delay is strictly necessary.
            yield return new WaitForSecondsRealtime(0.2f);
            Transform vrCameraTransform = Var.VRCamera.transform;
            Vector3 forwardFlat = Vector3.ProjectOnPlane(vrCameraTransform.forward, Vector3.up).normalized;
            Vector3 uiPositionInFront = vrCameraTransform.position + forwardFlat * 1.5f;
            vrui.transform.localPosition = vrui.transform.parent.InverseTransformPoint(uiPositionInFront);
            Vector3 lookDirection = uiPositionInFront - vrCameraTransform.position;
            lookDirection.y = 0; 
            vrui.transform.rotation = Quaternion.LookRotation(lookDirection);
            FixObstruction(vrui);


            GameObject tempObject = new GameObject("tempObject");
            GameObject playerAdvanced = GameObject.Find("PlayerAdvanced");
            GameObject smoothTransform = GameObject.Find("SmoothFollower");

            tempObject.transform.position = playerAdvanced.transform.position;
            tempObject.transform.rotation = playerAdvanced.transform.rotation;

            GameObject vrParent = GameObject.Find("VRParent");
            Var.VRParent = vrParent;
            vrParent.transform.parent = tempObject.transform;

            playerAdvanced.GetComponent<CharacterController>().enabled = false;
            playerAdvanced.transform.position = new Vector3(Var.VRCamera.transform.position.x, playerAdvanced.transform.position.y, Var.VRCamera.transform.position.z);
            playerAdvanced.GetComponent<CharacterController>().center = Vector3.zero;
            //smoothTransform.transform.localPosition = Vector3.zero;
            vrParent.transform.parent = smoothTransform.transform;
            Vector3 offset= playerAdvanced.transform.position-tempObject.transform.position;
            vrParent.transform.localPosition = new Vector3(0-Var.VRCamera.transform.localPosition.x, vrParent.transform.localPosition.y, 0 - Var.VRCamera.transform.localPosition.z);
            playerAdvanced.GetComponent<CharacterController>().enabled = true;
            GameObject.Destroy(tempObject);

            //GameObject playerAdvanced = GameObject.Find("PlayerAdvanced");
            //GameObject smoothTransform = GameObject.Find("SmoothFollower");
            //GameObject vrParent = GameObject.Find("VRParent");
            //Transform originalParent = vrParent.transform.parent;
            //GameObject tempObject = new GameObject("tempObject");


            //tempObject.transform.position = playerAdvanced.transform.position;
            //tempObject.transform.rotation = playerAdvanced.transform.rotation;
            //vrParent.transform.parent = tempObject.transform;

            //CharacterController characterController = playerAdvanced.GetComponent<CharacterController>();
            //characterController.enabled = false;

            //playerAdvanced.transform.position = new Vector3(Var.VRCamera.transform.position.x, playerAdvanced.transform.position.y, Var.VRCamera.transform.position.z);
            //playerAdvanced.transform.rotation = Quaternion.Euler(0, Var.VRCamera.transform.eulerAngles.y, 0);
            //smoothTransform.transform.localPosition = Vector3.zero;

            //vrParent.transform.parent = smoothTransform.transform;
            //vrParent.transform.localPosition = new Vector3(0, vrParent.transform.localPosition.y, 0);
            //characterController.center = Vector3.zero;
            //characterController.enabled = true;

            //GameObject.Destroy(tempObject);
            //GameObject playerAdvanced = GameObject.Find("PlayerAdvanced");
            //GameObject smoothTransform = GameObject.Find("SmoothFollower");
            //GameObject vrParent = GameObject.Find("VRParent");
            //Transform originalParent = vrParent.transform.parent;
            //GameObject tempObject = new GameObject("tempObject");

            //tempObject.transform.position = playerAdvanced.transform.position;
            //tempObject.transform.rotation = playerAdvanced.transform.rotation;

            //vrParent.transform.parent = tempObject.transform;
            //CharacterController characterController = playerAdvanced.GetComponent<CharacterController>();
            //characterController.enabled = false;

            //Vector3 vrCameraLocalOffset = Var.VRCamera.transform.localPosition;
            //playerAdvanced.transform.position = new Vector3(
            //    Var.VRCamera.transform.position.x - vrCameraLocalOffset.x,
            //    playerAdvanced.transform.position.y,
            //    Var.VRCamera.transform.position.z - vrCameraLocalOffset.z
            //);

            //playerAdvanced.transform.rotation = Quaternion.Euler(0, Var.VRCamera.transform.eulerAngles.y, 0);
            //smoothTransform.transform.localPosition = Vector3.zero;
            //vrParent.transform.parent = smoothTransform.transform;
            //vrParent.transform.localPosition = new Vector3(0, vrParent.transform.localPosition.y, 0);

            //characterController.center = Vector3.zero;
            //characterController.enabled = true;
            //GameObject.Destroy(tempObject);


            //var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //sphere.GetComponent<Collider>().enabled = false;
            //sphere.transform.position = GameObject.Find("PlayerAdvanced").transform.position;

            //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //cube.GetComponent<Collider>().enabled = false;
            //cube.transform.position = Camera.main.transform.position;

            //var capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            //capsule.GetComponent<Collider>().enabled = false;
            //capsule.transform.position=Var.VRCamera.transform.position;

            //to fix the hitboxes of levers being too small
            if (GameObject.Find("Dungeon") != null)
            {
                GameObject dungeon = GameObject.Find("Dungeon");

                DaggerfallAction[] actions=dungeon.GetComponentsInChildren<DaggerfallAction>();

                foreach (DaggerfallAction action in actions)
                {
                    if (action.gameObject.GetComponent<BoxCollider>() == null)
                    {

                        BoxCollider boxCollider = action.gameObject.AddComponent<BoxCollider>();
                        boxCollider.isTrigger = true;
                        boxCollider.size = new Vector3(1, 1, 1);

                    }
                }
            }

            Var.body.GetComponent<BodyRotationController>().ResetRotation();



        }
        //This prevents the UI from spawning inside of an object.
        static void FixObstruction(GameObject vrui)
        {
            Transform vrCameraTransform = Var.VRCamera.transform;
            Ray ray = new Ray(vrCameraTransform.position, vrui.transform.position - vrCameraTransform.position);
            RaycastHit hit;

            int layerMask = ~LayerMask.GetMask("Automap");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                
                if (hit.transform != vrui.transform)
                {
                    //Plugin.LoggerInstance.LogInfo($"Obstruction found: {hit.transform.name}");

                    Vector3 newUIPosition = hit.point + hit.normal * 0.1f; 
                    vrui.transform.position = newUIPosition;
                    Vector3 lookDirection = newUIPosition - vrCameraTransform.position;
                    lookDirection.y = 0; 
                    vrui.transform.rotation = Quaternion.LookRotation(lookDirection);
                }
            }
        }

    }
    
    //[HarmonyPatch(typeof(DaggerfallUI), "PopupMessage")]
    //public class PopuphPatch : MonoBehaviour
    //{
    //    [HarmonyPostfix]
    //    static void Postfix(DaggerfallUI __instance)
    //    {
    //        Plugin.LoggerInstance.LogInfo("Entered Window");
    //        GameObject vrui = GameObject.Find("VRUI");
    //        GameObject laserPointer = GameObject.Find("LaserPointer");
    //        GameObject vrParent = GameObject.Find("VRParent");
    //        vrui.transform.SetParent(vrParent.transform);
    //        laserPointer.transform.SetParent(vrParent.transform);

    //    }

    //}

    //hides UI when all windows are closed
    [HarmonyPatch(typeof(UserInterfaceManager), "PopWindow")]
    public class PopPatch : MonoBehaviour
    {
        [HarmonyPostfix]
        static void Postfix(UserInterfaceManager __instance)
        {
            Var.activeWindowCount--;
            //Plugin.LoggerInstance.LogInfo(Var.activeWindowCount);
            if (Var.activeWindowCount < 2)
            {
                //Plugin.LoggerInstance.LogInfo("Exited Window");
                GameObject vrui = GameObject.Find("VRUI");
                GameObject laserPointer = GameObject.Find("LaserPointer");
                GameObject idleParent;
                if (GameObject.Find("IdleParent") == null)
                {
                    idleParent = new GameObject("IdleParent");
                    idleParent.transform.position = Vector3.zero;

                    //This is the HUD
                    idleParent.AddComponent<HUDSpawner>();
                }
                else
                {
                    idleParent = GameObject.Find("IdleParent");

                }

                vrui.transform.SetParent(idleParent.transform);
                laserPointer.transform.SetParent(idleParent.transform);
                vrui.transform.localPosition=Vector3.zero;
                vrui.transform.localRotation = Quaternion.identity;
                laserPointer.transform.localPosition = Vector3.zero;
            }

        }

    }
    //handles most vr input, calibration mode and fixes some necessary settings.
    [HarmonyPatch(typeof(InputManager), "Update")]
    public class ControllerPatch : MonoBehaviour
    {
        public static bool flag = false;
        public static bool isChanging = false;
        private static bool changedCam = false;
        public static bool bindingCalibrated=false;
        [HarmonyPrefix]
        
        static void Prefix(InputManager __instance)
        {
            __instance.EnableController=false;
            Screen.fullScreen = true;

            //the default bindings. I'll move this somewhere else later. This really shouldn't be in update.
            //#region Bindings
            //InputManager.Instance.SetBinding(Var.lStickButton, Actions.Run, true);
            //InputManager.Instance.SetBinding(KeyCode.UpArrow, InputManager.Actions.ToggleConsole, true);
            //InputManager.Instance.SetBinding(Var.acceptButton, InputManager.Actions.ActivateCenterObject, true);
            //InputManager.Instance.SetBinding(Var.cancelButton, InputManager.Actions.Inventory, true);
            //InputManager.Instance.SetBinding(Var.gripButton, InputManager.Actions.RecastSpell, true);
            //InputManager.Instance.SetBinding(Var.rStickButton, InputManager.Actions.CastSpell, true);
            //#endregion

            //handles the player collision and makes it room scale
            if (Var.charControllerCalibrated)
            {
                Var.characterController.center = new Vector3(Var.VRCamera.transform.localPosition.x+Var.VRParent.transform.localPosition.x, Var.characterController.center.y, Var.VRCamera.transform.localPosition.z + Var.VRParent.transform.localPosition.z);
            }
            else if (!Var.charControllerCalibrated&&Var.characterController!=null)
            {
                if (GameManager.Instance.IsPlayingGame())
                {
                    CoroutineRunner.Instance.StartCoroutine(Waiter2());
                    //GameObject.Find("PLayerAdvanced").AddComponent<MeshFilter>();
                    


                }

            }
            //if (Input.GetKeyDown(KeyCode.L))
            //{
            //    Plugin.LoggerInstance.LogInfo("Sending");
            //    //System.Windows.Forms.SendKeys.Send("L");
            //    KeySender.PressKey(System.Windows.Forms.Keys.L,false);
            //    KeySender.PressKey(System.Windows.Forms.Keys.L, true);
            //    Plugin.LoggerInstance.LogInfo("Sent");
            //}

            //Calibration Mode
            if (isChanging)
            {
                //ControllerPatch.flag = false;
                //float input = Input.GetAxis("Axis5");
                //float input = Input.GetAxis(Var.rThumbStickVertical);
                if (Input.GetKeyDown(Var.acceptButton))
                {
                    Var.skyboxToggle = !Var.skyboxToggle;
                    Plugin.LoggerInstance.LogInfo(Var.skyboxToggle);
                }
                var rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

                Vector2 rThumbStick;
                rightHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out rThumbStick);
                

                float inputX1 = rThumbStick.x;
                float inputY1 = rThumbStick.y;

                float input = rThumbStick.y;

                Var.heightOffset += input / 100;
                //Var.sphereObject.transform.localPosition=Vector3.zero;
                Var.sheathOffset = Var.leftHand.transform.position;
                Var.sphereObject.transform.position = Var.sheathOffset;
                GameObject vrparent = GameObject.Find("VRParent");
                vrparent.transform.localPosition = new Vector3(vrparent.transform.localPosition.x, (float)Var.heightOffset, vrparent.transform.localPosition.z);
            }
            if (Input.GetKeyDown(Var.left2Button))
            {

                //Debug.Log("§");
                flag = true;
                Var.debugInt += 1;



            }
            
            //we don't want to open the pause menu when the user just wants to recalibrate
            if (flag) { InputManager.Instance.SetBinding(KeyCode.Escape, Actions.Escape, true); }
            else if (!flag) { InputManager.Instance.SetBinding(Var.left1Button, Actions.Escape, true); }

            //height and holster/sheath calibration
            if (flag && Input.GetKeyDown(Var.left1Button))
            {
                //Debug.Log("WOMBO COMBO");
                isChanging = true;



            }
            if ((Input.GetKeyUp(Var.left1Button) || Input.GetKeyUp(Var.left2Button)) && flag)
            {
                //Debug.Log("canceled");
                flag = false;
                isChanging = false;
                Var.SaveHeight();
            }

            if (Var.isNotOculus)
            {
                var leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

                bool primaryPressed;
                bool secondaryPressed;
                leftHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out primaryPressed);
                leftHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out secondaryPressed);

                if (primaryPressed)
                {
                    flag = true;
                    Var.debugInt += 1;

                }
                if (secondaryPressed && flag)
                {
                    isChanging = true;
                }
                if ((!primaryPressed || !secondaryPressed) && flag) 
                {
                    //Debug.Log("canceled");
                    flag = false;
                    isChanging = false;
                    Var.SaveHeight();

                }

            }
            //snap turning
            SnapTurnProvider.Snap();

            
            GameObject exterior = GameObject.Find("Exterior");

            //this atleast creates a sense of day and night
            if (exterior != null )//&& Var.skyboxToggle)
            {
                //Plugin.LoggerInstance.LogInfo(exterior.activeSelf);
                Var.VRCamera.clearFlags = CameraClearFlags.Nothing;
            }
            else { Var.VRCamera.clearFlags = CameraClearFlags.Skybox; }
            if (!bindingCalibrated) 
            {
                AccessTools.Method(typeof(InputManager), "UpdateBindingCache").Invoke(__instance, null);
                //__instance.UpdateBindingCache();
                bindingCalibrated = true;

            }
            
            //Plugin.LoggerInstance.LogInfo(Time.fixedDeltaTime);
            
        }
        public static IEnumerator Waiter2() {
            //Plugin.LoggerInstance.LogInfo("Coroutine 2");
            yield return new WaitForSecondsRealtime(1);
            Var.charControllerCalibrated = true;
            GameObject vrparent = GameObject.Find("VRParent");
            vrparent.transform.localPosition = new Vector3(vrparent.transform.localPosition.x, (float)Var.heightOffset, vrparent.transform.localPosition.z);
            #region Bindings
            if (!Var.isNotOculus)
            {
                InputManager.Instance.SetBinding(Var.lStickButton, Actions.Run, true);
                //InputManager.Instance.SetBinding(Var.acceptButton, Actions.AutoRun, true);
                //InputManager.Instance.SetBinding(KeyCode.UpArrow, InputManager.Actions.ToggleConsole, true);
                //InputManager.Instance.SetBinding(Var.acceptButton, InputManager.Actions.ActivateCenterObject, true);
                InputManager.Instance.SetBinding(Var.cancelButton, InputManager.Actions.Inventory, true);
                //InputManager.Instance.SetBinding(Var.gripButton, InputManager.Actions.RecastSpell, true);
                InputManager.Instance.SetBinding(Var.acceptButton, InputManager.Actions.RecastSpell, true);
                InputManager.Instance.SetBinding(Var.rStickButton, InputManager.Actions.CastSpell, true);
            }
            else
            {
                InputManager.Instance.SetBinding(KeyCode.Quote, Actions.Run, true);
                //InputManager.Instance.SetBinding(KeyCode.UpArrow, InputManager.Actions.ToggleConsole, true);
                InputManager.Instance.SetBinding(KeyCode.Quote, InputManager.Actions.ActivateCenterObject, true);
                InputManager.Instance.SetBinding(KeyCode.BackQuote, InputManager.Actions.Inventory, true);
                //InputManager.Instance.SetBinding(Var.gripButton, InputManager.Actions.RecastSpell, true);
                InputManager.Instance.SetBinding(KeyCode.DoubleQuote, InputManager.Actions.CastSpell, true);

            }
            #endregion
            Var.sword.SetActive(true);
            Var.dagger.SetActive(true);
            Var.battleaxe.SetActive(true);
            Var.elseA.SetActive(true);
            Var.mace.SetActive(true);
            Var.flail.SetActive(true);
            Var.hammer.SetActive(true);
            Var.staff.SetActive(true);
            Var.bow.SetActive(true);
        }
    }

    //[HarmonyPatch(typeof(DaggerfallInputMessageBox))]
    //public static class DaggerfallInputMessageBoxPatch 
    //{
    //    [HarmonyPrefix]
    //    public 
    //}

    //this patch fixes the orientation of most daggerfall billboards. 
    //[HarmonyPatch(typeof(DaggerfallBillboard), "Update")]
    //public class BillboardDirectionPatch:MonoBehaviour {
    //    [HarmonyPrefix]
    //    static void Prefix(DaggerfallBillboard __instance)
    //    {
    //        AccessTools.Field(typeof(DaggerfallBillboard),"mainCamera").SetValue(__instance,Var.VRCamera);


    //    }

    //}
    [HarmonyPatch(typeof(DaggerfallBillboard), "Start")]
    public class BillboardDirectionPatch : MonoBehaviour
    {
        [HarmonyPostfix]
        static void Postfix(DaggerfallBillboard __instance)
        {
            AccessTools.Field(typeof(DaggerfallBillboard), "mainCamera").SetValue(__instance, Var.VRCamera);


        }

    }

    //The Skybox hurts in the eyes as it's just a static image with a wrong depth. It's better to remove it for now
    [HarmonyPatch(typeof(DaggerfallSky), "OnPostRender")]
    public class SkyRemover
    {
        [HarmonyPrefix]
        static bool Prefix(DaggerfallSky __instance)
        {
            if (Var.skyboxToggle)
            {
                AccessTools.Method(typeof(DaggerfallSky), "UpdateSkyRects").Invoke(__instance, null);
                return false;
            }
            else
            {
                return true;
            }

        }
    }
    [HarmonyPatch(typeof(InputManager), "GetUIScrollMovement")]
    public class UIScrollPatch
    {
        [HarmonyPrefix]
        static bool Prefix(InputManager __instance, ref float __result)
        {
            //float vertical = Input.GetAxis("Axis5");
            //float vertical = Input.GetAxis(Var.rThumbStickVertical);
            var rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

            Vector2 lThumbStick;
            rightHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out lThumbStick);

            
            float vertical = lThumbStick.y;

            __result = vertical;
            return false;
        }
    }

    [HarmonyPatch(typeof(LevitateMotor), "Update")]
    public class LevitationPatch
    {
        [HarmonyPrefix]
        static void Prefix(LevitateMotor __instance) 
        {
            //float inputX1 = Input.GetAxis("Axis1");
            //float inputY1 = Input.GetAxis("Axis2");

            //float inputX1 = Input.GetAxis(Var.lThumbStickHorizontal);
            //float inputY1 = Input.GetAxis(Var.lThumbStickVertical);

            var leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

            Vector2 lThumbStick;
            leftHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out lThumbStick);

            float inputX1=lThumbStick.x;
            float inputY1=lThumbStick.y;

            PlayerMotor playerMotor=(PlayerMotor)AccessTools.Field(typeof(LevitateMotor),"playerMotor").GetValue(__instance);
            Camera playerCamera=Var.VRCamera;
            PlayerGroundMotor groundMotor =(PlayerGroundMotor)AccessTools.Field(typeof(LevitateMotor),"groundMotor").GetValue(__instance);

            if (inputX1 != 0.0f || inputY1 != 0.0f)
            {
                float inputModifyFactor = (inputX1 != 0.0f && inputY1 != 0.0f && playerMotor.limitDiagonalSpeed) ? .7071f : 1.0f;
                try
                {
                    AccessTools.Method(typeof(LevitateMotor), "AddMovement").Invoke(__instance, new object[] { playerCamera.transform.TransformDirection(new Vector3(inputX1 * inputModifyFactor, 0, inputY1 * inputModifyFactor)),false });
                }
                catch { Plugin.LoggerInstance.LogError("Error levitating"); }
                //AddMovement(playerCamera.transform.TransformDirection(new Vector3(inputX * inputModifyFactor, 0, inputY * inputModifyFactor)));
            }
            //groundMotor.MoveWithMovingPlatform((Vector3)AccessTools.Field(typeof(LevitateMotor), "moveDirection").GetValue(__instance));
            //Plugin.LoggerInstance.LogInfo((Vector3)AccessTools.Field(typeof(LevitateMotor), "moveDirection").GetValue(__instance));
            //AccessTools.Field(typeof(LevitateMotor), "moveDirection").SetValue(__instance,Vector3.zero);
            //Plugin.LoggerInstance.LogInfo((Vector3)AccessTools.Field(typeof(LevitateMotor), "moveDirection").GetValue(__instance));

            //moveDirection = Vector3.zero;


        }
    }
    //fixes Arrow and spell directions
    [HarmonyPatch(typeof(DaggerfallMissile), "GetAimDirection")]
    public class MissileDirectionPatch
    {
        [HarmonyPostfix]
        static void Postfix(DaggerfallMissile __instance, ref Vector3 __result)
        {
            
            if (__instance.CustomAimDirection != Vector3.zero)
            {
                __result = __instance.CustomAimDirection;
                return;
            }

            Vector3 aimDirection = Vector3.zero;
            DaggerfallEntityBehaviour caster = (DaggerfallEntityBehaviour)AccessTools.Field(typeof(DaggerfallMissile), "caster").GetValue(__instance);
            EnemySenses enemySenses = (EnemySenses)AccessTools.Field(typeof(DaggerfallMissile), "enemySenses").GetValue(__instance);

            if (caster == GameManager.Instance.PlayerEntityBehaviour)
            {
                aimDirection = Var.rightHand.transform.forward;
            }
            else if (enemySenses != null)
            {
                Vector3 predictedPosition;
                if (DaggerfallUnity.Settings.EnhancedCombatAI)
                    predictedPosition = enemySenses.PredictNextTargetPos(__instance.MovementSpeed);
                else
                    predictedPosition = enemySenses.LastKnownTargetPos;

                if (predictedPosition == EnemySenses.ResetPlayerPos)
                    aimDirection = caster.transform.forward;
                else
                    aimDirection = (predictedPosition - caster.transform.position).normalized;

                if (__instance.IsArrow && enemySenses.Target?.EntityType == EntityTypes.Player && GameManager.Instance.PlayerMotor.IsCrouching)
                    aimDirection += Vector3.down * 0.05f;
            }

           
            __result = aimDirection;
        }
    }
    //fixes Arrow and spell position
    [HarmonyPatch(typeof(DaggerfallMissile), "GetAimPosition")]
    public class MissilePositionPatch
    {
        [HarmonyPostfix]
        static void Postfix(DaggerfallMissile __instance, ref Vector3 __result)
        {
            DaggerfallEntityBehaviour caster = (DaggerfallEntityBehaviour)AccessTools.Field(typeof(DaggerfallMissile), "caster").GetValue(__instance);

            if (__instance.CustomAimPosition != Vector3.zero)
            {
                __result = __instance.CustomAimPosition;
                return;
            }

            Vector3 aimPosition = caster.transform.position;
            if (caster == GameManager.Instance.PlayerEntityBehaviour)
            {
                aimPosition = Var.rightHand.transform.position;
            }
            

            
            __result = aimPosition;
        }
    }

    [HarmonyPatch(typeof(DaggerfallMissile), "GetEntityTargetInTouchRange")]
    public class TouchSpellPatch
    {
        [HarmonyPrefix]
        static bool Prefix(DaggerfallMissile __instance, ref DaggerfallEntityBehaviour __result)
        {
            RaycastHit hit;

            Ray ray = new Ray(Var.rightHand.transform.position,Var.rightHand.transform.forward);
            if (Physics.SphereCast(ray, 0.25f, out hit, 3.0f))
                __result= hit.transform.GetComponent<DaggerfallEntityBehaviour>();
            else
                __result= null;

            return false;
        }
    }

    //makes stuff like doors interacteable
    [HarmonyPatch(typeof(PlayerActivate), "Update")]
    public class PlayerActivatePatch : MonoBehaviour 
    {
        [HarmonyPrefix]
        static void Prefix(PlayerActivate __instance)
        {
            //Plugin.LoggerInstance.LogInfo("EnteredGrippingPhase");
            //InputManager.Instance.SetBinding(KeyCode.UpArrow, InputManager.Actions.ToggleConsole, true);
            //InputManager.Instance.SetBinding(Var.acceptButton, InputManager.Actions.ActivateCenterObject, true);
            //InputManager.Instance.SetBinding(Var.cancelButton, InputManager.Actions.Inventory, true);
            
            AccessTools.Field(typeof(PlayerActivate), "mainCamera").SetValue(__instance, Var.handCam);

        }
    
    }
    //Sets reference for later use
    [HarmonyPatch(typeof(ClimbingMotor), "Start")]
    public class ClimbingMotorStartPatch
    {
        [HarmonyPostfix]
        static void Postfix(ClimbingMotor __instance) { Var.climbingMotor = __instance; }
    }
    //Sets reference for later use
    [HarmonyPatch(typeof(WeaponManager), "Start")]
    public class WeaponManagerStartPatch
    {
        [HarmonyPostfix]
        static void Postfix(WeaponManager __instance) { Var.weaponManager = __instance; }
    }
    //fixes bows
    [HarmonyPatch(typeof(WeaponManager), "Update")]
    public class BowPatch:MonoBehaviour
    {
        [HarmonyPostfix]
        static void Prefix(WeaponManager __instance) 
        {
            #region Fields
            PlayerEntity playerEntity =(PlayerEntity)AccessTools.Field(typeof(WeaponManager), "playerEntity").GetValue(__instance);
            DaggerfallUnityItem lastBowUsed = (DaggerfallUnityItem)AccessTools.Field(typeof(WeaponManager), "lastBowUsed").GetValue(__instance);
            bool usingRightHand = (bool)AccessTools.Field(typeof(WeaponManager), "usingRightHand").GetValue(__instance);
            DaggerfallUnityItem currentRightHandWeapon = (DaggerfallUnityItem)AccessTools.Field(typeof(WeaponManager), "currentRightHandWeapon").GetValue(__instance);
            DaggerfallUnityItem currentLeftHandWeapon = (DaggerfallUnityItem)AccessTools.Field(typeof(WeaponManager), "currentLeftHandWeapon").GetValue(__instance);
            #endregion
            if (!__instance.Sheathed) {
                if (__instance.ScreenWeapon.WeaponType == WeaponTypes.Bow)
                {
                    if (Input.GetKeyDown(Var.indexButton))
                    {
                        
                        DaggerfallMissile missile = Instantiate(__instance.ArrowMissilePrefab);
                        if (missile)
                        {
                            // Remove arrow
                            ItemCollection playerItems = playerEntity.Items;
                            DaggerfallUnityItem arrow = playerItems.GetItem(ItemGroups.Weapons, (int)Weapons.Arrow, allowQuestItem: false, priorityToConjured: true);
                            bool isArrowSummoned = arrow.IsSummoned;
                            playerItems.RemoveOne(arrow);

                            missile.Caster = GameManager.Instance.PlayerEntityBehaviour;
                            missile.TargetType = TargetTypes.SingleTargetAtRange;
                            missile.ElementType = ElementTypes.None;
                            missile.IsArrow = true;
                            missile.IsArrowSummoned = isArrowSummoned;
                            //Plugin.LoggerInstance.LogInfo("Bow almost used");
                            lastBowUsed = usingRightHand ? currentRightHandWeapon : currentLeftHandWeapon; ;
                            //Plugin.LoggerInstance.LogInfo("Bow used");
                        }
                    }
            } }
        
        }
    }
    //Grants skill points whenever the player hits an enemy
    [HarmonyPatch(typeof(WeaponManager), "WeaponDamage")]
    public class ExperiencePatch : MonoBehaviour
    {
        [HarmonyPostfix]
        static void Postfix(WeaponManager __instance)
        {
            
            PlayerEntity playerEntity = (PlayerEntity)AccessTools.Field(typeof(WeaponManager), "playerEntity").GetValue(__instance);
            DaggerfallUnityItem currentRightHandWeapon = (DaggerfallUnityItem)AccessTools.Field(typeof(WeaponManager), "currentRightHandWeapon").GetValue(__instance);
            DaggerfallUnityItem currentLeftHandWeapon = (DaggerfallUnityItem)AccessTools.Field(typeof(WeaponManager), "currentLeftHandWeapon").GetValue(__instance);
            bool usingRightHand = (bool)AccessTools.Field(typeof(WeaponManager), "usingRightHand").GetValue(__instance);
            if (__instance.ScreenWeapon.WeaponType == WeaponTypes.Melee || __instance.ScreenWeapon.WeaponType == WeaponTypes.Werecreature)
                playerEntity.TallySkill(DFCareer.Skills.HandToHand, 1);
            else if (usingRightHand && (currentRightHandWeapon != null))
                playerEntity.TallySkill(currentRightHandWeapon.GetWeaponSkillID(), 1);
            else if (currentLeftHandWeapon != null)
                playerEntity.TallySkill(currentLeftHandWeapon.GetWeaponSkillID(), 1);

            playerEntity.TallySkill(DFCareer.Skills.CriticalStrike, 1);
            //Plugin.LoggerInstance.LogInfo("Experience probably granted");
        }
    
    }


    //
    [HarmonyPatch(typeof(WeaponManager), "ToggleSheath")]
    public class CorrectWeaponPatch : MonoBehaviour
    {
        [HarmonyPrefix]
        static void Prefix(WeaponManager __instance)
        {
            
            //Plugin.LoggerInstance.LogInfo("Entered this method");
            if (__instance.Sheathed) 
            {
                Var.sheathObject.GetComponent<MeshRenderer>().enabled = true;
                Destroy(Var.weaponObject);
                Hands.rHand.SetActive(false);
                Hands.lHand.SetActive(false);
                GameObject tempObject=null;

                if (__instance.ScreenWeapon.WeaponType == WeaponTypes.LongBlade|| __instance.ScreenWeapon.WeaponType == WeaponTypes.LongBlade_Magic)
                {
                    //Plugin.LoggerInstance.LogInfo("We're here 1");
                    tempObject = Var.sword;
                    
                    //
                    //if (tempObject != null) { Plugin.LoggerInstance.LogInfo("found sword"); }




                }
                else if(__instance.ScreenWeapon.WeaponType==WeaponTypes.Dagger|| __instance.ScreenWeapon.WeaponType == WeaponTypes.Dagger_Magic|| __instance.ScreenWeapon.WeaponType == WeaponTypes.Dagger)
                {
                    //Plugin.LoggerInstance.LogInfo("We're here 2");
                    tempObject = Var.dagger;
                    //tempObject.transform.rotation = Quaternion.Euler(0, 90, 90);
                    
                    

                }
                else if(__instance.ScreenWeapon.WeaponType==WeaponTypes.Battleaxe|| __instance.ScreenWeapon.WeaponType == WeaponTypes.Battleaxe_Magic)
                {
                    //Plugin.LoggerInstance.LogInfo("We're here 3");
                    tempObject = Var.battleaxe;
                    //Var.dagger.transform.rotation = Quaternion.Euler(0, 90, -90);
                    //Var.weaponObject.transform.localPosition = Vector3.zero;
                    //Var.weaponObject.transform.localRotation = Quaternion.Euler(0, 180, 180);


                }
                else if(__instance.ScreenWeapon.WeaponType == WeaponTypes.Mace || __instance.ScreenWeapon.WeaponType == WeaponTypes.Mace_Magic||__instance.ScreenWeapon.WeaponType==WeaponTypes.Flail||__instance.ScreenWeapon.WeaponType==WeaponTypes.Flail_Magic)
                {
                    tempObject = Var.mace;

                }
                else if (__instance.ScreenWeapon.WeaponType == WeaponTypes.Bow)
                {
                    tempObject = Var.bow;
                    //AccessTools.Method(typeof(WeaponManager), "ToggleHand").Invoke(__instance, null);
                }

                else if(__instance.ScreenWeapon.WeaponType==WeaponTypes.Warhammer|| __instance.ScreenWeapon.WeaponType == WeaponTypes.Warhammer_Magic)
                {
                    tempObject=Var.hammer;
                }
                else if(__instance.ScreenWeapon.WeaponType == WeaponTypes.Staff || __instance.ScreenWeapon.WeaponType == WeaponTypes.Staff_Magic)
                {
                    //Plugin.LoggerInstance.LogInfo("Staff");
                    tempObject=Var.staff;
                }
                else if (__instance.ScreenWeapon.WeaponType == WeaponTypes.Melee)
                {
                    tempObject = Var.meleeHandR;
                }

                else
                {
                    //Plugin.LoggerInstance.LogInfo("We're here");
                    tempObject = Var.elseA;//If we don't find the appropriate wepaon type we give the user a placeholder weapon. This should never happen, unless I fucked up
                }
                //Plugin.LoggerInstance.LogInfo("Exited this method. Probably sucessfully");
                
                
                Hands.rHand.SetActive(false);
                Hands.lHand.SetActive(false);
                Var.weaponObject = Instantiate(tempObject);
                Var.weaponObject.GetComponent<Collider>().enabled = true;
                Var.weaponObject.transform.SetParent(Var.rightHand.transform);
                if (tempObject == Var.sword)
                {

                    Var.weaponObject.transform.localPosition = Vector3.zero;
                    Var.weaponObject.transform.localRotation = Quaternion.identity;

                }
                if (tempObject == Var.hammer)
                {

                    Var.weaponObject.transform.localPosition = Vector3.zero;
                    Var.weaponObject.transform.localRotation = Quaternion.Euler(0,0,90);

                }
                else if (tempObject == Var.dagger)
                {

                    Var.weaponObject.transform.localPosition = Vector3.zero;
                    Var.weaponObject.transform.localRotation = Quaternion.Euler(0, 90, 90);
                }
                else if (tempObject == Var.mace)
                {

                    //Var.weaponObject.transform.localPosition= Vector3.zero;
                    Var.weaponObject.transform.localPosition = new Vector3(0, 0, 0.1f);
                    Var.weaponObject.transform.localRotation = Quaternion.Euler(90, 0, 0);
                }
                else if (tempObject == Var.battleaxe)
                {

                    //Var.weaponObject.transform.localPosition= Vector3.zero;
                    Var.weaponObject.transform.localPosition = new Vector3(0, 0, 0);
                    Var.weaponObject.transform.localRotation = Quaternion.Euler(0, -90, 90);
                }
                else if (tempObject == Var.staff)
                {
                    Var.weaponObject.transform.localPosition = Vector3.zero;
                    Var.weaponObject.transform.localRotation = Quaternion.identity;
                }
                else if (tempObject == Var.bow)
                {
                    Var.weaponObject.transform.localPosition = Vector3.zero;
                    Var.weaponObject.transform.localRotation = Quaternion.Euler(270, 90, 0);
                }
                else if (tempObject == Var.meleeHandR)
                {
                    Var.weaponObject.transform.localPosition = new Vector3(0,0,-0.05f);

                    Var.weaponObject.transform.localRotation = Quaternion.Euler(20, 10, 270);
                }
                    //else if(tempObject==Var.)



                Var.weaponObject.SetActive(true);

            }
            //this sucks... but it'll do for now
            else
            {
                //Var.weaponObject.SetActive(false);
                //Var.weaponObject = null;
                if (Var.weaponObject != null)
                {
                    Var.weaponObject.GetComponent<Collider>().enabled = false;
                    Var.weaponObject.transform.SetParent(Var.sheathObject.transform);
                    Var.weaponObject.transform.localPosition = Vector3.zero;
                    if (__instance.ScreenWeapon.WeaponType == WeaponTypes.LongBlade || __instance.ScreenWeapon.WeaponType == WeaponTypes.LongBlade_Magic)
                    {
                        Var.sheathObject.GetComponent<MeshRenderer>().enabled = true;
                        Var.weaponObject.transform.localRotation = Quaternion.identity;
                    }
                    else if (__instance.ScreenWeapon.WeaponType == WeaponTypes.Dagger || __instance.ScreenWeapon.WeaponType == WeaponTypes.Dagger_Magic || __instance.ScreenWeapon.WeaponType == WeaponTypes.Dagger)
                    {
                        Var.sheathObject.GetComponent<MeshRenderer>().enabled = true;
                        Var.weaponObject.transform.localRotation = Quaternion.Euler(0, 90, 90);

                    }
                    else if (__instance.ScreenWeapon.WeaponType == WeaponTypes.Battleaxe || __instance.ScreenWeapon.WeaponType == WeaponTypes.Battleaxe_Magic)
                    {
                        Var.sheathObject.GetComponent<MeshRenderer>().enabled = false;
                        Var.weaponObject.transform.localRotation = Quaternion.Euler(0, 180, 180);
                    }
                    else if (__instance.ScreenWeapon.WeaponType == WeaponTypes.Mace || __instance.ScreenWeapon.WeaponType == WeaponTypes.Mace_Magic || __instance.ScreenWeapon.WeaponType == WeaponTypes.Flail_Magic || __instance.ScreenWeapon.WeaponType == WeaponTypes.Flail)
                    {
                        Var.sheathObject.GetComponent<MeshRenderer>().enabled = false;
                        Var.weaponObject.transform.localRotation = Quaternion.Euler(90, 0, 0);
                        Var.weaponObject.transform.localPosition = new Vector3(0, 0, 0.1f);
                    }
                    else if(__instance.ScreenWeapon.WeaponType == WeaponTypes.Warhammer || __instance.ScreenWeapon.WeaponType == WeaponTypes.Warhammer_Magic)
                    {
                        Var.sheathObject.GetComponent<MeshRenderer>().enabled = false;
                        Var.weaponObject.transform.localRotation = Quaternion.identity;

                    }
                    else if (__instance.ScreenWeapon.WeaponType == WeaponTypes.Staff || __instance.ScreenWeapon.WeaponType == WeaponTypes.Staff_Magic)
                    {
                        Var.sheathObject.GetComponent<MeshRenderer>().enabled = false;
                        Var.weaponObject.transform.localRotation = Quaternion.identity;
                    }
                    else if (__instance.ScreenWeapon.WeaponType == WeaponTypes.Bow)
                    {
                        Var.sheathObject.GetComponent<MeshRenderer>().enabled = false;
                        Var.weaponObject.transform.localRotation = Quaternion.Euler(270, 90, 0);

                    }
                    
                    //Var.weaponObject.transform.localRotation = Quaternion.identity;
                }
                Hands.rHand.SetActive(true);
                Hands.lHand.SetActive(true);

            }

            ////it didn't do for now D:
            //else
            //{
            //    //Var.weaponObject.SetActive(false);
            //    //Var.weaponObject = null;
            //    if (Var.weaponObject != null)
            //    {
            //        Var.weaponObject.transform.SetParent(Var.sheathObject.transform);
            //        Var.weaponObject.transform.localPosition = Vector3.zero;
            //        //Var.weaponObject.transform.localRotation = Quaternion.identity;
            //    }
            //    Hands.rHand.SetActive(true);
            //    Hands.lHand.SetActive(true);

            //}

        }

    }
    //fixes the billboard orientation of enemies
    [HarmonyPatch(typeof(DaggerfallMobileUnit), "Update")]
    public class UnitOrientationPatch : MonoBehaviour
    {
        [HarmonyPrefix]
        static void Prefix(DaggerfallMobileUnit __instance)
        {
            
            AccessTools.Field(typeof(DaggerfallMobileUnit), "mainCamera").SetValue(__instance, Var.VRCamera);

        }

    }




    //hard binds the thumbsticks to player movement. Makes movement smooth in all directions
    [HarmonyPatch(typeof(FrictionMotor), "GroundedMovement")]
    public class GroundedMovementPatch : MonoBehaviour
    {
        [HarmonyPrefix]
        static bool Prefix(FrictionMotor __instance, ref Vector3 moveDirection) 
        {
            #region Fields
            bool sliding = (bool)AccessTools.Field(typeof(FrictionMotor), "sliding").GetValue(__instance);
            RaycastHit hit = (RaycastHit)AccessTools.Field(typeof(FrictionMotor), "hit").GetValue(__instance);
            PlayerMotor playerMotor = (PlayerMotor)AccessTools.Field(typeof(FrictionMotor), "playerMotor").GetValue(__instance);
            bool playerControl = (bool)AccessTools.Field(typeof(FrictionMotor), "playerControl").GetValue(__instance);
            #endregion
            //SetSliding();
            AccessTools.Method(typeof(FrictionMotor), "SetSliding").Invoke(__instance, null);

            
            if ((sliding && __instance.slideWhenOverSlopeLimit) || (__instance.slideOnTaggedObjects && hit.collider.tag == "Slide"))
            {
                Vector3 hitNormal = hit.normal;
                moveDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
                Vector3.OrthoNormalize(ref hitNormal, ref moveDirection);
                moveDirection *= __instance.slideSpeed;
                AccessTools.Field(typeof(FrictionMotor), "playerControl").SetValue(__instance, false);
                playerControl = false;
            }
            
            else
            {

                //float inputX = Input.GetAxis("Axis1");
                //float inputY = Input.GetAxis("Axis2");
                //float inputX = Input.GetAxis(Var.lThumbStickHorizontal);
                //float inputY = Input.GetAxis(Var.lThumbStickVertical);

                var leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

                Vector2 lThumbStick;
                leftHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out lThumbStick);

                float inputX = lThumbStick.x;
                float inputY = lThumbStick.y;


                if (GameManager.Instance.PlayerEntity.IsParalyzed)
                {
                    inputX = 0;
                    inputY = 0;
                }
                GameObject vrcamera = GameObject.Find("VRCamera");
                //float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f && playerMotor.limitDiagonalSpeed) ? .7071f : 1.0f;
                //it's really disorienting in vr when the player moves faster in one direction than the other.
                float inputModifyFactor = 1.0f;
                moveDirection = new Vector3(inputX * inputModifyFactor, 0, inputY * inputModifyFactor);
                moveDirection = vrcamera.transform.TransformDirection(moveDirection) * playerMotor.Speed;
                AccessTools.Field(typeof(FrictionMotor), "playerControl").SetValue(__instance, true);
                playerControl = true;


                if (!GameManager.Instance.PlayerEntity.IsParalyzed)
                {
                    AccessTools.Method(typeof(FrictionMotor), "HeadDipHandling").Invoke(__instance, null);
                }
            }
            //Plugin.LoggerInstance.LogInfo($"sliding: {sliding}, hit: {hit}, playerControl: {playerControl}, moveDirection: {moveDirection}");
            return false;
            

        }
    }

    [HarmonyPatch(typeof(PlayerFootsteps),"FixedUpdate")]
    //this is to fix the annoying bug with an infinite amount of footstep sounds playing all at once
    public class FootstepPatch
    {
        public static float cooldown = 0.1f;
        private static float lastStepTime=0;
        [HarmonyPrefix]
        static bool Prefix(PlayerFootsteps __instance) 
        {
            if (Time.time - lastStepTime < cooldown)
            {
                return false;
            }
            lastStepTime = Time.time;
            return true;

        
        }
    }
    //fixes the automap orientation. Automap is currently not bound to any key though
    [HarmonyPatch(typeof(Automap), "UpdateAutomapStateOnWindowPush")]
    public class AutomapPatch : MonoBehaviour 
    {
        [HarmonyPrefix]
        static void Prefix(Automap __instance) 
        {
            #region Fields
            GameObject gameObjectPlayerAdvanced = (GameObject)AccessTools.Field(typeof(Automap), "gameObjectPlayerAdvanced").GetValue(__instance);
            GameObject gameobjectPlayerMarkerArrow = (GameObject)AccessTools.Field(typeof(Automap), "gameobjectPlayerMarkerArrow").GetValue(__instance);
            GameObject gameobjectBeaconPlayerPosition = (GameObject)AccessTools.Field(typeof(Automap), "gameobjectBeaconPlayerPosition").GetValue(__instance);
            Vector3 rayPlayerPosOffset = (Vector3)AccessTools.Field(typeof(Automap), "rayPlayerPosOffset").GetValue(__instance);
            #endregion
            AccessTools.Method(typeof(Automap), "CreateTeleporterMarkers").Invoke(__instance, null);
            //CreateTeleporterMarkers();
            AccessTools.Method(typeof(Automap), "SetActivationStateOfMapObjects").Invoke(__instance, new object[]{true});
            //SetActivationStateOfMapObjects(true);

            GameObject vrCamera = GameObject.Find("VRCamera");

            gameobjectPlayerMarkerArrow.transform.position = gameObjectPlayerAdvanced.transform.position;
            gameobjectPlayerMarkerArrow.transform.rotation = vrCamera.transform.rotation;

            gameobjectBeaconPlayerPosition.transform.position = gameObjectPlayerAdvanced.transform.position + rayPlayerPosOffset;

            // create camera (if not present) that will render automap level geometry
            AccessTools.Method(typeof(Automap), "CreateAutomapCamera").Invoke(__instance, null);
            //CreateAutomapCamera();

            // create lights that will light automap level geometry
            AccessTools.Method(typeof(Automap), "CreateLightsForAutomapGeometry").Invoke(__instance, null);
            //CreateLightsForAutomapGeometry();
            AccessTools.Method(typeof(Automap), "UpdateMicroMapTexture").Invoke(__instance, null);
            //UpdateMicroMapTexture();
            AccessTools.Method(typeof(Automap), "UpdateSlicingPositionY").Invoke(__instance, null);
            return;
            //UpdateSlicingPositionY();

        }
        
    
    }

    [HarmonyPatch(typeof(HUDCompass),"Update")]
    public class CompassPatch
    {
        [HarmonyPrefix]
        static void Prefix(HUDCompass __instance)
        {
            AccessTools.Field(typeof(HUDCompass), "compassCamera").SetValue(__instance, Var.VRCamera);

        }
    }
    //[HarmonyPatch(typeof(ClimbingMotor), "GetClimbedWallInfo")]
    //public static class Patch_GetClimbedWallInfo
    //{
    //    static void Postfix(ClimbingMotor __instance)
    //    {
    //        if (Var.VRCamera != null)
    //        {
    //            // Override myLedgeDirection only if it's zero (i.e., no wall detected)
    //            Vector3 currentDirection = (Vector3)AccessTools.Field(typeof(ClimbingMotor), "myLedgeDirection").GetValue(__instance);
    //            if (currentDirection == Vector3.zero)
    //            {
    //                AccessTools.Field(typeof(ClimbingMotor), "myLedgeDirection")
    //                    .SetValue(__instance, Var.VRCamera.transform.forward);
    //            }
    //        }
    //    }
    //}


    [HarmonyPatch(typeof(ClimbingMotor), "ClimbingCheck")]
    public class climbingCheckPatch : MonoBehaviour
    {
        //[HarmonyPostfix]
        //static void Postfix(ClimbingMotor __instance)
        //{
        //    // Get all private instance fields of type bool
        //    var boolFields = typeof(ClimbingMotor)
        //        .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
        //        .Where(field => field.FieldType == typeof(bool));

        //    foreach (var field in boolFields)
        //    {
        //        bool value = (bool)field.GetValue(__instance);
        //        Plugin.LoggerInstance.LogInfo($"{field.Name}: {value}");
        //    }
        //}
        [HarmonyPrefix]
        static bool Prefix(ClimbingMotor __instance)
        {
            try
            {
                bool isStanding = Var.VRCamera.transform.position.y > 1.5f;
                //Vector3 raycastOffset = Var.VRCamera.transform.position - new Vector3(0, isStanding ? 0 : 1, 0);
                //Vector3 raycastOffset = Var.VRCamera.transform.position - new Vector3(0, 1, 0);
                AccessTools.Field(typeof(ClimbingMotor), "myLedgeDirection").SetValue(__instance, Var.VRCamera.transform.forward);
                bool lGripButton;
                var leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                leftHand.TryGetFeatureValue(CommonUsages.gripButton, out lGripButton);
                //Plugin.LoggerInstance.LogInfo("entered patch");
                //AccessTools.Field(typeof(ClimbingMotor), "startClimbHorizontalTolerance").SetValue(__instance,1000f);
                ////Plugin.LoggerInstance.LogInfo(__instance.IsClimbing);
                CharacterController controller = (CharacterController)AccessTools.Field(typeof(ClimbingMotor), "controller").GetValue(__instance);
                PlayerMoveScanner moveScanner = (PlayerMoveScanner)AccessTools.Field(typeof(ClimbingMotor), "moveScanner").GetValue(__instance);
                PlayerMotor playerMotor = (PlayerMotor)AccessTools.Field(typeof(ClimbingMotor), "playerMotor").GetValue(__instance);
                Vector2 lastHorizontalPosition = (Vector2)AccessTools.Field(typeof(ClimbingMotor), "lastHorizontalPosition").GetValue(__instance);
                float startClimbHorizontalTolerance = (float)AccessTools.Field(typeof(ClimbingMotor), "startClimbHorizontalTolerance").GetValue(__instance);
                RappelMotor rappelMotor = (RappelMotor)AccessTools.Field(typeof(ClimbingMotor), "rappelMotor").GetValue(__instance);
                //////bool touchingSides = (controller.collisionFlags & CollisionFlags.Sides) != 0;
                //////Plugin.LoggerInstance.LogInfo($"touchingSides: {touchingSides}");
                //////Plugin.LoggerInstance.LogInfo(InputManager.Instance.HasAction(Actions.MoveForwards));
                ////Plugin.LoggerInstance.LogInfo((Vector2.Distance(lastHorizontalPosition, new Vector2(controller.transform.position.x, controller.transform.position.z))));
                ////Plugin.LoggerInstance.LogInfo(new Vector2(controller.transform.position.x, controller.transform.position.z));
                ////Plugin.LoggerInstance.LogError((Vector2.Distance(lastHorizontalPosition, new Vector2(controller.transform.position.x, controller.transform.position.z))) < startClimbHorizontalTolerance);
                //Plugin.LoggerInstance.LogInfo(Physics.Raycast(controller.transform.position, Vector3.down, controller.height / 2 + 0.12f));
                bool isClimbing = (bool)AccessTools.Field(typeof(ClimbingMotor), "isClimbing").GetValue(__instance);
                bool isSlipping = (bool)AccessTools.Field(typeof(ClimbingMotor), "isSlipping").GetValue(__instance);
                AcrobatMotor acrobatMotor = (AcrobatMotor)AccessTools.Field(typeof(ClimbingMotor), "acrobatMotor").GetValue(__instance);
                bool advancedClimbingOn = DaggerfallUnity.Settings.AdvancedClimbing;
                bool releasedFromCeiling = (bool)AccessTools.Field(typeof(ClimbingMotor), "releasedFromCeiling").GetValue(__instance);
                // true if we should try climbing wall and are airborne
                bool airborneGraspWall = (!isClimbing && !isSlipping && acrobatMotor.Falling);
                bool inputBack = InputManager.Instance.HasAction(InputManager.Actions.MoveBackwards);
                bool inputForward = lGripButton;//InputManager.Instance.HasAction(InputManager.Actions.MoveForwards);
                LevitateMotor levitateMotor = (LevitateMotor)AccessTools.Field(typeof(ClimbingMotor), "levitateMotor").GetValue(__instance);
                Vector3 wallDirection = (Vector3)AccessTools.Field(typeof(ClimbingMotor), "wallDirection").GetValue(__instance);
                //inputForward = true;

                // boolean that means ground directly below us is too close for climbing or rappelling
                bool tooCloseToGroundForClimb = (((isClimbing && (inputBack || isSlipping)) || airborneGraspWall)
                    // short circuit evaluate the raycast, also prevents bug where you could teleport across town
                    && Physics.Raycast(controller.transform.position, Vector3.down, controller.height / 2 + 0.12f));

                AccessTools.Method(typeof(ClimbingMotor), "CalcFrequencyAndToleranceOfWallChecks").Invoke(__instance, new object[] { airborneGraspWall });
                //CalcFrequencyAndToleranceOfWallChecks(airborneGraspWall);

                bool inputAbortCondition;
                if (advancedClimbingOn)
                {
                    inputAbortCondition = (InputManager.Instance.HasAction(InputManager.Actions.Crouch)
                                          || InputManager.Instance.HasAction(InputManager.Actions.Jump));
                }
                else
                    inputAbortCondition = !inputForward;

                // reset for next use
                //WallEject = false;
                AccessTools.Property(typeof(ClimbingMotor), "WallEject").SetValue(__instance, false);

                if (releasedFromCeiling)
                {
                    AccessTools.Method(typeof(ClimbingMotor), "MoveForwardIfNotClimbing").Invoke(__instance, null);
                    //MoveForwardIfNotClimbing();
                }
                if ((playerMotor.CollisionFlags & CollisionFlags.Sides) != 0)
                {
                    AccessTools.Field(typeof(ClimbingMotor), "releasedFromCeiling").SetValue(__instance, false);
                    releasedFromCeiling = false;
                }

                bool horizontallyStationary = Vector2.Distance(lastHorizontalPosition, new Vector2(controller.transform.position.x, controller.transform.position.z)) < startClimbHorizontalTolerance;
                //bool touchingSides = (playerMotor.CollisionFlags & CollisionFlags.Sides) != 0;
                bool touchingSides = (Physics.Raycast(Var.VRCamera.transform.position,Var.VRCamera.transform.forward,0.5f));
                bool touchingGround = (playerMotor.CollisionFlags & CollisionFlags.Below) != 0;
                //bool touchingAbove = (playerMotor.CollisionFlags & CollisionFlags.Above) != 0;
                bool slippedToGround = isSlipping && touchingGround;
                bool nonOrthogonalStart = !isClimbing && inputForward && !horizontallyStationary;
                //bool forwardStationaryNearCeiling = inputForward && hangingMotor.IsWithinHangingDistance && horizontallyStationary;
                bool pushingFaceAgainstWallNearCeiling = false;//hangingMotor.IsHanging && !isClimbing && touchingSides && forwardStationaryNearCeiling;
                bool climbingOrForwardOrGrasping = (isClimbing || inputForward || airborneGraspWall);
                bool hangTouchNonVertical = false;//hangingMotor.IsHanging && touchingSides && Physics.Raycast(controller.transform.position, controller.transform.forward, out hit, 0.40f) && Mathf.Abs(hit.normal.y) > 0.06f;
                AccessTools.Property(typeof(ClimbingMotor), "WallEject").SetValue(__instance, (inputBack && !moveScanner.HitSomethingInFront && moveScanner.FrontUnderCeiling != null));
                //ClimbQuitMoveUnderToHang = (inputBack && !moveScanner.HitSomethingInFront && moveScanner.FrontUnderCeiling != null);

                // Allow climbing slight overhangs when capsule hits above but upwards test rays have clear space overhead
                // Works by gently bumping player capsule away from wall at point of contact so they can acquire new vertical climb position
                // Provided angle not too extreme then player will keep climbing upwards or downwards
                // Allows player to climb up gently angled positions like the coffin tunnel in Scourg Barrow and up over shallow eaves
                if (isClimbing && (playerMotor.CollisionFlags & CollisionFlags.Above) == CollisionFlags.Above)
                {
                    //Vector3 frontTestPosition = controller.transform.position + wallDirection * controller.radius * 0.9f;
                    //Vector3 backTestPosition = controller.transform.position - wallDirection * controller.radius * 0.1f;
                    Vector3 frontTestPosition = Var.VRCamera.transform.position + wallDirection * controller.radius * 0.9f;
                    Vector3 backTestPosition = Var.VRCamera.transform.position - wallDirection * controller.radius * 0.1f;
                    //Debug.DrawLine(frontTestPosition, frontTestPosition + Vector3.up * 2, Color.red);
                    //Debug.DrawLine(backTestPosition, backTestPosition + Vector3.up * 2, Color.red);

                    // Test close to front of head for backward sloping wall like Scourg Barrow and bump a little backwards
                    // Then slightly further back for short overhangs like eaves and bump more backwards and a little upwards
                    // Height of raycast test is extended to help ensure there is clear space above not just an angled ceiling
                    //if (!Physics.Raycast(frontTestPosition, Vector3.up, controller.height / 2 + 0.3f))
                    //    controller.transform.position += -wallDirection * 0.1f;
                    //else if (!Physics.Raycast(backTestPosition, Vector3.up, controller.height / 2 + 0.5f))
                    //    controller.transform.position += -wallDirection * 0.4f + Vector3.up * 0.3f;
                    if (!Physics.Raycast(frontTestPosition, Vector3.up, controller.height / 2 + 0.3f))
                        Var.VRCamera.transform.position += -wallDirection * 0.1f;
                    else if (!Physics.Raycast(backTestPosition, Vector3.up, controller.height / 2 + 0.5f))
                        Var.VRCamera.transform.position += -wallDirection * 0.4f + Vector3.up * 0.3f;
                }

                // Handle recently restoring from save game where climbing active
                if (isClimbing && (bool)AccessTools.Field(typeof(ClimbingMotor), "isClimbing").GetValue(__instance) && !touchingSides)
                {
                    touchingSides = true;
                    //Debug.Log("Forced touchingSides...");
                }
                else
                    AccessTools.Field(typeof(ClimbingMotor), "isClimbing").SetValue(__instance, false);
                //touchingSidesRestoreForce = false;

                // Should we reset climbing starter timers?
                AccessTools.Field(typeof(ClimbingMotor), "wasClimbing").SetValue(__instance, isClimbing);
                //wasClimbing = isClimbing;
                //if (InputManager.Instance.HasAction(Actions.MoveForwards))
                //{
                //    Plugin.LoggerInstance.LogInfo("not pushing face and abort cond:" + (!pushingFaceAgainstWallNearCeiling && inputAbortCondition));
                //    Plugin.LoggerInstance.LogInfo("__instance.ClimbQuitMoveUnderToHang" + __instance.ClimbQuitMoveUnderToHang);
                //    Plugin.LoggerInstance.LogInfo("!climbingOrForwardOrGrasping" + !climbingOrForwardOrGrasping);
                //    Plugin.LoggerInstance.LogInfo("!touchingSides && !releasedFromCeiling" + (!touchingSides && !releasedFromCeiling));
                //    Plugin.LoggerInstance.LogInfo("levitateMotor.IsLevitating" + (levitateMotor.IsLevitating));
                //    Plugin.LoggerInstance.LogInfo("playerMotor.IsRiding" + (playerMotor.IsRiding));
                //    Plugin.LoggerInstance.LogInfo("slippedToGround " + (slippedToGround));
                //    Plugin.LoggerInstance.LogInfo("nonOrthogonalStart " + (nonOrthogonalStart));
                //    Plugin.LoggerInstance.LogInfo("hangTouchNonVertical " + (hangTouchNonVertical));
                //}
                if (!(Physics.Raycast(Var.VRCamera.transform.position-new Vector3(0,1,0), Var.VRCamera.transform.forward, 0.5f)))
                {
                    __instance.StopClimbing();
                    releasedFromCeiling = false;


                }
                if ((!pushingFaceAgainstWallNearCeiling)
                    &&
                    (inputAbortCondition
                    || __instance.ClimbQuitMoveUnderToHang
                    || !climbingOrForwardOrGrasping
                    || !touchingSides && !releasedFromCeiling
                    || !touchingSides
                    || levitateMotor.IsLevitating
                    || playerMotor.IsRiding
                    || slippedToGround
                    // quit climbing if climbing down and ground is really close, prevents teleportation bug
                    //|| tooCloseToGroundForClimb
                    // don't do horizontal position check if already climbing
                    //|| nonOrthogonalStart
                    // if we're hanging, and touching sides with a wall that isn't mostly vertical
                    || hangTouchNonVertical))
                {
                    //Plugin.LoggerInstance.LogInfo("Never even started.");
                    if (isClimbing && inputAbortCondition && advancedClimbingOn)
                        AccessTools.Property(typeof(ClimbingMotor), "WallEject").SetValue(__instance, true);

                    __instance.StopClimbing();
                    releasedFromCeiling = false;
                    // Reset position for horizontal distance check and timer to wait for climbing start
                    lastHorizontalPosition = new Vector2(controller.transform.position.x, controller.transform.position.z);
                }

                else // countdown climbing events
                {
                    // countdown to climbing start
                    if ((float)AccessTools.Field(typeof(ClimbingMotor), "climbingStartTimer").GetValue(__instance) <= (playerMotor.systemTimerUpdatesDivisor * (float)AccessTools.Field(typeof(ClimbingMotor), "startClimbSkillCheckFrequency").GetValue(__instance)))
                    {
                        float current = (float)AccessTools.Field(typeof(ClimbingMotor), "climbingStartTimer").GetValue(__instance);
                        AccessTools.Field(typeof(ClimbingMotor), "climbingStartTimer").SetValue(__instance, current + Time.deltaTime);
                        //climbingStartTimer += Time.deltaTime;
                    }
                    //if (InputManager.Instance.HasAction(InputManager.Actions.MoveForwards)) { Plugin.LoggerInstance.LogInfo("MF"); isClimbing = true; }

                    //if (InputManager.Instance.HasAction(InputManager.Actions.MoveForwards)) {  }
                    //// Begin Climbing
                    //else 
                    if (!isClimbing)
                    {
                        //if (hangingMotor.IsHanging)
                        //{   // grab wall from ceiling
                        //    overrideSkillCheck = true;
                        //    releasedFromCeiling = true;
                        //}

                        // automatic success if not falling
                        if ((!airborneGraspWall /*&& !hangingMotor.IsHanging*/) || releasedFromCeiling)
                        {
                            if (__instance.ClimbingSkillCheck(70))
                                AccessTools.Method(typeof(ClimbingMotor), "StartClimbing").Invoke(__instance, null);
                        } // skill check to see if we catch the wall 
                        else if (__instance.ClimbingSkillCheck(70))
                            AccessTools.Method(typeof(ClimbingMotor), "StartClimbing").Invoke(__instance, null);
                        else
                            AccessTools.Field(typeof(ClimbingMotor), "climbingStartTimer").SetValue(__instance, 0);
                    }

                    if (isClimbing)
                    {
                        float result;
                        if (isSlipping)
                        {

                            result = (float)AccessTools.Field(typeof(ClimbingMotor), "regainHoldSkillCheckFrequency").GetValue(__instance);
                        }
                        else
                        {
                            result = 15f;
                        }
                        // countdown to climb update, Faster updates if slipping
                        if ((float)AccessTools.Field(typeof(ClimbingMotor), "climbingContinueTimer").GetValue(__instance) <= (playerMotor.systemTimerUpdatesDivisor * (result)))
                        {
                            float current = (float)AccessTools.Field(typeof(ClimbingMotor), "climbingContinueTimer").GetValue(__instance);
                            AccessTools.Field(typeof(ClimbingMotor), "climbingContinueTimer").SetValue(__instance, current + Time.deltaTime);
                            //climbingContinueTimer += Time.deltaTime;
                        }

                        else
                        {
                            AccessTools.Field(typeof(ClimbingMotor), "climbingContinueTimer").SetValue(__instance, 0);
                            //climbingContinueTimer = 0;

                            // don't allow slipping if not moving.
                            //if (!InputManager.Instance.HasAction(InputManager.Actions.MoveForwards)
                            //        && !InputManager.Instance.HasAction(InputManager.Actions.MoveBackwards)
                            //        && !InputManager.Instance.HasAction(InputManager.Actions.MoveLeft)
                            //        && !InputManager.Instance.HasAction(InputManager.Actions.MoveRight))
                            //    isSlipping = false;
                            if (!lGripButton)
                                isSlipping = false;
                            // it's harder to regain hold while slipping than it is to continue climbing with a good hold on wall
                            else if (isSlipping)
                                isSlipping = !__instance.ClimbingSkillCheck(20);
                            else
                                isSlipping = !__instance.ClimbingSkillCheck(50);
                        }
                    }

                }

                // Climbing Cycle
                if (isClimbing)
                {
                    // evalate the ledge direction
                    AccessTools.Method(typeof(ClimbingMotor), "GetClimbedWallInfo").Invoke(__instance, null);
                    //GetClimbedWallInfo();
                    AccessTools.Method(typeof(ClimbingMotor), "ClimbMovement").Invoke(__instance, null);
                    //ClimbMovement();

                    // both variables represent similar situations, but different context
                    acrobatMotor.Falling = isSlipping;
                }
                else if (!rappelMotor.IsRappelling)
                {
                    isSlipping = false;
                    AccessTools.Field(typeof(ClimbingMotor), "overrideSkillCheck").SetValue(__instance, false);
                    AccessTools.Field(typeof(ClimbingMotor), "atOutsideCorner").SetValue(__instance, false);
                    AccessTools.Field(typeof(ClimbingMotor), "atInsideCorner").SetValue(__instance, false);
                    AccessTools.Field(typeof(ClimbingMotor), "showClimbingModeMessage").SetValue(__instance, true);
                    AccessTools.Field(typeof(ClimbingMotor), "moveDirection").SetValue(__instance, Vector3.zero);
                    //overrideSkillCheck = false;
                    //atOutsideCorner = false;
                    //atInsideCorner = false;
                    //showClimbingModeMessage = true;
                    //moveDirection = Vector3.zero;
                    // deletes locally and saves myLedgeDirection to variable in class
                    Vector3 myLedgeDirection = (Vector3)AccessTools.Field(typeof(ClimbingMotor), "myLedgeDirection").GetValue(__instance);
                    //if (myLedgeDirection != Vector3.zero)
                    //    moveScanner.CutAndPasteVectorToDetached(ref myLedgeDirection);
                }
                //Plugin.LoggerInstance.LogInfo(__instance.IsClimbing);
                return false;
            }
            catch (Exception e) { Plugin.LoggerInstance.LogError(e); return true; }


        }
    }
    //we don't want mouse controls
    [HarmonyPatch(typeof(PlayerMouseLook), "Update")]
    public class MousePatch : MonoBehaviour
    {
        [HarmonyPrefix]
        //We need to completely get rid of it.
        static bool Prefix(PlayerMouseLook __instance)
        {
            return false;
        
        }
    }
    //extra bindings because VR controllers don't have a lot of buttons
    [HarmonyPatch(typeof(InputManager), "FindKeyboardActions")]
    public class BindingPatch : MonoBehaviour
    {

        //private static bool isCrouching = false;
        [HarmonyPrefix]
        
        static void Prefix(InputManager __instance)
        {
            if (!ControllerPatch.isChanging)
            {
                List<Actions> currentActions = (List<Actions>)AccessTools.Field(typeof(InputManager), "currentActions").GetValue(__instance);
                //float inputY = Input.GetAxis("Axis5");

                //float inputY = Input.GetAxis(Var.rThumbStickVertical);

                var rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

                Vector2 rThumbStick;
                rightHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out rThumbStick);

                float inputX = rThumbStick.x;
                float inputY = rThumbStick.y;
                if (!Var.climbingMotor.IsClimbing)
                {
                    if (inputY >= 0.8f)
                    {
                        currentActions.Add(Actions.Jump);

                    }
                    else if (inputY <= -0.8f)
                    {
                        //currentActions.Add(Actions.Sneak);
                        currentActions.Add(Actions.Crouch);
                        PlayerMotor playerMotor = null;
                        try
                        {
                            playerMotor = GameObject.Find("PlayerAdvanced").GetComponent<PlayerMotor>();//Var.playerGameObject.GetComponent<PlayerMotor>();

                        }
                        catch { Plugin.LoggerInstance.LogError("PlayerMotorNotFound"); }
                        //if (playerMotor != null)
                        //{
                        //    Plugin.LoggerInstance.LogInfo(Var.playerGameObject.GetComponent<PlayerMotor>().IsCrouching);
                        //}
                        //else { Plugin.LoggerInstance.LogInfo("What the fuck is a player"); }
                        if (playerMotor.IsCrouching == true)
                        {
                            currentActions.Add(Actions.StealMode);

                        }
                        else
                        {
                            currentActions.Add(Actions.GrabMode);

                        }
                        //isCrouching=!isCrouching;
                        //
                        //
                    }
                }
                else
                {
                    if (inputY >= 0.5f)
                    {
                        currentActions.Add(Actions.MoveForwards);
                    }
                    else if (inputY <= -0.5f)
                    {
                        currentActions.Add(Actions.MoveBackwards);
                    }

                    if (inputX >= 0.5f)
                    {
                        currentActions.Add(Actions.MoveRight);
                    }
                    if (inputX <= -0.5f)
                    {
                        currentActions.Add(Actions.MoveLeft);
                    }


                }
                //if (Input.GetKeyDown(Var.gripButton)&&!ControllerPatch.flag) 
                //{ 
                //    currentActions.Add(Actions.ActivateCenterObject);

                //}
                bool gripButton;
                rightHand.TryGetFeatureValue(CommonUsages.gripButton, out gripButton);
                if (gripButton && !ControllerPatch.flag&&!Var.climbingMotor.IsClimbing)
                {
                    currentActions.Add(Actions.ActivateCenterObject);
                    //currentActions.Add(Actions.);

                }
                else if (gripButton && !ControllerPatch.flag && Var.climbingMotor.IsClimbing)
                {
                    currentActions.Add(Actions.Jump);
                }
                //if(Input.GetKeyDown(Var.acceptButton) && !ControllerPatch.flag)
                //{
                //    currentActions.Add(Actions.CharacterSheet);
                //}

                //if (Input.GetKeyDown(Var.lGripButton) && !ControllerPatch.flag)
                //{
                //    currentActions.Add(Actions.SwitchHand);
                //    GameObject.Find("PlayerAdvanced").GetComponent<WeaponManager>().ToggleSheath();
                //    GameObject.Find("PlayerAdvanced").GetComponent<WeaponManager>().ToggleSheath();
                //    GameObject.Find("PlayerAdvanced").GetComponent<WeaponManager>().ToggleSheath();
                //    GameObject.Find("PlayerAdvanced").GetComponent<WeaponManager>().ToggleSheath();
                //}
                var leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                if (ControllerPatch.flag)
                {
                    if (Input.GetKeyDown(Var.acceptButton))
                    {
                        currentActions.Add(Actions.TravelMap);
                    }
                    else if (Input.GetKeyDown(Var.cancelButton))
                    {
                        currentActions.Add(Actions.LogBook);
                    }
                    else if (Input.GetKeyDown(Var.gripButton))
                    {
                        currentActions.Add(Actions.Rest);
                    }
                    else if (Input.GetKeyDown(Var.lGripButton))
                    {
                        currentActions.Add(Actions.Transport);
                    }
                    if (Var.rTriggerDone)
                    {
                        currentActions.Add(Actions.AutoMap);

                    }

                    //var leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

                    bool leftTrigger;
                    leftHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out leftTrigger);

                    if (leftTrigger)
                    {
                        currentActions.Add(Actions.CharacterSheet);
                    }

                }

                //bool lGripButton;
                ////var leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                //leftHand.TryGetFeatureValue(CommonUsages.gripButton, out lGripButton);
                //if (lGripButton && !ControllerPatch.flag)
                //{

                //    //currentActions.Add(Actions.ActivateCenterObject);
                //    //if (!Var.climbingMotor.IsClimbing)
                //    //{
                //    //    Var.climbingMotor.ClimbingCheck();
                //    //}
                //    //if (!Var.climbingMotor.IsClimbing)
                //    //{
                //    //    currentActions.Add(Actions.MoveForwards);
                //    //}
                //    //else { currentActions.Add(Actions.MoveBackwards); }
                //    //Plugin.LoggerInstance.LogInfo("climbing");
                //    //currentActions.Add(Actions.);
                //    currentActions.Add(Actions.MoveForwards);

                //}



                if (Var.isNotOculus)
                {
                    bool acceptButton;
                    bool cancelButton;
                    bool primaryButton;
                    bool secondaryButton;

                    //var leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                    leftHand.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButton);
                    leftHand.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButton);
                    //bool lGripButton;

                    rightHand.TryGetFeatureValue(CommonUsages.primaryButton, out acceptButton);
                    rightHand.TryGetFeatureValue(CommonUsages.secondaryButton, out cancelButton);
                    //rightHand.TryGetFeatureValue(CommonUsages.gripButton, out gripButton);

                    if (ControllerPatch.flag)
                    {

                        if (acceptButton)
                        {
                            currentActions.Add(Actions.TravelMap);
                        }
                        else if (cancelButton)
                        {
                            currentActions.Add(Actions.LogBook);
                        }
                        else if (gripButton)
                        {
                            currentActions.Add(Actions.Rest);
                        }


                    }
                    else
                    {
                        //if (gripButton)
                        //{
                        //    currentActions.Add(Actions.ActivateCenterObject);
                        //}
                        if (!ControllerPatch.isChanging)
                        {
                            if (cancelButton && !ControllerPatch.flag)
                            {
                                currentActions.Add(Actions.Inventory);
                            }
                            if (secondaryButton && !ControllerPatch.flag) 
                            {
                                currentActions.Add(Actions.Escape);
                            }
                            if (primaryButton && !ControllerPatch.flag)
                            {
                                currentActions.Add(Actions.Escape);
                            }
                        }

                    }
                }
            }

        }
    }



    //fixes UI
    [HarmonyPatch(typeof(UserInterfaceRenderTarget), "CheckTargetTexture")]
    public class UIPatch : MonoBehaviour
    {
        [HarmonyPrefix]
        static void Prefix(UserInterfaceRenderTarget __instance)
        {
            #region Fields
            
            int customWidth = (int)AccessTools.Field(typeof(UserInterfaceRenderTarget), "customWidth").GetValue(__instance);
            int customHeight = (int)AccessTools.Field(typeof(UserInterfaceRenderTarget), "customHeight").GetValue(__instance);
            Vector2 targetSize = (Vector2)AccessTools.Field(typeof(UserInterfaceRenderTarget), "targetSize").GetValue(__instance);
            Panel parentPanel = (Panel)AccessTools.Field(typeof(UserInterfaceRenderTarget), "parentPanel").GetValue(__instance);
            RenderTexture targetTexture = (RenderTexture)AccessTools.Field(typeof(UserInterfaceRenderTarget), "targetTexture").GetValue(__instance);
            FilterMode filterMode = (FilterMode)AccessTools.Field(typeof(UserInterfaceRenderTarget), "filterMode").GetValue(__instance);
            int createCount = (int)AccessTools.Field(typeof(UserInterfaceRenderTarget), "createCount").GetValue(__instance);
            #endregion
            int width = (customWidth == 0) ? Screen.width : customWidth;
            int height = (customHeight == 0) ? Screen.height : customHeight;
            targetSize = new Vector2(width, height);

            
            float scaleX = (float)Screen.width / (float)customWidth;
            float scaleY = (float)Screen.height / (float)customHeight;
            parentPanel.RootSize = targetSize;
            parentPanel.Scale = new Vector2(scaleX, scaleY);
            parentPanel.AutoSize = AutoSizeModes.None;

            
            bool isReady = (bool)AccessTools.Method(typeof(UserInterfaceRenderTarget), "IsReady").Invoke(__instance, null);

            
            if (!isReady || targetTexture.width != width || targetTexture.height != height)
            {
                
                RenderTexture rTexture = GameObject.Find("DaggerfallUI").GetComponent<sTx>().sTxx;

                
                targetTexture = rTexture;
                targetTexture.filterMode = filterMode;
                targetTexture.name = string.Format("DaggerfallUI RenderTexture {0}", createCount++);
                targetTexture.Create();

                
                AccessTools.Field(typeof(UserInterfaceRenderTarget), "targetTexture").SetValue(__instance, targetTexture);
                AccessTools.Field(typeof(UserInterfaceRenderTarget), "createCount").SetValue(__instance, createCount);

                
                //Plugin.LoggerInstance.LogInfo($"Created UI RenderTexture with dimensions {width}, {height}");

                
                if (__instance.OutputImage)
                    __instance.OutputImage.texture = targetTexture;

                
                AccessTools.Method(typeof(UserInterfaceRenderTarget), "RaiseOnCreateTargetTexture").Invoke(__instance, null);
            }
        }
    }
    //Initialize the plugin
    [BepInPlugin("com.Lokius.DFUVR", "DFUVR", "0.5.5")]
    public class Plugin:BaseUnityPlugin
    {
        public static ManualLogSource LoggerInstance;
        void Awake()
        {
            Harmony harmony = new Harmony("com.Lokius.DFUVR");
            harmony.PatchAll();
            LoggerInstance = Logger;
            //LoggerInstance.LogInfo(Screen.currentResolution.refreshRate);
            //XRSettings.loadedDeviceName
            LoggerInstance.LogInfo(XRSettings.loadedDeviceName);
            LoggerInstance.LogInfo(XRSettings.eyeTextureResolutionScale);

            Haptics.TriggerHapticFeedback(XRNode.RightHand, 1f);
            Haptics.TriggerHapticFeedback(XRNode.LeftHand, 1f);

            //string[] joystickNames = Input.GetJoystickNames();
            //Plugin.LoggerInstance.LogInfo("Joysticks:"+joystickNames.Length);

            Var.Initialize();
            


            //Time.fixedDeltaTime = 1f / 80f;



        }
        


    }
}
