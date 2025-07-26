using UnityEngine;

public class TextuuriController : BaseController
{
    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;
    private int _MainTex = Shader.PropertyToID("_MainTex");
    private int _NoiseTex = Shader.PropertyToID("_NoiseTex");
    private int _Rotation = Shader.PropertyToID("_RotationArvo");
    private int _Tiling = Shader.PropertyToID("_TilingArvo");


    private int _TilingOffset = Shader.PropertyToID("_TilingOffsetArvo");



    //public Texture2D maini;
    public Texture2D overlayTexture;

    public Texture2D[] overlayTextures;

    public Material konttimateriaali;

    [Range(-180, 180)]
    public int rotationGraffiti = 0;
    //eli asetetaan joku koko ja sitten vaan randomisoidaan sitä
    [Range(0.1f, 20f)]
    public float kokox = 1.0f;
    [Range(0.1f, 20f)]
    public float kokoy = 1.0f;

    public float randomisointiprosentti = 40f;


    public float tilinoffsetx = 0.0f;
    public float tilinoffsety = 0.0f;

    //  private bool materiaalivaihtotehty = false;
    private void teeMateriaaliVaihto()
    {
        _spriteRenderers = GetComponentsInParent<SpriteRenderer>();
        _materials = new Material[_spriteRenderers.Length];
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            // Clone materials to avoid shared material side effects
            _spriteRenderers[i].material = new Material( konttimateriaali);
            _materials[i] = _spriteRenderers[i].material;
            _materials[i].SetTexture(_NoiseTex, originellitekstuuri);
            _materials[i].SetFloat(_Rotation, rotationGraffiti);
          //  _materials[i].SetVector(_Tiling, tilingvalue);
            
            //_materials[i].SetVector(_TilingOffset, tilingoffsetvalue);

      //      _materials[i].SetTextureScale(_NoiseTex, new Vector2(tilingX, tilingY));


        }
    }


    private void AsetaArvot()
    {
       // float GenerateRandomFromPosition(float alaraja, float ylaraja)

        float randomFactorX = GenerateRandomFromPosition(
        1f - randomisointiprosentti / 100f,
        1f + randomisointiprosentti / 100f
    );

        float randomFactorY = GenerateRandomFromPosition(
            1f - randomisointiprosentti / 100f,
            1f + randomisointiprosentti / 100f
        );

        float scaleX = kokox * randomFactorX;
        float scaleY = kokoy * randomFactorY;


        float tilinoffsetxr = kokox * tilinoffsetx;
        float tilinoffsetyr = kokoy * tilinoffsety;


        for (int i = 0; i < _materials.Length; i++)
        {
            Texture2D te2 = GetTexture();
            _materials[i].SetTexture(_NoiseTex, te2);
            _materials[i].SetVector(_Tiling, new Vector2(scaleX, scaleY));

            float angleInRadians = rotationGraffiti * Mathf.Deg2Rad;

            _materials[i].SetFloat(_Rotation, angleInRadians);
            _materials[i].SetVector(_TilingOffset, new Vector2(tilinoffsetxr, tilinoffsetyr));

        }
    }

    void Start()
    {
        _spriteRenderers = GetComponentsInParent<SpriteRenderer>();
        _materials = new Material[_spriteRenderers.Length];

        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            // Clone materials to avoid shared material side effects
            _spriteRenderers[i].material = new Material(konttimateriaali);
            _materials[i] = _spriteRenderers[i].material;
        }



        //GetComponent<SpriteRenderer>().sprite = overlayTexture;
        originellitekstuuri = GetComponent<SpriteRenderer>().sprite.texture;
        //bool graffikaytossa = GraffitiKaytossa();
        // teeMateriaaliVaihto();
        AsetaArvot();
        Muunto();


    }
    private Texture2D originellitekstuuri;
    /*
    public byte r = 255;
    public byte g = 255;
    public byte b = 255;
    public byte aa = 255;
    */
    /*
    //nämä kertoo sen muunnoksen
    public float offsetxInPercentsOffMainTextureWidth = 0;
    public float offsetyInPercentsOffMainTextureHeight = 0;
    public float percentOfOverlaytexturescale = 0;

    public float randomisointiMuutosprosenttiXYPositioneissa = 0;
    public float scaleRandomiprosentti = 0.0f;

    //public float varinArvonRandomiArvo = 0.0f;

    public float prosenttiOsuusMikaVoiMaksimissaanOllaGraffitinKokoSuhteessaKonttiin = 30.0f;
    */

    public float prosenttijollaylipaansalistaanGraffiti = 50.0f;


    public float muunnossykli = 1.0f;
    private float laskuri = 0;
    public bool muunnaUpdatessa = true;

    private bool GraffitiKaytossa()
    {
        float arpa = GenerateRandomFromPosition(0, 100);

        if (arpa >= 100 - prosenttijollaylipaansalistaanGraffiti)
        {
            return true;
        }
        return false;
    }
    void Update()


    {
        if (!OnkoOkToimiaUusi(gameObject))
        {
            return;
        }


        if (muunnaUpdatessa)
        {

            laskuri += Time.deltaTime;

            if (laskuri >= muunnossykli)
            {
                laskuri = 0;
                // Remove this block unless you want real-time overlay recomputing
                Muunto();
            }

        }
    }

    float GenerateRandomFromPosition(float alaraja, float ylaraja)
    {
        if (muunnaUpdatessa)
        {
            return Random.Range(alaraja, ylaraja);
        }
        else
        {
            Vector2 position = transform.position;

            // Muunnetaan koordinaatit deterministiseksi siemeneksi
            int seed = Mathf.FloorToInt(position.x * 73856093) ^ Mathf.FloorToInt(position.y * 19349663);
            System.Random rng = new System.Random(seed);

            // Arvo välillä 0.0 – 1.0
            float luku = (float)rng.NextDouble();

            // Skaalataan arvo haluttuun väliin
            return Mathf.Lerp(alaraja, ylaraja, luku);
        }

    }
    private Texture2D GetTexture()
    {
        if (overlayTexture!=null)
        {
            return overlayTexture;
        }
        else if (overlayTextures!=null && overlayTextures.Length>0)
        {
            float arpa= GenerateRandomFromPosition(0, overlayTextures.Length-1);
            return overlayTextures[(int)arpa];
        }
        else
        {
            return null;
        }
    }

    private void Muunto()
    {

        bool tehdaanko = GraffitiKaytossa();
        if (!tehdaanko)
        {
            return;
        }
        //tähän randomit
        AsetaArvot();

        /*
        if (!materiaalivaihtotehty)
        {
            teeMateriaaliVaihto(true);
        }
        */


        /*
float variarpa = GenerateRandomFromPosition(-varinArvonRandomiArvo, varinArvonRandomiArvo);
//-5 ja 5



int uusivariarvoR = Mathf.Clamp(r + (int)variarpa, 0, 255);

int uusivariarvoG = Mathf.Clamp(g + (int)variarpa, 0, 255);

int uusivariarvoB = Mathf.Clamp(b + (int)variarpa, 0, 255);

int uusivariarvoA = Mathf.Clamp(aa + (int)variarpa, 0, 255);

Color32 whiteTint = new Color32((byte)uusivariarvoR, (byte)uusivariarvoG, (byte)uusivariarvoB, (byte)uusivariarvoA);

*/

        /*

        Texture2D te = GetTexture();
        if (te!=null)
        {
            //r on siis 255
            //sitä pitäisi siis randomisoida
            //0-10% 10% on siis se 

            //offsetxInPercentsOffMainTextureWidth  5 % offsetti esim.
            //randomisoidaan tuota prosenttia eli esim. 10%
            //4.5 -5.5

            float uusioffsetxInPercentsOffMainTextureWidth=
            GenerateRandomFromPosition(offsetxInPercentsOffMainTextureWidth-randomisointiMuutosprosenttiXYPositioneissa/2, offsetxInPercentsOffMainTextureWidth+randomisointiMuutosprosenttiXYPositioneissa/2);

            float uusioffsetYInPercentsOffMainTextureWidth =
      GenerateRandomFromPosition(offsetyInPercentsOffMainTextureHeight - randomisointiMuutosprosenttiXYPositioneissa / 2, offsetyInPercentsOffMainTextureHeight + randomisointiMuutosprosenttiXYPositioneissa / 2);


            float scalenrandomi =
                GenerateRandomFromPosition(-scaleRandomiprosentti, scaleRandomiprosentti);

            float uusiscale = percentOfOverlaytexturescale + scalenrandomi;

            Texture2D a = te;


            
        }
        */
    }

    /*

    public Texture2D OverlayTexture(
    Texture2D mainTexture,
    Texture2D overlayTexture,
    Color32 overlayTint,
    float offsetxInPercentsOffMainTextureWidth,
    float offsetyInPercentsOffMainTextureHeight,
    float percentOfOverlaytexturescale)
    {
        int mainWidth = mainTexture.width;
        int mainHeight = mainTexture.height;

        float scale = 1f + (percentOfOverlaytexturescale / 100f); // alkuperäinen skaala

        int overlayWidth = Mathf.RoundToInt(overlayTexture.width * scale);
        int overlayHeight = Mathf.RoundToInt(overlayTexture.height * scale);

        float maxProsentti = prosenttiOsuusMikaVoiMaksimissaanOllaGraffitinKokoSuhteessaKonttiin / 100.0f;

        // Säilytetään overlayn alkuperäinen kuvasuhde
        float aspectRatio = (float)overlayTexture.width / overlayTexture.height;

        if ((float)overlayWidth / mainWidth > maxProsentti || (float)overlayHeight / mainHeight > maxProsentti)
        {
            // Aseta uusi leveys ja korkeus maksimien mukaan
            overlayWidth = Mathf.RoundToInt(mainWidth * maxProsentti);
            overlayHeight = Mathf.RoundToInt(overlayWidth / aspectRatio);

            // Tarkista vielä korkeusraja
            if ((float)overlayHeight / mainHeight > maxProsentti)
            {
                overlayHeight = Mathf.RoundToInt(mainHeight * maxProsentti);
                overlayWidth = Mathf.RoundToInt(overlayHeight * aspectRatio);
            }

            // Päivitä scale vastaamaan rajoitettua overlay-kokoa
            scale = (float)overlayWidth / overlayTexture.width;
        }

        float offix = offsetxInPercentsOffMainTextureWidth / 100;
        float offiy = offsetyInPercentsOffMainTextureHeight / 100;

        int offsetX = Mathf.RoundToInt(offix * mainWidth) - overlayWidth / 2;
        int offsetY = Mathf.RoundToInt(offiy * mainHeight) - overlayHeight / 2;

        Texture2D outTexture = new Texture2D(mainWidth, mainHeight, TextureFormat.RGBA32, false);
        outTexture.SetPixels32(mainTexture.GetPixels32());
        Color32[] basePixels = outTexture.GetPixels32();

        Color32[] originalOverlayPixels = overlayTexture.GetPixels32();

        for (int y = 0; y < overlayHeight; y++)
        {
            for (int x = 0; x < overlayWidth; x++)
            {
                int srcX = Mathf.Clamp(Mathf.FloorToInt(x / scale), 0, overlayTexture.width - 1);
                int srcY = Mathf.Clamp(Mathf.FloorToInt(y / scale), 0, overlayTexture.height - 1);
                int overlayIndex = srcY * overlayTexture.width + srcX;

                int targetX = x + offsetX;
                int targetY = y + offsetY;

                if (targetX < 0 || targetX >= mainWidth || targetY < 0 || targetY >= mainHeight)
                    continue;

                int baseIndex = targetY * mainWidth + targetX;

                Color32 o = originalOverlayPixels[overlayIndex];
                o.r = (byte)(o.r * overlayTint.r / 255);
                o.g = (byte)(o.g * overlayTint.g / 255);
                o.b = (byte)(o.b * overlayTint.b / 255);
                o.a = (byte)(o.a * overlayTint.a / 255);

                float alpha = o.a / 255f;
                basePixels[baseIndex] = Color32.Lerp(basePixels[baseIndex], o, alpha);
            }
        }

        outTexture.SetPixels32(basePixels);
        outTexture.Apply(false);
        return outTexture;
    }
    */
}
