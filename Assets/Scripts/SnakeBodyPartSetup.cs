using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SnakeBodyPartSetup : MonoBehaviour
{
    void Awake()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, Vector3.zero);
        lr.SetPosition(1, new Vector3(0.5f, 0, 0));
        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.green;
        lr.endColor = Color.green;
        lr.sortingOrder = 5;
    }
}
