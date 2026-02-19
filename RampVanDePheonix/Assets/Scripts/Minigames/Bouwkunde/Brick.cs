using UnityEngine;

public class Brick : MonoBehaviour
{
    HeightCheck height;
    void Start()
    {
        height = FindAnyObjectByType<HeightCheck>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        height.spawnedBricks.Add(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        height.spawnedBricks.Remove(gameObject);
        Destroy(gameObject);
    }
}
