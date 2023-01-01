using UnityEngine;

public class BuildModeManager : MonoBehaviour
{
    [Tooltip("The event channel used to tell the build mode camera to move to a certain object")] [SerializeField]
    private Vector3EventChannelSO moveToEventChannel;

    /// <summary>
    /// User clicked left mouse button and the BuildingModeCamera dispatch the event to us
    /// </summary>
    public void OnCameraFire(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit info))
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = info.point;
        }
    }

    /// <summary>
    /// User double clicked left mouse button and the BuildingModeCamera dispatch the event to us
    /// </summary>
    public void OnCameraDoubleFire(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit info))
        {
            moveToEventChannel.RaiseEvent(info.point);
        }
    }
}