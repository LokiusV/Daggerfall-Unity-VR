
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Utility;
using HarmonyLib;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SpatialTracking;
using UnityEngine.UI;
using uWindowCapture;

namespace DFUVR
{
    public class SPC : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        private TrackedPoseDriver trackedPoseDriver;
        private GraphicRaycaster graphicRaycaster;

        public GraphicRaycaster raycaster;
        public float raycastDistance = 20f;

        // Window properties
        public int windowPosX;
        public int windowPosY;
        public int windowWidth;
        public int windowHeight;

        private UwcWindowTexture wTexture;

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        void Start()
        {

        }
        public void Initialize(LineRenderer line, GraphicRaycaster graphicRaycaster, TrackedPoseDriver tracked)
        {
            if (Var.isFirst)
            {
                wTexture = GameObject.Find("VRUI").GetComponent<UwcWindowTexture>();
                wTexture.type = WindowTextureType.Window;
                wTexture.partialWindowTitle = "Daggerfall";
            }
            else {
                //wTexture = GameObject.Find("VRUI").GetComponent<UwcWindowTexture>();
                windowHeight =Var.windowHeight;
                windowWidth=Var.windowWidth;
            
            }
            lineRenderer = line;
            trackedPoseDriver = tracked;
            this.graphicRaycaster = graphicRaycaster;
        }

