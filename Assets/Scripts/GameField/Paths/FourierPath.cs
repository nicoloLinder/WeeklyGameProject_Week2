using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace GameField
{
    [CreateAssetMenu(menuName = "Paths/Fourier Path"), Serializable]
    public class FourierPath : Path
    {
        public int pointCount = 512;

        public override List<IndexedVector2> GeneratePath()
        {
            var rawPath = new List<IndexedVector2>();

            for (var i = 0; i < pointCount; i++)
            {
//                i += pointCount / bigRadius  % pointCount;
                var theta = (float) i / pointCount;

                var position = new Vector2
                {
                    x = 4/Mathf.PI * (Mathf.Sin(1 * Mathf.PI * theta)/1 - Mathf.Sin(3 * Mathf.PI * theta)/3),// + Mathf.Sin(5 * Mathf.PI * theta)/5- Mathf.Sin(7 * Mathf.PI * theta)/7),
                    y = 4/Mathf.PI * (Mathf.Cos(1 * Mathf.PI * theta)/1 - Mathf.Cos(3 * Mathf.PI * theta)/3)// + Mathf.Cos(5 * Mathf.PI * theta)/5- Mathf.Cos(7 * Mathf.PI * theta)/7)
                };

                rawPath.Add(new IndexedVector2(position, i));
            }

            _path = MakePathEquidistant(rawPath);

            SetPathIndices();

            CalculateBarycenter();
            CalculateMinMaxRadius();

            CorrectToRadius();

            return _path;
        }
    }
}