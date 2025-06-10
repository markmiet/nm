using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiviController : MonoBehaviour
{

    public Vector2 voimavektori;
    // Start is called before the first frame update
    private Rigidbody2D rigidbody2D;
    public float kiertomaara = 10.0f;

    public Vector2 velocity;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        if ( rigidbody2D!=null)
        {

            if (velocity!=null && ( velocity.x!=0.0f || velocity.y!=0.0f) )
            {
                rigidbody2D.velocity = velocity;
            }
            else if ( voimavektori != null)
            {
                rigidbody2D.AddForce(voimavektori);
            }

            
            // rigidbody2D.velocity = liikevektori;

            rigidbody2D.AddTorque(kiertomaara); // positive value = counter-clockwise
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
