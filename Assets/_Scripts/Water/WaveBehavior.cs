using DG.Tweening;
using UnityEngine;
using Singleton;
using Game.Managers;
using Cysharp.Threading.Tasks;

namespace Game
{
    public class WaveBehavior : MonoBehaviour
    {
        [SerializeField] private Transform destinationPhase1;
        [SerializeField] private Transform destinationPhase2;
        [SerializeField] private float velocityBegin;
        [Space]
        [SerializeField] private float velocity = 0;
        [Space]
        [Header("Start Position")]
        [SerializeField] private Vector3 startPos;

        private const float INCREASE_SPEED_RATE = 30f;

        private bool canMove = false;

        private void Start()
        {
            GameManager.Instance.OnChangeState += OnAfterChangeState;
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnChangeState -= OnAfterChangeState;
            }
        }

        private void StartMove()
        {
            canMove = true;
            velocity = velocityBegin;
        }

        private void ResetWave()
        {
            canMove = false;
            velocity = 0;
            transform.localPosition = startPos;
        }

        private void OnAfterChangeState(GameState state)
        {
            if(state == GameState.Phase_1)
            {
                StartMove();
            }

            if(state == GameState.Prepare)
            {
                ResetWave();
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                canMove = false;
                velocity = 0;
                UIManager.Instance.ShowTryAgain(() => GameManager.Instance.ChangeState(GameState.TryAgain)).Forget();
            }

            if (other.gameObject.tag.Equals("EndPhase1"))
            {
                Debug.Log("Water change Speed !");
                velocity = velocityBegin + INCREASE_SPEED_RATE;
            }

            if (other.gameObject.tag.Equals("EndPhase2"))
            {
                Debug.Log("Stop water !");
                canMove = false;
                velocity = 0;
            }
        }

        private void Update()
        {
            if(canMove)
                transform.Translate(Vector3.forward * velocity * Time.deltaTime);
        }

        public void SetVelocityBegin(float value)
        {
            velocityBegin = value;
        }
    }
}