using DFUVR;
using System;
using UnityEngine;

[ExecuteAlways]
public class DebugColliders : MonoBehaviour
{
    private Mesh m_SphereMesh;
    private Mesh m_CapsuleMesh;
    private Mesh m_CubeMesh;


    private Material m_ColliderMaterial;

    // little hack to get the default shape meshes
    // note that the normal collider Gizmos use actually more simple drawings but -as this is just for debugging this should be enough 
    private void Awake()
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        m_CubeMesh = cube.GetComponent<MeshFilter>().sharedMesh;

        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        m_SphereMesh = sphere.GetComponent<MeshFilter>().sharedMesh;

        var capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        m_CapsuleMesh = capsule.GetComponent<MeshFilter>().sharedMesh;

        m_ColliderMaterial = Var.handObjects[DaggerfallWorkshop.WeaponTypes.LongBlade].gameObject.GetComponent<Material>();

        if (Application.isPlaying)
        {
            Destroy(sphere);
            Destroy(cube);
            Destroy(capsule);
        }
        else
        {
            DestroyImmediate(sphere);
            DestroyImmediate(cube);
            DestroyImmediate(capsule);
        }
    }

    private void Update()
    {
        // find all active colliders in scene
        var colliders = FindObjectsOfType<Collider>();

        // then draw their meshes
        foreach (var collider in colliders)
        {
            Mesh mesh;
            var colliderTransform = collider.transform;
            var matrix = colliderTransform.localToWorldMatrix;

            switch (collider)
            {
                case BoxCollider boxCollider:
                    {
                        // for a box we take the cube mesh and additionally translate and scale it according to the settings
                        mesh = m_CubeMesh;
                        matrix *= Matrix4x4.TRS(boxCollider.center, Quaternion.identity, boxCollider.size);
                        break;
                    }

                case SphereCollider sphereCollider:
                    {
                        // sphere is almot the same but uses the maximum of the scales as radius 
                        // then we add center and radius settings
                        mesh = m_SphereMesh;
                        var lossyScale = colliderTransform.lossyScale;
                        matrix = Matrix4x4.TRS(colliderTransform.position, colliderTransform.rotation, Vector3.one * Mathf.Max(lossyScale.x, lossyScale.y, lossyScale.z)) * Matrix4x4.TRS(sphereCollider.center, Quaternion.identity, 2 * sphereCollider.radius * Vector3.one);
                        break;
                    }

                case MeshCollider meshCollider:
                    {
                        // mesh is trivial
                        mesh = meshCollider.sharedMesh;
                        break;
                    }

                case CapsuleCollider capsuleCollider:
                    {
                        // capsule is a bit complex as they actually use a sphere, cylinder combo but this comes close enough
                        // might need some more work though
                        mesh = m_CapsuleMesh;
                        var radius = capsuleCollider.radius * 2;
                        var height = capsuleCollider.height > radius ? capsuleCollider.height : radius;
                        height *= 0.5f;
                        var size = new Vector3(radius, height, radius);
                        var rotation = Quaternion.identity;
                        if (capsuleCollider.direction == 1)
                        {
                            rotation = Quaternion.identity;
                        }
                        else if (capsuleCollider.direction == 2)
                        {
                            rotation = Quaternion.Euler(90, 0, 0);
                        }
                        else if (capsuleCollider.direction == 0)
                        {
                            rotation = Quaternion.Euler(0, 0, 90);

                        }
                        matrix *= Matrix4x4.TRS(capsuleCollider.center, rotation, size);
                        break;
                    }

                default:
                    continue;
            }

            Graphics.DrawMesh(mesh, matrix, m_ColliderMaterial, 0);
        }
    }
}