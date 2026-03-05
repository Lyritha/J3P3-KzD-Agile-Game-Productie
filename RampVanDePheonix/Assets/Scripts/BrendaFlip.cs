using UnityEngine;

public class BrendaFlip : MonoBehaviour
{
    [SerializeField] private AudioClip[] flipSounds;
    [SerializeField] private AudioSource audioSource;

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
        // Reset sound combo if more than 2 seconds passed
        if (Time.time - lastFlipTime > 1f)
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
}