using System;
using DG.Tweening;
using TMPro;
using Tutorial;
using UnityEngine;

namespace Assets.Scripts._4_4_Scripts
{
    public class Hammer : Booster
    {
        public static Hammer Instance;
        public Action OnHammerPressed;

        private void Awake()
        {
            Instance = this;
        }

        public override void Select()
        {
            if (BoosterManager.Instance.SelectedBooster == this)
            {
                BoosterManager.Instance.SelectedBooster = null;
                return;
            }

            if (!EnoughCount())
            {
                if (!MoneySystem.Instance.CanAfford(25))
                {
                    return;
                }
                else
                {
                    AddCount(25);
                }
            }

            OnHammerPressed?.Invoke();
            BoosterManager.Instance.SelectedBooster = this;
        }

        public void Activate(Place place)
        {
            if (place.HasDot())
            {
                place.DestroyDot(0);

                PlaceManager.Instance.SaveLevel();
                
                if (!TutorialManager.Instance.IsCompleted(4) &&
                    TutorialManager.Instance.IsCurrent(4)) 
                {
                    TutorialManager.Instance.Complete();
                    PlaceManager.Instance.ReleaseOccupiedPlaces();
                }
            }

            BoosterManager.Instance.SelectedBooster = null;
        }
    }
}