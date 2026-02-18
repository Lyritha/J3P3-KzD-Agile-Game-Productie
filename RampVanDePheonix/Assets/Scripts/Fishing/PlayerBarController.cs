using UnityEngine;

public class PlayerBarController : MonoBehaviour
{
    public float moveForce = 600f;
    public float gravity = 1000f;
    public float minY = -180f;
    public float maxY = 180f;

    private RectTransform rect;
    private float velocity;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            velocity += moveForce * Time.deltaTime;
        }
        else
        {
            velocity -= gravity * Time.deltaTime;
        }

        Vector2 pos = rect.anchoredPosition;
        pos.y += velocity * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        rect.anchoredPosition = pos;
    }
}
