using System;
using UnityEngine;

namespace Utilities
{
    public static class DebugExtension
    {
        public static void DrawPolygon(this Debug debug, params Vector2[] points)
        {
            if (points.Length < 3)
            {
                throw new Exception("Polygon must be comosed of at least three points");
            }

            var lastPoint = points[points.Length];

            foreach (var point in points)
            {
                Debug.DrawLine(lastPoint, point);

                lastPoint = point;
            }
        }
    }
}