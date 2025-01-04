using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamera : MonoBehaviour
{
    // 

    public GameObject alus;
    public float skrollimaara;
    // Start is called before the first frame update
    void Start()
    {
        float y = 0f;
        y = alus.transform.position.y;
        if (alus != null)
        {
            float ero = alus.transform.position.x - transform.position.x;
            if (Mathf.Abs(ero)>8)
            {
                alus.transform.position = new Vector3(transform.position.x, y, 0);
            }

           
        }
    }

    // Update is called once per frame
    float stopinaloitusaika = 0.0f;

    int paljonkopitaavielaodottaa = -1;
    void Update()
    {
        //   gameObject.transform.position.Set(alus.transform.position.x, alus.transform.position.y, gameObject.transform.position.z);
        if (cameraInfo != null)
        {




            float xscr = cameraInfo.GetComponent<CameraInfoController>().scrollspeedx;


            bool stop = cameraInfo.GetComponent<CameraInfoController>().stop;
            float stoptime = cameraInfo.GetComponent<CameraInfoController>().stoptime;

            if (stop)
            {
                if (stopinaloitusaika == 0.0f)
                {
                    stopinaloitusaika = Time.time;
                    paljonkopitaavielaodottaa = (int)stoptime;
                }

                if (Time.time - stopinaloitusaika > stoptime)
                {
                    stop = false;
                    paljonkopitaavielaodottaa = -1;
                }
                else
                {
                    float odotusaika = stopinaloitusaika + stoptime - Time.time;
                    paljonkopitaavielaodottaa = (int)odotusaika;
                    xscr = 0.0f;
                }

            }

            skrollimaara = xscr;

        }
        Vector3 skrolli = new Vector3(skrollimaara, 0f, 0f) * Time.deltaTime;
        //Debug.Log("skrolli=" + skrolli);

        transform.position += skrolli;
        if (alus != null)
        {
            alus.transform.position += skrolli;

        }


    }


    public GameObject cameraInfo;

    public string PalautaOdotusAikaKunnesLiikkuu()
    {
        if (paljonkopitaavielaodottaa>=0)
        {
            return ""+paljonkopitaavielaodottaa;
        }
        {
            return "";
        }
    }

}
