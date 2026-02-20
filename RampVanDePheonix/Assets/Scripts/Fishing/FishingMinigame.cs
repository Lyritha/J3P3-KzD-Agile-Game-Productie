using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishingMinigame : MonoBehaviour
{
    [Header("World References")]
    [SerializeField] Transform fish;
    [SerializeField] Transform catchZone;

    [Header("UI")]
    [SerializeField] Slider catchSlider;
    [SerializeField] TMP_Text resultText;

    [Header("Score")]
    int score = 0;
    [SerializeField] TMP_Text scoreText;

    float resultDisplayTime = 1.5f;
    bool isResolving = false;

    float fishMoveSpeed = 4f;
    float fishChangeInterval = 1f;

    float barForce = 15f;
    float gravity = 25f;
    float maxBarSpeed = 10f;

    float catchRate = 0.8f;
    float loseRate = 0.6f;

    float barVelocity;
    float progress = 0.5f;

    float minY = -3f;
    float maxY = 3f;

    float fishTargetY;
    float fishTimer;

    void Start()
    {
        scoreText.text = "Score: " + score.ToString();
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
        if (isResolving) return;

        float distance = Mathf.Abs(fish.position.y - catchZone.position.y);

        if (distance < 0.8f)
            progress += catchRate * Time.deltaTime;
        else
            progress -= loseRate * Time.deltaTime;

        progress = Mathf.Clamp01(progress);
        catchSlider.value = progress;

        if (progress >= 1f)
        {
            StartCoroutine(HandleResult(true));
        }
        else if (progress <= 0f)
        {
            StartCoroutine(HandleResult(false));
        }
    }

    void CatchFish()
    {
        score += 1;
        Debug.Log("Caught! Score: " + score);

        if (scoreText != null)
            scoreText.text = "Score: " + score;

        ResetFishing();
    }

    void EscapeFish()
    {
        Debug.Log("Escaped!");

        ResetFishing();
    }

    void ResetFishing()
    {
        progress = 0.5f;
        catchSlider.value = progress;

        barVelocity = 0f;

        catchZone.position = new Vector3(
            catchZone.position.x,
            0f,
            catchZone.position.z
        );

        fish.position = new Vector3(
        fish.position.x,
        Random.Range(minY, maxY),
        fish.position.z
        );

        SetNewFishTarget();
    }

    System.Collections.IEnumerator HandleResult(bool caught)
    {
        isResolving = true;

        barVelocity = 0f;

        resultText.gameObject.SetActive(true);

        if (caught)
        {
            resultText.text = "Caught!";
            resultText.color = Color.green;
            score += 1;

            if (scoreText != null)
                scoreText.text = "Score: " + score;
        }
        else
        {
            resultText.color = Color.red;
            resultText.text = "Escaped!";
        }

        yield return new WaitForSeconds(resultDisplayTime);

        resultText.gameObject.SetActive(false);

        ResetFishing();

        isResolving = false;
    }
}
