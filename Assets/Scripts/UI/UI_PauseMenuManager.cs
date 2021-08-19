using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PauseMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    
    void Update()
    {
        CheckGamePaused();
    }

    private void CheckGamePaused() 
    {
        if(!GameManager.Instance.GetMainMenuOn() && !GameManager.Instance.IsFirstStart && !GameManager.Instance.isGameOver)
            pauseMenu.gameObject.SetActive(GameManager.Instance.IsGamePaused());
    }
}
