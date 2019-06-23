using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GameField
{
    [CreateAssetMenu(menuName = "Paths/Squircle Path")]
    public class SquirclePath : Path
    {
        public float divider = 1;
        public float power = 4;

        public int pointCount = 512;

        

        public override List<Vector2> GeneratePath()
        {
            var  rawPath = new List<Vector2>();

            for (var i = 0; i < pointCount; i++)
            {
                var theta = (float)i / pointCount * 2 * Mathf.PI;

                var y = -Mathf.Cos(theta);
                var x = Mathf.Sin(theta);

                rawPath.Add(new Vector2
                {
                    x = Mathf.Pow(divider - Mathf.Pow(Mathf.Abs(y), power), 1f / power) * Mathf.Sign(x),
                    y = Mathf.Pow(divider - Mathf.Pow(Mathf.Abs(x), power), 1f / power) * Mathf.Sign(y)
                });
            }

            _path = MakePathEquidistant(rawPath);
            return _path;
        }
    }
}