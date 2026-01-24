using UnityEngine;
using BepInEx;
using DaggerfallWorkshop.Game;
using UnityEngine.XR;
using static DaggerfallWorkshop.Game.InputManager;
using System.IO;
using System;

namespace DFUVR
{
    public class Slot : MonoBehaviour
    {
        public GameObject model;
        public GameObject sphere;
        public SphereCollider sphereCollider;
        Action<object[]> function = null;
        object[] args;
        public Actions action;
        public SlotCollision slotCollision;
        public int slotID = 0;

        public Vector3 location;
        public float xRot, yRot, zRot;

        private bool gripFlag = false;
        private bool alreadyGripped = false;
        private bool ready = false;

        //Constructor
        public void Init(GameObject model, Actions action, int slotID, Vector3 location, float xRot, float yRot, float zRot, object[] args, Action<object[]> function = null)
        {
            this.model = model;//TODO
            this.action = action;
            this.slotID = slotID;
            this.location = location;
            this.xRot = xRot;
            this.yRot = yRot;
            this.zRot = zRot;
            this.args = args;
            this.function = function;
            Init_2();
            ready = true;


        }
        void Start()
        {
            //Nothing
        }

        void Init_2()
        {
            Plugin.LoggerInstance.LogInfo("SlotStarted");
            sphere = new GameObject("Sphere" + slotID);
            sphere.transform.parent = GameObject.Find("Body").transform;
            if (GameObject.Find("Body") == null) { Plugin.LoggerInstance.LogError("Body not found. Are you a ghost?"); }
            //sphere.transform.Rotate(-231.724f, 0, 0);
            sphere.transform.Rotate(xRot, yRot, zRot);

            sphere.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            sphereCollider = sphere.AddComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = 0.25f;

            this.slotCollision = sphere.AddComponent<SlotCollision>();

            Rigidbody rb = sphere.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            //DebugSphere.CreateVisualizer(sphereCollider);

            sphere.transform.localPosition = location;


        }

        void Update()
        {
            if ((ready))
            {


                if (Var.isNotOculus)
                {
                    bool gripButton;
                    var rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                    if (Var.leftHanded) { rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand); }
                    rightHand.TryGetFeatureValue(CommonUsages.gripButton, out gripButton);
                    //rightHand.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButton);

                    if (gripButton) { gripFlag = true; }
                    if (!gripButton) { gripFlag = false; alreadyGripped = false; }
                    if ((gripFlag && slotCollision.flag) && !alreadyGripped)
                    {
                        alreadyGripped = true;
                        Var.actionList.Add(action);
                    }
                }
                if (Input.GetKeyDown(Var.gripButton))
                {
                    gripFlag = true;
                    //Plugin.LoggerInstance.LogInfo("Grip");
                }
                if (Input.GetKeyUp(Var.gripButton)) { gripFlag = false; alreadyGripped = false; }
                if ((gripFlag && slotCollision.flag) && !alreadyGripped)
                {
                    alreadyGripped = true;
                    //Plugin.LoggerInstance.LogInfo("Action!");
                    if (function != null)
                    {
                        function(args);
                    }
                    else
                    {
                        try { Var.actionList.Add(action); }
                        catch (Exception e) { Plugin.LoggerInstance.LogError(e); }
                        Plugin.LoggerInstance.LogInfo(Var.actionList.Count);
                    }



                }
            }
        }
        //Functions to better illustrate how slots work in SpawnHands.cs
        public static void TestFuncNoParams(object[] x)
        {
            Plugin.LoggerInstance.LogInfo($"testFuncNoParams");
        }
        public static void TestFuncWithParams(string x)
        {
            Plugin.LoggerInstance.LogInfo(x);
        }


    }
}
