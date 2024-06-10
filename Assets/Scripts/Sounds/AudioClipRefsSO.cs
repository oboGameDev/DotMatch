using UnityEngine;

namespace Sounds
{
    [CreateAssetMenu()]
    public class AudioClipRefsSO : ScriptableObject 
    {
        public AudioClip winPanel;
        public AudioClip losePanel;
        public AudioClip dotDrop;
        public AudioClip buttonClick;
        public AudioClip dotDestroy;
        public AudioClip dotReachUI;
        public AudioClip moneyAdded;
        public AudioClip floatingWords;
    }
}

