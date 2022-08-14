using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class AlusController : MonoBehaviour
{

    public GameObject speedbonusbutton;
    public GameObject missilebonusbutton;



    public Joystick joystick;

    public GameObject explosion;

    private Rigidbody2D m_Rigidbody2D;

    //vauhti
    private Animator m_Animator;

    private bool oikeaNappiPainettu = false;

    private bool vasenNappiPainettu = false;

    private float vauhtiOikea = 0.0f;
    public float vauhtiOikeaMax = 4.0f;

    public float hidastuvuusKunMitaanEiPainettu = 0.3f;
    private float nopeudenMuutosKunPainettu = 1f;
    //ylos/alla

    private bool ylosNappiPainettu = false;
    private bool alasNappiPainettu = false;


    private float vauhtiYlos = 0.0f;
    public float vauhtiYlosMax = 4.0f;

    private bool spaceNappiaPainettu = false;
    //private bool spaceNappiAlhaalla = false;
    //private bool spaceNappiYlhaalla = false;



    private bool ammusInstantioitiinviimekerralla = false;

    public GameObject ammusPrefab;

    public GameObject bulletPrefab;

    public GameObject gameoverPrefab;


    private SpriteRenderer m_SpriteRenderer;




    private Vector2 screenBounds;

    private float objectWidth;
    private float objectHeight;

    private bool gameover = false;


    private GameObject instanssiBullet;
    private GameObject instanssiBulletYlos;


    private int missileDownCollected = 0;
    //

    public float vauhdinLisaysKunSpeedbonusOtettu = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        //        Debug.Log("screenBounds=" + screenBounds);

        Debug.Log("ennen findia");
        if (Application.platform != RuntimePlatform.Android)
        {
            GameObject[] oo = GameObject.FindGameObjectsWithTag("painike");
            foreach (GameObject o in oo)
            {
                o.SetActive(false);
            }
        }



        BonusButtonController[] bs = (BonusButtonController[])FindObjectsOfType(typeof(BonusButtonController));

        foreach (BonusButtonController btc in bs)
        {
            // do whatever with each 'enemy' here
            // Debug.Log(""+btc.order+ " btc.selected=" +btc.selected.ToString() + " btc.used= " + btc.used.ToString);


            Debug.Log("btc.order=" + btc.order + " btc.selected=" + btc.selected + " btc.usedcount=" + btc.usedcount);
            bbc.Add(btc);

        }
        // words.Sort((a, b) => a.Length.CompareTo(b.Length));

        bbc.Sort((a, b) => a.order.CompareTo(b.order));


    }

    List<BonusButtonController> bbc = new List<BonusButtonController>();

    // Update is called once per frame
    void Update()
    {

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        float nappiHorizontal = Input.GetAxisRaw("Horizontal");

        oikeaNappiPainettu = (nappiHorizontal > 0.0f); // > 0 for right, < 0 for left
        vasenNappiPainettu = (nappiHorizontal < 0.0f);

        if (!oikeaNappiPainettu)
            oikeaNappiPainettu = (joystick.Horizontal > 0.1f);


        if (!vasenNappiPainettu)
            vasenNappiPainettu = (joystick.Horizontal < -0.1f);



        float nappiVertical = Input.GetAxisRaw("Vertical");


        ylosNappiPainettu = nappiVertical > 0; // > 0 for right, < 0 for left
        alasNappiPainettu = nappiVertical < 0;


        if (!ylosNappiPainettu)
            ylosNappiPainettu = (joystick.Vertical > 0.1f);

        if (!alasNappiPainettu)
            alasNappiPainettu = (joystick.Vertical < -0.1f);

        //spaceNappiaPainettu = Input.GetButton ("Jump");
        //spaceNappiAlhaalla = Input.GetButtonDown ("Jump");
        //spaceNappiYlhaalla = Input.GetButtonUp ("Jump");

        //spaceNappiaPainettu = Input.GetKeyDown (KeyCode.Space);
        if (Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            //Shoot ();
            //		Debug.Log ("space painettu " );
            spaceNappiaPainettu = true;

        }
        else
        {
            //		Debug.Log ("space ei painettu ");
            spaceNappiaPainettu = false;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            BonusButtonPressed();
        }


    }

    void FixedUpdate()
    {

        if (gameover)
        {
            GameObject instanssi = Instantiate(gameoverPrefab, new Vector3(10.1f +
+(m_SpriteRenderer.bounds.size.x / 2), 10, 0), Quaternion.identity);
            //  instanssi.GetComponent<Rigidbody2D>().velocity = new Vector2(20, 0);

            Destroy(instanssi, 1);
        }


        if (m_Animator.GetBool("explode"))
        {
            //    if (!gameover)
            //    {
            //        gameover = true;
            //        for (int i = 0; i < 100; i++)
            //        {
            //            GameObject instanssi = Instantiate(gameoverPrefab, new Vector3(0.1f +
            //m_Rigidbody2D.position.x + (m_SpriteRenderer.bounds.size.x / 2), m_Rigidbody2D.position.y, 0), Quaternion.identity);
            //        // instanssi.GetComponent<Rigidbody2D>().velocity = new Vector2(20, 0);

            //    }


            //}

            return;
        }




        if (oikeaNappiPainettu)
        {
            vauhtiOikea = vauhtiOikea + nopeudenMuutosKunPainettu;
        }
        if (vasenNappiPainettu)
        {
            //vasen painettu
            vauhtiOikea = vauhtiOikea - nopeudenMuutosKunPainettu;
        }
        if (!oikeaNappiPainettu && !vasenNappiPainettu)
        {
            if (vauhtiOikea > 0.0f)
            {
                vauhtiOikea = vauhtiOikea - hidastuvuusKunMitaanEiPainettu;
                if (vauhtiOikea < 0.0f)
                {
                    vauhtiOikea = 0.0f;
                }
            }
            else if (vauhtiOikea < 0.0f)
            {
                vauhtiOikea = vauhtiOikea + hidastuvuusKunMitaanEiPainettu;
                if (vauhtiOikea > 0.0f)
                {
                    vauhtiOikea = 0.0f;
                }
            }
        }



        if (vauhtiOikea > 0 && vauhtiOikea > vauhtiOikeaMax)
        {
            vauhtiOikea = vauhtiOikeaMax;
        }
        else if (vauhtiOikea < 0 && vauhtiOikea <= -(vauhtiOikeaMax))
        {
            vauhtiOikea = -(vauhtiOikeaMax);
        }




        //alas/ylös
        if (ylosNappiPainettu)
        {
            vauhtiYlos = vauhtiYlos + nopeudenMuutosKunPainettu;
        }
        if (alasNappiPainettu)
        {
            //vasen painettu
            vauhtiYlos = vauhtiYlos - nopeudenMuutosKunPainettu;
        }



        if (!ylosNappiPainettu && !alasNappiPainettu)
        {
            if (vauhtiYlos > 0.0f)
            {
                vauhtiYlos = vauhtiYlos - hidastuvuusKunMitaanEiPainettu;
                if (vauhtiYlos < 0.0f)
                {
                    vauhtiYlos = 0.0f;
                }
            }
            else if (vauhtiYlos < 0.0f)
            {
                vauhtiYlos = vauhtiYlos + hidastuvuusKunMitaanEiPainettu;
                if (vauhtiYlos > 0.0f)
                {
                    vauhtiYlos = 0.0f;
                }
            }
        }



        if (vauhtiYlos > 0 && vauhtiYlos > vauhtiYlosMax)
        {
            vauhtiYlos = vauhtiYlosMax;
        }
        else if (vauhtiYlos < 0 && vauhtiYlos <= -(vauhtiYlosMax))
        {
            vauhtiYlos = -(vauhtiYlosMax);
        }
        //float perusliike = 5;

        m_Rigidbody2D.velocity = new Vector2(vauhtiOikea, vauhtiYlos);

        //m_Rigidbody2D.position.x = 3f;


        //        m_Rigidbody2D
        if (vauhtiYlos > 0.0f)
        {
            m_Animator.SetBool("up", true);

        }
        else
        {
            m_Animator.SetBool("up", false);
        }

        //Debug.Log("vauhtiOikea=" + vauhtiOikea);
        //Debug.Log("vauhtiYlos=" + vauhtiYlos);
        //		Debug.Log ("spaceNappiYlhaalla=" + spaceNappiYlhaalla);

        //m_Rigidbody2D.position.
        bool ammusinstantioitiin = false;
        //tee vaan rämpytyksestä parempi...
        //tai sitten rajoitettu määrä ammuksia...

        if (spaceNappiaPainettu && !OnkoSeinaOikealla())
        {
            //	if (!ammusInstantioitiinviimekerralla) {
            Vector3 v3 =
            new Vector3(0.1f +
            m_Rigidbody2D.position.x + (m_SpriteRenderer.bounds.size.x / 2), m_Rigidbody2D.position.y, 0);




            GameObject instanssi = Instantiate(ammusPrefab, v3, Quaternion.identity);
            instanssi.GetComponent<Rigidbody2D>().velocity = new Vector2(20, 0);
            ammusinstantioitiin = true;


            //alas tippuva
            // missileDownCollected
            if (missileDownCollected >= 1 && instanssiBullet == null)
            {


                Vector3 v3alas =
new Vector3(0.1f +
m_Rigidbody2D.position.x + (m_SpriteRenderer.bounds.size.x / 2), m_Rigidbody2D.position.y -0.1f, 0);



                instanssiBullet = Instantiate(bulletPrefab, v3alas, Quaternion.identity);
                instanssiBullet.SendMessage("Alas", true);
                instanssiBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0.1f, -2);


                instanssiBullet.GetComponent<Rigidbody2D>().gravityScale = 1.0f;

            }
            /*
  if (missileDownCollected == 2 && instanssiBulletToinen == null)
  {
      instanssiBulletToinen = Instantiate(bulletPrefab, v3, Quaternion.identity);
      instanssiBulletToinen.SendMessage("Alas", true);
      instanssiBulletToinen.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -4);


      instanssiBulletToinen.GetComponent<Rigidbody2D>().gravityScale = 2.0f;

  }

              */


            if (missileDownCollected == 2 && instanssiBulletYlos == null)
            {


                Vector3 v3ylos =
    new Vector3(0.1f +
    m_Rigidbody2D.position.x + (m_SpriteRenderer.bounds.size.x / 2), m_Rigidbody2D.position.y+0.1f, 0);



                instanssiBulletYlos = Instantiate(bulletPrefab, v3ylos, Quaternion.identity);
                instanssiBulletYlos.SendMessage("Alas", false);
                instanssiBulletYlos.GetComponent<Rigidbody2D>().velocity = new Vector2(0.1f, 2);

                instanssiBulletYlos.GetComponent<Rigidbody2D>().gravityScale = -1.0f;

            }



            //	}
        }
        ammusInstantioitiinviimekerralla = ammusinstantioitiin;




    }
    void LateUpdate()
    {
        Vector3 viewpos = transform.position;


        viewpos.x =
            Mathf.Clamp(viewpos.x, screenBounds.x * -1 + objectWidth, screenBounds.x + objectWidth);


        //viewpos.x =
        //    Mathf.Clamp(viewpos.x, screenBounds.x * -1, screenBounds.x);






        //     viewpos.x =
        //   Mathf.Clamp(viewpos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);

        //Debug.Log(" viewpos.x=" + viewpos.x + " screenBounds.x=" + screenBounds.x + " objectWidth=" + objectWidth + " m_SpriteRenderer.transform.position.x=" +
        //m_SpriteRenderer.transform.position.x

        //       );
        /*
        if (viewpos.x<0)
        {
            viewpos.x = 0;
        }
        */


        //viewpos.y = Mathf.Clamp(viewpos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
        viewpos.y = Mathf.Clamp(viewpos.y, 5.0f + objectHeight, screenBounds.y - objectHeight);

        transform.position = viewpos;

        /*
        if (transform.position.x<0.0f)
        {
            transform.position.x = 0.0f;
        }
*/
    }



    public void Explode()
    {
        GameObject explosionIns = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(explosionIns, 1.0f);
        Destroy(gameObject, 0.1f);
        gameover = true;

    }



    public float rotationSpeed = 50.0f;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.75f, 0.0f, 0.0f, 0.75f);

        // Convert the local coordinate values into world
        // coordinates for the matrix transformation.
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }

    private bool OnkoSeinaOikealla()
    {

        Vector3 v = transform.position;

        Collider2D[] cs =
        Physics2D.OverlapBoxAll(new Vector2(v.x

            , v.y + 0.1f),
        new Vector2(1f, 0.1f), 0

            );

        if (cs != null && cs.Length > 0)
        {
            foreach (Collider2D c in cs)
            {
                if (c.gameObject == this.gameObject)
                {

                }
                else if (c.gameObject.tag == "tiilitag")
                {
                    return true;
                }
            }
        }
        return false;

    }





    /*
     * //If the Fire1 button is pressed, a projectile
    //will be Instantiated every 0.5 seconds.

    using UnityEngine;
    using System.Collections;

    public class Example : MonoBehaviour
    {
        public GameObject projectile;
        public float fireRate = 0.5f;
        private float nextFire = 0.0f;

        void Update()
        {
            if (Input.GetButton("Fire1") && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Instantiate(projectile, transform.position, transform.rotation);
            }
        }
    }
        */

    bool collision = false;


    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("on OnCollisionEnter2D ");
        collision = false;


        if (col.collider.tag == "tiilitag")
        {

            Explode();

            //Destroy (col.gameObject);

        }


    }


    void OnCollisionStay2D(Collision2D col)
    {
        //   Debug.Log("on OnCollisionStay ");
        collision = false;
    }


    void OnCollisionExit2D(Collision2D col)
    {
        Debug.Log("on collision exit");

        collision = false;
    }



    //void OnDrawGizmos()
    //{
    //    // Draws a 5 unit long red line in front of the object
    //    Gizmos.color = Color.red;
    //    Vector3 direction = transform.TransformDirection(Vector3.forward) * 50;
    //    Gizmos.DrawRay(transform.position, direction);
    //}



    //void OnDrawGizmosSelected()
    //{
    //    // Draws a 5 unit long red line in front of the object
    //    Gizmos.color = Color.red;
    //    Vector3 direction = transform.TransformDirection(Vector3.forward) * 50;
    //    Gizmos.DrawRay(transform.position, direction);
    //}
    //se raycasti tee sillä
    //tai sitten ammuskärki oma gameobjecti joka on se neliö



    public void BonusCollected()
    {


        //olemme koskeneet bonukseen

        //    public BonusButtonController.Bonusbuttontype speedbonusbutton;
        //    public BonusButtonController.Bonusbuttontype missilebonusbutton;

        //bool valittu=  speedbonusbutton.
        //     speedbonusbutton


        // BonusButtonController[] bs = (BonusButtonController[])FindObjectsOfType(typeof(BonusButtonController));
        /*      */


        //siinä on nyt järjestyksessä
        int selectedIndex = -1;
        foreach (BonusButtonController btc in bbc)
        {
            // do whatever with each 'enemy' here
            // Debug.Log(""+btc.order+ " btc.selected=" +btc.selected.ToString() + " btc.used= " + btc.used.ToString);


            Debug.Log("btc.order=" + btc.order + " btc.selected=" + btc.selected + " btc.usedcount=" + btc.usedcount);
            if (btc.selected)
            {
                selectedIndex = btc.order;
            }

        }
        if (selectedIndex < 0)
        {
            bbc[0].selected = true;
        }
        else
        {
            bbc[selectedIndex].selected = false;

            if (selectedIndex + 1 >= bbc.Count)
            {
                bbc[0].selected = true;
            }
            else
            {
                bbc[selectedIndex + 1].selected = true;
            }
        }

    }


    public void BonusButtonPressed()
    {
        foreach (BonusButtonController btc in bbc)
        {
            if (btc.selected && btc.usedcount <= btc.maxusedcount)
            {
                btc.usedcount = btc.usedcount + 1;
                btc.selected = false;
                if (btc.bonusbuttontype.Equals(BonusButtonController.Bonusbuttontype.Speed))
                {
                    Debug.Log("speed()");
                    vauhtiOikeaMax += vauhdinLisaysKunSpeedbonusOtettu;
                    vauhtiYlosMax += vauhdinLisaysKunSpeedbonusOtettu;

                }
                else if (btc.bonusbuttontype.Equals(BonusButtonController.Bonusbuttontype.Missile))
                {
                    Debug.Log("missile()");
                    missileDownCollected++;
                }
            }

        }
    }



}
