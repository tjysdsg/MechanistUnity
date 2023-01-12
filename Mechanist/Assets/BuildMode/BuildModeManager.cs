using System;
using System.Linq;
using Block;
using Core;
using UnityEngine;
using GameState;
using TransformHandle;
using UnityEngine.Assertions;

namespace BuildMode
{
    public enum BuildModeState
    {
        None,
        SingleClickBuild,
        TwoClickBuild,
        BallEdit,
        BallConnectionEdit,
    }

    public class BuildModeManager : MonoBehaviour
    {
        [Header("Configs")] [SerializeField] private GameModeEventChannelSO gameModeEventChannel;
        [SerializeField] private InputManager inputManager;
        [SerializeField] public CameraSO currentCamera;
        [SerializeField] private BlockListSO allBlocks;
        [SerializeField] private UIStateSO _uiState;
        [SerializeField] private LayerMask raycastMask;

        [Header("Building Block")] //
        [SerializeField]
        public BlockConfigSO blockConfig;

        [SerializeField] private BlockConnectionConfigSO _blockConnectionConfig;

        [Header("Event Channels")]
        [Tooltip("The event channel used to tell the build mode camera to move to a certain object")]
        [SerializeField]
        public Vector3EventChannelSO moveToEventChannel;

        [Header("UI Event Channels")]
        [Tooltip("The event channel is for UI controller to tell us what current block user selected")]
        [SerializeField]
        private BlockTypeEventChannelSO blockTypeUISelectionEventChannel;

        [SerializeField] public VoidEventChannelSO usePositionTransformHandleEventChannel;
        [SerializeField] public VoidEventChannelSO useRotationTransformHandleEventChannel;
        [SerializeField] private BlockConnectionTypeEventChannelSO _blockConnectionTypeUISelectionEventChannel;
        [SerializeField] private VoidEventChannelSO _rotateHingeConnectionEventChannel;

        #region Status variables

        private bool _escPressed = false;

        private BuildModeState _state = BuildModeState.None;
        private BuildModeState _prevState = BuildModeState.None;

        private BlockType _currentBlockType = BlockType.None;

        private class OnScreenSelectionData
        {
            /// <summary>
            /// Is the left mouse button clicked
            /// </summary>
            public bool isFired = false;

            /// <summary>
            /// Did raycast hit a block
            /// </summary>
            public bool didHitBlock = false;

            /// <summary>
            /// The ray fired when user left-clicked the mouse to build something
            /// </summary>
            public Ray ray;

            /// <summary>
            /// The hit result of <see cref="ray"/> at the time of firing
            /// </summary>
            public RaycastHit hitInfo;

            public void Clear()
            {
                isFired = false;
                didHitBlock = false;
            }
        }

        private OnScreenSelectionData _onScreenSelectionData = new OnScreenSelectionData();

        private struct TwoClickBuildData
        {
            public TheBall firstBlock;
            public bool firstBlockSelected;

            public void Clear()
            {
                firstBlock = null;
                firstBlockSelected = false;
            }
        }

        private TwoClickBuildData _twoClickBuildData = new TwoClickBuildData();

        private struct BallEditData
        {
            public RuntimeTransformHandle transformHandle;
            public TheBall ball;

            public void Clear()
            {
                transformHandle = null;
                ball = null;
            }

            public void SetTransformHandleTypeAsPosition()
            {
                transformHandle.type = HandleType.POSITION;
            }

            public void SetTransformHandleTypeAsRotation()
            {
                transformHandle.type = HandleType.ROTATION;
            }
        }

        private BallEditData _ballEditData = new BallEditData();

        private class BallConnectionEditData
        {
            public TheBall ball = null;
            public int connectionIndex = -1;
            public bool isRotatingHingeConnection = false;

            public void Clear()
            {
                ball = null;
                connectionIndex = -1;
                isRotatingHingeConnection = false;
            }
        }

        private readonly BallConnectionEditData _ballConnectionEditData = new BallConnectionEditData();

        #endregion

        private void OnEnable()
        {
            inputManager.FireEvent += OnScreenSelection;
            inputManager.DoubleFireEvent += OnDoubleFire;
            inputManager.EscPressedEvent += OnEsc;

            // we want to keep these callbacks below active even if this game object is disable
            // but we need to make sure they're not registered for multiple times

            gameModeEventChannel.OnEventRaised -= OnGameModeChange;
            gameModeEventChannel.OnEventRaised += OnGameModeChange;

            blockTypeUISelectionEventChannel.OnEventRaised += OnUIBlockTypeSelected;
            _blockConnectionTypeUISelectionEventChannel.OnEventRaised += OnUIBlockConnectionTypeSelected;
            _rotateHingeConnectionEventChannel.OnEventRaised += OnUIRotateHingeConnection;
        }

