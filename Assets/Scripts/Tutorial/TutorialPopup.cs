using System.Collections;
using DG.Tweening;
using NINESOFT.TUTORIAL_SYSTEM;
using UnityEngine;

namespace Tutorial
{
    public class TutorialPopup : TutorialModule
    {
        public CanvasGroup group;
       // public Transform Popup;
        public CanvasGroup PopUp;
        public GameObject Hand;

        public Transform from;
        public Transform to;
        public GameObject Booster;
        

        protected override void ResetProperties()
        {
            group.alpha = 0;
           // Popup.localScale = Vector3.zero;
             PopUp.alpha = 0;
            ShowHand(false);
        }

        public override IEnumerator ActiveTheModuleEnum()
        {
                group.DOFade(1, 0.6f); 
                PopUp.DOFade(1,0.6f );
           
            yield return null;
        }

        private void ShowHand(bool show)
        {
            if (Hand != null)
            {
                Hand.SetActive(show);
            }
        }

        public void Hide()
        {  
            ShowHand(false);
            Sequence sequence = DOTween.Sequence();
            sequence.Pause();
            sequence.OnComplete((() =>
            {
                ShowHand(true);
                gameObject.SetActive(false);
            }));
            sequence.Append(group.DOFade(0, 0.5f)/*.SetDelay(1f)*/);
            sequence.Join(PopUp.transform.DOMove(to.position, 0.8f).SetEase(Ease.InBack));
            sequence.Join(PopUp.transform.DOScale(to.localScale * 1.75f, 0.7f).Capture());
            sequence.Append(PopUp.DOFade(0, 0.2f));
            sequence.Join(Booster.transform.DOScale(1 * 1.3f, 0.3f)
                .OnComplete((() => Booster.transform.DOScale(1, 0.3f).Capture()))
            );
            /*sequence.AppendCallback((() =>
            {
                   }));*/
            sequence.Play();
            ShowHand(false);
        }
    }
}