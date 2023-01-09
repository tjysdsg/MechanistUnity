using UnityEngine;

namespace Core
{
    [CreateAssetMenu(menuName = "Game/UIStateSO")]
    public class UIStateSO : ScriptableObject
    {
        public bool isMouseOverUIElements = false;
        public bool isEditingBall = false;
        public BlockType currentBlockType = BlockType.None;
        public string currentBuildState = "None";

        public readonly BlockConnectionEditorUIData blockConnectionEditorUIData = new BlockConnectionEditorUIData();
    }

    public class BlockConnectionEditorUIData
    {
        public BlockConnectionType connectionType = BlockConnectionType.Fixed;
        public bool isEditingBallConnection = false;
        public bool isEditingHingeConnection = false;
        public bool isRotatingHingeConnection = false;
    }
}