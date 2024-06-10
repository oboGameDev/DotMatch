using System;
using System.Collections.Generic;
using System.Linq;
using _4_4_Scripts;
using _Scripts.Utils;
using Assets.Scripts._4_4_Scripts;
using Assets.Scripts.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using TutorialManager = Tutorial.TutorialManager;

namespace Assets.Scripts
{
    public class DragController : MonoBehaviour
    {
        public static DragController Instance;

        // public GameObject DotsBackground;

        private MouseEvents _mouseEvents;
        private Circle _circle;
        private Circle _clickedCircle;
        private Place _place;
        public GameObject Circle;

        public GameObject Overlay;
        public Window Losepanel;

        public float MouseOffset = 0f;
        public RectTransform Canvas; //height ucin

        private Vector3 _nextPoint; //this point is needed for saving the position
        public bool _canDrag = true;
        public bool _dragging;
        private float _nextRotation;
        private float _nextRotationArrow;
        private float _lerpFactor = 25f;

        private Vector3 _defaultPosition;
        private Vector3 _mouseOffset;
        private LoseChecker _loseChecker = new LoseChecker();
        public Action OnLost;
        public bool finish = true;
        public Action OnPlacement;

        public Action OnVibro;
        public Action OnClick;
        public GameObject TutorialCanvas;
        public Window WinPanel;
        public TextMeshProUGUI earnedMoneyText;
        public int earned;

        public Transform Arrows;
        public bool highlighted;

        private List<Place> calculatedPlaces = new List<Place>();

        public Action OnBallChildRotation;

        public Circle SelectedCircle
        {
            get => _circle;
        }

        public Circle GetCircle => _circle;

        public MouseEvents MouseEvents => _mouseEvents;

        public Vector3 MouseOffset2 => _mouseOffset;

        private Quaternion _ballRotation;

        private void Awake()
        {
            Instance = this;
            _mouseEvents = new MouseEvents();
            _mouseEvents.Init();
            // _mouseOffset = Vector3.zero;
            _mouseOffset = Canvas.sizeDelta.y * MouseOffset * Vector3.up;
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
            _defaultPosition = Circle.transform.position;
            DOVirtual.DelayedCall(0.4f, () => { _ballRotation = _circle.Dots[0].transform.rotation; });
        }


        void Update()
        {
            if (BoosterWindow.Instance.isActiveAndEnabled || !finish)
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

        private void DragStart()
        {
            if (MouseEvents.IsPointerOverUiObject(_mouseEvents.GetMousePosition))
            {
                _dragging = false;
                return;
            }

            if (!_canDrag)
            {
                _dragging = true;
                return;
            }

            if (Physics.Raycast(Camera.main.ScreenPointToRay(_mouseEvents.GetMousePosition), out RaycastHit hit))
            {
                if (hit.transform.TryGetComponent(out Circle circle))
                {
                    OnVibro?.Invoke();
                    _dragging = true;
                    _mouseEvents.DragDetector.StartRecording();

                    circle.Select();

                    Overlay.SetActive(true);
                    _circle = circle;
                }
                // shutayda dot laryn backgroundyna baslyp basylmadygyny bilmeli
            }
        }

        private void OnDrag()
        {
            if (!_canDrag) return;
            if (_mouseEvents.DragDetector.IsDragged)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(_mouseEvents.GetMousePosition + _mouseOffset),
                        out RaycastHit hit,
                        500,
                        LayerMask.GetMask("raycastLayer")))
                {
                    _nextPoint = hit.point;
//highlight etyan yeri
                }

                // Circle.transform.position = _nextPoint;
                Circle.transform.position = Vector3.Lerp(Circle.transform.position, _nextPoint,
                    _lerpFactor * Time.deltaTime);
                float scaleFactor = 1.3f;
                Circle.transform.localScale = Vector3.one * scaleFactor;
                _circle.PositionParent.localScale = new Vector3(1f / Circle.transform.localScale.x,
                    1f / Circle.transform.localScale.y, 1f / Circle.transform.localScale.z);

                DefaultNamespace.TutorialManager.Instance.HideTutorials();

