using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamera : MonoBehaviour
{
    // 

    public GameObject alus;
    public float skrollimaara;

    public LayerMask layerMask;
    public int vihollismaaranrajaarvo = 4;
    public float xsuunnanoffsettikamerasta=4.0f;

    // Start is called before the first frame update

    public GameObject savu;
    public float savuoffsetx = 2.0f;
    public float savuoffsety = 10.0f;

    void Start()
    {
        float y = 0f;
        y = alus.transform.position.y;
        if (alus != null)
        {
            float ero = alus.transform.position.x - transform.position.x;
            if (Mathf.Abs(ero) > 8)
            {
                alus.transform.position = new Vector3(transform.position.x, y, 0);
                alus.GetComponent<AlusController>().UusiohjauskaytossaAsetaSormi();
            }


        }
        if (savu != null)
        {
            savu.transform.position = transform.position + new Vector3(savuoffsetx, savuoffsety, 0);
        }
    }

    // Update is called once per frame
    float stopinaloitusaika = 0.0f;

    public int paljonkopitaavielaodottaa = -1;
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
            else
            {
                if (cameraInfo.GetComponent<CameraInfoController>().generoilisaavihollisia)
                {
                    GameObject go =
                    cameraInfo.GetComponent<CameraInfoController>().vihollinenjokageneroidaan;

                    int maara =
                    cameraInfo.GetComponent<CameraInfoController>().vihollismaaranrajaarvo;
                    float generointivali = cameraInfo.GetComponent<CameraInfoController>().vihollismaaranrajaarvo;

                    GeneroiLisaaVihollisia(go,maara, generointivali);
                }
            }

            skrollimaara = xscr;

        }
        Vector3 skrolli = new Vector3(skrollimaara, 0f, 0f) * Time.deltaTime;
        //Debug.Log("skrolli=" + skrolli);

        transform.position += skrolli;
        if (savu!=null)
        {
            savu.transform.position = transform.position + new Vector3(savuoffsetx, savuoffsety, 0);
        }
        if (alus != null)
        {
            //alus.transform.position += skrolli;
            alus.GetComponent<AlusController>().LisaaSkrollia(skrolli);

        }
       

    }


    public GameObject cameraInfo;

    public string PalautaOdotusAikaKunnesLiikkuu()
    {
        if (paljonkopitaavielaodottaa >= 0)
        {
            return "" + paljonkopitaavielaodottaa;
        }
        {
            return "";
        }
    }

    private float generointilaskuri = 0.0f;

    private void GeneroiLisaaVihollisia(GameObject go,int  vihollismaaranrajaarvo, float generointivali)
    {
        generointilaskuri += Time.deltaTime;
        if (generointilaskuri>= generointivali)
        {
            int maara = GetCollidersInCameraView(Camera.main);
            if (maara < vihollismaaranrajaarvo)
            {
                Debug.Log("generoi lisaa vihollisia" + maara);
                GeneroiViholinen(go);
            }
            generointilaskuri = 0;
        }


    }
    private void GeneroiViholinen(GameObject go)
    {

                Vector2 boxsize = new Vector2(1.0f,1.0f);
                Vector2 pos = transform.position;

                float camHeight = Camera.main.orthographicSize * 2f;
                float camWidth = camHeight * Camera.main.aspect;

                Vector2 bottomLeft = (Vector2)Camera.main.transform.position - new Vector2(camWidth / 2, camHeight / 2);
                Vector2 topRight = (Vector2)Camera.main.transform.position + new Vector2(camWidth / 2, camHeight / 2);

                Vector2 uus = new Vector2(topRight.x + xsuunnanoffsettikamerasta, transform.position.y);

                GameObject instanssiOption = Instantiate(go, uus, Quaternion.identity);
    }


    int GetCollidersInCameraView(Camera camera)
    {
        // Calculate the camera's world-space bounds
        float camHeight = camera.orthographicSize * 2f;
        float camWidth = camHeight * camera.aspect;

        Vector2 bottomLeft = (Vector2)camera.transform.position - new Vector2(camWidth / 2, camHeight / 2);
        Vector2 topRight = (Vector2)camera.transform.position + new Vector2(camWidth / 2, camHeight / 2);

        // Use Physics2D.OverlapArea to get colliders within the bounds
        Collider2D[] colliders = Physics2D.OverlapAreaAll(bottomLeft, topRight, layerMask);
        int count = 0;
        foreach (Collider2D c in colliders)
        {
            if (c.tag.Contains("vihollinen") && (!c.tag.Contains("tiili") && !c.tag.Contains("alus") ))
            {
                count++;
            }
        }
        return count;
    }
}
