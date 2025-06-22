using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankkiPiippuController : ChildColliderReporter
{
    // Start is called before the first frame update
    private GameObject spaceship;
    private HingeJoint2D hingeJoint2D;
    public GameObject tankinAmmus;

    public float maxMotorSpeed = 150f;
    public float maxTorque = 1000f;
    public float aimingTolerance = 1f; // kuinka l‰helle astetta t‰ht‰ys hyv‰ksyt‰‰n

    public GameObject kohtaMissaAmmusLaukaistaan;
    public GameObject kohtaMissaPiippuliittyytankkiin;
    public float ammusVoima = 2.0f;
    public float tulinopeus = 2.0f;
    private float seuraavaTuliAika = 0f;


    public bool ammuriippumattaonkojotainvalissa = true;

    //public GameObject piipunrajahdys;
    protected override void Start()

    {
        spaceship = PalautaAlus();
        hingeJoint2D = GetComponent<HingeJoint2D>();
        hingeJoint2D.useMotor = true;
        base.Start();

    }
    void Update()
    {
        if (spaceship == null) return;


        if (!OnkoOkToimiaUusi(gameObject))
        {
            return;
        }

       // bool onko = OnkoPisteenJaAluksenValillaTilltaTaiVihollista(kohtaMissaAmmusLaukaistaan.transform.position, tankinAmmus);

        Vector2 alku = kohtaMissaPiippuliittyytankkiin.transform.position;
        Vector2 loppu = kohtaMissaAmmusLaukaistaan.transform.position;

        Vector2 alus = spaceship.transform.position;

        // Suuntavektori piipun suuntaan
        Vector2 suunta = loppu - alku;

        // Kulma asteina (Atan2 antaa kulman suhteessa horisontaaliseen oikealle)
        float kulmaAsteina = Mathf.Atan2(suunta.y, suunta.x) * Mathf.Rad2Deg;



        Vector2 suuntaKiinnityskohdastaalukseen = alus - alku;
        float kulmaasteinaalukseen = Mathf.Atan2(suuntaKiinnityskohdastaalukseen.y, suuntaKiinnityskohdastaalukseen.x) * Mathf.Rad2Deg;

        //   Debug.Log("kulmaasteina=" + kulmaAsteina + " kulmaalukseen=" + kulmaasteinaalukseen);




        // 5. Kulmaero
        float angleError = Mathf.DeltaAngle(kulmaasteinaalukseen, kulmaAsteina);

        // 6. Moottorin ohjaus
        float motorSpeed = 0f;
        if (Mathf.Abs(angleError) > aimingTolerance)
        {
            motorSpeed = Mathf.Sign(angleError) * maxMotorSpeed;
        }

        JointMotor2D motor = hingeJoint2D.motor;
        motor.motorSpeed = motorSpeed;
        motor.maxMotorTorque = maxTorque;
        hingeJoint2D.motor = motor;
       // Debug.Log("motorspeed=" + motorSpeed);

        // 7. Ampuminen kun t‰ht‰ys kohdallaan
        if (Mathf.Abs(angleError) <= aimingTolerance && Time.time >= seuraavaTuliAika)
        {
            Ammu();
            seuraavaTuliAika = Time.time + tulinopeus;
        }

    }

    void Ammu()
    {
        if (tankinAmmus == null || kohtaMissaAmmusLaukaistaan == null) return;



        Vector2 ve = palautaAmmuksellaVelocityVector(spaceship, ammusVoima, kohtaMissaAmmusLaukaistaan.transform.position);

        //GameObject Instantiate(ammus);


        if (!ammuriippumattaonkojotainvalissa)
        {

            //return;
            bool onko = OnkoPisteenJaAluksenValillaTilltaTaiVihollista(kohtaMissaAmmusLaukaistaan.transform.position,tankinAmmus);
            if (onko)
            {
                Debug.Log("ei ole vapaa v‰yl‰");
                return;
            }

        }




        GameObject instanssi = Instantiate(tankinAmmus, kohtaMissaAmmusLaukaistaan.transform.position, Quaternion.identity);


        MakitaVihollinenAmmusScripti m = instanssi.GetComponent<MakitaVihollinenAmmusScripti>();
        if (m != null)
        {
            m.SetCreator(this.gameObject);
        }


        //   PalliController p = instanssi.GetComponent<PalliController>();
        //   p.alusGameObject = alusGameObject;

        instanssi.GetComponent<Rigidbody2D>().velocity = ve;

    }
    /*
    public void Explode()
    {
        if (piipunrajahdys!=null)
        {

            GameObject instanssi = Instantiate(piipunrajahdys, kohtaMissaAmmusLaukaistaan.transform.position, Quaternion.identity);
        }

        Destroy(gameObject);

    }
    */
}
