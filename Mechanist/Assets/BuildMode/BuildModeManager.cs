using System.Collections.Generic;
using Block;
using Core;
using UnityEngine;
using GameState;
using UnityEngine.Serialization;

namespace BuildMode
{
    [RequireComponent(typeof(StateMachine.StateMachine))]
    public class BuildModeManager : MonoBehaviour
    {
        [Header("Configs")] [SerializeField] private GameModeEventChannelSO gameModeEventChannel;
        [SerializeField] private InputManager inputManager;
        [SerializeField] public CameraSO currentCamera;
        [SerializeField] private BlockListSO allBlocks;
        [SerializeField] private LayerMask raycastMask;

        [Header("Building Block")] [SerializeField]
        public BlockConfigSO blockConfig;

        [Header("Event Channels")]
        [Tooltip("The event channel used to tell the build mode camera to move to a certain object")]
        [SerializeField]
        public Vector3EventChannelSO moveToEventChannel;

        [Header("UI Event Channels")]
        [Tooltip("The event channel is for UI controller to tell us what current block user selected, " +
                 "and for us to tell UI what we're building")]
        [SerializeField]
        private BlockTypeEventChannelSO blockTypeUISelectionEventChannel;

        [SerializeField] public VoidEventChannelSO usePositionTransformHandleEventChannel;
        [SerializeField] public VoidEventChannelSO useRotationTransformHandleEventChannel;
        [SerializeField] private StringEventChannelSO currentBuildStateEventChannel;

        /// <summary>
        /// Did user left-clicked the mouse
        /// </summary>
        [Header("Status variables")] public bool isFired = false;

        public bool escPressed = false;

        /// <summary>
        /// The pivot for the camera to go to
        /// </summary>
        public Vector3? cameraPivotPos = null;

        /// <summary>
        /// The ray fired when user left-clicked the mouse to build something
        /// </summary>
        public Ray selectionRay;

        /// <summary>
        /// The hit result of <see cref="selectionRay"/> at the time of firing
        /// </summary>
        public RaycastHit? selectionHitInfo = null;

        public HashSet<BaseBlock> blocksBeingEdited = new HashSet<BaseBlock>();

        /// <summary>
        /// The connection index of <see cref="blocksBeingEdited"/> that is currently being edited
        /// </summary>
        public int connectionIndexBeingEdited = -1;

        // =============================================

        private string _prevState = "";
        private BlockType _prevBlockType = BlockType.None;
        private BlockType _currentBlockType = BlockType.None;
        public BlockType CurrentBlockType => _currentBlockType;

        private StateMachine.StateMachine _sm;

        private void OnEnable()
        {
            inputManager.FireEvent += OnFire;
            inputManager.DoubleFireEvent += OnDoubleFire;
            inputManager.EscPressedEvent += OnEsc;

            // we want to keep these callbacks below active even if this game object is disable
            // but we need to make sure they're not registered for multiple times

            gameModeEventChannel.OnEventRaised -= OnGameModeChange;
            gameModeEventChannel.OnEventRaised += OnGameModeChange;

            blockTypeUISelectionEventChannel.OnEventRaised += OnBlockTypeSelected;

            _sm = GetComponent<StateMachine.StateMachine>();
        }

        private void OnDisable()
        {
            inputManager.FireEvent -= OnFire;
            inputManager.DoubleFireEvent -= OnDoubleFire;
            inputManager.EscPressedEvent -= OnEsc;

            blockTypeUISelectionEventChannel.OnEventRaised -= OnBlockTypeSelected;
        }

        private void Update()
        {
            // notify UI using event channels
            if (_sm.CurrentStateName != _prevState)
            {
                _prevState = _sm.CurrentStateName;
                currentBuildStateEventChannel.RaiseEvent(_prevState);
            }

            if (_currentBlockType != _prevBlockType)
            {
                _prevBlockType = _currentBlockType;
                blockTypeUISelectionEventChannel.RaiseEvent(_prevBlockType);
            }
        }

        #region APIs Used by State Actions

        public void AddCreatedBlock(BaseBlock block)
        {
            allBlocks.blocks.Add(block);
        }

