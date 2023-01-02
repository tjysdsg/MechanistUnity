using UnityEngine;

namespace MeshUtils
{
    public class ProceduralCylinderMesh
    {
        public Mesh Mesh => _mesh;
        private readonly Mesh _mesh = null;

        private float _topRadius = 0.5f;
        private float _bottomRadius = 0.5f;
        private float _height = 1;
        private readonly int _nRadialSegments = 10;
        private readonly int _nHeightSegments = 2;

        private readonly int _vertCols;
        private readonly int _vertRows;
        private readonly int _numVerts;
        private readonly int _numSideTris;

        private readonly Vector3[] _vertices;
        private readonly Vector2[] _uvs;
        private readonly int[] _triangles;

        public ProceduralCylinderMesh(float topRadius, float bottomRadius, float height, int nRadialSegments,
            int nHeightSegments)
        {
            _topRadius = topRadius;
            _bottomRadius = bottomRadius;
            _height = height;
            _nRadialSegments = nRadialSegments;
            _nHeightSegments = nHeightSegments;

            _mesh = new Mesh()
            {
                name = "Cylinder"
            };

            // how many vertices we need
            _vertCols = _nRadialSegments + 1;
            _vertRows = _nHeightSegments + 1;

            // side + the perimeters of caps + centers of caps
            _numVerts = _vertCols * _vertRows + 2 * _vertCols + 2;

            _numSideTris = _nRadialSegments * _nHeightSegments * 2;
            var numCapTris = _nRadialSegments * 2;

            // each triangle requires three indices
            var trisArrayLength = (_numSideTris + numCapTris) * 3;

            // initialize arrays
            _vertices = new Vector3[_numVerts];
            _uvs = new Vector2[_numVerts];
            _triangles = new int[trisArrayLength];
        }

        public Mesh UpdateMesh()
        {
            float angleStep = 2 * Mathf.PI / _nRadialSegments;

            // draw side faces
            float heightStep = _height / _nHeightSegments;
            float uvStepH = 1.0f / _nRadialSegments;
            float uvStepV = 1.0f / _nHeightSegments;
            for (int row = 0; row < _vertRows; row++)
            {
                for (int col = 0; col < _vertCols; col++)
                {
                    float angle = col * angleStep;

                    float ratio = (float)row / (_vertRows - 1);
                    float radius = Mathf.Lerp(_bottomRadius, _topRadius, ratio);
                    // first and last vertex of each row at same spot
                    if (col == _vertCols - 1)
                    {
                        angle = 0;
                    }

                    // Vertex positions and UVs
                    int vertIdx = row * _vertCols + col;
                    _vertices[vertIdx] = new Vector3(
                        radius * Mathf.Cos(angle),
                        radius * Mathf.Sin(angle),
                        row * heightStep - _height / 2
                    );
                    _uvs[vertIdx] = new Vector2(
                        col * uvStepH, row * uvStepV
                    );

                    // triangles
                    if (row == 0 || col >= _vertCols - 1)
                        continue;

                    int triIdx = ((row - 1) * _nRadialSegments * 6 + col * 6);
                    _triangles[triIdx + 0] = row * _vertCols + col + 1;
                    _triangles[triIdx + 1] = row * _vertCols + col;
                    _triangles[triIdx + 2] = (row - 1) * _vertCols + col;

                    _triangles[triIdx + 3] = (row - 1) * _vertCols + col + 1;
                    _triangles[triIdx + 4] = row * _vertCols + col + 1;
                    _triangles[triIdx + 5] = (row - 1) * _vertCols + col;
                }
            }

            // Draw caps
            _vertices[_numVerts - 2] = new Vector3(0, 0, _height / 2); // top center
            _vertices[_numVerts - 1] = new Vector3(0, 0, -_height / 2); // bottom center
            _uvs[_numVerts - 2] = new Vector2(0.5f, 0.5f);
            _uvs[_numVerts - 1] = new Vector2(0.5f, 0.5f);

            // perimeter vertices of the cap
            int topPerimeterVertIdxOffset = _vertCols * _vertRows;
            int bottomPerimeterVertIdxOffset = _vertCols * _vertRows + _vertCols;
            for (int i = 0; i <= _nRadialSegments; i++)
            {
                float angle = i * angleStep;
                _vertices[topPerimeterVertIdxOffset + i] = new Vector3(
                    _topRadius * Mathf.Cos(angle),
                    _topRadius * Mathf.Sin(angle),
                    _height / 2
                );
                _vertices[bottomPerimeterVertIdxOffset + i] = new Vector3(
                    _bottomRadius * Mathf.Cos(angle),
                    _bottomRadius * Mathf.Sin(angle),
                    -_height / 2
                );
                _uvs[topPerimeterVertIdxOffset + i] = new Vector2(
                    0.5f + 0.5f * Mathf.Cos(angle), 0.5f + 0.5f * Mathf.Sin(angle)
                );
                _uvs[bottomPerimeterVertIdxOffset + i] = new Vector2(
                    0.5f + 0.5f * Mathf.Cos(angle), 0.5f + 0.5f * Mathf.Sin(angle)
                );
            }

            // cap triangles
            int topTriIdxOffset = _numSideTris * 3;
            int bottomTriIdxOffset = (_numSideTris + _nRadialSegments) * 3;
            for (int i = 0; i < _nRadialSegments; i++)
            {
                // top
                _triangles[topTriIdxOffset + 0] = _numVerts - 2;
                _triangles[topTriIdxOffset + 1] = topPerimeterVertIdxOffset + i;
                _triangles[topTriIdxOffset + 2] = topPerimeterVertIdxOffset + i + 1;

                // bottom
                _triangles[bottomTriIdxOffset + 0] = _numVerts - 1;
                _triangles[bottomTriIdxOffset + 1] = bottomPerimeterVertIdxOffset + i + 1;
                _triangles[bottomTriIdxOffset + 2] = bottomPerimeterVertIdxOffset + i;

                topTriIdxOffset += 3;
                bottomTriIdxOffset += 3;
            }

            _mesh.vertices = _vertices;
            _mesh.uv = _uvs;
            _mesh.triangles = _triangles;

            _mesh.RecalculateNormals();
            // _mesh.RecalculateBounds();

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

        public float Height
        {
            get => _height;
            set
            {
                _height = value;
                UpdateMesh();
            }
        }

        public float TopRadius
        {
            get => _topRadius;
            set
            {
                _topRadius = value;
                UpdateMesh();
            }
        }

        public float BottomRadius
        {
            get => _bottomRadius;
            set
            {
                _bottomRadius = value;
                UpdateMesh();
            }
        }

        public Mesh UpdateMesh(float topRadius, float bottomRadius, float height)
        {
            _topRadius = topRadius;
            _bottomRadius = bottomRadius;
            _height = height;
            return UpdateMesh();
        }
    }
}