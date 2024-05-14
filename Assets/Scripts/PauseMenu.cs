using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    private AudioSource backgroundMusicAudioSource;

    void Start()
    {
        //backgroundMusicAudioSource = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>(); // find gameobject with "BackgroundMusic" and get component
    }

    void Update() // if esc key is pressed pause if not resume
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused == true)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false); // don't make the UI appear
        Time.timeScale = 1f; // resume
        GameIsPaused = false;

        //backgroundMusicAudioSource.UnPause();
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true); // make UI appear
        Time.timeScale = 0f; // pause
        GameIsPaused = true;

        //backgroundMusicAudioSource.Pause();

        //PlayerPrefs.SetInt("musicPaused", 1); // save preference meaning music is paused
        //PlayerPrefs.Save();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitMenu()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }


}