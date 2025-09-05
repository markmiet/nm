using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generate a pixel-perfect polygon collider from a runtime-modified texture.
/// Supports multiple shapes, holes, and path simplification.
/// </summary>
public class RuntimePixelPerfectCollider
{
    [Range(0f, 1f)]
    public float alphaThreshold = 0.1f; // Alpha cutoff for solid pixels

    [Range(0f, 5f)]
    public float simplifyTolerance = 5f; // Higher = fewer vertices

    /// <summary>
    /// Call this whenever the sprite’s texture changes.
    /// Pass in the PolygonCollider2D and SpriteRenderer to update.
    /// </summary>
    public void UpdateColliderMIksi(PolygonCollider2D poly, SpriteRenderer spriteRenderer)
    {
        if (poly == null || spriteRenderer == null) return;

        Sprite sprite = spriteRenderer.sprite;
        if (sprite == null) return;

        Texture2D tex = sprite.texture;
        if (tex == null) return;

        // Read alpha channel
        Color32[] pixels = tex.GetPixels32();
        int w = tex.width;
        int h = tex.height;

        // Run marching squares
        List<List<Vector2>> paths = MarchingSquares.Generate(pixels, w, h, alphaThreshold);

        // Simplify paths
        for (int i = 0; i < paths.Count; i++)
            paths[i] = PathSimplifier.Simplify(paths[i], simplifyTolerance);

        // Apply to collider
        poly.pathCount = paths.Count;
        for (int i = 0; i < paths.Count; i++)
            poly.SetPath(i, paths[i].ToArray());
    }
}

/// <summary>
/// Marching Squares implementation for Unity.
/// Handles multiple shapes and holes.
/// </summary>
public static class MarchingSquares
{
    public static List<List<Vector2>> Generate(Color32[] pixels, int width, int height, float alphaThreshold)
    {
        List<List<Vector2>> allPaths = new List<List<Vector2>>();

        bool[,] solid = new bool[width, height];
        bool[,] visited = new bool[width, height];

        // Build solid mask
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color32 c = pixels[x + y * width];
                solid[x, y] = c.a / 255f > alphaThreshold;
            }
        }

        // Find separate regions
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (solid[x, y] && !visited[x, y])
                {
                    List<Vector2> outline = TraceOutline(solid, visited, width, height, x, y);
                    if (outline.Count > 2)
                        allPaths.Add(outline);
                }
            }
        }

        return allPaths;
    }

    // Contour tracing (clockwise for outer shapes)
    private static List<Vector2> TraceOutline(bool[,] solid, bool[,] visited, int width, int height, int startX, int startY)
    {
        List<Vector2> points = new List<Vector2>();

        int cx = startX;
        int cy = startY;
        int dir = 0; // 0=right,1=down,2=left,3=up

        do
        {
            visited[cx, cy] = true;
            points.Add(new Vector2(cx, cy));

            // Check next move (clockwise search)
            bool moved = false;
            for (int i = 0; i < 4; i++)
            {
                int ndir = (dir + 3 + i) % 4;
                int nx = cx + (ndir == 0 ? 1 : ndir == 2 ? -1 : 0);
                int ny = cy + (ndir == 1 ? -1 : ndir == 3 ? 1 : 0);

                if (nx >= 0 && ny >= 0 && nx < width && ny < height && solid[nx, ny])
                {
                    cx = nx; cy = ny; dir = ndir;
                    moved = true;
                    break;
                }
            }

            if (!moved) break; // Dead end (shouldn’t happen for closed contours)
        }
        while (cx != startX || cy != startY);

        return points;
    }
}

/// <summary>
/// Path simplification (Ramer–Douglas–Peucker).
/// </summary>
public static class PathSimplifier
{
    public static List<Vector2> Simplify(List<Vector2> points, float tolerance)
    {
        if (points.Count < 3) return points;

        List<Vector2> result = new List<Vector2>();
        RDP(points, 0, points.Count - 1, tolerance, result);
        result.Add(points[points.Count - 1]); // close loop
        return result;
    }

    private static void RDP(List<Vector2> points, int start, int end, float tolerance, List<Vector2> result)
    {
        float maxDist = 0f;
        int index = 0;

        Vector2 a = points[start];
        Vector2 b = points[end];

        for (int i = start + 1; i < end; i++)
        {
            float dist = PerpendicularDistance(points[i], a, b);
            if (dist > maxDist)
            {
                index = i;
                maxDist = dist;
            }
        }

        if (maxDist > tolerance)
        {
            RDP(points, start, index, tolerance, result);
            RDP(points, index, end, tolerance, result);
        }
        else
        {
            if (result.Count == 0 || result[result.Count - 1] != a)
                result.Add(a);
        }
    }

    private static float PerpendicularDistance(Vector2 p, Vector2 a, Vector2 b)
    {
        if (a == b) return Vector2.Distance(p, a);
        return Mathf.Abs((b.x - a.x) * (a.y - p.y) - (a.x - p.x) * (b.y - a.y)) /
               Mathf.Sqrt((b.x - a.x) * (b.x - a.x) + (b.y - a.y) * (b.y - a.y));
    }
}
