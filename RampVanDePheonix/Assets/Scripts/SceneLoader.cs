using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {
        Timer.OnCountDone += EnterMainGame;
    }
    void EnterMainGame()
    {
        SceneManager.LoadScene("MainGame");
    }
}
