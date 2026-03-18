using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class RandomEventManager : MonoBehaviour
{
    IEnumerator intervalTimer()
    {
        yield return new WaitForSeconds(2);
        //event logica
    }

    private void Start()
    {
        StartCoroutine(intervalTimer());
    }
}
