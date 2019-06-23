using System;
using GameEvents;
using GameField;
using UnityEngine;

namespace Entities
{
    public class Player:Entity
    {
        #region Variables

        #region PublicVariables

        [Header("Line renderer")]
        
        [SerializeField]
        private int _width;

        [Header("GamePlay")] public float speed;

        [Header("Debug")] public bool debug;
        [Range(0,1)] public float positionSlider;
        
        #endregion

        #region PrivateVariables

        private LineRenderer _lineRenderer;
        private EdgeCollider2D _edgeCollider2D;

        #endregion

        #endregion

        #region Properties

        public int Width
        {
            get => _width;
            set
            {
                value = Mathf.Abs(value);
                if (value == 0) value = 2;
                value -= value % 2 == 0 ? 0 : 1;

                _width = value;
                
                _lineRenderer.positionCount = _width;

            } 
        }

        public Vector2 Position => _lineRenderer.GetPosition(_width / 2);
        public float FloatPosition => _position; 

        #endregion

        #region MonoBehaviourMethods

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _edgeCollider2D = GetComponent<EdgeCollider2D>();
            
            _lineRenderer.positionCount = _width;
            _edgeCollider2D.points = new Vector2[_width];
            
            SetPosition(0.75f);
//            SubscribeToEvents();
        }

        private void OnDrawGizmos()
        {
            if(Application.isPlaying) Gizmos.DrawSphere(GameFieldManager.Instance[_position], 0.02f);
        }

        private void OnValidate()
        {
            if(debug) SetPosition(positionSlider);
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
            _position = Mathf.Clamp01(FixPosition(position));
            SetLineRendererPositions();
        }

        #endregion

        #region PrivateMethods

        private void SetLineRendererPositions()
        {
            var positionIndex = GameFieldManager.Instance.GetPositionIndex(_position) - _width/2;

            var positions = new Vector2[_width];
            
            for (var i = 0; i < _width; i++)
            {
                var point = GameFieldManager.Instance.GetLerpValue(positionIndex++, _position);
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

            if(position < 0)
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