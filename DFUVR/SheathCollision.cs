using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DaggerfallWorkshop;
namespace DFUVR
{
    public class SheathCollision:MonoBehaviour
    {
        public static bool flag=false;
        public static bool rightHand=true;
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<HandLabel>() != null) 
            { 
                
                if (other.gameObject.GetComponent<HandLabel>().rightHand == false)
                {
                    Haptics.TriggerHapticFeedback(UnityEngine.XR.XRNode.LeftHand, 0.6f);
                }
                else
                {
                    Haptics.TriggerHapticFeedback(UnityEngine.XR.XRNode.RightHand, 0.6f);
                    flag = true;

                }

                //Plugin.LoggerInstance.LogInfo("Entered Collider");

            }


        }
        void OnTriggerExit(Collider other) 
        {
            if (other.gameObject.GetComponent<HandLabel>() != null)
            {
                flag = false;
                //Plugin.LoggerInstance.LogInfo("Exited Collider");

            }

        }
    }
}
