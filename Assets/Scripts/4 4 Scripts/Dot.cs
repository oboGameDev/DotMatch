using System;
using System.Collections.Generic;
using Assets.Scripts._4_4_Scripts;
using UnityEngine;

namespace Assets.Scripts
{
    public class Dot : MonoBehaviour
    {
        public DotColor Color;
        public ParticleSystem ParticleInDotDestroy;
        public ParticleSystem FlyingParticleOnDestroy;
        public ParticleSystem FlyingParticleColorOnDestroy;
        public ParticleSystem TrailParticle;

        public GameObject stopPosition;
        //public TrailRenderer TrailRendererOnDestroy;
        public Point Offset;
        public Place Place;
        public Transform RayDot;
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rigidbody;
        public bool isGem;

       
        public void Select()
        {
            _collider.enabled = false;
            _rigidbody.isKinematic = true;
        }

        public void DisableCollider()
        {
            _collider.enabled = false;
        }

        public void EnableCollider()
        {
            _collider.enabled = true;
        }

        public void Unselect()
        {
            _collider.enabled = true;
            _rigidbody.isKinematic = false;
        }

        public void PlaceDot()
        {
            List<Place> changed = new List<Place>();

            bool hasChanged = false;
            
            
            if (Physics.Raycast(transform.position, Camera.main.transform.forward, out RaycastHit hit, 1000,
                    LayerMask.GetMask("place")) && hit.transform.TryGetComponent(out Place place))
            {
                hasChanged = Place != place;
                Place.Dot = null;
                place.SetDots(this);
                changed.Add(place);
            }

            if (hasChanged)
            {
                Hand.Instance.SubtractCount();
            }

            PlaceManager.Instance.CheckPlaces(changed);
        }

        public bool CanBePlaced()
        {
            bool canBePlaced = true;
            List<Place> usedPlaces = new();

            if (Physics.Raycast(transform.position, Camera.main.transform.forward, out RaycastHit hit, 1000,
                    LayerMask.GetMask("place")) && hit.transform.TryGetComponent(out Place place))
            {
                Debug.Log($"Has dot {place.HasDot()} in {place.Location.X}:{place.Location.Y}");
                canBePlaced &= !place.HasDot() && !usedPlaces.Contains(place);
                usedPlaces.Add(place);
            }

            else
            {
                Debug.Log($"Not found for: {name}", gameObject);
                canBePlaced = false;
            }

            return canBePlaced;
        }

        private void OnMouseDown()
        {
            Debug.Log($"CLickedPlace {name}");
            BoosterManager.Instance.ClickedDot(this);
        }
    }


    [Serializable]
    public enum DotColor
    {
        Blue,
        Red,
        Green,
        Aqua,
        Darkblue,
        Orange,
        Pink,
        Raduga,
        BlueGem,
        RedGem,
        GreenGem,
        AquaGem,
        DarkblueGem,
        OrangeGem,
        PinkGem,
        // ...
    }
}