        private void OnDisable()
        {
            inputManager.FireEvent -= OnScreenSelection;
            inputManager.DoubleFireEvent -= OnDoubleFire;
            inputManager.EscPressedEvent -= OnEsc;

            blockTypeUISelectionEventChannel.OnEventRaised -= OnUIBlockTypeSelected;
            _blockConnectionTypeUISelectionEventChannel.OnEventRaised -= OnUIBlockConnectionTypeSelected;
            _rotateHingeConnectionEventChannel.OnEventRaised -= OnUIRotateHingeConnection;
        }

        private void Start()
        {
            ToNoneState();
        }

        private void Update()
        {
            _uiState.currentBlockType = _currentBlockType;

            // check for state change
            if (_prevState != _state)
            {
                Debug.Log($"Build mode state changed from {_prevState} to {_state}");

                // notify UI
                _uiState.currentBuildState = _state.ToString();

                if (_prevState is BuildModeState.SingleClickBuild or BuildModeState.TwoClickBuild)
                    _currentBlockType = BlockType.None;

                if (_prevState == BuildModeState.TwoClickBuild)
                    _twoClickBuildData.Clear();

                if (_prevState == BuildModeState.BallEdit)
                    OnExitBallEditState();
                if (_prevState == BuildModeState.BallConnectionEdit)
                    OnExitBallConnectionEditState();

                if (_state == BuildModeState.BallConnectionEdit)
                    OnEnterBallConnectionEditState();
            }

            // ======================== state check can only happen above this line ========================
            // ======================== state change can only happen below this line ========================
            _prevState = _state;
            switch (_state)
            {
                case BuildModeState.None:
                    NoneStateUpdate();
                    break;
                case BuildModeState.SingleClickBuild:
                    SingleClickBuildStateUpdate();
                    break;
                case BuildModeState.TwoClickBuild:
                    TwoClickBuildStateUpdate();
                    break;
                case BuildModeState.BallEdit:
                    BallEditStateUpdate();
                    break;
                case BuildModeState.BallConnectionEdit:
                    BallConnectionEditStateUpdate();
                    break;
            }

            _uiState.isEditingBall = _state == BuildModeState.BallEdit;
            _uiState.blockConnectionEditorUIData.isEditingBallConnection = _state == BuildModeState.BallConnectionEdit;

            if (_escPressed)
            {
                if (_state == BuildModeState.BallConnectionEdit)
                    ToBallEditState(_ballConnectionEditData.ball);
                else
                    ToNoneState();

                _escPressed = false;
            }
        }

        private void ToNoneState()
        {
            _state = BuildModeState.None;
        }

        private void NoneStateUpdate()
        {
            if (_onScreenSelectionData.isFired)
            {
                if (_currentBlockType != BlockType.None) // build a block
                {
                    ToNClickBuildState();
                }
                else if (_onScreenSelectionData.didHitBlock) // select an existing block to edit
                {
                    var t = _onScreenSelectionData.hitInfo.transform;
                    var ball = t.GetComponent<TheBall>();
                    if (ball != null)
                        ToBallEditState(ball);
                }

                _onScreenSelectionData.Clear();
            }
        }

        private void ToNClickBuildState()
        {
            if (blockConfig.IsSingleClickBuild(_currentBlockType))
            {
                _state = BuildModeState.SingleClickBuild;
            }
            else
            {
                Assert.IsTrue(blockConfig.IsTwoClickBuild(_currentBlockType));
                _state = BuildModeState.TwoClickBuild;
            }
        }

        private void SingleClickBuildStateUpdate()
        {
            if (!_onScreenSelectionData.isFired) return;

            Vector3 targetPos = Vector3.zero;

            // find a point on the sphere surface where the camera pivot is on
            Camera camera = currentCamera.camera;
            float distance = camera.GetComponent<BuildModeCamera>().distance;
            Vector3 camPosition = camera.transform.position;
            Vector3 displacement = _onScreenSelectionData.ray.direction.normalized * distance;
            targetPos = camPosition + displacement;

            // find an empty spot if something blocks us
            if (_onScreenSelectionData.didHitBlock && _onScreenSelectionData.hitInfo.distance <= distance)
                targetPos = _onScreenSelectionData.hitInfo.transform.position; // TODO: avoid overlap

            // instantiate brace prefab
            var go = GameObject.Instantiate(blockConfig.GetPrefab(_currentBlockType),
                targetPos, Quaternion.identity);
            var b = go.GetComponent<SingleClickBuildBlock>();
            b.EnterBuildMode();

            AddCreatedBlock(b);
            _currentBlockType = BlockType.None;

            _onScreenSelectionData.Clear();
            ToNoneState();
        }

