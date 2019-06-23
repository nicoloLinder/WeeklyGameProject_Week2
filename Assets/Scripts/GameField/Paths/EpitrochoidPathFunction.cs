using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameField
{
    [CreateAssetMenu(menuName = "Paths/Epitrochoid Path"), Serializable]
    public class EpitrochoidPathFunction : Path
    {
        public int bigRadius;
        public int smallRadius;
        public float distance;

        public int pointCount = 512;

        public Vector2 Function(float theta)
        {
            theta = theta * 2 * Mathf.PI;

            return new Vector2
            {
                x = ((bigRadius + smallRadius) * Mathf.Cos(theta) -
                     distance * Mathf.Cos((bigRadius + smallRadius) / smallRadius * theta)) / bigRadius,
                y = ((bigRadius + smallRadius) * Mathf.Sin(theta) -
                     distance * Mathf.Sin((bigRadius + smallRadius) / smallRadius * theta)) / bigRadius
            };
        }

        public override List<Vector2> GeneratePath()
        {
            var rawPath = new List<Vector2>();

            for (var i = 0; i < pointCount; i++)
            {
//                i += pointCount / bigRadius  % pointCount;
                var theta = (float) i / pointCount * 2 * Mathf.PI; 

                rawPath.Add(new Vector2
                {
                    y = -(((bigRadius + smallRadius) * Mathf.Cos(theta) -
                         distance * Mathf.Cos((bigRadius + smallRadius) / smallRadius * theta)) / bigRadius),
                    x = ((bigRadius + smallRadius) * Mathf.Sin(theta) -
                         distance * Mathf.Sin((bigRadius + smallRadius) / smallRadius * theta)) / bigRadius
                });
            }

            _path = MakePathEquidistant(rawPath);
            return _path;
        }
    }
}