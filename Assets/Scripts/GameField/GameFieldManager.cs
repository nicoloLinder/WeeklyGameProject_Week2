using System.Linq;
using GameEvents;
using UnityEngine;

namespace GameField
{
    public class GameFieldManager : Singleton<GameFieldManager>
    {
        #region Variables

        #region PublicVariables

        [Header("Path Shape")] public Path path;

        public Mesh debugMesh;
        
        [HideInInspector]
        public bool pathFoldout;

        #endregion

        #region PrivateVariables

        private MeshFilter _meshFilter;
        private EdgeCollider2D _edgeCollider2D;

        #endregion

        #endregion

        #region Properties

        public Vector2 this[float index] => path[GetPositionIndex(index)];

        public Vector2 this[int index]
        {
            get
            {
                if (index < 0) index += path.Length;
                else if (index > path.Length - 1) index -= path.Length;

                return path[index];
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
            if (path == null) return;
            var index = 0;
            foreach (Vector3 VARIABLE in path)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(VARIABLE, 0.001f);
            }

            for (var i = 0; i < path.Length; i++)
            {
                var nextIndex = i + 1;
                if (nextIndex > path.Length - 1)
                {
                    nextIndex = 0;
                }
                else if (nextIndex < 0)
                {
                    nextIndex = path.Length - 1;
                }

                var prevIndex = i - 1;
                if (prevIndex > path.Length - 1)
                {
                    prevIndex = 0;
                }
                else if (prevIndex < 0)
                {
                    prevIndex = path.Length - 1;
                }

                var currentToPrev = (path[prevIndex] - path[i]);
                var currentToNext = (path[nextIndex] - path[i]);

                var normalPervCurrent =
                    new Vector2(currentToPrev.normalized.y, -currentToPrev.normalized.x);
                var normalNextCurrent =
                    new Vector2(-currentToNext.normalized.y, currentToNext.normalized.x);

                Gizmos.color = Color.blue;
                Gizmos.DrawRay(path[i], currentToPrev / 10);
                Gizmos.color = Color.cyan;
                Gizmos.DrawRay(path[i], normalPervCurrent / 100);

                Gizmos.color = Color.red;
                Gizmos.DrawRay(path[i], currentToNext / 10);
                Gizmos.color = Color.magenta;
                Gizmos.DrawRay(path[i], normalNextCurrent / 100);

                Gizmos.color = Color.white;
                Gizmos.DrawRay(path[i], (normalNextCurrent + normalPervCurrent) / 100);
            }

            Gizmos.color = new Color(0, 0, 0, 0.1f);
//            Gizmos.DrawMesh(debugMesh, path.Barycenter, path.MaxRadius);
            Gizmos.DrawMesh(debugMesh, path.Barycenter, Quaternion.identity, Vector3.one * path.MaxRadius*2);
//            Gizmos.DrawSphere(path.Barycenter, path.MinRadius);
            Gizmos.DrawMesh(debugMesh, path.Barycenter, Quaternion.identity, Vector3.one * path.MinRadius*2);
            
        }

        #endregion

        #region Methods

        #region PublicMethods

        public void SetPath(Path path)
        {
            this.path = path;
            GenerateGameField();

            EventManager.TriggerEvent(GameEvent.GAME_FIELD_CHANGED);
        }

        public int ClosestIndex(Vector2 point)
        {
            var closestPoint = path.PathCopy.OrderBy(vector => Vector2.Distance(point, vector)).First();
            return path.PathCopy.IndexOf(closestPoint);
        }

        public void GenerateGameField()
        {
            path.GeneratePath();
            _meshFilter.mesh = MeshGenerator.GenerateMesh(path);
            var points = new Vector2[path.Length + 1];

            for (int i = 0; i < path.Length; i++)
            {
                points[i] = path.GetWallPoint(i);
            }

            points[path.Length] = path[0];
            _edgeCollider2D.points = points;

//            Camera.main.orthographicSize = path.MaxRadius * 2;
        }

        public int GetPositionIndex(float floatIndex)
        {
            return Mathf.FloorToInt(floatIndex % 1 * path.Length);
        }

        public float GetFloatIndex(int index)
        {
            return (float) index / path.Length;
        }

        public float GetPercentageBetweenPoints(float floatIndex)
        {
            var top = Mathf.Ceil(floatIndex % 1 * path.Length);
            var bottom = Mathf.Floor(floatIndex % 1 * path.Length);
            var value = floatIndex % 1 * path.Length;

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