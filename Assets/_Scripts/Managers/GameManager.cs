using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using System;
using Cysharp.Threading.Tasks;

namespace Game.Managers
{
    public enum GameState
    {
        None,
        Prepare,
        TitleScreen,
        Lobby,
        Play,
        End
    }

    public class GameManager : Singleton<GameManager>
    {
        private GameState _state = GameState.None;
        public GameState State => this._state;
        public event Action OnBeforeChangeState;
        public event Action OnAfterChangeState;

        private void Start() => ChangeState(GameState.Prepare);

        public void ChangeState(GameState newState)
        {
            if (_state == newState)
                return;

            OnBeforeChangeState?.Invoke();

            switch (newState)
            {
                case GameState.Prepare:
                    HandlePrepareGame();
                    break;
                case GameState.TitleScreen:
                    HandleTitleScreen();
                    break;
                case GameState.Lobby:
                    break;
                case GameState.Play:
                    break;
                case GameState.End:
                    break;
            }

            OnAfterChangeState?.Invoke();
        }

        private void HandlePrepareGame()
        {
            DimScreen.Instance.ForceShow();
            TitleScreen.Instance.ForceShow();

            ChangeState(GameState.TitleScreen);
        }

        private async void HandleTitleScreen()
        {
            await DimScreen.Instance.Hide();
            await UniTask.Delay(1000);
            await DimScreen.Instance.Show();
            TitleScreen.Instance.ForceHide();

            ChangeState(GameState.Lobby);
        }
    }
}
