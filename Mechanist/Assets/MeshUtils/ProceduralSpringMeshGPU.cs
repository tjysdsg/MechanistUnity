using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MeshUtils
{
    public class ProceduralSpringMeshGPU
    {
        private ComputeShader _shader;
        public Mesh Mesh => _mesh;
        private readonly Mesh _mesh = null;

        private float _cylinderRadius;
        private float _radius;
        private float _height;
        private int _nRadialSegments;
        private int _nSegments;
        private int _nCircles;

        private readonly int _vertCols;
        private readonly int _vertRows;

        private readonly int _numVerts;
        private readonly int _trisArrayLength;
        private readonly Vector3[] _vertices;
        private readonly Vector2[] _uvs;
        private readonly int[] _triangles;

        // the structure received from the compute shader
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct GeneratedVertex
        {
            public Vector3 position;
            public Vector2 uv;
        }

        private const int GENERATED_VERT_STRIDE = sizeof(float) * (3 + 2);
        private const int GENERATED_INDEX_STRIDE = sizeof(int);
        private GraphicsBuffer _genVertBuffer = null;
        private GraphicsBuffer _genIndexBuffer = null;

        public ProceduralSpringMeshGPU(float radius, float cylinderRadius, float height,
            int nCircles, int nRadialSegments, int nSegments)
        {
            _radius = radius;
            _cylinderRadius = cylinderRadius;
            _height = height;
            _nCircles = nCircles;
            _nRadialSegments = nRadialSegments;
            _nSegments = nSegments;

            _mesh = new Mesh { name = "Spring" };

            // how many vertices we need
            _vertCols = _nRadialSegments + 1;
            _vertRows = _nSegments + 1;
            _numVerts = _vertCols * _vertRows;

            // how many triangles
            var numSideTris = _nRadialSegments * _nSegments * 2;
            _trisArrayLength = numSideTris * 3;

            _vertices = new Vector3[_numVerts];
            _uvs = new Vector2[_numVerts];
            _triangles = new int[_trisArrayLength];

            _genVertBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, _numVerts, GENERATED_VERT_STRIDE);
            _genIndexBuffer =
                new GraphicsBuffer(GraphicsBuffer.Target.Structured, _trisArrayLength, GENERATED_INDEX_STRIDE);

            // find the our compute shader
            var guid = AssetDatabase.FindAssets("ProceduralSpring").FirstOrDefault();
            if (string.IsNullOrEmpty(guid))
                Debug.LogError("Cannot find compute shader: ProceduralSpring.compute");
            else
                _shader = AssetDatabase.LoadAssetAtPath<ComputeShader>(AssetDatabase.GUIDToAssetPath(guid));
        }

        ~ProceduralSpringMeshGPU()
        {
            // release the graphics buffers
            _genVertBuffer.Release();
            _genIndexBuffer.Release();
        }

        public Mesh UpdateMesh()
        {
            int shaderKernel = _shader.FindKernel("Main");

            // set variables
            _shader.SetFloat("cylinder_radius", _cylinderRadius);
            _shader.SetFloat("radius", _radius);
            _shader.SetFloat("height", _height);
            _shader.SetInt("n_radial_segments", _nRadialSegments);
            _shader.SetInt("n_segments", _nSegments);
            _shader.SetInt("n_circles", _nCircles);

            // buffer
            GeneratedVertex[] generatedVertices = new GeneratedVertex[_numVerts];
            int[] generatedIndices = new int[_trisArrayLength];
            _shader.SetBuffer(shaderKernel, "verts", _genVertBuffer);
            _shader.SetBuffer(shaderKernel, "triangles", _genIndexBuffer);

            // dispatch the compute shader
            _shader.GetKernelThreadGroupSizes(shaderKernel, out uint threadGroupSize, out _, out _);
            int dispatchSize = Mathf.CeilToInt((float)_numVerts / threadGroupSize);
            _shader.Dispatch(shaderKernel, dispatchSize, 1, 1);

            // get the data from the compute shader
            // TODO: _vertReadbackRequest = AsyncGPUReadback.Request(genVertBuffer);
            // TODO: _triReadbackRequest = AsyncGPUReadback.Request(genIndexBuffer);
            _genVertBuffer.GetData(generatedVertices);
            _genIndexBuffer.GetData(generatedIndices);
            for (int i = 0; i < _numVerts; ++i)
            {
                _vertices[i] = generatedVertices[i].position;
                _uvs[i] = generatedVertices[i].uv;
            }

            for (int i = 0; i < _trisArrayLength; ++i)
            {
                _triangles[i] = generatedIndices[i];
            }

            _mesh.vertices = _vertices;
            _mesh.uv = _uvs;
            _mesh.triangles = _triangles;

            // TODO: move normal calculation into shader
            _mesh.RecalculateNormals();
            // average the normals to avoid a hard edge at the cylinder seam
            for (int row = 0; row < _vertRows; row++)
            {
                Vector3[] normals = _mesh.normals;

                int i1 = row * _vertCols + 0;
                int i2 = row * _vertCols + _vertCols - 1;
                var avg = (normals[i1] + normals[i2]).normalized;
                normals[i1] = avg;
                normals[i2] = avg;

                _mesh.normals = normals;
            }

            return _mesh;
        }

        public Mesh UpdateMesh(float height)
        {
            _height = height;
            return UpdateMesh();
        }
    }
}