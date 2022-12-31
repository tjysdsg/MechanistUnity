namespace ProceduralPrimitives
{
    using UnityEngine;

    /// <summary>
    /// Holds extension methods for a Unity <see cref="GameObject"/>.
    /// </summary>
    public static class GameObjectExtensions
    {
        public static T AddComponentIfNotExist<T>(this GameObject go) where T : Component
        {
            T ret = go.GetComponent<T>();
            if (ret == null)
                return go.AddComponent<T>();
            return ret;
        }

        public static bool HasComponent<T>(this GameObject go) where T : Component
        {
            T ret = go.GetComponent<T>();
            return ret != null;
        }

        public static MeshFilter AddMeshFilter(this GameObject gameObject, Mesh mesh)
        {
            MeshFilter meshFilter = gameObject.AddComponentIfNotExist<MeshFilter>();
            meshFilter.sharedMesh = mesh;
            return meshFilter;
        }

        public static MeshRenderer AddMeshRenderer(this GameObject gameObject)
        {
            MeshRenderer meshRenderer = gameObject.AddComponentIfNotExist<MeshRenderer>();
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            return meshRenderer;
        }

        public static MeshRenderer AddMeshRenderer(this GameObject gameObject, Material material)
        {
            MeshRenderer meshRenderer = gameObject.AddMeshRenderer();
            meshRenderer.sharedMaterial = material;
            return meshRenderer;
        }

        public static (MeshFilter, MeshRenderer) AddMeshFilterAndRenderer(this GameObject gameObject, Material material,
            Mesh mesh)
        {
            MeshFilter meshFilter = gameObject.AddMeshFilter(mesh);
            meshFilter.sharedMesh = mesh;
            return (meshFilter, gameObject.AddMeshRenderer(material));
        }

        public static void CreateCircle(this GameObject gameObject, float radius, int segments, float startAngle,
            float angularSize)
        {
            Mesh mesh = new Mesh();
            mesh.CreateCircle(radius, segments, startAngle, angularSize);

            //gameObject.name = "Circle";

            Shader shader = Shader.Find("Standard");
            gameObject.AddMeshFilterAndRenderer(new Material(shader), mesh);
        }

        public static void CreatePlane(this GameObject gameObject, float width, float height, int widthSegments,
            int heightSegments)
        {
            Mesh mesh = new Mesh();
            mesh.CreatePlane(width, height, widthSegments, heightSegments);

            //gameObject.name = "Plane";

            Shader shader = Shader.Find("Standard");
            gameObject.AddMeshFilterAndRenderer(new Material(shader), mesh);
        }

        public static void CreateBox(this GameObject gameObject, float width, float height, float depth,
            int widthSegments, int heightSegments, int depthSegments)
        {
            Mesh mesh = new Mesh();
            mesh.CreateBox(width, height, depth, widthSegments, heightSegments, depthSegments);

            //gameObject.name = "Box";

            Shader shader = Shader.Find("Standard");
            gameObject.AddMeshFilterAndRenderer(new Material(shader), mesh);
        }
    }
}