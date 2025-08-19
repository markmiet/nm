using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HylsyController : BaseController
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
  //  public GameObject prefap;
    public float elamisenmaksimiaikaraja = 5.0f;

    public float nopeudenalaraja = 0.0f;
    // Update is called once per frame
    void Update()
    {
        //liian hidas niin tuhoa
        //    if (prefap!=null)
        if (IsGoingToBeDestroyed())
        {
            return;
        }

        TuhoaKunElamisenAikaRajaTayttyyTaiHidastuuLiikaa(GetPrefap(),gameObject, elamisenmaksimiaikaraja, nopeudenalaraja);
    }
}
