using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    // lazy way of clearing all music and sound effects when going to the main menu.
    private void Start()
    {
        AudioManager.Instance.StopAllLoopingSounds();
        AudioManager.Instance.StopMusic();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("MainGame_Tutorial");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
