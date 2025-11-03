using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class Path : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();

    private Vector3 startPos;



    public Vector2 GetPosition(float t)
    {
        if (points.Count == 0)
            return transform.position;

        int i = Mathf.FloorToInt(t);
        if (i >= points.Count - 1)
        {
            //return points[points.Count - 1].position;
            return Vector2.zero;

        }
            

        float f = t - i;
        return Vector2.Lerp(points[i].position, points[i + 1].position, f);
    }

    public bool FlipY(float t)
    {
        if (points.Count < 2)
            return false;

        // Selvitetään missä segmentissä ollaan
        int i = Mathf.FloorToInt(t);
        if (i >= points.Count - 1)
            i = points.Count - 2; // viimeinen väli

        // Vektori seuraavasta pisteestä edelliseen
        Vector3 dir = points[i + 1].position - points[i].position;

        // Jos liike suuntautuu vasemmalle (x negatiivinen) → flipX = true
        return !(dir.x < 0f);
    }


    public float Length => Mathf.Max(points.Count - 1, 0);

    void OnDrawGizmos()
    {
        if (points.Count < 2) return;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < points.Count - 1; i++)
        {
            if (points[i] && points[i + 1])
                Gizmos.DrawLine(points[i].position, points[i + 1].position);
        }

        Gizmos.color = Color.cyan;
        foreach (var p in points)
        {
            if (p)
                Gizmos.DrawSphere(p.position, 0.1f);
        }
    }
}
