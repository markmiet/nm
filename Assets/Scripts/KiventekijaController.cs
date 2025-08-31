using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif
public class KiventekijaController : BaseController
{


    public bool vaihdakivenvelocitya = false;
    public Vector2 vaihdakivenVelocityksiTama = Vector2.zero;

    public bool vaihdakiventorque = false;
    public float vaihdakiventorqueksitama = 0.0f;


   // private GameObject alus;
    public int maaraJokaTehdaan = 10;
    private int pallojennykymaara = 0;
    private float viimeisinx = 0.0f;
    public float xsuunnanLisays = 1.5f;
    public float viiveVali = 0.05f; // Optional: delay between spawns

    public Color gizmoColor = Color.green;
    public float gizmoRadius = 0.5f;

    public GameObject[] kivet;
    private bool pallottehty = false;
    private bool coroutineKaynnissa = false;

   // public Vector2 alabocenter = new Vector2(0, 0);
    public Vector2 boxsizealhaalla = new Vector2(2, 2);
    public LayerMask layerMask;

    public bool huomioivaikeustaso = false;


    

    void Update()
    {
        if (IsGoingToBeDestroyed()) {
            return;
        }

        bool nakyvissa = IsGameObjectVisible();
        if (!nakyvissa) return;

        if (!pallottehty && !coroutineKaynnissa)
        {
            StartCoroutine(LuoKivetCoroutine());
        }

        if (nakyvissa && pallottehty)
        {
            SpriteRenderer[] ss = GetComponentsInChildren<SpriteRenderer>();
            if (ss != null)
            {
                foreach (SpriteRenderer s in ss)
                {
                    if (s.isVisible)
                        return;
                }
            }
            Destroy(gameObject);
        }
    }


    public float ysuunnassaRandomisointiVali = 1.0f;
    public float xsuunnassaRandomisointiVali = 1.0f;


    public float xsuunnanlisaysTamaSiksiEttaLuodaanKiviHiukanKameranOikeallePuolen = 1.0f;

    IEnumerator LuoKivetCoroutine()
    {
        coroutineKaynnissa = true;
        int maara = maaraJokaTehdaan;
        if (huomioivaikeustaso)
        {
            maara = Mathf.RoundToInt(maaraJokaTehdaan * GameManager.Instance.PalautaDifficulty());
        }

        while (pallojennykymaara < maara)
        {
            float yarvo =  Random.Range(-1*ysuunnassaRandomisointiVali, ysuunnassaRandomisointiVali);
            float xrandomin = Random.Range(0, xsuunnassaRandomisointiVali);

            float seuraavaX = transform.position.x + viimeisinx + xsuunnanlisaysTamaSiksiEttaLuodaanKiviHiukanKameranOikeallePuolen;

            float yy = transform.position.y + yarvo;

            if (voikoInstantioida(seuraavaX, yy))
            {
               // Debug.Log("voidaan " + seuraavaX + " " + yarvo);
                Vector3 v3 = new Vector3(seuraavaX, yy, 0);
                GameObject instanssi = Instantiate(PalautaGameObjectRandomina(), v3, Quaternion.identity);
                instanssi.name = instanssi.name +"_"+ pallojennykymaara;

          
                    KiviController[] rbs =

instanssi.GetComponentsInChildren<KiviController>();


                    foreach(KiviController k in rbs)
                    {
                        if (vaihdakivenvelocitya)
                        {
                            k.velocity = vaihdakivenVelocityksiTama;
                        }
                        if (vaihdakiventorque)
                        {
                            k.kiertomaara = vaihdakiventorqueksitama;
                        }
                    }


                    /*
                     *     public bool vaihdakiventorque = false;
    public float vaihdakiventorqueksitama = 0.0f;
                    */

              



                pallojennykymaara++;
                yield return new WaitForSeconds(viiveVali); // optional delay
            }

            viimeisinx += xsuunnanLisays + xrandomin;
            yield return null; // wait one frame before trying again
        }

        pallottehty = true;
        viimeisinx = 0.0f;
        coroutineKaynnissa = false;
    }

    private GameObject PalautaGameObjectRandomina()
    {
        int satunnainenIndeksi = Random.Range(0, kivet.Length);
        return kivet[satunnainenIndeksi];
    }

