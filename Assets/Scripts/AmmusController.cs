using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmusController : BaseController, IExplodable {
	private Animator m_Animator;
	//private bool tuhoaViivella = false;
	//private bool tuhoa = false;
	//private bool tuhoamistoimenpiteetkaynnistetty = false;
	private int tormaysmaara = 0;

	private Rigidbody2D m_Rigidbody2D;

	private Renderer m_Renderer;
	private bool ollutnakyvissa = false;
	private bool ollutEinakyvissa = false;
	private bool firstime = true;

    public GameObject explosion;
	// Start is called before the first frame update

	private AudioplayerController ad;
	void Start ()
	{
		ad = FindObjectOfType<AudioplayerController>();
		m_Rigidbody2D = GetComponent<Rigidbody2D> ();

		m_Animator = GetComponent<Animator> ();
		m_Renderer = GetComponent<Renderer> ();
		//audiosourceexplode.Play();

		//	audiosourceexplode = GetComponent<AudioSource>();

	

	}

	// Update is called once per frame
	/*
	void Update ()
	{

		firstime = false;
	}
	*/

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

	public float nopeusjonkaalleTuhoutuu = 0.2f;

	private bool tuhoa = false;
	public void Update ()
	{



		/*
		if (tuhoa)
        {
			return;
        }
		*/
		//Debug.Log ("vauhti x=" + m_Rigidbody2D.velocity.x);
		//Debug.Log ("vauhti y=" + m_Rigidbody2D.velocity.y);
		/*
		if (Mathf.Abs (m_Rigidbody2D.velocity.x) <= nopeusjonkaalleTuhoutuu && Mathf.Abs (m_Rigidbody2D.velocity.y) <= nopeusjonkaalleTuhoutuu) {
			Debug.Log("ammuksen vauhti liian pieni tuhoa");
			Destroy (gameObject);
			//   Explode(0.0f);
			tuhoa = true;

            return;
		}
		*/
		float speed = m_Rigidbody2D.velocity.magnitude;
		Debug.Log("alusammuksen nopeus=" + speed);

		if (speed <= nopeusjonkaalleTuhoutuu)
		{
			Destroy(gameObject);
		}

		TuhoaJosVaarassaPaikassa(gameObject);

		/*
		if ( !m_Renderer.isVisible) {
				Debug.Log ("ei visible destroy");

				Destroy (gameObject);
		
		}
		el
		
		se
	*/

		/*
		//if (tuhoa && !tuhoamistoimenpiteetkaynnistetty) {
			//tuhoamistoimenpiteetkaynnistetty = true;
            //Explode(0.0f);

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
		*/

	}


	private float annaRandomiKulmaAmmukselleTormayksenJalkeen ()
	{
		float f = Random.Range (0f, 360f);
		//Debug.Log ("f=" + f);
		return f;


	}
	private bool tormattyviholliseen = false;

	void OnCollisionEnter2D (Collision2D col)
	{





		if (col.collider.tag.Equals("alustag"))
        {
			return;
        }

		if (tormattyviholliseen)
        {
			return;
        }
		

		if (col.collider.tag.Contains("vihollinen") && col.collider.tag.Contains("explode"))
		{
			tormattyviholliseen = true;
		//	Debug.Log("explodeeeeeeeeeeeeeeeee ");

			if (col.gameObject!=null)
            {
				//Debug.Log("gameobjektin tagi=" + col.gameObject.tag);
				
				//col.gameObject.SendMessage("Explode");
				IExplodable o =
				col.gameObject.GetComponent<IExplodable>();
				if (o!=null)
                {

					Debug.Log("	rajahdys.Play() kuuluuko");
					
					o.Explode();
				}
				else
                {
					Debug.Log("vihollinen ja explode mutta ei ookkaan "+ col.collider.tag);
                }
				tuhoa = true;
				Explode();
			}
			else
            {
				Debug.Log("gameobjekcti null");

            }
			
		}

		//Explode(0.0f);

	}


	void OnBecameInvisible ()
	{
		//Debug.Log ("OnBecameInvisible");
		// Destroy the enemy
		//tuhoa = true;

		Destroy (gameObject);
	}

	public void Explode()
	{
		GameObject explosionIns = Instantiate(explosion, transform.position, Quaternion.identity);
		Destroy(explosionIns, 1.0f);
		RajaytaSprite(gameObject, 3, 3, 1.0f, 0.5f);

		Destroy(gameObject);
	}

	/*
	bool IsVisibleFrom2 (this Renderer parent, Camera camera)
	{
		Plane [] planes = GeometryUtility.CalculateFrustumPlanes (camera);
		return GeometryUtility.TestPlanesAABB (planes, parent.bounds);
	}
	*/



	/*
	public void Explode(float viive)
    {
		Destroy(gameObject);

		GameObject explosionIns = Instantiate(explosion, transform.position, Quaternion.identity);

        Destroy(explosionIns, 1.0f);

//        Destroy(gameObject,viive);
		


	}
	*/



}