namespace SaveSystem
{
    public interface ISaveable
    {
        public int GetSaveDataId();

        public SaveData OnSave();

        /// <summary>
        /// Load save data.
        /// </summary>
        /// <param name="data">SaveData that belongs to a single game object</param>
        /// <param name="loader">Used to get the reference of another loaded game object if needed</param>
        public void OnLoad(SaveData data, ISaveableInstanceLoader loader);
    }
}