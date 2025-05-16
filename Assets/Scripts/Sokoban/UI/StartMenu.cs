using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * The Start Menu script is called by the buttons on the start menu, and have very simple functionality.
 * Therefore, it doesn't warrant being split up
 * This once I can break the Single Responsibility Principle intentionally
 */
public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
