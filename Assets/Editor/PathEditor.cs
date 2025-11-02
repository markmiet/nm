using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Path))]
public class PathEditor : Editor
{
    private Path path;

    void OnEnable()
    {
        path = (Path)target;
    }

    void OnSceneGUI()
    {
        Handles.color = Color.yellow;

        // Näytä ja siirrä pisteitä Scene-näkymässä
        for (int i = 0; i < path.points.Count; i++)
        {
            if (path.points[i] == null) continue;
            EditorGUI.BeginChangeCheck();
            Vector3 newPos = Handles.PositionHandle(path.points[i].position, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(path.points[i], "Move Path Point");
                path.points[i].position = newPos;
            }
        }

        // Lisää uusi piste, kun alt + klikataan Sceneen
        Event e = Event.current;
        if (e != null && e.alt && e.type == EventType.MouseDown && e.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                AddPoint(hit.point);
                e.Use();
            }
            else
            {
                // Jos ei collideria, otetaan suoraan 2D-näkymästä
                Vector3 mousePos = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;
                AddPoint(mousePos);
                e.Use();
            }
        }
    }

    private void AddPoint(Vector3 position)
    {
        GameObject newPoint = new GameObject("Point " + path.points.Count);
        newPoint.transform.position = position;
        newPoint.transform.SetParent(path.transform);
        Undo.RegisterCreatedObjectUndo(newPoint, "Add Path Point");
        path.points.Add(newPoint.transform);
        EditorUtility.SetDirty(path);
    }
}
