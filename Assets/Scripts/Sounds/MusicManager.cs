using System;
using UnityEngine;
using Utils;

namespace Sounds
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance { get; private set; }

        [SerializeField] private AudioSource _audioSource;

        private void Start()
        {
            SettingsChanged();
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SettingsChanged()
        {
            if (DataSaver.Load("music" , true))
            {
                if (!_audioSource.isPlaying)
                {
                    _audioSource.Play();
                }
            }
            else
            {
                if (_audioSource.isPlaying)
                {
                    _audioSource.Stop();
                }
            }
        }
    }
}