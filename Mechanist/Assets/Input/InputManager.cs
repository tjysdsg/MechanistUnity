using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputManager", menuName = "Game/Input Manager")]
public class InputManager : ScriptableObject, GameInput.IBuildingModeActions
{
    private GameInput _gameInput;

    public event UnityAction<float> BuildingModeZoomEvent = delegate { };
    public event UnityAction<Vector3> BuildingModeMoveCameraPivotEvent = delegate { };
    public event UnityAction<float> BuildingModeRotateCameraEvent = delegate { };
    public event UnityAction<float> BuildingModeDragCameraEvent = delegate { };
    public event UnityAction<float> BuildingModeFireEvent = delegate { };

    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();
            _gameInput.BuildingMode.SetCallbacks(this);
            _gameInput.BuildingMode.Enable();
        }
    }

    private void OnDisable()
    {
        DisableAllInput();
    }

    public void DisableAllInput()
    {
        _gameInput.BuildingMode.Disable();
    }

    public void EnableBuildingModeInput()
    {
        _gameInput.BuildingMode.Enable();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
    }

    public void OnMoveCameraPivot(InputAction.CallbackContext context)
    {
        BuildingModeMoveCameraPivotEvent.Invoke(context.ReadValue<Vector3>());
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        BuildingModeZoomEvent.Invoke(context.ReadValue<float>());
    }

    public void OnRotateCamera(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled)
            BuildingModeRotateCameraEvent.Invoke(context.ReadValue<float>());
    }

    public void OnDragCamera(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled)
            BuildingModeDragCameraEvent.Invoke(context.ReadValue<float>());
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled)
            BuildingModeFireEvent.Invoke(context.ReadValue<float>());
    }

    public void OnPointer(InputAction.CallbackContext context)
    {
    }

    public Vector2 GetBuildModePointerInput()
    {
        return _gameInput.BuildingMode.Pointer.ReadValue<Vector2>();
    }

    public Vector2 GetBuildModePointerDeltaInput()
    {
        return _gameInput.BuildingMode.Look.ReadValue<Vector2>();
    }
}