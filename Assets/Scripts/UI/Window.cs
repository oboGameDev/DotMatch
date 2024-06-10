using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class Window : MonoBehaviour
    {
        public static Window Instance;

        public Image Background;
        public Transform Panel;
        public CanvasGroup YouEarned;
        public Transform button;
        public CanvasGroup NoThanksText;

        public float OpenDelay = 0;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            UpdateMoney();
        }

        public void OpenWindow()
        {
            if (OpenDelay > 0)
            {
                DOVirtual.DelayedCall(OpenDelay, ShowWindow);
            }
            else
            {
                ShowWindow();
            }
        }

        private void ShowWindow()
        {
            gameObject.SetActive(true);
            Panel.localScale = Vector3.zero;
            button.localScale = Vector3.zero;
            /*
            NoThanksText.localScale = Vector3.zero;
            */

            DOVirtual.DelayedCall(0.1f, () =>
            {
                Panel.localScale = Vector3.one * 0.7f;

                Panel.DOScale(1.25f, 0.3f).SetEase(Ease.OutSine).OnComplete(() =>
                {
                    Panel.DOScale(1, 0.2f).SetEase(Ease.InSine).Capture();


                    DOVirtual.DelayedCall(0.3f,
                        () =>
                        {
                            ShowCanvasGroup(YouEarned);
                            //button.localScale = Vector3.one * 0.2f;
                            DOVirtual.DelayedCall(0.25f,
                                () =>
                                {
                                    button.DOScale(1f, 0.5f).SetEase(Ease.InOutSine).Capture();
                                    DOVirtual.DelayedCall(1.2f, () =>
                                    {
                                        if (NoThanksText != null)
                                        {
                                            ShowCanvasGroupSlower(NoThanksText);
                                        }
                                        //here might error appear 
                                    }).Capture();
                                }).Capture();
                        }).Capture();
                }).Capture();
            });
        }

        public void ShowCanvasGroup(CanvasGroup group)
        {
            group.DOFade(1, 0.5f);
            group.interactable = true;
            group.blocksRaycasts = true;
        }

        public void ShowCanvasGroupSlower(CanvasGroup group)
        {
            group.DOFade(1, 1f);
            group.interactable = true;
            group.blocksRaycasts = true;
        }

        public void HideCanvasGroup(CanvasGroup group)
        {
            group.DOFade(0, 0.1f);
            group.interactable = false;
            group.blocksRaycasts = false;
        }

        public void UpdateMoney()
        {
            /*
            GameLevelGoalUI.Instance.earned = 50 * PlayerPrefs.GetInt("level", 1);
            GameLevelGoalUI.Instance.earnedMoneyText.text= "+" + GameLevelGoalUI.Instance.earned;
            */
            GameLevelGoalUI.Instance.levelText.text = "Level " + PlayerPrefs.GetInt("level", 1).ToString();

            GameLevelGoalUI.Instance.earned = 10;
            GameLevelGoalUI.Instance.earnedMoneyText.text = "+ 10";
            MoneySystem.Instance.AddMoney(GameLevelGoalUI.Instance.earned);
        }
    }
}