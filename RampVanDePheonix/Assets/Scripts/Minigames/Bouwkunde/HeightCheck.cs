using UnityEngine;
using TMPro;

public class HeightCheck : MonoBehaviour
{
    BrickSpawner spawner;

    [Header("UI Elements")]
    [SerializeField] TMP_Text heightIndicator;
    void Start()
    {
        spawner = FindAnyObjectByType<BrickSpawner>();
    }

    void Update()
    {
        float highest = 0;

        foreach (GameObject brick in spawner.spawnedBricks)
        {
            if (brick.transform.position.y > highest)
            {
                highest = brick.transform.position.y;
            }
        }
        print(highest);
    }


}
