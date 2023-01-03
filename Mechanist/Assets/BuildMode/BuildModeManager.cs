using Block;
using Core;
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
        [SerializeField] private LayerMask attachableBlockMask;
        [SerializeField] private BlockListSO allBlocks;

        [Header("Building Block")] [SerializeField]
        public BlockTypeSO currentBlockType;

        [Header("Event Channels")]
        [Tooltip("The event channel used to tell the build mode camera to move to a certain object")]
        [SerializeField]
        public Vector3EventChannelSO moveToEventChannel;

        [Tooltip("The event channel is for UI controller to tell us what current block type we're building")]
        [SerializeField]
        private BlockTypeEventChannelSO BlockTypeUISelectionEventChannel;

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

        private void OnEnable()
        {
            inputManager.BuildingModeEnterPlacementEvent += OnEnterPlacementMode;
            inputManager.BuildingModeFireEvent += OnFire;
            inputManager.BuildingModeDoubleFireEvent += OnDoubleFire;

            // use -= then += to avoid being called multiple times after switching the game mode

            gameMode.OnEventRaised -= OnGameModeChange;
            gameMode.OnEventRaised += OnGameModeChange;

            BlockTypeUISelectionEventChannel.OnEventRaised -= OnBlockTypeSelected;
            BlockTypeUISelectionEventChannel.OnEventRaised += OnBlockTypeSelected;
        }

        private void OnDisable()
        {
            inputManager.BuildingModeEnterPlacementEvent -= OnEnterPlacementMode;
            inputManager.BuildingModeFireEvent -= OnFire;
            inputManager.BuildingModeDoubleFireEvent -= OnDoubleFire;
        }

        #region APIs Used by State Actions

        public void AddCreatedBlock(BaseBlock block)
        {
            allBlocks.blocks.Add(block);
        }

        public void ResetStateMachine(bool exitPlacement)
        {
            if (exitPlacement)
                isPlacing = false;

            isFired = false;
            selectionHitInfo = null;
            selectionRay = new Ray(Vector3.zero, Vector3.zero);
        }

        #endregion

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
            if (isPlacing && Physics.Raycast(
                    ray: selectionRay, hitInfo: out RaycastHit info, maxDistance: Mathf.Infinity,
                    layerMask: attachableBlockMask
                ))
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

        #region UI Event Handling

        private void OnBlockTypeSelected(BlockType blockType)
        {
            // changing block type cancels the current build action
            if (currentBlockType.type != blockType)
            {
                currentBlockType.type = blockType;
                ResetStateMachine(true);
            }
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
                foreach (var block in allBlocks.blocks)
                {
                    block.EnterBuildMode();
                }

                // notify UI the current block type
                BlockTypeUISelectionEventChannel.RaiseEvent(currentBlockType.type);
            }
            else
            {
                gameObject.SetActive(false);
                foreach (var block in allBlocks.blocks)
                {
                    block.EnterPlayMode();
                }
            }
        }
    }
}