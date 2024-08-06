using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalloVihollinenController : MonoBehaviour
{
    public GameObject alusGameObject;



    private SpriteRenderer m_SpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();



    }

    public float rotationSpeed = 90f; // Degrees per second


    // Update is called once per frame
    void Update()

    {
        if (alusGameObject!=null)
        {
            bool vasemmalle = false;

            float alusx = alusGameObject.transform.position.x;
            float x = transform.position.x;


            if (alusx < x)
            {
                vasemmalle = true;

            }

            float ero = Mathf.Abs(alusx - x);

            if (ero > 1.0f)
            {
                float uusix = x;

                Debug.Log("transform.rotation.x=" + transform.rotation.x);
                if (vasemmalle)
                {
                    uusix = uusix - 0.01f;
                    //   Debug.Log("vasemmalle");

                    Quaternion targetRotation = Quaternion.Euler(transform.rotation.x - 10f, 0, 0);
                    //transform.rotation = targetRotation;
                }
                else
                {
                    uusix = uusix + 0.01f;
                    Quaternion targetRotation = Quaternion.Euler(transform.rotation.x + 10f, 0, 0);
                    //transform.rotation = targetRotation;
                    //     Debug.Log("oikealleeee");
                }
                transform.Rotate(0,0, rotationSpeed * Time.deltaTime);

                //transform.position.Set(uusix, transform.position.y - 1f, 0);

                transform.position = new Vector2(uusix, transform.position.y);
                
 
            }

        }


    }

    private void FixedUpdate()
    {

    }

    public void Explode()
    {
        //tänne vielä animaatio
        Destroy(gameObject, 0.1f);
    }


}
