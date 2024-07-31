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
        
    }

    public void Alas(bool al)
    {
        alas = al;
    }


    public void Putoa()
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



    public void Liu()
    {
        Debug.Log("liukuuuuuuu");
        if (!liukuu)
        {
            liukuu = true;
            m_Rigidbody2D.velocity = new Vector2(6, 0);
        }

    }


    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("alaosa collidoi");

        if (col.collider.tag == "tiilitag")
        {

            //  col.otherCollider

            CapsuleCollider2D collider = col.otherCollider as CapsuleCollider2D;
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
        else if (col.collider.tag == "makitavihollinentag")
        {

            col.gameObject.SendMessage("Explode");
            Destroy(gameObject);

            //Destroy (col.gameObject);

        }

        else if (col.collider.tag == "pallerospritetag")
        {

            col.gameObject.SendMessage("ExplodePallero");
            //col.gameObject.SendMessage("Explode");
            Destroy (col.gameObject);

        }
    }



    void OnBecameInvisible()
    {
        //Debug.Log ("OnBecameInvisible");
        // Destroy the enemy
        //tuhoa = true;

        Destroy(gameObject);
    }

}
