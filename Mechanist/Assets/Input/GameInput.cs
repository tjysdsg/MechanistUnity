//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Input/GameInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @GameInput : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInput"",
    ""maps"": [
        {
            ""name"": ""BuildingMode"",
            ""id"": ""028c605f-29c8-4bd8-8cde-704ffcfd3855"",
            ""actions"": [
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""79e35e97-ba88-4a16-b38c-4a28a05fd155"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""RotateCamera"",
                    ""type"": ""Button"",
                    ""id"": ""044d363c-5ba6-47f4-a251-ee60ff721141"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveCameraPivot"",
                    ""type"": ""Value"",
                    ""id"": ""a9686e8c-9ad5-4e82-b9d9-79d9265253b5"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""e67dc4d3-e39e-4f33-8d66-174dcab4f3de"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""DragCamera"",
                    ""type"": ""Button"",
                    ""id"": ""da35ee6b-cad8-42d8-a934-12ec5462a272"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""a443427d-9b0b-4070-a650-b6a2018ec41b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8c8e490b-c610-4785-884f-f04217b23ca4"",
                    ""path"": ""<Pointer>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse;Touch"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9233bb34-114c-481c-9d43-003ec446615c"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""RotateCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""4c4478a1-2c06-4c99-b33c-941e3962cf92"",
                    ""path"": ""3DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCameraPivot"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""8e3be511-f2c6-4124-b9e8-0b7120460310"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCameraPivot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""14dac7ba-fc74-486c-9d5d-11b58b4177ea"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCameraPivot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""e4b4dbfe-f600-4edb-927c-82cbc4169cab"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCameraPivot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""c7ca2fa0-7a43-436d-9d8a-4092cdcd7454"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCameraPivot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Forward"",
                    ""id"": ""516b2382-f60f-44c5-a996-b93401d34873"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCameraPivot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Backward"",
                    ""id"": ""ddd841f1-8e31-4947-9ddf-c00d73e5ccb6"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCameraPivot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""78e4bee4-b4f3-4c21-adf9-ee651161fe13"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20cdf168-5601-4ab1-be4a-9069e5e36ffc"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""DragCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b2931c2b-049d-47ae-bbb8-3e874ebc7c90"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touch"",
            ""bindingGroup"": ""Touch"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Joystick"",
            ""bindingGroup"": ""Joystick"",
            ""devices"": [
                {
                    ""devicePath"": ""<Joystick>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""XR"",
            ""bindingGroup"": ""XR"",
            ""devices"": [
                {
                    ""devicePath"": ""<XRController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // BuildingMode
        m_BuildingMode = asset.FindActionMap("BuildingMode", throwIfNotFound: true);
        m_BuildingMode_Look = m_BuildingMode.FindAction("Look", throwIfNotFound: true);
        m_BuildingMode_RotateCamera = m_BuildingMode.FindAction("RotateCamera", throwIfNotFound: true);
        m_BuildingMode_MoveCameraPivot = m_BuildingMode.FindAction("MoveCameraPivot", throwIfNotFound: true);
        m_BuildingMode_Zoom = m_BuildingMode.FindAction("Zoom", throwIfNotFound: true);
        m_BuildingMode_DragCamera = m_BuildingMode.FindAction("DragCamera", throwIfNotFound: true);
        m_BuildingMode_Fire = m_BuildingMode.FindAction("Fire", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // BuildingMode
    private readonly InputActionMap m_BuildingMode;
    private IBuildingModeActions m_BuildingModeActionsCallbackInterface;
    private readonly InputAction m_BuildingMode_Look;
    private readonly InputAction m_BuildingMode_RotateCamera;
    private readonly InputAction m_BuildingMode_MoveCameraPivot;
    private readonly InputAction m_BuildingMode_Zoom;
    private readonly InputAction m_BuildingMode_DragCamera;
    private readonly InputAction m_BuildingMode_Fire;
    public struct BuildingModeActions
    {
        private @GameInput m_Wrapper;
        public BuildingModeActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Look => m_Wrapper.m_BuildingMode_Look;
        public InputAction @RotateCamera => m_Wrapper.m_BuildingMode_RotateCamera;
        public InputAction @MoveCameraPivot => m_Wrapper.m_BuildingMode_MoveCameraPivot;
        public InputAction @Zoom => m_Wrapper.m_BuildingMode_Zoom;
        public InputAction @DragCamera => m_Wrapper.m_BuildingMode_DragCamera;
        public InputAction @Fire => m_Wrapper.m_BuildingMode_Fire;
        public InputActionMap Get() { return m_Wrapper.m_BuildingMode; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BuildingModeActions set) { return set.Get(); }
        public void SetCallbacks(IBuildingModeActions instance)
        {
            if (m_Wrapper.m_BuildingModeActionsCallbackInterface != null)
            {
                @Look.started -= m_Wrapper.m_BuildingModeActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_BuildingModeActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_BuildingModeActionsCallbackInterface.OnLook;
                @RotateCamera.started -= m_Wrapper.m_BuildingModeActionsCallbackInterface.OnRotateCamera;
                @RotateCamera.performed -= m_Wrapper.m_BuildingModeActionsCallbackInterface.OnRotateCamera;
                @RotateCamera.canceled -= m_Wrapper.m_BuildingModeActionsCallbackInterface.OnRotateCamera;
                @MoveCameraPivot.started -= m_Wrapper.m_BuildingModeActionsCallbackInterface.OnMoveCameraPivot;
                @MoveCameraPivot.performed -= m_Wrapper.m_BuildingModeActionsCallbackInterface.OnMoveCameraPivot;
                @MoveCameraPivot.canceled -= m_Wrapper.m_BuildingModeActionsCallbackInterface.OnMoveCameraPivot;
                @Zoom.started -= m_Wrapper.m_BuildingModeActionsCallbackInterface.OnZoom;
                @Zoom.performed -= m_Wrapper.m_BuildingModeActionsCallbackInterface.OnZoom;
                @Zoom.canceled -= m_Wrapper.m_BuildingModeActionsCallbackInterface.OnZoom;
                @DragCamera.started -= m_Wrapper.m_BuildingModeActionsCallbackInterface.OnDragCamera;
                @DragCamera.performed -= m_Wrapper.m_BuildingModeActionsCallbackInterface.OnDragCamera;
                @DragCamera.canceled -= m_Wrapper.m_BuildingModeActionsCallbackInterface.OnDragCamera;
                @Fire.started -= m_Wrapper.m_BuildingModeActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_BuildingModeActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_BuildingModeActionsCallbackInterface.OnFire;
            }
            m_Wrapper.m_BuildingModeActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @RotateCamera.started += instance.OnRotateCamera;
                @RotateCamera.performed += instance.OnRotateCamera;
                @RotateCamera.canceled += instance.OnRotateCamera;
                @MoveCameraPivot.started += instance.OnMoveCameraPivot;
                @MoveCameraPivot.performed += instance.OnMoveCameraPivot;
                @MoveCameraPivot.canceled += instance.OnMoveCameraPivot;
                @Zoom.started += instance.OnZoom;
                @Zoom.performed += instance.OnZoom;
                @Zoom.canceled += instance.OnZoom;
                @DragCamera.started += instance.OnDragCamera;
                @DragCamera.performed += instance.OnDragCamera;
                @DragCamera.canceled += instance.OnDragCamera;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
            }
        }
    }
    public BuildingModeActions @BuildingMode => new BuildingModeActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_TouchSchemeIndex = -1;
    public InputControlScheme TouchScheme
    {
        get
        {
            if (m_TouchSchemeIndex == -1) m_TouchSchemeIndex = asset.FindControlSchemeIndex("Touch");
            return asset.controlSchemes[m_TouchSchemeIndex];
        }
    }
    private int m_JoystickSchemeIndex = -1;
    public InputControlScheme JoystickScheme
    {
        get
        {
            if (m_JoystickSchemeIndex == -1) m_JoystickSchemeIndex = asset.FindControlSchemeIndex("Joystick");
            return asset.controlSchemes[m_JoystickSchemeIndex];
        }
    }
    private int m_XRSchemeIndex = -1;
    public InputControlScheme XRScheme
    {
        get
        {
            if (m_XRSchemeIndex == -1) m_XRSchemeIndex = asset.FindControlSchemeIndex("XR");
            return asset.controlSchemes[m_XRSchemeIndex];
        }
    }
    public interface IBuildingModeActions
    {
        void OnLook(InputAction.CallbackContext context);
        void OnRotateCamera(InputAction.CallbackContext context);
        void OnMoveCameraPivot(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
        void OnDragCamera(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
    }
}
