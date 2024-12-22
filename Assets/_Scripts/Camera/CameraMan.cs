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
                GameManager.Instance.OnAfterChangeState -= OnAfterChangeState;
                GameManager.Instance.OnBeforeChangeState -= OnBeforeChangeState;
            }
        }

        private void Start()
        {
            followCam.Priority = 0;
            introCam.Priority = 1;

            cinemachineBrain = GetComponent<CinemachineBrain>();

            GameManager.Instance.OnAfterChangeState += OnAfterChangeState;
            GameManager.Instance.OnBeforeChangeState += OnBeforeChangeState;
        }

        private void SetupFollowCam()
        {
            followCam.Follow = GameObject.FindAnyObjectByType<PlayerController>().transform;
        }

        private async UniTask PlayIntroGame()
        {
            await UniTask.Delay(2000, cancellationToken: default);

            introCam.Priority = 0;
            followCam.Priority = 1;

            await UniTask.Delay(500, cancellationToken: default);

            GameManager.Instance.ChangeState(GameState.Phase_1);
        }

        private void OnAfterChangeState(GameState state)
        {
            if(state == GameState.Intro)
            {
                PlayIntroGame().Forget();
            }
        }

        private void OnBeforeChangeState(GameState state)
        {
            if (state == GameState.Prepare)
            {
                SetupFollowCam();
            }
        }
    }
}