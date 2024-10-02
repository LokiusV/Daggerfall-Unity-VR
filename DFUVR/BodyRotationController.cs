using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DFUVR
{
    

    public class BodyRotationController : MonoBehaviour
    {
        public Transform vrCamera; 
        public Transform body;
        public float rotationSpeed = 2.5f; 

        private float lastRotationY = 0f; 
        private float bodyTargetRotationY = 0f;
        private Quaternion targetRotation;

        void Start()
        {
            vrCamera=Var.VRCamera.transform;
            body=GameObject.Find("Body").transform;
            body.transform.parent = GameObject.Find("VRParent").transform;
            //body.transform.localPosition=new Vector3(body.transform.localPosition.x,GameObject.Find(),0);
            lastRotationY = vrCamera.eulerAngles.y;
        }

        void Update()
        {
            body.transform.position = new Vector3(vrCamera.transform.position.x, vrCamera.transform.position.y - 0.6f, vrCamera.transform.position.z);
            //Plugin.LoggerInstance.LogInfo("UwU");
            //thx chatgpt
            // Calculate the current rotation difference
            float currentRotationY = vrCamera.eulerAngles.y;
            float rotationDifference = Mathf.DeltaAngle(lastRotationY, currentRotationY);

            // Check if the rotation difference has surpassed 45 degrees
            if (Mathf.Abs(rotationDifference) >= 90f)
            {
                //// Update the target rotation for the body
                //bodyTargetRotationY += Mathf.Sign(rotationDifference) * 90f;
                //lastRotationY = currentRotationY; // Update the last rotation reference

                Vector3 forwardDirection = vrCamera.forward;
                forwardDirection.y = 0;  // Ignore vertical component (Y-axis)

                targetRotation = Quaternion.LookRotation(forwardDirection);  // Set the new target rotation

                // Update the last rotation Y reference to the current camera rotation
                lastRotationY = currentRotationY;
            }

            // Smoothly rotate the body towards the target rotation
            //body.rotation = Quaternion.Lerp(
            //    body.rotation,
            //    Quaternion.Euler(0, bodyTargetRotationY, 0),
            //    Time.deltaTime * rotationSpeed
            //);
            body.rotation = Quaternion.Lerp(
                body.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }

        public void ResetRotation()
        {
            Vector3 forwardDirection = vrCamera.forward;
            forwardDirection.y = 0;  // Ignore vertical component (Y-axis)
 

            body.rotation = Quaternion.LookRotation(forwardDirection);
        }
        public void ResetLastRotationY()
        {
            lastRotationY = Var.characterController.gameObject.transform.eulerAngles.y;
        }
    }


}
