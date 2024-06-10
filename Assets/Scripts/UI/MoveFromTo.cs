using System;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class MoveFromTo : MonoBehaviour
    {
        public Transform From;
        public Transform To;

        public float Delay = 0.1f;
        public float Duration = 0.3f;
        public Ease ease;
        

        public bool Local;

        private void Start()
        {
            SetPosition(From);

            if (Local)
            {
                transform.DOLocalMove(To.localPosition, Duration).SetDelay(Delay).SetEase(ease);
            }
            else
            {
                transform.DOMove(To.position, Duration).SetDelay(Delay).SetEase(ease);
            }
        }

        private void SetPosition(Transform to)
        {
            if (Local)
            {
                transform.localPosition = to.localPosition;
            }
            else
            {
                transform.position = to.position;
            }
        }
    }
}