using UnityEngine;

namespace BuildMode
{
    public class BuildModeManager : MonoBehaviour
    {
        [Tooltip("The event channel used to tell the build mode camera to move to a certain object")] [SerializeField]
        private Vector3EventChannelSO moveToEventChannel;

        [SerializeField] private BuildModeStateSO state;

        [SerializeField] private InputManager inputManager;

        public void OnEnable()
        {
            inputManager.BuildingModeEnterPlacementEvent += OnEnterPlacementMode;
        }

        public void OnDisable()
        {
            inputManager.BuildingModeEnterPlacementEvent -= OnEnterPlacementMode;
        }

        /// <summary>
        /// User clicked left mouse button and the BuildingModeCamera dispatch the event to us
        /// </summary>
        public void OnCameraFire(Ray ray)
        {
            if (state.state == BuildModeState.Placement && Physics.Raycast(ray, out RaycastHit info))
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
            if (state.state != BuildModeState.Placement && Physics.Raycast(ray, out RaycastHit info))
            {
                moveToEventChannel.RaiseEvent(info.point);
            }
        }

        public void OnEnterPlacementMode()
        {
            // TODO: UI for display current build mode state
            if (state.state == BuildModeState.Placement)
                state.state = BuildModeState.None;
            else
                state.state = BuildModeState.Placement;
        }
    }
}