        void Update()
        {

            //foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            //{
            //    if (Input.GetKey(kcode))
            //        Plugin.LoggerInstance.LogInfo("KeyCode down: " + kcode);
            //}
            //float input = Input.GetAxis("Axis" + Var.debugInt2);

            //Plugin.LoggerInstance.LogInfo("Input:" + input);
            //Plugin.LoggerInstance.LogInfo("Axis:" + Var.debugInt2);
            //Debug.Log(Var.heightOffset);

            //if (Input.GetKeyDown(Var.acceptButton)) { Var.debugInt2 += 1; }
            //Plugin.LoggerInstance.LogInfo(wTexture);
            //DaggerfallUI daggerfallUI = GameObject.Find("DaggerfallUI").GetComponent<DaggerfallUI>();
            //FieldInfo directionField = AccessTools.Field(typeof(DaggerfallUI), "customRenderTarget");
            //Plugin.LoggerInstance.LogInfo(directionField.GetValue(daggerfallUI));
            //if (Input.GetKeyDown(KeyCode.K))
            //{
            //    Plugin.LoggerInstance.LogInfo("Pressed");
            //    StartCoroutine(SpawnUI.Calibrate());
            //}
            //wTexture = GameObject.Find("VRUI").GetComponent<UwcWindowTexture>();
            //if (wTexture != null)
            //{
            //    windowPosX = wTexture.window.x;
            //    windowPosY = wTexture.window.y;
            //    windowWidth = wTexture.window.width;
            //    windowHeight = wTexture.window.height;
            //    Plugin.LoggerInstance.LogInfo(windowWidth+", "+windowHeight);
            //}

            TriggerProvider.CheckPressedRight();
            if (trackedPoseDriver != null && lineRenderer != null)
            {
                Vector3 controllerPosition = trackedPoseDriver.transform.localPosition;
                Quaternion controllerRotation = trackedPoseDriver.transform.localRotation;

                Vector3 lineStart = trackedPoseDriver.transform.position;
                Vector3 lineEnd = trackedPoseDriver.transform.position + trackedPoseDriver.transform.forward * 10f;

                lineRenderer.SetPosition(0, lineStart);
                lineRenderer.SetPosition(1, lineEnd);
                if (Input.GetKeyDown(Var.acceptButton)||Var.rTriggerDone||Var.lTriggerDone)
                {
                    if (!Var.isCalibrated)
                    {
                        var result = UwcWindowTexture.RayCast(lineStart, trackedPoseDriver.transform.forward, raycastDistance, -1);
                        if (result.hit)
                        {
                            //uWindowCapture really needs a proper documentation. I wish I knew sooner that it had a precise raycast function built in before making one myself...
                            //windowCoord = result.windowCoord;
                            Vector2 desktopCoord = result.desktopCoord;
                            SetCursorPos((int)desktopCoord.x, (int)desktopCoord.y);
                            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)desktopCoord.x, (uint)desktopCoord.y, 0, 0);
                            mouse_event(MOUSEEVENTF_LEFTUP, (uint)desktopCoord.x, (uint)desktopCoord.y, 0, 0);
                        }
                    }
                    else
                    {
                        //RaycastHit hit;

                        //if (Physics.Raycast(lineStart, trackedPoseDriver.transform.forward, out hit, raycastDistance))
                        //{
                        //    if (hit.collider.gameObject.name == "VRUI")
                        //    {
                        //        if (Input.GetKeyDown(Var.acceptButton))
                        //        {
                        //            Plugin.LoggerInstance.LogInfo("Raycast Hit: " + hit.point);
                        //            SimulateMouseClick(hit.point);
                        //        }
                        //    }
                        //    if (Input.GetKeyDown(Var.acceptButton))
                        //    {
                        //        if (hit.collider.gameObject.name != "VRUI")
                        //        { Plugin.LoggerInstance.LogInfo("Hit " + hit.collider.gameObject.name); }
                        //    }
                        //}
                        RaycastHit[] hits = Physics.RaycastAll(lineStart, trackedPoseDriver.transform.forward, raycastDistance);

                        foreach (var hit in hits)
                        {
                            if (hit.collider.gameObject.name == "VRUI")
                            {
                                Plugin.LoggerInstance.LogInfo("Raycast Hit: " + hit.point);
                                SimulateMouseClick(hit.point);
                                break;
                            }

                            if(hit.collider.gameObject.GetComponent<Button>() != null)
                            {
                                hit.collider.gameObject.GetComponent<Button>().onClick.Invoke();
                            }

                        }
                        }
                    }
            }


        }




        //deprecated
        private void SimulateMouseClick(Vector3 hitPoint)
        {


            GameObject vrui = GameObject.Find("VRUI");
            Vector3 localHitPoint = vrui.transform.InverseTransformPoint(hitPoint);
            Plugin.LoggerInstance.LogInfo($"Local Hit Point: {localHitPoint}");
            Plugin.LoggerInstance.LogInfo("Width: " + windowWidth);
            Plugin.LoggerInstance.LogInfo("Height: " + windowHeight);
            double asp = (double)windowWidth / windowHeight;
            Plugin.LoggerInstance.LogInfo("Aspect ratio: " + asp);

            double normalizedX = (localHitPoint.x * asp + (vrui.transform.localScale.x * 0.5f)) / vrui.transform.localScale.x;//(1.77f)
            double normalizedY = (localHitPoint.y + (vrui.transform.localScale.y * 0.5f)) / vrui.transform.localScale.y;
            //double normalizedX = (localHitPoint.x * 1.60f + (vrui.transform.localScale.x * 0.5f)) / vrui.transform.localScale.x;//(1.77f)
            //double normalizedY = (localHitPoint.y *0.9f + (vrui.transform.localScale.y * 0.5f)) / vrui.transform.localScale.y;
            Plugin.LoggerInstance.LogInfo($"Normalized Hit Point: ({normalizedX}, {normalizedY})");


            int screenX = (int)(normalizedX * windowWidth); //+ windowPosX;
            int screenY = (int)((1 - normalizedY) * windowHeight);// + windowPosY; 
            Plugin.LoggerInstance.LogInfo($"Screen Coordinates: ({screenX}, {screenY})");


            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)screenX, (uint)screenY, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, (uint)screenX, (uint)screenY, 0, 0);
            SetCursorPos((int)screenX, (int)screenY);
            //Plugin.LoggerInstance.LogInfo($"Simulated mouse click at ({screenX}, {screenY})");
        }
    }
}



