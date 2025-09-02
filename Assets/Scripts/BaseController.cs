using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class BaseController : MonoBehaviour, ReturnToPoolAble
{
    // Start is called before the first frame update

    /*
    private GameObject prefap;
    public GameObject GetPrefap()
    {
        return prefap;
    }

    public void SetPreFap(GameObject g)
    {
        prefap = g;
    }
    */
    

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

        //Vector3 screenPosition = mainCam.WorldToScreenPoint(worldPosition);
        //screenPosition = new Vector3((int)screenPosition.x, (int)screenPosition.y, screenPosition.z);
        Vector3 screenPosition;
        if (muutaworldpositionScreenpositionin)
        {
            screenPosition = mainCam.WorldToScreenPoint(worldPosition);
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
            screenPosition = mainCam.WorldToScreenPoint(worldPosition);
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
            screenPosition = mainCam.WorldToScreenPoint(worldPosition);
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
            screenPosition = mainCam.WorldToScreenPoint(worldPosition);
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
            screenPosition = mainCam.WorldToScreenPoint(worldPosition);
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
            ? mainCam.WorldToScreenPoint(worldPosition)
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

    public bool onkoTagiaBoxissaTransformPositionArvoonLisataanBoxLocation(string name, Vector2 boxsize, Vector2 boxlocation, LayerMask layerMask)
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



    public bool onkoTagiaBoxissaAlakaytaTransformia(string name, Vector2 boxsize, Vector2 boxlocation)
    {
        Vector2 uusi = boxlocation;

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


    public void TeeBonus(GameObject bonus, Vector2 boxsize)
    {
        TeeBonus(bonus, boxsize, 1);
    }

    public void TeeBonus(GameObject bonus, Vector2 boxsize, int bonuksienmaara)
    {
        for (int i = 0; i < bonuksienmaara; i++)
        {
            TeeBonusReal(bonus, boxsize, bonuksienmaara, false);
        }
    }


    public void TeeBonusReal(GameObject bonus, Vector2 boxsize, int bonuksienmaara, bool ignoraaAlkulaskuri)
    {
        if (!ignoraaAlkulaskuri && luotujenbonuksienmaara >= bonuksienmaara)
        {
            return;
        }
        for (float y = 0.0f; y < 1.5f; y += 0.5f)
        {
            for (float x = 0.0f; x < 1.5f; x += 0.5f)
            {
                //         Vector3 v3 =
                //new Vector3(0.1f +
                //m_Rigidbody2D.position.x+x, m_Rigidbody2D.position.y, 0);

                Vector2 v2 = new Vector2(x, y);




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
        Kamera k = mainCam.GetComponent<Kamera>();


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

    float GetGameObjectRadius(GameObject obj)
    {
        Vector3 scaleaaa = obj.transform.lossyScale;
        if (Mathf.Abs(scaleaaa.x - scaleaaa.y) > 0.001f)
        {
            Debug.LogWarning("Non-uniform scale detected on " + obj.name);
        }

        CircleCollider2D circle = obj.GetComponent<CircleCollider2D>();
        if (circle != null)
        {
            float scale = Mathf.Max(obj.transform.lossyScale.x, obj.transform.lossyScale.y);
            //            float radius = circle.radius * scale;
            float radius = circle.radius;

            return radius;
        }

        /*
        // Check for CircleCollider2D
        CircleCollider2D circle = obj.GetComponent<CircleCollider2D>();
        if (circle != null)
        {
            return circle.radius * obj.transform.lossyScale.x; // Assumes uniform scaling
        }
        */
        // Check for BoxCollider2D (approximate as circle using largest side)
        BoxCollider2D box = obj.GetComponent<BoxCollider2D>();
        if (box != null)
        {
            Vector3 scale = obj.transform.lossyScale;
            float width = box.size.x * scale.x;
            float height = box.size.y * scale.y;
            return Mathf.Max(width, height) * 0.5f;
        }

        // Check for SpriteRenderer bounds (fallback)
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Bounds bounds = sr.bounds;
            float diameter = Mathf.Max(bounds.size.x, bounds.size.y);
            return diameter * 0.5f;
        }

        // If no size-related components found
        //   Debug.LogWarning("No known radius source found on " + obj.name);
        return 0.0f;
    }


    public bool OnkoPisteenJaAluksenValillaTilltaTaiVihollista(Vector2 shooterPosition, GameObject go)
    {
        GameObject alus = PalautaAlus();

        Vector2 alusPosition = PalautaGameObjektinKeskipiste(alus);

        // Distance to target
        //Vector3 directionToAlus = alusPosition - shooterPosition;
        float distance = Vector2.Distance(shooterPosition, alusPosition);
        distance -= 0.2f;
        Vector2 direction = (alusPosition - shooterPosition).normalized;
        float radius = GetGameObjectRadius(go);
        // radius *= 1.04f;//laitetaas säteeseen 5% lisää
        //radius = 1.0f;
        RaycastHit2D[] hitsit;

        if (radius > 0.0f)
        {
            hitsit = Physics2D.CircleCastAll(shooterPosition, radius, direction, distance);

            //Debug.color = Color.red;
            Debug.DrawLine(shooterPosition, shooterPosition + direction * distance, Color.green);

            // Draw start and end circles
            DrawCircle(shooterPosition, radius, Color.green);
            DrawCircle(shooterPosition + direction * distance, radius, Color.yellow);

        }
        else
        {

            Debug.DrawLine(shooterPosition, shooterPosition + direction * distance, Color.green);
            hitsit = Physics2D.RaycastAll(shooterPosition, direction, distance);
        }

        if (hitsit != null & hitsit.Length > 0)
        {
            foreach (RaycastHit2D hit in hitsit)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.tag.Contains("tiili") || hit.collider.tag.Contains("laatikkovihollinen"))
                    {
                        return true;
                    }
                    else if (hit.collider.tag.Contains("vihollinen"))
                    {
                        //  Debug.Log("osutaan johonkin "+ hit.collider.tag);
                        if (!OnkoOma(hit.collider.gameObject))
                        {
                            return true;
                        }

                    }
                }
            }
        }

        return false;
    }


    void DrawCircle(Vector2 center, float radius, Color color, int segments = 32)
    {
        // Gizmos.color = color;
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + Vector2.right * radius;
        for (int i = 1; i <= segments; i++)
        {
            float rad = Mathf.Deg2Rad * angleStep * i;
            Vector3 newPoint = center + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
            Debug.DrawLine(prevPoint, newPoint, color);
            prevPoint = newPoint;
        }
    }

    GameObject FindRootWithTagContaining(GameObject go, string tagSubstring)
    {
        Transform current = go.transform;

        GameObject match = null;

        while (current != null)
        {
            if (current.gameObject.tag.Contains(tagSubstring))
            {
                match = current.gameObject; // Save current as potential match
            }

            current = current.parent;
        }
        if (match != null)
            return match;
        else
            return go;
    }

    private bool OnkoOma(GameObject go)
    {
        if (go == this.gameObject)
            return true;
        Transform t = FindRootWithTagContaining(go, "vihollinen").transform;

        foreach (Transform child in t)
        {
            if (IsDescendant(child, go))
                return true;
        }


        foreach (Transform child in this.transform)
        {
            if (IsDescendant(child, go))
                return true;
        }



        return false;
    }

    private bool IsDescendant(Transform parent, GameObject target)
    {
        if (parent.gameObject == target)
            return true;

        foreach (Transform child in parent)
        {
            if (IsDescendant(child, target))
                return true;
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
        Kamera k = mainCam.GetComponent<Kamera>();


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
        Kamera k = mainCam.GetComponent<Kamera>();

        ampumisevoimakkuus = ampumisevoimakkuus * GameManager.Instance.PalautaDifficulty();

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
        Vector2 velocityVector;
        if (directionToAlus.x > 0)
        {
            //ammutaan aluksen vasemmalla puolella
            float uusiampumis = GetKorjattuArvoPerustuenSkrollimaaraanTamaSiksiEttaAmmusLentaaNopeamminVasemmaltaOikealleSilloinKunKameraLiikkuuNopeammin
                (ampumisevoimakkuus);
            velocityVector = new Vector2(adjustedDirection.x, adjustedDirection.y) * uusiampumis;
        }
        else
        {
            velocityVector = new Vector2(adjustedDirection.x, adjustedDirection.y) * ampumisevoimakkuus;

        }
        return velocityVector;

    }

    public Vector2 PalautaGameObjektinKeskipiste(GameObject go)
    {
        Collider2D[] allColliders = go.GetComponents<Collider2D>();

        if (allColliders.Length == 0)
        {
            //Debug.Log("No 2D colliders found in the scene." + go);
            return go.transform.position;
        }

        // Sum of all collider positions
        Vector2 sum = Vector2.zero;

        foreach (Collider2D col in allColliders)
        {
            sum += (Vector2)col.bounds.center; // or col.transform.position if you prefer object pivot
        }

        // Calculate center point
        Vector2 centerPoint = sum / allColliders.Length;

        // Debug.Log("Center point of all 2D colliders: " + centerPoint);
        return centerPoint;

    }

    public Vector2 palautaAmmukselleVelocityVectorKaytaPiippua(GameObject alus, float ampumisevoimakkuus, Vector3 shooterPosition, Vector3 piipuntoinenpaa)
    {
        //Kamera k = mainCam.GetComponent<Kamera>();

        ampumisevoimakkuus = ampumisevoimakkuus * GameManager.Instance.PalautaDifficulty();

        if (alus == null) return Vector2.zero; // Return zero vector if target is null

        // Current positions
        //   Vector3 shooterPosition = objektijostasijaintilasketaan.transform.position;
        Vector3 alusPosition = PalautaGameObjektinKeskipiste(alus);

        // Distance to target
        Vector2 directionToAlusOikeasti = (alusPosition - shooterPosition).normalized;

        Vector2 directionToAlusOikeasti2 = ( shooterPosition- alusPosition).normalized;


        Vector2 directionToAlus = (shooterPosition - piipuntoinenpaa).normalized;

        if (directionToAlus.x > 0)
        {
            //ammutaan aluksen vasemmalla puolella
            float uusiampumis = GetKorjattuArvoPerustuenSkrollimaaraanTamaSiksiEttaAmmusLentaaNopeamminVasemmaltaOikealleSilloinKunKameraLiikkuuNopeammin
                (ampumisevoimakkuus);
            return directionToAlus * uusiampumis;
        }
        else
        {
            return directionToAlus * ampumisevoimakkuus;
        }



        /*
        // Bullet travel time (time = distance / speed)
        float distance = directionToAlus.magnitude;
        float travelTime = distance / ampumisevoimakkuus;

        // Predict future position of the target

        //EI TOIMI OK KUN KAMERAPOSITION ON 1.25

        Vector3 futureAlusPosition = alusPosition + new Vector3(k.skrollimaara * travelTime, 0f, 0f);

        // Adjust direction to aim at the predicted position
        Vector3 adjustedDirection = (futureAlusPosition - shooterPosition).normalized;

        // Calculate the velocity vector for the bullet
        Vector2 velocityVector;
        if (directionToAlus.x > 0)
        {
            //ammutaan aluksen vasemmalla puolella
            float uusiampumis = GetKorjattuArvoPerustuenSkrollimaaraanTamaSiksiEttaAmmusLentaaNopeamminVasemmaltaOikealleSilloinKunKameraLiikkuuNopeammin
                (ampumisevoimakkuus);
            velocityVector = directionToAlus * uusiampumis;
        }
        else
        {
            velocityVector = directionToAlus * ampumisevoimakkuus;

        }
        return velocityVector;
        */

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

    private AudioplayerController audioplayerControllerBaseControllerissa;

    public AudioplayerController PalautaAudioplayerController()
    {
        if (audioplayerControllerBaseControllerissa != null)
        {
            return audioplayerControllerBaseControllerissa;
        }
        audioplayerControllerBaseControllerissa = FindObjectOfType<AudioplayerController>();
        return audioplayerControllerBaseControllerissa;
    }




    private GameObject tilemaptieto;

    public GameObject PalautaTilemap()
    {
        if (tilemaptieto != null)
        {
            return tilemaptieto;
        }

        tilemaptieto = GameObject.Find("Tilemap");
        return tilemaptieto;

    }



    public GameObject palautaScoreGameObject()
    {
        return GameObject.Find("ScoreText");
        /*
        foreach (GameObject obstacles in allObstacles)
        {
            return obstacles;
        }
        return null;
        */


    }

    //

    public GameObject palautaHighScoreText()
    {
        return GameObject.Find("HighScoreText");
        /*
        foreach (GameObject obstacles in allObstacles)
        {
            return obstacles;
        }
        return null;
        */


    }

    //public float force = 5f;        // explosion force
    //public float lifetime = 2f;     // destroy pieces after this time
    public int fragmentPixelSize = 16; // size of each fragment in pixels

    /// <summary>
    /// Explodes a LineRenderer worm into physics fragments that match the material texture.
    /// </summary>
    public void ShatterWorm(LineRenderer worm, GameObject expl, float lifetime=2.0f,float prosenttiosuus=1.0f)
    {
        int points = worm.positionCount;

        int pointsprossat = (int)( points * prosenttiosuus);


        int steppi = points / pointsprossat;

        for (int i = 0; i < points; i+=steppi)
        {
            Vector3 pos = worm.GetPosition(i);

            //GameObject piece=Instantiate(expl, pos, Quaternion.identity);
            GameObject piece=
            ObjectPoolManager.Instance.GetFromPool(expl,pos, Quaternion.identity);


            // Cleanup
            //Destroy(piece, lifetime);
            ObjectPoolManager.Instance.ReturnToPool( piece, lifetime);
        }

        // Destroy the original worm
       
    }



    public void Shatterwormi()
    {
        HitCounter hc = GetComponent<HitCounter>();

        if (GetComponent<LineRenderer>() != null & hc != null && hc.rajaytaSpritenExplosion!=null)

        {

            ShatterWorm(GetComponent<LineRenderer>(), hc.rajaytaSpritenExplosion, hc.alivetime);
            return;
        }

    }


    public void RajaytaSpriteUusiMonimutkaisin(
    GameObject parenttigameobjekti,
    int rows,
    int columns,
    float explosionForce,
    float aliveTime,
    GameObject fadeexplosion = null,
    float fadeDuration = 0.5f,
    GameObject gameObjectjostalasketaanPosition = null,
    int maksimimaara = 36,
    bool teerigidbody = true,
    float gravityscale = 0.5f,
    float scalefactor = 100.0f // percentage of original size
)
    {
        if (IsGoingToBeDestroyed())
            return;

        // Clamp grid size to maximum allowed pieces
        if (columns * rows > maksimimaara)
        {
            columns = Mathf.Max(2, (int)Mathf.Sqrt(maksimimaara));
            rows = Mathf.Max(2, Mathf.CeilToInt(maksimimaara / (float)columns));
        }

        SpriteRenderer originalSR = GetComponent<SpriteRenderer>();
        if (originalSR == null)
        {
            //Debug.Log("ei spriterendereria " + name);
            Shatterwormi( );
            BaseDestroy();
            return;
        }

        Sprite originalSprite = originalSR.sprite;

        if (originalSR.sprite == null)
        {
           // Debug.Log("ei spritea " + name);
            Shatterwormi();
            BaseDestroy();

            return;
        }

        Texture2D texture = originalSprite.texture;

        int sliceWidth = texture.width / columns;
        int sliceHeight = texture.height / rows;

        float spriteWidthUnits = originalSprite.bounds.size.x * transform.localScale.x;
        float spriteHeightUnits = originalSprite.bounds.size.y * transform.localScale.y;

        float pieceWidthUnits = spriteWidthUnits / columns;
        float pieceHeightUnits = spriteHeightUnits / rows;

        // Explosion center
        Vector3 centerPos = gameObjectjostalasketaanPosition != null ?
            gameObjectjostalasketaanPosition.transform.position : transform.position;

        Quaternion rotation = gameObjectjostalasketaanPosition != null ?
            gameObjectjostalasketaanPosition.transform.rotation : transform.rotation;

        Vector2 originalVelocity = Vector2.zero;
        Rigidbody2D originalRB = parenttigameobjekti.GetComponent<Rigidbody2D>();

        Vector3 originellilokaaliskale =
        transform.localScale;
        bool massaoli = false;
        Rigidbody2D r = GetComponent<Rigidbody2D>();
        float massa = 0.0f;
        if (r!=null)
        {
            massa = r.mass;
            massaoli = true;
        }

        if (originalRB != null)
            originalVelocity = originalRB.velocity;

        int slicemaara = 0;

        float scale = scalefactor / 100f;
        //SpriteRenderer originalSR = GetComponent<SpriteRenderer>();
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                slicemaara++;
                // Flip X → reverse columns
                int sliceX = originalSR.flipX ? (columns - 1) - x : x;

                // Flip Y → reverse rows
                int sliceY = originalSR.flipY ? (rows - 1) - y : y;

                // Handle edges so last slices fit exactly
                int thisSliceWidth = (sliceX == columns - 1) ? texture.width - sliceX * sliceWidth : sliceWidth;
                int thisSliceHeight = (sliceY == rows - 1) ? texture.height - sliceY * sliceHeight : sliceHeight;

                Rect sliceRect = new Rect(
                    sliceX * sliceWidth,
                    sliceY * sliceHeight,
                    thisSliceWidth,
                    thisSliceHeight
                );

                /*
                int sliceX = x;
                if (originalSR.flipX)
                {
                    // Reverse order → right to left
                    sliceX = (columns - 1) - x;
                }

                // Allow last slices to fit properly
                int thisSliceWidth = (sliceX == columns - 1) ? texture.width - sliceX * sliceWidth : sliceWidth;
                int thisSliceHeight = (y == rows - 1) ? texture.height - y * sliceHeight : sliceHeight;

                Rect sliceRect = new Rect(sliceX * sliceWidth, y * sliceHeight, thisSliceWidth, thisSliceHeight);


                */
                Sprite newSprite = Sprite.Create(
                    texture,
                    sliceRect,
                    new Vector2(0.5f, 0.5f),
                    originalSprite.pixelsPerUnit
                );

                

                GameObject sliceObject = new GameObject($"Slice_{x}_{y}");
                SpriteRenderer sr = sliceObject.AddComponent<SpriteRenderer>();
                //flippi ei toimi
               // sr.flipX = originalSR.flipX;
               // sr.flipY = originalSR.flipY;

                sr.sprite = newSprite;
                sr.sortingLayerID = originalSR.sortingLayerID;
                sr.sortingOrder = originalSR.sortingOrder;
                sr.color = originalSR.color;

                if (originalSR.material != null)
                    sr.material = originalSR.material;



                Rigidbody2D pieceRigidbody = null;

                if (teerigidbody)
                {
                    // Collider scaled
                    BoxCollider2D collider = sliceObject.AddComponent<BoxCollider2D>();

                    /*
                    if (scale==1.0f)
                    {
                        collider.size = new Vector2(
    thisSliceWidth / originalSprite.pixelsPerUnit,
    thisSliceHeight / originalSprite.pixelsPerUnit
) * 
                    }

                    else
                    {
                        collider.size = new Vector2(
    thisSliceWidth / originalSprite.pixelsPerUnit,
    thisSliceHeight / originalSprite.pixelsPerUnit
) * scale;
                    }

                    */
                    collider.size = new Vector2(
thisSliceWidth / originalSprite.pixelsPerUnit,
thisSliceHeight / originalSprite.pixelsPerUnit
) * scale * 0.5f;//nämä tehdään aina tarkoituksella liian pieniksi

                    /*
                    collider.size = new Vector2(
                       thisSliceWidth / originalSprite.pixelsPerUnit,
                       thisSliceHeight / originalSprite.pixelsPerUnit
                   ) ;
                    */


                }
                // Rigidbody
                pieceRigidbody = sliceObject.AddComponent<Rigidbody2D>();
                pieceRigidbody.gravityScale = gravityscale;
                pieceRigidbody.velocity = originalVelocity;

                if (massaoli)
                {
                    pieceRigidbody.mass = massa;
                }

                pieceRigidbody.includeLayers = LayerMask.GetMask("Tiililayer");
                pieceRigidbody.excludeLayers = LayerMask.GetMask("AlusLayer", "AlusammusLayer", "AmmusLayer", "Default");
               
                sliceObject.layer = LayerMask.NameToLayer("Tiililayer");

                // Placement uses ORIGINAL grid size → keeps fragments together
                Vector3 localOffset = new Vector3(
                    (-spriteWidthUnits / 2f) + pieceWidthUnits * (x + 0.5f),
                    (-spriteHeightUnits / 2f) + pieceHeightUnits * (y + 0.5f),
                    0
                );

                sliceObject.transform.position = centerPos + rotation * localOffset;
                sliceObject.transform.rotation = rotation;

                // Scale DOWN the fragment visually
                //sliceObject.transform.localScale = Vector2.one * scale;

                // sliceObject.transform.localScale = new Vector3(scale, scale, 1f);

                sliceObject.transform.localScale = originellilokaaliskale * scale;

              //  Debug.Log( $"scale={scale}  {sliceObject.name} scale after assignment: {sliceObject.transform.localScale}, parent scale: {sliceObject.transform.parent?.localScale}");

                // Explosion
                if (pieceRigidbody != null)
                {
                    Vector2 dirFromCenter = (sliceObject.transform.position - centerPos).normalized;
                    float randomMult = Random.Range(0.9f, 1.1f);
                    pieceRigidbody.AddForce(dirFromCenter * explosionForce * randomMult, ForceMode2D.Impulse);
                    pieceRigidbody.AddTorque(Random.Range(-0.1f, 0.1f), ForceMode2D.Impulse);
                }

                // Fade behaviour
                sliceObject.AddComponent<FadeSlice>().Init(sr, aliveTime, fadeDuration, fadeexplosion);
            }
        }

       // Debug.Log($"Created {slicemaara} slices ({rows}x{columns}, scale {scalefactor}%) from {go.name}");
        BaseDestroy();
    }


    public void RajaytaSpriteUusiMonimutkaisinsdsdd(
    GameObject go,
    int rows,
    int columns,
    float explosionForce,
    float aliveTime,
    GameObject fadeexplosion = null,
    float fadeDuration = 0.5f,
    GameObject gameObjectjostalasketaanPosition = null,
    int maksimimaara = 36,
    bool teerigidbody = true,
    float gravityscale = 0.5f
)
    {
        if (IsGoingToBeDestroyed())
            return;

        // 🔹 Clamp grid size to maximum allowed pieces
        if (columns * rows > maksimimaara)
        {
            columns = Mathf.Max(2, (int)Mathf.Sqrt(maksimimaara));
            rows = Mathf.Max(2, Mathf.CeilToInt(maksimimaara / (float)columns));
        }

        SpriteRenderer originalSR = GetComponent<SpriteRenderer>();
        if (originalSR == null)
        {
            Debug.Log("ei spriterendereria " + go.name);
            BaseDestroy();
            return;
        }

        Sprite originalSprite = originalSR.sprite;
        Texture2D texture = originalSprite.texture;

        int sliceWidth = texture.width / columns;
        int sliceHeight = texture.height / rows;

        float spriteWidthUnits = originalSprite.bounds.size.x * transform.localScale.x;
        float spriteHeightUnits = originalSprite.bounds.size.y * transform.localScale.y;

        float pieceWidthUnits = spriteWidthUnits / columns;
        float pieceHeightUnits = spriteHeightUnits / rows;

        // 🔹 Explosion center
        Vector3 centerPos = gameObjectjostalasketaanPosition != null ?
            gameObjectjostalasketaanPosition.transform.position : transform.position;

        Quaternion rotation = gameObjectjostalasketaanPosition != null ?
            gameObjectjostalasketaanPosition.transform.rotation : transform.rotation;

        Vector2 originalVelocity = Vector2.zero;
        Rigidbody2D originalRB = go.GetComponent<Rigidbody2D>();
        if (originalRB != null)
            originalVelocity = originalRB.velocity;

        int slicemaara = 0;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                slicemaara++;

                // 🔹 adjust last slices if uneven
                int thisSliceWidth = (x == columns - 1) ? texture.width - x * sliceWidth : sliceWidth;
                int thisSliceHeight = (y == rows - 1) ? texture.height - y * sliceHeight : sliceHeight;

                Rect sliceRect = new Rect(x * sliceWidth, y * sliceHeight, thisSliceWidth, thisSliceHeight);

                Sprite newSprite = Sprite.Create(
                    texture,
                    sliceRect,
                    new Vector2(0.5f, 0.5f),
                    originalSprite.pixelsPerUnit
                );

                GameObject sliceObject = new GameObject($"Slice_{x}_{y}");
                SpriteRenderer sr = sliceObject.AddComponent<SpriteRenderer>();
                sr.sprite = newSprite;
                sr.sortingLayerID = originalSR.sortingLayerID;
                sr.sortingOrder = originalSR.sortingOrder;

                if (originalSR.material != null)
                    sr.material = originalSR.material;

                Rigidbody2D pieceRigidbody = null;

                if (teerigidbody)
                {
                    // 🔹 collider
                    BoxCollider2D collider = sliceObject.AddComponent<BoxCollider2D>();
                    collider.size = new Vector2(
                        thisSliceWidth / originalSprite.pixelsPerUnit,
                        thisSliceHeight / originalSprite.pixelsPerUnit
                    );


                }
                // 🔹 rigidbody
                pieceRigidbody = sliceObject.AddComponent<Rigidbody2D>();
                pieceRigidbody.gravityScale = gravityscale;
                pieceRigidbody.velocity = originalVelocity;

                // 🔹 Unity 2022+ collision filtering
                pieceRigidbody.includeLayers = LayerMask.GetMask("Tiililayer");
                pieceRigidbody.excludeLayers = LayerMask.GetMask("AlusLayer", "AlusammusLayer", "AmmusLayer", "Default");

                sliceObject.layer = LayerMask.NameToLayer("Tiililayer");
                // 🔹 place slice
                Vector3 localOffset = new Vector3(
                    (-spriteWidthUnits / 2f) + pieceWidthUnits * (x + 0.5f),
                    (-spriteHeightUnits / 2f) + pieceHeightUnits * (y + 0.5f),
                    0
                );

                sliceObject.transform.position = centerPos + rotation * localOffset;
                sliceObject.transform.rotation = rotation;
                sliceObject.transform.localScale = Vector3.one;

                // 🔹 explosion
                if (pieceRigidbody != null)
                {
                    Vector2 dirFromCenter = (sliceObject.transform.position - centerPos).normalized;
                    float randomMult = Random.Range(0.7f, 1.3f); // variation so not all fly equally
                    pieceRigidbody.AddForce(dirFromCenter * explosionForce * randomMult, ForceMode2D.Impulse);
                    pieceRigidbody.AddTorque(Random.Range(-10f, 10f), ForceMode2D.Impulse);
                }

                // 🔹 fade + destroy
                sliceObject.AddComponent<FadeSlice>().Init(sr, aliveTime, fadeDuration, fadeexplosion);
            }
        }

        Debug.Log($"Created {slicemaara} slices ({rows}x{columns}) from {go.name}");
        BaseDestroy();
    }


    public void RajaytaSpriteUusiMonimutkaisinds(
    GameObject go,
    int rows,
    int columns,
    float explosionForce,
    float aliveTime,
    GameObject fadeexplosion = null,
    float fadeDuration = 0.5f,
    GameObject gameObjectjostalasketaanPosition = null,
    int maksimimaara = 36,
    bool teerigidbody = true,
    float gravityscale = 0.5f
)
    {
        if (IsGoingToBeDestroyed())
            return;

        //  Clamp grid size to maximum allowed pieces
        if (columns * rows > maksimimaara)
        {
            columns = Mathf.Max(2, (int)Mathf.Sqrt(maksimimaara));
            rows = Mathf.Max(2, Mathf.CeilToInt(maksimimaara / (float)columns));
        }

        SpriteRenderer originalSR = GetComponent<SpriteRenderer>();
        if (originalSR == null)
        {
            Debug.Log("ei spriterendereria " + go.name);
            BaseDestroy();
            return;
        }

        Sprite originalSprite = originalSR.sprite;
        Texture2D texture = originalSprite.texture;

        int sliceWidth = texture.width / columns;
        int sliceHeight = texture.height / rows;

        float spriteWidthUnits = originalSprite.bounds.size.x * transform.localScale.x;
        float spriteHeightUnits = originalSprite.bounds.size.y * transform.localScale.y;

        float pieceWidthUnits = spriteWidthUnits / columns;
        float pieceHeightUnits = spriteHeightUnits / rows;

        //  Explosion center
        Vector3 centerPos = gameObjectjostalasketaanPosition != null ?
            gameObjectjostalasketaanPosition.transform.position : transform.position;

        Quaternion rotation = gameObjectjostalasketaanPosition != null ?
            gameObjectjostalasketaanPosition.transform.rotation : transform.rotation;

        Vector2 originalVelocity = Vector2.zero;
        Rigidbody2D originalRB = go.GetComponent<Rigidbody2D>();
        if (originalRB != null)
            originalVelocity = originalRB.velocity;

        int slicemaara = 0;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                slicemaara++;

                // allow last slices to fit
                int thisSliceWidth = (x == columns - 1) ? texture.width - x * sliceWidth : sliceWidth;
                int thisSliceHeight = (y == rows - 1) ? texture.height - y * sliceHeight : sliceHeight;

                Rect sliceRect = new Rect(x * sliceWidth, y * sliceHeight, thisSliceWidth, thisSliceHeight);

                Sprite newSprite = Sprite.Create(
                    texture,
                    sliceRect,
                    new Vector2(0.5f, 0.5f),
                    originalSprite.pixelsPerUnit
                );

                GameObject sliceObject = new GameObject($"Slice_{x}_{y}");
                SpriteRenderer sr = sliceObject.AddComponent<SpriteRenderer>();
                sr.sprite = newSprite;
                sr.sortingLayerID = originalSR.sortingLayerID;
                sr.sortingOrder = originalSR.sortingOrder;

                // copy material if exists
                if (originalSR.material != null)
                    sr.material = originalSR.material;

                Rigidbody2D pieceRigidbody = null;

                if (teerigidbody)
                {
                    //  add collider & rigidbody
                    BoxCollider2D collider = sliceObject.AddComponent<BoxCollider2D>();
                    collider.size = new Vector2(
                        thisSliceWidth / originalSprite.pixelsPerUnit,
                        thisSliceHeight / originalSprite.pixelsPerUnit
                    );


                }
                pieceRigidbody = sliceObject.AddComponent<Rigidbody2D>();
                pieceRigidbody.gravityScale = gravityscale;
                pieceRigidbody.velocity = originalVelocity;

                //  Unity 2022+ filtering
                pieceRigidbody.includeLayers = LayerMask.GetMask("Tiililayer");
                pieceRigidbody.excludeLayers = LayerMask.GetMask("AlusLayer", "AlusammusLayer", "AmmusLayer", "Default");

                sliceObject.layer = LayerMask.NameToLayer("Tiililayer");

                //  place slice in world
                Vector3 localOffset = new Vector3(
                    (-spriteWidthUnits / 2f) + pieceWidthUnits * (x + 0.5f),
                    (-spriteHeightUnits / 2f) + pieceHeightUnits * (y + 0.5f),
                    0
                );

                sliceObject.transform.position = centerPos + rotation * localOffset;
                sliceObject.transform.rotation = rotation;
                sliceObject.transform.localScale = Vector3.one; // prevent double-scaling

                //  apply explosion AFTER position is set
                if (pieceRigidbody != null)
                {
                    Vector2 dirFromCenter = (sliceObject.transform.position - centerPos).normalized;
                    pieceRigidbody.AddForce(dirFromCenter * explosionForce, ForceMode2D.Impulse);
                    pieceRigidbody.AddTorque(Random.Range(-10f, 10f), ForceMode2D.Impulse);
                }

                //  fade behaviour
                sliceObject.AddComponent<FadeSlice>().Init(sr, aliveTime, fadeDuration, fadeexplosion);
            }
        }

        Debug.Log($"Created {slicemaara} slices ({rows}x{columns}) from {go.name}");
        BaseDestroy();
    }





    public void RajaytaSpriteUusiMonimutkaisinTaas(
    GameObject go,
    int rows,
    int columns,
    float explosionForce,
    float aliveTime,
    GameObject fadeexplosion = null,
    float fadeDuration = 0.5f,
    GameObject gameObjectjostalasketaanPosition = null,
    int maksimimaara = 36, bool teerigidbody = true, float gravityscale = 0.5f
)
    {
        if (IsGoingToBeDestroyed())
        {
            return;
        }


        // Laske neliömäinen ruudukko jos rows*columns ylittää maksimin
        if (columns * rows > maksimimaara)
        {
            columns = (int)Mathf.Sqrt(maksimimaara);
            rows = Mathf.CeilToInt(maksimimaara / (float)columns);
        }

        if (columns < 2) columns = 2;
        if (rows < 2) rows = 2;

        SpriteRenderer originalSR = GetComponent<SpriteRenderer>();


        if (originalSR == null)
        {
            Debug.Log("ei spriterendereria " + go.name);
            BaseDestroy();
            return;
        }
        Sprite originalSprite = originalSR.sprite;
        Texture2D texture = originalSprite.texture;

        int sliceWidth = texture.width / columns;
        int sliceHeight = texture.height / rows;

        float spriteWidthUnits = originalSprite.bounds.size.x * transform.localScale.x;
        float spriteHeightUnits = originalSprite.bounds.size.y * transform.localScale.y;

        float pieceWidthUnits = spriteWidthUnits / columns;
        float pieceHeightUnits = spriteHeightUnits / rows;

        // Keskipiste
        Vector3 centerPos;
        Quaternion rotation;
        if (gameObjectjostalasketaanPosition != null)
        {
            centerPos = gameObjectjostalasketaanPosition.transform.position;
            rotation = gameObjectjostalasketaanPosition.transform.rotation;
        }
        else
        {
            centerPos = transform.position;
            rotation = transform.rotation;
        }

        Vector2 originalVelocity = Vector2.zero;
        Rigidbody2D originalRB = go.GetComponent<Rigidbody2D>();
        if (originalRB != null)
            originalVelocity = originalRB.velocity;

        int slicemaara = 0;
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                slicemaara++;

                // HUOM: viimeinen rivi/sarake voi olla leveämpi/korkeampi jos ei mene tasan
                int thisSliceWidth = (x == columns - 1) ? texture.width - x * sliceWidth : sliceWidth;
                int thisSliceHeight = (y == rows - 1) ? texture.height - y * sliceHeight : sliceHeight;

                Rect sliceRect = new Rect(x * sliceWidth, y * sliceHeight, thisSliceWidth, thisSliceHeight);

                Sprite newSprite = Sprite.Create(
                    texture,
                    sliceRect,
                    new Vector2(0.5f, 0.5f),
                    originalSprite.pixelsPerUnit
                );

                GameObject sliceObject = new GameObject($"Slice_{x}_{y}");
                SpriteRenderer sr = sliceObject.AddComponent<SpriteRenderer>();
                sr.sprite = newSprite;
                sr.sortingLayerID = originalSR.sortingLayerID;
                sr.sortingOrder = originalSR.sortingOrder;

                SpriteRenderer srori = go.GetComponent<SpriteRenderer>();
                if (srori != null)
                {
                    sr.material = srori.material;
                }

                if (teerigidbody)
                {

                    //ei tehdä tätä
                    BoxCollider2D collider = sliceObject.AddComponent<BoxCollider2D>();
                    collider.size = new Vector2(
                        thisSliceWidth / originalSprite.pixelsPerUnit,
                        thisSliceHeight / originalSprite.pixelsPerUnit
                    );



                }


                Rigidbody2D pieceRigidbody = sliceObject.AddComponent<Rigidbody2D>();
                pieceRigidbody.gravityScale = gravityscale;
                pieceRigidbody.velocity = originalVelocity;
                pieceRigidbody.includeLayers = LayerMask.GetMask("Tiililayer");
                pieceRigidbody.excludeLayers = LayerMask.GetMask("AlusLayer", "AlusammusLayer", "AmmusLayer", "Default");

                Vector2 dirFromCenter = (sliceObject.transform.position - centerPos).normalized;
                pieceRigidbody.AddForce(dirFromCenter * explosionForce, ForceMode2D.Impulse);
                pieceRigidbody.AddTorque(Random.Range(-10f, 10f), ForceMode2D.Impulse);

                sliceObject.layer = LayerMask.NameToLayer("Tiililayer");

                // HUOM: vain uudemmissa Unityissä toimii include/excludeLayers

                Vector3 localOffset = new Vector3(
                    (-spriteWidthUnits / 2f) + pieceWidthUnits * (x + 0.5f),
                    (-spriteHeightUnits / 2f) + pieceHeightUnits * (y + 0.5f),
                    0
                );

                sliceObject.transform.position = centerPos + rotation * localOffset;
                sliceObject.transform.rotation = rotation;
                sliceObject.transform.localScale = transform.localScale;


                // fade ja destroy
                sliceObject.AddComponent<FadeSlice>().Init(sr, aliveTime, fadeDuration, fadeexplosion);
            }
        }

        Debug.Log($"Created {slicemaara} slices ({rows}x{columns}) from {go.name}");
        BaseDestroy();
        //Destroy(go);

    }



    private IEnumerator FadeAndDestroy(SpriteRenderer sr, float aliveTime, float fadeDuration)
    {
        // Ensure fade starts only when there's time
        yield return new WaitForSeconds(Mathf.Max(0f, aliveTime - fadeDuration));

        Color originalColor = sr.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        //BaseDestroy();
        Destroy(sr.gameObject);
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
        RajaytaSprite(go, rows, columns, explosionForce, alivetime, sirpalemass, teeBoxcollider2d, ysaato, skaalaatekstuuria, gravityscale, xsaato, addDestroyController, explosion, false, null, null);
    }

    private static int maxExplosionsPertime = 20;
    private static Queue<float> explosionTimestamps = new Queue<float>();

    private float second = 0.5f;

    public void RajaytaSprite(GameObject go, int rows, int columns, float explosionForce, float alivetime,
    float sirpalemass, bool teeBoxcollider2d, float ysaato, bool skaalaatekstuuria, float gravityscale,
        float xsaato, bool addDestroyController, GameObject explosion, bool siirragameobjectinvelocitypaloihin, string tag,
        string layer, int maksimimaara = 36
        )
    {
        if (IsGoingToBeDestroyed())
        {
            return;
        }

        SpriteRenderer s = GetComponent<SpriteRenderer>();
        if (s == null)
        {
            BaseDestroy(go);

            return;
        }

        if (rows <= 1)
        {
            rows = 2;
        }
        if (columns <= 1)
        {
            columns = 2;
        }

        if (columns * rows > maksimimaara)
        {
            columns = (int)Mathf.Sqrt(maksimimaara);
            rows = Mathf.CeilToInt(maksimimaara / (float)columns);
        }

        float currentTime = Time.time;

        // Remove timestamps older than 1 second
        while (explosionTimestamps.Count > 0 && currentTime - explosionTimestamps.Peek() > second)
        {
            explosionTimestamps.Dequeue();
        }

        // Check if we can explode
        if (explosionTimestamps.Count < maxExplosionsPertime)
        {
            explosionTimestamps.Enqueue(currentTime);
            // Debug.Log("BOOM!");
            // Your explode logic here
        }
        else
        {
            Debug.Log("Explosion rate limit reached (max 5 per second).");
            BaseDestroy();

            return;
        }


        Sprite originalSprite = s.sprite;
        bool xflippi = s.flipX;
        bool yflippi = s.flipY;

        float ymin = s.bounds.min.y;
        float ymax = s.bounds.max.y;

        float puolet = (ymax - ymin) / 2;

        if (originalSprite == null)
        {
            BaseDestroy();

            return;
        }


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
                if (tag != null)
                {
                    sliceObject.tag = tag;
                }
                if (layer != null)
                {
                    sliceObject.layer = LayerMask.NameToLayer(layer);

                }
                else
                {
                    sliceObject.layer = go.layer;

                }

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

                if (siirragameobjectinvelocitypaloihin)
                {
                    if (gameObject.GetComponent<Rigidbody2D>() != null)
                    {
                        pieceRigidbody.velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
                    }
                }


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

                if (!siirragameobjectinvelocitypaloihin)
                {
                    pieceRigidbody.AddForce(randomDirection * randomForce, ForceMode2D.Impulse);


                    // Optionally, add random torque for rotation

                    pieceRigidbody.AddTorque(Random.Range(-10f, 10f), ForceMode2D.Impulse);
                }



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

        BaseDestroy();

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




    public void IgnoreChildCollisions(Transform parent)
    {
        if (parent == null) return;

        Collider2D parentCollider = parent.GetComponent<Collider2D>();
        Collider2D[] childColliders = parent.GetComponentsInChildren<Collider2D>();

        for (int i = 0; i < childColliders.Length; i++)
        {
            // Ignore collisions between the parent collider and child colliders
            if (parentCollider != null)
            {
                Physics2D.IgnoreCollision(childColliders[i], parentCollider);
            }

            // Ignore collisions between all child colliders
            for (int j = i + 1; j < childColliders.Length; j++)
            {
                Physics2D.IgnoreCollision(childColliders[i], childColliders[j]);
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
            if (go1 == null)
            {
                continue;
            }
            Collider2D[] colliders1 = go1.GetComponentsInChildren<Collider2D>();

            for (int j = i + 1; j < lista.Count; j++) // Avoid duplicate comparisons
            {
                GameObject go2 = lista[j];
                if (go2 == null)
                {
                    continue;
                }
                Collider2D[] colliders2 = go2.GetComponentsInChildren<Collider2D>();

                // Ignore collisions between all colliders in go1 and go2
                foreach (Collider2D c1 in colliders1)
                {
                    foreach (Collider2D c2 in colliders2)
                    {
                        Physics2D.IgnoreCollision(c1, c2);
                    }
                }
            }
        }
    }



    /*
    public bool IsObjectLeftOfCamera(GameObject go, float ero)
    {

        Vector3 objectPosition = go.transform.position;
        Vector3 cameraPosition = mainCam.transform.position;

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
        Camera camera = mainCam; // Reference to the main camera
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
        float ykamera = mainCam.transform.position.y;
        float koko = mainCam.orthographicSize;


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
            if (IsObjectRightOfCamera(mainCam, go.transform))
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
        Vector3 viewportPosition = mainCam.WorldToViewportPoint(transform.position);

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
            BaseDestroy();
            //Destroy(go);

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

    public bool IsFullyVisible2D()
    {
        Vector3 pos = mainCam.WorldToViewportPoint(transform.position);
        Renderer rend = GetComponent<Renderer>();
        if (rend == null) return false;

        float halfWidth = rend.bounds.size.x / 2;
        float halfHeight = rend.bounds.size.y / 2;

        Vector3 leftBottom = mainCam.WorldToViewportPoint(transform.position - new Vector3(halfWidth, halfHeight, 0));
        Vector3 rightTop = mainCam.WorldToViewportPoint(transform.position + new Vector3(halfWidth, halfHeight, 0));

        return leftBottom.x >= 0 && leftBottom.y >= 0 &&
               rightTop.x <= 1 && rightTop.y <= 1;
    }

    public bool IsMoreThanHalfOnLeft()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend == null || mainCam == null) return false;

        Bounds bounds = rend.bounds;

        // Sample points along the sprite’s width
        int sampleCount = 5; // More samples = better accuracy
        int countOnLeft = 0;

        for (int i = 0; i <= sampleCount; i++)
        {
            float t = i / (float)sampleCount;
            Vector3 worldPoint = new Vector3(Mathf.Lerp(bounds.min.x, bounds.max.x, t), bounds.center.y, bounds.center.z);
            Vector3 viewportPoint = mainCam.WorldToViewportPoint(worldPoint);

            if (viewportPoint.x < 0.5f && viewportPoint.z > 0) // z > 0 = in front of camera
            {
                countOnLeft++;
            }
        }

        return (countOnLeft / (float)(sampleCount + 1)) > 0.5f;
    }

    public bool IsAnyOfGameObjectVisible(List<GameObject> gos)
    {
        if (gos == null || gos.Count == 0)
        {
            return false;
        }
        Camera mainCamera = mainCam;
        foreach (GameObject go in gos)
        {
            if (mainCam == null) return false; // Ensure there is a main camera
            if (go == null || go.transform == null)
            {
                continue;
            }
            /*
            SpriteRenderer sp = go.GetComponent<SpriteRenderer>();
            if (sp != null && !sp.enabled)
            {
                continue;
            }
            */

            Renderer renderer = go.GetComponent<Renderer>();

            if (renderer != null && !renderer.enabled)
            {
                continue;
            }

            if (renderer != null && renderer.isVisible)
                return true;

            // Get the main camera


            // Convert the target's position to viewport coordinates
            Vector3 viewportPosition = mainCamera.WorldToViewportPoint(go.transform.position);

            // Check if the target is within the camera's viewport
            bool isVisible = viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
                             viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
                             viewportPosition.z > 0; // Ensure it's in front of the camera

            if (isVisible)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsGameObjectVisible()
    {

        // Get the main camera
        Camera mainCamera = mainCam;

        if (mainCam == null) return false; // Ensure there is a main camera


        // Convert the target's position to viewport coordinates
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        // Check if the target is within the camera's viewport
        bool isVisible = viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
                         viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
                         viewportPosition.z > 0; // Ensure it's in front of the camera

        return isVisible;
    }

    public bool IsGameObjectVisible(float xoffset)
    {
        if (mainCam == null) return false; // Ensure there is a main camera

        // Get the main camera
        Camera mainCamera = mainCam;

        // Convert the target's position to viewport coordinates
        Vector3 uusi = new Vector3(transform.position.x + xoffset, transform.position.y, transform.position.z);

        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(uusi);

        // Check if the target is within the camera's viewport
        bool isVisible = viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
                         viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
                         viewportPosition.z > 0; // Ensure it's in front of the camera

        return isVisible;
    }


    /*
    public Color gizmoColoraaaa = Color.cyan;

    private void OnDrawGizmos()
    {
        Camera cam = mainCam;
        if (cam == null || !cam.orthographic) return;

        Gizmos.color = gizmoColoraaaa;

        float height = cam.orthographicSize * 2;
        float width = height * cam.aspect;

        Vector3 camPos = cam.transform.position;
        Vector3 bottomLeft = new Vector3(camPos.x - width / 2f, camPos.y - height / 2f, 0);
        Vector3 bottomRight = new Vector3(camPos.x + width / 2f, camPos.y - height / 2f, 0);
        Vector3 topRight = new Vector3(camPos.x + width / 2f, camPos.y + height / 2f, 0);
        Vector3 topLeft = new Vector3(camPos.x - width / 2f, camPos.y + height / 2f, 0);

        // Draw rectangle
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
    }
    */
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


    public bool OnkoVasemmallaVaistettavaa(int rayCount, float rayDistance, LayerMask collisionLayer,

    RaycastHit2D[] hitsBuffer
        )
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
            //RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Vector2.left, rayDistance, collisionLayer);
            int hitCount = Physics2D.RaycastNonAlloc(rayOrigin, Vector2.left, hitsBuffer, rayDistance, collisionLayer);
            /*
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
            */

            for (int j = 0; j < hitCount; j++)
            {
                RaycastHit2D hit = hitsBuffer[j];
                // do something with hit
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
    public bool OlisikoylhaallaVaistotilaa(int rayCount, float rayDistance, LayerMask collisionLayer,
        RaycastHit2D[] hitsBuffer)
    {
        float yloskulunmaksi = CalculateMoveUpDistance(mainCam, transform.position);
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
            // RaycastHit2D[] hits = Physics2D.RaycastNonAlloc(rayOrigin, Vector2.left, rayDistance, collisionLayer, hitsBuffer);

            int hitCount = Physics2D.RaycastNonAlloc(rayOrigin, Vector2.left, hitsBuffer, rayDistance, collisionLayer);

            bool onkoosumaa = false;
            for (int j = 0; j < hitCount; j++)
            {
                RaycastHit2D hit = hitsBuffer[j];
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

    public bool OlisikoalhaallaVaistotilaa(int rayCount, float rayDistance, LayerMask collisionLayer, RaycastHit2D[] hitsBuffer)
    {
        float yloskulunmaksi = CalculateMoveDownDistance(mainCam, transform.position);
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
            int hitCount = Physics2D.RaycastNonAlloc(rayOrigin, Vector2.left, hitsBuffer, rayDistance, collisionLayer);

            bool onkoosumaa = false;
            for (int j = 0; j < hitCount; j++)
            {
                RaycastHit2D hit = hitsBuffer[j];
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
        float height = mainCam.orthographicSize * 2;
        float width = height * mainCam.aspect;

        // Top-right corner of the camera's view in world space
        Vector3 maxWorldPosition = mainCam.transform.position + new Vector3(width / 2, height / 2, 0);

        return maxWorldPosition;
    }
    public Vector3 GetCameraMinWorldPosition()
    {
        // Calculate the camera's dimensions in world space
        float height = mainCam.orthographicSize * 2;
        float width = height * mainCam.aspect;

        // Bottom-left corner of the camera's view in world space
        Vector3 minWorldPosition = mainCam.transform.position - new Vector3(width / 2, height / 2, 0);

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




    public void Vaista(float delta, LayerMask collisionLayer, RaycastHit2D[] hitsBuffer)
    {
        bool vasen = OnkoVasemmallaVaistettavaa(rayCount, rayDistance, collisionLayer, hitsBuffer);
        //Debug.Log("vasen=" + vasen);

        if (vasen)
        {
            bool ylos =
                OlisikoylhaallaVaistotilaa(rayCount, rayDistance, collisionLayer, hitsBuffer);

            //Debug.Log("ylos=" + ylos);
            if (ylos)
            {
                VaistaPystysuunnassa(delta, vaistonopeus);

            }
            else
            {
                bool alas = OlisikoalhaallaVaistotilaa(rayCount, rayDistance, collisionLayer, hitsBuffer);
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

    public void Tuhoa(GameObject go, float nopeusjonkaalletuhotaan)
    {


        TuhoaReal(go, true, nopeusjonkaalletuhotaan, true, 5.0f, true, 5.0f, true, 5.0f,
true,
600,
1, false, null
);

    }

    public void Tuhoa(GameObject prefap, GameObject go, float nopeusjonkaalletuhotaan)
    {


        TuhoaReal(go, true, nopeusjonkaalletuhotaan, true, 5.0f, true, 5.0f, true, 5.0f,
true,
600,
0.4f, false, prefap
);

    }

    /*
    public bool TuhoaKiinteatObjektit(GameObject prefap, GameObject go)
    {
        //@todoo
        syklilaskuri += Time.deltaTime;
        if (syklilaskuri >= tarkistussykli)
        {
            //sallitaan pienet alitukset ja ylitykset näille....
            if (!IsGameObjectVisible())
            {
                BaseDestroy(go, prefap);
                return true;
            }
        }
        return false;
    }
    */

    public bool TuhoaAmmukset( GameObject go, float hengissaolonraja = 100.0f, float liikkeenmininopeus = 1.0f)
    {

        hengissaoloaika += Time.deltaTime;

        if (hengissaoloaika >= hengissaolonraja)
        {
            //Destroy(go);
            BaseDestroy(go);
            return true;
        }
        syklilaskuri += Time.deltaTime;
        if (syklilaskuri >= tarkistussykli)
        {
            //sallitaan pienet alitukset ja ylitykset
            if (!IsGameObjectVisible())
            {
                BaseDestroy(go);
                return true;
            }
            Rigidbody2D r = go.GetComponent<Rigidbody2D>();
            if (r != null)
            {
                float speed = r.velocity.magnitude;
                if (speed <= liikkeenmininopeus)
                {
                    BaseDestroy(go);
                    return true;
                }
            }
        }
        return false;
    }


    public bool TuhoaKunElamisenAikaRajaTayttyyTaiHidastuuLiikaa(GameObject prefap, GameObject go, float hengissaolonraja, float nopeudenalaraja)
    {

        syklilaskuri += Time.deltaTime;
        if (syklilaskuri >= tarkistussykli)
        {
            /*
            if (prefap == null)
            {
                prefap = GetPrefap();
            }
            */
            syklilaskuri = 0.0f;
            hengissaoloaika += tarkistussykli;

            if (hengissaoloaika >= hengissaolonraja)
            {
                //Destroy(go);
                hengissaoloaika = 0.0f;
                
                    BaseDestroy();
                    return true;
                
            }

            bool liianylhaalla = OnkoKameranYlaPuolella(go, 5.0f);
            if (liianylhaalla)
            {
                //hengissaoloaika = 0.0f;
                //ObjectPoolManager.Instance.ReturnToPool(prefap, go);
                BaseDestroy();
                return true;

            }


            Rigidbody2D r = go.GetComponent<Rigidbody2D>();
            if (r != null)
            {
                float speed = r.velocity.magnitude;
                if (speed <= nopeudenalaraja)
                {
                    
                        BaseDestroy();
                        return true;
                    

                }
            }

        }
        return false;
    }


    public void TuhoaJosOikeallaPuolenKameraaTutkimuitakainEsimNopeus(GameObject go, float nopeusjonkaalle)
    {


        TuhoaReal(go, true, nopeusjonkaalle, true, 5.0f, true, 5.0f, true, 5.0f,
true,
600,
0.1f, true, null
);

    }





    public void Tuhoa(GameObject go)
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
        float offsettiylapuolellakavaisyyn = 0.5f;

        // Debug.Log("This code runs only in builds, not in the Unity Editor.");
        TuhoaReal(go, false, -1, true, 5.0f, true, alaoffsetti, true, offsettiylapuolellakavaisyyn,
true,
6000,
5, false, null
);

    }


    public void TuhoaJosOllaanSiirrettyJonkunVerranKameranVasemmallePuolenSalliPieniAlitusJaYlitys(GameObject go)
    {
        bool tuhoajosliianhidas = false;
        float nopeusJonkaAlletuhotaan = 10f;
        bool tuhoaJosKameranVasemmallaPuolella = true;
        float offsettijokasallitaanvasemmallakavaisyyn = 1.5f;
        bool tuhoaJosKameranAlapuolella = true;
        //float offsettijokasallitaanAlhaallaKavaisyyn = 0.1f;
        float offsettijokasallitaanAlhaallaKavaisyyn = 5.0f;

        bool tuhoaJosKameranYlapuolella = true;
        //float offsettijokasallitaanYlapuolellaKavaisyyn = 2.0f;
        float offsettijokasallitaanYlapuolellaKavaisyyn = 5.0f;

        bool tuhoajosliiankauanhengissa = true;
        float hengissaolonraja = 600f;//tarkista
        float tarkistussykli = 0.01f;
        bool tuhoajosoikeallakameraanVerrattuna = false;

        TuhoaReal(go, tuhoajosliianhidas, nopeusJonkaAlletuhotaan, tuhoaJosKameranVasemmallaPuolella, offsettijokasallitaanvasemmallakavaisyyn,
            tuhoaJosKameranAlapuolella, offsettijokasallitaanAlhaallaKavaisyyn, tuhoaJosKameranYlapuolella, offsettijokasallitaanYlapuolellaKavaisyyn,
            tuhoajosliiankauanhengissa, hengissaolonraja, tarkistussykli, tuhoajosoikeallakameraanVerrattuna, null
);

        /*
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
        */


    }


    public void TuhoaJosOllaanSiirrettyReilustiKameranVasemmallePuolenSalliPieniAlitusJaYlitys(GameObject go)
    {
        bool tuhoajosliianhidas = false;
        float nopeusJonkaAlletuhotaan = 10f;
        bool tuhoaJosKameranVasemmallaPuolella = true;
        float offsettijokasallitaanvasemmallakavaisyyn = 5.0f;
        bool tuhoaJosKameranAlapuolella = true;
        float offsettijokasallitaanAlhaallaKavaisyyn = 1.0f;
        bool tuhoaJosKameranYlapuolella = true;
        float offsettijokasallitaanYlapuolellaKavaisyyn = 1.0f;
        bool tuhoajosliiankauanhengissa = true;
        float hengissaolonraja = 600f;//tarkista
        float tarkistussykli = 0.1f;
        bool tuhoajosoikeallakameraanVerrattuna = false;

        TuhoaReal(go, tuhoajosliianhidas, nopeusJonkaAlletuhotaan, tuhoaJosKameranVasemmallaPuolella, offsettijokasallitaanvasemmallakavaisyyn,
            tuhoaJosKameranAlapuolella, offsettijokasallitaanAlhaallaKavaisyyn, tuhoaJosKameranYlapuolella, offsettijokasallitaanYlapuolellaKavaisyyn,
            tuhoajosliiankauanhengissa, hengissaolonraja, tarkistussykli, tuhoajosoikeallakameraanVerrattuna, null
);

        /*
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
        */


    }

    //private Dictionary<GameObject, Rigidbody2D> rigidbodyCache = new Dictionary<GameObject, Rigidbody2D>();
    private float onkoOkToimiaTarkistussykli = 0.2f;
    private float onkoOkToimiaTarkistuksensyklilaskuri = 0.2f;

    private bool viimeinentarkistustulos = false;


    public bool IsVisibleUseMargin(GameObject go)
    {
        Camera cam = PalautaMainCam();


        Vector3 viewportPos = cam.WorldToViewportPoint(go.transform.position);

        float margin = 0.5f; // 10% buffer
        bool isVisible = viewportPos.z > 0 &&
                         viewportPos.x >= -margin && viewportPos.x <= 1 + margin &&
                         viewportPos.y >= -margin && viewportPos.y <= 1 + margin;

        return isVisible;

    }

    public bool IsRendererVisibleWithMargin2D(GameObject go, float margin = 0.5f)
    {
        Camera cam = PalautaMainCam();
        if (cam == null || go == null) return false;

        Renderer renderer = go.GetComponent<Renderer>();
        if (renderer == null) return false;

        Bounds bounds = renderer.bounds;

        // Get the 4 corners in world space (Z stays the same as bounds.center.z)
        Vector3[] corners =
        {
        new Vector3(bounds.min.x, bounds.min.y, bounds.center.z),
        new Vector3(bounds.min.x, bounds.max.y, bounds.center.z),
        new Vector3(bounds.max.x, bounds.min.y, bounds.center.z),
        new Vector3(bounds.max.x, bounds.max.y, bounds.center.z)
    };

        foreach (var corner in corners)
        {
            Vector3 viewportPos = cam.WorldToViewportPoint(corner);

            // If any corner is behind the camera, it's not visible
            if (viewportPos.z < 0)
                return false;

            // If any corner goes outside viewport+margin, the whole object is not fully visible
            if (viewportPos.x < -margin || viewportPos.x > 1 + margin ||
                viewportPos.y < -margin || viewportPos.y > 1 + margin)
                return false;
        }

        return true; // all corners are inside
    }



    public bool AnyChildVisibleUseMargin(GameObject go)
    {
        Camera cam = PalautaMainCam();

        foreach (Transform child in go.transform)
        {
            Vector3 viewportPos = cam.WorldToViewportPoint(child.position);

            float margin = 0.2f; // 10% buffer
            bool isVisible = viewportPos.z > 0 &&
                             viewportPos.x >= -margin && viewportPos.x <= 1 + margin &&
                             viewportPos.y >= -margin && viewportPos.y <= 1 + margin;

            if (isVisible) return true;
        }

        return false;
    }



    //sitten
    public bool OnkoOkToimiaUusi(GameObject go)
    {

        if (GameManager.Instance.isGoingToBeDestroyed)
        {
            return false;
        }


        /*
        if (go.tag.Contains("tiili"))
        {
            return false;
        }
        */


        if (go == null)
        {
            Debug.Log("OnkoOnOkToimiaUusi kutsuttu nullilla");
            return false;
        }


        onkoOkToimiaTarkistuksensyklilaskuri += Time.deltaTime;
        if (onkoOkToimiaTarkistuksensyklilaskuri >= onkoOkToimiaTarkistussykli)
        {
            onkoOkToimiaTarkistuksensyklilaskuri = 0.0f;
        }
        else
        {
            return viimeinentarkistustulos;
        }


        /*
        //jos on näkyvillä
        //entäs muulloin


        if (s!=null && s.isVisible)
        {
            return true;
        }
        return false;
        */
        SpriteRenderer s = go.GetComponent<SpriteRenderer>();
        bool nakyvilla;
        if (s==null)
        {
            nakyvilla = false;
            SpriteRenderer[] ss= go.GetComponentsInChildren<SpriteRenderer>();


            
            foreach(SpriteRenderer m in ss)
            {
                bool tmp = IsVisibleFrom(m, mainCam);
                if (tmp)
                {
                    nakyvilla = tmp;
                    break;
                }
                else
                {
                    tmp = IsVisibleUseMargin(m.gameObject);
                    if (tmp)
                    {
                        nakyvilla = tmp;
                        break;
                    }
                    else
                    {
                        tmp = IsRendererVisibleWithMargin2D(m.gameObject);
                        if (tmp)
                        {
                            nakyvilla = tmp;
                            break;
                        }
                    }
                }
            
            }

            
            if (!nakyvilla)
            {
                nakyvilla = IsVisibleUseMargin(go);

                if (!nakyvilla)
                {
                    nakyvilla=IsRendererVisibleWithMargin2D(go);
                }





                if (nakyvilla)
                {
                   // Debug.Log("marginaalilla");
                }
                else
                {
                 //   Debug.Log("mEiilla");
                }
            }


            
        }
        else
        {
            nakyvilla = IsVisibleFrom(s, mainCam);

        }


        if (!nakyvilla)
        {
            Rigidbody2D r = go.GetComponent<Rigidbody2D>();
            if (r != null)
            {
                r.simulated = false;
            }
        }
        else
        {
            Rigidbody2D r = go.GetComponent<Rigidbody2D>();
            if (r != null)
            {
                r.simulated = true;
            }
        }
        viimeinentarkistustulos = nakyvilla;
        return nakyvilla;
    }


    public bool OnkoOkToimiaUusiKasitteleMyosChildienRigidBodytVanha(GameObject go)
    {



        if (go == null)
        {
            Debug.Log("OnkoOnOkToimiaUusi kutsuttu nullilla");
            return false;
        }


        onkoOkToimiaTarkistuksensyklilaskuri += Time.deltaTime;
        if (onkoOkToimiaTarkistuksensyklilaskuri >= onkoOkToimiaTarkistussykli)
        {
            onkoOkToimiaTarkistuksensyklilaskuri = 0.0f;
        }
        else
        {
            return viimeinentarkistustulos;
        }


        SpriteRenderer s = go.GetComponent<SpriteRenderer>();
        bool nakyvilla = false;
        if (s != null)
        {
            nakyvilla = IsPartiallyVisibleFrom(s, mainCam);
        }
        else
        {
            nakyvilla = !IsOffScreen();
        }

        if (!nakyvilla)
        {
            Rigidbody2D[] rigidbodies = go.GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D r in rigidbodies)
            {
                if (r != null)
                {
                    r.simulated = false;
                }
            }

        }
        else
        {
            Rigidbody2D[] rigidbodies = go.GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D r in rigidbodies)
            {
                if (r != null)
                {
                    r.simulated = true;
                }
            }
        }
        viimeinentarkistustulos = nakyvilla;
        return nakyvilla;
    }


    public bool OnkoOkToimiaUusiKasitteleMyosChildienRigidBodyt(GameObject go)
    {



        if (go == null)
        {
            Debug.Log("OnkoOnOkToimiaUusi kutsuttu nullilla");
            return false;
        }


        onkoOkToimiaTarkistuksensyklilaskuri += Time.deltaTime;
        if (onkoOkToimiaTarkistuksensyklilaskuri >= onkoOkToimiaTarkistussykli)
        {
            onkoOkToimiaTarkistuksensyklilaskuri = 0.0f;
        }
        else
        {
            return viimeinentarkistustulos;
        }


        SpriteRenderer s = go.GetComponent<SpriteRenderer>();
        bool nakyvilla = false;
        if (s != null)
        {
            nakyvilla = IsPartiallyVisibleFrom(s, mainCam);
        }
        else
        {
            nakyvilla = !IsOffScreen();
        }

        if (!nakyvilla)
        {
            Rigidbody2D[] rigidbodies = go.GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D r in rigidbodies)
            {
                if (r != null)
                {
                    r.simulated = false;
                }
            }

        }
        else
        {
            Rigidbody2D[] rigidbodies = go.GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D r in rigidbodies)
            {
                if (r != null)
                {
                    r.simulated = true;
                }
            }
        }
        viimeinentarkistustulos = nakyvilla;
        return nakyvilla;
    }



    private bool IsVisibleFrom(SpriteRenderer renderer, Camera camera)
    {
        if (!renderer || !camera)
            return false;

        // Get the world bounds of the sprite
        Bounds spriteBounds = renderer.bounds;

        // Calculate the camera's 2D bounds
        float camHeight = camera.orthographicSize * 2;
        float camWidth = camHeight * camera.aspect;

        Vector3 camPosition = camera.transform.position;

        Rect cameraRect = new Rect(
            camPosition.x - camWidth / 2,
            camPosition.y - camHeight / 2,
            camWidth,
            camHeight
        );

        // Check if the sprite bounds intersect the camera's bounds
        return cameraRect.Overlaps(new Rect(
            spriteBounds.min.x,
            spriteBounds.min.y,
            spriteBounds.size.x,
            spriteBounds.size.y
        ));
    }

    private bool IsPartiallyVisibleFrom3d(SpriteRenderer renderer, Camera camera)
    {
        if (!renderer || !camera)
            return false;

        Bounds spriteBounds = renderer.bounds;

        spriteBounds.extents *= 2.0f;

        float camHeight = camera.orthographicSize * 2;
        float camWidth = camHeight * camera.aspect;
        Vector3 camPosition = camera.transform.position;

        Bounds cameraBounds = new Bounds(
            camPosition,
            new Vector3(camWidth, camHeight, float.MaxValue) // Infinite Z range for 2D
        );

        return cameraBounds.Intersects(spriteBounds);
    }

    private bool IsPartiallyVisibleFrom(SpriteRenderer renderer, Camera camera)
    {
        if (!renderer || !camera)
            return false;

        Bounds spriteBounds = renderer.bounds;
        spriteBounds.extents *= 2.5f;

        // Sprite rect in 2D (X/Y only)
        Rect spriteRect = new Rect(
            spriteBounds.min.x,
            spriteBounds.min.y,
            spriteBounds.size.x,
            spriteBounds.size.y
        );

        // Get the center of the original rect
        Vector2 center = spriteRect.center;

        // Scale the size
        Vector2 newSize = spriteRect.size * 2.0f;// tätä muutetaan vain

        // Create a new rect centered at the same point
        spriteRect = new Rect(
            center - newSize / 2f,
            newSize
        );


        // Camera rect in 2D (X/Y only)
        float camHeight = camera.orthographicSize * 2f;
        float camWidth = camHeight * camera.aspect;
        Vector3 camPos = camera.transform.position;

        Rect cameraRect = new Rect(
            camPos.x - camWidth / 2f,
            camPos.y - camHeight / 2f,
            camWidth,
            camHeight
        );

        // Check if the 2D rectangles overlap
        bool ret = cameraRect.Overlaps(spriteRect);


        // Debug.Log("overlap"+ ret);

        if (ret)
        {
            Debug.Log(name);
        }

        return ret;
    }




    /*
    Dictionary<string, Vector3> GetAllWorldPositionsVanha(Transform parent)
    {
        Dictionary<string, Vector3> posDict = new Dictionary<string, Vector3>();

        // Add the parent itself
        posDict[parent.name] = parent.position;

        // Recurse through children
        foreach (Transform child in parent)
        {
            var childPositions = GetAllWorldPositions(child);
            foreach (var kvp in childPositions)
            {
                posDict[kvp.Key] = kvp.Value;
            }
        }

        return posDict;
    }
    */

    Dictionary<Transform, Vector3> GetAllWorldPositions(Transform parent)
    {
        Dictionary<Transform, Vector3> posDict = new Dictionary<Transform, Vector3>();
        CollectPositions(parent, posDict);
        return posDict;
    }

    void CollectPositions(Transform t, Dictionary<Transform, Vector3> dict)
    {
        dict[t] = t.position;
        foreach (Transform child in t)
        {
            CollectPositions(child, dict);
        }
    }




    private float viimeisinTarkistusAika = -1f;
    private float tarkistussykli = 0.1f;
    private bool viimeisin = false;
    public bool HoidaInterceptorTapaus(GameObject go)
    {
        if (go == null)
            return false;


        if (Time.time - viimeisinTarkistusAika < tarkistussykli)
            return viimeisin;

        // 2. Päivitetään viimeisin tarkistus
        viimeisinTarkistusAika = Time.time;

        GameObject targetRoot = go.transform.parent != null ? go.transform.parent.gameObject : go;

        // 1. Jos objekti on reilusti vasemmalla → tuhotaan
        if (OnkoKameranVasemmallaPuolella(go, 8.0f))
        {
            GameObject.Destroy(targetRoot);
            viimeisin = false;
            return viimeisin;
        }

        // 2. Tarkista onko hieman vasemmalla tai oikealla
        bool vasemmalla = OnkoKameranVasemmallaPuolella(go, 8.0f);
        bool oikealla = OnkoKameranOikeallaPuolella(go, 3.0f);

        // 3. Haetaan kaikki Rigidbody2D:t
        List<Rigidbody2D> rigidbodies = new List<Rigidbody2D>();
        rigidbodies.AddRange(targetRoot.GetComponentsInChildren<Rigidbody2D>());
        rigidbodies.AddRange(go.GetComponents<Rigidbody2D>());

        bool aktiivinen = !(vasemmalla || oikealla);


        foreach (Rigidbody2D rb in rigidbodies)
        {
            rb.simulated = aktiivinen;
        }
        viimeisin = aktiivinen;
        return aktiivinen;
    }



    public bool OnkoKameranVasemmassaReunassa(GameObject go, float percentKamerasta)
    {
        if (mainCam == null || go == null)
            return false;

        // Kamera-arvot
        float camHeight = mainCam.orthographicSize * 2f;
        float camWidth = camHeight * mainCam.aspect;
        Vector3 cameraMin = GetCameraMinWorldPosition(); // vasen alakulma

        // Lasketaan raja x-koordinaatille, jonka sisällä objekti on "vasemmassa reunassa"
        float rajaX = cameraMin.x + camWidth * percentKamerasta / 100.0f;

        // Objektin sijainti maailmakoordinaateissa
        SpriteRenderer sp = go.GetComponent<SpriteRenderer>();
        float objektinX;
        if (sp != null)
        {
            Bounds bounds = sp.bounds;
            objektinX = bounds.center.x;
        }
        else
        {
            objektinX = go.transform.position.x;
        }

        return objektinX <= rajaX;
    }



    public bool OnkoKameranOikeallaPuolella(GameObject go)
    {
        return OnkoKameranOikeallaPuolella(go, 0.1f);
    }

    public bool OnkoKameranVasemmallaPuolella(GameObject go, float offset)
    {
        Camera cam = mainCam;

        // Calculate the left edge of the camera in world space

        if (cam == null)
        {
            Debug.Log("cami nuli");
        }

        float cameraLeftX = cam.transform.position.x - cam.orthographicSize * cam.aspect;

        // Get the object's visual bounds
        SpriteRenderer renderer = go.GetComponentInChildren<SpriteRenderer>();
        if (renderer == null)
            return false; // No visual bounds

        float objectRightX = renderer.bounds.max.x;

        // Check if the object's right edge is completely to the left of the camera view + offset
        return objectRightX < cameraLeftX - offset;
    }



    public bool OnkoKameranVasemmallaPuolellaVanha(GameObject go, float offset)
    {
        Vector3 kameraminimi = GetCameraMinWorldPosition();
        //Dictionary<string, Vector3> positions = GetAllWorldPositions(transform);

        Dictionary<Transform, Vector3> positions = GetAllWorldPositions(go.transform);

        //eli kaikki pitää olla vasemmalla jolloin true, muuten false
        foreach (var entry in positions)
        {

            //Debug.Log($"{entry.Key}: {entry.Value}");
            if (entry.Value.x < kameraminimi.x)
            {
                float ero = Mathf.Abs(kameraminimi.x - entry.Value.x);
                if (ero >= offset)
                {
                    //return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        return true;

    }

    public bool OnkoKameranOikeallaPuolella(GameObject go, float offset)
    {
        Camera cam = mainCam;

        // Calculate the right edge of the camera in world space
        float cameraRightX = cam.transform.position.x + cam.orthographicSize * cam.aspect;

        // Get the object's Renderer (includes children)
        Renderer renderer = go.GetComponentInChildren<Renderer>();
        if (renderer == null)
            return false; // No visible parts

        float objectLeftX = renderer.bounds.min.x;

        // Check if the object is fully to the right of the camera by offset
        return objectLeftX > cameraRightX + offset;
    }



    public bool OnkoKameranOikeallaPuolellaVanha(GameObject go, float offset)
    {
        Vector3 kameraminimi = GetCameraMaxWorldPosition();

        //Dictionary<string, Vector3> positions = GetAllWorldPositions(go.transform);

        Dictionary<Transform, Vector3> positions = GetAllWorldPositions(go.transform);



        //eli kaikki pitää olla vasemmalla jolloin true, muuten false
        foreach (var entry in positions)
        {
            //Debug.Log($"{entry.Key}: {entry.Value}");
            if (entry.Value.x > kameraminimi.x)
            {
                float ero = Mathf.Abs(kameraminimi.x - entry.Value.x);
                if (ero >= offset)
                {
                    // return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }
        return true;

    }


    private bool OnkoKameranAlaPuolella(GameObject go, float offset)
    {
        Camera cam = mainCam;

        // Get the bottom Y of the camera in world space
        float cameraBottomY = cam.transform.position.y - cam.orthographicSize;

        // Get the Renderer (e.g., SpriteRenderer)
        Renderer renderer = go.GetComponentInChildren<Renderer>();
        if (renderer == null)
            return false; // No visible parts to evaluate

        float objectTopY = renderer.bounds.max.y;

        // Return true if the whole object is below the bottom of the camera by offset
        return objectTopY < cameraBottomY - offset;
    }

    private bool OnkoKameranAlaPuolellaVanha(GameObject go, float offset)
    {
        Vector3 kameraminimi = GetCameraMinWorldPosition();
        //Dictionary<string, Vector3> positions = GetAllWorldPositions(go.transform);

        Dictionary<Transform, Vector3> positions = GetAllWorldPositions(go.transform);


        foreach (var entry in positions)
        {

            //Debug.Log($"{entry.Key}: {entry.Value}");
            if (entry.Value.y < kameraminimi.y)
            {
                float ero = Mathf.Abs(kameraminimi.y - entry.Value.y);
                if (ero >= offset)
                {
                    // return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }
        return true;

    }


    private bool OnkoKameranYlaPuolella(GameObject go, float offset)
    {
        Camera cam = mainCam;

        // Get the top Y of the camera in world space
        float cameraTopY = cam.transform.position.y + cam.orthographicSize;

        // Try to get the Renderer bounds
        Renderer renderer = go.GetComponentInChildren<Renderer>();
        if (renderer == null)
            return false; // No visible bounds to check

        float objectBottomY = renderer.bounds.min.y;

        return objectBottomY > cameraTopY + offset;
    }



    private bool OnkoKameranYlaPuolellaVanha(GameObject go, float offset)
    {
        Vector3 kameramax = GetCameraMaxWorldPosition();
        //Dictionary<string, Vector3> positions = GetAllWorldPositions(go.transform);

        Dictionary<Transform, Vector3> positions = GetAllWorldPositions(go.transform);


        foreach (var entry in positions)
        {

            //Debug.Log($"{entry.Key}: {entry.Value}");
            if (entry.Value.y > kameramax.y)
            {
                float ero = Mathf.Abs(kameramax.y - entry.Value.y);
                if (ero >= offset)
                {
                    // Debug.Log("offsetti=" + offset);
                    // return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }
        return true;

    }

    private float syklilaskuri = 0.0f;
    // private float tarkistussykli = 5.0f;
    public float hengissaoloaika = 0.0f;//tämä siis mittaa sitä hengissäoloaikaa
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
        bool tuhoajosoikeallakameraanVerrattuna,
        GameObject prefap
        )
    {

        if (go == null)
        {
            Debug.Log("no nulli");
        }
        hengissaoloaika += Time.deltaTime;
        syklilaskuri += Time.deltaTime;
        /*
        if (go.GetComponent<MakitaVihollinenAmmusScripti>() != null)
        {
            Rigidbody2D r = go.GetComponent<Rigidbody2D>();
            if (r != null)
            {
                float speed = r.velocity.magnitude;
            //    Debug.Log("ammusnopesu=" + speed);
            }
        }
        */
        if (syklilaskuri >= tarkistussykli)
        {
            /*
            if (prefap == null)
            {
                prefap = GetPrefap();
            }
            */

            /*
            syklilaskuri = 0.0f;
            hengissaoloaika += tarkistussykli;
            */
            if (tuhoajosliiankauanhengissa && hengissaoloaika >= hengissaolonraja)
            {
                
                    BaseDestroy();
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
                      
                            BaseDestroy();
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
                    /*
                    if (prefap == null)
                    {
                        */
                        BaseDestroy();
                    return;
                    /*
                        //Destroy(go);

                        return;
                    }
                    else
                    {
                        hengissaoloaika = 0.0f;
                        ObjectPoolManager.Instance.ReturnToPool(prefap, go);
                        return;
                    }
                    */



                }

            }
            if (tuhoajosoikeallakameraanVerrattuna)
            {
                if (OnkoKameranOikeallaPuolella(go))
                {
                        //Destroy(go);
                        BaseDestroy();
                        return;
                    
                }
            }
            if (tuhoaJosKameranAlapuolella)
            {
                if (OnkoKameranAlaPuolella(go, offsettijokasallitaanAlhaallaKavaisyyn))
                {
                        BaseDestroy();
                    return;

                }
            }
            if (tuhoaJosKameranYlapuolella)
            {
                if (OnkoKameranYlaPuolella(go, offsettijokasallitaanYlapuolellaKavaisyyn))
                {
                        BaseDestroy();
                    return;

                }
            }


        }
    }



    //private Texture2D image; // The source image
    public int uusirajaytyscolumns = 8;  // Number of horizontal slices
    public int uusirajaytysrows = 8;     // Number of vertical slices
                                         //private Vector2 startPosition; // The original position of the SpriteRenderer



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

    public static string[] tagitjotkatuhoaaAluksen = { "pyoroovi", "tiili", "pyoroovi"
    , "pyoroovi", "laatikkovihollinenexplodetag", "pallovihollinen", "vihollinen", "eituhvih"
    };


    public static bool TuhoaakoAluksen(string tag)
    {
        foreach (string s in tagitjotkatuhoaaAluksen)
        {
            if (tag.Contains(s))
            {
                return true;
            }

        }
        return false;
    }


    public static string[] tagitjotkaAluksenShieldiTuhoaa = { "Apple", "Banana", "Cherry" };


    /*
    public static float GetKameranSkrolliMaara()
    {

        return mainCam.GetComponent<Kamera>().skrollimaara;

    }
    */

    public static Vector2 PalautaKaikkienCollidereidenKeskipiste(GameObject go)
    {
        Collider2D[] allColliders = go.GetComponents<Collider2D>();

        if (allColliders.Length == 0)
        {
            Debug.Log("No 2D colliders found in the scene." + go);
            return Vector2.zero;
        }

        // Sum of all collider positions
        Vector2 sum = Vector2.zero;

        foreach (Collider2D col in allColliders)
        {
            sum += (Vector2)col.bounds.center; // or col.transform.position if you prefer object pivot
        }

        // Calculate center point
        Vector2 centerPoint = sum / allColliders.Length;

        // Debug.Log("Center point of all 2D colliders: " + centerPoint);
        return centerPoint;
    }

    public float GetKorjattuArvoPerustuenSkrollimaaraanTamaSiksiEttaAmmusLentaaNopeamminVasemmaltaOikealleSilloinKunKameraLiikkuuNopeammin(float alkuperainenNopeus)
    {
        return
                    mainCam.GetComponent<Kamera>().GetKorjattuArvoPerustuenSkrollimaaraanTamaSiksiEttaAmmusLentaaNopeamminVasemmaltaOikealleSilloinKunKameraLiikkuuNopeammin(alkuperainenNopeus, 1.5f);



    }

    public void IgnoraaCollisiotVihollistenValilla(GameObject cp, GameObject c)
    {
        cp = FindRootWithTagContaining(cp, "vihollinen");
        c = FindRootWithTagContaining(c, "vihollinen");
        if (cp == null || c == null || cp == c) return;
        Collider2D[] allColliders = cp.GetComponentsInChildren<Collider2D>();

        Collider2D[] allColliders2 = c.GetComponentsInChildren<Collider2D>();

        foreach (Collider2D c1 in allColliders)
        {
            foreach (Collider2D c2 in allColliders2)
            {
                //Physics.IgnoreCollision
                Physics2D.IgnoreCollision(c1, c2);


            }
        }
    }

    public void IgnoraaCollisiotVihollistenValillaALakasittelevihollinen(GameObject cp, GameObject c)
    {
        if (cp == null || c == null || cp == c) return;
        Collider2D[] allColliders = cp.GetComponentsInChildren<Collider2D>();

        Collider2D[] allColliders2 = c.GetComponentsInChildren<Collider2D>();

        foreach (Collider2D c1 in allColliders)
        {
            foreach (Collider2D c2 in allColliders2)
            {
                //Physics.IgnoreCollision
                Physics2D.IgnoreCollision(c1, c2);


            }
        }
    }



    public void IgnoraaChildienCollisiot()
    {
        Collider2D[] allColliders = GetComponentsInChildren<Collider2D>();

        foreach (Collider2D c in allColliders)
        {
            foreach (Collider2D b in allColliders)
            {
                Physics2D.IgnoreCollision(c, b);

            }
        }
    }




    private bool isGoingToBeDestroyed = false;



    public void BaseDestroy()
    {
        isGoingToBeDestroyed = true;

        //Destroy(gameObject);
        BaseDestroy(gameObject);
    }

    public bool IsGoingToBeDestroyed()
    {
        return isGoingToBeDestroyed;
    }

    public void SetDestroy()
    {
        SetGoingToBeDestroyed(true);
    }

    public void SetGoingToBeDestroyed(bool val)
    {
        isGoingToBeDestroyed = val;
    }

    public void BaseDestroy(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();
        if (bc != null)
        {
            bc.isGoingToBeDestroyed = true;

        }

        if (go.GetComponent<PooledObject>() != null && go.GetComponent<PoolNotAble>() == null)
        {
            ObjectPoolManager.Instance.ReturnToPool( go);
            return;

        }
        else
        {
            Destroy(go);
        }
       
    }



    public void BaseDestroy(GameObject go, float time)
    {
        BaseController bc = go.GetComponent<BaseController>();
        if (bc != null)
        {
            bc.isGoingToBeDestroyed = true;
        }

        if (go.GetComponent<PooledObject>() != null && go.GetComponent<PoolNotAble>()==null)
        {
            ObjectPoolManager.Instance.ReturnToPool(go,time);
            return;
        }
        else
        {
            Destroy(go, time);
        }
    }

    public virtual void OnDestroyPoolinlaittaessa()
    {

    }


    private Dictionary<FieldInfo, object> _initialValues;

    public void SaveInitialState()
    {
        _initialValues = new Dictionary<FieldInfo, object>();

        // Haetaan kaikki kentät (myös private/protected)
        var fields = GetType().GetFields(
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        foreach (var field in fields)
        {
            // tallennetaan alkuarvo
            _initialValues[field] = field.GetValue(this);
        }
    }


    public int poolistapalautusmaara = 0;
    public void ResetState()
    {
        int originellipoolistapalautusmaara = poolistapalautusmaara;
        if (_initialValues == null) return;

        foreach (var kvp in _initialValues)
        {
            if (kvp.Value!=null) 
                kvp.Key.SetValue(this, kvp.Value);
        }
        poolistapalautusmaara = originellipoolistapalautusmaara;

        //@todoo
        /*
        Collider2D[] c=
        GetComponentsInChildren<Collider2D>();

        foreach(Collider2D cc in c)
        {
            cc.enabled = true;
        }
        */
    }

    private Camera mainCam;

    void Awakevanha()
    {
        mainCam = Camera.main;
   
    }

    // Käytetään Awake:ssa automaattisesti tallennukseen
    protected virtual void Awake()
    {
        mainCam = Camera.main;
        SaveInitialState();
    }

    public Camera PalautaMainCam()
    {
        return mainCam;
    }

    public bool registeroiOnEnablessa = false;

    void OnEnable()
    {
        if (registeroiOnEnablessa)
        {
            FindObjectOfType<CameraObjectManager>().Register(gameObject);
        }
      
    }
}
