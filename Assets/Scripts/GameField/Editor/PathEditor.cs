using UnityEditor;
using UnityEngine;

namespace GameField
{
    [CustomEditor(typeof(Path),true)]
    public class PathEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (GUILayout.Button("Generate path")) ((Path) target).GeneratePath();
        }
    }

}