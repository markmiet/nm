using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomForcesController : BaseController
{
    public float forceMin = 1f; // Minimum force
    public float forceMax = 2f; // Maximum force
    private Transform target; // Target GameObject

    public float maxSpeed = 5f; // Maximum speed the object can reach

    private Rigidbody2D rb;
    private float nextForceTime;

    public float rangetimemin = 1;
    public float rangetimemax = 3;
    private float startingmaxspeed = 0.0f;

    private SpriteRenderer sp;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = PalautaAlus().transform;
        nextForceTime = Time.time + Random.Range(rangetimemin, rangetimemax);
        okc = GetComponentInParent<OnkoOkToimiaController>();
        startingmaxspeed = maxSpeed;
        sp = GetComponentInParent<SpriteRenderer>();
    }
    private OnkoOkToimiaController okc;
    void Update()
    {
        if (IsGoingToBeDestroyed())
        {
            return;
        }

        if (okc!=null && okc.voikotoimia && Time.time >= nextForceTime)
        {
            ApplyRandomForce();
            nextForceTime = Time.time + Random.Range(rangetimemin, rangetimemax);
        }

    }

    void ApplyRandomForce()
    {
        // Calculate the direction and random force
        Vector2 directionToTarget = (target.position - transform.position).normalized;

        if (aiheutavoimaaVainjosAlusvasemmalla)
        {
            if (  directionToTarget.x > 0)
            {
                //return;
                directionToTarget = new Vector2(-1, 0);
            }
        }

        float randomForce = Random.Range(forceMin, forceMax);

        // Apply force to the main object
        rb.AddForce(directionToTarget * randomForce, ForceMode2D.Impulse);


        // After applying the force, clamp the speed of this object
        ClampSpeed();
    }

    public bool aiheutavoimaaVainjosAlusvasemmalla = false;

    void ClampSpeed()
    {
        // If the object's velocity exceeds the maxSpeed, limit it
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private Coroutine kasvataCoroutine;

    public void PistaMaxSpeednsinKymmenesosaanSiitamitaneOliSenJalkeenKasvataTakaisinViidessaSekunnissa(
        )
    {
        if (kasvataCoroutine != null)
            StopCoroutine(kasvataCoroutine);

        kasvataCoroutine=StartCoroutine(KasvataTakaisinEaseOutCoroutine());
    }
    
    private IEnumerator KasvataTakaisinEaseOutCoroutine()
    {

        float originelli = startingmaxspeed;
        float start = 0;
        maxSpeed = start;
        float kesto = 7f;
        float t = 0f;
        float origgravity = 0.0f;
        if (rb != null)
        {
            origgravity = rb.gravityScale;
            rb.gravityScale = 2.0f;

        }
        Color orig = Color.white;

        if (sp!=null)
        {
            orig = sp.color;
            sp.color = Color.red;
        }
        float stoppilaskuri = 0f;
        float totaalistoppi = 1.5f;//eli sekunti punaista
        while (stoppilaskuri < totaalistoppi)
        {
            stoppilaskuri += Time.deltaTime;

            yield return null; // odota seuraava frame

        }

        stoppilaskuri = 0f;
        totaalistoppi = 1.5f;//sitten sekunnin pätkä että väri vaihtuu pikku hiljaa
        while (stoppilaskuri < totaalistoppi)
        {
            stoppilaskuri += Time.deltaTime;
            //skeletonin väriksi punainen
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(stoppilaskuri / totaalistoppi);
            sp.color = Color.Lerp(Color.red, orig, progress);

            yield return null; // odota seuraava frame
        }


        if (rb != null)
        {
            rb.gravityScale = origgravity;
        }
        if (sp!=null && orig!=null)
        {
            sp.color = orig;
        }


        while (t < kesto)
        {
            t += Time.deltaTime;
            float progress = t / kesto;

            // Ease-out: alkaa hitaasti, kiihtyy lopussa
            float eased = Mathf.SmoothStep(0f, 1f, progress);

            maxSpeed = Mathf.Lerp(start, originelli, eased);
            //Debug.Log("mas max=" + maxSpeed);


           // float eased = 1f - Mathf.Pow(1f - progress, 2f);

           // maxSpeed = Mathf.Lerp(start, originelli, eased);

            yield return null; // odota seuraava frame
        }

        maxSpeed = originelli; // varmista tarkka lopputulos
    }

    private bool hidastusmenossa = false;
}

