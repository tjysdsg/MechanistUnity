using System;
using Core;
using UnityEngine;

namespace GameState
{
    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] private GameModeEventChannelSO gameModeEventChannel;
        [SerializeField] private InputManager inputManager;

        private void Start()
        {
            // initial game mode entrance
            gameModeEventChannel.ChangeMode(gameModeEventChannel.CurrentMode);

            // enable relevant input mapping
            switch (gameModeEventChannel.CurrentMode)
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
            inputManager.EnterBuildModeEvent += OnEnterBuildMode;
            inputManager.EnterPlayModeEvent += OnEnterPlayMode;
        }

        public void OnEnterPlayMode()
        {
            Debug.Log("GameStateManager: OnEnterPlayMode");
            gameModeEventChannel.ChangeMode(GameMode.PlayMode);
        }

        public void OnEnterBuildMode()
        {
            Debug.Log("GameStateManager: OnEnterBuildMode");
            gameModeEventChannel.ChangeMode(GameMode.BuildMode);
        }
    }
}