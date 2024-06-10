using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CanvasElements : MonoBehaviour
    {
        public CanvasElementsMoveInfo[] CanvasElementsMoveInfos; //sheyle public edip almasak dashyna gornenok

        public static CanvasElements Instance;
        public GameObject sceneCircle;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            foreach (CanvasElementsMoveInfo moveInfo in CanvasElementsMoveInfos)
            {
                moveInfo.Target.position = moveInfo.From.position;
            }
        }

        public void Hide()
        {
            List<CanvasElementsMoveInfo> moveInfoList = CanvasElementsMoveInfos.ToList();
            moveInfoList.Reverse();

            foreach (CanvasElementsMoveInfo moveInfo in moveInfoList)
            {
                moveInfo.Target.DOMove(moveInfo.To.position, 0.3f).SetUpdate(true);
                
            }
            
            sceneCircle.SetActive(false);
        }


        public void Show()
        {
            foreach (CanvasElementsMoveInfo moveInfo in CanvasElementsMoveInfos)
            {
                moveInfo.Target.DOMove(moveInfo.From.position, 0.3f).SetUpdate(true);
            }
            
            sceneCircle.SetActive(true);
        }
    }

    [Serializable]
    public class CanvasElementsMoveInfo

    {
        public Transform From;
        public Transform To;
        public Transform Target;
    }
}