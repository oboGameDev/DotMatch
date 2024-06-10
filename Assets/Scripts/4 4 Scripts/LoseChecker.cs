using System;
using _4_4_Scripts;
using DG.Tweening;
using UnityEngine;
using Utils;

namespace Assets.Scripts._4_4_Scripts
{
    public class LoseChecker
    {
        private int loseCount = 0;

        public void CheckLose()
        {
            bool hasFreePlace = false;
            foreach (Place place in PlaceManager.Instance.Places)
            {
                int[] angles = new[] { 0, 90, 180, 270 };
                var isFree = false;
                foreach (int angle in angles)
                {
                    isFree |= CanFit(place, angle);
                    if (isFree) break;
                }

                if (isFree)
                {
                    hasFreePlace = true;
                    break;
                }
            }

            if (!hasFreePlace)
            {
                Debug.Log("Lost");
                DataSaver.Increment("losed_times");
                DragController.Instance.Losepanel.OpenWindow();
                DragController.Instance.OnLostGame();
            }
        }

        private static bool CanFit(Place place, float angle)
        {
            bool isFree = true;
            foreach (Dot dot in DragController.Instance.SelectedCircle.Dots)
            {
                var nextPlace = PlaceManager.Instance.GetCell(place.Location + dot.Offset.RotatePoint(angle));
                Debug.Log($"nextplace {nextPlace}");
                if (nextPlace == null || nextPlace.HasDot())
                {
                    isFree = false;
                    break;
                }
            }

            return isFree;
        }
    }
}