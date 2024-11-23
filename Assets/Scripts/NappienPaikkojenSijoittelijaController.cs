using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NappienPaikkojenSijoittelijaController : MonoBehaviour
{ 
    [Header("GameObject References")]
    public RectTransform firstGameObject;   // Parent GameObject with RectTransform
    public RectTransform secondGameObject; // Child GameObject with RectTransform
    public GameObject thirdGameObject;     // Third GameObject without RectTransform (non-UI)

    [Header("Offset Settings")]
    public float padding = 10f; // Space between the second and third GameObjects (optional)

    void Start()
    {
        if (firstGameObject == null || secondGameObject == null || thirdGameObject == null)
        {
            Debug.LogError("Missing required GameObject references!");
            return;
        }

        // Position the second GameObject and resize it to be square
    //    PositionAndResizeSecondGameObjectSquare();
    }

    void PositionAndResizeSecondGameObjectSquare()
    {
        // Get the world position of the third GameObject
        Vector3 thirdWorldPosition = thirdGameObject.transform.position;

        // Convert the third GameObject's world position to screen space
        Camera mainCamera = Camera.main;
        Vector3 thirdScreenPosition = mainCamera.WorldToScreenPoint(thirdWorldPosition);

        // Debugging: Log the world and screen position of the third GameObject
        Debug.Log("Third GameObject World Position: " + thirdWorldPosition);
        Debug.Log("Third GameObject Screen Position: " + thirdScreenPosition);

        // Convert the screen space position to local Canvas space
        Canvas parentCanvas = firstGameObject.GetComponentInParent<Canvas>();
        if (parentCanvas == null)
        {
            Debug.LogError("Parent Canvas not found!");
            return;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            firstGameObject,
            thirdScreenPosition,
            parentCanvas.worldCamera,
            out Vector2 thirdLocalPosition
        );

        // Debugging: Log the local position of the third GameObject in Canvas space
        Debug.Log("Third GameObject Local Position in Canvas: " + thirdLocalPosition);

        // Calculate the space above the third GameObject (top of the third GameObject to the top of the canvas)
        float spaceAboveThird = Mathf.Abs(thirdLocalPosition.y) + padding;

        // Debugging: Log the available space above the third GameObject
        Debug.Log("Corrected Space Above Third GameObject: " + spaceAboveThird);

        // Now, the width of the second GameObject will be the same as the height (making it a square)
        float availableWidth = firstGameObject.rect.width; // Get the parent width in local space

        // Debugging: Log the available width of the parent
        Debug.Log("Available Width of Parent GameObject: " + availableWidth);

        // Calculate the new square size for the second GameObject to fill space above the third GameObject
        float newSize = Mathf.Min(spaceAboveThird, availableWidth); // Ensure the size is constrained by both available space above and width

        // Debugging: Log the calculated new size for the second GameObject
        Debug.Log("Calculated New Size for Second GameObject: " + newSize);

        // Set the second GameObject's position and size to fill the space above the third GameObject
        secondGameObject.anchorMin = new Vector2(0.5f, 1); // Center horizontally, align to top
        secondGameObject.anchorMax = new Vector2(0.5f, 1); // Same as anchorMin to keep it in place
        secondGameObject.pivot = new Vector2(0.5f, 1);    // Pivot at the top center for proper alignment

        // Apply the square size to both width and height
        secondGameObject.sizeDelta = new Vector2(newSize, newSize);

        // Set the Y position to place the second GameObject just above the third GameObject
        Vector2 newPosition = secondGameObject.anchoredPosition;
        newPosition.y = -(spaceAboveThird); // Position it above the third GameObject
        secondGameObject.anchoredPosition = newPosition;

        // Debugging: Log the final position and size of the second GameObject
        Debug.Log("Final Position of Second GameObject: " + secondGameObject.anchoredPosition);
        Debug.Log("Final Size of Second GameObject: " + secondGameObject.sizeDelta);
    }
}
