using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BrendaFlip : MonoBehaviour
{
    [SerializeField] private AudioClip[] flipSounds;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Image fadeInOutImg;
    [SerializeField] private float fadeDuration = 0.25f;
    private Coroutine fadeRoutine;

    private int currentSoundIndex = 0;
    private float lastFlipTime = -10f;

    float flipSpeed = 720f;

    private bool isFlipping = false;
    private float rotatedAmount = 0f;
    private Quaternion startRotation;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isFlipping)
        {
            StartFlip();
        }

        if (isFlipping)
        {
            float rotationThisFrame = flipSpeed * Time.deltaTime;

            if (rotatedAmount + rotationThisFrame >= 360f)
            {
                transform.rotation = startRotation;
                isFlipping = false;
                return;
            }

            transform.Rotate(Vector3.back * rotationThisFrame);
            rotatedAmount += rotationThisFrame;
        }
    }

    void StartFlip()
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeFlash());

        // Reset sound combo if more than 2 seconds passed
        if (Time.time - lastFlipTime > 1.5f)
        {
            currentSoundIndex = 0;
        }

        // Play sound
        audioSource.PlayOneShot(flipSounds[currentSoundIndex]);

        // Increase index but keep inside array
        currentSoundIndex = Mathf.Min(currentSoundIndex + 1, flipSounds.Length - 1);

        lastFlipTime = Time.time;

        isFlipping = true;
        rotatedAmount = 0f;
        startRotation = transform.rotation;
    }

    IEnumerator FadeFlash()
    {
        Color c = fadeInOutImg.color;

        // Fade in
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeInOutImg.color = c;
            yield return null;
        }

        // Fade out
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            fadeInOutImg.color = c;
            yield return null;
        }

        c.a = 0f;
        fadeInOutImg.color = c;
    }
}