using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(PolygonCollider2D))]
public class PolygonColliderRotationSaver : MonoBehaviour
{
    [Tooltip("Assign a PolygonColliderRotationData ScriptableObject to store collider points per rotation.")]
    public PolygonColliderRotationData savedData;

    // Context menu option (right-click on component → Save Collider Points)
    [ContextMenu("Save Collider Points for Current Rotation")]
    public void SavePoints()
    {
        // Check if PolygonCollider2D exists
        PolygonCollider2D poly = GetComponent<PolygonCollider2D>();
        if (poly == null)
        {
            Debug.LogError("PolygonCollider2D component not found on this GameObject.");
            return;
        }

        // Check if collider has at least one path
        if (poly.pathCount == 0)
        {
            Debug.LogError("PolygonCollider2D has no paths! Draw collider points first.");
            return;
        }

        // Check if ScriptableObject is assigned
        if (savedData == null)
        {
            Debug.LogError("No PolygonColliderRotationData assigned in the Inspector.");
            return;
        }

        // Ensure colliders array is initialized
        if (savedData.colliders == null)
            savedData.colliders = new RotationCollider[0];

        Vector2[] points = poly.GetPath(0);
        float rotation = transform.eulerAngles.z;

        RotationCollider entry = new RotationCollider
        {
            rotation = rotation,
            colliderPoints = points
        };

        // Append or replace if rotation already exists
        bool replaced = false;
        var list = new System.Collections.Generic.List<RotationCollider>(savedData.colliders);

        for (int i = 0; i < list.Count; i++)
        {
            if (Mathf.Approximately(list[i].rotation, rotation))
            {
                list[i] = entry;
                replaced = true;
                break;
            }
        }

        if (!replaced)
            list.Add(entry);

        savedData.colliders = list.ToArray();

#if UNITY_EDITOR
        EditorUtility.SetDirty(savedData); // Mark asset dirty so changes are saved
#endif

        Debug.Log($"Saved collider points for rotation {rotation}°, total saved rotations: {savedData.colliders.Length}");
    }
}
