using UnityEngine;
using TMPro;

public class TextFade : MonoBehaviour
{
    TMP_Text text;
    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }
    void FixedUpdate()
    {
        text.alpha -= .01f;

        if(text.alpha <= 0)
        {
            Destroy(gameObject);
        }
    }
}
