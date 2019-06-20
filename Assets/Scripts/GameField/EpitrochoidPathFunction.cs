using System;
using UnityEngine;

namespace GameField
{
    [CreateAssetMenu(menuName = "Paths/Epitrochoid Path"),Serializable]
    public class EpitrochoidPathFunction : PathFunction
    {
        public int bigRadius;
        public int smallRadius;
        public float distance;

        public override Vector2 Function(float theta)
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
    }
}