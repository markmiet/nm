using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(LineRenderer))]
//[RequireComponent(typeof(PolygonCollider2D))]


//[ExecuteAlways]   // Run in Editor + Play
public class SnakeSmootFollow : BaseController
{



    [Header("Snake Setup, talla on siis childreneilla ne colliderit")]
    public GameObject bodyPartPrefab;
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

    public float angleadd = 90.0f;

    private LineRenderer line;
    private PolygonCollider2D polyCollider;
    private List<Transform> bodyParts = new List<Transform>();
    private List<Vector3> velocities = new List<Vector3>();
    private List<float> rotationVelocities = new List<float>();
    private List<float> currentAngles = new List<float>();
    private List<Vector3> positions = new List<Vector3>();



    private void OnDrawGizmos()
    {
        // Draw a wire sphere at this GameObject's position
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1.0f);



#if UNITY_EDITOR

        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontSize = 8;
        labelStyle.normal.textColor = Color.red;
        Handles.Label(transform.position + Vector3.up * 0.2f, $"snake {gameObject.name}", labelStyle);
#endif

        // If a target is assigned, draw a line to it

    }





    /*
    void Update()
    {
        // In Editor mode, just keep the LineRenderer updated
        if (!Application.isPlaying)
        {
            UpdateLineRenderer();
           // if (updateCollider)
             //   UpdatePolygonCollider();
        }
    }
    */


    void Start()
    {



        line = GetComponent<LineRenderer>();



        GameObject targetObj = PalautaAlus();
        target = targetObj.transform;
        InitializeSnake();


        /*
        List<GameObject> allParts = new List<GameObject>();

        GameObject head = new GameObject("Head");

        head.transform.SetParent(transform);

        // Set position
        head.transform.localPosition = Vector3.zero;


        allParts.Add(head);
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

        for (int i = 0; i < initialBodyParts; i++)
        {
            GameObject part = Instantiate(bodyPartPrefab, positions[i + 1], Quaternion.identity, transform);
            bodyParts.Add(part.transform);
            velocities.Add(Vector3.zero);
            rotationVelocities.Add(0f);
            currentAngles.Add(0f);
            allParts.Add(part);
        }

        */
        //InitializeSnake();



        // transform.position = bodyParts[0].position; // move GameObject pivot to head
        // line.useWorldSpace = false;                 // now positions are local

    }


 


    void InitializeSnake()
    {


        List<GameObject> allParts = new List<GameObject>();

        GameObject head = new GameObject("Head");

        head.transform.SetParent(transform);

        // Set position
        head.transform.localPosition = Vector3.zero;


        allParts.Add(head);
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

        for (int i = 0; i < initialBodyParts; i++)
        {
            GameObject part = Instantiate(bodyPartPrefab, positions[i + 1], Quaternion.identity, transform);
            bodyParts.Add(part.transform);
            velocities.Add(Vector3.zero);
            rotationVelocities.Add(0f);
            currentAngles.Add(0f);
            allParts.Add(part);
        }

        IgnoreCollisions(allParts);
    }




    // private Vector3 lastColliderUpdatePos;

    void FixedUpdate()
    {
        MoveHead();
        MoveBody();
        UpdateLineRenderer();
        // UpdatePolygonCollider();
    }


    void MoveHead()
    {
        if (target == null) return;

        Vector2 dir = (target.position - bodyParts[0].position).normalized;
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
            positions.Add(bodyParts[0].position);
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
        line.positionCount = bodyParts.Count;
        for (int i = 0; i < bodyParts.Count; i++)
            line.SetPosition(i, bodyParts[i].position);
    }


    public void AddBodyPart()
    {
        Transform tail = bodyParts[bodyParts.Count - 1];
        Vector3 spawnPos = tail.position - tail.up * minDistance;
        GameObject part = Instantiate(bodyPartPrefab, spawnPos, tail.rotation, transform);

        bodyParts.Add(part.transform);
        velocities.Add(Vector3.zero);
        rotationVelocities.Add(0f);
        currentAngles.Add(tail.eulerAngles.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bonustag"))
        {
            AddBodyPart();
            Destroy(collision.gameObject);
        }
    }
}
