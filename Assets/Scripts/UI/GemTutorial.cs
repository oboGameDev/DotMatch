using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class GemTutorial : MonoBehaviour
    {
        public CanvasGroup Group;
        public int runLevel;
        private void Start()
        {
            if (PlayerPrefs.GetInt("level", 1) == runLevel)
            {
                DOVirtual.DelayedCall(2, () => ShowCanvasGroup(Group));
            }
         
        }
        
        private void ShowCanvasGroup(CanvasGroup group)
        {
            group.DOFade(1, 0.3f);
            group.interactable = true;
            group.blocksRaycasts = true;
        }

        public void HideCanvasGroup(CanvasGroup group)
        {
            group.DOFade(0, 0.5f);
            group.interactable = false;
            group.blocksRaycasts = false;
        }
        }
        
    
    }
