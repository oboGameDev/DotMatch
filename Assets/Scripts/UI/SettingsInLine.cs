using System;
using Sounds;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class SettingsInLine : MonoBehaviour
    {
        public Image Image;
        public Sprite off;
        public Sprite on;
        public string key;

        private void Start()
        {
            UpdateImage();
        }

        public void Toggle()
        {
            DataSaver.Save(key, !DataSaver.Load(key, true));
            MusicManager.Instance.SettingsChanged();
            SoundManager.Instance.SettingsChanged();
            UpdateImage();
        }

        private void UpdateImage()
        {
            Image.sprite = DataSaver.Load(key, true) ? on : off;
        }
    }
}