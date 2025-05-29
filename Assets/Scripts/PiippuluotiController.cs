using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiippuluotiController : BaseController,IDamagedable
{
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (ve!=null)
		GetComponent<Rigidbody2D>().velocity = ve;

		TuhoaMuttaAlaTuhoaJosOllaanEditorissa(gameObject);

	}

	void OnBecameInvisible()
	{
		//	Debug.Log ("OnBecameInvisible");
		// Destroy the enemy
		//tuhoa = true;

			Destroy (gameObject);
	}
	public Vector2 ve;
	void OnCollisionEnter2D(Collision2D col)
	{
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
			)
		{
		}
		else if (col.collider.tag.Contains("ammus"))
		{

		}
		else if (col.collider.tag.Contains("vihollinen"))
		{
		}

	}
	

	public void Explode()
	{
		Destroy(gameObject);
	}
	public float osumiemaarajokaTarvitaanRajahdykseen = 1000f;
	private float nykyinenosuminenmaara = 0;

	public bool AiheutaDamagea(float damagemaara)
    {
		nykyinenosuminenmaara += damagemaara;
		if (nykyinenosuminenmaara >= osumiemaarajokaTarvitaanRajahdykseen)
		{
			Explode();
			return true;
		}
		return false;
	}
}