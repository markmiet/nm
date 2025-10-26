using UnityEngine;

/// <summary>
/// Handles smooth 2D gun aiming and positioning for a skeleton character.
/// Keeps the grip point locked to the hand, smoothly rotates toward the target,
/// and flips the sprite when aiming left.
/// </summary>
public class GunAimer2D : BaseController
{
    [Header("Gun Setup")]
    [Tooltip("The character's hand bone or transform controlled by IK or animation.")]
    public Transform hand;          // Käden sijainti

    [Tooltip("The gun's transform (parent of the gun sprite).")]
    public Transform gun;           // Aseen transformi

    [Tooltip("Grip point on the gun (where the hand holds it).")]
    public Transform handPoint;     // Otekohta aseessa

    [Tooltip("Rear sight (back of the gun).")]
    public Transform takatahtain;   // Takatahtain

    [Tooltip("Muzzle (barrel end of the gun).")]
    public Transform piippu;        // Piippu / suuliekki

    [Header("Target")]
    [Tooltip("The object to aim toward (enemy, player, or crosshair).")]
    public Transform target;        // Kohde johon tähdätään

    [Header("Settings")]
    [Tooltip("Flip the gun sprite when aiming left.")]
    public bool flipWithDirection = true;  // Käännä sprite kun tähdätään vasemmalle

    [Tooltip("Reference to the skeleton's IK aiming controller.")]
    public SkeletonAiming2DIK skeletonAiming2DIK;

    [Tooltip("How quickly the gun rotates toward the target.")]
    [Range(0.1f, 20f)]
    public float rotationSpeed = 10f;

    [Tooltip("Optional neutral rotation when not aiming (degrees).")]
    public float neutralAngle = 0f;

    [Tooltip("How quickly the gun returns to neutral when not aiming.")]
    public float relaxSpeed = 3f;

    [Tooltip("Show debug lines in Scene view.")]
    public bool showDebug = true;

    private float currentAngle;
    public DirectionSpriteSwitcher directionSpriteSwitcher;

    public void Start()
    {
        target = PalautaAlus().transform;
    }

    private void LateUpdate()
    {
        if (!hand || !gun || !handPoint || !takatahtain || !piippu)
            return;


        if (directionSpriteSwitcher.currentState == DirectionSpriteSwitcher.State.IdleCenter)
        {
            return;
        }

        bool isAiming = skeletonAiming2DIK && skeletonAiming2DIK.canSeePlayer;
        float aimBlend = 1f;

        if (skeletonAiming2DIK && skeletonAiming2DIK != null)
        {
           aimBlend = skeletonAiming2DIK.aimWeight; // blend with IK influence
        }
            

        // === 1. Compute target angle ===
        float targetAngle = neutralAngle;
        if (isAiming && target)
        {
            Vector2 dir = (target.position - takatahtain.position).normalized;
            targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float parentSign = Mathf.Sign(transform.lossyScale.x);
            if (parentSign<0)
            {
                targetAngle += 180;
            }
            
        }

        // === 2. Smoothly interpolate rotation ===
        float speed = isAiming ? rotationSpeed : relaxSpeed;
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * speed);
        float finalAngle = Mathf.LerpAngle(neutralAngle, currentAngle, aimBlend);

        gun.rotation = Quaternion.Euler(0, 0, finalAngle);
       // gun.rotation = Quaternion.Euler(0, 0, targetAngle);



        // === 3. Keep grip point locked to hand position ===
        Vector3 gripWorldPos = hand.position;
        gun.position += (gripWorldPos - handPoint.position);

        // === 4. Flip sprite if aiming left ===
        if (flipWithDirection)
        {
            Vector3 scale = gun.localScale;
            if (Mathf.Abs(finalAngle) > 90f)
                scale.x = -Mathf.Abs(scale.x);
            else
                scale.x = Mathf.Abs(scale.x);
            gun.localScale = scale;
        }

        // === 5. Debug visualization ===
        if (showDebug)
        {
            Debug.DrawLine(takatahtain.position, piippu.position, Color.green);
            if (target) Debug.DrawLine(piippu.position, target.position, Color.yellow);
        }
    }
}
