using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakitaVihollinenScripti : BaseController
{

    private Rigidbody2D m_Rigidbody2D;

    private GameObject alus;
    private Animator m_Animator;
    // Start is called before the first frame update


    private SpriteRenderer m_SpriteRenderer;

    public GameObject ammusPrefab;
    //    int laskuri = 0;

    GameObject instanssi = null;
    //public int FixedUpdateMaaraJokaVaaditaanEttaAmmutaan;


    //  private BoxCollider2D boxCollider2D;
    private PolygonCollider2D polygonCollider2D;
    private Sprite sprite;


    private SpriteRenderer alusSpriteRenderer;

    private BoxCollider2D piipunboxit;

    float previousAngle = 0.0f;

    public GameObject explosion;

    public float ampumakertojenvalinenviive;

    public GameObject bonus;
    private Vector2 boxsize;// = new Vector2(0, 0);

    void Start()
    {

        piipunboxit = GetComponentInChildren<BoxCollider2D>();

        m_Animator = GetComponent<Animator>();

        m_SpriteRenderer = GetComponent<SpriteRenderer>();


        alus = GameObject.FindGameObjectWithTag("alustag");


        alusSpriteRenderer = alus.GetComponent<SpriteRenderer>();


        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        //    boxCollider2D = GetComponent<BoxCollider2D>();


        //  boxCollider2D.

        //boxCollider2D.enabled = false;



        polygonCollider2D = GetComponent<PolygonCollider2D>();

        sprite = m_SpriteRenderer.sprite;


        boxsize = new Vector2(m_SpriteRenderer.size.x, m_SpriteRenderer.size.y);

    }
    float deltaaikojensumma = 0f;

    bool vaihdaPolygonia = false;
    int pollaskuri = 0;
    // Update is called once per frame
    void Update()
    {

        //if (vaihdaPolygonia && pollaskuri++ >= 11)
        //{

        //    PolygonCollider2D[] old = this.gameObject.GetComponents<PolygonCollider2D>();
        //    foreach (PolygonCollider2D o in old)
        //    {
        //        Destroy(o);
        //    }
        //    this.gameObject.AddComponent<PolygonCollider2D>();
        //    pollaskuri = 0;
        //    vaihdaPolygonia = false;
        //}



    }

    bool firstime = true;

    private bool OnkoOkLiikkua()
    {
        return m_SpriteRenderer.isVisible;
    }

    void FixedUpdate()
    {


        if (alusSpriteRenderer == null || !OnkoOkLiikkua())
        {
            return;
        }

        // UpdatePolygonCollider2D();

        //vaaka,vali,ylos
        //Debug.Log ("alus x="+ alus.transform.position.x+" vihollinen x="+transform.position.x);

        //vasen, vasenylä, ylä
        //      Vector3 vihollinenPos = Camera.main.ScreenToWorldPoint(transform.position);
        //     Vector3 alusPos = Camera.main.ScreenToWorldPoint(alus.transform.position);

        /*
		if (alus.transform.position.y >= transform.position.y) {
			//alus ylapuolellago.AddComponent<PolygonCollider2D>();
			m_SpriteRenderer.flipY = false;

		} else {
			//alus alapuolella
			m_SpriteRenderer.flipY = true;


		}
	*/

        //m_Animator.SetBool ("ylos", false);

        /*

        if (alus.transform.position.x <= transform.position.x)
        {
            //m_SpriteRenderer.flipX = false;

            //float angle = Vector2.Angle (alus.transform.position, transform.position);


            //Debug.Log ("vasen");

            m_Animator.SetBool("left", true);


        }
        else if (alus.transform.position.x > transform.position.x)
        {
            //m_SpriteRenderer.flipX = true;

            //Debug.Log ("oikea");

            m_Animator.SetBool("left", false);


        }

        */



        //        m_SpriteRenderer.bounds.center.x
        //        alusSpriteRenderer.bounds.center.x

        float ammusx = 0f;//= m_SpriteRenderer.bounds.center.x;

        if (alusSpriteRenderer != null & alusSpriteRenderer.bounds.center.x <= m_SpriteRenderer.bounds.center.x)
        {
            //m_SpriteRenderer.flipX = false;

            //float angle = Vector2.Angle (alus.transform.position, transform.position);


            //Debug.Log ("vasen");

            m_Animator.SetBool("left", true);

            //  ammusx= polygonCollider2D.bounds.min.x;
        }
        else if (alusSpriteRenderer.bounds.center.x > m_SpriteRenderer.bounds.center.x)
        {
            //m_SpriteRenderer.flipX = true;

            //Debug.Log ("oikea");

            m_Animator.SetBool("left", false);
            // ammusx = polygonCollider2D.bounds.max.x;


            //   piipunboxit

        }







        ammusx = piipunboxit.bounds.center.x;

        //   polygonCollider2D.bounds.min.y


        //float angle = Mathf.Atan2(alus.transform.position.y - transform.position.y, alus.transform.position.x - transform.position.x) *
        //Mathf.Rad2Deg;


        float lisays = 0.0f;
        float alusy = alusSpriteRenderer.bounds.center.y;
        float alusx = alusSpriteRenderer.bounds.center.x;

        //float ammusy = m_SpriteRenderer.bounds.max.y;


        //float ammusy= polygonCollider2D.bounds.max.y;

        float ammusy = piipunboxit.bounds.center.y;


        if (alusSpriteRenderer.bounds.max.y < m_SpriteRenderer.bounds.min.y)
        {
            m_Animator.SetBool("animchangeallowed", false);
        }
        else
        {
            m_Animator.SetBool("animchangeallowed", true);
        }

        bool fireallowed;
        m_Animator.SetBool("animchangeallowed", true);


        //ei sallittua jos alus on ammuksen alapuolella
        /*
        if (alusSpriteRenderer.bounds.max.y < m_SpriteRenderer.bounds.min.y- )
        {
            fireallowed = false;

        }
        else
        {

            fireallowed = true;

        }
        */



        //sallittua jos alus on vähintään vihollisen alatasolla

        float aika = Time.deltaTime;
        deltaaikojensumma += aika;

        if (instanssi == null && deltaaikojensumma > ampumakertojenvalinenviive && m_SpriteRenderer.isVisible && alusSpriteRenderer.bounds.max.y + m_SpriteRenderer.size.y > m_SpriteRenderer.bounds.min.y)
        {
            fireallowed = true;

            deltaaikojensumma = 0;
        }
        else
        {

            fireallowed = false;

        }




        // fireallowed = false;

        //        Debug.Log("animchangeallowed=" + m_Animator.GetBool("animchangeallowed"));


        //    float ammusx= m_SpriteRenderer.bounds.center.x;



        //float angle = Mathf.Atan2(alusy -ammusy+lisays,alusx - ammusx) *
        //Mathf.Rad2Deg;




        //     float angle = Mathf.Atan2(alusy - polygonCollider2D.bounds.max.y + lisays, alusx - polygonCollider2D.bounds.center.x) *
        //Mathf.Rad2Deg;



        float angle = Mathf.Atan2(alusy - transform.position.y + lisays, alusx - transform.position.x) *
 Mathf.Rad2Deg;



        //if (m_Animator.GetBool("left"))
        //{
        //    angle = Mathf.Abs(angle);
        //}

        angle = Mathf.Abs(angle);

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

        /*
        if (angle >= 0 && angle <= 10)
        {
            //oikea
            // m_SpriteRenderer.flipX = true;
            m_Animator.SetBool("flipx", true);

            m_Animator.SetBool("vaaka", true);
            m_Animator.SetBool("vali", false);
            m_Animator.SetBool("ylos", false);


        }
        else if (angle > 10 && angle <= 45f)
        {
            //oikea väli
            //            m_SpriteRenderer.flipX = true;
            m_Animator.SetBool("flipx", true);

            m_Animator.SetBool("vali", true);
            m_Animator.SetBool("ylos", false);
            m_Animator.SetBool("vaaka", false);


        }
        else if (angle > 45f && angle <= 67.5f)
        {
            //oikea väli
            //            m_SpriteRenderer.flipX = true;
            m_Animator.SetBool("flipx", true);

            m_Animator.SetBool("ylos", false);
            m_Animator.SetBool("vali", true);
            m_Animator.SetBool("vaaka", false);
        }
        else if (angle > 67.5f && angle <= 90f)
        {
            //ylos
            //            m_SpriteRenderer.flipX = true;
            m_Animator.SetBool("flipx", true);

            m_Animator.SetBool("ylos", true);
            m_Animator.SetBool("vali", false);
            m_Animator.SetBool("vaaka", false);
        }
        else if (angle > 90f && angle <= 112.5f)
        {
            //ylos
            //            m_SpriteRenderer.flipX = false;
            m_Animator.SetBool("flipx", false);

            m_Animator.SetBool("ylos", true);
            m_Animator.SetBool("vali", false);
            m_Animator.SetBool("vaaka", false);
        }
        else if (angle > 112.5f && angle <= 135f)
        {
            //vali
            // m_SpriteRenderer.flipX = false;
            m_Animator.SetBool("flipx", false);

            m_Animator.SetBool("ylos", false);
            m_Animator.SetBool("vali", true);
            m_Animator.SetBool("vaaka", false);
        }
        else if (angle > 112.5f && angle <= 170)
        {
            //vali
            //m_SpriteRenderer.flipX = false;
            m_Animator.SetBool("flipx", false);


            m_Animator.SetBool("ylos", false);
            m_Animator.SetBool("vali", true);
            m_Animator.SetBool("vaaka", false);
        }
        else if (angle > 170 && angle <= 180f)
        {
            //vasen alakulma
            //m_SpriteRenderer.flipX = false;

            m_Animator.SetBool("flipx", false);

            m_Animator.SetBool("ylos", false);
            m_Animator.SetBool("vali", false);
            m_Animator.SetBool("vaaka", true);
        }

        */

        //eli vaihda sitä polygoncollideria vain jos tuo angle muuttuu ja säädä noi anglet järkeviksi...
        //        m_Animator.GetBool("1");


        bool animatorChanged = false;


        m_Animator.SetBool("left", false);
        if (angle >= 0 && angle <= 15)
        {
            //oikea
            // m_SpriteRenderer.flipX = true;
            //m_Animator.SetBool("flipx", true);

            //m_Animator.SetBool("vaaka", true);
            //m_Animator.SetBool("vali", false);
            //m_Animator.SetBool("ylos", false);

            //Debug.Log("angle=" + angle + " kohta 1");
            //m_Animator.SetBool("1", true);
            //m_Animator.SetBool("2", false);
            //m_Animator.SetBool("3", false);
            //m_Animator.SetBool("4", false);
            //m_Animator.SetBool("5", false);
            //m_Animator.SetBool("6", false);


            //m_Animator.SetBool("left", true);

            animatorChanged = SetAnimatorTrueksi(1);

        }
        else if (angle > 15 && angle <= 60f)
        {
            //oikea väli
            //            m_SpriteRenderer.flipX = true;
            //m_Animator.SetBool("flipx", true);

            //m_Animator.SetBool("vali", true);
            //m_Animator.SetBool("ylos", false);
            //m_Animator.SetBool("vaaka", false);
            //Debug.Log("angle=" + angle + " kohta 2");


            //m_Animator.SetBool("1", false);
            //m_Animator.SetBool("2", true);
            //m_Animator.SetBool("3", false);
            //m_Animator.SetBool("4", false);
            //m_Animator.SetBool("5", false);
            //m_Animator.SetBool("6", false);

            animatorChanged = SetAnimatorTrueksi(2);

        }
        else if (angle > 60 && angle <= 90)
        {
            //oikea väli
            //            m_SpriteRenderer.flipX = true;
            //m_Animator.SetBool("flipx", true);

            //m_Animator.SetBool("ylos", true);
            //m_Animator.SetBool("vali", false);
            //m_Animator.SetBool("vaaka", false);
            //Debug.Log("angle=" + angle + " kohta 3");


            //m_Animator.SetBool("1", false);
            //m_Animator.SetBool("2", false);
            //m_Animator.SetBool("3", true);
            //m_Animator.SetBool("4", false);
            //m_Animator.SetBool("5", false);
            //m_Animator.SetBool("6", false);

            animatorChanged = SetAnimatorTrueksi(3);
        }
        else if (angle > 90 && angle <= 120)
        {
            //ylos
            //            m_SpriteRenderer.flipX = true;
            //m_Animator.SetBool("flipx", false);

            //m_Animator.SetBool("ylos", true);
            //m_Animator.SetBool("vali", false);
            //m_Animator.SetBool("vaaka", false);
            //Debug.Log("angle=" + angle + " kohta 4");

            //m_Animator.SetBool("1", false);
            //m_Animator.SetBool("2", false);
            //m_Animator.SetBool("3", false);
            //m_Animator.SetBool("4", true);
            //m_Animator.SetBool("5", false);
            //m_Animator.SetBool("6", false);
            animatorChanged = SetAnimatorTrueksi(4);//vaihda takas 4
        }
        else if (angle > 120 && angle <= 165)
        {
            //ylos
            //            m_SpriteRenderer.flipX = false;
            //m_Animator.SetBool("flipx", false);

            //m_Animator.SetBool("ylos", false);
            //m_Animator.SetBool("vali", true);
            //m_Animator.SetBool("vaaka", false);
            //Debug.Log("angle=" + angle + " kohta 5");

            //m_Animator.SetBool("1", false);
            //m_Animator.SetBool("2", false);
            //m_Animator.SetBool("3", false);
            //m_Animator.SetBool("4", false);
            //m_Animator.SetBool("5", true);
            //m_Animator.SetBool("6", false);

            animatorChanged = SetAnimatorTrueksi(5);
        }
        else if (angle > 165 || (angle <= -180 && angle > -15))
        {
            //vali
            // m_SpriteRenderer.flipX = false;
            //m_Animator.SetBool("flipx", false);

            //m_Animator.SetBool("ylos", false);
            //m_Animator.SetBool("vali", false);
            //m_Animator.SetBool("vaaka", true);

            //Debug.Log("angle=" + angle + " kohta 6");

            //m_Animator.SetBool("1", false);
            //m_Animator.SetBool("2", false);
            //m_Animator.SetBool("3", false);
            //m_Animator.SetBool("4", false);
            //m_Animator.SetBool("5", false);
            //m_Animator.SetBool("6", true);

            animatorChanged = SetAnimatorTrueksi(6);

            m_Animator.SetBool("left", true);
        }

        m_Animator.SetFloat("angle", angle);


        //        Debug.Log("angle=" + angle);


        if (animatorChanged)
        {
            //laskuri = 0;
            vaihdaPolygonia = true;
            pollaskuri = 0;
        }


        /*
        if (angle < -30)
        {
            m_Animator.SetBool("animchangeallowed", false);


        }
        else
        {
            m_Animator.SetBool("animchangeallowed", true);
        }


*/



        //float angle2 = Mathf.Atan2(alus.transform.position.y - transform.position.y, alus.transform.position.x - transform.position.x) *
        //Mathf.Rad2Deg;




        //laskuri++;

        //ampuminen eli
        if (instanssi == null && !firstime && fireallowed)
        {

            //oli 0.3f
            //eli spriten asennosta on kiinni

            //        instanssi = Instantiate(ammusPrefab, new Vector3(
            //m_Rigidbody2D.position.x + (m_SpriteRenderer.bounds.size.x / 2), m_Rigidbody2D.position.y + 0.5f, 0), Quaternion.identity);

            instanssi = Instantiate(ammusPrefab, new Vector3(
    ammusx, ammusy + lisays, 0), Quaternion.identity);

            /*

            float pysty = alus.transform.position.y - transform.position.y;
            float vaaka = alus.transform.position.x - transform.position.x;

            float suhdeluku = pysty / vaaka;

            float kokonaisvoima = 2f;

            float vaakavoima = kokonaisvoima * suhdeluku;
            float pystyvoima = kokonaisvoima - vaakavoima;


            //           float angle = Mathf.Atan2(alusSpriteRenderer.bounds.center.y - m_SpriteRenderer.bounds.center.y, alusSpriteRenderer.bounds.center.x - m_SpriteRenderer.bounds.center.x) *
            //Mathf.Rad2Deg;


            Vector2 vv = new Vector2(alusx - ammusx,
               alusy - ammusy + lisays);
            */


            Vector2 vv = palautaAmmuksellaVelocityVector(alus, ampumisenkokonaisvoima);

            instanssi.GetComponent<Rigidbody2D>().velocity = vv;


        }

        //	transform.position = new Vector2 (transform.position.x - 0.01f, 0);



        //laskuri++;



        //}


        firstime = false;

    }
    public float ampumisenkokonaisvoima = 2.0f;



    private int laskuri = 0;

    private int lastAnimator = 0;

    private bool SetAnimatorTrueksi(int anim)
    {
        bool changed = lastAnimator != anim;
        if (changed)
        {
            //            Debug.Log("animaattori=" + anim);

            for (int i = 1; i <= 6; i++)
            {
                if (i == anim)
                {
                    // m_Animator.SetBool("" + i, true);
                }
                else
                {
                    //    m_Animator.SetBool("" + i, false);
                }

            }
        }
        lastAnimator = anim;
        return changed;
    }



    void LateUpdate()
    {

    }


    public void Explode()
    {

        GameObject explosionIns = Instantiate(explosion, transform.position, Quaternion.identity);

        Destroy(explosionIns, 1.0f);
        Destroy(gameObject);

        Vector3 v3 =
new Vector3(
m_Rigidbody2D.position.x, m_Rigidbody2D.position.y, 0);

        //  Instantiate(bonus, v3, Quaternion.identity);

        TeeBonus(bonus, v3, boxsize, 1);

    }



    void OnBecameInvisible()
    {
        //Debug.Log ("OnBecameInvisible");
        // Destroy the enemy
        //tuhoa = true;

        Destroy(gameObject);
    }
}
