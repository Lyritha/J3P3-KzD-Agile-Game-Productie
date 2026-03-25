using System.Collections;
using UnityEngine;

public class PopupAnim : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private void OnEnable()
    {
        animator.SetBool("Popup", true);
    }

    public void CloseAnim()
    {
        animator.SetBool("Popup", false);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        animator.SetBool("Popup", false);
    }
}
