using UnityEngine;

public class PooledObject : MonoBehaviour
{
    [HideInInspector] public GameObject prefab;  // the prefab this object was instantiated from
}
