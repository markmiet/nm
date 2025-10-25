using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 1.k‰velee normaalisti k‰det heiluu, jos ei n‰e alusta. jos tulee sein‰‰n vastaa pys‰htyy tai hypp‰‰ jos voi hyp‰t‰.
 * 2.paikallaan, mutta t‰ht‰ilee, jos n‰kee aluksen. jos on reunalla josta ei voi hyp‰t‰ alas.
 * 3.k‰velee, mutta t‰ht‰lee samalle, jos n‰kee aluksen oma animaatio
 * -vaihtaa toisen 
 * 4.katsoo vasemmalle, mutta k‰‰ntyy katsomaan suoraan pelaajaa silmiin(=vaihtuu sprite)
 * t‰m‰ jos alus on jonkun offsetin sis‰ll‰.
 * 5.katsoo pelaajaa suoraan silmiin, mutta k‰‰ntyy jonnekin p‰in alus siirtyy offsetist‰ pois
 * 6. k‰velee mutta tulee pudotus. joko a pys‰htyy siihen tai sitten tippuu alustalle. eli tutkinta jatkaako vai ei jatka.
 * 
 * */


public class SkelePienempiController : BaseController
{
    public State currentState = State.Centre;

    public enum State
    {
        WalkingFacingLeft,
        WalkingFacingRight,
        MovingFromFacingLeftToCentre,
        MovingFromFacingRightToCentre,
        Centre
    }

    public GameObject maapaikantutkintakohta; // the ground check point
    public Vector2 size = Vector2.one;        // box size for overlap area
    private Animator animator;
    private Rigidbody2D torsoRigidbody2D;

    public Vector2 kavelyvoima = Vector2.zero;
    public ForceMode2D forceMode2D = ForceMode2D.Impulse;
    public float hypyngravityscale = 0.4f;

    public Vector2 hyppyvoima = Vector2.zero;
    public ForceMode2D hypppyforceMode2D = ForceMode2D.Impulse;
    private bool hyppyvoimaannettu = false;

    public GameObject vasenjalkakohta;
    public Vector2 vasenjalkakohtasize = Vector2.one;


    public GameObject oikeajalkakohta;
    public Vector2 oikeajalkakohtasize = Vector2.one;

    private HandIKFollower handIKFollower;

    public GameObject tippumismaakohta;
    public Vector2 tippumismaakohtasize = Vector2.one;
    public GameObject seinaoikeallakohta;
    public Vector2 seinaoikeallakohtasize = Vector2.one;


    public GameObject oikeallaylhaallakohta;
    public Vector2 oikeallaylhaallakohtasize = Vector2.one;


    public Vector2 hyppyvoimaoikealleylos = Vector2.zero;
    public ForceMode2D hypppyoikealleylosforceMode2D = ForceMode2D.Impulse;


    private bool oikealleyloshyppyvoimaannettu = false;

    private StateChanger stateChanger;
    private DirectionSpriteSwitcher directionSpriteSwitcher;
    void Start()
    {
        animator =
        GetComponent<Animator>();

        torsoRigidbody2D =
        GetComponentInChildren<Rigidbody2D>();

        handIKFollower =
        GetComponentInChildren<HandIKFollower>();


        stateChanger = GetComponentInParent<StateChanger>();

        directionSpriteSwitcher = GetComponentInParent<DirectionSpriteSwitcher>();
    }
    public float changedelay = 1.0f;
    private float changedelaylaskuri = 0.0f;

    private void Update()
    {
        changedelaylaskuri += Time.deltaTime;


        bool seinaoikealla = onkoSeinaoikealla();
        if (seinaoikealla)
        {
            /*
            if (directionSpriteSwitcher.currentState== DirectionSpriteSwitcher.State.WalkRight)
            {

                stateChanger.ChangeState(DirectionSpriteSwitcher.State.WalkLeft);

            }
            else if (directionSpriteSwitcher.currentState == DirectionSpriteSwitcher.State.WalkLeft)
            {

                stateChanger.ChangeState(DirectionSpriteSwitcher.State.WalkRight);

            }
            */
            if (changedelaylaskuri> changedelay)
            {
                stateChanger.ChangeState(DirectionSpriteSwitcher.State.IdleCenter);
                changedelaylaskuri = 0.0f;
            }
           
        }
    }


    void Update2222()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (true)
        {
            return;
        }

        if (onkoSeinaoikealla())
        {
            if (!animator.GetBool("tippuukovemmin") && !animator.GetBool("tippuu"))
            {
                animator.SetBool("kavely", false);
                animator.SetBool("kavelylahto", false);

                animator.SetBool("paikallaan", true);//ki‰nny :)
                //animator.SetBool("kaanny", true);//ki‰nny :)

            }
            bool kavely = animator.GetBool("kavely");
            bool paikallaan = animator.GetBool("paikallaan");
            bool onkooikeallaylhaallatilaa = onkoOikeallaYlhaallatilaa();
            if (!oikealleyloshyppyvoimaannettu && onkooikeallaylhaallatilaa && (
                stateInfo.IsName("skelepienempipaikallaan") || stateInfo.IsName("skelepienempikavely")) && OnkoMaataVasemmanjalanAlla()

                && OnkoMaataOikeanjalanAlla() )
            {
                animator.SetBool("hyppaaoikealleylos", true);

                //                 public Vector2 hyppyvoimaoikealleylos = Vector2.zero;
                //   public ForceMode2D hypppyoikealleylosforceMode2D = ForceMode2D.Impulse;

                torsoRigidbody2D.AddForce(hyppyvoimaoikealleylos, hypppyoikealleylosforceMode2D);
                torsoRigidbody2D.gravityScale =hypyngravityscale;
                oikealleyloshyppyvoimaannettu = true;
                return;
            }

        }

