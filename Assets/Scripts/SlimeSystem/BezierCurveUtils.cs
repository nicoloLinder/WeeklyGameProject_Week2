using System;
using Unity.VectorGraphics;
using UnityEngine;
using Utilities;

namespace SlimeSystem
{
    public class BezierCurveUtils
    {
        public static bool CheckIBezierCurveIntersection(BezierSegment bezierSegment1, BezierSegment bezierSegment2)
        {

            DebugDraw.DrawBezierPolygon(bezierSegment1);
            DebugDraw.DrawBezierPolygon(bezierSegment2);
            
            if (!BoundingBoxIntersect(bezierSegment1, bezierSegment2)) return false;

            if (BoundingBoxArea(bezierSegment1) + BoundingBoxArea(bezierSegment2) < 0.01f)
            {
                return true;
            }

            //    Divide first bezier segment
            var bezierSegment1MidPoint = GetPointOnBezier(bezierSegment1, 0.5f);
            var bezierSegment1A = new BezierSegment
            {
                P0 = bezierSegment1.P0,
                P1 = Vector2.Lerp(bezierSegment1.P0, bezierSegment1.P1, 0.5f),
                P2 = bezierSegment1MidPoint.P1,
                P3 = bezierSegment1MidPoint.P0
            };
            var bezierSegment1B = new BezierSegment
            {
                P0 = bezierSegment1MidPoint.P0,
                P1 = bezierSegment1MidPoint.P2,
                P2 = Vector2.Lerp(bezierSegment1.P2, bezierSegment1.P3, 0.5f),
                P3 = bezierSegment1.P3
            };

            var bezierSegment2MidPoint = GetPointOnBezier(bezierSegment2, 0.5f);
            var bezierSegment2A = new BezierSegment
            {
                P0 = bezierSegment2.P0,
                P1 = Vector2.Lerp(bezierSegment2.P0, bezierSegment2.P1, 0.5f),
                P2 = bezierSegment2MidPoint.P1,
                P3 = bezierSegment2MidPoint.P0
            };

            var bezierSegment2B = new BezierSegment
            {
                P0 = bezierSegment2MidPoint.P0,
                P1 = bezierSegment2MidPoint.P2,
                P2 = Vector2.Lerp(bezierSegment2.P2, bezierSegment2.P3, 0.5f),
                P3 = bezierSegment2.P3
            };
            
            return CheckIBezierCurveIntersection(bezierSegment1A, bezierSegment2A) || CheckIBezierCurveIntersection(bezierSegment1A, bezierSegment2B) || CheckIBezierCurveIntersection(bezierSegment1B, bezierSegment2A) || CheckIBezierCurveIntersection(bezierSegment1B, bezierSegment2B);
        }
        
