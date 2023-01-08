using UnityEngine;

namespace Core
{
    [CreateAssetMenu(menuName = "Game/UIStateSO")]
    public class UIStateSO : ScriptableObject
    {
        public bool isMouseOverUIElements = false;
        public bool isEditingBall = false;
        public bool isEditingBallConnection = false;
        public BlockType currentBlockType = BlockType.None;
        public string currentBuildState = "None";
    }
}