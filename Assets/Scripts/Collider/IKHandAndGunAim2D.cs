using UnityEngine;
using UnityEngine.U2D.IK;

public class IKHandAndGunAim2D : MonoBehaviour
{
    [Header("References")]
    public Transform target;          // What to aim and reach for (enemy, mouse follower, etc.)
    public Transform handIKTarget;    // IK target (effector) assigned in the Limb Solver 2D
    public Transform handBone;        // Actual hand bone transform
    public Transform gunTransform;    // Gun sprite transform (child of hand bone)

    [Header("IK Motion Settings")]
    public float moveSpeed = 5f;      // How fast the hand moves toward the target
    public float maxReach = 2f;       // Max reach distance for the arm

    [Header("Gun Aiming Settings")]
    public float aimSmooth = 10f;     // Rotation smoothing for gun aim
    public float angleOffset = 0f;    // Adjust if sprite points in a different direction (e.g. 90° if pointing up)

    void Update()
    {
        if (target == null || handIKTarget == null) return;

        // Move the IK target (effector) toward the target position in 2D
        Vector2 currentPos = handIKTarget.position;
        Vector2 targetPos = target.position;
        Vector2 dir = targetPos - currentPos;

        // Limit reach
        if (dir.magnitude > maxReach)
            targetPos = currentPos + dir.normalized * maxReach;

        // Smooth movement
        handIKTarget.position = Vector2.Lerp(currentPos, targetPos, Time.deltaTime * moveSpeed);
    }

    void LateUpdate()
    {
        // Aiming happens *after* IK update to stay in sync
        if (target == null || gunTransform == null || handBone == null) return;

        // Compute direction from hand bone to target
        Vector2 aimDir = (Vector2)(target.position - handBone.position);
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg + angleOffset;

        // Smooth rotation toward the target
        Quaternion targetRot = Quaternion.Euler(0, 0, angle);
        gunTransform.rotation = Quaternion.Lerp(
            gunTransform.rotation,
            targetRot,
            Time.deltaTime * aimSmooth
        );
        /*
        // Flip gun if aiming left (optional)
        if (aimDir.x < 0)
            gunTransform.localScale = new Vector3(0.2f, -0.3f, 1);
        else
            gunTransform.localScale = new Vector3(0.2f, 0.3f, 1);
        */
    }
}
