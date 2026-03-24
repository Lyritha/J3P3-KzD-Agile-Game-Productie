using UnityEngine;

public class PopupAnim : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private void OnEnable()
    {
        animator.SetBool("Popup", true);
    }

    private void OnDisable()
    {
        animator.SetBool("Popup", false);
    }
}
