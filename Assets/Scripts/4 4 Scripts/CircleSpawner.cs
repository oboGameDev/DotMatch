using System.Collections.Generic;
using System.Linq;
using _Scripts.Utils;
using Assets.Scripts._4_4_Scripts;
using Tutorial;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class CircleSpawner : MonoBehaviour
    {
        public static CircleSpawner Instance;

        [SerializeField] private Circle _prefab;
        [SerializeField] private Transform _circleLocation;
        [SerializeField] private Dot[] _dotsPrefab;
        [SerializeField] private Dot[] allDots;
        public Dot RadugaDot;
        public int[] LevelDots;

        public Dot[] AllDots => _dotsPrefab;

        private void Awake()
        {
            Instance = this;
        }


        private void Start()
        {
            int Count = 0;
            int level = PlayerPrefs.GetInt("level", 1) - 1;
            if (level >= LevelDots.Length)
            {
                level = LevelDots.Length - 1;
            }

            Count = LevelDots[level];
            _dotsPrefab = allDots.Take(Count).ToArray();

            Spawn();
        }

        private void Simulate(Circle instance)
        {
            Dot prefab3 = _dotsPrefab[1];
            Dot prefab2 = _dotsPrefab[0];

            SpawnDot(instance, 4, prefab2);
            SpawnDot(instance, 5, prefab3);
            DragController.Instance.SetCircle(instance);
        }

        public void Spawn()
        {
            Circle instance = Instantiate(_prefab, _circleLocation.position, _circleLocation.rotation);

            if (Emulator.Instance.Emulate)
            {
                Simulate(instance);
                return;
            }

            if (SpawnTutorialDots(instance)) return;

            float random = Random.value;

            /*
            Debug.Log($"random {random}"); // it can be used later can be useful that 's why didn't delete
            */

            if (random >= 0.40f)
            {
                Dot prefab3 = GetRandomDot();


                SpawnDot(instance, 4, prefab3);
                SpawnDot(instance, 5, prefab3);

                //Spawning 2
                // 1st case, where x are equal color 
            }
            else if (random <= 0.1f)
            {
                Dot prefab = GetRandomDot();

                SpawnDot(instance, 0, prefab);
                SpawnDot(instance, 1, prefab);
                SpawnDot(instance, 2, prefab);
                SpawnDot(instance, 3, prefab);

                // 3rd case, where all four one color
                //Spawned 4
            }
            else if (random <= 0.80 / 8f)
            {
                (Dot prefab1, Dot prefab2) = GetRandomDots();


                SpawnDot(instance, 0, prefab1);
                SpawnDot(instance, 1, prefab1);
                SpawnDot(instance, 2, prefab2);
                SpawnDot(instance, 3, prefab2);
                //Spawning 4.2 , second case of 4 spawning
            }

            else if (random <= 0.80 / 4f)
            {
                Dot prefab1 = GetRandomDot();

                SpawnDot(instance, 6, prefab1);
                // only 1 dot spawn 
            }
            else if (random <= 0.80f / 2)
            {
                (Dot prefab1, Dot prefab2) = GetRandomDots();

                //SpawnDot(instance, 0, prefab1);
                SpawnDot(instance, 1, prefab2);
                SpawnDot(instance, 2, prefab2);
                SpawnDot(instance, 3, prefab2); //they were 1 2 3 
                //Spawning 4.3 , 3rd case of 4
            }
            else
            {
                (Dot prefab1, Dot prefab2) = GetRandomDots();


                SpawnDot(instance, 0, prefab1);
                SpawnDot(instance, 1, prefab2);
                SpawnDot(instance, 2, prefab1);
                SpawnDot(instance, 3, prefab2);
                //Spawning 4 , but first case of four
                // 2nd case, where y are equal color , dine 2 sany cykyan yerem shuna girer da ? da. yone bu yokarky setir bir renk, asaky setir bir renk
            }

            DragController.Instance.SetCircle(instance);
        }

        private Dot GetRandomDot()
        {
            if (GameLevelGoalUI.Instance.hasLastColor())
            {
                var lastColor = GameLevelGoalUI.Instance.GetLastColor();
                int index = lastColor.IndexOf(_dotsPrefab.Select(c => c.Color).ToArray());

                RandomHelper random = new RandomHelper(_dotsPrefab.Length, new[] { new IndexPercent(index, 0.6f) });

                return _dotsPrefab[random.GetRandom()];
            }

            return _dotsPrefab[Random.Range(0, _dotsPrefab.Length)];
        }

        private (Dot, Dot) GetRandomDots()
        {
            if (GameLevelGoalUI.Instance.hasLastColor())
            {
                var dot = GetRandomDot();
                return (dot, dot);
            }

            Dot prefab1 = _dotsPrefab[Random.Range(0, _dotsPrefab.Length)];
            IEnumerable<Dot> except = _dotsPrefab.Except(new[] { prefab1 });
            Dot prefab2 = except.ElementAt(Random.Range(0, except.Count()));
            return (prefab1, prefab2);
        }

        private bool SpawnTutorialDots(Circle instance)
        {
            if (!TutorialManager.Instance.IsCompleted(0))
            {
                Spawn2Dots(instance, 0);
                return true;
            }

            if (!TutorialManager.Instance.IsCompleted(2))
            {
                Debug.Log("Horizontal stage ");
                Spawn2Dots(instance, 1);
                return true;
            }

            if (!TutorialManager.Instance.IsCompleted(3))
            {
                Debug.Log("4 stage ");
                Spawn4Dots(instance, 0);
                return true;
            }

            if (!TutorialManager.Instance.IsCompleted(4) && TutorialManager.Instance.CanStart(4))
            {
                Debug.Log("5 stage ");
                Spawn1Dot(instance, 0);
                return true;
            }

            if (!TutorialManager.Instance.IsCompleted(5) && TutorialManager.Instance.CanStart(5))
            {
                Debug.Log("6 stage");
                Spawn2Dots(instance, 0);
                return true;
            }

            if (!TutorialManager.Instance.IsCompleted(6) && TutorialManager.Instance.CanStart(6))
            {
                Debug.Log("7 stage");
                Spawn2Dots(instance, 0);
                return true;
            }

            return false;
        }

        private void Spawn2Dots(Circle instance, int number = -1)
        {
            int range = number == -1 ? Random.Range(0, _dotsPrefab.Length) : number;
            Dot prefab3 = _dotsPrefab[range];

            SpawnDot(instance, 4, prefab3);
            SpawnDot(instance, 5, prefab3);
            DragController.Instance.SetCircle(instance);
        }

        private void Spawn1Dot(Circle instance, int number = -1)
        {
            int range = number == -1 ? Random.Range(0, _dotsPrefab.Length) : number;
            Dot prefab3 = _dotsPrefab[range];

            SpawnDot(instance, 6, prefab3);
            DragController.Instance.SetCircle(instance);
        }

        private void Spawn4Dots(Circle instance, int number = -1)
        {
            int range = number == -1 ? Random.Range(0, _dotsPrefab.Length) : number;

            Dot prefab3 = _dotsPrefab[range];

            SpawnDot(instance, 0, prefab3);
            SpawnDot(instance, 1, prefab3);
            SpawnDot(instance, 2, prefab3);
            SpawnDot(instance, 3, prefab3);
            DragController.Instance.SetCircle(instance);
        }

        private void SpawnDot(Circle circle, int index, Dot prefab)
        {
            Dot instance = Instantiate(prefab);
            instance.transform.SetParent(circle.transform);
            instance.transform.localPosition = circle.DotsLocations[index].transform.localPosition;
            instance.transform.localRotation = circle.DotsLocations[index].transform.localRotation;
            instance.Offset = circle.DotsLocations[index].Offset;
            instance.RayDot = circle.DotsLocations[index].transform.GetChild(0);
            instance.RayDot.SetParent(circle.PositionParent.transform, true); //shunda false diyip durdy 
            //shonun ucinem rotate edilen wagty child rotate etyadi

            instance.DisableCollider();
            circle.Dots.Add(instance);
        }

        public Dot SpawnFreeDot()
        {
            Dot dotPrefab = DragController.Instance.GetCircle.Dots.First();
            return Instantiate(dotPrefab);
        }

        public Dot SpawnFreeDot(int index)
        {
            Dot dotPrefab = DragController.Instance.GetCircle.Dots[index];
            return Instantiate(dotPrefab);
        }

        public Dot SpawnFreeCustomDot(int index)
        {
            Dot dotPrefab;
            if (index == 0)
            {
                dotPrefab = DragController.Instance.GetCircle.Dots[index];
                Debug.Log($"index {index}, prefab {dotPrefab.Color}");
            }
            else
            {
                var where = allDots.Where(d => d.Color != DragController.Instance.GetCircle.Dots[0].Color).ToArray();
                dotPrefab = where[Random.Range(0, where.Length)];
                Debug.Log($"index {index}, prefab: {dotPrefab.Color}, {where.Length}");
            }

            return Instantiate(dotPrefab);
        }

        public Dot SpawnDotByColor(DotColor color)
        {
            Dot dotPrefab = AllDots.First(d => d.Color == color);
            return Instantiate(dotPrefab);
        }
    }
}