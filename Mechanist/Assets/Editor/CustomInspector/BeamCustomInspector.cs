using UnityEditor;
using Block;
using UnityEngine;

namespace CustomEditor
{
    [UnityEditor.CustomEditor(typeof(Beam))]
    public class BeamCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
    
            Beam beam = (Beam)target;
    
            if (GUILayout.Button("Refresh"))
                beam.UpdateMeshInEditor();
        }
    }
}