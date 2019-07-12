using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using Utilities;

namespace GameField
{
    public abstract class Path : ScriptableObject, IEnumerable<IndexedVector2>
    {
        #region Variables

        #region PublicVariables

        public float radius;

        #endregion

        #region PrivateVariables
        
        protected List<IndexedVector2> _path;

        protected float circumference;

        #endregion

        #endregion

        #region Properties

        public IndexedVector2 this[int index] => _path[index];
        public IndexedVector2 this[float index] => _path[(int) (index % 1 * Length)];
//        public IndexedVector2 this[Vector2 closestPoint] => _path.OrderBy(c => Vector2.Distance(closestPoint.vector, c.vector)).First();
        public int Length => _path.Count;
        public List<IndexedVector2> PathCopy => new List<IndexedVector2>(_path);

        public List<IndexedVector2> WallOutline { get; set; }

        public float MinRadius { get; protected set; }
        public float MaxRadius { get; protected set; }
        public IndexedVector2 Barycenter { get; protected set; }

        public float DistanceBetweenPoints => circumference / Length;

        #endregion

        #region Methods

        #region Constructors

        #endregion

        #region PublicMethods

        public void CalculateMinMaxRadius()
        {
            MaxRadius = float.MinValue;
            MinRadius = float.MaxValue;

            foreach (var point in _path)
            {
                var distance = Mathf.Abs(IndexedVector2.Distance(point, Barycenter));

                if (distance > MaxRadius) MaxRadius = distance;
                if (distance < MinRadius) MinRadius = distance;
            }
        }

        public void CorrectToRadius()
        {
            var multiplier = radius / MaxRadius;

            for (var i = 0; i < _path.Count; i++)
            {
                _path[i] *= multiplier;
            }

            MaxRadius *= multiplier;
            MinRadius *= multiplier;
        }

        public void CalculateBarycenter()
        {
            Barycenter = Vector2.zero;
            foreach (var point in _path)
            {
                Barycenter += (Vector2)point;
            }

            Barycenter /= _path.Count;
        }

        public abstract List<IndexedVector2> GeneratePath();

        public List<IndexedVector2> MakePathEquidistant(List<IndexedVector2> rawPath)
        {
            var equidistantPath = new List<IndexedVector2>();
            var pathCircumference = circumference =
                rawPath.Select((t, i) => Vector2.Distance(t, rawPath[(i + 1) % rawPath.Count])).Sum();
            
            var distanceBetweenPoints = pathCircumference / rawPath.Count;

            var currentPoint = rawPath[0];
            var nextPoint = rawPath[1];
            
            equidistantPath.Add(currentPoint);

            var distanceLeftToTravel = distanceBetweenPoints;

            for (var i = 1; i <= rawPath.Count;)
            {
                var distance = Vector2.Distance(currentPoint, nextPoint);

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
                    nextPoint = rawPath[++i % rawPath.Count];
                }

                if (pathCircumference > 0) continue;

                var distanceToFirstPoint = IndexedVector2.Distance(currentPoint, equidistantPath[0]);
                var pointsToAdd = (int) (distanceToFirstPoint / distanceBetweenPoints);

                for (var j = 1; j <= pointsToAdd; j++)
                {
                    equidistantPath.Add(IndexedVector2.Lerp(currentPoint, equidistantPath[0], (float) j / (pointsToAdd + 1)));
                }

                break;
            }
            
            return equidistantPath;
        }

        public void SetPathIndices()
        {
            for (var i = 0; i < _path.Count; i++)
            {
                _path[i] = new IndexedVector2(_path[i].vector, i);
            }
        }

        public IndexedVector2 GetClosestPoint(Vector2 closestPoint)
        {
            var distance = float.MaxValue;
            IndexedVector2 realClosestPoint = new IndexedVector2();
            
            foreach (var point in _path)
            {
                var currentDistance = Vector2.Distance(closestPoint, point);
                if (distance > currentDistance)
                {
                    
                    distance = currentDistance;
                    realClosestPoint = point;
                }
            }
            return realClosestPoint;
        }

        public Vector2 GetPointNormal(int index)
        {
            index = AdjustIndex(index);
            
            var nextIndex = index + 1;
            if (nextIndex > _path.Count - 1)
            {
                nextIndex = 0;
            }
            else if (nextIndex < 0)
            {
                nextIndex = _path.Count - 1;
            }

            var prevIndex = index - 1;
            if (prevIndex > _path.Count - 1)
            {
                prevIndex = 0;
            }
            else if (prevIndex < 0)
            {
                prevIndex = _path.Count - 1;
            }

            var currentToPrev = _path[prevIndex] - _path[index];
            var currentToNext = _path[nextIndex] - _path[index];

            var normalPervCurrent = Vector2.Perpendicular(currentToPrev);
            //new Vector2(currentToPrev.normalized.y, -currentToPrev.normalized.x);
            var normalNextCurrent = -Vector2.Perpendicular(currentToNext);
            //new Vector2(-currentToNext.normalized.y, currentToNext.normalized.x);

            return (normalPervCurrent + normalNextCurrent).normalized;
        }
        

        public IndexedVector2 GetPointOnWall(int index)
        {
            index = AdjustIndex(index);
            return _path[index] + (IndexedVector2)GetPointNormal(index) * GameFieldManager.Offset;
        }

        public IndexedVector2 GetPointOnWall(float index)
        {
            var realIndex = (int) (index % 1 * Length); 
            return _path[realIndex] + (IndexedVector2)GetPointNormal(realIndex) * GameFieldManager.Offset;
        }

        public IndexedVector2 GetPointOnWall(Vector2 closestPoint)
        {
            var index = GetClosestPoint(closestPoint).index;
            return _path[index] + (IndexedVector2)GetPointNormal(index) * GameFieldManager.Offset;
        }

//        public int IndexOf(Vector2 point)
//        {
//            return _path.IndexOf(point);
//        }

        public int AdjustIndex(int index)
        {
            index %= Length;

            if (index < 0)
            {
                index += Length;
            }

            return index;
        }

        #endregion

        #region PrivateMethods

        #endregion

        #endregion

        #region Coroutines

        #endregion

        public IEnumerator<IndexedVector2> GetEnumerator()
        {
            return _path.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}