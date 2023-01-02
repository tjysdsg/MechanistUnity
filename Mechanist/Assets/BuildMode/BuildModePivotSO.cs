using UnityEngine;

namespace BuildMode
{
    [CreateAssetMenu(menuName = "Game/BuildModePivotSO")]
    public class BuildModePivotSO : ScriptableObject
    {
        public Vector3 pivot;
    }
}