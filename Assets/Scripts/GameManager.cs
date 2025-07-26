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
    //private TextMeshProUGUI scoreTextMeshProUGUI;
    //public GameObject scoreGameObject;

    private TextMeshProUGUI scoreTextMeshProUGUI = null;
    private TextMeshProUGUI highscoreTextMeshProUGUI = null;

   public GameObject checkPoint;
    

    public AudioSource gameoverAudioSource;

    public AudioSource looserAudioSource;

    public void kasvataHighScorea(GameObject go)
    {

        if (go.GetComponent<HitCounter>() != null)
        {

            score += go.GetComponent<HitCounter>().hitThreshold*100;
        }

        else if (go.GetComponent<SkeletonController>()!=null)
        {
            score += 1000;
        }
        else if (go.GetComponent<IDamagedable>() != null)
        {
           score += 100;
        }
        else
        {
            score += 10;
        }
       // scoreTextMeshProUGUI.SetText("" + score);


        //palautaScoreGameObject().GetComponent<TextMeshProUGUI>().SetText("" + score);
        AsetaScoreTeksti();

    }

    private void AsetaScoreTeksti()
    {
        if (scoreTextMeshProUGUI==null)
        {
            GameObject scoreGameObject = GameObject.Find("ScoreText"); // Use the correct name
            if (scoreGameObject != null)
            {
                scoreTextMeshProUGUI = scoreGameObject.GetComponent<TextMeshProUGUI>();
                scoreTextMeshProUGUI.SetText(score.ToString());
            }
        }
        else
        {
            scoreTextMeshProUGUI.SetText(score.ToString());
        }
        SetHighScore();
    }


    public int score = 0;
    void Awake()
    {
        Application.targetFrameRate = 60;//t‰rke‰‰‰
        if (Instance == null)
        {
            Instance = this;
         //   scoreTextMeshProUGUI = scoreGameObject.GetComponent<TextMeshProUGUI>();

            DontDestroyOnLoad(gameObject); // Keep across scenes
            DontDestroyOnLoad(gameoverAudioSource); // Keep across scenes
            DontDestroyOnLoad(looserAudioSource);
            //DontDestroyOnLoad(scoreGameObject); // Keep across scenes
            //DontDestroyOnLoad(scoreTextMeshProUGUI); // Keep across scenes

            //DontDestroyOnLoad(score); // Keep across scenes
            DontDestroyOnLoad(checkPoint);
            

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
                //scoreTextMeshProUGUI.SetText("" + score);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                // scoreTextMeshProUGUI = scoreGameObject.GetComponent<TextMeshProUGUI>();
                // scoreTextMeshProUGUI.SetText("" + score);
                // palautaScoreGameObject().GetComponent<TextMeshProUGUI>().SetText("" + score);

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
            looserAudioSource.PlayDelayed(1.0f);
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
            SaveHighScore();
            score = 0;
            gameoverAudioSource.PlayDelayed(0.5f);
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
            checkPoint.GetComponent<CheckPointController>().asetettu = false;
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

            /*
            GameObject scoreGameObject = GameObject.Find("ScoreText"); // Use the correct name
            if (scoreGameObject != null)
            {
                TextMeshProUGUI scoreTextMeshProUGUI = scoreGameObject.GetComponent<TextMeshProUGUI>();
                scoreTextMeshProUGUI.SetText(score.ToString());
            }
            */
            currentHighScore = PlayerPrefs.GetInt("HighScore", 0); // Default is 0 if not set
            AsetaScoreTeksti();
            //t‰ss‰ tutkitaan onko checkpointti
            /*if (checkpointAsetettu)
             {
                 //                public bool checkpointAsetettu = false;
                 //public float checkpointx;
                 //public float checkpointy;
                 Camera.main.transform.position = new Vector2(checkpointx, checkpointy);
                 Camera.main.GetComponent<Kamera>().AsetaAlusKeskelleKameraa();
             }
             */
            if (checkPoint!=null)
             {
               
                GameObject go=GameObject.Find("CheckPointStart");
                if ( !checkPoint.GetComponent<CheckPointController>().asetettu) {
                    checkPoint.GetComponent<CheckPointController>().x = go.transform.position.x;

                    checkPoint.GetComponent<CheckPointController>().asetettu = true;
                }
                
                Debug.Log("checkPoint.transform.position.x=" + checkPoint.transform.position.x);
                 Camera.main.transform.position = new Vector3(checkPoint.GetComponent<CheckPointController>().x, Camera.main.transform.position.y, Camera.main.transform.position.z);
                 //Camera.main.GetComponent<Kamera>().AsetaAlusKeskelleKameraa();

             }
            
        }
        if (scene.name.Equals("StartMenu"))
        {
            GameObject mainMenu = GameObject.Find("MainMenu");
            mainMenu.GetComponent<MainMenu>().SetTeksti("");
            //GameObject go = GameObject.Find("CheckPointStart");
            //checkPointGetComponent<CheckPointController>().asetettu = false;
        }
            
    }




    private IEnumerator ReloadSceneWithDelay(float delay,string name)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(name);
    }
    private int currentHighScore = 0;


    private void SetHighScore()
    {
        if (currentHighScore < score)
        {
            currentHighScore = score;
        }


        if (highscoreTextMeshProUGUI == null)
        {
            GameObject scoreGameObject = GameObject.Find("HighScoreText"); // Use the correct name
            if (scoreGameObject != null)
            {
                highscoreTextMeshProUGUI = scoreGameObject.GetComponent<TextMeshProUGUI>();
                highscoreTextMeshProUGUI.SetText(currentHighScore.ToString());
            }
        }
        else
        {
            highscoreTextMeshProUGUI.SetText(currentHighScore.ToString());
        }
    }

    public void SaveHighScore()
    {

        currentHighScore=PlayerPrefs.GetInt("HighScore", 0); // Default is 0 if not set
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save(); // Optional, forces saving immediately
            Debug.Log("New High Score Saved: " + score);
        }
        
    }


}
