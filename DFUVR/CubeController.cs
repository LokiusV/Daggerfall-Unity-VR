using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DFUVR
{
    public class CubeController : MonoBehaviour
    {
        public GameObject cube;
        private int changed;

        void Start()
        {
            cube=this.gameObject;
            changed=0;
        }

        void Update()
        {
            float inputX;
            float inputY;
            try
            {
                //cube.transform.localPosition
                inputX = Input.GetAxis("Joystick" + Var.calibrationInt + "Axis1");
                inputY = Input.GetAxis("Joystick" + Var.calibrationInt + "Axis2");
            }
            catch
            {
                Plugin.LoggerInstance.LogInfo("failed to find Joystick" + Var.calibrationInt + "Axis1 or 2");
                inputX = 0f;
                inputY = 0f;
            }
            //for (int joystick = 0; joystick <= 16; joystick++) // Test up to 4 joysticks
            //{
            //    for (int axis = 1; axis <= 16; axis++) // Test up to 8 axes per joystick
            //    {
            //        string axisName = $"Joystick{joystick}Axis{axis}";
            //        float value = Input.GetAxis(axisName);
            //        if (Mathf.Abs(value) > 0.01f)
            //        {
            //            Plugin.LoggerInstance.LogInfo($"{axisName} value: {value}");
            //        }
            //    }
            //}

            //Plugin.LoggerInstance.LogInfo("Joystick "+ Var.calibrationInt+" "+inputX);
            if (changed != Var.calibrationInt)
            { 
                changed = Var.calibrationInt;
                cube.transform.localPosition = Vector3.zero;
            
            }

            else
            {
                cube.transform.localPosition += new Vector3(inputX,inputY,0);
            }

        }
    }
}
