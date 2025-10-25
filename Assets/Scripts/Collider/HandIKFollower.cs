using UnityEngine;
using UnityEngine.U2D.IK;

public class HandIKFollower : BaseController
{
   // [Header("Assign the target the hand should reach")]
    private Transform target; // e.g., enemy, object, mouse follow
   // [Header("Assign the IK effector transform")]
   // public Transform handIKTarget; // the hand IK target transform
    [Header("Settings")]
    public float moveSpeed = 5f; // how fast the hand moves
    public float maxReach = 2f;  // maximum arm reach distance
    public void Start()
    {
        target = PalautaAlus().transform;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        // Get target position in 2D
        Vector2 currentPos = transform.position;
        Vector2 targetPos = target.position;

        // Clamp distance (optional: limits how far hand can reach)
        Vector2 dir = targetPos - currentPos;
        float dist = dir.magnitude;
        if (dist > maxReach)
            targetPos = currentPos + dir.normalized * maxReach;

        // Move IK target smoothly
        transform.position = Vector2.Lerp(currentPos, targetPos, Time.deltaTime * moveSpeed);
    }
}
