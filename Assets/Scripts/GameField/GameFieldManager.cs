using System;
using UnityEngine;

namespace GameField
{
    public class GameFieldManager : Singleton<GameFieldManager>
    {
        #region Variables

        #region PublicVariables

        [Header("Path Shape")] public PathFunction pathFunction;
        public int detail;
        public float minPathPointsDistance;

        #endregion

        #region PrivateVariables

        private Path _path;
        private MeshFilter _meshFilter;

        #endregion

        #endregion

        #region Properties

        public Vector2 this[float index] => _path[GetPositionIndex(index)];

        public Vector2 this[int index]
        {
            get
            {
                if (index > _path.Length) index -= _path.Length;
                else if (index > _path.Length - 1) index -= _path.Length;

                return _path[index];
            }
        }

        #endregion

        #region MonoBehaviourMethods

        protected override void Awake()
        {
            base.Awake();
            Initialize();
            GenerateGameField();
        }

        private void Start()
        {
        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }

        private void OnDestroy()
        {
        }

        private void OnDrawGizmos()
        {
            if (_path == null) return;
            foreach (Vector3 VARIABLE in _path)
            {
                Gizmos.DrawSphere( VARIABLE, 0.01f);
            }
        }

        #endregion

        #region Methods

        #region PublicMethods

        public void GenerateGameField()
        {
            _path = new Path(minPathPointsDistance, detail, pathFunction);
            _meshFilter.mesh = MeshGenerator.GenerateMesh(_path);
        }

        public int GetPositionIndex(float floatIndex)
        {
            return Mathf.FloorToInt(floatIndex % 1 * _path.Length);
        }

        public float GetPercentageBetweenPoints(float floatIndex)
        {
            var top = Mathf.Ceil(floatIndex % 1 * _path.Length);
            var bottom = Mathf.Floor(floatIndex % 1 * _path.Length);
            var value = floatIndex % 1 * _path.Length;
            
            return Mathf.Abs(top-bottom) > 0 ?  (value - bottom) / (top - bottom) : 0;
        }

        public Vector2 GetLerpValue(int index, float floatIndex)
        {
            var percentage = GetPercentageBetweenPoints(floatIndex);
            return Vector2.Lerp(this[index], this[index+1], percentage);
        }

        #endregion

        #region PrivateMethods

        private void Initialize()
        {
            _meshFilter = GetComponent<MeshFilter>();
        }

        #endregion

        #endregion

        #region Coroutines

        #endregion
    }
}