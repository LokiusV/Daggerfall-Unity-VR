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
    //fixes arrow spawn point
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

    //shows UI, recalibrates its position and repositions the player so that enemies can actually hit it
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
        static IEnumerator Waiter1()
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
        static void FixObstruction(GameObject vrui)
        {
            Transform vrCameraTransform = Var.VRCamera.transform;
            Ray ray = new Ray(vrCameraTransform.position, vrui.transform.position - vrCameraTransform.position);
            RaycastHit hit;

            
            if (Physics.Raycast(ray, out hit))
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

            //height calibration
            if (isChanging)
            {
                //ControllerPatch.flag = false;
                float input = Input.GetAxis("Axis5");
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
                Debug.Log("canceled");
                flag = false;
                isChanging = false;
                Var.SaveHeight();
            }

            //snap turning
            SnapTurnProvider.Snap();

            
            GameObject exterior = GameObject.Find("Exterior");

            //this atleast creates a sense of day and night
            if (exterior != null)
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
            InputManager.Instance.SetBinding(Var.lStickButton, Actions.Run, true);
            InputManager.Instance.SetBinding(KeyCode.UpArrow, InputManager.Actions.ToggleConsole, true);
            InputManager.Instance.SetBinding(Var.acceptButton, InputManager.Actions.ActivateCenterObject, true);
            InputManager.Instance.SetBinding(Var.cancelButton, InputManager.Actions.Inventory, true);
            InputManager.Instance.SetBinding(Var.gripButton, InputManager.Actions.RecastSpell, true);
            InputManager.Instance.SetBinding(Var.rStickButton, InputManager.Actions.CastSpell, true);
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
            AccessTools.Method(typeof(DaggerfallSky), "UpdateSkyRects").Invoke(__instance, null);
            return false;
            
        }
    }
    [HarmonyPatch(typeof(InputManager), "GetUIScrollMovement")]
    public class UIScrollPatch
    {
        [HarmonyPrefix]
        static bool Prefix(InputManager __instance, ref float __result)
        {
            float vertical = Input.GetAxis("Axis5");
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
            float inputX1 = Input.GetAxis("Axis1");
            float inputY1 = Input.GetAxis("Axis2");

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
                
                float inputX = Input.GetAxis("Axis1");
                float inputY = Input.GetAxis("Axis2");

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
                float inputY = Input.GetAxis("Axis5");
                if (inputY >= 0.5f)
                {
                    currentActions.Add(Actions.Jump);

                }
                else if (inputY <= -0.5f)
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
                    if (playerMotor.IsCrouching==true)
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
                if (Input.GetKeyDown(Var.gripButton)&&!ControllerPatch.flag) 
                { 
                    currentActions.Add(Actions.ActivateCenterObject);
                
                }
                //if (Input.GetKeyDown(Var.lGripButton) && !ControllerPatch.flag)
                //{
                //    currentActions.Add(Actions.SwitchHand);
                //    GameObject.Find("PlayerAdvanced").GetComponent<WeaponManager>().ToggleSheath();
                //    GameObject.Find("PlayerAdvanced").GetComponent<WeaponManager>().ToggleSheath();
                //    GameObject.Find("PlayerAdvanced").GetComponent<WeaponManager>().ToggleSheath();
                //    GameObject.Find("PlayerAdvanced").GetComponent<WeaponManager>().ToggleSheath();
                //}

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
    [BepInPlugin("com.Lokius.DFUVR", "DFUVR", "0.5.0")]
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
            Var.Initialize();
            

            //Time.fixedDeltaTime = 1f / 80f;



        }
        


    }
}
