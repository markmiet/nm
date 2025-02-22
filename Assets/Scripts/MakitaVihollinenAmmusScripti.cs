using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakitaVihollinenAmmusScripti : BaseController, IExplodable {



	private Rigidbody2D m_Rigidbody2D;

	// Start is called before the first frame update
	void Start ()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

	}

	// Update is called once per frame



	public float speedjonkaalletuhotaan = 0.5f;

	/*
	private bool TuhoaJosKameranAlla()
	{
		bool alla = IsObjectDownOfCamera(gameObject);
		if (alla)
		{
			Destroy(gameObject);
			//Debug.Log("alla tuhoa");
			return true;
		}
		return false;


	}
	*/

	//private float checkInterval = 0.3f;
	//private float nextCheckTime;
	//public float nopeusjonkaalletuhotaan;
	void Update ()
	{

		TuhoaMuttaAlaTuhoaJosOllaanEditorissa(gameObject, speedjonkaalletuhotaan);

			/*
			if (Time.time >= nextCheckTime)
			{
				nextCheckTime = Time.time + checkInterval;
				float speed = m_Rigidbody2D.velocity.magnitude;
			//	Debug.Log("speed="+speed);
				if (speed <= speedjonkaalletuhotaan)
				{
				//	Debug.Log("Projectile too slow, destroying...");
					//Destroy(gameObject);
					Explode();
					return;
				}

			}
			TuhoaJosVaarassaPaikassa(gameObject);
			TuhoaJosEiKamerassa(gameObject);
			*/
		}


	void OnBecameInvisible ()
	{
	//	Debug.Log ("OnBecameInvisible");
		// Destroy the enemy
		//tuhoa = true;

	//	Destroy (gameObject);
	}
	public float damagemaarajokaaiheutetaan = 1.0f;

	void OnCollisionEnter2D(Collision2D col)
	{
		/*
		//      Debug.Log("tagi=" + col.collider.tag);
		if (col.collider.CompareTag("alustag")) {
			//Destroy (col.gameObject);
			//Debug.Log("game over");

			//Animator aa = col.gameObject.GetComponent<Animator>();
			// aa.SetBool("explode", true);
			//  tuhoa = true;
			//Destroy(gameObject, 0.2f);

			Explode();

			// col.gameObject.SendMessage("Explode");
			IExplodable ex = col.gameObject.GetComponent<IExplodable>();
			if (ex!=null)
            {
				ex.Explode();
            }
			else
            {
				IDamagedable o =
col.gameObject.GetComponent<IDamagedable>();
				if (o != null)
				{
					o.AiheutaDamagea(damagemaarajokaaiheutetaan);


				}
				else
				{
					//		Debug.Log("alus ja explode mutta ei ookkaan " + col.collider.tag);
				}
			}

		}
		else 
		*/
		if (col.collider.tag.Contains("alus"))
		{
			Destroy(gameObject);
			//Explode();

		}
		else if (col.collider.tag.Contains("tiili"))
		{
			//Destroy(gameObject);
			Explode();

		}
		else if (col.collider.CompareTag("makitavihollinenexplodetag") &&
			col.gameObject.transform == gameObject.transform.parent
			) {
		//	Debug.Log("ampui itesaan ignere");
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
			//Explode();

		} 
	}


	public float alivetimeRajahdyksenJalkeen = 0.5f;
	public void Explode()
	{
		//0.5f
		RajaytaSprite(gameObject, 3, 3, 1.0f, alivetimeRajahdyksenJalkeen);

		Destroy(gameObject);
	}
}
