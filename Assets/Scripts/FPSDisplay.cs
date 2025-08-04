using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;
    GUIStyle style;
    Rect rect;

    public int fontsize = 18;
    void Start()
    {
        style = new GUIStyle();
        style.fontSize = fontsize;
        style.normal.textColor = Color.white;

        // Bottom-left corner position
        float padding = 40f;
        rect = new Rect(
            padding,                                   // X: left edge + padding
            Screen.height - fontsize - padding,        // Y: bottom edge - font size - padding
            300,                                       // Width (adjust as needed)
            40                                         // Height (adjust as needed)
        );
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    
    void OnGUI()
    {
#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS

        float fps = 1.0f / deltaTime;
        int myInt = Mathf.CeilToInt(fps);
        float difficulty = GameManager.Instance.PalautaDifficulty();

        string text = string.Format("{0} FPS / Difficulty: {1:0.00}", myInt, difficulty);
        GUI.Label(rect, text, style);
#endif
    }
    
}
