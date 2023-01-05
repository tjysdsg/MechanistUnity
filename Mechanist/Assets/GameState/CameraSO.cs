using UnityEngine;

namespace GameState
{
    [CreateAssetMenu(menuName = "Game/CameraSO")]
    public class CameraSO : ScriptableObject
    {
        public Camera camera;
    }
}