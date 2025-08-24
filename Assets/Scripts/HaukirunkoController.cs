using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaukirunkoController : BaseController, IExplodable
{
    // Start is called before the first frame update
    private GameObject haukipaa;
    private GameObject haukisiivet;
    private GameObject haukipyrsto;

    private GameObject haukialaeva;
    private GameObject haukiylaeva;

    private GameObject haukikeskieva;

    private GameObject haukisilma;

    private GameObject haukietueva;


    public float siivetrotatemax = 5.0f;
    public float siivetrotatemin = -5.0f;


    public float paarotatemax = 5.0f;
    public float paarotatemin = -5.0f;


    public float haukipyrstorotatemax = 5.0f;
    public float haukipyrstorotatemin = -5.0f;

    public float haukirunkozrotatemax = 5.0f;
    public float haukirunkozrotatemin = -5.0f;

    public float haukirunkoyrotatemax = 5.0f;
    public float haukirunkoyrotatemin = -5.0f;


    public float silmarotatemax = 5.0f;
    public float silmarotatemin = -5.0f;


    public float alaevarotatemax = 5.0f;
    public float alaevarotatemin = -5.0f;


    public float ylaevarotatemax = 5.0f;
    public float ylaevarotatemin = -5.0f;


    public float etuevarotatemax = 5.0f;
    public float etuevarotatemin = -5.0f;

    public float keskievarotatemax = 5.0f;
    public float keskievarotatemin = -5.0f;




    public float rotatetimeseconds = 2.0f;//sekkaa
    private float rotationTime = 0f;       // Timer to control the rotation

    public float liikex = -0.2f;

    public float liikey = 1.0f;

    private SpriteRenderer haukisiivetspriterender;

    private SpriteRenderer haukirunkospriterender;

    private float siipienmaksimikoko = 0.0f;
    private Vector2 hauenlocalpos;
    private Vector2 hauenpaanlocalpos;
    private Quaternion hauenpaaquaternion;

    private Rigidbody2D m_Rigidbody2D;
    private AudioplayerController ad;
    void Start()
    {
        ad = PalautaAudioplayerController();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        haukirunkospriterender = GetComponent<SpriteRenderer>();

        haukipaa = gameObject.transform.Find("Haukipaa").gameObject;
        haukisiivet = gameObject.transform.Find("Haukisiivet").gameObject;

        haukipyrsto = gameObject.transform.Find("Haukipyrsto").gameObject;

        haukialaeva = gameObject.transform.Find("Haukialaeva").gameObject;
        haukiylaeva = gameObject.transform.Find("Haukiylaeva").gameObject;
        haukisilma = gameObject.transform.Find("Haukisilma").gameObject;
        haukikeskieva = gameObject.transform.Find("Haukikeskieva").gameObject;
        haukietueva = gameObject.transform.Find("Haukietueva").gameObject;


        haukisiivetspriterender = haukisiivet.GetComponent<SpriteRenderer>();
        siipienmaksimikoko = haukisiivetspriterender.bounds.size.y;

        hauenlocalpos = haukisiivet.transform.localPosition;

        hauenpaanlocalpos = haukipaa.transform.localPosition;

        hauenpaaquaternion = haukipaa.transform.rotation;
        /*
        GameObject[] allObstacles = GameObject.FindGameObjectsWithTag("alustag");
        foreach (GameObject obstacles in allObstacles)
        {
            alus = obstacles;
        }
        */
        alus = PalautaAlus();

        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        //  SliceSprite(sprite, 3, 2);

        IgnoreChildCollisions(transform);
        //   SliceSprite(sprite, 3, 2);



    }
    private GameObject alus;

    // Update is called once per frame
    void Update()
    {
        Tuhoa(gameObject); 

    }
    private bool silmaheitettyilmaan = false;

    private bool OnkoOkLiikkua()
    {
        return haukirunkospriterender.isVisible;
    }


    public float swayAmount = 0.5f;       // Amplitude of the swaying motion
    public float swaySpeed = 1.0f;          // Speed of the swaying (frequency of the tail movement)

    private float swayTimer = 0f;         // Timer for the sway


    public float tippumisaikasiipienmenettamisenjalkeen = 0.6f;
    private float kestoaikasiipienlahdosta = 0.0f;
    private bool tippuminensuoritettu = false;
    public void FixedUpdate()
    {
        if (!OnkoOkToimiaUusi(gameObject))
        {
            return;
        }
        /*
        transform.Translate(Vector2.left * 0.2f * Time.deltaTime);

        // Sway movement (up and down to simulate fish swimming)
        swayTimer += Time.deltaTime * swaySpeed;
        float sway = Mathf.Sin(swayTimer) * swayAmount;

        // Apply the swaying on the Y axis (up and down)
        transform.Translate(Vector2.up * sway * Time.deltaTime);

        if (true)
            return;
        */
        //haukip‰‰
        rotationTime += Time.deltaTime;

        float t = Mathf.PingPong(rotationTime / rotatetimeseconds, 1f);
        float currentRotationsiivet = Mathf.Lerp(siivetrotatemin, siivetrotatemax, t);

        float currentRotationpaa = Mathf.Lerp(paarotatemin, paarotatemax, t);

        float currentRotationpyrsto = Mathf.Lerp(haukipyrstorotatemax, haukipyrstorotatemin, t);

        float currentRotationrunko = Mathf.Lerp(haukirunkozrotatemax, haukirunkozrotatemin, t);

        float currentRotationrunkoy = Mathf.Lerp(haukirunkoyrotatemax, haukirunkoyrotatemin, t);


        float currentRotationsilma = Mathf.Lerp(silmarotatemax, silmarotatemin, t);
        float currentRotationylaeva = Mathf.Lerp(ylaevarotatemax, ylaevarotatemin, t);
        float currentRotationalaeva = Mathf.Lerp(alaevarotatemax, alaevarotatemin, t);

        float currentRotationetueva = Mathf.Lerp(etuevarotatemax, etuevarotatemin, t);



        float currentRotationkeskieva = Mathf.Lerp(keskievarotatemax, keskievarotatemin, t);


        //  haukisilma.transform.localRotation = Quaternion.Euler(0, 0, currentRotationsilma);  // Z-axis rotation (for 2D)

        transform.rotation = Quaternion.Euler(0, currentRotationrunkoy, currentRotationrunko);
        if (haukipaa != null)
        {
            haukipaa.transform.localRotation = Quaternion.Euler(0, currentRotationpaa, 0);  // Z-axis rotation (for 2D)
                                                                                            //   if (haukipaa.GetComponent<Rigidbody2D>().gravityScale>0)
                                                                                            //     {
                                                                                            //   haukisilma.GetComponent<Rigidbody2D>().gravityScale = haukipaa.GetComponent<Rigidbody2D>().gravityScale / 2;

            //   haukisilma.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 5.0f);
            //  }
        }
        else
        {
            /*
            if (!silmaheitettyilmaan)
            {
                haukisilma.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 60f));
                haukisilma.GetComponent<Rigidbody2D>().gravityScale = 0.5f;

                silmaheitettyilmaan = true;
            }
            */
        }


        //siivet



        haukipyrsto.transform.localRotation = Quaternion.Euler(0, currentRotationpyrsto, 0);  // Z-axis rotation (for 2D)


        haukialaeva.transform.localRotation = Quaternion.Euler(0, 0, currentRotationalaeva);  // Z-axis rotation (for 2D)

        haukiylaeva.transform.localRotation = Quaternion.Euler(0, 0, currentRotationylaeva);  // Z-axis rotation (for 2D)

        haukikeskieva.transform.localRotation = Quaternion.Euler(currentRotationkeskieva, 0, 0);  // Z-axis rotation (for 2D)

        //  haukietueva.transform.localRotation = Quaternion.Euler(currentRotationetueva, 0, 0);  // Z-axis rotation (for 2D)

        haukietueva.transform.localRotation = Quaternion.Euler(0, 0, currentRotationetueva);  // Z-axis rotation (for 2D)


        //float siipiennykykoko = PalautaSiipienKorkeus();
        //haukisiivet.transform.localPosition.x

        if (haukisiivet != null)
        {
            haukisiivet.transform.localRotation = Quaternion.Euler(currentRotationsiivet, 0, 0);  // Z-axis rotation (for 2D)

            float peruspositio = hauenlocalpos.y;
            float siipiennykykorkeus = PalautaSiipienKorkeus();
            float siivenkoonerotus = (siipienmaksimikoko - siipiennykykorkeus) / 2.0f;

            peruspositio = peruspositio - siivenkoonerotus;
            Vector2 uusloca = new Vector2(hauenlocalpos.x, peruspositio);
            haukisiivet.transform.localPosition = uusloca;
        }
        else
        {
            if (!tippuminensuoritettu)
            {
                if (kestoaikasiipienlahdosta == 0.0f)
                {
                    kestoaikasiipienlahdosta = Time.deltaTime;
                    m_Rigidbody2D.gravityScale = 0.5f;
                    return;
                }
                else
                {
                    kestoaikasiipienlahdosta = kestoaikasiipienlahdosta += Time.deltaTime;

                    if (kestoaikasiipienlahdosta >= tippumisaikasiipienmenettamisenjalkeen)
                    {
                        m_Rigidbody2D.gravityScale = 0.0f;
                        m_Rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
                        tippuminensuoritettu = true;
                        if (haukipaa==null)
                        {
                            Explode();
                        }
                    }
                }
            }


          
            //LiikuKunSiipiaEiOle();
            return;
            //  haukipaa.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
            //    haukisilma.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
        }
        // if (haukipaa!=null)
        // {
        //      haukipaa.transform.localPosition = hauenpaanlocalpos;
        //  haukipaa.transform.rotation = hauenpaaquaternion;
        // }


        bool ylos = AlusYlhaalla();
        bool alas = AlusAlhaalla();

        if (haukipaa != null && m_Rigidbody2D.gravityScale == 0.0f)
        {
            if (OnkoTiiliYlhaalla())
            {
             //   Debug.Log("tiili");
            }

            if (ylos && !OnkoTiiliYlhaalla())
            {
                m_Rigidbody2D.velocity = new Vector2(liikex,liikey);
                // haukipaa.GetComponent<Rigidbody2D>().velocity = new Vector2(liikex, 1.0f);
            }
            else if (alas && !OnkoTiiliAlhaalla())
            {
                m_Rigidbody2D.velocity = new Vector2(liikex, -liikey);
                // haukipaa.GetComponent<Rigidbody2D>().velocity = new Vector2(liikex, -1.0f);
            }
            else
            {
                m_Rigidbody2D.velocity = new Vector2(liikex, 0.0f);
                // haukipaa.GetComponent<Rigidbody2D>().velocity = new Vector2(liikex, 0.0f);
            }

            swayTimer += Time.deltaTime * swaySpeed;
            float sway = Mathf.Sin(swayTimer) * swayAmount;

            // Apply the swaying on the Y axis (up and down)
            //transform.Translate(Vector2.up * sway * Time.deltaTime);

            Vector2 vector2 = new Vector2(m_Rigidbody2D.velocity.x, m_Rigidbody2D.velocity.y+sway);
            m_Rigidbody2D.velocity = vector2;
        }



    }


    private float PalautaSiipienKorkeus()
    {
        return haukisiivetspriterender.bounds.size.y;
        //         haukisiivetspriterender.size.y;

    }


    private void LiikuKunSiipiaEiOle()
    {
        if (!OnkoOkToimiaUusi(gameObject))
        {
            return;
        }
        bool ylos = AlusYlhaalla();
        bool alas = AlusAlhaalla();
        // transform.position = new Vector2(transform.position.x + liikex, transform.position.y);
        if (ylos && !OnkoTiiliYlhaalla())
        {
            m_Rigidbody2D.velocity = new Vector2(liikex, 0.5f);
            
            // haukipaa.GetComponent<Rigidbody2D>().velocity = new Vector2(liikex, 1.0f);
        }
        else if (alas && !OnkoTiiliAlhaalla())
        {
            m_Rigidbody2D.velocity = new Vector2(liikex, -0.5f);
            // haukipaa.GetComponent<Rigidbody2D>().velocity = new Vector2(liikex, -1.0f);
        }
        else
        {
            m_Rigidbody2D.velocity = new Vector2(liikex, 0.0f);
            // haukipaa.GetComponent<Rigidbody2D>().velocity = new Vector2(liikex, 0.0f);
        }
    }

    public bool AlusYlhaalla()
    {
        bool onko = alus.transform.position.y > transform.position.y;

        return onko && Mathf.Abs(alus.transform.position.y - transform.position.y) > 0.5;

    }

    public bool AlusAlhaalla()
    {
        bool onko = alus.transform.position.y < transform.position.y;

        return onko && Mathf.Abs(alus.transform.position.y - transform.position.y) > 0.5;

    }

    public void Explode()
    {
        ad.ExplodePlay();
        Destroy(gameObject);
        RajaytaSprite(gameObject, 10, 10, 10.0f, 0.3f);

    }

    private bool OnkoTiiliAlhaalla()
    {
        // alabocenter, boxsizealhaalla

        return onkoTagiaBoxissaTransformPositionArvoonLisataanBoxLocation("vihollinen", boxsizealhaalla, alabocenter, layerMask);

    }

    //  public float rayDistance = 5.0f;  // Distance for the raycast
    // public string tileTag = "tiilivihollinentag"; // The tag to check for

    public LayerMask layerMask;
    private bool OnkoTiiliYlhaalla()
    {


        return onkoTagiaBoxissaTransformPositionArvoonLisataanBoxLocation("vihollinen", boxsizeylhaalla, ylaboxcenter, layerMask);


        /*
        int tileLayerMask = LayerMask.GetMask("Keskilayer");  // Ensure "TileLayer" is the correct name

    //  RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, rayDistance, tileLayerMask);

    for (float i = 0; i < rayDistance; i += 0.1f)
    {
        //tehd‰‰n 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, rayDistance, tileLayerMask);

        // Check if the ray hit something
        if (hit.collider != null)
        {
            // Check if the object we hit has the tag we are looking for
            Debug.Log("colliderin tagi=" + hit.collider.tag);

            if (hit.collider.CompareTag("tiilivihollinentag"))
            {
                Debug.Log("Up direction is colliding with a tiletag object in 2D.");
                // Add further logic if necessary
                return true;
            }
        }
    }

    return false;
    */
    }
    /*
        private void OnDrawGizmos()
        {
            // Set the color for the gizmo ray
            Gizmos.color = Color.red;

            // Draw the ray going upwards from the GameObject's position in the Scene view
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * rayDistance);
        }
        */

    public Vector2 ylaboxcenter = new Vector2(1, 2);
    public Vector2 boxsizeylhaalla = new Vector2(2, 2);


    public Vector2 alabocenter = new Vector2(1, -2);
    public Vector2 boxsizealhaalla = new Vector2(2, 2);


    void OnDrawGizmos()
    {

        // Set the color of the Gizmos
        //Gizmos.color = Color.green;

        // Draw the box representing the overlap area
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)transform.position + ylaboxcenter, boxsizeylhaalla);

        Gizmos.DrawWireCube((Vector2)transform.position + alabocenter, boxsizealhaalla);



    }

    /*
    void SliceSprite(Sprite originalSprite, int rows, int columns)
    {
        // Get the original sprite's texture
        Texture2D texture = originalSprite.texture;

        // Calculate the width and height of each slice
        int sliceWidth = texture.width / columns;
        int sliceHeight = texture.height / rows;

        // Create a list to store the sliced sprites
        List<Sprite> slicedSprites = new List<Sprite>();

        // Loop through each row and column to create slices
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                // Calculate the rectangle for the slice
                Rect sliceRect = new Rect(x * sliceWidth, y * sliceHeight, sliceWidth, sliceHeight);

                // Create a new sprite from the slice
                Sprite newSprite = Sprite.Create(
                    texture,
                    sliceRect,
                    new Vector2(0.5f, 0.5f),
                    originalSprite.pixelsPerUnit
                );

                // Store the new sprite in the list
                slicedSprites.Add(newSprite);

                // Optionally, instantiate a GameObject with the new sprite in the scene
                GameObject sliceObject = new GameObject($"Slice_{x}_{y}");
                SpriteRenderer sr = sliceObject.AddComponent<SpriteRenderer>();
                sr.sprite = newSprite;

                // Set the position of the slice in the scene (adjust if needed)
                sliceObject.transform.position = new Vector3(transform.position.x+ x * sliceWidth / originalSprite.pixelsPerUnit, 
                    transform.position.y+
                    y * sliceHeight / originalSprite.pixelsPerUnit, 0);
                Instantiate(sliceObject);
            }
        }


        Debug.Log("Slicing completed. Number of slices: " + slicedSprites.Count);

    }
    */


    public void PaaIrtiSekoita(float sekoituskerroin)
    {


        // haukipyrstorotatemax *= sekoituskerroin;
        // haukipyrstorotatemin *= sekoituskerroin;

        haukirunkozrotatemax *= sekoituskerroin;
        haukirunkozrotatemin *= sekoituskerroin;
        haukirunkoyrotatemax *= sekoituskerroin;
        haukirunkoyrotatemin *= sekoituskerroin;


        //  silmarotatemax *= sekoituskerroin;
        //  silmarotatemin *= sekoituskerroin;


        // alaevarotatemax *= sekoituskerroin;
        //  alaevarotatemin *= sekoituskerroin;


        //   ylaevarotatemax *= sekoituskerroin;
        //   ylaevarotatemin *= sekoituskerroin;


        etuevarotatemax *= sekoituskerroin;
        etuevarotatemin *= sekoituskerroin;

        //   keskievarotatemax *= sekoituskerroin;
        //   keskievarotatemin *= sekoituskerroin;

        rotatetimeseconds = rotatetimeseconds / 1.5f;

    }

    void OnBecameInvisible()
    {
        //Debug.Log ("OnBecameInvisible");
        // Destroy the enemy
        //tuhoa = true;

      //  Destroy(gameObject);
    }
    /*
    private bool TuhoaJosKameranAlla()
    {
        bool alla = IsObjectDownOfCamera(gameObject);
        if (alla)
        {
            Destroy(gameObject);
            Debug.Log("alla tuhoa");
            return true;
        }
        return false;


    }
    */
}
