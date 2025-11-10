using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SpriteGridSplitter : MonoBehaviour
{
    /*
    [Tooltip("Destroy the original object after creating the pieces.")]
    public bool destroyOriginal = true;

    [Tooltip("Parent all pieces under the original object for easy management.")]
    public bool parentPieces = true;
    */

    [Tooltip("How many rows to split into.")]
    public int rows = 5;

    [Tooltip("How many columns to split into.")]
    public int cols = 5;


    public RigidbodyType2D bodytype = RigidbodyType2D.Static;
    public float gravityscale = 0.0f;

    public int hitcount = 5;
    public bool teerajaytasprite = false;
    public bool teerajaytaspriteuusiversio = false;

    public int uusirajaytyscolumns = 2;
    public int uusirajaytysrows = 2;
    public float rajaytaspritenScaleFactorProsentti = 10f;
    public float alivetime = 0.1f;

    //todoo tämä siihen matomaailmaan....
    //hitcounter pieneksi ja paljon....
    public GameObject explosion;//joku buff räjähdys peittämään 

    //tämä myös käyttöön siihen kun kk ampuu alhaalta seiniin niin ne hajoaakin alta pois..
    //mutta ei liikaa
    

    [ContextMenu("Split Now")]
    public void SplitIntoGrid()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (!sr || sr.sprite == null)
        {
            Debug.LogError("SpriteGridSplitter: No SpriteRenderer/Sprite found.");
            return;
        }

        Sprite originalSprite = sr.sprite;
        Texture2D tex = originalSprite.texture;

        // Pixel rect of the sprite within its texture
        Rect spriteRect = originalSprite.rect;
        int sx = Mathf.RoundToInt(spriteRect.x);
        int sy = Mathf.RoundToInt(spriteRect.y);
        int sw = Mathf.RoundToInt(spriteRect.width);
        int sh = Mathf.RoundToInt(spriteRect.height);

        // Pixels Per Unit and pivot (in pixels relative to spriteRect)
        float ppu = originalSprite.pixelsPerUnit;
        Vector2 pivotPx = originalSprite.pivot;

        // Split dimensions so the sum matches exactly
        var colWidths = Partition(sw, cols);
        var rowHeights = Partition(sh, rows);

        // Copy render settings
        var baseMaterial = sr.sharedMaterial;
        var sortingLayerID = sr.sortingLayerID;
        var sortingOrder = sr.sortingOrder;
        var color = sr.color;
        var flipX = sr.flipX;
        var flipY = sr.flipY;

        int pieceIndex = 0;
        int yCursor = sy;

        for (int r = 0; r < rows; r++)
        {
            int h = rowHeights[r];
            int xCursor = sx;

            for (int c = 0; c < cols; c++)
            {
                int w = colWidths[c];
                var subRect = new Rect(xCursor, yCursor, w, h);

                // Pivot inside the sub-rect
                Vector2 pivotInSubPx = pivotPx - new Vector2(xCursor - sx, yCursor - sy);
                Vector2 pivotInSubNormalized = new Vector2(
                    Mathf.Clamp01(pivotInSubPx.x / w),
                    Mathf.Clamp01(pivotInSubPx.y / h)
                );

                int maaraaa= CountNonAlphaPixelcount(tex, xCursor, yCursor, w, h);
               // Debug.Log("maaraaa=" + maaraaa);
                if (maaraaa< alpharaja)
                {
                    xCursor += w;
                    continue;
                }


                // Create sub-sprite referencing the SAME texture
                Sprite subSprite = Sprite.Create(
                    tex,
                    subRect,
                    pivotInSubNormalized,
                    ppu,
                    0,
                    SpriteMeshType.FullRect
                );
                /*
                int maara =
                CountNonAlphaPixels2(subSprite);
                Debug.Log("maara=" + maara);

               
                if (maara < 50)
                {
                    Destroy(subSprite);
                    xCursor += w;

                    continue;

                }
                */
                
                

                subSprite.name = $"{originalSprite.name}_r{r}_c{c}";

                // Create new GameObject
                GameObject piece = new GameObject($"{gameObject.name}_piece_{pieceIndex++}");

                /*
                GameObject piece = Instantiate(gameObject);
                piece.name= $"{gameObject.name}_piece_{pieceIndex++}";
                DestroyImmediate(piece.GetComponent<SpriteRenderer>());

                DestroyImmediate(piece.GetComponent<SpriteGridSplitter>());
                */
                HitCounter hc =
                piece.AddComponent<HitCounter>();
                hc.hitThreshold = hitcount;
                hc.teerajaytasprite = teerajaytasprite;
                hc.teerajaytaspriteuusiversio = teerajaytaspriteuusiversio;
                hc.uusirajaytyscolumns = uusirajaytyscolumns;
                hc.uusirajaytysrows = uusirajaytysrows;
                hc.gravityscale = gravityscale;
                hc.alivetime = alivetime;
                hc.rajaytaspritenScaleFactorProsentti = rajaytaspritenScaleFactorProsentti;
                hc.rajaytaSpritenExplosion = explosion;
                //            public int uusirajaytyscolumns = 2;
                //public int uusirajaytysrows = 2;


                SpriteRenderer psr = piece.AddComponent<SpriteRenderer>();
                psr.sprite = subSprite;
                psr.sharedMaterial = baseMaterial;
                psr.sortingLayerID = sortingLayerID;
                psr.sortingOrder = sortingOrder;
                psr.color = color;
                psr.flipX = flipX;
                psr.flipY = flipY;

               // if (parentPieces)
                    piece.transform.SetParent(transform.parent, true);

                // --- Correct positioning ---
                Vector2 offsetPx = new Vector2(xCursor - sx, yCursor - sy) - pivotPx;
                offsetPx += subSprite.pivot;

                // Convert to world units and apply scale
                Vector2 localOffset = offsetPx / ppu;
                Vector3 scaledOffset = Vector3.Scale(localOffset, transform.localScale);

                piece.transform.position = transform.position + transform.rotation * scaledOffset;
                piece.transform.rotation = transform.rotation;
                piece.transform.localScale = transform.localScale;
                Rigidbody2D rr=
                piece.AddComponent<Rigidbody2D>();
                rr.bodyType = bodytype;
                //gravityscale = -1 * gravityscale;
                rr.gravityScale = gravityscale;
                //piece.AddComponent<PolygonCollider2D>();
                piece.AddComponent<BoxCollider2D>();

                piece.tag = gameObject.tag;
                piece.layer = gameObject.layer;

                xCursor += w;


            }
            yCursor += h;
        }

        // Optionally remove or disable the original
        /*
        if (destroyOriginal)
        {
            Destroy(gameObject);
        }
        else
        {
            sr.enabled = false;
        }
        */
        Destroy(gameObject);
       // sr.enabled = false;
    }

    private static int[] Partition(int total, int parts)
    {
        int[] result = new int[parts];
        int baseSize = total / parts;
        int remainder = total % parts;

        for (int i = 0; i < parts; i++)
            result[i] = baseSize + (i < remainder ? 1 : 0);

        return result;
    }
    public int alpharaja = 200;

    private void Start()
    {
        SplitIntoGrid();
    }




    public int CountNonAlphaPixelcount(Texture2D tex, int x, int y, int width, int height, byte alphaThreshold = 0)
    {
        if (tex == null)
        {
            Debug.LogError("CountNonAlphaPixelcount: texture is null");
            return 0;
        }

        if (!tex.isReadable)
        {
            Debug.LogError($"CountNonAlphaPixelcount: texture '{tex.name}' is not readable. Enable Read/Write in import settings.");
            return 0;
        }

        // Clamp rectangle to texture bounds
        x = Mathf.Clamp(x, 0, tex.width);
        y = Mathf.Clamp(y, 0, tex.height);
        width = Mathf.Clamp(width, 0, tex.width - x);
        height = Mathf.Clamp(height, 0, tex.height - y);

        Color32[] pixels = tex.GetPixels32();
        int count = 0;

        for (int yy = y; yy < y + height; yy++)
        {
            int rowStart = yy * tex.width + x;
            for (int xx = 0; xx < width; xx++)
            {
                Color32 px = pixels[rowStart + xx];
                if (px.a > alphaThreshold) // counts pixels above threshold
                    count++;
            }
        }

        return count;
    }


}
