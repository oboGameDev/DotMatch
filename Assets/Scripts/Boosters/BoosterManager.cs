using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts._4_4_Scripts
{
    public class BoosterManager : MonoBehaviour
    {
        public static BoosterManager Instance;

        public Booster SelectedBooster;
        public Action OnClickedPlaceAndDot;
        private int _activeLevel;
        private int _actualLevel;
        [SerializeField] private BoosterUnlock[] _boosters;

        public Dot dot;
        private void Awake()
        {
            Instance = this;
        }

        private IEnumerator Start()
        {
            _actualLevel = _activeLevel = PlayerPrefs.GetInt("actual_level", 1);
            yield return null;
            ChangeBoosterImages();
        }

        private void ChangeBoosterImages()
        {
            foreach (BoosterUnlock boosterUnlock in _boosters)
            {
                boosterUnlock.ChangeBoosterImage(_actualLevel, _activeLevel);
            }
        }

        public void ClickedPlace(Place place)
        {
            
            if (!place.canUseBooster ) return;
            Debug.Log($"Clicked Place for hammer {place.name}");
            if (SelectedBooster is Hammer hammer && place.HasDot() && place.Dot != null)
            {
                if (Hammer.Instance.EnoughCount() || MoneySystem.Instance.CanAfford(25))
                {
                    Hammer.Instance.SubtractCount();
                    OnClickedPlaceAndDot?.Invoke();
                    hammer.Activate(place);
                }
            }

            if (SelectedBooster is Raduga raduga)
            {
                Debug.Log($"Clicked Place for hammer {place.name}");
                if (Raduga.Instance.EnoughCount() || MoneySystem.Instance.CanAfford(35))
                {
                    Raduga.Instance.SubtractCount();
                    OnClickedPlaceAndDot?.Invoke();
                    raduga.Activate(place);
                }
            }
        }

        public void ClickedDot(Dot dot)
        {
            if ((dot.Place != null && !dot.Place.canUseBooster) || dot.isGem) return;
            Debug.Log($"{gameObject.name} is clicked  , {dot.name}");
            if (SelectedBooster is Hammer hammer)
            {
                if (dot.Place != null)
                {
                    if (Hammer.Instance.EnoughCount() || MoneySystem.Instance.CanAfford(25) && dot.Place.HasDot())
                    {
                        Hammer.Instance.SubtractCount();
                        OnClickedPlaceAndDot?.Invoke();
                        hammer.Activate(dot.Place);
                    }
                    /*
                 hammer.transform.DOScale(1.1f, 0.1f);
             */
                } //place select edilse bolyady dine destroy hammer saylanan wagty dota basan wagtam bolar yaly etmek ucin gerek shu
            }
            else if (SelectedBooster is Hand hand)
            {
                if (Hand.Instance.EnoughCount() || MoneySystem.Instance.CanAfford(25))
                {
                    OnClickedPlaceAndDot?.Invoke();
                    hand.Activate(dot);
                }
            }
        }
    }
}