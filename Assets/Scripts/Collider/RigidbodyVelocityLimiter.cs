using UnityEngine;

public class RigidbodyVelocityLimiter : MonoBehaviour
{
    [Header("Linear Velocity")]
    public float maxSpeed = 1f;

    [Header("Angular Velocity")]
    public float maxAngularSpeed = 1f; // degrees per second

    private Rigidbody2D[] rigidbodies;

    void Start()
    {
        // Cache all Rigidbody2D components in this object and children
        rigidbodies = GetComponentsInChildren<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        foreach (Rigidbody2D rb in rigidbodies)
        {
            if (rb.gameObject.GetComponent<RigidBodyMaxSpeedControl>()!=null 
                && rb.gameObject.GetComponent<ApplyForces2D>()!=null)
            {

                // Clamp linear velocity
                if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
                {
                    rb.velocity = rb.velocity.normalized * maxSpeed;
                }

                // Clamp angular velocity
                if (Mathf.Abs(rb.angularVelocity) > maxAngularSpeed)
                {
                    rb.angularVelocity = Mathf.Sign(rb.angularVelocity) * maxAngularSpeed;
                }
            }

        }
    }
}
