using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using FSM;
using GameEvents;
using GameField;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;
using BezierContour = Unity.VectorGraphics.BezierContour;

namespace Entities
{
    public class Ball : MonoBehaviour
    {
        #region Variables

        #region PublicVariables

        public float minHitDistance;
        public float radius = 0.15f;
        public float speed;

        #endregion

        #region PrivateVariables

        public Vector2 velocity;
        private bool moving;

        #endregion

        #endregion

        #region Properties

        #endregion

        #region MonoBehaviourMethods

        void Start()
        {
            transform.localScale = Vector3.one * radius * 2;
        }

        #endregion

        #region Methods

        #region PublicMethods

        public Vector3 HitPosition()
        {
            var hit = Physics2D.Raycast(transform.position, velocity.normalized, 100, LayerMask.GetMask("Wall"));
            return hit.point;
        }

        public void ThrowBall(Vector2 direction)
        {
            velocity = direction;
            moving = true;
            StartCoroutine(Throw());
        }

        public void Move(Vector2 direction)
        {
            var hit = Physics2D.Raycast(transform.position, direction.normalized, minHitDistance);
            if (hit)
            {
                if (hit.transform.tag == "Player")
                {
                    EventManager.TriggerEvent(GameEvent.BALL_PLAYER_HIT);

                    velocity = Vector2.Reflect(direction, hit.normal);
                    SetPosition((Vector2) transform.position + Vector2.Reflect(direction, hit.normal) * Time.deltaTime * speed);
                    return;
                }

//                else
//                {
////                    ResetBall();
////                    EventManager.TriggerEvent(GameEvent.BALL_WALL_HIT);
////                    velocity = Vector2.Reflect(direction, hit.normal);
//                    break;
////                    return;
//                }
            }

//                hit = Physics2D.Raycast(transform.position, direction.normalized, 100);
//                if (hit.point == Vector2.zero)
//                {
//                    velocity = -velocity;
//                }
            SetPosition((Vector2) transform.position + direction * Time.deltaTime * speed);
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;

            if (Vector2.Distance(transform.position, GameFieldManager.Instance.Barycenter) >
                GameFieldManager.Instance.MaxRadius * 2)
            {
                ResetBall();
            }
        }

        #endregion

        #region PrivateMethods

        private void ResetBall()
        {
            transform.position = -PlayStateManager.Instance.player.Position.normalized * 2;
            velocity = PlayStateManager.Instance.player.Position.normalized * 2;
        }

        #endregion

        #endregion

        #region Coroutines

        private IEnumerator Throw()
        {
//            yield break;
            while (moving)
            {
                Move(velocity);
//                if (Metaball.CreateMetaball(radius, metaballRadius, transform.position, Vector2.zero, v,
//                    ref mesh, ref attached, distanceBeforeDetach, distanceBeforeDissolve))
//                {
//                    bezierMesh.gameObject.SetActive(true);
//                    bezierMesh.mesh = mesh;
//                }
//                else
//                {
//                    bezierMesh.gameObject.SetActive(false);
//                }


                yield return null;
            }
        }

        #endregion
    }
}