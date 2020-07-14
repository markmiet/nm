using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlusController : MonoBehaviour {

	private Rigidbody2D m_Rigidbody2D;

	//vauhti
	private Animator m_Animator;

	private float oikeaNappiPainettu = 0.0f;

	private float vauhtiOikea = 0.0f;
	private float vauhtiOikeaMax = 4.0f;

	private float hidastuvuusKunMitaanEiPainettu = 0.3f;
	private float nopeudenMuutosKunPainettu = 1f;
	//ylos/alla

	private float ylosNappiPainettu = 0.0f;

	private float vauhtiYlos = 0.0f;
	private float vauhtiYlosMax = 4.0f;

	private bool spaceNappiaPainettu = false;
	private bool spaceNappiAlhaalla = false;
	private bool spaceNappiYlhaalla = false;



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
		oikeaNappiPainettu = Input.GetAxisRaw ("Horizontal"); // > 0 for right, < 0 for left
		ylosNappiPainettu = Input.GetAxisRaw ("Vertical"); // > 0 for right, < 0 for left
		spaceNappiaPainettu = Input.GetButton ("Jump");
		spaceNappiAlhaalla = Input.GetButtonDown ("Jump");
		spaceNappiYlhaalla = Input.GetButtonUp ("Jump");


	}

	void FixedUpdate ()
	{

		if (oikeaNappiPainettu == 0.0f) {
			//kumpaakaan ei painettu joten vaakasuunta nopeutta pitää vaan vähentä
			if (vauhtiOikea > 0) {
				vauhtiOikea = vauhtiOikea - hidastuvuusKunMitaanEiPainettu;
				if (vauhtiOikea < 0) {
					vauhtiOikea = 0.0f;
				}
			} else if (vauhtiOikea < 0) {
				vauhtiOikea = vauhtiOikea + hidastuvuusKunMitaanEiPainettu;
				if (vauhtiOikea > 0.0f) {
					vauhtiOikea = 0.0f;
				}
			}
		} else if (oikeaNappiPainettu > 0) {
			vauhtiOikea = vauhtiOikea + nopeudenMuutosKunPainettu;
		} else {
			//vasen painettu
			vauhtiOikea = vauhtiOikea - nopeudenMuutosKunPainettu;
		}

		if (vauhtiOikea > 0 && vauhtiOikea > vauhtiOikeaMax) {
			vauhtiOikea = vauhtiOikeaMax;
		} else if (vauhtiOikea < 0 && vauhtiOikea <= -(vauhtiOikeaMax)) {
			vauhtiOikea = -(vauhtiOikeaMax);
		}

		//alas/ylös
		if (ylosNappiPainettu == 0.0f) {
			//kumpaakaan ei painettu joten vaakasuunta nopeutta pitää vaan vähentä
			if (vauhtiYlos > 0) {
				vauhtiYlos = vauhtiYlos - hidastuvuusKunMitaanEiPainettu;
				if (vauhtiYlos < 0) {
					vauhtiYlos = 0.0f;
				}
			} else if (vauhtiYlos < 0) {
				vauhtiYlos = vauhtiYlos + hidastuvuusKunMitaanEiPainettu;
				if (vauhtiYlos > 0.0f) {
					vauhtiYlos = 0.0f;
				}
			}
		} else if (ylosNappiPainettu > 0) {
			vauhtiYlos = vauhtiYlos + nopeudenMuutosKunPainettu;
		} else {
			//vasen painettu
			vauhtiYlos = vauhtiYlos - nopeudenMuutosKunPainettu;
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
		Debug.Log ("spaceNappiYlhaalla=" + spaceNappiYlhaalla);

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