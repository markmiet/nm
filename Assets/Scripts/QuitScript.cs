using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class QuitScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) || CrossPlatformInputManager.GetButtonDown("Quit"))
        {
            Debug.Log("quiiiiiiiiiiiiiiiiit");
            // Application.Quit();

            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                activity.Call<bool>("moveTaskToBack", true);
            }
            else
            {
                Application.Quit();
            }

            //Application.Quit();
        }
    }



}
