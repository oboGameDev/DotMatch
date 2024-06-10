using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{
    public class CellsPositionSetter : EditorWindow
    {
        private UnityEngine.Object Target;

        [MenuItem("Tools/CellsPosition Setter")]
        public static void OpenWindow()
        {
            GetWindow<CellsPositionSetter>();
        }

        private void OnGUI()
        {
            Target = EditorGUILayout.ObjectField("Target Parent", Target, typeof(Transform));

            if (GUILayout.Button("Name"))
            {
                Transform parent = (Transform)Target;
                int x = 0;
                int y = 0;
                for (var i = 0; i < parent.childCount; i++)
                {
                    if (parent.GetChild(i).TryGetComponent(out Place place))
                    {
                        place.Location = new Point(x, y);
                        EditorUtility.SetDirty(place);
                        x++;
                        if (x >= 9)
                        {
                            x = 0;
                            y++;
                        }
                    }
                }
            }
        }
    }
}