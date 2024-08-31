using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakitaVihollinenAmmusScripti : MonoBehaviour {

	private bool tuhoa = false;

	private Rigidbody2D m_Rigidbody2D;

	// Start is called before the first frame update
	void Start ()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update ()
	{
		//jos ei liiku tarpeeksi niin 

		/*
		Vector2 vv = new Vector2(alusx - ammusx,
		   alusy - ammusy + lisays);

		vv.Normalize();
		vv.Scale(new Vector2(4.0f, 4.0f));

		instanssi.GetComponent<Rigidbody2D>().velocity = vv;
		*/
		float x =
		m_Rigidbody2D.velocity.x;
		float y = m_Rigidbody2D.velocity.y;
		float speed = m_Rigidbody2D.velocity.magnitude;
		Debug.Log("ammuksen nopeus=" + speed);

		if (speed<0.5f)
        {
			Destroy(gameObject);
        }
	}

	void FixedUpdate ()
	{
		if (tuhoa) {
			Destroy (gameObject);
		}
	}


	void OnBecameInvisible ()
	{
//		Debug.Log ("OnBecameInvisible");
		// Destroy the enemy
		tuhoa = true;

		//Destroy (gameObject);
	}


	void OnCollisionEnter2D (Collision2D col)
    {

  //      Debug.Log("tagi=" + col.collider.tag);
		if (col.collider.tag == "alustag") {
			//Destroy (col.gameObject);
			Debug.Log ("game over");

            Animator aa=col.gameObject.GetComponent<Animator>();
            // aa.SetBool("explode", true);
            //  tuhoa = true;
            //Destroy(gameObject, 0.2f);

            // Destroy(gameObject,2f);

            col.gameObject.SendMessage("Explode");
        }
		else if (col.collider.tag == "tiilivihollinentag")
		{
			tuhoa = true;
		}
		else if (col.collider.tag.Contains("vihollinen") || col.collider.tag== "makitavihollinenammustag") {
			//Destroy (col.gameObject);
			//Destroy(gameObject);
			//Debug.Log ("dame over");
			//just continue
		} else {
			tuhoa = true;
		}
	}

}
