using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakitaVihollinenAmmusScripti : MonoBehaviour {

	private bool tuhoa = false;



	// Start is called before the first frame update
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

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
			//Debug.Log ("dame over");
	    		//just continue
		} else {
			tuhoa = true;
		}
	}

}
