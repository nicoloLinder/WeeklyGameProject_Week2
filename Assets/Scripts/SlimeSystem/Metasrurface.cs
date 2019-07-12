using System;
using System.Collections.Generic;
using FSM;
using GameField;
using SlimeSystem;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class Metasrurface : Metaball
    {
        #region Variables

        #region PublicVariables

        #endregion

        #region PrivateVariables

        #endregion

        #endregion

        #region Properties

        public IndexedVector2 center; // { get; set; }

        private int centerIndex;
        private int point1Index;
        private int point2Index;

        private float lastD2;

        #endregion

        #region MonoBehaviourMethods

        private void LateUpdate()
        {
            var ballPosition = GameManager.Ball.transform.position;

            if (!Attached)
            {
                center = GameFieldManager.Instance.GetPointOnWall(GameManager.Ball.ExpectedHitPositionFromCenter());

                point1Index = center.index + GameFieldManager.Instance.GetDistanceInPoints(ballRadius);
                point2Index = center.index + GameFieldManager.Instance.GetDistanceInPoints(-ballRadius);
            }

            SetRendererEnabled(CreateMetaball(GameManager.Ball.radius, ballPosition));
        }

        #endregion

        #region Methods

        #region PublicMethods

        public override bool CreateMetaball(float radius1, Vector2 center1)
        {
            var distance = Vector2.Distance(center1, center);

            float u1;

            var point1 = GameFieldManager.Instance.GetPointOnWall(point1Index);
            var point2 = GameFieldManager.Instance.GetPointOnWall(point2Index);

            var perpendicularLine = Vector2.Perpendicular(point1 - point2);

            var point5 = center1 + perpendicularLine.normalized * radius1;

            //    Check if balls are intersecting

            if (distance > (Attached ? 100 : distanceBeforeDissolve))
            {
                return false;
            }

            if (distance < radius1 + ballRadius)
            {
                if (CheckSide(point2, point1, point5) > 0)
                {
                    return false;
                }

                // case circles are overlapping
                u1 = Mathf.Acos((radius1 * radius1 + distance * distance - ballRadius * ballRadius) /
                                (2 * radius1 * distance));
            }
            else
            {
                if (CheckSide(point2, point1, point5) > 0)
                {
                    return false;
                }

                u1 = 0;
            }

            //    Calculate all angles needed

            var angleBetweenCenters = AngleBetweenCenters(center, center1);
            var maxSpread = Mathf.Acos((radius1 - ballRadius) / distance);

            // Circle 1 (left)
            var angle1 = angleBetweenCenters + u1 + (maxSpread - u1) * v;
            var angle2 = angleBetweenCenters - (u1 + (maxSpread - u1) * v);

            //    Calculate the four bezier points

            var point3 = GetPoint(center1, angle1, radius1);
            var point4 = GetPoint(center1, angle2, radius1);


            var tangentPoint1 = GameFieldManager.Instance.GetPointOnWall(point1Index - 1);
            var tangentPoint2 = GameFieldManager.Instance.GetPointOnWall(point2Index + 1);
            //    Calculate the four handles

            var totalRadius = radius1 + ballRadius;

            var d2 = Mathf.Min(v * 10f, Vector2.Distance(point1, point3) / totalRadius);

            if (float.IsNaN(d2))
            {
                d2 = lastD2;
            }

            lastD2 = d2;

            var r1 = radius1 * d2;
            var r2 = ballRadius * d2;

            //    Handle point 1 Right surface
            var handle1 = GetPoint(point1, AngleBetweenCenters(tangentPoint1, point1), r1);
            //    Handle point 2 Left surface
            var handle2 = GetPoint(point2, AngleBetweenCenters(tangentPoint2, point2), r1);
            //    Handle point 3 Right Ball
            var handle3 = GetPoint(point3, angle1 - Mathf.PI / 2, r2);
            //    Handle point 4 Left Ball
            var handle4 = GetPoint(point4, angle2 + Mathf.PI / 2, r2);

            //    Handle point 5 Right
            var handle5 = point5 + Vector2.Perpendicular(point5).normalized * radius1;
            //    Handle point 5 Left
            var handle6 = point5 - Vector2.Perpendicular(point5).normalized * radius1;

            //    Define the bezier segments
            var numberOfPoints = point1Index - point2Index;

            int index;

            BezierPathSegment[] bezierSegments;

            if (distance <= Mathf.Abs(radius1 - ballRadius))
            {
                bezierSegments = new BezierPathSegment[2 + numberOfPoints];
//                return true;
                bezierSegments[0] = new BezierPathSegment
                {
                    P0 = point1,
                    P1 = handle1,
                    P2 = handle5
                };
                bezierSegments[1] = new BezierPathSegment
                {
                    P0 = point5,
                    P1 = handle6,
                    P2 = handle2
                };
                index = 2;
            }
            else
            {
                if (BezierCurveUtils.CheckIBezierCurveIntersection(
                    new BezierSegment {P0 = point1, P1 = handle1, P2 = handle3, P3 = point3},
                    new BezierSegment {P0 = point2, P1 = handle2, P2 = handle4, P3 = point4}))
                {
                    return false;
                }
                bezierSegments = new BezierPathSegment[3 + numberOfPoints];
                bezierSegments[0] = new BezierPathSegment
                {
                    P0 = point1,
                    P1 = handle1,
                    P2 = handle3
                };
                bezierSegments[1] = new BezierPathSegment
                {
                    P0 = point3,
                    P1 = point3,
                    P2 = point4
                };
                bezierSegments[2] = new BezierPathSegment
                {
                    P0 = point4,
                    P1 = handle4,
                    P2 = handle2
                };
                index = 3;
            }

            for (var i = 0; i < numberOfPoints; i++)
            {
                var bezierPointToAdd = GameFieldManager.Instance.GetPointOnWall(point2Index + i);
                var nextBezierPoint = GameFieldManager.Instance.GetPointOnWall(point2Index + i + 1);
                bezierSegments[index++] = new BezierPathSegment
                {
                    P0 = bezierPointToAdd,
                    P1 = bezierPointToAdd,
                    P2 = nextBezierPoint
                };
            }

            var bezierContour = new BezierContour
            {
                Segments = bezierSegments,
                Closed = false
            };

            //    Draw the bezier curve

            GenerateBezierCurve(new[] {bezierContour});

            return true;
        }

        private static int CheckSide(Vector2 pointA, Vector2 pointB, Vector2 position)
        {
            return (int) Mathf.Sign((position.x - pointA.x) * (pointB.y - pointA.y) -
                                    (position.y - pointA.y) * (pointB.x - pointA.y));
        }

        #endregion

        #region PrivateMethods

        #endregion

        #endregion

        #region Coroutines

        #endregion
    }
}