using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolygonCollider2D))]
public class PolygonColliderRotationUpdater : MonoBehaviour
{
    public PolygonColliderRotationData savedData;
    public float updateInterval = 0.2f; // seconds

    private PolygonCollider2D poly;

    void Awake()
    {
        poly = GetComponent<PolygonCollider2D>();
        StartCoroutine(UpdateColliderRoutine());
    }

    IEnumerator UpdateColliderRoutine()
    {
        while (true)
        {
            UpdateColliderForRotation();
            yield return new WaitForSeconds(updateInterval);
        }
    }

    void UpdateColliderForRotation()
    {
        if (savedData == null || savedData.colliders.Length == 0) return;

        float currentRot = transform.eulerAngles.z;

        // Find the closest saved rotation
        RotationCollider closest = savedData.colliders[0];
        float minDiff = Mathf.Abs(Mathf.DeltaAngle(currentRot, closest.rotation));

        foreach (var rc in savedData.colliders)
        {
            float diff = Mathf.Abs(Mathf.DeltaAngle(currentRot, rc.rotation));
            if (diff < minDiff)
            {
                minDiff = diff;
                closest = rc;
            }
        }

        // Apply collider points
        poly.pathCount = 1;
        poly.SetPath(0, closest.colliderPoints);
    }
}
