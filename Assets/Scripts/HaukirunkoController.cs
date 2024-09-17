using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaukirunkoController : BaseController
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

    public float liikex = -0.1f;

    private SpriteRenderer haukisiivetspriterender;

    private SpriteRenderer haukirunkospriterender;

    private float siipienmaksimikoko = 0.0f;
    private Vector2 hauenlocalpos;
    private Vector2 hauenpaanlocalpos;
    private Quaternion hauenpaaquaternion;

    private Rigidbody2D m_Rigidbody2D;
    void Start()
    {
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

        IgnoreChildCollisions(transform);
    }
    private GameObject alus;

    // Update is called once per frame
    void Update()
    {

    }
    private bool silmaheitettyilmaan = false;
    public void FixedUpdate()
    {
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
        float currentRotationalaeva= Mathf.Lerp(alaevarotatemax, alaevarotatemin, t);

        float currentRotationetueva = Mathf.Lerp(etuevarotatemax, etuevarotatemin, t);



        float currentRotationkeskieva = Mathf.Lerp(keskievarotatemax, keskievarotatemin, t);


      //  haukisilma.transform.localRotation = Quaternion.Euler(0, 0, currentRotationsilma);  // Z-axis rotation (for 2D)

        transform.rotation= Quaternion.Euler(0, currentRotationrunkoy, currentRotationrunko);
        if (haukipaa!=null)
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

        haukikeskieva.transform.localRotation = Quaternion.Euler(currentRotationkeskieva,0, 0);  // Z-axis rotation (for 2D)

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
            m_Rigidbody2D.gravityScale = 0.5f;
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
        
        if (m_Rigidbody2D.gravityScale ==0.0f)
        {
            if (ylos)
            {
                m_Rigidbody2D.velocity = new Vector2(liikex, 1.0f);
                // haukipaa.GetComponent<Rigidbody2D>().velocity = new Vector2(liikex, 1.0f);
            }
            else if (alas)
            {
                m_Rigidbody2D.velocity = new Vector2(liikex, -1.0f);
                // haukipaa.GetComponent<Rigidbody2D>().velocity = new Vector2(liikex, -1.0f);
            }
            else
            {
                m_Rigidbody2D.velocity = new Vector2(liikex, 0.0f);
                // haukipaa.GetComponent<Rigidbody2D>().velocity = new Vector2(liikex, 0.0f);
            }
        }
        

      
    }


    private float PalautaSiipienKorkeus()
    {
       return  haukisiivetspriterender.bounds.size.y;
//         haukisiivetspriterender.size.y;

    }

    public bool AlusYlhaalla()
    {
      bool onko=alus.transform.position.y > transform.position.y;

      return onko && Mathf.Abs(alus.transform.position.y - transform.position.y) > 0.5 ;

    }

    public bool AlusAlhaalla()
    {
        bool onko= alus.transform.position.y < transform.position.y;

        return onko && Mathf.Abs(alus.transform.position.y - transform.position.y) > 0.5;

    }

    public void Explode()
    {
        Destroy(gameObject);
    }

    void IgnoreChildCollisions(Transform parent)
    {
     
        Collider[] childColliders = parent.GetComponentsInChildren<Collider>();

        for (int i = 0; i < childColliders.Length; i++)
        {
            Physics.IgnoreCollision(childColliders[i], parent.gameObject.GetComponent<Collider>());
            for (int j = i + 1; j < childColliders.Length; j++)
            {
                Physics.IgnoreCollision(childColliders[i], childColliders[j]);

            }
        }
    }

}
