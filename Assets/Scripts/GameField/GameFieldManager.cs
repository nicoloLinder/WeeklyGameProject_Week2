using System.Linq;
using FSM;
using GameEvents;
using UnityEngine;
using Utilities;

namespace GameField
{
    public class GameFieldManager : Singleton<GameFieldManager>
    {
        #region Variables

        #region PublicVariables

        [Header("Path Shape")] public Path path;

        [HideInInspector]
        public bool pathFoldout;

        #endregion

        #region PrivateVariables

        private MeshFilter _meshFilter;
        private EdgeCollider2D _edgeCollider2D;

        #endregion

        #endregion

        #region Properties

        public IndexedVector2 this[float index] => path[GetPositionIndex(index)];

        public IndexedVector2 this[int index]
        {
            get
            {
                if (index < 0) index += path.Length;
                else if (index > path.Length - 1) index -= path.Length;

                return path[index];
            }
        }

//        public int this[Vector2 point] => path.IndexOf(point); 

        public Vector2 Barycenter => path.Barycenter;
        public float MaxRadius => path.MaxRadius;

        public float Offset => MenuStateManager.Instance.player.thickness;

        public int Length => path.Length;

        #endregion

        #region MonoBehaviourMethods

        protected override void Awake()
        {
            base.Awake();
            Initialize();
            GenerateGameField();
        }

        private void OnValidate()
        {
            Initialize();
        }

//        private void OnDrawGizmos()
//        {
//            if (path == null) return;
//            
//            foreach (var point in path)
//            {
//                Gizmos.color = Color.black;
//                Gizmos.DrawSphere(point, 0.01f);
//            }
//
//            Gizmos.color = new Color(0, 0, 0, 0.1f);
////            Gizmos.DrawMesh(debugMesh, path.Barycenter, Quaternion.identity, Vector3.one * path.MaxRadius*2);
////            Gizmos.DrawMesh(debugMesh, path.Barycenter, Quaternion.identity, Vector3.one * path.MinRadius*2);
//        }

        #endregion

        #region Methods

        #region PublicMethods

        public void SetPath(Path path)
        {
            this.path = path;
            GenerateGameField();

            EventManager.TriggerEvent(GameEvent.GAME_FIELD_CHANGED);
        }

        public void GenerateGameField()
        {
            path.GeneratePath();
            
            _meshFilter.mesh = MeshGenerator.GenerateMesh(path);
            var points = new Vector2[path.Length + 1];

            for (int i = 0; i < path.Length; i++)
            {
                points[i] = path.GetPointOnWall(i);
            }

            points[path.Length] = path.GetPointOnWall(0);
            _edgeCollider2D.points = points;

//            Camera.main.orthographicSize = path.MaxRadius * 2;
        }
        
        public Vector2 GetLerpedPoint(int index, float positionInPercentage)
        {
            var percentage = GetPercentageBetweenPoints(positionInPercentage);
            return Vector2.Lerp(this[index], this[index + 1], percentage);
        }
        
        public float GetPercentageBetweenPoints(float positionInPercentage)
        {
            var top = Mathf.Ceil(positionInPercentage % 1 * path.Length);
            var bottom = Mathf.Floor(positionInPercentage % 1 * path.Length);
            var value = positionInPercentage % 1 * path.Length;

            return Mathf.Abs(top - bottom) > 0 ? (value - bottom) / (top - bottom) : 0;
        }
        
        public int ClosestIndex(Vector2 point)
        {
            var closestPoint = path.PathCopy.OrderBy(vector => Vector2.Distance(point, vector)).First();
            return closestPoint.index;
        }

        public float ClosestPercentage(Vector2 point)
        {
            return (float)ClosestIndex(point) / Length;
        }
        

        public int GetPositionIndex(float positionInPercentage)
        {
            return Mathf.FloorToInt(positionInPercentage % 1 * path.Length);
        }

        public Vector2 GetPointNormal(int index)
        {
            return path.GetPointNormal(index);
        }
        
        public Vector2 GetPointOnWall(int index)
        {
            return path.GetPointOnWall(index);
        }

        public Vector2 GetPointOnWall(float index)
        {
            return path.GetPointOnWall(index);
        }

        public IndexedVector2 GetPointOnWall(Vector2 closestPoint)
        {
            return path.GetPointOnWall(closestPoint);
        }

        public int GetDistanceInPoints(float distance)
        {
            return Mathf.FloorToInt(distance / path.DistanceBetweenPoints);
        }

        

        #endregion

        #region PrivateMethods

        private void Initialize()
        {
            if(_meshFilter == null)
                _meshFilter = GetComponent<MeshFilter>();
            if(_edgeCollider2D == null)
                _edgeCollider2D = GetComponent<EdgeCollider2D>();
            
        }

        #endregion

        #endregion

        #region Coroutines

        #endregion
    }
}