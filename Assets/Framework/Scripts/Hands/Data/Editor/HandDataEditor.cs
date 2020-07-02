using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Framework
{
    [CustomEditor(typeof(HandData))]
    public class HandDataEditor : Editor
    {
        private ReorderableList _poses;

        private void OnEnable()
        {
            _poses = new ReorderableList(serializedObject, serializedObject.FindProperty("poses"), true, true, true,
                true);
            _poses.drawHeaderCallback = rect => { EditorGUI.LabelField(rect, "Custom Poses"); };

            _poses.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                rect.height -= 2;
                rect.x += 2;
                var element = serializedObject.FindProperty("poses").GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, element, new GUIContent(""));

            };
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            _poses.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
