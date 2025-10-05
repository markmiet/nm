using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickControl : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform joystickBG;   // Joystick background
    public RectTransform joystickKnob; // Joystick knob
    public GameObject spaceship;       // Spaceship (world sprite)
    public Camera mainCamera;          // Main camera

    private Vector2 inputVector;
    public float moveSpeed = 5f;   // responsiveness to joystick input

    private float shipWidth;
    private float shipHeight;

    void Start()
    {
        if (spaceship != null)
        {
            // Get spaceship size in world units
            SpriteRenderer sr = spaceship.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                shipWidth = sr.bounds.size.x;
                shipHeight = sr.bounds.size.y;
            }
        }

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    // --- Touch/Pointer events ---
    public void OnDrag(PointerEventData eventData)
    {
        UpdateJoystick(eventData.position, eventData.pressEventCamera);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateJoystick(eventData.position, eventData.pressEventCamera);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        joystickKnob.anchoredPosition = Vector2.zero;
    }

    // --- Mouse Fallback (works in Editor/PC) ---
    void Update()
    {
        if (Input.GetMouseButton(0)) // left mouse held down
        {
            UpdateJoystick(Input.mousePosition, null);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            inputVector = Vector2.zero;
            joystickKnob.anchoredPosition = Vector2.zero;
        }

        if (spaceship == null || mainCamera == null) return;

        // --- Camera viewport size in world units ---
        float camHeight = mainCamera.orthographicSize * 2f;
        float camWidth = camHeight * mainCamera.aspect;

        Vector3 camPos = mainCamera.transform.position;

        // --- Calculate offset inside camera bounds ---
        float offsetX = inputVector.x * (camWidth / 2f - shipWidth / 2f);
        float offsetY = inputVector.y * (camHeight / 2f - shipHeight / 2f);

        // --- Target position: camera center + offset ---
        Vector3 targetPos = new Vector3(
            camPos.x + offsetX,
            camPos.y + offsetY,
            spaceship.transform.position.z
        );

        // Smooth move toward target
        spaceship.transform.position = Vector3.Lerp(
            spaceship.transform.position,
            targetPos,
            Time.deltaTime * moveSpeed
        );
    }

    // --- Helper: Update joystick position ---
    private void UpdateJoystick(Vector2 screenPos, Camera eventCamera)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBG,
            screenPos,
            eventCamera,
            out pos
        );

        pos /= joystickBG.sizeDelta / 2f;
        inputVector = (pos.magnitude > 1f) ? pos.normalized : pos;

        joystickKnob.anchoredPosition = new Vector2(
            inputVector.x * (joystickBG.sizeDelta.x / 2f),
            inputVector.y * (joystickBG.sizeDelta.y / 2f)
        );
    }
}
