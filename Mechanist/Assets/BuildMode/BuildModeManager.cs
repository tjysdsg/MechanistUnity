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

        [Header("Camera")] public BuildModePivotSO buildModePivotSO;
        [SerializeField] private CurrentCameraSO currentCamera;

        [Header("Building Block")] [SerializeField]
        public BlockTypeSO currentBlockType;

        [Header("Event Channels")]
        [Tooltip("The event channel used to tell the build mode camera to move to a certain object")]
        [SerializeField]
        public Vector3EventChannelSO moveToEventChannel;

        [HideInInspector] public bool isBuilding = false;
        [HideInInspector] public RaycastHit? selectionHitInfo;
        [HideInInspector] public Vector3? cameraPivotPos = null;

        private readonly List<BaseBlock> _createdBlocks = new List<BaseBlock>();

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
            if (isBuilding && Physics.Raycast(ray, out RaycastHit info))
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
            if (!isBuilding && Physics.Raycast(ray, out RaycastHit info))
            {
                cameraPivotPos = info.point;
            }
        }

        public void OnEnterPlacementMode()
        {
            isBuilding = !isBuilding;
        }

        public void AddCreatedBlock(BaseBlock block)
        {
            _createdBlocks.Add(block);
        }

        public void OnGameModeChange(GameMode mode)
        {
            // we always reset no matter what game mode we entered
            isBuilding = false;
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