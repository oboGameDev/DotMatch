using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts._4_4_Scripts;
using Assets.Scripts.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

public class GameLevelGoalUI : MonoBehaviour
{
    public static GameLevelGoalUI Instance;
    [SerializeField] private GoalInfo Goals;
    [SerializeField] private LevelInfo _info;
    [SerializeField] private DotsLocaitons[] Locations;
    [SerializeField] private TextMeshPro MeshProPrefab;
    [SerializeField] private DotRender[] _renders;
    [SerializeField] private RawImage[] Images;
    [SerializeField] private Transform ParticlesParent;
    private bool _shownWinPanel;
    private int _current;
    public Action OnGameFinished;
    public List<GoalItemItem> Containers;
    private CheckPlaces CheckPlaces = new CheckPlaces();
    public Window WinPanel;
    public TextMeshProUGUI earnedMoneyText;

    public TextMeshProUGUI levelText;
    public int earned;

    public CanvasGroup HardLevel;
    public Action OnWinPanelShowSound;

    public ParticleSystem winParticle;

    public bool HasGem;

    public int[] runlevel;

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        //HardLevel.DOFade(0, 0.1f);
        
        foreach (int run in runlevel)
        {
            if (PlayerPrefs.GetInt("level", 1) == run)
            {
                HardLevel.DOFade(1, 0.01f);
            }
        }

        Place.Instance.OnLevelBarFilled += OnLevelBarFilled;
        var dictionary = _renders.ToDictionary(r => r.Color, render => render);
        yield return null;
        int level = PlayerPrefs.GetInt("level", 1) - 1;
        GoalItem goal = Goals.Goals[Mathf.Min(level, Goals.Goals.Length - 1)];

        if (level >= _info.DestroyCount.Length)
        {
            _current = Random.Range(100, 300);
        }
        else
        {
            _current = _info.DestroyCount[level];
        }

        int goalTypes = goal.Types;
        if (goalTypes > 0)
        {
            Dot[] dots = CircleSpawner.Instance.AllDots;

            List<Dot> usedDots = new List<Dot>();

            if (DataSaver.Load("losed_times", 0) >= 2 && goalTypes > 1)
            {
                goalTypes--;
            }

            for (var i = 0; i < goalTypes; i++)
            {
                var dot = dots.Except(usedDots).Random();
                usedDots.Add(dot);

                Images[i].texture = dictionary[dot.Color].render;

                var container = Containers[i];
                // container.TextMeshPro = text;
                container.TextMeshPro.text = goal.Count[i].ToString();
                container.count = goal.Count[i];
                container.SetSaveCount(container.count);
                var particle = dictionary[dot.Color].Particle;
                particle.transform.parent.SetParent(container.transform, true);
                particle.transform.localPosition = Vector3.zero;
                particle.transform.parent.SetParent(ParticlesParent, true);
                container.OnBallBounce = particle;

                container.DotColor = dot.Color;
            }

            var gemCount =
                PlaceManager.Instance.LevelGems[Mathf.Min(level, PlaceManager.Instance.LevelGems.Length - 1)];
            bool hasGem =
                gemCount > 0;
            HasGem = hasGem;
            if (hasGem)
            {
                var container = Containers[goalTypes];
                container.TextMeshPro.text = gemCount.ToString();
                container.count = gemCount;
                container.IsGem = true;
                container.SetSaveCount(gemCount);
                Images[goalTypes].texture = _renders.First(r => r.isGem).render;
                // container.gem;
            }


            for (int i = goalTypes + (hasGem ? 1 : 0); i < Images.Length; i++)
            {
                Containers[i].gameObject.SetActive(false);
            }

            if (hasGem)
            {
                Containers.Last().count = 0;
                Containers.Last().gameObject.SetActive(false);
                Containers.Last().SetSaveCount(Containers.Last().count);
                Containers.Last().TextMeshPro.text = 0.ToString();
            }
            else
            {
                Containers.Last().count = _current;
                Containers.Last().SetSaveCount(Containers.Last().count);
                Containers.Last().TextMeshPro.text = _current.ToString();
            }
        }

