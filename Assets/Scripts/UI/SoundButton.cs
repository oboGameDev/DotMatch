using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SoundButton : MonoBehaviour
    {
        public static SoundButton Instance;

        private void Awake()
        {
            Instance = this;
        }

         
        private void Start()
        {
            GetComponent<Button>() .onClick.AddListener(Sound);
        }

        public void Sound()
        {
            SoundManager.Instance.ButtonSound();
        }
    }
}