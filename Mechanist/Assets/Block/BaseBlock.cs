using UnityEngine;

namespace Block
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BaseBlock : MonoBehaviour
    {
        protected GameObject _go;

        protected virtual void Start()
        {
            Initialize();
        }

        /// <summary>
        /// Enter game play mode, should create relevant components such as joints
        /// </summary>
        public abstract void EnterPlayMode();

        /// <summary>
        /// Enter build mode, should disable physics components
        /// </summary>
        public abstract void EnterBuildMode();

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
        }

        public virtual void OnBuildModeSelected()
        {
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