using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScrollingTMPConsole : MonoBehaviour
{
    public TMP_Text tmpText;        // assign in inspector
    public int maxLines = 5;

    private Queue<string> lines = new Queue<string>();

    public void AddLine(string newLine)
    {
        // enqueue new line
        lines.Enqueue(newLine);

        // keep only last N
        while (lines.Count > maxLines)
            lines.Dequeue();

        // rebuild text
        tmpText.text = string.Join("\n", lines.ToArray());
    }

    public void Clear()
    {
        lines.Clear();
        tmpText.text = "";
    }
}
