using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : BaseController, IExplodable, IAlas
{

    public bool alas = true;
    bool liukuu = false;
    private Rigidbody2D m_Rigidbody2D;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        android = OnkoAndroidi();
    }
    private bool android;

    public float nopeusjonkaalleTuhoutuu = 0.2f;

    //public float maksimiAikajonkavoiollaElossa = 10.0f;
    //private float eloaika = 0.0f;
    // Update is called once per frame
    void Update()
    {

        TuhoaJosOikeallaPuolenKameraaTutkimuitakainEsimNopeus(gameObject, nopeusjonkaalleTuhoutuu);


            /*
            if (!android)
            {
                TuhoaJosVaarassaPaikassa(gameObject, true, true);
            }

            eloaika += Time.deltaTime;
            if (eloaika >= maksimiAikajonkavoiollaElossa)
            {
                Explode();
            }

            */
        }
        public void FixedUpdate()
    {
      //  float speed = m_Rigidbody2D.velocity.magnitude;
        //       Debug.Log("ammuksen nopeus=" + speed);

        //if (speed <= nopeusjonkaalleTuhoutuu)
       // {
          //  Destroy(gameObject);
        //}
    }


    public void Alas(bool al)
    {
        alas = al;
    }


    public void Putoa()
    {

        if (m_Rigidbody2D != null)
        {
            if (alas)
            {
                m_Rigidbody2D.velocity = new Vector2(0.1f, -20);
            }
            else
            {
                m_Rigidbody2D.velocity = new Vector2(0.1f, 20);
            }
        }
    }


    public float liukumisenjalkeinengravity = 20.0f;

    public float liukumisnopeus = 6.0f;
    public void Liu()
    {
        // Debug.Log("liukuuuuuuu");
        if (!liukuu)
        {
            liukuu = true;
            if (m_Rigidbody2D != null)
            {
                m_Rigidbody2D.velocity = new Vector2(liukumisnopeus, 0);
                if (alas)
                {
                    m_Rigidbody2D.gravityScale = liukumisenjalkeinengravity;
                }
                else
                {
                    m_Rigidbody2D.gravityScale = -liukumisenjalkeinengravity;
                }
            }


        }

    }

    public float damagemaarajokaaiheutetaan = 5.0f;

    private HashSet<GameObject> parentalreadyTriggered = new HashSet<GameObject>();
    void OnCollisionEnter2D(Collision2D col)
    {
        //  Debug.Log("alaosa collidoi");

        if (col.collider.tag.Contains("tiili") || col.collider.tag.Contains("laatikkovihollinenexplodetag") || col.collider.tag.Contains("eituhvih"))
        {

            //  col.otherCollider

            CapsuleCollider2D collider = col.otherCollider as CapsuleCollider2D;

            //eli jos törmäävä osa on tämä capsulocollider niin tuhoa 
            if (collider != null)
            {
                //Destroy(gameObject);
                // Explode();

                ExplodeLiukutormayksenJalkeen();

            }
            else
            {
                Liu();
            }



            //   Destroy(transform.parent);

            //Destroy (col.gameObject)

            // transform.parent.gameObject.SendMessage("Liu");
        }
        else if (col.collider.tag.Contains("vihollinen") && col.collider.tag.Contains("explode"))
        {
            /*
            if (col.collider.tag.Contains("vihollinen") && col.collider.tag.Contains("explode"))
            {
                tormattyviholliseen = true;
                //	Debug.Log("explodeeeeeeeeeeeeeeeee ");

                if (col.gameObject != null)
                {
                    Debug.Log("gameobjektin tagi=" + col.gameObject.tag);

                    //col.gameObject.SendMessage("Explode");
                    IExplodable o =
                    col.gameObject.GetComponent<IExplodable>();
                    */

            // col.gameObject.SendMessage("Explode");
            if (col.gameObject != null)
            {
                if (col.gameObject.transform.parent != null)
                {
                    HitCounter hc = col.gameObject.transform.parent.gameObject.GetComponent<HitCounter>();
                    if (hc == null)
                    {
                        if (parentalreadyTriggered.Contains(col.gameObject.transform.parent.gameObject))
                        {
                            return;
                        }
                        parentalreadyTriggered.Add(col.gameObject.transform.parent.gameObject);
                    }
                }

                HitCounter hitcounter = col.gameObject.GetComponent<HitCounter>();

                if (hitcounter != null)
                {

                    Vector2 contactPoint = col.GetContact(0).point;
                    hitcounter.RegisterHit(contactPoint);
                }
                else
                {

                    ChildColliderReporter childColliderReporter = col.gameObject.GetComponent<ChildColliderReporter>();
                    if (childColliderReporter != null)
                    {

                        Vector2 contactPoint = col.GetContact(0).point;
                        childColliderReporter.RegisterHit(contactPoint);

                    }
                    else
                    {
                        SkeletonController sc = col.gameObject.GetComponent<SkeletonController>();
                        if (sc != null)
                        {
                            GetComponent<Collider2D>().enabled = false;
                            Vector2 contactPoint = col.GetContact(0).point;
                            bool rajahtiko = sc.Explode(contactPoint);
                            if (rajahtiko)
                            {
                                GameManager.Instance.kasvataHighScorea(col.gameObject);
                            }

                        }
                        else
                        {
                            IDamagedable damageMahdollinen = col.gameObject.GetComponent<IDamagedable>();
                            if (damageMahdollinen != null)
                            {
                                bool rajahti = damageMahdollinen.AiheutaDamagea(damagemaarajokaaiheutetaan);
                                if (rajahti)
                                {
                                    GameManager.Instance.kasvataHighScorea(col.gameObject);
                                }
                            }
                            else
                            {
                                IExplodable o =
            col.gameObject.GetComponent<IExplodable>();
                                if (o != null)
                                {
                                    GetComponent<Collider2D>().enabled = false;

                                    o.Explode();
                                    GameManager.Instance.kasvataHighScorea(col.gameObject);
                                }
                                else
                                {
                                    Debug.Log("vihollinen ja explode mutta ei ookkaan " + col.collider.tag);
                                }
                            }
                        }
                    }
                }
            }


            //Destroy(gameObject);
            Explode();


            //Destroy (col.gameObject);

        }
        /*
        else if (col.collider.tag == "pallerospritetag")
        {

            col.gameObject.SendMessage("ExplodePallero");
            //col.gameObject.SendMessage("Explode");
            Destroy (col.gameObject);

        }
        */
    }



    void OnBecameInvisible()
    {
        //Debug.Log ("OnBecameInvisible");
        // Destroy the enemy
        //tuhoa = true;
        if (android)
        {
            Destroy(gameObject);
        }

    }


    public void Explode()
    {
        //   RajaytaSprite(gameObject, 4, 4, 1.0f, 0.3f);
        Destroy(gameObject);
    }

    public float force = 1;
    public float alive = 2;
    public float massa = 1;
    public int rows = 5;
    public int cols = 5;
    public GameObject explosion;
    public float explosionlivetime = 1.0f;

    public bool rajaytasprite = true;
    public bool teexplosion = true;
    public void ExplodeLiukutormayksenJalkeen()
    {
        if (rajaytasprite)
        {
            RajaytaSprite(gameObject, rows, cols, force, alive, massa, true);
        }

        /* 
          Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, affectedLayers);

          foreach (Collider2D collider in colliders)
          {
              Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();

              if (rb != null)
              {
                  // Calculate direction from the explosion center to the object
                  Vector2 direction = (rb.position - (Vector2)transform.position).normalized;

                  // Apply force to the Rigidbody2D
                  rb.AddForce(direction * explosionForce);
              }
          }

       */


        if (teexplosion)
        {
            GameObject instanssi = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(instanssi, explosionlivetime);
        }


        Destroy(gameObject);
    }


}
