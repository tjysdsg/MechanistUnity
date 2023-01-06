using System.Collections.Generic;
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
        [SerializeField] public CameraSO currentCamera;
        [SerializeField] private LayerMask attachableBlockMask;
        [SerializeField] private BlockListSO allBlocks;

        [Header("Building Block")] [SerializeField]
        public BlockConfigSO currentBlockConfig;

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
        [HideInInspector] public bool isFired = false;

        [HideInInspector] public bool escPressed = false;

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

        public HashSet<BaseBlock> blocksBeingEdited = new HashSet<BaseBlock>();

        // =============================================

        private string _prevState = "";
        private BlockType _prevBlockType = BlockType.None;
        StateMachine.StateMachine _sm;

        private void Start()
        {
            _sm = GetComponent<StateMachine.StateMachine>();
        }

        private void OnEnable()
        {
            inputManager.FireEvent += OnFire;
            inputManager.DoubleFireEvent += OnDoubleFire;
            inputManager.EscPressedEvent += OnEsc;

            // we want to keep these callbacks below active even if this game object is disable
            // but we need to make sure they're not registered for multiple times

            gameMode.OnEventRaised -= OnGameModeChange;
            gameMode.OnEventRaised += OnGameModeChange;

            blockTypeUISelectionEventChannel.OnEventRaised += OnBlockTypeSelected;
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

            if (currentBlockConfig.type != _prevBlockType)
            {
                _prevBlockType = currentBlockConfig.type;
                blockTypeUISelectionEventChannel.RaiseEvent(_prevBlockType);
            }
        }

        #region APIs Used by State Actions

        public void AddCreatedBlock(BaseBlock block)
        {
            allBlocks.blocks.Add(block);
        }

        public void ResetStateMachine(bool clearCurrentBlockTypeSelection)
        {
            if (clearCurrentBlockTypeSelection)
                currentBlockConfig.type = BlockType.None;

            blocksBeingEdited.Clear();
            escPressed = false;
            isFired = false;
            selectionHitInfo = null;
            selectionRay = new Ray(Vector3.zero, Vector3.zero);
            cameraPivotPos = null;
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
                        currentBlockConfig.GetDimmedMaterial(block.GetBlockType());
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
                    currentBlockConfig.GetBuildModeMaterial(block.GetBlockType());
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
                    layerMask: attachableBlockMask
                ))
                selectionHitInfo = info;
        }

        /// <summary>
        /// User double clicked left mouse button and the BuildingModeCamera dispatch the event to us
        /// </summary>
        public void OnDoubleFire()
        {
            if (!currentBlockConfig.IsNone()) return;

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
            if (currentBlockConfig.type != blockType)
            {
                currentBlockConfig.type = blockType;
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
                blockTypeUISelectionEventChannel.RaiseEvent(currentBlockConfig.type);
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

        #endregion
    }
}