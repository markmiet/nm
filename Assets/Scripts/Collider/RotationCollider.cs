using UnityEngine;

[System.Serializable]
public class RotationCollider
{
    public float rotation;         // The rotation angle you saved for
    public Vector2[] colliderPoints; // Collider points for that rotation
}

[CreateAssetMenu(fileName = "PolygonColliderRotationData", menuName = "Custom/PolygonColliderRotationData")]
public class PolygonColliderRotationData : ScriptableObject
{
    public RotationCollider[] colliders;
}
