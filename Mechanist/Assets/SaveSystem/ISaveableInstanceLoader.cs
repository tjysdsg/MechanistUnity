using System.Collections.Generic;

namespace SaveSystem
{
    public interface ISaveableInstanceLoader
    {
        /// <summary>
        /// A list of <see cref="ISaveable.GetId()"/> that whose game save loading
        /// is handled by this instance loader.
        /// </summary>
        public IEnumerable<string> GetHandledIds();

        public bool Load(SaveData data);
    }
}