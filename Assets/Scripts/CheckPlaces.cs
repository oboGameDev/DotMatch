using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts._4_4_Scripts
{
    public class CheckPlaces
    {
        public bool gameFinished = false;

        public void OnGameFinished()
        {
            gameFinished = false;
            bool hasFreePlace = false;
            float delay = 1;
            foreach (Place place in PlaceManager.Instance.Places)
            {
                if (place.HasDot() && !place.TutorialOccupied)
                {
                    place.DestroyDot(delay);
                    delay += 0.05f;
                }
            }
            gameFinished = true;
        }
    }
}