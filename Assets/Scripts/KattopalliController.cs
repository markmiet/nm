using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KattopalliController : BaseController, IExplodable
{

    //ei hypi

    public Vector2 boxsizeylhaalla = new Vector2(2.0f, 2.0f);  // Size of the box
    public Vector2 boxsizekeskella = new Vector2(2.0f, 0.1f);  // Size of the box
    public Vector2 boxsizelaidatalhaalla = new Vector2(2.0f, 0.1f);  // Size of the box
    public Vector2 boxsizealhaalla = new Vector2(2.0f, 0.1f);  // Size of the box
    public float hatahypynkestoaika = 2.0f;

    public Vector2 vasenylaboxcenter = Vector2.zero;
    public Vector2 oikeaylaboxcenter = Vector2.zero;

    public Vector2 vasenboxcenter = Vector2.zero;



    public Vector2 ammustavartevasenboxcenter = Vector2.zero;
    public Vector2 boxsizekeskellaammustavartevasen = new Vector2(2.0f, 0.1f);  // Size of the box


    public Vector2 oikeaboxcenter = Vector2.zero;


    public Vector2 vasenalaboxcenter = Vector2.zero;
    public Vector2 oikeaalaboxcenter = Vector2.zero;


    public Vector2 alaboxcenter = Vector2.zero;



    public Vector2 oikeayla2center = Vector2.zero;
    public Vector2 vasenyla2center = Vector2.zero;

    public Vector2 boxsizeyla2 = new Vector2(2.0f, 0.1f);  // Size of the box



    private GameObject alusGameObject;

    public GameObject bonus;

    public GameObject ammus;
    public string[] tagilistaJoitaTutkitaan;


    //private Camera mainCamera;


    private SpriteRenderer m_SpriteRenderer;
    private Rigidbody2D rb;


    public int osumiemaarajokaTarvitaanRajahdykseen = 1;
    private float nykyinenosuminenmaara = 0.0f;
    private Vector2 boxsize;// = new Vector2(0, 0);
    private AudioplayerController ad;


    public int ampumistenmaksimimaara = 1;
    private int montakoonammuttu = 0;

    void Start()
    {

        ad = FindObjectOfType<AudioplayerController>();
        //   mainCamera = Camera.main;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();





        boxsize = new Vector2(m_SpriteRenderer.size.x, m_SpriteRenderer.size.y);
        rb.simulated = false;

        alusGameObject = PalautaAlus();

        /*
        if (gameObject.tag.Contains("ammus"))
        {
            onkoammus = true;
        }
        else
        {
            onkoammus = false;
        }
        */


    }


    //   public float rotationSpeed = 90f; // Degrees per second


    // Update is called once per frame
    void Update()

    {

        /*
        //nykyinenosuminenmaara;
        Color color = m_SpriteRenderer.color;
        color.a = PalautaFadeArvo();
        
        m_SpriteRenderer.color = color;

        float startAlpha = m_SpriteRenderer.color.a;


        */
        //  Debug.Log("alpha=" + startAlpha);

        TuhoaJosVaarassaPaikassa(gameObject);



    }



    public float erominimissaanJottaLiikutaan = 0.4f;


    private bool OnkoOkLiikkua(float ero)
    {
        return ero > erominimissaanJottaLiikutaan && m_SpriteRenderer.isVisible;
    }


    public float leftsidetutkinnanmaara = 0.4f;


    public float elamisenmaksimiaikajosammus = 10.0f;

    bool IsInLeftSide(GameObject obj)
    {

        // Convert the object's world position to viewport position
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(obj.transform.position);

        // Check if the object is within the left 10% of the camera's view
        if (viewportPosition.x >= 0 && viewportPosition.x <= leftsidetutkinnanmaara)
        {
            // Also ensure the object is within the vertical bounds of the screen
            if (viewportPosition.y >= 0 && viewportPosition.y <= 1)
            {
                // Ensure the object is in front of the camera
                return true;
            }
        }



        return false;
    }

    public float torqueForce = 10f;


    public float ammuntasykli = 3.0f;
    private float deltojensumma = 0.0f;

    public float nukkumisaikajosammus = 1.0f;
    private float deltojensummaliikkumis = -3.0f;

    public void FixedUpdate()
    {

        if (alusGameObject != null)
        {
            if (!OnkoOkToimia(gameObject))
            {
                return;
            }



            Ammu();

            /*
            if (onkoammus && deltojensumma >= 5.0f)
            {
                return;
            }
            */


            // if (deltojensumma> ammuntasykli)
            // {

            //deltojensumma = 0;

            //}



            rb.simulated = true;


            bool vasemmalle = false;

            float alusx = alusGameObject.transform.position.x;
            float x = transform.position.x;
            if (alusx < x)
            {
                //    bool onkopalleronruudunvasemmassareunassa = IsInLeftSide(gameObject);
                //    if (!onkopalleronruudunvasemmassareunassa)
                //    {
                vasemmalle = true;
                //    }  
            }
            //10% alue ruudusta 





            float ero = Mathf.Abs(alusx - x);
            /*
            bool onkoTiiliPallonAlla = OnkoTiiliPallonAlla();

            bool onkoTiiliVasemmallaAlhaalla = OnkoTiiliVasemmallaAlhaalla();

            bool onkoTiiliOikeallaAlhaalla = OnkoTiiliOikeallaAlhaalla();


            bool onkoTiiliOikealla = OnkoTiiliOikealla();
            bool onkoTiiliVasemmalla = OnkoTiiliVasemmalla();

            bool onkoTiiliVasemmallaYlhaalla = OnkoTiiliVasemmallaYlhaalla();

            bool onkoTiiliOikeallaYlhaalla = OnkoTiiliOikeallaYlhaalla();



            bool onkoTiiliOikeallaLahellaTutkitaanHypynAikanaVoidaankoSinnePainSiirtya = OnkoTiiliOikeallaLahellaTutkitaanHypynAikanaVoidaankoSinnePainSiirtya();

            bool onkoTiiliVasemmallaLahellaTutkitaanHypynAikanaVoidaankoSinnePainSiirtya = OnkoTiiliVasemmallaLahellaTutkitaanHypynAikanaVoidaankoSinnePainSiirtya();

            bool onkomenossaYlospain = OnkoMenossaylospain();
            bool onkomenossaAlaspain = OnkoMenossaAlaspain();
            //bool onkohyppyohi = OnkoHyppytyyppi1Ohi();//onkohypynsuuntavasemmalle


            bool onkoMakitaOikealla = OnkoMakitaOikealla();

            bool onkoPallovasemmalla = OnkoPalloVasemmalla();
            bool onkoPalloOikealla=OnkoPalloOikealla();

            //bool onkohyppy2ohi = OnkoHyppytyyppi2Ohi();

            bool aluksenammusvasemmalla=OnkoAluksenammusVasemmalla();

            bool palloovioikealla = OnkoPyorivaPalloOviOikealla();
            bool palloovivasemmalla = OnkoPyorivaPalloOviVasemmalla();
            */
            if (OnkoOkLiikkua(ero))
            {
                if (vasemmalle)
                {
                    rb.AddTorque(-torqueForce);
                }
                else
                {
                    rb.AddTorque(torqueForce);
                }
            }


            /*

            if (OnkoOkLiikkua(ero) &&   !onkomenossaYlospain)
            {

                if (vasemmalle)
                {

                    // Debug.Log("vasemmalle=" + vasemmalle + " onkoTiiliVasemmalla=" + onkoTiiliVasemmalla + " onkoTiiliVasemmallaAlhaalla=" + onkoTiiliVasemmallaAlhaalla);
                    //    rb.velocity = new Vector2(-1f, rb.velocity.y);
                    //                    transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);


                    //rb.AddTorque(torqueDirectionUp * torqueAmount, ForceMode.Force);

                    if (!onkoTiiliVasemmalla && onkoTiiliVasemmallaAlhaalla && !onkoPallovasemmalla)
                    {
                        
                      c
                        


                    }
                    else
                    {
                       // JointMotor2D motor = wheelJoint.motor;
                       // motor.motorSpeed = 0.0f;//stopataan
                       // wheelJoint.motor = motor;
                    }



                }
                else
                {

                    //  rb.velocity = new Vector2(1f, rb.velocity.y);
                    // transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);

                    //   Debug.Log("vasemmalle=" + vasemmalle + " onkoTiiliOikealla=" + onkoTiiliOikealla + " onkoTiiliOikeallaAlhaalla=" + onkoTiiliOikeallaAlhaalla);
                    if (!onkoTiiliOikealla && onkoTiiliOikeallaAlhaalla && !onkoPalloOikealla)
                    {

                        rb.AddTorque(torqueForce);


                    }
                    else
                    {

                    }
                }
            }

            else
            {

//stop

            }
            */


        }
    }

    private bool OnkoMenossaylospain()
    {
        bool ylos = rb.velocity.y > 0.03f;
        return ylos;

    }
    private bool OnkoMenossaAlaspain()
    {
        bool ylos = rb.velocity.y < -0.03f;
        return ylos;

    }
    private bool OnkoMenossaAlasPainKovaaVauhtia()
    {
        bool ylos = rb.velocity.y < -0.03f;
        return ylos;
    }




    /*
    public void hyppaa(bool vasen)
    {
        if (!OnkoSeinaYlhaalla())
        {
            float xsuunnassa;
            if (vasen)
            {
                xsuunnassa = -20.0f;
            }
            else
            {
                xsuunnassa = 20.0f;
            }

            rb.velocity = new Vector2(rb.velocity.x, 20.0f);

            //   StartCoroutine(ChangeVelocityAfterDelay(xsuunnassa));
            deltojensumma = 0;
        }
    }
    IEnumerator ChangeVelocityAfterDelay(float xsuunnassa)
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(0.1f);
        rb.velocity = new Vector2(rb.velocity.x + xsuunnassa, rb.velocity.y);

    }
    */





    void OnDrawGizmos()
    {

        // Set the color of the Gizmos
        Gizmos.color = Color.green;

        // Draw the box representing the overlap area
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)transform.position + vasenylaboxcenter, boxsizeylhaalla);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube((Vector2)transform.position + oikeaylaboxcenter, boxsizeylhaalla);


        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube((Vector2)transform.position + oikeaboxcenter, boxsizekeskella);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube((Vector2)transform.position + vasenboxcenter, boxsizekeskella);


        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube((Vector2)transform.position + ammustavartevasenboxcenter, boxsizekeskellaammustavartevasen);



        //kulmat

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + vasenalaboxcenter, boxsizelaidatalhaalla);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube((Vector2)transform.position + oikeaalaboxcenter, boxsizelaidatalhaalla);

        Gizmos.color = Color.black;
        Gizmos.DrawWireCube((Vector2)transform.position + alaboxcenter, boxsizealhaalla);


        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube((Vector2)transform.position + oikeayla2center, boxsizeyla2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube((Vector2)transform.position + vasenyla2center, boxsizeyla2);



        //
        // Gizmos.color = Color.yellow;
        // Gizmos.DrawWireCube((Vector2)transform.position + oikeaboxcenterJokakasvaa, boxsizekeskella);

        // oikeaboxcenterJokakasvaa=new Vector2(oikeaboxcenterJokakasvaa.x,oikeaboxcenterJokakasvaa.y+0.01f);

        //   Vector2 aloituspiste = Vector2.zero;


    }
    bool tutkitaankorkeutta = false;


    // public Vector2 oikeaboxcenterJokakasvaa = Vector2.zero;


    private bool OnkoTiiliPallonAlla()
    {
        // return true;

        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissaErikoisversio(tagilistaJoitaTutkitaan, boxsizealhaalla, alaboxcenter, layerMask);
        return onkotiilioikeallaAlhaalla;

    }

    private bool OnkoTiiliVasemmallaAlhaalla()
    {


        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissaErikoisversio(tagilistaJoitaTutkitaan, boxsizelaidatalhaalla, vasenalaboxcenter, layerMask);
        return onkotiilioikeallaAlhaalla;

    }

    private bool OnkoTiiliOikeallaAlhaalla()
    {


        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissaErikoisversio(tagilistaJoitaTutkitaan, boxsizelaidatalhaalla, oikeaalaboxcenter, layerMask);
        return onkotiilioikeallaAlhaalla;

    }


    private bool OnkoTiiliOikealla()
    {


        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissaErikoisversio(tagilistaJoitaTutkitaan, boxsizekeskella, oikeaboxcenter, layerMask);
        return onkotiilioikeallaAlhaalla;

    }


    private bool OnkoPyorivaPalloOviOikealla()
    {

        string[] tagit = new string[1];
        tagit[0] = "pyoroovivihollinentag";

        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissaErikoisversio(tagit, boxsizekeskella, oikeaboxcenter);
        return onkotiilioikeallaAlhaalla;
    }
    private bool OnkoPyorivaPalloOviVasemmalla()
    {

        string[] tagit = new string[1];
        tagit[0] = "pyoroovivihollinentag";

        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissaErikoisversio(tagit, boxsizekeskella, vasenboxcenter);
        return onkotiilioikeallaAlhaalla;
    }




    private bool OnkoMakitaOikealla()
    {


        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissaErikoisversio(tagilistaJoitaTutkitaan, boxsizekeskella, oikeaboxcenter, layerMask);
        return onkotiilioikeallaAlhaalla;

    }

    private bool OnkoTiiliVasemmalla()
    {


        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissaErikoisversio(tagilistaJoitaTutkitaan, boxsizekeskella, vasenboxcenter, layerMask);
        return onkotiilioikeallaAlhaalla;

    }

    public string[] aluksenammuksentagilista;

    private bool OnkoAluksenammusVasemmalla()
    {



        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissaErikoisversio(aluksenammuksentagilista, boxsizekeskella, vasenboxcenter);
        return onkotiilioikeallaAlhaalla;

    }



    private bool OnkoTiiliVasemmallaYlhaalla()
    {


        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissaErikoisversio(tagilistaJoitaTutkitaan, boxsizeylhaalla, vasenylaboxcenter, layerMask);
        return onkotiilioikeallaAlhaalla;

    }

    private bool OnkoTiiliOikeallaYlhaalla()
    {


        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissaErikoisversio(tagilistaJoitaTutkitaan, boxsizeylhaalla, oikeaylaboxcenter, layerMask);
        return onkotiilioikeallaAlhaalla;

    }
    private bool OnkoTiiliOikeallaLahellaTutkitaanHypynAikanaVoidaankoSinnePainSiirtya()
    {


        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissaErikoisversio(tagilistaJoitaTutkitaan, boxsizeyla2, oikeayla2center, layerMask);
        return onkotiilioikeallaAlhaalla;

    }

    private bool OnkoTiiliVasemmallaLahellaTutkitaanHypynAikanaVoidaankoSinnePainSiirtya()
    {


        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissaErikoisversio(tagilistaJoitaTutkitaan, boxsizeyla2, vasenyla2center, layerMask);
        return onkotiilioikeallaAlhaalla;

    }



    public LayerMask layerMask;


    public bool tuhoaJosOnBecameInvisible = true;

    void OnBecameInvisible()
    {
        //MJM 18.12.2023 OTA POIS KOMMENTEISTA
        if (tuhoaJosOnBecameInvisible)
        {
            Destroy(gameObject);
        }
        // 

        /*
        if (gameObject==null)
        {
            return;
        }
        bool vasemmalla=
        IsObjectLeftOfCamera(gameObject);
        if (vasemmalla)
        {
            Destroy(gameObject);
            Debug.Log("vsen tuhoa");

        }
        bool alla = IsObjectDownOfCamera(gameObject);
        if (alla)
        {
            Destroy(gameObject);
            Debug.Log("alla tuhoa");

        }
        */
    }
    /*
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
    */


    private void OnBecameVisible()
    {

    }

    bool IsObjectRightOfCamera(Camera cam, Transform objTransform)
    {
        if (objTransform == null)
        {
            return false;
        }
        // Convert object's world position to viewport coordinates
        Vector3 viewportPoint = cam.WorldToViewportPoint(objTransform.position);

        // Check if the object is on the right side of the camera and not visible
        // x > 1 means the object is beyond the right edge of the camera's view
        // y should be between 0 and 1 (within the vertical bounds of the camera)
        // z > 0 means the object is in front of the camera, not behind
        return viewportPoint.x > 1 && viewportPoint.z > 0;
    }

    public bool OnkoPalloVasemmalla()
    {
        string[] tagit = new string[1];
        tagit[0] = "pallovihollinenexplodetag";


        bool ret = onkoTagiaBoxissaErikoisversio(tagit, boxsizekeskella, vasenboxcenter, layerMask);
        return ret;
    }

    public bool OnkoPalloOikealla()
    {
        string[] tagit = new string[1];
        tagit[0] = "pallovihollinenexplodetag";


        bool ret = onkoTagiaBoxissaErikoisversio(tagit, boxsizekeskella, oikeaboxcenter, layerMask);
        return ret;
    }


    public bool onkoTagiaBoxissaErikoisversio(string[] tagit, Vector2 boxsize, Vector2 boxlocation, LayerMask layerMask)
    {
        //string aa = "pallovihollinenexplodetag";

        foreach (string name in tagit)
        {

            // Physics2D.OverlapBoxAll

            //Collider2D[] cs = Physics2D.OverlapBoxAll((Vector2)transform.position + boxlocation, boxsize, 0f);

            Collider2D[] cs = Physics2D.OverlapBoxAll((Vector2)transform.position + boxlocation, boxsize, 0f, layerMask);


            if (cs != null && cs.Length > 0)
            {
                foreach (Collider2D c in cs)
                {
                    if (c.gameObject == this.gameObject)
                    {

                    }
                    else if (c.gameObject.tag.Contains(name))
                    {
                        //  bool onko = IsInView((Vector2)transform.position + boxlocation);
                        //  return onko;
                        return true;
                    }
                }
            }


        }
        return false;

    }

    public bool onkoTagiaBoxissaErikoisversio(string[] tagit, Vector2 boxsize, Vector2 boxlocation)
    {
        //string aa = "pallovihollinenexplodetag";

        foreach (string name in tagit)
        {

            // Physics2D.OverlapBoxAll

            //Collider2D[] cs = Physics2D.OverlapBoxAll((Vector2)transform.position + boxlocation, boxsize, 0f);

            Collider2D[] cs = Physics2D.OverlapBoxAll((Vector2)transform.position + boxlocation, boxsize, 0f);


            if (cs != null && cs.Length > 0)
            {
                foreach (Collider2D c in cs)
                {
                    if (c.gameObject == this.gameObject)
                    {

                    }
                    else if (c.gameObject.tag.Contains(name))
                    {
                        //  bool onko = IsInView((Vector2)transform.position + boxlocation);
                        //  return onko;
                        return true;
                    }
                }
            }


        }
        return false;

    }




    bool IsInView(Vector2 worldPosition)
    {

        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(worldPosition);
        return viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
               viewportPoint.y >= 0 && viewportPoint.y <= 1;
    }



    public GameObject explosion;
    public float gravitymodifiermuutoskunammusosuu = 0.1f;

    //lahto on -1
    float gvarinalkutilanne;

    float gvarinlopputilanne = 0.0f;

    private float PalautaGvari()
    {
        float prosentti = nykyinenosuminenmaara / osumiemaarajokaTarvitaanRajahdykseen;

        float ero = gvarinalkutilanne - gvarinlopputilanne;
        float summaerosta = ero * prosentti;

        float arvo = gvarinalkutilanne - summaerosta;
        return arvo;

    }


    public void Explode()
    {

        ExplodeOikeasti();

    }



    public int rajaytysrows = 4;
    public int rajaytyscols = 4;

    public float rajaytysvoima = 2f;
    public float rajaytyskestoaika = 0.8f;
    public bool teerajaytys = false;
    public void ExplodeOikeasti()
    {


        //  Instantiate(bonus, v3, Quaternion.identity)
        int bonusmaara = 1;

        ad.ExplodePlay();


        //  
        //RajaytaSprite(gameObject, 5, 5, 4.0f, 1.5f);
        if (teerajaytys)
        {
            RajaytaSprite(gameObject, rajaytysrows, rajaytyscols, rajaytysvoima, rajaytyskestoaika);
        }
        if (explosion!=null)
        {
            GameObject explosionIns = Instantiate(explosion, transform.position, Quaternion.identity);

        }
         

        // Destroy(explosionIns,1.0f);
        Destroy(gameObject);


        Vector3 v3 =
new Vector3(
rb.position.x, rb.position.y, 0);
        TeeBonus(bonus, v3, boxsize, bonusmaara);




    }

    public float ampumisenvoimakkuus = 6.0f;
    public float pieninmahdollinenAikaValiAmpumisissa = 2.0f;
    private float viimeksiAmmuttu = 0.0f;

    public bool ammu = false;

    public float ampumiskohdanmiinustus = 0.5f;

    public bool ampuukoVaikkaAluseinakyvissa = true;
    public void Ammu()
    {
        if (ammu && montakoonammuttu < ampumistenmaksimimaara)
        {
            float aikanyt = Time.time;
            if (aikanyt - viimeksiAmmuttu > pieninmahdollinenAikaValiAmpumisissa)
            {
                Vector3 ampumisenkohta =
                new Vector3(
         transform.position.x, transform.position.y - ampumiskohdanmiinustus, 0);


                if (!ampuukoVaikkaAluseinakyvissa && !VoikoVihollinenAmpua(ampumisenkohta))
                {
                    // viimeksiAmmuttu = Time.time;//jotta ei tuliteta koko aikaa
                    viimeksiAmmuttu = Time.time;
                    return;
                }



                Vector2 ve = palautaAmmuksellaVelocityVector(alusGameObject, ampumisenvoimakkuus, ampumisenkohta);


                //GameObject Instantiate(ammus);
                //box

                GameObject instanssi = Instantiate(ammus, ampumisenkohta, Quaternion.identity);

                List<GameObject> lista = new List<GameObject>();
                lista.Add(instanssi);
                lista.Add(gameObject);

                IgnoreCollisions(lista);



                /*
                Vector3 belowPosition = transform.position + transform.TransformDirection(Vector3.down * -1.0f);

                // Adjust the Z-position to match the projectile's layer or depth

                // Instantiate the projectile
                GameObject instanssi = Instantiate(
                    ammus,
                    belowPosition,
                    Quaternion.identity
                );
                */

                //   PalliController p = instanssi.GetComponent<PalliController>();
                //   p.alusGameObject = alusGameObject;

                instanssi.GetComponent<Rigidbody2D>().velocity = ve;
                viimeksiAmmuttu = Time.time;

                montakoonammuttu++;

            }
        }

    }

    /*
        public void OnCollisionEnter2D(Collision2D col)
        {
            if (onkoammus)
            {


            }
        }
        */
}
