using System;
using SlimeSystem;
using Unity.VectorGraphics;
using UnityEngine;

namespace Utilities
{
    public static class DebugDraw
    {

        #region Polygons

        public static void DrawBezierPolygon(BezierSegment bezierSegment)
        {
            DrawBezierolygon(Color.magenta, bezierSegment);
        }
        
        private static void DrawBezierolygon(Color color, BezierSegment bezierSegment)
        {
            DrawPolygon(color, bezierSegment.P0, bezierSegment.P1, bezierSegment.P2, bezierSegment.P3);
        }
        
        private static void DrawPolygon(params Vector2[] points)
        {
            DrawPolygon(Color.magenta, points);
        }
        private static void DrawPolygon(Color color, params Vector2[] points)
        {
            if (points.Length < 3)
            {
                throw new Exception("Polygon must be composed of at least three points");
            }

            var lastPoint = points[points.Length - 1];

            foreach (var point in points)
            {
                Debug.DrawLine(lastPoint, point, color);

                lastPoint = point;
            }
        }

        #endregion

        #region BezierDraw

        private static void DrawBezier(BezierSegment bezierSegment)
        {
            for (var i = 1; i <= 20; i++)
            {
                Gizmos.DrawLine(BezierCurveUtils.GetPointOnBezier(bezierSegment, (i - 1) / 20f).P0,
                    BezierCurveUtils.GetPointOnBezier(bezierSegment, i / 20f).P0);
            }
        }

        #endregion
        
        
    }
}