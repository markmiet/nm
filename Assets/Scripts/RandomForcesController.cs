using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomForcesController : BaseController
{
    public float forceMin = 1f; // Minimum force
    public float forceMax = 2f; // Maximum force
    private Transform target; // Target GameObject

    public float maxSpeed = 5f; // Maximum speed the object can reach

    private Rigidbody2D rb;
    private float nextForceTime;

    public float rangetimemin = 1;
    public float rangetimemax = 3;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = PalautaAlus().transform;
        nextForceTime = Time.time + Random.Range(rangetimemin, rangetimemax);
        okc = GetComponentInParent<OnkoOkToimiaController>();
    }
    private OnkoOkToimiaController okc;
    void Update()
    {
 

        if (okc!=null && okc.voikotoimia && Time.time >= nextForceTime)
        {
            ApplyRandomForce();
            nextForceTime = Time.time + Random.Range(rangetimemin, rangetimemax);
        }

    }

    void ApplyRandomForce()
    {
        // Calculate the direction and random force
        Vector2 directionToTarget = (target.position - transform.position).normalized;
        float randomForce = Random.Range(forceMin, forceMax);

        // Apply force to the main object
        rb.AddForce(directionToTarget * randomForce, ForceMode2D.Impulse);


        // After applying the force, clamp the speed of this object
        ClampSpeed();
    }

    void ClampSpeed()
    {
        // If the object's velocity exceeds the maxSpeed, limit it
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}

