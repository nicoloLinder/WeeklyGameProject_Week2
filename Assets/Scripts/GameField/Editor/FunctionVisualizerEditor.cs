using UnityEditor;
using UnityEngine;

namespace GameField
{
    [CustomEditor(typeof(FunctionVisualizer))]
    public class FunctionVisualizerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Generate path")) ((FunctionVisualizer) target).GenerateBasicPath();
        }
    }
}