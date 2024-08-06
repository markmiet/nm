using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalloVihollinenController : MonoBehaviour
{
    public GameObject alusGameObject;



    private SpriteRenderer m_SpriteRenderer;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
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

    private void FixedUpdate()
    {
        if (alusGameObject != null)
        {
            bool vasemmalle = false;

            float alusx = alusGameObject.transform.position.x;
            float x = transform.position.x;

            Debug.Log("eoruut=" + Mathf.Abs(previousx - x));
            bool pyori = false;

            if (Mathf.Abs(previousx - x)>0.0f)
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
                    if (pyori)
                    {

                        transform.Rotate(0, 0, 2.0f);
                    }

                }
                else
                {
                    uusix = uusix + 0.02f;
                    Quaternion targetRotation = Quaternion.Euler(transform.rotation.x + 10f, 0, 0);
                    //transform.rotation = targetRotation;
                    //     Debug.Log("oikealleeee");
                    // transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
                    if (pyori)
                    {

                        transform.Rotate(0, 0, -2.0f);
                    }
                }


                // transform.Rotate(0,0, rotationSpeed * Time.deltaTime);

                //transform.position.Set(uusix, transform.position.y - 1f, 0);

                transform.position = new Vector2(uusix, transform.position.y);


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


}
