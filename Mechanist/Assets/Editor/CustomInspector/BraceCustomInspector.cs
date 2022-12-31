using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Brace))]
public class BraceCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Brace brace = (Brace)target;

        EditorGUILayout.LabelField("Length", brace.Length.ToString());

        // if (GUILayout.Button("Refresh"))
        //     brace.OnValidate();
    }
}