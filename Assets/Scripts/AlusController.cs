using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlusController : MonoBehaviour {

	private Rigidbody2D m_Rigidbody2D;

	//vauhti
	private Animator m_Animator;

	private bool oikeaNappiPainettu = false;

	private bool vasenNappiPainettu = false;

	private float vauhtiOikea = 0.0f;
	private float vauhtiOikeaMax = 4.0f;

	private float hidastuvuusKunMitaanEiPainettu = 0.3f;
	private float nopeudenMuutosKunPainettu = 1f;
	//ylos/alla

	private bool ylosNappiPainettu = false;
	private bool alasNappiPainettu = false;


	private float vauhtiYlos = 0.0f;
	private float vauhtiYlosMax = 4.0f;

	private bool spaceNappiaPainettu = false;
	//private bool spaceNappiAlhaalla = false;
	//private bool spaceNappiYlhaalla = false;



	private bool ammusInstantioitiinviimekerralla = false;

	public GameObject ammusPrefab;

	private SpriteRenderer m_SpriteRenderer;


	// Start is called before the first frame update
	void Start ()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D> ();
		m_Animator = GetComponent<Animator> ();

		m_SpriteRenderer = GetComponent<SpriteRenderer> ();

	}

	// Update is called once per frame
	void Update ()
	{
		float nappiHorizontal = Input.GetAxisRaw ("Horizontal");

		oikeaNappiPainettu = (nappiHorizontal > 0.0f); // > 0 for right, < 0 for left
		vasenNappiPainettu = (nappiHorizontal < 0.0f);

		float nappiVertical = Input.GetAxisRaw ("Vertical");


		ylosNappiPainettu = nappiVertical > 0; // > 0 for right, < 0 for left
		alasNappiPainettu = nappiVertical < 0;
		//spaceNappiaPainettu = Input.GetButton ("Jump");
		//spaceNappiAlhaalla = Input.GetButtonDown ("Jump");
		//spaceNappiYlhaalla = Input.GetButtonUp ("Jump");

		//spaceNappiaPainettu = Input.GetKeyDown (KeyCode.Space);
		if (Input.GetKeyDown (KeyCode.Space) ) {
			//Shoot ();
			Debug.Log ("space painettu " );
			spaceNappiaPainettu = true;

		}
		else {
			Debug.Log ("space ei painettu ");
			spaceNappiaPainettu = false;
		}


	}

	void FixedUpdate ()
	{

		if (oikeaNappiPainettu) {
			vauhtiOikea = vauhtiOikea + nopeudenMuutosKunPainettu;
		}
		if (vasenNappiPainettu) {
			//vasen painettu
			vauhtiOikea = vauhtiOikea - nopeudenMuutosKunPainettu;
		}
		if (!oikeaNappiPainettu && !vasenNappiPainettu) {
			if (vauhtiOikea > 0.0f) {
				vauhtiOikea = vauhtiOikea - hidastuvuusKunMitaanEiPainettu;
				if (vauhtiOikea < 0.0f) {
					vauhtiOikea = 0.0f;
				}
			} else if (vauhtiOikea < 0.0f) {
				vauhtiOikea = vauhtiOikea + hidastuvuusKunMitaanEiPainettu;
				if (vauhtiOikea > 0.0f) {
					vauhtiOikea = 0.0f;
				}
			}
		}



		if (vauhtiOikea > 0 && vauhtiOikea > vauhtiOikeaMax) {
			vauhtiOikea = vauhtiOikeaMax;
		} else if (vauhtiOikea < 0 && vauhtiOikea <= -(vauhtiOikeaMax)) {
			vauhtiOikea = -(vauhtiOikeaMax);
		}




		//alas/ylös
		if (ylosNappiPainettu) {
			vauhtiYlos = vauhtiYlos + nopeudenMuutosKunPainettu;
		}
		if (alasNappiPainettu) {
			//vasen painettu
			vauhtiYlos = vauhtiYlos - nopeudenMuutosKunPainettu;
		}



		if (!ylosNappiPainettu && !alasNappiPainettu) {
			if (vauhtiYlos > 0.0f) {
				vauhtiYlos = vauhtiYlos - hidastuvuusKunMitaanEiPainettu;
				if (vauhtiYlos < 0.0f) {
					vauhtiYlos = 0.0f;
				}
			} else if (vauhtiYlos < 0.0f) {
				vauhtiYlos = vauhtiYlos + hidastuvuusKunMitaanEiPainettu;
				if (vauhtiYlos > 0.0f) {
					vauhtiYlos = 0.0f;
				}
			}
		}



		if (vauhtiYlos > 0 && vauhtiYlos > vauhtiYlosMax) {
			vauhtiYlos = vauhtiYlosMax;
		} else if (vauhtiYlos < 0 && vauhtiYlos <= -(vauhtiYlosMax)) {
			vauhtiYlos = -(vauhtiYlosMax);
		}

		m_Rigidbody2D.velocity = new Vector2 (vauhtiOikea, vauhtiYlos);

		//        m_Rigidbody2D
		if (vauhtiYlos > 0.0f) {
			m_Animator.SetBool ("up", true);

		} else {
			m_Animator.SetBool ("up", false);
		}

		//Debug.Log("vauhtiOikea=" + vauhtiOikea);
		//Debug.Log("vauhtiYlos=" + vauhtiYlos);
//		Debug.Log ("spaceNappiYlhaalla=" + spaceNappiYlhaalla);

		//m_Rigidbody2D.position.
		bool ammusinstantioitiin = false;
		//tee vaan rämpytyksestä parempi...
		//tai sitten rajoitettu määrä ammuksia...

		if (spaceNappiaPainettu) {
			if (!ammusInstantioitiinviimekerralla) {
				GameObject instanssi = Instantiate (ammusPrefab, new Vector3 (
				m_Rigidbody2D.position.x + (m_SpriteRenderer.bounds.size.x / 2), m_Rigidbody2D.position.y, 0), Quaternion.identity);
				instanssi.GetComponent<Rigidbody2D> ().velocity = new Vector2 (20, 0);
				ammusinstantioitiin = true;
			}
		}
		ammusInstantioitiinviimekerralla = ammusinstantioitiin;

	}

}