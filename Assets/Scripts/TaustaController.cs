using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TaustaController : BaseController
{
    //public float parallaxFactor = 0.5f;  // The speed factor for the background movement

    private Tilemap tilemap;  // Reference to the Tilemap component
    public TileBase[] tile;    // The tile you want to set


    //private Vector3 lastCameraPosition;
    public float ysaato = 0.0f;
    public float xsaato = 0.0f;
    public int todennakoisyysettatileluodaan = 0;
    public int sarakkeidenmaara = 1000;
    public int rivienmaara = 10;

    /*
    private Tilemap tilemap;

    public CustomTile customTile; // Assign your CustomTile in the Inspector

    private GameObject alus;
    public float scaleSpeed = 0.0001f; // Speed at which the tile scales
    public float minScale = 0.5f; // Minimum scale value
    public float maxScale = 1.5f; // Maximum scale value

    private float currentScale = 1f; // Track the current scale
    private bool scalingUp = true; // Flag to control scale direction


    private float[][] xy;//offsetx
    private float[][] xyoffsety;//offsety
    */

    void Start()
    {

      //  Debug.Log("aksaa=" + transform.position.x);
        transform.position = new Vector2(transform.position.x+xsaato, ysaato);
        tilemap = GetComponent<Tilemap>();
       // GetComponent<TilemapRenderer>().sortingOrder = -3;
       // GetComponent<TilemapRenderer>().sortingLayerName = "KeskiLayer";

        if (tilemap != null)
        {
            //Debug.Log("Tilemap found.");
        }
        else
        {
            Debug.LogWarning("Tilemap not found.");
        }
        //lastCameraPosition = Camera.main.transform.position;

       


        // Cache the camera's transform for efficiency
        cameraTransform = Camera.main.transform;

        // Initialize the last camera position
        lastCameraPosition = cameraTransform.position;

        TeeRandomit();

    }

    public void TeeRandomit()
    {
       // transform.position = new Vector2(xsaato, ysaato);

        if (tile != null && tile.Length > 0)
        {
            TyhjaaKaikkiTilet(tilemap);
            for (int x = 0; x < sarakkeidenmaara; x++)
            {
                bool edellisellaluotiin = false;
                for (int y = 0; y < rivienmaara; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    //float r=Random.Range(0, 10);
                    int randomNumber = Random.Range(0, 100);
                    if (/*!edellisellaluotiin &&*/ randomNumber < todennakoisyysettatileluodaan)
                    {
                        //0,1,2,3
                        int tiili = Random.Range(0, tile.Length);

                        tilemap.SetTile(tilePosition, tile[tiili]);
                        edellisellaluotiin = true;
                    }
                    else
                    {
                        edellisellaluotiin = false;
                    }
                   
                }
            }
        }
    }

    private void TyhjaaKaikkiTilet(Tilemap map)
    {
        List<TileBase> tiles = new List<TileBase>();

        // Get the bounds of the Tilemap
        BoundsInt bounds = map.cellBounds;

        // Iterate through each cell in the bounds
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                for (int z = bounds.zMin; z < bounds.zMax; z++)
                {
                    Vector3Int pos = new Vector3Int(x, y, z);

                    // Get the tile at the current cell position
                    TileBase tile = map.GetTile(pos);

                    if (tile != null)
                    {
                        //tiles.Add(tile);  // Add to list if the tile exists

                        tilemap.SetTile(pos, null);
                    }
                }
            }
        }

    }


    public float parallaxFactor = 0.5f; // Lower values for slower movement (further away)

    private Vector3 lastCameraPosition; // Tracks the camera's last position
    private Transform cameraTransform; // Cached reference to the camera's transform

    /*
    void Start()
    {
        // Cache the camera's transform for efficiency
        cameraTransform = Camera.main.transform;

        // Initialize the last camera position
        lastCameraPosition = cameraTransform.position;
    }
    */

    void Update()
    {
        
        // Calculate the camera's movement since the last frame
        Vector3 cameraMovement = cameraTransform.position - lastCameraPosition;

        // Move the background in proportion to the camera movement
        transform.position += new Vector3(cameraMovement.x * parallaxFactor, cameraMovement.y * parallaxFactor, 0);

        // Update the last camera position for the next frame
        lastCameraPosition = cameraTransform.position;
        
    }
}
