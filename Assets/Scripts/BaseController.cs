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

    public bool onkoEroa(Vector3 worldPosition,int positionumeroJohonVerrataan, bool muutaworldpositionScreenpositionin)
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


        Vector3 listassa= aluksenpositiotCameraViewissa[positionumeroJohonVerrataan];

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

        if ( screenPosition.x < listassa.x)
        {
            float erotus = Mathf.Abs(screenPosition.x - listassa.x);
            if (erotus> vaadittavaero)
            {
                return true;
            }
        }
        return false;

    }
    public bool onkoliikkunutOikealle(Vector3 worldPosition, int positionumeroJohonVerrataan, bool muutaworldpositionScreenpositionin,float vaadittavaero)
    {
        if (aluksenpositiotCameraViewissa.Count<positionumeroJohonVerrataan+1)
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
    public void tallennaSijaintiSailytaVainNkplViimeisinta(int nkpl,bool muutaworldpositionScreenpositionin, bool tallennavainyksiloivatArvot)
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
}