using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioplayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource taustamusiikki;
    public AudioSource bonus;
    public AudioSource alusammus;
    public AudioSource explode;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BonusPlay()
    {
        bonus.Play();
    }

    public void TaustaMusiikkiPlay()
    {
        taustamusiikki.loop = true;  
        taustamusiikki.Play();
    }

    public void TaustaMusiikkiStop()
    {
        taustamusiikki.loop = true;
        taustamusiikki.Stop();
    }


    public void AlusammusPlay()
    {
        alusammus.Play();
    }

    public void ExplodePlay()
    {
        if (explode!=null)
            explode.Play();
    }

}
