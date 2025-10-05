using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class AlusController : BaseController, IDamagedable, IExplodable
{

    public bool enabloiboxcollider = true;//EI TOIMI JOS EDITORISSA VAIHTAA ARVOA

    //ensinnäkin gameobjecti jolla voi säätää kameran nopeutta

    //toisekseen joku gameobjekti jolla voi tehdä näitä savepointteja


    // public int elamienmaara = 0;
    // public int score = 0;//tälle se 
    // public float difficalty = 1.0f;//peli kiertää uusiksi sitten kun pääsee läpi, mutta difficalt


    public bool uusiohjauskaytossa = true;

    public float maksimimaaradamageajokakestetaan = 100.0f;
    public float damagenmaara = 0.0f;
    public bool damagemodekaytossa = false;


    public float gravitymodifiermuutoskunammusosuu = 0.01f;


    public float gravitynminimi = -0.2f;


    public float damagemaarakasvastatuskunsavutaan = 5.0f;

    public float turboviiveSekunneissa = 0.4f;
    public float maksiminopeusylosalas = 0.08f;


    public float maksiminopeusvertikaalinen = 0.08f;

    public float kiihtyvyys = 0.05f;

    private float oikeanappipainettukestoaika = 0.0f;

    private float vasennappipainettukestoaika = 0.0f;


    private float ylosnappipainettukestoaika = 0.0f;
    private float alasnappipainettukestoaika = 0.0f;

    public float perusnopeus = 0.05f;


    float deltaaikojensumma = 0f;

    private Vector3 _offset; // Erotus aluksen ja kosketuksen välillä
    private bool _isDragging = false;


    public float optiotfollowDistance = 0.5f; // Distance behind the player
    public float optiotmoveSpeed = 1f; // Speed of the Option Ball's movement

    private int aluksenluomienElossaOlevienAmmustenMaara;

    private int aluksenluomienElossaOlevienAlasAmmustenMaara;
    private int aluksenluomienElossaOlevienYlosAmmustenMaara;


    /*
    public InputActionReference moveInputActionReference;


    public InputActionReference moveDownInputActionReference;

    public InputActionReference moveUpInputActionReference;
    */

    public bool autofire;
    //  public GameObject speedbonusbutton;
    //  public GameObject missilebonusbutton;

    public int ammustenmaksimaaraProperty;

    public float ampumakertojenvalinenviive;


    public float ampujakertojenvalisenViiveenPienennysKunSpeedBonusButtonOtettu;
    public int ammustenmaksimaaranLisaysKunSpeedBonusButtonOtettu;

    public int nykyinenoptioidenmaara;

    public int optioidenmaksimaara;


    //   public Joystick joystick;

    public GameObject explosion;

    private Rigidbody2D m_Rigidbody2D;

    //vauhti
    private Animator m_Animator;

    private bool oikeaNappiPainettu = false;

    private bool vasenNappiPainettu = false;


    public bool laserkaytossa = true;

    public bool forcefieldkaytossa = false;

    public GameObject laserPrefab;

    //ylos/alla

    private bool ylosNappiPainettu = false;
    private bool alasNappiPainettu = false;



    private bool spaceNappiaPainettu = false;
    //private bool spaceNappiAlhaalla = false;
    //private bool spaceNappiYlhaalla = false;


    public GameObject extraGun;


    private bool ammusInstantioitiinviimekerralla = false;

    public GameObject ammusPrefab;

    public GameObject bulletPrefab;

    public GameObject gameoverPrefab;


    private SpriteRenderer m_SpriteRenderer;




    private Vector2 screenBounds;

    private float objectWidth;
    private float objectHeight;

    private bool restartscene = false;


    // private GameObject instanssiBulletAlas;
    //private GameObject instanssiBulletYlos;



    public GameObject forceFieldEtuOsa;
    public GameObject forceFieldEtuosanPaikka;



    public GameObject instanssiOption1;
    private bool teeOptionPallukka = false;

    private int missileDownCollected = 0;
    private int missileUpCollected = 0;

    //

    public GameObject debugloota;


    public GameObject elamat;



    /*
     * LOOSER HUUTO ok
     * kellon kuva ruudulle, jonka voi poimia aika hidaastuu esim. puoleen 10 sekunniksi
     * sitten se camera stop ominaisuus, pitää tehdä esim. niitä tulivuoria
     * ja sitten levelin loppuun se pääpahis
     * sama scene josta tehdään useita leveleitä
     * new game aloita alusta
     * tai sitten aloita pelatuista leveleistä mutta ilman aseita


    


    /*
    public Vector3 palautaViimeinenSijaintiScreenpositioneissa(int optionjarjestysnumero)
    {

        //return aluksenpositiotCameraViewissa[optioidenmaksimaara * 10 - optionjarjestysnumero* 10];

        return base.palautaScreenpositioneissa(optioidenmaksimaara * 8 - optionjarjestysnumero * 8);

    }
    */
    //   private List<Vector3> aluksenpositiotCameraViewissa = new List<Vector3>();
    private void TallennaSijaintiSailytaVainKymmenenViimeisinta()
    {
        // base.TallennaSijaintiSailytaVainNkplViimeisinta(optioidenmaksimaara * 8, true, true, debugloota);
        if (true)
        {
            return;
        }
        /**
        Vector3 worldPosition = transform.position;

        // Convert the world position to screen position
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        screenPosition = new Vector3( (int)screenPosition.x, (int)screenPosition.y, screenPosition.z);
        if (aluksenpositiotCameraViewissa.Count == 0)
        {
            // Vector3Int sijainti = new Vector3Int( (int)screenPosition.x, (int)screenPosition.y,(int)screenPosition.z );

            //Vector3 sijainti = new Vector3( (int)screenPosition.x, (int)screenPosition.y, screenPosition.z);
            aluksenpositiotCameraViewissa.Add(screenPosition);
        }
        else
        {
            Vector3 viimeisin = aluksenpositiotCameraViewissa[aluksenpositiotCameraViewissa.Count - 1];
            if (!Vector3.Equals(viimeisin,screenPosition))
        
            {
                //viimeisin.x != screenPosition.x || viimeisin.y != screenPosition.y)
                //Debug.Log("viimeisin.x=" + viimeisin.x + "screenPosition.x=" + screenPosition.x+" viimeisin.y="+viimeisin.y+ " screenPosition.y="+ screenPosition.y);
                //Vector3 sijainti = new Vector3((int)screenPosition.x, (int)screenPosition.y, screenPosition.z);
                aluksenpositiotCameraViewissa.Add(screenPosition);

            }
        }
        //40,30 optio 3,20 optio2 ,10 optio1 ,0


        //0 optio3,10 optio2,20 optio 1,30
        if (aluksenpositiotCameraViewissa.Count >optioidenmaksimaara*10)
        {
            aluksenpositiotCameraViewissa.RemoveAt(0);
        }
        **/
    }

    public float vauhdinLisaysKunSpeedbonusOtettu = 1.0f;

    public GameObject maincameraGameObject;
    private Camera mainCamera;
    // Start is called before the first frame update



    private AudioplayerController ad;


    private ParticleSystem particleSystem;


    private BoxCollider2D boxCollider2D;
    private bool android;

    //   private LineRenderer lineRenderer;
    void Start()
    {
        android = Application.platform == RuntimePlatform.Android;

        particleSystem = GetComponentInChildren<ParticleSystem>();
        if (particleSystem.isPlaying)
        {
            particleSystem.Stop();
        }

        //  audiosourcetaustamusiikki.Play();
        //   Application.targetFrameRate = 60;//tärkeäää
        // Application.targetFrameRate = 10; // For example, cap to 60 FPS
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        //   mainCamera = Camera.main;

        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        //        Debug.Log("screenBounds=" + screenBounds);

        // Debug.Log("ennen findia");
        if (Application.platform != RuntimePlatform.Android)
        {
            GameObject[] oo = GameObject.FindGameObjectsWithTag("painike");
            foreach (GameObject o in oo)
            {
                o.SetActive(false);

            }
        }



        BonusButtonController[] bs = (BonusButtonController[])FindObjectsOfType(typeof(BonusButtonController));

        foreach (BonusButtonController btc in bs)
        {
            // do whatever with each 'enemy' here
            // Debug.Log(""+btc.order+ " btc.selected=" +btc.selected.ToString() + " btc.used= " + btc.used.ToString);


            //       Debug.Log("btc.order=" + btc.order + " btc.selected=" + btc.selected + " btc.usedcount=" + btc.usedcount);

            if (btc.bonusbuttontype.Equals(BonusButtonController.Bonusbuttontype.Option))
            {
                btc.maxusedCount = optioidenmaksimaara;
            }


            bbc.Add(btc);

        }
        // words.Sort((a, b) => a.Length.CompareTo(b.Length));

        bbc.Sort((a, b) => a.order.CompareTo(b.order));


        ad = PalautaAudioplayerController();
        ad.TaustaMusiikkiPlay();

        //TeeOptioni();

        GameObject myObject = GameObject.Find("QuitButtonKaytossa");
        myObject.GetComponent<Image>().enabled = false;
        Color color = m_SpriteRenderer.color;
        gvarinalkutilanne = color.g;
        boxCollider2D = GetComponent<BoxCollider2D>();

        damagemittariController = damageMittari.GetComponent<DamagemittariController>();


        if (!damagemodekaytossa)
        {
            //   gameObject.transform.parent.gameObject.SetActive(false);
        }



        GameObject foundObject = GameObject.Find("Ruudunkeskiteksti");
        keskellaTextMeshProUGUI = foundObject.GetComponent<TextMeshProUGUI>();

        GameObject foundObject2 = GameObject.Find("Ruudunvasenylakulmateksti");
        ruudunvasenylakulmatekstiTextMeshProUGUI = foundObject2.GetComponent<TextMeshProUGUI>();


        //  sormi = GameObject.Find("Sormialus");
        //  sormitoisessakamerassa = GameObject.Find("SormialusToinenkamera");
        // sormenkamera = GameObject.Find("SormiCamera");


        //   sormikamera = sormenkamera.GetComponent<Camera>();
        mainCamera = maincameraGameObject.GetComponent<Camera>();
        sormikamera = sormenkamera.GetComponent<Camera>();


        //sormiKameraController = sormenkamera.GetComponent<SormiKameraController>();
        if (uusiohjauskaytossa)
        {
            //   sormi.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            //prosenttiosuusmikaonvarattuohjaukseenkorkeudessa = CalculatePercentageAboveBottom2(alamaksiminMaarittava);
            //säätää myös pääkameran koon
            //  if (sormiKameraController==null)
            //  {
            //    Debug.Log("sormiKameraController==null");
            // }

            SetOsuudet(prosenttiosuusalasuoja, prosenttiosuusmikaonvarattuohjaukseenkorkeudessa,
                mainCamera);


            //         UusiohjauskaytossaAsetaSormi();
        }
        else
        {

        }
        //   lineRenderer = GetComponent<LineRenderer>();

        SetElamienMaara(GameManager.Instance.lives);

        SetEnabloiboxcollider(enabloiboxcollider);


        for (int i = 0; i < nykyinenoptioidenmaara; i++)
        {
            TeeOptioni();
        }
        ForceField(forcefieldkaytossa);


    }

    private void ForceField(bool kayttoon)
    {
        /*
            public GameObject forceFieldEtuOsa;
    public GameObject forceFieldEtuosanPaikka;
    */


        if (kayttoon)
        {
            ForceFieldController ff = GetComponent<ForceFieldController>();
            if (ff == null)
            {
                GameObject obj = Instantiate(forceFieldEtuOsa, forceFieldEtuosanPaikka.transform.position, Quaternion.identity);

                obj.transform.SetParent(gameObject.transform, true);
            }
        }
        else
        {
            ForceFieldController ff = GetComponent<ForceFieldController>();
            if (ff != null)
            {
                Destroy(ff.gameObject);
            }
        }
    }





    public float checkDistance = 5.0f; // Distance threshold
    private float timeSinceLastCheck = 0f; // Track time for 5-second intervals

    /*

    private void PerformDistanceCheck()
    {
        if (this.gameObject == null) return;

        // Get the position of gameObject1 on the main thread
        Vector3 referencePosition = this.gameObject.transform.position;

        // Get all GameObjects in the scene
//        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        GameObject[] allObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();


        // Perform the distance check and toggle GameObjects based on distance
        foreach (GameObject obj in allObjects)
        {

            Debug.Log("tagiaaaaaaaa='" + obj.tag+"'");
            if (obj != null && obj != this.gameObject && obj.tag != null &&  obj.tag.Contains("pallovihollinentag")
                && !obj.tag.Contains("tiili")) // Skip the reference GameObject itself
                {

                   
                float distance = Vector3.Distance(obj.transform.position, referencePosition);
                obj.SetActive(distance <= checkDistance); // Enable or disable based on distance
            }
        }
    }
    */

    private void PerformDistanceCheck()
    {
        if (this.gameObject == null) return;

        // Get the position of gameObject1 on the main thread
        Vector3 referencePosition = this.gameObject.transform.position;

        // Get all root GameObjects in the scene
        GameObject[] allRootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        // Perform the distance check for all objects (active and inactive)
        foreach (GameObject obj in allRootObjects)
        {
            // Check recursively for all child objects, including inactive ones
            CheckAllObjects(obj, referencePosition);
        }
    }



    private void CheckAllObjects(GameObject parent, Vector3 referencePosition)
    {
        // Check the parent object itself (this will include inactive objects)
        if (parent != this.gameObject)
        {
            if (parent != null && parent != this.gameObject && parent.tag != null && (parent.tag.Contains("eituhvih")
                || parent.tag.Contains("vihollinen")) && !parent.tag.Contains("tiili"))
            {

                float distance = Vector3.Distance(parent.transform.position, referencePosition);
                parent.SetActive(distance <= checkDistance); // Enable or disable based on distance
            }
        }

        // Recursively check all child GameObjects (including inactive ones)
        foreach (Transform child in parent.transform)
        {
            // Recursively check the child GameObjects
            CheckAllObjects(child.gameObject, referencePosition);
        }
    }



    public GameObject sormitoisessakamerassa;

    public GameObject sormenkamera;

    private Camera sormikamera;

    //private SormiKameraController sormiKameraController;

    private GameObject damagetausta;

    public GameObject keskari;


    public void SetOsuudet(float p_prosenttiosuusalasuoja, float p_prosenttiosuusmikaonvarattuohjaukseenkorkeudessa,
    Camera paakamera)
    {
        if (paakamera == null)
        {
            Debug.Log("paakamera on nulli");
        }
        prosenttiosuusalasuoja = p_prosenttiosuusalasuoja;
        prosenttiosuusmikaonvarattuohjaukseenkorkeudessa = p_prosenttiosuusmikaonvarattuohjaukseenkorkeudessa;

        float yht = (prosenttiosuusalasuoja + prosenttiosuusmikaonvarattuohjaukseenkorkeudessa) / 100.0f;


        // / Bottom camera(50 % height, bottom of the screen)
        sormikamera.rect = new Rect(0, 0, 1, yht);

        paakamera.rect = new Rect(0, yht, 1, 1);
        //
        float kerroin = 100.0f / (prosenttiosuusalasuoja + prosenttiosuusmikaonvarattuohjaukseenkorkeudessa);
        prosenttiosuusalasuoja = kerroin * prosenttiosuusalasuoja;
        prosenttiosuusmikaonvarattuohjaukseenkorkeudessa = kerroin * prosenttiosuusmikaonvarattuohjaukseenkorkeudessa;
        // Debug.Log("prosenttiosuusalasuoja=" + prosenttiosuusalasuoja + " prosenttiosuusmikaonvarattuohjaukseenkorkeudessa=" +
        //   prosenttiosuusmikaonvarattuohjaukseenkorkeudessa);


    }


    public void LisaaSkrollia(Vector3 skrolli)
    {
        transform.position += skrolli;
        // sormitoisessakamerassa.transform.position += skrolli;
    }

    public void UusiohjauskaytossaAsetaSormi()
    {
        float sormiuusisijainti = PalautaOhjausAlueenYPelialueenYylla(transform.position);
        //float sormiuusisijaintix= PalautaOhjausAlueenxPelialueenxylla(transform.position);
        /*
        sormitoisessakamerassa.transform.position = new Vector3(transform.position.x, sormiuusisijainti, 0);

        Vector2 screenPosition = sormikamera.WorldToScreenPoint(transform.position);

        Vector3 worlidi= sormikamera.ScreenToWorldPoint(screenPosition);

        sormitoisessakamerassa.transform.position = new Vector3(worlidi.x, sormiuusisijainti, 0);
        */

        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);

        // Step 2: Convert the viewport position to world position in camera2
        Vector3 worldPosInCamera2 = sormikamera.ViewportToWorldPoint(new Vector3(viewportPos.x, viewportPos.y, sormikamera.nearClipPlane));

        // Step 3: Update only the X position of gameObject2 to align horizontally
        Vector3 newPosition = sormitoisessakamerassa.transform.position;
        newPosition.x = worldPosInCamera2.x;

        // Apply the updated position to gameObject2
        // gameObject2.transform.position = newPosition;
        float sormikorkeus = PalautaSormenkorkeus();




        sormitoisessakamerassa.transform.position = new Vector3(newPosition.x, sormiuusisijainti - sormikorkeus, 0);



        if (damagemittarilinerenderer != null)

            damagemittarilinerenderer.transform.position = new Vector3(PalautaKameranMinX(), PalautaPelialueenAlarajaY(), 0);
        /*
        if (true)
            return;


        float screenHeightInWorld = Camera.main.orthographicSize * 2;

        float aluksenpieninkohta = Calculate25PercentAboveBottom(prosenttiosuusmikaonvarattuohjaukseenkorkeudessa);

        float aluksentilaysuunnassa = screenHeightInWorld * ((100 - prosenttiosuusmikaonvarattuohjaukseenkorkeudessa) / 100.0f);


        float ohjausosuudentilaysuunnassa = screenHeightInWorld * ((prosenttiosuusmikaonvarattuohjaukseenkorkeudessa) / 100.0f);




        float alusprossylialarajan =
            Laskemontakoprosenttiaalusonalarajanylapuolella(aluksenpieninkohta, aluksentilaysuunnassa);


        float aloituskohta = ohjausosuudentilaysuunnassa * alusprossylialarajan;

        // float screenHeightInWorld = 2 * Camera.main.orthographicSize; // Total height
        float bottomY = Camera.main.transform.position.y - Camera.main.orthographicSize; // Bottom of the screen



        float objectHeight = sormi.GetComponent<Renderer>().bounds.size.y;


        sormi.transform.position = new Vector3(transform.position.x, bottomY + aloituskohta - objectHeight / 2.0f, 0);
        */

        /*
        //float sormenprosentuaalinenyosuus = sormi.transform.position.y / aluksenpieninkohta;

        float sormiprossat = CalculatePercentageAboveBottom(sormi);
        Debug.Log("sormiprossat=" + sormiprossat);

        //eli siitä 25% pitäis saada 100%
        //ja siitä 0% pitäis saada 0&
        //12.5%=100
        float laskentay = 100.0f / prosenttiosuusmikaonvarattuohjaukseenkorkeudessa;
        float alusjuttu = laskentay * sormiprossat;


        float montakoprossaasormestakaytetty = sormiprossat / prosenttiosuusmikaonvarattuohjaukseenkorkeudessa;


        float aluksenysuunnassalisays = aluksentilaysuunnassa * (montakoprossaasormestakaytetty);

        float alussijainti = aluksenpieninkohta + aluksenysuunnassalisays;
        */


        //SetUIPosition(new Vector3(transform.position.x, transform.position.y , 0));

    }

    /*
    public void SetUIPosition(Vector3 worldPosition)
    {
        // Convert the world position to screen position
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        // Convert the screen position to local position in the canvas
        RectTransform canvasRect = sormi.GetComponent<RectTransform>();
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            Camera.main, // Use null if Canvas Render Mode is Screen Space - Overlay
            out localPosition
        );

        // Set the local position of the UI element
        sormi.GetComponent<RectTransform>().localPosition = localPosition;
    }
    */

    // private GameObject sormi;

    private bool UusiohjauskaytossaAsetaAlussijainti()
    {
        // float alussijaintiy = this.PalautaPeliAlueenYOhjausAlueenYylla(sormi.transform.position);
        // transform.position = new Vector3(sormi.transform.position.x, alussijaintiy, 0);

        float alussijaintiy = PalautaPeliAlueenYOhjausAlueenYylla(sormitoisessakamerassa.transform.position);

        /*
        sormitoisessakamerassa.transform.position = new Vector3(transform.position.x, sormiuusisijainti, 0);

        Vector2 screenPosition = sormenkamera.GetComponent<Camera>().WorldToScreenPoint(transform.position);

        Vector3 worlidi = sormenkamera.GetComponent<Camera>().ScreenToWorldPoint(screenPosition);

        sormitoisessakamerassa.transform.position = new Vector3(worlidi.x, sormiuusisijainti, 0);
*/
        Vector3 viewportPos = sormenkamera.GetComponent<Camera>().WorldToViewportPoint(sormitoisessakamerassa.transform.position);

        // Step 2: Convert the viewport position to world position in camera2
        Vector3 worldPosInCamera2 = Camera.main.ViewportToWorldPoint(new Vector3(viewportPos.x, viewportPos.y, Camera.main.nearClipPlane));

        // Step 3: Update only the X position of gameObject2 to align horizontally
        Vector3 newPosition = transform.position;
        newPosition.x = worldPosInCamera2.x;

        //eli joku 10 sekunnin kuolemattomuuspätkä tällä...
        //mutta ei ole hyvä kun pitäis saada liukumaan nätisti tiiliseinää pitkin...
        if (!enabloiboxcollider)
        {
            Vector2 uusimahdollinenpositio = new Vector3(newPosition.x, alussijaintiy);


            //Vector2 boxlocation = new Vector2(0, 0);
            //"tiilivihollinentag"
            bool onkotiilta = onkoTagiaBoxissaAlakaytaTransformia("vihollinen", this.boxCollider2D.size, uusimahdollinenpositio);
            //Debug.Log("onkotiilta=" + onkotiilta);
            if (!onkotiilta)
            {
                transform.position = new Vector3(newPosition.x, alussijaintiy, 0);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            transform.position = new Vector3(newPosition.x, alussijaintiy, 0);
            return true;
        }


        return false;




        //Vector2 boxsize = new Vector2(1f, 1f);


        //      bool onkoTagiaBoxissa(string name, Vector2 boxsize, Vector2 boxlocation, l);


        // Apply the updated position to gameObject2
        //gameObject2.transform.position = newPosition;


        /*
        Vector3 sormiworldi = sormitoisessakamerassa.transform.position;
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(sormiworldi);
        Vector3 worlidi = Camera.main.ScreenToWorldPoint(screenPosition);

        transform.position = new Vector3(worlidi.x, alussijaintiy, 0);
        */

        float alueeny = PalautaPelialueenAlarajaY();


        //  Vector3 minkamera = GetCameraMinWorldPosition();
        //  Vector3 maxkamera = GetCameraMaxWorldPosition();

        // lineRenderer.SetPosition(0, new Vector3(minkamera.x, alueeny,0)); // Start point
        // lineRenderer.SetPosition(1, new Vector3(maxkamera.x, alueeny, 0)); // End point

        float ohjausalueenalaraja = PalautaOhjausalueenAlarajaY();

        //lineRenderer2.SetPosition(0, new Vector3(minkamera.x, ohjausalueenalaraja, 0)); // Start point
        //lineRenderer2.SetPosition(1, new Vector3(maxkamera.x, ohjausalueenalaraja, 0)); // End point



        //  @todoo tahan  private LineRenderer lineRenderer; piirto
        //  ja toki pitää rajoittaa sormiliike

        if (true)
            return true;

        //  sormi.transform.localPosition=new Vector3(0, -5, 0);
        /*
          float screenHeightInWorld = Camera.main.orthographicSize * 2;

          // Calculate half of the screen height
          float halfScreenHeight = screenHeightInWorld / 2;

          // Set the local position of 'sormi' half a screen height down
          sormi.transform.localPosition = new Vector3(0, -halfScreenHeight, 0);
          */
        /*
        float screenHeightInWorld = Camera.main.orthographicSize * 2;
        // float aluksenpieninkohta =
        //     mainCamera.transform.position.y - Camera.main.orthographicSize/2 +
        //     screenHeightInWorld * (prosenttiosuusmikaonvarattuohjaukseenkorkeudessa/100.0f);

        float aluksenpieninkohta = Calculate25PercentAboveBottom(prosenttiosuusmikaonvarattuohjaukseenkorkeudessa);

        // float sormenprosentit = prosenttiosuusmikaonvarattuohjaukseenkorkeudessa;// sormi.transform.position.y / screenHeightInWorld;

        float aluksentilaysuunnassa = screenHeightInWorld * ((100 - prosenttiosuusmikaonvarattuohjaukseenkorkeudessa) / 100.0f);


        //float sormenprosentuaalinenyosuus = sormi.transform.position.y / aluksenpieninkohta;

        float sormiprossat = CalculatePercentageAboveBottom(sormi);
        Debug.Log("sormiprossat=" + sormiprossat);

        //eli siitä 25% pitäis saada 100%
        //ja siitä 0% pitäis saada 0&
        //12.5%=100
        float laskentay = 100.0f / prosenttiosuusmikaonvarattuohjaukseenkorkeudessa;
        float alusjuttu = laskentay * sormiprossat;


        float montakoprossaasormestakaytetty = sormiprossat / prosenttiosuusmikaonvarattuohjaukseenkorkeudessa;

        //        float aluksenysuunnassalisays = aluksentilaysuunnassa * (alusjuttu/100.0f);

        float aluksenysuunnassalisays = aluksentilaysuunnassa * (montakoprossaasormestakaytetty);



        float alussijainti = aluksenpieninkohta + aluksenysuunnassalisays;
        transform.position = new Vector3(sormi.transform.position.x, alussijainti, 0);
        //  Debug.Log("alussijainti=" + alussijainti);
        */
    }


    public float prosenttiosuusalasuoja = 5.0f;
    public float prosenttiosuusmikaonvarattuohjaukseenkorkeudessa = 25.0f;

    //eli
    private float PalautaPelialueenProsentit()
    {
        return 100.0f - prosenttiosuusalasuoja - prosenttiosuusmikaonvarattuohjaukseenkorkeudessa;

    }
    private float PalautaPeliAlueenYOhjausAlueenYylla(Vector3 yohjaus)
    {
        //yohj
        float y = yohjaus.y;
        float koko = PalautaPelialueenYkoko();

        float ohjausprossa = PalautaOhjausAlueenprossa(y);

        float ala = PalautaPelialueenAlarajaY();
        float lisays = koko * (ohjausprossa / 100.0f);
        return ala + lisays;
    }

    public float PalautaOhjausAlueenYPelialueenYylla(Vector3 pelialue)
    {
        float ohjauskoko = PalautaOhjausalueekoko();
        float prosas = PalautaPeliAlueenprossa(pelialue.y);
        float ohjausalaraja = PalautaOhjausalueenAlarajaY();
        float lisays = ohjauskoko * (prosas / 100.0f);

        float sormikoko = PalautaSormenkorkeus();

        return ohjausalaraja + lisays + sormikoko / 2.0f;
    }

    public float PalautaOhjausAlueenxPelialueenxylla(Vector3 pelialue)
    {
        float ohjauskoko = PalautaOhjausalueekoko();
        float prosas = PalautaPeliAlueenprossa(pelialue.y);
        float ohjausalaraja = PalautaOhjausalueenAlarajaX();
        float lisays = ohjauskoko * (prosas / 100.0f);

        float sormikoko = PalautaSormenkorkeus();

        return ohjausalaraja + lisays + sormikoko / 2.0f;
    }



    private float PalautaPeliAlueenprossa(float pelialueeny)
    {
        //eli al
        float pelialueenalaraja = PalautaPelialueenAlarajaY();
        //float pelialueenylaraja = PalautaPelialueenAlarajaY();
        float koko = PalautaPelialueenYkoko();


        float valimatka = Mathf.Abs(pelialueeny - pelialueenalaraja);

        float p = valimatka / koko;
        return p * 100.0f;

    }

    private float PalautaPeliAlueenprossaX(float pelialueenx)
    {
        //eli al
        float pelialueenalaraja = PalautaPelialueenAlarajaY();
        //float pelialueenylaraja = PalautaPelialueenAlarajaY();
        float koko = PalautaPelialueenYkoko();


        float valimatka = Mathf.Abs(pelialueenx - pelialueenalaraja);

        float p = valimatka / koko;
        return p * 100.0f;

    }


    private float PalautaOhjausAlueenprossa(float ohjausy)
    {
        //eli al
        float ohjausalueenalaraja = PalautaOhjausalueenAlarajaY();
        float koko = PalautaOhjausalueekoko();
        float spritekoko = PalautaSormenkorkeus();
        // + spritekoko/2.0f

        float valimatka = Mathf.Abs(ohjausy + spritekoko / 2.0f - ohjausalueenalaraja);
        float p = valimatka / koko;
        return p * 100.0f;

    }

    private float PalautaSormenkorkeus()
    {

        float objectHeight = sormitoisessakamerassa.GetComponent<Renderer>().bounds.size.y;
        return objectHeight;
    }

    private float PalautaSormenleveys()
    {

        float objectHeight = sormitoisessakamerassa.GetComponent<Renderer>().bounds.size.x;
        return objectHeight;
    }

    private float PalautaAluksenKorkeus()
    {
        float aluskorkeus = m_SpriteRenderer.bounds.size.y;
        return aluskorkeus;
    }


    private float PalautaPelialueenAlarajaY()
    {
        // float pelialueenalarajay=PalautaOhjausalueenYlarajaY();
        // Gizmos.color = Color.cyan;

        Vector3 kameramin = GetCameraMinWorldPosition();
        Vector3 kameramax = GetCameraMaxWorldPosition();
        //Gizmos.DrawLine(new Vector3(kameramin.x, pelialueenalarajay,0), new Vector3(kameramax.x, pelialueenalarajay, 0));

        //Debug.DrawRay(new Vector3(kameramin.x, pelialueenalarajay, 0), Vector2.right * 10.0f, Color.red);
        //pelialueenalarajatallessa = pelialueenalarajay;
        //return pelialueenalarajay;
        float kameraminimi = PalautaKameranMinY();
        return kameraminimi;

    }

    private float PalautaPelialueenAlarajaX()
    {
        // float pelialueenalarajay=PalautaOhjausalueenYlarajaY();
        // Gizmos.color = Color.cyan;

        Vector3 kameramin = GetCameraMinWorldPosition();
        Vector3 kameramax = GetCameraMaxWorldPosition();
        //Gizmos.DrawLine(new Vector3(kameramin.x, pelialueenalarajay,0), new Vector3(kameramax.x, pelialueenalarajay, 0));

        //Debug.DrawRay(new Vector3(kameramin.x, pelialueenalarajay, 0), Vector2.right * 10.0f, Color.red);
        //pelialueenalarajatallessa = pelialueenalarajay;
        //return pelialueenalarajay;
        float kameraminimi = PalautaKameranMinX();
        return kameraminimi;

    }

    private float pelialueenalarajatallessa = 0.0f;

    private float PalautaPelialueenYkoko()
    {
        //float a =
        PalautaPelialueenYlarajaY();
        //float b= PalautaPelialueenAlarajaY();

        //return Mathf.Abs(a - b);

        float ala = PalautaKameranMinY();
        float yla = PalautaKameranMaxY();
        return Mathf.Abs(ala - yla);
    }

    private float PalautaPelialueenYlarajaY()
    {
        return PalautaKameranMaxY();

    }

    public float PalautaOhjausalueekoko()
    {
        float a = PalautaOhjausalueenYlarajaY();
        float b = PalautaOhjausalueenAlarajaY();
        return Mathf.Abs(a - b);

        // return sormiKameraController.PalautaOhjausalueekoko();

    }

    public float PalautaOhjausalueenAlarajaY()
    {
        float r = PalautaOhjausKameranMinY() + (prosenttiosuusalasuoja / 100.0f) * PalautaOhjausKameranKorkeus();
        return r;
    }
    public float PalautaOhjausalueenAlarajaX()
    {
        float r = PalautaOhjausKameranMinX();// + (prosenttiosuusalasuoja / 100.0f) * PalautaKameranKorkeus();
        return r;
    }

    public float PalautaOhjausalueenYlarajaX()
    {
        float r = PalautaOhjausKameranMaxX();// + (prosenttiosuusalasuoja / 100.0f) * PalautaKameranKorkeus();
        return r;
    }

    public float PalautaOhjausKameranMinX()
    {
        //float screenHeightInWorld = 2 * Camera.main.orthographicSize; // Total height
        //float bottomY = Camera.main.transform.position.y - Camera.main.orthographicSize; // Bottom of the screen
        //return bottomY;

        float r = GetOhjausCameraMinWorldPosition().x;
        return r;
    }
    public float PalautaOhjausKameranMaxX()
    {
        //float screenHeightInWorld = 2 * Camera.main.orthographicSize; // Total height
        //float bottomY = Camera.main.transform.position.y - Camera.main.orthographicSize; // Bottom of the screen
        //return bottomY;

        float r = GetOhjausCameraMaxWorldPosition().x;
        return r;
    }


    public float PalautaOhjausalueenYlarajaY()
    {
        float r =
            PalautaOhjausKameranMinY();
        float a = ((prosenttiosuusalasuoja + prosenttiosuusmikaonvarattuohjaukseenkorkeudessa) / 100.0f) * PalautaOhjausKameranKorkeus();



        return r + a;
    }
    public float PalautaOhjausKameranKorkeus()
    {
        float screenHeightInWorld = 2 * sormikamera.orthographicSize; // Total height
        return screenHeightInWorld;
    }

    public float PalautaOhjausKameranMinY()
    {
        //float screenHeightInWorld = 2 * Camera.main.orthographicSize; // Total height
        //float bottomY = Camera.main.transform.position.y - Camera.main.orthographicSize; // Bottom of the screen
        //return bottomY;

        float r = GetOhjausCameraMinWorldPosition().y;
        return r;
    }

    public Vector3 GetOhjausCameraMinWorldPosition()
    {

        // Calculate the camera's dimensions in world space
        float height = sormikamera.orthographicSize * 2;
        float width = height * sormikamera.aspect;

        // Bottom-left corner of the camera's view in world space
        Vector3 minWorldPosition = sormikamera.transform.position - new Vector3(width / 2, height / 2, 0);

        return minWorldPosition;
    }


    public Vector3 GetOhjausCameraMaxWorldPosition()
    {

        // Calculate the camera's dimensions in world space
        float height = sormikamera.orthographicSize * 2;
        float width = height * sormikamera.aspect;

        // Bottom-left corner of the camera's view in world space
        Vector3 minWorldPosition = sormikamera.transform.position + new Vector3(width / 2, height / 2, 0);

        return minWorldPosition;
    }

    private float PalautaKameranMinY()
    {
        //float screenHeightInWorld = 2 * Camera.main.orthographicSize; // Total height
        //float bottomY = Camera.main.transform.position.y - Camera.main.orthographicSize; // Bottom of the screen
        //return bottomY;

        float r = GetCameraMinWorldPosition().y;
        return r;
    }

    private float PalautaKameranMinX()
    {
        //float screenHeightInWorld = 2 * Camera.main.orthographicSize; // Total height
        //float bottomY = Camera.main.transform.position.y - Camera.main.orthographicSize; // Bottom of the screen
        //return bottomY;

        float r = GetCameraMinWorldPosition().x;
        return r;
    }





    private float PalautaKameranMaxY()
    {
        float r = GetCameraMaxWorldPosition().y;
        return r;
    }




    private float PalautaKameranKorkeus()
    {
        float screenHeightInWorld = 2 * Camera.main.orthographicSize; // Total height
        return screenHeightInWorld;
    }


    float Laskemontakoprosenttiaalusonalarajanylapuolella(float alaraja, float aluksenruututilankorkeus)
    {
        // float screenHeightInWorld = 2 * Camera.main.orthographicSize; // Total height
        float alarajanylitys = transform.position.y - alaraja;

        return alarajanylitys / aluksenruututilankorkeus;
    }

    float Calculate25PercentAboveBottom(float sormiprossa)
    {
        float screenHeightInWorld = 2 * Camera.main.orthographicSize; // Total height
        float bottomY = Camera.main.transform.position.y - Camera.main.orthographicSize; // Bottom of the screen
        return bottomY + sormiprossa / 100.0f * screenHeightInWorld; // 25% above the bottom
    }



    float CalculatePercentageAboveBottom2(GameObject obj)
    {
        // Get the Y position of the object in world space




        //  float objectY = obj.GetComponent<RectTransform>().position.y;

        //Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, obj.GetComponent<RectTransform>().position);
        //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        float objectY = obj.transform.position.y;
        //  worldPosition.z = uiElement.position.z; // Preserve the UI element's z-coordinate


        // Get the bottom and top Y world positions of the screen
        float bottomY = mainCamera.transform.position.y - mainCamera.orthographicSize;
        float topY = mainCamera.transform.position.y + mainCamera.orthographicSize;

        // Calculate the percentage above the bottom
        float percentage = ((objectY - bottomY) / (topY - bottomY)) * 100f;

        // Clamp the percentage between 0% and 100%
        return Mathf.Clamp(percentage, 0f, 100f);
    }

    float CalculatePercentageAboveBottom(GameObject obj)
    {
        // Get the Y position of the object in world space
        float objectY = obj.transform.position.y;

        float objectHeight = obj.GetComponent<Renderer>().bounds.size.y;

        objectY = objectY - objectHeight / 2;

        // Get the bottom and top Y world positions of the screen
        float bottomY = mainCamera.transform.position.y - mainCamera.orthographicSize;
        float topY = mainCamera.transform.position.y + mainCamera.orthographicSize;

        // Check if the object is below the bottom of the screen
        if (objectY < bottomY)
        {
            return 0f; // The object is below the screen, so return 0%
        }

        // Calculate the percentage above the bottom
        float percentage = ((objectY - bottomY) / (topY - bottomY)) * 100f;

        // Clamp the percentage between 0% and 100%
        return Mathf.Clamp(percentage, 0f, 100f);
    }

    float CalculatePercentageAboveB(GameObject obj)
    {
        // Get the object's Y position in world space (center of the object)
        float objectY = obj.transform.position.y;

        // Calculate the object's height in world space
        float objectHeight = obj.GetComponent<Renderer>().bounds.size.y;

        // Get the bottom and top Y world positions of the screen
        float bottomY = mainCamera.transform.position.y - mainCamera.orthographicSize;
        float topY = mainCamera.transform.position.y + mainCamera.orthographicSize;

        // Calculate the object's bottom position (taking height into account)
        float objectBottomY = objectY - (objectHeight / 2);

        // Check if the object is fully below the screen
        if (objectBottomY < bottomY)
        {
            return 0f; // The object is fully below the screen, so return 0%
        }

        // Calculate the object's top position (taking height into account)
        float objectTopY = objectY + (objectHeight / 2);

        // If the top of the object is above the screen's top, clamp it
        float effectiveTopY = Mathf.Min(objectTopY, topY);

        // Calculate the percentage above the bottom
        float percentage = ((effectiveTopY - bottomY) / (topY - bottomY)) * 100f;

        // Clamp the percentage between 0% and 100%
        return Mathf.Clamp(percentage, 0f, 100f);
    }


    TextMeshProUGUI keskellaTextMeshProUGUI;

    TextMeshProUGUI ruudunvasenylakulmatekstiTextMeshProUGUI;

    float minX;
    float maxX;
    float minY;
    float maxY;

    public void SetkeskellaTextMeshProUGUI(string text)
    {
        keskellaTextMeshProUGUI.text = text;
    }
    public void SetruudunvasenylakulmatekstiTextMeshProUGUI(string text)
    {
        ruudunvasenylakulmatekstiTextMeshProUGUI.text = text;
    }


    public GameObject damageMittari;
    private DamagemittariController damagemittariController;


    private void TeeOptioni()
    {
        Vector3 vektori =
new Vector3(
m_Rigidbody2D.position.x, m_Rigidbody2D.position.y, 0);

        GameObject instanssiOption = Instantiate(instanssiOption1, vektori, Quaternion.identity);
        //  instanssiOption.transform.SetParent(transform);


        OptionController myScript = instanssiOption.GetComponent<OptionController>();
        if (myScript != null)
        {
            myScript.jarjestysnro = nykyinenoptioidenmaara;
            optionControllerit.Add(myScript);
        }
        else
        {
            Debug.Log("ei ole controlleria");
        }
    }




    float gvarinalkutilanne;

    float gvarinlopputilanne = 0.0f;

    private float PalautaGvari()
    {
        float prosentti = damagenmaara / maksimimaaradamageajokakestetaan;

        float ero = gvarinalkutilanne - gvarinlopputilanne;
        float summaerosta = ero * prosentti;

        float arvo = gvarinalkutilanne - summaerosta;
        return arvo;

    }


    //public float maksimimaaradamageajokakestetaan = 100.0f;
    //public float damagenmaara = 0.0f;
    //public float damagenmaarakunammusosuus = 1.0f;




    private List<BonusButtonController> bbc = new List<BonusButtonController>();


    private void OnkoAndroidi()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("android");

        }
        else
        {
            Debug.Log("EI OLE android");

        }
    }

    private bool osuudetasetettu = false;

    // Update is called once per frame


    void Update()
    {

        /*

        if (forcefieldkaytossa)
        {
            AsetaForceFieldiNakyviin();
        }
        */

        AsetaForceFieldiButtoni();

        if (uusiohjauskaytossa & !osuudetasetettu)
        {
            //   sormi.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            //prosenttiosuusmikaonvarattuohjaukseenkorkeudessa = CalculatePercentageAboveBottom2(alamaksiminMaarittava);
            //säätää myös pääkameran koon
            // if (sormiKameraController == null)
            // {
            //     Debug.Log("sormiKameraController==null");
            // }

            //  sormiKameraController.SetOsuudet(prosenttiosuusalasuoja, prosenttiosuusmikaonvarattuohjaukseenkorkeudessa,
            //      mainCamera);


            //  UusiohjauskaytossaAsetaSormi();
            osuudetasetettu = true;
        }

        if (restartscene)
        {
            return;
        }

        if (Input.GetKey(KeyCode.P) || CrossPlatformInputManager.GetButtonDown("Pause"))
        {
            Debug.Log("pausepressed");
            TogglePause();
        }

        if (isPaused && (


            Input.GetKey("escape") || CrossPlatformInputManager.GetButtonDown("Quit")))
        {
            //  Debug.Log("Application would quit now.");

            //Application.Quit();
            Debug.Log("Level1");

            SceneManager.LoadScene("StartMenu");
            return;

        }

        if (isPaused)
        {
            return;
        }
        PaivitaStoptime();

        SetEnabloiboxcollider(enabloiboxcollider);//mjM OTA TAMA OPIS
        if (Input.GetKey(KeyCode.N) || CrossPlatformInputManager.GetButtonDown("Bonus"))
        {
            //   Debug.Log("BonusButtonPressed");
            BonusButtonPressed();
        }

        //m_Rigidbody2D.velocity = new Vector2(0, 0);
        m_Rigidbody2D.velocity = Vector2.zero;





        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            //Vector3 touchPosition = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));
            Vector3 touchPosition = touch.position;
            switch (touch.phase)
            {

                case UnityEngine.TouchPhase.Began:
                    CheckIfTouched(touchPosition);
                    break;

                case UnityEngine.TouchPhase.Moved:
                    if (_isDragging)
                    {
                        MoveObject(touchPosition);
                    }
                    break;

                case UnityEngine.TouchPhase.Ended:
                    _isDragging = false;
                    break;

                case UnityEngine.TouchPhase.Stationary:
                    if (_isDragging)
                    {
                        MoveObject(touchPosition);
                    }
                    break;
            }
        }
        // Handle mouse input for PC testing
        else if (!android && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            CheckIfTouched(mousePosition);
        }
        else if (!android && Input.GetMouseButton(0) && _isDragging)
        {
            MoveObject(Input.mousePosition);
        }
        else if (!android && Input.GetMouseButtonUp(0))
        {
            //     Debug.Log("Input.GetMouseButtonUp(0)");
            _isDragging = false;
        }


        /*
        Vector2 moveDirection = moveInputActionReference.action.ReadValue<Vector2>();


        float moveDirectionAlas = moveDownInputActionReference.action.ReadValue<float>();

        float moveDirectionUp = moveUpInputActionReference.action.ReadValue<float>();
        */



        //    m_Rigidbody2D.velocity = new Vector2(0.0f, 0.0f);



        float nappiHorizontal = Input.GetAxisRaw("Horizontal");
        if (nappiHorizontal == 0.0f)
        {
            // nappiHorizontal = moveDirection.x;
        }

        //float nappiHorizontal = moveDirection.x;


        oikeaNappiPainettu = (nappiHorizontal > 0.0f); // > 0 for right, < 0 for left
        vasenNappiPainettu = (nappiHorizontal < 0.0f);

        /*
        if (!oikeaNappiPainettu)
            oikeaNappiPainettu = (joystick.Horizontal > 0.1f);


        if (!vasenNappiPainettu)
            vasenNappiPainettu = (joystick.Horizontal < -0.1f);
        */


        float nappiVertical = Input.GetAxisRaw("Vertical");
        if (nappiVertical == 0.0f)
        {
            //   nappiVertical = moveDirection.y;
        }

        // float nappiVertical = moveDirection.y;

        //float nappiVertical = -moveDirectionAlas;

        //if (nappiVertical == 0.0)
        //{
        //    nappiVertical = moveDirectionUp;
        //}


        ylosNappiPainettu = nappiVertical > 0; // > 0 for right, < 0 for left
        alasNappiPainettu = nappiVertical < 0;

        /*
        if (!ylosNappiPainettu)
            ylosNappiPainettu = (joystick.Vertical > 0.1f);

        if (!alasNappiPainettu)
            alasNappiPainettu = (joystick.Vertical < -0.1f);
*/
        //spaceNappiaPainettu = Input.GetButton ("Jump");
        //spaceNappiAlhaalla = Input.GetButtonDown ("Jump");
        //spaceNappiYlhaalla = Input.GetButtonUp ("Jump");

        //spaceNappiaPainettu = Input.GetKeyDown (KeyCode.Space);
        // Input.GetKey

        //CrossPlatformInputManager.GetButton

        if (autofire || Input.GetKey(KeyCode.Space) || CrossPlatformInputManager.GetButton("Jump"))
        {
            //Shoot ();
            //		Debug.Log ("space painettu " );
            spaceNappiaPainettu = true;

        }
        else
        {
            //		Debug.Log ("space ei painettu ");
            spaceNappiaPainettu = false;
        }




        //  transform.position += new Vector3(0.4f, 0f, 0f) * Time.deltaTime;//sama skrolli kuin kamerassa

        TallennaSijaintiSailytaVainKymmenenViimeisinta();
        if (tutkaileAsioitaActivated)
        {
            TutkaileAsioita(); //tässä tutkaillaan miten kaukana on jos liian kaukana niin disable


        }

    }
    public bool tutkaileAsioitaActivated = true;
    public float tutkailunSykli = 5f;

    private void TutkaileAsioita()
    {
        timeSinceLastCheck += Time.deltaTime; // Increment the elapsed time

        // If 5 seconds have passed, perform the logic
        if (timeSinceLastCheck >= tutkailunSykli)
        {
            // Reset the time counter and run the check
            timeSinceLastCheck = 0f;
            PerformDistanceCheck(); // Run the distance check synchronously every 5 seconds
        }
    }


    public bool isPaused = false;

    public void TogglePause()
    {
        if (isPaused)
        {
            GameObject myObject = GameObject.Find("PauseButtonKaytossa");
            PauseButtonControlleri s = myObject.GetComponent<PauseButtonControlleri>();
            s.setPauseImage(false);

            ResumeGame();
        }
        else
        {

            GameObject myObject = GameObject.Find("PauseButtonKaytossa");

            PauseButtonControlleri s = myObject.GetComponent<PauseButtonControlleri>();
            s.setPauseImage(true);
            PauseGameAction();


        }
    }

    void PauseGameAction()
    {
        GameManager.Instance.isPaused = true;
        Time.timeScale = 0f; // Stop game time
        isPaused = true;
        // Optional: Show pause menu UI
        ad.TaustaMusiikkiStop();

        GameObject myObject = GameObject.Find("QuitButtonKaytossa");
        myObject.GetComponent<Image>().enabled = true;


    }

    void ResumeGame()
    {
        GameManager.Instance.isPaused = false;
        Time.timeScale = 1f; // Resume game time
        isPaused = false;
        // Optional: Hide pause menu UI

        ad.TaustaMusiikkiPlay();
        GameObject myObject = GameObject.Find("QuitButtonKaytossa");
        myObject.GetComponent<Image>().enabled = false;
    }





    /*
    private void CheckIfTouched(Vector3 inputPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(inputPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.transform == transform)
        {
            _isDragging = true;
            _offset = transform.position - inputPosition;
        }
    }
*/

    public Vector2 sormipaikkakerto = new Vector2(1.3f, 1.3f);

    public LayerMask sormenlayermaski;

    private void CheckIfTouched(Vector3 inputPosition)
    {
        //  Debug.Log("CheckIfTouched");

        // Convert input position to world space
        //Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, 0));

        Vector3 worldPosition = sormenkamera.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, 0));

        /*
        // Check if the input position overlaps a 2D collider
        Collider2D hit = Physics2D.OverlapPoint(worldPosition);

        //  Debug.Log("hitti=" + hit);
        if (hit != null)
        {
            bool onko = hit.transform == transform;
            //     Debug.Log("onko=" + onko);
        }

        if (hit != null && hit.transform == transform)
        {
            _isDragging = true;
            // Calculate offset between the object and touch/mouse position
            _offset = transform.position - worldPosition;
        }
        */
        string tagii = "alustag";
        if (uusiohjauskaytossa)
        {
            tagii = "sormitag";
        }

        Vector2 boxSize = boxCollider2D.size * transform.localScale;

        boxSize = boxSize * sormipaikkakerto;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(worldPosition, boxSize, sormenlayermaski);



        foreach (Collider2D collider in colliders)
        {
            //
            if (collider.gameObject.tag.Contains(tagii))
            {
                _isDragging = true;
                // Calculate offset between the object and touch/mouse position
                _offset = collider.gameObject.transform.position - worldPosition;
                // _offset = new Vector3(+100, 0, 0);

                kosketuksenaloitusaika = Time.time;
                break;
            }
        }


    }


    //Vector3 viimeisin = Vector3.zero;

    //public float offsettisormenjaaluksenvalilla = 200.0f;

    //   public float sormenoffsettiprosenteissaRuudunleveydesta = 10.0f;

    //  private float offsetvalue = 100.0f;
    // public float offsetvalueprocentsOffScreenWidth = 10.0f;


    //  public float offsetvalueprocentsOffScreenWidthInRightSide = 100.0f;

    //   float offsetvalue;

    //   float offsetvalueRight;


    //   public float offsetprocentvalueup = 0.0f;


    //    public float offsetkertokorjain = 0.5f;

    //   public float offsetinkerto = 2.0f;





    public float ruudunvasemmanreunansuojaalueProsentti = 5.0f;

    public float erikoisalueenoikeareunaProsentti = 15f;
    public float offsetmaaraProsentti = 10;
    public float offsetprosentinminimi = 0.0f;

    private void MoveObject(Vector3 screenpositionSormen)
    {

        if (uusiohjauskaytossa)
        {
            //Vector2 yritysWorldpoint = RajoitaMoveaPalautaWorldPoint(screenpositionSormen);
            //yritysWorldpoint = new Vector2(yritysWorldpoint.x, yritysWorldpoint.y);

            Vector3 worldPosition = sormenkamera.GetComponent<Camera>().ScreenToWorldPoint(screenpositionSormen);
            Vector3 sormikohta =
            sormitoisessakamerassa.transform.position;
            TryMove(worldPosition);
            bool liikkuiko = UusiohjauskaytossaAsetaAlussijainti();
            if (!liikkuiko)
            {
                sormitoisessakamerassa.transform.position = sormikohta;
            }
        }
        else
        {
            float spriteWidth = m_SpriteRenderer.bounds.size.x;
            float spriteHeight = m_SpriteRenderer.bounds.size.y;
            Bounds bounds = m_SpriteRenderer.bounds;
            Vector3 worldBottomLeft = new Vector3(bounds.min.x, bounds.min.y, bounds.center.z);
            Vector3 worldTopRight = new Vector3(bounds.max.x, bounds.max.y, bounds.center.z);

            // Convert world space corners to screen space
            Vector3 screenBottomLeft = Camera.main.WorldToScreenPoint(worldBottomLeft);
            Vector3 screenTopRight = Camera.main.WorldToScreenPoint(worldTopRight);

            // Calculate width and height in screen space
            float screenWidth = screenTopRight.x - screenBottomLeft.x;
            float screenHeight = screenTopRight.y - screenBottomLeft.y;


            //offsetvalue = Screen.width * (offsetvalueprocentsOffScreenWidth / 100f);
            float ruudunvasemmanreunansuojaalue = Screen.width * ruudunvasemmanreunansuojaalueProsentti / 100.0f;
            float erikoisalueenoikeareuna = Screen.width * erikoisalueenoikeareunaProsentti / 100.0f;
            float offsetmaara = Screen.width * offsetmaaraProsentti / 100.0f;

            float offsetmaaraminimi = Screen.width * offsetprosentinminimi / 100.0f;


            Vector3 screeni;

            if (screenpositionSormen.x <= ruudunvasemmanreunansuojaalue)
            {
                //screenpositionSormen.x = ruudunvasemmanreunansuojaalue;
                //screeni = screenpositionSormen;

                screenpositionSormen.x = ruudunvasemmanreunansuojaalue + offsetmaaraminimi;
                screeni = screenpositionSormen;




            }
            else if (screenpositionSormen.x > ruudunvasemmanreunansuojaalue && screenpositionSormen.x <= erikoisalueenoikeareuna)
            {
                //prosenttilaseknta
                float erikoisalueenleveys = (erikoisalueenoikeareuna - ruudunvasemmanreunansuojaalue);
                float sormisiirrettynavasemmalle = screenpositionSormen.x - ruudunvasemmanreunansuojaalue;
                float erikoisaluemaksimisiirrettyvasemmalle = erikoisalueenoikeareuna - ruudunvasemmanreunansuojaalue;

                float prossa = sormisiirrettynavasemmalle / erikoisaluemaksimisiirrettyvasemmalle;


                float offsettiprosentilla = offsetmaara * prossa;
                if (offsettiprosentilla < offsetmaaraminimi)
                {
                    offsettiprosentilla = offsetmaaraminimi;
                }

                screeni = new Vector3(screenpositionSormen.x + offsettiprosentilla, screenpositionSormen.y, 0);

            }
            else
            {
                screeni = new Vector3(screenpositionSormen.x + offsetmaara, screenpositionSormen.y, 0);
            }
            Vector2 yritysWorldpoint = RajoitaMoveaPalautaWorldPoint(screeni);

            // transform.position = worldPosition + _offset;
            TryMove(yritysWorldpoint);

        }

    }

    private Vector2 RajoitaMoveaPalautaWorldPoint(Vector3 screeni)
    {
        float spriteWidth = m_SpriteRenderer.bounds.size.x;
        float spriteHeight = m_SpriteRenderer.bounds.size.y;

        minX = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)).x + spriteWidth / 2;
        maxX = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, mainCamera.nearClipPlane)).x - spriteWidth / 2;
        minY = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)).y + spriteHeight / 2;
        maxY = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, mainCamera.nearClipPlane)).y - spriteHeight / 2;

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screeni);


        float clampedX = Mathf.Clamp(worldPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(worldPosition.y, minY, maxY);


        Vector2 yritys = new Vector2(clampedX, clampedY);

        if (alamaksiminMaarittava != null)
        {
            if (yritys.y < alamaksiminMaarittava.transform.position.y)
            {
                yritys.y = alamaksiminMaarittava.transform.position.y;
            }
        }
        if (ylamaksiminMaarittava != null)
        {
            if (yritys.y > ylamaksiminMaarittava.transform.position.y)
            {
                yritys.y = ylamaksiminMaarittava.transform.position.y;
            }
        }

        return yritys;
    }

    private void AsetaSijainti(Vector2 transformposition)
    {
        if (uusiohjauskaytossa)
        {
            return;
        }

        //Vector3 worldPosition = mainCamera.WorldToScreenPoint(transformposition);
        Vector3 screenpositionSormen = Camera.main.WorldToScreenPoint(transformposition);
        float ruudunvasemmanreunansuojaalue = Screen.width * ruudunvasemmanreunansuojaalueProsentti / 100.0f;

        Vector2 screeni;
        if (screenpositionSormen.x <= ruudunvasemmanreunansuojaalue)
        {
            screenpositionSormen.x = ruudunvasemmanreunansuojaalue;
            screeni = new Vector2(screenpositionSormen.x, screenpositionSormen.y);

        }
        else
        {
            screeni = Camera.main.WorldToScreenPoint(transformposition);
        }
        Vector2 rajoitettuWorldpoint = RajoitaMoveaPalautaWorldPoint(screeni);
        //Vector2 worldi = Camera.main.ScreenToWorldPoint(screeni);
        TryMove(rajoitettuWorldpoint);
        //transform.position = rajoitettuWorldpoint;

        /*
        if (true)
            return;

        if (mainCamera == null) mainCamera = Camera.main;
        if (m_SpriteRenderer == null) m_SpriteRenderer = GetComponent<SpriteRenderer>();
        if (m_Rigidbody2D == null) m_Rigidbody2D = GetComponent<Rigidbody2D>();

        float halfWidth = m_SpriteRenderer.bounds.extents.x;
        float halfHeight = m_SpriteRenderer.bounds.extents.y;

        float camHeight = mainCamera.orthographicSize;
        float camWidth = mainCamera.aspect * mainCamera.orthographicSize;

        Vector3 pos = transform.position;

        // Calculate camera bounds
        float minX = mainCamera.transform.position.x - camWidth;
        float maxX = mainCamera.transform.position.x + camWidth;
        float minY = mainCamera.transform.position.y - camHeight;
        float maxY = mainCamera.transform.position.y + camHeight;

        // Convert the 100-pixel screen-space width to world-space

        float offsetvalue = Screen.width * (offsetvalueprocentsOffScreenWidth / 100f);

        // float offsetvalue = cameraWidth * (offsetvalueprocentsOffScreenWidth / 100f);


        //  Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(inputPosition.x + offsetvalue, inputPosition.y, 0));


        float restrictedWidthInWorld = mainCamera.ScreenToWorldPoint(new Vector3(offsetvalue, 0)).x - mainCamera.ScreenToWorldPoint(Vector3.zero).x;

        // Adjust the minimum X to account for the restricted area
        float restrictedMinX = minX + restrictedWidthInWorld;

        // Clamp X and Y within bounds, considering the restricted area
        // pos.x = Mathf.Clamp(pos.x, restrictedMinX + halfWidth, maxX - halfWidth); OLI KAYTOSS

      
        pos.y = Mathf.Clamp(pos.y,
                            Mathf.Max(minY + halfHeight, alamaksiminMaarittava.transform.position.y),
                            ylamaksiminMaarittava != null
                                ? Mathf.Min(maxY - halfHeight, ylamaksiminMaarittava.transform.position.y)
                                : maxY - halfHeight);
    


        // Reset velocity if hitting vertical boundaries
        if (pos.y == alamaksiminMaarittava.transform.position.y ||
            (ylamaksiminMaarittava != null && pos.y == ylamaksiminMaarittava.transform.position.y))
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0.0f);
        }

        // Apply position
        transform.position = pos;
        */
    }



    void OnDrawGizmos()
    {
        /*
        Vector3 kameramin = GetCameraMinWorldPosition();
        Debug.DrawRay(new Vector3(kameramin.x, pelialueenalarajatallessa, 0), Vector2.right * 10.0f, Color.red);


        float ohjausala = PalautaOhjausalueenAlarajaY();
        Debug.DrawRay(new Vector3(kameramin.x, ohjausala, 0), Vector2.right * 10.0f, Color.blue);
        */

        if (mainCamera != null && sormikamera != null)
        {
            Debug.DrawRay(new Vector3(-1000, PalautaOhjausalueenAlarajaY(), 0), Vector2.right * 10000.0f, Color.blue);
            Debug.DrawRay(new Vector3(-1000, PalautaOhjausalueenYlarajaY(), 0), Vector2.right * 10000.0f, Color.red);


        }



        //    // Draws a 5 unit long red line in front of the object
        //    Gizmos.color = Color.red;
        //    Vector3 direction = transform.TransformDirection(Vector3.forward) * 50;
        //    Gizmos.DrawRay(transform.position, direction);

        if (boxCollider2D != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, boxCollider2D.size * transform.localScale * sormipaikkakerto);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, boxCollider2D.size * transform.localScale);

            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(tormayskohta, tormayskoko);


        }
        /*
             float minXkamera = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)).x;
             float maxXkamera = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, mainCamera.nearClipPlane)).x;
             float minYkamera = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)).y;
             float maxYkamera = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, mainCamera.nearClipPlane)).y;

             Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(ruudunvasemmanreunansuojaalue, 0, Camera.main.nearClipPlane));
             Vector3 worldEnd = Camera.main.ScreenToWorldPoint(new Vector3(ruudunvasemmanreunansuojaalue, Screen.height, Camera.main.nearClipPlane));

             Gizmos.DrawLine(worldStart, worldEnd);

             Gizmos.color = Color.cyan;
             Vector3 worldStart2 = Camera.main.ScreenToWorldPoint(new Vector3(erikoisalueenoikeareuna, 0, Camera.main.nearClipPlane));
             Vector3 worldEnd2 = Camera.main.ScreenToWorldPoint(new Vector3(erikoisalueenoikeareuna, Screen.height, Camera.main.nearClipPlane));

             Gizmos.DrawLine(worldStart2, worldEnd2);
             */

    }

    public Vector2 tormayskoko = new Vector2(0.1f, 01f);

    private Vector2 tormayskohta = Vector2.zero;



    public Vector2 kerto = new Vector2(1.2f, 1.2f);

    //public float stepSizeTrytomove = 0.02f;

    public float aikakosketuksestajolloinmovespeedonHitaampi = 2.0f;

    private float kosketuksenaloitusaika = 0.0f;
    public float movespeedkerroinalussa = 0.5f;
    void TryMove(Vector2 targetPosition)
    {

        /* turhaa
        // Calculate the direction and total distance to move
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        float totalDistance = Vector2.Distance(transform.position, targetPosition);

        // Step size for progressive movement checks
        float stepSize = stepSizeTrytomove; // Adjust for precision (smaller values are more precise but slower)

        Vector2 currentPosition = transform.position;
        //Vector2 boxSize = boxCollider2D.size * transform.localScale;
        //boxSize = boxSize / 10;
        //boxSize = boxSize * kerto;
        Vector2 boxSize = tormayskoko;

        bool rajayta = false;
        // Incrementally check for collisions along the path
        for (float traveled = 0; traveled < totalDistance; traveled += stepSize)
        {
            // Calculate the next step position
            Vector2 nextPosition = currentPosition + direction * stepSize;

            // Perform an OverlapBox check at the next position
            Collider2D[] colliders = Physics2D.OverlapBoxAll(nextPosition, boxSize, sormenlayermaski);

            bool isBlocked = false;
            foreach (Collider2D collider in colliders)
            {
                //
                if (collider.gameObject.tag.Contains("tiili") || collider.gameObject.tag.Contains("laatikkovihollinen"))
                {
                    tormayskohta = nextPosition;
                    isBlocked = true;
                    break;
                }
            }

            // Stop movement if a collision is detected
            if (isBlocked)
            {
                Debug.Log("Movement blocked before reaching the target.");
                rajayta = true;
                break;
            }

            // Update currentPosition to the next step
            currentPosition = nextPosition;
        }
        */



        //    m_Rigidbody2D.MovePosition(Vector2.MoveTowards(transform.position, currentPosition, moveSpeed * Time.deltaTime));



        //public float aikakosketuksestajolloinmovespeedonHitaampi = 1.0f;

        //private float kosketuksenaloitusaika = 0.0f;

        float nykyaika = Time.time;

        float movespeedjotakaytetaan = moveSpeed;
        float erotus = nykyaika - kosketuksenaloitusaika;
        if (erotus < aikakosketuksestajolloinmovespeedonHitaampi)
        {
            movespeedjotakaytetaan = movespeedkerroinalussa * moveSpeed;
        }

        /*
        if (rajayta)
        {
            Vector2 kohta = Vector2.MoveTowards(transform.position, tormayskohta, movespeedjotakaytetaan * Time.deltaTime);
            transform.position = kohta;
            damagenmaara += maksimimaaradamageajokakestetaan;
            PaivitaDamagePalkkia();
        }
        else
        {
            */

        if (uusiohjauskaytossa)
        {
            //Vector2 kohta = Vector2.MoveTowards(transform.position, targetPosition, movespeedjotakaytetaan * Time.deltaTime);

            float ohjausalaraja = PalautaOhjausalueenAlarajaY();

            float aluskorkeus = PalautaAluksenKorkeus();

            ohjausalaraja = ohjausalaraja - (PalautaSormenkorkeus() / 2.0f) + (aluskorkeus / 2.0f);

            if (targetPosition.y < ohjausalaraja)
            {
                targetPosition = new Vector3(targetPosition.x, ohjausalaraja, 0);
            }

            float ohjausylaraja = PalautaOhjausalueenYlarajaY();
            ohjausylaraja = ohjausylaraja - (PalautaSormenkorkeus() / 2.0f) - aluskorkeus / 2.0f;
            if (targetPosition.y > ohjausylaraja)
            {
                targetPosition = new Vector3(targetPosition.x, ohjausylaraja, 0);
            }

            float sormileveys = PalautaSormenleveys();

            float minx = PalautaOhjausalueenAlarajaX() + (sormileveys / 2.0f);
            if (targetPosition.x < minx)
            {
                targetPosition = new Vector3(minx, targetPosition.y, 0);
            }
            float maxx = PalautaOhjausalueenYlarajaX();
            maxx = maxx - sormileveys / 2.0f;

            if (targetPosition.x > maxx)
            {
                targetPosition = new Vector3(maxx, targetPosition.y, 0);
            }

            /*
            Vector2 kohta = Vector2.MoveTowards((Vector2)sormitoisessakamerassa.transform.position, targetPosition, movespeedjotakaytetaan * Time.deltaTime);

            sormitoisessakamerassa.transform.position = kohta;
            */


            Vector2 currentPosition = sormitoisessakamerassa.transform.position;

            float newX = Mathf.MoveTowards(
                currentPosition.x,
                targetPosition.x,
                movespeedjotakaytetaan / nopeusjakox * Time.deltaTime
            );

            float newY = Mathf.MoveTowards(
                currentPosition.y,
                targetPosition.y,
                (movespeedjotakaytetaan / nopeusjakoy) * Time.deltaTime
            );

            Vector2 kohta = new Vector2(newX, newY);

            if (enabloiboxcollider)
            {
                sormitoisessakamerassa.transform.position = kohta;
            }
            else
            {


                /*ei toimi*/
                float moveSpeed = 0.01f; // units per iteration (not per second)
                //Vector2 kohtajohonliikutaan = kohta;
                Vector2 edellinenkunnollinenkohta = Vector2.zero;
                bool kunnonkohtaloytyi = false;

                while (Vector2.Distance(currentPosition, kohta) > 0.01f)
                {
                    currentPosition = Vector2.MoveTowards(currentPosition, kohta, moveSpeed);
                    // Debug.Log("Moved to: " + currentPosition);
                    //onkoTagiaBoxissaAlakaytaTransformia("vihollinen",this.m_Rigidbody2D.)

                    float alussijaintiy = PalautaPeliAlueenYOhjausAlueenYylla(currentPosition);


                    Vector3 viewportPos = sormenkamera.GetComponent<Camera>().WorldToViewportPoint(currentPosition);

                    // Step 2: Convert the viewport position to world position in camera2
                    Vector3 worldPosInCamera2 = Camera.main.ViewportToWorldPoint(new Vector3(viewportPos.x, viewportPos.y, Camera.main.nearClipPlane));

                    // Step 3: Update only the X position of gameObject2 to align horizontally
                    //Vector3 newPosition = transform.position;
                    //newPosition.x = worldPosInCamera2.x;

                    float uusialuksenx = worldPosInCamera2.x;

                    bool onko = onkoTagiaBoxissaAlakaytaTransformia("vihollinen", this.boxCollider2D.size * 1.2f, new Vector2(uusialuksenx, alussijaintiy));
                    if (onko)
                    {
                        //   Debug.Log("onko=" + onko);
                        break;
                    }
                    edellinenkunnollinenkohta = new Vector2(currentPosition.x, currentPosition.y);
                    kunnonkohtaloytyi = true;

                    //kohtajohonliikutaan = new Vector2(uusialuksenx, alussijaintiy);
                }
                //transform.position = kohtajohonliikutaan;
                if (kunnonkohtaloytyi)
                {
                    sormitoisessakamerassa.transform.position = edellinenkunnollinenkohta;
                }
            }


            /*
            Vector2 currentPosition = sormitoisessakamerassa.transform.position;

            Vector2 direction = (targetPosition - currentPosition).normalized;
            float deltaX = direction.x * movespeedjotakaytetaan/ nopeusjakox * Time.deltaTime;
            float deltaY = direction.y * (movespeedjotakaytetaan/ nopeusjakoy) * Time.deltaTime;

            float newX = Mathf.MoveTowards(currentPosition.x, targetPosition.x, Mathf.Abs(deltaX));
            float newY = Mathf.MoveTowards(currentPosition.y, targetPosition.y, Mathf.Abs(deltaY));

            Vector2 uusiKohta = new Vector2(newX, newY);
            sormitoisessakamerassa.transform.position = uusiKohta;
            */
        }
        else
        {

            Vector2 kohta = Vector2.MoveTowards((Vector2)transform.position, targetPosition, movespeedjotakaytetaan * Time.deltaTime);
            transform.position = kohta;
            //}
        }
    }

    public float nopeusjakox = 1.5f;
    public float nopeusjakoy = 6.0f;

    public float movespeedinKasvatussteppi = 1;
    public void LisaaMoveSpeedia()
    {
        moveSpeed += movespeedinKasvatussteppi;
    }


    public float moveSpeed = 50f;
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        return mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));
    }

    public void SiirryGameOverJalkeiseenTilaan()
    {
        Debug.Log("Level1");

        SceneManager.LoadScene("StartMenu");
        return;
    }


    void FixedUpdate()
    {

        if (restartscene)
        {
            /*
            //gameoverinajankohta = Time.time;
            float aikanyt = Time.realtimeSinceStartup;

            if (aikanyt - gameoverinajankohta > aikamaarajokajatketaangameoverinjalkeen)
            {
                Time.timeScale = 0;
               // SiirryGameOverJalkeiseenTilaan();
            }
            */
            return;

        }


        if (isPaused)
        {
            return;
        }

        /*
        if (gameover)
        {
            GameObject instanssi = Instantiate(gameoverPrefab, new Vector3(10.1f +
+(m_SpriteRenderer.bounds.size.x / 2), 10, 0), Quaternion.identity);
            //  instanssi.GetComponent<Rigidbody2D>().velocity = new Vector2(20, 0);

            Destroy(instanssi, 1);
            return;
        }
        */


        if (m_Animator.GetBool("explode"))
        {

            return;
        }

        //mainCamera
        //mainCamera

        // screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));


        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;







        float xsuuntamuutos = 0.0f;
        float ysuuntamuutos = 0.0f;
        //sijaintimuutos tämä siis 0.05 joka on se kiinteä arvo paljonko se liikkuu jos nappi pohjassa
        //xsuuntamuutos kasvattaa vielä tuon päälle sitten sitä sijaintia
        //xsuuntamuutos kasvaa niin kauan kuin nappula pohjassa




        if (oikeaNappiPainettu)
        {

            oikeanappipainettukestoaika += Time.fixedDeltaTime;
            if (oikeanappipainettukestoaika > turboviiveSekunneissa)
            {
                xsuuntamuutos += (oikeanappipainettukestoaika - turboviiveSekunneissa) * kiihtyvyys;
            }
            xsuuntamuutos += perusnopeus;
        }
        else
        {
            oikeanappipainettukestoaika = 0.0f;
        }
        if (vasenNappiPainettu)
        {
            //vasennappipainettukestoaika += Time.deltaTime;//OLI AIEMMIN
            vasennappipainettukestoaika += Time.fixedDeltaTime;


            if (vasennappipainettukestoaika > turboviiveSekunneissa)
            {
                xsuuntamuutos -= (vasennappipainettukestoaika - turboviiveSekunneissa) * kiihtyvyys;
            }
            xsuuntamuutos -= perusnopeus;
        }
        else
        {
            vasennappipainettukestoaika = 0.0f;
        }
        if (ylosNappiPainettu)
        {
            ylosnappipainettukestoaika += Time.fixedDeltaTime;
            if (ylosnappipainettukestoaika > turboviiveSekunneissa)
            {
                ysuuntamuutos += (ylosnappipainettukestoaika - turboviiveSekunneissa) * kiihtyvyys;
            }

            ysuuntamuutos += perusnopeus;
            // edellinenylosNappiPainettu += muuttuja;




        }
        else
        {
            ylosnappipainettukestoaika = 0.0f;
        }
        if (alasNappiPainettu)
        {
            //alasnappipainettukestoaika += Time.deltaTime;// OLI AIEMMIN
            alasnappipainettukestoaika += Time.fixedDeltaTime;
            if (alasnappipainettukestoaika > turboviiveSekunneissa)
            {
                ysuuntamuutos -= (alasnappipainettukestoaika - turboviiveSekunneissa) * kiihtyvyys;
            }

            ysuuntamuutos -= perusnopeus;
        }
        else
        {
            alasnappipainettukestoaika = 0.0f;
        }


        //edellinenoikeaNappiPainettu = oikeaNappiPainettu;

        //   xsuuntamuutos = Mathf.Clamp(xsuuntamuutos, 0, maksiminopeus);
        //   ysuuntamuutos = Mathf.Clamp(ysuuntamuutos, 0, maksiminopeus);


        //Debug.Log("xsuuntamuutos=" + oikeanappipainettukestoaika);


        if (xsuuntamuutos > maksiminopeusvertikaalinen)
        {
            xsuuntamuutos = maksiminopeusvertikaalinen;
        }
        if (xsuuntamuutos < -maksiminopeusvertikaalinen)
        {
            xsuuntamuutos = -maksiminopeusvertikaalinen;

        }

        if (ysuuntamuutos > maksiminopeusylosalas)
        {
            ysuuntamuutos = maksiminopeusylosalas;
        }
        if (ysuuntamuutos < -maksiminopeusylosalas)
        {
            ysuuntamuutos = -maksiminopeusylosalas;

        }


        // Debug.Log("xuusntam=" + xsuuntamuutos);

        // transform.position = new Vector2(transform.position.x + xsuuntamuutos, transform.position.y + ysuuntamuutos);

        AsetaSijainti(new Vector2(transform.position.x + xsuuntamuutos, transform.position.y + ysuuntamuutos));

        if (ysuuntamuutos > 0.0f)
        {
            m_Animator.SetBool("up", true);

        }
        else
        {
            m_Animator.SetBool("up", false);
        }



        bool ammusinstantioitiin = false;

        //float aika = Time.deltaTime;//OLI AIEMMIN
        float aika = Time.fixedDeltaTime;

        deltaaikojensumma += aika;
        //@todoo
        //pitäisikö vain etsiä cameraviewistä ammusten määrä.

        //ja mikä on se peruslogiikka taas tässä :) =ampumakertojenvalinenviive0.01
        /*
        if (spaceNappiaPainettu)
        {
            Debug.Log("deltaaikojensumma=" + deltaaikojensumma);
        }
        */

        if (spaceNappiaPainettu && deltaaikojensumma > ampumakertojenvalinenviive & OnkoAmmustenMaaraAlleMaksimin() && !OnkoSeinaOikealla()

            &&
            !onkoAluksenAmmuksetDisabloitu()
            )
        {
            //	if (!ammusInstantioitiinviimekerralla) {
            Vector3 v3 =
            new Vector3(0.1f +
            m_Rigidbody2D.position.x + (m_SpriteRenderer.bounds.size.x / 2), m_Rigidbody2D.position.y, 0);


            //audiosourcelaukaus.
            //    audiosourcelaukaus.Play();

            GameObject instanssi;
            if (laserkaytossa)
            {
                ad.AluslaserPlay();
                //                instanssi = Instantiate(
                //                  laserPrefab, v3, Quaternion.identity);

                instanssi =
                ObjectPoolManager.Instance.GetFromPool(laserPrefab, v3, Quaternion.identity);




                instanssi.GetComponent<Rigidbody2D>().velocity = new Vector2(ammuksenalkuvelocity, 0);

                LaserController aa = instanssi.GetComponent<LaserController>();
                //aa.SetLaserkaytossa(laserkaytossa);
                aa.alus = this.gameObject;
                aa.SetAluksenluoma(true);
                aa.alkuvelocity = new Vector2(ammuksenalkuvelocity, 0);
            }
            else
            {
                ad.AlusammusPlay();
                //instanssi = Instantiate(ammusPrefab, v3, Quaternion.identity);

                // Debug.Log("ammus otetaan poolista");

                instanssi =
                ObjectPoolManager.Instance.GetFromPool(ammusPrefab, v3, Quaternion.identity);


                AmmusController aa = instanssi.GetComponent<AmmusController>();
                //aa.SetLaserkaytossa(laserkaytossa);
                aa.alus = this.gameObject;
                aa.SetAluksenluoma(true);


                instanssi.GetComponent<Rigidbody2D>().velocity = new Vector2(ammuksenalkuvelocity, 0);


            }
            InstantioiBulletAlasTarvittaessa();

            InstantioiBulletYlosTarvittaessa();
            AmmuOptioneilla();

            aluksenluomienElossaOlevienAmmustenMaara++;




            ammusinstantioitiin = true;
            deltaaikojensumma = 0;
            // instantioinninajankohta=



            //alas tippuva
            // missileDownCollected


        }
        ammusInstantioitiinviimekerralla = ammusinstantioitiin;


        if (teeOptionPallukka)
        {

            TeeOptioni();
            teeOptionPallukka = false;
        }


    }

    public float ammuksenalkuvelocity = 20.0f;

    private void InstantioiBulletAlasTarvittaessa()
    {
        // if (missileDownCollected >= 1 && (instanssiBulletAlas == null ||
        //     instanssiBulletAlas.GetComponent<BaseController>().IsGoingToBeDestroyed()) )
        // {

        if (missileDownCollected >= 1 && OnkoAlasAmmustenMaaraAlleMaksimin())
        {

            aluksenluomienElossaOlevienAlasAmmustenMaara++;
            // private int aluksenluomienElossaOlevienYlosAmmustenMaara;

            Vector3 v3alas =
        new Vector3(bulletinalkuxoffsetti +
        m_Rigidbody2D.position.x + (m_SpriteRenderer.bounds.size.x / 2), m_Rigidbody2D.position.y + 0.0f, 0);

            GameObject instanssiBulletAlas = ObjectPoolManager.Instance.GetFromPool(bulletPrefab, v3alas, Quaternion.identity);



            instanssiBulletAlas.GetComponent<BulletScript>().alus = this.gameObject;

            instanssiBulletAlas.GetComponent<BulletScript>().SetAluksenluoma(true);


            //  instanssiBulletAlas = Instantiate(bulletPrefab, v3alas, Quaternion.identity);

            // instanssiBulletAlas.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90.0f);
            instanssiBulletAlas.GetComponent<BulletScript>().alas = true;

            //IAlas alas = instanssiBulletAlas.GetComponent<IAlas>();
            //if (alas != null)
            // {
            //instanssiBulletAlas.SendMessage("Alas", true);
            //  alas.Alas(true);
            instanssiBulletAlas.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -bulletinalkuvelocityy);
            instanssiBulletAlas.GetComponent<Rigidbody2D>().gravityScale = bulletingravityscale;
            //}

            //}

        }

    }

    private void InstantioiBulletYlosTarvittaessa()
    {
        if (missileUpCollected >= 1 && OnkoYlosAmmustenMaaraAlleMaksimin())
        {

            aluksenluomienElossaOlevienYlosAmmustenMaara++;
            Vector3 v3ylos =
new Vector3(bulletinalkuxoffsetti +
m_Rigidbody2D.position.x + (m_SpriteRenderer.bounds.size.x / 2), m_Rigidbody2D.position.y + 0.0f, 0);



            //instanssiBulletYlos = Instantiate(bulletPrefab, v3ylos, Quaternion.identity);

            GameObject instanssiBulletYlos = ObjectPoolManager.Instance.GetFromPool(bulletPrefab, v3ylos, Quaternion.identity);

            instanssiBulletYlos.GetComponent<BulletScript>().alus = this.gameObject;

            instanssiBulletYlos.GetComponent<BulletScript>().SetAluksenluoma(true);

            instanssiBulletYlos.GetComponent<BulletScript>().alas = false;

            //instanssiBulletYlos.SendMessage("Alas", false);

            // IAlas alas = instanssiBulletYlos.GetComponent<IAlas>();
            // if (alas != null)
            // {
            //instanssiBulletAlas.SendMessage("Alas", true);
            //   alas.Alas(false);
            // }

            instanssiBulletYlos.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, bulletinalkuvelocityy);

            instanssiBulletYlos.GetComponent<Rigidbody2D>().gravityScale = -bulletingravityscale;

        }
    }
    public float bulletingravityscale = 2.0f;

    public float bulletinalkuvelocityy = 2.0f;
    public float bulletinalkuxoffsetti = -0.4f;
    public void VahennaaluksenluomienElossaOlevienAmmustenMaaraa()
    {
        aluksenluomienElossaOlevienAmmustenMaara = aluksenluomienElossaOlevienAmmustenMaara - 1;
    }

    public void LisaaaluksenluomienElossaOlevienAmmustenMaaraa()
    {
        aluksenluomienElossaOlevienAmmustenMaara = aluksenluomienElossaOlevienAmmustenMaara + 1;

    }

    public void VahennaaluksenluomienElossaOlevienAlasAmmustenMaaraa()
    {
        aluksenluomienElossaOlevienAlasAmmustenMaara = aluksenluomienElossaOlevienAlasAmmustenMaara - 1;
    }

    public void VahennaaluksenluomienElossaOlevienYlosAmmustenMaaraa()
    {
        aluksenluomienElossaOlevienYlosAmmustenMaara = aluksenluomienElossaOlevienYlosAmmustenMaara - 1;
    }



    public void SetEnabloiboxcollider(bool arvo)
    {
        enabloiboxcollider = arvo;

        //   boxCollider2D.enabled = arvo;no jätetään tämä nyt päälle, jotta bonukset toimii...

        //boxCollider2D.enabled = true;

        boxCollider2D.isTrigger = !arvo;

        //eli jos enabloidaan niin itseasiassa pitää semmonen iso oikein vahva forcefieldi pistääkin päälle
        //eli koodaa joko uusi tai sitten sitä vanhaa skaalataan tms...



    }

    private void AmmuOptioneilla()
    {
        foreach (OptionController obj in optionControllerit)
        {
            //   obj.gameObject.transform.position
            // Set the position

            if (laserkaytossa)
            {
                obj.ammuNormilaukaus(laserPrefab, new Vector2(ammuksenalkuvelocity, 0));
            }
            else
            {
                obj.ammuNormilaukaus(ammusPrefab, new Vector2(ammuksenalkuvelocity, 0));
            }

            if (missileDownCollected >= 1)
            {
                // instanssiBulletYlos.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, bulletinalkuvelocityy);

                // instanssiBulletYlos.GetComponent<Rigidbody2D>().gravityScale = -bulletingravityscale;

                obj.ammuAlaslaukaus(bulletPrefab, new Vector2(0.0f, -bulletinalkuvelocityy), bulletingravityscale);
            }
            if (missileUpCollected >= 1)
            {
                if (bulletPrefab == null)
                {
                    Debug.Log("bulletti null");
                    return;
                }
                if (obj == null)
                {
                    Debug.Log("objekti null");
                }
                else
                {
                    obj.ammuYloslaukaus(bulletPrefab, new Vector2(0.0f, bulletinalkuvelocityy), -bulletingravityscale);
                }

            }
        }
    }

    private List<OptionController> optionControllerit = new List<OptionController>();


    public GameObject alamaksiminMaarittava;
    public GameObject ylamaksiminMaarittava;


    void LateUpdate()
    {
        // asetaSijainti();

        //  Explode();
    }


    public GameObject damagemittarilinerenderer;

    private void PaivitaDamagePalkkia()
    {
        //jos se on liikaa niin eikun gameoveria
        if (damagemodekaytossa)
            damagemittariController.SetDamage(damagenmaara, maksimimaaradamageajokakestetaan);


        //  damagemittarilinerenderer.GetComponent<DamageMittariLineRendererController>().SetDamage(damagenmaara, maksimimaaradamageajokakestetaan, sormikamera);


        TeeGameOver();



    }
    public bool demomode = true;


    public float gameoverinjalkeintimescale = 0.1f;


    public float viiveAluksenRajahdyksissaGameOverissaKunOllaanDemossa = 1.0f;
    public float viimeisinAluksenRajahdysDemoModessa = 0.0f;

    /*
    private IEnumerator PaivitaDamagePalkkiaViiveella()
    {
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds
        damagenmaara = 0.0f;
        PaivitaDamagePalkkia();
    }
    */

    private void TeeGameOver()
    {
        //PaivitaDamagePalkkia();

        if (demomode && !restartscene && damagenmaara >= maksimimaaradamageajokakestetaan)
        {
            if (Time.realtimeSinceStartup >= viimeisinAluksenRajahdysDemoModessa + viiveAluksenRajahdyksissaGameOverissaKunOllaanDemossa)
            {
                Vector3 vektori =
new Vector3(
m_Rigidbody2D.position.x, m_Rigidbody2D.position.y, 0);
                GameObject rajahdys = Instantiate(explosion, vektori, Quaternion.identity);
                damagenmaara = 0.0f;
                viimeisinAluksenRajahdysDemoModessa = Time.realtimeSinceStartup;
                damagenmaara = 0.0f;
                //PaivitaDamagePalkkia();
                if (damagemodekaytossa)
                    damagemittariController.SetDamage(damagenmaara, maksimimaaradamageajokakestetaan);
                //StartCoroutine(PaivitaDamagePalkkiaViiveella());

                if (gameoverPrefab != null)
                    Instantiate(gameoverPrefab, transform.position, Quaternion.identity);
                GameManager.Instance.PlayerDied();

            }
            else
            {
                damagenmaara = 0.0f;
                if (damagemodekaytossa)
                    damagemittariController.SetDamage(damagenmaara, maksimimaaradamageajokakestetaan);
            }

        }

        else if (!demomode && !restartscene && damagenmaara >= maksimimaaradamageajokakestetaan)
        {

            //SetkeskellaTextMeshProUGUI("Game over");

            //Time.timeScale = gameoverinjalkeintimescale;

            ad.ExplodePlay();
            Vector3 vektori =
    new Vector3(
    m_Rigidbody2D.position.x, m_Rigidbody2D.position.y, 0);

            // GameObject instanssiOption = Instantiate(gameoverPrefab, vektori, Quaternion.identity);

            GameObject rajahdys = Instantiate(explosion, vektori, Quaternion.identity);
            //RajaytaSprite(gameObject, 4, 4, 1, aikamaarajokajatketaangameoverinjalkeen);
            //m_SpriteRenderer.enabled = false;antaa spriten nakya

            //  Destroy(instanssiOption, 10);


            GameManager.Instance.PlayerDied();



            //TogglePause();

            GameObject myObject = GameObject.Find("PauseButtonKaytossa");
            Image spriteRenderer = myObject.GetComponent<Image>();

            //Image image = GetComponent<Image>();
            if (spriteRenderer != null)
            {
                //  spriteRenderer.enabled = false; // Hides the sprite
            }
            //gameoverinajankohta = Time.realtimeSinceStartup;

            restartscene = true;
        }

    }

    //public float aikamaarajokajatketaangameoverinjalkeen = 5.0f;

    //private float gameoverinajankohta;

    public void Explode()
    {
        if (!enabloiboxcollider)
        {
            return;
        }
        damagenmaara = maksimimaaradamageajokakestetaan;


        NaytaIsollaSeJohonTormattiin(null, "Explode");

        PaivitaDamagePalkkia();

    }

    public void SetElamienMaara(int elamienmaara)
    {
        elamat.GetComponent<ElamatController>().SetElamienMaara(elamienmaara);
    }

    public bool AiheutaDamagea(float damagemaara, Vector2 contactpoint)
    {
        damagenmaara += damagemaara;
        // ExplodeTarvittaesssa();
        PaivitaDamagePalkkia();
        return false;
    }

    public void Savua()
    {
        if (!particleSystem.isPlaying)
        {
            particleSystem.Play();
        }

        var mainModule = particleSystem.main;
        float currentGravityModifier = mainModule.gravityModifier.constant;
        float uusiarvo = currentGravityModifier - gravitymodifiermuutoskunammusosuu;
        if (uusiarvo > gravitynminimi)
        {

            mainModule.gravityModifier = uusiarvo;
        }
        //mitäs sitten kun savuttaa


        Color color = m_SpriteRenderer.color;
        //color.r = r;
        //color.g = g;
        //color.b = b;
        //antaa savuta, mutta ei kasvateta damagea :)
        //  damagenmaara += damagemaarakasvastatuskunsavutaan;
        color.g = PalautaGvari();

        m_SpriteRenderer.color = color;
        //PaivitaDamagePalkkia();

        //sitten jos tehdään damagepalkilla niin sitten vasta vaikuttaa damage määrään

    }
    private bool OnkoSeinaOikealla()
    {

        Vector3 v = transform.position;

        Collider2D[] cs =
        Physics2D.OverlapBoxAll(new Vector2(v.x

            , v.y + 0.1f),
        new Vector2(1f, 0.1f), 0

            );

        if (cs != null && cs.Length > 0)
        {
            foreach (Collider2D c in cs)
            {
                if (c.gameObject == this.gameObject)
                {

                }
                else if (c.gameObject.tag.Contains("tiili"))
                {
                    return true;
                }
            }
        }
        return false;

    }
    /*
    private bool OnkoForceFieldPaalla()
    {
        ForceFieldController c = GetComponentInChildren<ForceFieldController>();
        if (c == null)
        {
            return false;
        }
        return c.IsOnkotoiminnassa();
    }
    */





    void OnCollisionEnter2D(Collision2D col)
    {
        //haukisilmavihollinenexplodetag

        //  col.otherCollider tää on alus


        //explodetag
        //if (col.collider.tag.Contains("hauki") || col.collider.tag.Contains("tiili") ||
        //    col.collider.tag.Contains("pyoroovi") || col.collider.tag.Contains("laatikkovihollinenexplodetag"))
        // {
        // damagenmaara += maksimimaaradamageajokakestetaan;
        //ExplodeTarvittaesssa();
        // PaivitaDamagePalkkia();
        // }
        //   else if (col.collider.tag.Contains("pallovihollinen") /*&& !OnkoForceFieldPaalla() */)
        // {
        // damagenmaara += maksimimaaradamageajokakestetaan;
        //ExplodeTarvittaesssa();
        // PaivitaDamagePalkkia();
        // }
        //else if (col.collider.tag.Contains("vihollinen") /*&& !OnkoForceFieldPaalla() */)
        // {
        //damagenmaara += maksimimaaradamageajokakestetaan;
        //ExplodeTarvittaesssa();
        // PaivitaDamagePalkkia();

        //}

        if (enabloiboxcollider && col.enabled && BaseController.TuhoaakoAluksen(col.collider.tag))
        {

            NaytaIsollaSeJohonTormattiin(col.gameObject, "OnCollisionEnter2D ");
            damagenmaara += maksimimaaradamageajokakestetaan;
            //ExplodeTarvittaesssa();
            PaivitaDamagePalkkia();
        }


    }



    void OnParticleCollision(GameObject other)
    {
        // Debug.Log("Particle hit: " + other.name);

        // Example: Apply damage if hitting an enemy
        //  if (other.CompareTag("Enemy"))
        // {
        // EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        // if (enemyHealth != null)
        // {
        //     enemyHealth.TakeDamage(10);
        // }
        //   }
        //eli tämä tarvitaan sitä kalaskeletonia varten
        if (enabloiboxcollider && other.tag.Contains("vihollinen") /*  && !OnkoForceFieldPaalla()*/ )
        {

            GameObject go = other;

            GameObject klooni = Instantiate(go);
            Collider2D[] ccc = klooni.GetComponents<Collider2D>();
            foreach (Collider2D c in ccc)
            {
                c.enabled = false;
            }
            klooni.transform.localScale = klooni.transform.localScale * 3f;

            SetruudunvasenylakulmatekstiTextMeshProUGUI("part=" + go.name);


            damagenmaara += maksimimaaradamageajokakestetaan;
            //ExplodeTarvittaesssa();
            PaivitaDamagePalkkia();



        }

    }






    public void BonusCollected()
    {



        //siinä on nyt järjestyksessä
        int selectedIndex = -1;
        foreach (BonusButtonController btc in bbc)
        {
            if (btc.selected)
            {
                selectedIndex = btc.order;
                break;
            }

        }

        if (selectedIndex < 0)
        {
            bbc[0].selected = true;
        }
        else
        {
            bbc[selectedIndex].selected = false;
            //0,1,2,3 =4
            if (selectedIndex + 1 >= bbc.Count)
            {
                bbc[0].selected = true;
            }
            else
            {
                bbc[selectedIndex + 1].selected = true;

            }
        }
        //  Debug.Log("selectedIndex=" + selectedIndex);

        foreach (BonusButtonController btc in bbc)
        {
            // Debug.Log("order=" + btc.order + " selected=" + btc.selected);


        }
    }

    public void BonusButtonPressed()
    {
        bool asetakaikkieiselectoiduiksi = false;
        foreach (BonusButtonController btc in bbc)
        {
            if (btc.selected && btc.usedcount < btc.maxusedCount)
            {
                btc.usedcount = btc.usedcount + 1;
                btc.selected = false;
                asetakaikkieiselectoiduiksi = true;
                if (btc.bonusbuttontype.Equals(BonusButtonController.Bonusbuttontype.Speed))
                {
                    //   Debug.Log("speed()");
                    //vauhtiOikeaMax += vauhdinLisaysKunSpeedbonusOtettu;
                    //vauhtiYlosMax += vauhdinLisaysKunSpeedbonusOtettu;
                    maksiminopeusylosalas += vauhdinLisaysKunSpeedbonusOtettu;
                    //ammustenmaksimaaraProperty += ammustenmaksimaaranLisaysKunSpeedBonusButtonOtettu;
                    //ampumakertojenvalinenviive += ampujakertojenvalisenViiveenPienennysKunSpeedBonusButtonOtettu;

                    LisaaMoveSpeedia();


                }
                else if (btc.bonusbuttontype.Equals(BonusButtonController.Bonusbuttontype.MissileDown))
                {
                    //Debug.Log("missile()");
                    missileDownCollected++;
                }
                else if (btc.bonusbuttontype.Equals(BonusButtonController.Bonusbuttontype.MissileUp))
                {
                    //  Debug.Log("missile()");
                    missileUpCollected++;
                }
                else if (btc.bonusbuttontype.Equals(BonusButtonController.Bonusbuttontype.Option))
                {
                    //                  public int nykyinenoptioidenmaara;

                    //public int optioidenmaksimaara;
                    if (nykyinenoptioidenmaara < optioidenmaksimaara)
                    {
                        teeOptionPallukka = true;
                        nykyinenoptioidenmaara++;


                    }
                    else
                    {
                        //        Debug.Log("nykyinenoptioidenmaara="+ nykyinenoptioidenmaara);
                    }
                }
                else if (btc.bonusbuttontype.Equals(BonusButtonController.Bonusbuttontype.ForceField))
                {
                    //  Debug.Log("missile()");
                    //missileUpCollected++;
                    Debug.Log("instantioi ForceField");
                    AsetaForceFieldiNakyviin();
                }
                else if (btc.bonusbuttontype.Equals(BonusButtonController.Bonusbuttontype.Laser))
                {
                    //  Debug.Log("missile()");
                    //missileUpCollected++;
                    Debug.Log("aseta laserkayttöön");
                    //AsetaForceFieldiNakyviin();
                    laserkaytossa = true;
                }

            }
            else
            {
                //  Debug.Log("painettu bonusta, mutta selectoitu jo kaytetty");
            }
        }
        if (asetakaikkieiselectoiduiksi)
        {
            foreach (BonusButtonController btc in bbc)
            {
                btc.selected = false;
            }
        }

    }

    private void AsetaForceFieldiNakyviin()
    {

        ForceField(true);
        //   GetComponentInChildren<ForceFieldController>().SetOnkotoiminnassa(true);

    }

    /*
    public void AlustaForceFied()
    {
        bool onko = GetComponentInChildren<ForceFieldController>().IsOnkotoiminnassa();
        GetComponentInChildren<ForceFieldController>().SetOnkotoiminnassa(onko);
    }
    */

    private bool OnkoForceFieldiKaytossa()
    {
        ForceFieldController ff =
        GetComponentInChildren<ForceFieldController>();

        return ff != null;

    }

    private void AsetaForceFieldiButtoni()
    {
        bool onko = OnkoForceFieldiKaytossa();//jos force field kaytossa 
        AsetaForceFieldiButtonTila(onko);

    }


    public void AsetaForceFieldiButtonTila(bool onkoJoKaytossa)
    {
        foreach (BonusButtonController btc in bbc)
        {
            if (btc.bonusbuttontype.Equals(BonusButtonController.Bonusbuttontype.ForceField))
            {
                //  Debug.Log("missile()");
                //missileUpCollected++;
                //btc.enabled = enable;
                if (onkoJoKaytossa)
                {
                    btc.usedcount = 1;
                }
                else
                {
                    btc.usedcount = 0;
                }

            }
        }

    }


    private bool OnkoAmmustenMaaraAlleMaksimin()
    {
        int maksimi = ammustenmaksimaaraProperty;
        // int nykymaara = palautaAmmustenMaara();
        int nykymaara = aluksenluomienElossaOlevienAmmustenMaara;
        return nykymaara < maksimi;
    }


    private bool OnkoAlasAmmustenMaaraAlleMaksimin()
    {
        int maksimi = ammustenmaksimaaraProperty;
        // int nykymaara = palautaAmmustenMaara();
        int nykymaara = aluksenluomienElossaOlevienAlasAmmustenMaara;
        return nykymaara < maksimi;
    }

    private bool OnkoYlosAmmustenMaaraAlleMaksimin()
    {
        int maksimi = ammustenmaksimaaraProperty;
        // int nykymaara = palautaAmmustenMaara();
        int nykymaara = aluksenluomienElossaOlevienYlosAmmustenMaara;
        return nykymaara < maksimi;
    }




    void OnParticleTrigger()
    {
        // Debug.Log($"OnParticleTrigger hit: {other.name}");
        Debug.Log("OnParticleTrigger");
        // Add your collision handling logic here
    }

    private bool cameraolipysahtynytviimeksi = false;
    public void PaivitaStoptime()
    {
        Kamera k = mainCamera.GetComponent<Kamera>();
        string aika = k.PalautaOdotusAikaKunnesLiikkuu();

        SetruudunvasenylakulmatekstiTextMeshProUGUI(aika);


    }

    public bool onkoAluksenAmmuksetDisabloitu()
    {
        Kamera k = mainCamera.GetComponent<Kamera>();
        return k.onkoAluksenAmmuksetDisabloitu();
    }


    public void OnTriggerEnter2D(Collider2D col)
    {
        // Debug.Log("OnTriggerEnter2D " + col);

        if (enabloiboxcollider && col.enabled && BaseController.TuhoaakoAluksen(col.tag))
        {
            GameObject go = col.gameObject;

            NaytaIsollaSeJohonTormattiin(go, "OnTriggerEnter2D");
            Debug.Log("OnTriggerEnter2D collisiontagi joka tuhoaa=" + col.tag);

            /* otettu pois mikä on tämä triggeri muka joka tarvitaan
             * akita-ammus ei oo triggeri mutta silti tuli muka collision
            damagenmaara += maksimimaaradamageajokakestetaan;
            //ExplodeTarvittaesssa();
            PaivitaDamagePalkkia();


            */
        }

    }


    public void NaytaIsollaSeJohonTormattiin(GameObject go, string info)
    {

        if (go != null)
        {
            /*
            GameObject klooni = Instantiate(go);
            Collider2D[] ccc = klooni.GetComponents<Collider2D>();
            foreach (Collider2D c in ccc)
            {
                c.enabled = false;
            }
            klooni.transform.localScale = klooni.transform.localScale * 3f;
            */

            //SetruudunvasenylakulmatekstiTextMeshProUGUI(info + " " + go.name);

            Debug.Log(info + " collisiontagi joka tuhoaa=" + go.tag + "nimi on=" + go.name);
        }
        else
        {
            SetruudunvasenylakulmatekstiTextMeshProUGUI("nulli" + " " + go.name);
        }


    }


}
