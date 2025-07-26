using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectTekijaController : BaseController
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public GameObject gameObjecktiJotaEtsitaan;
    private bool suoritettu = false;
    // Update is called once per frame
    void Update()
    {
        
        if (!suoritettu && gameObject != null )
        {
            bool nakyvilla = !IsOffScreen();
            if (nakyvilla)
            {
                suoritettu = true;
                //kameralle funktiolta jota voi kysyä montako instanssia tälläisiä on ruudulla
                //if showing and there are no instances of gameObject visible in screen

                //var comp1 = gameObject.GetComponent<MonoBehaviour>();
                int maara=
                Camera.main.GetComponent<Kamera>().GetCountOfTheseObjects(Camera.main, gameObjecktiJotaEtsitaan);
                if (maara==0)
                {
                    Instantiate(gameObjecktiJotaEtsitaan, transform.position, Quaternion.identity);
                }
            }
        }
    }
}
