using System;
using _Scripts.Utils;
using Assets.Scripts;
using Lofelt.NiceVibrations;
using UI;
using UnityEngine;
using Utils;

namespace _Scripts.Engine
{
	public class VibrationManager : MonoBehaviour
	{
		public static VibrationManager Vibration;

		private void Awake()
		{
			Vibration = this;
		}

		private void Start()
		{
			PlaceManager.Instance.Destroy += Rumble;
			SoundManager.Instance.VibrateOnButtonClick += Rumble;
			DragController.Instance.OnVibro += Rumble;
		}

		public void VibrateHaptic()
		{
			if (DataSaver.Load("Haptic", true))
			{
				// ConsoleLogger.Log("Vibrate");
				HapticPatterns.PlayEmphasis(1.0f, 0.0f);
			}
		}
		public void VibrateWeakHaptic()
		{
			if (DataSaver.Load("Haptic", true))
			{
				// ConsoleLogger.Log("Vibrate");
				HapticPatterns.PlayEmphasis(0.1f, 0.0f);
			}
		}
		public void VeryVibrateWeak()
		{
			if (DataSaver.Load("Haptic" , true))
			{
				HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
			}
		}
		public void VibrateWarning()
		{
			if (DataSaver.Load("Haptic", true))
			{
				HapticPatterns.PlayPreset(HapticPatterns.PresetType.Failure);
			}
		}
		public void VibrateSoftImpact()
		{
			if (DataSaver.Load("Haptic" , true))
			{
				HapticPatterns.PlayEmphasis(0.00005f , -1f);
			}
		}

		public void Rumble()
		{
			if (DataSaver.Load("Haptic" , true))
			{
				HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
			}
		}
		public static void Vibrate() => Vibration.VibrateHaptic();
		public static void VibrateWeak() => Vibration.VibrateWeakHaptic();
		public static void VibrateVeryWeak() => Vibration.VeryVibrateWeak();
		public static void VibrateSoft() => Vibration.VeryVibrateWeak();
		
	}
}