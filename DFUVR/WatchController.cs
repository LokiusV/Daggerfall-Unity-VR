using DaggerfallWorkshop;
using DaggerfallWorkshop.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace DFUVR
{
    public class WatchController: MonoBehaviour
    {
        public static TextMesh text;
        public static DaggerfallDateTime daggerfallDateTime;
        void Start()
        {
            //For when the game isn't properly initialized yet(for example in the main menu)
            //try
            //{
            //    text = GameObject.Find("Watch").GetComponent<TextMesh>();
            //    daggerfallDateTime = GameObject.Find("DaggerfallUnity").GetComponent<WorldTime>().DaggerfallDateTime;
            //}
            //catch (Exception e) { Plugin.LoggerInstance.LogError(e.ToString()); }



        }

        // Update is called once per frame
        void Update()
        {
            try
            {
                if (daggerfallDateTime == null) { daggerfallDateTime = GameObject.Find("DaggerfallUnity").GetComponent<WorldTime>().DaggerfallDateTime; }
                if(text == null) { text = this.gameObject.GetComponent<TextMesh>(); }
                text.text = daggerfallDateTime.ShortTimeString();
                //Plugin.LoggerInstance.LogInfo(text.text);
                //Debug.Log(GameObject.Find("DaggerfallUnity").GetComponent<WorldTime>().DaggerfallDateTime.LongDateTimeString());
            }
            catch (Exception e) {

                Plugin.LoggerInstance.LogError(e.ToString());
            }

            //Plugin.LoggerInstance.LogInfo(text.text);

        }
    }
}
