// https://github.com/GlitchEnzo/UnityProceduralPrimitives
// https://github.com/ogrew/Unity-ProceduralPrimitives

namespace ProceduralPrimitives
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Holds extension methods for a Unity <see cref="Mesh"/>.
    /// </summary>
    public static class MeshExtensions
    {
        /// <summary>
        /// The formatted path to a <see cref="Mesh"/> asset.
        /// </summary>
        private const string MeshPath = "Assets/Meshes/{0}.asset";

        /// <summary>
        /// Serializes the <see cref="Mesh"/> out to the hard drive as an asset.  This only works in the Editor and will do nothing at runtime.
        /// </summary>
        /// <param name="mesh">The <see cref="Mesh"/> to create the asset from.</param>
        public static void CreateAsset(this Mesh mesh)
        {
#if UNITY_EDITOR
            // Detect if Meshes folder exists and create it if it does not
            if (!Directory.Exists(Application.dataPath + "/Meshes"))
            {
                UnityEditor.AssetDatabase.CreateFolder("Assets", "Meshes");
            }

            UnityEditor.AssetDatabase.CreateAsset(mesh, string.Format(MeshPath, mesh.name));
#endif
        }

        public static void CreateBox(this Mesh mesh, float width, float height, float depth, int widthSegments,
            int heightSegments, int depthSegments)
        {
            mesh.name = "Box";
            mesh.Clear();

            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            var widthHalf = width / 2.0f;
            var heightHalf = height / 2.0f;
            var depthHalf = depth / 2.0f;

            BuildBoxSide(ref vertices, ref uvs, ref triangles, 2, 1, -1, -1, depth, height, widthHalf, widthSegments,
                heightSegments, depthSegments); // px
            BuildBoxSide(ref vertices, ref uvs, ref triangles, 2, 1, 1, -1, depth, height, -widthHalf, widthSegments,
                heightSegments, depthSegments); // nx
            BuildBoxSide(ref vertices, ref uvs, ref triangles, 0, 2, 1, 1, width, depth, heightHalf, widthSegments,
                heightSegments, depthSegments); // py
            BuildBoxSide(ref vertices, ref uvs, ref triangles, 0, 2, 1, -1, width, depth, -heightHalf, widthSegments,
                heightSegments, depthSegments); // ny
            BuildBoxSide(ref vertices, ref uvs, ref triangles, 0, 1, 1, -1, width, height, depthHalf, widthSegments,
                heightSegments, depthSegments); // pz
            BuildBoxSide(ref vertices, ref uvs, ref triangles, 0, 1, -1, -1, width, height, -depthHalf, widthSegments,
                heightSegments, depthSegments); // nz

            //this.computeCentroids();
            //this.mergeVertices();

            mesh.vertices = vertices.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateNormals();
        }

        /// <summary>
        /// Helper method used to build a side of a box.
        /// </summary>
        /// <param name="vertices">Vertices.</param>
        /// <param name="uvs">Uvs.</param>
        /// <param name="triangles">Triangles.</param>
        /// <param name="u">U.</param>
        /// <param name="v">V.</param>
        /// <param name="udir">Udir.</param>
        /// <param name="vdir">Vdir.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="depth">Depth.</param>
        /// <param name="widthSegments">Width segments.</param>
        /// <param name="heightSegments">Height segments.</param>
        /// <param name="depthSegments">Depth segments.</param>
        private static void BuildBoxSide(ref List<Vector3> vertices, ref List<Vector2> uvs, ref List<int> triangles,
            int u, int v, int udir, int vdir, float width, float height, float depth, int widthSegments,
            int heightSegments, int depthSegments)
        {
            int w = 2;
            int ix;
            int iy;
            var gridX = widthSegments;
            var gridY = heightSegments;
            var widthHalf = width / 2.0f;
            var heightHalf = height / 2.0f;
            var offset = vertices.Count;

            if ((u == 0 && v == 1) || (u == 1 && v == 0))
            {
                w = 2;
            }
            else if ((u == 0 && v == 2) || (u == 2 && v == 0))
            {
                w = 1;
                gridY = depthSegments;
            }
            else if ((u == 2 && v == 1) || (u == 1 && v == 2))
            {
                w = 0;
                gridX = depthSegments;
            }

            var gridX1 = gridX + 1;
            var gridY1 = gridY + 1;
            var segmentWidth = width / gridX;
            var segmentHeight = height / gridY;
            var normal = new Vector3();

            normal[w] = depth > 0 ? 1 : -1;

            for (iy = 0; iy < gridY1; iy++)
            {
                for (ix = 0; ix < gridX1; ix++)
                {
                    var vector = new Vector3
                    {
                        [u] = (ix * segmentWidth - widthHalf) * udir,
                        [v] = (iy * segmentHeight - heightHalf) * vdir,
                        [w] = depth
                    };

                    var uv = new Vector2(1.0f - (float)ix / gridX, 1.0f - (float)iy / gridY);

                    vertices.Add(vector);
                    uvs.Add(uv);
                }
            }

            for (iy = 0; iy < gridY; iy++)
            {
                for (ix = 0; ix < gridX; ix++)
                {
                    var a = ix + gridX1 * iy;
                    var b = ix + gridX1 * (iy + 1);
                    var c = (ix + 1) + gridX1 * (iy + 1);
                    var d = (ix + 1) + gridX1 * iy;

                    triangles.Add(a + offset);
                    triangles.Add(b + offset);
                    triangles.Add(d + offset);

                    triangles.Add(b + offset);
                    triangles.Add(c + offset);
                    triangles.Add(d + offset);
                }
            }
        }

        public static void CreateCircle(this Mesh mesh, float radius, int segments, float startAngle, float angularSize)
        {
            mesh.name = "Circle";
            mesh.Clear();

            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            vertices.Add(Vector3.zero);
            uvs.Add(new Vector2(0.5f, 0.5f));

            float stepAngle = angularSize / segments;

            for (int i = 0; i <= segments; i++)
            {
                var vertex = new Vector3();
                float angle = startAngle + stepAngle * i;

                //Debug.Log(string.Format("{0}: {1}", i, angle));
                vertex.x = radius * Mathf.Cos(angle);
                vertex.y = radius * Mathf.Sin(angle);

                vertices.Add(vertex);
                uvs.Add(new Vector2((vertex.x / radius + 1) / 2, (vertex.y / radius + 1) / 2));
            }

            //var n = new Vector3(0, 0, 1);

            for (int i = 1; i <= segments; i++)
            {
                triangles.Add(i + 1);
                triangles.Add(i);
                triangles.Add(0);
            }

            mesh.vertices = vertices.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateNormals();
        }

        public static void CreateSphere(this Mesh mesh, float radius, int widthSegments, int heightSegments,
            float phiStart, float phiLength, float thetaStart, float thetaLength)
        {
            mesh.name = "Sphere";
            mesh.Clear();

            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            int x, y;

            List<List<int>> verticesLists = new List<List<int>>();
            List<List<Vector2>> uvsLists = new List<List<Vector2>>();

            for (y = 0; y <= heightSegments; y++)
            {
                List<int> verticesRow = new List<int>();
                List<Vector2> uvsRow = new List<Vector2>();

                for (x = 0; x <= widthSegments; x++)
                {
                    var u = x / (float)widthSegments;
                    var v = y / (float)heightSegments;

                    var vertex = new Vector3();
                    vertex.x = -radius * Mathf.Cos(phiStart + u * phiLength) * Mathf.Sin(thetaStart + v * thetaLength);
                    vertex.y = radius * Mathf.Cos(thetaStart + v * thetaLength);
                    vertex.z = radius * Mathf.Sin(phiStart + u * phiLength) * Mathf.Sin(thetaStart + v * thetaLength);

                    vertices.Add(vertex);
                    uvs.Add(new Vector2(u, 1 - v));

                    verticesRow.Add(vertices.Count - 1);
                    uvsRow.Add(new Vector2(u, 1 - v));
                }

                verticesLists.Add(verticesRow);
                uvsLists.Add(uvsRow);
            }

            for (y = 0; y < heightSegments; y++)
            {
                for (x = 0; x < widthSegments; x++)
                {
                    var v1 = verticesLists[y][x + 1];
                    var v2 = verticesLists[y][x];
                    var v3 = verticesLists[y + 1][x];
                    var v4 = verticesLists[y + 1][x + 1];

                    // normalize
                    //var n1 = vertices[ v1 ];
                    //var n2 = vertices[ v2 ];
                    //var n3 = vertices[ v3 ];
                    //var n4 = vertices[ v4 ];

                    var uv1 = uvsLists[y][x + 1];
                    var uv2 = uvsLists[y][x];
                    var uv3 = uvsLists[y + 1][x];
                    var uv4 = uvsLists[y + 1][x + 1];

                    if (Mathf.Abs(vertices[v1].y) == radius)
                    {
                        uv1.x = (uv1.x + uv2.x) / 2;

                        triangles.Add(v1);
                        triangles.Add(v3);
                        triangles.Add(v4);

                        //this.faces.push( new THREE.Face3( v1, v3, v4, [ n1, n3, n4 ] ) );
                        //this.faceVertexUvs[ 0 ].push( [ uv1, uv3, uv4 ] );
                    }
                    else if (Mathf.Abs(vertices[v3].y) == radius)
                    {
                        uv3.x = (uv3.x + uv4.x) / 2;

                        triangles.Add(v1);
                        triangles.Add(v2);
                        triangles.Add(v3);

                        //this.faces.push( new THREE.Face3( v1, v2, v3, [ n1, n2, n3 ] ) );
                        //this.faceVertexUvs[ 0 ].push( [ uv1, uv2, uv3 ] );
                    }
                    else
                    {
                        triangles.Add(v1);
                        triangles.Add(v2);
                        triangles.Add(v4);

                        triangles.Add(v2);
                        triangles.Add(v3);
                        triangles.Add(v4);

                        //this.faces.push( new THREE.Face3( v1, v2, v4, [ n1, n2, n4 ] ) );
                        //this.faceVertexUvs[ 0 ].push( [ uv1, uv2, uv4 ] );

                        //this.faces.push( new THREE.Face3( v2, v3, v4, [ n2.clone(), n3, n4.clone() ] ) );
                        //this.faceVertexUvs[ 0 ].push( [ uv2.clone(), uv3, uv4.clone() ] );
                    }
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateNormals();
        }

        /// <summary>
        /// Fills this <see cref="Mesh"/> with vertices forming a 2D plane.
        /// </summary>
        /// <param name="mesh">The <see cref="Mesh"/> to fill with vertices.</param>
        /// <param name="width">Width of the plane. Value should be greater than or equal to 0.0f.</param>
        /// <param name="height">Height of the plane. Value should be greater than or equal to 0.0f.</param>
        /// <param name="widthSegments">The number of subdivisions along the width direction.</param>
        /// <param name="heightSegments">The number of subdivisions along the height direction.</param>
        public static void CreatePlane(this Mesh mesh, float width, float height, int widthSegments, int heightSegments)
        {
            mesh.name = "Plane";
            mesh.Clear();

            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            var widthHalf = width / 2.0f;
            var heightHalf = height / 2.0f;

            var gridX = widthSegments;
            var gridZ = heightSegments;

            var gridX1 = gridX + 1;
            var gridZ1 = gridZ + 1;

            var segmentWidth = width / gridX;
            var segmentHeight = height / gridZ;

            //var normal = new Vector3(0, 0, 1);

            for (int iz = 0; iz < gridZ1; iz++)
            {
                for (int ix = 0; ix < gridX1; ix++)
                {
                    var x = ix * segmentWidth - widthHalf;
                    var y = iz * segmentHeight - heightHalf;

                    var uv = new Vector2((float)ix / gridX, 1.0f - (float)iz / gridZ);

                    vertices.Add(new Vector3(x, -y, 0));
                    uvs.Add(uv);
                }
            }

            for (int iz = 0; iz < gridZ; iz++)
            {
                for (int ix = 0; ix < gridX; ix++)
                {
                    var a = ix + gridX1 * iz;
                    var b = ix + gridX1 * (iz + 1);
                    var c = (ix + 1) + gridX1 * (iz + 1);
                    var d = (ix + 1) + gridX1 * iz;

                    //var uva = new Vector2( ix / gridX, 1 - iz / gridZ );
                    //var uvb = new Vector2( ix / gridX, 1 - ( iz + 1 ) / gridZ );
                    //var uvc = new Vector2( ( ix + 1 ) / gridX, 1 - ( iz + 1 ) / gridZ );
                    //var uvd = new Vector2( ( ix + 1 ) / gridX, 1 - iz / gridZ );

                    triangles.Add(a);
                    triangles.Add(d);
                    triangles.Add(b);

                    triangles.Add(b);
                    triangles.Add(d);
                    triangles.Add(c);
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateNormals();
        }
    }
}