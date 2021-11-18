using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject creditMenu;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject btnCredit;
    [SerializeField] private GameObject btnPlay;

    public void StartGame()
    {
        SceneManager.LoadScene("Eternal_Test");
    }

    private void DisableButtons()
    {
        btnCredit.SetActive(false);
        btnPlay.SetActive(false);
    }

    private void EnableButtons()
    {
        btnCredit.SetActive(true);
        btnPlay.SetActive(true);
    }

    public void CreditBack()
    {
        gamePanel.SetActive(false);
        creditMenu.SetActive(false);
        EnableButtons();
    }

    public void CreditOpen()
    {
        gamePanel.SetActive(true);
        creditMenu.SetActive(true);
        Debug.Log("hello");
        DisableButtons();
    }
}
