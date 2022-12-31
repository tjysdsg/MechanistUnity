using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProceduralCylinder))]
public class ProceduralCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ProceduralCylinder cylinder = (ProceduralCylinder)target;

        if (GUILayout.Button("Refresh"))
            cylinder.UpdateMeshInEditor();
    }
}