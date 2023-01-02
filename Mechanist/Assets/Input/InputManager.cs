using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputManager", menuName = "Game/Input Manager")]
public class InputManager : ScriptableObject, GameInput.IBuildingModeActions, GameInput.IPlayModeActions
{
    private GameInput _gameInput;

    public event UnityAction<float> BuildingModeZoomEvent = delegate { };
    public event UnityAction<Vector3> BuildingModeMoveCameraPivotEvent = delegate { };
    public event UnityAction<float> BuildingModeRotateCameraEvent = delegate { };
    public event UnityAction<float> BuildingModeDragCameraEvent = delegate { };
    public event UnityAction BuildingModeFireEvent = delegate { };
    public event UnityAction BuildingModeDoubleFireEvent = delegate { };
    public event UnityAction BuildingModeEnterPlacementEvent = delegate { };
    public event UnityAction BuildModeToPlayModeEvent = delegate { };
    public event UnityAction PlayModeToBuildModeEvent = delegate { };

    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();
            _gameInput.BuildingMode.SetCallbacks(this);
            _gameInput.PlayMode.SetCallbacks(this);
            DisableAllInput();
        }
    }

    private void OnDisable()
    {
        DisableAllInput();
    }

    public void DisableAllInput()
    {
        _gameInput.BuildingMode.Disable();
        _gameInput.PlayMode.Disable();
    }

    public void EnableBuildingModeInput()
    {
        _gameInput.BuildingMode.Enable();
    }

    public void EnablePlayModeInput()
    {
        _gameInput.PlayMode.Enable();
    }

    #region Empty Handlers

    public void OnPointerDelta(InputAction.CallbackContext context)
    {
    }

    public void OnPointer(InputAction.CallbackContext context)
    {
    }

    #endregion

    #region BuildMode

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
        if (context.phase == InputActionPhase.Performed)
            BuildingModeFireEvent.Invoke();
    }

    public void OnDoubleFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            BuildingModeDoubleFireEvent.Invoke();
    }

    public void OnEnterPlacementMode(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            BuildingModeEnterPlacementEvent.Invoke();
    }

    public Vector2 GetBuildModePointerInput()
    {
        return _gameInput.BuildingMode.Pointer.ReadValue<Vector2>();
    }

    public Vector2 GetBuildModePointerDeltaInput()
    {
        return _gameInput.BuildingMode.PointerDelta.ReadValue<Vector2>();
    }

    #endregion

    #region ModeSwitching

    public void OnEnterPlayMode(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("InputManager: Enter Play Mode");
            DisableAllInput();
            EnablePlayModeInput();
            BuildModeToPlayModeEvent.Invoke();
        }
    }

    public void OnEnterBuildMode(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("InputManager: Enter Build Mode");
            DisableAllInput();
            EnableBuildingModeInput();
            PlayModeToBuildModeEvent.Invoke();
        }
    }

    #endregion
}