using DG.Tweening;
using UnityEngine;
using Utils;
namespace UI
{
	public class ProgressShadowed : ProgressUI
	{
		[SerializeField] private RectTransform Shadow;
		private Tween[] _animations;

		private float _fillShadow;

		public override float FillAmount
		{
			get => _fill;
			set
			{
				value = value.Clamp(0, 1f);
				_fill = value;
				_fillShadow = value;
				Init();
				SetFill(_fill * Width);
			}
		}

		private void SetShadowFillAmount(float value)
		{
			Init();
			_fillShadow = value.Clamp(0, 1f);
			value = _fillShadow * Width;
			Shadow.sizeDelta = Rect.sizeDelta.SetX(Mathf.Max(value, MinWidth));
		}
		private void SetMainFillAmount(float value)
		{
			Init();
			_fill = value.Clamp(0, 1f);
			value = _fill * Width;
			Rect.sizeDelta = Rect.sizeDelta.SetX(Mathf.Max(value, MinWidth));
		}

		protected override void SetFill(float value)
		{
			base.SetFill(value);
			Shadow.sizeDelta = Rect.sizeDelta;
		}

		public override void DoFill(float value, float duration = 0.3f)
		{
			StopAnimations();
			value = value.Clamp(0, 1f);
			Init();

			if (_fill > value) _fill = 0;
			if (_fillShadow > value) _fillShadow = 0;

			_animations = new Tween[]
			{
				DOVirtual.Float(_fillShadow, value, duration, SetShadowFillAmount), DOVirtual.Float(FillAmount, value, duration, SetMainFillAmount).SetDelay(0.2f).OnComplete(ResetAnimations)
			};
		}

		private void ResetAnimations()
		{
			_animations = null;
		}

		private void StopAnimations()
		{
			if (_animations is { Length: > 0 })
			{
				foreach (Tween tween in _animations)
				{
					if (tween != null && tween.IsActive())
					{
						tween.Kill();
					}
				}
			}
		}
	}
}