﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakitaVihollinenAmmusScripti : BaseController, IExplodable {

	private bool tuhoa = false;

	private Rigidbody2D m_Rigidbody2D;

	// Start is called before the first frame update
	void Start ()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame



	public float speedjonkaalletuhotaan = 0.5f;

	private bool TuhoaJosKameranAlla()
	{
		bool alla = IsObjectDownOfCamera(gameObject);
		if (alla)
		{
			Destroy(gameObject);
			Debug.Log("alla tuhoa");
			return true;
		}
		return false;


	}
	void Update ()
	{
		/*
		if (tuhoa) {
			Destroy (gameObject);
		}
		*/



		float x =
m_Rigidbody2D.velocity.x;
		float y = m_Rigidbody2D.velocity.y;
		float speed = m_Rigidbody2D.velocity.magnitude;
		Debug.Log("ammuksen nopeus=" + speed);

		if (speed <= speedjonkaalletuhotaan)
		{
			Debug.Log("liian hidas ammus tuhotaaan");
			Destroy(gameObject);
		}
		TuhoaJosVaarassaPaikassa(gameObject);
	}


	void OnBecameInvisible ()
	{
		Debug.Log ("OnBecameInvisible");
		// Destroy the enemy
		//tuhoa = true;

	//	Destroy (gameObject);
	}


	void OnCollisionEnter2D(Collision2D col)
	{

		//      Debug.Log("tagi=" + col.collider.tag);
		if (col.collider.CompareTag("alustag")) {
			//Destroy (col.gameObject);
			Debug.Log("game over");

			Animator aa = col.gameObject.GetComponent<Animator>();
			// aa.SetBool("explode", true);
			//  tuhoa = true;
			//Destroy(gameObject, 0.2f);

			// Destroy(gameObject,2f);

			// col.gameObject.SendMessage("Explode");

			IExplodable o =
col.gameObject.GetComponent<IExplodable>();
			if (o != null)
			{
				o.Explode();
			}
			else
			{
				Debug.Log("alus ja explode mutta ei ookkaan " + col.collider.tag);
			}
		}
		else if (col.collider.CompareTag("tiilivihollinentag"))
		{
			tuhoa = true;
			//Destroy(gameObject);
			Explode();

		}
		else if (col.collider.CompareTag("makitavihollinenexplodetag") &&
			col.gameObject.transform == gameObject.transform.parent
			) {
			Debug.Log("ampui itesaan ignere");
			//instanssi.transform.parent = gameObject.transform;

		}
		else if (col.collider.tag.Contains("ammus")) { 
			
		}
		else if (col.collider.tag.Contains("vihollinen") /*|| col.collider.tag== "makitavihollinenammustag"*/) {
			//Destroy (col.gameObject);
			//Destroy(gameObject);
			//Debug.Log ("dame over");
			//just continue
			//Destroy(gameObject);
			Explode();

		} else {
			tuhoa = true;
		}
	}

	public void Explode()
	{
		RajaytaSprite(gameObject, 3, 3, 1.0f, 0.5f);

		Destroy(gameObject);
	}
}
