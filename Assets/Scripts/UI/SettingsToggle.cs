using DG.Tweening;
using Sounds;
using UnityEngine;
using UnityEngine.UI;
using Utils;
namespace UI
{
	public class SettingsToggle : MonoBehaviour
	{

		[SerializeField] private Image On;
		[SerializeField] private Image Handle;
		[SerializeField] private Transform HandleOff;
		[SerializeField] private Transform HandleOn;
		[SerializeField] private bool Default;
		[SerializeField] private string Key;

		[SerializeField] private Color onHandleColor;
		
		[SerializeField] private Sprite OffHandleImage;
		[SerializeField] private Sprite OnHandleImage;

		private Tween LastAnim;

		private bool _value;

		public bool Value
		{
			get => _value;
			set => Activate(value);
		}

		public bool ValueInstant
		{
			get => _value;
			set => ActivateInstant(value);
		}

		private void Awake()
		{
			if (!DataSaver.Has(Key))
			{
				DataSaver.Save(Key, Default);
			}
			_value = DataSaver.Load(Key, Default);
		}

		private void Start()
		{
			MoveInstant();
		}

		private void Activate(bool to)
		{
			_value = to;
			DataSaver.Save(Key, _value);
			Move();
		}

		private void ActivateInstant(bool to)
		{
			_value = to;
			MoveInstant();
		}

		private void Move()
		{
			StopAnim();
			if (_value)
			{
				On.DOFade(1, 0.2f).SetUpdate(true);
				Handle.transform.DOMove(HandleOn.position, 0.2f).SetUpdate(true);
				LastAnim = 	Handle.DOColor(onHandleColor, 0.2f).SetUpdate(true).OnComplete(() =>
				{
					Handle.sprite = OnHandleImage;
					Handle.color = Color.white;
				});
			}
			else
			{
				On.DOFade(0, 0.2f).SetUpdate(true);
				Handle.transform.DOMove(HandleOff.position, 0.2f).SetUpdate(true);
				Handle.sprite = OffHandleImage;
				if (Handle.color == Color.white)
				{
					Handle.color = onHandleColor;
				}

				LastAnim = 	Handle.DOColor(Color.white, 0.2f).SetUpdate(true);
			}
		}

		private void StopAnim()
		{
			if (LastAnim != null && LastAnim.IsActive())
			{
				LastAnim.Kill();
			}
		}
		private void MoveInstant()
		{
			On.FadeAlpha(_value ? 1 : 0);
			Handle.transform.position = _value ? HandleOn.position : HandleOff.position;
			Handle.sprite = _value ? OnHandleImage : OffHandleImage;
			Handle.color = Color.white;
		}

		public void Toggle()
		{
			Debug.Log($"Changing {Value} to {!Value}");
			Value = !Value;
			MusicManager.Instance.SettingsChanged();
			SoundManager.Instance.SettingsChanged();
			
		}

	}
}