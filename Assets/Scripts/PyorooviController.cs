using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyorooviController : BaseController
{

    private SpriteRenderer m_SpriteRenderer;
    private Rigidbody2D rb;
    // Start is called before the first frame update
   // public float torqueAmount = 10000f; // Control the strength of the rotation
   // public Vector3 torqueDirection = Vector3.up; // Direction of the rotation (around the Y-axis)


    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        rb=GetComponent<Rigidbody2D>();
        
     //   rb.AddTorque(torqueAmount);
    }
    public float rotationSpeed = 30.0f;
    // Update is called once per frame

    void OnBecameInvisible()
    {
        //Debug.Log ("OnBecameInvisible");
        // Destroy the enemy
        //tuhoa = true;

   //     Destroy(gameObject);
    }
    void Update()
    {

     //   m_SpriteRenderer = GetComponent<SpriteRenderer>();
      //  rb = GetComponent<Rigidbody2D>();

    //    rb.AddTorque(torqueAmount);
    
    //       if (!OnkoOkLiikkua())
    //       {
    //           return;
    //       }
        float rotationAmount = rotationSpeed * Time.deltaTime;

    // Apply the rotation around the Y-axis (you can change the axis as needed)
    transform.Rotate(0, 0, rotationAmount);

        TuhoaJosVaarassaPaikassa(gameObject);

}
    public void FixedUpdate()
    {
       
    }

    


    private bool OnkoOkLiikkua()
    {
        return m_SpriteRenderer.isVisible;
    }

}
