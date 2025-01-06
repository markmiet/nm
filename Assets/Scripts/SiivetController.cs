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

    private GameObject alus;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        peruskokoysuunnassa = PalautaSiipienKorkeus();
        aloituslocalposition = transform.localPosition;
        alus = PalautaAlus();


    }

    // Update is called once per frame
    void Update()
    {
        if (spriteRenderer.isVisible)
        {
            rotationTime += Time.deltaTime;
            float t = Mathf.PingPong(rotationTime / rotatetimeseconds, 1f);
            float currentRotationsiivet = Mathf.Lerp(siivetrotatemin, siivetrotatemax, t);

            perusrotationTime += Time.deltaTime;
            t = Mathf.PingPong(perusrotationTime / perusrotatetimeseconds, 1f);
            float currentRotationperus = Mathf.Lerp(perusrotatemin, perusrotatemax, t);



            transform.localRotation = Quaternion.Euler(currentRotationsiivet, 0, currentRotationperus);  // Z-axis rotation (for 2D)
                                                                                                         //  haukisiivet.transform.localPosition = uusloca;
            float nykykorkeus = PalautaSiipienKorkeus();
            float originelli = peruskokoysuunnassa;
            float erotus = originelli - nykykorkeus;

            Vector2 uusloca = new Vector3(aloituslocalposition.x, aloituslocalposition.y - erotus);

            //transform.localPosition = uusloca;
            PyoritaZsuunnassa();


            Vector3 directionToAlus = alus.transform.position - transform.position;

            float y = transform.position.y;
            //if (directionToAlus.x<0)
            //{
                if (directionToAlus.y == 0)
                {

                }
                else if (directionToAlus.y>0)
                {
                    y += nopeusy;
                }
                else
                {
                    y -= nopeusy;
                }
            //}
            transform.position = new Vector2(transform.position.x + nopeusx, y);
            
        }

   
    }
    private void PyoritaZsuunnassa()
    {

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
}
