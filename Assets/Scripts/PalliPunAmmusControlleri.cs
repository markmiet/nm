using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalliPunAmmusControlleri : BaseController
{
    // Start is called before the first frame update
    private GameObject alus;
    private Rigidbody2D m_Rigidbody2D;

    public bool kaannaObjektiLiikkeenSuuntaiseksi = false;
    void Start()
    {
        alus = PalautaAlus();

        target = alus.transform;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        /*
        if (kaannaObjektiLiikkeenSuuntaiseksi)
        {
            Vector2 velocity = m_Rigidbody2D.velocity;
            if (velocity.sqrMagnitude > 0.001f)
            {
                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }

        */

        /*
        Vector2 ve = palautaAmmuksellaVelocityVector(alus, force, transform.position);

        m_Rigidbody2D.velocity = ve;

        Vector2 velocity = m_Rigidbody2D.velocity;
        if (velocity.sqrMagnitude > 0.001f)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle+90);
        }
    */
    }


    //public float force = 1.0f;

    //public float sykli = 0.5f;
    //private float syklilaskuri = 0.0f;

    // Update is called once per frame
    private Transform target;      // Your "alus" target

    public float turnRate = 180f; // Degrees per second the missile can rotate
    public float speed = 5f;      // Constant forward speed
    void FixedUpdate()
    {
        if (IsGoingToBeDestroyed())
        {
            return;
        }

        if (target == null) return;

        // 1. Direction to target
        Vector2 toTarget = (Vector2)target.position - m_Rigidbody2D.position;
        toTarget.Normalize();

        // 2. Current facing direction (from Rigidbody rotation)
        Vector2 forward = transform.up; // Assuming missile's sprite points "up"

        // 3. Rotate toward target smoothly
        float rotateAmount = Vector3.Cross(forward, toTarget).z;
        m_Rigidbody2D.angularVelocity = rotateAmount * turnRate;

        // 4. Always move forward at constant speed
        m_Rigidbody2D.velocity = forward * speed;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (IsGoingToBeDestroyed())
        {
            return;
        }


        if (col.collider.tag.Contains("tiilivihollinen") || col.collider.tag.Contains("eituhvih") || col.collider.tag.Contains("sudenkorentovihollinenexplodetag") )
        {
            //RajaytaUudellaTavalla();

            RajaytaSpriteUusiMonimutkaisin(gameObject, uusirajaytyscolumns, uusirajaytysrows, 1.0f, 1.0f, null, 0.5f);

            //RajaytaSpriteUusiMonimutkaisin(gameObject, uusirajaytyscolumns, uusirajaytysrows, rajahdysvoima, alivetime, rajaytaSpritenExplosion, rajaytaspritenviive);

            
            //   Destroy(gameObject);
        }
    }

}
