using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulikkoPiippuController : BaseController
{
    // Start is called before the first frame update
    private GameObject alus;

    public GameObject ammussavu;
    public float ammussavukesto = 0.5f;
    void Start()
    {
        alus = PalautaAlus();
    }
    public float shootingcycle = 2.0f;


    public bool ammuvainjosalusvasemmmalla = true;
    private float l = 0.0f;
    public GameObject ammus;
    public float ampumisenvoimakkuus = 1.0f;

    public GameObject shooterposition;
    public GameObject piipuntoinenpaa;

    public float kulmalimit = 45;

    public float rekyyliVoima = 1.0f;

    public float torqueStrength = 10.5f;     // how fast to rotate
    public float damping = 0.5f;

    public float maarajokasallitaanaluksenoikeallaoloon = 1.0f;
    // Update is called once per frame


    public GameObject hylsynpaikka;
    public GameObject hylsy;
    public float hylsyeloaika = 2.0f;

    public float hylsyforce = 1.0f;
    public float hylsyrandomkulmaalku = 200;
    public float hylsyrandomkulmaloppu = 300;


    public bool ignorekulmalimit = false;
    void Update()
    {
        //if (OnkoOkToimiaUusi(gameObject))
        //{
        l += Time.deltaTime;
        float value = Random.Range(-0.1f, 0.3f);
        if (l >= shootingcycle+value)
        {
            if (ammus != null)
            {
                /*
                Vector2 directionToTarget = (alus.transform.position - shooterposition.transform.position).normalized;

                if (ammuvainjosalusvasemmmalla)
                {
                    if (directionToTarget.x > maarajokasallitaanaluksenoikeallaoloon)
                    {
                        return;
                    }
                }
                */
                Vector2 directionToTarget = (shooterposition.transform.position - alus.transform.position);

                if (!ammuvainjosalusvasemmmalla || (directionToTarget.x > maarajokasallitaanaluksenoikeallaoloon))
                {


                    Vector2 vektori = shooterposition.transform.position - piipuntoinenpaa.transform.position;

                    Vector2 aluspos = alus.transform.position;

                    //laske vektorin muodostamasta viivasta kulma tähän aluspos sijaintiin

                    Vector2 alusvektori = aluspos - (Vector2)shooterposition.transform.position;

                    float kulma = Vector2.Angle(vektori, alusvektori);

                    if (ignorekulmalimit || kulma <= kulmalimit)
                    {

                        Vector2 v2 =
                            palautaAmmukselleVelocityVectorKaytaPiippua(alus, ampumisenvoimakkuus, shooterposition.transform.position, piipuntoinenpaa.transform.position);

                        GameObject ins = Instantiate(ammus, shooterposition.transform.position, Quaternion.identity);
                        IgnoraaCollisiotVihollistenValilla(ins, gameObject);

                        ins.GetComponent<Rigidbody2D>().velocity = v2;
                        l = 0.0f;

                        if (rekyyliVoima > 0.0f)
                        {
                            Vector2 alku = piipuntoinenpaa.transform.position;
                            Vector2 loppu = shooterposition.transform.position;

                            Vector2 suunta = alku - loppu;

                            GetComponent<Rigidbody2D>().AddForce(suunta * rekyyliVoima, ForceMode2D.Impulse);

                            ApplyForces2D ap = GetComponent<ApplyForces2D>();
                            if (ap!=null)
                            {
                                ap.AsetaBoostedSpeedKayttoon();
                            }
                        }

                        if (ammussavu!=null && ammussavukesto>0.0f)
                        {
                            GameObject savu=
                                Instantiate(ammussavu, shooterposition.transform.position, Quaternion.identity);
                            Destroy(savu, ammussavukesto);
                        }
                        TeeHylsy();
                    }
                    else
                    {
                        /*
                        //float kulma2 = Vector2.SignedAngle(vektori, alusvektori);
                        //transform.Rotate(0, 0, kulma2);

                        Vector2 vektori2 = shooterposition.transform.position - piipuntoinenpaa.transform.position;
                        Vector2 aluspos2 = alus.transform.position;
                        Vector2 alusvektori2 = aluspos2 - (Vector2)shooterposition.transform.position;

                        // Desired angle (world-space)
                        float targetAngle = Mathf.Atan2(alusvektori2.y, alusvektori2.x) * Mathf.Rad2Deg;

                        // Current angle (from Rigidbody2D)
                        float currentAngle = GetComponent<Rigidbody2D>().rotation;

                        // Smallest signed angle difference (-180..180)
                        float angleDiff = Mathf.DeltaAngle(currentAngle, targetAngle);

                        // Apply torque proportional to how far off we are
                        float torqueStrength = 1.5f;  // adjust for responsiveness
                        GetComponent<Rigidbody2D>().AddTorque(angleDiff * torqueStrength);

                        */

                        Vector2 sdsdd = (

                        shooterposition.transform.position - piipuntoinenpaa.transform.position).normalized;


                        Vector2 directionToTarget2 = (alus.transform.position -
                            piipuntoinenpaa.transform.position).normalized;

                        // Desired angle in degrees
                        float targetAngle = Mathf.Atan2(directionToTarget2.y, directionToTarget2.x) * Mathf.Rad2Deg;

                        // Current angle (Rigidbody2D rotation)
                        //float currentAngle = GetComponent<Rigidbody2D>().rotation;

                        float currentAngle = Mathf.Atan2(sdsdd.y, sdsdd.x) * Mathf.Rad2Deg;

                        // Find smallest signed angle difference (-180..180)
                        float angleDiff = Mathf.DeltaAngle(currentAngle, targetAngle);

                        // Apply torque based on the difference
                       // float torqueStrength = 5.5f;     // how fast to rotate
                       // float damping = 0.5f;            // how much to smooth rotation

                        float torque = angleDiff * torqueStrength - GetComponent<Rigidbody2D>().angularVelocity * damping;
                        GetComponent<Rigidbody2D>().AddTorque(torque);

                    }

                }
            }
            //  }
        }

    }


    private void TeeHylsy()
    {
        if (hylsynpaikka!=null && hylsy!=null && hylsyeloaika>0.0f )
        {
            GameObject instanssihylsy = Instantiate(hylsy, hylsynpaikka.transform.position, Quaternion.identity);
            Destroy(instanssihylsy, hylsyeloaika);


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
}
