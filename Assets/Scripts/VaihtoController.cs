using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VaihtoController : MonoBehaviour
{
    //@todo ota turhat pois

    //yleisesti ottaen käytetään vain niin että taustaumuoto on olemassa
    //ja sitten asetetaan vain tekstuuri jolla se täytetään

    //toinen mode on sitten se, että sitä voi ampua ja ampuessa tulee reikiä -> osia irtoo
    //tälle joku flagi missä modessa on

    //ja siis update metodi voi olla tyhjä?

    public bool bulletholemode = false;


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

    public float leveydenkertoma = 5;//bulletin

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
    void Start()
    {
        _spriteRenderers = GetComponent<SpriteRenderer>();
        _materials = new Material(_spriteRenderers.material);
        // _spriteRenderers.material = _materials;

        Texture2D original = _spriteRenderers.sprite.texture;

        maintekstuuri = new Texture2D(original.width, original.height, original.format, false);
        maintekstuuri.SetPixels(original.GetPixels());
        maintekstuuri.Apply();
        TeeMainTekstuuriVaihto();

        //  _spriteRenderers.material = _materials;

        // gameObject.AddComponent<PolygonCollider2D>();
        /*
         * 
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

            // Apply new sprite to the renderer
            _spriteRenderers.sprite = newSprite;
        }



        // Remove and recreate collider on next frame
        StartCoroutine(RebuildColliderNextFrame());
        */
    }

    float CalculatePolygonArea(PolygonCollider2D collider, GameObject para)
    {
        float scaleX = para.transform.lossyScale.x;
        float scaleY = para.transform.lossyScale.y;

        float area = 0f;

        for (int path = 0; path < collider.pathCount; path++)
        {
            Vector2[] points = collider.GetPath(path);
            int count = points.Length;

            for (int i = 0; i < count; i++)
            {
                Vector2 a = points[i];
                Vector2 b = points[(i + 1) % count];
                area += (a.x * b.y) - (b.x * a.y);
            }
        }

        //return Mathf.Abs(area * 0.5f);

        //*Mathf.Abs(scaleX * scaleY);

        float arvo = area * Mathf.Abs(scaleX * scaleY);
        return arvo;
    }



    /*
    public static int CountNonAlphaPixels(Sprite sprite)
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

                if (pixel.a > 0) // count only visible (non-transparent) pixels
                    count++;
            }
        }

        return count;
    }
    */


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
        Debug.Log("maara=" + maara);
        if (maara < pikseliminimimaara)
        {
            Destroy(gameObject);
            return;
        }
        int korkeus = GetSpriteHeightInPixels();
        if (korkeus < spritenkorkeudenmini)
        {
            Destroy(gameObject);
            return;
        }
    }



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

    /*
    // Update is called once per frame
    void Update()
    {
        
        laskuri += Time.deltaTime;

        if (laskuri >= muunnossykli)
        {
            int maara = CountNonAlphaPixels(GetComponent<SpriteRenderer>().sprite);
            Debug.Log("maaraupd=" + maara);

            if (gameObject.GetComponent<PolygonCollider2D>() != null)
            {

                //GeneratePreciseCollider();
                // GenerateCollider();

            }
            //RebuildCollider();

            //  laskuri = 0;
            //   TeeMainTekstuuriVaihto();
        }
    }
    */

    private IEnumerator RebuildColliderNextFrame()
    {
        yield return null; // Wait one frame to ensure Sprite is updated
        RebuildCollider();


    }


    private void RebuildCollider()
    {


        PolygonCollider2D[] oldCol = GetComponents<PolygonCollider2D>();
        foreach (PolygonCollider2D pp in oldCol)
        {
            Destroy(pp);
        }
        PolygonCollider2D collider = gameObject.AddComponent<PolygonCollider2D>();
        // GenerateBoxCollidersFast();





        TarkistaKoko();

        bool splittitehtiin = SplitDisconnectedRegions();
        //if (!splittitehtiin)
        //  GenerateCollider();

        // GenerateCollider();
        //  GeneratePreciseCollider();
        // 
    }


    

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (!bulletholemode)
        {
            return;
        }

        if (col.collider.tag.Contains("ammus"))
        {
            float currentTime = Time.time;

            // Rate limit
            if (currentTime - lastBulletHoleTime < bulletHoleCooldown)
                return;


            lastBulletHoleTime = currentTime;

            col.collider.enabled = false;
            //laskuri += Time.deltaTime;

            //  if (laskuri >= muunnossykli)
            //  {
            //      laskuri = 0;

            // Get world hit point
            Vector2 hitPoint = col.GetContact(0).point;

            //hitPoint = GetNearestWorldPoint(hitPoint, col.gameObject);
            /*
            hitPoint = GetNearestWorldPoint(hitPoint, col.gameObject, col.rigidbody.velocity

Vector2 bulletVelocity, float maxAngleDegrees = 60f, int maxRadius = 32)
                */

            List<Vector2> lista = GetWorldPointList(hitPoint, col.gameObject, bulletholelistankoko, col.relativeVelocity, bulletholestep);

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


                bool change = DrawBulletHole3(texX, texY, col.gameObject);
                if (change)
                {
                    break;
                }

            }
            //DrawBulletHole3(hitPoint, col.gameObject);


            //}
        }
    }

    private List<Vector2> GetWorldPointList(Vector2 startPoint, GameObject go, int listSize, Vector2 velocity, float stepDistance = 0.1f)
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


    


    private void TeeMainTekstuuriVaihto()
    {
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
                RebuildCollider();//MJM TODOO OTA POIS KOMMENTEISTA


            }

        }
    }
    /*

    public int gridResolutionX = 10; // how many cells horizontally
    public int gridResolutionY = 10; // how many cells vertically
    public byte alphaThresholdBox = 10; // treat pixels with alpha <= this as invisible



    public void GenerateBoxCollidersFast()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null)
        {
            Debug.LogWarning("No SpriteRenderer or Sprite found.");
            return;
        }

        // Clean up old colliders
        foreach (var box in GetComponentsInChildren<BoxCollider2D>())
        {
            Destroy(box.gameObject);
        }

        Texture2D tex = sr.sprite.texture;
        Rect rect = sr.sprite.textureRect;
        Vector2 pivot = sr.sprite.pivot;
        float ppu = sr.sprite.pixelsPerUnit;
        Color32[] pixels = tex.GetPixels32();
        int texWidth = tex.width;

        // Cache values
        float unitWidth = rect.width / ppu;
        float unitHeight = rect.height / ppu;
        float cellW = unitWidth / gridResolutionX;
        float cellH = unitHeight / gridResolutionY;

        int pixelW = Mathf.RoundToInt(rect.width);
        int pixelH = Mathf.RoundToInt(rect.height);

        // Use a single parent to hold all box colliders
        GameObject container = new GameObject("BoxColliderContainer");
        container.transform.SetParent(transform, false);

        for (int gx = 0; gx < gridResolutionX; gx++)
        {
            for (int gy = 0; gy < gridResolutionY; gy++)
            {
                int pxMin = Mathf.FloorToInt(rect.x + gx * pixelW / gridResolutionX);
                int pyMin = Mathf.FloorToInt(rect.y + gy * pixelH / gridResolutionY);
                int pxMax = Mathf.FloorToInt(rect.x + (gx + 1) * pixelW / gridResolutionX);
                int pyMax = Mathf.FloorToInt(rect.y + (gy + 1) * pixelH / gridResolutionY);

                bool found = false;
                int stride = texWidth;

                for (int y = pyMin; y < pyMax && !found; y++)
                {
                    int rowOffset = y * stride;
                    for (int x = pxMin; x < pxMax; x++)
                    {
                        int index = rowOffset + x;
                        if (index >= 0 && index < pixels.Length && pixels[index].a > alphaThresholdBox)
                        {
                            found = true;
                            break;
                        }
                    }
                }

                if (found)
                {
                    GameObject cellBox = new GameObject("AutoBox");
                    cellBox.transform.SetParent(container.transform, false);
                    cellBox.transform.localPosition = new Vector3(
                        gx * cellW - pivot.x / ppu + cellW / 2,
                        gy * cellH - pivot.y / ppu + cellH / 2,
                        0f
                    );
                    var box = cellBox.AddComponent<BoxCollider2D>();
                    box.size = new Vector2(cellW, cellH);
                }
            }
        }
    }

    */





    //  public int holeRadius = 8;



    float GetSpriteScreenWidth(GameObject go, Camera camera)
    {
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null || camera == null)
            return 0f;

        // Get sprite bounds in world space
        Bounds bounds = sr.bounds;

        Vector3 worldLeft = new Vector3(bounds.min.x, bounds.center.y, 0);
        Vector3 worldRight = new Vector3(bounds.max.x, bounds.center.y, 0);

        // Convert world points to screen space
        Vector3 screenLeft = camera.WorldToScreenPoint(worldLeft);
        Vector3 screenRight = camera.WorldToScreenPoint(worldRight);

        float screenWidth = Mathf.Abs(screenRight.x - screenLeft.x);
        return screenWidth;
    }


    public static Vector2 GetWorldPositionFromPixelIndex(int index, int textureWidth, SpriteRenderer spriteRenderer)
    {
        // Get texX and texY from 1D index
        int texX = index % textureWidth;
        int texY = index / textureWidth;

        Sprite sprite = spriteRenderer.sprite;
        Transform transform = spriteRenderer.transform;

        float pixelsPerUnit = sprite.pixelsPerUnit;
        Rect rect = sprite.rect;
        Vector2 pivot = sprite.pivot;

        // Convert texture space (pixels) to local space (units)
        float localX = (texX - rect.x) / pixelsPerUnit - pivot.x / pixelsPerUnit;
        float localY = (texY - rect.y) / pixelsPerUnit - pivot.y / pixelsPerUnit;

        Vector2 localPosition = new Vector2(localX, localY);

        // Convert local to world space
        return transform.TransformPoint(localPosition);
    }



    private float lastSmokeTime = 0f;

    //public float travelDistance = 140f;
    

    bool DrawBulletHole3(int centerX, int centerY, GameObject go)
    {

        float leveys = GetSpriteScreenWidth(go, Camera.main);
        //int holeRadius =(int)( leveys / 2.0f);
        int holeRadius = (int)(leveys * leveydenkertoma);

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

        Color32[] pixels = maintekstuuri.GetPixels32();
        int width = maintekstuuri.width;
        int height = maintekstuuri.height;

        bool changed = false;

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

                        // Only clear non-transparent pixels
                        if (c.a != 0 || c.r != 0 || c.g != 0 || c.b != 0)
                        {
                            pixels[index] = new Color32(0, 0, 0, 0); // transparent



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
                        }
                    }
                }
            }
        }

        if (changed)
        {
            maintekstuuri.SetPixels32(pixels);
            maintekstuuri.Apply();

            // If visual or physics change is needed:
            TeeMainTekstuuriVaihto(); // your own method
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

        Color[] pixels = originalTex.GetPixels();
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
            Debug.Log("No disconnected regions found.");
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
            /*
            // Create sprite
            Sprite regionSprite = Sprite.Create(
                regionTex,
                new Rect(0, 0, bounds.width, bounds.height),
                originalSprite.pivot / originalSprite.rect.size,
                ppu
            );
            */
            /*
            Sprite regionSprite = Sprite.Create(
             regionTex,
             new Rect(0, 0, bounds.width, bounds.height),
             originalSprite.pivot,
             ppu
         );
            */

            /*
            Vector2 normalizedPivot = new Vector2(
        pivotPx.x / originalSprite.rect.width,
        pivotPx.y / originalSprite.rect.height
    );
*/
            //Sprite regionSprite = Sprite.Create(regionTex, new Rect(0, 0, bounds.width, bounds.height), normalizedPivot, ppu);




            // Clone material if needed

            /*
            if (first)
            {

            }
           // else
           // {
               
                // Position correction
                Vector2 regionCenterPx = bounds.position + new Vector2(bounds.width, bounds.height) * 0.5f;
                Vector2 offset = (regionCenterPx - pivotPx) / ppu;
                newPart.transform.position = transform.position + (Vector3)offset;
            //}
            */



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

                vaihto.RebuildCollider();

                /*
                PolygonCollider2D p = newPart.GetComponent<PolygonCollider2D>();
                float alue = CalculatePolygonArea(p, newPart);
                Debug.Log("alue=" + alue);
                if (alue < polygoniminiarvo)
                {
                    //      Destroy(newPart);
                    //         continue;
                    // return;
                }
                */


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
            Destroy(gameObject);
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

        // Optional: Clear or replace specific components (e.g., remove original collider)
        /*
         * PolygonCollider2D oldCol = clone.GetComponent<PolygonCollider2D>();
        if (oldCol) DestroyImmediate(oldCol);
        */



        // Important: Clone shared materials so modifying them won't affect the original
        SpriteRenderer sr = clone.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.material = new Material(sr.material); // Deep clone material
        }

        return clone;
    }
    

}
