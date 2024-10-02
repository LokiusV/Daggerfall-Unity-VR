//using DaggerfallWorkshop.Game;
//using HarmonyLib;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace DFUVR
//{
//    public class CollisionFixMissile : MonoBehaviour
//    {
//        private Collider collider;
//        private GameObject missile;

//        void Start()
//        {
//            collider=this.gameObject.GetComponent<Collider>();
//            Plugin.LoggerInstance.LogInfo(collider.gameObject.name);
//            missile=this.gameObject;
//        }
//        void FixedUpdate()
//        {
//            //collider.transform.position= missile.transform.position;
//            //
//            Plugin.LoggerInstance.LogInfo(collider.transform.position);
//            Rigidbody rb = missile.GetComponent<Rigidbody>();
//            GameObject goModel=(GameObject)AccessTools.Field(typeof(DaggerfallMissile),"goModel").GetValue(missile.GetComponent<DaggerfallMissile>());

//            //missile.transform.localPosition=goModel.transform.position;
            
//            //if (rb != null)
//            //{
//            //    rb.MovePosition(missile.transform.position);
//            //    Var.debugSphere.transform.position = rb.transform.position;
//            //}
//            //else
//            //{
//            //    Plugin.LoggerInstance.LogInfo("AHHHHHH");
//            //    collider.transform.position = missile.transform.position;
//            //}
//            //Var.debugSphere.transform.position=collider.transform.position;

//        }
//    }
//}
