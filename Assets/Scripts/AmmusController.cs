using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmusController : MonoBehaviour {
	private Animator m_Animator;
	private bool tuhoaViivella = false;
	private bool tuhoa = false;
	private bool tuhoamistoimenpiteetkaynnistetty = false;
	private int tormaysmaara = 0;

	private Rigidbody2D m_Rigidbody2D;
	// Start is called before the first frame update
	void Start ()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D> ();

		m_Animator = GetComponent<Animator> ();



	}

	// Update is called once per frame
	void Update ()
	{

	}


	void FixedUpdate ()
	{

		if (tuhoa && !tuhoamistoimenpiteetkaynnistetty) {
			tuhoamistoimenpiteetkaynnistetty = true;
			Destroy (gameObject);
		} else if (tuhoaViivella && !tuhoamistoimenpiteetkaynnistetty) {
			tuhoamistoimenpiteetkaynnistetty = true;
			m_Rigidbody2D.gravityScale = 5;

			//m_Animator.SetBool ("tormasi", true);

			m_Rigidbody2D.freezeRotation = false;

			//m_Rigidbody2D.velocity = new Vector2 (-1.0f, 0);

			//m_Rigidbody2D.rotation =45.0f;

			//		m_Rigidbody2D.rotation = annaRandomiKulmaAmmukselleTormayksenJalkeen ();

			Destroy (gameObject, 5);
		} else if (tuhoaViivella && tuhoamistoimenpiteetkaynnistetty && tormaysmaara > 2) {
			//transform.position = new Vector2 (transform.position.x - 0.01f, transform.position.y);

			Debug.Log ("tormaysmaara= " + tormaysmaara);
			Destroy (gameObject);
		}

	}


	private float annaRandomiKulmaAmmukselleTormayksenJalkeen ()
	{
		float f = Random.Range (0f, 360f);
		Debug.Log ("f=" + f);
		return f;


	}


	void OnCollisionEnter2D (Collision2D col)
	{
		Debug.Log (col.collider.tag);

		//Debug.Log ("OnCollisionEnter2D");

		//col.otherCollider.gameObject.name
		/*
			if (!tuhottu && col.collider.tag=="tiilitag" && m_Rigidbody2D!=null ) {
				tuhottu = true;
				m_Rigidbody2D.gravityScale = 5;
				Debug.Log ("ookoo");
				m_Rigidbody2D.velocity = new Vector2 (-1.0f, 0);
				Destroy (gameObject, 2);
			}
			else if (!tuhottu && col.collider.tag == "ammustag" && m_Rigidbody2D != null) {
				tuhottu = true;
				m_Rigidbody2D.gravityScale = 5;
				Debug.Log ("ookoo");
				m_Rigidbody2D.velocity = new Vector2 (-1.0f, 0);
				Destroy (gameObject, 2);
			} else if (!tuhottu && col.collider.tag == "alustag" && m_Rigidbody2D != null) {
				tuhottu = true;
				m_Rigidbody2D.gravityScale = 5;
				Debug.Log ("ookoo");
				m_Rigidbody2D.velocity = new Vector2 (-1.0f, 0);
				Destroy (gameObject, 2);

			}
		*/
		tuhoaViivella = true;
		tormaysmaara++;




	}

	void OnBecameInvisible ()
	{
		Debug.Log ("OnBecameInvisible");
		// Destroy the enemy
		tuhoa = true;

		//Destroy (gameObject);
	}

}