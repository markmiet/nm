using UnityEngine;

public class DamageablePart :BaseController, IDamagedable
{
    public int holeRadius = 3;

    private Texture2D dynamicTexture;
    private SpriteRenderer sr;
    private BoxCollider2D col;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();

        // Luo muokattava tekstuuri
        Texture2D original = sr.sprite.texture;
        dynamicTexture = Instantiate(original);
        dynamicTexture.Apply();

        // Luo uusi sprite käyttäen tätä tekstuuria
        Sprite newSprite = Sprite.Create(
            dynamicTexture,
            sr.sprite.rect,
            new Vector2(0.5f, 0.5f),
            sr.sprite.pixelsPerUnit
        );
        sr.sprite = newSprite;

        //eli tämä on vain sitä varten jos on semmonen olio kuten
        //skelekumi alaspäin joka koostuu miljoonasta eri osasta, tämä vain tuhoaa osan kerrallaan
    }

    public void DamageAt(Vector2 worldPoint)
    {

        Destroy(col);
        Destroy(gameObject, 1f); // voit myös vain piilottaa

        if (true)
            return;

        Vector2 localPoint = sr.transform.InverseTransformPoint(worldPoint);
        Vector2 pivotPixels = sr.sprite.pivot;
        Vector2 texCoord = localPoint * sr.sprite.pixelsPerUnit + pivotPixels;

        int cx = Mathf.RoundToInt(texCoord.x);
        int cy = Mathf.RoundToInt(texCoord.y);

        bool anyChange = false;

        for (int y = -holeRadius; y <= holeRadius; y++)
        {
            for (int x = -holeRadius; x <= holeRadius; x++)
            {
                if (x * x + y * y <= holeRadius * holeRadius)
                {
                    int px = cx + x;
                    int py = cy + y;

                    if (px >= 0 && px < dynamicTexture.width && py >= 0 && py < dynamicTexture.height)
                    {
                        dynamicTexture.SetPixel(px, py, new Color(0, 0, 0, 0));
                        anyChange = true;
                    }
                }
            }
        }

        if (anyChange)
        {
            dynamicTexture.Apply();

            // Jos koko tekstuuri on nyt tyhjä → poista collider ja mahdollisesti koko pala
            if (IsFullyTransparent(dynamicTexture))
            {
                Destroy(col);
                Destroy(gameObject, 1f); // voit myös vain piilottaa
            }
        }
    }

    private bool IsFullyTransparent(Texture2D tex)
    {
        Color[] pixels = tex.GetPixels();
        foreach (var p in pixels)
        {
            if (p.a > 0.01f)
                return false;
        }
        return true;
    }

    public bool AiheutaDamagea(float damagemaara, Vector2 contanctpoint)
    {
        //throw new System.NotImplementedException();
        DamageAt(contanctpoint);
        return false;
    }
}
