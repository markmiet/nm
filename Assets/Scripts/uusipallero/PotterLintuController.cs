using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PotterLintuController : MonoBehaviour
{
    [Header("Physics")]
    public Rigidbody2D rb;
    public float liftForce = 5f;
    public float forwardForce = 1f;
    public int pulses = 3;
    public float totalDuration = 0.2f;
    public float xvelocitymaxlimit = 0.1f;

    public float gravityScale = 1f;

    [Header("Rotation Settings")]
    public float tiltMultiplier = -3f;
    public float maxTilt = 30f;
    public float rotationReturnSpeed = 5f;

    [Header("Dodge Settings")]
    public float detectionDistance = 5f;
    public float dodgeForce = 6f;
    public float dodgeLift = 4f;
    public float dodgeDuration = 0.6f;
    public float dodgeTiltAngle = 45f;
    public float dodgeCooldown = 1.0f;
    public LayerMask obstacleLayer;


    [Header("Tilt Settings")]
    public float flapTiltAngle = 25f;
    public float tiltSpeed = 10f;

    [Header("Animation (Optional)")]
    public Animator animator;

    [Header("Raycast Settings (Auto)")]
    public Transform sijaintijostatutkitaan;
    public int rayCount = 3;

    private float verticalCheckDistance;
    private float startRotationZ;
    private bool isReturningRotation = false;
    public bool isDodging = false;
    private float dodgeTimer = 0f;
    private float dodgeCooldownTimer = 0f;
    private Vector2 dodgeDirection;
    private Transform lastDodgedObstacle;

    // 🧩 Debug: tallennetaan reittitestien tulokset Gizmoja varten
    private Vector2 upTestOrigin, downTestOrigin;
    private bool upPathBlocked, downPathBlocked;


    public float maxSpeedX = 2f;
    public float maxSpeedY = 2f;
    
    public float maxAngularSpeed = 200f;

    [Header("kulkupolku Settings")]
    public Path kulkupolku;
    public bool kuljetaanpolkua=false;
    public float pathSpeed = 1.0f;      // Kuinka nopeasti etenee reittiä pitkin
    public float followStrength = 0.5f; // Kuinka voimakkaasti hakeutuu reitille
    //public float flapAmplitude = 0.5f;  // Kuinka paljon aaltoilee y-suunnassa
    //public float flapFrequency = 2f;    // Kuinka usein "siipi iskee"
    private float pathProgress = 0f;
    private float baseY;
    void Start()
    {
        baseY = transform.position.y;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        startRotationZ = transform.eulerAngles.z;

        if (sijaintijostatutkitaan == null)
            sijaintijostatutkitaan = transform;

        verticalCheckDistance = CalculateCombinedColliderHalfHeight();

        /*
        currentImpulse = liftForce;
        targetHeight = transform.position.y;
        smoothedTargetHeight = targetHeight;
        averageY = targetHeight;
        */

    }

    private float CalculateCombinedColliderHalfHeight()
    {
        BoxCollider2D[] colliders = GetComponentsInChildren<BoxCollider2D>();
        if (colliders.Length == 0)
            return 1f;

        Bounds combined = colliders[0].bounds;
        foreach (BoxCollider2D c in colliders)
            combined.Encapsulate(c.bounds);

        return combined.extents.y;
    }

    public void OnFlapVanha()
    {
        if (rb.velocity.y<0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }


        rb.AddForce(Vector2.up * liftForce, ForceMode2D.Impulse);
        rb.AddForce(Vector2.right * forwardForce, ForceMode2D.Force);

        if (rb.velocity.x > 0.1f)
            rb.velocity = new Vector2(0.1f, rb.velocity.y);


   
        isReturningRotation = true;
    }






    public void OnFlap()
    {
        // Nollaa pystynopeus vain jos ollaan putoamassa,
        // jolloin siivenisku ei "törmää" alaspäin suuntautuvaan nopeuteen.
        if (rb.velocity.y < 0)
            rb.velocity = new Vector2(rb.velocity.x, 0);

        // Symmetrinen X-nopeuden rajoitus molempiin suuntiin
        //  float clampedX = Mathf.Clamp(rb.velocity.x, -xvelocitymaxlimit, xvelocitymaxlimit);
        //  rb.velocity = new Vector2(clampedX, rb.velocity.y);
       // isReturningRotation = true;
        // Käynnistetään monipulssinen siivenisku
        StartCoroutine(FlapRoutine());
       // isReturningRotation = true;
    }
    public float alasvoima = 2.0f;
    public void VoimaAlaspain()
    {
     //   rb.AddForce(Vector2.down * alasvoima, ForceMode2D.Impulse);
    }

    private IEnumerator FlapRoutine()
    {
        float delay = totalDuration / pulses;
        float forcePerPulse = liftForce / pulses;
        float forwardPerPulse = forwardForce / pulses;

        // Tiltti takaisin normaaliin, jos oli aiemmin dodge-tilassa
        isReturningRotation = false;

        // Aktivoi hetkellinen siiveniskun kallistus
        StartCoroutine(HandleFlapTilt());
        for (int i = 0; i < pulses; i++)
        {
            // Ensimmäinen pulssi: nopea, terävä impulssi
            ForceMode2D mode = (i == 0) ? ForceMode2D.Force  : ForceMode2D.Impulse;
           // ForceMode2D mode = ForceMode2D.Impulse;
            // Lisätään nostovoima ja eteneminen
           // if (!isDodging)
                rb.AddForce(Vector2.up * forcePerPulse, mode);
            if (isDodging)
            {
                rb.AddForce(Vector2.right * forwardPerPulse/2.0f, mode);

            }
            else
            {
                rb.AddForce(Vector2.right * forwardPerPulse, mode);
            }

            yield return new WaitForSeconds(delay);
        }
        // Kun flappi loppuu, palautetaan kallistus pehmeästi
        isReturningRotation = true;
    }

    private IEnumerator HandleFlapTilt()
    {
        // Tavoitekallistus – käännä positiivinen arvo negatiiviseksi jos lintu on “ylösalaisin”
        //float flapTiltAngle = 25f; // paljonko lintu kallistuu ylöspäin siivenlyönnin aikana
        //float tiltSpeed = 10f;     // kallistuksen nopeus

        // Nosta kallistus ylöspäin
        float elapsed = 0f;
        while (elapsed < 0.1f)
        {
            float tilt = Mathf.LerpAngle(transform.eulerAngles.z, -flapTiltAngle, elapsed * tiltSpeed);
            transform.rotation = Quaternion.Euler(0, 0, tilt);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Palautus aloitetaan heti kun OnFlap päättyy (isReturningRotation = true)
    }

    void FixedUpdate()
    {
        Vector2 v = rb.velocity;



        v.x = Mathf.Clamp(v.x, -maxSpeedX, maxSpeedX);
        v.y = Mathf.Clamp(v.y, -maxSpeedY, maxSpeedY);

        rb.velocity = v;


        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxAngularSpeed, maxAngularSpeed);
        if (kulkupolku!=null && kuljetaanpolkua)
        {
                 Kuljepolkua();
            return;
        }

        // Rajoitetaan rigidbody nopeus
        KuljeIlmanPolkua();
        KeepInCameraView();
        
     //   rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxAngularSpeed, maxAngularSpeed);
      



       // v.x = Mathf.Clamp(v.x, -maxSpeedX, maxSpeedX);
        v.y = Mathf.Clamp(v.y, -maxSpeedY, maxSpeedY);



        Vector2 pathPos = kulkupolku.GetPosition(pathProgress);
        Vector2 futurePos = kulkupolku.GetPosition(pathProgress + 0.1f);
        Vector2 dir = (futurePos - pathPos).normalized;

        if (dir.sqrMagnitude > 0.0001f)
        {
            float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180f;
            float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, Time.fixedDeltaTime * 5f);
            transform.rotation = Quaternion.Euler(0, 0, smoothAngle);
            // --- FlipY jos lentää vasemmalle ---
            // Tämä pitää linnun "pää ylhäällä", vaikka se kääntyisi ympäri
            SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>();
            bool flipx = kulkupolku.FlipY(pathProgress);
            foreach (SpriteRenderer spriteRenderer in sr)
            {
                if (spriteRenderer != null)
                {
                    bool flipY = targetAngle > 90f || targetAngle < -90f;
                    spriteRenderer.flipY = flipx;
                }
            }
        }

        //   rb.velocity = v;


    }

    private void Kuljepolkua2()
    {
        // Liiku reittiä pitkin X-akselilla
        pathProgress += pathSpeed * Time.fixedDeltaTime;
        Vector2 pathPos = kulkupolku.GetPosition(pathProgress);



        // "Tavoitekorkeus" on reitin korkeus + aaltoilu
        float targetY = pathPos.y;// + Mathf.Sin(Time.time * flapFrequency) * flapAmplitude;

        // Fysiikkavoima joka hakee kohti reittiä
        float diff = targetY - transform.position.y;
        rb.AddForce(Vector2.up * diff * followStrength, ForceMode2D.Force);

        // X-asema seuraa reittiä suoraan (voi myös interpoloida)
        transform.position = new Vector2(pathPos.x, transform.position.y);
    }
    int laskuri = 0;
    private void Kuljepolkua()
    {
        pathProgress += pathSpeed * Time.fixedDeltaTime;

        //@todoo just return 


        laskuri++;
        if (laskuri<4)
        {
            return;
        }

        laskuri = 0;

        Vector2 pathPos = kulkupolku.GetPosition(pathProgress);

       // bool flipx = kulkupolku.FlipY(pathProgress);
       // GetComponent<SpriteRenderer>().flipY = flipx;
        if (pathPos==Vector2.zero)
        {
            //PalautaTakaisinKulkupolkuun();
            //StartCoroutine(PalautaTakaisinKulkupolkuun());
            PalatauTakaisinKulkupolkuunIlmanViivetta();

            return;
        }

        


        // "Tavoitekorkeus" on reitin korkeus
        float targetY = pathPos.y;

        // Fysiikkavoima joka hakee kohti reittiä
        float diff = targetY - transform.position.y;

        Vector2 normal = (  pathPos- (Vector2)transform.position).normalized;
        rb.AddForce(normal*followStrength, ForceMode2D.Force);



        /*
        rb.AddForce(Vector2.up * diff * followStrength, ForceMode2D.Force);

        // ✅ X-asema fysiikan kautta (ei ohita törmäyksiä)
        //Vector2 newPos = new Vector2(pathPos.x, transform.position.y);
        //rb.MovePosition(newPos);

        float followSmoothness = 50f;

        float followSmoothnesswhenpathwhenpathposypienempikuintransformpositiony = 50;

        Vector2 currentPos = rb.position;
        Vector2 targetPos = new Vector2(pathPos.x, transform.position.y);
        Vector2 smoothedPos;
        if (pathPos.y<transform.position.y)
        {

            smoothedPos = Vector2.Lerp(currentPos, targetPos, Time.fixedDeltaTime *
                followSmoothnesswhenpathwhenpathposypienempikuintransformpositiony);
        }
        else
        {

            smoothedPos = Vector2.Lerp(currentPos, targetPos, Time.fixedDeltaTime * followSmoothness);
        }

        rb.MovePosition(smoothedPos);
        */


        // 🔥 ROTATION – käännetään linnun nenä lentosuuntaan
        Vector2 futurePos = kulkupolku.GetPosition(pathProgress + 0.1f);
        Vector2 dir = (futurePos - pathPos).normalized;

        if (dir.sqrMagnitude > 0.0001f)
        {
            float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180f;
            float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, Time.fixedDeltaTime * 5f);
            transform.rotation = Quaternion.Euler(0, 0, smoothAngle);
            // --- FlipY jos lentää vasemmalle ---
            // Tämä pitää linnun "pää ylhäällä", vaikka se kääntyisi ympäri
            SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>();
             bool flipx = kulkupolku.FlipY(pathProgress);
            foreach (SpriteRenderer spriteRenderer in sr)
            {
                if (spriteRenderer != null)
                {
                    bool flipY = targetAngle > 90f || targetAngle < -90f;
                    spriteRenderer.flipY = flipx;
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.collider.tag.Contains("ammustag"))
        {
            rb.AddForce(Vector2.down * 1f, ForceMode2D.Impulse);
        }
            // Jos törmätään "tiileen", poistutaan reitiltä ja siirrytään väistötilaan
        if (kuljetaanpolkua && collision.collider.tag.Contains("tiili") )
        {
            Debug.Log($"{name} törmäsi tiileen – poistutaan reitiltä ja aloitetaan väistö.");
            kuljetaanpolkua = false;
            //kulkupolku = null; // Tämä käynnistää automaattisesti KuljeIlmanPolkua() -tilan
            isDodging = false; // nollataan mahdollinen väistön tila
            isReturningRotation = true; // palautetaan rotaatio
            //rb.velocity = Vector2.zero; // pysäytetään hetkeksi
            StartCoroutine(PalautaTakaisinKulkupolkuun());
            // Halutessasi voit lisätä pienen pompun / reaktion törmäykseen:
            rb.AddForce(Vector2.down * 1f, ForceMode2D.Impulse);
        }
    }

    private IEnumerator PalautaTakaisinKulkupolkuunfdfd()
    {
        yield return new WaitForSeconds(2);
        float sallittuero = 1.0f;
        for (int i=0;i<kulkupolku.Length;i++)
        {
            Transform possi = kulkupolku.points[i];
            Vector2 v = possi.position;

            float distance = Vector2.Distance(transform.position, v);
            if (distance <= 0.5f)
            {
                //set pathprogress so that it points to that position in kulkupolku points
                //kulkupolku/path
            }
        }


        kuljetaanpolkua = true;
    }


    private void PalatauTakaisinKulkupolkuunIlmanViivetta()
    {

        // Etsitään lähin piste reitiltä
        float closestDist = Mathf.Infinity;
        int closestIndex = -1;

        for (int i = 0; i < kulkupolku.points.Count; i++)
        {
            Transform possi = kulkupolku.points[i];
            float distance = Vector2.Distance(transform.position, possi.position);

            if (distance < closestDist)
            {
                closestDist = distance;
                closestIndex = i;
            }
        }

        // Jos löytyi sopiva piste
        if (closestIndex >= 0)
        {
            // Asetetaan pathProgress lähelle tuota pistettä
            // Huomaa: Path.GetPosition käyttää float-arvoa, jossa 1 = segmentti 0–1, 2 = segmentti 1–2, jne.
            pathProgress = closestIndex;

            Debug.Log($"Palautetaan reitille, lähin piste: {closestIndex}, etäisyys: {closestDist:F2}");
        }

        kuljetaanpolkua = true; // jos sinulla on oma bool, joka kontrolloi sitä
    }

    private IEnumerator PalautaTakaisinKulkupolkuun()
    {
        if (kulkupolku == null || kulkupolku.points == null || kulkupolku.points.Count == 0)
            yield break;

        // Odota hetki ennen paluuta reitille
        yield return new WaitForSeconds(1f);

        PalatauTakaisinKulkupolkuunIlmanViivetta();
    }


    void KuljeIlmanPolkua()
    {

        if (dodgeCooldownTimer > 0)
            dodgeCooldownTimer -= Time.deltaTime;

        if (!isDodging && dodgeCooldownTimer <= 0)
            DetectAndDodgeObstacle();

        HandleRotation();

        if (isDodging)
            HandleDodge();

        KeepInCameraView();

      //  Update2();
    }
    /*

    [Header("Altitude Control")]
    [Tooltip("Tavoitekorkeus, jota toinen skripti voi muuttaa vapaasti.")]
    //laitetaan tuohon alus
    public float targetHeight = 0f;
    public float adjustSpeed = 0.2f;
    [Range(0f, 1f)]
    public float smoothFactor = 0.9f;
    public float targetLerpSpeed = 0.5f;


    public float currentImpulse;
    private float averageY;
    private float smoothedTargetHeight;

    [Header("Flap Settings")]
 //   public float baseImpulse = 5f;
    public float maxImpulse = 10f;
    public float minImpulse = 0f;

    public float smoothlaskuri = 0.0f;
    void Update2()
    {
        smoothlaskuri += Time.deltaTime;

        if (smoothlaskuri>=1.0f)
        {
            smoothlaskuri = 0.0f;
            // Pehmennetään tavoitekorkeutta
            smoothedTargetHeight = Mathf.Lerp(smoothedTargetHeight, targetHeight, Time.deltaTime * targetLerpSpeed);

            // Suodatetaan todellinen korkeus
            averageY = Mathf.Lerp(averageY, transform.position.y, 1f - smoothFactor);

            // Lasketaan virhe
            float heightError = smoothedTargetHeight - averageY;

            // Päivitetään impulssi virheen perusteella
            currentImpulse += heightError * adjustSpeed * Time.deltaTime * 60f;

            // Clamp
            currentImpulse = Mathf.Clamp(currentImpulse, minImpulse, maxImpulse);
        }

    }
    */


    private void DetectAndDodgeObstacle()
    {
        verticalCheckDistance = CalculateCombinedColliderHalfHeight();

        float totalHeight = verticalCheckDistance * 2f;
        float step = totalHeight / (rayCount - 1);

        bool obstacleFound = false;
        Transform hitObstacle = null;

        // 🔎 Useita raycasteja eteen eri korkeuksilta
        for (int i = 0; i < rayCount; i++)
        {
            float offsetY = -verticalCheckDistance + (step * i);
            Vector2 origin = sijaintijostatutkitaan.position + new Vector3(0, offsetY, 0);

            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.left, detectionDistance, obstacleLayer);
            Debug.DrawLine(origin, origin + Vector2.left * detectionDistance, Color.red);

            if (hit.collider != null && hit.collider.tag.Contains("tiili"))
            {
                Transform obstacle = hit.collider.transform;

                /*
                if (obstacle.position.x >= transform.position.x)
                    continue;
                */

                /*
                if (lastDodgedObstacle != null && obstacle == lastDodgedObstacle)
                    continue;
                */

                obstacleFound = true;
                hitObstacle = obstacle;
                break;
            }
        }

        if (!obstacleFound)
            return;

        Transform obs = hitObstacle;

        bool upClear = !Physics2D.Raycast(transform.position, Vector2.up, verticalCheckDistance, obstacleLayer);
        bool downClear = !Physics2D.Raycast(transform.position, Vector2.down, verticalCheckDistance, obstacleLayer);

        if (upClear && !downClear)
        {
            StartDodge(Vector2.up, obs);
            return;
        }
        else if (downClear && !upClear)
        {
            StartDodge(Vector2.down, obs);
            return;
        }

        // 🧠 Smart Lookahead: testaa molemmat reitit (ylös/alas → vasemmalle)
        float lookAheadOffset = verticalCheckDistance * sidecheckinkerto;
        float sideCheckDistance = detectionDistance * sidecheckinkerto;

        upTestOrigin = transform.position + Vector3.up * lookAheadOffset;
        upPathBlocked = Physics2D.Raycast(upTestOrigin, Vector2.left, sideCheckDistance, obstacleLayer);

        downTestOrigin = transform.position + Vector3.down * lookAheadOffset;
        downPathBlocked = Physics2D.Raycast(downTestOrigin, Vector2.left, sideCheckDistance, obstacleLayer);

        Debug.DrawLine(upTestOrigin, upTestOrigin + Vector2.left * sideCheckDistance, upPathBlocked ? Color.red : Color.green);
        Debug.DrawLine(downTestOrigin, downTestOrigin + Vector2.left * sideCheckDistance, downPathBlocked ? Color.red : Color.green);

        if (!upPathBlocked && downPathBlocked)
        {
            StartDodge(Vector2.up, obs);
        }
        else if (!downPathBlocked && upPathBlocked)
        {
            StartDodge(Vector2.down, obs);
        }
        else if (!upPathBlocked && !downPathBlocked)
        {
            float cameraTop = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
            float cameraBottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
            float distToTop = cameraTop - transform.position.y;
            float distToBottom = transform.position.y - cameraBottom;

            StartDodge(distToTop > distToBottom ? Vector2.up : Vector2.down, obs);
        }
        else
        {
            StartDodge(Vector2.up, obs);
        }
    }

    public float sidecheckinkerto = 1.5f;

    private void StartDodge(Vector2 direction, Transform obstacle)
    {
        isDodging = true;
        dodgeTimer = dodgeDuration;
        dodgeDirection = direction;
        lastDodgedObstacle = obstacle;

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(direction * dodgeLift, ForceMode2D.Impulse);

        float tilt = (direction == Vector2.up) ? -dodgeTiltAngle : dodgeTiltAngle;
        transform.rotation = Quaternion.Euler(0, 0, tilt);

        if (animator)
            animator.SetTrigger("Dodge");
    }

    private void HandleDodge()
    {
        dodgeTimer -= Time.deltaTime;
        rb.AddForce(dodgeDirection * dodgeForce, ForceMode2D.Force);

        if (dodgeTimer <= 0)
        {
            isDodging = false;
            isReturningRotation = true;
            dodgeCooldownTimer = dodgeCooldown;
        }
    }

    private void HandleRotation()
    {
        if (isReturningRotation)
        {
            float currentZ = transform.eulerAngles.z;
            float newZ = Mathf.LerpAngle(currentZ, startRotationZ, Time.deltaTime * rotationReturnSpeed);
            transform.rotation = Quaternion.Euler(0, 0, newZ);

            if (Mathf.Abs(Mathf.DeltaAngle(currentZ, startRotationZ)) < 0.5f)
            {
                transform.rotation = Quaternion.Euler(0, 0, startRotationZ);
                isReturningRotation = false;
            }
        }
        else if (!isDodging)
        {
            float tilt = Mathf.Clamp(rb.velocity.y * tiltMultiplier, -maxTilt, maxTilt);
            transform.rotation = Quaternion.Euler(0, 0, tilt);
        }
    }

    private void KeepInCameraView()
    {
        Vector3 pos = transform.position;
        float camTop = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.95f, 0)).y;
        float camBottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.05f, 0)).y;
        pos.y = Mathf.Clamp(pos.y, camBottom, camTop);
        transform.position = pos;
    }

    void OnDrawGizmosSelected()
    {
        if (sijaintijostatutkitaan == null)
            return;

        float vcd = CalculateCombinedColliderHalfHeight();
        float totalHeight = vcd * 2f;
        float step = (rayCount > 1) ? totalHeight / (rayCount - 1) : 0f;

        // 🔶 Eturaycastit eri korkeuksilla
        Gizmos.color = Color.yellow;
        for (int i = 0; i < rayCount; i++)
        {
            float offsetY = -vcd + (step * i);
            Vector3 origin = sijaintijostatutkitaan.position + new Vector3(0, offsetY, 0);
            Gizmos.DrawLine(origin, origin + Vector3.left * detectionDistance);
            Gizmos.DrawSphere(origin, 0.05f);
        }

        // 🟩 Pystytarkistukset
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * vcd);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * vcd);

        // 🔴🟢 Lookahead-reittien testiviivat
        float sideCheckDistance = detectionDistance * sidecheckinkerto;

        if (upTestOrigin != Vector2.zero)
        {
            Gizmos.color = upPathBlocked ? Color.red : Color.green;
            Gizmos.DrawLine(upTestOrigin, upTestOrigin + Vector2.left * sideCheckDistance);
        }

        if (downTestOrigin != Vector2.zero)
        {
            Gizmos.color = downPathBlocked ? Color.red : Color.green;
            Gizmos.DrawLine(downTestOrigin, downTestOrigin + Vector2.left * sideCheckDistance);
        }
    }
}
