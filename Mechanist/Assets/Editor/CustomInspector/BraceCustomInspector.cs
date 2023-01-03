using UnityEditor;
using Block;

[CustomEditor(typeof(Beam))]
public class BraceCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Beam beam = (Beam)target;

        EditorGUILayout.LabelField("Length", beam.Length.ToString());

        // if (GUILayout.Button("Refresh"))
        //     beam.OnValidate();
    }
}