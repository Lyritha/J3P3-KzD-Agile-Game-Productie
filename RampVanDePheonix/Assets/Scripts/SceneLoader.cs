using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] MinigameFinished minigameFinished;
    void Start()
    {
        Timer.OnCountDone += EnterMainGame;
    }

    void OnDestroy()
    {
        Timer.OnCountDone -= EnterMainGame;
    }

    void EnterMainGame()
    {
        minigameFinished.gameObject.SetActive(true);
    }
}
