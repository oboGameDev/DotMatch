using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Editor
{
    public class SpaceSetter : EditorWindow
    {
        private Object parent;
        private float offset;

        [MenuItem("Tools/SpaceSet")]
        public static void OpenWindow()
        {
            GetWindow<SpaceSetter>();
        }

        private void OnGUI()
        {
            parent = EditorGUILayout.ObjectField("parent", parent, typeof(RectTransform), true);
            offset = EditorGUILayout.FloatField("offset", offset);

            if (GUILayout.Button("setOfSet"))
            {
                setOfSet();
            }
        }

        private void setOfSet()
        {
            RectTransform parentRect = parent as RectTransform;

            float y = parentRect.GetChild(0).GetComponent<RectTransform>().anchoredPosition.y - offset;

            for (var i = 0; i < parentRect.childCount; i++)
            {
                RectTransform rect = parentRect.GetChild(i).GetComponent<RectTransform>();
                var pos = rect.anchoredPosition;
                y += offset;
                pos.y = y;
                rect.anchoredPosition = pos;

                EditorUtility.SetDirty(rect);
            }

            EditorSceneManager.MarkSceneDirty(parentRect.gameObject.scene);
        }
    }
}