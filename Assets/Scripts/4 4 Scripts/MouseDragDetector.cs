using System;
using UnityEngine;

namespace _Scripts.Utils
{
    public class MouseDragDetector
    {
        private float _screenMin;
        private float _distanceRecognition;
        private Vector2 _pressedPoint;

        private MouseEvents _mouseEvents;
        private DateTime _pressedTime;
        private int _time = 200;

        public MouseDragDetector(MouseEvents mouseEvents)
        {
            _mouseEvents = mouseEvents;
            _screenMin = Mathf.Min(Screen.width, Screen.height);
            _distanceRecognition = 0.10f * _screenMin;
        }

        public void StartRecording()
        {
            _pressedPoint = _mouseEvents.GetMousePosition;
            _pressedTime = DateTime.Now;
        }

        public bool IsDragged => IsTraveledFar || PassedMuchTime;

        private bool IsTraveledFar =>
            Vector2.Distance(_pressedPoint, _mouseEvents.GetMousePosition) >= _distanceRecognition;

        private bool PassedMuchTime => (DateTime.Now - _pressedTime).TotalMilliseconds >= _time;

        public void SetConfiguration(DragConfiguration configuration)
        {
            float percent = configuration.Percent;

            if (percent > 0.75f)
            {
                percent /= 100f;
            }

            int time = configuration.Milliseconds;

            _distanceRecognition = percent * _screenMin;
            _time = time;
        }
    }
}
