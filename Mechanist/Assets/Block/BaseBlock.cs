using Core;
using UnityEngine;
using UnityEngine.Assertions;

namespace Block
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BaseBlock : MonoBehaviour
    {
        [SerializeField] private BlockListSO allBlocks;

        protected GameObject _go;
        protected Rigidbody _rigidbody;
        private Vector3? _origPos = null;
        private Quaternion? _origRotation = null;

        /// <summary>
        /// Must be before GameStateManager.start()
        /// </summary>
        protected virtual void OnEnable()
        {
            Initialize();
        }

        protected abstract void OnEnterPlayMode();
        protected abstract void OnEnterBuildMode();

        public abstract BlockType GetBlockType();
        public abstract bool HasInterBlockCollision();

        /// <summary>
        /// Enter game play mode, should create relevant components such as joints
        /// </summary>
        public void EnterPlayMode()
        {
            _origPos = transform.position;
            _origRotation = transform.rotation;

            gameObject.layer = ObjectLayer.GetPhysicalBlockLayerIndex();

            _rigidbody.isKinematic = false;
            OnEnterPlayMode();
        }

        /// <summary>
        /// Enter build mode, should disable physics components
        /// </summary>
        public void EnterBuildMode()
        {
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
            gameObject.layer = ObjectLayer.GetBuildModeBlockLayerIndex();

            _rigidbody.isKinematic = true;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            OnEnterBuildMode();
        }

        /// <summary>
        /// Initialize the block.
        /// </summary>
        ///
        /// <remarks>
        /// You should always add <code>base.Initialize()</code> to the beginning if overridden.
        /// 
        /// This function is called in <see cref="BaseBlock.Start()"/> function.
        /// It's also intended to be called multiple times, so make sure to check for repeated initialization.
        /// </remarks>
        public virtual void Initialize()
        {
            if (_go == null)
                _go = gameObject;
            if (_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody>();
            Assert.IsNotNull(_rigidbody);

            allBlocks.blocks.Add(this);
        }
    }

    public abstract class SingleClickBuildBlock : BaseBlock
    {
    }

    public abstract class TwoClickBuildBlock : BaseBlock
    {
        [SerializeField] public Transform block1;
        [SerializeField] public Transform block2;
    }
}