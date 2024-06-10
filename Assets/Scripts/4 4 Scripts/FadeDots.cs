using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace _4_4_Scripts
{
    public class FadeDots : MonoBehaviour
    {
        public CanvasGroup dots;
        public RectTransform target;

        private float dotAlpha;
        public AnimationCurve Curve;

        private void Start()
        {
            dotAlpha = dots.alpha;
            Curve = AnimationCurve.Linear(0, 0, 0.25f, dotAlpha);
        }

        private void Update()
        {
            dots.alpha = Curve.Evaluate(target.localScale.x);
        }
    }
}