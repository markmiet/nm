using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterceptorController : BaseController
{
    // Start is called before the first frame update

    public GameObject eturengas;
    public GameObject takarengas;
    public float eroalukseenJottaLiikutaan = 0.1f;
    public float moottorivoima = 1000f;
    public float maxTorque = 10000f;
    private Rigidbody2D rb;

    private TankkiPiippuController tankkiPiippuController;

    void Start()
    {
        tankkiPiippuController=gameObject.transform.parent.gameObject.GetComponentInChildren<TankkiPiippuController>();
        rb = GetComponent<Rigidbody2D>();
        rb.centerOfMass = new Vector2(0f, -0.5f); // Lower center of mass vertically


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //tutkitaan ollaanko ruudun vasemman reunan viimeisessä 20%
        //jos näin niin sitten moottorit käyntiin aina...


        //pitäisi siis kontrolloida wheelejä sen mukaan että ollaan kamerassa
        //toiminta aktivoituu vasta kun ollaan yli puolen välin ruudusta vasemmalle
        /*
        if (!OnkoKameranVasemmallaPuolella(gameObject, 3.0f) && !OnkoOkToimiaUusi(gameObject))
        {
            return ;
        }
        
        if (OnkoKameranVasemmallaPuolella(gameObject,3.0f))
        {
            //Destroy(gameObject);
            Destroy(gameObject.transform.parent.gameObject);
            return;
        }
        */
        bool toimi=HoidaInterceptorTapaus(gameObject);
        if (!toimi)
        {
         //   tankkiPiippuController.estaAmpuminen = true;
            return;
        }
        //eli vain jos molemmat renkaat on maassa niin silloin kaasua?

        //lateupdateen tai jonnekin vielä se että jos on liian kallellaan niin korjaa automaattisesti kaltevuuden

        //tänne vielä tutkinta jos on oikealla tiiltä niin ei kaasua väkisellä

        //pistä moottorivoimat isommalle eli ne on nyt vain puolet siitä mitä oli

        //no jos menee ihan nurin niin eikun vaan kunnon räjäytys? :)

        bool etuilmassa = OnkoRengasIlmassa(eturengas);
        bool takailmassa = OnkoRengasIlmassa(takarengas);
        bool tiilivasemmalla = OnkoTiiliVasemmalla(gameObject);

        bool autonkeskikohdanallaontiili = OnkoAutonRungonAllaTiili(gameObject, 0.7f);

        bool tiilioikealla = OnkoTiiliOikealla(eturengas);
        /* */
        if (tiilioikealla)
        {
            Debug.Log("tiilioikealla");
        }
       

        //miten saadaan selville onko lähellä hyppyri johon pitää ottaa vauhtia


        // rb.freezeRotation = etuilmassa && takailmassa;

        if (etuilmassa && takailmassa)
        {
            if (Mathf.Abs(rb.angularVelocity) > 50f)
                rb.angularVelocity = Mathf.Sign(rb.angularVelocity) * 50f;
        }
        /* */
        bool ylosalaisin = OllaankoYlosAlaisin();
        //ylosalaisin ja maassa
        if (ylosalaisin && transform.parent != null && KoskettaakoRunkoMaata())
        {
            //tähän vielä räjäytkset
            //eli childcollisionreporterin avulla :)


            HitCounter[] ccc =
            transform.parent.gameObject.GetComponentsInChildren<HitCounter>();
            foreach(HitCounter aa in ccc)
            {
               // int uupuu = aa.hitThreshold - aa.hitCount;
                //for (int i=0;i<uupuu;i++)
                //{
                    aa.RajaytaChildrenit();
                //}
            }
            
            Destroy(transform.parent.gameObject);
            return;
        }
       

        /*
        if (etuilmassa || takailmassa)
        {

       //     tankkiPiippuController.estaAmpuminen = true;
            return;
        }
        */
        //  Debug.Log("rb.angularVelocity=" + rb.angularVelocity);

        //vielä v
        //tankkiPiippuController.estaAmpuminen = false;
        if ((etuilmassa || takailmassa ) && OnkoKeulaYlhaallaRajaAsteenVerran())
        {
            tankkiPiippuController.AsetaVaakaan(true);
        }
        else
        {
            tankkiPiippuController.AsetaVaakaan(false);
        }

        if ( /*(!etuilmassa || !takailmassa) &&*/ autonkeskikohdanallaontiili && (OnkoKeulaYlhaallaRajaAsteenVerran() || tiilioikealla) && !AlusVasemmalla())
        {
            WheelJoint2D[] wh =
GetComponents<WheelJoint2D>();
            foreach (WheelJoint2D w in wh)
            {

                JointMotor2D motor = w.motor;
                motor.motorSpeed = -moottorivoima * 12f;
                motor.maxMotorTorque = maxTorque*5;

                //motor.motorSpeed = -moottorivoima * 8f;
                //motor.maxMotorTorque = maxTorque * 4;

                w.motor = motor;
            }
        }
        else if (OnkoKameranVasemmallaPuolella(gameObject, 0.0f))
        {
            WheelJoint2D[] wh =
GetComponents<WheelJoint2D>();
            foreach (WheelJoint2D w in wh)
            {

                JointMotor2D motor = w.motor;
                motor.motorSpeed = -moottorivoima * 6.0f;
                motor.maxMotorTorque = maxTorque;
                w.motor = motor;
            }
        }
        else if ((Camera.main.GetComponent<Kamera>().GetCurrentScrollSpeed() > 0.0f && OnkoKameranVasemmassaReunassa(gameObject, 2.5f)))
        {
            WheelJoint2D[] wh =
GetComponents<WheelJoint2D>();
            foreach (WheelJoint2D w in wh)
            {

                JointMotor2D motor = w.motor;
                motor.motorSpeed = -moottorivoima * 5f;
                motor.maxMotorTorque = maxTorque;
                w.motor = motor;
            }
        }
        else if ((Camera.main.GetComponent<Kamera>().GetCurrentScrollSpeed() > 0.0f && OnkoKameranVasemmassaReunassa(gameObject, 5)))
        {
            WheelJoint2D[] wh =
GetComponents<WheelJoint2D>();
            foreach (WheelJoint2D w in wh)
            {

                JointMotor2D motor = w.motor;
                motor.motorSpeed = -moottorivoima*2.5f;
                motor.maxMotorTorque = maxTorque;
                w.motor = motor;
            }
        }

        else if ( (Camera.main.GetComponent<Kamera>().GetCurrentScrollSpeed()>0.0f &&  OnkoKameranVasemmassaReunassa(gameObject,15)) ||  AlusOikealla())
        {
            WheelJoint2D[] wh =
GetComponents<WheelJoint2D>();
            foreach (WheelJoint2D w in wh)
            {

                JointMotor2D motor = w.motor;
                motor.motorSpeed = -moottorivoima;
                 motor.maxMotorTorque = maxTorque;
                w.motor = motor;
            }
        }
        else if (AlusVasemmalla() && ( !tiilivasemmalla || OnSlope()))
        {
            WheelJoint2D[] wh =
GetComponents<WheelJoint2D>();
            foreach (WheelJoint2D w in wh)
            {

                JointMotor2D motor = w.motor;
                motor.motorSpeed = moottorivoima;
                motor.maxMotorTorque = maxTorque;
                w.motor = motor;
            }
        }
        /*
        else if (tiilivasemmalla)
        {
            WheelJoint2D[] wh =
GetComponents<WheelJoint2D>();
            foreach (WheelJoint2D w in wh)
            {

                JointMotor2D motor = w.motor;
                motor.motorSpeed = -moottorivoima/10.0f;
                motor.maxMotorTorque = maxTorque;
                w.motor = motor;
            }
        }
        */
        else 
        {
            WheelJoint2D[] wh =
            GetComponents<WheelJoint2D>();
            foreach (WheelJoint2D w in wh)
            {

                JointMotor2D motor = w.motor;
                motor.motorSpeed = 0.0f;
                 motor.maxMotorTorque = maxTorque;
                w.motor = motor;
            }
        }
    }

    private bool AlusVasemmalla()
    {
        float interx = GetCenterX(GetComponent<SpriteRenderer>());
        float aluksenx = GetCenterX(PalautaAlus().GetComponent<SpriteRenderer>());

        if (aluksenx<interx)
        {
            if (interx-aluksenx> eroalukseenJottaLiikutaan)
            {
                return true;
            }
        }
        return false;
    }

    public bool AlusOikealla()
    {
        float interx = GetCenterX(GetComponent<SpriteRenderer>());
        float aluksenx = GetCenterX(PalautaAlus().GetComponent<SpriteRenderer>());

        if (aluksenx > interx)
        {
            if (aluksenx-interx > eroalukseenJottaLiikutaan)
            {
                return true;
            }
        }
        return false;
    }



    float GetCenterX(SpriteRenderer sr)
    {
        Bounds bounds = sr.bounds;
        return bounds.center.x;
    }

    public bool OnkoRengasIlmassa(GameObject rengasObj, float etaisyys = 0.1f)
    {
        Collider2D col = rengasObj.GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogWarning("Rengasobjektilla ei ole Collider2D:ta.");
            return false;
        }

        // Lasketaan renkaan alareuna colliderin boundsien mukaan
        Vector2 alaPiste = new Vector2(col.bounds.center.x, col.bounds.min.y);

        // Raycast alaspäin
        RaycastHit2D osuma = Physics2D.Raycast(alaPiste, Vector2.down, etaisyys, LayerMask.GetMask("Tiililayer"));

        // Debug-viiva näkyviin Scene-näkymässä
        Debug.DrawRay(alaPiste, Vector2.down * etaisyys, Color.red);

        return osuma.collider == null;
    }

    public bool OnkoAutonRungonAllaTiili(GameObject autoRunko, float etaisyys = 0.5f)
    {
        Rigidbody2D rb = autoRunko.GetComponent<Rigidbody2D>();
        Collider2D col = autoRunko.GetComponent<Collider2D>();

        if (rb == null || col == null)
        {
            Debug.LogWarning("Autolla ei ole Rigidbody2D:tä tai Collider2D:tä.");
            return false;
        }

        // Kulma radiaaneina
        float kulmaRad = (rb.rotation - 90f) * Mathf.Deg2Rad;

        // Lasketaan suunta 90 astetta auton rotaatiosta alaspäin (eli "auton alapuolelle")
        Vector2 suunta = new Vector2(Mathf.Cos(kulmaRad), Mathf.Sin(kulmaRad)).normalized;

        // Raycastin lähtöpiste (auton colliderin keskipiste)
        //Vector2 alkuPiste = col.bounds.center;
        Vector2 alkuPiste = new Vector2(col.bounds.max.x, col.bounds.max.y);

        // Raycast
        RaycastHit2D osuma = Physics2D.Raycast(alkuPiste, suunta, etaisyys, LayerMask.GetMask("Tiililayer"));

        // Debug-viiva
        Debug.DrawRay(alkuPiste, suunta * etaisyys, osuma.collider ? Color.blue : Color.white);

        bool oliko= osuma.collider != null;
        if (oliko)
        {
            return true;
        }
        alkuPiste = new Vector2(col.bounds.max.x*0.8f, col.bounds.max.y);
        osuma = Physics2D.Raycast(alkuPiste, suunta, etaisyys, LayerMask.GetMask("Tiililayer"));

        // Debug-viiva
        Debug.DrawRay(alkuPiste, suunta * etaisyys, osuma.collider ? Color.blue : Color.white);
        return osuma.collider != null;


    }

    public bool KoskettaakoRunkoMaata()
    {
        BoxCollider2D[] bb = GetComponents<BoxCollider2D>();
        foreach(BoxCollider2D box in bb)
        {
            Vector2 position = box.bounds.center;
            Vector2 size = box.bounds.size;
            float angle = 0f; // or box.transform.eulerAngles.z if rotated
            Collider2D[]  cc=Physics2D.OverlapBoxAll(position, size, angle);
            foreach(Collider2D c in cc)
            {
                if (c.tag.Contains("tiili"))
                {
                    return true;
                }
            }
        }

        return false;

    }


    public bool OnkoTiiliVasemmalla(GameObject rengasObj, float etaisyys = 0.8f)
    {
        Collider2D col = rengasObj.GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogWarning("Rengasobjektilla ei ole Collider2D:ta.");
            return false;
        }

        // Lasketaan renkaan alareuna colliderin boundsien mukaan
        Vector2 alaPiste = new Vector2(col.bounds.min.x, col.bounds.max.y);

        // Raycast alaspäin
        RaycastHit2D osuma = Physics2D.Raycast(alaPiste, Vector2.left, etaisyys, LayerMask.GetMask("Tiililayer"));

        // Debug-viiva näkyviin Scene-näkymässä
        Debug.DrawRay(alaPiste, Vector2.left * etaisyys, Color.red);

        return osuma.collider != null;
    }


    public bool OnkoTiiliOikealla(GameObject rengasObj, float etaisyys = 0.8f)
    {
        Collider2D col = rengasObj.GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogWarning("Rengasobjektilla ei ole Collider2D:ta.");
            return false;
        }

        // Lasketaan renkaan alareuna colliderin boundsien mukaan
        Vector2 alaPiste = new Vector2(col.bounds.min.x, col.bounds.max.y);

        // Raycast alaspäin
        RaycastHit2D osuma = Physics2D.Raycast(alaPiste, Vector2.right, etaisyys, LayerMask.GetMask("Tiililayer"));

        // Debug-viiva näkyviin Scene-näkymässä
        Debug.DrawRay(alaPiste, Vector2.right * etaisyys, Color.green);

        return osuma.collider != null;
    }



    public float rajaKulma = 30f; // esim. 30 astetta

    private bool OnkoKeulaYlhaallaRajaAsteenVerran()
    {
        float kulma = rb.rotation; // Rotation in degrees
        kulma = NormalizeKulma(kulma); // Muutetaan kulma välille [-180, 180]
       // Debug.Log("keulakulma=" + kulma);
        return kulma > rajaKulma;
    }

    // Apufunktio kulman normalisoimiseksi
    private float NormalizeKulma(float kulma)
    {
        kulma %= 360f;
        if (kulma > 180f) kulma -= 360f;
        return kulma;
    }


    private bool OllaankoYlosAlaisin(float sallittuPoikkeama = 30f)
    {
        float kulma = NormalizeKulma(rb.rotation); // -180 … 180

        // Tarkistetaan onko kulma lähellä ±180 astetta (eli ylösalaisin)
        return Mathf.Abs(Mathf.DeltaAngle(kulma, 180f)) < sallittuPoikkeama;
    }

    public float tuhoKynnys = 3f;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //@todoo savuttamaan kun ammus osuu
        //ja kun ammus osuu niin vähentääkin moottorivoimaa
        //tai sitten tekee childcollider reporterin ominaisuudeksi tämän
        //tai sitten sinne ammuksien collidereihin

        Debug.Log("MUISTA KOOOOODATAAAAA");
        if (true)
            return;

        // Haetaan törmäyksen kokonaisimpulssi
        float tormaysvoima = collision.relativeVelocity.magnitude * collision.rigidbody.mass;

        Debug.Log("Törmäysvoima: " + tormaysvoima);

        // Jos voima ylittää kynnyksen, tuhotaan objekti
        if (tormaysvoima > tuhoKynnys)
        {
            ChildColliderReporter c=
            GetComponent<ChildColliderReporter>();
            if (c!=null)
            {
                if (c.explosion)
                {
                    /*
                    GameObject instanssisavu = Instantiate(c.explosion, transform.position, Quaternion.identity);

                    Destroy(instanssisavu, 1.0f);
                    */
                    
                    GameObject instanssisavu = ObjectPoolManager.Instance.GetFromPool(c.explosion, transform.position, Quaternion.identity);

                    ObjectPoolManager.Instance.ReturnToPool(c.explosion, instanssisavu, 1.0f);
                }
            }

            Destroy(gameObject);

        }
    }


    bool OnSlope()
    {
        float rayLength = 0.1f;
        Vector2 origin = tankkiPiippuController.gameObject.transform.position;

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, rayLength);

        if (hit.collider != null)
        {
            // Normaalivektori kertoo pinnan suunnan
            Vector2 normal = hit.normal;

            // Jos normaalin suunta poikkeaa ylöspäin osoittavasta vektorista, ollaan kaltevalla pinnalla
            return Vector2.Angle(normal, Vector2.up) > 0.1f;
        }

        return false;
    }
}
