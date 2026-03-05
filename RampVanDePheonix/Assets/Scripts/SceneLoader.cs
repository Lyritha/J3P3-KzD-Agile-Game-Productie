using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
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
        EventStateManager.Instance.SetState(State.Walking);

        // Get the current scene this script is part of
        Scene currentScene = gameObject.scene;

        // Show main scene if a SceneHider exists
        SceneHider sceneHider = FindAnyObjectByType<SceneHider>();
        if (sceneHider != null) sceneHider.ShowMainScene();

        // Unload it asynchronously
        SceneManager.UnloadSceneAsync(currentScene);

    }
}
