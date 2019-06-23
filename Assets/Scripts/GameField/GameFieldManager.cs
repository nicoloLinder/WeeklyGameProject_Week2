using System.Linq;
using UnityEngine;

namespace GameField
{
    public class GameFieldManager : Singleton<GameFieldManager>
    {
        #region Variables

        #region PublicVariables

        [Header("Path Shape")]
        public Path _path;

        #endregion

        #region PrivateVariables

        
        private MeshFilter _meshFilter;
        private EdgeCollider2D _edgeCollider2D;

        #endregion

        #endregion

        #region Properties

        public Vector2 this[float index] => _path[GetPositionIndex(index)];

        public Vector2 this[int index]
        {
            get
            {
                if (index < 0) index += _path.Length;
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
            var index = 0;
            foreach (Vector3 VARIABLE in _path)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(VARIABLE, 0.001f);
            }

            for (var i = 0; i < _path.Length; i++)
            {
                var nextIndex = i + 1;
                if (nextIndex > _path.Length - 1)
                {
                    nextIndex = 0;
                }
                else if (nextIndex < 0)
                {
                    nextIndex = _path.Length - 1;
                }

                var prevIndex = i - 1;
                if (prevIndex > _path.Length - 1)
                {
                    prevIndex = 0;
                }
                else if (prevIndex < 0)
                {
                    prevIndex = _path.Length - 1;
                }

                var currentToPrev = (_path[prevIndex] - _path[i]);
                var currentToNext = (_path[nextIndex] - _path[i]);

                var normalPervCurrent =
                    new Vector2(currentToPrev.normalized.y, -currentToPrev.normalized.x);
                var normalNextCurrent =
                    new Vector2(-currentToNext.normalized.y, currentToNext.normalized.x);

                Gizmos.color = Color.blue;
                Gizmos.DrawRay(_path[i], currentToPrev / 10);
                Gizmos.color = Color.cyan;
                Gizmos.DrawRay(_path[i], normalPervCurrent / 100);

                Gizmos.color = Color.red;
                Gizmos.DrawRay(_path[i], currentToNext / 10);
                Gizmos.color = Color.magenta;
                Gizmos.DrawRay(_path[i], normalNextCurrent / 100);
                
                Gizmos.color = Color.white;
                Gizmos.DrawRay(_path[i], (normalNextCurrent + normalPervCurrent)/ 100);
            }
        }

        #endregion

        #region Methods

        #region PublicMethods

        public int ClosestIndex(Vector2 point)
        {
            var closestPoint = _path.PathCopy.OrderBy(vector => Vector2.Distance(point, vector)).First();
            return _path.PathCopy.IndexOf(closestPoint);
        }

        public void GenerateGameField()
        {
            _path.GeneratePath();
            _meshFilter.mesh = MeshGenerator.GenerateMesh(_path);
            var points = new Vector2[_path.Length + 1];
            
            for (int i = 0; i < _path.Length; i++)
            {
                points[i] = _path.GetWallPoint(i);
            }

            points[_path.Length] = _path[0];
            _edgeCollider2D.points = points;
        }

        public int GetPositionIndex(float floatIndex)
        {
            return Mathf.FloorToInt(floatIndex % 1 * _path.Length);
        }
        
        public float GetFloatIndex(int index)
        {
            return (float)index / _path.Length;
        }

        public float GetPercentageBetweenPoints(float floatIndex)
        {
            var top = Mathf.Ceil(floatIndex % 1 * _path.Length);
            var bottom = Mathf.Floor(floatIndex % 1 * _path.Length);
            var value = floatIndex % 1 * _path.Length;

            return Mathf.Abs(top - bottom) > 0 ? (value - bottom) / (top - bottom) : 0;
        }

        public Vector2 GetLerpValue(int index, float floatIndex)
        {
            var percentage = GetPercentageBetweenPoints(floatIndex);
            return Vector2.Lerp(this[index], this[index + 1], percentage);
        }

        #endregion

        #region PrivateMethods

        private void Initialize()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _edgeCollider2D = GetComponent<EdgeCollider2D>();
        }

        #endregion

        #endregion

        #region Coroutines

        #endregion
    }
}