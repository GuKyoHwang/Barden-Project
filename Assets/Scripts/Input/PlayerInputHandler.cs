using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private Vector2 moveInput;
    
    // 입력키로 지정한 키들이 입력될 경우 수행하게 되는 함수
    // 입력키: WASD, 방향키
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log(moveInput);
    }
    
    // 점프키로 지정한 키가 입력될 경우 수행하게 되는 함수
    // 점프키: Space
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Jump button pushed down now");
        }

        if (context.performed)
        {
            Debug.Log("Jump button is being held down");
        }

        if (context.canceled)
        {
            Debug.Log("Jump button has been released");
        }
    }
}
