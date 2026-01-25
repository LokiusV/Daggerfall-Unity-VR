using UnityEngine;

namespace DFUVR
{
    public class SheathCollision : MonoBehaviour
    {
        public HandLabel handInside = null;

        void OnTriggerEnter(Collider other)
        {
            var handLabel = other.gameObject.GetComponent<HandLabel>();
            if (handLabel == null)
                return;

            Haptics.TriggerHapticFeedback(handLabel.xrHandNode, 0.6f);
            handInside = handLabel;

            //Plugin.LoggerInstance.LogInfo("Entered Collider");
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<HandLabel>() != null)
            {
                handInside = null;
                //Plugin.LoggerInstance.LogInfo("Exited Collider");
            }
        }
    }
}
