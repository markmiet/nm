using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;
    GUIStyle style;
    Rect rect;

    public int fontsize = 22;
    void Start()
    {
        // Define text style
        style = new GUIStyle();
        style.fontSize = fontsize;
        style.normal.textColor = Color.white;

        // Define text position (top-left corner)
        rect = new Rect(100 , 100, 200, 40);
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
        string text = string.Format("{0:0.}", myInt);
        GUI.Label(rect, text, style);
#endif
    }
}
