using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {


    }
    // Update is called once per frame
    void Update()
    {

    }


    public Vector3 palautaScreenpositioneissa(int positionumero)
    {

        if (aluksenpositiotCameraViewissa == null || positionumero >= aluksenpositiotCameraViewissa.Count)
        {
            return Vector3.zero; // Return a default value
        }

        return aluksenpositiotCameraViewissa[positionumero];
    }

    public bool onkoEroa(Vector3 worldPosition, int positionumeroJohonVerrataan, bool muutaworldpositionScreenpositionin)
    {
        if (aluksenpositiotCameraViewissa.Count < positionumeroJohonVerrataan + 1)
        {
            return false;
        }

        //Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        //screenPosition = new Vector3((int)screenPosition.x, (int)screenPosition.y, screenPosition.z);
        Vector3 screenPosition;
        if (muutaworldpositionScreenpositionin)
        {
            screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            screenPosition = new Vector3((int)screenPosition.x, (int)screenPosition.y, screenPosition.z);

        }
        else
        {
            screenPosition = worldPosition;
        }


        Vector3 listassa = aluksenpositiotCameraViewissa[positionumeroJohonVerrataan];

        return !Vector3.Equals(screenPosition, listassa);

    }


    public float palautaEro(Vector3 worldPosition, int positionumeroJohonVerrataan, bool muutaworldpositionScreenpositionin)
    {
        if (aluksenpositiotCameraViewissa.Count < positionumeroJohonVerrataan + 1)
        {
            return 0.0f;
        }

        Vector3 screenPosition;
        if (muutaworldpositionScreenpositionin)
        {
            screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            screenPosition = new Vector3((int)screenPosition.x, (int)screenPosition.y, screenPosition.z);

        }
        else
        {
            screenPosition = worldPosition;
        }


        Vector3 listassa = aluksenpositiotCameraViewissa[positionumeroJohonVerrataan];
        float erotus = screenPosition.x - listassa.x;
        return erotus;

    }
    /*
    public bool OnkoOkToimia(GameObject go)
    {
        SpriteRenderer spriterrend = GetSpriteRenderer(go);

        return spriterrend.isVisible;
    }
    */


    public bool onkoliikkunutVasemmalle(Vector3 worldPosition, int positionumeroJohonVerrataan, bool muutaworldpositionScreenpositionin, float vaadittavaero)
    {
        if (aluksenpositiotCameraViewissa.Count < positionumeroJohonVerrataan + 1)
        {
            return false;
        }

        Vector3 screenPosition;
        if (muutaworldpositionScreenpositionin)
        {
            screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            screenPosition = new Vector3((int)screenPosition.x, (int)screenPosition.y, screenPosition.z);

        }
        else
        {
            screenPosition = worldPosition;
        }


        Vector3 listassa = aluksenpositiotCameraViewissa[positionumeroJohonVerrataan];

        if (screenPosition.x < listassa.x)
        {
            float erotus = Mathf.Abs(screenPosition.x - listassa.x);
            if (erotus > vaadittavaero)
            {
                return true;
            }
        }
        return false;

    }
    public bool onkoliikkunutOikealle(Vector3 worldPosition, int positionumeroJohonVerrataan, bool muutaworldpositionScreenpositionin, float vaadittavaero)
    {
        if (aluksenpositiotCameraViewissa.Count < positionumeroJohonVerrataan + 1)
        {
            return false;
        }
        Vector3 screenPosition;
        if (muutaworldpositionScreenpositionin)
        {
            screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            screenPosition = new Vector3((int)screenPosition.x, (int)screenPosition.y, screenPosition.z);

        }
        else
        {
            screenPosition = worldPosition;
        }


        Vector3 listassa = aluksenpositiotCameraViewissa[positionumeroJohonVerrataan];

        if (screenPosition.x > listassa.x)
        {
            float erotus = Mathf.Abs(screenPosition.x - listassa.x);
            if (erotus > vaadittavaero)
            {
                return true;
            }
        }
        return false;

    }

    private List<Vector3> aluksenpositiotCameraViewissa = new List<Vector3>();
    public void tallennaSijaintiSailytaVainNkplViimeisintaVanha(int nkpl, bool muutaworldpositionScreenpositionin, bool tallennavainyksiloivatArvot)
    {

        Vector3 worldPosition = transform.position;

        // Convert the world position to screen position
        Vector3 screenPosition;
        if (muutaworldpositionScreenpositionin)
        {
            screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            screenPosition = new Vector3((int)screenPosition.x, (int)screenPosition.y, screenPosition.z);
        }
        else
        {
            screenPosition = worldPosition;
        }

        if (aluksenpositiotCameraViewissa.Count == 0)
        {
            // Vector3Int sijainti = new Vector3Int( (int)screenPosition.x, (int)screenPosition.y,(int)screenPosition.z );

            //Vector3 sijainti = new Vector3( (int)screenPosition.x, (int)screenPosition.y, screenPosition.z);
            aluksenpositiotCameraViewissa.Add(screenPosition);
        }
        else
        {
            Vector3 viimeisin = aluksenpositiotCameraViewissa[aluksenpositiotCameraViewissa.Count - 1];
            if (!Vector3.Equals(viimeisin, screenPosition) || !tallennavainyksiloivatArvot)

            {
                //viimeisin.x != screenPosition.x || viimeisin.y != screenPosition.y)
                //Debug.Log("viimeisin.x=" + viimeisin.x + "screenPosition.x=" + screenPosition.x+" viimeisin.y="+viimeisin.y+ " screenPosition.y="+ screenPosition.y);
                //Vector3 sijainti = new Vector3((int)screenPosition.x, (int)screenPosition.y, screenPosition.z);
                aluksenpositiotCameraViewissa.Add(screenPosition);

            }
        }
        //40,30 optio 3,20 optio2 ,10 optio1 ,0


        //0 optio3,10 optio2,20 optio 1,30
        if (aluksenpositiotCameraViewissa.Count > nkpl)
        {
            aluksenpositiotCameraViewissa.RemoveAt(0);
        }

    }


    public void TallennaSijaintiSailytaVainNkplViimeisinta(int maxCount, bool useScreenPosition, bool saveUniqueValuesOnly, GameObject go)
    {
        // Get the current world position of the GameObject
        Vector3 worldPosition = go.transform.position;

        // Convert to screen position if required
        Vector3 positionToSave = useScreenPosition
            ? Camera.main.WorldToScreenPoint(worldPosition)
            : worldPosition;

        // Round screen position to integers for consistency
        if (useScreenPosition)
        {
            positionToSave = new Vector3((int)positionToSave.x, (int)positionToSave.y, positionToSave.z);
        }

        // Add the position if:
        // 1. The list is empty.
        // 2. The new position is different from the last one (if saveUniqueValuesOnly is true).
        if (aluksenpositiotCameraViewissa.Count == 0)
        {
            aluksenpositiotCameraViewissa.Add(positionToSave);
        }
        else
        {
            Vector3 lastPosition = aluksenpositiotCameraViewissa[aluksenpositiotCameraViewissa.Count - 1];
            float threshold = 4.0f;
            // Check uniqueness if saveUniqueValuesOnly is true
            //  if (!saveUniqueValuesOnly || !Vector3.Equals(lastPosition, positionToSave))
            //  {
            //      aluksenpositiotCameraViewissa.Add(positionToSave);
            //  }

            float ero = Vector3.Distance(lastPosition, positionToSave);
            if (go != null)
            {
                //    go.GetComponent<TextMesh>().text = "ero=" + ero;
            }

            if (!saveUniqueValuesOnly)
            {
                aluksenpositiotCameraViewissa.Add(positionToSave);
            }
            else
            {
                //if (ero > threshold)
                //{

                //}

                if (!Vector3.Equals(lastPosition, positionToSave))
                {
                    aluksenpositiotCameraViewissa.Add(positionToSave);
                }

            }
            /* || ero > threshold 
            // Check uniqueness if saveUniqueValuesOnly is true
            if (!saveUniqueValuesOnly)
            {
                aluksenpositiotCameraViewissa.Add(positionToSave);
            }*/
        }

        // Maintain the list size to maxCount by removing the oldest entry if necessary
        if (aluksenpositiotCameraViewissa.Count > maxCount)
        {
            aluksenpositiotCameraViewissa.RemoveAt(0);
        }
    }


    /*
    Vector2 boxinsize = new Vector2(1, 1);
    Vector2 vyboxlocation = new Vector2(-1, 1);
    //
    Vector2 yboxlocation = new Vector2(0, 1);
    Vector2 oyboxlocation = new Vector2(1, 1);
    Vector2 okboxlocation = new Vector2(1, 0);
    Vector2 oaboxlocation = new Vector2(1, -1); 
    Vector2 aboxlocation = new Vector2(0, -1);
    Vector2 vaboxlocation = new Vector2(-1, -1);

    Vector2 vkboxlocation = new Vector2(-1, 0);


    List<Vector2> locatiot = new List<Vector2>();

    public bool onkoTagiaBoxissaJokaonVasenylos(string tagname, LayerMask layerMas)
    {


        return onkoTagiaBoxissa(tagname, boxinsize, vyboxlocation, layerMas);
    }

    public bool onkoTagiaBoxissaJokaonYlos(string tagname, LayerMask layerMas)
    {

        return onkoTagiaBoxissa(tagname, boxinsize, yboxlocation, layerMas);
    }
    public bool onkoTagiaBoxissaJokaonOikeaYlos(string tagname, LayerMask layerMas)
    {

        return onkoTagiaBoxissa(tagname, boxinsize, oyboxlocation, layerMas);
    }

    public bool onkoTagiaBoxissaJokaonOikeaKeski(string tagname, LayerMask layerMas)
    {

        return onkoTagiaBoxissa(tagname, boxinsize, okboxlocation, layerMas);
    }

    public bool onkoTagiaBoxissaJokaonOikeaAlas(string tagname, LayerMask layerMas)
    {
        Vector2 boxsize = new Vector2(1, 1);

        return onkoTagiaBoxissa(tagname, boxsize, oaboxlocation, layerMas);
    }

    public bool onkoTagiaBoxissaJokaonAlas(string tagname, LayerMask layerMas)
    {

        return onkoTagiaBoxissa(tagname, boxinsize, aboxlocation, layerMas);
    }

    public bool onkoTagiaBoxissaJokaonVasenAlas(string tagname, LayerMask layerMas)
    {
        return onkoTagiaBoxissa(tagname, boxinsize, vaboxlocation, layerMas);
    }

    public bool onkoTagiaBoxissaJokaonVasenKeski(string tagname, LayerMask layerMas)
    {
        return onkoTagiaBoxissa(tagname, boxinsize, vkboxlocation, layerMas);
    }
    */
    /*
    public bool onkoTagiaBoxissa(string tagname, Vector2 boxsize, Vector2 boxlocation, LayerMask layerMask)
    {

        Collider2D[] cs = Physics2D.OverlapBoxAll((Vector2)transform.position + boxlocation, boxsize, 0f, layerMask);

        if (cs != null && cs.Length > 0)
        {
            foreach (Collider2D c in cs)
            {
                if (c.gameObject == this.gameObject)
                {

                }
                else if (c.gameObject.tag == tagname)
                {
                    return true;
                }
            }
        }
        return false;

    }
    */

    public bool onkoTagiaBoxissa(string name, Vector2 boxsize, Vector2 boxlocation, LayerMask layerMask)
    {
        Vector2 uusi = (Vector2)transform.position + boxlocation;

        //foreach (string name in tagit)
        //{
        // Collider2D[] cs = Physics2D.OverlapBoxAll((Vector2)transform.position + boxlocation, boxsize, 0f, layerMask);
        Collider2D[] cs = Physics2D.OverlapBoxAll(uusi, boxsize, 0f);

        if (cs != null && cs.Length > 0)
        {
            foreach (Collider2D c in cs)
            {
                // Debug.Log("tagi=" + c.gameObject.tag);
                if (c.gameObject == this.gameObject)
                {

                }
                else if (c.gameObject.transform.parent == this.gameObject.transform)
                {

                }
                else if (c.gameObject.tag.Contains(name))
                {
                    return true;
                }
            }
        }


        //}
        return false;

    }




    /*
    void OnDrawGizmos()
    {
        locatiot.Clear();
        locatiot.Add(vyboxlocation);
        locatiot.Add(yboxlocation);
        locatiot.Add(oyboxlocation);
        locatiot.Add(okboxlocation);
        locatiot.Add(oaboxlocation);
        locatiot.Add(aboxlocation);
        locatiot.Add(vaboxlocation);
        locatiot.Add(vkboxlocation);


        


    // Set the color of the Gizmos
        Gizmos.color = Color.green;
        SpriteRenderer rb = GetComponent<SpriteRenderer>();
        Vector2 ccc = new Vector2(rb.bounds.size.x, rb.bounds.size.y);
        // Draw the box representing the overlap area
        foreach (Vector2 item in locatiot)
        {
            Gizmos.DrawWireCube((Vector2)transform.position + item, ccc);
        }


    }
    */

    private int luotujenbonuksienmaara = 0;


    public void TeeBonus(GameObject bonus, Vector2 v2, Vector2 boxsize)
    {
        TeeBonus(bonus, v2, boxsize, 1);
    }

    public void TeeBonus(GameObject bonus, Vector2 v2, Vector2 boxsize, int bonuksienmaara)
    {
        for (int i = 0; i < bonuksienmaara; i++)
        {
            TeeBonusReal(bonus, v2, boxsize, bonuksienmaara);
        }
    }


    private void TeeBonusReal(GameObject bonus, Vector2 v2, Vector2 boxsize, int bonuksienmaara)
    {
        if (luotujenbonuksienmaara >= bonuksienmaara)
        {
            return;
        }
        for (float y = 0.0f; y < 1.0f; y += 0.3f)
        {
            for (float x = 0.0f; x < 1.0f; x += 0.3f)
            {
                //         Vector3 v3 =
                //new Vector3(0.1f +
                //m_Rigidbody2D.position.x+x, m_Rigidbody2D.position.y, 0);

                v2 = new Vector2(x, y);




                bool onko = OnkoBonusJoTuossa(v2, boxsize);
                if (!onko)
                {
                    Vector3 v3 =
    new Vector3(
    transform.position.x + v2.x, transform.position.y + v2.y, 0);
                    Instantiate(bonus, v3, Quaternion.identity);
                    //  bonusjotehty = true;
                    luotujenbonuksienmaara++;

                    ///   Debug.Log("instantioidaan bonus");
                    return;
                }
                else
                {
                    //    Debug.Log("oli jo tuossa");
                }
            }
        }
    }

    private bool OnkoBonusJoTuossa(Vector2 parametri, Vector2 boxsize)
    {
        // Vector2 v2 = new Vector2(v3.x, v3.y);
        string[] tagit = new string[1];
        tagit[0] = "bonustag";

        // SpriteRenderer s =bonus.GetComponent<SpriteRenderer>();

        // Vector2 boxsize = new Vector2(s.size.x, s.size.y);
        //   Vector2 boxsize = new Vector2(10.0f, 10.0f);

        bool onko = onkoTagiaBoxissa(tagit, boxsize, parametri, LayerMask.GetMask("BonusLayer"));

        return onko;
    }


    /*

    void OnDrawGizmos()
    {

        // Set the color of the Gizmos
        Gizmos.color = Color.green;

        // Draw the box representing the overlap area
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)transform.position + v2, boxsize);
    }
    */


    public bool onkoTagiaBoxissa(string[] tagit, Vector2 boxsize, Vector2 boxlocation, LayerMask layerMask)
    {
        Vector2 uusi = (Vector2)transform.position + boxlocation;

        foreach (string name in tagit)
        {
            // Collider2D[] cs = Physics2D.OverlapBoxAll((Vector2)transform.position + boxlocation, boxsize, 0f, layerMask);
            Collider2D[] cs = Physics2D.OverlapBoxAll(uusi, boxsize, 0f);

            if (cs != null && cs.Length > 0)
            {
                foreach (Collider2D c in cs)
                {
                    // Debug.Log("tagi=" + c.gameObject.tag);
                    if (c.gameObject == this.gameObject)
                    {

                    }
                    else if (c.gameObject.tag.Contains(name))
                    {
                        //  bool onko = IsInView((Vector2)transform.position + boxlocation);
                        //  return onko;
                        float olemassaolevanpalleronx = c.gameObject.transform.position.x;
                        float olemassaolevanpallerony = c.gameObject.transform.position.y;

                        float erox = Mathf.Abs(olemassaolevanpalleronx - uusi.x);
                        float eroy = Mathf.Abs(olemassaolevanpallerony - uusi.y);

                        if (erox <= 0.3f && eroy <= 0.3f)
                        {
                            return true;//onko
                        }
                        else
                        {
                            //    Debug.Log("eri paikassa");
                        }
                        //return true;//
                    }
                }
            }


        }
        return false;

    }

    //  private bool bonusjotehty = false;

    /*
    public Vector2 palautaAmmuksellaVelocityVector(GameObject alus, float ampumisevoimakkuus)
    {
        float pysty = alus.transform.position.y - transform.position.y;
        float vaaka = alus.transform.position.x - transform.position.x;

        float suhdeluku = pysty / vaaka;

        float kokonaisvoima = ampumisevoimakkuus;

        float vaakavoima = kokonaisvoima * suhdeluku;
        float pystyvoima = kokonaisvoima - vaakavoima;


        //           float angle = Mathf.Atan2(alusSpriteRenderer.bounds.center.y - m_SpriteRenderer.bounds.center.y, alusSpriteRenderer.bounds.center.x - m_SpriteRenderer.bounds.center.x) *
        //Mathf.Rad2Deg;
        SpriteRenderer alusSpriteRenderer = alus.GetComponent<SpriteRenderer>();

        float alusy = alusSpriteRenderer.bounds.center.y;
        float alusx = alusSpriteRenderer.bounds.center.x;


        float ammusx = transform.position.x;
        float ammusy = transform.position.y;

        //ammusx = piipunboxit.bounds.center.x;

        Vector2 vv = new Vector2(alusx - ammusx,
           alusy - ammusy);

        vv.Normalize();
        vv.Scale(new Vector2(4.0f, 4.0f));
        return vv;
    }

    public Vector2 palautaAmmuksellaVelocityVector(GameObject alus, float ampumisevoimakkuus)
    {
        // Extract the positions of the shooter and target
        SpriteRenderer alusSpriteRenderer = alus.GetComponent<SpriteRenderer>();

        float alusy = alusSpriteRenderer.bounds.center.y; // Target center (y)
        float alusx = alusSpriteRenderer.bounds.center.x; // Target center (x)

        float ammusx = transform.position.x; // Shooter position (x)
        float ammusy = transform.position.y; // Shooter position (y)

        // Calculate direction vector to the target
        Vector2 direction = new Vector2(alusx - ammusx, alusy - ammusy);
        direction.Normalize(); // Normalize to unit vector

        // Scale the direction by the shooting force
        Vector2 velocityVector = direction * ampumisevoimakkuus;

        return velocityVector; // Return the velocity vector
    }
    */



    public Vector2 palautaAmmuksellaVelocityVector(GameObject alus, float ampumisevoimakkuus)
    {
        Kamera k = Camera.main.GetComponent<Kamera>();


        if (alus == null) return Vector2.zero; // Return zero vector if target is null



        // Current positions
        Vector3 shooterPosition = transform.position;
        Vector3 alusPosition = alus.transform.position;

        // Distance to target
        Vector3 directionToAlus = alusPosition - shooterPosition;

        // Bullet travel time (time = distance / speed)
        float distance = directionToAlus.magnitude;
        float travelTime = distance / ampumisevoimakkuus;

        // Predict future position of the target
        Vector3 futureAlusPosition = alusPosition + new Vector3(k.skrollimaara * travelTime, 0f, 0f);

        // Adjust direction to aim at the predicted position
        Vector3 adjustedDirection = (futureAlusPosition - shooterPosition).normalized;

        // Calculate the velocity vector for the bullet
        Vector2 velocityVector = new Vector2(adjustedDirection.x, adjustedDirection.y) * ampumisevoimakkuus;

        return velocityVector;

    }

    public bool OnkoVihollisenJaAluksenValillaTiilia()
    {
        return OnkoVihollisenJaAluksenValillaTiilia(Vector3.zero);
    }


    public bool OnkoVihollisenJaAluksenValillaTiilia(Vector3 shooterPositionpara)
    {
        Vector3 shooterPosition;
        if (shooterPositionpara != Vector3.zero)
        {
            shooterPosition = transform.position;
        }
        else
        {
            shooterPosition = shooterPositionpara;
        }
        //tarkistetaan osuuko seinaan
        //Vector3 shooterPosition = transform.position;
        //   Debug.Log("shooterPosition=" + shooterPosition);
        Vector3 alusPosition = PalautaAlus().transform.position;

        // Distance to target
        //Vector3 directionToAlus = alusPosition - shooterPosition;
        float distance = Vector3.Distance(shooterPosition, alusPosition);
        Vector3 direction = (alusPosition - shooterPosition).normalized;

        RaycastHit2D[] hitsit = Physics2D.RaycastAll(transform.position, direction, distance);
        if (hitsit != null & hitsit.Length > 0)
        {
            foreach (RaycastHit2D hit in hitsit)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.tag.Contains("tiili") || hit.collider.tag.Contains("laatikkovihollinen"))
                    {
                        Debug.Log("osutaan tiileen");
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool VoikoVihollinenAmpua(Vector3 shooterPositionpara)
    {
        return !OnkoVihollisenJaAluksenValillaTiilia(shooterPositionpara);
    }
    public bool VoikoVihollinenAmpua()
    {
        return !OnkoVihollisenJaAluksenValillaTiilia(Vector3.zero);
    }


    public Vector2 palautaAmmuksellaVelocityVector(GameObject alus, float ampumisevoimakkuus, GameObject objektijostasijaintilasketaan)
    {
        Kamera k = Camera.main.GetComponent<Kamera>();


        if (alus == null) return Vector2.zero; // Return zero vector if target is null

        // Current positions
        Vector3 shooterPosition = objektijostasijaintilasketaan.transform.position;
        Vector3 alusPosition = alus.transform.position;

        // Distance to target
        Vector3 directionToAlus = alusPosition - shooterPosition;

        // Bullet travel time (time = distance / speed)
        float distance = directionToAlus.magnitude;
        float travelTime = distance / ampumisevoimakkuus;

        // Predict future position of the target
        Vector3 futureAlusPosition = alusPosition + new Vector3(k.skrollimaara * travelTime, 0f, 0f);

        // Adjust direction to aim at the predicted position
        Vector3 adjustedDirection = (futureAlusPosition - shooterPosition).normalized;

        // Calculate the velocity vector for the bullet
        Vector2 velocityVector = new Vector2(adjustedDirection.x, adjustedDirection.y) * ampumisevoimakkuus;

        return velocityVector;

    }


    public Vector2 palautaAmmuksellaVelocityVector(GameObject alus, float ampumisevoimakkuus, Vector3 shooterPosition)
    {
        Kamera k = Camera.main.GetComponent<Kamera>();


        if (alus == null) return Vector2.zero; // Return zero vector if target is null

        // Current positions
        //   Vector3 shooterPosition = objektijostasijaintilasketaan.transform.position;
        Vector3 alusPosition = alus.transform.position;

        // Distance to target
        Vector3 directionToAlus = alusPosition - shooterPosition;

        // Bullet travel time (time = distance / speed)
        float distance = directionToAlus.magnitude;
        float travelTime = distance / ampumisevoimakkuus;

        // Predict future position of the target

        //EI TOIMI OK KUN KAMERAPOSITION ON 1.25

        Vector3 futureAlusPosition = alusPosition + new Vector3(k.skrollimaara * travelTime, 0f, 0f);

        // Adjust direction to aim at the predicted position
        Vector3 adjustedDirection = (futureAlusPosition - shooterPosition).normalized;

        // Calculate the velocity vector for the bullet
        Vector2 velocityVector = new Vector2(adjustedDirection.x, adjustedDirection.y) * ampumisevoimakkuus;

        return velocityVector;

    }

    private GameObject alustieto;

    public GameObject PalautaAlus()
    {
        if (alustieto != null)
        {
            return alustieto;
        }

        GameObject[] allObstacles = GameObject.FindGameObjectsWithTag("alustag");
        foreach (GameObject obstacles in allObstacles)
        {
            alustieto = obstacles;
            break;
        }
        return alustieto;

    }

    public void RajaytaSpriteUUsi(GameObject go, int rows, int columns, float explosionForce, float alivetime)
    {
        Sprite originalSprite = GetComponent<SpriteRenderer>().sprite;

        // Get the original sprite's texture
        Texture2D texture = originalSprite.texture;

        // Calculate slice dimensions
        int sliceWidth = texture.width / columns;
        int sliceHeight = texture.height / rows;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                // Create slice rectangle
                Rect sliceRect = new Rect(x * sliceWidth, y * sliceHeight, sliceWidth, sliceHeight);

                // Create sprite from the slice
                Sprite newSprite = Sprite.Create(
                    texture,
                    sliceRect,
                    new Vector2(0.5f, 0.5f), // Pivot
                    originalSprite.pixelsPerUnit
                );

                // Create GameObject for the slice
                GameObject sliceObject = new GameObject($"Slice_{x}_{y}");
                SpriteRenderer sr = sliceObject.AddComponent<SpriteRenderer>();
                sr.sprite = newSprite;

                // Add Rigidbody2D for physics
                Rigidbody2D pieceRigidbody = sliceObject.AddComponent<Rigidbody2D>();
                pieceRigidbody.gravityScale = 0.5f;

                // Optional: Add BoxCollider2D for physics interaction
                BoxCollider2D collider = sliceObject.AddComponent<BoxCollider2D>();
                collider.size = new Vector2(sliceWidth / originalSprite.pixelsPerUnit, sliceHeight / originalSprite.pixelsPerUnit);

                // Position the slice
                sliceObject.transform.position = new Vector3(
                    transform.position.x + x * sliceWidth / originalSprite.pixelsPerUnit,
                    transform.position.y + y * sliceHeight / originalSprite.pixelsPerUnit,
                    0
                );

                // Apply random force and torque
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                float randomForce = Random.Range(explosionForce * 0.5f, explosionForce);
                pieceRigidbody.AddForce(randomDirection * randomForce, ForceMode2D.Impulse);
                pieceRigidbody.AddTorque(Random.Range(-10f, 10f), ForceMode2D.Impulse);

                // Destroy slice after a given time
                Destroy(sliceObject, alivetime);
            }
        }

        // Optionally destroy the original GameObject
        Destroy(go);
    }

    public void RajaytaSprite(GameObject go, int rows, int columns, float explosionForce, float alivetime
       )
    {
        RajaytaSprite(go, rows, columns, explosionForce, alivetime, -1, false, 0, false, 0.0f, -0.2f, false, null);

    }

    int RoundUpToMultipleOfFour(int value)
    {
        return (value + 3) & ~3; // Rounds up to the nearest multiple of 4
    }
    public static Texture2D ScaleTextureaaa(Texture2D source, int newWidth, int newHeight)
    {
        // Create a new empty texture with the desired dimensions
        Texture2D scaledTexture = new Texture2D(newWidth, newHeight, source.format, false);

        // Scale each pixel of the new texture based on the original texture
        for (int y = 0; y < newHeight; y++)
        {
            for (int x = 0; x < newWidth; x++)
            {
                // Map the new texture's pixel to the original texture
                float u = (float)x / (newWidth - 1); // U-coordinate (0 to 1)
                float v = (float)y / (newHeight - 1); // V-coordinate (0 to 1)

                // Get the pixel color from the source texture
                Color pixelColor = source.GetPixelBilinear(u, v);

                // Set the pixel in the new texture
                scaledTexture.SetPixel(x, y, pixelColor);
            }
        }

        // Apply changes to the new texture
        scaledTexture.Apply();

        return scaledTexture;
    }


    // Cache for storing scaled textures
    public static Dictionary<string, Texture2D> cachedTextures = new Dictionary<string, Texture2D>();

    public static Texture2D ScaleTexture(Texture2D source, int newWidth, int newHeight)
    {
        if (source == null)
            throw new System.ArgumentNullException("Source texture cannot be null.");

        if (newWidth <= 0 || newHeight <= 0)
            throw new System.ArgumentException("Texture dimensions must be greater than 0.");

        // Create a unique key for the cache based on texture and dimensions
        string cacheKey = $"{source.GetInstanceID()}_{newWidth}x{newHeight}";

        // Check if the texture is already cached
        if (cachedTextures.TryGetValue(cacheKey, out Texture2D cachedTexture))
        {
            Debug.Log("Using cached texture.");
            return cachedTexture;
        }

        // Scale the texture if not in the cache
        Texture2D scaledTexture = new Texture2D(newWidth, newHeight, source.format, false);

        for (int y = 0; y < newHeight; y++)
        {
            for (int x = 0; x < newWidth; x++)
            {
                float u = (float)x / (newWidth - 1); // U-coordinate
                float v = (float)y / (newHeight - 1); // V-coordinate
                Color pixelColor = source.GetPixelBilinear(u, v);
                scaledTexture.SetPixel(x, y, pixelColor);
            }
        }

        scaledTexture.Apply();

        // Cache the scaled texture
        cachedTextures[cacheKey] = scaledTexture;

        Debug.Log("Scaled texture created and cached.");
        return scaledTexture;
    }



    public void RajaytaSprite(GameObject go, int rows, int columns, float explosionForce, float alivetime,
        float sirpalemass, bool teerigitbody)
    {
        RajaytaSprite(go, rows, columns, explosionForce, alivetime, sirpalemass, teerigitbody, 0.0f, false, 0.0f, -0.2f, false, null);
    }


    public void RajaytaSprite(GameObject go, int rows, int columns, float explosionForce, float alivetime,
    float sirpalemass, bool teeBoxcollider2d, float ysaato, bool skaalaatekstuuria, float gravityscale,
        float xsaato, bool addDestroyController, GameObject explosion
        )
    {
        SpriteRenderer s = GetComponent<SpriteRenderer>();

        if (rows <= 1)
        {
            rows = 2;
        }
        if (columns <= 1)
        {
            columns = 2;
        }



        Sprite originalSprite = s.sprite;
        bool xflippi = s.flipX;
        bool yflippi = s.flipY;

        float ymin = s.bounds.min.y;
        float ymax = s.bounds.max.y;

        float puolet = (ymax - ymin) / 2;




        // Get the original sprite's texture
        Texture2D texture = originalSprite.texture;


        Vector3 scale = go.transform.localScale;
        //sliceObject.transform.localScale = scale;

        if (skaalaatekstuuria && scale != Vector3.one)
        {
            //tää on hidas ja textuurin asetuksissa pitää olla read/write ja compressio pois jotta toimii
            int adjustedWidth = RoundUpToMultipleOfFour((int)(texture.width * scale.x));
            int adjustedHeight = RoundUpToMultipleOfFour((int)(texture.height * scale.y));


            texture = ScaleTexture(texture, adjustedWidth, adjustedHeight);
        }


        // Calculate the width and height of each slice
        float sliceWidth = texture.width / columns;

        float sliceHeight = texture.height / rows;



        //   SpriteRenderer srSpriteRenderer =
        //   go.GetComponent<SpriteRenderer>();
        //   sliceWidth= srSpriteRenderer.size.x / columns;
        //   sliceHeight = srSpriteRenderer.size.y / rows; 



        // Create a list to store the sliced sprites
        // List<Sprite> slicedSprites = new List<Sprite>();

        // Loop through each row and column to create slices
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                // Calculate the rectangle for the slice
                Rect sliceRect = new Rect(x * sliceWidth, y * sliceHeight, sliceWidth, sliceHeight);

                // Create a new sprite from the slice
                Vector2 pivot = originalSprite.pivot;
                Vector2 normalizedPivot = pivot / originalSprite.rect.size;
                Sprite newSprite = Sprite.Create(
                    texture,
                    sliceRect,
                    normalizedPivot,
                    // new Vector2(0.5f, 0.5f),
                    //new Vector2(0.0f, 0.0f),

                    originalSprite.pixelsPerUnit
                );
                if (teeBoxcollider2d)
                {
                    int maara = CountNonTransparentPixels(newSprite);
                    // Debug.Log("aaaaaaaaaaaaaei piks=" + maara);
                    if (maara < 300)
                    {
                        //    Debug.Log("ei piks=" + maara);
                        continue;
                    }


                }


                // Store the new sprite in the list
                //    slicedSprites.Add(newSprite);

                // Optionally, instantiate a GameObject with the new sprite in the scene
                /**/
                GameObject sliceObject = new GameObject($"Slice_{x}_{y}");
                sliceObject.layer = go.layer;

                SpriteRenderer sr = sliceObject.AddComponent<SpriteRenderer>();
                //sr.flipX = xflippi;
                //sr.flipY = yflippi;
                sr.sortingOrder = -10;
                sr.sprite = newSprite;

                //Debug.0lo0ff000ddddddddtagi=" + sliceObject.tag);


                //sliceObject.tag="vih"
                // Set the position of the slice in the scene (adjust if needed)
                /*   */
                Rigidbody2D pieceRigidbody =
                sliceObject.AddComponent<Rigidbody2D>();
                //pieceRigidbody.gravityScale = 0.5f;
                //pieceRigidbody.gravityScale = 0.5f;

                pieceRigidbody.gravityScale = gravityscale;

                pieceRigidbody.simulated = true;

                // pieceRigidbody.bodyType = RigidbodyType2D.Static;
                //          pieceRigidbody.simulated = false;

                if (sirpalemass != -1)
                {
                    pieceRigidbody.mass = sirpalemass;
                }

                if (teeBoxcollider2d)
                {

                    PolygonCollider2D p2 =
                    sliceObject.AddComponent<PolygonCollider2D>();
                    /*
                    bool onko = IsPolygonColliderSizeOverLimit(p2, 0.1f);
                  

                    if (!onko)
                    {
                        Destroy(sr);
                        Destroy(sliceObject);
                        continue;
                    }
                    */



                    /*
                    BoxCollider2D p=
                    sliceObject.AddComponent<BoxCollider2D>();
                    p.size = new Vector2(0.2f, 0.2f);
                    */
                    GameObject alus = PalautaAlus();
                    if (alus != null)
                    {
                        List<GameObject> laa = new List<GameObject>();
                        laa.Add(alus);
                        laa.Add(sliceObject);
                        IgnoreCollisions(laa);
                    }
                }

                /*        
                     sliceObject.transform.position = new Vector3(
            -0.2f +
            transform.position.x + x * sliceWidth / originalSprite.pixelsPerUnit,
                -0.2f +
transform.position.y +
y * sliceHeight / originalSprite.pixelsPerUnit, 0);

        */
                float korotus = (y * (sliceHeight / originalSprite.pixelsPerUnit));
                // Debug.Log("korotus =" + korotus);
                // Debug.Log("transform.position.y =" + transform.position.y);

                //3 on testi
                /*
                sliceObject.transform.position = new Vector3(
                    -0.2f +
                    transform.position.x  + x * sliceWidth / originalSprite.pixelsPerUnit,
                        +ysaato- puolet+
      transform.position.y + korotus , transform.position.z);
                */
                //oli -0.2f
                sliceObject.transform.position = new Vector3(
                   xsaato +
                    transform.position.x + x * sliceWidth / originalSprite.pixelsPerUnit,
                        +ysaato - puolet +
      transform.position.y + korotus, transform.position.z);

                //   sliceObject.transform.rotation.z=

                float sourceZRotation = go.transform.rotation.eulerAngles.z;

                // sliceObject.tag = "vihollinensirpaleexplode";
                // sliceObject.layer = go.layer;

                //sliceObject.transform.localScale = new Vector3(2.0f, 2.0f, 0f);


                /*
                float zrotaatio =
  go.transform.localRotation.z;
                sliceObject.transform.localRotation = Quaternion.Euler(0, 0, zrotaatio);
                */

                Vector3 targetRotation = sliceObject.transform.rotation.eulerAngles;
                targetRotation.z = sourceZRotation;
                sliceObject.transform.rotation = Quaternion.Euler(targetRotation);
                /*
                //MJM uusi
                Vector3 scale = go.transform.localScale;
                sliceObject.transform.localScale = scale;
                */


                /*
            sliceObject.transform.position = new Vector3(
                -0.2f +
                transform.position.x,
                    -0.1f +
  transform.position.y +
  y * sliceHeight / originalSprite.pixelsPerUnit, 0);
*/


                /*    
                Vector3 explosionCenter = new Vector3(transform.position.x, transform.position.y,0);
          float forceAmount = 10.0f;



          Vector2 explosionForce = (sliceObject.transform.position - explosionCenter).normalized * forceAmount;
          pieceRigidbody.AddForce(explosionForce, ForceMode2D.Impulse);
         */

                Vector3 randomDirection = Random.insideUnitSphere.normalized;

                // Apply force in that direction with random strength
                /*
                float randomForce = Random.Range(explosionForce * 0.5f, explosionForce);
                pieceRigidbody.AddForce(randomDirection * randomForce, ForceMode2D.Impulse);

                */
                //float randomForce = Random.Range(explosionForce * 0.5f, explosionForce);


                float randomForce = explosionForce;
                /*MJM TESTI*/
                pieceRigidbody.AddForce(randomDirection * randomForce, ForceMode2D.Impulse);


                // Optionally, add random torque for rotation

                pieceRigidbody.AddTorque(Random.Range(-10f, 10f), ForceMode2D.Impulse);


                //Destroy(sliceObject, 0.3f);

                // DestroyController
                //  DestroyController c=
                // sliceObject.AddComponent<DestroyController>();
                // c.alivetime = 4.0f;

                if (addDestroyController)
                {
                    DestroyController c =
                     sliceObject.AddComponent<DestroyController>();
                    c.SetAlivetime(alivetime);
                    c.SetExplosion(explosion);

                }
                else
                {
                    Destroy(sliceObject, alivetime);
                }


                //StartCoroutine(ChangeToStaticAfterDelay(1f, pieceRigidbody));


                //  Instantiate(sliceObject);

            }
        }


        //  Debug.Log("Slicing completed. Number of slices: " + slicedSprites.Count);

    }

    public static bool IsPolygonColliderSizeOverLimit(PolygonCollider2D polygonCollider, float limit)
    {
        Vector2 siz = GetPolygonColliderSize(polygonCollider);

        float koko = siz.x * siz.y;
        //  Debug.Log("koko=" + koko);
        return koko >= limit;


    }

    public static Vector2 GetPolygonColliderSize(PolygonCollider2D polygonCollider)
    {
        if (polygonCollider == null)
        {
            Debug.LogError("PolygonCollider2D is null. Please provide a valid collider.");
            return Vector2.zero;
        }

        Vector2[] points = polygonCollider.points;

        // Initialize min and max values
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;

        // Iterate through the points to calculate the bounding box
        foreach (Vector2 point in points)
        {
            // Transform local points to world points
            Vector2 worldPoint = polygonCollider.transform.TransformPoint(point);

            if (worldPoint.x < minX) minX = worldPoint.x;
            if (worldPoint.y < minY) minY = worldPoint.y;
            if (worldPoint.x > maxX) maxX = worldPoint.x;
            if (worldPoint.y > maxY) maxY = worldPoint.y;
        }

        // Calculate size
        float width = maxX - minX;
        float height = maxY - minY;

        return new Vector2(width, height);
    }

    IEnumerator ChangeToStaticAfterDelay(float delay, Rigidbody2D pieceRigidbody)
    {
        Debug.Log("ennen delay");
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);
        Debug.Log("eka loki Rigidbody bodyType changed to Static after delay");
        //        pieceRigidbody.bodyType = RigidbodyType2D.Static;

        // Change the Rigidbody bodyType to Static
        pieceRigidbody.bodyType = RigidbodyType2D.Static;

        pieceRigidbody.simulated = false;

        Debug.Log("Rigidbody bodyType changed to Static after delay");
    }


    public void IgnoreChildCollisionsvanhaaa(Transform parent)
    {

        Collider[] childColliders = parent.GetComponentsInChildren<Collider>();

        for (int i = 0; i < childColliders.Length; i++)
        {
            Physics.IgnoreCollision(childColliders[i], parent.gameObject.GetComponent<Collider>());
            for (int j = i + 1; j < childColliders.Length; j++)
            {
                Physics.IgnoreCollision(childColliders[i], childColliders[j]);

            }
        }
    }

    public void IgnoreChildCollisions(Transform parent)
    {
        if (parent == null) return;

        Collider parentCollider = parent.GetComponent<Collider>();
        Collider[] childColliders = parent.GetComponentsInChildren<Collider>();

        for (int i = 0; i < childColliders.Length; i++)
        {
            // Ignore collisions between the parent collider and child colliders
            if (parentCollider != null)
            {
                Physics.IgnoreCollision(childColliders[i], parentCollider);
            }

            // Ignore collisions between all child colliders
            for (int j = i + 1; j < childColliders.Length; j++)
            {
                Physics.IgnoreCollision(childColliders[i], childColliders[j]);
            }
        }
    }

    /// <summary>
    /// Counts the number of non-transparent pixels in a given sprite.
    /// </summary>
    /// <param name="sprite">The sprite to analyze.</param>
    /// <returns>The count of non-transparent pixels.</returns>
    public static int CountNonTransparentPixels(Sprite sprite)
    {
        if (sprite == null)
        {
            Debug.LogError("Sprite is null!");
            return 0;
        }

        // Get the texture and sprite rect
        Texture2D texture = sprite.texture;
        Rect spriteRect = sprite.rect;

        // Ensure the texture is readable
        if (!texture.isReadable)
        {
            Debug.LogError("The sprite's texture is not readable. Enable 'Read/Write' in the texture import settings.");
            return 0;
        }

        // Get the pixel data within the sprite's bounds
        Color[] pixels = texture.GetPixels(
            (int)spriteRect.x,
            (int)spriteRect.y,
            (int)spriteRect.width,
            (int)spriteRect.height
        );

        int count = 0;

        // Count non-transparent pixels
        foreach (Color pixel in pixels)
        {
            if (pixel.a > 0f) // Check if the pixel has any opacity
            {
                count++;
            }
        }

        return count;
    }

    /*

    public void IgnoreCollisionsBetweenParentAndChildsAndBetweenAllChilds(GameObject go)
    {
        //List<GameObject> childit = GetChildGameObjects(go.transform);

        IgnoreChildCollisions(go.transform);







    }
    public List<GameObject> GetChildGameObjects(Transform transform)
    {
        List<GameObject> childObjects = new List<GameObject>();

        // Iterate through all children of the current transform
        foreach (Transform child in transform)
        {
            childObjects.Add(child.gameObject); // Add the child GameObject to the list
        }

        return childObjects;
    }
    */

    public void IgnoreCollisionsEITOIMI(List<GameObject> lista)
    {
        if (lista == null || lista.Count == 0)
        {
            return;
        }
        foreach (GameObject go in lista)
        {
            Collider[] childColliders = go.GetComponents<Collider>();
            foreach (GameObject go2 in lista)
            {
                Collider[] childColliders2 = go2.GetComponents<Collider>();
                foreach (Collider c in childColliders)
                {
                    foreach (Collider c2 in childColliders2)
                    {
                        Physics.IgnoreCollision(c, c2);
                    }
                }
            }
        }
    }
    public void IgnoreCollisions(List<GameObject> lista)
    {
        if (lista == null || lista.Count == 0)
        {
            return;
        }

        // Iterate through all GameObjects in the list
        for (int i = 0; i < lista.Count; i++)
        {
            GameObject go1 = lista[i];
            Collider[] colliders1 = go1.GetComponentsInChildren<Collider>();

            for (int j = i + 1; j < lista.Count; j++) // Avoid duplicate comparisons
            {
                GameObject go2 = lista[j];
                Collider[] colliders2 = go2.GetComponentsInChildren<Collider>();

                // Ignore collisions between all colliders in go1 and go2
                foreach (Collider c1 in colliders1)
                {
                    foreach (Collider c2 in colliders2)
                    {
                        Physics.IgnoreCollision(c1, c2);
                    }
                }
            }
        }
    }



    /*
    public bool IsObjectLeftOfCamera(GameObject go, float ero)
    {

        Vector3 objectPosition = go.transform.position;
        Vector3 cameraPosition = Camera.main.transform.position;

        // Check if the object is to the left of the camera
        if (objectPosition.x < cameraPosition.x)
        {
            // Calculate the distance between the object and the camera
            float distance = Vector3.Distance(objectPosition, cameraPosition);

            // Check if the distance is approximately 1 unit
            bool onko= distance >= ero;
            if (onko)
            {
                Debug.Log("no on");
            }

            return onko;
        }
        return false;
    }
    */








    public bool OnkoYsuunnassaKamerassa(GameObject gameObject)
    {
        Camera camera = Camera.main; // Reference to the main camera
        GameObject obj = gameObject; // Reference to the object you want to check

        // Get the camera's orthographic size and aspect ratio
        float height = camera.orthographicSize * 2; // Full height of the camera view
        float width = height * camera.aspect; // Full width of the camera view

        // Get the camera's position
        Vector3 cameraPosition = camera.transform.position;

        // Calculate the bounds of the camera view in world space
        Vector3 topLeft = cameraPosition + new Vector3(-width / 2, height / 2, 0);
        Vector3 bottomRight = cameraPosition + new Vector3(width / 2, -height / 2, 0);

        // Get the object's Y position
        float objYPosition = obj.transform.position.y;

        // Check if the object's Y position is within the camera's bounds
        bool isVisible = objYPosition >= bottomRight.y && objYPosition <= topLeft.y;

        if (isVisible)
        {
            //  Debug.Log("The object is within the visible Y range of the camera.");
            return true;
        }
        else
        {
            //  Debug.Log("The object is outside the visible Y range of the camera.");
            return false;
        }
    }


    GameObject alamaksimi = null;

    /*
    private static int tarkistuskertojenmaara = 10;//suoritusyky syistä tarkistetaan vain joka 100 kerta

    private int tarkistuslaskuri = 0;

    public void TuhoaJosVaarassaPaikassa(GameObject go)
    {
        TuhoaJosVaarassaPaikassa(go, true, false);
    }

    public void TuhoaJosVaarassaPaikassaaaaaaaaaaaaaa(GameObject go, bool tuhoaJosLiianKorkeallaKameraanverrattuna)
    {
        TuhoaJosVaarassaPaikassa(go, tuhoaJosLiianKorkeallaKameraanverrattuna, false);
    }

    public void TuhoaJosVaarassaPaikassaaaaaaaaaaaaaaa(GameObject go, bool tuhoaJosLiianKorkeallaKameraanverrattuna, bool tuhoajosoikeallakameraanverrattuna)
    {
    }

    public void TuhoaJosVaarassaPaikassaErikois(GameObject go, bool tuhoaJosLiianKorkeallaKameraanverrattuna, bool tuhoajosoikeallakameraanverrattuna)
    {



        if (alamaksimi == null)
        {
            alamaksimi = GameObject.FindWithTag("alamaksiminmaarittavatag");
        }
        tarkistuslaskuri++;

        if (tarkistuslaskuri < tarkistuskertojenmaara)
        {
            return;
        }
        tarkistuslaskuri = 0;






        float y = go.transform.position.y;
        float ykamera = Camera.main.transform.position.y;
        float koko = Camera.main.orthographicSize;


        if (tuhoaJosLiianKorkeallaKameraanverrattuna && (y > ykamera + koko + 1.0f || y < ykamera - koko - 1.0f))
        {
            Destroy(go);
            return;
        }



 

        if (IsObjectLeftOfCamera(go, 10.0f))
        {
            //     Debug.Log("object left of camerea" + go);
            Destroy(go);
            return;
        }
        if (tuhoajosoikeallakameraanverrattuna)
        {
            if (IsObjectRightOfCamera(Camera.main, go.transform))
            {
                Destroy(go);
                return;
            }
        }




        float objectHeight = 0.0f;
        SpriteRenderer renderer = GetSpriteRenderer(go);

        if (renderer != null)
        {
            objectHeight = renderer.bounds.size.y; // Half height for top/bottom calculation
        }


        if (y + objectHeight < alamaksimi.transform.position.y)
        {
            Destroy(go);
        }
    }
    */
    private SpriteRenderer renderer = null;


    private SpriteRenderer GetSpriteRenderer(GameObject go)
    {
        if (renderer == null)
        {
            renderer = go.GetComponent<SpriteRenderer>();
        }
        return renderer;
    }

    public bool IsOffScreen()
    {
        // Convert the world position of the bullet to viewport space
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        // Check if the bullet is outside the camera's visible area
        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            return true;  // Bullet is off-screen
        }

        return false;  // Bullet is within the screen bounds
    }

    public void TuhoaJosEiKamerassa(GameObject go)
    {
        if (IsOffScreen())
        {
            Destroy(go);
        }
    }
    public bool OnkoAndroidi()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            // Debug.Log("android");
            return true;

        }
        else
        {
            //   Debug.Log("EI OLE android");
            return false;

        }
    }

    /*
    public void IgnoreChildCollisions(Transform parent)
    {

        Collider[] childColliders = parent.GetComponentsInChildren<Collider>();

        for (int i = 0; i < childColliders.Length; i++)
        {
            Physics.IgnoreCollision(childColliders[i], parent.gameObject.GetComponent<Collider>());
            for (int j = i + 1; j < childColliders.Length; j++)
            {
                Physics.IgnoreCollision(childColliders[i], childColliders[j]);

            }
        }
    }
    */

    public bool IsGameObjectVisible()
    {
        if (Camera.main == null) return false; // Ensure there is a main camera

        // Get the main camera
        Camera mainCamera = Camera.main;

        // Convert the target's position to viewport coordinates
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        // Check if the target is within the camera's viewport
        bool isVisible = viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
                         viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
                         viewportPosition.z > 0; // Ensure it's in front of the camera

        return isVisible;
    }
    AlusController ac = null;

    public AlusController PalautaAlusController()
    {
        if (ac == null)
        {
            ac = FindObjectOfType<AlusController>();
        }
        return ac;
    }
    public float PalautaAluksenMaksimiDamage()
    {
        return PalautaAlusController().maksimimaaradamageajokakestetaan;
    }

    public float GetSpriteScreenWidth(SpriteRenderer m_SpriteRenderer, Camera mainCamera)
    {
        // Get the bounds of the SpriteRenderer in world space
        Bounds spriteBounds = m_SpriteRenderer.bounds;

        // Convert the left and right edges of the bounds to screen space
        Vector3 leftEdgeScreen = mainCamera.WorldToScreenPoint(new Vector3(spriteBounds.min.x, spriteBounds.center.y, 0));
        Vector3 rightEdgeScreen = mainCamera.WorldToScreenPoint(new Vector3(spriteBounds.max.x, spriteBounds.center.y, 0));

        // Calculate the width in screen space
        float screenWidth = rightEdgeScreen.x - leftEdgeScreen.x;

        return screenWidth;
    }

    public float GetSpriteScreenHeight(SpriteRenderer m_SpriteRenderer, Camera mainCamera)
    {
        // Get the bounds of the SpriteRenderer in world space
        Bounds spriteBounds = m_SpriteRenderer.bounds;

        // Convert the left and right edges of the bounds to screen space
        Vector3 leftEdgeScreen = mainCamera.WorldToScreenPoint(new Vector3(spriteBounds.min.x, spriteBounds.center.y, 0));
        Vector3 rightEdgeScreen = mainCamera.WorldToScreenPoint(new Vector3(spriteBounds.max.x, spriteBounds.center.y, 0));

        // Calculate the width in screen space
        //  float screenWidth = rightEdgeScreen.x - leftEdgeScreen.x;

        float screenHeight = rightEdgeScreen.y - leftEdgeScreen.y;

        // return screenHeight;
        return leftEdgeScreen.y;
    }

    public (float, float) GetFromAllColliders()
    {
        float top = 0;
        float bottom = 0;
        Collider2D[] ccs = GetComponents<Collider2D>();
        foreach (Collider2D c in ccs)
        {
            Bounds bounds = c.bounds;
            float maxy = bounds.max.y;
            float miny = bounds.min.y;

            if (maxy > top)
            {
                top = maxy;
            }
            if (miny < bottom)
            {
                bottom = miny;
            }


        }
        //   top = top * 0.7f;
        //   bottom = bottom * 0.7f;
        top = top * 0.5f;
        bottom = bottom * 0.5f;
        return (top, bottom);
    }
    (float, float) GetSpriteRendererMinMaxY()
    {
        SpriteRenderer sp = GetComponent<SpriteRenderer>();

        if (sp != null)
        {
            Bounds bounds = sp.bounds;

            // Extract min and max Y values
            float minY = bounds.min.y;
            float maxY = bounds.max.y;

            return (minY, maxY);
        }
        else
        {
            return (0, 0);
        }
    }


    public bool OnkoVasemmallaVaistettavaa(int rayCount, float rayDistance, LayerMask collisionLayer)
    {
        (float top, float bottom) = GetFromAllColliders();

        // (float top, float bottom) = GetSpriteRendererMinMaxY();
        top += 0.5f;
        bottom -= 0.5f;

        float ero = top - bottom;

        float step = ero / rayCount;

        for (float i = bottom; i < top; i += step)
        {
            // Calculate the position of the ray along the vertical axis
            float t = (float)i / (rayCount - 1); // Normalized position between 0 and 1
            //Vector2 rayOrigin = new Vector2(transform.position.x, Mathf.Lerp(top, bottom, t));
            //Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y+i);
            //Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y+i);

            //Vector2 rayOrigin = new Vector2(transform.position.x, i);

            Vector2 rayOrigin = new Vector2(transform.position.x + 2, i);




            // Cast the ray
            RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Vector2.left, rayDistance, collisionLayer);
            foreach (RaycastHit2D hit in hits)
            {
                // Visualize the ray in the Scene view
                Debug.DrawRay(rayOrigin, Vector2.left * rayDistance, Color.red);
                // Check if a relevant obstacle was detected
                if (hit.collider != null && hit.collider.gameObject != gameObject &&
                    (hit.collider.tag.Contains("tiili") || hit.collider.tag.Contains("vihollinen")))
                {
                    //   Debug.Log("Obstacle detected at: " + hit.collider.name);

                    return true;
                }
            }
        }
        return false;

    }

    float CalculateMoveUpDistance(Camera camera, Vector3 objectPosition)
    {
        // Get the camera's top Y bound in world space
        float camHeight = camera.orthographicSize * 2f; // Total height of the camera in world space
        float camTop = camera.transform.position.y + camHeight / 2f;

        // Calculate the distance to move the object up so it's outside the camera's view
        float distanceToMoveUp = (camTop - objectPosition.y); // Adding a small buffer (e.g., 0.1) to ensure it's outside

        return distanceToMoveUp;
    }


    float CalculateMoveDownDistance(Camera camera, Vector3 objectPosition)
    {
        // Get the camera's bottom Y bound in world space
        float camHeight = camera.orthographicSize * 2f; // Total height of the camera in world space
        float camBottom = camera.transform.position.y - camHeight / 2f;

        // Calculate the distance to move the object down so it's outside the camera's view
        float distanceToMoveDown = (objectPosition.y - camBottom); // Adding a small buffer (e.g., 0.1) to ensure it's outside

        return distanceToMoveDown;
    }
    public bool OlisikoylhaallaVaistotilaa(int rayCount, float rayDistance, LayerMask collisionLayer)
    {
        float yloskulunmaksi = CalculateMoveUpDistance(Camera.main, transform.position);
        (float top, float bottom) = GetFromAllColliders();
        top = top + yloskulunmaksi;
        float ero = top - bottom;

        float step = ero / rayCount;

        for (float i = bottom; i < top; i += step)
        {
            // Calculate the position of the ray along the vertical axis
            float t = (float)i / (rayCount - 1); // Normalized position between 0 and 1
            //Vector2 rayOrigin = new Vector2(transform.position.x, Mathf.Lerp(top, top+ yloskulunmaksi, t));
            Vector2 rayOrigin = new Vector2(transform.position.x, i);
            // Cast the ray
            RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Vector2.left, rayDistance, collisionLayer);
            bool onkoosumaa = false;
            foreach (RaycastHit2D hit in hits)
            {
                // Visualize the ray in the Scene view
                Debug.DrawRay(rayOrigin, Vector2.left * rayDistance, Color.green);
                // Check if a relevant obstacle was detected
                if (hit.collider != null && hit.collider.gameObject != gameObject &&
                    (hit.collider.tag.Contains("tiili") || hit.collider.tag.Contains("vihollinen")))
                {
                    // Debug.Log("Obstacle detected at: " + hit.collider.name);

                    onkoosumaa = true;
                    break;
                }
            }
            if (!onkoosumaa)
            {
                return true;
            }
        }
        return false;

    }

    public bool OlisikoalhaallaVaistotilaa(int rayCount, float rayDistance, LayerMask collisionLayer)
    {
        float yloskulunmaksi = CalculateMoveDownDistance(Camera.main, transform.position);
        (float top, float bottom) = GetFromAllColliders();
        bottom = bottom - yloskulunmaksi;
        float ero = top - bottom;

        float step = ero / rayCount;

        for (float i = bottom; i < top; i += step)
        {
            // Calculate the position of the ray along the vertical axis
            float t = (float)i / (rayCount - 1); // Normalized position between 0 and 1
            //Vector2 rayOrigin = new Vector2(transform.position.x, Mathf.Lerp(top, top+ yloskulunmaksi, t));
            Vector2 rayOrigin = new Vector2(transform.position.x, i);
            // Cast the ray
            RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Vector2.left, rayDistance, collisionLayer);
            bool onkoosumaa = false;
            foreach (RaycastHit2D hit in hits)
            {
                // Visualize the ray in the Scene view
                Debug.DrawRay(rayOrigin, Vector2.left * rayDistance, Color.green);
                // Check if a relevant obstacle was detected
                if (hit.collider != null && hit.collider.gameObject != gameObject &&
                    (hit.collider.tag.Contains("tiili") || hit.collider.tag.Contains("vihollinen")))
                {
                    //   Debug.Log("Obstacle detected at: " + hit.collider.name);

                    onkoosumaa = true;
                    break;
                }
            }
            if (!onkoosumaa)
            {
                return true;
            }
        }
        return false;

    }
    public Vector3 GetCameraMaxWorldPosition()
    {
        // Calculate the camera's dimensions in world space
        float height = Camera.main.orthographicSize * 2;
        float width = height * Camera.main.aspect;

        // Top-right corner of the camera's view in world space
        Vector3 maxWorldPosition = Camera.main.transform.position + new Vector3(width / 2, height / 2, 0);

        return maxWorldPosition;
    }
    public Vector3 GetCameraMinWorldPosition()
    {
        // Calculate the camera's dimensions in world space
        float height = Camera.main.orthographicSize * 2;
        float width = height * Camera.main.aspect;

        // Bottom-left corner of the camera's view in world space
        Vector3 minWorldPosition = Camera.main.transform.position - new Vector3(width / 2, height / 2, 0);

        return minWorldPosition;
    }

    public void VaistaAlas(float delta, float nopeusy)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - nopeusy * delta, 0);

    }

    public void VaistaPystysuunnassa(float delta, float nopeusy)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + nopeusy * delta, 0);

    }

    private int rayCount = 5;
    private float rayDistance = 5f; // Distance of the raycast

    private float vaistonopeus = 3.0f;




    public void Vaista(float delta, LayerMask collisionLayer)
    {
        bool vasen = OnkoVasemmallaVaistettavaa(rayCount, rayDistance, collisionLayer);
        Debug.Log("vasen=" + vasen);

        if (vasen)
        {
            bool ylos =
                OlisikoylhaallaVaistotilaa(rayCount, rayDistance, collisionLayer);

            //Debug.Log("ylos=" + ylos);
            if (ylos)
            {
                VaistaPystysuunnassa(delta, vaistonopeus);

            }
            else
            {
                bool alas = OlisikoalhaallaVaistotilaa(rayCount, rayDistance, collisionLayer);
                //  Debug.Log("alas=" + alas);
                if (alas)
                {

                    VaistaPystysuunnassa(delta, -vaistonopeus);
                }
                else
                {
                    //  transform.position = new Vector3(transform.position.x + 2.1f, transform.position.y, 0);
                    // Rigidbody2D v = GetComponent<Rigidbody2D>();
                    // v.gravityScale = -1.0f;

                    transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, 0);
                }
            }
        }

    }

    public Vector3 GetOhjausCameraMinWorldPosition(Camera sormikamera)
    {

        // Calculate the camera's dimensions in world space
        float height = sormikamera.orthographicSize * 2;
        float width = height * sormikamera.aspect;

        // Bottom-left corner of the camera's view in world space
        Vector3 minWorldPosition = sormikamera.transform.position - new Vector3(width / 2, height / 2, 0);

        return minWorldPosition;
    }


    public Vector3 GetOhjausCameraMaxWorldPosition(Camera sormikamera)
    {

        // Calculate the camera's dimensions in world space
        float height = sormikamera.orthographicSize * 2;
        float width = height * sormikamera.aspect;

        // Bottom-left corner of the camera's view in world space
        Vector3 minWorldPosition = sormikamera.transform.position + new Vector3(width / 2, height / 2, 0);

        return minWorldPosition;
    }

    public void TuhoaMuttaAlaTuhoaJosOllaanEditorissa(GameObject go, float nopeusjonkaalletuhotaan)
    {


        TuhoaReal(go, true, nopeusjonkaalletuhotaan, true, 5.0f, true, 5.0f, true, 5.0f,
true,
600,
1, false
);

    }

    public void TuhoaMuttaAlaTuhoaJosOllaanEditorissaTuhoaJosOikeallapuolen(GameObject go, float nopeusjonkaalle)
    {


        TuhoaReal(go, true, nopeusjonkaalle, true, 5.0f, true, 5.0f, true, 5.0f,
true,
600,
0.1f, true
);

    }




    public void TuhoaMuttaAlaTuhoaJosOllaanEditorissa(GameObject go)
    {
        /**
#if !UNITY_EDITOR
    Debug.Log("This code runs only in builds, not in the Unity Editor.");
                TuhoaReal(go,false,-1,true,5.0f,true,5.0f,true,5.0f,
    true,
    600,
    1,false
    );
#endif

        **/
        float alaoffsetti = 1.0f;

       // Debug.Log("This code runs only in builds, not in the Unity Editor.");
        TuhoaReal(go, false, -1, true, 5.0f, true, alaoffsetti, true, 5.0f,
true,
600,
1, false
);

    }



    //sitten
    public bool OnkoOkToimiaUusi(GameObject go)
    {
        if (go==null)
        {
            Debug.Log("OnkoOnOkToimiaUusi kutsuttu nullilla");
            return false;
        }
        //jos on näkyvillä
        //entäs muulloin
        SpriteRenderer s = go.GetComponent<SpriteRenderer>();

        if (s!=null && s.isVisible)
        {
            return true;
        }
        return false;

    }

    private bool OnkoKameranVasemmallaPuolella(GameObject go, float offset)
    {
        Vector3 kameraminimi = GetCameraMinWorldPosition();

        if (go.transform.position.x < kameraminimi.x)
        {
            float ero = Mathf.Abs(kameraminimi.x - go.transform.position.x);
            if (ero >= offset)
            {
                return true;
            }

        }
        return false;

    }

    private bool OnkoKameranOikeallaPuolella(GameObject go)
    {
        Vector3 kameraminimi = GetCameraMaxWorldPosition();

        if (go.transform.position.x > kameraminimi.x)
        {
            float ero = Mathf.Abs(kameraminimi.x - go.transform.position.x);
            if (ero >= 0.1f)
            {
                return true;
            }

        }
        return false;

    }


    private bool OnkoKameranAlaPuolella(GameObject go, float offset)
    {
        Vector3 kameraminimi = GetCameraMinWorldPosition();

        if (go.transform.position.y < kameraminimi.y)
        {
            float ero = Mathf.Abs(kameraminimi.y - go.transform.position.y);
            if (ero >= offset)
            {
                return true;
            }

        }
        return false;

    }

    private bool OnkoKameranYlaPuolella(GameObject go, float offset)
    {
        Vector3 kameramax = GetCameraMaxWorldPosition();

        if (go.transform.position.y > kameramax.y)
        {
            float ero = Mathf.Abs(kameramax.y - go.transform.position.y);
            if (ero >= offset)
            {
                return true;
            }

        }
        return false;

    }

    private float syklilaskuri = 0.0f;
    // private float tarkistussykli = 5.0f;
    private float hengissaoloaika = 0.0f;
    private void TuhoaReal(GameObject go, bool tuhoajosliianhidas, float nopeusJonkaAlletuhotaan,
        bool tuhoaJosKameranVasemmallaPuolella,
        float offsettijokasallitaanvasemmallakavaisyyn,
        bool tuhoaJosKameranAlapuolella,
        float offsettijokasallitaanAlhaallaKavaisyyn,
        bool tuhoaJosKameranYlapuolella,
        float offsettijokasallitaanYlapuolellaKavaisyyn,
        bool tuhoajosliiankauanhengissa,
        float hengissaolonraja,
        float tarkistussykli,
        bool tuhoajosoikeallakameraanVerrattuna
        )
    {

        syklilaskuri += Time.deltaTime;
        if (syklilaskuri >= tarkistussykli)
        {

            syklilaskuri = 0.0f;
            hengissaoloaika += tarkistussykli;

            if (tuhoajosliiankauanhengissa && hengissaoloaika >= hengissaolonraja)
            {
                Destroy(go);
                return;
            }
            if (tuhoajosliianhidas)
            {
                Rigidbody2D r = go.GetComponent<Rigidbody2D>();
                if (r != null)
                {
                    float speed = r.velocity.magnitude;
                    if (speed <= nopeusJonkaAlletuhotaan)
                    {
                        Destroy(go);
                        return;
                    }
                }
            }
            if (tuhoaJosKameranVasemmallaPuolella)
            {
                bool vasen =
                OnkoKameranVasemmallaPuolella(go, offsettijokasallitaanvasemmallakavaisyyn);
                if (vasen)
                {
                    Destroy(go);
                    return;

                }

            }
            if (tuhoajosoikeallakameraanVerrattuna)
            {
                if (OnkoKameranOikeallaPuolella(go))
                {
                    Destroy(go);
                    return;
                }
            }
            if (tuhoaJosKameranAlapuolella)
            {
                if (OnkoKameranAlaPuolella(go, offsettijokasallitaanAlhaallaKavaisyyn))
                {
                    Destroy(go);
                    return;

                }
            }
            if (tuhoaJosKameranYlapuolella)
            {
                if (OnkoKameranYlaPuolella(go, offsettijokasallitaanYlapuolellaKavaisyyn))
                {
                    Destroy(go);
                    return;

                }
            }


        }
    }



    //private Texture2D image; // The source image
    public int uusirajaytyscolumns = 10;  // Number of horizontal slices
    public int uusirajaytysrows = 10;     // Number of vertical slices
    //private Vector2 startPosition; // The original position of the SpriteRenderer

    public void RajaytaUudellaTavalla()
    {
        Vector2 startPosition;
        Texture2D image;
        SpriteRenderer originalRenderer = GetComponent<SpriteRenderer>();
        if (originalRenderer == null || originalRenderer.sprite == null)
        {
            Debug.LogError("No SpriteRenderer or Sprite found!");
            return;
        }

        image = originalRenderer.sprite.texture;
        originalRenderer.enabled = false; // Hide the original image

        startPosition = transform.position;

        // Ensure pixel-perfect rendering
        image.filterMode = FilterMode.Point;

        // Get the original sprite's size in world units
        Vector2 spriteWorldSize = originalRenderer.bounds.size;

        // Get individual slice sizes in world units
        float sliceWidth = spriteWorldSize.x / uusirajaytyscolumns;
        float sliceHeight = spriteWorldSize.y / uusirajaytysrows;

        // Get individual slice sizes in pixels
        int pixelWidth = image.width / uusirajaytyscolumns;
        int pixelHeight = image.height / uusirajaytysrows;

        // Loop through rows and columns
        for (int y = 0; y < uusirajaytysrows; y++)
        {
            for (int x = 0; x < uusirajaytyscolumns; x++)
            {
                // Fix texture bleeding by expanding the rect slightly to ensure there is no gap between slices
                //int extraPixel = 1;  // No need for extra pixel expansion, this can be adjusted if needed
                int extraPixel = 1;  // No need for extra pixel expansion, this can be adjusted if needed

                int invertedY = (uusirajaytysrows - 1 - y) * pixelHeight;

                Rect rect = new Rect(
                    Mathf.Max(0, x * pixelWidth - extraPixel),  // Starting X position
                    Mathf.Max(0, invertedY - extraPixel),        // Starting Y position (inverted for the image coordinates)
                    Mathf.Min(pixelWidth + extraPixel * 2, image.width - x * pixelWidth),  // Ensure width fits within bounds
                    Mathf.Min(pixelHeight + extraPixel * 2, image.height - invertedY)    // Ensure height fits within bounds
                );

                if (!HasNonTransparentPixels(rect,image))
                {
                    continue; // Skip this slice if it's completely transparent
                }

                // Create a sprite for the slice
                //                Sprite sprite = Sprite.Create(image, rect, new Vector2(0.5f, 0.5f), image.width / spriteWorldSize.x);

                Texture2D sliceTexture = new Texture2D(Mathf.FloorToInt(rect.width), Mathf.FloorToInt(rect.height));

                // Get the pixels from the original texture
                Color[] slicePixels = image.GetPixels(Mathf.FloorToInt(rect.x), Mathf.FloorToInt(rect.y), Mathf.FloorToInt(rect.width), Mathf.FloorToInt(rect.height));
                sliceTexture.SetPixels(slicePixels);

                // Apply changes to the new texture
                sliceTexture.Apply();

                // Create a new sprite from the new texture
                Sprite sliceSprite = Sprite.Create(sliceTexture, new Rect(0, 0, sliceTexture.width, sliceTexture.height), new Vector2(0.5f, 0.5f), image.width / spriteWorldSize.x);


                // Create a new GameObject for this slice
                GameObject newObject = new GameObject($"Piece_{y}_{x}");

                newObject.tag = gameObject.tag;

                newObject.layer = LayerMask.NameToLayer("Default");
                SpriteRenderer renderer = newObject.AddComponent<SpriteRenderer>();
                renderer.sprite = sliceSprite;

                // Add BoxCollider2D to the new object
                BoxCollider2D collider = newObject.AddComponent<BoxCollider2D>();
                collider.size = new Vector2(sliceWidth, sliceHeight);

                // Calculate local position for perfect alignment
                float posX = startPosition.x - (spriteWorldSize.x / 2) + (x * sliceWidth) + (sliceWidth / 2);
                float posY = startPosition.y + (spriteWorldSize.y / 2) - (y * sliceHeight) - (sliceHeight / 2);

                // Set position to exactly match original image
                newObject.transform.position = new Vector2(posX, posY);

                // Optionally, add additional logic or components as needed
                newObject.AddComponent<SliceController>();
            }
        }
    }

    // Helper method to check if there are any non-transparent pixels in the slice
    private bool HasNonTransparentPixels(Rect rect, Texture2D image)
    {
        // Get the pixels in the specified area
        Color[] pixels = image.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);

        // Check each pixel to see if it's non-transparent
        foreach (var pixel in pixels)
        {
            if (pixel.a > 0) // If alpha is greater than 0, the pixel is not fully transparent
            {
                return true;
            }
        }

        // If all pixels are transparent, return false
        return false;
    }





}