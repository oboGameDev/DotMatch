using System;
using _Scripts.Utils;
using UnityEngine;
namespace Misc
{
	public class UITargetFollower : MonoBehaviour
	{
		public Transform Target;
		public Camera Camera;

		public Vector3 Offset;
		private readonly int _instantMoveThreshold = 2;

		private UITargetPositioner _targetPositioner;

		private bool _inited;

		private void Start()
		{
			Init();
		}
		private void Init()
		{
			if (_inited) return;
			_inited = true;
			_targetPositioner = new UITargetPositioner(Camera);
		}

		public void Positionate()
		{
			Init();
			transform.position = _targetPositioner.GetTargetPosition(Target, Offset);
		}

		private void Update()
		{
			Vector2 targetPos = _targetPositioner.GetTargetPosition(Target, Offset);
			if (Vector3.Distance(transform.position, targetPos) > _instantMoveThreshold)
			{
				transform.position = targetPos;
			}
			else
			{
				transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 16);
			}

		}

	}
}