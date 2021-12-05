using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject moves;
    private bool movesMenu = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && player.GetState())
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public bool GetPauseState()
    {
        return isPaused;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Moves()
    {
        if (!movesMenu)
        {
            moves.SetActive(true);
            movesMenu = true;
        }
        else
        {
            moves.SetActive(false);
            movesMenu = false;
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit the Game");
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
