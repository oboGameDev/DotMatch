using System;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class SideMovePanel : MonoBehaviour
    {
        public CanvasGroup target;
        public Transform from;
        public Transform to;

        private bool shown;

        private void Start()
        {
            target.blocksRaycasts = shown;
            target.alpha = 0;
            target.transform.position = from.position;
        }

        public void Toggle()
        {
            shown = !shown;
            target.transform.DOMove(shown ? to.position : from.position, 0.2f);
            target.DOFade(shown ? 1 : 0, 0.2f).OnComplete(() =>
            {
                target.blocksRaycasts = shown;
            });
        }
    }
}