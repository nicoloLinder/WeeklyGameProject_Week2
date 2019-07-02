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

        [Header("Debug")] public bool debug;
        [Range(0, 1)] public float positionSlider;

        #endregion

        #region PrivateVariables

        private LineRenderer _lineRenderer;
        private EdgeCollider2D _edgeCollider2D;
        private int _intWidth;

        #endregion

        #endregion

        #region Properties

//        public int Width
//        {
//            get => _intWidth;
//            set
//            {
//                value = Mathf.Abs(value);
//                if (value == 0) value = 2;
//                value -= value % 2 == 0 ? 0 : 1;
//
//                _intWidth = value;
//
//                _lineRenderer.positionCount = _intWidth;
//            }
//        }

        public Vector2 Position => _lineRenderer.GetPosition(_intWidth / 2);
        public float FloatPosition => _position;

        #endregion

        #region MonoBehaviourMethods

        private void Awake()
        {
            Initialize();

            SetPosition(0.75f);
            SubscribeToEvents();
        }

//        private void OnDrawGizmos()
//        {
//            if (!Application.isPlaying)
//            {
//                Initialize();
//                if (debug) SetPosition(positionSlider);
//            }
//        }

//        private void OnValidate()
//        {
//            Initialize();
//            if (debug) SetPosition(positionSlider);
//            
//        }

        #endregion

        #region Methods

        #region PublicMethods

        public void SubscribeToEvents()
        {
            EventManager.StartListening(GameEvent.GAME_FIELD_CHANGED, () => SetPosition(_position));
        }

        public override void Move(float direction)
        {
            SetPosition(_position + direction * speed);
        }

        public override void SetPosition(float position)
        {
            _position = Mathf.Clamp01(FixPosition(position));
            SetLineRendererPositions();
        }

        public void SetWidth(float newWidth)
        {
            _intWidth = GameFieldManager.Instance.GetDistanceInPoints(newWidth);
            Debug.Log(GameFieldManager.Instance[_intWidth]);
            _intWidth += _intWidth % 2 == 0 ? 0 : 1;
        }

        #endregion

        #region PrivateMethods

        private void Initialize()
        {
            SetWidth(width);

            if (_lineRenderer == null)
                _lineRenderer = GetComponent<LineRenderer>();
            if (_edgeCollider2D == null)
                _edgeCollider2D = GetComponent<EdgeCollider2D>();
            
            _lineRenderer.positionCount = _intWidth;
            _edgeCollider2D.points = new Vector2[_intWidth];
        }

        private void SetLineRendererPositions()
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

        private float FixPosition(float position)
        {
            if (position > 1)
            {
                return FixPosition(position - 1);
            }

            if (position < 0)
            {
                return FixPosition(position + 1);
            }

            return position;
        }

        #endregion

        #endregion

        #region Coroutines

        #endregion
    }
}