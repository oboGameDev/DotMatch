using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

public class PopUpSettings : MonoBehaviour
{
    public static PopUpSettings Instance;

    public GameObject Background;

    [SerializeField] private CanvasGroup Settings;

    public Transform popUpSettings;
    public Transform xButton;
    public Transform restartButton;
    public Transform homeButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Background.SetActive(false);
        HideCanvasGroup(Settings);
    }

    public void SettingsPopUp()
    {
        foreach (Place place in PlaceManager.Instance.Places)
        {
            if (place.TutorialOccupied)
            {
                return;
            }
        }

        Background.SetActive(true);
        ShowCanvasGroup(Settings);
        popUpSettings.localScale = Vector3.zero;
        xButton.localScale = Vector3.zero;
        restartButton.localScale = Vector3.zero;
        homeButton.localScale = Vector3.zero;
        DOVirtual.DelayedCall(0.1f, () =>
        {
            popUpSettings.localScale = Vector3.one * 0.5f;
            popUpSettings.DOScale(1f, 0.2f).SetEase(Ease.Linear).OnComplete((() =>
            {
                /*
                popUpSettings.DOScale(1, 0.16f).SetEase(Ease.InSine);
                */
                restartButton.localScale = Vector3.one * 0.5f;
                homeButton.localScale = Vector3.one * 0.5f;
                restartButton.DOScale(1f, 0.18f).SetEase(Ease.Linear).Capture();
                homeButton.DOScale(1f, 0.18f).SetEase(Ease.Linear).Capture();

                DOVirtual.DelayedCall(0.1f,
                    () =>
                    {
                        xButton.localScale = Vector3.one * 0.7f;

                        xButton.DOScale(1f, 0.18f).SetEase(Ease.Linear).Capture() /*.OnComplete(() =>
                        {
                           xButton.DOScale(1, 0.2f).SetEase(Ease.InSine);
                        })*/;
                    }).Capture();
            })).Capture();
        }).Capture();
    }

    public void HideSettings()
    {
        Background.SetActive(false);
        HideCanvasGroup(Settings);
    }

    private void ShowCanvasGroup(CanvasGroup group)
    {
        group.DOFade(1, 0.3f);
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    private void HideCanvasGroup(CanvasGroup group)
    {
        group.DOFade(0, 0.1f);
        group.interactable = false;
        group.blocksRaycasts = false;
    }
}