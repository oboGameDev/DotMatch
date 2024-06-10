using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using DG.Tweening;
namespace Assets.Scripts.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Intance;
        [SerializeField] private MenuLevel[] _levels;

        private int _selectedLevel;
        private int _actualLevel;

        private float _delay;

        public ScrollRect ScrollRect;

        
        private void Start()
        {
            _actualLevel = _selectedLevel = PlayerPrefs.GetInt("actual_level", 1);
            ChangeLevelImages();
            ScrollToActiveLevel();
        }
        
        

        private void Update()
        {
            
            /*if (Input.GetMouseButton(0))
            {
                _delay = 0;
            }*///shu gerek dal eken in this case 
            Time.timeScale = 1f;
            _delay += Time.deltaTime;
           // Debug.Log($"{_delay} before time delta time");
           // Debug.Log($"{Time.timeScale}");
            if (_delay >= 2f )
            {
                _delay = 0;
            ScrollToActiveLevel();
            Debug.Log($"afterscrolled {_delay}");
            }
        }

        private void ScrollToActiveLevel()
        {
            int maxLevel = _levels.Length; 
            int smoothSpeed = 3;
            float normalizedPosition = (float)_actualLevel / (maxLevel);
            Debug.Log($"{_actualLevel}/{maxLevel} , {normalizedPosition}");
            DOVirtual.Float(ScrollRect.verticalNormalizedPosition, normalizedPosition, 0.8f,
                value => ScrollRect.verticalNormalizedPosition = value);
        }

        private void ChangeLevelImages()
        {
            foreach (MenuLevel menuLevel in _levels)
            {
                menuLevel.ChangeImageByLevel(_actualLevel, _selectedLevel);
            }
        }

        private void Awake()
        {
            Intance = this;
        }

        public void GoToGamePlayPressed()
        {
            PlayerPrefs.SetInt("level", _selectedLevel);
            Loader.Load(Loader.Scene.GameScene);
            Time.timeScale = 1f;//time sacle 0 etyan yeri bar , shondan son bardede 1 etmesem time 0lygyna gaya
            
        }

        /*public void Select(int level)
        {
            _selectedLevel = level;
            ChangeLevelImages();
        }*/
    }
}