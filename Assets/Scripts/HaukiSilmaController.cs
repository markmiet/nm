using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaukiSilmaController : BaseController, IExplodable
{
	// Start is called before the first frame update
	private Rigidbody2D m_Rigidbody2D;
	void Start()
    {
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	public bool paassakiinni = true;
	public float nopeusjonkaalleTuhoutuu = 0.2f;
	private bool tuhoa = false;
	// Update is called once per frame
	void Update()
    {
		if (tuhoa)
			return;
        float t = Mathf.PingPong(Time.time / duration, 1);

        // Lerp between the minScale and maxScale based on the t value
        transform.localScale = Vector3.Lerp(minScale, maxScale, t);

		if (!paassakiinni)
        {
			/*
			if (Mathf.Abs(m_Rigidbody2D.velocity.x) < nopeusjonkaalleTuhoutuu && Mathf.Abs(m_Rigidbody2D.velocity.y) < nopeusjonkaalleTuhoutuu)
			{
				Destroy(gameObject);
				//   Explode(0.0f);
			

				return;
			}
			*/
			float speed = m_Rigidbody2D.velocity.magnitude;
			//Debug.Log("ammuksen nopeus=" + speed);

			if (speed <= nopeusjonkaalleTuhoutuu)
			{
				Destroy(gameObject);
			}
		}
    }

    public Vector3 minScale = new Vector3(1, 1, 1);  // Minimum scale (e.g., original scale)
    public Vector3 maxScale = new Vector3(2.0f,2.0f, 2.0f);  // Maximum scale (e.g., doubled size)
    public float duration = 2f;                      // Time in seconds for one full cycle


    public void Explode()
    {
        RajaytaSprite(gameObject, 3, 3, 1.0f, 0.5f);
        
            Destroy(gameObject);
    }
    void OnBecameInvisible()
    {
        //Debug.Log ("OnBecameInvisible");
        // Destroy the enemy
        //tuhoa = true;

        Destroy(gameObject);
    }

    private bool tormattyviholliseen = false;
	/*
	void OnCollisionEnter2D(Collision2D col)
	{

		//      Debug.Log("tagi=" + col.collider.tag);
		if (col.collider.CompareTag("alustag"))
		{
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
			//tuhoa = true;
			//Destroy(gameObject);
			Explode();

		}
		else if (col.collider.CompareTag("makitavihollinenexplodetag") &&
			col.gameObject.transform == gameObject.transform.parent
			)
		{
			Debug.Log("ampui itesaan ignere");
			//instanssi.transform.parent = gameObject.transform;

		}
		else if (col.collider.tag.Contains("ammus"))
		{

		}
		else if (col.collider.tag.Contains("vihollinen") )
		{
			//Destroy (col.gameObject);
			//Destroy(gameObject);
			//Debug.Log ("dame over");
			//just continue
			//Destroy(gameObject);
			Explode();

		}
		else
		{
		//	tuhoa = true;
		}
	}

	*/
}
