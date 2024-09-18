using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaukiSilmaController : MonoBehaviour, IExplodable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Explode()
    {
        Destroy(gameObject);
    }

}
