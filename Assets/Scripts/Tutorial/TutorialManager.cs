using System;
using Assets.Scripts;
using Cinemachine;
using DG.Tweening;
using NINESOFT.TUTORIAL_SYSTEM;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

namespace Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance;

        [SerializeField] private TutorialModules[] Modules;

        private int _index;
        private TutorialModules _current;

        private void Awake()
        {
            Instance = this;
            _index = DataSaver.Load("tutorial", 0);
            if (_index < Modules.Length)
                _current = Modules[_index];
        }


        private void Start()
        {

            if (_index < Modules.Length && DragController.Instance._dragging && _current != null)
            {
                _current.Disable();
            }
            // return;
            if (_index < Modules.Length)
                _current.Activate();
        }

        public bool IsCompleted(int index)
        {
            // return true;
            return _index > index;
        }

        public bool IsCurrent(int index)
        {
            // return true;
            bool isActive = true;
            if (Modules.Length > _index)
            {
                isActive = Modules[_index].isActive;
            }

            return _index == index && isActive;
        }
        public bool CanStart(int index)
        {
            // return true;
            bool isActive = true;
            if (Modules.Length > _index)
            {
                isActive = Modules[_index].CanActivate();
            }

            return _index == index && isActive;
        }

        public void Complete()
        {
            if (_current != null /*|| DragController.Instance._dragging*/)
                _current.Disable();
            
            _index++;
            DataSaver.Save("tutorial", _index);
            if (_index < Modules.Length)
            {
                _current = Modules[_index];
                _current.Activate();
            }
        }
    }

    [Serializable]
    public class TutorialModules
    {
        public int Index;
        public TutorialModule[] Modules;
        public TutorialModule[] ModulesToDisable;
        public Button[] ToDisable;
        public bool autoRun = true;
        public int runLevel;
        public UnityEvent StageStarted;
        public UnityEvent StageEnded;

        public float delay;
        

        public bool isActive;

        public void Activate()
        {
            if (!CanActivate()) return;

            DOVirtual.DelayedCall(delay, () =>
            {
                foreach (TutorialModule module in Modules)
                {
                    module.gameObject.SetActive(true);
                }

                foreach (Button button in ToDisable)
                {
                    button.enabled = false;
                }
            });
          

            isActive = true;
            StageStarted?.Invoke();
        }

        public bool CanActivate()
        {
            if (autoRun|| (!autoRun && PlayerPrefs.GetInt("level", 1) == runLevel))
            {
                return true;
            }

            return false;
        }

        public void Disable()
        {
            foreach (TutorialModule module in ModulesToDisable)
            {
                module.gameObject.SetActive(false);
            }

            foreach (Button button in ToDisable)
            {
                button.enabled = true;
            }

            isActive = false;
            StageEnded?.Invoke();
        }
    }
}