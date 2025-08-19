using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceController : BaseController
{
    // Start is called before the first frame update
   // private Rigidbody2D rigidbody2D;
    void Start()
    {
     //   rigidbody2D =
    //    GetComponent<Rigidbody2D>();
      //   randomValue = Random.Range(0.2f, 0.6f);
    }
    /*
    float randomValue;
    private bool rigib = true;

    private float aikaraja = 0.05f;//talloin rigid
    private float aikarajaJolloinEirigid = 0.4f;

    private float laskuri = 0.0f;
    // Update is called once per frame

    private bool odotettu = false;
    void QQQQQQQQQQQQQQQFixedUpdate()
    {
        laskuri += Time.deltaTime;

        if (!odotettu && laskuri<randomValue)
        {
            return;
        }
        if (!odotettu && laskuri >= randomValue)
        {
            odotettu = true;
            return;
        }
        if (rigib && laskuri > aikaraja)  {
            

                rigib = !rigib;
                rigidbody2D.simulated = rigib;

                laskuri = 0;
            
        }
        else if (!rigib && laskuri > aikarajaJolloinEirigid)
        {

            rigib = !rigib;
            rigidbody2D.simulated = rigib;

            laskuri = 0;

        }




    }
    */

    private bool ammukseentormatty = false;

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (IsGoingToBeDestroyed())
        {
            return;
        }
        if (!ammukseentormatty && collision.gameObject.tag.Contains("ammus")) {
            ammukseentormatty = true;
            RajaytaSprite(gameObject, 2, 2);
            //Destroy(gameObject);
            BaseDestroy();
        }
        if (!ammukseentormatty && collision.gameObject.tag.Contains("pallovihollinen"))
        {
            ammukseentormatty = true;
            RajaytaSprite(gameObject, 2, 2);
            //Destroy(gameObject);
            BaseDestroy();
        }

    }

    public void RajaytaSprite(GameObject go, int rows, int columns)
    {
        SpriteRenderer originalRenderer = go.GetComponent<SpriteRenderer>();
        if (originalRenderer == null || originalRenderer.sprite == null)
        {
            Debug.LogError("No SpriteRenderer or Sprite found!");
            BaseDestroy();
            return;
        }



        Texture2D image = originalRenderer.sprite.texture;
        //originalRenderer.enabled = false; // Hide the original image

        Vector2 startPosition = go.transform.position;

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
                // Fix texture bleeding by expanding the rect by 1 pixel in all directions
                int extraPixel = 0;
                int invertedY = (rows - 1 - y) * pixelHeight;
                Rect rect = new Rect(
                    Mathf.Max(0, x * pixelWidth - extraPixel),
                    Mathf.Max(0, invertedY - extraPixel),
                    Mathf.Min(pixelWidth + extraPixel * 2, image.width - x * pixelWidth),
                    Mathf.Min(pixelHeight + extraPixel * 2, image.height - invertedY)
                );

                //Sprite sprite = Sprite.Create(image, rect, new Vector2(0.5f, 0.5f), image.width / spriteWorldSize.x);

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
                //newObject.layer = LayerMask.NameToLayer("AlusammusLayer");
                //newObject.layer = LayerMask.NameToLayer("AlusLayer");
                //newObject.layer = LayerMask.NameToLayer("Default");
                //newObject.tag = "haukivihollinenexplodetag";
                SpriteRenderer renderer = newObject.AddComponent<SpriteRenderer>();
                renderer.sprite = sliceSprite;

                // Add BoxCollider2D
                if (level < 1)
                {
                    BoxCollider2D collider = newObject.AddComponent<BoxCollider2D>();
                    collider.size = new Vector2(sliceWidth, sliceHeight);


                }
                else
                {
                    Debug.Log("level=" + level);
                }



                //newObject.excludeLayers(LayerMask.NameToLayer("BonusLayer"));


                // Add Rigidbody2D (no gravity)
                 Rigidbody2D rb = newObject.AddComponent<Rigidbody2D>();
                 rb.gravityScale = 0.5f; // Disable gravity so pieces don't fall
               // if (level < 2)
               // {
                    ApplyExplosionForce(rb);
               // }
                // Calculate local position (perfect alignment)
                float posX = startPosition.x - (spriteWorldSize.x / 2) + (x * sliceWidth) + (sliceWidth / 2);
                float posY = startPosition.y + (spriteWorldSize.y / 2) - (y * sliceHeight) - (sliceHeight / 2);

                // Set position to exactly match original image
                newObject.transform.position = new Vector2(posX, posY);
                SliceController s=
                newObject.AddComponent<SliceController>();
                s.level = level + 1;
                // newObject.AddComponent<SliceController>();
                //Destroy(newObject, 4);
                BaseDestroy(newObject, 4);

            }
        }
    }

    public int level = 0;

    private void ApplyExplosionForce(Rigidbody2D rb)
    {
        // Random force parameters
        float explosionForce = 5f; // The strength of the force
        float explosionRadius = 2f; // The radius in which the force is applied
        Vector2 explosionDirection = Random.insideUnitCircle.normalized; // Random direction for the force

        // Apply explosion force
        rb.AddForce(explosionDirection * explosionForce, ForceMode2D.Impulse);

        // Optional: Apply some randomness to simulate more chaotic explosion (e.g. slight rotation)
        rb.angularVelocity = Random.Range(-50f, 50f); // Random rotational velocity for effect
    }
}