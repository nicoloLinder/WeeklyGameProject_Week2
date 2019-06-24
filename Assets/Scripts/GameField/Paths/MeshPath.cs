using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameField
{
    [CreateAssetMenu(menuName = "Paths/Mesh Path")]
    public class MeshPath : Path
    {
        public Mesh mesh;
        
        public override List<Vector2> GeneratePath()
        {
            if (_path == null)
            {
                _path = new List<Vector2>();
            }
            _path.Clear();

            var unorderedVertices = mesh.vertices.ToList();

            var currentVertex = unorderedVertices[0];

            _path.Add(currentVertex);
            unorderedVertices.Remove(currentVertex);

            while (unorderedVertices.Count > 0)
            {
                currentVertex = unorderedVertices.OrderBy(c => Vector3.Distance(currentVertex, c)).First();

                _path.Add(currentVertex);
                unorderedVertices.Remove(currentVertex);
            }
            
            CalculateBarycenter();
            CalculateMinMaxRadius();

            CorrectToRadius();

            return _path;
        }
    }
}