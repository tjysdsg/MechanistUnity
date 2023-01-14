using Core;
using SaveSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace Block
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BaseBlock : MonoBehaviour, ISaveable
    {
        [SerializeField] private BlockListSO allBlocks;

        protected GameObject _go;
        protected Rigidbody _rigidbody;
        private Vector3? _origPos = null;
        private Quaternion? _origRotation = null;

        protected GameMode _gameMode = GameMode.MainMenu;
        private GameMode _prevGameMode = GameMode.MainMenu;

        public abstract bool IsBlockAttachment();

        protected virtual void OnEnable()
        {
            if (_go == null)
                _go = gameObject;
            if (_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody>();
            Assert.IsNotNull(_rigidbody);

            allBlocks.blocks.Add(this);

            Initialize();
        }

        protected virtual void Start()
        {
            LateInitialize();
        }

        protected virtual void Update()
        {
            if (_prevGameMode != _gameMode) // game mode has changed
            {
                _prevGameMode = _gameMode;

                if (_gameMode == GameMode.PlayMode) // enter play mode
                {
                    // remember last transform
                    _origPos = transform.position;
                    _origRotation = transform.rotation;

                    // enable physics 
                    gameObject.layer = ObjectLayer.GetPhysicalBlockLayerIndex();
                    _rigidbody.isKinematic = false;

                    OnEnterPlayMode();
                }
                else if (_gameMode == GameMode.BuildMode) // enter build mode
                {
                    // restore original transform
                    if (_origPos.HasValue && _origRotation.HasValue)
                    {
                        transform.SetPositionAndRotation(_origPos.Value, _origRotation.Value);
                    }
                    else
                    {
                        _origPos = transform.position;
                        _origRotation = transform.rotation;
                    }

                    transform.SetPositionAndRotation(_origPos.Value, _origRotation.Value);

                    // disable physics
                    gameObject.layer = ObjectLayer.GetBuildModeBlockLayerIndex();
                    _rigidbody.isKinematic = true;
                    _rigidbody.velocity = Vector3.zero;
                    _rigidbody.angularVelocity = Vector3.zero;

                    OnEnterBuildMode();
                }
            }
        }

        public void EnterPlayMode() => _gameMode = GameMode.PlayMode;
        public void EnterBuildMode() => _gameMode = GameMode.BuildMode;

        /// <summary>
        /// Callback when the entering the play mode.
        ///
        /// This is guaranteed to be called only in <see cref="UnityEngine.PlayerLoop.Update"/>.
        /// </summary>
        protected abstract void OnEnterPlayMode();

        /// <summary>
        /// Callback when the entering the build mode.
        ///
        /// This is guaranteed to be called only in <see cref="UnityEngine.PlayerLoop.Update"/>.
        /// </summary>
        protected abstract void OnEnterBuildMode();

        public abstract BlockType GetBlockType();
        public abstract bool HasInterBlockCollision();

        /// <summary>
        /// Initialize the block in <see cref="MonoBehaviour.OnEnable"/> event
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// Initialize the block in <see cref="MonoBehaviour.Start"/> event
        /// </summary>
        protected abstract void LateInitialize();

        #region Save and load

        public int GetSaveDataId() => gameObject.GetInstanceID();
        public abstract SaveData OnSave();
        public abstract void OnLoad(SaveData data, ISaveableInstanceLoader loader);

        #endregion
    }

    public abstract class SingleClickBuildBlock : BaseBlock
    {
    }

    public abstract class TwoClickBuildBlock : BaseBlock
    {
        [SerializeField] public Transform block1;
        [SerializeField] public Transform block2;

        /// <summary>
        /// When a block connects to a <see cref="TwoClickBuildBlock"/>,
        /// it might be physically connected to another rigidbody instead of directly to this game object.
        ///
        /// For example, a spring has two connection points ("plugs"),
        /// the actual physical connection is between a block and one of these plugs.
        ///
        /// This function returns the assigned plug for <see cref="scbb"/>.
        /// </summary>
        public abstract Rigidbody GetPlugForAttachedBlock(SingleClickBuildBlock scbb);
    }
}