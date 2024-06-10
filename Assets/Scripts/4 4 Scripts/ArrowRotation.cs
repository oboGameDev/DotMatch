using System;
using Assets.Scripts;
using DG.Tweening;
using UnityEngine;

namespace _4_4_Scripts
{
    public class ArrowRotation : MonoBehaviour
    {
        public static ArrowRotation Instance;
        public Transform ArrowsParent;
        [SerializeField] private Color onHandleColor;

        public Color onRotateEndColor;
        public Renderer[] myRenderer;
        public bool _colorChanged = true;

        private float _nextRotation;

        public Vector3 targetRotation;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            targetRotation = ArrowsParent.localRotation.eulerAngles;
            _nextRotation = targetRotation.y;
            DragController.Instance.OnClick += OnClick;
            myRenderer = GetComponentsInChildren<Renderer>();
        }

        private void OnClick()
        {
            foreach (Renderer renderer1 in myRenderer)
            {
                renderer1.material.DOColor(onHandleColor, 0.2f);
            }

            _nextRotation += 90;
            ArrowsParent.DOLocalRotate((new Vector3(0, _nextRotation, 0)), 0.36f)
                .OnComplete((() =>
                {
                    foreach (Renderer renderer1 in myRenderer)
                    {
                        renderer1.material.DOColor(onRotateEndColor, 0.2f);
                    }
                }));
        }
    }
}