﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DFUVR
{
    public class SnapTurnProvider
    {
        //positive
        public static bool SnapRight()
        {
            if (!Var.snapDone && Input.GetAxis("Axis4") >= 0.7f)
            {
                Var.snapDone = true;
                return true;

            }
            if (Var.snapDone && Input.GetAxis("Axis4") <= 0.25f)
            {
                Var.snapDone = false;
                return false;
            }

            return false;

        }

        //negative
        public static bool SnapLeft()
        {
            if (!Var.snapDone && Input.GetAxis("Axis4") <= -0.7f)
            {
                Var.snapDone = true;
                return true;

            }
            if (Var.snapDone && Input.GetAxis("Axis4") >= -0.25f)
            {
                Var.snapDone = false;
                return false;
            }
            return false;

        }
        public static void Snap()
        {
            if (SnapRight())
            {
                if (!(Time.time - Var.lastSnapTime < Var.snapCooldown))
                {
                    try
                    {
                        Var.characterController.gameObject.transform.Rotate(0, 45, 0);
                        //GameObject.Find("VRParent").transform.Rotate(0, 45, 0);
                        Var.lastSnapTime = Time.time;

                        //experimental sheath fix:
                        BodyRotationController bodyRotationController = Var.characterController.GetComponent<BodyRotationController>();
                        if (bodyRotationController != null)
                        {
                            bodyRotationController.ResetLastRotationY();
                        }
                    }
                    catch (Exception e) { Debug.LogException(e); }
                }
            }
            if (SnapLeft())
            {
                if (!(Time.time - Var.lastSnapTime < Var.snapCooldown))
                {
                    try
                    {
                        Var.characterController.gameObject.transform.Rotate(0, -45, 0);
                        //GameObject.Find("VRParent").transform.Rotate(0, -45, 0);
                        Var.lastSnapTime = Time.time;
                        //experimental sheath fix:
                        BodyRotationController bodyRotationController = Var.characterController.GetComponent<BodyRotationController>();
                        if (bodyRotationController != null)
                        {
                            bodyRotationController.ResetLastRotationY();
                        }
                    }
                    catch (Exception e) { Debug.LogException(e); }
                }
            }

        }


    }
}