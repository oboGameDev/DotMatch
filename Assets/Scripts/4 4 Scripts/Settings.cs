using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts
{
   
    public class Settings : MonoBehaviour
    {

        public SettingsMoveInfo[] SettingsMoveInfo;//sheyle public edip almasak dashyna gornenok

        private bool _show = false;
        public GameObject Background;


        // Start is called before the first frame update
        void Start()
        {

            foreach (SettingsMoveInfo moveInfo in SettingsMoveInfo)
            {
                moveInfo.Target.position = moveInfo.From.position;
            }
            Background.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Toggle()
        {
            _show = !_show; // here show yada hide etya show case -y ozi uytgedip durmaly

            if (_show)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        public void Hide()
        {
            Background.SetActive(false);
            float delay = 0;
            List<SettingsMoveInfo> moveInfoList = SettingsMoveInfo.ToList();
            moveInfoList.Reverse();

            foreach (SettingsMoveInfo moveInfo in moveInfoList)
            {
                moveInfo.Target.DOMove(moveInfo.From.position, 0.24f).SetDelay(delay).SetUpdate(true);
                delay += 0.1f;
            }

            Time.timeScale = 1f;
            /*float delay = 0;
            foreach (SettingsMoveInfo moveInfo in SettingsMoveInfo)
            {
                moveInfo.Target.DOMove(moveInfo.From.position, 0.3f).SetDelay(delay);
                delay += 0.1f;
            }*/
        }


        public void Show()
        {
            float delay = 0;
            foreach (SettingsMoveInfo moveInfo in SettingsMoveInfo)
            {
                Background.SetActive(true);
                moveInfo.Target.DOMove(moveInfo.To.position, 0.24f).SetDelay(delay).SetUpdate(true);
                delay += 0.1f;
                Time.timeScale = 0;
            }
        }


    }
    
    [Serializable]
    public class SettingsMoveInfo
    {
        public Transform From;
        public Transform To;
        public Transform Target;
    }
}

