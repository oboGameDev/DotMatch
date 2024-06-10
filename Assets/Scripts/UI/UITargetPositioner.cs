using UnityEngine;
namespace _Scripts.Utils
{
	public class UITargetPositioner
	{
		private Camera _camera;
		public UITargetPositioner(Camera camera)
		{
			_camera = camera;
		}


		public Vector2 GetTargetPosition(Transform target, Vector3 offset = default)
		{
			Vector3 targPos = target.transform.position + offset;
			Vector3 camForward = _camera.transform.forward;
			Vector3 camPos = _camera.transform.position + camForward;
			float distInFrontOfCamera = Vector3.Dot(targPos - camPos, camForward);
			if (distInFrontOfCamera < 0f)
			{
				targPos -= camForward * distInFrontOfCamera;
			}
			Vector2 targetPos = (Vector2)RectTransformUtility.WorldToScreenPoint(_camera, targPos);
			return targetPos;
		}

	}
}