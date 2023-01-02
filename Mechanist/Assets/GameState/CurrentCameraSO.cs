using UnityEngine;

namespace GameState
{
    [CreateAssetMenu(menuName = "Game/CurrentCameraSO")]
    public class CurrentCameraSO : ScriptableObject
    {
        public Camera camera;
    }
}