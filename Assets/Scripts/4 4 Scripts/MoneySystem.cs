using System;
using Assets.Scripts._4_4_Scripts;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MoneySystem : MonoBehaviour
{
    public static MoneySystem Instance;
    [SerializeField] private TextMeshProUGUI _moneyText;

    private int _balance;
    private int _moneySpent;
    private int _moneyEarned;

    public event Action<int> BalanceChanged;


    private void Awake()
    {
        Instance = this;
        _balance = PlayerPrefs.GetInt("money", 200); // Initial balance

        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        _moneyText.text = _balance.ToString();
    }

    public void AddMoney(int amount)
    {
        _balance += amount;
        PlayerPrefs.SetInt("money", _balance);
        UpdateMoneyText();
        BalanceChanged?.Invoke(_balance);
    }

    public void MultiplyMoney(int amount)
    {
        _balance *= amount;
        PlayerPrefs.SetInt("money", _balance);
        UpdateMoneyText();
        BalanceChanged?.Invoke(_balance);
    }

    public void SpendMoney(int cost)
    {
        _balance -= cost;
        PlayerPrefs.SetInt("money", _balance);
        UpdateMoneyText();
        BalanceChanged?.Invoke(_balance);
    }

    public bool CanAfford(int cost)
    {
        return _balance >= cost;
    }

    public void ScaleMoneyText()
    {
        DOVirtual.DelayedCall(0.18f, () =>
            _moneyText.transform.DOScale(1.4f, 0.2f).SetEase(Ease.OutBounce)
                .OnComplete(() => { ScaleToDefaultScale(); })
        );
    }

    public void ScaleToDefaultScale()
    {
        _moneyText.transform.DOScale(1f, 0.2f).SetEase(Ease.InBack) /*.OnComplete(() =>
        {

        })*/;
        AddMoney(5);
        FloatingText.Instance.HideCanvasGroup(FloatingText.Instance.money);
        FloatingText.Instance.moneyToAdd.SetActive(false);
    }

    public RectTransform TextPosition()
    {
        return _moneyText.rectTransform;
    }

    public int GetBalance()
    {
        return _balance;
    }
}