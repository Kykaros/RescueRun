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
        Intro,
        IncreaseSpeed,
        Phase_1,
        Phase_2,
        TryAgain,
        NextLevel
    }

    public class GameManager : Singleton<GameManager>
    {
        private GameState _state = GameState.None;
        public GameState State => this._state;
        public event Action<GameState> OnChangeState;

        private void OnDestroy()
        {
            OnChangeState = null;
        }

        private void Start() => ChangeState(GameState.TitleScreen);

        public void ChangeState(GameState newState)
        {
            if (_state == newState)
                return;

            _state = newState;
            OnChangeState?.Invoke(_state);

            switch (newState)
            {
                case GameState.Prepare:
                    HandlePrepareGame();
                    break;
                case GameState.TitleScreen:
                    HandleTitleScreen();
                    break;
                case GameState.IncreaseSpeed:
                    HandleIncreaseSpeed();
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
            await DimScreen.Instance.Show();
            LevelManager.Instance.ResetLevel();
            ChangeState(GameState.Prepare);
            Debug.Log("TryAgain");
        }

        private async void NextLevel()
        {
            //reset state game
            await DimScreen.Instance.Show();
            LevelManager.Instance.ResetLevel();
            LevelManager.Instance.NextLevel();
            ChangeState(GameState.Prepare);
            Debug.Log("NextLevel");
        }

        private async void HandleIncreaseSpeed()
        {
            //TODO: Tap for increase speed
            Debug.Log("HandleIncreaseSpeed");
            UIManager.Instance.ShowTapIncreaseSpeed().Forget();
            await UniTask.Delay(2000);
            ChangeState(GameState.Phase_1);
        }
    }
}
