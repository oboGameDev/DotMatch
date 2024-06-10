using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utils;
namespace UI
{
	public class ProgressUI : MonoBehaviour
	{
		[SerializeField] protected RectTransform Rect;

		[SerializeField] protected float MinWidth;

		protected float Width;

		protected float _fill;
		private bool _inited;

		public virtual float FillAmount
		{
			get => _fill;
			set
			{
				value = value.Clamp(0, 1f);
				_fill = value;
				SetFill(_fill * Width);
			}
		}

		private void Start()
		{
			Init();
		}
		protected void Init()
		{

			if (_inited) return;

			_inited = true;
			Width = Rect.sizeDelta.x;

			SetFill(0);
		}

		protected virtual void SetFill(float value)
		{
			Init();
			Rect.sizeDelta = Rect.sizeDelta.SetX(Mathf.Max(value, MinWidth));
		}

		public virtual void DoFill(float value, float duration = 0.3f)
		{
			value = value.Clamp(0, 1f);
			Init();
			DOVirtual.Float(_fill, value, duration, f =>
			{
				FillAmount = f;
			});
		}
	}
}