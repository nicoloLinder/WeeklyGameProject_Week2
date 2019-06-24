using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameField
{
    [CreateAssetMenu(menuName = "Paths/Epitrochoid Polygon Path"), Serializable]
    public class EpitrochoidPolygonalPath : EpitrochoidPath
    {
        public override List<Vector2> GeneratePath()
        {
            distance = 1f / (bigRadius - 1f);
            smallRadius = -1;
            return base.GeneratePath();
        }
    }
}