using System;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class FadeFromTo : MonoBehaviour
    {
        public float From;
        public float To;

        public CanvasGroup Group;
        public float Delay = 0.1f;
        public float Duration = 0.3f;
        public Ease ease;

        public bool disableOnFade;

        private void Start()
        {
            Group.alpha = From;
            Debug.Log($"Moving {name}");
            Group.DOFade(To, Duration).SetDelay(Delay).SetEase(ease).OnComplete(() =>
            {
                if (disableOnFade)
                {
                    gameObject.SetActive(false);
                }
            });
        }
    }
}