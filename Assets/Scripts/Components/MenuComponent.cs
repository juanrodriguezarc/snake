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
        btnNewGame.onClick.AddListener(HideMenu);
        btnExit.onClick.AddListener(ExitGame);
    }


    void ShowLeaderboard()
    {
        Debug.Log("You have clicked the button show game");
    }

    void ExitGame()
    {
        Debug.Log("You have clicked the button exit game");
        Application.Quit();
    }


    void HideMenu()
    {
        Debug.Log("You have clicked the button! new game");
        isShowing = !isShowing;
        menu.SetActive(isShowing);
    }


}