    public bool voikoInstantioida(float pos, float posy)
    {
        uusi = new Vector2( pos, posy);
        //tulos = !onkoTagiaBoxissaTransformPositionArvoonLisataanBoxLocation("vihollinen", boxsizealhaalla, uusi, layerMask);


        tulos=!onkoTagiaBoxissaAlakaytaTransformia("vihollinen", boxsizealhaalla, uusi);


        //  public bool onkoTagiaBoxissa(string name, Vector2 boxsize, Vector2 boxlocation, LayerMask layerMask)
/*
#if UNITY_EDITOR
        // Piirretään laatikon ääriviivat Scene view:ssä
        Vector3 center = new Vector3(uusi.x, uusi.y, 0f);
        Vector3 half = (Vector3)boxsizealhaalla * 0.5f;

        Color c = tulos ? Color.green : Color.red;

        Vector3 topLeft = center + new Vector3(-half.x, half.y, 0f);
        Vector3 topRight = center + new Vector3(half.x, half.y, 0f);
        Vector3 bottomLeft = center + new Vector3(-half.x, -half.y, 0f);
        Vector3 bottomRight = center + new Vector3(half.x, -half.y, 0f);

        Debug.DrawLine(topLeft, topRight, c, 0f, false);
        Debug.DrawLine(topRight, bottomRight, c, 0f, false);
        Debug.DrawLine(bottomRight, bottomLeft, c, 0f, false);
        Debug.DrawLine(bottomLeft, topLeft, c, 0f, false);
#endif
        */

        return tulos;
    }
    Vector2 uusi = Vector2.zero;
    bool tulos = false;

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR

        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontSize = 10;
        labelStyle.normal.textColor = Color.red;
   
        gizmoColor = Color.white;

        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius);

        Handles.Label(transform.position + Vector3.up * 0.2f, $"kivi {gameObject.name}", labelStyle);


        //uusi = new Vector2(transform.position.x, transform.position.y);
        //tulos = !onkoTagiaBoxissaTransformPositionArvoonLisataanBoxLocation("vihollinen", boxsizealhaalla, uusi, layerMask);
        /*
                Vector2 vv = transform.position;
                vv.x += xsuunnanlisaysTamaSiksiEttaLuodaanKiviHiukanKameranOikeallePuolen;

               bool tuloslocal = onkoTagiaBoxissaAlakaytaTransformia("vihollinen", boxsizealhaalla,vv);


                Handles.Label(transform.position + Vector3.up * 0.2f, $"kivi {gameObject.name} tulos={tuloslocal}", labelStyle);


                Vector3 center = vv;
                Vector3 half = (Vector3)boxsizealhaalla * 0.5f;

                gizmoColor = tuloslocal ? Color.white : Color.blue;
                Gizmos.color = gizmoColor;
                Vector3 topLeft = center + new Vector3(-half.x, half.y, 0f);
                Vector3 topRight = center + new Vector3(half.x, half.y, 0f);
                Vector3 bottomLeft = center + new Vector3(-half.x, -half.y, 0f);
                Vector3 bottomRight = center + new Vector3(half.x, -half.y, 0f);

                Gizmos.DrawLine(topLeft, topRight);
                Gizmos.DrawLine(topRight, bottomRight);
                Gizmos.DrawLine(bottomRight, bottomLeft);

                Gizmos.DrawLine(bottomLeft, topLeft);
        */
#endif
    }

    public bool voikoInstantioidavana(float pos, float posy)
    {
        Vector2 uusi = new Vector2( pos,  posy);
        return !onkoTagiaBoxissaTransformPositionArvoonLisataanBoxLocation("vihollinen", boxsizealhaalla, uusi, layerMask);
    }
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)transform.position + alabocenter, boxsizealhaalla);
    }
    */

    /*
    bool IsObjectInView(Camera cam, Transform objTransform)
    {
        Vector3 viewportPoint = cam.WorldToViewportPoint(objTransform.position);
        return viewportPoint.x > 0 && viewportPoint.x < 1 &&
               viewportPoint.y > 0 && viewportPoint.y < 1 &&
               viewportPoint.z > 0;
    }
    */
                }
