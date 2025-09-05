using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SpriteGridSplitter : MonoBehaviour
{
    [Tooltip("Destroy the original object after creating the pieces.")]
    public bool destroyOriginal = true;

    [Tooltip("Parent all pieces under the original object for easy management.")]
    public bool parentPieces = true;

    [Tooltip("How many rows to split into.")]
    public int rows = 5;

    [Tooltip("How many columns to split into.")]
    public int cols = 5;

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

                // Create sub-sprite referencing the SAME texture
                var subSprite = Sprite.Create(
                    tex,
                    subRect,
                    pivotInSubNormalized,
                    ppu,
                    0,
                    SpriteMeshType.FullRect
                );
                subSprite.name = $"{originalSprite.name}_r{r}_c{c}";

                // Create new GameObject
                GameObject piece = new GameObject($"{gameObject.name}_piece_{pieceIndex++}");
                var psr = piece.AddComponent<SpriteRenderer>();
                psr.sprite = subSprite;
                psr.sharedMaterial = baseMaterial;
                psr.sortingLayerID = sortingLayerID;
                psr.sortingOrder = sortingOrder;
                psr.color = color;
                psr.flipX = flipX;
                psr.flipY = flipY;

                if (parentPieces)
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
                Rigidbody2D r=
                piece.AddComponent<Rigidbody2D>();
                r.bodyType = RigidbodyType2D.Static;
                piece.AddComponent<PolygonCollider2D>();

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

    private void Start()
    {
        SplitIntoGrid();
    }
}
