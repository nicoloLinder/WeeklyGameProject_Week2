using UnityEngine;

namespace GameField
{
    [CreateAssetMenu(menuName = "Paths/Squircle Path")]
    public class SquirclePathFunction : PathFunction
    {
        public float divider = 1;
        public float power = 4;

        public override Vector2 Function(float theta)
        {
            theta *= 2 * Mathf.PI;

            var x = Mathf.Cos(theta);
            var y = Mathf.Sin(theta);

            return new Vector2
            {
                x = Mathf.Pow(divider - Mathf.Pow(Mathf.Abs(y), power), 1f / power) * Mathf.Sign(x),
                y = Mathf.Pow(divider - Mathf.Pow(Mathf.Abs(x), power), 1f / power) * Mathf.Sign(y)
            };
        }
    }
}