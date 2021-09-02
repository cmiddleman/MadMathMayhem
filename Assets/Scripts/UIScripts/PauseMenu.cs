using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;

    public  GameObject pauseMenu;
    public GameObject mathTexts;


    public void Pause()
    {
        paused = true;
        pauseMenu.SetActive(true);
        mathTexts.SetActive(false);
        AudioManager.Instance.StopAll();
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        paused = false;
        pauseMenu.SetActive(false);
        mathTexts.SetActive(true);
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Resume();
        SceneManager.LoadScene("Menu");
    }
}
