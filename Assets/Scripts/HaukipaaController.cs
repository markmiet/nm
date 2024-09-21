using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaukipaaController : BaseController, IExplodable
{

    public float explosionForce = 10f; // Base force magnitude
   // public float explosionRadius = 5f; // Radius of explosion


    // Start is called before the first frame update
    private Sprite sprite;
    void Start()
    {
        //mit‰ tapahtuu kun hauen p‰‰h‰n osuu?
        //kuoleeko koko roska vai ei?

        //1.kuolee
        //r‰j‰yt‰ eri osat pikku viiveill‰ :)

        //2. ei kuole
        //1.r‰j‰yt‰ pelkk‰ p‰‰ -> muuta liikkuminen kuten p‰‰ttˆm‰ll‰ kanalla :)
        //2.p‰‰ ei r‰j‰hd‰ vaan lent‰‰ jonnekin ja tippuu ja t‰ss‰ pit‰‰ tehd‰ veriefekti eli p‰‰n kohdalta pursuaa verta
        //kun se tippuu niin sillon vasta lent‰‰ se silm‰ eli ammus ja kun ammus on lent‰nyt niin samalla hauen p‰‰ r‰j‰ht‰‰


        //kuolee vasta kun runkoon osutaan siis

        sprite = GetComponent<SpriteRenderer>().sprite;
      


    }

    // Update is called once per frame
    int maara = 0;
    void Update()
    {


    }
    public void Explode()
    {
        //Tipu();
        TipUEnsinSenJalkeenAmmuJaRajahda();
        //     GetComponent<Rigidbody2D>().gravityScale = 0.5f;
        //   Destroy(gameObject, 0.5f);
    }

    public void Tipu()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0.5f;
        FixedJoint2D jointi = GetComponent<FixedJoint2D>();
        Destroy(jointi);
        Ammu();
    }

    public void Ammu()
    {
        //ampu
        // Destroy(gameObject);
        if (transform!=null && 
            transform.parent!=null && transform.parent.Find("Haukisilma")!=null)
        {
            GameObject haukisilma = transform.parent.Find("Haukisilma").gameObject;
            
            //GameObject instantiate()
            GameObject alus = PalautaAlus();

            Vector2 vv = palautaAmmuksellaVelocityVector(alus, 2.0f);

            HaukiSilmaController h=
            haukisilma.GetComponent<HaukiSilmaController>();
            h.paassakiinni = false;

              haukisilma.transform.parent = null;
            //haukisilma.transform.localPosition=new Vector3(0, 0, 0);
            haukisilma.GetComponent<Rigidbody2D>().simulated = true;
            haukisilma.GetComponent<Rigidbody2D>().velocity = vv;
        }

    }

    public void TipUEnsinSenJalkeenAmmuJaRajahda()
    {

        //antaa tippua ihan hetki pieni ja sitten vasta t‰m‰
        GetComponent<Rigidbody2D>().gravityScale = 0.5f;
        FixedJoint2D jointi = GetComponent<FixedJoint2D>();
        Destroy(jointi);
        Ammu();
        StartCoroutine(ExecuteAfterDelay(1.0f));

        HaukirunkoController h=
        gameObject.GetComponentInParent<HaukirunkoController>();
        h.PaaIrtiSekoita(1.2f);

    }


    private IEnumerator ExecuteAfterDelay(float delay)
    {
        // Wait for the specified amount of time
        yield return new WaitForSeconds(delay);

        // After the delay, execute your action
        PerformAction();
    }

    // Example of what could happen after the delay
    private void PerformAction()
    {
       // Debug.Log("Action performed after delay!");
        Destroy(gameObject);
     
        RajaytaSprite(gameObject, 10, 10, explosionForce, 0.3f);
        // Add your actual action logic here
    }



    public bool ollaankoTiputtuAlasasti()
    {
        return true;
    }

    

}
