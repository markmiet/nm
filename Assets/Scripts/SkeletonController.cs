using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : BaseController, IExplodable
{

    public Sprite[] sprites; // Assign the new sprite in the Inspector

    private Rigidbody2D rb;
    // Start is called before the first frame update
    private SpriteRenderer sp;
    public GameObject lopullinenexplosion;

    public GameObject uusi;

    private int hitcount = 0;
    public int hitcoutneeded = 5;
    // public float hitdelay = 0.1f;

    //  private float hitelapsedtime = 0.0f;
    public bool destroy = false;

    public GameObject explosion;

    private float startingZRotation; // Store the initial Z rotation
    private float rotationLimit;    // 10% of the starting rotation

    [Range(0f, 100f)] // Slider in Inspector for easier adjustment
    public float rotationPercentage = 30f; // Default percentage is 30%
    public bool limitrotation = true;

    public bool teeEfektia = true;
    public bool vaihdasuuntaa = true;

    public bool ylosalaisin = false;
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        sp.sprite = sprites[0];

        startingZRotation = transform.rotation.eulerAngles.z;

        // Calculate 10% of the starting Z rotation

        if (ylosalaisin)
        {
            rb.gravityScale = -1 * rb.gravityScale;
            sp.flipY = !sp.flipY;
            //liikevasen = !liikevasen;
        }

        IgnoreChildCollisions(gameObject.transform);

    }

    public float spritechangetime = 1.0f;
    private float kesto = 0.0f;
    private int spriteindeksi = 0;
    private int step = 1;
    // Update is called once per frame


    public float efektivali = 2.0f;
    private float efektilaskuri = 0.0f;
    public float rajahdysgravity = 0.5f;

    public bool laskespritechangetimelennossa = false;
    private void LaskeSpriteChangeTime()
    {
        float vauhti = Mathf.Abs(liikex);
        //2=0.01
        //1=0.02
        if (laskespritechangetimelennossa)
            spritechangetime = 0.02f / liikex;
    }

    void Update()
    {
        if (!sp.isVisible)
        {
            //   return;
        }

        LaskeSpriteChangeTime();
        rotationLimit = 360f * (rotationPercentage / 100f); // Percentage of the full rotation
        float currentZRotation = NormalizeAngle(transform.rotation.eulerAngles.z);

        // Calculate clamped rotation range
        float minRotation = startingZRotation - rotationLimit / 2;
        float maxRotation = startingZRotation + rotationLimit / 2;

        // Clamp the Z rotation
        float clampedZRotation = Mathf.Clamp(currentZRotation, minRotation, maxRotation);

        // Apply the clamped rotation
        if (limitrotation)
        {
            // Apply the clamped rotation
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y,
                clampedZRotation
            );


        }
        if (teeEfektia)
        {
            efektilaskuri += Time.deltaTime;
            if (efektilaskuri >= efektivali)
            {
                RajaytaSprite(gameObject, rows, cols, explosionforce, aliviteme, sirpalemass, false,
                    rajaytyksenysaato, true, rajahdysgravity);
                efektilaskuri = 0;
            }

        }


        kesto += Time.deltaTime;



        if (stoppaa && stoppauslaskuri < kuinkakaunstopataan)
        {
            stoppauslaskuri += Time.deltaTime;
            return;
        }
        stoppaa = false;


        yhteensuuntaanliikkumisaika += Time.deltaTime;
        if (kesto >= spritechangetime)
        {


            if (sprites != null && sprites.Length > 0)
            {
                spriteindeksi += step;
                if (spriteindeksi < 0)
                {
                    spriteindeksi = 1;
                    step = step * -1;
                }

                if (spriteindeksi >= sprites.Length)
                {

                    step = step * -1;
                    spriteindeksi = sprites.Length - 1;
                }
                sp.sprite = sprites[spriteindeksi];
                kesto = 0;
            }
            /*
            if (vaihdasuuntaa && yhteensuuntaanliikkumisaika >= yhteensuuntaanliikkumisaikavaihtovali)
            {
                liikevasen = !liikevasen;
                yhteensuuntaanliikkumisaika = 0.0f;
                sp.flipX = liikevasen;
            }
            */

            //transform.position = new Vector3(transform.position.x + Time.deltaTime * liikex, transform.position.y, transform.position.z);

            //rb.AddForce(new Vector2(liikex, 0));

            //rb.AddForce(Vector2.right * kerroin * liikex);

        }
        float kerroin = liikevasen ? -1 : 1;



        float sinYMovement = Mathf.Sin(yhteensuuntaanliikkumisaika * sinfrequency) * sinamplitude;


        transform.position = new Vector3(transform.position.x + (Time.deltaTime * kerroin * liikex),
            transform.position.y + sinYMovement, transform.position.z);

    }
    private float yhteensuuntaanliikkumisaika = 0.0f;

    public float yhteensuuntaanliikkumisaikavaihtovali = 3.0f;
    private bool rajaytetty = false;
    public void Explode(Collision2D col)
    {
        if (rajaytetty)
        {
            return;
        }

        hitcount++;
        //   Debug.Log("hitcount=" + hitcount);
        if (hitcount >= hitcoutneeded)
        {
            // RajaytaSprite(gameObject, rows * 2, cols * 2, explosionforce * 8, aliviteme * 7, sirpalemass / 2, false,
            //     rajaytyksenysaato, true);
            //   RajaytaSprite(gameObject, rows , cols , explosionforce , aliviteme , sirpalemass, false,
            //rajaytyksenysaato, true);

            RajaytaSprite(gameObject, rows, cols, explosionforce, aliviteme, sirpalemass, false,
    rajaytyksenysaato, true, rajahdysgravity);

            if (lopullinenexplosion != null)
            {
                GameObject instanssi = Instantiate(lopullinenexplosion,transform.position, Quaternion.identity);


                Destroy(instanssi, aliviteme);

            }
            if (uusi != null)
            {
                //GameObject instanssi2 = Instantiate(uusi, transform.position, Quaternion.identity);
                //Invoke("InstantiatePrefab", delay);

                StartCoroutine(InstantiatePrefab());
                //s SkeletonController sc = instanssi2.GetComponent<SkeletonController>();

            }
            if (destroy)
            {
                Destroy(gameObject);
            }

            rajaytetty = true;
            hitcount = 0;
        }
        else
        {

            // RajaytaSprite(gameObject, rows, cols, explosionforce, aliviteme, sirpalemass, false, -1.0f);
            // Destroy(gameObject);
            if (explosion != null)
            {
                GameObject instanssi2 = Instantiate(explosion, col.otherCollider.transform.position, Quaternion.identity);

            }

        }

    }
    public float delay = 5.0f;

    IEnumerator InstantiatePrefab()
    {
        new WaitForSeconds(delay);
        Instantiate(uusi, transform.position, Quaternion.identity);
        Debug.Log("Object instantiated using Invoke!");

        yield return new WaitForSeconds(delay);
    }

    public void Explode()
    {
        /*
        public void RajaytaSprite(GameObject go, int rows, int columns, float explosionForce, float alivetime,
    float sirpalemass, bool teerigitbody, float ysaato)

        RajaytaSprite(gameObject,rows,cols, explosionforce, aliviteme,-1.0f);
        */


        /*


        hitelapsedtime += Time.deltaTime;

        if (hitelapsedtime >= hitdelay)
        {
          
            hitelapsedtime = 0.0f;
        }
       
        */
        hitcount++;
        if (hitcount >= hitcoutneeded)
        {
            //     RajaytaSprite(gameObject, rows * 2, cols * 2, explosionforce * 8, aliviteme * 7, sirpalemass / 2, false, -1.0f,false);
            if (lopullinenexplosion != null)
            {
                GameObject instanssi = Instantiate(lopullinenexplosion, transform.position, Quaternion.identity);

            }
            if (uusi != null)
            {
                GameObject instanssi2 = Instantiate(uusi, transform.position, Quaternion.identity);
            }
            if (destroy)
            {
                rajaytetty = true;
                Destroy(gameObject);
            }

            hitcount = 0;


        }
        else
        {

            // RajaytaSprite(gameObject, rows, cols, explosionforce, aliviteme, sirpalemass, false, -1.0f);
            // Destroy(gameObject);
            if (explosion != null)
            {
                GameObject instanssi2 = Instantiate(explosion, transform.position, Quaternion.identity);

            }

        }







    }

    public int rows = 3;
    public int cols = 3;
    public float explosionforce = 0.5f;
    public float aliviteme = 0.5f;
    public float sirpalemass = 1.0f;
    public float rajaytyksenysaato = 0.0f;
    public float liikex = 0.1f;
    public bool stoppaa = false;

    public bool liikevasen = true;

    public float kuinkakaunstopataan = 2.0f;
    public float stoppauslaskuri = 0.0f;

    //pientä ylösalas liikettä
    public float sinfrequency = 2f;
    public float sinamplitude = 0.5f;
    private float rotationTime = 0f;       // Timer to control the rotation

    /*
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (vaihdasuuntaa)
        {
            if (!stoppaa && !col.tag.Contains("makitavihollinenammus") && (col.tag.Contains("vihollinen") || col.tag.Contains("tiili")))
            {
                Debug.Log("collider tagi=" + col.tag);
                stoppaa = true;
                stoppauslaskuri = 0.0f;

                // Determine whether the trigger happened on the left or right side
                if (liikevasen)
                {
                   // Debug.Log("Triggered on the left side.");
                   // Debug.Log("triggeri eventti");
                    if (vaihdasuuntaa)
                    {
                        liikevasen = false;
                        sp.flipX = false;
                    }

                }
                else
                {
                  //  Debug.Log("Triggered on the right side.");
                    if (vaihdasuuntaa)
                    {
                        liikevasen = true;
                        sp.flipX = true;
                    }

                }
            }
            // Normalize angle to [-180, 180] range for consistent comparison
        }

    }
    */
    public void Tormatty(bool tormaysvasen)
    {
        Debug.Log("tormaysvasen=" + tormaysvasen);
        if (vaihdasuuntaa)
        {
            if (liikevasen == tormaysvasen)
            {
                liikevasen = !tormaysvasen;
                sp.flipX = !tormaysvasen;

                yhteensuuntaanliikkumisaika = 0.0f;
            }
        }
    }

    private float NormalizeAngle(float angle)
    {
        angle = angle % 360;
        if (angle > 180)
            angle -= 360;
        else if (angle < -180)
            angle += 360;

        return angle;
    }


}



