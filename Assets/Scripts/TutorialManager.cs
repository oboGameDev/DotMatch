using System;
using DG.Tweening;
using UnityEngine;
using Utils;

namespace DefaultNamespace
{
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance;
        [SerializeField] private CanvasGroup[] Tutorials;

        private void Awake()
        {
            Instance = this;
        }

        public void HideTutorials()
        {
            Tutorials.ForEach(f => f.DOFade(0, 0.2f));;
        }
        public void ShowTutorials()
        {
            Tutorials.ForEach(f => f.DOFade(1, 0.2f));;
        }
        
    }
}