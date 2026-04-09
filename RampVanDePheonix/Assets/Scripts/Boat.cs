using UnityEngine;

public class Boat : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip[] randomSounds;
    [SerializeField] private AudioSource audioSource;

    [Header("Idle Movement")]
    [SerializeField] private float sineAmplitude = 50f;
    [SerializeField] private float maxSineFrequency = 5f;
    private float pauseSpeed = 0.5f;
    private float currentSineFrequency = 0;
    private float targetSineFrequency = 0;
    private float sinePhase = 0f;

    [Header("UI")]
    [SerializeField] private RectTransform wiggleTarget;

    private float baseX;
    private float baseY;
    private float baseRot;

    private bool isTooting = false;

    void Start()
    {
        baseX = wiggleTarget.anchoredPosition.x;
        baseY = wiggleTarget.anchoredPosition.y;
        baseRot = wiggleTarget.localRotation.eulerAngles.z;
    }

    public void StartMoving()
    {
        targetSineFrequency = maxSineFrequency;
    }

    public void StopMoving()
    {
        targetSineFrequency = 0;
    }




    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTooting)
        {
            AudioClip randomSound = randomSounds[Random.Range(0, randomSounds.Length)];
            audioSource.PlayOneShot(randomSound);

            isTooting = true;
            Invoke(nameof(ResetToot), randomSound.length);
        }

        HandleMovement();
    }

    private void ResetToot()
    {
        isTooting = false;
    }

    void HandleMovement()
    {
        // Update frequency
        currentSineFrequency = Mathf.MoveTowards(
            currentSineFrequency,
            targetSineFrequency,
            pauseSpeed * Time.deltaTime * maxSineFrequency
        );

        // Advance phase based on current frequency
        sinePhase += currentSineFrequency * Time.deltaTime;

        // Continuous side-to-side sway
        float x = baseX;
        float y = baseY + Mathf.Sin(sinePhase) * sineAmplitude;
        float rot = baseRot + Mathf.Sin(sinePhase + 1214.23f) * sineAmplitude * 0.25f;

        wiggleTarget.anchoredPosition = new Vector2(x, y);
        wiggleTarget.localRotation = Quaternion.Euler(0, 0, rot);
    }
}
