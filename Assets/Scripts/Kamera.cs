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
    public float xsuunnanoffsettikamerasta = 4.0f;

    // Start is called before the first frame update

 //   public GameObject savu;
    public float savuoffsetx = 2.0f;
    public float savuoffsety = 10.0f;


    public void AsetaAlusKeskelleKameraa()
    {
        if (alus != null)
        {
            float y = 0f;
            y = alus.transform.position.y;
            float ero = alus.transform.position.x - transform.position.x;
                alus.transform.position = new Vector3(transform.position.x, y, 0);
              //  alus.GetComponent<AlusController>().UusiohjauskaytossaAsetaSormi();
         
        }
    }

    void Start()
    {
        float y = 0f;
        //y = alus.transform.position.y;
        if (alus != null)
        {
            
            float ero = alus.transform.position.x - transform.position.x;
            if (Mathf.Abs(ero) > 8)
            {
                alus.transform.position = new Vector3(transform.position.x, y, 0);
            //    alus.GetComponent<AlusController>().UusiohjauskaytossaAsetaSormi();
            }
            

        }
        if (cameraInfo != null)
        {
            CameraInfoController cami =
cameraInfo.GetComponent<CameraInfoController>();
            if (cami.savu != null)
            {
                cami.savu.SetActive(true);
                cami.savu.transform.position = transform.position + new Vector3(savuoffsetx, savuoffsety, 0);
            }
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
            /*

                public GameObject savu;
    public GameObject tausta1;
    public GameObject tausta2;
    public GameObject tausta3;
    */
            CameraInfoController cami =
            cameraInfo.GetComponent<CameraInfoController>();
            if (cami.naytasavu)
            {
                if (cami.savu != null)
                {
                    cami.savu.active = true;
                    cami.savu.transform.position = transform.position + new Vector3(savuoffsetx, savuoffsety, 0);
                }
            }
            else
            {
                if (cami.savu != null)
                {
                    cami.savu.SetActive(false);
                }
            }
            if (cami.tausta1.active!=cami.naytataustat)
            cami.tausta1.active=cami.naytataustat;
            if (cami.tausta2.active != cami.naytataustat)
                cami.tausta2.active=cami.naytataustat;
            if (cami.tausta3.active != cami.naytataustat)
                cami.tausta3.active=cami.naytataustat;
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
                    GameObject[] go =
                    cameraInfo.GetComponent<CameraInfoController>().vihollisetjokageneroidaan;

                    int maara =
                    cameraInfo.GetComponent<CameraInfoController>().vihollismaaranrajaarvo;
                    float generointivali = cameraInfo.GetComponent<CameraInfoController>().generointivali;


                    GameObject[] olemassa = cameraInfo.GetComponent<CameraInfoController>().vihollisetJoidenOlemassaOloTutkitaan;

                    GeneroiLisaaVihollisia(go, maara, generointivali, olemassa);
                }
            }

            skrollimaara = xscr;

        }
        Vector3 skrolli = new Vector3(skrollimaara, 0f, 0f) * Time.deltaTime;
        //Debug.Log("skrolli=" + skrolli);

        transform.position += skrolli;




        if (alus != null)
        {
            //alus.transform.position += skrolli;
            alus.GetComponent<AlusController>().LisaaSkrollia(skrolli);

        }


    }


    public GameObject cameraInfo;

    public string PalautaOdotusAikaKunnesLiikkuu()
    {
        string lisateksti = "";
        if (cameraInfo!=null)
        {
            if (cameraInfo.GetComponent<CameraInfoController>().lisateksti!=null)
            {
                lisateksti = cameraInfo.GetComponent<CameraInfoController>().lisateksti;
            }
        }


        if (paljonkopitaavielaodottaa >= 0)
        {
            return lisateksti + paljonkopitaavielaodottaa;
        }
        {
            return "";
        }
    }


    public bool onkoAluksenAmmuksetDisabloitu()
    {


        
        if (cameraInfo != null)
        {
            return cameraInfo.GetComponent<CameraInfoController>().disablealuksenammukset;
            
        }

        return false;
    }

    private float generointilaskuri = 0.0f;

    private void GeneroiLisaaVihollisia(GameObject[] go, int vihollismaaranrajaarvo, float generointivali, GameObject[] vihollisetjoidenOlemassaOloTutkitaan)
    {
        generointilaskuri += Time.deltaTime;
        if (generointilaskuri >= generointivali)
        {
            int maara = 0;
            foreach (GameObject g in vihollisetjoidenOlemassaOloTutkitaan)
            {
                maara+= GetCountOfTheseObjects(Camera.main, g);
            }
            

           
            if (maara < vihollismaaranrajaarvo)
            {
                Debug.Log("generoi lisaa vihollisia nykymaara=" + maara);
                GameObject randomGO = go[Random.Range(0, go.Length)];
                Debug.Log("generoi lisaa vihollisia generoitu oli=" + randomGO);
                GeneroiViholinen(randomGO);

            }
            generointilaskuri = 0;
        }


    }
   
    private void GeneroiViholinen(GameObject go)
    {

        Vector2 boxsize = new Vector2(1.0f, 1.0f);
        Vector2 pos = transform.position;

        float camHeight = Camera.main.orthographicSize * 2f;
        float camWidth = camHeight * Camera.main.aspect;

        Vector2 bottomLeft = (Vector2)Camera.main.transform.position - new Vector2(camWidth / 2, camHeight / 2);
        Vector2 topRight = (Vector2)Camera.main.transform.position + new Vector2(camWidth / 2, camHeight / 2);

        Vector2 uus = new Vector2(topRight.x + xsuunnanoffsettikamerasta, transform.position.y);

        GameObject instanssiOption = Instantiate(go, uus, Quaternion.identity);
    }

    /*
   int GetCollidersInCameraView(Camera camera)
   {

       HashSet<GameObject> uniikit = new HashSet<GameObject>();
       // Calculate the camera's world-space bounds
       float camHeight = camera.orthographicSize * 2f;
       float camWidth = camHeight * camera.aspect;

       Vector2 bottomLeft = (Vector2)camera.transform.position - new Vector2(camWidth / 2, camHeight / 2);
       Vector2 topRight = (Vector2)camera.transform.position + new Vector2(camWidth / 2, camHeight / 2);

       // Use Physics2D.OverlapArea to get colliders within the bounds
       //Collider2D[] colliders = Physics2D.OverlapAreaAll(bottomLeft, topRight, layerMask);
       Collider2D[] colliders = Physics2D.OverlapAreaAll(bottomLeft, topRight);


       int count = 0;
       foreach (Collider2D c in colliders)
       {
           if (c.tag.Contains("vihollinen") && (!c.tag.Contains("tiili") && !c.tag.Contains("alus")))
           {
               //count++;
               GameObject root = c.gameObject.transform.root.gameObject;
               if (root!=null)
               {
                   uniikit.Add(root);
               }
               else
               {
                   uniikit.Add(c.gameObject);
               }
           }
       }
       return uniikit.Count;
   }
   */

    public List<GameObject> GetGameObjectsInCameraView(Camera camera)
    {
        // Calculate the camera's world-space bounds
        List<GameObject> ret = new List<GameObject>();
        float camHeight = camera.orthographicSize * 2f;
        float camWidth = camHeight * camera.aspect;

        Vector2 bottomLeft = (Vector2)camera.transform.position - new Vector2(camWidth / 2, camHeight / 2);
        Vector2 topRight = (Vector2)camera.transform.position + new Vector2(camWidth / 2, camHeight / 2);

        // Use Physics2D.OverlapArea to get colliders within the bounds
        //Collider2D[] colliders = Physics2D.OverlapAreaAll(bottomLeft, topRight, layerMask);
        Collider2D[] colliders = Physics2D.OverlapAreaAll(bottomLeft, topRight);

        int count = 0;
        foreach (Collider2D c in colliders)
        {
            if (c.tag.Contains("vihollinen") && (!c.tag.Contains("tiili") && !c.tag.Contains("alus")))
            {
                if (c.gameObject.activeInHierarchy)
                {
                    ret.Add(c.gameObject);
                }
                
            }
        }
        return ret;
    }

    public int GetCountOfTheseObjectsVanha(Camera camera,GameObject go)
    {
        int count = 0;
        List<GameObject> lista = GetGameObjectsInCameraView(camera);
        var comp1=go.GetComponent<MonoBehaviour>();
        foreach (GameObject g in lista)
        {
            var comp2 = g.GetComponent<MonoBehaviour>();
            if (comp1 != null && comp2 != null && comp1.GetType() == comp2.GetType())
            {
                count++;
            }
        }
      //  Debug.Log("count objektit=" + count);
        return count;

    }

    public int GetCountOfTheseObjects(Camera camera, GameObject referenceObject)
    {


        if (referenceObject == null) return 0;

        var referenceComponent = referenceObject.GetComponent<MonoBehaviour>();
        if (referenceComponent == null) return 0;

        System.Type typeToMatch = referenceComponent.GetType();
        int count = 0;

        List<GameObject> objectsInView = GetGameObjectsInCameraView(camera);
        foreach (var obj in objectsInView)
        {
            var component = obj.GetComponent(typeToMatch);
            if (component != null)
            {
                count++;
            }
        }

        return count;
    }


    public float GetCurrentScrollSpeed()
    {
        return skrollimaara;
    }

    public float GetKorjattuArvoPerustuenSkrollimaaraanTamaSiksiEttaAmmusLentaaNopeamminVasemmaltaOikealleSilloinKunKameraLiikkuuNopeammin(float alkuperainenNopeus,float kerroin)
    {
        if (Mathf.Approximately(skrollimaara, 0f))
        {
            return alkuperainenNopeus;
        }

        // Jos ammus liikkuu oikealle (positiivinen nopeus), lisää skrollimaara
        if (alkuperainenNopeus > 0)
        {
            return alkuperainenNopeus + skrollimaara * kerroin;//randomi luvulla 2.0f
        }
        // Jos ammus liikkuu vasemmalle (negatiivinen nopeus), vähennä skrollimaara
        else if (alkuperainenNopeus < 0)
        {
         //   return alkuperainenNopeus - skrollimaara;
        }

        return alkuperainenNopeus;
    }


}
