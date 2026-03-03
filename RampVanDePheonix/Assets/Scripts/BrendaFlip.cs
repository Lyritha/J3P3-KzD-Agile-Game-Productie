using UnityEngine;

public class BrendaFlip : MonoBehaviour
{
    float flipSpeed = 720f;

    private bool isFlipping = false;
    private float rotatedAmount = 0f;
    private Quaternion startRotation;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isFlipping)
        {
            isFlipping = true;
            rotatedAmount = 0f;
            startRotation = transform.rotation;
        }

        if (isFlipping)
        {
            float rotationThisFrame = flipSpeed * Time.deltaTime;

            // Prevent overshooting 360
            if (rotatedAmount + rotationThisFrame >= 360f)
            {
                transform.rotation = startRotation; // snap perfectly back
                isFlipping = false;
                return;
            }

            transform.Rotate(Vector3.back * rotationThisFrame);
            rotatedAmount += rotationThisFrame;
        }
    }
}

