﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityStandardAssets.CrossPlatformInput;

public class AlusController : BaseController, IExplodable
{



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

    public Vector3 palautaViimeinenSijaintiScreenpositioneissa(int optionjarjestysnumero)
    {

        //return aluksenpositiotCameraViewissa[optioidenmaksimaara * 10 - optionjarjestysnumero* 10];

        return base.palautaScreenpositioneissa(optioidenmaksimaara * 8 - optionjarjestysnumero * 8);

    }
    //   private List<Vector3> aluksenpositiotCameraViewissa = new List<Vector3>();
    private void tallennaSijaintiSailytaVainKymmenenViimeisinta()
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
    void Start()
    {
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




    }

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

    public float gravitymodifiermuutoskunammusosuu = 0.01f;


    public float gravitynminimi =- 0.2f;

    public void Savua()
    {
        if (!particleSystem.isPlaying)
        {
            particleSystem.Play();
        }

        var mainModule = particleSystem.main;
        float currentGravityModifier = mainModule.gravityModifier.constant;
        float uusiarvo = currentGravityModifier - gravitymodifiermuutoskunammusosuu;
        if (uusiarvo> gravitynminimi)
        {

            mainModule.gravityModifier = uusiarvo;
        }
        //mitäs sitten kun savuttaa
    }



    private List<BonusButtonController> bbc = new List<BonusButtonController>();

    // Update is called once per frame
    void Update()
    {

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
            }
        }
        // Handle mouse input for PC testing
        else if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            CheckIfTouched(mousePosition);
        }
        else if (Input.GetMouseButton(0) && _isDragging)
        {
            MoveObject(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }



        Vector2 moveDirection = moveInputActionReference.action.ReadValue<Vector2>();


        float moveDirectionAlas  =moveDownInputActionReference.action.ReadValue<float>();

        float moveDirectionUp= moveUpInputActionReference.action.ReadValue<float>();


        if (Input.GetKey("escape") || CrossPlatformInputManager.GetButtonDown("Quit"))
        {
            Debug.Log("Application would quit now.");
            Application.Quit();
        }

        m_Rigidbody2D.velocity = new Vector2(0.0f, 0.0f);

        

        float nappiHorizontal = Input.GetAxisRaw("Horizontal");
        if (nappiHorizontal==0.0f)
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

        if (autofire  || Input.GetKey(KeyCode.Space) || CrossPlatformInputManager.GetButton("Jump"))
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

        if (Input.GetKey(KeyCode.N) || CrossPlatformInputManager.GetButtonDown("Bonus") )
        {
            Debug.Log("BonusButtonPressed");
            BonusButtonPressed();
        }

        //  transform.position += new Vector3(0.4f, 0f, 0f) * Time.deltaTime;//sama skrolli kuin kamerassa

        tallennaSijaintiSailytaVainKymmenenViimeisinta();
    }
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
    private void CheckIfTouched(Vector3 inputPosition)
    {
        // Convert input position to world space
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, 0));

        // Check if the input position overlaps a 2D collider
        Collider2D hit = Physics2D.OverlapPoint(worldPosition);

        Debug.Log("hitti=" + hit);
        if (hit!=null)
        {
            bool onko = hit.transform == transform;
            Debug.Log("onko=" + onko);
        }

        if (hit != null && hit.transform == transform)
        {
            _isDragging = true;
            // Calculate offset between the object and touch/mouse position
            _offset = transform.position - worldPosition;
        }
    }


    //Vector3 viimeisin = Vector3.zero;

    private void MoveObject(Vector3 inputPosition)
    {
  //      float ero = Vector3.Distance(inputPosition, viimeisin);
    //    Debug.Log("hiiriero=" + ero);
       // if (ero>20.0f)
       // {
         //   viimeisin = inputPosition;
            // Offset the input position by 50 pixels to the right in screen space
            float offsetti = 100;
            Vector3 screenPositionWithOffset = new Vector3(inputPosition.x + offsetti, inputPosition.y, 0);

            // Convert the offset screen position to world space
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPositionWithOffset);

            //Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, 0));
            //Vector3 te = new Vector3(50, 0, 0);

            transform.position = worldPosition + _offset;
