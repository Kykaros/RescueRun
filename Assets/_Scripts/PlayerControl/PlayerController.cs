using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private Animator animator;

        [SerializeField] private float _moveSpeed;

        private enum state
        {
            None,
            Idle,
            Run
        }

        private void Awake()
        {
            this.animator.SetBool("Run", false);
            this.animator.speed = this._moveSpeed * SPEED_RATE;

            this.joystick = GameObject.FindObjectOfType<FixedJoystick>();
        }

        private FixedJoystick joystick;
        private state currentState = state.None;
        private const float SPEED_RATE = 0.1667f;

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
    }
}