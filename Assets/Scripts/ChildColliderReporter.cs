using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildColliderReporter : BaseController 
{
    private HitCounter parent;
    public GameObject explosion;
    private HitCounter tamaHitCounter;

    public GameObject osumanSavu;
    public float osumanSavunKesto = 2.0f;



    public GameObject osumanLiekki;
    public float prosenttimaarakokonaisuudestaJokaVaaditaaanLiekkiin = 90.0f;

    public bool rajoitaOikealleMenemista = false;
    public float oikeallemenemisenVelocityrajaarvo = 3.0f;
    public float rajoituskerto = 0.1f;
    private Rigidbody2D rb;

    public float aiheutavoimaaRigidbodyyn = 0.0f;


    public int tamanchildcolliderreporterinOsumienMaara = 0;

    public bool saadadissolveamountverrattunaOsumiinKaytaVainTamanOsumia = false;


    private bool PitaakoLiekinTulla()
    {
        float prossa = PalautaHittienMaaraKokonaisuudestaProsentteina();
        return prossa >= prosenttimaarakokonaisuudestaJokaVaaditaaanLiekkiin;
    }

    private bool PitaakoIsommanLiekinTulla()
    {
        float loppuosa = 100 - prosenttimaarakokonaisuudestaJokaVaaditaaanLiekkiin;
        //loppuosa=20%
        //otetaan puolet siit‰
        float lisaosa = loppuosa / 2.0f;


        float prossa = PalautaHittienMaaraKokonaisuudestaProsentteina();
        return prossa >= prosenttimaarakokonaisuudestaJokaVaaditaaanLiekkiin+lisaosa;
    }


    private DissolveMatController p;
    private float dissolveoriginal;
    protected virtual void Start()
    {
        parent = GetComponentInParent<HitCounter>();
        tamaHitCounter = GetComponent<HitCounter>();
        rb = GetComponent<Rigidbody2D>();
        
        p = GetComponent<DissolveMatController>();
        if (p != null)
        {
            dissolveoriginal = p.dissolveamount;
        }

    }


    public void FixedUpdate()
    {
        if (IsGoingToBeDestroyed())
        {
            return;
        }

         if (rajoitaOikealleMenemista && rb!=null)
        {
            // Tarkistetaan onko liike oikealle
            if (rb.velocity.x > oikeallemenemisenVelocityrajaarvo)
            {
                if (Camera.main.GetComponent<Kamera>().skrollimaara>0.0f)
                {
                    // Estet‰‰n oikealle meno asettamalla x-velosyyti nollaksi
                    rb.velocity = new Vector2(rb.velocity.x* rajoituskerto, rb.velocity.y);
                }
            }
        }
    }
    /*
    void OnCollisionEnter2D(Collision2D collision)
    {
        parent?.RegisterHit();
    }
    */

    private void AiheutaVoimaa(GameObject go)
    {
        if (rb!=null && aiheutavoimaaRigidbodyyn > 0.0f)
        {
            Rigidbody2D r =
            go.GetComponent<Rigidbody2D>();
            if (r!=null)
            {
                ImpactTransfer2D ii = go.GetComponent<ImpactTransfer2D>();
                if (ii!=null)
                {
                    //@todoo ei toimi oikein
                    //koska se suunta just k‰‰ntyy v‰‰rinp‰in...
                    //pit‰is tehd‰ impact 
                    Vector2 suunta = ii.lastVelocity.normalized;
                    rb.AddForce(suunta * aiheutavoimaaRigidbodyyn, ForceMode2D.Impulse);
                }
            }
            /*
            Vector2 alku = piipunToinenPaa.transform.position;
            Vector2 loppu = piipunpaaJohonGameObjectInstantioidaan.transform.position;

            Vector2 suunta = alku - loppu;

           rb.AddForce(suunta * aiheutavoimaaRigidbodyyn, ForceMode2D.Impulse);
            */
        }
    }


    public bool RegisterHit(Vector2 contactPoint,GameObject go)
    {

        if (IsGoingToBeDestroyed())
        {
            return true;
        }

        tamanchildcolliderreporterinOsumienMaara++;

        bool ret1 = false;
        if (parent!=null)
        {
          ret1=parent.RegisterHit();
        }
        bool ret2 = false;
        if (tamaHitCounter != null)
        {
            ret2=tamaHitCounter.RegisterHit();
        }


        if (explosion != null)
        {
            Instantiate(explosion, contactPoint, Quaternion.identity);
        }

        //pit‰iskˆ savun ja liekin koon skaalautua ihan suoraan osumien m‰‰r‰n mukaan...
        //t‰st‰ vois tehd‰ parametrit ja k‰ytt‰‰ jossain toisella eri arvoja...
        if (osumanSavu != null)
        {
            GameObject savu = Instantiate(osumanSavu, contactPoint, Quaternion.identity);
            savu.transform.SetParent(transform, worldPositionStays: true);
            Destroy(savu, osumanSavunKesto);
        }

        if (osumanLiekki != null && PitaakoIsommanLiekinTulla())
        {
            GameObject liekki = Instantiate(osumanLiekki, contactPoint, Quaternion.identity);
            Vector3 currentScale = liekki.transform.localScale;
            currentScale.x *= 1.5f;
            currentScale.y *= 1.5f;
            liekki.transform.localScale = currentScale;

            liekki.transform.SetParent(transform, worldPositionStays: true);
            Destroy(liekki, osumanSavunKesto); // HUOM: k‰ytet‰‰n samaa arvoa
        }
        
        else if (osumanLiekki != null && PitaakoLiekinTulla())
        {
            GameObject liekki = Instantiate(osumanLiekki, contactPoint, Quaternion.identity);
            liekki.transform.SetParent(transform, worldPositionStays: true);
            Destroy(liekki, osumanSavunKesto); // HUOM: k‰ytet‰‰n samaa arvoa
        }
        AiheutaVoimaa(go);
        SaadaDissolveAmountVerrattunaOsumiin();
        return ret1 || ret2;
        //80 alkaa palamaan
        //ent‰s joku 95% loput isot liekit viel‰ :)

    }


    public int PalautaVaadittuHittienMaara()
    {
        if (parent != null)
        {
            return parent.hitThreshold;
        }
        else if (tamaHitCounter != null)
        {
            return tamaHitCounter.hitThreshold;
        }
        return 1;
    }

    public int PalautaNykyinenOsumienMaara()
    {
        if (parent != null)
        {
            return parent.hitCount;
        }
        else if (tamaHitCounter != null)
        {
            return tamaHitCounter.hitCount;
        }
        return 1;
    }


    public float PalautaHittienMaaraKokonaisuudestaProsentteina()
    {
        int threadshold = PalautaVaadittuHittienMaara();
        int osumienmaara = PalautaNykyinenOsumienMaara();

        float prossa = (float)osumienmaara / (float)threadshold;

        return prossa * 100.0f;

    }
    
    private void SaadaDissolveAmountVerrattunaOsumiin()
    {
        if (p!=null && saadadissolveamountverrattunaOsumiinKaytaVainTamanOsumia)
        {
            int threadshold = PalautaVaadittuHittienMaara();


            float prossat = (float)tamanchildcolliderreporterinOsumienMaara / (float) threadshold;
            float uusiarvo = prossat * dissolveoriginal;
            p.dissolveamount = uusiarvo;
        }
    }
    
}
