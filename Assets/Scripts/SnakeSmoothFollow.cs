using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]
public class SnakeSmootFollow : BaseController
{
    [Header("Snake Setup")]
    public GameObject bodyPartPrefab;

    public GameObject headPartPrerab;
    public int initialBodyParts = 10;
    public float minDistance = 0.2f;
    public float headSpeed = 5f;
    public Transform target;

    [Header("Smoothing")]
    public float movementSmoothTime = 0.05f;
    public float rotationSmoothTime = 0.05f;
    public float tailFollowFactor = 0.2f;

    [Header("Snake Length Limit")]
    public float maxSnakeLength = 20f;
    [Range(5, 200)]
    public int maxInitialParts = 50;

    [Header("Raycast Settings")]
    public float raycastDistance = 1f;
    public Vector2 raycastBoxSize = new Vector2(0.3f, 0.3f); // paksuus

    public float angleadd = 90.0f;

    private LineRenderer line;
    private EdgeCollider2D edgeCollider;

    private List<Transform> bodyParts = new List<Transform>();
    private List<Vector3> velocities = new List<Vector3>();
    private List<float> rotationVelocities = new List<float>();
    private List<float> currentAngles = new List<float>();
    private List<Vector3> positions = new List<Vector3>();

    public GameObject toinenMato;

