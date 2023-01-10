using UnityEngine;

namespace MeshUtils
{
    public class ProceduralSpringMesh
    {
        public Mesh Mesh => _mesh;
        private readonly Mesh _mesh = null;

        private float _cylinderRadius;
        private float _radius;
        private float _height;
        private int _nRadialSegments;
        private int _nSegments;
        private int _nCircles;
        private float _angleStep;

        private readonly int _vertCols;
        private readonly int _vertRows;

        private readonly Vector3[] _vertices;
        private readonly Vector2[] _uvs;
        private readonly int[] _triangles;

        public ProceduralSpringMesh(float radius, float cylinderRadius, float height,
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
            var numVerts = _vertCols * _vertRows;

            // how many triangles
            var numSideTris = _nRadialSegments * _nSegments * 2;
            var trisArrayLength = numSideTris * 3;

            // initialize arrays
            _vertices = new Vector3[numVerts];
            _uvs = new Vector2[numVerts];
            _triangles = new int[trisArrayLength];
        }

        public Mesh UpdateMesh()
        {
            float angleStep = 2 * Mathf.PI / _nRadialSegments;

            // helix:
            //   x(t) = R cos(t)
            //   y(t) = R sin(t)
            //   z(t) = a t
            // where R = _radius

            // draw side faces
            float heightPerSeg = _height / _nSegments;
            float tStep = 2 * Mathf.PI * _nCircles / _nSegments;
            float uvStepH = 1.0f / _nRadialSegments;
            float uvStepV = 1.0f / _nSegments;
            for (int seg_i = 0; seg_i < _vertRows; seg_i++)
            {
                for (int col = 0; col < _vertCols; col++)
                {
                    float angle = col * angleStep;
                    if (col == _vertCols - 1) angle = 0;

                    Vector3 cylinderCoord = new Vector3(
                        _cylinderRadius * Mathf.Cos(angle),
                        _cylinderRadius * Mathf.Sin(angle),
                        0
                    );

                    // Vertex positions and UVs
                    int vertIdx = seg_i * _vertCols + col;
                    float R = _radius - cylinderCoord.x;
                    float t = seg_i * tStep;
                    _vertices[vertIdx] = new Vector3(
                        R * Mathf.Cos(t),
                        R * Mathf.Sin(t),
                        heightPerSeg * seg_i + cylinderCoord.y - _height / 2
                    );
                    _uvs[vertIdx] = new Vector2(
                        col * uvStepH, seg_i * uvStepV
                    );

                    // triangles
                    if (seg_i == 0 || col >= _vertCols - 1)
                        continue;

                    int triIdx = ((seg_i - 1) * _nRadialSegments * 6 + col * 6);
                    _triangles[triIdx + 0] = seg_i * _vertCols + col + 1;
                    _triangles[triIdx + 1] = seg_i * _vertCols + col;
                    _triangles[triIdx + 2] = (seg_i - 1) * _vertCols + col;

                    _triangles[triIdx + 3] = (seg_i - 1) * _vertCols + col + 1;
                    _triangles[triIdx + 4] = seg_i * _vertCols + col + 1;
                    _triangles[triIdx + 5] = (seg_i - 1) * _vertCols + col;
                }
            }

            _mesh.vertices = _vertices;
            _mesh.uv = _uvs;
            _mesh.triangles = _triangles;

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