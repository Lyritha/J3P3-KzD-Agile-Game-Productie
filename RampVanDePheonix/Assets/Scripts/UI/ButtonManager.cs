using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    // lazy way of clearing all music and sound effects when going to the main menu.
    private void Start()
    {
        AudioManager.Instance.StopAmbiance();
        AudioManager.Instance.StopMusic();
    }

    public void StartGame()
    {
        bool tutorialCompleted = PlayerPrefs.GetInt("MainTutorialCompleted", 0) == 1;
        string sceneToLoad = tutorialCompleted ? "MainGame" : "MainGame_Tutorial";
        PlayerPrefs.SetInt("MainTutorialCompleted", 1);

        SceneManager.LoadScene(sceneToLoad);
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
