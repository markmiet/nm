using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKoiraController : MonoBehaviour
{
    //[SerializeField]
 //   public bool hyppaa = false;
 //   public bool laskeudu = false;

    // Start is called before the first frame update
    private Animator animator;
    private Rigidbody2D rb;
    void Start()

    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }


    public float forceAmount = 5f;       // Total force to apply



    void Update()
    {

        //if (hyppaa)
        //{
            //animator.SetBool("hyppaa", true);
            //ollaankohyppaamassa = true;
            // rigidbody2D.AddForce(new Vector2(0, 10));

            //rb.velocity = new Vector2(rb.velocity.x, 3);

            // elapsedTime = 0.0f;

            //elapsedTime += Time.deltaTime;
        //}
        /*
        if (hyppaa &&  elapsedTime > hypynloppu)
        {
            elapsedTime = 0.0f;
           // ollaankohyppaamassa = false;
            hyppaa = false;
            hyppyvoimaannettu = false;


        }
        */
        /*
            if (hyppaa && elapsedTime>=hypynalku && elapsedTime<=hypynloppu)
        {
            float deltaForce = (forceAmount / (hypynloppu- hypynalku)) * Time.deltaTime;

            // Apply force
            rb.AddForce(Vector2.up * deltaForce);

            // Track how much force has been applied and time elapsed
            forceApplied += deltaForce;

            //float remainingForce = forceAmount - forceApplied;
            //if (remainingForce > 0)
            //{
              //  rb.AddForce(Vector2.up * remainingForce);
            //}

        }
            */

        //if (hyppaa && !hyppyvoimaannettu && elapsedTime<=hypynalku)
        //{
        //    rb.AddForce(Vector2.up * forceAmount);
        //    hyppyvoimaannettu = true;
        //}

        bool maassa = IsMaassa();
        if (OnkoHyppaamassaLoppuvaiheissa() && rb.velocity.y < 0 && maassa)
        {
            Debug.Log("The Rigidbody2D is moving downward.");
            //hyppaa = false;
            //laskeudu = true;
            animator.SetBool("hyppaa", false);
            animator.SetBool("laskeudu", true);
        }
        animator.SetBool("maassa", maassa);

        //animator.SetBool("hyppaa", hyppaa);
        //animator.SetBool("laskeudu", laskeudu);

        if (OnkoHyppaamassa() && currentstate == CharacterState.Jumping3)
        {
        //    rb.AddForce(Vector2.up * forceAmount);
          //  rb.AddForce(Vector2.left * forceAmount);

        }


        //   if (currentstate == CharacterState.Jumping6)
        //   {
        //       hyppaa = false;
        //       laskeudu = true;
        //   }
    }

    private bool OnkoHyppaamassa()
    {
        if (currentstate== CharacterState.Jumping1 ||
            currentstate == CharacterState.Jumping2 ||
            currentstate == CharacterState.Jumping3 ||
            currentstate == CharacterState.Jumping4 ||
            currentstate == CharacterState.Jumping5 ||
            currentstate == CharacterState.Jumping6 ||
            currentstate == CharacterState.Jumping7)
        {
            return true;
        }
        return false;
    }
    private bool OnkoHyppaamassaLoppuvaiheissa()
    {
        //currentstate == CharacterState.Jumping5 ||

        if (
            currentstate == CharacterState.Jumping6 ||
            currentstate == CharacterState.Jumping7)
        {
            return true;
        }
        return false;
    }


    public CharacterState currentstate;

    //public bool tuotahyppyvoimaa = false;

    public enum CharacterState
    {
        Konttaus0, //0
        Jumping1, //1
        Jumping2,//2
        Jumping3,//3
        Jumping4,//hyppyvoima 4
        Jumping5,//hyppyvoima5 
        Jumping6, //6
        Jumping7,//7
        Laskeudu8,//9
        Laskeudu9,//10
        Laskeudu10,//11
        Laskeudu11,//12
        Laskeudu12,//13
        Laskeudu13,//14

    }

    public bool IsMaassa()
    {
        NilkkaController[] nc =
        GetComponentsInChildren<NilkkaController>();

        if (nc!=null)
        {
            foreach (NilkkaController n in nc)
            {
                if (n.IsMaassa())
                {
                    return true;
                }
            }
        }
        return false;

    }


    /*
    private bool maassa = false;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        CircleCollider2D circleCollider = collision.collider.GetComponent<CircleCollider2D>();

        if (circleCollider != null)
        {
            Debug.Log("Collision with a CircleCollider2D!");

            if (collision.collider.tag.Contains("tiili"))
            {
                maassa = true;
            }
            else
            {
                maassa = false;
            }
        }
        else
        {
            Debug.Log("Collision with a non-CircleCollider2D object.");
            maassa = false;
        }
    }
    */
}
