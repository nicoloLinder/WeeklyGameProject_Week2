using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace GameField
{
    [CreateAssetMenu(menuName = "Paths/Epitrochoid Star Path"), Serializable]
    public class EpitrochoidStarPath : EpitrochoidPath
    {
        public override List<IndexedVector2> GeneratePath()
        {
            distance = 0.75f - (bigRadius - 3) * 0.05f;
//            smallRadius = -1;b 
            return base.GeneratePath();
        }
    }
}