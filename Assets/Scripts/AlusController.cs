using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class AlusController : MonoBehaviour
{
    public Joystick joystick;

    public GameObject explosion;

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

    public GameObject bulletPrefab;
    public GameObject gameoverPrefab;


    private SpriteRenderer m_SpriteRenderer;




    private Vector2 screenBounds;

    private float objectWidth;
    private float objectHeight;

    private bool gameover = false;


    private GameObject instanssiBullet;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        //        Debug.Log("screenBounds=" + screenBounds);
    }

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


    }

    void FixedUpdate()
    {


        if (m_Animator.GetBool("explode"))
        {
            if (!gameover)
            {
                gameover = true;
                for (int i = 0; i < 100; i++)
                {
                    GameObject instanssi = Instantiate(gameoverPrefab, new Vector3(0.1f +
        m_Rigidbody2D.position.x + (m_SpriteRenderer.bounds.size.x / 2), m_Rigidbody2D.position.y, 0), Quaternion.identity);
                    // instanssi.GetComponent<Rigidbody2D>().velocity = new Vector2(20, 0);

                }


            }

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

            if (instanssiBullet == null)
            {
                instanssiBullet = Instantiate(bulletPrefab, v3, Quaternion.identity);
                instanssiBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -2);

            }



            //	}
        }
        ammusInstantioitiinviimekerralla = ammusinstantioitiin;




    }
    void LateUpdate()
    {
        Vector3 viewpos = transform.position;


        viewpos.x =
            Mathf.Clamp(viewpos.x, screenBounds.x * -1 + objectWidth, screenBounds.x+ objectWidth);


        //viewpos.x =
        //    Mathf.Clamp(viewpos.x, screenBounds.x * -1, screenBounds.x);






        //     viewpos.x =
        //   Mathf.Clamp(viewpos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);

        Debug.Log(" viewpos.x=" + viewpos.x + " screenBounds.x=" + screenBounds.x + " objectWidth=" + objectWidth+ " m_SpriteRenderer.transform.position.x="+
        m_SpriteRenderer.transform.position.x

            );
        /*
        if (viewpos.x<0)
        {
            viewpos.x = 0;
        }
        */


        viewpos.y = Mathf.Clamp(viewpos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
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
        //v.x = v.x + 100.0f;





        //Collider[] hitColliders = Physics.OverlapSphere(v, 0.1f);
        //if (hitColliders != null && hitColliders.Length > 0)
        //{
        //    Debug.Log("seina oikealla");
        //    return true;
        //}
        //return false;

        // public static Collider2D OverlapBox(Vector2 point, Vector2 size, float angle);
        //  Physics2D.OverlapBoxAll

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

        //return Physics2D.OverlapPoint(new Vector2(v.x+ m_SpriteRenderer.bounds.size.x

        //, v.y));

    }

    public Vector3 m_DetectorOffset = Vector3.zero;
    public Vector3 m_DetectorSize = Vector3.zero;


    public bool CheckForCollisions()
    {
        Vector3 colliderPos = transform.TransformPoint(m_DetectorOffset);
        Collider[] colliders = Physics.OverlapBox(colliderPos, m_DetectorSize, transform.rotation);
        if (colliders.Length == 1)
        {
            // Ignore collision with itself
            if (colliders[0].gameObject == gameObject)
                return false;
            return true;
        }
        if (colliders.Length > 0)
            return true;
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
    }


    void OnCollisionStay2D(Collision2D col)
    {
        Debug.Log("on OnCollisionStay ");
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



}


