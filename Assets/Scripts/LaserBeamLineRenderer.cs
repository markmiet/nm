using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class LaserBeamLineRenderer : BaseController
{
    public Transform startPoint;
    public Transform endPoint;
    public float colliderThickness = 0.1f;

    private LineRenderer rend;
    private BoxCollider2D boxCollider;

    public float offsetkerto = 2.0f;
    public bool muutax = false;
    public bool muutay = false;
    private Material mat;
    public Vector2 texturescale = new Vector2(1, 1);

    public Texture newTexture; // Assign this in the Inspector

    public float speed = 2f;    // Movement speed
    public float waitTime = 2f; // Time to wait at each end

    private Vector3 targetPosition;
    private bool movingToEnd = true;
    private float waitTimer = 0f;

    void Start()
    {
        rend = GetComponent<LineRenderer>();
        rend.positionCount = 2;

        boxCollider = GetComponent<BoxCollider2D>();

        mat = rend.material;

        // Initialize position at start
        transform.position = startPoint.position;
        targetPosition = endPoint.position;
        //gameObject.transform.parent.gameObject
        IgnoraaCollisiotVihollistenValilla(gameObject, gameObject.transform.parent.gameObject);
        IgnoraaCollisiotVihollistenValilla(gameObject, gameObject.transform.parent.gameObject.transform.parent.gameObject);


    }

   // private Transform endPointOriginelli;
    private Vector3 currentPos;
    void Update()
    {
        if (!OnkoOkToimiaUusi(gameObject))
        {
            return;
        }


        /*
        float t = Mathf.PingPong(Time.time * speed, 1f);

        // Interpolate between start and end
        Vector3 currentPos = Vector3.Lerp(startPoint.position, endPoint.position, t);
        */

        float moveDuration = 1f / speed; // time to go from start -> end
        float cycle = moveDuration * 2f + waitTime * 2f; // full cycle length

        // time within cycle
        float t = Time.time % cycle;

        float lerpT;
        if (t < moveDuration) // going forward
        {
            lerpT = t / moveDuration;
        }
        else if (t < moveDuration + waitTime) // wait at end
        {
            lerpT = 1f;
        }
        else if (t < moveDuration * 2f + waitTime) // going back
        {
            lerpT = 1f - ((t - (moveDuration + waitTime)) / moveDuration);
        }
        else // wait at start
        {
            lerpT = 0f;
            endPoint.position = targetPosition;
        }

        // Interpolate position
        currentPos = Vector3.Lerp(startPoint.position, endPoint.position, lerpT);


        // Update LineRenderer positions
        Vector3 start = startPoint.position;
        Vector3 end = endPoint.position;

        rend.SetPosition(0, start);
        //rend.SetPosition(1, end);
        rend.SetPosition(1, currentPos);


        // Update BoxCollider2D
        Vector3 midPoint = (start + end) / 2f;
        transform.position = midPoint;

        //float length = Vector3.Distance(start, end);
        float length = Vector3.Distance(start, currentPos);

        boxCollider.size = new Vector2(length, colliderThickness);

        Vector3 direction = end - start;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        UpdateColliderAnchoredAtStart(start, currentPos);

        // Update texture offset
        Vector2 offset = mat.GetTextureOffset("_BaseMap");
        if (muutax) offset.x += Time.deltaTime * offsetkerto;
        if (muutay) offset.y += Time.deltaTime * offsetkerto;

        if (muutay || muutax)
            mat.SetTextureOffset("_BaseMap", offset);

        // Change texture if assigned
        if (newTexture != null)
            mat.SetTexture("_BaseMap", newTexture);
    }

    // call this from Update after you calculate start and end and set the LineRenderer positions
    void UpdateColliderAnchoredAtStart(Vector3 start, Vector3 end)
    {
        // direction & length in world space
        Vector3 direction = end - start;
        float length = direction.magnitude;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Anchor the transform at the START position and rotate toward END
        transform.position = start;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // If parent/ancestors have non-1 scale, BoxCollider2D.size must be set in local-space units.
        Vector3 lossy = transform.lossyScale;
        float sx = Mathf.Approximately(lossy.x, 0f) ? 1f : lossy.x;
        float sy = Mathf.Approximately(lossy.y, 0f) ? 1f : lossy.y;

        // size in local space so that world size equals 'length' and thickness parameter
        float localSizeX = length / sx;
        float localSizeY = colliderThickness / sy;

        boxCollider.size = new Vector2(localSizeX, localSizeY);

        // offset so the collider's left edge sits on the transform.position (start)
        boxCollider.offset = new Vector2(localSizeX * 0.5f, 0f);
    }

    public GameObject osumaExplosion;
    public float osumaExplosionkesto = 1.0f;

    public float spawnCooldown = 0.5f;  // Min time between spawns

    public bool pysaytaKunOsutaanjohonkin = false;

    private float lastSpawnTime = -999f;
    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag.Contains("vihollinen") || col.collider.tag.Contains("alus")) {
            //Vector2 contactPoint = col.GetContact(0).point;
            Vector2 contactPoint = col.GetContact(0).point;
            if (pysaytaKunOsutaanjohonkin)
                endPoint.position =(Vector3) currentPos;
            if (Time.time - lastSpawnTime >= spawnCooldown)
            {
                AiheutaVoimaa(col.gameObject);

                if (osumaExplosion != null)
                {
                    GameObject ins = Instantiate(osumaExplosion, contactPoint, Quaternion.identity);
                    Destroy(ins, osumaExplosionkesto);
                }

                lastSpawnTime = Time.time;
            }
        }
    }


    public float aiheutavoimaaRigidbodyyn = 5.0f;
    private void AiheutaVoimaa(GameObject go)
    {
        Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        if (rb != null && aiheutavoimaaRigidbodyyn > 0.0f)
        {
            Rigidbody2D r =
            go.GetComponent<Rigidbody2D>();
            if (r != null)
            {

                Vector2 suunta = r.velocity.normalized;

                rb.AddForce(suunta * aiheutavoimaaRigidbodyyn, ForceMode2D.Impulse);
            }
            /*
            Vector2 alku = piipunToinenPaa.transform.position;
            Vector2 loppu = piipunpaaJohonGameObjectInstantioidaan.transform.position;

            Vector2 suunta = alku - loppu;

           rb.AddForce(suunta * aiheutavoimaaRigidbodyyn, ForceMode2D.Impulse);
            */
        }
    }
}
