using System.Collections;
using System.Collections.Generic;
using ProceduralPrimitives;
using UnityEngine;

public class TestProceduralCylinder : MonoBehaviour
{
    void Start()
    {
        GameObject go = new GameObject();
        go.CreateCylinder(0.5f, 1f, 1, 10, 2);

        GameObject go1 = new GameObject();
        go1.CreateCylinder(0.3f, 0.3f, 10, 30, 2);
        go1.transform.Rotate(new Vector3(45, 45, 0), Space.Self);
        go1.transform.Translate(new Vector3(-2, 0, 0), Space.Self);

        go1.AddComponent<PhysicalBlockBase>();
        MeshCollider meshCollider = go1.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = go1.GetComponent<MeshFilter>().sharedMesh;
        meshCollider.convex = true;
    }
}