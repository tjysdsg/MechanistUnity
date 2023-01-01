using UnityEngine;

public class BuildModeManager : MonoBehaviour
{
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
}