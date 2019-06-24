using UnityEditor;
using UnityEngine;

namespace GameField
{
    [CustomEditor(typeof(GameFieldManager))]
    public class GameFieldManagerEditor : Editor
    {
        private Editor pathEditor;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GameFieldManager gameFieldManager = (GameFieldManager) target;

            if (GUILayout.Button("Generate Path"))
            {
                gameFieldManager.GenerateGameField();
            }

            DrawSettingsEditor(gameFieldManager.path, gameFieldManager.GenerateGameField,
                ref gameFieldManager.pathFoldout, ref pathEditor);
        }

        void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
        {
            if (settings != null)
            {
                foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    if (foldout)
                    {
                        CreateCachedEditor(settings, null, ref editor);
                        editor.OnInspectorGUI();

                        if (check.changed)
                        {
                            if (onSettingsUpdated != null && Application.isPlaying)
                            {
                                onSettingsUpdated();
                            }
                        }
                    }
                }
            }
        }
    }
}