using UnityEditor;

namespace TransformHandle
{
    [CustomEditor(typeof(RuntimeTransformHandle))]
    public class TransformHandleCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            RuntimeTransformHandle handle = (RuntimeTransformHandle)target;
            EditorGUILayout.HelpBox("Remember to set the layer to one that doesn't collide with physical objects",
                MessageType.Info, true);
        }
    }
}