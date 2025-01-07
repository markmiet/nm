using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapControllerri : MonoBehaviour
{
    public Color newColor = Color.red; // Set the desired color in the Inspector or code.

    private Tilemap tilemap;
    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        if (tilemap != null)
        {
      //      tilemap.color = newColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
