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

    private bool developmentflag = false;
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

    public GameObject[] objektitjoihincollisiotIgnorataan;

    public bool enableEdgeCollider = false;


    public GameObject paanparticleGameObject;
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        try
        {
            Handles.Label(transform.position + Vector3.up * 0.5f, gameObject.name);
            if (true)
                return;
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

            NahdaankoAlus();

        }
        catch (Exception ex)
        {
            //just i
        }
#endif
    }

    private HitCounter hc = null;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        edgeCollider.edgeRadius = 0.1f;

        edgeCollider.enabled = enableEdgeCollider;
        GameObject targetObj = PalautaAlus();
        target = targetObj.transform;
        InitializeSnake();
        //muuta niin että etitään madot kamerasta ja tehdään ignore

        if (objektitjoihincollisiotIgnorataan != null)
        {
            foreach (GameObject g in objektitjoihincollisiotIgnorataan)
            {
                IgnoraaCollisiotVihollistenValillaALakasittelevihollinen(gameObject, g);
            }

        }
        hc = GetComponent<HitCounter>();
        headlaskuri = headsykli;

    }
    private GameObject head;

    public bool teeHeadingParticleVainKunMatoNahnytAluksen = true;
    private bool alusnahty = false;

    private GameObject inst;

    private ParticleSystem ps;
    private ParticleSystem.EmissionModule emission;

    [Header("Fade Settings")]
    public float fadeSpeed = 2f;          // How fast emission fades in/out
    public float maxEmissionRate = 100f;  // Maximum emission rate

    [Header("Vision Cone")]
    //public Transform target;              // Target to detect
    public float visionFOV = 30f;         // Half-angle of vision cone
    public int raysInCone = 5;            // Number of rays for soft cone

    private float currentRate = 0f;       // Current emission rate


    private float timecounter = 0;   // Counts consecutive frames in same state
    public float timeRequired = 0.5f;

    private bool nahdaanStable = false;     // Current stable value
    private bool lastNahdaan = false;       // Last raw value

    public bool emittoiriippumattanahdaanko = false;

    private bool emittoiriippumattanahdaankoJaOnJotehty = false;
    public void HeadParticleSystem()
    {
        //tutkitaan nähdäänkö, jos nähdään niin particle play
        //viivettää tähän...



        bool nahdaanCurrent = NahdaankoAlus();

        // If raw value changed from last frame, reset counter
        if (nahdaanCurrent != lastNahdaan)
        {
            //nahdaanFrameCounter = 1; // start counting new state
            timecounter = Time.deltaTime;

        }
        else
        {
            timecounter += Time.deltaTime;
        }

        lastNahdaan = nahdaanCurrent;

        // Update stable value only if counter reached threshold
        if (timecounter >= timeRequired)
        {
            nahdaanStable = nahdaanCurrent;
        }

        if (nahdaanStable)
        {
            viimeksinahty = Time.time;
        }

        if (emittoiriippumattanahdaankoJaOnJotehty)
        {
            return;
        }


        // Debug.Log("nahdaanStable=" + nahdaanStable);

        // Use nahdaanStable for particle emission
        //float targetRate = nahdaanStable ? maxEmissionRate : 0f;
        // Debug.Log("nahdaan=" + nahdaan);
        // Target emission rate
        float targetRate = nahdaanStable ? maxEmissionRate : 0f;



        if (!emittoiriippumattanahdaanko)
        {

            if (inst == null)
            {
                inst = Instantiate(paanparticleGameObject, head.transform.position, paanparticleGameObject.transform.rotation, head.transform);

                inst.transform.localPosition = Vector3.zero;
                inst.transform.localScale = paanparticleGameObject.transform.localScale;

                ps = inst.GetComponent<ParticleSystem>();
            }
            // Smooth fade
            currentRate = Mathf.Lerp(currentRate, targetRate, Time.deltaTime * fadeSpeed);


            emission = ps.emission;
            // Update emission properly
            var rate = emission.rateOverTime;
            rate.constant = currentRate;
            emission.rateOverTime = rate;
            ParticleSystemRenderer psr = ps.GetComponent<ParticleSystemRenderer>();

            psr.sortingLayerName = "SkeletonLayer";  // match your drawing layer
            psr.sortingOrder = 5;

            // Start/stop particle system as needed
            if (!ps.isPlaying && currentRate > 10f)
                ps.Play();
            else if (ps.isPlaying && currentRate <= 10f)
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                // ParticleSystemStopBehavior.StopEmittingAndClear();
            }

        }
        else
        {
            emittoiriippumattanahdaankoJaOnJotehty = true;

            if (inst == null)
            {
                /*
                inst = Instantiate(paanparticleGameObject, head.transform.position, paanparticleGameObject.transform.rotation, head.transform);

                inst.transform.localPosition = Vector3.zero;
                inst.transform.localScale = paanparticleGameObject.transform.localScale;

                ps = inst.GetComponent<ParticleSystem>();

                emission = ps.emission;
                // Update emission properly
                var rate = emission.rateOverTime;
                rate.constant = maxEmissionRate;
                emission.rateOverTime = rate;
                ParticleSystemRenderer psr = ps.GetComponent<ParticleSystemRenderer>();

                psr.sortingLayerName = "SkeletonLayer";  // match your drawing layer
                psr.sortingOrder = 5;
                */
                foreach (Transform t in bodyParts)
                {
                    GameObject g = t.gameObject;

                    if (g.GetComponentInChildren<ParticleSystem>() == null)
                    {
                        GameObject instbody = Instantiate(paanparticleGameObject, g.transform.position,
                            g.transform.rotation, g.transform);

                        instbody.transform.localPosition = Vector3.zero;
                        instbody.transform.localScale = g.transform.localScale;

                        ParticleSystem psbody = instbody.GetComponent<ParticleSystem>();

                        ParticleSystem.EmissionModule emissionbody = psbody.emission;
                        // Update emission properly
                        var ratebody = emissionbody.rateOverTime;
                        ratebody.constant = maxEmissionRate;
                        emissionbody.rateOverTime = ratebody;
                        ParticleSystemRenderer psrbody = psbody.GetComponent<ParticleSystemRenderer>();

                        psrbody.sortingLayerName = "SkeletonLayer";  // match your drawing layer
                        psrbody.sortingOrder = 5;
                    }
                }
            }
        }


        /*
        bool nahdaan=NahdaankoAlus();

        ParticleSystem ps =
        inst.GetComponent<ParticleSystem>();
        if (nahdaan)
        {
            if (!ps.isPlaying)
            {
                ps.Play();
            }

        }
        else
        {
            if (ps.isPlaying)
            {
                ps.Stop();
            }
        }

        */


    }
    /*
    private bool NahdaankoAlus()
    {
        Vector2 t = transform.position;

        Vector2 aluksensijainti = target.position;

        //@todo raycast from t to aluksensijainti
        //between raycast should not be any objects that tag contains "vihollinen"
        //ignore this gameobject also

        return true;
    }
    */


    private bool NahdaankoAlus()
    {
        Vector2 start = head.transform.position;
        //Vector2 toTarget = (Vector2)target.position - start;

        Vector2 toTarget = (Vector2)target.position - start;

        float distanceToTarget = toTarget.magnitude;
        Vector2 targetDir = toTarget.normalized;

        float angleStep = visionFOV * 2 / (raysInCone - 1);

        for (int i = 0; i < raysInCone; i++)
        {
            float angleOffset = -visionFOV + i * angleStep;
            Vector2 rayDir = RotateVector(targetDir, angleOffset);

            // Debug visualization in Scene view
            Debug.DrawRay(start, rayDir * distanceToTarget, Color.red);

            RaycastHit2D[] hits = Physics2D.RaycastAll(start, rayDir, distanceToTarget);
            bool blocked = false;

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider == null) continue;

                Transform hitTransform = hit.collider.transform;

                // Ignore self and children
                if (hitTransform == transform || hitTransform.IsChildOf(transform))
                    continue;

                if (hitTransform.gameObject.tag.Contains("vihollinen"))
                {
                    blocked = true;
                    break;
                }
            }

            if (!blocked)
                return true; // At least one ray reached target
        }

        return false; // All rays blocked
    }


    // Rotate 2D vector by degrees
    private Vector2 RotateVector(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos).normalized;
    }




    void InitializeSnake()
    {
        //GameObject head = new GameObject("Head");

        if (head != null)
        {
            Destroy(head);
        }
        foreach (Transform t in bodyParts)
        {
            Destroy(t.gameObject);
        }
        bodyParts.Clear();
        velocities.Clear();
        rotationVelocities.Clear();
        currentAngles.Clear();

        List<GameObject> lista = new List<GameObject>();

        head = Instantiate(headPartPrerab, transform.position, Quaternion.identity, transform);
        lista.Add(head);
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
        {
            positions.Add(head.transform.position - initialDir * minDistance * i);

            //            positions.Add(head.transform.position);

        }



        Transform prev = head.transform;
        for (int i = 0; i < initialBodyParts; i++)
        {
            GameObject part = Instantiate(bodyPartPrefab, positions[i + 1], Quaternion.identity, transform);
            //GameObject part = Instantiate(bodyPartPrefab, positions[i], Quaternion.identity, transform);
            lista.Add(part);
            bodyParts.Add(part.transform);
            velocities.Add(Vector3.zero);
            rotationVelocities.Add(0f);
            currentAngles.Add(0f);
        }
        //IgnoreCollisions(lista);
        IgnoreSelfCollisions(gameObject);



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
        if (jointtilaskuri >= jointtitekokohta)
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
    public float sykli = 10.0f;
    float laskuri = 0.0f;


    public bool saadapaanDissolveamount = false;

    private float dissolveoriginal;
    private DissolveMatController dissovlpaa = null;

    private bool headTutkinta = true;
    private float headlaskuri = 0.0f;
    public float headsykli = 0.1f;

    private void Update()
    {
        if (developmentflag)
        {
            laskuri += Time.deltaTime;

            if (laskuri >= sykli)
            {
                InitializeSnake();
                laskuri = 0.0f;
            }
        }
        /*
        if (headTutkinta && OnkoOkToimiaUusi(gameObject))
        {
            HeadParticleSystem();
           
        }
        headTutkinta = !headTutkinta;
        */
        headlaskuri += Time.deltaTime;

        if (headlaskuri >= headsykli /* && OnkoOkToimiaUusi(gameObject)*/)
        {
            HeadParticleSystem();
            headlaskuri = 0;

        }



        if (saadapaanDissolveamount && hc != null)
        {

            //GetComponentInChildren<DissolveMatController>()
            if (head != null)
            {
                if (dissovlpaa == null)
                {
                    dissovlpaa =
                head.GetComponentInChildren<DissolveMatController>();
                    if (dissovlpaa != null)
                        dissolveoriginal = dissovlpaa.dissolveamount;

                }
                //pistetaan vain pää hehkumaan, oisko nätimpi jos koko vartalo hehkuis
                //pää punaisena :)
                if (dissovlpaa != null)
                {
                    float prosentit = Mathf.Clamp01(hc.hitCount / (float)hc.hitThreshold) * 100f;
                    float maksimidissolve = dissolveoriginal;
                    float minimidissolve = 0.0f;
                    float uusiarvo = (prosentit / 100.0f) * maksimidissolve;
                    dissovlpaa.dissolveamount = uusiarvo;

                    foreach (Transform t in bodyParts)
                    {
                        if (t.gameObject.GetComponent<DissolveMatController>())
                        {
                            t.gameObject.GetComponent<DissolveMatController>().dissolveamount = uusiarvo;
                        }

                    }


                }

            }
            //     float arvo =hc.PalautaDissolveAmountLaskettunaosumista();

            /*
             *     public float PalautaDissolveAmountLaskettunaosumista()
    {
        float prosentit = Mathf.Clamp01(hitCount / (float)hitThreshold) * 100f;
        float maksimidissolve = dissolveoriginal;
        float minimidissolve = 0.0f;
        float uusiarvo = (prosentit / 100.0f) * maksimidissolve;
        return uusiarvo;
    }*/
        }
    }

    void FixedUpdate()
    {
        if (IsGoingToBeDestroyed())
        {
            return;
        }

        if (!OnkoOkToimiaUusi(gameObject))
        {
            return;
        }

        if (firsttime || nahdaanStable)
        {
            MoveHead();
            MoveBody();
            UpdateLineRenderer();
            UpdateEdgeCollider();
            firsttime = false;
        }
        else if (viimeksinahty + liikkumisaikaViimeisenNahdynJalkeen > Time.time)
        {
            MoveHead();
            MoveBody();
            UpdateLineRenderer();
            UpdateEdgeCollider();
            firsttime = false;
        }

        //LisaaJointit();
    }
    private bool firsttime = true;
    private float viimeksinahty = 0.0f;
    public float liikkumisaikaViimeisenNahdynJalkeen = 5.0f;




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

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bonustag"))
        {
            AddBodyPart();
            Destroy(collision.gameObject);
        }
    }
    */

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
    /*
    Vector2 RotateVector(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos).normalized;
    }
    */

    void IgnoreSelfCollisions(GameObject root)
    {
        //Collider2D[] colliders = root.GetComponentsInChildren<Collider2D>();
        /*
        for (int i = 0; i < colliders.Length; i++)
            for (int j = i + 1; j < colliders.Length; j++)
                Physics2D.IgnoreCollision(colliders[i], colliders[j], true);
        */
        IgnoreChildCollisions(root.transform);


    }

    private void OnEnable()
    {
        IgnoreSelfCollisions(gameObject);
    }
}