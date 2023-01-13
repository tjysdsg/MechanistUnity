using System.Collections.Generic;

namespace SaveSystem
{
    public interface ISaveableInstanceLedger
    {
        public IEnumerable<ISaveable> GetSaveableInstances();
    }
}