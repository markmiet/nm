using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public bool alas = true;
    bool liukuu = false;
    private Rigidbody2D m_Rigidbody2D;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();


    }

    // Update is called once per frame
    void Update()
    {
        float speed = m_Rigidbody2D.velocity.magnitude;
 //       Debug.Log("ammuksen nopeus=" + speed);

        if (speed < 0.5f)
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
        Debug.Log("liukuuuuuuu");
        if (!liukuu)
        {
            liukuu = true;
            if (m_Rigidbody2D!=null)
            {
                m_Rigidbody2D.velocity = new Vector2(6, 0);
            }
    
        }

    }


    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("alaosa collidoi");

        if (col.collider.tag.Contains("tiili"))
        {

            //  col.otherCollider

            CapsuleCollider2D collider = col.otherCollider as CapsuleCollider2D;

            //eli jos törmäävä osa on tämä capsulocollider niin tuhoa 
            if (collider!=null)
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

            col.gameObject.SendMessage("Explode");
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

}
