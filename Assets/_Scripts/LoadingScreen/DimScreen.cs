using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Singleton;

namespace Game
{
    public class DimScreen : Singleton<DimScreen>
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Canvas canvas;

        public void ForceShow()
        {
            this.canvas.enabled = true;
            this.canvasGroup.alpha = 1.0f;
        }

        public void ForceHide()
        {
            this.canvas.enabled = false;
            this.canvasGroup.alpha = 0.0f;
        }

        public async UniTask Show()
        {
            this.canvas.enabled = true;
            await Fade(1.0f, 0.74f);
        }

        public async UniTask Hide()
        {
            await Fade(0f, .74f);
            this.canvas.enabled = false;
        }

        public async UniTask Fade(float value, float duration)
        {
            this.canvas.enabled = true;
            await this.canvasGroup.DOFade(value, duration);
        }
    }
}