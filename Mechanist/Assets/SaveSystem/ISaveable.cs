namespace SaveSystem
{
    public interface ISaveable
    {
        /// <summary>
        /// ID is used to distinguish different types of objects.
        /// When loading,
        /// a <see cref="ISaveableInstanceLoader"/> that recognizes certain IDs will be called to load them.
        /// </summary>
        public string GetId();

        public string OnSave();
        public void OnLoad(SaveData data);
    }
}