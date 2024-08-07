using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PallovihollinenOikeaLaitaController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject pallero;
    private SpriteRenderer palleroSpriteRenderer;
    float leveys = 0.0f;
    PalloVihollinenController myScript;

   // BoxCollider2D palleroBoxCollider2D;
    void Start()
    {
        // palleroRigidbody2D = pallero.GetComponent<Rigidbody2D>();
        palleroSpriteRenderer = pallero.GetComponent<SpriteRenderer>();
        leveys = palleroSpriteRenderer.bounds.size.x;

        myScript = pallero.GetComponent<PalloVihollinenController>();


        //   palleroBoxCollider2D = pallero.GetComponent<BoxCollider2D>();
      //  BoxCollider2D sdds= GetComponent<BoxCollider2D>();
      //  sdds.offset
    }

    // Update is called once per frame
    void Update()
    {


    //    transform.position = new Vector2(pallero.transform.position.x + leveys + 0.1f, pallero.transform.position.y);

        transform.position = new Vector2(pallero.transform.position.x , pallero.transform.position.y);


    }


    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("pallovihollinenoikealaita on OnTriggerEnter2D ");


        if (col.CompareTag("tiilitag"))
        {
            Debug.Log("OnTriggerEnter2D estaPyoriminen(true) ");
            myScript.estaPyoriminen(true);
        }

    }
    void OnTriggerStay2D(Collider2D col)
    {
        //   Debug.Log("on OnCollisionStay ");
        if (col.CompareTag("tiilitag")) 
        {
            Debug.Log("OnTriggerStay2D estaPyoriminen(true) ");

            myScript.estaPyoriminen(true);
        }
    }


    void OnTriggerExit2D(Collider2D col)
    {
      //  Debug.Log("pallovihollinenoikealaita on OnTriggerExit2D ");


        if (col.CompareTag("tiilitag"))
        {
            Debug.Log("OnTriggerExit2D estaPyoriminen(false) ");
            myScript.estaPyoriminen(false);
        }

    }


    void OnDrawGizmos()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 100;
        Gizmos.DrawRay(transform.position, direction);
    }


    void OnDrawGizmosSelected()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 100;
        Gizmos.DrawRay(transform.position, direction);
    }

}