using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakitaVihollinenScripti : MonoBehaviour
{

	private Rigidbody2D m_Rigidbody2D;

	private GameObject alus;
	private Animator m_Animator;
	// Start is called before the first frame update


	private SpriteRenderer m_SpriteRenderer;

	public GameObject ammusPrefab;


	GameObject instanssi = null;
	//public int FixedUpdateMaaraJokaVaaditaanEttaAmmutaan;
	void Start ()
       {
		m_Animator = GetComponent<Animator> ();

		m_SpriteRenderer = GetComponent<SpriteRenderer> ();


		alus = GameObject.FindGameObjectWithTag("alustag");

		m_Rigidbody2D = GetComponent<Rigidbody2D> ();

	}

	// Update is called once per frame
	void Update()
       {



	}

	void FixedUpdate ()
	{
		//vaaka,vali,ylos
		//Debug.Log ("alus x="+ alus.transform.position.x+" vihollinen x="+transform.position.x);

		//vasen, vasenylä, ylä
		Vector3 vihollinenPos=Camera.main.ScreenToWorldPoint (transform.position);
		Vector3 alusPos = Camera.main.ScreenToWorldPoint( alus.transform.position);

		/*
		if (alus.transform.position.y >= transform.position.y) {
			//alus ylapuolella
			m_SpriteRenderer.flipY = false;

		} else {
			//alus alapuolella
			m_SpriteRenderer.flipY = true;


		}
	*/

		//m_Animator.SetBool ("ylos", false);


		/*
		if (alus.transform.position.x <= transform.position.x) {
			m_SpriteRenderer.flipX = false;

			//float angle = Vector2.Angle (alus.transform.position, transform.position);


			//Debug.Log ("vasen");

		} else if (alus.transform.position.x > transform.position.x) {
			m_SpriteRenderer.flipX = true;

			//Debug.Log ("oikea");


		}
	*/

		float angle = Mathf.Atan2 (alus.transform.position.y - transform.position.y, alus.transform.position.x - transform.position.x) *
		 Mathf.Rad2Deg;

/*
		if (angle>=0 && angle<=22.5f) {
			//oikea 
			m_SpriteRenderer.flipX = true;
			m_Animator.SetBool ("vaaka", true);
			m_Animator.SetBool ("vali", false);
			m_Animator.SetBool ("ylos", false);


		} else if (angle > 22.5f && angle <= 45f) {
			//oikea väli 
			m_SpriteRenderer.flipX = true;
			m_Animator.SetBool ("vali", true);
			m_Animator.SetBool ("ylos", false);
			m_Animator.SetBool ("vaaka", false);


		} else if (angle >45f && angle <= 67.5f) {
			//oikea väli
			m_SpriteRenderer.flipX = true;
			m_Animator.SetBool ("ylos", false);
			m_Animator.SetBool ("vali", true);
			m_Animator.SetBool ("vaaka", false);
		} else if (angle > 67.5f && angle <= 90f) {
			//ylos 
			m_SpriteRenderer.flipX = true;
			m_Animator.SetBool ("ylos", true);
			m_Animator.SetBool ("vali", false);
			m_Animator.SetBool ("vaaka", false);
		}
		 else if (angle > 90f && angle <= 112.5f) {
			//ylos 
			m_SpriteRenderer.flipX = false;
			m_Animator.SetBool ("ylos", true);
			m_Animator.SetBool ("vali", false);
			m_Animator.SetBool ("vaaka", false);
		} else if (angle > 112.5f && angle <= 135f) {
			//vali 
			m_SpriteRenderer.flipX = false;
			m_Animator.SetBool ("ylos", false);
			m_Animator.SetBool ("vali", true);
			m_Animator.SetBool ("vaaka", false);
		} else if (angle > 112.5f && angle <= 157.5f) {
			//vali 
			m_SpriteRenderer.flipX = false;
			m_Animator.SetBool ("ylos", false);
			m_Animator.SetBool ("vali", true);
			m_Animator.SetBool ("vaaka", false);
		} else if (angle > 157.5f && angle <=180f) {
			//vasen alakulma 
			m_SpriteRenderer.flipX = false;
			m_Animator.SetBool ("ylos", false);
			m_Animator.SetBool ("vali", false);
			m_Animator.SetBool ("vaaka", true);
		}
*/

		if (angle >= 0 && angle <=10) {
			//oikea 
			m_SpriteRenderer.flipX = true;
			m_Animator.SetBool ("vaaka", true);
			m_Animator.SetBool ("vali", false);
			m_Animator.SetBool ("ylos", false);


		} else if (angle >10 && angle <= 45f) {
			//oikea väli 
			m_SpriteRenderer.flipX = true;
			m_Animator.SetBool ("vali", true);
			m_Animator.SetBool ("ylos", false);
			m_Animator.SetBool ("vaaka", false);


		} else if (angle > 45f && angle <= 67.5f) {
			//oikea väli
			m_SpriteRenderer.flipX = true;
			m_Animator.SetBool ("ylos", false);
			m_Animator.SetBool ("vali", true);
			m_Animator.SetBool ("vaaka", false);
		} else if (angle > 67.5f && angle <= 90f) {
			//ylos 
			m_SpriteRenderer.flipX = true;
			m_Animator.SetBool ("ylos", true);
			m_Animator.SetBool ("vali", false);
			m_Animator.SetBool ("vaaka", false);
		} else if (angle > 90f && angle <= 112.5f) {
			//ylos 
			m_SpriteRenderer.flipX = false;
			m_Animator.SetBool ("ylos", true);
			m_Animator.SetBool ("vali", false);
			m_Animator.SetBool ("vaaka", false);
		} else if (angle > 112.5f && angle <= 135f) {
			//vali 
			m_SpriteRenderer.flipX = false;
			m_Animator.SetBool ("ylos", false);
			m_Animator.SetBool ("vali", true);
			m_Animator.SetBool ("vaaka", false);
		} else if (angle > 112.5f && angle <= 170) {
			//vali 
			m_SpriteRenderer.flipX = false;
			m_Animator.SetBool ("ylos", false);
			m_Animator.SetBool ("vali", true);
			m_Animator.SetBool ("vaaka", false);
		} else if (angle >170 && angle <= 180f) {
			//vasen alakulma 
			m_SpriteRenderer.flipX = false;
			m_Animator.SetBool ("ylos", false);
			m_Animator.SetBool ("vali", false);
			m_Animator.SetBool ("vaaka", true);
		}



		float angle2 = Mathf.Atan2 (alus.transform.position.y - transform.position.y, alus.transform.position.x - transform.position.x) *
			 Mathf.Rad2Deg;

		Debug.Log ("angle2=" + angle2);

		//ampuminen eli
		if (instanssi==null) {
			instanssi = Instantiate (ammusPrefab, new Vector3 (
	m_Rigidbody2D.position.x + (m_SpriteRenderer.bounds.size.x / 2), m_Rigidbody2D.position.y+0.3f, 0), Quaternion.identity);


			float pysty = alus.transform.position.y - transform.position.y;
			float vaaka = alus.transform.position.x - transform.position.x;

			float suhdeluku = pysty / vaaka;

			float kokonaisvoima = 2f;

			float vaakavoima = kokonaisvoima * suhdeluku;
			float pystyvoima = kokonaisvoima - vaakavoima;

			Vector2  vv=new Vector2 (alus.transform.position.x - transform.position.x,
				alus.transform.position.y - transform.position.y);

			vv.Normalize ();
			vv.Scale(new Vector2(4.0f,4.0f));

			instanssi.GetComponent<Rigidbody2D> ().velocity = vv;

		}

	//	transform.position = new Vector2 (transform.position.x - 0.01f, 0);



	}
}
