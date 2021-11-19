using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject creditMenu;
    [SerializeField] private GameObject btnCredit;
    [SerializeField] private GameObject btnPlay;
    [SerializeField] private GameObject btnQuit;


    public void StartGame()
    {
        SceneManager.LoadScene("Eternal_Test");
    }

    private void DisableButtons()
    {
        btnCredit.SetActive(false);
        btnPlay.SetActive(false);
        btnQuit.SetActive(false);
    }

    private void EnableButtons()
    {
        btnCredit.SetActive(true);
        btnPlay.SetActive(true);
        btnQuit.SetActive(true);
    }

    public void CreditBack()
    {
        creditMenu.SetActive(false);
        EnableButtons();
    }

    public void CreditOpen()
    {
        creditMenu.SetActive(true);
        DisableButtons();
    }

    public void QuitGame()
    {
        Debug.Log("Quit the Game");
        Application.Quit();
    }
}
