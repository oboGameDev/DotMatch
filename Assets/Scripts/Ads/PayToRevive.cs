using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.UI;
using UnityEngine;
using Utils;

public class PayToRevive : MonoBehaviour
{
    public GameObject losePanel;
    public void PayToReviveLevel()
    {
        Time.timeScale = 1f;
        if (MoneySystem.Instance.CanAfford(100))
        {
            var group2 = PlaceManager.Instance._groupsDictionary[1].Union(PlaceManager.Instance._groupsDictionary[4])
                .Union(PlaceManager.Instance._groupsDictionary[7]);
            var places2 = group2;
            float delay = 0;
            foreach (var place in places2)
            {
                Debug.Log($"Destroying {place.Location}", place);
                if (place.HasDot())
                {
                    place.DestroyDotIfNotGem(delay);
                    delay += 0.03f;
                }
            }

            MoneySystem.Instance.SpendMoney(100);
            DataSaver.Decrease("losed_times", 0, 1);
            losePanel.SetActive(false);
        }
    }
}