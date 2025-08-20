using UnityEngine;
using System.Diagnostics;

public class DebugStats : MonoBehaviour
{
    private float deltaTime = 0.0f;
    private GUIStyle style;
    private Rect rect;
    private Process currentProcess;

    void Start()
    {
        style = new GUIStyle();
        style.fontSize = 32;
        style.normal.textColor = Color.green;

        rect = new Rect(10, 10, Screen.width, 200);
        currentProcess = Process.GetCurrentProcess();
    }

    void Update()
    {
        // liukuva keskiarvo FPS:lle
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        float fps = 1.0f / deltaTime;
        int activeObjects = FindObjectsOfType<GameObject>().Length;

        // muistin käyttö (vain editorissa ja standalone buildissa näkyy)
        long memoryUsed = currentProcess.PrivateMemorySize64 / (1024 * 1024); // MB

        string text = string.Format(
            "FPS: {0:0.0}\nActive Objects: {1}\nMemory: {2} MB\nFixedDeltaTime: {3:0.000}",
            fps, activeObjects, memoryUsed, Time.fixedDeltaTime);

        GUI.Label(rect, text, style);
    }
}
