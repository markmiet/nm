using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class CameraInfoController : BaseController
{

    Camera main;
    Kamera kamera;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    private AudioplayerController ad;
    void Start()
    {
        main = Camera.main;
        kamera = main.GetComponent<Kamera>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteRenderer.enabled = false;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr!=null)
        {
            Color c = sr.color;
            c.a = 0f; // alpha = 0 (fully transparent)
            sr.color = Color.clear;
        }
        

        

        ad = PalautaAudioplayerController();

       // scrollspeedx = scrollspeedx * 10;
    }




    private bool stoppistartattu = false;
    private float stoppilaskuri = 0.0f;
    private void HoidaStoppi()
    {

        if (stop)
        {
            stoppilaskuri += Time.deltaTime;
           
        }
    }

    public float PalautaOdotusAika()
    {
        if (stop)
        {
            return stoptime - stoppilaskuri;
        }
        return -1;
    }


    // Update is called once per frame
    void Update()
    {
        //bool onko = IsObjectInOrthographicView(transform, main);

        bool vasen= OnkoKameranVasemmallaPuolella(transform, main);
        if (vasen)
        {
            if (main.GetComponent<Kamera>()!=null &&
                main.GetComponent<Kamera>().cameraInfo!=null &&
                main.GetComponent<Kamera>().cameraInfo==gameObject
                )
            {

            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        bool onko = IsObjectInCameraView(transform.position);

        if (onko)
        {
            HoidaStoppi();

            GameObject olemassa =
            kamera.cameraInfo;
            if (olemassa != null)
            {
                bool onkotoinen = IsObjectInCameraView(olemassa.transform.position);
                if (!onkotoinen)
                {
                    kamera.cameraInfo = this.gameObject;
                }
                else if (transform.position.x > olemassa.transform.position.x)
                {
                    kamera.cameraInfo = this.gameObject;
                }
            }
            else
            {
                kamera.cameraInfo = this.gameObject;
            }
            if (!taustamusavaihdettu)
            {
                if (taustamusa != null)
                {
                    ad.TaustaMusiikkiStop();
                    ad.taustamusiikki = taustamusa;
                    ad.TaustaMusiikkiPlay();
                }

                taustamusavaihdettu = true;
            }
            if (aaniefekti != null && !aaniefektisoitettu)
            {

                aaniefektilaskuri += Time.deltaTime;

                if (aaniefektilaskuri >= aaniefektinsoittoaika)
                {
                    aaniefekti.Play();
                    aaniefektisoitettu = true;
                    aaniefektilaskuri = 0;
                }
            }
        }
    }

    private bool taustamusavaihdettu = false;

    private bool aaniefektisoitettu = false;
    public float aaniefektinsoittoaika = 10.0f;

    private float aaniefektilaskuri = 0.0f;


    public float scrollspeedx = 3.0f;
    public float scrollspeedy = 3.0f;

    public float rotationz = 0.0f;
    public bool stop;
    public float stoptime;


    public bool generoilisaavihollisia = false;
    public GameObject[] vihollisetjokageneroidaan;
    public GameObject[] vihollisetJoidenOlemassaOloTutkitaan;


    public float generointivali = 5.0f;
    public float generointilaskuri = 0.0f;
    public bool onkogeneroitukoskaan = false;

    public int vihollistenmaarajokageneroidaan = 1;
    public int generoitujenvihollistenmaara = 0;

    public bool naytasavu = false;
    public bool naytataustat = true;
    public GameObject savu;
    public GameObject tausta1;
    public GameObject tausta2;
    public GameObject tausta3;

    public string lisateksti;

    public bool disablealuksenammukset = false;

    private bool IsObjectInOrthographicView(Transform target, Camera camera)
    {
        Vector3 position = camera.WorldToScreenPoint(target.position);
        return position.x >= 0 && position.x <= Screen.width &&
               position.y >= 0 && position.y <= Screen.height &&
               position.z > 0; // Ensure it's in front of the camera
    }

    private bool IsObjectInCameraView(Vector3 worldPosition)
    {
        Vector3 viewportPosition = main.WorldToViewportPoint(worldPosition);
        bool viewportissa = viewportPosition.x > 0 && viewportPosition.x < 1 &&
               viewportPosition.y > 0 && viewportPosition.y < 1 &&
               viewportPosition.z > 0; // Ensure the object is in front of the camera


        float x = worldPosition.x;
        float kamerax = main.transform.position.x;
        bool onkonakyvissa = spriteRenderer.isVisible;
        bool kameraxylix = kamerax >= x;

        if (viewportissa && onkonakyvissa && kameraxylix)
        {
            return true;
        }
        return false;
    }


    public bool OnkoKameranVasemmallaPuolella(Transform t, Camera cam)
    {
        // Muutetaan maailman sijainti kameran viewport-koordinaatteihin
        // (0,0) = vasen-alakulma, (1,1) = oikea-yläkulma
        Vector3 viewportPos = cam.WorldToViewportPoint(t.position);

        // Jos x < 0, niin objekti on vasemmalla kameran näkymästä
        return viewportPos.x < 0f;
    }

    public AudioSource taustamusa;

    public AudioSource aaniefekti;


    private void OnDrawGizmos()
    {
#if UNITY_EDITOR

        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontSize = 10;
        labelStyle.normal.textColor = Color.red;



        Handles.Label(transform.position + Vector3.up * 0.2f, $"{gameObject.name}", labelStyle);


        //uusi = new Vector2(transform.position.x, transform.position.y);
        //tulos = !onkoTagiaBoxissaTransformPositionArvoonLisataanBoxLocation("vihollinen", boxsizealhaalla, uusi, layerMask);

#endif
    }


}
