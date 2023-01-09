using UnityEngine;

namespace Core
{
    public static class ObjectLayer
    {
        public static int GetGizmosLayerIndex() => LayerMask.NameToLayer("Gizmos");
        public static int GetPhysicalBlockLayerIndex() => LayerMask.NameToLayer("PhysicalBlock");
        public static int GetBuildModeBlockLayerIndex() => LayerMask.NameToLayer("BuildModeBlock");
        public static int GetBuildModeBlockLayerMask() => LayerMask.GetMask("BuildModeBlock");
        public static int GetBlockAttachmentLayerIndex() => LayerMask.NameToLayer("BlockAttachment");
        public static int GetOutlinedBlockLayerIndex() => LayerMask.NameToLayer("OutlinedBlock");
        public static int GetUILayerMask() => LayerMask.GetMask("UI");
    }
}