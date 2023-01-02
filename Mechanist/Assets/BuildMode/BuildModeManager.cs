using System.Collections.Generic;
using Block;
using UnityEngine;
using GameState;

namespace BuildMode
{
    [RequireComponent(typeof(StateMachine.StateMachine))]
    public class BuildModeManager : MonoBehaviour
    {
        [Header("Configs")] [SerializeField] private GameModeSO gameMode;
        [SerializeField] private InputManager inputManager;
        [SerializeField] public CurrentCameraSO currentCamera;

        [Header("Building Block")] [SerializeField]
        public BlockTypeSO currentBlockType;

        [Header("Event Channels")]
        [Tooltip("The event channel used to tell the build mode camera to move to a certain object")]
        [SerializeField]
        public Vector3EventChannelSO moveToEventChannel;

        [HideInInspector] public bool isPlacing = false;

        /// <summary>
        /// Did user left-clicked the mouse to build something
        /// </summary>
        [HideInInspector] public bool isFired = false;

        /// <summary>
        /// The pivot for the camera to go to
        /// </summary>
        [HideInInspector] public Vector3? cameraPivotPos = null;

        /// <summary>
        /// The ray fired when user left-clicked the mouse to build something
        /// </summary>
        [HideInInspector] public Ray selectionRay;

        /// <summary>
        /// The hit result of <see cref="selectionRay"/> at the time of firing
        /// </summary>
        [HideInInspector] public RaycastHit? selectionHitInfo = null;

        private readonly List<BaseBlock> _createdBlocks = new List<BaseBlock>();

        private void OnEnable()
        {
            inputManager.BuildingModeEnterPlacementEvent += OnEnterPlacementMode;
            inputManager.BuildingModeFireEvent += OnFire;
            inputManager.BuildingModeDoubleFireEvent += OnDoubleFire;

            gameMode.OnEventRaised += OnGameModeChange;
        }

        private void OnDisable()
        {
            inputManager.BuildingModeEnterPlacementEvent -= OnEnterPlacementMode;
            inputManager.BuildingModeFireEvent -= OnFire;
            inputManager.BuildingModeDoubleFireEvent -= OnDoubleFire;
        }

        public void AddCreatedBlock(BaseBlock block)
        {
            _createdBlocks.Add(block);
        }

        public void ResetStateMachine(bool exitPlacement)
        {
            if (exitPlacement)
                isPlacing = false;

            isFired = false;
            selectionHitInfo = null;
            selectionRay = new Ray(Vector3.zero, Vector3.zero);
        }

        #region Input Handling

        /// <summary>
        /// User clicked left mouse button and the BuildingModeCamera dispatch the event to us
        /// </summary>
        public void OnFire()
        {
            if (!isPlacing) return;

            isFired = true;
            Vector2 pointer = inputManager.GetBuildModePointerInput();
            selectionRay = currentCamera.camera.ScreenPointToRay(pointer);
            if (isPlacing && Physics.Raycast(selectionRay, out RaycastHit info))
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
            if (!isPlacing && Physics.Raycast(ray, out RaycastHit info))
            {
                cameraPivotPos = info.point;
            }
        }

        public void OnEnterPlacementMode()
        {
            isPlacing = !isPlacing;
        }

        #endregion

        public void OnGameModeChange(GameMode mode)
        {
            // we always reset no matter what game mode we entered
            isPlacing = false;
            selectionHitInfo = null;
            cameraPivotPos = null;

            // enable/disable this game object
            if (mode == GameMode.BuildMode)
            {
                gameObject.SetActive(true);
                foreach (var block in _createdBlocks)
                {
                    block.EnterBuildMode();
                }
            }
            else
            {
                gameObject.SetActive(false);
                foreach (var block in _createdBlocks)
                {
                    block.EnterPlayMode();
                }
            }
        }
    }
}