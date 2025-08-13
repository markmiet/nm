using UnityEngine;

public class DestroyWhenChildrenGone : MonoBehaviour
{
    public float checkInterval = 1f;
    public float delayBeforeDestroy = 0f;

    void Start()
    {
        InvokeRepeating(nameof(CheckChildren), 0f, checkInterval);
    }

    void CheckChildren()
    {
        if (transform.childCount == 0)
        {
            Destroy(gameObject, delayBeforeDestroy);
        }
        else
        {
            //int maarajoillaeiole = 0;
            DestroyWhenChildrenGone[] d=
            transform.GetComponentsInChildren<DestroyWhenChildrenGone>();

            if (d.Length==transform.childCount)
            {
                Destroy(gameObject, delayBeforeDestroy);
            }

        }
    }
}
