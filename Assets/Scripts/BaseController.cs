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


    public void TallennaSijaintiSailytaVainNkplViimeisinta(int maxCount, bool useScreenPosition, bool saveUniqueValuesOnly,GameObject go)
    {
        // Get the current world position of the GameObject
        Vector3 worldPosition = transform.position;

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
            if (go!=null)
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
        Kamera k=Camera.main.GetComponent<Kamera>();
     

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

    public Vector2 palautaAmmuksellaVelocityVector(GameObject alus, float ampumisevoimakkuus,GameObject objektijostasijaintilasketaan)
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
        Vector3 futureAlusPosition = alusPosition + new Vector3(k.skrollimaara * travelTime, 0f, 0f);

        // Adjust direction to aim at the predicted position
        Vector3 adjustedDirection = (futureAlusPosition - shooterPosition).normalized;

        // Calculate the velocity vector for the bullet
        Vector2 velocityVector = new Vector2(adjustedDirection.x, adjustedDirection.y) * ampumisevoimakkuus;

        return velocityVector;

    }



    public GameObject PalautaAlus()
    {
        GameObject[] allObstacles = GameObject.FindGameObjectsWithTag("alustag");
        foreach (GameObject obstacles in allObstacles)
        {
            GameObject alus = obstacles;
            return alus;
        }
        return null;

    }

    public void RajaytaSprite(GameObject go, int rows, int columns, float explosionForce, float alivetime)
    {

        Sprite originalSprite = GetComponent<SpriteRenderer>().sprite;

        // Get the original sprite's texture
        Texture2D texture = originalSprite.texture;

        // Calculate the width and height of each slice
        int sliceWidth = texture.width / columns;
        int sliceHeight = texture.height / rows;

        // Create a list to store the sliced sprites
        List<Sprite> slicedSprites = new List<Sprite>();

        // Loop through each row and column to create slices
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                // Calculate the rectangle for the slice
                Rect sliceRect = new Rect(x * sliceWidth, y * sliceHeight, sliceWidth, sliceHeight);

                // Create a new sprite from the slice
                Sprite newSprite = Sprite.Create(
                    texture,
                    sliceRect,
                    new Vector2(0.5f, 0.5f),
                    //new Vector2(0.0f, 0.0f),

                    originalSprite.pixelsPerUnit
                );

                // Store the new sprite in the list
                slicedSprites.Add(newSprite);

                // Optionally, instantiate a GameObject with the new sprite in the scene
                /**/
                GameObject sliceObject = new GameObject($"Slice_{x}_{y}");
                SpriteRenderer sr = sliceObject.AddComponent<SpriteRenderer>();
                sr.sprite = newSprite;
                //Debug.0lo0ff000ddddddddtagi=" + sliceObject.tag);


                //sliceObject.tag="vih"
                // Set the position of the slice in the scene (adjust if needed)
                /*   */
                Rigidbody2D pieceRigidbody =
                sliceObject.AddComponent<Rigidbody2D>();
                pieceRigidbody.gravityScale = 0.5f;
                pieceRigidbody.simulated = true;

                /*
                BoxCollider2D p =
                sliceObject.AddComponent<BoxCollider2D>();
                p.size = new Vector2(0.1f, 0.1f);
                                
                             sliceObject.transform.position = new Vector3(
                    -0.2f +
                    transform.position.x + x * sliceWidth / originalSprite.pixelsPerUnit,
                        -0.2f +
      transform.position.y +
      y * sliceHeight / originalSprite.pixelsPerUnit, 0);

                */

                sliceObject.transform.position = new Vector3(
                    -0.2f +
                    transform.position.x + x * sliceWidth / originalSprite.pixelsPerUnit,
                        -0.1f +
      transform.position.y +
      y * sliceHeight / originalSprite.pixelsPerUnit, 0);

                /*    
                Vector3 explosionCenter = new Vector3(transform.position.x, transform.position.y,0);
          float forceAmount = 10.0f;



          Vector2 explosionForce = (sliceObject.transform.position - explosionCenter).normalized * forceAmount;
          pieceRigidbody.AddForce(explosionForce, ForceMode2D.Impulse);
         */

                Vector3 randomDirection = Random.insideUnitSphere.normalized;

                // Apply force in that direction with random strength
                float randomForce = Random.Range(explosionForce * 0.5f, explosionForce);
                pieceRigidbody.AddForce(randomDirection * randomForce, ForceMode2D.Impulse);

                // Optionally, add random torque for rotation

                pieceRigidbody.AddTorque(Random.Range(-10f, 10f), ForceMode2D.Impulse);

                Destroy(sliceObject, 0.3f);
                //  Instantiate(sliceObject);

            }
        }


        //  Debug.Log("Slicing completed. Number of slices: " + slicedSprites.Count);

    }
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


    bool IsObjectRightOfCamera(Camera cam, Transform objTransform)
    {
        if (objTransform == null)
        {
            return false;
        }
        // Convert object's world position to viewport coordinates
        Vector3 viewportPoint = cam.WorldToViewportPoint(objTransform.position);

        // Check if the object is on the right side of the camera and not visible
        // x > 1 means the object is beyond the right edge of the camera's view
        // y should be between 0 and 1 (within the vertical bounds of the camera)
        // z > 0 means the object is in front of the camera, not behind
        return viewportPoint.x > 1 && viewportPoint.z > 0;
    }

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
            return distance >= ero;
        }
        return false;
    }

    public bool IsObjectLeftOfCamera(GameObject go)
    {

        return IsObjectLeftOfCamera(go, 1.0f);
    }


    public bool IsObjectDownOfCamera(GameObject go)
    {
        Camera camera = Camera.main; // or reference your specific camera
        float distance = 1f;

        float height = 2 * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad) * distance;
        float width = height * camera.aspect;

        Debug.Log($"Camera Size (Perspective) - Width: {width}, Height: {height}");


        Vector3 objectPosition = go.transform.position;
        Vector3 cameraPosition = Camera.main.transform.position;

        // onko alla eli y pienempi

        //y on suurempi ylh‰‰ll‰
        //0-220
        //y=-0.2
        //z=-1
        if (objectPosition.y < cameraPosition.y)
        {
            // Calculate the distance between the object and the camera
            float distance2 = Vector3.Distance(objectPosition, cameraPosition);

            // Check if the distance is approximately 1 unit
            return distance2 >= 10.0f;
        }
        return false;
    }

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

    public void TuhoaJosVaarassaPaikassa(GameObject go)
    {

        if (alamaksimi==null)
        {
            alamaksimi = GameObject.FindWithTag("alamaksiminmaarittavatag");
        }
       

        float y = go.transform.position.y;
        float ykamera = Camera.main.transform.position.y;
        float koko = Camera.main.orthographicSize;
        

        if (y>ykamera+koko+1.0f || y<ykamera-koko-1.0f)
        {
            Destroy(go);
            return;
        }



        /*
       if (!OnkoYsuunnassaKamerassa(go))
       {
           Destroy(go);
           return;
       }

  
       */

        if (IsObjectLeftOfCamera(go, 50.0f))
        {
       //     Debug.Log("object left of camerea" + go);
            Destroy(go);
            return;
        }
        float objectHeight = 0.0f;
        SpriteRenderer renderer = go.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            objectHeight = renderer.bounds.size.y; // Half height for top/bottom calculation
        }


        if (y + objectHeight < alamaksimi.transform.position.y )
        {
            Destroy(go);
        }
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



}