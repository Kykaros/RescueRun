using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Game.Managers;
using System;
using Cysharp.Threading.Tasks;

namespace Game
{
    [Serializable]
    public class PlayerInfo 
    {
        public float Stamina;
        public float Speed;
        public float InCome;
    }

    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private Animator animator;
        //public Transform followCam;

        [Header("Player Info")]
        [SerializeField] private PlayerInfo playerInfo;

        private bool canMove = false;

        private enum state
        {
            None,
            Idle,
            Run
        }

        private FixedJoystick joystick;
        private Canvas joystickCanvas;
        private CinemachineVirtualCamera virtualCamera;
        private state currentState = state.None;
        private const float SPEED_RATE = 0.1f;
        private float moveSpeed = 0;

        private void Awake()
        {
            this.animator.SetBool("Run", false);
            this.animator.speed = this.playerInfo.Speed * SPEED_RATE;

            this.joystick = GameObject.FindObjectOfType<FixedJoystick>();
            this.joystickCanvas = this.joystick.transform.parent.GetComponent<Canvas>();

            this.moveSpeed = this.playerInfo.Speed;
        }

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

        private void FixedUpdate()
        {
            var velocity = new Vector3(joystick.Horizontal * this.moveSpeed, rigidbody.velocity.y, joystick.Vertical * this.moveSpeed);
            rigidbody.velocity = canMove ? velocity : Vector3.zero;

            currentState = state.Idle;
            if(joystick.Horizontal != 0 || joystick.Vertical != 0)
            {
                transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
                currentState = state.Run;
            }

            changeState(currentState);
        }

        private void OnChangeState(GameState state)
        {
            if(state == GameState.Phase_1)
            {
                this.canMove = true;
                this.joystickCanvas.enabled = true;
            }

            if(state == GameState.TryAgain)
            {
                StopMove();
            }

            if(state == GameState.Prepare)
            {
                this.moveSpeed = this.playerInfo.Speed;
            }
        }

        private void StopMove()
        {
            this.canMove = false;
            this.joystickCanvas.enabled = false;
            this.moveSpeed = 0;
        }

        private void changeState(state state)
        {
            switch (state)
            {
                case state.Idle:
                    this.animator.SetBool("Run", false);
                    break;
                case state.Run:
                    this.animator.SetBool("Run", true);
                    break;
                default:
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("EndPhase1"))
            {
                GameManager.Instance.ChangeState(GameState.Phase_2);
            }

            if (other.gameObject.tag.Equals("EndPhase2"))
            {
                StopMove();
                UIManager.Instance.ShowNextLevel(() =>
                {
                    GameManager.Instance.ChangeState(GameState.NextLevel);
                }).Forget();
                
            }
        }
    }
}