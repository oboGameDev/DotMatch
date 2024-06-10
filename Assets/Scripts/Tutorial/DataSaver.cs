// #define SAVE_DISABLED

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
namespace Utils
{
	public static class DataSaver
	{
		private static List<string> _keys = new();

		static DataSaver()
		{
			string keys = PlayerPrefs.GetString("keys", "");
			if (keys.Length > 0)
			{
				_keys.AddRange(keys.Split(",", StringSplitOptions.RemoveEmptyEntries));
			}
		}

		public static void Save(string key, bool value)
		{
#if SAVE_DISABLED
			return;
#endif
			ValidateKey(key);
			Add(key);
			PlayerPrefs.SetInt(key, value ? 1 : 0);
			Save();
			PlayerPrefs.Save();
		}

		public static void Save(string key, int value)
		{
#if SAVE_DISABLED
			return;
#endif
			// Debug.Log($"saving: {key}");
			ValidateKey(key);
			Add(key);
			PlayerPrefs.SetInt(key, value);
			Save();
			PlayerPrefs.Save();
		}

		public static void Save(string key, float value)
		{
#if SAVE_DISABLED
			return;
#endif
			ValidateKey(key);
			Add(key);
			PlayerPrefs.SetFloat(key, value);
			Save();
			PlayerPrefs.Save();
		}


		public static void Save(string key, string value)
		{
#if SAVE_DISABLED
			return;
#endif
			ValidateKey(key);
			Add(key);
			PlayerPrefs.SetString(key, value);
			Save();
			PlayerPrefs.Save();
		}

		private static void ValidateKey(string key)
		{
			if (string.IsNullOrEmpty(key)) throw new InvalidDataException($"Key is empty. Key is '{key}'");
		}

		private static void Add(string key)
		{
			if (!_keys.Contains(key))
				_keys.Add(key);
		}

		private static void Save()
		{
#if SAVE_DISABLED
			return;
#endif
			if (_keys.Count > 0)
				PlayerPrefs.SetString("keys", string.Join(",", _keys.ToArray()));
			else
				PlayerPrefs.SetString("keys", "");
		}

		private static void Remove(string key)
		{
			if (_keys.Contains(key))
				_keys.Remove(key);
		}

		public static bool Load(string key, bool defaultValue = true)
		{
			var i = PlayerPrefs.GetInt(key, defaultValue ? 1 : 0);
			return i == 1;
		}

		public static int Load(string key, int defaultValue = 0)
		{
			return PlayerPrefs.GetInt(key, defaultValue);
		}

		public static bool Has(string key)
		{
			return PlayerPrefs.HasKey(key);
		}

		public static float Load(string key, float defaultValue = 0)
		{
			return PlayerPrefs.GetFloat(key, defaultValue);
		}

		public static string Load(string key, string defaultValue = "")
		{
			return PlayerPrefs.GetString(key, defaultValue);
		}

		public static void Delete(string key)
		{
#if SAVE_DISABLED
			return;
#endif
			DeleteWithoutSave(key);
			PlayerPrefs.Save();
		}

		private static void DeleteWithoutSave(string key)
		{
#if SAVE_DISABLED
			return;
#endif
			PlayerPrefs.DeleteKey(key);
			Remove(key);
		}

		public static void DeleteKeyContains(string key)
		{
#if SAVE_DISABLED
			return;
#endif
			string[] keys = KeysContains(key);

			foreach (string _key in keys)
			{
				DeleteWithoutSave(_key);
			}

			Save();
			PlayerPrefs.Save();
		}

		private static string[] KeysContains(string key)
		{
			IEnumerable<string> where = _keys.Where(k => k.Contains(key));
			if (where.Any())
				return where.ToArray();
			return Array.Empty<string>();
		}

		public static string[] GetAllKeys() => _keys.ToArray();

		public static void DeleteAll()
		{
#if SAVE_DISABLED
			return;
#endif
			PlayerPrefs.DeleteAll();
			_keys.Clear();
			PlayerPrefs.Save();
		}

		public static void Increment(string key, int defaultValue = 0)
		{
			Add(key, defaultValue);
		}

		public static void Decrement(string key, int defaultValue = 0)
		{
			Decrease(key, defaultValue);
		}

		public static void Add(string key, int defaultValue, int add = 1)
		{
			Save(key, Load(key, defaultValue) + add);
		}

		public static void Decrease(string key, int defaultValue, int decrease = 1, bool minZero = false)
		{
			int value = Load(key, defaultValue) - decrease;
			if (minZero) value = Mathf.Max(value, 0);
			Save(key, value);
		}
		public static string[] FindKeys(string state)
		{
			return _keys.Where(k => k.Contains(state)).ToArray();
		}
	}
}