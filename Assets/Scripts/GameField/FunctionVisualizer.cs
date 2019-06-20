using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameField
{
    [ExecuteInEditMode]
    public class FunctionVisualizer : MonoBehaviour
    {
        public int detail;

        public LineRenderer lineRenderer;

        public MeshFilter meshFilter;
        public float minDistance;

        private List<Vector2> path = new List<Vector2>();

        public PathFunction pathFunction;
        public float position;

        public int width;

        private void OnDrawGizmos()
        {
            foreach (var VARIABLE in path) Gizmos.DrawSphere(VARIABLE, 0.01f);
        }

        private void OnValidate()
        {
            path = GeneratePath(minDistance, detail, pathFunction);

            width = Mathf.Abs(width);
            if (width == 0) width = 2;
            width -= width % 2 == 0 ? 0 : 1;

            var points = new Vector3[width];

            var index = 0;
            for (var i = -points.Length / 2; i < points.Length / 2; i++)
            {
                var pathIndex = (int) (Mathf.Abs(position / 100 % 1) * path.Count) + i;

                if (pathIndex < 0) pathIndex += path.Count;
                else if (pathIndex > path.Count - 1) pathIndex -= path.Count;

                points[index++] = (Vector3) path[pathIndex] + Vector3.back;
            }

            lineRenderer.positionCount = points.Length;
            lineRenderer.SetPositions(points);
            meshFilter.sharedMesh = MeshGenerator.GenerateMesh(path);
        }


        public List<Vector2> GeneratePath(float minPointsDistance, int iterationCount,
            params PathFunction[] pathFunctions)
        {
            var pathPoints = new List<Vector2>
            {
                pathFunctions.Aggregate(Vector2.zero, (current, function) => current + function.Function(0))
            };

            var currentPosition =
                pathFunctions.Aggregate(Vector2.zero,
                    (current, function) => current + function.Function(1f / iterationCount));

            for (var i = 0; i < iterationCount; i++)
            {
                currentPosition = pathFunctions.Aggregate(Vector2.zero,
                    (current, function) => current + function.Function(i / (float) iterationCount));

                if (Vector2.Distance(pathPoints[pathPoints.Count - 1], currentPosition) > minPointsDistance)
                    pathPoints.Add(currentPosition);
            }

            return pathPoints;
        }

        public void GenerateBasicPath()
        {
            path = GeneratePath(minDistance, detail, pathFunction);
        }
    }
}