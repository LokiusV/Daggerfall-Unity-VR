using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DFUVR
{
    public class HandLabel : MonoBehaviour
    {
        public bool rightHand;
        void Start()
        {
            Plugin.LoggerInstance.LogInfo("added Handlabel");
        }

    }
}
