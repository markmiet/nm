using UnityEngine;

/// <summary>
/// Smoothly limits an object's Z rotation between defined minimum and maximum angles
/// relative to its starting rotation. Both limits can be toggled individually.
/// Draws an arc gizmo showing the allowed range in Scene view.
/// </summary>
[AddComponentMenu("SmoothZRotationLimiter Custom/Smooth Z Rotation Limiter (Min/Max + Gizmo Arc)")]
public class SmoothZRotationLimiter : MonoBehaviour
{
    [Header("Rotation Limits")]
    [Tooltip("Enable the minimum rotation limit.")]
    public bool useMinLimit = true;

    [Tooltip("Minimum allowed rotation (in degrees) relative to the starting Z rotation.")]
    public float minDegrees = -30f;

    [Tooltip("Enable the maximum rotation limit.")]
    public bool useMaxLimit = true;

    [Tooltip("Maximum allowed rotation (in degrees) relative to the starting Z rotation.")]
    public float maxDegrees = 45f;

    [Header("Smooth Settings")]
    [Tooltip("How fast the rotation moves toward the limited range (degrees per second).")]
    public float smoothSpeed = 180f;

    [Header("Update Settings")]
    [Tooltip("If true, limit rotation in LateUpdate instead of Update.")]
    public bool limitInLateUpdate = false;

    [Tooltip("If true, use local rotation instead of world rotation.")]
    public bool useLocalRotation = false;

#if UNITY_EDITOR
    [Header("Gizmo Settings")]
    [Tooltip("Draws an arc in Scene view showing allowed rotation range.")]
    public bool drawArcGizmo = true;

    [Tooltip("Radius of the gizmo arc.")]
    public float gizmoRadius = 2f;
#endif

    private float startZ;

    void Start()
    {
        float initialZ = useLocalRotation ? transform.localEulerAngles.z : transform.eulerAngles.z;
        startZ = NormalizeAngle(initialZ);
    }

    void Update()
    {
        if (!limitInLateUpdate)
            LimitRotationSmooth();
    }

    void LateUpdate()
    {
        if (limitInLateUpdate)
            LimitRotationSmooth();
    }

    private void LimitRotationSmooth()
    {
        Vector3 euler = useLocalRotation ? transform.localEulerAngles : transform.eulerAngles;
        float currentZ = NormalizeAngle(euler.z);

        float lowerLimit = useMinLimit ? startZ + minDegrees : float.NegativeInfinity;
        float upperLimit = useMaxLimit ? startZ + maxDegrees : float.PositiveInfinity;

        // Calculate clamped target
        float targetZ = Mathf.Clamp(currentZ, lowerLimit, upperLimit);

        // Smoothly move toward clamped value
        float smoothedZ = Mathf.MoveTowards(currentZ, targetZ, smoothSpeed * Time.deltaTime);
        euler.z = smoothedZ;

        if (useLocalRotation)
            transform.localRotation = Quaternion.Euler(euler);
        else
            transform.rotation = Quaternion.Euler(euler);
    }

    private float NormalizeAngle(float angle)
    {
        return Mathf.Repeat(angle + 180f, 360f) - 180f;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!drawArcGizmo) return;

        Gizmos.color = Color.yellow;
        Vector3 pos = transform.position;

        float baseZ = Application.isPlaying ? startZ :
            (useLocalRotation ? transform.localEulerAngles.z : transform.eulerAngles.z);

        float startAngle = useMinLimit ? baseZ + minDegrees : baseZ;
        float endAngle = useMaxLimit ? baseZ + maxDegrees : baseZ;

        const int segments = 32;
        float step = (endAngle - startAngle) / segments;
        Vector3 prev = pos + Quaternion.Euler(0, 0, startAngle) * Vector3.right * gizmoRadius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = startAngle + step * i;
            Vector3 next = pos + Quaternion.Euler(0, 0, angle) * Vector3.right * gizmoRadius;
            Gizmos.DrawLine(prev, next);
            prev = next;
        }

        Gizmos.color = Color.cyan;
        if (useMinLimit)
            Gizmos.DrawLine(pos, pos + Quaternion.Euler(0, 0, baseZ + minDegrees) * Vector3.right * gizmoRadius);
        if (useMaxLimit)
            Gizmos.DrawLine(pos, pos + Quaternion.Euler(0, 0, baseZ + maxDegrees) * Vector3.right * gizmoRadius);
    }
#endif
}
