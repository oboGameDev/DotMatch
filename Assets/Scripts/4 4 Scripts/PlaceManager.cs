using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts._4_4_Scripts;
using DG.Tweening;
using Tutorial;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Assets.Scripts
{
    public class PlaceManager : MonoBehaviour
    {
        public static PlaceManager Instance;
        private MoneySystem _moneySystem;

        public bool CheckGroups; //9*9 lygy bildirmek ucin , sho bolsa shona gora check eder yaly

        public Dictionary<int, Place[]>
            _groupsDictionary { get; private set; } // shu gornushde alyas 9*9 lyk ucin placelaryn arrayyny

        //check etmek ucin place array almaly , dine ozuni alsam dine 4 4 lik ucin ishleya 
        //9 9 lyk ishleyandigini bildirmek ucin olara int bilen duran placyny almak gerek
        public Place[] Places;

        public Transform Canvas;
        public Transform ParticlesParent;

        public event Action DotDestroyed;
        public event Action Destroy;
        public event Action OnMoneyAdded;
        private Dot[] _gems;
        public Dot[] allGems;
        public int[] LevelGems;
        public bool canRotate = true;

        public GameObject verticalTutorialCanvas;
        public GameObject HorizontalTutorialCanvas;
        public GameObject ThreeByThree;
        public Action OnFloatingWords;

        public Dot[] AllGems => _gems;

        private void Awake()
        {
            Instance = this;
        }

        private IEnumerator Start()
        {
            /*LoadPlaces();*/
            /*SaveLoadSystem();*/
            if (CheckGroups) //eger Checkgroups true bolsa , it means 9 9 lyk ucin ishlemeli placelary alyas
            {
                var groups = Places.Select(p => p.GroupIndex).Distinct();
                _groupsDictionary = new Dictionary<int, Place[]>();
                foreach (var group in groups)
                {
                    _groupsDictionary.Add(group, Places.Where(p => p.GroupIndex == group).ToArray());
                }
            }

            if (Emulator.Instance.Emulate)
            {
                Simulate();
            }

            if (TutorialManager.Instance.IsCompleted(3))
            {
                SpawnGems();
            }

            yield return null;
            if (PlayerPrefs.HasKey("places_with_dots"))
            {
                LoadPlaces();
            }
        }

        public void SpawnGems()
        {
            int Count = 0;
            int level = PlayerPrefs.GetInt("level", 1) - 1;
            if (level >= CircleSpawner.Instance.LevelDots.Length)
            {
                level = CircleSpawner.Instance.LevelDots.Length - 1;
            }

            Count = CircleSpawner.Instance.LevelDots[level];
            _gems = allGems.Take(Count).ToArray();

            if (level >= LevelGems.Length)
            {
                level = LevelGems.Length - 1;
            }

            List<Place> usedPlaces = new List<Place>();
            for (int i = 0; i < LevelGems[level]; i++)
            {
                Place place = Places.Except(usedPlaces).Random();
                usedPlaces.Add(place);
                SpawnGem(place, _gems.Random());
            }
        }

        private void SpawnGem(Place place, Dot gemPrefab)
        {
            var gem = Instantiate(gemPrefab);
            place.StoreDots(gem);
        }

        private void Simulate()
        {
            SpawnHorizontal2Test(5, 0);
            Spawn92Test(1);
        }

        private void SpawnVertical2Test(int x, int i)
        {
            
            Place[] places = GetPlacesOnX(x);

            var tookPlaces = places.Take(places.Length - 1);
            foreach (Place tookPlace in tookPlaces)
            {
                Dot dot = CircleSpawner.Instance.SpawnFreeDot(i);

                tookPlace.StoreDots(dot);
            }
        }

        private void SpawnHorizontal2Test(int x, int i)
        {
            
            Place[] places = GetPlacesOnY(x);

            var tookPlaces = places.Take(places.Length - 1);
            foreach (Place tookPlace in tookPlaces)
            {
                Dot dot = CircleSpawner.Instance.SpawnFreeDot(i);

                tookPlace.StoreDots(dot);
            }
        }

        private void Spawn92Test(int i)
        {
            
            // Place[] places = GetPlacesOnY(x);

            var group = _groupsDictionary.Last().Value;

            var places = group;

            IEnumerable<Place> skip = places;
            var tookPlaces = skip.Take(2).Union(skip.Skip(3));

            // var tookPlaces = places.Take(places.Length - 1);
            foreach (Place tookPlace in tookPlaces)
            {
                Dot dot = CircleSpawner.Instance.SpawnFreeDot(i);

                tookPlace.StoreDots(dot);
            }
        }

        public Place[] GetPlacesOnY(int y)
        {
            return Places.Where(p => p.Location.Y == y).ToArray();
        }

        public Place[] GetPlacesOnX(int x)
        {
            return Places.Where(p => p.Location.X == x).ToArray();
        }

        public void CheckPlaces(List<Place> changed)
        {
            int destroyed = 0;
            CheckVertical(changed, ref destroyed);

            CheckHorizontal(changed, ref destroyed);

            Check9Squad(ref destroyed);
            Check4Squad(changed, ref destroyed);


            if (destroyed > 1)
            {
                
                FloatingText.Instance.WordAppear();
                OnFloatingWords?.Invoke();
            }
        }

        private void Check9Squad(ref int destroyed)
        {
            if (CheckGroups)
            {
                foreach (int group in _groupsDictionary.Keys)
                {
                    var cells = _groupsDictionary[group];
                    if (cells.Any(c => c.Dot == null))
                    {
                        string nullPlaces = string.Join(", ", cells.Where(c => c.Dot == null));
                        
                        
                        continue; // for yalylar ucin return dalde continue ulanmaly 
                    }

                    var color = GetDotColor(cells);
                    var placeColor = cells[0].Color;
                    float delay = 0;
                   
                    if (cells.All(ValidPlace(color)) && cells.All(c => c.Color == placeColor))
                    {
                        destroyed++;
                        foreach (var cell in cells)
                        {
                            cell.DestroyDot(delay);
                            delay += 0.135f;
                        }

                        if (!TutorialManager.Instance.IsCompleted(3) && TutorialManager.Instance.IsCurrent(3))
                        {
                            canRotate = true;
                            TutorialManager.Instance.Complete();
                            ReleaseOccupiedPlaces();
                            ThreeByThree.SetActive(false);
                        }
                    }
                }
            }
        }

        private static DotColor GetDotColor(Place[] cells)
        {
            DotColor color;
            Place first = cells.FirstOrDefault(c => c.HasDot() && !c.TutorialOccupied);
            if (first == null)
            {
                return DotColor.Raduga;
            }

            if (first.Dot.Color != DotColor.Raduga)
            {
                color = first.Dot.Color;
            }
            else
            {
                var NotRadugaPlaces = cells.Where(c => c.Dot != null && c.Dot.Color != DotColor.Raduga);
                if (NotRadugaPlaces.Any())
                {
                    color = NotRadugaPlaces.First().Dot.Color;
                }
                else
                {
                    color = first.Dot.Color;
                }
            }

            return color;
        }

        private void Check4Squad(List<Place> changed, ref int offsetX)
        {
            if (CheckGroups)
            {
                return;
            }

            foreach (var place in changed)
            {
                Check4Squad(place, 1, 1);
                Check4Squad(place, -1, 1);
                Check4Squad(place, 1, -1);
                Check4Squad(place, -1, -1);
            }
        }

        private void CheckHorizontal(List<Place> changed, ref int destroyed)
        {
            var changedY = changed.Select(s => s.Location.Y).Distinct().ToArray();
            float delay = 0;
            for (var i = 0; i < changedY.Length; i++)
            {
                delay = 0;
                Place[] places = GetPlacesOnY(changedY[i]);
                var color = GetDotColor(places);
                if (places.All(ValidPlace(color)))
                {
                    destroyed++;
                    foreach (var place in places)
                    {
                        place.DestroyDot(delay);
                        delay += 0.135f;
                    }

                  
                    if (!TutorialManager.Instance.IsCompleted(2) &&
                        TutorialManager.Instance.IsCurrent(2)) // dushundida da , index bilen gidip dur 
                    {
                        TutorialManager.Instance.Complete();
                        ReleaseOccupiedPlaces();
                        HorizontalTutorialCanvas.SetActive(false);
                    }
                }
            }
        }

        private static Func<Place, bool> ValidPlace(DotColor color)
        {
            return c => c.Dot != null && (c.Dot.Color == color || c.Dot.Color == DotColor.Raduga);
        }

        private void CheckVertical(List<Place> changed, ref int destroyed)
        {
            var changedX =
                changed.Select(s => s.Location.X).Distinct()
                    .ToArray(); //shu wagtlykca dine x ya-da y boyunca ediler
            float delay = 0;
            for (var i = 0; i < changedX.Length; i++)
            {
                delay = 0;
                Place[] places = GetPlacesOnX(changedX[i]);
                var color = GetDotColor(places);
                if (places.All(ValidPlace(color)))
                {
                    destroyed++;
                    foreach (var place in places)
                    {
                       
                        place.DestroyDot(delay);
                        delay += 0.135f;
                    }

                    if (!TutorialManager.Instance.IsCompleted(1) &&
                        TutorialManager.Instance.IsCurrent(1)) // Indiki boljak horizontal 
                    {
                        TutorialManager.Instance.Complete();
                        ReleaseOccupiedPlaces();
                        verticalTutorialCanvas.SetActive(false);
                    }

                    if (!TutorialManager.Instance.IsCompleted(5) &&
                        TutorialManager.Instance.IsCurrent(5))
                    {
                        TutorialManager.Instance.Complete();
                        ReleaseOccupiedPlaces();
                    }
                }
            }
        }

        public void ReleaseOccupiedPlaces()
        {
            foreach (Place place in Places)
            {
                place.TutorialOccupied = false;
                place.canUseBooster = true;
            }
        }

        public Place GetCell(Point point) => GetCell(point.X, point.Y);

        public Place GetCell(int x, int y)
        {
            return Places.FirstOrDefault(p => p.Location.X == x && p.Location.Y == y);
        }

        public void Check4Squad(Place place, int offsetX, int offsetY)
        {
            var cell1 = GetCell(place.Location.X, place.Location.Y);
            var cell2 = GetCell(place.Location.X + offsetX, place.Location.Y);
            var cell3 = GetCell(place.Location.X, place.Location.Y + offsetY);
            var cell4 = GetCell(place.Location.X + offsetX, place.Location.Y + offsetY);

            var cell1Null = cell1 == null;
            var cell2Null = cell2 == null;
            var cell3Null = cell3 == null;
            bool cell4Null = cell4 == null;
            if (cell1Null || cell2Null || cell3Null || cell4Null)
            {
                return;
            }

            Place[] cells = new[] { cell1, cell2, cell3, cell4 };
            if (cells.Any(c => c.Dot == null)) return;

            var color = GetDotColor(cells);
            var placeColor = cell1.Color;
            float delay = 0;

            if (cells.All(ValidPlace(color)))
            {
               
                
                bool equal = true;
                foreach (var cell in cells)
                {
                   
                    equal &= cell.Color == placeColor;
                }

                if (!equal) return;

                foreach (var cell in cells)
                {
                    cell.DestroyDot(delay);
                    delay += 0.1f;
                    /*
                    _moneySystem.AddMoney(2);
                    */
                }
            }
        }

        public void SpawnVerticalDots()
        {
            verticalTutorialCanvas.SetActive(true);

            canRotate = false;
            Place[] places = GetPlacesOnX(Places.Max(p => p.Location.X));

            var tookPlaces = places.Take(places.Length - 2);
            foreach (Place tookPlace in tookPlaces)
            {
                if (tookPlace.Dot != null) continue;
                Dot dot = CircleSpawner.Instance.SpawnFreeDot();

                tookPlace.StoreDots(dot);
            }


            Place[] placesToOccupy = Places.Except(places).ToArray();
            foreach (var place in placesToOccupy)
            {
                place.TutorialOccupied = true; // tutorial ucin gerek, basga zat goydurmaz yaly
            }
        }

        public void SpawnHammerVerticalDots()
        {
            canRotate = false;
            DragController.Instance._canDrag = false;
            Place[] places = GetPlacesOnX(Places.Max(p => p.Location.X));

            var tookPlaces = places;

            for (var i = 0; i < tookPlaces.Length; i++)
            {
                var tookPlace = tookPlaces[i];

                if (tookPlace.Dot != null) continue;
                var isCenter = i == places.Length / 2;
                Dot dot = CircleSpawner.Instance.SpawnFreeCustomDot(isCenter ? 1 : 0);
                tookPlace.canUseBooster = isCenter;

                tookPlace.StoreDots(dot);
            }


            Place[] placesToOccupy = Places.Except(places).ToArray();
            foreach (var place in placesToOccupy)
            {
                place.canUseBooster = false;
                place.TutorialOccupied = true; // tutorial ucin gerek, basga zat goydurmaz yaly
            }
        }

        public void SpawnRainbowVerticalDots()
        {
            canRotate = false;
            DragController.Instance._canDrag = false;
            Place[] places = GetPlacesOnY(Places.Max(p => p.Location.Y));
            
           
            var tookPlaces = places.Skip(1).ToArray();

            for (var i = 0; i < tookPlaces.Length; i++)
            {
                var tookPlace = tookPlaces[i];
                if (tookPlace.Dot != null) continue;
                Dot dot = CircleSpawner.Instance.SpawnFreeDot();
                tookPlace.canUseBooster = false;

                tookPlace.StoreDots(dot);
            }


            Place[] placesToOccupy = Places.Except(places).ToArray();
            foreach (var place in placesToOccupy)
            {
                place.canUseBooster = false;
                place.TutorialOccupied = true; // tutorial ucin gerek, basga zat goydurmaz yaly
            }

            /*
            newPlace.canUseBooster = true;
            newPlace.TutorialOccupied = false;//and this part
            */
            
        }

        public void SpawnHandVerticalDots()
        {
            canRotate = false;
            DragController.Instance._canDrag = false;
            Place[] places = GetPlacesOnX(Places.Max(p => p.Location.X));

            var newPlace = GetPlacesOnX(places.First().Location.X - 1).Last();
            var tookPlaces = places.Take(places.Length - 1).Union(new[] { newPlace });
            foreach (Place tookPlace in tookPlaces)
            {
                if (tookPlace.Dot != null) continue;
                Dot dot = CircleSpawner.Instance.SpawnFreeDot();
                tookPlace.canUseBooster = false;
                tookPlace.StoreDots(dot);
            }


            Place[] placesToOccupy = Places.Except(places.Union(new[] { places.Last() })).ToArray();
            foreach (var place in placesToOccupy)
            {
                place.canUseBooster = false;
                place.TutorialOccupied = true; // tutorial ucin gerek, basga zat goydurmaz yaly
            }

            newPlace.canUseBooster = true;
            newPlace.TutorialOccupied = false;
        }

        public void SpawnHorizontalDots()
        {
            HorizontalTutorialCanvas.SetActive(true);
            canRotate = false;
            DOVirtual.DelayedCall(0.1f, () =>
            {
                Place[] places = GetPlacesOnY(Places.Max(p => p.Location.Y));

                var tookPlacesY = places.Take(places.Length - 2);
                foreach (Place tookPlace in tookPlacesY)
                {
                    if (tookPlace.Dot != null) continue;
                    Dot dot = CircleSpawner.Instance.SpawnFreeDot();

                    tookPlace.StoreDots(dot);
                }

                Place[] placesToOccupy = Places.Except(places).ToArray();
                foreach (var place in placesToOccupy)
                {
                    place.TutorialOccupied = true; // tutorial ucin gerek, basga zat goydurmaz yaly
                }
            });
        }

        public void Spawn3By3Dots()
        {
            ThreeByThree.SetActive(true);
            DOVirtual.DelayedCall(0.1f, () =>
            {
                var group = _groupsDictionary[4];

                var places = group;

                IEnumerable<Place> skip = places.Skip(2);
                var tookPlacesY = skip.Take(1).Union(skip.Skip(3));
                foreach (Place tookPlace in tookPlacesY)
                {
                    if (tookPlace.Dot != null) continue;
                    Dot dot = CircleSpawner.Instance.SpawnFreeDot();

                    tookPlace.StoreDots(dot);
                }

                Place[] placesToOccupy = Places.Except(places).ToArray();
                foreach (var place in placesToOccupy)
                {
                    place.TutorialOccupied = true; // tutorial ucin gerek, basga zat goydurmaz yaly
                }
            });
        }

        public void ActivateRotation()
        {
            canRotate = true;
        }

        public void OnDotDestroyed()
        {
            DotDestroyed?.Invoke();
        }

        public void OnDestroyDot()
        {
            Destroy?.Invoke();
        }

        public void MoneyAdded()
        {
            OnMoneyAdded?.Invoke();
        }


        public void SaveLevel()
        {
            var places = Places;

            List<SavingDots> placesToSave = new List<SavingDots>();
            foreach (Place place in places)
            {
                if (place.HasDot() && !place.TutorialOccupied)
                {
                    placesToSave.Add(new SavingDots(place.Location, (int)place.Dot.Color));
                }
            }

            List<GoalSaveItem> goals = new List<GoalSaveItem>();

            foreach (var item in GameLevelGoalUI.Instance.Containers)
            {
                if (item.gameObject.activeSelf)
                {
                    var save = new GoalSaveItem(item.DotColor, item.IsGem, item.All, item.SaveCount);
                    goals.Add(save);
                }
            }

            SavingSet set = new SavingSet() { dotsArray = placesToSave.ToArray(), Goals = goals.ToArray()};

            string placeListString = JsonUtility.ToJson(set);
            PlayerPrefs.SetString("places_with_dots", placeListString);
            PlayerPrefs.Save();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                PlayerPrefs.DeleteKey("places_with_dots");
                SceneManager.LoadScene(gameObject.scene.buildIndex);
            }
        }

        public void LoadPlaces()
        {
            string placeListString = PlayerPrefs.GetString("places_with_dots", "[]"); // Default empty list
            SavingSet loadedPlaces = JsonUtility.FromJson<SavingSet>(placeListString);

            foreach (SavingDots dot in loadedPlaces.dotsArray)
            {
                var place = GetCell(dot.Location);


                Dot newDot = CircleSpawner.Instance.SpawnDotByColor((DotColor)dot.dotType);

                place.StoreDots(newDot);
            }

            GameLevelGoalUI.Instance.LoadGoals(loadedPlaces.Goals);
        }
    }
}

[Serializable]
public class SavingDots
{
    public Point Location;
    public int dotType;

    public SavingDots(Point placeLocation, int dotType)
    {
        Location = placeLocation;
        this.dotType = dotType;
    }
}

[Serializable]
public class SavingSet
{
    public SavingDots[] dotsArray;
    public GoalSaveItem[] Goals;
}

[Serializable]
public class GoalSaveItem
{
    public DotColor Color;
    public bool IsGem;
    public bool IsAll;
    public int Count;

    public GoalSaveItem(DotColor color, bool isGem, bool isAll, int count)
    {
        Color = color;
        IsGem = isGem;
        IsAll = isAll;
        Count = count;
    }
}