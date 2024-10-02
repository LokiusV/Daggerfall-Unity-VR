using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.XR;

namespace DFUVR
{
    public class Haptics
    {
        public static void TriggerHapticFeedback(XRNode hand, float length)
        {


            InputDevice device = InputDevices.GetDeviceAtXRNode(hand);

            //HapticCapabilities capabilities;
            try
            {
                device.SendHapticImpulse(0, 0.5f, length);
            }
            catch (Exception e)
            {
                Plugin.LoggerInstance.LogError("Failed to trigger haptics: "+e.Message);
            }

        }
    }
}
