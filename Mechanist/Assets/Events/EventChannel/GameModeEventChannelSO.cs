using Core;
using UnityEngine;

namespace GameState
{
    [CreateAssetMenu(menuName = "Game/GameModeEventChannelSO")]
    public class GameModeEventChannelSO : BaseEventChannelSO<GameMode>
    {
        [SerializeField] private GameMode mode = GameMode.BuildMode;
        public GameMode CurrentMode => mode;

        public void ChangeMode(GameMode m)
        {
            mode = m;
            RaiseEvent(mode);
        }
    }
}