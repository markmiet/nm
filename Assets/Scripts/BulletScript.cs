using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : BaseController,  IExplodable, IAlas
{

    public bool alas = true;
    bool liukuu = false;
    private Rigidbody2D m_Rigidbody2D;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();


    }

    public float nopeusjonkaalleTuhoutuu = 0.2f;
    // Update is called once per frame
    void Update()
    {
        TuhoaJosVaarassaPaikassa(gameObject);
    }
    private void FixedUpdate()
    {
        float speed = m_Rigidbody2D.velocity.magnitude;
        //       Debug.Log("ammuksen nopeus=" + speed);

        if (speed <= nopeusjonkaalleTuhoutuu)
        {
            Destroy(gameObject);
        }
    }


    public void Alas(bool al)
    {
        alas = al;
    }


    public void Putoa()
    {

        if (m_Rigidbody2D != null)
        {
            if (alas)
            {
                m_Rigidbody2D.velocity = new Vector2(0.1f, -20);
            }
            else
            {
                m_Rigidbody2D.velocity = new Vector2(0.1f, 20);
            }
        }
    }



    public void Liu()
    {
        // Debug.Log("liukuuuuuuu");
        if (!liukuu)
        {
            liukuu = true;
            if (m_Rigidbody2D != null)
            {
                m_Rigidbody2D.velocity = new Vector2(6, 0);
            }

        }

    }


    void OnCollisionEnter2D(Collision2D col)
    {
        //  Debug.Log("alaosa collidoi");

        if (col.collider.tag.Contains("tiili"))
        {

            //  col.otherCollider

            CapsuleCollider2D collider = col.otherCollider as CapsuleCollider2D;

            //eli jos törmäävä osa on tämä capsulocollider niin tuhoa 
            if (collider != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Liu();
            }



            //   Destroy(transform.parent);

            //Destroy (col.gameObject)

            // transform.parent.gameObject.SendMessage("Liu");
        }
        else if (col.collider.tag.Contains("vihollinen") && col.collider.tag.Contains("explode"))
        {
            /*
            if (col.collider.tag.Contains("vihollinen") && col.collider.tag.Contains("explode"))
            {
                tormattyviholliseen = true;
                //	Debug.Log("explodeeeeeeeeeeeeeeeee ");

                if (col.gameObject != null)
                {
                    Debug.Log("gameobjektin tagi=" + col.gameObject.tag);

                    //col.gameObject.SendMessage("Explode");
                    IExplodable o =
                    col.gameObject.GetComponent<IExplodable>();
                    */

            // col.gameObject.SendMessage("Explode");
            if (col.gameObject != null)
            {
                IExplodable o =
col.gameObject.GetComponent<IExplodable>();
                if (o != null)
                {
                    o.Explode();
                }
                else
                {
                    Debug.Log("vihollinen ja explode mutta ei ookkaan " + col.collider.tag);
                }
            }


            Destroy(gameObject);

            //Destroy (col.gameObject);

        }
        /*
        else if (col.collider.tag == "pallerospritetag")
        {

            col.gameObject.SendMessage("ExplodePallero");
            //col.gameObject.SendMessage("Explode");
            Destroy (col.gameObject);

        }
        */
    }



    void OnBecameInvisible()
    {
        //Debug.Log ("OnBecameInvisible");
        // Destroy the enemy
        //tuhoa = true;

        Destroy(gameObject);
    }

    public void Explode()
    {
       // RajaytaSprite(gameObject, 3, 3, 1.0f, 0.5f);
        Destroy(gameObject);
    }

}
