using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KalaskeletonAlaspainController : BaseController
{
    public float forceAmount = 10f; // Amount of vertical force to apply
    public float interval = 1f;    // Time interval in seconds between applying force

    private float timer = 0f;      // Timer to track the interval
    private Rigidbody2D rb;       // Reference to the Rigidbody2D component


    // Start is called before the first frame update
    private Vector2 orig;
    void Start()
    {
        // Get the Rigidbody2D component attached to this GameObject
        rb = GetComponent<Rigidbody2D>();

        // Ensure Rigidbody2D exists
        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D attached to " + gameObject.name);
        }
        orig = new Vector2(transform.position.x, transform.position.y);
    }
    public float skrollimaara = -0.1f;
    public float skrollimaaraylos = 0.1f;

    // Update is called once per frame
    void Update()
    {  
        if (IsGoingToBeDestroyed())
        {
            return;
        }

        if (rb!=null && forceAmount>0)
        {
            // Increment the timer
            timer += Time.deltaTime;

            // Check if the interval has passed
            if (timer >= interval)
            {
                timer = 0f; // Reset the timer
                ApplyVerticalForce();
            }
        }
        if (forceAmount<=0.0f)
        {
            Vector2 skrolli = new Vector3(skrollimaara, skrollimaaraylos) * Time.deltaTime;
            //Debug.Log("skrolli=" + skrolli);

            orig += skrolli;
            MoveAnchor(orig);
        }

    }


    void ApplyVerticalForce()
    {
        // Apply an upward force to the Rigidbody2D
        rb.AddForce(Vector2.left * forceAmount, ForceMode2D.Force);

        Debug.Log($"Applied vertical force of {forceAmount} to {gameObject.name}");
    }
    void MoveAnchor(Vector2 worldPosition)
    {
        // Convert the world position to local space relative to the GameObject
        Vector2 localPosition = transform.InverseTransformPoint(worldPosition);


        // Set the new anchor position in local space
        GetComponent<HingeJoint2D>().anchor = localPosition;

        // Optional: Update connectedAnchor for consistent behavior if connected body is null
        if (GetComponent<HingeJoint2D>().connectedBody == null)
        {
            GetComponent<HingeJoint2D>().connectedAnchor = worldPosition;
          
        }
    }
}
