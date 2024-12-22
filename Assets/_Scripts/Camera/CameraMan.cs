using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using Cinemachine;
using Game.Managers;
using Cysharp.Threading.Tasks;
using System;

namespace Game
{
    public class CameraMan : Singleton<CameraMan>
    {
        [SerializeField] private CinemachineVirtualCamera followCam;
        [SerializeField] private CinemachineVirtualCamera introCam;

        private CinemachineBrain cinemachineBrain;

        private void OnDestroy()
        {
            if(GameManager.Instance != null)
            {
                GameManager.Instance.OnChangeState -= OnChangeState;
            }
        }

        private void Start()
        {
            ResetPriorityCam();

            cinemachineBrain = GetComponent<CinemachineBrain>();

            GameManager.Instance.OnChangeState += OnChangeState;
        }

        private void SetupFollowCam()
        {
            followCam.Follow = GameObject.FindAnyObjectByType<PlayerController>().transform;
        }

        private void ResetPriorityCam()
        {
            followCam.Priority = 0;
            introCam.Priority = 1;
        }

        private async UniTask PlayIntroGame()
        {
            await UniTask.Delay(2000, cancellationToken: default);

            introCam.Priority = 0;
            followCam.Priority = 1;

            await UniTask.Delay(1500, cancellationToken: default);

            GameManager.Instance.ChangeState(GameState.IncreaseSpeed);
        }

        private void OnChangeState(GameState state)
        {
            if(state == GameState.Intro)
            {
                PlayIntroGame().Forget();
            }

            if (state == GameState.Intro)
            {
                SetupFollowCam();
            }

            if(state == GameState.Prepare)
            {
                ResetPriorityCam();
            }
        }
    }
}