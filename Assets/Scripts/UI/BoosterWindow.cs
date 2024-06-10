using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts._4_4_Scripts;
using DG.Tweening;
using TMPro;
using Tutorial;
using UnityEngine;
using UnityEngine.UI;

public class BoosterWindow : MonoBehaviour
{
    public static BoosterWindow Instance;

    [SerializeField] private CanvasGroup fadeHammer;

    //[SerializeField] private GameObject popUpParentHammer;
    [SerializeField] private CanvasGroup fadeHand;

    // [SerializeField] private GameObject popUpParentHand;
    [SerializeField] private CanvasGroup fadeRaduga;

    //[SerializeField] private GameObject popUpParentRaduga;
    public Transform PopUpHand;
    public Transform PopUpHammer;
    public Transform PopUpRainbow;
    public Transform XButtonHammer;
    public Transform XButtonHand;
    public Transform XButtonRainbow;

    [SerializeField] private BoosterTutorialHands HammerHands;
    [SerializeField] private BoosterTutorialHands HandHands;
    [SerializeField] private BoosterTutorialHands RainbowHands;

    public GameObject HidePanel;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Hammer.Instance.OnHammerPressed += OnHammerPressed;
        BoosterManager.Instance.OnClickedPlaceAndDot += OnClickedPlaceAndDot;
        Hand.Instance.OnHandPressed += OnHandPressed;
        Raduga.Instance.OnRadugaPressed += OnRainbowPressed;

