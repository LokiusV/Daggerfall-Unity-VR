using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DaggerfallWorkshop;
namespace DFUVR
{
    public class SlotCollision : MonoBehaviour
    {
        public bool flag = false;
        public bool rightHand = true;
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<HandLabel>() != null)
            {
                //Plugin.LoggerInstance.LogInfo("Collision detected");

                if (other.gameObject.GetComponent<HandLabel>().isMainHand == false)
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
