using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakitaVihollinenScripti : MonoBehaviour
{
	private GameObject alus;
	private Animator m_Animator;
	// Start is called before the first frame update


	private SpriteRenderer m_SpriteRenderer;



	void Start ()
       {
		m_Animator = GetComponent<Animator> ();

		m_SpriteRenderer = GetComponent<SpriteRenderer> ();


		alus = GameObject.FindGameObjectWithTag("alustag");
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




		Debug.Log ("angle=" + angle);


	}
}
