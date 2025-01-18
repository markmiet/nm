using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SormiKameraController : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera sormikamera;
    void Start()
    {
        sormikamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float PalautaOhjausKameranMinY()
    {
        //float screenHeightInWorld = 2 * Camera.main.orthographicSize; // Total height
        //float bottomY = Camera.main.transform.position.y - Camera.main.orthographicSize; // Bottom of the screen
        //return bottomY;

        float r = GetOhjausCameraMinWorldPosition().y;
        return r;
    }

    public float PalautaOhjausKameranMinX()
    {
        //float screenHeightInWorld = 2 * Camera.main.orthographicSize; // Total height
        //float bottomY = Camera.main.transform.position.y - Camera.main.orthographicSize; // Bottom of the screen
        //return bottomY;

        float r = GetOhjausCameraMinWorldPosition().x;
        return r;
    }


    public Vector3 GetOhjausCameraMinWorldPosition()
    {
        if (sormikamera ==null)
        {
            sormikamera = GetComponent<Camera>();
        }

        // Calculate the camera's dimensions in world space
        float height = sormikamera.orthographicSize * 2;
        float width = height * sormikamera.aspect;

        // Bottom-left corner of the camera's view in world space
        Vector3 minWorldPosition = sormikamera.transform.position - new Vector3(width / 2, height / 2, 0);

        return minWorldPosition;
    }


    public float PalautaOhjausKameranMaxY()
    {
        float r = GetOhjausCameraMaxWorldPosition().y;
        return r;
    }

    public Vector3 GetOhjausCameraMaxWorldPosition()
    {
        // Calculate the camera's dimensions in world space
        float height = sormikamera.orthographicSize * 2;
        float width = height * sormikamera.aspect;

        // Top-right corner of the camera's view in world space
        Vector3 maxWorldPosition = sormikamera.transform.position + new Vector3(width / 2, height / 2, 0);

        return maxWorldPosition;
    }


    public float PalautaOhjausKameranKorkeus()
    {
        float screenHeightInWorld = 2 * sormikamera.orthographicSize; // Total height
        return screenHeightInWorld;
    }

    public float PalautaOhjausalueekoko()
    {
        float a = PalautaOhjausalueenYlarajaY();
        float b = PalautaOhjausalueenAlarajaY();
        return Mathf.Abs(a - b);

        // return sormiKameraController.PalautaOhjausalueekoko();

    }


    public float PalautaOhjausalueenAlarajaY()
    {
        float r = PalautaOhjausKameranMinY() + (prosenttiosuusalasuoja / 100.0f) * PalautaOhjausKameranKorkeus();
        return r;
    }
    public float PalautaOhjausalueenAlarajaX()
    {
        float r = PalautaOhjausKameranMinX();// + (prosenttiosuusalasuoja / 100.0f) * PalautaKameranKorkeus();
        return r;
    }
    

    public float PalautaOhjausalueenYlarajaY()
    {
        float r =
            PalautaOhjausKameranMinY();
        float a = ((prosenttiosuusalasuoja + prosenttiosuusmikaonvarattuohjaukseenkorkeudessa) / 100.0f) * PalautaOhjausKameranKorkeus();



        return r + a;
    }


    private float prosenttiosuusalasuoja = 5.0f;
    private float prosenttiosuusmikaonvarattuohjaukseenkorkeudessa = 15.0f;

    public void SetOsuudet(float p_prosenttiosuusalasuoja, float p_prosenttiosuusmikaonvarattuohjaukseenkorkeudessa,
        Camera paakamera)
    {
        if (paakamera == null)
        {
            Debug.Log("paakamera on nulli");
        }
        prosenttiosuusalasuoja = p_prosenttiosuusalasuoja;
        prosenttiosuusmikaonvarattuohjaukseenkorkeudessa = p_prosenttiosuusmikaonvarattuohjaukseenkorkeudessa;

        float yht = (prosenttiosuusalasuoja + prosenttiosuusmikaonvarattuohjaukseenkorkeudessa) / 100.0f;


        // / Bottom camera(50 % height, bottom of the screen)
        sormikamera.rect = new Rect(0, 0, 1, yht);

        paakamera.rect = new Rect(0, yht, 1, 1);
        //
        float kerroin = 100.0f / (prosenttiosuusalasuoja + prosenttiosuusmikaonvarattuohjaukseenkorkeudessa);
        prosenttiosuusalasuoja = kerroin * prosenttiosuusalasuoja;
        prosenttiosuusmikaonvarattuohjaukseenkorkeudessa = kerroin * prosenttiosuusmikaonvarattuohjaukseenkorkeudessa;
        Debug.Log("prosenttiosuusalasuoja=" + prosenttiosuusalasuoja + " prosenttiosuusmikaonvarattuohjaukseenkorkeudessa=" +
            prosenttiosuusmikaonvarattuohjaukseenkorkeudessa);


    }

    void OnDrawGizmos()
    {
        Debug.DrawRay(new Vector3(-1000, PalautaOhjausalueenAlarajaY(), 0), Vector2.right * 10000.0f, Color.blue);
        Debug.DrawRay(new Vector3(-1000, PalautaOhjausalueenYlarajaY(), 0), Vector2.right * 10000.0f, Color.red);

    }
}