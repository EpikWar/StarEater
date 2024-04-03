using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class VirtualCursor : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;   
    
    [Header("Initialize")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private RectTransform virtualCursor;
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private Canvas canvas;
    
    private Mouse virtualMouse;
    private MouseState leftButtonMouseState;
    private MouseState idleMouseState;


    private void Start()
    {
        Mouse.current.CopyState(out idleMouseState);

        InputState.Change(virtualMouse.position, new Vector2(Screen.width / 2, Screen.height / 2));
    }

    private void OnEnable()
    {
        if (virtualMouse == null) {
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }
        else if (!virtualMouse.added) {
            InputSystem.AddDevice(virtualMouse);
        }

        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        if (virtualCursor != null) {
            Vector2 position = virtualCursor.anchoredPosition;
            InputState.Change(virtualMouse.position, position);
        }

        InputSystem.onAfterUpdate += UpdateMotion;
    }

    private void OnDisable()
    {
        InputSystem.RemoveDevice(virtualMouse);
        InputSystem.onAfterUpdate -= UpdateMotion;
    }

    private void UpdateMotion()
    {
        Vector2 virtualCursorPosition = virtualMouse.position.ReadValue();
        virtualCursorPosition.x += Input.GetAxis("Mouse X") * mouseSensitivity;
        virtualCursorPosition.y += Input.GetAxis("Mouse Y") * mouseSensitivity;
        virtualCursorPosition.x = Math.Clamp(virtualCursorPosition.x, 0, Screen.width);
        virtualCursorPosition.y = Math.Clamp(virtualCursorPosition.y, 0, Screen.height);
        
        //Left Click Init
        if (Input.GetMouseButtonDown(0)) {
            InputState.Change(virtualMouse, leftButtonMouseState.WithButton(MouseButton.Left));
        
            InputState.Change(virtualMouse, idleMouseState);
            InputState.Change(virtualMouse, idleMouseState);
        }

        //Disallow Move Cursor
        if (Input.GetMouseButton(1) && GameManager.instance.IsRPGState())
            return;

        //Virtual Mouse Pos
        InputState.Change(virtualMouse.position, virtualCursorPosition);
        
        //Virtual Mouse Sprite Movement
        AnchorCursor(virtualCursorPosition);
    }

    private void AnchorCursor(Vector2 cursorPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, 
            cursorPosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main, 
            out var anchoredPosition
        );
        
        virtualCursor.anchoredPosition = anchoredPosition;
    }
}
