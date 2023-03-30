using UnityEngine;

namespace SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private SaveSystemSO _saveSystem;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                _saveSystem.Save(0);
            }
            else if (Input.GetKeyDown(KeyCode.F6))
            {
                _saveSystem.Load(0);
            }
        }
    }
}