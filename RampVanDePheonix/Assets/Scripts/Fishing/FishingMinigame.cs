using UnityEngine;
using UnityEngine.UI;

public class FishingMinigame : MonoBehaviour
{
    [Header("World References")]
    public Transform fish;
    public Transform catchZone;

    [Header("UI")]
    public Slider catchSlider;

    [Header("Fish Settings")]
    public float fishMoveSpeed = 4f;
    public float fishChangeInterval = 1f;

    [Header("Bar Settings")]
    public float barForce = 15f;
    public float gravity = 25f;
    public float maxBarSpeed = 10f;

    [Header("Catch Settings")]
    public float catchRate = 0.8f;
    public float loseRate = 0.6f;

    private float barVelocity;
    private float progress = 0.5f;

    private float minY = -3f;
    private float maxY = 3f;

    private float fishTargetY;
    private float fishTimer;

    void Start()
    {
        catchSlider.value = progress;
        SetNewFishTarget();
    }

    void Update()
    {
        MoveFish();
        MoveBar();
        CheckCatch();
    }

    void MoveFish()
    {
        fishTimer -= Time.deltaTime;

        if (fishTimer <= 0f)
        {
            SetNewFishTarget();
        }

        float newY = Mathf.MoveTowards(
            fish.position.y,
            fishTargetY,
            fishMoveSpeed * Time.deltaTime
        );

        fish.position = new Vector3(fish.position.x, newY, fish.position.z);
    }

    void SetNewFishTarget()
    {
        fishTargetY = Random.Range(minY, maxY);
        fishTimer = fishChangeInterval;
    }

    void MoveBar()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            barVelocity = 8f;
        }

        barVelocity -= gravity * Time.deltaTime;

        barVelocity = Mathf.Clamp(barVelocity, -15f, 15f);

        catchZone.position += Vector3.up * barVelocity * Time.deltaTime;

        float y = Mathf.Clamp(catchZone.position.y, minY, maxY);
        catchZone.position = new Vector3(catchZone.position.x, y, catchZone.position.z);

        if (y == minY || y == maxY)
            barVelocity = 0f;
    }


    void CheckCatch()
    {
        float distance = Mathf.Abs(fish.position.y - catchZone.position.y);

        if (distance < 0.8f)
            progress += catchRate * Time.deltaTime;
        else
            progress -= loseRate * Time.deltaTime;

        progress = Mathf.Clamp01(progress);
        catchSlider.value = progress;

        if (progress >= 1f)
            Debug.Log("Caught!");

        if (progress <= 0f)
            Debug.Log("Escaped!");
    }
}
