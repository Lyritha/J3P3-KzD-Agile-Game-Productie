using UnityEngine;
using UnityEngine.UI;

public class PlayButtonSound : MonoBehaviour
{
    private void Awake()
    {
        if (TryGetComponent(out Button button))
            button.onClick.AddListener(() => AudioManager.Instance.PlaySingleClip(SoundEffects.button));
    }


}
