using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Block
{
    /// <summary>
    /// This component is added to blocks that is affected by physics (gravity, collision, etc.)
    ///
    /// A Rigidbody component is added if not, and the mass is automatically calculated based on the estimated mesh volume.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(MeshFilter))]
    public class VolumeBasedRigidbodyMass : MonoBehaviour
    {
        public float density = 10; // density of steel

        private Rigidbody _rigidbody;

        float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float v321 = p3.x * p2.y * p1.z;
            float v231 = p2.x * p3.y * p1.z;
            float v312 = p3.x * p1.y * p2.z;
            float v132 = p1.x * p3.y * p2.z;
            float v213 = p2.x * p1.y * p3.z;
            float v123 = p1.x * p2.y * p3.z;
            return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
        }

        // https://stackoverflow.com/questions/1406029/how-to-calculate-the-volume-of-a-3d-mesh-object-the-surface-of-which-is-made-up
        float VolumeOfMesh(Mesh mesh)
        {
            float volume = 0;
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                Vector3 p1 = vertices[triangles[i + 0]];
                Vector3 p2 = vertices[triangles[i + 1]];
                Vector3 p3 = vertices[triangles[i + 2]];
                volume += SignedVolumeOfTriangle(p1, p2, p3);
            }

            return Mathf.Abs(volume);
        }

        void Start()
        {
            Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

            // initialize the mass using the volume and density of the object
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.mass = VolumeOfMesh(mesh) * density;
        }
    }
}