        /// <summary>
        /// Reset status variables related to the fire input
        /// </summary>
        public void ResetFireEventStatus()
        {
            isFired = false;
            selectionHitInfo = null;
            selectionRay = new Ray(Vector3.zero, Vector3.zero);
        }

        public void ResetStateMachine(bool clearCurrentBlockTypeSelection)
        {
            if (clearCurrentBlockTypeSelection)
                _currentBlockType = BlockType.None;

            blocksBeingEdited.Clear();
            escPressed = false;
            cameraPivotPos = null;
            connectionIndexBeingEdited = -1;
            ResetFireEventStatus();
            ResetHighlight();
        }

        public void SelectBlockToEdit(BaseBlock block)
        {
            blocksBeingEdited.Clear();
            blocksBeingEdited.Add(block);
        }

        /// <summary>
        /// 1. Highlight current selected block with an outline
        /// 2. Dim all blocks other than currently selected ones by apply a different material
        /// </summary>
        public void HighlightCurrentlySelectedBlock()
        {
            foreach (var block in blocksBeingEdited)
            {
                block.gameObject.layer = ObjectLayer.GetOutlinedBlockLayerIndex();
            }

            // dim other blocks
            foreach (var block in allBlocks.blocks)
            {
                if (!blocksBeingEdited.Contains(block))
                {
                    block.GetComponent<MeshRenderer>().material =
                        blockConfig.GetDimmedMaterial(block.GetBlockType());
                }
            }
        }

        public void ResetHighlight()
        {
            foreach (var block in allBlocks.blocks)
            {
                if (block.gameObject.layer == ObjectLayer.GetOutlinedBlockLayerIndex())
                    block.gameObject.layer = ObjectLayer.GetBuildModeBlockLayerIndex();

                block.GetComponent<MeshRenderer>().material =
                    blockConfig.GetBuildModeMaterial(block.GetBlockType());
            }
        }

        #endregion

        #region Input Handling

        private void OnEsc()
        {
            escPressed = true;
        }

        /// <summary>
        /// User clicked left mouse button and the BuildingModeCamera dispatch the event to us
        /// </summary>
        public void OnFire()
        {
            isFired = true;
            Vector2 pointer = inputManager.GetPointerScreenPosition();
            selectionRay = currentCamera.camera.ScreenPointToRay(pointer);
            selectionHitInfo = null;
            if (Physics.Raycast(
                    ray: selectionRay, hitInfo: out RaycastHit info, maxDistance: Mathf.Infinity,
                    layerMask: raycastMask
                ))
                selectionHitInfo = info;
        }

        /// <summary>
        /// User double clicked left mouse button and the BuildingModeCamera dispatch the event to us
        /// </summary>
        public void OnDoubleFire()
        {
            if (_currentBlockType != BlockType.None) return;

            Vector2 pointer = inputManager.GetPointerScreenPosition();
            Ray ray = currentCamera.camera.ScreenPointToRay(pointer);
            if (Physics.Raycast(ray, out RaycastHit info))
            {
                cameraPivotPos = info.point;
            }
        }

        #endregion

        #region Event Handling

        private void OnBlockTypeSelected(BlockType blockType)
        {
            // changing block type cancels the current build action
            if (_currentBlockType != blockType)
            {
                _currentBlockType = blockType;
                ResetStateMachine(false);
            }
        }

        public void OnGameModeChange(GameMode mode)
        {
            // we always reset no matter what game mode we entered
            ResetStateMachine(true);

            // enable/disable this game object
            if (mode == GameMode.BuildMode)
            {
                gameObject.SetActive(true);
                foreach (var block in allBlocks.blocks)
                {
                    block.EnterBuildMode();
                }

                // notify UI the current block type
                blockTypeUISelectionEventChannel.RaiseEvent(_currentBlockType);
            }
            else
            {
                _sm.Update(); // give StateAction one last chance to clean up
                gameObject.SetActive(false);
                foreach (var block in allBlocks.blocks)
                {
                    block.EnterPlayMode();
                }
            }
        }

        #endregion
    }
}