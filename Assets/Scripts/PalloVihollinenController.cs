using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalloVihollinenController : BaseController
{
    public GameObject alusGameObject;

    //private Camera mainCamera;


    private SpriteRenderer m_SpriteRenderer;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        //   mainCamera = Camera.main;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();

    }

    public float rotationSpeed = 90f; // Degrees per second


    // Update is called once per frame
    void Update()

    {


    }

    public float torqueAmount = 99.0f; // The amount of torque to apply
    public Vector3 torqueDirectionUp = Vector3.up; // The axis around which to apply the torque (e.g., up for rotation around the Y-axis)
    public Vector3 torqueDirectionDown = Vector3.down; // The axis around which to apply the torque (e.g., up for rotation around the Y-axis)



    private Vector3 lastPosition;

    private float previousx = 0;
    private int laskuri = 0;

    private Vector3 ed = new Vector3(0, 0, 0);
    private void FixedUpdate()
    {
        if (alusGameObject != null)
        {
            bool vasemmalle = false;

            float alusx = alusGameObject.transform.position.x;
            float x = transform.position.x;
            if (alusx < x)
            {
                vasemmalle = true;

            }

            float ero = Mathf.Abs(alusx - x);

            if (ero > 1f)
            {

                if (vasemmalle)
                {
                    rb.velocity = new Vector2(-1f, rb.velocity.y);
                    //                    transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);


                    //rb.AddTorque(torqueDirectionUp * torqueAmount, ForceMode.Force);
                }
                else
                {

                   rb.velocity = new Vector2(1f, rb.velocity.y);
                   // transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);

                }
            }
            base.tallennaSijaintiSailytaVainNkplViimeisinta(2, false,false);

            float eroo = base.palautaEro(transform.position, 0, false);

          //  Debug.Log("eroo=" + eroo);

           // if (Mathf.Abs(eroo) >0.005f)
           // {
                transform.Rotate(0, 0, -eroo*150);
           // }
            

            /*
            if (base.onkoliikkunutVasemmalle(transform.position,0,false, 0.01f))
            {
                //tämä rotate vauhdin mukaan...
                transform.Rotate(0, 0, 5.0f);
                Debug.Log("rotate vasen");


            }
            else if (base.onkoliikkunutOikealle(transform.position, 0,false,0.01f))
            {
                transform.Rotate(0, 0, -5.0f);
                Debug.Log("rotate oikea");
            }
            else
            {
               // transform.Rotate(0, 0, 0);
                Debug.Log("ei rotate");
            }
            */
        }
    }

    public void Explode()
    {
        if (transform.parent != null)

        {
            GameObject parentObject = transform.parent.gameObject;
            Destroy(parentObject, 0.1f);
            //tänne vielä animaatio
            //Destroy(gameObject, 0.1f);

        }


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

    private bool pyorimisesto = false;

    public void estaPyoriminen(bool esta)
    {
        pyorimisesto = esta;
    }

    private bool liikkumisesto = false;

    public void setLiikkumisesto(bool p_liikkumisesto)
    {
        this.liikkumisesto = p_liikkumisesto;
    }

    void OnBecameInvisible()
    {
        //Debug.Log ("OnBecameInvisible");
        // Destroy the enemy
        //tuhoa = true;

        Destroy(gameObject);
    }
}
