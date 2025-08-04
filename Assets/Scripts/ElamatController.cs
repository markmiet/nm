using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
public class ElamatController : MonoBehaviour
{
    // Start is called before the first frame update

    // public GameObject[] elamat;



    public Sprite elamakuva;
    void Awake()
    {
        elamatversio2 = new GameObject[50];


        for (int i = 0; i < elamatversio2.Length; i++)
        {
            GameObject go = new GameObject("MyUIElement" + i, typeof(RectTransform));

            RectTransform r = go.GetComponent<RectTransform>();
            r.sizeDelta = new Vector2(100, 100);
            r.localScale = new Vector3(0.35f, 0.35f, 0.0f);
            // r.anchoredPosition = new Vector2(i * 60, 0); // Spaced 40 units apart
            elamatversio2[i] = go;
            Image image = go.AddComponent<Image>();
            image.sprite = elamakuva;
            go.transform.SetParent(transform, false);
            r.anchoredPosition = new Vector2(i * 60, 0);




        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    private int edellinenmaara = 0;
    public GameObject[] elamatversio2;
    public void SetElamienMaaraold(int elamatmaara)
    {
        //@todoo eli elamatversio2 pitäisi pistää aktiiviseksi niin monta kuin on elämien määrä
        //loput ei aktiiviseksi
        //jos elämiä tulisi lisää niin tälle pitää kutsua     StartFlashing(GameObject go,  Color.black;)
        //jos elämiä vähenee niin tälle pitää kutsua   StartFlashing(GameObject go,  Color.red;)
        if (elamatversio2 != null)
        {
            for (int i = 0; i < elamatversio2.Length; i++)
            {


               // elamatversio2[i].SetActive(false);
            }

            int laskuri = 0;
            for (int i = 0; i < elamatmaara && i < elamatversio2.Length; i++)
            {
                laskuri = i;
                elamatversio2[i].SetActive(true);
                /*
                if (edellinenmaara!=0)
                {
                    StartFlashing(elamatversio2[i].GetComponent<Image>());
                }
                */


            }
            if (edellinenmaara!=0)
            {
                if (edellinenmaara< elamatmaara)
                {
                    //saatu elämä lisää
                }
                else
                {
                    //menetetty
                }
            }
            
            edellinenmaara = elamatmaara;



        }
    }

    public void SetElamienMaara(int elamatmaara)
    {
        if (elamatversio2 == null) return;

        // 1. Deactivate all
        for (int i = 0; i < elamatversio2.Length; i++)
        {
            elamatversio2[i].SetActive(false);
        }

        // 2. Activate required hearts
        for (int i = 0; i < elamatmaara && i < elamatversio2.Length; i++)
        {
            elamatversio2[i].SetActive(true);
        }

        // 3. Handle flashing if count changed
        if (edellinenmaara != 0)
        {
            if (elamatmaara > edellinenmaara)
            {
                // Gained life — flash the new heart(s)
                for (int i = edellinenmaara; i < elamatmaara && i < elamatversio2.Length; i++)
                {
                  //  Image img = elamatversio2[i].GetComponent<Image>();
                    StartFlashing(elamatversio2[i], flashElamaLisaa,true);
                }
            }
            else if (elamatmaara < edellinenmaara)
            {
                // Lost life — flash the removed heart(s)
                for (int i = elamatmaara; i < edellinenmaara && i < elamatversio2.Length; i++)
                {
                    //Image img = elamatversio2[i].GetComponent<Image>();
                    elamatversio2[i].SetActive(true);
                    StartFlashing(elamatversio2[i], flashColor, false);
                }
            }
        }

        edellinenmaara = elamatmaara;
    }

    public Color flashColor = Color.red;
    public float flashDuration = 5f;


    public Color flashElamaLisaa = Color.black;
    public void StartFlashing(GameObject go, Color c,bool active)
    {
        StartCoroutine(FlashImageForSeconds(go, c,active));
    }

    private IEnumerator FlashImageForSeconds(GameObject go, Color c,bool active)
    {
        Image image = go.GetComponent<Image>();
        Color originalColor = go.GetComponent<Image>().color;

        float elapsed = 0f;
        float flashInterval = 0.2f; // blink every 0.2 sec

        while (elapsed < flashDuration)
        {
            image.color = c;
            yield return new WaitForSeconds(flashInterval / 3);
            image.color = originalColor;
            yield return new WaitForSeconds(flashInterval / 3);
            elapsed += flashInterval;
        }

        image.color = originalColor; // ensure it ends on normal
        go.SetActive(active);
    }

}
