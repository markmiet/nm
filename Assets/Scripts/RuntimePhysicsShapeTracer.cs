using UnityEngine;
using System.Collections.Generic;
using System.Linq;
[RequireComponent(typeof(PolygonCollider2D), typeof(SpriteRenderer))]
public class RuntimePhysicsShapeTracer : MonoBehaviour
{
    private Texture2D texture;
    public float alphaThreshold = 0.1f;

    private PolygonCollider2D polygonCollider;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        texture = spriteRenderer.sprite.texture;

 //       GenerateColliderFromTexture(texture, color => color.a > alphaThreshold);
    }

//    public float alphaThreshold = 0.1f;
    public float simplificationTolerance = 1.5f;
    public float smoothingStrength = 0.5f;

    public void tee()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        texture = spriteRenderer.sprite.texture;
       // GeneratePhysicsShape(texture, color => color.a > alphaThreshold);
        ApplyPhysicsShape();

    }
    public float pixelsPerUnit = 100f;
    void ApplyPhysicsShape()
    {
        bool[,] map = AlphaMapFromTexture(texture, alphaThreshold);
        List<List<Vector2>> contours = TraceContours(map);

        // Simplify paths
        /*
        List<Vector2[]> simplifiedContours = contours
            .Select(c => Simplify(c, simplificationTolerance).ToArray())
            .Where(c => c.Length >= 3 && c.Length <= 256)
            .ToList();
        */

        List<Vector2[]> simplifiedContours = contours
    .Select(c => Simplify(c, simplificationTolerance).ToArray())
    .Where(c => c.Length >= 3 && c.Length <= 1000)
    .ToList();

        // Assign to collider
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        collider.pathCount = simplifiedContours.Count;

        int pathIndex = 0;
        foreach (var path in simplifiedContours)
        {
            Vector2[] worldPath = path.Select(p => p / pixelsPerUnit).ToArray();
            collider.SetPath(pathIndex++, worldPath);
        }
    }

    bool[,] AlphaMapFromTexture(Texture2D tex, float threshold)
    {
        int width = tex.width;
        int height = tex.height;
        bool[,] map = new bool[width, height];

        Color[] pixels = tex.GetPixels();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float alpha = pixels[y * width + x].a;
                map[x, y] = alpha > threshold;
            }
        }
        return map;
    }

    List<List<Vector2>> TraceContours(bool[,] map)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        bool[,] visited = new bool[width, height];

        List<List<Vector2>> contours = new List<List<Vector2>>();

        for (int y = 0; y < height - 1; y++)
        {
            for (int x = 0; x < width - 1; x++)
            {
                if (map[x, y] && !visited[x, y])
                {
                    var contour = TraceSingleContour(map, visited, x, y);
                    if (contour != null && contour.Count >= 3)
                        contours.Add(contour);
                }
            }
        }

        return contours;
    }

    List<Vector2> TraceSingleContour(bool[,] map, bool[,] visited, int startX, int startY)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);

        List<Vector2> points = new List<Vector2>();
        int x = startX;
        int y = startY;
        int startCellX = x;
        int startCellY = y;

        Vector2Int dir = Vector2Int.right;
        int loopLimit = width * height * 10;
        int steps = 0;

        do
        {
            if (x < 0 || y < 0 || x >= width - 1 || y >= height - 1)
                break;

            visited[x, y] = true;

            int cell = 0;
            if (map[x, y]) cell |= 1;
            if (x + 1 < width && map[x + 1, y]) cell |= 2;
            if (x + 1 < width && y + 1 < height && map[x + 1, y + 1]) cell |= 4;
            if (y + 1 < height && map[x, y + 1]) cell |= 8;

            Vector2 corner = new Vector2(x, y);

            switch (cell)
            {
                case 1:
                case 5:
                case 13:
                    points.Add(corner + new Vector2(0, 0.5f));
                    dir = Vector2Int.down; break;
                case 8:
                case 10:
                case 11:
                    points.Add(corner + new Vector2(0.5f, 1));
                    dir = Vector2Int.left; break;
                case 4:
                case 6:
                case 7:
                    points.Add(corner + new Vector2(1, 0.5f));
                    dir = Vector2Int.up; break;
                case 2:
                case 3:
                case 14:
                    points.Add(corner + new Vector2(0.5f, 0));
                    dir = Vector2Int.right; break;
                default:
                    return null;
            }

            x += dir.x;
            y += dir.y;
            steps++;

            if (steps > loopLimit)
            {
                Debug.LogWarning("Contour tracing exceeded loop limit.");
                break;
            }

        } while (x != startCellX || y != startCellY);

        return points;
    }

    List<Vector2> Simplify(List<Vector2> points, float tolerance)
    {
        if (points == null || points.Count < 3) return points;

        List<Vector2> result = new List<Vector2>();
        DouglasPeucker(points, 0, points.Count - 1, tolerance, result);
        result.Add(points[0]);
        return result;
    }

    void DouglasPeucker(List<Vector2> points, int start, int end, float epsilon, List<Vector2> outPoints)
    {
        if (end <= start + 1)
            return;

        float maxDist = 0f;
        int index = 0;

        Vector2 A = points[start];
        Vector2 B = points[end];

        for (int i = start + 1; i < end; i++)
        {
            float dist = PerpendicularDistance(points[i], A, B);
            if (dist > maxDist)
            {
                index = i;
                maxDist = dist;
            }
        }

        if (maxDist > epsilon)
        {
            DouglasPeucker(points, start, index, epsilon, outPoints);
            outPoints.Add(points[index]);
            DouglasPeucker(points, index, end, epsilon, outPoints);
        }
    }

    float PerpendicularDistance(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
    {
        float dx = lineEnd.x - lineStart.x;
        float dy = lineEnd.y - lineStart.y;

        if (dx == 0 && dy == 0)
            return Vector2.Distance(point, lineStart);

        float numerator = Mathf.Abs(dy * point.x - dx * point.y + lineEnd.x * lineStart.y - lineEnd.y * lineStart.x);
        float denominator = Mathf.Sqrt(dx * dx + dy * dy);

        return numerator / denominator;
    }


}
