using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalleroScripti : BaseController
{
    //jaahas scripti
    private Rigidbody2D m_Rigidbody2D;



    public int vasenkertojamaaramax;
    public int oikeayloskertojenmaaramax;
    public bool menossavasemmalle;

    private int vasenkertojamaara = 0;
    private int oikeayloskertojenmaara = 0;
    private bool meneeylospain = true;


    public GameObject ekapallero;

    public GameObject alus;
    public GameObject ammus;
    public int jarjestysnro;


    private bool alkuloysaatekemassa = true;

    private float ekanpalleronAloitusX;
    // Start is called before the first frame update

    private int starttimaara = 0;
    public GameObject explosion;

    public GameObject pallerotkokonaisuus;

    public GameObject bonus;
    private SpriteRenderer m_SpriteRenderer;

    private SpriteRenderer m_SpriteRendererEkapallero;

    private Vector2 v2 = new Vector2(0, 0);
    private Vector2 boxsize;// = new Vector2(0, 0);

    public float ampumisenvoimakkuus = 2.0f;
    public bool ammuBonuksenTekovaiheessa = true;

    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        //aloituspisteen perusteella 
        //transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        float ypos =
        transform.position.y;

        //Debug.Log("ypos=" + ypos);

        //Debug.Log("kameran koko=" + Camera.main.rect.height);
        // Camera.main.rect.
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));


        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        //  Debug.Log(" Screen.height=" + Screen.height);
        //  Debug.Log(" starttimaara=" + starttimaara++);

        if (ekapallero != null)
        {
            m_SpriteRendererEkapallero =
   ekapallero.GetComponent<SpriteRenderer>();

        }


        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);


        //Debug.Log(" screenPos=" + screenPos);


        float keski = Screen.height / 2;


        float kohtajossapyorylaon = screenPos.y;

        if (kohtajossapyorylaon >= keski)
        {
            meneeylospain = false;
        }

        /*
        Vector3 v3 =
new Vector3(
m_Rigidbody2D.position.x , m_Rigidbody2D.position.y, 0);




        GameObject instanssi = Instantiate(palleroPrefab, v3, Quaternion.identity);
        //instanssi.GetComponent<Rigidbody2D>().velocity = new Vector2(20, 0);
        
        */


        if (jarjestysnro != 0)
        {

            ekanpalleronAloitusX = ekapallero.transform.position.x;


        }
        boxsize = new Vector2(m_SpriteRenderer.size.x, m_SpriteRenderer.size.y);
    }

    // Update is called once per frame
    void Update()
    {


    }
    private bool OnkoOkLiikkua()
    {
        // return m_SpriteRendererEkapallero != null && m_SpriteRendererEkapallero.isVisible;
        if (onkook)
        {
            return true;
        }

        if (m_SpriteRendererEkapallero == null)
        {

            onkook = m_SpriteRenderer.isVisible;
        }
        else
        {
            onkook = m_SpriteRendererEkapallero.isVisible;
        }
        return onkook;

    }
    private bool onkook = false;



    private float kestoaika = 0.0f;

    void FixedUpdate()
    {

        if (!OnkoOkLiikkua())
        {
            return;
        }



        //   v2 = new Vector2(0.1f + transform.position.x, transform.position.y);


        //        boxsize = new Vector2(m_SpriteRenderer.size.x, m_SpriteRenderer.size.y);


        /*
        private void TeeBonus()
        {
            Vector3 v3 =
    new Vector3(0.1f +
    m_Rigidbody2D.position.x, m_Rigidbody2D.position.y, 0);
            bool onko = OnkoBonusJoTuossa(v3);
            Instantiate(bonus, v3, Quaternion.identity);


            SpriteRenderer s = bonus.GetComponent<SpriteRenderer>();
*/

        if (jarjestysnro != 0)
        {
            if (alkuloysaatekemassa)
            {
                if (ekanpalleronAloitusX >= transform.position.x)
                {
                    alkuloysaatekemassa = false;
                }
                else
                {
                    m_Rigidbody2D.velocity = new Vector2(-1f, 0);
                    return;
                }
            }

        }




        // m_Rigidbody2D.velocity = new Vector2(vauhtiOikea, vauhtiYlos);
        if (menossavasemmalle)
        {
            m_Rigidbody2D.velocity = new Vector2(-1f, 0);

            vasenkertojamaara++;

            if (vasenkertojamaara >= vasenkertojamaaramax)
            {
                vasenkertojamaara = 0;
                menossavasemmalle = false;

            }
        }
        else
        {
            if (meneeylospain)
            {
                m_Rigidbody2D.velocity = new Vector2(0.75f, 0.75f);
            }
            else
            {
                m_Rigidbody2D.velocity = new Vector2(0.75f, -0.75f);
            }

            oikeayloskertojenmaara++;
            if (oikeayloskertojenmaara >= oikeayloskertojenmaaramax)
            {
                oikeayloskertojenmaara = 0;
                menossavasemmalle = true;

            }
        }
    }



    public void Explode()
    {
        Debug.Log("pallero explore");

        //    GameObject explosionIns = Instantiate(explosion, transform.position, Quaternion.identity);

        //    Destroy(explosionIns, 1.0f);



        //onko kaikki pallerot destroytu
        if (pallerotkokonaisuus == null)
        {
            Debug.Log("pallerotnullia");
        }

        int lapset = pallerotkokonaisuus.transform.childCount;
        Debug.Log("lapset=" + lapset);
        if (lapset == 1)
        {
            //       Debug.Log("lapset=" + lapset);
            //   for (int i = 0; i < 3; i++)
            //   {
            //  TeeBonus();

            TeeBonus(bonus, v2, boxsize, 1);
            Ammu();


        }


        // pallerotkokonaisuus = null;
        RajaytaSprite(gameObject, 10, 10, 10.0f, 0.1f);
        Destroy(gameObject);

    }

    public void Ammu()
    {
        if (ammuBonuksenTekovaiheessa)
        {
            Vector2 ve = palautaAmmuksellaVelocityVector(alus, ampumisenvoimakkuus);

            Instantiate(ammus);



            GameObject instanssi = Instantiate(ammus, new Vector3(
     transform.position.x, transform.position.y, 0), Quaternion.identity);

            instanssi.GetComponent<Rigidbody2D>().velocity = ve;
        }


    }

    void OnBecameInvisible()
    {
        //Debug.Log ("OnBecameInvisible");
        // Destroy the enemy
        //tuhoa = true;

        Destroy(gameObject);
    }

}