using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject grenade;
    public GameObject rifle;
    public Player player;

    void Update()
    {
        if (player.level >= 3)
            rifle.SetActive(false);

        if (player.level >= 4)
            grenade.SetActive(false);

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gameIsPaused)
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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
