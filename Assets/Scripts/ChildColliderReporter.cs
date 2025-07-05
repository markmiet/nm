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


    private bool PitaakoLiekinTulla()
    {
        float prossa = PalautaHittienMaaraKokonaisuudestaProsentteina();
        return prossa >= prosenttimaarakokonaisuudestaJokaVaaditaaanLiekkiin;
    }

    protected virtual void Start()
    {
        parent = GetComponentInParent<HitCounter>();
        tamaHitCounter = GetComponent<HitCounter>();
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
        if (explosion!=null)
        {
            GameObject instanssi2 = Instantiate(explosion, contactPoint, Quaternion.identity);
        }
        if (osumanSavu != null)
        {
            
            
            //GameObject instanssi2 = Instantiate(osumanSavu, contactPoint, Quaternion.identity);

           // GameObject instanssi2 = Instantiate(osumanSavu, position, rotation, gameObject.transform);
           // Destroy(instanssi2, osumanSavunKesto);

            GameObject instance = Instantiate(osumanSavu, contactPoint, Quaternion.identity);
            instance.transform.SetParent(gameObject.transform, worldPositionStays: true);
            Destroy(instance, osumanSavunKesto);
        }
        if (osumanLiekki!=null)
        {
            if (PitaakoLiekinTulla())
            {
                GameObject instance = Instantiate(osumanLiekki, contactPoint, Quaternion.identity);
                instance.transform.SetParent(gameObject.transform, worldPositionStays: true);
                Destroy(instance, osumanSavunKesto);
            }
        }
    }

    public int PalautaHittienMaara()
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

    public int PalautaOsumienMaara()
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
        int threadshold = PalautaHittienMaara();
        int osumienmaara = PalautaOsumienMaara();

        float prossa = (float)osumienmaara / (float)threadshold;

        return prossa * 100.0f;

    }


}
