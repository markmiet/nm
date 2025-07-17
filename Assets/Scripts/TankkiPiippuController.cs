using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankkiPiippuController : ChildColliderReporter
{
    // Start is called before the first frame update
    private GameObject spaceship;
    private HingeJoint2D hingeJoint2D;
    public GameObject tankinAmmus;

    public int ammusmaara = 1;
    public float ammusrandomiprossa = 0f;


    public float maxMotorSpeed = 150f;
    public float maxTorque = 1000f;
    public float aimingTolerance = 1f; // kuinka l‰helle astetta t‰ht‰ys hyv‰ksyt‰‰n

    public GameObject piipunpaaJohonGameObjectInstantioidaan;
    public GameObject piipunToinenPaa;
    public float ammusVoima = 2.0f;
    public float tulinopeus = 2.0f;
    private float seuraavaTuliAika = 0f;


    public bool ammuriippumattaonkojotainvalissa = true;


    public float rekyyliVoima = 5f;
    public GameObject laukaisusavu;
    public float savukesto = 0.2f;

    public bool pakotaVaakaan = false;


    public GameObject hylsy;
    public GameObject hylsynpaikka;
    public float hylsyforce = 1.0f;
    public float hylsyrandomkulmaalku = 200;
    public float hylsyrandomkulmaloppu = 300;
    public bool teeHylsyja = false;
    public bool piippuosoittaaVasemmalle=true;

    public void AsetaVaakaan(bool aseta)
    {
        pakotaVaakaan = aseta;
    }

    //public GameObject piipunrajahdys;
    protected override void Start()

    {
        spaceship = PalautaAlus();
        hingeJoint2D = GetComponent<HingeJoint2D>();
        hingeJoint2D.useMotor = true;
        base.Start();

    }

    public float motorSpeedGain = 5.0f; // Controls responsiveness of rotation


    private bool KorjaaKulma()
    {
        float angle = hingeJoint2D.jointAngle;
        float min = hingeJoint2D.limits.min;
        float max = hingeJoint2D.limits.max;

        JointMotor2D motor = hingeJoint2D.motor;

        // If outside limits, activate motor to push back
        if (angle < min)
        {
            motor.motorSpeed = maxMotorSpeed;   // Positive speed = clockwise
            motor.maxMotorTorque = maxTorque*1000;
            hingeJoint2D.motor = motor;
            hingeJoint2D.useMotor = true;
            return true;
        }
        else if (angle > max)
        {
            motor.motorSpeed = -maxMotorSpeed;  // Negative speed = counter-clockwise
            motor.maxMotorTorque = maxTorque*1000;
            hingeJoint2D.motor = motor;
            hingeJoint2D.useMotor = true;
            return true;
        }
        else
        {
            return false;
        }

    }
    void Update()
    {
        if (spaceship == null) return;

        
        if (!OnkoOkToimiaUusi(gameObject))
        {
            return;
        }

        if (KorjaaKulma())
        {
            return;
        }

        // bool onko = OnkoPisteenJaAluksenValillaTilltaTaiVihollista(kohtaMissaAmmusLaukaistaan.transform.position, tankinAmmus);

        Vector2 alku = piipunToinenPaa.transform.position;
        Vector2 loppu = piipunpaaJohonGameObjectInstantioidaan.transform.position;

        Vector2 alus = spaceship.transform.position;

        // Suuntavektori piipun suuntaan
        Vector2 suunta = loppu - alku;

        // Kulma asteina (Atan2 antaa kulman suhteessa horisontaaliseen oikealle)
        float kulmaAsteina = Mathf.Atan2(suunta.y, suunta.x) * Mathf.Rad2Deg;



        Vector2 suuntaKiinnityskohdastaalukseen = alus - alku;
        float kulmaasteinaalukseen = Mathf.Atan2(suuntaKiinnityskohdastaalukseen.y, suuntaKiinnityskohdastaalukseen.x) * Mathf.Rad2Deg;

        //   Debug.Log("kulmaasteina=" + kulmaAsteina + " kulmaalukseen=" + kulmaasteinaalukseen);




        // 5. Kulmaero
        // Find angle error (shortest path, handles wrap-around)
        float angleError = Mathf.DeltaAngle(kulmaasteinaalukseen, kulmaAsteina);

        float motorSpeed = 0f;

        if (pakotaVaakaan)
        {
            motorSpeed = maxMotorSpeed; // Force constant rotation
        }
        else
        {
            // If angle is close enough, stop motor
            if (Mathf.Abs(angleError) < aimingTolerance)
            {
                motorSpeed = 0f;
            }
            else
            {
                // Proportional control for smooth rotation
                motorSpeed = Mathf.Clamp(angleError * motorSpeedGain, -maxMotorSpeed, maxMotorSpeed);
            }
        }

        // Apply motor settings
        JointMotor2D motor = hingeJoint2D.motor;
        motor.motorSpeed = motorSpeed;
        motor.motorSpeed = motorSpeed;
        motor.maxMotorTorque = maxTorque;
        hingeJoint2D.motor = motor;


        // Debug.Log("motorspeed=" + motorSpeed);

        // 7. Ampuminen kun t‰ht‰ys kohdallaan
        if ((ignoraaAmmuttaessaAimingToleranceKokonaan || Mathf.Abs(angleError) <= aimingTolerance) && Time.time >= seuraavaTuliAika)
        {
            bool ammuttiin=Ammu();
            if (ammuttiin)
            {
                //ei ammettu joten ei kasvateta
                seuraavaTuliAika = Time.time + tulinopeus;
            }
            
        }

    }
    public bool ignoraaAmmuttaessaAimingToleranceKokonaan = false;


    bool Ammu()
    {
        if (tankinAmmus == null || piipunpaaJohonGameObjectInstantioidaan == null) return false;



        if (IsOffScreen())
        {
            return false;
        }
        //eli jos ampujana on interceptor niin pit‰‰ olla suurinpiirtein vaakatasossa jottei ammuta


        //GameObject Instantiate(ammus);


        if (!ammuriippumattaonkojotainvalissa)
        {

            //return;
            bool onko = OnkoPisteenJaAluksenValillaTilltaTaiVihollista(piipunpaaJohonGameObjectInstantioidaan.transform.position,tankinAmmus);
            if (onko)
            {
                Debug.Log("ei ole vapaa v‰yl‰");
                return false;
            }

        }



        for (int i = 0; i < ammusmaara; i++)
        {

            Vector2 ve = palautaAmmukselleVelocityVectorKaytaPiippua(spaceship, ammusVoima, piipunpaaJohonGameObjectInstantioidaan.transform.position, piipunToinenPaa.transform.position);
            //Vector2 ve = (piipunpaaJohonGameObjectInstantioidaan.transform.position - piipunToinenPaa.transform.position).normalized;


            if (ammusrandomiprossa!=0.0f)
            {
                ve = RandomizeVector2(ve, ammusrandomiprossa);
            }
            //GameObject instanssi = Instantiate(tankinAmmus, piipunpaaJohonGameObjectInstantioidaan.transform.position, Quaternion.identity);

            GameObject instanssi = ObjectPoolManager.Instance.GetFromPool(tankinAmmus, piipunpaaJohonGameObjectInstantioidaan.transform.position, Quaternion.identity);


            MakitaVihollinenAmmusScripti m = instanssi.GetComponent<MakitaVihollinenAmmusScripti>();
            if (m != null)
            {
                m.SetCreator(this.gameObject);
                if (m.prefap==null)
                     m.prefap = tankinAmmus;


                //
            }


                //   PalliController p = instanssi.GetComponent<PalliController>();
                //   p.alusGameObject = alusGameObject;

                instanssi.GetComponent<Rigidbody2D>().velocity = ve;
            /*
                public GameObject hylsy;
    public GameObject hylsynpaikka;
    public float hylsyforce = 1.0f;
    public float hylsyrandomkulmaalku = 200;
    public float hylsyrandomkulmaloppu = 300;
    public bool teeHylsyja = false;
    */
            if (teeHylsyja && hylsy!=null && hylsynpaikka!=null)
            {
                //GameObject instanssihylsy = Instantiate(hylsy, hylsynpaikka.transform.position, Quaternion.identity);

                GameObject instanssihylsy = ObjectPoolManager.Instance.GetFromPool(hylsy, hylsynpaikka.transform.position, Quaternion.identity);
                if (instanssihylsy.GetComponent<HylsyController>().prefap==null)
                    instanssihylsy.GetComponent<HylsyController>().prefap = hylsy;


                IgnoraaCollisiotVihollistenValilla(instanssihylsy, gameObject);

                Rigidbody2D rb = instanssihylsy.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // Random angle between hylsyrandomkulmaalku and hylsyrandomkulmaloppu
                    float kulma = Random.Range(hylsyrandomkulmaalku, hylsyrandomkulmaloppu);

                    // Convert angle to direction vector
                    Vector2 suunta = Quaternion.Euler(0, 0, kulma) * Vector2.up;

                    // Apply force
                    rb.AddForce(suunta * hylsyforce, ForceMode2D.Impulse);
                }
            }
}

        if (laukaisusavu != null & savukesto > 0.0f)
        {
            GameObject instanssisavu = Instantiate(laukaisusavu, piipunpaaJohonGameObjectInstantioidaan.transform.position, Quaternion.identity);
            Destroy(instanssisavu, savukesto);

        }

        if (rekyyliVoima>0.0f)
        {
            Vector2 alku = piipunToinenPaa.transform.position;
            Vector2 loppu = piipunpaaJohonGameObjectInstantioidaan.transform.position;

            Vector2 suunta = alku - loppu;

            GetComponent<Rigidbody2D>().AddForce(suunta * rekyyliVoima, ForceMode2D.Impulse);
        }

        return true;
    }

    Vector2 RandomizeVector2(Vector2 input, float percent)
    {
        float xOffset = input.x * (percent / 100f);
        float yOffset = input.y * (percent / 100f);

        float newX = input.x + Random.Range(-xOffset, xOffset);
        float newY = input.y + Random.Range(-yOffset, yOffset);

        return new Vector2(newX, newY);
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
