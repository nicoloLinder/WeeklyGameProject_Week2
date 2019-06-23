using System.Collections;
using FSM;
using GameEvents;
using UnityEngine;

namespace Entities
{
    public class Ball : MonoBehaviour
    {
        #region Variables

        #region PublicVariables

        public float minHitDistance;

        #endregion

        #region PrivateVariables

        private Vector2 velocity;
        private bool moving;

        #endregion

        #endregion

        #region Properties

        #endregion

        #region MonoBehaviourMethods

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
                }else
                {
                    EventManager.TriggerEvent(GameEvent.BALL_WALL_HIT);
                }
                velocity = Vector2.Reflect(direction, hit.normal);
                SetPosition((Vector2) transform.position + Vector2.Reflect(direction, hit.normal) * Time.deltaTime);
            }
            else
            {
//                hit = Physics2D.Raycast(transform.position, direction.normalized, 100);
//                if (hit.point == Vector2.zero)
//                {
//                    velocity = -velocity;
//                }
                SetPosition((Vector2) transform.position + direction * Time.deltaTime);
            }
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        #endregion

        #region PrivateMethods

        #endregion

        #endregion

        #region Coroutines

        private IEnumerator Throw()
        {
            while (moving)
            {
                Move(velocity);
                yield return null;
            }
        }

        #endregion
    }
}