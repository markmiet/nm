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
                TeeBonus();
         //   }

        }

        // pallerotkokonaisuus = null;
        Destroy(gameObject);

    }

    private void TeeBonus()
    {
        for (float x = 0.0f; x < 100.0f; x += 0.1f)
        {
            //         Vector3 v3 =
            //new Vector3(0.1f +
            //m_Rigidbody2D.position.x+x, m_Rigidbody2D.position.y, 0);

            v2 = new Vector2(x, 0.0f);




            bool onko = OnkoBonusJoTuossa(v2);
            if (!onko)
            {
                Vector3 v3 =
new Vector3(
transform.position.x + v2.x, transform.position.y + v2.y, 0);
                Instantiate(bonus, v3, Quaternion.identity);


                Debug.Log("instantioidaan bonus");
                return;
            }
            else
            {
                Debug.Log("oli jo tuossa");
            }
        }

    }

    private bool OnkoBonusJoTuossa(Vector2 parametri)
    {
        // Vector2 v2 = new Vector2(v3.x, v3.y);
        string[] tagit = new string[1];
        tagit[0] = "bonustag";

        // SpriteRenderer s =bonus.GetComponent<SpriteRenderer>();

        // Vector2 boxsize = new Vector2(s.size.x, s.size.y);
        //   Vector2 boxsize = new Vector2(10.0f, 10.0f);

        bool onko = onkoTagiaBoxissa(tagit, boxsize, parametri, LayerMask.GetMask("BonusLayer"));

        return onko;
    }



    void OnDrawGizmos()
    {

        // Set the color of the Gizmos
        Gizmos.color = Color.green;

        // Draw the box representing the overlap area
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)transform.position + v2, boxsize);
    }


    public bool onkoTagiaBoxissa(string[] tagit, Vector2 boxsize, Vector2 boxlocation, LayerMask layerMask)
    {
        Vector2 uusi = (Vector2)transform.position + boxlocation;

        foreach (string name in tagit)
        {
            // Collider2D[] cs = Physics2D.OverlapBoxAll((Vector2)transform.position + boxlocation, boxsize, 0f, layerMask);
            Collider2D[] cs = Physics2D.OverlapBoxAll(uusi, boxsize, 0f);

            if (cs != null && cs.Length > 0)
            {
                foreach (Collider2D c in cs)
                {
                    Debug.Log("tagi=" + c.gameObject.tag);
                    if (c.gameObject == this.gameObject)
                    {

                    }
                    else if (c.gameObject.tag.Contains(name))
                    {
                        //  bool onko = IsInView((Vector2)transform.position + boxlocation);
                        //  return onko;
                        float olemassaolevanpalleronx = c.gameObject.transform.position.x;
                        float olemassaolevanpallerony = c.gameObject.transform.position.y;

                        float erox = Mathf.Abs(olemassaolevanpalleronx - uusi.x);
                        float eroy = Mathf.Abs(olemassaolevanpallerony - uusi.y);

                        if (erox < 0.5f || eroy < 0.5f)
                        {
                            return true;
                        }
                        else
                        {
                            Debug.Log("eri paikassa");
                        }
                        //return true;//
                    }
                }
            }


        }
        return false;

    }
}