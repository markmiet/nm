using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
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

}
