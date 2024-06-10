using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _4_4_Scripts
{
    public class GemBlink : MonoBehaviour
    {
        public MeshRenderer Renderer;
        private Tween _tween;

        private void OnEnable()
        {
            Blink();
        }

        private void Blink()
        {
            _tween = Renderer.material.DOFloat(1, "_Scroll", 1f).SetDelay(Random.Range(2.5f, 3.5f)).OnComplete(() =>
            {
                Renderer.material.SetFloat("_Scroll", 0);
                Blink();
            });
        }

        private void OnDisable()
        {
            if (_tween != null && _tween.IsActive())
            {
                _tween.Kill();
            }
        }
    }
}