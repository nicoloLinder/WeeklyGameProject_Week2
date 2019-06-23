using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameField
{
    public abstract class Path : ScriptableObject, IEnumerable<Vector2>
    {
        #region Variables

        #region PublicVariables

        #endregion

        #region PrivateVariables

        protected List<Vector2> _path;

        #endregion

        #endregion

        #region Properties

        public Vector2 this[int index] => _path[index];
        public int Length => _path.Count;
        public List<Vector2> PathCopy => new List<Vector2>(_path);

        public List<Vector3> WallOutline { get; set; }

        #endregion

        #region Methods

        #region Constructors

        #endregion

        #region PublicMethods

        public abstract List<Vector2> GeneratePath();

        public List<Vector2> MakePathEquidistant(List<Vector2> rawPath)
        {

            var equidistantPath = new List<Vector2>();
            var pathCircumference = rawPath.Select((t, i) => Vector2.Distance(t, rawPath[(i + 1) % rawPath.Count])).Sum();
            
            var distanceBetweenPoints = pathCircumference / rawPath.Count;

            var currentPoint = rawPath[0];
            var nextPoint = rawPath[1];
            
            equidistantPath.Add(currentPoint);
            
            var distanceLeftToTravel = distanceBetweenPoints;

            for (var i = 1; i <= rawPath.Count;)
            {
                var distance = Mathf.Abs(Vector2.Distance(currentPoint, nextPoint));

                if (distance >= distanceLeftToTravel)
                {
                    currentPoint += (nextPoint - currentPoint).normalized * distanceLeftToTravel;
                    distanceLeftToTravel = distanceBetweenPoints;
                    pathCircumference -= distanceBetweenPoints;
                    equidistantPath.Add(currentPoint);
                    
                }
                else
                {
                    distanceLeftToTravel -= distance;
                    currentPoint = nextPoint;
                    nextPoint = rawPath[++i%rawPath.Count];
                }

                if (!(pathCircumference <= 0)) continue;
                
                var distanceToFirstPoint = Vector2.Distance(currentPoint, equidistantPath[0]);
                var pointsToAdd = (int) (distanceToFirstPoint / distanceBetweenPoints);

                for (var j = 1; j <= pointsToAdd; j++)
                {
                    equidistantPath.Add(Vector2.Lerp(currentPoint, equidistantPath[0],(float)j/(pointsToAdd+1)));
                }
                break;
            }

            return equidistantPath;
        }
        

        public Vector2 GetPointNormal(int index)
        {
            var nextIndex = index + 1;
            if (nextIndex > _path.Count - 1)
            {
                nextIndex = 0;
            }
            else if (nextIndex < 0)
            {
                nextIndex = _path.Count- 1;
            }

            var prevIndex = index - 1;
            if (prevIndex > _path.Count - 1)
            {
                prevIndex = 0;
            }
            else if (prevIndex < 0)
            {
                prevIndex =_path.Count - 1;
            }

            var currentToPrev = (_path[prevIndex] - _path[index]);
            var currentToNext = (_path[nextIndex] - _path[index]);

            var normalPervCurrent = Vector2.Perpendicular(currentToPrev);
            //new Vector2(currentToPrev.normalized.y, -currentToPrev.normalized.x);
            var normalNextCurrent = -Vector2.Perpendicular(currentToNext);
            //new Vector2(-currentToNext.normalized.y, currentToNext.normalized.x);

            return (normalPervCurrent + normalNextCurrent).normalized;
        }

        public Vector2 GetWallPoint(int index)
        {
            return this[index] + GetPointNormal(index) * 0.1f;
        }

        public Vector2 GetWallPoint(float percentage)
        {
            return GetWallPoint((int)(percentage % 1f * WallOutline.Count));
        }

        #endregion

        #region PrivateMethods

        #endregion

        #endregion

        #region Coroutines

        #endregion

        public IEnumerator<Vector2> GetEnumerator()
        {
            return _path.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}