using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace DFUVR
{
    public class SheathCollision : MonoBehaviour
    {
        public Dictionary<XRNode, HandLabel> handsInside = new Dictionary<XRNode, HandLabel>(2);

        public SheathCollision()
        {
            handsInside.Add(XRNode.LeftHand, null);
            handsInside.Add(XRNode.RightHand, null);
        }

        void OnTriggerEnter(Collider other)
        {
            var handLabel = other.gameObject.GetComponent<HandLabel>();
            if (handLabel == null)
                return;

            Haptics.TriggerHapticFeedback(handLabel.xrHandNode, 0.6f);
            handsInside[handLabel.xrHandNode] = handLabel;

            //Plugin.LoggerInstance.LogInfo("Entered Collider");
        }

        void OnTriggerExit(Collider other)
        {
            var handLabel = other.gameObject.GetComponent<HandLabel>();
            if (handLabel == null)
                return;

            handsInside[handLabel.xrHandNode] = null;

            //Plugin.LoggerInstance.LogInfo("Exited Collider");
        }
    }
}
