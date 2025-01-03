﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class AlusController : BaseController, IDamagedable, IExplodable
{
    //ensinnäkin gameobjecti jolla voi säätää kameran nopeutta

    //toisekseen joku gameobjekti jolla voi tehdä näitä savepointteja


    public int elamienmaara = 0;
    public int score = 0;//tälle se 
    public float difficalty = 1.0f;//peli kiertää uusiksi sitten kun pääsee läpi, mutta difficalt



    public float maksimimaaradamageajokakestetaan = 100.0f;
    public float damagenmaara = 0.0f;


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

    public InputActionReference moveInputActionReference;


    public InputActionReference moveDownInputActionReference;

    public InputActionReference moveUpInputActionReference;


    public bool autofire;
    //  public GameObject speedbonusbutton;
    //  public GameObject missilebonusbutton;

    public int ammustenmaksimaaraProperty;

    public float ampumakertojenvalinenviive;


    public float ampujakertojenvalisenViiveenPienennysKunSpeedBonusButtonOtettu;
    public int ammustenmaksimaaranLisaysKunSpeedBonusButtonOtettu;

    public int nykyinenoptioidenmaara;

    public int optioidenmaksimaara;


    public Joystick joystick;

    public GameObject explosion;

    private Rigidbody2D m_Rigidbody2D;

    //vauhti
    private Animator m_Animator;

    private bool oikeaNappiPainettu = false;

    private bool vasenNappiPainettu = false;



    //ylos/alla

    private bool ylosNappiPainettu = false;
    private bool alasNappiPainettu = false;



    private bool spaceNappiaPainettu = false;
    //private bool spaceNappiAlhaalla = false;
    //private bool spaceNappiYlhaalla = false;



    private bool ammusInstantioitiinviimekerralla = false;

    public GameObject ammusPrefab;

    public GameObject bulletPrefab;

    public GameObject gameoverPrefab;


    private SpriteRenderer m_SpriteRenderer;




    private Vector2 screenBounds;

    private float objectWidth;
    private float objectHeight;

    private bool gameover = false;


    private GameObject instanssiBulletAlas;
    private GameObject instanssiBulletYlos;






    public GameObject instanssiOption1;
    private bool teeOptionPallukka = false;

    private int missileDownCollected = 0;
    private int missileUpCollected = 0;

    //

    public GameObject debugloota;

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
        base.TallennaSijaintiSailytaVainNkplViimeisinta(optioidenmaksimaara * 8, true, true, debugloota);
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


    private Camera mainCamera;
    // Start is called before the first frame update



    private AudioplayerController ad;


    private ParticleSystem particleSystem;


    private BoxCollider2D boxCollider2D;
    private bool android;
    void Start()
    {
        android = Application.platform == RuntimePlatform.Android;

        particleSystem = GetComponentInChildren<ParticleSystem>();
        if (particleSystem.isPlaying)
        {
            particleSystem.Stop();
        }

        //  audiosourcetaustamusiikki.Play();
        //   Application.targetFrameRate = 45;
        // Application.targetFrameRate = 10; // For example, cap to 60 FPS
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

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
            bbc.Add(btc);

        }
        // words.Sort((a, b) => a.Length.CompareTo(b.Length));

        bbc.Sort((a, b) => a.order.CompareTo(b.order));


        ad = FindObjectOfType<AudioplayerController>();
        ad.TaustaMusiikkiPlay();

        //TeeOptioni();

        GameObject myObject = GameObject.Find("QuitButtonKaytossa");
        myObject.GetComponent<Image>().enabled = false;
        Color color = m_SpriteRenderer.color;
        gvarinalkutilanne = color.g;
        boxCollider2D = GetComponent<BoxCollider2D>();

        damagemittariController = damageMittari.GetComponent<DamagemittariController>();






    }

    float minX;
    float maxX;
    float minY;
    float maxY;

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

    // Update is called once per frame
    void Update()
    {
        if (gameover)
        {
            return;
        }

        if (Input.GetKey(KeyCode.P) || CrossPlatformInputManager.GetButtonDown("Pause"))
        {
            Debug.Log("pausepressed");
            TogglePause();
        }

        if (Input.GetKey("escape") || CrossPlatformInputManager.GetButtonDown("Quit"))
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


        if (Input.GetKey(KeyCode.N) || CrossPlatformInputManager.GetButtonDown("Bonus"))
        {
            //   Debug.Log("BonusButtonPressed");
            BonusButtonPressed();
        }



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



        Vector2 moveDirection = moveInputActionReference.action.ReadValue<Vector2>();


        float moveDirectionAlas = moveDownInputActionReference.action.ReadValue<float>();

        float moveDirectionUp = moveUpInputActionReference.action.ReadValue<float>();




        //    m_Rigidbody2D.velocity = new Vector2(0.0f, 0.0f);



        float nappiHorizontal = Input.GetAxisRaw("Horizontal");
        if (nappiHorizontal == 0.0f)
        {
            nappiHorizontal = moveDirection.x;
        }

        //float nappiHorizontal = moveDirection.x;


        oikeaNappiPainettu = (nappiHorizontal > 0.0f); // > 0 for right, < 0 for left
        vasenNappiPainettu = (nappiHorizontal < 0.0f);

        if (!oikeaNappiPainettu)
            oikeaNappiPainettu = (joystick.Horizontal > 0.1f);


        if (!vasenNappiPainettu)
            vasenNappiPainettu = (joystick.Horizontal < -0.1f);



        float nappiVertical = Input.GetAxisRaw("Vertical");
        if (nappiVertical == 0.0f)
        {
            nappiVertical = moveDirection.y;
        }

        // float nappiVertical = moveDirection.y;

        //float nappiVertical = -moveDirectionAlas;

        //if (nappiVertical == 0.0)
        //{
        //    nappiVertical = moveDirectionUp;
        //}


        ylosNappiPainettu = nappiVertical > 0; // > 0 for right, < 0 for left
        alasNappiPainettu = nappiVertical < 0;


        if (!ylosNappiPainettu)
            ylosNappiPainettu = (joystick.Vertical > 0.1f);

        if (!alasNappiPainettu)
            alasNappiPainettu = (joystick.Vertical < -0.1f);

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
        Time.timeScale = 0f; // Stop game time
        isPaused = true;
        // Optional: Show pause menu UI
        ad.TaustaMusiikkiStop();

        GameObject myObject = GameObject.Find("QuitButtonKaytossa");
        myObject.GetComponent<Image>().enabled = true;


    }

    void ResumeGame()
    {
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
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, 0));
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
        Vector2 boxSize = boxCollider2D.size * transform.localScale;

        boxSize = boxSize * sormipaikkakerto;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(worldPosition, boxSize, sormenlayermaski);

        foreach (Collider2D collider in colliders)
        {
            //
            if (collider.gameObject.tag.Contains("alustag"))
            {
                _isDragging = true;
                // Calculate offset between the object and touch/mouse position
                _offset = collider.gameObject.transform.position - worldPosition;
                // _offset = new Vector3(+100, 0, 0);


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


    private void MoveObject(Vector3 screenpositionSormen)
    {
        //offsetvalue = Screen.width * (offsetvalueprocentsOffScreenWidth / 100f);
        float ruudunvasemmanreunansuojaalue = Screen.width * ruudunvasemmanreunansuojaalueProsentti / 100.0f;
        float erikoisalueenoikeareuna = Screen.width * erikoisalueenoikeareunaProsentti / 100.0f;
        float offsetmaara = Screen.width * offsetmaaraProsentti / 100.0f;

        Vector3 screeni;

        if (screenpositionSormen.x <= ruudunvasemmanreunansuojaalue)
        {
            screenpositionSormen.x = ruudunvasemmanreunansuojaalue;
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

    }

    public Vector2 tormayskoko = new Vector2(0.1f, 01f);

    private Vector2 tormayskohta = Vector2.zero;



    public Vector2 kerto = new Vector2(1.2f, 1.2f);

    void TryMove(Vector2 targetPosition)
    {
        // Calculate the direction and total distance to move
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        float totalDistance = Vector2.Distance(transform.position, targetPosition);

        // Step size for progressive movement checks
        float stepSize = 0.01f; // Adjust for precision (smaller values are more precise but slower)

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



        //    m_Rigidbody2D.MovePosition(Vector2.MoveTowards(transform.position, currentPosition, moveSpeed * Time.deltaTime));



        if (rajayta)
        {
            Vector2 kohta = Vector2.MoveTowards(transform.position, currentPosition, moveSpeed * Time.deltaTime);
            transform.position = kohta;
            damagenmaara += maksimimaaradamageajokakestetaan;
            PaivitaDamagePalkkia();
        }
        else
        {
            Vector2 kohta = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            transform.position = kohta;
        }


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

        if (gameover)
        {
            //gameoverinajankohta = Time.realtimeSinceStartup;
            float aikanyt = Time.realtimeSinceStartup;

            if (aikanyt - gameoverinajankohta > aikamaarajokajatketaangameoverinjalkeen)
            {
                Time.timeScale = 0;
                SiirryGameOverJalkeiseenTilaan();
            }

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



        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

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

        if (spaceNappiaPainettu && deltaaikojensumma > ampumakertojenvalinenviive && OnkoAmmustenMaaraAlleMaksimin() && !OnkoSeinaOikealla())
        {
            //	if (!ammusInstantioitiinviimekerralla) {
            Vector3 v3 =
            new Vector3(0.1f +
            m_Rigidbody2D.position.x + (m_SpriteRenderer.bounds.size.x / 2), m_Rigidbody2D.position.y, 0);


            //audiosourcelaukaus.
            //    audiosourcelaukaus.Play();
            ad.AlusammusPlay();


            GameObject instanssi = Instantiate(ammusPrefab, v3, Quaternion.identity);

            AmmusController aa = instanssi.GetComponent<AmmusController>();
            aa.alus = this.gameObject;
            aa.SetAluksenluoma(true);
            aluksenluomienElossaOlevienAmmustenMaara++;



            instanssi.GetComponent<Rigidbody2D>().velocity = new Vector2(20, 0);
            AmmuOptioneilla();

            ammusinstantioitiin = true;
            deltaaikojensumma = 0;
            // instantioinninajankohta=



            //alas tippuva
            // missileDownCollected
            InstantioiBulletAlasTarvittaessa();

            InstantioiBulletYlosTarvittaessa();

        }
        ammusInstantioitiinviimekerralla = ammusinstantioitiin;


        if (teeOptionPallukka)
        {

            TeeOptioni();
            teeOptionPallukka = false;
        }


    }

    private void InstantioiBulletAlasTarvittaessa()
    {
        if (missileDownCollected >= 1 && instanssiBulletAlas == null)
        {


            Vector3 v3alas =
new Vector3(0.1f +
m_Rigidbody2D.position.x + (m_SpriteRenderer.bounds.size.x / 2), m_Rigidbody2D.position.y + 0.0f, 0);



            instanssiBulletAlas = Instantiate(bulletPrefab, v3alas, Quaternion.identity);
            IAlas alas = instanssiBulletAlas.GetComponent<IAlas>();
            if (alas != null)
            {
                //instanssiBulletAlas.SendMessage("Alas", true);
                alas.Alas(true);
                instanssiBulletAlas.GetComponent<Rigidbody2D>().velocity = new Vector2(0.1f, -2);
                instanssiBulletAlas.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
            }

        }
    }

    private void InstantioiBulletYlosTarvittaessa()
    {
        if (missileUpCollected >= 1 && instanssiBulletYlos == null)
        {


            Vector3 v3ylos =
new Vector3(0.1f +
m_Rigidbody2D.position.x + (m_SpriteRenderer.bounds.size.x / 2), m_Rigidbody2D.position.y + 0.0f, 0);



            instanssiBulletYlos = Instantiate(bulletPrefab, v3ylos, Quaternion.identity);
            //instanssiBulletYlos.SendMessage("Alas", false);

            IAlas alas = instanssiBulletYlos.GetComponent<IAlas>();
            if (alas != null)
            {
                //instanssiBulletAlas.SendMessage("Alas", true);
                alas.Alas(false);
            }

            instanssiBulletYlos.GetComponent<Rigidbody2D>().velocity = new Vector2(0.1f, 2);

            instanssiBulletYlos.GetComponent<Rigidbody2D>().gravityScale = -1.0f;

        }
    }

    public void VahennaaluksenluomienElossaOlevienAmmustenMaaraa()
    {
        aluksenluomienElossaOlevienAmmustenMaara = aluksenluomienElossaOlevienAmmustenMaara - 1;
    }


    private void AmmuOptioneilla()
    {
        foreach (OptionController obj in optionControllerit)
        {
            //   obj.gameObject.transform.position
            // Set the position
            obj.ammuNormilaukaus(ammusPrefab);
            if (missileDownCollected >= 1)
            {
                obj.ammuAlaslaukaus(bulletPrefab);
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
                    obj.ammuYloslaukaus(bulletPrefab);
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


    
    
    private void PaivitaDamagePalkkia()
    {
        //jos se on liikaa niin eikun gameoveria
        damagemittariController.SetDamage(damagenmaara, maksimimaaradamageajokakestetaan);

        TeeGameOver();



    }
    public bool demomode = true;

    private void TeeGameOver()
    {
        if (!demomode && !gameover && damagenmaara >= maksimimaaradamageajokakestetaan)
        {
            Time.timeScale = 0.1f;

            ad.ExplodePlay();
            Vector3 vektori =
    new Vector3(
    m_Rigidbody2D.position.x, m_Rigidbody2D.position.y, 0);

            // GameObject instanssiOption = Instantiate(gameoverPrefab, vektori, Quaternion.identity);

            GameObject rajahdys = Instantiate(explosion, vektori, Quaternion.identity);
            RajaytaSprite(gameObject, 4, 4, 1, aikamaarajokajatketaangameoverinjalkeen);
            m_SpriteRenderer.enabled = false;

            //  Destroy(instanssiOption, 10);



            //TogglePause();

            GameObject myObject = GameObject.Find("PauseButtonKaytossa");
            Image spriteRenderer = myObject.GetComponent<Image>();

            //Image image = GetComponent<Image>();
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false; // Hides the sprite
            }
            gameoverinajankohta = Time.realtimeSinceStartup;

            gameover = true;
        }

    }

    public float aikamaarajokajatketaangameoverinjalkeen = 5.0f;

    private float gameoverinajankohta;

    public void Explode()
    {
        damagenmaara = maksimimaaradamageajokakestetaan;
        PaivitaDamagePalkkia();

    }

    public void AiheutaDamagea(float damagemaara)
    {
        damagenmaara += damagemaara;
        // ExplodeTarvittaesssa();
        PaivitaDamagePalkkia();
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
        damagenmaara += damagemaarakasvastatuskunsavutaan;
        color.g = PalautaGvari();

        m_SpriteRenderer.color = color;
        PaivitaDamagePalkkia();

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





    

    void OnCollisionEnter2D(Collision2D col)
    {
        //haukisilmavihollinenexplodetag

        //explodetag
        if (col.collider.tag.Contains("hauki") || col.collider.tag.Contains("tiili") || col.collider.tag.Contains("pyoroovi") || col.collider.tag.Contains("laatikkovihollinenexplodetag"))
        {
            damagenmaara += maksimimaaradamageajokakestetaan;
            //ExplodeTarvittaesssa();
            PaivitaDamagePalkkia();
        }
        else if (col.collider.tag.Contains("pallovihollinen"))
        {
            damagenmaara += maksimimaaradamageajokakestetaan;
            //ExplodeTarvittaesssa();
            PaivitaDamagePalkkia();
        }
        else if (col.collider.tag.Contains("vihollinen"))
        {
            damagenmaara += 100;
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
                    ammustenmaksimaaraProperty += ammustenmaksimaaranLisaysKunSpeedBonusButtonOtettu;
                    ampumakertojenvalinenviive += ampujakertojenvalisenViiveenPienennysKunSpeedBonusButtonOtettu;

                }
                else if (btc.bonusbuttontype.Equals(BonusButtonController.Bonusbuttontype.MissileDown))
                {
                    Debug.Log("missile()");
                    missileDownCollected++;
                }
                else if (btc.bonusbuttontype.Equals(BonusButtonController.Bonusbuttontype.MissileUp))
                {
                    Debug.Log("missile()");
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

    private bool OnkoAmmustenMaaraAlleMaksimin()
    {
        int maksimi = ammustenmaksimaaraProperty;
        // int nykymaara = palautaAmmustenMaara();
        int nykymaara = aluksenluomienElossaOlevienAmmustenMaara;
        return nykymaara < maksimi;
    }


    void OnParticleTrigger()
    {
        // Debug.Log($"OnParticleTrigger hit: {other.name}");
        Debug.Log("OnParticleTrigger");
        // Add your collision handling logic here
    }






}
