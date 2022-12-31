using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputManager", menuName = "Input/Input Manager")]
public class InputManager : ScriptableObject, GameInput.IBuildingModeCameraActions
{
    private GameInput _gameInput;

    public event UnityAction<float> BuildingModeCameraRotateEvent = delegate { };
    public event UnityAction<float> BuildingModeCameraZoomEvent = delegate { };
    public event UnityAction<float> BuildingModeCameraDragViewEvent = delegate { };
    public event UnityAction<Vector3> BuildingModeCameraMovePivotEvent = delegate { };
    public event UnityAction<Vector2> BuildingModeCameraLookEvent = delegate { };

    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();
            _gameInput.BuildingModeCamera.SetCallbacks(this);
            _gameInput.BuildingModeCamera.Enable();
        }
    }

    private void OnDisable()
    {
        DisableAllInput();
    }

    public void DisableAllInput()
    {
        _gameInput.BuildingModeCamera.Disable();
    }

    public void EnableBuildingModeCameraInput()
    {
        _gameInput.BuildingModeCamera.Enable();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        BuildingModeCameraLookEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnRotateCamera(InputAction.CallbackContext context)
    {
        BuildingModeCameraRotateEvent.Invoke(context.ReadValue<float>());
    }

    public void OnMovePivot(InputAction.CallbackContext context)
    {
        BuildingModeCameraMovePivotEvent.Invoke(context.ReadValue<Vector3>());
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        BuildingModeCameraZoomEvent.Invoke(context.ReadValue<float>());
    }

    public void OnDragView(InputAction.CallbackContext context)
    {
        BuildingModeCameraDragViewEvent.Invoke(context.ReadValue<float>());
    }
}