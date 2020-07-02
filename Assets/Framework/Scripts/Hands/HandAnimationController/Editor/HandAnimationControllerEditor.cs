using UnityEditor;
using UnityEngine;

namespace Framework
{
    [CustomEditor(typeof(HandAnimationController))]
    public class HandAnimationControllerEditor : Editor
    {
        private HandAnimationController _controller;

        private void OnEnable()
        {
            _controller = (HandAnimationController) target;
            if (_controller.HandData &&
                !_controller.IsInitialized)
            {
                _controller.Initialize();
            }

            EditorApplication.update += Update;
        }

        void OnDisable()
        {
            EditorApplication.update -= Update;
        }

        void Update()
        {
            if (!EditorApplication.isPlaying)
            {
                if (_controller.graph.IsValid())
                {

                    _controller.Update();
                }
                else
                {
                    _controller.Initialize();
                }
            }
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DisplayHandEditor(_controller, serializedObject);
        }

        private static void DisplayHandEditor(HandAnimationController controller, SerializedObject serializedObject)
        {
            if (controller.HandData &&
                !controller.IsInitialized)
            {
                controller.Initialize();
            }

            var fingers = serializedObject.FindProperty("fingers");
            EditorGUI.BeginChangeCheck();
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("handData"));
            serializedObject.ApplyModifiedProperties();
            
            if (EditorGUI.EndChangeCheck())
            {
                controller.Initialize();
            }

            EditorGUI.BeginChangeCheck();
            controller.StaticPose = EditorGUILayout.Toggle(new GUIContent("static Pose"), controller.StaticPose);
            
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RegisterFullObjectHierarchyUndo(controller, "Changed static pose");
            }

            if (controller.StaticPose)
            {
                var currentPose = serializedObject.FindProperty("pose");
                var poses = serializedObject.FindProperty("poses");
                var posesArray = new string[poses.arraySize];
                
                for (var i = 0; i < posesArray.Length; i++)
                {
                    posesArray[i] = ((AnimationClip) poses.GetArrayElementAtIndex(i).FindPropertyRelative("clip")
                        .objectReferenceValue).name;
                }

                controller.Pose =
                    EditorGUILayout.Popup(new GUIContent("pose"), currentPose.intValue, posesArray);
            }
            else
            {
                for (var i = 0; i < fingers.arraySize; i++)
                {
                    var fingerWeight = fingers.GetArrayElementAtIndex(i).FindPropertyRelative("weight");
                    EditorGUILayout.PropertyField(fingerWeight, new GUIContent(((FingerName) i).ToString()));
                }

                serializedObject.ApplyModifiedProperties();
                for (var i = 0; i < 5; i++)
                {
                    controller[i] = controller[i];
                }
            }
        }
    }
}
