using System;
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
        private int hits;

        #endregion

        #endregion

        #region Properties

        #endregion

        #region MonoBehaviourMethods

        void Start()
        {
            velocity = Vector2.one;
        }

        void Update()
        {
            Move(velocity);
        }

        #endregion

        #region Methods

        #region PublicMethods

        public Vector3 HitPosition()
        {
            var hit = Physics2D.Raycast(transform.position, velocity.normalized, 100, LayerMask.GetMask("Wall"));
            return hit.point;
        }

        public void Move(Vector2 direction)
        {
            var hit = Physics2D.Raycast(transform.position, direction.normalized, minHitDistance);
            if (hit)
            {
                if (hit.transform.tag == "Player")
                {
                    hits++;
                    MenuStateManager.Instance.uiText.text = hits.ToString();
                }else
                {
                    hits = 0;
                    EventManager.TriggerEvent(GameEvent.WEAK_SCREEN_SHAKE);
                    MenuStateManager.Instance.uiText.text = hits.ToString();
                }
                velocity = Vector2.Reflect(direction, hit.normal);
                SetPosition((Vector2) transform.position + Vector2.Reflect(direction, hit.normal) * Time.deltaTime);
            }
            else
            {
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

        #endregion
    }
}