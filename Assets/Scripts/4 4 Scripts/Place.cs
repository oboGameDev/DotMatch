using System;
using Assets.Scripts;
using Assets.Scripts._4_4_Scripts;
using DG.Tweening;
using UnityEngine;
using Utils.HelperScripts.Utils;
using Random = UnityEngine.Random;

public class Place : MonoBehaviour
{
    public static Place Instance;

    public Point Location;
    public Dot Dot;
    public MeshRenderer PlaceMesh;
    public PlaceColor Color;
    public int GroupIndex;
    public Action OnLevelBarFilled;
    private bool isHighlighted = false;
    private float _defaultAlpha;
    public bool TutorialOccupied;
    public bool canUseBooster = true;
    private float speed;

    public enum PlaceColor
    {
        Blue,
        Darkblue,
        // ...
    }

    public Transform PositionLocation;

    //public PlaceManager PlaceManager;
    public void SetDots(Dot draggingDot)
    {
        if (Dot == null)
        {
            StoreDots(draggingDot);
        }
    }

    public bool HasDot()
    {
        return Dot != null || TutorialOccupied;
    }

    public void StoreDots(Dot draggingDot)
    {
        draggingDot.EnableCollider();
        draggingDot.Place = this;
        draggingDot.transform.SetParent(transform, true);
        draggingDot.transform.localPosition = PositionLocation.localPosition;
        Dot = draggingDot;
        if (Dot != null)
        {
            Dot.transform.DOScale(0.38f, 0.2f).Capture();
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Color color = PlaceMesh.material.color;
        _defaultAlpha = color.a;
        color.a = 0;
        PlaceMesh.material.color = color;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DestroyDot(float delay)
    {
        Dot dot = Dot;
        DotColor color = dot.Color;
        bool dotIsGem = dot.isGem;
        GameLevelGoalUI.Instance.RemoveForSave(color, dotIsGem);
        DOVirtual.DelayedCall(delay, () =>
        {
            PlaceManager.Instance.MoneyAdded();
            Sequence dotDisappear = DOTween.Sequence();
            dotDisappear
                .Pause(); //pause hokman goymaly gerekli yzygiderlikde ishelemegi ucin , sequence doredilende goymaly bashynda
            ParticleSystem particle = dot.ParticleInDotDestroy;
            ParticleSystem flyingParticle = dot.FlyingParticleOnDestroy;
            ParticleSystem flyingColorParticle = dot.FlyingParticleColorOnDestroy;
            var targetColor = GameLevelGoalUI.Instance.GetTargetColor(dot.Color);
            if (dot.isGem)
            {
                targetColor = GameLevelGoalUI.Instance.GetTargetGem();
            }
            Transform particleParent = flyingParticle.transform.parent;
            particleParent.SetParent(PlaceManager.Instance.ParticlesParent, true);
            particleParent.position =
                GetTargetPosition(dot.transform, Camera.main); //here is the problem , it is not working
            Transform particleColorParent = flyingColorParticle.transform.parent;
            particleColorParent.SetParent(PlaceManager.Instance.ParticlesParent, true);
            particleColorParent.position =
                GetTargetPosition(dot.transform, Camera.main); //here is the problem , it is not working
            //worlddan scrina gecyan kody bolmaly shutayda
            var uiTargetPosition =
                GetTargetPosition(dot.stopPosition.transform,
                    Camera.main); //stopPosition Ui a gecirya , sebabi world positionda dur ol
            // assign particle to something , so that it won't be destroyed together with dot 
            Debug.Log($"Dot {dot.gameObject.name}, {dot.transform != null}");
            dotDisappear.Append(dot.gameObject.transform.DOScale(0.48f, 0.08f).SetEase(Ease.Linear).OnComplete(() =>
            {
                PlaceManager.Instance.OnDestroyDot();
            }).Capture());
            dotDisappear.AppendCallback(() => particle.transform.SetParent(transform, true));
            //flyingParticle.transform.position = Vector3.up;// here it must be the position of levelbar
            dotDisappear.AppendInterval(0.01f);
            dotDisappear.AppendCallback(() => particle.Play());
            if (!GameLevelGoalUI.Instance.HasGem)
            {
                dotDisappear.AppendCallback(() => dot.TrailParticle.Play());
                dotDisappear.AppendCallback(() => flyingParticle.Play()); //ishlanok shu
            }

            if (targetColor != null)
            {
                dotDisappear.AppendCallback(() => flyingColorParticle.Play()); //ishlanok shu
            }

            /*
            dotDisappear.AppendInterval(0.1f);
          */
            dotDisappear.Join(DOVirtual.DelayedCall(0.01f, () => Destroy(dot.gameObject)));
            /*
            dotDisappear.AppendCallback((() =>Destroy(dot.gameObject) ));
            */
            var fromTop = Random.value >= 0.5f;
            dotDisappear.Append(particleParent.DOMoveCircularUI(uiTargetPosition, 0.6f, fromTop));
            dotDisappear.Join(particleColorParent.DOMoveCircularUI(uiTargetPosition, 0.6f, fromTop));
            //Stop positionyn Ui daky positionyna gecya using DOMoveCircularUI
            dotDisappear.AppendCallback((() =>
            {
                GoalItemItem all = GameLevelGoalUI.Instance.GetTargetGoal();
                if (GameLevelGoalUI.Instance.HasGem)
                {
                    PlaceManager.Instance.OnDotDestroyed();
                    OnLevelBarFilled?.Invoke(); /*
                    MoneySystem.Instance.AddMoney(1);*/
                    GameLevelGoalUI.Instance.RemoveColor(color, dotIsGem);
                    Destroy(flyingParticle);
                }
                else
                {
                    particleParent.DOMove(all.transform.position, 0.5f)
                        .OnComplete(() =>
                        {
                            /*
                            GameLevelUI.Instance.AddCount();
                            */
                            PlaceManager.Instance.OnDotDestroyed(); /*
                            MoneySystem.Instance.AddMoney(1);*/
                            GameLevelGoalUI.Instance.RemoveColor(color, dotIsGem);
                            Debug.Log($"Bouncing: {all.name}", all);
                            all.BallBounce();
                            OnLevelBarFilled?.Invoke();
                            Destroy(flyingParticle);
                        });
                }

                if (targetColor != null)
                {
                    particleColorParent.DOMove(targetColor.transform.position, 0.5f)
                        .OnComplete(() =>
                        {
                            Destroy(flyingColorParticle);

                            targetColor.BallBounce();
                            // GameLevelUI.Instance.BallBounce();
                        });
                }
                /*flyingParticle.transform.parent.position =*/
            }));
            dotDisappear.AppendInterval(0.1f);
            dotDisappear.AppendCallback(() => Destroy(particle.gameObject, 0.6f));
            // Destroy(dot.gameObject);
            dotDisappear.Play();
        });


        Dot = null;
    }

    public void DestroyDotIfNotGem(float delay)
    {
        Dot dot = Dot;
        if (!dot.isGem)
        {
            DotColor color = dot.Color;
            bool dotIsGem = dot.isGem;
            GameLevelGoalUI.Instance.RemoveForSave(color, dotIsGem);
            DOVirtual.DelayedCall(delay, () =>
            {
                PlaceManager.Instance.MoneyAdded();

                Sequence dotDisappear = DOTween.Sequence();
                dotDisappear
                    .Pause(); //pause hokman goymaly gerekli yzygiderlikde ishelemegi ucin , sequence doredilende goymaly bashynda
                ParticleSystem particle = dot.ParticleInDotDestroy;
                ParticleSystem flyingParticle = dot.FlyingParticleOnDestroy;
                ParticleSystem flyingColorParticle = dot.FlyingParticleColorOnDestroy;
                var targetColor = GameLevelGoalUI.Instance.GetTargetColor(dot.Color);
                if (dot.isGem)
                {
                    targetColor = GameLevelGoalUI.Instance.GetTargetGem();
                }

                Transform particleParent = flyingParticle.transform.parent;
                particleParent.SetParent(PlaceManager.Instance.ParticlesParent, true);
                particleParent.position =
                    GetTargetPosition(dot.transform, Camera.main); //here is the problem , it is not working
                Transform particleColorParent = flyingColorParticle.transform.parent;
                particleColorParent.SetParent(PlaceManager.Instance.ParticlesParent, true);
                particleColorParent.position =
                    GetTargetPosition(dot.transform, Camera.main); //here is the problem , it is not working
                //worlddan scrina gecyan kody bolmaly shutayda
                var uiTargetPosition =
                    GetTargetPosition(dot.stopPosition.transform,
                        Camera.main); //stopPosition Ui a gecirya , sebabi world positionda dur ol
                // assign particle to something , so that it won't be destroyed together with dot 
                Debug.Log($"Dot {dot.gameObject.name}, {dot.transform != null}");
                dotDisappear.Append(dot.gameObject.transform.DOScale(0.48f, 0.08f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    PlaceManager.Instance.OnDestroyDot();
                }).Capture());
                dotDisappear.AppendCallback(() => particle.transform.SetParent(transform, true));
                //flyingParticle.transform.position = Vector3.up;// here it must be the position of levelbar
                dotDisappear.AppendInterval(0.01f);
                dotDisappear.AppendCallback(() => particle.Play());
                if (!GameLevelGoalUI.Instance.HasGem)
                {
                    dotDisappear.AppendCallback(() => dot.TrailParticle.Play());
                    dotDisappear.AppendCallback(() => flyingParticle.Play()); //ishlanok shu
                }

                if (targetColor != null)
                {
                    dotDisappear.AppendCallback(() => flyingColorParticle.Play()); //ishlanok shu
                }

                /*
                dotDisappear.AppendInterval(0.1f);
              */
                dotDisappear.Join(DOVirtual.DelayedCall(0.01f, () => Destroy(dot.gameObject)));
                /*
                dotDisappear.AppendCallback((() =>Destroy(dot.gameObject) ));
                */
                var fromTop = Random.value >= 0.5f;
                dotDisappear.Append(particleParent.DOMoveCircularUI(uiTargetPosition, 0.6f, fromTop));
                dotDisappear.Join(particleColorParent.DOMoveCircularUI(uiTargetPosition, 0.6f, fromTop));
                //Stop positionyn Ui daky positionyna gecya using DOMoveCircularUI
                dotDisappear.AppendCallback((() =>
                {
                    GoalItemItem all = GameLevelGoalUI.Instance.GetTargetGoal();
                    if (GameLevelGoalUI.Instance.HasGem)
                    {
                        PlaceManager.Instance.OnDotDestroyed();
                        OnLevelBarFilled?.Invoke(); /*
                        MoneySystem.Instance.AddMoney(1);*/
                        GameLevelGoalUI.Instance.RemoveColor(color, dotIsGem);
                        Destroy(flyingParticle);
                    }
                    else
                    {
                        particleParent.DOMove(all.transform.position, 0.5f)
                            .OnComplete(() =>
                            {
                                /*
                                GameLevelUI.Instance.AddCount();
                                */
                                PlaceManager.Instance.OnDotDestroyed(); /*
                                MoneySystem.Instance.AddMoney(1);*/
                                GameLevelGoalUI.Instance.RemoveColor(color, dotIsGem);
                                Debug.Log($"Bouncing: {all.name}", all);
                                all.BallBounce();
                                OnLevelBarFilled?.Invoke();
                                Destroy(flyingParticle);
                            });
                    }

                    if (targetColor != null)
                    {
                        particleColorParent.DOMove(targetColor.transform.position, 0.5f)
                            .OnComplete(() =>
                            {
                                Destroy(flyingColorParticle);

                                targetColor.BallBounce();
                                // GameLevelUI.Instance.BallBounce();
                            });
                    }
                    /*flyingParticle.transform.parent.position =*/
                }));
                dotDisappear.AppendInterval(0.1f);
                dotDisappear.AppendCallback(() => Destroy(particle.gameObject, 0.6f));
                // Destroy(dot.gameObject);
                dotDisappear.Play();
            });
        }

        if (HasDot() && !dot.isGem)
        {
            Dot = null;
        }
    }

    public Vector2 GetTargetPosition(Transform target, Camera camera)
    {
        Vector3 targPos = target.transform.position;
        Vector3 camForward = camera.transform.forward;
        Vector3 camPos = camera.transform.position + camForward;
        float distInFrontOfCamera = Vector3.Dot(targPos - camPos, camForward);
        if (distInFrontOfCamera < 0f)
        {
            targPos -= camForward * distInFrontOfCamera;
        }

        Vector2 targetPos = (Vector2)RectTransformUtility.WorldToScreenPoint(camera, targPos);
        return targetPos;
    }

    private void OnMouseDown()
    {
        BoosterManager.Instance.ClickedPlace(this);
    }

    public void Highlight()
    {
        if (isHighlighted)
        {
            return;
        }

        isHighlighted = true;
        DragController.Instance.highlighted = true;
        PlaceMesh.material.DOFade(_defaultAlpha, 0.3f);
    }

    public void UnHighlight()
    {
        if (!isHighlighted)
        {
            return;
        }

        isHighlighted = false;
        DragController.Instance.highlighted = false;
        PlaceMesh.material.DOFade(0, 0.3f);
    }
}