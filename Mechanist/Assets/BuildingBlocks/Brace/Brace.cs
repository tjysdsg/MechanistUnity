using System;
using ProceduralPrimitives;
using ScriptableObjectArchitecture;
using UnityEngine;

[ExecuteInEditMode]
public class Brace : MonoBehaviour
{
    [SerializeField] private float length;

    private ProceduralCylinder _proceduralCylinder;
    private MeshCollider _meshCollider;
    private MeshFilter _meshFilter;

    void Start()
    {
        if (_proceduralCylinder == null)
            _proceduralCylinder = new ProceduralCylinder(0.2f, 0.2f, length, 10, 2);

        (_meshFilter, _) =
            gameObject.AddMeshFilterAndRenderer(new Material(Shader.Find("Standard")), _proceduralCylinder.Mesh);

        _meshCollider = gameObject.AddComponentIfNotExist<MeshCollider>();
        _meshCollider.sharedMesh = _proceduralCylinder.Mesh;
        _meshCollider.convex = true;

        // must be after the mesh filter is fully initialized
        gameObject.AddComponentIfNotExist<PhysicalBlockBase>();
    }

    public void OnValidate()
    {
        UpdateProceduralModel();
    }

    public void UpdateProceduralModel()
    {
        if (_proceduralCylinder == null)
            _proceduralCylinder = new ProceduralCylinder(0.2f, 0.2f, length, 10, 2);
        _proceduralCylinder.SetHeight(length);
        _meshFilter.sharedMesh = _proceduralCylinder.Mesh;
        _meshCollider.sharedMesh = _proceduralCylinder.Mesh;
    }
}