        public static bool BoundingBoxIntersect(BezierSegment bezierSegment1, BezierSegment bezierSegment2)
        {
            var intersects = false;

            intersects |= LineSegmentsIntersection(bezierSegment1.P0, bezierSegment1.P1, bezierSegment2.P0, bezierSegment2.P1, out _);
            intersects |= LineSegmentsIntersection(bezierSegment1.P0, bezierSegment1.P1, bezierSegment2.P1, bezierSegment2.P2, out _);
            intersects |= LineSegmentsIntersection(bezierSegment1.P0, bezierSegment1.P1, bezierSegment2.P2, bezierSegment2.P3, out _);
            intersects |= LineSegmentsIntersection(bezierSegment1.P0, bezierSegment1.P1, bezierSegment2.P3, bezierSegment2.P0, out _);

            intersects |= LineSegmentsIntersection(bezierSegment1.P1, bezierSegment1.P2, bezierSegment2.P0, bezierSegment2.P1, out _);
            intersects |= LineSegmentsIntersection(bezierSegment1.P1, bezierSegment1.P2, bezierSegment2.P1, bezierSegment2.P2, out _);
            intersects |= LineSegmentsIntersection(bezierSegment1.P1, bezierSegment1.P2, bezierSegment2.P2, bezierSegment2.P3, out _);
            intersects |= LineSegmentsIntersection(bezierSegment1.P1, bezierSegment1.P2, bezierSegment2.P3, bezierSegment2.P0, out _);

            intersects |= LineSegmentsIntersection(bezierSegment1.P2, bezierSegment1.P3, bezierSegment2.P0, bezierSegment2.P1, out _);
            intersects |= LineSegmentsIntersection(bezierSegment1.P2, bezierSegment1.P3, bezierSegment2.P1, bezierSegment2.P2, out _);
            intersects |= LineSegmentsIntersection(bezierSegment1.P2, bezierSegment1.P3, bezierSegment2.P2, bezierSegment2.P3, out _);
            intersects |= LineSegmentsIntersection(bezierSegment1.P2, bezierSegment1.P3, bezierSegment2.P3, bezierSegment2.P0, out _);

            intersects |= LineSegmentsIntersection(bezierSegment1.P3, bezierSegment1.P0, bezierSegment2.P0, bezierSegment2.P1, out _);
            intersects |= LineSegmentsIntersection(bezierSegment1.P3, bezierSegment1.P0, bezierSegment2.P1, bezierSegment2.P2, out _);
            intersects |= LineSegmentsIntersection(bezierSegment1.P3, bezierSegment1.P0, bezierSegment2.P2, bezierSegment2.P3, out _);
            intersects |= LineSegmentsIntersection(bezierSegment1.P3, bezierSegment1.P0, bezierSegment2.P3, bezierSegment2.P0, out _);

            return intersects;
        }
        
        public static bool LineSegmentsIntersection(Vector2 point1A, Vector2 point1B, Vector2 point2A, Vector3 point2B, out Vector2 intersection)
        {
            intersection = Vector2.zero;

            var d = (point1B.x - point1A.x) * (point2B.y - point2A.y) - (point1B.y - point1A.y) * (point2B.x - point2A.x);

            if (Math.Abs(d) < 0.01f)
            {
                return false;
            }

            var u = ((point2A.x - point1A.x) * (point2B.y - point2A.y) - (point2A.y - point1A.y) * (point2B.x - point2A.x)) / d;
            var v = ((point2A.x - point1A.x) * (point1B.y - point1A.y) - (point2A.y - point1A.y) * (point1B.x - point1A.x)) / d;

            if (u < 0.0f || u > 1.0f || v < 0.0f || v > 1.0f)
            {
                return false;
            }

            intersection.x = point1A.x + u * (point1B.x - point1A.x);
            intersection.y = point1A.y + u * (point1B.y - point1A.y);

            return true;
        }

        public static float BoundingBoxArea(BezierSegment bezierSegment)
        {
            var center = (bezierSegment.P0 + bezierSegment.P1 + bezierSegment.P2 + bezierSegment.P3) / 4;

            var area = AreaOfTriangle(center, bezierSegment.P0, bezierSegment.P1);
            area += AreaOfTriangle(center, bezierSegment.P1, bezierSegment.P2);
            area += AreaOfTriangle(center, bezierSegment.P2, bezierSegment.P3);
            area += AreaOfTriangle(center, bezierSegment.P3, bezierSegment.P0);

            return area;
        }
        
        
        public static float AreaOfTriangle(Vector2 pointA, Vector2 pointB, Vector2 pointC)
        {
            return Mathf.Abs(pointA.x * (pointB.y - pointC.y) + pointB.x * (pointC.y - pointA.y) + pointC.x * (pointA.y - pointB.y)) / 2;
        }

        public static BezierSegment GetPointOnBezier(BezierSegment bezierSegment, float t)
        {
            var midPoint1 = Vector2.Lerp(bezierSegment.P0, bezierSegment.P1, t);
            var midPoint2 = Vector2.Lerp(bezierSegment.P1, bezierSegment.P2, t);
            var midPoint3 = Vector2.Lerp(bezierSegment.P2, bezierSegment.P3, t);

            var handle3 = Vector2.Lerp(midPoint1, midPoint2, t);
            var handle4 = Vector2.Lerp(midPoint2, midPoint3, t);
            var point3 = Vector2.Lerp(handle3, handle4, t);

            return new BezierSegment {P0 = point3, P1 = handle3, P2 = handle4};
        }
    }
}