using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
namespace _Scripts.Utils
{
	public class MouseEvents
	{
		public bool IsMousePressed => _isMousePressed();
		public bool IsMouseMoving => _isMouseMoving();
		public bool IsMouseReleased => _isMouseReleased();
		public Vector3 GetMousePosition => _getMousePosition();

		private Func<bool> _isMousePressed = () => Input.GetMouseButtonDown(0);
		private Func<bool> _isMouseMoving = () => Input.GetMouseButton(0);
		private Func<bool> _isMouseReleased = () => Input.GetMouseButtonUp(0);
		private Func<Vector3> _getMousePosition = () => Input.mousePosition;

		private MouseDragDetector _mouseDragDetector;
		public MouseDragDetector DragDetector => _mouseDragDetector;

		private void Init(bool mobile)
		{
			if (mobile)
			{
				AddMobileInput();
			}
			else
			{
				AddPcInput();
			}
			_mouseDragDetector = new MouseDragDetector(this);
		}

		public void Init()
		{
			Init(
				#if UNITY_EDITOR
				false
			#else
				true
  #endif
			);
		}

		private void AddPcInput()
		{
			_isMousePressed = () => Input.GetMouseButtonDown(0);
			_isMouseMoving = () => Input.GetMouseButton(0);
			_isMouseReleased = () => Input.GetMouseButtonUp(0);
			_getMousePosition = () => Input.mousePosition;
		}

		private void AddMobileInput()
		{
			_isMousePressed = () => Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
			_isMouseMoving = () => Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Moved ||
			                                                Input.GetTouch(0).phase == TouchPhase.Stationary);
			_isMouseReleased = () => Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended;
			_getMousePosition = () => Input.touchCount > 0 ? Input.GetTouch(0).position : Input.mousePosition;
		}


		public static bool IsPointerOverUiObject(Vector2 screenLocation)
		{
			var eventDataCurrentPosition = new PointerEventData(EventSystem.current);
			eventDataCurrentPosition.position = new Vector2(screenLocation.x, screenLocation.y);

			List<RaycastResult> results = new List<RaycastResult>();
			int layer = LayerMask.NameToLayer("UI");

			EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
			
			return results.Count > 0 && results.Any(p => p.gameObject.layer == layer);
		}
		
		public void SetDragConfiguration(DragConfiguration configuration)
		{
			_mouseDragDetector.SetConfiguration(configuration);
		}
	}

	[Serializable]
	public struct DragConfiguration
	{
		public int Milliseconds;
		public float Percent;
		public DragConfiguration(int milliseconds, float percent)
		{
			Milliseconds = milliseconds;
			Percent = percent;
		}
	}
}

