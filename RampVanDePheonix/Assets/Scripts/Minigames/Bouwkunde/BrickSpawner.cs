using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 original;
    Vector2 dir;
    float threshold = 3;


    [Header("Movement")]
    [SerializeField] Transform target;

    [Header("BrickDropping")]
    [SerializeField] GameObject brickPrefab;
    [SerializeField] private Transform parent;

    public bool allowDrop = true;
    void Start()
    {
        InputManager.OnSpacePressed += DropBrick;
        rb = GetComponent<Rigidbody2D>();
        original = rb.transform.position;
        StartCoroutine(AllowDropping());
    }

    void Update()
    {
        MoveSpawner();
    }
    GameObject current;
    void DropBrick()
    {
        if (allowDrop)
        {
            current = Instantiate(brickPrefab, parent);
            current.transform.position = rb.transform.position;
            allowDrop = false;
        }
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
    IEnumerator AllowDropping()
    {
        yield return null;

        allowDrop = true;

        yield return new WaitForSeconds(1f);

        StartCoroutine(AllowDropping());
    }
    
}
