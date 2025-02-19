using UnityEngine;

public class HingeControl : MonoBehaviour
{
    public float torqueForce = 10f; // Force for rotation
    public float linearForce = 5f; // Force for linear movement
    public Vector2 forceDirection = Vector2.right; // Direction for linear force

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D found!");
        }
    }

    void Update()
    {
        if (rb != null)
        {
            // Rotate clockwise with "A" key and counter-clockwise with "D"
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddTorque(torqueForce);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                rb.AddTorque(-torqueForce);
            }

            // Move the object linearly with "W" and "S"
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(forceDirection.normalized * linearForce);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(-forceDirection.normalized * linearForce);
            }
        }
    }
}
