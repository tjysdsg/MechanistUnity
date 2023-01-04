using System.Collections.Generic;
using Block;
using Core;
using UnityEngine;
using GameState;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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

        [Tooltip("The event channel is for UI controller to tell us what current block user selected, " +
                 "and for us to tell UI what we're building")]
        [SerializeField]
        private BlockTypeEventChannelSO BlockTypeUISelectionEventChannel;

        [Tooltip("The event channel is for us to tell UI controller what state we are in," +
                 "such as placement, ball editor, ...")]
        [SerializeField]
        private StringEventChannelSO CurrentBuildStateEventChannel;

        /// <summary>
        /// Did user left-clicked the mouse
        /// </summary>
        [HideInInspector] public bool isFired = false;

        [HideInInspector]
        public bool escPressed = false;

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

        public List<BaseBlock> blocksBeingEdited = new List<BaseBlock>();

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

            BlockTypeUISelectionEventChannel.OnEventRaised -= OnBlockTypeSelected;
            BlockTypeUISelectionEventChannel.OnEventRaised += OnBlockTypeSelected;
        }

        private void OnDisable()
        {
            inputManager.FireEvent -= OnFire;
            inputManager.DoubleFireEvent -= OnDoubleFire;
            inputManager.EscPressedEvent -= OnEsc;
        }

        private void Update()
        {
            // notify UI using event channels
            if (_sm.CurrentStateName != _prevState)
            {
                _prevState = _sm.CurrentStateName;
                CurrentBuildStateEventChannel.RaiseEvent(_prevState);
            }

            if (currentBlockType.type != _prevBlockType)
            {
                _prevBlockType = currentBlockType.type;
                BlockTypeUISelectionEventChannel.RaiseEvent(_prevBlockType);
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
                currentBlockType.type = BlockType.None;

            blocksBeingEdited.Clear();
            escPressed = false;
            isFired = false;
            selectionHitInfo = null;
            selectionRay = new Ray(Vector3.zero, Vector3.zero);
            cameraPivotPos = null;
        }

        public void SelectBlockToEdit(BaseBlock block)
        {
            blocksBeingEdited.Clear();
            blocksBeingEdited.Add(block);
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
            if (!currentBlockType.IsNone()) return;

            Vector2 pointer = inputManager.GetPointerScreenPosition();
            Ray ray = currentCamera.camera.ScreenPointToRay(pointer);
            if (Physics.Raycast(ray, out RaycastHit info))
            {
                cameraPivotPos = info.point;
            }
        }

        #endregion

        #region UI Event Handling

        private void OnBlockTypeSelected(BlockType blockType)
        {
            // changing block type cancels the current build action
            if (currentBlockType.type != blockType)
            {
                currentBlockType.type = blockType;
                ResetStateMachine(false);
            }
        }

        #endregion

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