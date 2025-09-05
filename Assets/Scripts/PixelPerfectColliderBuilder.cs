using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

[RequireComponent(typeof(PolygonCollider2D))]
public class PixelPerfectColliderBuilder : MonoBehaviour
{
   // [SerializeField] private Texture2D sourceTexture;
    [SerializeField] private int alphaTolerance = 10; // kuinka läpinäkyvä saa olla
    [SerializeField] private bool autoRebuildOnStart = true;

    private PolygonCollider2D polyCollider;

    void Awake()
    {
        polyCollider = GetComponent<PolygonCollider2D>();
    }

    void Start()
    {
        Texture2D sourceTexture = GetComponent<SpriteRenderer>().sprite.texture;
        if (autoRebuildOnStart && sourceTexture != null)
        {
     //       StartCoroutine(RebuildColliderCoroutine());
        }
    }

    /// <summary>
    /// Käynnistä manuaalisesti
    /// </summary>
    public void RebuildCollider()
    {
        Texture2D sourceTexture = GetComponent<SpriteRenderer>().sprite.texture;

        if (sourceTexture == null)
        {
            Debug.LogError("PixelPerfectColliderBuilder: sourceTexture is missing!");
            return;
        }
        StartCoroutine(RebuildColliderCoroutine());
    }

    private IEnumerator RebuildColliderCoroutine()
    {
        float start = Time.realtimeSinceStartup;
        Texture2D sourceTexture = GetComponent<SpriteRenderer>().sprite.texture;

        // 1. Tyhjennä vanha collider
        polyCollider.pathCount = 0;
        yield return null;

        // 2. Lue tekstuurin pikselit NativeArray:ksi
        NativeArray<Color32> pixels = new NativeArray<Color32>(sourceTexture.GetPixels32(), Allocator.Persistent);
        int width = sourceTexture.width;
        int height = sourceTexture.height;

        // 3. Generoi polygonit (yksinkertainen outline-esimerkki)
        List<Vector2> path = new List<Vector2>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color32 c = pixels[y * width + x];
                if (c.a * 255 >= alphaTolerance)
                {
                    // yksinkertainen reunan tarkistus
                    if (IsEdgePixel(pixels, x, y, width, height, alphaTolerance))
                    {
                        path.Add(new Vector2(x / (float)width, y / (float)height));

                        // FPS:n suojaus: jaetaan työtä usealle framelle
                        if (path.Count % 200 == 0)
                            yield return null;
                    }
                }
            }
        }

        // 4. Aseta colliderin path
        if (path.Count >= 3)
        {
            polyCollider.pathCount = 1;
            polyCollider.SetPath(0, path.ToArray());
        }

        pixels.Dispose();

        float end = Time.realtimeSinceStartup;
        Debug.Log($"Pixel perfect collider valmis, kesto: {end - start:0.000} s, pisteitä: {path.Count}");
    }

    private bool IsEdgePixel(NativeArray<Color32> pixels, int x, int y, int width, int height, int alphaTolerance)
    {
        // Jos pikseli on reunalla → se on aina reunapikseli
        if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
            return true;

        int index = y * width + x;
        if (pixels[index].a * 255 < alphaTolerance) return false;

        // Tarkistetaan naapurit
        int[,] offsets = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
        for (int i = 0; i < 4; i++)
        {
            int nx = x + offsets[i, 0];
            int ny = y + offsets[i, 1];
            int ni = ny * width + nx;

            if (pixels[ni].a * 255 < alphaTolerance)
                return true; // reunapikseli
        }

        return false;
    }
}
