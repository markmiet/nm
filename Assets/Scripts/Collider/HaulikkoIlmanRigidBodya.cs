using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulikkoIlmanRigidBodya : BaseController
{

    private GameObject alus;
    public bool ammuvainjosvapaalinja = true;
    public GameObject ammus;
    public float ampumisenvoimakkuus = 1.0f;
    public GameObject shooterposition;
    public GameObject piipuntoinenpaa;
    public GameObject ammussavu;
    public float ammussavukesto = 0.5f;
    public float ampumissykli = 0.1f;
    private float ampumislaskuri = 0.0f;



    public SkeletonAiming2DIK skeletonAiming2DIK;
    public PotLintuRatKasiTahtain potLintuRatKasiTahtain;

    // Start is called before the first frame update
    void Start()
    {
        alus = PalautaAlus();
    }
    /*
    public bool OllaankoKamerassa()
    {
        if (objektijonkapitaaOllaKamerassa==null || !pitaakollaKamerassaJottaAmpuu)
        {
            return true;
        }
        OnkoKameranOikeallaPuolella()
    }
    */


    private bool NakeekoSilmat()
    {
        if (skeletonAiming2DIK==null)
        {
            if (potLintuRatKasiTahtain!=null)
            {
                return potLintuRatKasiTahtain.CanEnemySeePlayer();
            }
            return true;
        }
        return skeletonAiming2DIK != null && skeletonAiming2DIK.canSeePlayer;
    }


    


    // Update is called once per frame


    void Update()
    {
        if (alus!=null && ammus!=null)
        {
            ampumislaskuri += Time.deltaTime;
            if (ampumislaskuri>= ampumissykli)
            {
                bool vapaata = true;
                if (ammuvainjosvapaalinja)
                {
                    vapaata = NakeekoSilmat();
                }
                if (vapaata)
                {


                    Vector2 v2 =
        palautaAmmukselleVelocityVectorKaytaPiippua(alus, ampumisenvoimakkuus, shooterposition.transform.position, piipuntoinenpaa.transform.position);

                    GameObject ins = Instantiate(ammus, shooterposition.transform.position, Quaternion.identity);
                    IgnoraaCollisiotVihollistenValilla(ins, gameObject);

                    ins.GetComponent<Rigidbody2D>().velocity = v2;

                    if (ammussavu != null && ammussavukesto > 0.0f)
                    {
                        GameObject savu =
                            Instantiate(ammussavu, shooterposition.transform.position, Quaternion.identity);
                        Destroy(savu, ammussavukesto);
                    }
                    ampumislaskuri = 0;
                }
            }
        }
    }
}
