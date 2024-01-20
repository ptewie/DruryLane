using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject TitleScreenObject;
    public GameObject CreditsMenuObject;
    public GameObject MainGameObject;
    public GameObject GameOverObject;
    public GameObject VictoryObject;

    // Start is called before the first frame update
    void Start()
    {
        TitleScreenObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void HideTitleScreenUI()
    {
        TitleScreenObject.SetActive(false);
    }

    public void ShowTitleScreenUI()
    {
        TitleScreenObject.SetActive(true);
    }

    public void ShowCreditsMenu()
    {
        CreditsMenuObject.SetActive(true);
    }

    public void HideCreditsMenu()
    {
        CreditsMenuObject.SetActive(false);
    }

    public void ShowGameOver()
    {
        GameOverObject.SetActive(true);
    }

    public void HideGameOver()
    {
        GameOverObject.SetActive(false);
    }

    public void ShowVictory()
    {
        VictoryObject.SetActive(true);
    }

    public void HideVictory()
    {
        VictoryObject.SetActive(false);
    }

    public void ShowMainGame()
    {
        MainGameObject.SetActive(true);
    }

    public void HideMainGame()
    {
        MainGameObject.SetActive(false);
    }


    public void HandleGameStateChanged(GameState previousState, GameState newState)
    {
        switch (previousState)
        {
            case GameState.TitleState:
                HideTitleScreenUI();
                break;
            case GameState.GameplayState:
                HideMainGame();
                break;
            case GameState.GameOverState:
                HideGameOver();
                break;
            case GameState.CreditsState:
                HideCreditsMenu();
                break;
            case GameState.VictoryState:
                HideVictory();
                break;
        }
        switch (newState)
        {
            case GameState.TitleState:
                ShowTitleScreenUI();
                break;
            case GameState.GameplayState:
                ShowMainGame();
                break;
            case GameState.GameOverState:
                ShowGameOver();
                break;
            case GameState.CreditsState:
                ShowCreditsMenu();
                break;
            case GameState.VictoryState:
                ShowVictory();
                break;
        }
    }
}

