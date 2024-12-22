using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using Cysharp.Threading.Tasks;
using Game.Managers;
using DG.Tweening;
using System;

namespace Game
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private Transform tryAgainTxt;
        [SerializeField] private Transform winTxt;
        [SerializeField] private Transform NextLevelTxt;
        [SerializeField] private Transform IncreaseSpeed;

        private void Start()
        {
            GameManager.Instance.OnChangeState += OnChangeState;
        }

        private void OnDestroy()
        {
            if(GameManager.Instance != null)
            {
                GameManager.Instance.OnChangeState -= OnChangeState;
            }
        }

        private void OnChangeState(GameState state)
        {
            if(state == GameState.Prepare)
            {
                ResetUI();
            }
        }

        private void ResetUI()
        {
            tryAgainTxt.localScale = Vector3.zero;
            winTxt.localScale = Vector3.zero;
            NextLevelTxt.localScale = Vector3.zero;
            IncreaseSpeed.localScale = Vector3.zero;

            tryAgainTxt.gameObject.SetActive(false);
            winTxt.gameObject.SetActive(false);
            NextLevelTxt.gameObject.SetActive(false);
            IncreaseSpeed.gameObject.SetActive(false);
        }

        public async UniTask ShowTryAgain(Action callBack = null)
        {
            tryAgainTxt.gameObject.SetActive(true);
            await tryAgainTxt.DOScale(1, .74f);
            await UniTask.Delay(500);
            await HideTryAgain();
            callBack?.Invoke();
        }

        public async UniTask HideTryAgain()
        {
            await tryAgainTxt.DOScale(0, .74f);
            tryAgainTxt.gameObject.SetActive(false);
        }

        public async UniTask ShowWin(Action callBack = null)
        {
            winTxt.gameObject.SetActive(true);
            await winTxt.DOScale(1, .74f);
            await UniTask.Delay(500);
            await HideWin();
            callBack?.Invoke();
        }

        public async UniTask HideWin()
        {
            await winTxt.DOScale(0, .74f);
            winTxt.gameObject.SetActive(false);
        }

        public async UniTask ShowNextLevel(Action callBack = null)
        {
            NextLevelTxt.gameObject.SetActive(true);
            await NextLevelTxt.DOScale(1, .74f);
            await UniTask.Delay(500);
            await HideNextLevel();
            callBack?.Invoke();
        }

        public async UniTask HideNextLevel()
        {
            await NextLevelTxt.DOScale(0, .74f);
            NextLevelTxt.gameObject.SetActive(false);
        }

        public async UniTask ShowTapIncreaseSpeed(Action callBack = null)
        {
            IncreaseSpeed.gameObject.SetActive(true);
            await IncreaseSpeed.DOScale(1, .74f);
            await UniTask.Delay(500);
            await HideTapIncreaseSpeed();
            callBack?.Invoke();
        }

        public async UniTask HideTapIncreaseSpeed()
        {
            await IncreaseSpeed.DOScale(0, .74f);
            IncreaseSpeed.gameObject.SetActive(false);
        }
    }
}