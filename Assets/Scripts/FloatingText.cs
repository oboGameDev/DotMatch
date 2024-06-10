using System;
using _Scripts.Utils;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Scripts._4_4_Scripts
{
    public class FloatingText : MonoBehaviour
    {
        public FloatingTextPos floatingTextPos; //sheyle public edip almasak dashyna gornenok

        public static FloatingText Instance;
        public Transform floatingTextObj;
        private Image _defaultImage;
        [SerializeField] private CanvasGroup canvasGroup;

        private MouseEvents _mouseEvents;
        public Image[] textImages;

        public GameObject moneyToAdd;
        public CanvasGroup money;

        private int _randomIndex;

        private void Awake()
        {
            gameObject.SetActive(false);
            Instance = this;
        }

        public void WordAppear()
        {
            _randomIndex = Random.Range(0, textImages.Length);
            // Assign the random sprite to the defaultImage

            _defaultImage = textImages[_randomIndex];
            textImages[_randomIndex].gameObject.SetActive(true);
            moneyToAdd.SetActive(true);
            /*_defaultImage.enabled = true;*/
            gameObject.SetActive(true);
            ShowCanvasGroup(canvasGroup);
            ShowCanvasGroup(money);
            floatingTextObj.localScale = Vector3.zero;

            DOVirtual.DelayedCall(0.01f, () =>
            {
                floatingTextObj.localScale = Vector3.one * 0.7f;
                floatingTextObj.DOScale(1.3f, 0.1f).SetEase(Ease.Linear).Capture() /*.OnComplete(() =>
                {
                    /*floatingTextObj.DOScale(1, 0.16f).SetEase(Ease.InSine);#1#
                })*/;

                DOVirtual.DelayedCall(0.1f, () =>
                {
                    floatingTextPos.Target.DOMove(floatingTextPos.To.position, 1.5f);
                    floatingTextPos.TargetMoney.DOMove(floatingTextPos.ToMoney.position, 0.5f);
                    /*.OnComplete(() =>
                    {
                        MoneySystem.Instance.ScaleMoneyText();
                    });*/
                    MoneySystem.Instance.ScaleMoneyText();
                    /*MoneySystem.Instance.ScaleToDefaultScale();*/
                    DOVirtual.DelayedCall(1f, () =>
                    {
                        //floatingTextObj.DOScale(0.4f, 0.16f) /*.SetEase(Ease.InSine)*/;
                        HideCanvasGroup(canvasGroup);
                    });
                });
            }).OnComplete((() =>
            {
                floatingTextPos.Target.DOMove(floatingTextPos.From.position, 0.01f);
                floatingTextPos.TargetMoney.DOMove(floatingTextPos.FromMoney.position, 0.01f);
            }));
            DOVirtual.DelayedCall(1.8f, () =>
            {
                textImages[_randomIndex].gameObject.SetActive(false);
            });
        }

        private void ShowCanvasGroup(CanvasGroup group)
        {
            group.DOFade(1, 0.3f);
            group.interactable = true;
            group.blocksRaycasts = true;
        }

        public void HideCanvasGroup(CanvasGroup group)
        {
            group.DOFade(0, 0.3f);
            group.interactable = false;
            group.blocksRaycasts = false;
        }
    }

    [Serializable]
    public class FloatingTextPos

    {
        public Transform From;
        public Transform To;
        public Transform Target;
        public Transform FromMoney;
        public Transform ToMoney;
        public Transform TargetMoney;
    }
}