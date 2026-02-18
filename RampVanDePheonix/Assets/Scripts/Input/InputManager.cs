using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour
{
    public static Action OnSpacePressed;
    
    public void GetSpacePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnSpacePressed();
        }
    }
}
