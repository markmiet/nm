using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutonrunkoController : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public float downforceStrength = 10.0f;
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(-transform.up * downforceStrength);
    }
}
