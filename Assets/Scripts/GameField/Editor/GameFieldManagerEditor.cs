using System;
using FSM;
using UnityEditor;
using UnityEngine;
using Utilities;
using Object = UnityEngine.Object;

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
//                MenuStateManager.Instance.player.SetPosition(0f);
            }

            DrawSettingsEditor(gameFieldManager.path, gameFieldManager.GenerateGameField,
                ref gameFieldManager.pathFoldout, ref pathEditor);

            
                
        }

        public void OnSceneGUI()
        {
            GameFieldManager gameFieldManager = (GameFieldManager) target;
            
            foreach (IndexedVector2 point in gameFieldManager.path)
            {
                Handles.Label(point + gameFieldManager.GetPointNormal(point.index) * GameFieldManager.Offset, point.index.ToString());
            }
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