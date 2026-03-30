using UnityEngine;
using UnityEngine.UI;

public class PlayButtonSound : MonoBehaviour
{
    public bool playOnClick = true;

    private void Awake()
    {
        if (TryGetComponent(out Button button)) button.onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        if (playOnClick) AudioManager.Instance.PlaySingleClip(SoundEffects.button);
    }
}
