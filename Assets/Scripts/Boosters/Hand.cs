using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Utils;
using Assets.Scripts._4_4_Scripts;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Assets.Scripts._4_4_Scripts
{
    public class Hand : Booster
    {
        public static Hand Instance;

        private MouseEvents _mouseEvents;
        private bool _dragging;
        public GameObject Overlay;
        private Dot _dot;
        private Dot _selectedDot;
        private float _lerpFactor = 10f;
        private Vector3 _nextPoint; //this point is needed for saving the position
        private bool handBool;
        private Vector3 defaultScale;
        public RectTransform Canvas; //height ucin

        public float MouseOffset = 0.05f;
        private Vector3 _mouseOffset;
        public Action OnHandPressed;

        private void Awake()
        {
            Instance = this;
            _mouseOffset = Canvas.sizeDelta.y * MouseOffset * Vector3.up;
        }

        protected override void OnStart()
        {
            _mouseEvents = new MouseEvents();
            _mouseEvents.Init();
        }

        public override void Select()
        {
            if (BoosterManager.Instance.SelectedBooster == this)
            {
                BoosterManager.Instance.SelectedBooster = null;
                return;
            }


            if (!EnoughCount())
            {
                if (!MoneySystem.Instance.CanAfford(25))
                {
                    return;
                }
                else
                {
                    AddCount(25);
                }
            }

            OnHandPressed?.Invoke();
            BoosterManager.Instance.SelectedBooster = this;
            // MoneySystem.Instance.SpendMoney(50);//after activation spending 50

            //  }
        }

        private void Update()
        {
            if (!handBool)
            {
                return;
            }

            
            if (_mouseEvents.IsMousePressed)
            {
                DragStart();
            }
            else if (_dragging) //initiate edilya dragging mouse event
            {
                if (_mouseEvents.IsMouseMoving)
                {
                    OnDrag();
                }

                if (_mouseEvents.IsMouseReleased)
                {
                    DragEnd();
                }
            }
        }

        public void Activate(Dot dot)
        {
            if (dot.isGem)
            {
                return;
            }
            Debug.Log($"clickedPlace {dot.name}", dot);
            if (dot.Place != null)
            {
                handBool = true;
            }
        }

        private void DragStart()
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(_mouseEvents.GetMousePosition), out RaycastHit hit))
            {
                if (hit.transform.TryGetComponent(out Dot dot))
                {
                    _dragging = true;
                    _mouseEvents.DragDetector.StartRecording();
                    defaultScale = dot.transform.localScale;
                    dot.Select();

                    Overlay.SetActive(true);
                    _dot = dot;
                }
            }
        }

        private void OnDrag()
        {
            if (_mouseEvents.DragDetector.IsDragged)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(_mouseEvents.GetMousePosition + _mouseOffset), out RaycastHit hit,
                        300,
                        LayerMask.GetMask("raycastLayer")))
                {
                    _nextPoint = hit.point;

                    _dot.transform.position = Vector3.Lerp(_dot.transform.position, _nextPoint,
                        _lerpFactor * Time.deltaTime);
                    //float scaleFactor = 1.3f;
                    // Circle.transform.DOScale(1.3f, 0.009f);
                    //_dot.transform.localScale = Vector3.one * scaleFactor;
                    _dot.transform.DOScale(defaultScale * 1.2f, 0.1f).Capture();
                }
            }
        }

        private void DragEnd()
        {
            Overlay.SetActive(false);
            _dragging = false;

            _dot.Unselect();
            handBool = false;

            if (_mouseEvents.DragDetector.IsDragged)
            {
                _selectedDot = null;
                if (_dot.CanBePlaced())
                {
                  
                    _dot.PlaceDot();
                   
                    PlaceManager.Instance.SaveLevel();
                    BoosterManager.Instance.SelectedBooster = null;
                }
                else
                {
                    _dot.transform.localPosition = _dot.Place.PositionLocation.localPosition;
                    _dot.transform.DOScale(defaultScale, 0.2f).Capture();
                   BoosterWindow.Instance. ToggleHandTutorialHand(true);
                }
            }
        }

    }
}