        if (!OnkoMaataEdessa() && animator.GetBool("kavely") && OnkoMaataVasemmanjalanAlla()   && !onkoSeinaoikealla())
        {
            if (!hyppyvoimaannettu)
            {
                torsoRigidbody2D.AddForce(hyppyvoima, hypppyforceMode2D);
                hyppyvoimaannettu = true;
                torsoRigidbody2D.gravityScale = hypyngravityscale;


            }
            // e.g., you can jump only if grounded
            // Debug.Log("Ground detected!");

            animator.SetBool("kavely", false);
            animator.SetBool("paikallaan", false);
            animator.SetBool("katsoaalas", true);
            animator.SetBool("tippuu", true);
            animator.SetBool("makuulle", true);

            animator.SetBool("tippuukovemmin", true);

            animator.SetBool("paikallaan", false);
        }
        else
        {
            if (animator.GetBool("kavely"))
            {

                if (stateInfo.IsName("skelepienempikavely") || stateInfo.IsName("skelepienempimeneemakuule") || stateInfo.IsName("skelepienempikatsooalas"))
                {
                  //  torsoRigidbody2D.AddForce(kavelyvoima, forceMode2D);
                  //  torsoRigidbody2D.gravityScale = 1.0f;
                }
            }

        }
        if (stateInfo.IsName("skelelepienempihyppyoikeallekesken"))
        {
            //tutkitaan onko maata alla jos on niin staten vaihto
        }
        if (stateInfo.IsName("skelepienempitippuukovemmin") )
        {
            if (OnkoMaataTulossaTippumisessa())
            {
                animator.SetBool("tippuumaataalla", true);
                animator.SetBool("kavely", true);

                animator.SetBool("katsoaalas", false);
                animator.SetBool("tippuu", false);
                animator.SetBool("makuulle", false);

                animator.SetBool("tippuukovemmin", false);
                animator.SetBool("paikallaan", true);

                /*
                animator.enabled = true;
                if (handIKFollower != null)
                {
                    handIKFollower.enabled = false;
                    //skelepienempitippuualustalle
                }
                */
            }
            else
            {
                /*
                animator.enabled = false;
                if (handIKFollower != null)
                {
                    handIKFollower.enabled = true;
                    //skelepienempitippuualustalle
                }
                */

            }
        }
    }


    private bool OnkoMaataEdessa()
    {
        // Run a box overlap check at the given point
        Collider2D[] hits = Physics2D.OverlapBoxAll(maapaikantutkintakohta.transform.position, size, 0);

        foreach (Collider2D hit in hits)
        {
            if (hit.tag.Contains("tiili"))
            {
                return true;
            }
        }

        return false;
    }

    private bool OnkoMaataVasemmanjalanAlla()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(vasenjalkakohta.transform.position, vasenjalkakohtasize, 0);

        foreach (Collider2D hit in hits)
        {
            if (hit.tag.Contains("tiili"))
            {
                return true;
            }
        }

        return false;
    }
    private bool OnkoMaataOikeanjalanAlla()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(oikeajalkakohta.transform.position, oikeajalkakohtasize, 0);

        foreach (Collider2D hit in hits)
        {
            if (hit.tag.Contains("tiili"))
            {
                return true;
            }
        }

        return false;
    }



    private bool onkoSeinaoikealla()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(seinaoikeallakohta.transform.position, seinaoikeallakohtasize, 0);

        foreach (Collider2D hit in hits)
        {
            if (hit.tag.Contains("tiili"))
            {
                return true;
            }
        }

        return false;
    }

    private bool onkoOikeallaYlhaallatilaa()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(oikeallaylhaallakohta.transform.position, oikeallaylhaallakohtasize, 0);

        foreach (Collider2D hit in hits)
        {
            if (hit.tag.Contains("tiili"))
            {
                return true;
            }
        }

        return false;
    }

    public bool OnkoMaataTulossaTippumisessa()
    {

        //   public GameObject tippumismaakohta;
        //    public Vector2 tippumismaakohtasize = Vector2.one;

        Collider2D[] hits = Physics2D.OverlapBoxAll(tippumismaakohta.transform.position, tippumismaakohtasize, 0);

        foreach (Collider2D hit in hits)
        {
            if (hit.tag.Contains("tiili"))
            {
                return true;
            }
        }

        return false;

    }

    // Optional: visualize ground check area in Scene view
    private void OnDrawGizmosSelected()
    {
        if (maapaikantutkintakohta != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(maapaikantutkintakohta.transform.position, size);

        }
        if (tippumismaakohta != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(tippumismaakohta.transform.position, tippumismaakohtasize);

        }


        if (vasenjalkakohta != null)
        {

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(vasenjalkakohta.transform.position, vasenjalkakohtasize);
        }

        if (seinaoikeallakohta != null)
        {

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(seinaoikeallakohta.transform.position, seinaoikeallakohtasize);
        }


        if (oikeajalkakohta != null)
        {

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(oikeajalkakohta.transform.position, oikeajalkakohtasize);
        }
    }
}
