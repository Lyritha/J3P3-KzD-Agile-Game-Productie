using UnityEditor;
using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 original;
    Vector2 dir;
    float threshold = 3;
    [SerializeField] Transform target;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        original = rb.transform.position;
    }

    void Update()
    {
        MoveSpawner();
    }


    void MoveSpawner()
    {
        if (Vector3.Distance(rb.transform.position, original) < threshold)
        {
            dir = Vector2.right;
        }
        else if (Vector3.Distance(rb.transform.position, target.position) < threshold)
        {
            dir = Vector2.left;
        }


        if (dir == Vector2.left)
        {
            rb.transform.position = Vector3.Lerp(rb.transform.position, original, Time.deltaTime * .5f);
        }
        else if (dir == Vector2.right)
        {
            rb.transform.position = Vector3.Lerp(rb.transform.position, target.position, Time.deltaTime * .5f);
        }
    }
}
