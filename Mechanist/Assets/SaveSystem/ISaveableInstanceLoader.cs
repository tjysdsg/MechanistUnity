using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    public interface ISaveableInstanceLoader
    {
        /// <summary>
        /// A list of <see cref="ISaveable.GetTypeName"/> whose game save loading
        /// is handled by this instance loader.
        /// </summary>
        public IEnumerable<string> GetHandledTypes();

        public bool Load(SaveData data);

        /// <summary>
        /// Clear cache, required to be called if loading a new save file.
        /// </summary>
        public void Clear();

        /// <summary>
        /// Find a loaded game object using its save data ID. Will load and spawn a new one if not found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GameObject GetObjWithId(int id);
    }
}