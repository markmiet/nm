using UnityEngine;

[DisallowMultipleComponent]
public class SmartFlipByTarget2D : MonoBehaviour
{
    [Header("References")]
    public Transform target;             // Object to face (e.g. player, enemy, mouse)
    public Transform objectToFlip;       // The main visual root (sprite/mesh)

    [Header("Child Handling")]
    [Tooltip("Automatically fix local rotations of child sprites & colliders when flipped.")]
    public bool preserveChildOrientation = true;

    private bool facingRight = true;
    private Vector3 originalScale;

    void Reset()
    {
        objectToFlip = transform;
    }

    void Start()
    {
        if (!objectToFlip)
            objectToFlip = transform;

        originalScale = objectToFlip.localScale;
        facingRight = true;
    }

    void Update()
    {
      //  GetComponent<Animator>().enabled = true;

        if (!target) return;

        // Determine if target is left or right of this object
        bool targetIsLeft = target.position.x < transform.position.x;

        // Flip only when needed
        if (targetIsLeft && facingRight)
        {
            Flip(false);
        }
        else if (!targetIsLeft && !facingRight)
        {
            Flip(true);
        }
    }

    private void Flip(bool faceRight)
    {
       // GetComponent<Animator>().enabled = false;

        facingRight = faceRight;

        Vector3 scale = originalScale;
        scale.x *= facingRight ? 1f : -1f;
        objectToFlip.localScale = scale;

        Debug.Log("localscacel=" + objectToFlip.localScale);

        // Optionally fix child transforms (rotations, offsets)
        if (preserveChildOrientation)
            FixChildOrientations(objectToFlip, facingRight);
    }

    private void FixChildOrientations(Transform parent, bool faceRight)
    {
        foreach (Transform child in parent)
        {
            // Skip particle systems, lights, etc.
            if (child.GetComponent<SpriteRenderer>() || child.GetComponent<Collider2D>() || child.childCount > 0)
            {
                // Keep their local rotation visually correct
                Vector3 euler = child.localEulerAngles;
                euler.y = faceRight ? 0f : 180f;
                child.localEulerAngles = euler;

                // Recurse if nested children exist
                if (child.childCount > 0)
                    FixChildOrientations(child, faceRight);
            }
        }
    }
}
