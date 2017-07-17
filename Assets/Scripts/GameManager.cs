using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour {

    private float slowness = 10f;
    static public bool isPaused = false;

    public static float timeScale;
    public static float fixedDeltaTime;

    public static bool isGreenEnabled = false, isRedEnabled = false, isYellowEnabled = false, isPurpleEnabled = false, isBlueEnabled = false, isOrangeEnabled = false, isIndigoEnabled = false, isRainbowEnabled = false;
    public static int level = 1;
    public Text pixelLabel;
    public Animator pixelLabelAnimator;
    public Animator cameraAnimator;
    public GameObject PauseCanvas;
    public GameObject ResumeButton, RestartButton;
    public GameObject highscore;
    private AudioSource[] sounds;

    public static GameManager instance;

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else instance = this;
    }

    public static GameManager GetInstance()
    {
        return instance;   
    }

    public void Start()
    {
        timeScale = Time.timeScale;
        fixedDeltaTime = Time.fixedDeltaTime;
        LeftInput.moveLeft = false;
        RightInput.moveRight = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        pixelLabel.text = "Dodge the blocks";
        level = 1;
        SwitchLevel();
        LoadHighScore();
        isPaused = false;
        sounds = AudioManager.GetInstance().GetSounds();
		ResumeButton.SetActive(true);
		RestartButton.SetActive(false);
	}

    private void LoadHighScore()
    {
        highscore.GetComponent<Text>().text = "Highscore: " + PlayerPrefs.GetInt("highscore", 0);
    }

    public void EndGame()
    {
        StartCoroutine(RestartLevel());
    }

    public void StartSlowMotion()
    {
        StartCoroutine(SlowMotionEffect());
    }

    public void StartFastMotion()
    {
        StartCoroutine(FastMotionEffect());
    }

    public IEnumerator SlowMotionEffect()
    {
        float slowness = 3f;
        Debug.Log("Starting Slow Motion");
        Time.timeScale = 1f / slowness;
        Time.fixedDeltaTime = Time.fixedDeltaTime / slowness;

        yield return new WaitForSeconds(3f / slowness); //Let's just wait one second / slowness

        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.fixedDeltaTime * slowness;

        Debug.Log("Slow Motion has ended");
    }

    public IEnumerator FastMotionEffect()
    {
        float fastness = 1.7f;
        Debug.Log("Starting Fast Motion");
        Time.timeScale = 1f * fastness;
        Time.fixedDeltaTime = Time.fixedDeltaTime * fastness;

        yield return new WaitForSeconds(2f * fastness); //Let's wait one second * fastness

        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.fixedDeltaTime / fastness;

        Debug.Log("Fast Motion has ended");
    }

    //Coroutines in order create the Slow effect
    IEnumerator RestartLevel()
    {
        //Before one second

        //slow effect, the second statement it's needed because we have fixedDeltaTime in Player script
        Time.timeScale = 1f / slowness;
        Time.fixedDeltaTime = Time.fixedDeltaTime / slowness;

        yield return new WaitForSeconds(1f / slowness); //Let's just wait one second / slowness

        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.fixedDeltaTime * slowness;

        //After one second


        //Using buildindex is more conveniente because integers are smaller than strings
        ShowAdOrRestart(AD_CHANCE);
    }

	private const int AD_CHANCE = 30;
	private bool adShown = false;
    
    public void ShowAdOrRestart(int CHANCE)
	{
		int adRate = Random.Range(1, 101);

        //Case: Show Advertisement
		if (adRate >= 1 && adRate <= CHANCE)
		{
			if (Advertisement.IsReady())
			{
				adShown = true;
                PauseGame();
				Advertisement.Show();
			}
			else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

        //Case: Do not show Advertisement
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);	
    }

    public void SwitchLevel()
    {
        /*
         * * * * * * * * * * * * * * * * * * * * * * * * * LEVEL DESIGN * * * * * * * * * * * * * * * * * * * * * * * * *
         *                                                                                                              *
         *          Level 1 (Score range 1-15): 1 Block Layout and no colored blocks                                    *
         *          Level 2 (Score range 16-30): 2 Blocks Layout and colored blocks (Slow Down - Speed Up)              *
         *          Level 3 (Score range 31-45): 3 Blocks Layout and colored blocks (Size Down - Size Up)               *
         *          Level 4 (Score range 46-60): 4 Blocks Layout and colored blocks (Time Down - Time Up)               *
         *          Level 5 (Score range 61-75): 4 Blocks Layout and colored blocks (Green Block)                       *
         *          Level 6 (Score range 76-90): 4 Blocks Layout and colored blocks (Invincibility Block)               *
         *          Level 7 Endless (Score range > 90): Random Layout and every colored block enabled                   *
         *                                                                                                              *
         * * * * * * * * * * * * * * * * * * * * * * * * * LEVEL DESIGN * * * * * * * * * * * * * * * * * * * * * * * * *
        */

        switch (level)
        {
            case 1:
                //Layout 1 No Colored Blocks
                Debug.Log("level 1");
				pixelLabel.text = "Dodge the Blocks";
                pixelLabelAnimator.SetTrigger("FadeText");
				BlockSpawner.selectedLayout = 1;
                break;

            case 2:
                //Layout 2 Slow Down - Speed Up
                Debug.Log("level 2");
                pixelLabel.text = "Gotta go faster!";
				pixelLabelAnimator.SetTrigger("FadeText");
                cameraAnimator.SetInteger("Next Level", 2);
				BlockSpawner.selectedLayout = 2;
				isRedEnabled = true;
                isYellowEnabled = true;
                break;

            case 3:
                //Layout 3 Size Down - Size Up
                Debug.Log("level 3");
                pixelLabel.text = "One makes\nyou larger...";
				pixelLabelAnimator.SetTrigger("FadeText");
                cameraAnimator.SetInteger("Next Level", 3);
                BlockSpawner.selectedLayout = 3;
				isPurpleEnabled = true;
                isBlueEnabled = true;
                break;

            case 4:
                //Layout 4 Time Down - Time Up
                Debug.Log("level 4");
                pixelLabel.text = "Do you like\nSlow Motion?";
				pixelLabelAnimator.SetTrigger("FadeText");
                cameraAnimator.SetInteger("Next Level", 4);
                BlockSpawner.selectedLayout = 4;
                isIndigoEnabled = true;
                isOrangeEnabled = true;
                break;

            case 5:
                //Layout 4 Green Block
                Debug.Log("level 5");
                pixelLabel.text = "Your world is\nupside down";
				pixelLabelAnimator.SetTrigger("FadeText");
                cameraAnimator.SetInteger("Next Level", 5);
				BlockSpawner.selectedLayout = 4;
                isGreenEnabled = true;
                break;

            case 6:
                //Layout 4 Invincibility Block
                Debug.Log("level 6");
                pixelLabel.text = "Need some help?";
                pixelLabelAnimator.SetTrigger("FadeText");
                cameraAnimator.SetInteger("Next Level", 6);
                BlockSpawner.selectedLayout = 4;
                isRainbowEnabled = true;
                break;

            case 7:
                //Layout Random
				Debug.Log("level 7 - Endless");
				pixelLabel.text = "To Infinite and\nBeyond! Good luck!";
                pixelLabelAnimator.SetTrigger("FadeText");
				cameraAnimator.SetInteger("Next Level", 7);
                BlockSpawner.selectedLayout = 5;
                break;
        }
    }

    public static void SetHighscore()
    {
        int currentScore = Score.score;
        int currentHighscore = PlayerPrefs.GetInt("highscore", 0);
        if (currentHighscore < currentScore) PlayerPrefs.SetInt("highscore", currentScore);
    }

    public void GetHighscore()
    {
        int currentHighscore = PlayerPrefs.GetInt("highscore", 0);
        highscore.GetComponent<Text>().text = "Highscore: " + currentHighscore;
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            isPaused = true;
            sounds[1].mute = true;
            if(adShown)
            {
                //showing the restart button instead of the resume
                ResumeButton.SetActive(false);
                RestartButton.SetActive(true);
            }
            PauseCanvas.SetActive(true);
            GetHighscore();
            Time.timeScale = 0;
        }
    }

    public void ResumeGame()
    {
        if (isPaused)
        {
            isPaused = false;
            sounds[1].mute = false;
            PauseCanvas.SetActive(false);
            Time.timeScale = 1;
            if(adShown) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
        }
    }

    public void Ragequit()
    {
        sounds[1].Stop();
        sounds[1].mute = false;
        sounds[0].Play();
        SceneManager.LoadScene("Menu");
    }
}
