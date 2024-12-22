using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Game.Managers;

namespace Game
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private Animator animator;
        //public Transform followCam;

        [SerializeField] private float _moveSpeed;

        private enum state
        {
            None,
            Idle,
            Run
        }

        private FixedJoystick joystick;
        private CinemachineVirtualCamera virtualCamera;
        private state currentState = state.None;
        private const float SPEED_RATE = 0.1667f;

        private void Awake()
        {
            this.animator.SetBool("Run", false);
            this.animator.speed = this._moveSpeed * SPEED_RATE;

            this.joystick = GameObject.FindObjectOfType<FixedJoystick>();
        }

        private void FixedUpdate()
        {
            rigidbody.velocity = new Vector3(joystick.Horizontal * _moveSpeed, rigidbody.velocity.y, joystick.Vertical * _moveSpeed);

            currentState = state.Idle;
            if(joystick.Horizontal != 0 || joystick.Vertical != 0)
            {
                transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
                currentState = state.Run;
            }

            changeState(currentState);
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
                GameManager.Instance.ChangeState(GameState.NextLevel);
            }
        }
    }
}