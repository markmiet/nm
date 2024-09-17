using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaukipaaController : BaseController
{
    // Start is called before the first frame update
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
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Explode()
    {
        Tipu();
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
        GameObject haukisilma = transform.parent.Find("Haukisilma").gameObject;
        GameObject alus = PalautaAlus();

        Vector2 vv= palautaAmmuksellaVelocityVector(alus, 2.0f);

        haukisilma.GetComponent<Rigidbody2D>().simulated = true;
        haukisilma.GetComponent<Rigidbody2D>().velocity = vv;

    }
    public bool ollaankoTiputtuAlasasti()
    {
        return true;
    }
}
