using ProceduralPrimitives;
using ScriptableObjectArchitecture;
using UnityEngine;

public class Brace : MonoBehaviour
{
    [SerializeField] private FloatReference cylinderHeight;

    private GameObject _go;
    private ProceduralCylinder _proceduralCylinder;

    void Start()
    {
        _proceduralCylinder = new ProceduralCylinder(0.2f, 0.2f, cylinderHeight.Value, 10, 2);

        _go = new GameObject();
        _go.AddMeshFilterAndRenderer(new Material(Shader.Find("Standard")), _proceduralCylinder.Mesh);

        MeshCollider meshCollider = _go.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = _proceduralCylinder.Mesh;
        meshCollider.convex = true;

        _go.AddComponent<PhysicalBlockBase>();
    }

    public void OnCylinderHeightChanged()
    {
        _proceduralCylinder.SetHeight(cylinderHeight.Value);
        _go.GetComponent<MeshCollider>().sharedMesh = _proceduralCylinder.Mesh;
        _go.GetComponent<MeshFilter>().sharedMesh = _proceduralCylinder.Mesh;
    }
}