using UnityEngine;

namespace BuildMode
{
    public enum BuildModeState
    {
        None,
        Placement,
    }

    [CreateAssetMenu(menuName = "Game/BuildModeStateSO")]
    public class BuildModeStateSO : ScriptableObject
    {
        public BuildModeState state = BuildModeState.None;
    }
}