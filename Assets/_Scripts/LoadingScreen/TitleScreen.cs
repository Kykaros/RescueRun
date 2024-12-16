using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Singleton;

namespace Game
{
    public class TitleScreen : Singleton<TitleScreen>
    {
        [SerializeField] private Canvas canvas;

        public void ForceShow()
        {
            canvas.enabled = true;
        }

        public void ForceHide()
        {
            canvas.enabled = true;
        }
    }
}
