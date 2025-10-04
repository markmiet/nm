using UnityEngine;
using System.Collections.Generic;

public struct ColliderCorners
{
    public Vector3 lowerLeft;
    public Vector3 lowerRight;
    public Vector3 upperLeft;
    public Vector3 upperRight;

    public ColliderCorners(Vector3 ll, Vector3 lr, Vector3 ul, Vector3 ur)
    {
        lowerLeft = ll;
        lowerRight = lr;
        upperLeft = ul;
        upperRight = ur;
    }
}

public class ColliderExtremes : MonoBehaviour
{
    /// <summary>
    /// Returns the extreme corners (lower-left, lower-right, upper-left, upper-right)
    /// of all Collider2D components on this GameObject and its children.
    /// </summary>
    public ColliderCorners GetCorners()
    {
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        if (colliders == null || colliders.Length == 0)
        {
            Debug.LogWarning("No Collider2D components found in this object or its children.");
            return new ColliderCorners();
        }

        List<Vector3> allWorldPoints = new List<Vector3>();

        foreach (var col in colliders)
        {
            Vector3[] points = GetWorldPoints(col);
            if (points != null && points.Length > 0)
                allWorldPoints.AddRange(points);
        }

        if (allWorldPoints.Count == 0)
        {
            Debug.LogWarning("No points found across colliders.");
            return new ColliderCorners();
        }

        // Find global min/max X and Y
        float minX = float.MaxValue, maxX = float.MinValue;
        float minY = float.MaxValue, maxY = float.MinValue;

        foreach (var p in allWorldPoints)
        {
            if (p.x < minX) minX = p.x;
            if (p.x > maxX) maxX = p.x;
            if (p.y < minY) minY = p.y;
            if (p.y > maxY) maxY = p.y;
        }

        return new ColliderCorners(
            new Vector3(minX, minY, 0f), // lowerLeft
            new Vector3(maxX, minY, 0f), // lowerRight
            new Vector3(minX, maxY, 0f), // upperLeft
            new Vector3(maxX, maxY, 0f)  // upperRight
        );
    }

    private Vector3[] GetWorldPoints(Collider2D col)
    {
        if (col is BoxCollider2D box)
        {
            Vector2 offset = box.offset;
            Vector2 halfSize = box.size * 0.5f;

            Vector2[] localCorners = new Vector2[4];
            localCorners[0] = offset + new Vector2(-halfSize.x, -halfSize.y);
            localCorners[1] = offset + new Vector2(-halfSize.x, halfSize.y);
            localCorners[2] = offset + new Vector2(halfSize.x, halfSize.y);
            localCorners[3] = offset + new Vector2(halfSize.x, -halfSize.y);

            Vector3[] worldCorners = new Vector3[4];
            for (int i = 0; i < 4; i++)
                worldCorners[i] = col.transform.TransformPoint(localCorners[i]);

            return worldCorners;
        }
        else if (col is PolygonCollider2D poly)
        {
            Vector3[] points = new Vector3[poly.points.Length];
            for (int i = 0; i < poly.points.Length; i++)
                points[i] = col.transform.TransformPoint(poly.points[i] + poly.offset);
            return points;
        }
        else if (col is CircleCollider2D circle)
        {
            Vector3 center = col.transform.TransformPoint(circle.offset);
            float radius = circle.radius * Mathf.Max(col.transform.lossyScale.x, col.transform.lossyScale.y);
            return new Vector3[]
            {
                center + Vector3.left   * radius,
                center + Vector3.right  * radius,
                center + Vector3.up     * radius,
                center + Vector3.down   * radius
            };
        }
        else
        {
            return null; // unsupported collider type
        }
    }
}
