using System;
using GameEvents;
using GameField;
using UnityEditor.UIElements;
using UnityEngine;

namespace Entities
{
    public class Player:Entity
    {
        #region Variables

        #region PublicVariables

        [Header("Line renderer")]
        public LineRenderer lineRenderer;
        [SerializeField]
        private int _width;

        [Header("GamePlay")] public float speed;
        
        #endregion

        #region PrivateVariables

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
                
                lineRenderer.positionCount = _width;

            } 
        }

        #endregion

        #region MonoBehaviourMethods

        private void Awake()
        {
            lineRenderer.positionCount = _width;
//            SubscribeToEvents();
        }

        private void OnDrawGizmos()
        {
            if(Application.isPlaying) Gizmos.DrawSphere(GameFieldManager.Instance[_position], 0.1f);
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
            _position = FixPosition(position);
            SetLineRendererPositions();
        }
        
        

        #endregion

        #region PrivateMethods

        private void SetLineRendererPositions()
        {
            var positionIndex = GameFieldManager.Instance.GetPositionIndex(_position);
                
            for (var i = 0; i < _width; i++)
            {
                lineRenderer.SetPosition(i, GameFieldManager.Instance[positionIndex++]);
            }
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