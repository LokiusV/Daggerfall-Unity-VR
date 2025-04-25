using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;

namespace DFUVR
{
    public class SnapTurnProvider
    {
        //positive
        public static bool SnapRight()
        {
            //if (!Var.snapDone && Input.GetAxis("Axis4") >= 0.7f)
            //{
            //    Var.snapDone = true;
            //    return true;

            //}
            //if (Var.snapDone && Input.GetAxis("Axis4") <= 0.25f)
            //{
            //    Var.snapDone = false;
            //    return false;
            //}

            //return false;

            var rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

            Vector2 rThumbStick;
            rightHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out rThumbStick);

            float inputX = rThumbStick.x;
            float inputY = rThumbStick.y;


            if (!Var.snapDone && inputX >= 0.7f)
            {
                Var.snapDone = true;
                return true;

            }
            if (Var.snapDone && inputX <= 0.25f)
            {
                Var.snapDone = false;
                return false;
            }

            return false;

        }

        //negative
        public static bool SnapLeft()
        {
            var rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

            Vector2 rThumbStick;
            rightHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out rThumbStick);

            float inputX = rThumbStick.x;
            float inputY = rThumbStick.y;

            if (!Var.snapDone && inputX <= -0.7f)
            {
                Var.snapDone = true;
                return true;

            }
            if (Var.snapDone && inputX >= -0.25f)
            {
                Var.snapDone = false;
                return false;
            }
            return false;
            //if (!Var.snapDone && Input.GetAxis("Axis4") <= -0.7f)
            //{
            //    Var.snapDone = true;
            //    return true;

            //}
            //if (Var.snapDone && Input.GetAxis("Axis4") >= -0.25f)
            //{
            //    Var.snapDone = false;
            //    return false;
            //}
            //return false;

        }
        public static void Snap()
        {
            if (!Var.noTurn)
            {
                if (!Var.smoothTurn)
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
                else
                {
                    Var.snapCooldown = 0.0005f;
                    if (SnapRight())
                    {
                        if (!(Time.time - Var.lastSnapTime < Var.snapCooldown))
                        {
                            try
                            {
                                Var.characterController.gameObject.transform.Rotate(0, 2, 0);
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
                                Var.characterController.gameObject.transform.Rotate(0, -2, 0);
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


    }
}
