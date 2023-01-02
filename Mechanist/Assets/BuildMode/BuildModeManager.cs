using UnityEngine;
using GameState;

namespace BuildMode
{
    [RequireComponent(typeof(StateMachine.StateMachine))]
    public class BuildModeManager : MonoBehaviour
    {
        [Tooltip("The event channel used to tell the build mode camera to move to a certain object")] [SerializeField]
        public Vector3EventChannelSO moveToEventChannel;

        [SerializeField] private InputManager inputManager;
        [SerializeField] private CurrentCameraSO currentCamera;
        [SerializeField] private GameModeSO gameMode;
        [SerializeField] public GameObject currBlockPrefab;

        [HideInInspector] public bool twoClickBuilding = false;
        [HideInInspector] public RaycastHit? selectionHitInfo;
        [HideInInspector] public Vector3? cameraPivotPos = null;

        public void OnEnable()
        {
            inputManager.BuildingModeEnterPlacementEvent += OnEnterPlacementMode;
            inputManager.BuildingModeFireEvent += OnFire;
            inputManager.BuildingModeDoubleFireEvent += OnDoubleFire;

            gameMode.OnEventRaised += OnGameModeChange;
        }

        public void OnDisable()
        {
            inputManager.BuildingModeEnterPlacementEvent -= OnEnterPlacementMode;
            inputManager.BuildingModeFireEvent -= OnFire;
            inputManager.BuildingModeDoubleFireEvent -= OnDoubleFire;
        }

        /// <summary>
        /// User clicked left mouse button and the BuildingModeCamera dispatch the event to us
        /// </summary>
        public void OnFire()
        {
            Vector2 pointer = inputManager.GetBuildModePointerInput();
            Ray ray = currentCamera.camera.ScreenPointToRay(pointer);
            if (twoClickBuilding && Physics.Raycast(ray, out RaycastHit info))
                selectionHitInfo = info;
            else
                selectionHitInfo = null;
        }

        /// <summary>
        /// User double clicked left mouse button and the BuildingModeCamera dispatch the event to us
        /// </summary>
        public void OnDoubleFire()
        {
            Vector2 pointer = inputManager.GetBuildModePointerInput();
            Ray ray = currentCamera.camera.ScreenPointToRay(pointer);
            if (!twoClickBuilding && Physics.Raycast(ray, out RaycastHit info))
            {
                cameraPivotPos = info.point;
            }
        }

        public void OnEnterPlacementMode()
        {
            twoClickBuilding = !twoClickBuilding;
        }

        public void OnGameModeChange(GameMode mode)
        {
            // we always reset no matter what game mode we entered
            twoClickBuilding = false;
            selectionHitInfo = null;
            cameraPivotPos = null;

            // enable/disable this game object
            if (mode == GameMode.BuildMode)
                gameObject.SetActive(true);
            else
                gameObject.SetActive(false);
        }
    }
}