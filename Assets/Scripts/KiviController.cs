using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiviController : BaseController
{

    public Vector2 voimavektori;
    // Start is called before the first frame update
    private Rigidbody2D r2d;
    public float kiertomaara = 10.0f;

    public Vector2 velocity;



    void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
        if (r2d != null)
        {

            if (velocity!=null && ( velocity.x!=0.0f || velocity.y!=0.0f) )
            {
             //   r2d.velocity = velocity;
            }
            else if ( voimavektori != null)
            {
                r2d.AddForce(voimavektori);
            }


            // rigidbody2D.velocity = liikevektori;

            r2d.AddTorque(kiertomaara); // positive value = counter-clockwise
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    //public float acceleration = 10f; // kuinka nopeasti saavutetaan targetVelocity
    public float kiihtyvyys5 = 4f;

    void FixedUpdate()
   
    {
        if (IsGoingToBeDestroyed())
        {
            return;
        }

        if (r2d == null) return;


        if (velocity.x != 0.0f || velocity.y != 0.0f)
        {
            // Liikuta velocityä asteittain kohti targetVelocity
            r2d.velocity = Vector2.MoveTowards(
                r2d.velocity,   // nykyinen nopeus
                velocity,         // haluttu nopeus
                kiihtyvyys5 * Time.fixedDeltaTime // muutos per frame
            );
        }

    }

}
