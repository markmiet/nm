using DigitalRuby.AdvancedPolygonCollider;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


public class VaihtoController : BaseController
{
    //@todo ota turhat pois

    //käytä tätä konttimateriaalin kanssa

    //yleisesti ottaen käytetään vain niin että taustaumuoto on olemassa
    //ja sitten asetetaan vain tekstuuri jolla se täytetään

    //toinen mode on sitten se, että sitä voi ampua ja ampuessa tulee reikiä -> osia irtoo
    //tälle joku flagi missä modessa on

    //ja siis update metodi voi olla tyhjä?

    public bool bulletholemode = false;

    public bool jakaudutarvittaessa = false;


    public Texture2D noisetex;//tämän on siis se tekstuuri joka piirretään maintex eli sprite muodon päälle
    public Vector2 tilingoffset = Vector2.one;//1,1 ei toista 
    public float muunnossykli = 1.0f;//ei käytössä

    public float polygoniminiarvo = 0.5f;//eli jos polygon colliderin alue on tätä pienempi niin destroy

    public byte alphaThresholdCountissa = 0;
    public byte blackThresholdCountissa = 5;

    [Range(0f, 1f)] public float alphaThreshold = 0.1f; //SplitDisconnectedRegions tässsä käytössä
                                                        // public float simplificationTolerance = 0.5f; // Optional: simplify final shape

    public float bulletHoleCooldown = 0.1f; // 20 bullet holes per second max oncollisionenterissä siis rajoitetaan
    public float regioninminimikoko = 10.0f;

    public float radiuskertoma = 1000.0f;//bulletin

    public int bulletholenradiusminimi = 40;
    public int bulletholenradiusmaksimi = 45;

    //public int maxSearchRadius = 20; //bulletin tune this as needed

    public GameObject savu;//bulletin savu
    public float savunkesto = 0.5f;//bullet osuu niin savun kesto
    public float smokeCooldown = 0.05f; // 10 particles per second max

    private float lastBulletHoleTime = 0f;

    // public int vahintaan = 50;
    public float pikseliminimimaara = 100;
    public int spritenkorkeudenmini = 200;//ihmetyttää miksi näin iso


    //  public Texture2D bulletHoleMask; // Must be a small alpha-blended texture (e.g. transparent circle)

    private SpriteRenderer _spriteRenderers;
    private Material _materials;
    private int _NoiseTex = Shader.PropertyToID("_NoiseTex");
    private int _MainTex = Shader.PropertyToID("_MainTex");

    private int _TilingOffset = Shader.PropertyToID("_TilingOffset");

    public int bulletholelistankoko = 10;
    public float bulletholestep = 0.1f;


    private Texture2D maintekstuuri;


    // private float laskuri = 0;
    // Start is called before the first frame update

    private float las = 0.0f;
    private float sykl = 1.0f;
    public void Update()
    {
        las += Time.deltaTime;
        if (las>=sykl)
        {
            //   TeeMainTekstuuriVaihtoReal();
            // a.StartRebuild(gameObject);
            bool splittitehtiin = SplitDisconnectedRegions();

            las = 0.0f;
        }

    }


    NativeArray<Color32> pixelData;

    void Start()
    {

   //     rr = GetComponent<RuntimePixelPerfectCollider>();

        //  a = gameObject.AddComponent<AdvancedPolygonColliderManager>();
        las = sykl;
      //  Time.fixedDeltaTime = 0.001f;
        _spriteRenderers = GetComponent<SpriteRenderer>();
        _materials = new Material(_spriteRenderers.material);
        // _spriteRenderers.material = _materials;

        Texture2D original = _spriteRenderers.sprite.texture;

        maintekstuuri = new Texture2D(original.width, original.height, original.format, false);

        //pixelData = new NativeArray<Color32>(original.width* original.height, Allocator.Persistent);
        // pixelData = original.GetPixelData();
        // Get pixel data as a NativeArray<Color32>


        maintekstuuri.SetPixels(original.GetPixels());
        maintekstuuri.Apply();

        pixelData = maintekstuuri.GetPixelData<Color32>(0); // 0 = mip level

        TeeMainTekstuuriVaihtoReal();
        collisiolaskuri = minimiaikaMikaPitaaOllaCollisioidenValissa;
        
      //  GetComponent<RuntimePixelPerfectCollider>();


    }

