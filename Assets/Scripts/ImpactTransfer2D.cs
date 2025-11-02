using UnityEngine;

/// <summary>
/// Detects collisions and transfers a portion of this object's motion energy
/// (based on velocity before collision) as an impulse force to the other Rigidbody2D.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class ImpactTransfer2D : MonoBehaviour
{
    [Header("Impact Settings")]
    [Tooltip("Multiplier for how much force is transferred on impact.")]
    public float forceMultiplier = 1f;

    [Tooltip("Maximum impulse magnitude that can be applied.")]
    public float maxImpulse = 20f;

    [Tooltip("If true, draw debug rays for collision direction and applied force.")]
    public bool debugDraw = true;

    private Rigidbody2D rb;
    public Vector2 lastVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Save velocity before physics step (used to detect pre-collision motion)
        lastVelocity = rb.velocity;
    }
    /*

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If we or the other object don't move, skip
        if (lastVelocity.sqrMagnitude < 0.01f) return;

        Rigidbody2D otherRb = collision.rigidbody;
        if (otherRb == null) return; // No Rigidbody2D on other object

        // Calculate impact direction based on pre-collision motion
        Vector2 impactDir = lastVelocity.normalized;

        // Determine force magnitude based on pre-collision speed
        float impactSpeed = lastVelocity.magnitude;
        float impulse = Mathf.Min(impactSpeed * forceMultiplier, maxImpulse);

        // Apply impulse to the other object
        otherRb.AddForce(impactDir * impulse, ForceMode2D.Impulse);

        if (debugDraw)
        {
            Debug.DrawRay(collision.contacts[0].point, impactDir * impulse, Color.red, 1f);
            Debug.Log($"{name} hit {otherRb.name} with impulse {impulse:F2}");
        }
    }
    */
}
