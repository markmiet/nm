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



    protected virtual void Start()
    {
        parent = GetComponentInParent<HitCounter>();
        tamaHitCounter = GetComponent<HitCounter>();
        rb = GetComponent<Rigidbody2D>();
    }


    public void FixedUpdate()
    {
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

    public void RegisterHit(Vector2 contactPoint)
    {
        parent?.RegisterHit();
        tamaHitCounter?.RegisterHit();

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


}
