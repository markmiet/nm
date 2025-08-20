using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AutoSpriteSlicer : EditorWindow
{
    Texture2D texture;
    GameObject partPrefab;
    int tileWidth = 16;
    int tileHeight = 16;
    float pixelsPerUnit = 100f;
    string outputName = "AutoSlicedObject";
    bool createJoints = true;

    [MenuItem("Tools/Auto Sprite Slicer")]
    static void Init()
    {
        AutoSpriteSlicer window = (AutoSpriteSlicer)GetWindow(typeof(AutoSpriteSlicer));
        window.titleContent = new GUIContent("Auto Sprite Slicer");
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Auto Slice Texture2D", EditorStyles.boldLabel);
        texture = (Texture2D)EditorGUILayout.ObjectField("Texture", texture, typeof(Texture2D), false);
        partPrefab = (GameObject)EditorGUILayout.ObjectField("Part Prefab", partPrefab, typeof(GameObject), false);
        tileWidth = EditorGUILayout.IntField("Tile Width (px)", tileWidth);
        tileHeight = EditorGUILayout.IntField("Tile Height (px)", tileHeight);
        pixelsPerUnit = EditorGUILayout.FloatField("Pixels Per Unit", pixelsPerUnit);
        outputName = EditorGUILayout.TextField("Output Name", outputName);
        createJoints = EditorGUILayout.Toggle("Create Fixed Joints", createJoints);

        if (GUILayout.Button("Slice and Generate"))
        {
            if (texture == null || partPrefab == null)
            {
                Debug.LogError("Please assign both texture and prefab.");
                return;
            }

            GenerateFromTexture();
        }
    }

    void GenerateFromTexture()
    {
        int cols = texture.width / tileWidth;
        int rows = texture.height / tileHeight;
        GameObject container = new GameObject(outputName);

        GameObject[,] grid = new GameObject[cols, rows];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                Rect rect = new Rect(x * tileWidth, y * tileHeight, tileWidth, tileHeight);

                if (IsRectTransparent(texture, rect))
                    continue;

                Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), pixelsPerUnit);
                GameObject part = (GameObject)PrefabUtility.InstantiatePrefab(partPrefab);
                part.name = $"Part_{x}_{y}";
                Vector2 position = new Vector2(x * tileWidth / pixelsPerUnit, y * tileHeight / pixelsPerUnit);
                part.transform.position = position;
                part.transform.SetParent(container.transform);

                SpriteRenderer sr = part.GetComponent<SpriteRenderer>();
                if (sr != null) sr.sprite = sprite;

                BoxCollider2D collider = part.GetComponent<BoxCollider2D>();
                if (collider == null) collider = part.AddComponent<BoxCollider2D>();
                //saadan collideria pienemmiksi eli 0,8
                //todooo testaaa
                collider.size = new Vector2(rect.width / pixelsPerUnit, rect.height / pixelsPerUnit) * 0.8f;
              
                collider.offset = Vector2.zero;

                Rigidbody2D rb = part.GetComponent<Rigidbody2D>();
                if (rb == null) rb = part.AddComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 1f;

                grid[x, y] = part;
            }
        }

        if (createJoints)
        {
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    GameObject current = grid[x, y];
                    if (current == null) continue;

                    Rigidbody2D rb = current.GetComponent<Rigidbody2D>();
                    if (rb == null) continue;

                    Vector2 offset = new Vector2(tileWidth / pixelsPerUnit, tileHeight / pixelsPerUnit);

                    // Right neighbor
                    if (x + 1 < cols && grid[x + 1, y] != null)
                        CreateJoint(rb, grid[x + 1, y].GetComponent<Rigidbody2D>());

                    // Top neighbor
                    if (y + 1 < rows && grid[x, y + 1] != null)
                        CreateJoint(rb, grid[x, y + 1].GetComponent<Rigidbody2D>());
                }
            }
        }

        Debug.Log("Auto-slicing complete!");
    }

    void CreateJoint(Rigidbody2D from, Rigidbody2D to)
    {
        FixedJoint2D joint = from.gameObject.AddComponent<FixedJoint2D>();
        joint.connectedBody = to;
        joint.autoConfigureConnectedAnchor = true;
        joint.enableCollision = false;
      
    }

    bool IsRectTransparent(Texture2D tex, Rect rect)
    {
        Color[] pixels = tex.GetPixels(
            (int)rect.x,
            (int)rect.y,
            (int)rect.width,
            (int)rect.height
        );

        foreach (Color c in pixels)
        {
            if (c.a > 0.01f)
                return false;
        }

        return true;
    }
}
