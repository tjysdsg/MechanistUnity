using UnityEngine;

namespace Core
{
    [CreateAssetMenu(menuName = "Game/UIStateSO")]
    public class UIStateSO : ScriptableObject
    {
        public bool isMouseOverUIElements = false;
    }
}