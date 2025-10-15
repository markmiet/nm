using System.Collections;
using UnityEngine;

public class ApplyForces2D : BaseController
{
    public GameObject targetObject;
    public float force = 10f;                // How strong the push is
    public float maxSpeed = 5f;              // Optional: limits velocity

    public float boostedSpeed = 350f;
    public ForceMode2D forceMode = ForceMode2D.Force;

    public float maxAngularSpeed = 20.0f;

    private Rigidbody2D rb;


    public bool aiheutavoimaaVainjosAlusvasemmalla = true;

    public float maarajokasallitaanaluksenoikeallaoloon = -1.0f;

    public Vector2 palautusVectori = Vector2.left;

    public GameObject paa;

    void Start()
    {
        targetObject = PalautaAlus();
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("No Rigidbody2D found on " + gameObject.name);

        originalMaxSpeed = maxSpeed;
    }

    void FixedUpdate()
    {
       // if (!OnkoOkToimiaUusi(gameObject))
      //  {
        //    return;
       // }
        /*
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        */

        if (targetObject == null || rb == null)
            return;
        
        //Vector2 directionToTarget = (targetObject.transform.position - transform.position).normalized;

        Vector2 directionToTarget = (targetObject.transform.position - paa.transform.position);


        if (aiheutavoimaaVainjosAlusvasemmalla)
        {
            if (directionToTarget.x > maarajokasallitaanaluksenoikeallaoloon)
            {
               
                rb.AddForce(palautusVectori * force, forceMode);
                /*
                // Optional: Limit the max speed
                if (rb.velocity.magnitude > maxSpeed)
                {
                    rb.velocity = rb.velocity.normalized * maxSpeed;
                }
                */
                return;
            }
        }
        Vector2 direction = (targetObject.transform.position - transform.position).normalized;

        // Apply force in that direction
        rb.AddForce( direction * force, forceMode);

        // Optional: Limit the max speed
        
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        if (Mathf.Abs(rb.angularVelocity) > maxAngularSpeed)
        {
            rb.angularVelocity = Mathf.Sign(rb.angularVelocity) * maxAngularSpeed;
        }


        /*
        Vector2 directionToTarget = (targetObject.transform.position- transform.transform.position);

        if (!aiheutavoimaaVainjosAlusvasemmalla || (directionToTarget.x > maarajokasallitaanaluksenoikeallaoloon))
        {

            // Get direction toward the target
            Vector2 direction = (targetObject.transform.position - transform.position).normalized;

            // Apply force in that direction
            rb.AddForce(dir * direction * force, forceMode);

            // Optional: Limit the max speed
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }

        }
        */


    }

    private float originalMaxSpeed;
    private Coroutine speedBoostCoroutine;
    public void AsetaBoostedSpeedKayttoon()
    {
        
        if (speedBoostCoroutine != null)
            StopCoroutine(speedBoostCoroutine);

        speedBoostCoroutine = StartCoroutine(AsetaMaxSpeedSuuremmaksiHetkeksi());
        
    }

    public IEnumerator AsetaMaxSpeedSuuremmaksiHetkeksi()
    {

        maxSpeed = boostedSpeed;
        /*
        if (rb!=null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        }
        */
        /*
        Collider2D[] c =
        GetComponents<Collider2D>();
        foreach(Collider2D cc in c)
        {
            cc.enabled = false;
        }

        */
        yield return new WaitForSeconds(0.8f);
        maxSpeed = originalMaxSpeed;
        /*
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }
        */
        /*
        Collider2D[] cc2 =
GetComponents<Collider2D>();
        foreach (Collider2D cc in cc2)
        {
            cc.enabled = true;
        }
        */
    }

    private float oboostedSpeed = 5;
    private float oforce= 0.5f;
    private float omaxAngularSpeed = 2.5f;

    private float omaxSpeed = 0.5f;
    public void PuolitaArvot()
    {

        oboostedSpeed = boostedSpeed;
        oforce = force;
        omaxAngularSpeed = maxAngularSpeed;

        omaxSpeed = maxSpeed;

        boostedSpeed = boostedSpeed / 10.0f;
        force = force / 10.0f;
        maxAngularSpeed = maxAngularSpeed / 10.0f;
        maxSpeed = maxSpeed / 10.0f;

    }
    public void PalautaArvot()
    {
        boostedSpeed = oboostedSpeed;
        force = oforce;
        maxAngularSpeed = omaxAngularSpeed;
        maxSpeed = omaxSpeed;

    }

}
