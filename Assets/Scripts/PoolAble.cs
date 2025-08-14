using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolAble : ReturnToPoolAble
{
    // Start is called before the first frame update
    private GameObject prefap;
    public GameObject GetPrefap()
    {
        return prefap;
    }

    public void SetPreFap(GameObject g)
    {
        prefap = g;
    }
}
