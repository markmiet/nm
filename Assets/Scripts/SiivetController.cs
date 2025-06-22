using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiivetController : BaseController, IExplodable
{
    public float siivetrotatemax = 5.0f;
    public float siivetrotatemin = -5.0f;
    private float rotationTime = 0f;       // Timer to control the rotation
    public float rotatetimeseconds = 2.0f;//sekkaa
    private SpriteRenderer spriteRenderer;

    private float peruskokoysuunnassa = 0.0f;
    // Start is called before the first frame update
    private Vector3 aloituslocalposition = Vector3.zero;


    public float perusrotatemax = 5.0f;
    public float perusrotatemin = -5.0f;
    private float perusrotationTime = 0f;       // Timer to control the rotation
    public float perusrotatetimeseconds = 2.0f;//sekkaa
    public float nopeusx = -0.01f;
    public float nopeusy = 0.01f;
    public float eroalukseen = 1.0f;
    private GameObject alus;

    public float sinfrequency = 2f;
    public float sinamplitude = 0.5f;

    private Vector2 fixedColliderSize;

    private BoxCollider2D boxCollider;
    private LayerMask collisionLayer;

    private void Randomize()
    {
        float randomNumber = Random.Range(1 - randomisointiprossa, 1 + randomisointiprossa);
        sinfrequency = sinfrequency * randomNumber;
        sinamplitude = sinamplitude * randomNumber;

        nopeusx = nopeusx * randomNumber;
        nopeusy = nopeusy * randomNumber;
        collisionLayer = 1 << gameObject.layer;
    }
    public float randomisointiprossa = 0.10f;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        peruskokoysuunnassa = PalautaSiipienKorkeus();
        aloituslocalposition = transform.localPosition;
        alus = PalautaAlus();

        boxCollider = GetComponent<BoxCollider2D>();

        // Ensure the BoxCollider2D is found
        if (boxCollider != null)
        {
            // Set the BoxCollider2D size to the fixed size
            //  boxCollider.size = fixedColliderSize;
            fixedColliderSize = boxCollider.size;
        }
        else
        {
            Debug.LogError("No BoxCollider2D found on this GameObject!");
        }
        Randomize();
    }

    // Update is called once per frame
    void Update()
    {
        if (OnkoOkToimiaUusi(gameObject))
        {

            if (boxCollider != null)
            {
                //      boxCollider.size = fixedColliderSize;
            }
            float delta = Time.deltaTime;

            rotationTime += delta;
            float currentRotationsiivet = CalculatePingPongRotation(siivetrotatemin, siivetrotatemax, rotationTime, rotatetimeseconds);

            perusrotationTime += delta;
            float currentRotationperus = CalculatePingPongRotation(perusrotatemin, perusrotatemax, perusrotationTime, perusrotatetimeseconds);

            transform.localRotation = Quaternion.Euler(currentRotationsiivet, 0, currentRotationperus);

            float nykykorkeus = PalautaSiipienKorkeus();
            float originelli = peruskokoysuunnassa;
            float erotus = originelli - nykykorkeus;

            Vector2 uusloca = new Vector3(aloituslocalposition.x, aloituslocalposition.y - erotus);

            //transform.localPosition = uusloca;
            PyoritaZsuunnassa();

            MoveTowardsAlus(delta);
            Vaista(delta, collisionLayer);
        }

        Tuhoa(gameObject);
    }
    
    private void MoveTowardsAlus(float delta)
    {
        if (alus == null) return;

        // Directional movement towards 'alus'
        Vector3 directionToAlus = alus.transform.position - transform.position;

        // Vertical adjustment based on distance to 'alus'
        float yAdjustment = 0f;
        if (Mathf.Abs(directionToAlus.y) >= eroalukseen)
        {
            yAdjustment = directionToAlus.y > 0 ? nopeusy : -nopeusy;
        }

        // Sinusoidal movement along the y-axis

        float sinYMovement = Mathf.Sin(rotationTime * sinfrequency) * sinamplitude;

        //   if (!tormatty)
        // {
        transform.position += new Vector3(delta * nopeusx, delta * (yAdjustment + sinYMovement), 0f);
        // }

        // Apply movement

    }
  //  public int rayCount = 5; // Number of rays to cast
//    public float vaistonopeus = 1.0f;


    /*
    private void Vaista(float delta)
    {
        bool vasen = OnkoVasemmallaVaistettavaa(rayCount, rayDistance, collisionLayer);
        Debug.Log("vasen=" + vasen);

        if (vasen)
        {
            bool ylos =
                OlisikoylhaallaVaistotilaa(rayCount, rayDistance, collisionLayer );

            Debug.Log("ylos=" + ylos);
            if (ylos)
            {
                VaistaPystysuunnassa(delta, vaistonopeus);
                //vaistettuylos = true;
            }
            else
            {
                bool alas = OlisikoalhaallaVaistotilaa(rayCount, rayDistance, collisionLayer);
                Debug.Log("alas=" + alas);
                if (alas)
                {
                    VaistaPystysuunnassa(delta,-vaistonopeus);
                }
               
            }
        }

    }





    public float rayDistance = 0.5f; // Distance of the raycast
    public LayerMask collisionLayer; // Layers to detect collisions
    */


    private void PyoritaZsuunnassa()
    {

    }

    float CalculatePingPongRotation(float min, float max, float time, float duration)
    {
        float t = Mathf.PingPong(time / duration, 1f);
        return Mathf.Lerp(min, max, t);
    }

    private float PalautaSiipienKorkeus()
    {
        return spriteRenderer.bounds.size.y;
    }

    public void Explode()
    {
        RajaytaSprite(gameObject, 3, 3, 3, 1);
        Destroy(gameObject);
    }

    private bool tormatty = false;
    void OnCollisionEnter2D(Collision2D col)
    {
        //haukisilmavihollinenexplodetag

        //explodetag
        if (col.collider.tag.Contains("hauki") || col.collider.tag.Contains("tiili") || col.collider.tag.Contains("pyoroovi") ||
            col.collider.tag.Contains("laatikkovihollinenexplodetag"))
        {
            tormatty = true;
        }
        else if (col.collider.tag.Contains("pallovihollinen"))
        {
            tormatty = true;
        }
        else if (col.collider.tag.Contains("vihollinen"))
        {
            tormatty = true;

        }

    }

}
