using UnityEngine;

public class IgnoreCollisionGroups : MonoBehaviour
{
    public GameObject[] g1;
    public GameObject[] g2; 

    void Start()
    {
        // Loop through all objects in g1
        foreach (GameObject obj1 in g1)
        {
            if (obj1 == null) continue;

            Collider2D[] colliders1 = obj1.GetComponentsInChildren<Collider2D>();
            foreach (GameObject obj2 in g2)
            {
                if (obj2 == null) continue;

                Collider2D[] colliders2 = obj2.GetComponentsInChildren<Collider2D>();

                // Ignore collision between each collider pair
                foreach (Collider2D c1 in colliders1)
                {
                    foreach (Collider2D c2 in colliders2)
                    {
                        if (c1 != null && c2 != null)
                        {

                            Physics2D.IgnoreCollision(c1, c2);
                            Debug.Log("nimi1=" + c1.name + " nimi2=" + c2.name);
                        }
                    }
                }
            }
        }

        Debug.Log("All collisions between g1 and g2 are now ignored.");
    }
}