    RuntimePixelPerfectCollider rr;
    //  AdvancedPolygonColliderManager a;






    public int CountNonAlphaPixels(Sprite sprite)
    {
        if (sprite == null || sprite.texture == null)
            return 0;

        Texture2D texture = sprite.texture;
        Color32[] pixels = texture.GetPixels32();

        Rect rect = sprite.textureRect;
        int textureWidth = texture.width;

        int count = 0;

        int xMin = (int)rect.x;
        int yMin = (int)rect.y;
        int width = (int)rect.width;
        int height = (int)rect.height;

        for (int y = yMin; y < yMin + height; y++)
        {
            for (int x = xMin; x < xMin + width; x++)
            {
                int index = y * textureWidth + x;
                Color32 pixel = pixels[index];

                // Treat pixel as non-visible if it's fully transparent or nearly black
                bool isTransparent = pixel.a <= alphaThresholdCountissa;
                bool isAlmostBlack = pixel.r <= blackThresholdCountissa &&
                                     pixel.g <= blackThresholdCountissa &&
                                     pixel.b <= blackThresholdCountissa;

                if (!isTransparent && !isAlmostBlack)
                {
                    count++;
                }
            }
        }

        return count;
    }

    
    private void TarkistaKoko()
    {
        /*
        PolygonCollider2D p = gameObject.GetComponent<PolygonCollider2D>();
        if (p != null)
        {
            float alue = CalculatePolygonArea(p, gameObject);
            //  Debug.Log("alue=" + alue);
            if (alue < polygoniminiarvo)
            {
                //    Destroy(gameObject);
                //       return;
            }
        }
        */


        int maara = CountNonAlphaPixels(GetComponent<SpriteRenderer>().sprite);
       // Debug.Log("maara=" + maara);
        if (maara < pikseliminimimaara)
        {
            //Destroy(gameObject);
            BaseDestroy();
            return;
        }
        int korkeus = GetSpriteHeightInPixels();
        if (korkeus < spritenkorkeudenmini)
        {
            //Destroy(gameObject);
            BaseDestroy();
            return;
        }

        if (GetComponent<SpriteRenderer>()!=null)
        {
            Sprite s = GetComponent<SpriteRenderer>().sprite;
            float xkoko=s.bounds.size.x;
            float ykoko = s.bounds.size.y;
           // Debug.Log("sprite kokox=" + xkoko + " ykoko=" + ykoko);
            float ala = xkoko * ykoko;
            if (ala< minimipintaala)
            {
                //Destroy(gameObject);
                BaseDestroy();

            }
        }


    }
    public float minimipintaala = 1.0f;


    public int GetSpriteHeightInPixels()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;


        if (sprite == null)
        {
            Debug.LogWarning("Sprite is null.");
            return 0;
        }

