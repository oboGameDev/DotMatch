using System;
using System.Collections;
using Assets.Scripts.UI;
using DG.Tweening;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Scripts._4_4_Scripts
{
    public class GameLevelUI : MonoBehaviour
    {
        public static GameLevelUI Instance;
        [SerializeField] private Image _levelFillBar;
        protected Image DelayedIncreasingImage;
        [SerializeField] private LevelInfo _info;
        public TextMeshProUGUI MaxLevelTextMeshProUgui;
        public TextMeshProUGUI DestroyedDotsTextMeshProUgui;
        public TextMeshProUGUI GameLevelText;
        public GameObject Ball;
        private float _destroyedDots;
        private float _maxDotsToBeDestroyed => _current;
        private int _current;
        
        private bool _shownWinPanel;
        public Action OnGameFinished;

        public ParticleSystem OnBallBounce;
        private CheckPlaces CheckPlaces = new CheckPlaces();
        

        public Window WinPanel;
        [SerializeField] private ProgressShadowed _progressShadowed;

        private void Update()
        {
            Instance = this;
        }

        private void Start()
        {
            var level = PlayerPrefs.GetInt("level", 1) - 1;
            if (level >= _info.DestroyCount.Length)
            {
                _current = Random.Range(100, 300);
            }
            else
            {
                _current = _info.DestroyCount[level];
            }

            MaxLevelTextMeshProUgui.text = _maxDotsToBeDestroyed.ToString();
            DestroyedDotsTextMeshProUgui.text = _destroyedDots.ToString();
            GameLevelText.text = $"{DestroyedDotsTextMeshProUgui.text} / {MaxLevelTextMeshProUgui.text} ";

            Place.Instance.OnLevelBarFilled += OnLevelBarFilled;

            _progressShadowed.FillAmount = 0;
        }

        private void OnLevelBarFilled()
        {
            if (!_shownWinPanel && _maxDotsToBeDestroyed <= 0 && GameLevelGoalUI.Instance.HasFinished())
            {
                _shownWinPanel = true;

                OnGameFinished?.Invoke();
                CheckPlaces.OnGameFinished();
                DOVirtual.DelayedCall(2f, () =>
                {
                    WinPanel.OpenWindow();
                    
                });
            }
        }


        private void UpdateProgressBar()
        {
            _progressShadowed.DoFill((float)_destroyedDots / _maxDotsToBeDestroyed, 0.3f);
        }

        public void AddCount()
        {
            OnLevelBarFilled();
            if (_maxDotsToBeDestroyed <= _destroyedDots)
            {
                return;
            }
           
            UpdateProgressBar();
            _destroyedDots ++; //destroy edilende goshulmaly san 

            var generalEarnedLevel = _destroyedDots.ToString();
            DestroyedDotsTextMeshProUgui.text = generalEarnedLevel;
            GameLevelText.text = $"{DestroyedDotsTextMeshProUgui.text} / {MaxLevelTextMeshProUgui.text} ";
        }

        public void BallBounce()
        {
            ParticleSystem onBallBounce = OnBallBounce;

            Ball.transform.DOScale(1.3f, 0.1f).OnComplete((() =>
            {
                onBallBounce.Play();
                DOVirtual.DelayedCall(0.1f, () =>
                {
                    Ball.transform.DOScale(1, 0.1f).Capture();
                }).Capture();
            })).Capture();
        }
    }
}