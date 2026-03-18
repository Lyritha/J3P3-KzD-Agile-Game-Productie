using UnityEngine;

public class stealing_enemy : MonoBehaviour
{
    GameObject brendaTarget;
    
    void Start()
    {
        brendaTarget = FindAnyObjectByType<Player>().gameObject;
    }

    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, brendaTarget.transform.position, 2);
    }
}
