using UnityEngine;
using Block;
using JetBrains.Annotations;

namespace BuildMode
{
    [RequireComponent(typeof(StateMachine.StateMachine))]
    public class BuildModeManager : MonoBehaviour
    {
        [Tooltip("The event channel used to tell the build mode camera to move to a certain object")] [SerializeField]
        public Vector3EventChannelSO moveToEventChannel;

        [SerializeField] private InputManager inputManager;
        [SerializeField] public GameObject currBlockPrefab;

        [HideInInspector] public bool twoClickBuilding = false;

        [HideInInspector] [CanBeNull] public RaycastHit? selectionHitInfo;

        [HideInInspector] public Vector3? cameraPivotPos = null;

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
            if (twoClickBuilding && Physics.Raycast(ray, out RaycastHit info))
                selectionHitInfo = info;
            else
                selectionHitInfo = null;
        }

        /// <summary>
        /// User double clicked left mouse button and the BuildingModeCamera dispatch the event to us
        /// </summary>
        public void OnCameraDoubleFire(Ray ray)
        {
            if (!twoClickBuilding && Physics.Raycast(ray, out RaycastHit info))
            {
                cameraPivotPos = info.point;
            }
        }

        public void OnEnterPlacementMode()
        {
            twoClickBuilding = !twoClickBuilding;
        }
    }
}