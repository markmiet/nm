using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagemittariController : MonoBehaviour
{
    // Start is called before the first frame update
    private RectTransform tc;
    void Start()
    {

        tc = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDamage(float currentDamage,float maxDamage)
    { 
        if (currentDamage> maxDamage)
        {
            currentDamage = maxDamage;
        }
        //100

  //      float kerroin = 100 / maxDamage;
//        float nykyarvo = currentDamage * kerroin;

        float arvo = currentDamage / maxDamage;


        tc.localScale = new Vector3(arvo, tc.localScale.y, tc.localScale.z);

    }
}
