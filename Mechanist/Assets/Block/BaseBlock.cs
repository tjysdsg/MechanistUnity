using UnityEngine;

namespace Block
{
    [RequireComponent(typeof(Rigidbody))]
    public class BaseBlock : MonoBehaviour
    {
        protected GameObject _go;

        protected virtual void Start()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize the block.
        ///
        /// <remarks>
        /// You should always add <code>base.Initialize()</code> to the beginning if overridden.
        /// 
        /// This function is called in <see cref="BaseBlock.Start()"/> function.
        /// It's also intended to be called multiple times, so make sure to check for repeated initialization.
        /// </remarks>
        /// 
        /// </summary>
        public virtual void Initialize()
        {
            if (_go == null)
                _go = gameObject;
        }
    }
}