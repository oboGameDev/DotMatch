using UnityEngine;

namespace Assets.Scripts._4_4_Scripts
{
    public class Emulator: MonoBehaviour
    {
        public static Emulator Instance;

        public bool Emulate;
        
        private void Awake()
        {
            Instance = this;
        }
    }
}