using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGunController : BaseController
{

    public GameObject extraGunLauncher;


    private AudioplayerController ad;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool onkoAlukseenJoTormatty = false;

    public void OnTriggerEnter2D(Collider2D col)
    {

        if (IsGoingToBeDestroyed())
        {
            return;
        }



        //  Debug.Log($"This object's collider: {col.collider.name}");
        //  Debug.Log($"Other object's collider: {col.otherCollider.name}");
        if (!onkoAlukseenJoTormatty && col.CompareTag("alustag"))
        {
            onkoAlukseenJoTormatty = true;
            GetComponent<Collider2D>().enabled = false;//t‰m‰n pit‰isi riitt‰‰
            RajaytaSprite(gameObject, 3, 3, 2.0f, 1f);
            //Destroy(this.gameObject);
            
            if (ad!=null)
            {
                ad.BonusPlay();
            }
            

            AlusController myScript = col.gameObject.GetComponent<AlusController>();
            if (myScript != null)
            {
                GameObject uusiaselaukaisija= Instantiate(extraGunLauncher, col.gameObject.transform.position, Quaternion.identity);
                myScript.extraGun= extraGunLauncher;
                //ExtraGunLauncher

            }
            BaseDestroy();
        }
    }

}
