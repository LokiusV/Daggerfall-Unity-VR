using BepInEx;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//using static DaggerfallConnect.Save.SaveVars;
namespace DFUVR
{
    public class HUDSpawner : MonoBehaviour
    {
        public RenderTexture healthTexture;
        public RenderTexture compassTexture;
        public RenderTexture notificationTexture;
        // Start is called before the first frame update
        void Start()
        {
            healthTexture = new RenderTexture(480, 270, 16, RenderTextureFormat.ARGB32);
            compassTexture = new RenderTexture(480, 270, 16, RenderTextureFormat.ARGB32);
            notificationTexture = new RenderTexture(480, 270, 16, RenderTextureFormat.ARGB32);

            GameObject parentObject = GameObject.Find("IdleParent");

            GameObject healthCameraObject = new GameObject("HealthCamera");
            GameObject compassCameraObject = new GameObject("CompassCamera");
            GameObject notificationCameraObject = new GameObject("notificationCameraObject");

            Camera healthCamera = healthCameraObject.AddComponent<Camera>();
            Camera compassCamera = compassCameraObject.AddComponent<Camera>();
            Camera notificationCamera = notificationCameraObject.AddComponent<Camera>();

            GameObject rightHand = GameObject.Find("RightHand");
            GameObject leftHand = GameObject.Find("LeftHand");

            GameObject healthObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
            healthObject.name = "HealthObject";

            GameObject compassObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
            compassObject.name = "CompassObject";

            compassObject.transform.SetParent(rightHand.transform);
            healthObject.transform.SetParent(leftHand.transform);

            healthObject.transform.localPosition = new Vector3(0.0183f, 0, -0.1673f);
            healthObject.transform.localRotation = Quaternion.Euler(0, -90, 0);
            healthObject.transform.localScale = new Vector3(0.1f, 0.06f, 0.1f);

            compassObject.transform.localPosition = new Vector3(-0.0199f, 0, -0.1097f);
            compassObject.transform.localRotation = Quaternion.Euler(0, 90, 0);
            compassObject.transform.localScale = new Vector3(0.1f, 0.04f, 0.1f);

            //healthObject.AddComponent<MeshFilter>();


            List<Camera> list = new List<Camera>() { healthCamera, compassCamera, notificationCamera };
            foreach (Camera c in list)
            {
                c.stereoTargetEye = StereoTargetEyeMask.None;
                c.gameObject.transform.SetParent(parentObject.transform);
                c.farClipPlane = 1f;
                c.useOcclusionCulling = false;
                c.clearFlags = CameraClearFlags.SolidColor;
            }

            healthCamera.fieldOfView = 23.3f;
            compassCamera.fieldOfView = 23.3f;
            notificationCamera.fieldOfView = 27.8f;
            //-0,7 -0,39 -0,697
            healthCameraObject.transform.localPosition = new Vector3(-0.7f, -0.39f, -0.697f);
            compassCameraObject.transform.localPosition = new Vector3(0.768f, -0.43f, -0.5210294f);
            notificationCameraObject.transform.localPosition = new Vector3(0, 0.403f, -0.474f);

            healthCamera.targetTexture = healthTexture;
            compassCamera.targetTexture = compassTexture;
            notificationCamera.targetTexture = notificationTexture;

            string pluginFolderPath = Paths.PluginPath;
            string assetBundlePath = Path.Combine(pluginFolderPath, "AssetBundles/assetbundle4");
            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);
            Shader shader;
            try
            {
                shader = assetBundle.LoadAsset<Shader>("NoBlack");
            }
            catch { Plugin.LoggerInstance.LogError("Didn't find shader. Reverting to standard Unlit");
                shader = Shader.Find("Unlit/Texture");
            }

            
            Material healthMaterial = new Material(shader);
            healthMaterial.mainTexture = healthTexture;

            Material compassMaterial = new Material(shader);
            compassMaterial.mainTexture = compassTexture;

            healthObject.GetComponent<MeshRenderer>().material = healthMaterial;
            compassObject.GetComponent<MeshRenderer>().material = compassMaterial;

            healthObject.GetComponent<MeshCollider>().enabled=false;
            compassObject.GetComponent<MeshCollider>().enabled=false;
            assetBundle.Unload(false);


        }

        
    }
}