        return Mathf.RoundToInt(sprite.rect.height);
    }


    private bool splittitutki = true;


    private void RebuildCollider()
    {
        
        float alku = Time.realtimeSinceStartup;

        
        PolygonCollider2D[] oldCol = GetComponents<PolygonCollider2D>();
        foreach (PolygonCollider2D pp in oldCol)
        {
            Destroy(pp);
        }
        PolygonCollider2D collider = gameObject.AddComponent<PolygonCollider2D>();

                TarkistaKoko();
        splittitutki = true;
     //   bool splittitehtiin = SplitDisconnectedRegions();


        // a.StartRebuild(gameObject);

        //pa.RebuildCollider();

        float loppu = Time.realtimeSinceStartup;

      //  Debug.Log("collider " + (loppu - alku));

        // GenerateBoxCollidersFast();

       //  RecalculatePolygon();

        //RecalculatePolygonCo();

        // StartCoroutine(RecalculatePolygonCo());
        //        TarkistaKoko();

        //bool splittitehtiin = SplitDisconnectedRegions();


        //if (!splittitehtiin)
        //  GenerateCollider();

        // GenerateCollider();
        //  GeneratePreciseCollider();
        // 
    }

    
    public string[] tagijohonreagoidaan = new string[] { "ammus" };


    private bool Reagoidaanko(string tag)
    {
        if (tagijohonreagoidaan==null || tagijohonreagoidaan.Length==0)
        {
            return false;
        }
        foreach(string t in tagijohonreagoidaan)
        {
            if (tag.Contains(t))
            {
                return true;
            }
        }
        return false;
    }

    public bool tutkitormaysvoima = false;

    public float tormaysvoimajokavaaditaan = 10.0f;



    public bool ignoraaOsaCollisioista = false;

  
    public float minimiaikaMikaPitaaOllaCollisioidenValissa = 0.1f;
    private float collisiolaskuri = 0.0f;

    public bool tutkiFps=false;
    private float minFPS = 50f; // raja, alle tätä ei instansioida


    public void OnCollisionEnter2D(Collision2D col)
    {
        if (IsGoingToBeDestroyed()) {
            return;
        }

        if (!bulletholemode)
        {
            return;
        }
        if (ignoraaOsaCollisioista)
        {
            collisiolaskuri += Time.deltaTime;
        }


        //tutki voima
        if (Reagoidaanko( col.collider.tag))
        {
            if (tutkiFps)
            {
                float smoothFPS = 1f / Time.smoothDeltaTime;
                if (smoothFPS < minFPS)
                {
                    Debug.Log("fps oli " + smoothFPS + " joka on alle rajan " + minFPS);
                    return;
                }
            }
            if (ignoraaOsaCollisioista)
            {
                
                if (collisiolaskuri < minimiaikaMikaPitaaOllaCollisioidenValissa)
                {

                    return;
                }
                else
                {
                    collisiolaskuri = 0.0f;
                }
            }



            if (tutkitormaysvoima)
            {
                // collision2D.relativeVelocity = törmäyksen suhteellinen nopeus
                float voimakkuus = col.relativeVelocity.magnitude;
                if (voimakkuus >= tormaysvoimajokavaaditaan)
                {
                 //   Debug.Log($"Reagoidaan törmäykseen tagilla {col.collider.tag}, voima {voimakkuus}");
                    // Tee haluttu toiminto (esim. lisää bullet hole)
                }
                else
                {
                   // Debug.Log($"EI REAGOIDA törmäykseen tagilla {col.collider.tag}, voima {voimakkuus}");
                    return;
                }
            }
            

            float currentTime = Time.time;

            // Rate limit
            if (currentTime - lastBulletHoleTime < bulletHoleCooldown)
                return;


            lastBulletHoleTime = currentTime;
            Vector2 hitPoint = col.GetContact(0).point;
            BulletHole(col.relativeVelocity, hitPoint, col.gameObject);

        }
    }




    public void BulletHole(Vector2 relvel, Vector2 hitPoint,GameObject go)
    {
        bool muuttuiko = NewMethod(relvel, hitPoint, go,1);

        if (!muuttuiko)
        {
            Debug.Log("ei muutosta");
            //RebuildCollider();
           bool uus= NewMethod(relvel, hitPoint, go, 2);
            if (!uus)
            {
                Debug.Log("ei uusikaan muutosta");
            }
        }
    }

    private bool NewMethod(Vector2 relvel, Vector2 hitPoint, GameObject go,int listakerroin)
    {
        bool muuttuiko = false;
        List<Vector2> lista = GetWorldPointList(hitPoint, bulletholelistankoko*listakerroin, relvel, bulletholestep);

        foreach (Vector2 v in lista)
        {
            Vector2 localPoint = transform.InverseTransformPoint(v);

            // Convert local point to texture space
            Sprite sprite = _spriteRenderers.sprite;
            Rect rect = sprite.rect;
            //Vector2 pivot = sprite.pivot;

            // int texX = Mathf.RoundToInt((localPoint.x * sprite.pixelsPerUnit) + pivot.x);
            // int texY = Mathf.RoundToInt((localPoint.y * sprite.pixelsPerUnit) + pivot.y);

            Vector2 pivot = sprite.pivot / sprite.pixelsPerUnit;
            Vector2 texCoord = (localPoint + pivot) * sprite.pixelsPerUnit;

            // Draw bullet hole at texX, texY
            // DrawBulletHole(texX, texY, col.gameObject.GetComponent<SpriteRenderer>().sprite.texture);
            int texX = Mathf.RoundToInt(texCoord.x);
            int texY = Mathf.RoundToInt(texCoord.y);


            bool change = DrawBulletHole3(texX, texY, go);
            if (change)
            {
                muuttuiko = true;
                break;
            }

        }

        return muuttuiko;
    }

    private List<Vector2> GetWorldPointList(Vector2 startPoint, int listSize, Vector2 velocity, float stepDistance = 0.1f)
    {
        List<Vector2> points = new List<Vector2>();

        //Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        //if (rb == null || rb.velocity == Vector2.zero) return points;

        //Vector2 direction = rb.velocity.normalized;
        Vector2 direction = velocity.normalized;



        for (int i = 0; i < listSize; i++)
        {
            Vector2 point = startPoint + direction * stepDistance * i;
            points.Add(point);
        }

        return points;
    }

    private bool polyisdirty = true;
   // private void TeeMainTekstuuriVaihto()
  //  {
    //    polyisdirty = true;
   // }

  //  Color32[] pixels;

    private void TeeMainTekstuuriVaihtoReal()
    {
        /*
        polyisdirty = true;
        if (!polyisdirty)
        {
            return;
        }
        */
       // maintekstuuri.Apply();
        _materials.SetTexture(_NoiseTex, noisetex);
        _materials.SetVector(_TilingOffset, tilingoffset);
        if (maintekstuuri != null)
        {
            _materials.SetTexture(_MainTex, maintekstuuri);
            // Create a new sprite from the texture using original sprite settings
            Sprite baseSprite = _spriteRenderers.sprite;
            Sprite newSprite = Sprite.Create(
                maintekstuuri,
                new Rect(0, 0, maintekstuuri.width, maintekstuuri.height),
                baseSprite != null ? baseSprite.pivot / baseSprite.rect.size : new Vector2(0.5f, 0.5f),
                baseSprite != null ? baseSprite.pixelsPerUnit : 100f
            );
            _spriteRenderers.sprite = newSprite;
            _spriteRenderers.material = _materials;

            if (bulletholemode)
            {
                float aika = Time.realtimeSinceStartup;

                    RebuildCollider();//MJM TODOO OTA POIS KOMMENTEISTA

                //  rr.UpdateColliderMIksi();
                // GenerateBoxCollidersFast();
                //StartCoroutine(RebuildColliderCoroutine());

                float loppu = Time.realtimeSinceStartup;

                float erotus = loppu - aika;
            //    Debug.Log("erotus=" + erotus);



            }

        }
       // polyisdirty = false;
    }
 

    //private bool collre = false;
    




    
    public static Vector2 GetWorldPositionFromPixelIndex(int index, int textureWidth, SpriteRenderer spriteRenderer)
    {
        int texX = index % textureWidth;
        int texY = index / textureWidth;

        Sprite sprite = spriteRenderer.sprite;
        Transform transform = spriteRenderer.transform;

        float ppu = sprite.pixelsPerUnit;
        Rect rect = sprite.rect;         // Rect inside the texture
        Vector2 pivot = sprite.pivot;    // Pivot in pixels

        // Adjust texX and texY relative to the sprite's rect and pivot
        float spriteX = texX - rect.x;
        float spriteY = texY - rect.y;

        // Offset from pivot
        float localX = (spriteX - pivot.x) / ppu;
        float localY = (spriteY - pivot.y) / ppu;

        Vector2 localPos = new Vector2(localX, localY);

        // Transform to world space
        return transform.TransformPoint(localPos);
    }



    private float lastSmokeTime = 0f;

    //public float travelDistance = 140f;


    private bool pixeltaulukkoalustettu = false;
    Color32[] pixels;

    bool DrawBulletHole3(int centerX, int centerY, GameObject go)
    {
        //ei hyvä laske jotenkin colliderista
        //tai sitten siitä mikä on prosentuaalisesti x suunnassa leveys kuvasta
        //tällä kerrotaan sitten lopulta
        /*
        float leveys = GetSpriteScreenWidth(go, Camera.main);
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null || camera == null)
            return 0f;
        */
        // Get sprite bounds in world space
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        Bounds bounds = sr.bounds;
        float korkeus = bounds.size.y * bounds.size.x;



        /*
        Vector3 worldLeft = new Vector3(bounds.min.x, bounds.center.y, 0);
        Vector3 worldRight = new Vector3(bounds.max.x, bounds.center.y, 0);

        // Convert world points to screen space
        Vector3 screenLeft = Camera.main.WorldToScreenPoint(worldLeft);
        Vector3 screenRight = Camera.main.WorldToScreenPoint(worldRight);

        float screenWidth = Mathf.Abs(screenRight.x - screenLeft.x);


        float ppu = go.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;

        float spritewidthinworld=leveys*
            */
        /*
        leveys = 0.0f;
        if (Mathf.Approximately(leveys, 0.0f))
        {
            float maxWidth = 0f;

            Collider2D[] colliders = go.GetComponentsInParent<Collider2D>();
            foreach (Collider2D col in colliders)
            {

               
                float worldWidth = col.bounds.size.x; // Width in world units
                

                if (worldWidth > maxWidth)
                    maxWidth = worldWidth;


            }

            leveys = maxWidth;
        }
        */


        //int holeRadius =(int)( leveys / 2.0f);
        //     int holeRadius = (int)(korkeus * leveydenkertoma);
        int holeRadius = Mathf.RoundToInt(korkeus * radiuskertoma);

        holeRadius=Math.Clamp(holeRadius, bulletholenradiusminimi, bulletholenradiusmaksimi);

        /*
            public float bulletholenleveydenminimi = 10;
    public float bulletholenleveydenmaksimi = 100;
    */


    /*
    // Estimate size from GameObject scale (e.g., bullet size)
    float avgScale = (go.transform.lossyScale.x + go.transform.lossyScale.y) * 0.5f;
    float worldRadius = avgScale * 0.5f; // in world units

    // Convert to texture-space pixels
    float pixelsPerUnit = _spriteRenderers.sprite.pixelsPerUnit;

    float scaleFactor = 0.1f; // tweak this to get good hole size (0.1–0.3)
    int holeRadius = Mathf.RoundToInt(worldRadius * pixelsPerUnit * scaleFactor);
    holeRadius = Mathf.Clamp(holeRadius, 1, 16); // optional safety clamp
    */

    //Color32[] pixels = maintekstuuri.GetPixels32();
      //  int width = maintekstuuri.width;
        //int height = maintekstuuri.height;


        if (!pixeltaulukkoalustettu)
        {
            pixels = maintekstuuri.GetPixels32();
            pixeltaulukkoalustettu = true;


        }
        else
        {

        }
        int width = maintekstuuri.width;
        int height = maintekstuuri.height;

        bool changed = false;
        //tällä, mutta hidas?
        //        Vector2 vv = GetWorldPositionFromPixelIndex(index, width, GetComponent<SpriteRenderer>());

        // int changecount = 0;

        int indeksipienin = -1;
        int indeksisuurin = -1;
        for (int y = -holeRadius; y <= holeRadius; y++)
        {
            for (int x = -holeRadius; x <= holeRadius; x++)
            {
                if (x * x + y * y <= holeRadius * holeRadius)
                {
                    int texX = centerX + x;
                    int texY = centerY + y;

                    if (texX >= 0 && texX < width && texY >= 0 && texY < height)
                    {
                        int index = texY * width + texX;
                        Color32 c = pixels[index];
                        //Vector2Int pixelCoord = new Vector2Int(texX, texY);

  
                        // Only clear non-transparent pixels
                        if (c.a != 0 || c.r != 0 || c.g != 0 || c.b != 0)
                        {
                            //pixels[index] = new Color32(0, 0, 0, 0); // transparent

                            pixelData[index] = new Color32(0, 0, 0, 0); // transparent
                            /*
                            if (indeksipienin<0)
                            {
                                indeksipienin = index;
                            }
                            if (indeksipienin>index)
                            {
                                indeksipienin = index;
                            }

                            if (indeksisuurin < 0)
                            {
                                indeksisuurin = index;
                            }
                            if (indeksisuurin < index)
                            {
                                indeksisuurin = index;
                            }
                            */

                            if (savu != null && Time.time - lastSmokeTime >= smokeCooldown)
                            {
                                Vector2 vv = GetWorldPositionFromPixelIndex(index, width, GetComponent<SpriteRenderer>());
    //                            Debug.Log("vv=" + vv);
                                //tähän joku rajoitin, että ei voi tehdä tuhatta sekunnissa jne.
                                GameObject savuinsta = Instantiate(savu, vv, Quaternion.identity);
                                Destroy(savuinsta, savunkesto);
                                lastSmokeTime = Time.time;
                            }


                            changed = true;
                            /*
                            changecount++;
                            if (changecount>=1000)
                            {
                                break;
                            }
                            */
                        }
                    }
                }
            }
        }

        if (changed)
        {
            //maintekstuuri.SetPixels(xMin, yMin, width, height, modifiedPixels);
            //maintekstuuri.Apply();


            //maintekstuuri.SetPixels32(pixels);

          
            maintekstuuri.SetPixelData(pixelData, 0);


           // float alku = Time.realtimeSinceStartup;
            maintekstuuri.Apply();
            // float loppu = Time.realtimeSinceStartup;

            TeeMainTekstuuriVaihtoReal();

            // Debug.Log("set maintekstuuri apply kesto=" + (loppu - alku));
            /*
            NativeArray<Color32> holePixels = new NativeArray<Color32>(indeksisuurin- indeksipienin+1, Allocator.Temp);
            int j = 0;
            for (int i= indeksipienin; i<= indeksisuurin;i++)
            {
                holePixels[j++]=pixels[i];
            }



            maintekstuuri.SetPixelData(holePixels, 0, indeksipienin);
            */

            // maintekstuuri.SetPixelData(pixelData, 0, 10);

            //  maintekstuuri.Apply();

            // If visual or physics change is needed:
         //   TeeMainTekstuuriVaihtoReal(); // your own method
                                      // RebuildCollider(); // optionally refresh collider
            return true;
        }
        else
        {
            if (true)
                return false;
            /*
            bool pixelCleared = false;

            int maara = 0;

            for (int radius = 1; radius <= maxSearchRadius && !pixelCleared; radius++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    for (int dx = -radius; dx <= radius; dx++)
                    {
                        // Only search the edges of the current square ring
                        if (Mathf.Abs(dx) != radius && Mathf.Abs(dy) != radius) continue;

                        int texX = centerX + dx;
                        int texY = centerY + dy;

                        if (texX >= 0 && texX < width && texY >= 0 && texY < height)
                        {
                            int index = texY * width + texX;
                            Color32 c = pixels[index];

                            if (c.a != 0 || c.r != 0 || c.g != 0 || c.b != 0)
                            {
                                pixels[index] = new Color32(0, 0, 0, 0); // make transparent
                                changed = true;
                                pixelCleared = true;
                                //break;
                                maara++;
                            }
                        }
                    }
                    // if (pixelCleared) break;
                    if (maara >= vahintaan)
                    {
                        break;
                    }
                }
            }

            if (changed)
            {
                maintekstuuri.SetPixels32(pixels);
                maintekstuuri.Apply();
                TeeMainTekstuuriVaihto(); // your update logic
            }
            */
        }

    }



    //splitti osuus:


    public bool SplitDisconnectedRegions()
    {
        float alku = Time.realtimeSinceStartup;

        if (!jakaudutarvittaessa)
        {
            return false;
        }
        if (!splittitutki)
        {
            return false;
        }

        splittitutki = false;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Sprite originalSprite = sr.sprite;
        Texture2D originalTex = originalSprite.texture;

        if (!originalTex.isReadable)
        {
            Debug.LogError("Texture must be marked as Read/Write in import settings.");
            return false;
        }

        int width = originalTex.width;
        int height = originalTex.height;
        float ppu = originalSprite.pixelsPerUnit;
        Vector2 pivotPx = originalSprite.pivot;

        //        Color[] pixels = originalTex.GetPixels();//mjm nopeutuuko?

        Color32[] pixels = originalTex.GetPixels32();


        bool[,] alphaMask = new bool[width, height];

        // Build alpha mask
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                alphaMask[x, y] = pixels[y * width + x].a > alphaThreshold;
            }
        }


        List<List<Vector2Int>> regions = FindConnectedRegions(alphaMask, width, height);


        regions.Sort((a, b) => b.Count.CompareTo(a.Count));

        if (regions.Count <= 1)
        {
         //   Debug.Log("No disconnected regions found.");
            return false;
        }
        bool first = true;
        bool jokutehtiin = false;
        foreach (var region in regions)
        {
            // if (region.Count < 10) continue; // Skip tiny debris

            RectInt bounds = GetRegionBounds(region);
            if (bounds.width * bounds.height < regioninminimikoko)
            {
                Debug.Log("liian pieni leveys=" + bounds.width + " korkeus=" + bounds.height);

                //    continue;

            }


            Texture2D regionTex = new Texture2D(bounds.width, bounds.height);
            /*
            Color[] regionPixels = new Color[bounds.width * bounds.height];
            for (int i = 0; i < regionPixels.Length; i++)
            {
                regionPixels[i] = new Color(0, 0, 0, 0);
            }
            */
            Color[] regionPixels = new Color[bounds.width * bounds.height];
            Array.Fill(regionPixels, new Color(0, 0, 0, 0));
            int nonTransparentPixelCount = 0;
            // Copy pixels into new texture and clear them from the original

            float aa = Time.realtimeSinceStartup;

            foreach (var p in region)
            {
                int localX = p.x - bounds.x;
                int localY = p.y - bounds.y;
                Color c = pixels[p.y * width + p.x];
                regionPixels[localY * bounds.width + localX] = c;

                // Clear from original
                pixels[p.y * width + p.x] = new Color(0, 0, 0, 0);


                if (c.a > 0f) // Count only non-transparent pixels
                    nonTransparentPixelCount++;
            }

         //   float bb = Time.realtimeSinceStartup;
       //     Debug.Log("looppi kesto=" + (bb - aa));


            if (nonTransparentPixelCount < pikseliminimimaara)
            {
                Debug.Log("nonTransparentPixelCount=" + nonTransparentPixelCount);

                continue;
            }

            regionTex.SetPixels(regionPixels);
            regionTex.Apply();


            Vector3 originalPivotWorld = transform.position;

            // New pivot (in normalized space) within the cropped region
            Vector2 pivotInRegion = (pivotPx - bounds.position) / new Vector2(bounds.width, bounds.height);

            // Create the sprite with corrected pivot
            Sprite regionSprite = Sprite.Create(
                regionTex,
                new Rect(0, 0, bounds.width, bounds.height),
                pivotInRegion,
                ppu
            );
            int pikselimaara = CountNonAlphaPixels(regionSprite);
            if (pikselimaara < pikseliminimimaara)
            {
                Destroy(regionSprite);

                continue;

            }


            // Clone original GameObject (you must ensure CloneGameObject doesn’t copy SplitDisconnectedRegions again)
            GameObject newPart = CloneGameObject(gameObject);
            jokutehtiin = true;

            SpriteRenderer newSR = newPart.GetComponent<SpriteRenderer>();




            // Clone material if needed





            newSR.sprite = regionSprite;

            // Position stays the same (since we aligned pivot properly)
            newPart.transform.position = originalPivotWorld;

            if (newSR.material != null)
            {
                newSR.material = new Material(newSR.material);
            }

            // Rebuild collider
            var vaihto = newPart.GetComponent<VaihtoController>();
            if (vaihto != null)
            {
             //   float a = Time.realtimeSinceStartup;
                vaihto.RebuildCollider();
               // float b = Time.realtimeSinceStartup;
               // Debug.Log("vaihto.RebuildCollider() kesto=" + (b - a));


                // StartCoroutine(vaihto.RebuildColliderNextFrame());
                //        vaihto.RebuildCollider();
            }
            //vaihto.RebuildCollider();


            first = false;
        }

        // Update the original texture
        //originalTex.SetPixels(pixels);
        //  originalTex.Apply();

        // Destroy the original object
        if (jokutehtiin)
        {
            //Destroy(gameObject);
            BaseDestroy();

        }
       // float loppu = Time.realtimeSinceStartup;

       // Debug.Log("splitin kesto=" + (loppu - alku));

        return true;
    }



    List<List<Vector2Int>> FindConnectedRegions(bool[,] mask, int width, int height)
    {
        List<List<Vector2Int>> regions = new List<List<Vector2Int>>();
        bool[,] visited = new bool[width, height];
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (!mask[x, y] || visited[x, y]) continue;

                List<Vector2Int> region = new List<Vector2Int>();
                Queue<Vector2Int> queue = new Queue<Vector2Int>();
                queue.Enqueue(new Vector2Int(x, y));
                visited[x, y] = true;

                while (queue.Count > 0)
                {
                    Vector2Int p = queue.Dequeue();
                    region.Add(p);

                    for (int i = 0; i < 4; i++)
                    {
                        int nx = p.x + dx[i];
                        int ny = p.y + dy[i];
                        if (nx >= 0 && ny >= 0 && nx < width && ny < height &&
                            mask[nx, ny] && !visited[nx, ny])
                        {
                            visited[nx, ny] = true;
                            queue.Enqueue(new Vector2Int(nx, ny));
                        }
                    }
                }

                regions.Add(region);
            }
        }

        return regions;
    }

    RectInt GetRegionBounds(List<Vector2Int> region)
    {
        int minX = int.MaxValue, minY = int.MaxValue;
        int maxX = int.MinValue, maxY = int.MinValue;

        foreach (var p in region)
        {
            if (p.x < minX) minX = p.x;
            if (p.x > maxX) maxX = p.x;
            if (p.y < minY) minY = p.y;
            if (p.y > maxY) maxY = p.y;
        }

        return new RectInt(minX, minY, maxX - minX + 1, maxY - minY + 1);
    }


    public GameObject CloneGameObject(GameObject original, string name = "ClonedObject")
    {
        GameObject clone = Instantiate(original);
        clone.name = name;

        // Optional: Reset position, rotation, scale
        clone.transform.position = original.transform.position;
        clone.transform.rotation = original.transform.rotation;
        clone.transform.localScale = original.transform.localScale;




        // Important: Clone shared materials so modifying them won't affect the original
        SpriteRenderer sr = clone.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.material = new Material(sr.material); // Deep clone material
        }

        return clone;
    }




}
