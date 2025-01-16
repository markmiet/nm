using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyController : BaseController,IExplodable
{
    // Start is called before the first frame update
    private float alivetime = 3.0f;
    private GameObject explosion;
    private Rigidbody2D pieceRigidbody;



    void Start()
    {
        pieceRigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetAlivetime(float p_alivetime)
    {
        alivetime = p_alivetime;
    }
    public void SetExplosion(GameObject go)
    {
        explosion= go;
    }

    // Update is called once per frame
    private float elossa = 0.0f;

    private float explosiontahti = 2.5f;
    private float explosionlaskuri = 0.0f;
    void Update()
    {

        elossa += Time.deltaTime;
        explosionlaskuri+= Time.deltaTime;



        if (elossa>= alivetime)
        {
            if (pieceRigidbody!=null)
            {
                /*
              pieceRigidbody.bodyType = RigidbodyType2D.Dynamic;
              pieceRigidbody.mass = 0.5f;

              pieceRigidbody.simulated = true;
              pieceRigidbody.constraints = RigidbodyConstraints2D.None;
              pieceRigidbody.gravityScale = 1.0f;

             */
                pieceRigidbody.bodyType = RigidbodyType2D.Static;

                pieceRigidbody.simulated = false;
                //if (explosionlaskuri >= explosiontahti)
                //{
                    if (explosion!=null)
                        Instantiate(explosion, transform.position, Quaternion.identity);
                //    explosionlaskuri = 0.0f;
                    Destroy(gameObject);
                //}
            }

        }
        else
        {
        //    pieceRigidbody.simulated = false;
          //  transform.Rotate(0,0 , kaatotahti * Time.deltaTime);

        }
        // pieceRigidbody.gravityScale = 0.0f;
        // transform.position = new Vector3(transform.position.x + liikex * Time.deltaTime, transform.position.y, transform.position.z);
    }

    private bool rajaytetty = false;
    public void Explode()
    {
        /*
        if (!rajaytetty)
        {
            rajaytetty = true;
            
            RajaytaSprite(gameObject, 3, 3, 3, 10.0f, 3, false,
    0, true, 1);
            Destroy(gameObject);
        }
        */

        //Explode();
    }

    private float liikex = -1f;
}
