using UnityEngine;

public class IgnoreChildCollisions2D : MonoBehaviour
{
    [Header("Settings")]
    public bool ignoreOnStart = true; // Automatically set up on Start

    private Collider2D[] childColliders;

    private void Start()
    { 
        if (ignoreOnStart)
            IgnoreAllChildCollisions();
    }

    /// <summary>
    /// Makes all Collider2D components in this GameObject’s hierarchy ignore each other.
    /// </summary>
    public void IgnoreAllChildCollisions()
    {
        // Get all Collider2D components in this GameObject and its children
        childColliders = GetComponentsInChildren<Collider2D>(includeInactive: true);

        // Go through every pair and make them ignore each other
        for (int i = 0; i < childColliders.Length; i++)
        {
            for (int j = i + 1; j < childColliders.Length; j++)
            {
                Collider2D c1 = childColliders[i];
                Collider2D c2 = childColliders[j];

                if (c1 != null && c2 != null)
                {
                    Physics2D.IgnoreCollision(c1, c2, true);
                }
            }
        }
    }

    /// <summary>
    /// Re-enables collisions between child colliders if previously ignored.
    /// </summary>
    public void RestoreAllChildCollisions()
    {
        if (childColliders == null || childColliders.Length == 0)
            childColliders = GetComponentsInChildren<Collider2D>(includeInactive: true);

        for (int i = 0; i < childColliders.Length; i++)
        {
            for (int j = i + 1; j < childColliders.Length; j++)
            {
                Collider2D c1 = childColliders[i];
                Collider2D c2 = childColliders[j];

                if (c1 != null && c2 != null)
                {
                    Physics2D.IgnoreCollision(c1, c2, false);
                }
            }
        }
    }
}
