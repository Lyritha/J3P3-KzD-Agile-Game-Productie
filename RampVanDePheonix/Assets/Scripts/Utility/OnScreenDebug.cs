using TMPro;
using UnityEngine;

public class OnScreenDebug : MonoBehaviour
{
    public static OnScreenDebug Instance { get; private set; }

    [SerializeField]
    private TMP_Text debugText;

    protected void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Print(string message)
    {
        debugText.text += $"\n{message}";
    }
}
