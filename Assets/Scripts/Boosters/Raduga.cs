using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts._4_4_Scripts;
using DG.Tweening;
using TMPro;
using Tutorial;
using UnityEngine;

public class Raduga : Booster
{
    public static Raduga Instance;

    public Action OnRadugaPressed;

    private void Awake()
    {
        Instance = this;
    }

    public override void Select()
    {
        /*if (MoneySystem.Instance.CanAfford(50))
        {*/
        if (BoosterManager.Instance.SelectedBooster == this)
        {
            BoosterManager.Instance.SelectedBooster = null;
            return;
        }
        

        if (!EnoughCount())
        {
            if (!MoneySystem.Instance.CanAfford(35))
            {
                return;
            }
            else
            {
                AddCount(35);
            }
        }

        OnRadugaPressed?.Invoke();
        BoosterManager.Instance.SelectedBooster = this;
        //MoneySystem.Instance.SpendMoney(50);

        // }
    }

    public void Activate(Place place)
    {
        if (place != null && !place.HasDot())
        {
            var radugaPrefab = CircleSpawner.Instance.RadugaDot;
            var Raduga = Instantiate(radugaPrefab, place.transform);
            Raduga.EnableCollider();
            place.SetDots(Raduga);
            PlaceManager.Instance.CheckPlaces(new[] { place }.ToList());
            BoosterManager.Instance.SelectedBooster = null;
            PlaceManager.Instance.SaveLevel();
            if (!TutorialManager.Instance.IsCompleted(6) &&
                TutorialManager.Instance.IsCurrent(6)) 
            {
                TutorialManager.Instance.Complete();
                PlaceManager.Instance.ReleaseOccupiedPlaces();
            }
            //Spend etdirmeli 
        }
    }
    
}


