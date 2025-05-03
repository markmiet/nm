using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class MainMenu : MonoBehaviour
{
    public GameObject loadteksti;

    public void PlayLevel1()
    {
        Debug.Log("PlayLevel1");

        Time.timeScale = 1.0f; // Stop game time
        SceneManager.LoadScene("Level1");
        

       // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayLevel2()
    {
        Debug.Log("PlayLevel2");

        Time.timeScale = 1.0f; // Stop game time
        SceneManager.LoadScene("Level2");


        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Application would quit now.");
        Application.Quit();
    }

    public void EnableLevel(int indeksi)
    {

    }
    public void SetTeksti(string teksti)
    {
        loadteksti.GetComponent<TMPro.TextMeshProUGUI>().text = teksti;
    }


    public void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Play1") || CrossPlatformInputManager.GetButtonUp("Play1")) {
            Debug.Log("GetButtonDown");

            //SetTeksti("Loading");

            //PlayLevel1();
            StartCoroutine(LoadLevelWithText("Loading..."));

            // SetTeksti("");

        }
        /*
        else if ( CrossPlatformInputManager.GetButton("Play1"))
        {
            Debug.Log("GetButton");
            //SetTeksti("Loading...");
            //SetTeksti("GetButton");

           //  PlayLevel1();
            //SetTeksti("");


            //LoadLevelWithText("Loading...");
            //StartCoroutine(LoadLevelWithText("Loading..."));
            // SetTeksti("");

        }
        else if ( CrossPlatformInputManager.GetButtonDown("Play1"))
        {
            Debug.Log("GetButtonDown");
            //SetTeksti("GetButtonDown");

             PlayLevel1();
            // SetTeksti("");

        }
        */
    }
    public void FixedUpdate()
    {
        /*
        if (CrossPlatformInputManager.GetButtonDown("Play1")  || CrossPlatformInputManager.GetButton("Play1"))
        {
            PlayLevel1();
        }
        */
    }

    private IEnumerator LoadLevelWithText(string text)
    {
        /*
        SetTeksti(text);
        //yield return null; // Wait one frame
        yield return new WaitForSeconds(0.5f); // short wait to allow UI update
        PlayLevel1();

        // Optional: yield return new WaitForSeconds(1);
        //  SetTeksti(""); // Clear if needed after level starts
        */
        SetTeksti(text);
        yield return new WaitForEndOfFrame(); // Forces Unity to render this frame

        // Optional: wait a tiny bit longer to make sure the user sees it
       // yield return new WaitForSeconds(0.1f);

        // Start async scene load
        //AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level1");

        PlayLevel1();
        // Optionally, show a loading progress bar here
        //while (!asyncLoad.isDone)
        //{
        //    yield return null;
        //}

       
    }

}