        else
        {
            var gemCount =
                PlaceManager.Instance.LevelGems[Mathf.Min(level, PlaceManager.Instance.LevelGems.Length - 1)];
            bool hasGem =
                gemCount > 0;
            HasGem = hasGem;
            if (hasGem)
            {
                var container = Containers[goalTypes];
                container.TextMeshPro.text = gemCount.ToString();
                container.count = gemCount;
                container.SetSaveCount(container.count);
                container.IsGem = true;
                Images[goalTypes].texture = _renders.First(r => r.isGem).render;
                // container.gem;
            }


            for (int i = goalTypes + (hasGem ? 1 : 0); i < Images.Length; i++)
            {
                Containers[i].gameObject.SetActive(false);
            }

            if (hasGem)
            {
                Containers.Last().count = 0;
                Containers.Last().gameObject.SetActive(false);
                Containers.Last().SetSaveCount(Containers.Last().count);
                Containers.Last().TextMeshPro.text = 0.ToString();
            }
            else
            {
                Containers.Last().count = _current;
                Containers.Last().SetSaveCount(Containers.Last().count);
                Containers.Last().TextMeshPro.text = _current.ToString();
            }
        }
    }

    private void OnLevelBarFilled()
    {
        TryToFinish();
    }

    public void RemoveColor(DotColor color, bool dotIsGem)
    {
        GoalItemItem[] goals = GetContainer(color, dotIsGem);
        if (goals != null && goals.Length > 0)
        {
            foreach (var goal in goals)
            {
                goal.RemoveCount();
            }
        }

        TryToFinish();
    }


    public void RemoveForSave(DotColor color, bool dotIsGem)
    {
        GoalItemItem[] goals = GetContainer(color, dotIsGem);
        if (goals != null && goals.Length > 0)
        {
            foreach (var goal in goals)
            {
                goal.RemoveSavingCount();
            }
        }
    }


    private GoalItemItem[] GetContainer(DotColor color, bool dotIsGem)
    {
        IEnumerable<GoalItemItem> found =
            Containers.Where((c => c.DotColor == color && c.gameObject.activeSelf && !c.All));
        List<GoalItemItem> items = new List<GoalItemItem>();
        if (found.Any())
        {
            items.Add(found.First());
        }

        if (!dotIsGem)
        {
            var all = Containers.Where(c => c.All);
            if (all.Any())
            {
                items.Add(all.First());
            }
        }
        else
        {
            var gem = Containers.Where(c => c.IsGem);
            if (gem.Any())
            {
                items.Add(gem.First());
            }
        }

        return items.ToArray();
    }

    public void TryToFinish()
    {
        var activeContainers = Containers.Where(c => c.gameObject.activeSelf);
        if (activeContainers.All(c => c.count <= 0) && !_shownWinPanel)
        {
            DragController.Instance.finish = false;
            DragController.Instance.MoveToDefaultPosition();
            UnhighlightPlaces();

            Debug.Log($"Win panel is working {activeContainers.Count()}");
            _shownWinPanel = true;

            OnGameFinished?.Invoke();
            CheckPlaces.OnGameFinished();
            if (CheckPlaces.gameFinished)
            {
                DOVirtual.DelayedCall(1.8f, () =>
                {
                    OnWinPanelShowSound?.Invoke();
                    winParticle.Play();
                });
                DOVirtual.DelayedCall(3f, () => { WinPanel.OpenWindow(); });
            }
        }
    }

    private void UnhighlightPlaces()
    {
        foreach (Place place in PlaceManager.Instance.Places)
        {
            place.UnHighlight();
        }
    }

    public GoalItemItem GetTargetColor(DotColor color)
    {
        return Containers.FirstOrDefault(c => c.DotColor == color);
    }

    public GoalItemItem GetTargetGem()
    {
        return Containers.FirstOrDefault(c => c.IsGem);
    }

    public GoalItemItem GetTargetGoal()
    {
        return Containers.FirstOrDefault(c => c.All);
    }

    public bool HasFinished()
    {
        if (Containers.Count == 0)
        {
            return true;
        }

        return Containers.All(c => c.hasFinished);
    }

    public void LoadGoals(GoalSaveItem[] goals)
    {
        StartCoroutine(LoadingGoals(goals));
    }

    private IEnumerator LoadingGoals(GoalSaveItem[] goals)
    {
        yield return null;
        foreach (GoalSaveItem goal in goals)
        {
            GoalItemItem target;
            if (goal.IsAll)
            {
                target = Containers.First(c => c.All);
            }
            else if (goal.IsGem)
            {
                target = Containers.FirstOrDefault(c => c.IsGem);
            }
            else
            {
                target = Containers.FirstOrDefault(c => c.DotColor == goal.Color);
                if (target == null) continue;
            }

            target.count = goal.Count;
            target.SetSaveCount(goal.Count);
            target.UpdateCount();
        }
    }

    [Serializable]
    public class DotsLocaitons
    {
        public Transform[] Points;
    }

    [Serializable]
    public class DotRender
    {
        public RenderTexture render;
        public ParticleSystem Particle;
        public DotColor Color;
        public bool isGem;
    }

    public bool hasLastColor()
    {
        var activeContainers = Containers.Where(c => c.gameObject.activeSelf && !c.All && !c.IsGem);

        return activeContainers.Any() && activeContainers.Count(c => c.count > 0) == 1;
    }

    public DotColor GetLastColor()
    {
        var activeContainers = Containers.Where(c => c.gameObject.activeSelf && !c.All && !c.IsGem);

        return activeContainers.First(c => c.count > 0).DotColor;
    }
}