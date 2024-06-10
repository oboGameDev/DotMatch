using System;
using System.Collections.Generic;
using System.Linq;
using _4_4_Scripts;
using Assets.Scripts;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public Place CurrentPlace;

    [SerializeField] private Collider _collider;
    [SerializeField] private Rigidbody _rigidbody;
    public List<SpawnPoint> DotsLocations;

    public List<Dot> Dots;

    public Transform PositionParent;

    public Vector3 DotDragOffset;

    public Point[] DotOffsets;

    public Action OnPlaced;

    public void Select()
    {
        _collider.enabled = false;
        _rigidbody.isKinematic = true;
    }

    public bool CanBePlaced()
    {
        bool canBePlaced = true;
        List<Place> usedPlaces = new();
        foreach (Dot dot in Dots)
        {
            if (!canBePlaced) break;
            Debug.DrawRay(dot.RayDot.transform.position, Camera.main.transform.forward, Color.red, 0.2f);
            if (Physics.Raycast(dot.RayDot.transform.position, Camera.main.transform.forward, out RaycastHit hit, 10000,
                    LayerMask.GetMask("place")))
            {
                if (hit.transform.TryGetComponent(out Place place))
                {
                    /*
                    Debug.Log($"Has dot {place.HasDot()} in {place.Location.X}:{place.Location.Y}");
                    */
                    canBePlaced &= !place.HasDot() && !usedPlaces.Contains(place);
                    usedPlaces.Add(place);
                }
                else
                {
                    Debug.Log($"Not found for: {dot.name}, hit: {hit.transform.name}", dot.gameObject);
                    canBePlaced = false;
                }
            }

            else
            {
                /*
                Debug.Log($"Not found for: {dot.name}", dot.gameObject);
                */
                canBePlaced = false;
            }
        }

        return canBePlaced;
    }

    public bool CannotHighlight()
    {
        bool cannotPlace = false;
        foreach (Dot dot in Dots)
        {
            if (cannotPlace) break;
            Debug.DrawRay(dot.RayDot.transform.position, Camera.main.transform.forward, Color.red, 0.2f);
            if (Physics.Raycast(dot.RayDot.transform.position, Camera.main.transform.forward, out RaycastHit hit, 10000,
                    LayerMask.GetMask("place")))
            {
                if (hit.transform.TryGetComponent(out Place place))
                {
                    /*
                    Debug.Log($"Has dot {place.HasDot()} in {place.Location.X}:{place.Location.Y}");
                    */
                    cannotPlace |= place.HasDot();
                }
                else
                {
                    Debug.Log($"Not found for: {dot.name}, hit: {hit.transform.name}", dot.gameObject);
                    cannotPlace = true;
                }
            }
            else
            {
                cannotPlace = true;
            }
        }

        return cannotPlace;
    }

    public Place[] GetNearestPlaces()
    {
        List<Place> exception = new List<Place>();
        foreach (var dot in Dots)
        {
            if (Physics.Raycast(dot.RayDot.transform.position, Camera.main.transform.forward,
                    out RaycastHit hit, 500, LayerMask.GetMask("raycastNearest")))
            {
                var place = PlaceManager.Instance.Places
                    .OrderBy(z => Vector3.Distance(z.transform.position, hit.point)).Except(exception).FirstOrDefault();
                exception.Add(place);
            }
        }

        return exception.ToArray();
    }

    public void Place()
    {
        List<Place> changed = new List<Place>();
        foreach (Dot dot in Dots)
        {
            Debug.Log($"Dot: {dot != null}");

            if (Physics.Raycast(dot.RayDot.transform.position, Camera.main.transform.forward, out RaycastHit hit, 1000,
                    LayerMask.GetMask("place")) && hit.transform.TryGetComponent(out Place place))
            {
                dot.EnableCollider();
                place.SetDots(dot);
                changed.Add(place);
            }
            else
            {
                throw new InvalidOperationException("Couldn't find place for dot");
            }
        }

        DragController.Instance.OnDotPlacement();
        PlaceManager.Instance.CheckPlaces(changed);
        PlaceManager.Instance.SaveLevel();
    }
    public void PlaceByList(List<Place> changed)
    {
        for (var i = 0; i < Dots.Count; i++)
        {
            var dot = Dots[i];
                dot.EnableCollider();
                changed[i].SetDots(dot);
                changed.Add(changed[i]);
                
        }

        DragController.Instance.OnDotPlacement();
        PlaceManager.Instance.CheckPlaces(changed);
        PlaceManager.Instance.SaveLevel();
    }

    public void Highlight()
    {
        List<Place> available = new List<Place>();

        foreach (Dot dot in Dots)
        {
            if (Physics.Raycast(dot.RayDot.transform.position, Camera.main.transform.forward, out RaycastHit hit, 1000,
                    LayerMask.GetMask("place")) && hit.transform.TryGetComponent(out Place place))
            {
                available.Add(place);
            }
            else
            {
                throw new InvalidOperationException("Couldn't find place for dot");
            }
        }

        foreach (Place place in PlaceManager.Instance.Places.Except(available))
        {
            place.UnHighlight();
            DragController.Instance.highlighted = false;
        }

        foreach (Place place in available)
        {
            place.Highlight();
        }
    }

    public void Unselect()
    {
        _collider.enabled = true;
        _rigidbody.isKinematic = false;
    }

    public void InitOffsets()
    {
        DotOffsets = Dots.Select(s => s.Offset).ToArray();
    }

    public void RotateCircle(float angle)
    {
        for (var i = 0; i < DotOffsets.Length; i++)
        {
            DotOffsets[i] = DotOffsets[i].RotatePoint(angle, true);
        }
    }
}