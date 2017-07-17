using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public GameObject highscore;
    static MenuManager instance;
	public Button audio_on, audio_off, music_on, music_off;
    public AudioSource[] sounds;

	private void Awake()
    {
        //Handling Singleton D.P.
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    // Use this for initialization
    void Start()
    {
        //Load highscore
        sounds = AudioManager.GetInstance().GetSounds();
        LoadHighScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    public void StartGame()
    {
        sounds[0].Stop();
        sounds[1].Play();
        SceneManager.LoadScene("Main");
    }

    public void MoreButton()
    {
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Cesarsk+Dev+Team");
    }

    private void LoadHighScore()
    {
        highscore.GetComponent<Text>().text = "Highscore: " + PlayerPrefs.GetInt("highscore", 0);
    }

    public void Github()
    {
        Application.OpenURL("https://github.com/Cesarsk");
        Application.OpenURL("https://github.com/Shiro23");
        Debug.Log("Opened external website");
    }

	public void ToggleAudio()
    {
        if (!AudioManager.isAudioMuted)
        {
            AudioManager.isAudioMuted = true;
            
            //Sound Effects
            sounds[2].mute = true;
            sounds[3].mute = true;
            sounds[4].mute = true;

            audio_on.gameObject.SetActive(false);
            audio_off.gameObject.SetActive(true);
        }

        else
        {
            AudioManager.isAudioMuted = false;
            
            //Sound effects
            sounds[2].mute = false;
            sounds[3].mute = false;
            sounds[4].mute = false;

            audio_on.gameObject.SetActive(true);
            audio_off.gameObject.SetActive(false);
        }
    }

    public void ToggleMusic()
    {
        if (!AudioManager.isMusicMuted)
        {
            AudioManager.isMusicMuted = true;
            //Menu and Main Sounds
            sounds[0].mute = true;
            sounds[1].mute = true;

            music_on.gameObject.SetActive(false);
            music_off.gameObject.SetActive(true);
        }

        else
        {
            AudioManager.isMusicMuted = false;
            //Menu and Main Sounds
            sounds[0].mute = false;
            sounds[1].mute = false;

            music_on.gameObject.SetActive(true);
            music_off.gameObject.SetActive(false);
        }
    }
}
