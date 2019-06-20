using System;
using UnityEngine;

namespace GameField
{
    [Serializable]
    public abstract class PathFunction : ScriptableObject
    {
        public abstract Vector2 Function(float theta);
    }
}