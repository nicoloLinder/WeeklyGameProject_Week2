using System.Collections;
using GameEvents;
using GameField;
using UnityEngine;

namespace Entities
{
    public class Ball : MonoBehaviour
    {
        #region Variables

        #region PublicVariables

        public float minHitDistance = 0.01f;
        public float radius = 0.15f;
        public float speed;

        public float maxSpeedDivider;

        public float maxSquashDistance;

        #endregion

        #region PrivateVariables

        private Vector2 _velocity;
        private Vector2 _playerVelocityOnImpact;
        private bool moving;
        public bool squishing;

        #endregion

        #endregion

        #region Properties

        public float DistanceToPlayer
        {
            get;
            private set;
        }

        #endregion

        #region MonoBehaviourMethods

        private void Start()
        {
            Initialize();
        }

        #endregion

        #region Methods

        #region PublicMethods

        /// <summary>
        /// Calculate the expected hit position on the wall
        /// </summary>
        /// <returns>The point of impact of the ball with the current velocity</returns>
        public Vector3 ExpectedHitPosition()
        {
            var direction = _velocity.normalized;
            var hit = Physics2D.Raycast((Vector2)transform.position + direction * radius, direction, 100, LayerMask.GetMask("Wall"));
            return hit.point;
        }
        
        public Vector3 ExpectedHitPositionFromCenter()
        {
            var direction = _velocity.normalized;
            var hit = Physics2D.Raycast(transform.position, direction, 100, LayerMask.GetMask("Wall"));
            return hit.point;
        }

        /// <summary>
        /// Start the perpetual motion of the ball
        /// </summary>
        /// <param name="startingVelocity">The starting velocity</param>
        public void ThrowBall(Vector2 startingVelocity)
        {
            _velocity = startingVelocity;
            moving = true;
            
            StartCoroutine(MovementCoroutine());
        }

        public void AddForce(Vector2 force)
        {
            var magnitude = _velocity.magnitude;
            
            _velocity += force;
            _velocity.Normalize();

            _velocity *= magnitude;


        }

        /// <summary>
        /// Move the ball with the current velocity
        /// </summary>
        private void Move()
        {
//            if (squishing) return;
            var direction = _velocity.normalized;
            var hit = Physics2D.Raycast(transform.position, direction, 10);
            var reverseHit = Physics2D.Raycast(transform.position, -direction, 2*radius);

            squishing = false;
            
            DistanceToPlayer = 100;
            
            if (hit)
            {
                if (hit.transform.CompareTag("Player"))
                {
                    DistanceToPlayer = Mathf.Clamp(hit.distance, maxSquashDistance, float.MaxValue);
                    if (hit.distance< minHitDistance)
                    {
                        EventManager.TriggerEvent(GameEvent.BALL_PLAYER_HIT);
//                    StartCoroutine(SquashCoroutine());
                        AddForce(_playerVelocityOnImpact/5);
                        _playerVelocityOnImpact = Vector2.zero;
                        _velocity = Vector2.Reflect(_velocity, hit.normal);
                    }

                    if (hit.distance < radius)
                    {
                        if (!squishing)
                        {
                            squishing = true;
                            _playerVelocityOnImpact = GameManager.Player.Velocity.normalized;
                        }
                        var distancePercentage = (hit.distance + radius - minHitDistance) / (radius - minHitDistance);
                        
                        
                        SetPosition((Vector2) transform.position + Time.deltaTime * speed * _velocity / (Mathf.Lerp(1,maxSpeedDivider,distancePercentage)));
                        transform.localRotation = Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.up, hit.normal));
                        return;
                    }
                    
//                    Debug.Break();
                   

                }
            }
            
            if (reverseHit)
            {
                if (reverseHit.transform.CompareTag("Player"))
                {
                    DistanceToPlayer = reverseHit.distance;
                }
            }
            
            transform.localRotation = Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.up, direction));

            SetPosition((Vector2) transform.position + Time.deltaTime * speed * _velocity);
        }

        /// <summary>
        /// Set the ball position
        /// </summary>
        /// <param name="position">The new ball position</param>
        private void SetPosition(Vector2 position)
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

        /// <summary>
        /// Initialize the ball 
        /// </summary>
        private void Initialize()
        {
            transform.localScale = radius * 2 * Vector3.one;
            ResetBall();
        }

        /// <summary>
        /// Reset the ball to be thrown outside the field toward the player
        /// </summary>
        private void ResetBall()
        {
            transform.position = -GameManager.Player.Position.normalized * 2;
            _velocity = GameManager.Player.Position.normalized * 2;
        }

        #endregion

        #endregion

        #region Coroutines

        /// <summary>
        /// Movement coroutine, moves the ball each frame as long as the ball is set to moving
        /// </summary>
        /// <returns></returns>
        private IEnumerator MovementCoroutine()
        {
            while (moving)
            {
                Move();
                yield return null;
            }
        }

        private IEnumerator SquashCoroutine()
        {
            squishing = true;
            var direction = _velocity.normalized;
            var hit = Physics2D.Raycast(transform.position, direction, 100);
            var startingScale = transform.localScale;
            
//            transform.Rotate(Vector3.forward, -Vector2.Angle(Vector2.up, direction));
//            transform.localRotation = Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.up, hit.normal));
            var squishPercentage = 0f;

            Debug.Log(hit.distance > maxSquashDistance);
            
            while (hit.distance > maxSquashDistance)
            {
                hit = Physics2D.Raycast(transform.position, direction, 100);
                
//                transform.localScale = new Vector2(transform.localScale.x, Mathf.Clamp(hit.distance, maxSquashDistance, radius));
                
                squishPercentage = Mathf.Clamp01(hit.distance - maxSquashDistance) / (radius - maxSquashDistance);
                var actualVeocity = Time.deltaTime * speed * _velocity;
                
                SetPosition((Vector2) transform.position + actualVeocity - actualVeocity * squishPercentage);
                
                
                Debug.Log("Squishing");
                
                yield return null;
            }

            _velocity = Vector2.Reflect(_velocity, hit.normal);
            direction = -_velocity.normalized;
            
            while (hit.distance < radius)
            {
                hit = Physics2D.Raycast(transform.position, direction, 100);
                
//                transform.localScale = new Vector2(transform.localScale.x, Mathf.Clamp(hit.distance, maxSquashDistance, radius));
                
                squishPercentage = (hit.distance - maxSquashDistance) / (radius - maxSquashDistance);
                var actualVeocity = Time.deltaTime * speed * _velocity;
                
                SetPosition((Vector2) transform.position + actualVeocity - actualVeocity * squishPercentage);

                yield return null;
            }

            transform.localScale = startingScale;

            squishing = false;
        }

        #endregion
    }
}