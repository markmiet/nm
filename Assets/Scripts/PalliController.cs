using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalliController : BaseController
{
    public Vector2 boxsizeylhaalla = new Vector2(2.0f, 2.0f);  // Size of the box
    public Vector2 boxsizekeskella = new Vector2(2.0f, 0.1f);  // Size of the box
    public Vector2 boxsizelaidatalhaalla = new Vector2(2.0f, 0.1f);  // Size of the box
    public Vector2 boxsizealhaalla = new Vector2(2.0f, 0.1f);  // Size of the box


    public Vector2 vasenylaboxcenter = Vector2.zero;
    public Vector2 oikeaylaboxcenter = Vector2.zero;

    public Vector2 vasenboxcenter = Vector2.zero;
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
    // Start is called before the first frame update
    private WheelJoint2D wheelJoint;

    public int osumiemaarajokaTarvitaanRajahdykseen = 5;
    private float nykyinenosuminenmaara = 0.0f;
    private Vector2 boxsize;// = new Vector2(0, 0);
    void Start()
    {
        //   mainCamera = Camera.main;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        wheelJoint = GetComponent<WheelJoint2D>();
        rb = GetComponent<Rigidbody2D>();

        if (boostParticles != null && boostParticles.isPlaying)
        {
            boostParticles.Stop();
        }

        boxsize = new Vector2(m_SpriteRenderer.size.x, m_SpriteRenderer.size.y);
        rb.simulated = false;
        GameObject[] allObstacles = GameObject.FindGameObjectsWithTag("alustag");
        foreach (GameObject obstacles in allObstacles)
        {
            alusGameObject = obstacles;
        }
    }

    //   public float rotationSpeed = 90f; // Degrees per second


    // Update is called once per frame
    void Update()

    {
        //nykyinenosuminenmaara;
        Color color = m_SpriteRenderer.color;
        color.a = PalautaFadeArvo();
        
        m_SpriteRenderer.color = color;

        float startAlpha = m_SpriteRenderer.color.a;
      //  Debug.Log("alpha=" + startAlpha);

    }

    private float PalautaFadeArvo()
    {
        //1.0 originelli

        //0.1 näkyy jotain
        float alkuper = 1.0f;
        float vah = alkuper - nykyinenosuminenmaara / osumiemaarajokaTarvitaanRajahdykseen;
        return vah + 0.1f;

    }

    private bool hyppymenossaTyyppi1 = false;
    private float hypynkestotyyppi1 = 0.0f;
    private float hypynkestominimiTyyppi1 = 0.1f;
    private bool onkohypynsuuntavasemmalleKyseHyppytyypista1 = false;


    private bool hyppymenossaTyyppi2 = false;
    private float hypynkestotyyppi2 = 0.0f;

    private float hypynkestominimiTyyppi2 = 0.1f;
    private bool onkohypynsuuntavasemmalleKyseHyppytyypista2 = false;

    public ParticleSystem boostParticles; // Particle system for the rocket flames


    public float hyppyjenValinenViive = 2.0f;


    private float viimeisenhypynaloitusajankohta = 0.0f;


    public float erominimissaanJottaLiikutaan = 0.4f;

    private bool OnkoOkLiikkua(float ero)
    {
        return ero > erominimissaanJottaLiikutaan &&  m_SpriteRenderer.isVisible;
    }

    private bool OnkoHyppytyyppi1Ohi()
    {
        // if (hypynAloitusAjankohta==0.0f)
        // {
        //     return true;
        // }
        bool tiilalla = OnkoTiiliPallonAlla();
        hypynkestotyyppi1 += Time.deltaTime;
        if (tiilalla && hypynkestotyyppi1 > hypynkestominimiTyyppi1)
        {
            //  hyppymenossaTyyppi1 = false;
            return true;
        }
        return false;
    }

    private bool OnkoHyppytyyppi2Ohi()
    {
        // if (hypynAloitusAjankohta==0.0f)
        // {
        //     return true;
        // }
        bool tiilalla = OnkoTiiliPallonAlla();
        hypynkestotyyppi2 += Time.deltaTime;
        if (tiilalla && hypynkestotyyppi2 > hypynkestominimiTyyppi2)
        {
            //hyppymenossaTyyppi2 = false;
            return true;
        }
        return false;
    }


    //int laskuri = 0;
    float deltojensumma = 0.0f;

    private bool OnkoOkToimia()
    {
        return m_SpriteRenderer.isVisible;
    }


    private void FixedUpdate()
    {

        if (alusGameObject != null)
        {


            if (!OnkoOkToimia())
            {
                return;
            }
            rb.simulated = true;


            // JointMotor2D motor = wheelJoint.motor;
            // motor.motorSpeed = -100f;  // Negative for clockwise rotation
            // wheelJoint.motor = motor;


            // wheelJoint.motor.motorSpeed = -100f;

            bool vasemmalle = false;

            float alusx = alusGameObject.transform.position.x;
            float x = transform.position.x;
            if (alusx < x)
            {
                vasemmalle = true;

            }

            float ero = Mathf.Abs(alusx - x);

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

            if (onkoTiiliPallonAlla)
            {
                hyppymenossaTyyppi2 = false;
                hyppymenossaTyyppi1 = false;
                if (boostParticles.isPlaying)
                {
                    boostParticles.Stop();
                }


            }


            if (OnkoOkLiikkua(ero)  && !hyppymenossaTyyppi2 && /*onkohyppyohi && onkohyppy2ohi &&*/ !onkomenossaYlospain && !hyppymenossaTyyppi1)
            {

                if (vasemmalle)
                {

                    // Debug.Log("vasemmalle=" + vasemmalle + " onkoTiiliVasemmalla=" + onkoTiiliVasemmalla + " onkoTiiliVasemmallaAlhaalla=" + onkoTiiliVasemmallaAlhaalla);
                    //    rb.velocity = new Vector2(-1f, rb.velocity.y);
                    //                    transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);


                    //rb.AddTorque(torqueDirectionUp * torqueAmount, ForceMode.Force);

                    if (!onkoTiiliVasemmalla && onkoTiiliVasemmallaAlhaalla && !onkoPallovasemmalla)
                    {
                        JointMotor2D motor = wheelJoint.motor;
                        motor.motorSpeed = -180;  // Negative for clockwise rotation

                        wheelJoint.motor = motor;
                    }
                    else
                    {
                        JointMotor2D motor = wheelJoint.motor;
                        motor.motorSpeed = 0.0f;//stopataan
                        wheelJoint.motor = motor;
                    }



                }
                else
                {

                    //  rb.velocity = new Vector2(1f, rb.velocity.y);
                    // transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);

                    //   Debug.Log("vasemmalle=" + vasemmalle + " onkoTiiliOikealla=" + onkoTiiliOikealla + " onkoTiiliOikeallaAlhaalla=" + onkoTiiliOikeallaAlhaalla);
                    if (!onkoTiiliOikealla && onkoTiiliOikeallaAlhaalla && !onkoPalloOikealla)
                    {
                        JointMotor2D motor = wheelJoint.motor;
                        motor.motorSpeed = 180;  // Negative for clockwise rotation
                        wheelJoint.motor = motor;
                    }
                    else
                    {
                        JointMotor2D motor = wheelJoint.motor;
                        motor.motorSpeed = 0.0f;//stopataan
                        wheelJoint.motor = motor;
                    }
                }
            }

            else
            {

                JointMotor2D motor = wheelJoint.motor;
                motor.motorSpeed = 0.0f;//stopataan
                wheelJoint.motor = motor;

            }
            if (onkomenossaYlospain && hyppymenossaTyyppi2)
            {
                //
                if (!onkohypynsuuntavasemmalleKyseHyppytyypista2)
                {
                    if (onkoTiiliOikeallaLahellaTutkitaanHypynAikanaVoidaankoSinnePainSiirtya)
                    {
                        //eikun ylöspäin
                        //   float nykyinenylos = rb.velocity.y;
                        //   rb.velocity = new Vector2(rb.velocity.x/2.0f, nykyinenylos*4.0f);
                        hyppymenossaTyyppi2 = false;
                        LoikkaaYlos(true);
                        rb.velocity = new Vector2(0.0f, rb.velocity.y);
                        boostParticles.Play();

                    }
                }
                else
                {
                    //VASEN
                    if (onkoTiiliVasemmallaLahellaTutkitaanHypynAikanaVoidaankoSinnePainSiirtya)
                    {
                        //eikun ylöspäin
                        //   float nykyinenylos = rb.velocity.y;
                        //   rb.velocity = new Vector2(rb.velocity.x/2.0f, nykyinenylos*4.0f);
                        hyppymenossaTyyppi2 = false;
                        LoikkaaYlos(false);
                        rb.velocity = new Vector2(0.0f, rb.velocity.y);
                        boostParticles.Play();



                    }
                }

            }
            if (OnkoMenossaylospain() && !hyppymenossaTyyppi2 /*&& !onkohyppyohi && onkohyppy2ohi*/)
            {

                if (!onkohypynsuuntavasemmalleKyseHyppytyypista1 && !onkoTiiliOikeallaLahellaTutkitaanHypynAikanaVoidaankoSinnePainSiirtya)
                {
                    rb.velocity = new Vector2(moveSpeed, rb.velocity.y);//oikealle
                                                                        //hyppy1
                                                                        //oikea
                                                                        // if (onkoTiiliOikealla)
                                                                        // {
                                                                        //     LoikkaaYlos();
                                                                        // }
                }
                else if (onkohypynsuuntavasemmalleKyseHyppytyypista1 && !OnkoTiiliVasemmallaLahellaTutkitaanHypynAikanaVoidaankoSinnePainSiirtya())
                {
                    rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);//vasen
                                                                         //hyppy1
                                                                         //oikea
                                                                         // if (onkoTiiliOikealla)
                                                                         // {
                                                                         //     LoikkaaYlos();
                                                                         // }
                }
            }
            //hyppäykset tähän
            else if (onkoTiiliPallonAlla)
            {
                if (!vasemmalle)
                {
                    //hyppy1
                    //oikea


                    //onko tiilettämän kohdan vieressä oikealla tiiliseinä?
                    //if (!onkoTiiliOikeallaAlhaalla)
                    //{

                    //}
                    //else 
                    if (onkoTiiliOikealla && !onkoTiiliOikeallaYlhaalla && !onkoPalloOikealla)
                    {
                        LoikkaaYlos(onkoTiiliOikealla);
                      //  Ammu();
                    }
                    //hyppy2
                    else if (!onkoTiiliOikeallaAlhaalla && !onkoTiiliOikeallaYlhaalla && !onkoPalloOikealla)
                    {
                        //!onkoTiiliOikeallaAlhaalla=reikä lattiassa
                        //    Debug.Log("loikkaa aukon yli oikealle");
                        LoikkaaAukonYli(!vasemmalle);
                        //Ammu();
                    }

                }
                else
                {
                    //hyppy1
                    //vasen
                    if (onkoTiiliVasemmalla && !onkoTiiliVasemmallaYlhaalla && !onkoPallovasemmalla)
                    {
                        LoikkaaYlos(!onkoTiiliVasemmalla);
                    }
                    //hyppy2
                    else if (!onkoTiiliVasemmallaAlhaalla && !onkoTiiliVasemmallaYlhaalla && !onkoPallovasemmalla)
                    {
                        //!onkoTiiliOikeallaAlhaalla=reikä lattiassa
                        //      Debug.Log("loikkaa aukon yli vasemmalle");
                        LoikkaaAukonYli(!vasemmalle);
                       // Ammu();

                    }
                }
            }
            bool alasmenossa = OnkoMenossaAlasPainKovaaVauhtia();
            if (alasmenossa)
            {
                bool oikealla =
                OnkoTiiliOikeallaLahellaTutkitaanHypynAikanaVoidaankoSinnePainSiirtya();
                bool vasen = OnkoTiiliVasemmallaLahellaTutkitaanHypynAikanaVoidaankoSinnePainSiirtya();

                if (!oikealla && !vasen)
                {
               //     Ammu();
                }
            }

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

    public float jumpForce = 5f;  // Vertical force applied to jump
    public float moveSpeed = 2f;  // Horizontal movement speed

    Vector2 aloituspiste = Vector2.zero;
    private void LoikkaaYlos(bool tiilionoikealla)
    {

        if(Time.realtimeSinceStartup - hyppyjenValinenViive > viimeisenhypynaloitusajankohta)
        {
            for (float yx = 0; yx < hypppaaylostutkinnanmaara; yx += 0.01f)
            {
                if (tiilionoikealla)
                {
                    aloituspiste = new Vector2(oikeaboxcenter.x, oikeaboxcenter.y + yx);
                    onkohypynsuuntavasemmalleKyseHyppytyypista1 = false;


                }
                else
                {
                    aloituspiste = new Vector2(vasenboxcenter.x, vasenboxcenter.y + yx);
                    onkohypynsuuntavasemmalleKyseHyppytyypista1 = true;

                }

                bool onkotiiliatuossakohti = onkoTagiaBoxissa(tagilistaJoitaTutkitaan, boxsizekeskella, aloituspiste, layerMask);
                if (!onkotiiliatuossakohti)
                {
                    float voima = jumpForce;
                    float lisays = 0.0f;
                    voima = 2.34f * yx + 4.5f;

                    //Debug.Log("voima=" + voima + " yx=" + yx);


                    rb.velocity = new Vector2(rb.velocity.x, voima);
                    hyppymenossaTyyppi1 = true;
                    hypynkestotyyppi1 = 0.0f;
                    viimeisenhypynaloitusajankohta = Time.realtimeSinceStartup;
                    boostParticles.Play();
                  // Ammu();

                    break;
                }
            }
        }

    }


    private void LoikkaaAukonYli(bool aukkoOnOikealla)
    {
 
        if (Time.realtimeSinceStartup- hyppyjenValinenViive> viimeisenhypynaloitusajankohta )
        {


            for (float yx = 0; yx < hypppaaylostutkinnanmaara; yx += 0.01f)
            {
                if (aukkoOnOikealla)
                {
                    //eli siirretään alalaatikkoa oikealle niin paljon, että on tiilittömässä kohdassa
                    aloituspiste = new Vector2(alaboxcenter.x + yx + 0.5f, alaboxcenter.y);
                    // onkohypynsuuntavasemmalleKyseHyppytyypista1 = false;

                    onkohypynsuuntavasemmalleKyseHyppytyypista2 = false;

                }
                else
                {
                    aloituspiste = new Vector2(alaboxcenter.x - yx - 0.5f, alaboxcenter.y);
                    //onkohypynsuuntavasemmalleKyseHyppytyypista1 = true;

                    onkohypynsuuntavasemmalleKyseHyppytyypista2 = true;
                }

                bool onkotiiliatuossakohti = onkoTagiaBoxissa(tagilistaJoitaTutkitaan, boxsizealhaalla, aloituspiste, layerMask);
                if (onkotiiliatuossakohti)
                {
                    //laskeutusmispaikka löytyi
                    float voima = jumpForce;
                    float lisays = 0.0f;
                    voima = 2.34f * yx + 4.4f;

                    //                Debug.Log("voima=" + voima + " yx=" + yx);

                    if (aukkoOnOikealla)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, voima);

                        rb.velocity = new Vector2(voima / xsuunnanjakomaara, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(rb.velocity.x, voima);

                        rb.velocity = new Vector2(-voima / xsuunnanjakomaara, rb.velocity.y);
                    }




                    hyppymenossaTyyppi2 = true;
                    hypynkestotyyppi2 = 0.0f;

                    viimeisenhypynaloitusajankohta = Time.realtimeSinceStartup;

                    boostParticles.Play();

                   // Ammu();
                    break;
                }
            }
        }
    }
    public float hypppaaylostutkinnanmaara=4.0f;
    public float xsuunnanjakomaara = 2.0f;

    public float delay = 0.3f;  // Delay in seconds

    private void LoikkaaVasemmalle()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);//oikealle
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);//ylös
    }

    IEnumerator ApplyVelocityWithDelay(float viive)
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(viive);

        // Apply the velocity to the Rigidbody2D
        // rb.velocity = velocity;
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);//oikealle
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
        if (aloituspiste != Vector2.zero)
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawWireCube((Vector2)transform.position + aloituspiste, boxsizekeskella);
            Gizmos.DrawWireCube((Vector2)transform.position + aloituspiste, boxsizealhaalla);




        }

    }
    bool tutkitaankorkeutta = false;


    // public Vector2 oikeaboxcenterJokakasvaa = Vector2.zero;


    private bool OnkoTiiliPallonAlla()
    {
        // return true;

        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissa(tagilistaJoitaTutkitaan, boxsizealhaalla, alaboxcenter, layerMask);
        return onkotiilioikeallaAlhaalla;

    }

    private bool OnkoTiiliVasemmallaAlhaalla()
    {


        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissa(tagilistaJoitaTutkitaan, boxsizelaidatalhaalla, vasenalaboxcenter, layerMask);
        return onkotiilioikeallaAlhaalla;

    }

    private bool OnkoTiiliOikeallaAlhaalla()
    {


        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissa(tagilistaJoitaTutkitaan, boxsizelaidatalhaalla, oikeaalaboxcenter, layerMask);
        return onkotiilioikeallaAlhaalla;

    }


    private bool OnkoTiiliOikealla()
    {


        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissa(tagilistaJoitaTutkitaan, boxsizekeskella, oikeaboxcenter, layerMask);
        return onkotiilioikeallaAlhaalla;

    }


    private bool OnkoMakitaOikealla()
    {


        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissa(tagilistaJoitaTutkitaan, boxsizekeskella, oikeaboxcenter, layerMask);
        return onkotiilioikeallaAlhaalla;

    }

    private bool OnkoTiiliVasemmalla()
    {


        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissa(tagilistaJoitaTutkitaan, boxsizekeskella, vasenboxcenter, layerMask);
        return onkotiilioikeallaAlhaalla;

    }

    private bool OnkoTiiliVasemmallaYlhaalla()
    {


        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissa(tagilistaJoitaTutkitaan, boxsizeylhaalla, vasenylaboxcenter, layerMask);
        return onkotiilioikeallaAlhaalla;

    }

    private bool OnkoTiiliOikeallaYlhaalla()
    {


        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissa(tagilistaJoitaTutkitaan, boxsizeylhaalla, oikeaylaboxcenter, layerMask);
        return onkotiilioikeallaAlhaalla;

    }
    private bool OnkoTiiliOikeallaLahellaTutkitaanHypynAikanaVoidaankoSinnePainSiirtya()
    {


        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissa(tagilistaJoitaTutkitaan, boxsizeyla2, oikeayla2center, layerMask);
        return onkotiilioikeallaAlhaalla;

    }

    private bool OnkoTiiliVasemmallaLahellaTutkitaanHypynAikanaVoidaankoSinnePainSiirtya()
    {


        bool onkotiilioikeallaAlhaalla = onkoTagiaBoxissa(tagilistaJoitaTutkitaan, boxsizeyla2, vasenyla2center, layerMask);
        return onkotiilioikeallaAlhaalla;

    }



    public LayerMask layerMask;


    void OnBecameInvisible()
    {
        if (true)
            return;
       if (!rb.IsAwake())
        {
            return;
        }
        //Debug.Log ("OnBecameInvisible");
        // Destroy the enemy
        //tuhoa = true;
        if  (transform==null )
        {
            Debug.Log("transformi null");
            return;
        }
        bool ollaankokameranoikealla = IsObjectRightOfCamera(Camera.main, transform);
        if (!ollaankokameranoikealla)
        {
            Destroy(gameObject);
        }
        //onko 
    }


    private void OnBecameVisible()
    {
        
    }

    bool IsObjectRightOfCamera(Camera cam, Transform objTransform)
    {
        if (objTransform==null)
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

       
        bool ret = onkoTagiaBoxissa(tagit, boxsizekeskella, vasenboxcenter, layerMask);
        return ret;
    }

    public bool OnkoPalloOikealla()
    {
        string[] tagit = new string[1];
        tagit[0] = "pallovihollinenexplodetag";


        bool ret = onkoTagiaBoxissa(tagit, boxsizekeskella, oikeaboxcenter, layerMask);
        return ret;
    }


    public bool onkoTagiaBoxissa(string[] tagit, Vector2 boxsize, Vector2 boxlocation, LayerMask layerMask)
    {
        //string aa = "pallovihollinenexplodetag";

        foreach (string name in tagit)
        {
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

    bool IsInView(Vector2 worldPosition)
    {
  
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(worldPosition);
        return viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
               viewportPoint.y >= 0 && viewportPoint.y <= 1;
    }



    public GameObject explosion;
    public void Explode()
    {
        nykyinenosuminenmaara += 1.0f;
        if (nykyinenosuminenmaara>=osumiemaarajokaTarvitaanRajahdykseen)
        {
            ExplodeOikeasti();
        }
    }


    public void ExplodeOikeasti()
    {

        GameObject explosionIns = Instantiate(explosion, transform.position, Quaternion.identity);
        RajaytaSprite(gameObject, 10, 10, 10.0f, 0.6f);
        Destroy(explosionIns);
        Destroy(gameObject);


        Vector3 v3 =
new Vector3(
rb.position.x, rb.position.y, 0);

        //  Instantiate(bonus, v3, Quaternion.identity)
        int bonusmaara = 3;
        
            TeeBonus(bonus, v3, boxsize, bonusmaara);

        


    }

    public float ampumisenvoimakkuus = 2.0f;
    public bool ammu = false;
    public void Ammu()
    {
        if (ammu)
        {
            Vector2 ve = palautaAmmuksellaVelocityVector(alusGameObject, ampumisenvoimakkuus);

            Instantiate(ammus);



            GameObject instanssi = Instantiate(ammus, new Vector3(
     transform.position.x, transform.position.y, 0), Quaternion.identity);

            PalliController p = instanssi.GetComponent<PalliController>();
            p.alusGameObject = alusGameObject;

            instanssi.GetComponent<Rigidbody2D>().velocity = ve;
        }

    }
}
