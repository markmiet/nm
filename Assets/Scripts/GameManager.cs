using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : BaseController
{
    public static GameManager Instance;

    public int lives = 3;
    private TextMeshProUGUI scoreTextMeshProUGUI;
    public GameObject scoreGameObject;

    private int score = 0;

    public void kasvataHighScorea(GameObject go)
    {
        if (go.GetComponent<SkeletonController>()!=null)
        {
            score += 1000;
        }
        if (go.GetComponent<IDamagedable>() != null)
        {
            score += 100;
        }
        else
        {
            score += 10;
        }
        scoreTextMeshProUGUI.SetText("" + score);
    }


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            scoreTextMeshProUGUI = scoreGameObject.GetComponent<TextMeshProUGUI>();

            DontDestroyOnLoad(gameObject); // Keep across scenes
            SceneManager.sceneLoaded += OnSceneLoaded;

            siirtymisenaloituskohta = Time.realtimeSinceStartup;
            //     elamat.GetComponent<ElamatController>().SetElamienMaara(lives);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //tilat normal mode
    //tehd‰‰n sit‰ aloitusviivett‰

    //ollaan kuoltu ja tehd‰‰n hidastus

    public float siirtymaviive = 2.0f;
    private bool siirtymassastartmenuun = false;
    private bool siirtymassascenenalkuun = false;

    private float siirtymisenaloituskohta;

    public void Update()
    {
        if (siirtymassascenenalkuun)
        {
            if (Time.realtimeSinceStartup-siirtymisenaloituskohta<siirtymaviive)
            {
                Time.timeScale = 0.1f;
            }
            else
            {
                Time.timeScale = 1.0f;
                siirtymassascenenalkuun = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        if (siirtymassastartmenuun)
        {
            if (Time.realtimeSinceStartup - siirtymisenaloituskohta < siirtymaviive)
            {
                Time.timeScale = 0.1f;
            }
            else
            {
                Time.timeScale = 1.0f;
                siirtymassastartmenuun = false;
                SceneManager.LoadScene("StartMenu");
            }
        }

    }

    public void PlayerDied()
    {
        lives--;
        //   
        Debug.Log("lives=" + lives);
        if (lives > 0)
        {
            // Reload current scene but preserve lives
            GameObject alus = PalautaAlus();
            alus.GetComponent<AlusController>().SetElamienMaara(lives);
            alus.GetComponent<AlusController>().SetkeskellaTextMeshProUGUI("Restart");
            siirtymassascenenalkuun = true;
            siirtymassastartmenuun = false;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            siirtymisenaloituskohta = Time.realtimeSinceStartup;


            //ja siis t‰ss‰ resetoidaan damaget

        }
        else
        {
            // Game over logic
            Debug.Log("Game Over");
            // Optionally: Load Game Over Scene
            GameObject alus = PalautaAlus();
            alus.GetComponent<AlusController>().SetElamienMaara(lives);
            alus.GetComponent<AlusController>().SetkeskellaTextMeshProUGUI("Game over");
            siirtymassastartmenuun = true;
            siirtymassascenenalkuun = false;
            //SceneManager.LoadScene("StartMenu");
            //ReloadSceneWithDelay(5f, "StartMenu");
            siirtymisenaloituskohta = Time.realtimeSinceStartup;

            lives = 3;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (lives > 0)
        {
            GameObject alus = GameObject.FindWithTag("Player");
            if (alus != null)
            {
            //    alus.GetComponent<AlusController>().SetElamienMaara(lives);
                // Resetoi damaget t‰ss‰
            }



        }
        if (scene.name.Equals("StartMenu"))
        {
            GameObject mainMenu = GameObject.Find("MainMenu");
            mainMenu.GetComponent<MainMenu>().SetTeksti("");

        }
    }




    private IEnumerator ReloadSceneWithDelay(float delay,string name)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(name);
    }

}
