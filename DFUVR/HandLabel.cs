using UnityEngine;
using UnityEngine.XR;

namespace DFUVR
{
    public class HandLabel : MonoBehaviour
    {
        public bool isMainHand;
        public XRNode xrHandNode;
        public InputDevice inputDevice;
        public KeyCode grabButton;

        public void Init(bool isMainHand, XRNode xrHandNode, InputDevice inputDevice, KeyCode grabButton)
        {
            this.isMainHand = isMainHand;
            this.xrHandNode = xrHandNode;
            this.inputDevice = inputDevice;
            this.grabButton = grabButton;
        }

        void Start()
        {
            Plugin.LoggerInstance.LogInfo("added Handlabel");
        }
    }
}
