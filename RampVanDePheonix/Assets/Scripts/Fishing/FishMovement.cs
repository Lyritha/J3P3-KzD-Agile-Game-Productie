using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float speed = 200f;
    public float minY = -180f;
    public float maxY = 180f;

    private RectTransform rect;
    private float direction;
    private float changeTimer;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        ChangeDirection();
    }

    void Update()
    {
        changeTimer -= Time.deltaTime;

        if (changeTimer <= 0)
        {
            ChangeDirection();
        }

        Vector2 pos = rect.anchoredPosition;
        pos.y += direction * speed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        rect.anchoredPosition = pos;
    }

    void ChangeDirection()
    {
        direction = Random.Range(-1f, 1f);
        changeTimer = Random.Range(0.5f, 1.5f);
    }
}
