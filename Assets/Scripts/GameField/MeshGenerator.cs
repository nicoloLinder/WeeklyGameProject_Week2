using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using MeshUtilities;

namespace GameField
{
    public static class MeshGenerator
    {
        public static Mesh GenerateMesh(Path path)
        {
            var mesh = new Mesh();
            var vertices = new Vector3[path.Length-1];
            var normal = new Vector2[path.Length];

//            var triangles = new int[vertices.Length * 3];

            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] = path[i];
            }

            for (var i = 0; i < vertices.Length; i++) vertices[i] += (Vector3)path.GetPointNormal(i) * 0.1f;
            
            mesh.vertices = vertices;
            mesh.triangles = new Triangulator(vertices).Triangulate();

            return mesh;
        }

        public static Mesh GenerateMesh(List<Vector2> path)
        {
            var mesh = new Mesh();
            var vertices = new Vector3[path.Count + 1];
            var triangles = new int[vertices.Length * 3];

            for (var i = 1; i < vertices.Length; i++) vertices[i] = path[i - 1];

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

            return mesh;
        }
    }
}