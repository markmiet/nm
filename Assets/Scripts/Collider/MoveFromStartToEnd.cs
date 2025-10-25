using UnityEngine;
using System.Collections;

public class MoveAlongPoints : MonoBehaviour
{
    [Header("Path Points (in order)")]
    public GameObject[] points; // Assign in Inspector

    [Header("Movement Settings")]
    public float duration = 1f;         // Time (in seconds) to move between two points
    public bool smoothMovement = true;  // Use smooth Lerp or linear MoveTowards
    public bool loop = false;           // Loop back to start after reaching the end

    private int currentIndex = 0;

    private void Start()
    {
        if (points == null || points.Length < 2)
        {
            Debug.LogWarning("Assign at least 2 points in the 'points' array.");
            enabled = false;
            return;
        }

        // Start at the first point
        transform.position = points[0].transform.position;

        // Start movement
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            int nextIndex = currentIndex + 1;

            // Handle loop or end
            if (nextIndex >= points.Length)
            {
                if (loop)
                    nextIndex = 0;
                else
                    yield break;
            }

            Vector3 start = points[currentIndex].transform.position;
            Vector3 targetori = points[nextIndex].transform.position;
            Vector3 target = new Vector3(targetori.x, targetori.y, targetori.z);

            float elapsed = 0f;

            // Move over the specified duration
            while (elapsed < duration)
            {
                float t = elapsed / duration;

                // Optionally smooth the interpolation curve
                if (smoothMovement)
                    t = Mathf.SmoothStep(0f, 1f, t);

                transform.position = Vector3.Lerp(start, target, t);

                elapsed += Time.deltaTime;
                yield return null;
            }

            // Snap to final position
            transform.position = target;

            // Advance to next point
            currentIndex = nextIndex;
        }
    }
}
