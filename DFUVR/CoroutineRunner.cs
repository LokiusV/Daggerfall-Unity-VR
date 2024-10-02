using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DFUVR
{
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner __instance;

        public static CoroutineRunner Instance
        {
            get
            {
                if (__instance == null)
                {
                    
                    GameObject runnerObject = new GameObject("CoroutineRunner");
                    __instance = runnerObject.AddComponent<CoroutineRunner>();
                    //runnerObject.AddComponent<DebugColliders>();
                    DontDestroyOnLoad(runnerObject);
                }
                return __instance;
            }
        }

        public void StartRoutine(IEnumerator routine)
        {
            StartCoroutine(routine);
        }
    }
}
