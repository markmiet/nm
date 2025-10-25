using UnityEngine;

public class ZRotationLimiter : MonoBehaviour
{
    [Tooltip("Maximum allowed rotation (in degrees) from the starting Z rotation.")]
    public float allowedDegreesToRotate = 45f;

    private float startZ;

    void Start()
    {
        // Record initial Z rotation (in -180..180 range)
        startZ = NormalizeAngle(transform.eulerAngles.z);
    }

    void Update()
    {
        Vector3 euler = transform.eulerAngles;
        float currentZ = NormalizeAngle(euler.z);

        // Clamp relative rotation
        float clampedZ = Mathf.Clamp(currentZ, startZ - allowedDegreesToRotate, startZ + allowedDegreesToRotate);

        // Apply the corrected rotation
        transform.rotation = Quaternion.Euler(euler.x, euler.y, clampedZ);
    }

    /// <summary>
    /// Converts any 0–360 angle into -180..180 range
    /// </summary>
    private float NormalizeAngle(float angle)
    {
        angle = Mathf.Repeat(angle + 180f, 360f) - 180f;
        return angle;
    }
}