                if (_circle.CanBePlaced())
                {
                    Debug.Log($"Found by raycast");
                    _circle.Highlight();
                    calculatedPlaces.Clear();
                }
                else if (_circle.CannotHighlight())
                {
                    calculatedPlaces.Clear();
                    foreach (Place place in PlaceManager.Instance.Places)
                    {
                        place.UnHighlight();
                    }
                }
                else
                {
                    foreach (Place place in PlaceManager.Instance.Places)
                    {
                        place.UnHighlight();
                    }

                    calculatedPlaces.Clear();
                    FindPlacesByMath();
                }
            }
        }

        private void DragEnd()
        {
            Overlay.SetActive(false);
            _dragging = false;
            DefaultNamespace.TutorialManager.Instance.ShowTutorials();
            foreach (Place place in PlaceManager.Instance.Places)
            {
                place.UnHighlight();
            }

            if (_canDrag)
            {
                _circle.Unselect();


                if (_mouseEvents.DragDetector.IsDragged)
                {
                    _clickedCircle = null;
                    if (_circle.CanBePlaced())
                    {
                        Debug.Log($"Place null: {_circle != null}");
                        _circle.Place();

                        Destroy(_circle.gameObject);
                        CircleSpawner.Instance.Spawn();
                    }
                    else if (_circle.CannotHighlight())
                    {
                        MoveToDefaultPosition();
                    }
                    else
                    {
                        if (calculatedPlaces.Count == 0)
                        {
                            MoveToDefaultPosition();
                        }
                        else
                        {
                            _circle.PlaceByList(calculatedPlaces);

                            Destroy(_circle.gameObject);
                            CircleSpawner.Instance.Spawn();
                        }
                    }
                }
                else
                {
                    Click();
                }
            }
            else
            {
                _canDrag = true;
                Click();
            }
        }

        private void FindPlacesByMath()
        {
            Dot pivot = _circle.Dots.First(d => d.Offset.X == 0 && d.Offset.Y == 0);
            bool foundPlace = false;
            List<Place> places = new List<Place>();
            Place pivotPlace = null;
            Debug.DrawRay(pivot.RayDot.transform.position, Camera.main.transform.forward, Color.red, 0.2f);
            if (Physics.Raycast(pivot.RayDot.transform.position, Camera.main.transform.forward, out RaycastHit hit,
                    10000,
                    LayerMask.GetMask("place")) && hit.transform.TryGetComponent(out Place place) && !place.HasDot())
            {
                foundPlace = true;
                pivotPlace = place;
                Debug.Log($"Pivot has dot {place.HasDot()}", pivotPlace);
                places.Add(pivotPlace);
            }

            if (!foundPlace) return;

            for (var i = 0; i < _circle.Dots.Count; i++)
            {
                var dot = _circle.Dots[i];
                if (dot.Offset.X == 0 && dot.Offset.Y == 0) continue;
                Place nextPlace = PlaceManager.Instance.GetCell(pivotPlace.Location + _circle.DotOffsets[i]);
                if (nextPlace == null || nextPlace.HasDot())
                {
                    return;
                }

                places.Add(nextPlace);
            }

            Debug.Log($"Found by math");
            foreach (var place1 in places)
            {
                place1.Highlight();
            }

            calculatedPlaces.AddRange(places);
        }

        public void MoveToDefaultPosition()
        {
            if (_circle != null)
            {
                _circle.transform.position = _defaultPosition;
                _circle.transform.localScale = Vector3.one;

                // _circle.transform.DOScale(1, 0.2f);
            }
        }

        private void Click()
        {
            if (!PlaceManager.Instance.canRotate)
            {
                return;
            }

            // FloatingText.Instance.WordAppear();// it was for testing
            if (_clickedCircle == null)
            {
                /*Losepanel.OpenWindow();*/

                /*WinPanel.OpenWindow();*/
                OnVibro?.Invoke();
                OnClick?.Invoke();
                _clickedCircle = _circle;
                _nextRotation -= 90;
                _circle.RotateCircle(-90);
                Circle.transform.DORotate(new Vector3(0, 0, _nextRotation), 0.36f);
                OnBallChildRotation?.Invoke();
                foreach (var circleDot in _circle.Dots)
                {
                    circleDot.transform.DORotateQuaternion(_ballRotation, 0.36f);
                }

                _clickedCircle = null;


                if (!TutorialManager.Instance.IsCompleted(0))
                {
                    TutorialManager.Instance.Complete();
                    TutorialCanvas.SetActive(false);
                }

                // null dal bolsa rotate etyadi , sebabi eger null bolsa onda 
                //clicked circle = circle bolup uzhe null dal bolyady , if dan cykanson tak kak yenede null- a denlanok , 
                //null dal diyip hasap edip rotate edenokdy 
            }
        }

        public void SetCircle(Circle newCircle)
        {
            _circle = newCircle;
            Circle = newCircle.gameObject;
            _nextRotation = Circle.transform.eulerAngles.z;
            _circle.InitOffsets();
            Debug.Log($"rotation of arrow {_nextRotationArrow}");
            _loseChecker.CheckLose();
        }

        public void ClickCircle()
        {
            _canDrag = false;
            TutorialCanvas.SetActive(true);
        }

        public void OnLostGame()
        {
            OnLost?.Invoke();
        }

        public void OnDotPlacement()
        {
            Debug.Log("dots placed");
            OnPlacement?.Invoke();
        }
    }
}