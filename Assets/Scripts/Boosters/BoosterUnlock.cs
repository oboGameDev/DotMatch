using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts._4_4_Scripts
{
    public class BoosterUnlock : MonoBehaviour
    {
        public int level;
        public Image BoosterImage;
        public Sprite OpenedImage;
        public Sprite ClosedImage;
        public Button Booster;
        public GameObject CostImage;
        public GameObject QuantityImage;


        public void ChangeBoosterImage(int toCheck, int current)
        {
            if (current == level || toCheck >= level)
            {
                BoosterImage.sprite = OpenedImage;
                /*
                CostImage.SetActive(true);
                QuantityImage.SetActive(true);
            */
            }

            else
            {
                BoosterImage.sprite = ClosedImage;
                Booster.enabled = toCheck >= level;
                CostImage.SetActive(false);
                QuantityImage.SetActive(false);
            }
        }
    }
}