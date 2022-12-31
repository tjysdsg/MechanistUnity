using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ProceduralCylinder : MonoBehaviour
{
    public Mesh Mesh => _proceduralCylinderMesh.Mesh;
    private ProceduralCylinderMesh _proceduralCylinderMesh;

    public int nRadialSegments = 10;
    public int nHeightSegments = 2;
    public float height = 1;
    public float topRadius = 0.5f;
    public float bottomRadius = 0.5f;

    private MeshFilter _meshFilter;
    private GameObject _go;

    public void Awake()
    {
        _proceduralCylinderMesh =
            new ProceduralCylinderMesh(topRadius, bottomRadius, height, nRadialSegments, nHeightSegments);

        _meshFilter = GetComponent<MeshFilter>();
        _meshFilter.sharedMesh = _proceduralCylinderMesh.Mesh;
    }

    public void UpdateMesh()
    {
        _meshFilter.sharedMesh = _proceduralCylinderMesh.UpdateMesh(topRadius, bottomRadius, height);
    }

    public void UpdateMeshInEditor()
    {
        ProceduralCylinderMesh mesh =
            new ProceduralCylinderMesh(topRadius, bottomRadius, height, nRadialSegments, nHeightSegments);
        GetComponent<MeshFilter>().sharedMesh = mesh.UpdateMesh();
    }

    public void SetHeight(float h, bool updateMesh = true)
    {
        this.height = h;
        if (updateMesh) UpdateMesh();
    }
}