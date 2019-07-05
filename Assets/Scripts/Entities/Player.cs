using System;
using GameEvents;
using GameField;
using UnityEngine;

namespace Entities
{
    public class Player : Entity
    {
        #region Variables

        #region PublicVariables

        [Header("Line renderer")]
        public float width;
        public float thickness;

        [Header("GamePlay")] public float speed;
        
        #endregion

        #region PrivateVariables

        private LineRenderer _lineRenderer;
        private EdgeCollider2D _edgeCollider2D;
        private int _intWidth;

        #endregion

        #endregion

        #region Properties

        public Vector2 Position => _lineRenderer.GetPosition(_intWidth / 2);
        public float FloatPosition => _position;

        #endregion

        #region MonoBehaviourMethods
        
        private void Awake()
        {
            Initialize();
            SubscribeToEvents();
            SetPosition(0);
        }
        
        #endregion

        #region Methods

        #region PublicMethods

        public override void Move(float direction)
        {
            SetPosition(_position + direction * speed);
        }

        public override void SetPosition(float position)
        {
            _position = Mathf.Clamp01(Remap01(position));
            SetLinePositions();
        }

        public void SetWidth(float newWidth)
        {
            _intWidth = GameFieldManager.Instance.GetDistanceInPoints(newWidth);
            _intWidth += _intWidth % 2 == 0 ? 0 : 1;
        }

        #endregion

        #region PrivateMethods
        
        /// <summary>
        /// Initialize the player
        /// </summary>
        private void Initialize()
        {
            SetWidth(width);

            if (_lineRenderer == null)
                _lineRenderer = GetComponent<LineRenderer>();
            if (_edgeCollider2D == null)
                _edgeCollider2D = GetComponent<EdgeCollider2D>();
            
            _lineRenderer.positionCount = _intWidth;
            _edgeCollider2D.points = new Vector2[_intWidth];

            _edgeCollider2D.edgeRadius = thickness/2;
            _lineRenderer.widthMultiplier = thickness;
        }
        
        private void SubscribeToEvents()
        {
            EventManager.StartListening(GameEvent.GAME_FIELD_CHANGED, () => SetPosition(_position));
        }

        /// <summary>
        /// Set the positions for the line renderer and edgeCollider
        /// </summary>
        private void SetLinePositions()
        {
            var positionIndex = GameFieldManager.Instance.GetPositionIndex(_position) - _intWidth / 2;

            var positions = new Vector2[_intWidth];

            for (var i = 0; i < _intWidth; i++)
            {
                var point = GameFieldManager.Instance.GetLerpedPoint(positionIndex++, _position);
                _lineRenderer.SetPosition(i, point);
                positions[i] = point;
            }

            _edgeCollider2D.points = positions;
        }

        /// <summary>
        /// Remap the value so that it's between 0 - 1
        /// </summary>
        /// <param name="value">The value to remap</param>
        /// <returns>The remapped value</returns>
        private static float Remap01(float value)
        {
            if (value > 1)
            {
                return Remap01(value - 1);
            }
            else if (value < 0)
            {
                return Remap01(value + 1);
            }
            
            return value;
        }

        #endregion

        #endregion

        #region Coroutines

        #endregion
    }
}