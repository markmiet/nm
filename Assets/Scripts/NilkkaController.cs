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
        return maassa;
    }

    private bool maassa = false;
    public void OnCollisionEnter2D(Collision2D collision)
    {
  /*
        CircleCollider2D circleCollider = collision.collider.GetComponent<CircleCollider2D>();

        if (circleCollider != null)
        {
            Debug.Log("Collision with a CircleCollider2D!");
*/
            if (collision.collider.tag.Contains("tiili"))
            {
                maassa = true;
            }
            else
            {
                maassa = false;
            }
            /*
        }
        else
        {
            Debug.Log("Collision with a non-CircleCollider2D object.");
            maassa = false;
        }
        */
    }
}