//        }





    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        return mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));
    }

    void FixedUpdate()
    {

        if (gameover)
        {
            GameObject instanssi = Instantiate(gameoverPrefab, new Vector3(10.1f +
+(m_SpriteRenderer.bounds.size.x / 2), 10, 0), Quaternion.identity);
            //  instanssi.GetComponent<Rigidbody2D>().velocity = new Vector2(20, 0);

            Destroy(instanssi, 1);
        }


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

        transform.position = new Vector2(transform.position.x + xsuuntamuutos, transform.position.y + ysuuntamuutos);

        asetaSijainti();

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

        if (spaceNappiaPainettu && deltaaikojensumma > ampumakertojenvalinenviive && onkoAmmustenMaaraAlleMaksimin() && !OnkoSeinaOikealla())
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
            ammuOptioneilla();

            ammusinstantioitiin = true;
            deltaaikojensumma = 0;
            // instantioinninajankohta=



            //alas tippuva
            // missileDownCollected
            if (missileDownCollected >= 1 && instanssiBulletAlas == null)
            {


                Vector3 v3alas =
new Vector3(0.1f +
m_Rigidbody2D.position.x + (m_SpriteRenderer.bounds.size.x / 2), m_Rigidbody2D.position.y +0.0f, 0);



                instanssiBulletAlas = Instantiate(bulletPrefab, v3alas, Quaternion.identity);
                IAlas alas= instanssiBulletAlas.GetComponent<IAlas>();
                if (alas!=null)
                {
                    //instanssiBulletAlas.SendMessage("Alas", true);
                    alas.Alas(true);
                    instanssiBulletAlas.GetComponent<Rigidbody2D>().velocity = new Vector2(0.1f, -2);
                    instanssiBulletAlas.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
                }

            }


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
            //	}
        }
        ammusInstantioitiinviimekerralla = ammusinstantioitiin;


        if (teeOptionPallukka)
        {

            TeeOptioni();
            /*
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

            //    instanssiOption.GetComponent<Rigidbody2D>().velocity = new Vector2(20, 0);
            */
            teeOptionPallukka = false;
        }

       
    }

    public void VahennaaluksenluomienElossaOlevienAmmustenMaaraa()
    {
        aluksenluomienElossaOlevienAmmustenMaara=aluksenluomienElossaOlevienAmmustenMaara-1;
    }


    private void ammuOptioneilla()
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
                if (bulletPrefab==null)
                {
                    Debug.Log("bulletti null");
                    return;
                }
                if(obj==null)
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

    private void asetaSijainti2()
    {
        // Debug.Log(alamaksiminMaarittava);

        // Vector3 

        // screenPosition = Camera.main.WorldToScreenPoint(alamaksiminMaarittava.transform.position);

        //screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        float halfWidth = m_SpriteRenderer.bounds.extents.x;
        float halfHeight = m_SpriteRenderer.bounds.extents.y;


        float camHeight = mainCamera.orthographicSize;
        float camWidth = mainCamera.aspect * mainCamera.orthographicSize;


        Vector3 pos = transform.position;

        // Calculate the bounds based on the camera size
        float minX = mainCamera.transform.position.x - camWidth;
        float maxX = mainCamera.transform.position.x + camWidth;
        float minY = mainCamera.transform.position.y - camHeight;
        float maxY = mainCamera.transform.position.y + camHeight;

        // Clamp the position to restrict the GameObject within the camera bounds
        //pos.x = Mathf.Clamp(pos.x, minX, maxX);
        //pos.y = Mathf.Clamp(pos.y, minY, maxY);



        pos.x = Mathf.Clamp(pos.x, minX + halfWidth, maxX - halfWidth);
        pos.y = Mathf.Clamp(pos.y, minY + halfHeight, maxY - halfHeight);

        if (pos.y < alamaksiminMaarittava.transform.position.y)
        {
            pos.y = alamaksiminMaarittava.transform.position.y;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0.0f);
        }

        if (ylamaksiminMaarittava != null)
        {
            if (pos.y > ylamaksiminMaarittava.transform.position.y)
            {
                pos.y = ylamaksiminMaarittava.transform.position.y;
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0.0f);
            }
        }


        // Apply the clamped position back to the GameObject
        transform.position = pos;

    }


    private void asetaSijainti()
    {
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
        float restrictedWidthInWorld = mainCamera.ScreenToWorldPoint(new Vector3(100.0f, 0)).x - mainCamera.ScreenToWorldPoint(Vector3.zero).x;

        // Adjust the minimum X to account for the restricted area
        float restrictedMinX = minX + restrictedWidthInWorld;

        // Clamp X and Y within bounds, considering the restricted area
        pos.x = Mathf.Clamp(pos.x, restrictedMinX + halfWidth, maxX - halfWidth);
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
    }


    public void Explode()
    {
        /*
        
  
        Destroy(gameObject, 0.1f);
        gameover = true;
        
        Time.timeScale = 0f;
        */
       // GameObject explosionIns = Instantiate(explosion, transform.position, Quaternion.identity);
       // Destroy(explosionIns, 1.0f);
       // Debug.Log("gameover");

        ad.ExplodePlay();
        Savua();

        // audiosourceexplode.Play();
    }



    public float rotationSpeed = 50.0f;

    /*
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.75f, 0.0f, 0.0f, 0.75f);

        // Convert the local coordinate values into world
        // coordinates for the matrix transformation.
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
    */

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
                else if (c.gameObject.tag == "tiilitag")
                {
                    return true;
                }
            }
        }
        return false;

    }





    /*
     * //If the Fire1 button is pressed, a projectile
    //will be Instantiated every 0.5 seconds.

    using UnityEngine;
    using System.Collections;

    public class Example : MonoBehaviour
    {
        public GameObject projectile;
        public float fireRate = 0.5f;
        private float nextFire = 0.0f;

        void Update()
        {
            if (Input.GetButton("Fire1") && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Instantiate(projectile, transform.position, transform.rotation);
            }
        }
    }
        */

    bool collision = false;


    void OnCollisionEnter2D(Collision2D col)
    {

        collision = true;
        //explodetag

        if (col.collider.tag.Contains("vihollinen"))
        {
 
            Explode();

        }

        /*
     if (col.collider.tag == "tiilitag")
     {

         Explode();

         //Destroy (col.gameObject);

     }
     else if (col.collider.tag == "pallerospritetag")
     {

         Explode();

         //Destroy (col.gameObject);

     }

     else if (col.collider.tag == "makitavihollinentag")
     {

         Explode();

         //Destroy (col.gameObject);

     }
     */


    }


    void OnCollisionStay2D(Collision2D col)
    {
        //   Debug.Log("on OnCollisionStay ");
        collision = false;
    }


    void OnCollisionExit2D(Collision2D col)
    {
        //    Debug.Log("on collision exit");

        collision = false;
    }



    //void OnDrawGizmos()
    //{
    //    // Draws a 5 unit long red line in front of the object
    //    Gizmos.color = Color.red;
    //    Vector3 direction = transform.TransformDirection(Vector3.forward) * 50;
    //    Gizmos.DrawRay(transform.position, direction);
    //}



    //void OnDrawGizmosSelected()
    //{
    //    // Draws a 5 unit long red line in front of the object
    //    Gizmos.color = Color.red;
    //    Vector3 direction = transform.TransformDirection(Vector3.forward) * 50;
    //    Gizmos.DrawRay(transform.position, direction);
    //}
    //se raycasti tee sillä
    //tai sitten ammuskärki oma gameobjecti joka on se neliö



    public void BonusCollected()
    {
     //   audiosourcebonus.Play();

        //Debug.Log("BonusCollected aluksella");

        //olemme koskeneet bonukseen

        //    public BonusButtonController.Bonusbuttontype speedbonusbutton;
        //    public BonusButtonController.Bonusbuttontype missilebonusbutton;

        //bool valittu=  speedbonusbutton.
        //     speedbonusbutton


        // BonusButtonController[] bs = (BonusButtonController[])FindObjectsOfType(typeof(BonusButtonController));
        /*      */


        //siinä on nyt järjestyksessä
        int selectedIndex = -1;
        foreach (BonusButtonController btc in bbc)
        {
            // do whatever with each 'enemy' here
            // Debug.Log(""+btc.order+ " btc.selected=" +btc.selected.ToString() + " btc.used= " + btc.used.ToString);


            //Debug.Log("btc.order=" + btc.order + " btc.selected=" + btc.selected + " btc.usedcount=" + btc.usedcount);
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
    private int palautaAmmustenMaara()
    {
        return GameObject.FindGameObjectsWithTag("ammustag").Length;

    }
    private bool onkoAmmustenMaaraAlleMaksimin()
    {
        int maksimi = ammustenmaksimaaraProperty;
       // int nykymaara = palautaAmmustenMaara();
        int nykymaara = aluksenluomienElossaOlevienAmmustenMaara;
        return nykymaara < maksimi;
    }

    float reloadEndTime = 0f;
    float reloadTimer = 3f;
    private bool onkoRealoadMenossa()
    {
        bool IsReloading = Time.time < reloadEndTime;
        // bool CanShoot => !IsReloading; // other conditions...
        reloadEndTime = Time.time + reloadTimer;

        return IsReloading;
    }


    void OnParticleTrigger()
    {
        // Debug.Log($"OnParticleTrigger hit: {other.name}");
        Debug.Log("OnParticleTrigger");
        // Add your collision handling logic here
    }

    /*

    void OnParticleCollision(GameObject other)
    {
        // Check if the object that collided is a Particle System
        ParticleSystem particleSystem = other.GetComponent<ParticleSystem>();

      

        if (particleSystem != null)
        {
            Debug.Log("particleSystem.name=" + particleSystem.name);
            Debug.Log("Particle collided with " + gameObject.name);
            // Additional logic here
        }
    }
    */

}
