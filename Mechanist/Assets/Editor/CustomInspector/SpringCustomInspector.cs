using UnityEditor;
using Block;
using UnityEngine;

namespace CustomEditor
{
    [UnityEditor.CustomEditor(typeof(Spring))]
    public class SpringCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Spring t = (Spring)target;

            if (GUILayout.Button("Refresh"))
                t.UpdateProceduralModel();
        }
    }
}