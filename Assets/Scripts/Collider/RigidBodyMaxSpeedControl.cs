using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyMaxSpeedControl : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;

    public float ms = 1;

    public float maxAngularSpeed = 20.0f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    private bool pysaytysKaynnissa = false;

    void Update()
    {
        
        if (rb != null && rb.velocity.magnitude > ms )
        {
           // rb.velocity = rb.velocity.normalized * ms;
            rb.velocity = new Vector2(0, 0);
            //StartCoroutine(PysaytaHetkeksi());

            if (Mathf.Abs(rb.angularVelocity) > maxAngularSpeed)
            {
                rb.angularVelocity = Mathf.Sign(rb.angularVelocity) * maxAngularSpeed;
            }

        }
        
    }

    private IEnumerator PysaytaHetkeksi()
    {
        pysaytysKaynnissa = true;

        if (rb != null)
            rb.simulated = false;

        yield return new WaitForSeconds(0.01f);

        if (rb != null)
            rb.simulated = true;

        pysaytysKaynnissa = false;
    }
}
