using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameField
{
    public class Path
    {
        
        #region Variables

        #region PublicVariables

        #endregion

        #region PrivateVariables

        private readonly List<Vector2> _path;

        #endregion

        #endregion

        #region Properties

        public Vector2 this[int index] => _path[index];
        public int Length => _path.Count;

        #endregion
        
        #region Methods

        #region Constructors

        public Path(float minPointsDistance, int iterationCount,params PathFunction[] pathFunctions)
        {
            _path = GeneratePath(minPointsDistance, iterationCount, pathFunctions);
        }

        #endregion
        
        #region PublicMethods
        
        public static List<Vector2> GeneratePath(float minPointsDistance, int iterationCount, params PathFunction[] pathFunctions)
        {
            var pathPoints = new List<Vector2>
            {
                pathFunctions.Aggregate(Vector2.zero, (current, function) => current + function.Function(0))
            };

            var currentPosition =
                pathFunctions.Aggregate(Vector2.zero,
                    (current, function) => current + function.Function(1f / iterationCount));

            for (var i = 0; i < iterationCount; i++)
            {
                currentPosition = pathFunctions.Aggregate(Vector2.zero,
                    (current, function) => current + function.Function(i / (float) iterationCount));

                if (Vector2.Distance(pathPoints[pathPoints.Count - 1], currentPosition) > minPointsDistance)
                    pathPoints.Add(currentPosition);
            }

            return pathPoints;
        }

        #endregion

        #region PrivateMethods

        #endregion

        #endregion

        #region Coroutines

        #endregion
    }
}