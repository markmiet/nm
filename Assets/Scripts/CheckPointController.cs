using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : BaseController
{
    public bool asetettu = false;
    public float x;
    public bool makeflash = false;
    public float flashtime = 2.0f;
    // Start is called before the first frame update

    private float flashTimer = 0.0f;
    private bool isFlashing = false;
    private SpriteRenderer rend;
    public AudioSource checkpointPlay;


    void Start()
    {
        x = transform.position.x;
        // asetettu = true;
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        bool more = IsMoreThanHalfOnLeft();
        bool visi = IsGameObjectVisible();
        Debug.Log(gameObject.name + " more=" + more + " visi=" + visi+" asetettu="+asetettu);
        */
        if (!asetettu && IsMoreThanHalfOnLeft() && IsGameObjectVisible())
        {
            GameManager.Instance.checkPoint.GetComponent<CheckPointController>().x = transform.position.x;

            /*
            if (message!=null)
            {
                //GameManager.Instance.LisaaTeksti(message,messagetime);

                string[] rows = message.Split('\n');
                float delay = 0.0f;
                foreach (var row in rows)
                {
                    GameManager.Instance.LisaaTekstiViiveella(row, delay, messagedestroytime);
                    delay += delaybetweenlines;
                }
               
            }
        */
           //eli tähän soundi ja sitten flashi
           asetettu = true;
            if (makeflash)
            {
                isFlashing = true;
                flashTimer = flashtime;
            }
            if (checkpointPlay != null)
            {
                checkpointPlay.Play();
            }
        }
        if (isFlashing)
        {
            flashTimer -= Time.deltaTime;

            // Toggle renderer every 0.1 seconds (can adjust)
            float blinkSpeed = 0.1f;
            bool visible = Mathf.FloorToInt(flashTimer / blinkSpeed) % 2 == 0;

            if (rend != null)
                rend.enabled = visible;

            if (flashTimer <= 0)
            {
                if (rend != null)
                    rend.enabled = true; // Make sure it's visible at the end

                isFlashing = false;

                //RajaytaSprite(gameObject, 4, 4, 0.5f, 2.0f);
                RajaytaSprite(gameObject, 6, 6, 1.0f, 2.0f,
               1.0f, false, 1.0f, false, 0.2f,
                   -1.5f, false, null);


                Destroy(gameObject);
            }

        }
    }



}
