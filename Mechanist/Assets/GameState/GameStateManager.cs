using System;
using UnityEngine;

namespace GameState
{
    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] private GameModeSO gameMode;
        [SerializeField] private InputManager inputManager;

        private void Start()
        {
            // initial game mode entrance
            gameMode.ChangeMode(gameMode.CurrentMode);
            Debug.Log($"Initial game mode: {gameMode.CurrentMode}");

            // enable relevant input mapping
            switch (gameMode.CurrentMode)
            {
                case GameMode.BuildMode:
                    inputManager.EnableBuildingModeInput();
                    break;
                case GameMode.PlayMode:
                    inputManager.EnablePlayModeInput();
                    break;
                case GameMode.Loading:
                case GameMode.MainMenu:
                    throw new NotImplementedException();
            }

            // register handlers for input that changes the game mode
            inputManager.PlayModeToBuildModeEvent += OnEnterBuildMode;
            inputManager.BuildModeToPlayModeEvent += OnEnterPlayMode;
        }

        public void OnEnterPlayMode()
        {
            gameMode.ChangeMode(GameMode.PlayMode);
        }

        public void OnEnterBuildMode()
        {
            gameMode.ChangeMode(GameMode.BuildMode);
        }
    }
}