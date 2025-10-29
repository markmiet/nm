using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveCycle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable()
    {
        if (gameObject.name.Equals("UistinKatotekstuurillaPolygonCollideri"))
        {
            Debug.Log("OnDisable" + gameObject.name);
        }
    }

    void OnDestroy()
    {
        if (gameObject.name.Equals("UistinKatotekstuurillaPolygonCollideri"))
        {
            Debug.Log("OnDestroy" + gameObject.name);
        }
    }
    
}
