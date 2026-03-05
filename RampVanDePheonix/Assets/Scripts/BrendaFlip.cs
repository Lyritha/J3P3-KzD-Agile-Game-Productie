using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BrendaFlip : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip[] flipSounds;
    [SerializeField] private AudioSource audioSource;

    [Header("Flash")]
    [SerializeField] private Image fadeInOutImg;
    [SerializeField] private float fadeDuration = 0.25f;
    private Coroutine fadeRoutine;

    [Header("Idle Movement")]
    [SerializeField] private float sineAmplitude = 50f;
    [SerializeField] private float sineFrequency = 5f;

    [Header("Flip")]
    [SerializeField] private float flipSpeed = 720f;
    [SerializeField] private float flipHeight = 120f;

    [Header("UI")]
    [SerializeField] private RectTransform rect;

    private int currentSoundIndex = 0;
    private float lastFlipTime = -10f;

    private bool isFlipping = false;
    private float rotatedAmount = 0f;
    private Quaternion startRotation;

    private float baseX;
    private float baseY;

    void Start()
    {
        baseX = rect.anchoredPosition.x;
        baseY = rect.anchoredPosition.y;
    }

    void Update()
    {
        HandleMovement();

        if (Input.GetKeyDown(KeyCode.Space) && !isFlipping)
        {
            StartFlip();
        }

        HandleFlip();
    }

    void HandleMovement()
    {
        // Continuous side-to-side sway
        float x = baseX + Mathf.Sin(Time.time * sineFrequency) * sineAmplitude;

        float y = baseY;

        // Jump arc while flipping
        if (isFlipping)
        {
            float flipProgress = rotatedAmount / 360f;
            y += Mathf.Sin(flipProgress * Mathf.PI) * flipHeight;
        }

        rect.anchoredPosition = new Vector2(x, y);
    }

    void HandleFlip()
    {
        if (!isFlipping) return;

        float rotationThisFrame = flipSpeed * Time.deltaTime;

        if (rotatedAmount + rotationThisFrame >= 360f)
        {
            transform.rotation = startRotation;
            isFlipping = false;
            return;
        }

        transform.Rotate(Vector3.forward * rotationThisFrame);
        rotatedAmount += rotationThisFrame;
    }

    void StartFlip()
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeFlash());

        // Reset combo if too slow
        if (Time.time - lastFlipTime > 1.5f)
        {
            currentSoundIndex = 0;
        }

        // Play sound
        audioSource.PlayOneShot(flipSounds[currentSoundIndex]);

        // Advance combo
        currentSoundIndex = Mathf.Min(currentSoundIndex + 1, flipSounds.Length - 1);

        lastFlipTime = Time.time;

        isFlipping = true;
        rotatedAmount = 0f;
        startRotation = transform.rotation;
    }

    IEnumerator FadeFlash()
    {
        Color c = fadeInOutImg.color;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeInOutImg.color = c;
            yield return null;
        }

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