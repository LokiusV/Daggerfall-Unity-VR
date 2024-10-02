using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DFUVR
{
    public class DebugSphere:MonoBehaviour
    {
        public static void CreateVisualizer(SphereCollider sphereCollider)
        {
            // Create a new GameObject to represent the visual sphere
            GameObject sphereVisualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            // Make it semi-transparent by applying a transparent material
            Material transparentMaterial = new Material(Shader.Find("Standard"));
            transparentMaterial.color = new Color(0, 1, 0, 0.3f);  // Green and transparent
            sphereVisualizer.GetComponent<Renderer>().material = transparentMaterial;

            // Sync position and size with the SphereCollider
            UpdateVisualizer(sphereCollider, sphereVisualizer);

            // Optionally: Add a script to update it continuously during runtime
            sphereVisualizer.AddComponent<SphereColliderUpdater>().Initialize(sphereCollider, sphereVisualizer);
        }

        static void UpdateVisualizer(SphereCollider sphereCollider, GameObject sphereVisualizer)
        {
            // Set the position and size of the sphere visualizer to match the collider
            sphereVisualizer.transform.position = sphereCollider.transform.TransformPoint(sphereCollider.center);
            sphereVisualizer.transform.localScale = Vector3.one * sphereCollider.radius * 2;
        }

    }
    public class SphereColliderUpdater : MonoBehaviour
    {
        private SphereCollider sphereCollider;
        private GameObject sphereVisualizer;

        public void Initialize(SphereCollider collider, GameObject visualizer)
        {
            this.sphereCollider = collider;
            this.sphereVisualizer = visualizer;
        }

        void Update()
        {
            // Continuously update the position and size of the visualizer
            if (sphereCollider != null && sphereVisualizer != null)
            {
                sphereVisualizer.transform.position = sphereCollider.transform.TransformPoint(sphereCollider.center);
                sphereVisualizer.transform.localScale = Vector3.one * sphereCollider.radius * 2;
            }
        }
    }
}
