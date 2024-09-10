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
    public void tallennaSijaintiSailytaVainNkplViimeisinta(int nkpl, bool muutaworldpositionScreenpositionin, bool tallennavainyksiloivatArvot)
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
        for (int i=0;i< bonuksienmaara;i++)
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
}