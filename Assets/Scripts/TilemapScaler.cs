using UnityEngine;

public class TilemapScaler : MonoBehaviour
{
    public Camera mainCamera; // Assign the main camera
    public int horizontalTiles = 5; // Desired number of horizontal tiles
    public int verticalTiles = 5; // Desired number of vertical tiles
    public float tileSize = 1f; // Size of a single tile (assuming square tiles)

    void Start()
    {
        ScaleParentToFit();
    }
    private void Update()
    {
        ScaleParentToFit();
    }
    void ScaleParentToFit()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Get the camera's orthographic size
        float verticalSize = mainCamera.orthographicSize * 2; // Total vertical size
        float horizontalSize = verticalSize * Screen.width / Screen.height; // Total horizontal size

        // Calculate the required scale for the parent object
        float horizontalScale = horizontalSize / (horizontalTiles * tileSize);
        float verticalScale = verticalSize / (verticalTiles * tileSize);

        // Apply the scale uniformly
        float scale = Mathf.Min(horizontalScale, verticalScale);

        // Set the parent object's scale
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}
