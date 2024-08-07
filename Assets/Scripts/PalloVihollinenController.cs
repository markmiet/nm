using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalloVihollinenController : MonoBehaviour
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

            // Debug.Log("eoruut=" + Mathf.Abs(previousx - x));

          //  Debug.Log(" rb.velocity.magnitude=" + rb.velocity.magnitude);
          //  Debug.Log(" OnkoSeinaOikealla()=" + OnkoSeinaOikealla());
            

            bool pyori = false;

            //if (Mathf.Abs(previousx - x)>0.0f && !OnkoSeinaOikealla())
            //{
                //pyori = true;
           // }

           // if (this.transform.hasChanged)
           // {
            //    print("Player is not moving");
            //    pyori = true;
            //}


            //  if (Mathf.Approximately(Vector3.Distance(ed, transform.position), 0))
            //  {
            //      pyori = true;
            //  }
            //  ed = transform.position;

            if (rb.velocity.magnitude>=0.0001f)
            {
                pyori = true;
            }

            previousx = x;

            
            //       Debug.Log("transform.localPosition.x=" + transform.localPosition.x);
            if (alusx < x)
            {
                vasemmalle = true;

            }

            float ero = Mathf.Abs(alusx - x);
            pyori = true;
            if (ero > 0.5f)
            {
                float uusix = x;

                float speed = rb.velocity.magnitude;
                //float erotus = x - previousx;

               // Debug.Log("Mathf.Abs(erotus)=" + Mathf.Abs(erotus));
                //if (Mathf.Abs(erotus)>0.01f)
                //{
                //    pyori = true;
                //}

                //        Time.timeScale = 1; // Ensure the time scale is set to normal

               // Debug.Log("pyori="+ pyori);
                //   Debug.Log("transform.rotation.x=" + transform.rotation.x);
                if (vasemmalle)
                {
                    uusix = uusix - 0.02f;
                    //   Debug.Log("vasemmalle");

                    Quaternion targetRotation = Quaternion.Euler(transform.rotation.x - 10f, 0, 0);
                    //transform.rotation = targetRotation;
                    //  transform.Rotate(0, 0,-rotationSpeed * Time.deltaTime);
                    if (pyori && !pyorimisesto)
                    {

                        transform.Rotate(0, 0, 2.0f);
                    }
                    //rb.velocity = new Vector2(-1f, rb.velocity.y);

                }
                else
                {
                    uusix = uusix + 0.02f;
                    Quaternion targetRotation = Quaternion.Euler(transform.rotation.x + 10f, 0, 0);
                    //transform.rotation = targetRotation;
                    //     Debug.Log("oikealleeee");
                    // transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
                    if (pyori && !pyorimisesto)
                    {

                        transform.Rotate(0, 0, -2.0f);
                    }
                  //  rb.velocity = new Vector2(1f, rb.velocity.y);

                }


                // transform.Rotate(0,0, rotationSpeed * Time.deltaTime);

                //transform.position.Set(uusix, transform.position.y - 1f, 0);

              

                transform.position = new Vector2(uusix, transform.position.y);
                //rb.position = new Vector2(uusix, rb.position.y);
                //rb.MovePosition(new Vector2(uusix, rb.position.y));
                



            }
            if (laskuri==100)
            {
                laskuri = 0;
              //  previousx = transform.position.x;
            }
      


            laskuri++;
        }

    }

    public void Explode()
    {
        //tänne vielä animaatio
        Destroy(gameObject, 0.1f);
    }

    private bool OnkoSeinaOikealla()
    {

        Vector3 v = transform.position;


        Collider2D[] cs =
        Physics2D.OverlapBoxAll(new Vector2(v.x

            , v.y +0.1f),
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
}
