using System.Collections.Generic;
using FSM;
using GameField;
using Unity.VectorGraphics;
using UnityEngine;

namespace DefaultNamespace
{
    public class Metaball : MonoBehaviour
    {
        #region Variables

        #region PublicVariables

        public float ballRadius;


        public float v;

        public bool attached;

        public float distanceBeforeDetach;
        public float distanceBeforeDissolve;

        public bool spriteVisualization;

        #endregion

        #region PrivateVariables

        private MeshFilter meshFilter;
        protected MeshRenderer meshRenderer;
        protected SpriteRenderer spriteRenderer;

        #endregion

        #endregion

        #region Properties

        private Vector2 Center => transform.position;

        #endregion

        #region MonoBehaviourMethods

        private void Awake()
        {
            Initialize();
        }

        void LateUpdate()
        {
            CreateMetaball(MenuStateManager.Instance.ball.radius, MenuStateManager.Instance.ball.transform.position);
        }

//        private void OnValidate()
//        {
//            if (!enabled) return;
//            Initialize();
//            CreateMetaball(MenuStateManager.Instance.ball.radius, MenuStateManager.Instance.ball.transform.position);
//        }

//        private void OnDrawGizmos()
//        {
//            if (!enabled) return;
//            if (!Application.isPlaying)
//            {
//                Initialize();
//                CreateMetaball(MenuStateManager.Instance.ball.radius,
//                    MenuStateManager.Instance.ball.transform.position);
//            }
//        }

        #endregion

        #region Methods

        #region PublicMethods

        public virtual bool CreateMetaball(float radius1, Vector2 center1)
        {
            var distance = Vector2.Distance(center1, Center);

            float u1;
            float u2;

            //    Check if balls are intersecting

            if (distance > (attached ? distanceBeforeDetach : distanceBeforeDissolve) ||
                distance <= Mathf.Abs(radius1 - ballRadius))
            {
                attached = false;
                meshRenderer.enabled = false;
                return false;
            }

            if (distance < radius1 + ballRadius)
            {
                // case circles are overlapping
                u1 = Mathf.Acos((radius1 * radius1 + distance * distance - ballRadius * ballRadius) /
                                (2 * radius1 * distance));
                u2 = Mathf.Acos((ballRadius * ballRadius + distance * distance - radius1 * radius1) /
                                (2 * ballRadius * distance));
            }
            else
            {
                u1 = 0;
                u2 = 0;
            }

            attached = true;
            meshRenderer.enabled = true;

            //    Calculate all angles needed

            var angleBetweenCenters = AngleBetweenCenters(Center, center1);
            var maxSpread = Mathf.Acos((radius1 - ballRadius) / distance);

            // Circle 1 (left)
            var angle1 = angleBetweenCenters + u1 + (maxSpread - u1) * v;
            var angle2 = angleBetweenCenters - (u1 + (maxSpread - u1) * v);
            // Circle 2 (right)
            var angle3 = angleBetweenCenters + Mathf.PI - u2 - (Mathf.PI - u2 - maxSpread) * v;
            var angle4 = angleBetweenCenters - (Mathf.PI - u2 - (Mathf.PI - u2 - maxSpread) * v);

            //    Calculate the four bezier points

            var point1 = GetPoint(center1, angle1, radius1);
            var point2 = GetPoint(center1, angle2, radius1);
            var point3 = GetPoint(Center, angle3, ballRadius);
            var point4 = GetPoint(Center, angle4, ballRadius);

            //    Calculate the four handles

            var totalRadius = radius1 + ballRadius;

            var d2 = Mathf.Min(v * 10, Vector2.Distance(point1, point3) / totalRadius);

            var r1 = radius1 * d2;
            var r2 = ballRadius * d2;

            var handle1 = GetPoint(point1, angle1 - Mathf.PI / 2, r1);
            var handle2 = GetPoint(point2, angle2 + Mathf.PI / 2, r1);
            var handle3 = GetPoint(point3, angle3 + Mathf.PI / 2, r2);
            var handle4 = GetPoint(point4, angle4 - Mathf.PI / 2, r2);

            //    Define the bezier segments

            var bezierSegments = new[]
            {
                new BezierPathSegment
                {
                    P0 = point1 - (Vector2)transform.parent.position,
                    P1 = handle1- (Vector2)transform.parent.position,
                    P2 = handle3- (Vector2)transform.parent.position
                },
                new BezierPathSegment
                {
                    P0 = point3- (Vector2)transform.parent.position,
                    P1 = point3- (Vector2)transform.parent.position,
                    P2 = point4- (Vector2)transform.parent.position
                },
                new BezierPathSegment
                {
                    P0 = point4- (Vector2)transform.parent.position,
                    P1 = handle4- (Vector2)transform.parent.position,
                    P2 = handle2- (Vector2)transform.parent.position
                },
                new BezierPathSegment
                {
                    P0 = point2- (Vector2)transform.parent.position,
                    P1 = point2- (Vector2)transform.parent.position,
                    P2 = point1- (Vector2)transform.parent.position
                }
            };

            //    Define the bezier contour for the shape

            var bezierContour = new BezierContour
            {
                Segments = bezierSegments,
                Closed = false
            };

            //    Unite everything together

            var vectorScene = new Scene
            {
                Root = new SceneNode
                {
                    Shapes = new List<Shape>
                    {
                        new Shape
                        {
                            Contours = new[] {bezierContour},
                            Fill = new SolidFill()
                        }
                    }
                }
            };

            var tessellationOptions = new VectorUtils.TessellationOptions
            {
                MaxCordDeviation = float.MaxValue,
                MaxTanAngleDeviation = Mathf.PI / 2.0f,
                SamplingStepSize = 0.5f,
                StepDistance = 0.01f
            };

            VectorUtils.TessellateScene(vectorScene, tessellationOptions);
            var geometry = VectorUtils.TessellateScene(vectorScene, tessellationOptions);

            VectorUtils.FillMesh(meshFilter.mesh, geometry, 1f);

//            var mesh = meshFilter.sharedMesh;

            return true;
        }

        #endregion

        #region PrivateMethods

        protected void Initialize()
        {
            if (!spriteVisualization)
            {
                if (meshFilter == null)
                {
                    meshFilter = GetComponent<MeshFilter>();
                }

                if (meshRenderer == null)
                {
                    meshRenderer = GetComponent<MeshRenderer>();
                }
            }else
            {
                if (spriteRenderer == null)
                {
                    spriteRenderer = GetComponent<SpriteRenderer>();
                }
            }
        }

        protected float AngleBetweenCenters(Vector2 pointA, Vector2 pointB)
        {
            return Mathf.Atan2(pointA.y - pointB.y, pointA.x - pointB.x);
        }

        protected Vector2 GetPoint(Vector2 point, float angle, float radius)
        {
            return new Vector2(point.x + radius * Mathf.Cos(angle), point.y + radius * Mathf.Sin(angle));
        }

        #endregion

        #endregion

        #region Coroutines

        #endregion
    }
}