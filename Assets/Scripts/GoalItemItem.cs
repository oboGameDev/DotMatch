using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalItemItem : MonoBehaviour
{
    public static GoalItemItem Instance;
    public int count;
    private int _saveCount;
    public TextMeshProUGUI TextMeshPro;
    public DotColor DotColor;
    public bool All;
    public bool IsGem;
    public ParticleSystem OnBallBounce;
    public ParticlesGoal[] OnBallBounces;
    public Transform Ball;
    public Transform Tick;

    private bool _shownTick;

    private float _time;
    private int updatedCount;

    private void Start()
    {
        Tick.localScale = Vector3.zero;
    }

    private void Awake()
    {
        Instance = this;
    }


    public bool hasFinished => count == 0;

    public int SaveCount => _saveCount;

    public void RemoveCount()
    {
        count = Mathf.Clamp(count - 1, 0, 9999);
        UpdateCount();
    }
    
    public void UpdateCount()
    {
        TextMeshPro.text = count.ToString();
        if (count == 0 && !_shownTick)
        {
            _shownTick = true;
            TextMeshPro.transform.localScale = Vector3.zero;
            Tick.DOScale(1, 0.3f).Capture(); // shu bolmaly
        }
    }

    public void RemoveSavingCount()
    {
        _saveCount = Mathf.Clamp(_saveCount - 1, 0, 9999);
        UpdateCount();
    }

    public void BallBounce()
    {
        ParticleSystem onBallBounce;
        if (!IsGem)
        {
            onBallBounce = OnBallBounce;
        }
        else
        {
            onBallBounce = OnBallBounces.First(c => c.Color == DotColor).Particle;
        }

        Ball.localScale = Vector3.one;
        Debug.Log($"Scaling: {name}");

        Ball.DOScale(1.3f, 0.1f).OnComplete((() =>
        {
            onBallBounce.Play();
            DOVirtual.DelayedCall(0.1f, () => { Ball.DOScale(1, 0.1f).Capture(); });
        })).Capture();
    }

    private void Update()
    {
        if (updatedCount >= 4) return;
        if (_time >= 0.2f)
        {
            _time = 0;
            updatedCount++;
            OnBallBounce.transform.parent.position = transform.position;
            foreach (ParticlesGoal goal in OnBallBounces)
            {
                goal.Particle.transform.parent.position = transform.position;
            }
        }

        _time += Time.deltaTime;
    }

    public void SetSaveCount(int newSaveCount)
    {
        _saveCount = newSaveCount;
    }

    [Serializable]
    public class ParticlesGoal
    {
        public ParticleSystem Particle;
        public DotColor Color;
    }
}