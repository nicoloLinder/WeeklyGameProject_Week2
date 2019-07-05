using UnityEngine;

namespace GameField
{
    public static class MeshGenerator
    {
        public static Mesh GenerateMesh(Path path)
        {
            var mesh = new Mesh();
            var vertices = new Vector3[path.Length];

//            var triangles = new int[vertices.Length * 3];

            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] = path[i];
            }

            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] += (Vector3)path.GetPointNormal(i) * GameFieldManager.Instance.Offset;
            }
            
            mesh.vertices = vertices;
            mesh.triangles = new Triangulator(vertices).Triangulate();

            return mesh;
        }

        public static Mesh GenerateMesh2(Path path)
        {
            var mesh = new Mesh();
            var vertices = new Vector3[path.Length + 1];
            var triangles = new int[vertices.Length * 3];
            var uv = new Vector2[vertices.Length];

            for (var i = 1; i < vertices.Length; i++) vertices[i] = path[i - 1];
            
            for (var i = 1; i < vertices.Length; i++)
            {
                vertices[i] += (Vector3)path.GetPointNormal(i-1) * GameFieldManager.Instance.Offset;
                uv[i] = Vector2.right;
            }

            var triangleIndex = 0;
            
            

            for (var i = 0; i < vertices.Length - 1; i++)
            {
                triangles[triangleIndex++] = 0;
                triangles[triangleIndex++] = i + 1;
                triangles[triangleIndex++] = i;
            }

            triangles[triangleIndex++] = 0;
            triangles[triangleIndex++] = 1;
            triangles[triangleIndex++] = vertices.Length - 1;

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;

            return mesh;
        }
    }
}