
using BepInEx;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace DFUVR
{
    public class sTx : MonoBehaviour
    {
        // Start is called before the first frame update
        public RenderTexture sTxx;
        void Start()
        {
            Plugin.LoggerInstance.LogInfo("!");
            string assetBundlePath = Path.Combine(Paths.PluginPath, "AssetBundles", "assetbundle1");


            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);

            RenderTexture rT = assetBundle.LoadAsset<RenderTexture>("Rtxt");
            


            assetBundle.Unload(false);
            sTxx = rT;//Resources.Load<RenderTexture>("Rtxt");
            Plugin.LoggerInstance.LogInfo(sTxx);

        }
        public IEnumerator UICal()
        {
            yield return new WaitForSecondsRealtime(1);
            this.StartCoroutine(UI.Calibrate());
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
