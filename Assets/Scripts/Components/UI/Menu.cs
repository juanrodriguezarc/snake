using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuComponent : MonoBehaviour
{
    
    public Button btnNewGame,btnLeaderboard,btnExit;
    public GameObject menu;
    private bool isShowing = true;

    void Start()
    {
        btnLeaderboard.onClick.AddListener(ShowLeaderboard);
        btnNewGame.onClick.AddListener(StartNewGame);
        btnExit.onClick.AddListener(ExitGame);
    }

    /// <summary>
    /// Show leaderboard modal
    /// </summary>
    void ShowLeaderboard()
    {
        Debug.Log("You have clicked the button show game");
    }

    /// <summary>
    /// Ends application
    /// </summary>
    void ExitGame()
    {
        Debug.Log("You have clicked the button exit game");
        Application.Quit();
    }

    /// <summary>
    /// Start a new game
    /// </summary>
    void StartNewGame()
    {
        Debug.Log("You have clicked the button! new game");
        isShowing = !isShowing;
        menu.SetActive(isShowing);
    }


}