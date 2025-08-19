using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LisaelamaController : BaseController
{
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        glowColor = baseColor * colorkerto; // subtle glow
        ad = PalautaAudioplayerController();
    }
    public float pulseSpeed = 2f;
    private SpriteRenderer sr;


    public Color baseColor = Color.white;
    private Color glowColor;

    public float colorkerto = 1.2f;
    public void Update()
    {
        glowColor = baseColor * colorkerto; // subtle glow
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f; // range 0ñ1
        t = Mathf.Lerp(0f, 0.3f, t); // now a subtle glow factor (0ñ0.3)
        sr.color = Color.Lerp(baseColor, glowColor, t);
    }

    //public Color flashElamaLisaa = Color.black;

    public AudioplayerController ad;


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
            RajaytaSprite(gameObject, 4, 4, 3.0f, 2f);
            //Destroy(this.gameObject);
            BaseDestroy(gameObject);

            if (ad!=null)
            {
                ad.ElamaLisaaPlay();
            }
           

            //AlusController myScript = col.gameObject.GetComponent<AlusController>();
            //if (myScript != null)
            //{
                //myScript.ElamaKeratty();
                GameManager.Instance.ElamaKeratty();

            /*
            }
            else
            {
                Debug.Log("aluscontrollerin skripti oli null");
            }
            */

        }
    }

}
