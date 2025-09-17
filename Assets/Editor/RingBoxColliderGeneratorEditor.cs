// RingBoxColliderGeneratorEditor.cs
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RingBoxColliderGenerator))]
public class RingBoxColliderGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RingBoxColliderGenerator gen = (RingBoxColliderGenerator)target;

        GUILayout.Space(6);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate colliders"))
        {
            Undo.RegisterCompleteObjectUndo(gen.gameObject, "Generate Ring Colliders");
            gen.Generate();
        }
        if (GUILayout.Button("Remove generated"))
        {
            Undo.RegisterCompleteObjectUndo(gen.gameObject, "Remove Ring Colliders");
            gen.RemoveGenerated();
        }
        EditorGUILayout.EndHorizontal();

        // pientä infoa
        EditorGUILayout.HelpBox("Aseta outer/inner säteet world-yksiköissä (tai anna SpriteRenderer ja yritä automaattisesti). Käytä Generate-painiketta.", MessageType.Info);
    }
}
#endif