        private void TwoClickBuildStateUpdate()
        {
            if (_onScreenSelectionData.isFired && _onScreenSelectionData.didHitBlock)
            {
                var info = _onScreenSelectionData.hitInfo;
                var selectionTransform = info.transform;

                TheBall selectedBlock = selectionTransform.GetComponent<TheBall>();
                if (selectedBlock != null)
                {
                    if (!_twoClickBuildData.firstBlockSelected)
                    {
                        _twoClickBuildData.firstBlockSelected = true;
                        _twoClickBuildData.firstBlock = selectedBlock;
                    }
                    else
                    {
                        // instantiate brace prefab
                        var go = GameObject.Instantiate(blockConfig.GetPrefab(_currentBlockType));
                        var tcbb = go.GetComponent<TwoClickBuildBlock>();
                        // these two block will be notified using OnAttach() during during the initialization of tcbb
                        tcbb.block1 = _twoClickBuildData.firstBlock.transform;
                        tcbb.block2 = selectionTransform;
                        tcbb.EnterBuildMode();

                        AddCreatedBlock(tcbb);

                        _twoClickBuildData.Clear();
                        _currentBlockType = BlockType.None;
                        ToNoneState();
                    }
                }
            }

            _onScreenSelectionData.Clear();
        }

        private void ToBallEditState(BaseBlock block)
        {
            _state = BuildModeState.BallEdit;

            // TODO: support grouped editing
            _ballEditData.ball = (TheBall)block;

            HighlightBlocks(_ballEditData.ball);

            _ballEditData.transformHandle =
                RuntimeTransformHandle.Create(
                    block.transform,
                    currentCamera,
                    HandleType.POSITION,
                    ObjectLayer.GetGizmosLayerIndex()
                );
            _ballEditData.transformHandle.space = HandleSpace.WORLD;

            usePositionTransformHandleEventChannel.OnEventRaised += _ballEditData.SetTransformHandleTypeAsPosition;
            useRotationTransformHandleEventChannel.OnEventRaised += _ballEditData.SetTransformHandleTypeAsRotation;
        }

        private void BallEditStateUpdate()
        {
            // switch to ball connection editor if a connected beam is selected
            if (_onScreenSelectionData.isFired && _onScreenSelectionData.didHitBlock)
            {
                var t = _onScreenSelectionData.hitInfo.transform;
                if (t.gameObject.layer == ObjectLayer.GetBlockAttachmentLayerIndex())
                {
                    ToBallConnectionEditState(
                        _ballEditData.ball,
                        _ballEditData.ball.FindConnectionIndexFromOther(t.GetComponent<Beam>())
                    );
                }
            }

            _onScreenSelectionData.Clear();
        }

        private void OnExitBallEditState()
        {
            ResetHighlight();

            usePositionTransformHandleEventChannel.OnEventRaised -= _ballEditData.SetTransformHandleTypeAsPosition;
            useRotationTransformHandleEventChannel.OnEventRaised -= _ballEditData.SetTransformHandleTypeAsRotation;

            GameObject.Destroy(_ballEditData.transformHandle.gameObject);
            _ballEditData.Clear();
        }

        private void ToBallConnectionEditState(TheBall ball, int connectionIndex)
        {
            _state = BuildModeState.BallConnectionEdit;
            _ballConnectionEditData.ball = ball;
            _ballConnectionEditData.connectionIndex = connectionIndex;
        }

        private void OnEnterBallConnectionEditState()
        {
            HighlightBlocks(_ballConnectionEditData.ball,
                _ballConnectionEditData.ball.GetConnectedBeam(_ballConnectionEditData.connectionIndex));
        }

        private void BallConnectionEditStateUpdate()
        {
            var ball = _ballConnectionEditData.ball;
            var connectionIndex = _ballConnectionEditData.connectionIndex;
            var conn = ball.GetConnectionAtIndex(connectionIndex);

            _uiState.blockConnectionEditorUIData.connectionType = conn.GetConnectionType();

            // allow user to rotate the axis of hinge joint
            if (_ballConnectionEditData.isRotatingHingeConnection)
            {
                HingeBallBeamConnection hingeConn = (HingeBallBeamConnection)conn;

                var ray = currentCamera.camera.ScreenPointToRay(inputManager.GetPointerScreenPosition());
                if (Physics.Raycast(
                        ray: ray, hitInfo: out RaycastHit info, maxDistance: Mathf.Infinity,
                        layerMask: 1 << ball.gameObject.layer
                    ))
                {
                    if (info.transform.gameObject == ball.gameObject)
                    {
                        hingeConn.SetAxis(info.point - ball.transform.position, Space.World);
                    }
                }

                // stop rotating on click
                if (_onScreenSelectionData.isFired)
                {
                    _ballConnectionEditData.isRotatingHingeConnection = false;
                    _onScreenSelectionData.Clear();
                }
            }
        }