        /*popUpParentHammer.SetActive(false);
        popUpParentHand.SetActive(false);
        popUpParentRaduga.SetActive(false);*/
        gameObject.SetActive(false);
    }

    private void DoScaleAnimHammer()
    {
        /*
        if (!Hammer.Instance.EnoughtCount())
        {
            return;
        }
        */
        gameObject.SetActive(true);
        ShowCanvasGroup(fadeHammer);
        PopUpHammer.localScale = Vector3.zero;
        XButtonHammer.localScale = Vector3.zero;
        DOVirtual.DelayedCall(0.1f, () =>
        {
            PopUpHammer.localScale = Vector3.one * 0.7f;
            PopUpHammer.DOScale(1.15f, 0.18f).SetEase(Ease.Linear).OnComplete(() =>
            {
                PopUpHammer.DOScale(1, 0.16f).SetEase(Ease.InSine).Capture();

                DOVirtual.DelayedCall(0.1f,
                    () =>
                    {
                        XButtonHammer.DOScale(1.25f, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            XButtonHammer.DOScale(1, 0.2f).SetEase(Ease.InSine).Capture();
                        }).Capture();
                    }).Capture();
            }).Capture();
        });
        CanvasElements.Instance.Hide();
    }

    private void DoScaleAnimHand()
    {
        gameObject.SetActive(true);
        ShowCanvasGroup(fadeHand);
        PopUpHand.localScale = Vector3.zero;
        XButtonHand.localScale = Vector3.zero;
        DOVirtual.DelayedCall(0.1f, () =>
        {
            PopUpHand.localScale = Vector3.one * 0.7f;
            PopUpHand.DOScale(1.15f, 0.18f).SetEase(Ease.Linear).OnComplete(() =>
            {
                PopUpHand.DOScale(1, 0.16f).SetEase(Ease.InSine).Capture();

                DOVirtual.DelayedCall(0.1f,
                    () =>
                    {
                        XButtonHand.DOScale(1.25f, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            XButtonHand.DOScale(1, 0.2f).SetEase(Ease.InSine).Capture();
                        }).Capture();
                    }).Capture();
            }).Capture();
        });
        CanvasElements.Instance.Hide();
    }

    private void DoScaleAnimRainbow()
    {
        gameObject.SetActive(true);
        ShowCanvasGroup(fadeRaduga);
        PopUpRainbow.localScale = Vector3.zero;
        XButtonRainbow.localScale = Vector3.zero;
        DOVirtual.DelayedCall(0.1f, () =>
        {
            PopUpRainbow.localScale = Vector3.one * 0.7f;
            PopUpRainbow.DOScale(1.15f, 0.18f).SetEase(Ease.Linear).OnComplete(() =>
            {
                PopUpRainbow.DOScale(1, 0.16f).SetEase(Ease.InSine);

                DOVirtual.DelayedCall(0.1f,
                    () =>
                    {
                        XButtonRainbow.DOScale(1.25f, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            XButtonRainbow.DOScale(1, 0.2f).SetEase(Ease.InSine).Capture();
                        }).Capture();
                    }).Capture();
            }).Capture();
        });
        CanvasElements.Instance.Hide();
    }

    private void OnRainbowPressed()
    {
        HidePanel.SetActive(true);
        DoScaleAnimRainbow();
        ToggleRainbowTutorialHand(false);
    }

    private void OnHandPressed()
    {
        HidePanel.SetActive(true);
        DoScaleAnimHand();
        ToggleHandTutorialHand(false);
    }

    private void OnClickedPlaceAndDot()
    {
        HidePopUp();

        gameObject.SetActive(false);
    }

    private void OnHammerPressed()
    {
        HidePanel.SetActive(true);
        DoScaleAnimHammer();
        ToggleHammerTutorialHand(false);
    }

    private void ToggleHammerTutorialHand(bool showFirst)
    {
        if (!TutorialManager.Instance.IsCompleted(4) && TutorialManager.Instance.CanStart(4))
        {
            HammerHands.Hand1.gameObject.SetActive(showFirst);
            HammerHands.Hand2.gameObject.SetActive(!showFirst);
        }
    }

    public void ToggleHandTutorialHand(bool showFirst)
    {
        if (!TutorialManager.Instance.IsCompleted(5) && TutorialManager.Instance.CanStart(5))
        {
            HandHands.Hand1.gameObject.SetActive(showFirst);
            HandHands.Hand2.gameObject.SetActive(!showFirst);
        }
    }
    private void ToggleRainbowTutorialHand(bool showFirst)
    {
        if (!TutorialManager.Instance.IsCompleted(6) && TutorialManager.Instance.CanStart(6))
        {
            RainbowHands.Hand1.gameObject.SetActive(showFirst);
            RainbowHands.Hand2.gameObject.SetActive(!showFirst);
        }
    }

    public void HidePopUp()
    {
        /*
        DOVirtual.DelayedCall(0.1f, () =>
        {
            PopUpHammer.localScale = Vector3.one * 0.7f;

            PopUpHammer.DOScale(0f, 0.3f).SetEase(Ease.OutSine);
        });
        */

        CanvasElements.Instance.Show();
        fadeHammer.DOFade(0, 0.5f);
        fadeHand.DOFade(0, 0.5f);
        fadeRaduga.DOFade(0, 0.5f);
        gameObject.SetActive(false);
        BoosterManager.Instance.SelectedBooster = null;

        Debug.Log($"All hide");
        HidePanel.SetActive(false);
    }

    public void HidePopUpForXButtonHammer()
    {
        CanvasElements.Instance.Show();
        HideCanvasGroup(fadeHammer);
        gameObject.SetActive(false);
        BoosterManager.Instance.SelectedBooster = null;
        Debug.Log($"Hammer hide");
        ToggleHammerTutorialHand(true);
        HidePanel.SetActive(false);
    }

    public void HideForHand()
    {
        CanvasElements.Instance.Show();
        HideCanvasGroup(fadeHand);
        gameObject.SetActive(false);
        BoosterManager.Instance.SelectedBooster = null;
        Debug.Log($"Hand hide");
        ToggleHandTutorialHand(true);
        HidePanel.SetActive(false);
    }

    public void HideForRainbow()
    {
        CanvasElements.Instance.Show();
        HideCanvasGroup(fadeRaduga);
        gameObject.SetActive(false);
        BoosterManager.Instance.SelectedBooster = null;
        Debug.Log($"Rainbow hide");
        ToggleRainbowTutorialHand(true);
        HidePanel.SetActive(false);
    }

    private void ShowCanvasGroup(CanvasGroup group)
    {
        group.DOFade(1, 0.3f);
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    private void HideCanvasGroup(CanvasGroup group)
    {
        group.DOFade(0, 0.5f);
        group.interactable = false;
        group.blocksRaycasts = false;
    }
}