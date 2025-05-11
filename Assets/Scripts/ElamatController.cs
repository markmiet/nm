using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElamatController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] elamat;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetElamienMaara(int elamatmaara)
    {
        if (elamat!=null)
        {
            for (int i=0;i<elamat.Length;i++)
            {
                elamat[i].SetActive(false);
            }
            for (int i=0;i< elamatmaara; i++)
            {
                elamat[i].SetActive(true);
            }
        }
    }

}
