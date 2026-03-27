using UnityEngine;
using UnityEngine.UI;

public class ImageAnimator : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private float fps = 12f;

    [SerializeField]
    private Sprite[] frames;
    private int index;
    private float timer;

    private void Start()
    {
        if (frames == null) return;
        image.sprite = frames[0];
    }

    void Update()
    {
        if (frames == null) return;

        timer += Time.deltaTime;
        if (timer >= 1f / fps)
        {
            timer = 0f;
            index = (index + 1) % frames.Length;
            image.sprite = frames[index];
        }
    }
}
