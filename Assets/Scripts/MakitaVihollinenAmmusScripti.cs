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
		Debug.Log ("OnBecameInvisible");
		// Destroy the enemy
		tuhoa = true;

		//Destroy (gameObject);
	}


	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.collider.tag == "alustag") {
			//Destroy (col.gameObject);
			Debug.Log ("dame over");
		}
		else if (col.collider.tag == "makitavihollinentag" || col.collider.tag== "makitavihollinenammustag") {
			//Destroy (col.gameObject);
			//Debug.Log ("dame over");
	    		//just continue
		} else {
			tuhoa = true;
		}
	}

}
