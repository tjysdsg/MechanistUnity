using UnityEngine;

namespace GameState
{
    public enum GameMode
    {
        MainMenu,
        Loading,
        BuildMode,
        PlayMode,
    }

    [CreateAssetMenu(menuName = "Game/GameModeSO")]
    public class GameModeSO : BaseEventChannelSO<GameMode>
    {
        [SerializeField] private GameMode mode = GameMode.BuildMode;
        public GameMode CurrentMode => mode;

        public void ChangeMode(GameMode m)
        {
            mode = m;
            Debug.Log($"GameModeSO: {mode}");
            RaiseEvent(mode);
        }
    }
}