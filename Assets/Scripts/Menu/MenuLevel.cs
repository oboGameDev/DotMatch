using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    public class MenuLevel : MonoBehaviour
    {
        public int level;
        public Image levelCircle;
        public Sprite DefaultImage;
        public Sprite SelectedImage;
        public Sprite ClosedImage;
        public Button levelButton;

        public void ChangeImageByLevel(int toCheck, int current)
        {
            if (current == level)
                levelCircle.sprite = SelectedImage;
            else if (toCheck > level)
                levelCircle.sprite = DefaultImage;
            else
                levelCircle.sprite = ClosedImage;
            levelButton.enabled = toCheck >= level;
        }

        /*public void Select()
        {
            MenuManager.Intance.Select(level);
        }*/
    }
}