using System;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class ScaleFromTo : MonoBehaviour
    {
        public Transform From;
        public Transform To;

        public float Delay = 0.1f;
        public float Duration = 0.3f;
        public Ease ease;


        private void Start()
        {
            transform.localScale = From.localScale;
            Debug.Log($"Moving {name}");
            transform.DOScale(To.localScale, Duration).SetDelay(Delay).SetEase(ease).Capture();
        }
    }
}