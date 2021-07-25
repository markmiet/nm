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


	private Renderer m_Renderer;
	private bool ollutnakyvissa = false;
	private bool ollutEinakyvissa = false;
	private bool firstime = true;

    public GameObject explosion;
    	// Start is called before the first frame update
	void Start ()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D> ();

		m_Animator = GetComponent<Animator> ();
		m_Renderer = GetComponent<Renderer> ();

	}

	// Update is called once per frame
	void Update ()
	{
		/*
	if (m_Renderer.isVisible) {
		ollutnakyvissa = true;

	}
	if (!m_Renderer.isVisible && !firstime) {
		ollutEinakyvissa = true;
		Destroy (gameObject);
	}


	if (ollutEinakyvissa && ollutnakyvissa) {
		Debug.Log ("ei visible destroy updatessa");

		Destroy (gameObject);

	}
*/
		firstime = false;
	}


	void FixedUpdate ()
	{
		//Debug.Log ("vauhti x=" + m_Rigidbody2D.velocity.x);
		//Debug.Log ("vauhti y=" + m_Rigidbody2D.velocity.y);

		if (Mathf.Abs (m_Rigidbody2D.velocity.x) <0.2 && Mathf.Abs (m_Rigidbody2D.velocity.y) < 0.2) {
			Destroy (gameObject);
            Explode(0.0f);

            return;
		}


		/*
		if ( !m_Renderer.isVisible) {
				Debug.Log ("ei visible destroy");

				Destroy (gameObject);
		
		}
		el
		
		se
	*/

		if (tuhoa && !tuhoamistoimenpiteetkaynnistetty) {
			tuhoamistoimenpiteetkaynnistetty = true;
            Explode(0.0f);

        //   Destroy (gameObject);
		} else if (tuhoaViivella && !tuhoamistoimenpiteetkaynnistetty) {
			tuhoamistoimenpiteetkaynnistetty = true;
			m_Rigidbody2D.gravityScale = 5;

			//m_Animator.SetBool ("tormasi", true);

			m_Rigidbody2D.freezeRotation = false;

			//m_Rigidbody2D.velocity = new Vector2 (-1.0f, 0);

			//m_Rigidbody2D.rotation =45.0f;

			//		m_Rigidbody2D.rotation = annaRandomiKulmaAmmukselleTormayksenJalkeen ();

			//Destroy (gameObject,5f);

            Explode(0.0f);
        } else if (tuhoaViivella && tuhoamistoimenpiteetkaynnistetty && tormaysmaara > 2) {
			//transform.position = new Vector2 (transform.position.x - 0.01f, transform.position.y);

	//		Debug.Log ("tormaysmaara= " + tormaysmaara);

	//		Destroy (gameObject);
		}


	}


	private float annaRandomiKulmaAmmukselleTormayksenJalkeen ()
	{
		float f = Random.Range (0f, 360f);
		//Debug.Log ("f=" + f);
		return f;


	}


	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.collider.tag=="makitavihollinentag") {
			
            col.gameObject.SendMessage("Explode");

            //Destroy (col.gameObject);

        }


		/*
		if (col.collider.tag=="ammustag") {
			Debug.Log ("ammustörmäys");
			Destroy (gameObject);
			Destroy (col.collider);
		}
	*/

		//Debug.Log ("tormaystagi="+col.collider.tag);

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


		if (tormaysmaara > 20) {
			//Destroy (gameObject);
		}
        Explode(0.0f);

    }

	void OnBecameInvisible ()
	{
		//Debug.Log ("OnBecameInvisible");
		// Destroy the enemy
		//tuhoa = true;

		Destroy (gameObject);
	}


    /*
	bool IsVisibleFrom2 (this Renderer parent, Camera camera)
	{
		Plane [] planes = GeometryUtility.CalculateFrustumPlanes (camera);
		return GeometryUtility.TestPlanesAABB (planes, parent.bounds);
	}
	*/



    public void Explode(float viive)
    {


        GameObject explosionIns = Instantiate(explosion, transform.position, Quaternion.identity);

        Destroy(explosionIns, 1.0f);

        Destroy(gameObject,viive);

    }



}