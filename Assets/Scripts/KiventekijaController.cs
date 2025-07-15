using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiventekijaController : BaseController
{
    private GameObject alus;
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

    public Vector2 alabocenter = new Vector2(0, 0);
    public Vector2 boxsizealhaalla = new Vector2(2, 2);
    public LayerMask layerMask;

    void Update()
    {
        bool nakyvissa = IsObjectInView(Camera.main, transform);
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


    IEnumerator LuoKivetCoroutine()
    {
        coroutineKaynnissa = true;
        while (pallojennykymaara < maaraJokaTehdaan)
        {
            float yarvo =  Random.Range(-1*ysuunnassaRandomisointiVali, ysuunnassaRandomisointiVali);
            float xrandomin = Random.Range(0, xsuunnassaRandomisointiVali);

            float seuraavaX = transform.position.x + viimeisinx;

            if (voikoInstantioida(seuraavaX, yarvo))
            {
                Vector3 v3 = new Vector3(seuraavaX, transform.position.y + yarvo, 0);
                GameObject instanssi = Instantiate(PalautaGameObjectRandomina(), v3, Quaternion.identity);
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
        Vector2 uusi = new Vector2(alabocenter.x + pos, alabocenter.y + posy);
        return !onkoTagiaBoxissa("vihollinen", boxsizealhaalla, uusi, layerMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)transform.position + alabocenter, boxsizealhaalla);
    }

    bool IsObjectInView(Camera cam, Transform objTransform)
    {
        Vector3 viewportPoint = cam.WorldToViewportPoint(objTransform.position);
        return viewportPoint.x > 0 && viewportPoint.x < 1 &&
               viewportPoint.y > 0 && viewportPoint.y < 1 &&
               viewportPoint.z > 0;
    }
}
