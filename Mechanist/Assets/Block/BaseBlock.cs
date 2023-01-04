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
        private Vector3 _origPos;
        private Quaternion _origRotation;

        protected virtual void Start()
        {
            Initialize();
        }

        protected abstract void OnEnterPlayMode();
        protected abstract void OnEnterBuildMode();

        public abstract BlockType GetBlockType();

        /// <summary>
        /// Enter game play mode, should create relevant components such as joints
        /// </summary>
        public void EnterPlayMode()
        {
            _origPos = transform.position;
            _origRotation = transform.rotation;

            _rigidbody.isKinematic = false;
            OnEnterPlayMode();
        }

        /// <summary>
        /// Enter build mode, should disable physics components
        /// </summary>
        public void EnterBuildMode()
        {
            transform.SetPositionAndRotation(_origPos, _origRotation);

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

            _origPos = transform.position;
            _origRotation = transform.rotation;
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