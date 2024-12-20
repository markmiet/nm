using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButtonControlleri : MonoBehaviour
{
    // Start is called before the first frame update

    public Sprite pauseImage;
    public Sprite resumeImage;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPauseImage(bool pause)
    {
        Image imageComponent = this.GetComponent<Image>();

        if (imageComponent != null)
        {
            // Assign the appropriate sprite
            imageComponent.sprite = pause ? resumeImage : pauseImage;
        }
        else
        {
            Debug.LogWarning("No Image component found on this GameObject!");
        }
    }

}
