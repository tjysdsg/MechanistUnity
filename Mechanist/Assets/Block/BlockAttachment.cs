using System;

namespace Block
{
    [Serializable]
    public struct BlockAttachment
    {
        /// <summary>
        /// The block to connect to
        /// </summary>
        public BaseBlock obj;

        public BlockAttachment(BaseBlock b)
        {
            obj = b;
        }
    }
}