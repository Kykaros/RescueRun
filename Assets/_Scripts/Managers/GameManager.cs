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
        TitleScreen,
        Prepare,
        SetupMap,
        Intro,
        Phase_1,
        Phase_2,
        TryAgain,
        NextLevel
    }

    public class GameManager : Singleton<GameManager>
    {
        private GameState _state = GameState.None;
        public GameState State => this._state;
        public event Action<GameState> OnBeforeChangeState;
        public event Action<GameState> OnAfterChangeState;

        private void OnDestroy()
        {
            OnBeforeChangeState = null;
            OnAfterChangeState = null;
        }

        private void Start() => ChangeState(GameState.TitleScreen);

        public void ChangeState(GameState newState)
        {
            if (_state == newState)
                return;

            OnBeforeChangeState?.Invoke(_state);
            _state = newState;

            switch (newState)
            {
                case GameState.Prepare:
                    HandlePrepareGame();
                    break;
                case GameState.TitleScreen:
                    HandleTitleScreen();
                    break;
                case GameState.Intro:
                    HandleIntro();
                    break;
                case GameState.Phase_1:
                    HandlePhase1();
                    break;
                case GameState.Phase_2:
                    HandlePhase2();
                    break;
                case GameState.TryAgain:
                    TryAgain();
                    break;
                case GameState.NextLevel:
                    NextLevel();
                    break;
                default:
                    break;
            }

            OnAfterChangeState?.Invoke(_state);
        }

        private void HandlePrepareGame()
        {
            //setup level
            LevelManager.Instance.SetupLevel();

            Debug.Log("HandlePrepareGame");
            ChangeState(GameState.Intro);
        }

        private async void HandleTitleScreen()
        {
            DimScreen.Instance.ForceShow();
            TitleScreen.Instance.ForceShow();

            await DimScreen.Instance.Hide();
            await UniTask.Delay(1000);
            await DimScreen.Instance.Show();
            TitleScreen.Instance.ForceHide();

            ChangeState(GameState.Prepare);
        }

        private async void HandleIntro()
        {
            await DimScreen.Instance.Hide();
            Debug.Log("HandleIntro");
        }

        private async void HandlePhase1()
        {
            Debug.Log("HandlePhase1");
        }

        private async void HandlePhase2()
        {
            Debug.Log("HandlePhase2");
        }

        private async void TryAgain()
        {
            Debug.Log("TryAgain");
        }

        private async void NextLevel()
        {
            Debug.Log("NextLevel");
        }

        private async void HandleSetupMap()
        {
            Debug.Log("HandleSetupMap");
        }
    }
}
