using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_TitleMgr : MonoBehaviour
{
    /// <summary>
    /// Start a new game
    /// </summary>
    public void NewGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    /// <summary>
    /// Watch Tutorial
    /// </summary>
    public void Guide()
    {
        SceneManager.LoadScene("Tutorial");
    }
    
    /// <summary>
    /// Exit Game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
