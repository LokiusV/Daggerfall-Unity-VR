using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;

namespace DFUVR
{
    public class TriggerProvider
    {
        public static float coolDown=0.2f;
        public static float lastPressed;
        public static bool pressedDone;

        public static bool CheckPressedRight()
        {
            //Var.rTriggerDone = false;
            //if (Time.time - lastPressed >= coolDown)
            //{
            //Input.GetAxis("Axis3")
            //    if (!pressedDone && Input.GetAxis(Var.triggers) >= 0.8f)
            //    {
            //        Var.rTriggerDone = true;
            //        pressedDone = true;
            //        lastPressed = Time.time;
            //    //Plugin.LoggerInstance.LogInfo("pressedTrigger");
            //        return true;

            //    }
            ////}
            //if (pressedDone && Input.GetAxis(Var.triggers) <= 0.3f)
            //{
            //    Var.rTriggerDone = false;
            //    pressedDone = false;
            //    return false;
            //}
            //Var.rTriggerDone = false;
            //return false;
            var rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            if (Var.leftHanded) { rightHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand); }
            float triggerPressed;
            rightHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out triggerPressed);

            

            if (!pressedDone && triggerPressed >= 0.8f)
            {
                Var.rTriggerDone = true;
                pressedDone = true;
                lastPressed = Time.time;
                //Plugin.LoggerInstance.LogInfo("pressedTrigger");
                return true;

            }
            //}
            if (pressedDone && triggerPressed <= 0.3f)
            {
                Var.rTriggerDone = false;
                pressedDone = false;
                return false;
            }
            Var.rTriggerDone = false;
            return false;

            //return false;

        }
    }

}
