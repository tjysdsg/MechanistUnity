using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{
    public Transform focusedObject;
    public InputActionReference mouseLookInputActionRef;
    public Transform cameraTransform;
    public float cameraRotateSpeed = 20;

    private bool _rotating = false;

    public void Update()
    {
        if (_rotating)
        {
            Vector3 delta = mouseLookInputActionRef.action.ReadValue<Vector2>();
            Vector3 axis = cameraTransform.TransformDirection(Vector3.Cross(Vector3.forward, delta));

            cameraTransform.RotateAround(focusedObject.position, axis, delta.magnitude * cameraRotateSpeed);
            cameraTransform.LookAt(focusedObject);
        }
    }

    public void OnRotate(InputAction.CallbackContext callbackContext)
    {
        _rotating = callbackContext.ReadValue<float>() > 0.001;
    }
}