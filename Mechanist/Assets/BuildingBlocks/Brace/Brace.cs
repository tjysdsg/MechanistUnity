using System;
using ProceduralPrimitives;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Brace : MonoBehaviour
{
    [SerializeField] public Transform block1;
    [SerializeField] public Transform block2;

    private float _length;
    public float Length => _length;

    private ProceduralCylinder _proceduralCylinder;
    // private MeshCollider _meshCollider;
    private MeshFilter _meshFilter;

    private GameObject _go;

    public void Awake()
    {
        InitializeInEditModeOrRuntime();

        UpdateProceduralModel();

        // _meshCollider.sharedMesh = _proceduralCylinder.Mesh;
        _meshFilter.sharedMesh = _proceduralCylinder.Mesh;
        _go.AddComponentIfNotExist<VolumeBasedRigidbodyMass>(); // uses the mesh
    }

    public void Start()
    {
        // make sure the connected two objects have rigidbody
        if (!block1.gameObject.HasComponent<Rigidbody>() || !block2.gameObject.HasComponent<Rigidbody>())
        {
            String msg = "Brace requires block1 and block2 has a RigidBody";
            if (Application.isPlaying)
                throw new Exception(msg);
            else
                Debug.Log(msg);
        }

        // Add fixed joint to the connected blocks
        if (Application.isPlaying)
        {
            FixedJoint j1 = _go.AddComponent<FixedJoint>();
            j1.connectedBody = block1.GetComponent<Rigidbody>();
            FixedJoint j2 = _go.AddComponent<FixedJoint>();
            j2.connectedBody = block2.GetComponent<Rigidbody>();
        }
    }

    private void InitializeInEditModeOrRuntime()
    {
        _go = gameObject;

        if (_meshFilter == null)
            _meshFilter = GetComponent<MeshFilter>();

        // _meshCollider = _go.AddComponentIfNotExist<MeshCollider>();
        // _meshCollider.convex = true;
    }

    // Update the position and the mesh of the cylinder based on the two connected objects
    void UpdateCylinder()
    {
        transform.localScale = Vector3.one;

        Vector3 pos1 = block1.position;
        Vector3 pos2 = block2.position;

        // position
        Vector3 center = (pos1 + pos2) / 2;
        Vector3 direction = pos1 - pos2;
        transform.SetPositionAndRotation(center, Quaternion.FromToRotation(Vector3.up, pos1 - pos2));

        // generate/update cylinder mesh
        _length = direction.magnitude;
        if (_proceduralCylinder == null)
            _proceduralCylinder = new ProceduralCylinder(0.2f, 0.2f, _length, 10, 2);
        else
            _proceduralCylinder.SetHeight(_length);
    }

    public void OnValidate()
    {
        InitializeInEditModeOrRuntime();
        UpdateProceduralModel();
    }

    public void UpdateProceduralModel()
    {
        if (block1 == null || block2 == null)
        {
            throw new UnassignedReferenceException("block1 and block2 of Brace must be assigned");
        }

        UpdateCylinder();
        _meshFilter.sharedMesh = _proceduralCylinder.Mesh;
        // _meshCollider.sharedMesh = _proceduralCylinder.Mesh;
    }
}