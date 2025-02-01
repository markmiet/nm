using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NilkkaController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool IsMaassa()
    {

        // Replace this with your grounded logic
        // return Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
        float raydist = 1.4f;

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, raydist);
        Debug.DrawRay(transform.position, Vector2.down * raydist, Color.green);
        // Check if a relevant obstacle was detected
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject != gameObject &&
            (hit.collider.tag.Contains("tiili")))
            {
                //   Debug.Log("Obstacle detected at: " + hit.collider.name);
                return true;
            }
        }
        return false;

    }

   // private bool maassa = false;
  //  public void OnCollisionEnter2D(Collision2D collision)
  //  {
  /*
        CircleCollider2D circleCollider = collision.collider.GetComponent<CircleCollider2D>();

        if (circleCollider != null)
        {
            Debug.Log("Collision with a CircleCollider2D!");
*/
       //     if (collision.collider.tag.Contains("tiili"))
       //     {
       //         maassa = true;
       //     }
      //      else
       //     {
     //           maassa = false;
   //         }
            /*
        }
        else
        {
            Debug.Log("Collision with a non-CircleCollider2D object.");
            maassa = false;
        }
        */
   // }
}
