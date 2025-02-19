using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoKuvaController : MonoBehaviour
{
    private Texture2D image; // The source image
    public int columns = 10;  // Number of horizontal slices
    public int rows = 10;     // Number of vertical slices
    private Vector2 startPosition; // The original position of the SpriteRenderer

    void Start()
    {
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
        float sliceWidth = spriteWorldSize.x / columns;
        float sliceHeight = spriteWorldSize.y / rows;

        // Get individual slice sizes in pixels
        int pixelWidth = image.width / columns;
        int pixelHeight = image.height / rows;

        // Loop through rows and columns
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                // Fix texture bleeding by expanding the rect slightly to ensure there is no gap between slices
                //int extraPixel = 1;  // No need for extra pixel expansion, this can be adjusted if needed
                int extraPixel = 1;  // No need for extra pixel expansion, this can be adjusted if needed

                int invertedY = (rows - 1 - y) * pixelHeight;

                Rect rect = new Rect(
                    Mathf.Max(0, x * pixelWidth - extraPixel),  // Starting X position
                    Mathf.Max(0, invertedY - extraPixel),        // Starting Y position (inverted for the image coordinates)
                    Mathf.Min(pixelWidth + extraPixel * 2, image.width - x * pixelWidth),  // Ensure width fits within bounds
                    Mathf.Min(pixelHeight + extraPixel * 2, image.height - invertedY)    // Ensure height fits within bounds
                );

                if (!HasNonTransparentPixels(rect))
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
                Rigidbody2D de=
                newObject.AddComponent<Rigidbody2D>();
                de.gravityScale = 1f;
                

                // Optionally, add additional logic or components as needed
                newObject.AddComponent<SliceController>();
                //previous = PalautaRandomiListasta();
                if (previous != null)
                {
                    HingeJoint2D joint = newObject.AddComponent<HingeJoint2D>();
                    joint.connectedBody = previous.GetComponent<Rigidbody2D>();
                    joint.breakForce = 150.0f;
                  
                }

                if (previous != null)
                {
               //     DistanceJoint2D joint = newObject.AddComponent<DistanceJoint2D>();
                 //   joint.connectedBody = previous.GetComponent<Rigidbody2D>();
                }

                previous = newObject;
                lista.Add(newObject);

            }
        }
    }

    private GameObject PalautaRandomiListasta()
    {
        if (lista!=null && lista.Count>=1 )
        {
            int randomNumber = Random.Range(0, lista.Count);
            return lista[randomNumber];
        }
        return null;
    }

    private GameObject previous;
    private List<GameObject> lista = new List<GameObject>();

    public int transparentpixelcount = 100;
    // Helper method to check if there are any non-transparent pixels in the slice
    private bool HasNonTransparentPixels(Rect rect)
    {

        int maara = 0;
        // Get the pixels in the specified area
        Color[] pixels = image.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);

        // Check each pixel to see if it's non-transparent
        foreach (var pixel in pixels)
        {
            if (pixel.a > 0) // If alpha is greater than 0, the pixel is not fully transparent
            {
                //return true;
                maara++;
            }
            if (maara>= transparentpixelcount)
            {
                return true;
            }
        }

        // If all pixels are transparent, return false
        return false;
    }
}
