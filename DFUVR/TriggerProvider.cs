using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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

                if (!pressedDone && Input.GetAxis("Axis3") >= 0.8f)
                {
                    Var.rTriggerDone = true;
                    pressedDone = true;
                    lastPressed = Time.time;
                //Plugin.LoggerInstance.LogInfo("pressedTrigger");
                    return true;

                }
            //}
            if (pressedDone && Input.GetAxis("Axis3") <= 0.3f)
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
