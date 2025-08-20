using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteEdgeBlender : MonoBehaviour
{
    public int edgeWidth = 8;

    void Start()
    {
        StartCoroutine(BlendVisibleTaggedSprites());
    }

    IEnumerator BlendVisibleTaggedSprites()
    {
        yield return new WaitForEndOfFrame();

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Main camera not found.");
            yield break;
        }

        GameObject[] allWithTag = GameObject.FindGameObjectsWithTag("tiilivihollinentag");
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);

        List<(GameObject go, SpriteRenderer sr)> targets = new List<(GameObject, SpriteRenderer)>();

        foreach (GameObject go in allWithTag)
        {
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                if (GeometryUtility.TestPlanesAABB(planes, sr.bounds))
                {
                    targets.Add((go, sr));
                }
            }
            else
            {
                Debug.LogWarning($"GameObject '{go.name}' has no SpriteRenderer.");
            }
        }

        if (targets.Count < 2)
        {
            Debug.Log("Not enough visible sprites to blend.");
            yield break;
        }

        // Sortataan GameObjectit X-koordinaatin mukaan vasemmalta oikealle
        targets.Sort((a, b) => a.go.transform.position.x.CompareTo(b.go.transform.position.x));

        for (int i = 0; i < targets.Count - 1; i++)
        {
            var left = targets[i];
            var right = targets[i + 1];

            Sprite newLeft = BlendSpriteEdgeScaled(left.sr.sprite, right.sr.sprite, isRightEdge: true, edgeWidth);
            Sprite newRight = BlendSpriteEdgeScaled(right.sr.sprite, left.sr.sprite, isRightEdge: false, edgeWidth);

            left.sr.sprite = newLeft;
            right.sr.sprite = newRight;
        }

        Debug.Log("Reunojen blendaus valmis.");
    }

    Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight);
        for (int y = 0; y < targetHeight; y++)
        {
            for (int x = 0; x < targetWidth; x++)
            {
                float u = (float)x / targetWidth;
                float v = (float)y / targetHeight;
                Color color = source.GetPixelBilinear(u, v);
                result.SetPixel(x, y, color);
            }
        }
        result.Apply();
        return result;
    }

    public Sprite BlendSpriteEdgeScaled(Sprite baseSprite, Sprite blendFromSprite, bool isRightEdge, int edgeWidth = 8)
    {
        int width = Mathf.RoundToInt(baseSprite.textureRect.width);
        int height = Mathf.RoundToInt(baseSprite.textureRect.height);

        Texture2D baseCropped = new Texture2D(width, height);
        baseCropped.SetPixels(baseSprite.texture.GetPixels(
            Mathf.RoundToInt(baseSprite.textureRect.x),
            Mathf.RoundToInt(baseSprite.textureRect.y),
            width, height));
        baseCropped.Apply();

        int blendWidth = Mathf.RoundToInt(blendFromSprite.textureRect.width);
        int blendHeight = Mathf.RoundToInt(blendFromSprite.textureRect.height);
        Texture2D blendCropped = new Texture2D(blendWidth, blendHeight);
        blendCropped.SetPixels(blendFromSprite.texture.GetPixels(
            Mathf.RoundToInt(blendFromSprite.textureRect.x),
            Mathf.RoundToInt(blendFromSprite.textureRect.y),
            blendWidth, blendHeight));
        blendCropped.Apply();

        Texture2D blendScaled = ScaleTexture(blendCropped, width, height);

        Color[] basePixels = baseCropped.GetPixels();
        Color[] blendPixels = blendScaled.GetPixels();
        Color[] resultPixels = new Color[basePixels.Length];
        basePixels.CopyTo(resultPixels, 0);

        for (int x = 0; x < edgeWidth; x++)
        {
            float t = (float)x / (edgeWidth - 1);
            int baseX = isRightEdge ? width - edgeWidth + x : x;
            int blendX = isRightEdge ? x : width - edgeWidth + x;

            for (int y = 0; y < height; y++)
            {
                int idx = y * width + baseX;
                int blendIdx = y * width + blendX;

                Color baseColor = basePixels[idx];
                Color blendColor = blendPixels[blendIdx];
                resultPixels[idx] = Color.Lerp(baseColor, blendColor, 1f - t);
            }
        }

        Texture2D finalTex = new Texture2D(width, height);
        finalTex.SetPixels(resultPixels);
        finalTex.Apply();

        return Sprite.Create(finalTex, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), baseSprite.pixelsPerUnit);
    }
}