        private void OnExitBallConnectionEditState()
        {
            _ballConnectionEditData.Clear();
            ResetHighlight();
        }

        private void AddCreatedBlock(BaseBlock block)
        {
            allBlocks.blocks.Add(block);
        }

        /// <summary>
        /// 1. Highlight current selected block with an outline
        /// 2. Dim all blocks other than currently selected ones by apply a different material
        /// </summary>
        private void HighlightBlocks(params BaseBlock[] blocks)
        {
            foreach (var b in blocks)
                b.gameObject.layer = ObjectLayer.GetOutlinedBlockLayerIndex();

            // dim other blocks
            foreach (var block in allBlocks.blocks)
            {
                if (!blocks.Contains(block))
                    block.GetComponent<MeshRenderer>().material = blockConfig.GetDimmedMaterial(block.GetBlockType());
            }
        }

        private void ResetHighlight()
        {
            foreach (var block in allBlocks.blocks)
            {
                if (block.gameObject.layer == ObjectLayer.GetOutlinedBlockLayerIndex())
                {
                    if (block.IsBlockAttachment())
                        block.gameObject.layer = ObjectLayer.GetBlockAttachmentLayerIndex();
                    else
                        block.gameObject.layer = ObjectLayer.GetBuildModeBlockLayerIndex();
                }

                block.GetComponent<MeshRenderer>().material =
                    blockConfig.GetBuildModeMaterial(block.GetBlockType());
            }
        }

        #region Input Handling

        private void OnEsc()
        {
            _escPressed = true;
        }

        /// <summary>
        /// User clicked left mouse button
        /// </summary>
        private void OnScreenSelection()
        {
            // check if UI was clicked
            Vector2 pointer = inputManager.GetPointerScreenPosition();
            if (IsMouseOverAnyUI())
                return;

            _onScreenSelectionData.isFired = true;

            // raycast
            var ray = currentCamera.camera.ScreenPointToRay(pointer);
            _onScreenSelectionData.ray = ray;
            if (Physics.Raycast(
                    ray: ray, hitInfo: out RaycastHit info, maxDistance: Mathf.Infinity,
                    layerMask: raycastMask
                ))
            {
                _onScreenSelectionData.didHitBlock = true;
                _onScreenSelectionData.hitInfo = info;
            }
        }

        /// <summary>
        /// User double clicked left mouse button to move camera to a cursor position
        /// </summary>
        private void OnDoubleFire()
        {
            if (_currentBlockType != BlockType.None) return;

            Vector2 pointer = inputManager.GetPointerScreenPosition();
            Ray ray = currentCamera.camera.ScreenPointToRay(pointer);
            if (Physics.Raycast(ray, out RaycastHit info))
            {
                moveToEventChannel.RaiseEvent(info.point);
            }
        }

        private bool IsMouseOverAnyUI() => _uiState.isMouseOverUIElements;

        #endregion

        #region Event Handling

        private void OnUIBlockTypeSelected(BlockType blockType)
        {
            if (_state != BuildModeState.None)
                return;

            _currentBlockType = blockType;
            ToNClickBuildState();
        }

        private void OnUIBlockConnectionTypeSelected(BlockConnectionType type)
        {
            var ball = _ballConnectionEditData.ball;
            var index = _ballConnectionEditData.connectionIndex;
            var beam = ball.GetConnectionAtIndex(index).Beam;
            var plug = beam.GetPlugForAttachedBlock(ball);

            BallBeamConnection conn = null;
            switch (type)
            {
                case BlockConnectionType.Fixed:
                    conn = new FixBallBeamConnection(ball, beam, plug);
                    break;
                case BlockConnectionType.Hinge:
                    conn = new HingeBallBeamConnection(
                        ball, beam, plug, _blockConnectionConfig.GetPrefab(BlockConnectionType.Hinge)
                    );
                    break;
                case BlockConnectionType.Free:
                    throw new NotImplementedException();
            }

            Assert.IsNotNull(conn);
            ball.SetConnectionAtIndex(index, conn);
        }

        private void OnUIRotateHingeConnection()
        {
            _ballConnectionEditData.isRotatingHingeConnection = true;
        }

        private void OnGameModeChange(GameMode mode)
        {
            // enable/disable this game object
            if (mode == GameMode.BuildMode)
            {
                ToNoneState();
                gameObject.SetActive(true);
                foreach (var block in allBlocks.blocks)
                    block.EnterBuildMode();
            }
            else
            {
                ToNoneState();
                _currentBlockType = BlockType.None;
                Update(); // one last chance to clean up
                gameObject.SetActive(false);
                foreach (var block in allBlocks.blocks)
                    block.EnterPlayMode();
            }
        }

        #endregion
    }
}