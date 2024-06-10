using TMPro;
using UnityEngine;

namespace Assets.Scripts._4_4_Scripts
{
    public abstract class Booster : MonoBehaviour
    {
        public int Count;
        public TextMeshProUGUI MaxQuantityText;
        public GameObject quantityGameObject;
        public GameObject CostGameObject;

        protected void Start()
        {
            UpdateCount();
            OnStart();
        }

        protected virtual void OnStart()
        {
        }

        public void SubtractCount()
        {
            Count = PlayerPrefs.GetInt($"{GetType().Name}_count", 2);
            Count--;

            PlayerPrefs.SetInt($"{GetType().Name}_count", Count);
            UpdateCount();
        }


        public bool EnoughCount()
        {
            Count = PlayerPrefs.GetInt($"{GetType().Name}_count", 2);
            return Count > 0;
        }


        protected void UpdateCount()
        {
            Count = PlayerPrefs.GetInt($"{GetType().Name}_count", 2);

            MaxQuantityText.text = Count.ToString();
            quantityGameObject.SetActive(Count > 0);
            CostGameObject.SetActive(Count <= 0);
        }

        protected void AddCount(int price)
        {
            if (MoneySystem.Instance.CanAfford(price))
            {
                Count = PlayerPrefs.GetInt($"{GetType().Name}_count", 2);
                Count++;

                PlayerPrefs.SetInt($"{GetType().Name}_count", Count);

                MoneySystem.Instance.SpendMoney(price);
                UpdateCount();
            }
        }

        public abstract void Select();
    }
}