    public bool enableEdgeCollider = false;

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        try
        {

        Handles.Label(transform.position + Vector3.up * 0.5f, gameObject.name);

        // Trail
        Gizmos.color = Color.yellow;
        if (positions != null && positions.Count > 1)
        {
            for (int i = 1; i < positions.Count; i++)
                Gizmos.DrawLine(positions[i - 1], positions[i]);
        }

        // Body connections
        Gizmos.color = Color.cyan;
        if (bodyParts != null && bodyParts.Count > 1)
        {
            for (int i = 1; i < bodyParts.Count; i++)
                Gizmos.DrawLine(bodyParts[i - 1].position, bodyParts[i].position);
        }

        // BoxCast visualization
        if (bodyParts != null && bodyParts.Count > 0 && target != null)
        {
            Vector2 origin = bodyParts[0].position;
            Vector2 dir = (target.position - bodyParts[0].position).normalized;

            DrawBoxCastGizmo(origin, dir, raycastDistance);

            for (int angleOffset = 15; angleOffset <= 180; angleOffset += 15)
            {
                DrawBoxCastGizmo(origin, RotateVector(dir, angleOffset), raycastDistance);
                DrawBoxCastGizmo(origin, RotateVector(dir, -angleOffset), raycastDistance);
            }
        }


        }
        catch (Exception ex)
        {
            //just i
        }
#endif
    }

    void Start()
    {
        line = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        edgeCollider.edgeRadius = 0.1f;

        edgeCollider.enabled = enableEdgeCollider;
        GameObject targetObj = PalautaAlus();
        target = targetObj.transform;
        InitializeSnake();
        IgnoreSelfCollisions(gameObject);
        //muuta niin että etitään madot kamerasta ja tehdään ignore

        if (toinenMato != null)
        {
            IgnoraaCollisiotVihollistenValillaALakasittelevihollinen(gameObject, toinenMato);
        }

    }
    private GameObject head;
    void InitializeSnake()
    {
        //GameObject head = new GameObject("Head");

        head = Instantiate(headPartPrerab, transform.position, Quaternion.identity, transform);
        head.transform.SetParent(transform);
        head.transform.localPosition = Vector3.zero;

        head.transform.position = transform.position;
        bodyParts.Add(head.transform);
        velocities.Add(Vector3.zero);
        rotationVelocities.Add(0f);
        currentAngles.Add(0f);

        int maxPartsByLength = Mathf.FloorToInt(maxSnakeLength / minDistance);
        initialBodyParts = Mathf.Min(maxPartsByLength, maxInitialParts);

        Vector3 initialDir = Vector3.left;
        positions.Clear();
        for (int i = 0; i <= initialBodyParts; i++)
            positions.Add(head.transform.position - initialDir * minDistance * i);


        Transform prev = head.transform;
        for (int i = 0; i < initialBodyParts; i++)
        {
            GameObject part = Instantiate(bodyPartPrefab, positions[i + 1], Quaternion.identity, transform);
            bodyParts.Add(part.transform);
            velocities.Add(Vector3.zero);
            rotationVelocities.Add(0f);
            currentAngles.Add(0f);



        }
    }
    private bool jointitlisatty = true;
    public float jointtitekokohta = 1.0f;
    public float jointtilaskuri = 0.0f;
    private void LisaaJointit()
    {
        if (jointitlisatty)
        {
            return;
        }
        jointtilaskuri += Time.fixedDeltaTime;
        if (jointtilaskuri>=jointtitekokohta)
        {
            Transform prev = head.transform;
            for (int i = 1; i < bodyParts.Count; i++)
            {
                GameObject part = bodyParts[i].gameObject;
                //DistanceJoint2D joint = part.AddComponent<DistanceJoint2D>();
                SpringJoint2D joint = part.AddComponent<SpringJoint2D>();

                joint.connectedBody = prev.GetComponent<Rigidbody2D>();
                joint.autoConfigureDistance = false;
                float dis = Vector2.Distance(part.transform.position, prev.position);
                joint.distance = dis;

                joint.frequency = 5f;      // spring stiffness
                joint.dampingRatio = 0.7f; // damping (reduce bouncing)
                joint.enableCollision = false;
                prev = part.transform;
            }
            jointitlisatty = true;
        }

    }


    void FixedUpdate()
    {
        MoveHead();
        MoveBody();
        UpdateLineRenderer();
        UpdateEdgeCollider();
        LisaaJointit();
    }

    void MoveHead()
    {
        if (target == null) return;

        Vector2 dir = GetBestDirection(bodyParts[0].position, target.position, raycastDistance);

        Vector3 targetPos = bodyParts[0].position + (Vector3)(dir * headSpeed * Time.fixedDeltaTime);
        Vector3 tempVel = velocities[0];
        bodyParts[0].position = Vector3.SmoothDamp(bodyParts[0].position, targetPos, ref tempVel, movementSmoothTime);
        velocities[0] = tempVel;

        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float tempRot = rotationVelocities[0];
        currentAngles[0] = Mathf.SmoothDampAngle(currentAngles[0], targetAngle, ref tempRot, rotationSmoothTime);
        rotationVelocities[0] = tempRot;
        bodyParts[0].rotation = Quaternion.Euler(0, 0, currentAngles[0] + angleadd);

        if (positions.Count == 0 || Vector3.Distance(bodyParts[0].position, positions[positions.Count - 1]) >= minDistance / 2f)
        {
            positions.Add(bodyParts[0].position);
            while (positions.Count > bodyParts.Count)
                positions.RemoveAt(0);
        }
    }

    void MoveBody()
    {
        if (positions.Count == 0) return;

        for (int i = 1; i < bodyParts.Count; i++)
        {
            float targetDist = i * minDistance;
            Vector3 targetPos = GetPointAlongPath(targetDist);

            Vector3 tempVel = velocities[i];
            bodyParts[i].position = Vector3.SmoothDamp(bodyParts[i].position, targetPos, ref tempVel,
                movementSmoothTime * (1 + i * tailFollowFactor));
            velocities[i] = tempVel;

            float targetAngle = Mathf.Atan2(
                bodyParts[i - 1].position.y - bodyParts[i].position.y,
                bodyParts[i - 1].position.x - bodyParts[i].position.x
            ) * Mathf.Rad2Deg;

            float tempRot = rotationVelocities[i];
            currentAngles[i] = Mathf.SmoothDampAngle(currentAngles[i], targetAngle, ref tempRot,
                rotationSmoothTime * (1 + i * tailFollowFactor));
            rotationVelocities[i] = tempRot;

            bodyParts[i].rotation = Quaternion.Euler(0, 0, currentAngles[i] + angleadd);
        }
    }

    Vector3 GetPointAlongPath(float distance)
    {
        if (positions.Count == 0) return bodyParts[0].position;

        float accumulated = 0f;
        for (int i = positions.Count - 1; i > 0; i--)
        {
            float segment = Vector3.Distance(positions[i], positions[i - 1]);
            if (accumulated + segment >= distance)
            {
                float t = (distance - accumulated) / segment;
                return Vector3.Lerp(positions[i], positions[i - 1], t);
            }
            accumulated += segment;
        }
        return positions[0];
    }

    void UpdateLineRenderer()
    {
        line.positionCount = positions.Count;
        for (int i = 0; i < positions.Count; i++)
            line.SetPosition(i, positions[i]);
    }

    void UpdateEdgeCollider()
    {
        if (!enableEdgeCollider)
            return;

        if (edgeCollider == null || positions.Count < 2) return;

        Vector2[] points = new Vector2[positions.Count];
        for (int i = 0; i < positions.Count; i++)
            points[i] = transform.InverseTransformPoint(positions[i]);
        edgeCollider.points = points;
    }

    public void AddBodyPart()
    {
        Transform tail = bodyParts[bodyParts.Count - 1];
        Vector3 dir = (bodyParts[bodyParts.Count - 2].position - tail.position).normalized;
        Vector3 spawnPos = tail.position - dir * minDistance;

        GameObject part = Instantiate(bodyPartPrefab, spawnPos, tail.rotation, transform);

        bodyParts.Add(part.transform);
        velocities.Add(Vector3.zero);
        rotationVelocities.Add(0f);
        currentAngles.Add(tail.eulerAngles.z);

        positions.Insert(0, spawnPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bonustag"))
        {
            AddBodyPart();
            Destroy(collision.gameObject);
        }
    }

    // ===============================
    // Esteiden kierto
    // ===============================
    Vector2 GetBestDirection(Vector2 currentPos, Vector2 targetPos, float checkDistance)
    {
        Vector2 desiredDir = (targetPos - currentPos).normalized;

        if (IsDirectionClear(currentPos, desiredDir, checkDistance))
            return desiredDir;

        Vector2 bestDir = desiredDir;
        float closestDistance = float.PositiveInfinity;

        for (int angleOffset = 15; angleOffset <= 360; angleOffset += 15)
        {
            Vector2[] testDirs = new Vector2[]
            {
                RotateVector(desiredDir, angleOffset),
                RotateVector(desiredDir, -angleOffset)
            };

            foreach (var dir in testDirs)
            {
                if (IsDirectionClear(currentPos, dir, checkDistance))
                {
                    float dist = Vector2.Distance(currentPos + dir * checkDistance, targetPos);
                    if (dist < closestDistance)
                    {
                        closestDistance = dist;
                        bestDir = dir;
                    }
                }
            }
        }

        return bestDir;


    }




    bool IsDirectionClear(Vector2 origin, Vector2 dir, float distance)
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(
            origin,
            raycastBoxSize,
            0f,
            dir,
            distance
        );

        foreach (var hit in hits)
        {
            if (hit.collider == null) continue;
            if (hit.collider.transform.IsChildOf(transform)) continue;
            if (hit.collider.tag.Contains("vihollinen"))
                return false;
        }
        return true;
    }

    void DrawBoxCastGizmo(Vector2 origin, Vector2 dir, float distance)
    {
        Color col = IsDirectionClear(origin, dir, distance) ? Color.green : Color.red;
        Gizmos.color = col;

        Quaternion rot = Quaternion.LookRotation(Vector3.forward, dir);
        Vector3 center = origin + dir * distance * 0.5f;
        Vector3 size = new Vector3(raycastBoxSize.x, distance, raycastBoxSize.y);

        Gizmos.matrix = Matrix4x4.TRS(center, rot, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);
        Gizmos.matrix = Matrix4x4.identity;
    }

    Vector2 RotateVector(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos).normalized;
    }

    void IgnoreSelfCollisions(GameObject root)
    {
        Collider2D[] colliders = root.GetComponentsInChildren<Collider2D>();
        for (int i = 0; i < colliders.Length; i++)
            for (int j = i + 1; j < colliders.Length; j++)
                Physics2D.IgnoreCollision(colliders[i], colliders[j], true);
    }
}
