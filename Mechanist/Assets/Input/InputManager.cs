using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputManager", menuName = "Game/Input Manager")]
public class InputManager : ScriptableObject,
    GameInput.ICommonActions, GameInput.IBuildingModeActions, GameInput.IPlayModeActions
{
    private GameInput _gameInput;

    public event UnityAction<float> BuildingModeZoomEvent = delegate { };
    public event UnityAction<Vector3> BuildingModeMoveCameraPivotEvent = delegate { };
    public event UnityAction<float> BuildingModeRotateCameraEvent = delegate { };
    public event UnityAction<float> BuildingModeDragCameraEvent = delegate { };
    public event UnityAction FireEvent = delegate { };
    public event UnityAction DoubleFireEvent = delegate { };
    public event UnityAction<InputActionPhase> HoldFireEvent = delegate { };
    public event UnityAction EnterPlayModeEvent = delegate { };
    public event UnityAction EnterBuildModeEvent = delegate { };
    public event UnityAction EscPressedEvent = delegate { };

    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();
            _gameInput.Common.SetCallbacks(this);
            _gameInput.BuildingMode.SetCallbacks(this);
            _gameInput.PlayMode.SetCallbacks(this);

            DisableAllInput();
            _gameInput.Common.Enable();
        }
    }

    private void OnDisable()
    {
        DisableAllInput();
    }

    public void DisableAllInput()
    {
        _gameInput.Common.Disable();
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

    #region APIs

    public Vector2 GetPointerScreenPosition()
    {
        return _gameInput.Common.Pointer.ReadValue<Vector2>();
    }

    public Vector2 GetPointerDeltaPosition()
    {
        return _gameInput.Common.PointerDelta.ReadValue<Vector2>();
    }

    #endregion

    #region Common

    public void OnPointerDelta(InputAction.CallbackContext context)
    {
    }

    public void OnPointer(InputAction.CallbackContext context)
    {
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            FireEvent.Invoke();
    }

    public void OnDoubleFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            DoubleFireEvent.Invoke();
    }

    public void OnHoldFire(InputAction.CallbackContext context)
    {
        HoldFireEvent.Invoke(context.phase);
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

    #endregion

    #region ModeSwitching

    public void OnEnterPlayMode(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("InputManager: Enter Play Mode");
            DisableAllInput();
            _gameInput.Common.Enable();
            EnablePlayModeInput();
            EnterPlayModeEvent.Invoke();
        }
    }

    public void OnEsc(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            EscPressedEvent.Invoke();
    }

    public void OnEnterBuildMode(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("InputManager: Enter Build Mode");
            DisableAllInput();
            _gameInput.Common.Enable();
            EnableBuildingModeInput();
            EnterBuildModeEvent.Invoke();
        }
    }

    #endregion
}