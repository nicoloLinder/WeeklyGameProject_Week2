using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace GameField
{
    [CreateAssetMenu(menuName = "Paths/Epitrochoid Path"), Serializable]
    public class EpitrochoidPath : Path
    {
        [Range(3,20)]
        public int bigRadius;
        public int smallRadius;
        public float distance;

        public int pointCount = 512;

        public override List<IndexedVector2> GeneratePath()
        {
            var rawPath = new List<IndexedVector2>();
            
            for (var i = 0; i < pointCount; i++)
            {
//                i += pointCount / bigRadius  % pointCount;
                var theta = (float) i / pointCount * 2 * Mathf.PI;

                var position = new Vector2
                {
                    y = -(((bigRadius + smallRadius) * Mathf.Cos(theta) -
                           distance * Mathf.Cos((bigRadius + smallRadius) / smallRadius * theta)) / bigRadius),
                    x = ((bigRadius + smallRadius) * Mathf.Sin(theta) -
                         distance * Mathf.Sin((bigRadius + smallRadius) / smallRadius * theta)) / bigRadius
                };

                rawPath.Add(new IndexedVector2(position, i));
            }

            _path = MakePathEquidistant(rawPath);
//            _path = rawPath;
            SetPathIndices();
            
            CalculateBarycenter();
            CalculateMinMaxRadius();

            CorrectToRadius();
            
            return _path;
        }
    }
}